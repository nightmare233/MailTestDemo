using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebAPITest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public ActionResult ReceiveNotify(string msg)
        {
            //System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory +  @"\logs\log1.log.", "I receive your message. time:" + DateTime.Now.ToLongTimeString());
            
            Random random = new Random();
            int i = random.Next(3, 10);

            Thread.Sleep(1000 * i); //15s
            var result = new
            {
                code = i,
                message = msg
            };
            return Json(result);
        }
    }
}
