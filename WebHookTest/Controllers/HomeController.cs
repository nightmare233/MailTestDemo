using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

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

            var param = new
            {
                msg = "web hook 1"
            };
            var param2 = new
            {
                msg = "web hook 2"
            };
            string valueStr = JsonConvert.SerializeObject(param);
            string valueStr2 = JsonConvert.SerializeObject(param2);
            Task<HttpResponseMessage> task = PostAsync(webhookURL, valueStr);
            task.ContinueWith(p =>
            {
                var res = p.Result.Content.ReadAsStringAsync().Result;
                var theadId = Thread.CurrentThread.ManagedThreadId;
            });

            //Task<HttpResponseMessage> task2 = PostAsync(webhookURL, valueStr2);
            //task2.ContinueWith(p =>
            //{ 
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    var theadId2 = Thread.CurrentThread.ManagedThreadId;
            //});
            return Content("api response");
        }

        public ActionResult TriggerWebhook2(string msg)
        {
            string webhookURL = "http://localhost:82/Home/ReceiveNotify";
            var param = new
            {
                msg = "web hook 1"
            };
            string valueStr = JsonConvert.SerializeObject(param);
            Task<HttpResponseMessage> task = PostAsync(webhookURL, valueStr);
            task.ContinueWith(p =>
            {
                //todo something.
                //Thread.Sleep(2000);
                var res = p.Result.Content.ReadAsStringAsync().Result;
            });
            return Content("OK");
        }


        private static Task<HttpResponseMessage> PostAsync(string url, string paramJson)
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
                    //req.Headers.Add("Authorization", $"Bearer {TokenManager.Instance.Token.AccessToken}");
                    return client.SendAsync(req);
                    //var res = client.SendAsync(req);
                    //if (res.Result.IsSuccessStatusCode)
                    //{
                    //    return res;
                    //}
                    //else
                    //{
                    //    throw new Exception(res.Result.ReasonPhrase);
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
