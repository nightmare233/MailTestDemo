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
            MailBee.Global.LicenseKey = "MN110-BD758AFA74AB752575128ACF6CAE-EEE7"; //comm100 license
            try
            {
                Console.WriteLine("begin...");
                Pop3Server.ReceiveEmail();
                //ExchangeEmailServer exchange = new ExchangeEmailServer();
                //exchange.Test();
                //exchange.ReceiveEmail();
                //Pop3Server.SendEmail();
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
