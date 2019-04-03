﻿using System;
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
            
            Task<HttpResponseMessage> task = PostAsync2(webhookURL, valueStr);
            task.ContinueWith(p =>
            {
                try
                {
                    if (p.Result.IsSuccessStatusCode)
                    {
                        var res = p.Result.Content.ReadAsStringAsync().Result;
                        var theadId = Thread.CurrentThread.ManagedThreadId;
                        LogHelper.Error(res);
                        LogHelper.Error("result thread:" + theadId);
                    }
                    else
                    {
                        LogHelper.Error(p.Result.ReasonPhrase);
                        //webhook 发送异常， URL错误，或者对方内部程序抛出异常！
                        //todo:重发，或者存进webhook log 表。
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteExceptionLog(ex);
                    //webhook 发送异常， URL错误，或者对方内部程序抛出异常！
                    //todo:重发，或者存进webhook log 表。
                }
                
            });

            //Task<HttpResponseMessage> task2 = PostAsync2(webhookURL, valueStr2);
            //task2.ContinueWith(p =>
            //{
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    LogHelper.Error(res);
            //    var theadId2 = Thread.CurrentThread.ManagedThreadId;
            //    LogHelper.Error("result thread2: " + theadId2);
            //});

            //Task<HttpResponseMessage> task3 = PostAsync2(webhookURL, valueStr3);
            //task3.ContinueWith(p =>
            //{
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    var theadId = Thread.CurrentThread.ManagedThreadId;
            //    LogHelper.Error(res);
            //    LogHelper.Error("result thread 3:" + theadId);
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

            HttpClient client = new HttpClient { Timeout = new TimeSpan(0, 0, 10 * 60) }; //10min
            LogHelper.Error("post webhook: " + paramJson);
            return client.PostAsync(url, content);
        }
    }
}