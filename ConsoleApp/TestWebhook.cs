using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Comm100.Framework.Common;
using Newtonsoft.Json;

namespace ConsoleApp
{
    public class TestWebhook
    {
        public static void TriggerWebhook()
        {
            string webhookURL = "http://localhost:51968/Home/ReceiveNotify"; 
            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("main thread:" + mainThreadId);
            var param = new
            {
                msg = "web hook 13"
            };
            var param2 = new
            {
                msg = "web hook 23"
            };
            var param3 = new
            {
                msg = "james 24"
            };
            var param4 = new
            {
                msg = "oden 56"
            };
            string valueStr = JsonConvert.SerializeObject(param);
            string valueStr2 = JsonConvert.SerializeObject(param2);
            string valueStr3 = JsonConvert.SerializeObject(param3);
            string valueStr4 = JsonConvert.SerializeObject(param4);
            //Task<HttpResponseMessage> task = PostAsync2(webhookURL, valueStr);
            //task.ContinueWith(p =>
            //{
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    var theadId = Thread.CurrentThread.ManagedThreadId;
            //    Console.WriteLine(res);
            //    Console.WriteLine("result thread:" + theadId);
            //});

            //Task<HttpResponseMessage> task2 = PostAsync2(webhookURL, valueStr2);
            //task2.ContinueWith(p =>
            //{
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    Console.WriteLine(res);
            //    var theadId2 = Thread.CurrentThread.ManagedThreadId;
            //    Console.WriteLine("result thread2: " + theadId2);
            //});

            //Task<HttpResponseMessage> task3 = PostAsync2(webhookURL, valueStr3);
            //task3.ContinueWith(p =>
            //{
            //    var res = p.Result.Content.ReadAsStringAsync().Result;
            //    var theadId = Thread.CurrentThread.ManagedThreadId;
            //    Console.WriteLine(res);
            //    Console.WriteLine("result thread 3:" + theadId);
            //});

            PostAsync(webhookURL, valueStr4); 
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
                    //req.Headers.Add("Authorization", $"Bearer {TokenManager.Instance.Token.AccessToken}");
                    Console.WriteLine("post webhook: " + paramJson);
                    //return client.SendAsync(req);
                    var res = client.SendAsync(req);
                    if (res.Result.IsSuccessStatusCode)
                    {
                        var res1 = res.Result.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(res1);
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
               Console.Write(ex);
                throw;
            }

        }

        //这个方法能收到response，
        private static Task<HttpResponseMessage> PostAsync2(string url, string paramJson)
        {
            HttpContent content = new StringContent(paramJson);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpClient client = new HttpClient { Timeout = new TimeSpan(0, 0, 10 * 60) }; //10min
            Console.WriteLine("post webhook: " + paramJson);
            return client.PostAsync(url, content);
        }

    }
}
