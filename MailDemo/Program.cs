using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MailDemo
{
    class Program
    {
       
        static void Main(string[] args)
        {
            MailBee.Global.LicenseKey = "MN110-D91111D8119D1153115F786B0AC5-BA28"; //2019-3-19 ~ 4-18 expired.
            try
            {
                Console.WriteLine("begin...");
                //Pop3Server.ReceiveEmail();
                //ExchangeEmailServer exchange = new ExchangeEmailServer();
                //exchange.ReceiveEmail();
                SMTPServer.SendMail126();
                Console.WriteLine("done!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Console.ReadKey();
            }
        }

    }
}
