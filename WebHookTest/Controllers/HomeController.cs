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

        public ActionResult TriggerWebhook(int count)
        {
            string webhookURL = "http://localhost:82/Home/ReceiveNotify";
            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            LogHelper.Error("main thread:" + mainThreadId);

            //单线程从webhook log表中拿数据，根据next post time值拿一定数量(如100个)。
            //这里模拟构建100个webhook。
            List<WebhookStruct> list = new List<WebhookStruct>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new WebhookStruct
                {
                    eventId = i,
                    webhookURL = webhookURL,
                    payload = JsonConvert.SerializeObject(new
                    {
                        msg = "webhook " + i
                    })
                });
            }
            //发送webhook, 异步等待response， 超时时间为15s。失败去更新webhook log表的last&next post time，成功则删除。
            //所有这100个webhook都有返回了，这个方法才结束。 
            BatchPostWebhook(list);
            LogHelper.Error("TriggerWebhook ended...");
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

        //发送webhook，返回task
        private static Task<HttpResponseMessage> PostAsync2(string url, string paramJson)
        {
            HttpContent content = new StringContent(paramJson);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpClient client = new HttpClient { Timeout = new TimeSpan(0, 0, 60) }; //设置每次webhook的超时时间为60秒。
            return client.PostAsync(url, content);
        }

        //批量发送webhook， 异步等待response，所有response 返回才算结束。
        private static void BatchPostWebhook(List<WebhookStruct> list)
        {
            int i = 0;
            foreach(var item in list)
            {
                try
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
                                LogHelper.Error(p.Result.ReasonPhrase + "," + p.Result.Content);
                                //webhook 发送异常， URL错误，或者对方内部程序抛出异常，或者超时
                                //todo: 更新webhook log表。 
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(item.eventId + ": " + ex.Message);
                            //webhook 发送异常。
                            //todo:重发，或者存进webhook log 表。
                        }
                    });
                    Thread.Sleep(300);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(item.eventId + ": " + ex.Message);
                    //webhook 发送异常。
                    //todo:重发，或者存进webhook log 表。
                }

            }
            while (true)
            {
                if (i >= list.Count)
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
