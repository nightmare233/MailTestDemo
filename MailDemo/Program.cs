﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using MailBee.Mime;
using MailBee.SmtpMail;

namespace MailDemo
{
    class Program
    {
        public static string bodyCotent = "Frank Test Custom Set Message ID!";
       
        static void Main(string[] args)
        {
            //MailBee.Global.LicenseKey = "MN110-24ECECA1EC25EC74EC0082EEF666-7441";
            try
            {
                //IMAPServer.TestReceiveEmail();
                //SendMail126();
                //SendMailWithTLS();
                //SendMailQQ();
                //SendMail163();
                //Console.WriteLine(EncodeUtil.GenSHA1("12345678"));
                //Console.WriteLine(EncodeUtil.GenSHA1("123456"));
                //Console.WriteLine("long date: "+ DateTime.Now.ToLongDateString());
                //Console.WriteLine("short date: " + DateTime.Now.ToString("yyyy-MM-dd-HH"));
                //Console.WriteLine("hour: " + DateTime.Now.Hour);
                //Pop3ReceiveEmail();
                ExchangeTest.TestExchange();
                Console.WriteLine("done!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Console.ReadKey();
            }
        }

        public static void SendMailWithTLS()
        {
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            string type = "tls";  //
            string body = "no ssl";
            MailBee.SmtpMail.Smtp smtp = new MailBee.SmtpMail.Smtp();
            MailBee.SmtpMail.SmtpServer smtpServer = new MailBee.SmtpMail.SmtpServer();
            switch (type)
            {
                case "ssl":
                    smtpServer.SslMode = MailBee.Security.SslStartupMode.OnConnect;
                    smtpServer.SslProtocol = MailBee.Security.SecurityProtocol.Auto;
                    smtpServer.Port = 465;
                    body = "with ssl";
                    break;
                case "tls":
                    smtpServer.SslMode = MailBee.Security.SslStartupMode.UseStartTlsIfSupported;
                    //smtpServer.SslProtocol = MailBee.Security.SecurityProtocol.Tls12;
                    smtpServer.Port = 587;
                    body = "with tls";
                    break;
                default:
                    smtpServer.Port = 25;
                    break;
            }
            MailBee.Mime.MailMessage message = new MailBee.Mime.MailMessage();
            BuildMessage("qq", message, smtpServer);
            smtpServer.AuthMethods = MailBee.AuthenticationMethods.SaslLogin;
            smtp.SmtpServers.Add(smtpServer);
            //bool con = smtp.Connect();
            //bool login = smtp.Login(); 
            message.To.Add(new EmailAddress("fengchufu@126.com"));
            message.BodyHtmlText = body;

            smtp.Message = message;
            smtp.Log.Enabled = true;   //写日志
            smtp.Log.Filename = @"F:\DevProject\MailTestDemo\log.txt";  
            smtp.Send();
        }

        public static void BuildMessage(string mailType, MailBee.Mime.MailMessage message, SmtpServer server)
        {
            switch (mailType)
            {
                case "126":

                    message.From = new EmailAddress("frankfeng23@126.com");
                    message.Subject = "Test Send from 126 Mail";
                    server.Name = "smtp.126.com";
                    server.AccountName = "frankfeng23";
                    server.Password = "Aa00000000";
                    break;
                case "qq":
                    message.From = new EmailAddress("306836903@qq.com");
                    message.Subject = "Test Send from qq Mail";
                    server.Name = "smtp.qq.com";
                    server.AccountName = "306836903";
                    server.Password = "iqrcxrkdlbsjcbdd";
                    break;
                case "gmail":
                    message.From = new EmailAddress("fengchufu@gmail.com");
                    message.Subject = "Test Send from gmail Mail";
                    server.Name = "smtp.gmail.com";
                    server.AccountName = "fengchufu";
                    server.Password = "fcf.1130,gmail";
                    break;
                default:
                    break;
            }
        }
        public void Test()
        {
            MailBee.Mime.MailMessage email = Pop3ReceiveEmail();
            if (email != null)
            {
                Console.WriteLine("Subject: " + email.Subject);
                Console.WriteLine("**********************Headers：*****************************");
                foreach (MailBee.Mime.Header header in email.Headers)
                {
                    Console.WriteLine(header.Name + ": " + header.Value);
                }
                Console.WriteLine("**********************Attachments：*****************************");
                foreach (MailBee.Mime.Attachment att in email.Attachments)
                {
                    Console.WriteLine("attachment contentId: " + att.ContentID);
                    Console.WriteLine("attachment ContentType: " + att.ContentType);
                    Console.WriteLine("attachment Filename: " + att.Filename);
                    Console.WriteLine("attachment FilenameOriginal: " + att.FilenameOriginal);
                    Console.WriteLine("attachment IsInline: " + att.IsInline);
                    Console.WriteLine("attachment AsMimePart: " + att.AsMimePart);
                    Console.WriteLine("attachment ContentLocation: " + att.ContentLocation);
                }
                Console.WriteLine("*************************Body：**************************");
                Console.WriteLine(email.BodyHtmlText);
            }
        }

        public static void SendMail126()
        {
            string account = "frankfeng23";
            string password = "Aa00000000";

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new MailAddress("frankfeng23@126.com");
            message.To.Add(new MailAddress("fengchufu@126.com"));

            message.Subject = "Test Send 126 Mail";
            message.BodyEncoding = System.Text.UTF8Encoding.UTF8;
            message.Body = bodyCotent;

            SmtpClient client = new SmtpClient("smtp.126.com", 25);
            client.Credentials = new System.Net.NetworkCredential(account, password);
            client.EnableSsl = true;
            client.Send(message);
        }

        public static void SendMailQQ()
        {
            string account = "306836903";
            string password = "fcf.1130,qq";

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new MailAddress("306836903@qq.com");
            message.To.Add(new MailAddress("fengchufu@126.com"));

            message.Subject = "Test Send Mail";
            message.Body = "Hello world!";

            SmtpClient client = new SmtpClient("smtp.qq.com", 25);
            client.Credentials = new System.Net.NetworkCredential(account, password);
            client.EnableSsl = true;
            client.Send(message);
        }

        public static void SendMail163()
        {
            string account = "fengchufu";
            string password = "fcf.1130,wy";

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new MailAddress("fengchufu@163.com");
            message.To.Add(new MailAddress("fengchufu@126.com"));
            //message.To.Add(new MailAddress("lolly.chen@comm100.com"));
            message.To.Add(new MailAddress("alex@comm100.com"));
            //string keys = message.Headers.GetKey(0);
            string[] allkeys = message.Headers.AllKeys;
            message.Headers.Add("x_name_fcf", "frank");
            message.Headers.Set("Message-ID", "ASDFGH-123456789-FrankSetMID");
            message.Subject = "Test Set Message ID";
            message.BodyEncoding = System.Text.UTF8Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Body = bodyCotent;

            SmtpClient client = new SmtpClient("smtp.163.com", 25);
            client.Credentials = new System.Net.NetworkCredential(account, password);
            client.EnableSsl = true;
            client.Send(message);
        }

        public static MailBee.Mime.MailMessage Pop3ReceiveEmail()
        {
            try
            {
                MailBee.Pop3Mail.Pop3 pop3 = new MailBee.Pop3Mail.Pop3();
                pop3.Log.Enabled = true;
                pop3.Log.Filename = @"F:\DevProject\MailTestDemo\log.txt"; 

                if (!pop3.IsConnected)
                {
                    bool ifConnect = pop3.Connect("pop3.126.com", 110);
                }
                if (!pop3.IsLoggedIn)
                {
                    bool ifLogin = pop3.Login("frankfeng23", "Aa00000000");
                }

                int count = pop3.InboxMessageCount;
                //MailBee.Mime.MailMessageCollection emails = pop3.DownloadEntireMessages(count-6,5); 
                return pop3.DownloadEntireMessage(-1);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
