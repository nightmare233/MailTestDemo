using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using MailBee.Mime;

namespace MailDemo
{
    class Program
    {
       public static string bodyCotent = "Frank Test Custom Set Message ID!" +
                "<br/>one custom field name: x-name-fcf" +
                "<br/>add one hidden string:[DQ759E-0P5X] at the bottom." +
                 "<br/>测试编码，加上中文。" +
                "<br/><span style='color:#FFFFFF'>[DQ759E-0P5X]</span>";


        static void Main(string[] args)
        {
            MailBee.Global.LicenseKey = "MN110-9E565627568556335608A32D4F47-FC3B";
            try
            {
                IMAPServer.TestReceiveEmail();
                //SendMail126();
                //SendMailQQ();
                //SendMail163();
                //Console.WriteLine(EncodeUtil.GenSHA1("12345678"));
                //Console.WriteLine(EncodeUtil.GenSHA1("123456"));
                //Console.WriteLine("Send Success!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.ReadKey();
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
            message.Headers.Add("x_name_fcf","frank");
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
