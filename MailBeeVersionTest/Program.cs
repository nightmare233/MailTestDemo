﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBeeVersionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MailBee.Global.LicenseKey = "MN110-BD758AFA74AB752575128ACF6CAE-EEE7"; //comm100 license
            //MailBee.Global.LicenseKey = "MN120-8E46462646F9467F4692147658F3-D41B";
            try
            {
                Console.WriteLine("begin...");
                Pop3Server.Pop3ReceiveEmail();
                //ExchangeEmailServer exchange = new ExchangeEmailServer();
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
