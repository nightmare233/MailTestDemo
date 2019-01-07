using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using MailBee.Mime;
using MailBee.SmtpMail;

namespace MailDemo
{
    public class SMTPServer
    {
        private static string bodyCotent = "Frank Test Custom Set Message ID!";
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
    }
}
