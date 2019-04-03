using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using Com.Comm100.Framework.Common;

namespace WebHookTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult TriggerWebhook(string msg)
        {
            string webhookURL = "http://localhost:51968/Home/ReceiveNotify";
            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            LogHelper.Error("main thread:" + mainThreadId);
           
            string valueStr = JsonConvert.SerializeObject(new
            {
                msg = msg
            });

            //单线程从webhook log表中拿数据，每个周期根据next post time值拿一定数量(如100个)，构建list。 下个周期拿的是另外100个。
            //这里模拟构建50个webhook。
            List<WebhookStruct> list = new List<WebhookStruct>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new WebhookStruct {
                    eventId= i,
                    webhookURL = webhookURL,
                    payload = valueStr
                });
            }
            //多线程去发送webhook, 超时时间为15s,所以这50个webhook log必然有结果，
            //失败去更新webhook log表的last&next post time，成功则删除。
            BatchPostWebhook(list);
            LogHelper.Error("TriggerWebhook ended...");


            //Task<HttpResponseMessage> task = PostAsync2(webhookURL, valueStr);
            //task.ContinueWith(p =>
            //{
            //    try
            //    {
            //        if (p.Result.IsSuccessStatusCode)
            //        {
            //            //webhook发送成功，判断response中是否带有扫描结果。
            //            var res = p.Result.Content.ReadAsStringAsync().Result;
            //            var theadId = Thread.CurrentThread.ManagedThreadId;
            //            LogHelper.Error(res);
            //            LogHelper.Error("result thread:" + theadId);
            //        }
            //        else
            //        {
            //            LogHelper.Error(p.Result.ReasonPhrase);
            //            //webhook 发送异常， URL错误，或者对方内部程序抛出异常！
            //            //todo:重发，或者存进webhook log 表。
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.WriteExceptionLog(ex);
            //        //webhook 发送异常， URL错误，或者对方内部程序抛出异常！
            //        //todo:重发，或者存进webhook log 表。
            //    }

            //});

            //Task<HttpResponseMessage> task2 = PostAsync2(webhookURL, valueStr2);
            //task2.ContinueWith(p =>
            //{
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    LogHelper.Error(res);
            //    var theadId2 = Thread.CurrentThread.ManagedThreadId;
            //    LogHelper.Error("result thread2: " + theadId2);
            //});


            //PostAsync(webhookURL, valueStr4); 
            return Content("api response");
        } 

        //这个方法不能收到response，因为httpclient已经释放掉了。
        private static void PostAsync(string url, string paramJson)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpContent content = new StringContent(paramJson);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var req = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = content,
                    };
                    LogHelper.Error("post webhook: " + paramJson);
                    var res = client.SendAsync(req);
                    if (res.Result.IsSuccessStatusCode)
                    {
                        var res1 = res.Result.Content.ReadAsStringAsync().Result;
                        LogHelper.Error(res1);
                        //return res;
                    }
                    else
                    {
                        throw new Exception(res.Result.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteExceptionLog(ex);
               //发送失败， URL错误，或者对方内部程序抛出异常。
               //重发， 或者 存进webhook log 表。
            }

        }

        //这个方法能收到response，
        private static Task<HttpResponseMessage> PostAsync2(string url, string paramJson)
        {
            HttpContent content = new StringContent(paramJson);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpClient client = new HttpClient { Timeout = new TimeSpan(0, 0, 3) }; //设置每次webhook的超时时间为3秒。
            return client.PostAsync(url, content);
        }

        //批量发送webhook， 每批发送20个。
        private static void BatchPostWebhook(List<WebhookStruct> list)
        {
            int i = 1;
            foreach(var item in list)
            {  
                Task<HttpResponseMessage> task = PostAsync2(item.webhookURL, item.payload);
                //处理response结果。
                task.ContinueWith(p =>
                {
                    i++;
                    try
                    {
                        if (p.Result.IsSuccessStatusCode)
                        {
                            var theadId = Thread.CurrentThread.ManagedThreadId;
                            //webhook发送成功，判断response中是否带有扫描结果, 如果有扫描结果则对结果进行更新，如果没有结果就不作处理。
                            var res = p.Result.Content.ReadAsStringAsync().Result;
                            LogHelper.Error("success, result thread:" + theadId + item.eventId + ": " + res); 
                        }
                        else
                        {
                            LogHelper.Error(p.Result.ReasonPhrase);
                            //webhook 发送异常， URL错误，或者对方内部程序抛出异常，或者超时
                            //todo: 更新webhook log表。 
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(item.eventId +": "+ ex.Message);
                        //webhook 发送异常。
                        //todo:重发，或者存进webhook log 表。
                    }
                });
            }
            while (true)
            {
                if (i >= 50)
                {
                    LogHelper.Error("BatchPostWebhook ended...i=" + i);
                    return;
                } 
            }
        }
    }

    public class WebhookStruct
    {
        public int eventId { get; set; }
        public string webhookURL { get; set; }
        public string payload { get; set; }
        //public int status { get; set; } //0： 未发送， 1：正在发送， 2：已经发送
    }
}
