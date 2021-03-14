using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.Mime;
using MailBee.Pop3Mail;
using MailBee.SmtpMail;

namespace MailDemo
{
    public class Pop3Server
    {
      
        public static void ReceiveEmail()
        {
            MailMessage msg;
            try
            {
                //msg = Pop3.QuickDownloadMessage("pop.126.com", "frankfeng24", "Aa00000000", 1);
                msg = Pop3.QuickDownloadMessage("pop.163.com", "fengchufu", "Weisheng0409", -1);
                if (msg != null)
                {
                    Console.WriteLine("Subject: " + msg.Subject);
                    Console.WriteLine("HtmlText: " + msg.BodyHtmlText);
                    Console.WriteLine("PlainText: " + msg.BodyPlainText);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            } 
        }
         
        public static void Pop3ReceiveEmail()
        {
            try
            {
                MailBee.Pop3Mail.Pop3 pop3 = new MailBee.Pop3Mail.Pop3();
                pop3.Log.Enabled = true;
                pop3.Log.Filename = @"log.txt";

                if (!pop3.IsConnected)
                {
                    pop3.Timeout = 10000;
                    //bool ifConnect = pop3.Connect("pop.gmail.com", 995);
                    //bool ifConnect = pop3.Connect("pop.163.com", 995);
                    //bool ifConnect = pop3.Connect("pop.126.com", 995);
                    pop3.SslMode = MailBee.Security.SslStartupMode.OnConnect;
                    bool ifConnect = pop3.Connect("outlook.office365.com", 995);
                    
                }
                if (!pop3.IsLoggedIn)
                {
                    //bool ifLogin = pop3.Login("customerservice@shopvintagebrand.com", "dcsrbisbxztxqqru");
                    //bool ifLogin = pop3.Login("fengchufu@gmail.com", "fcf.1130,gmail");
                    //bool ifLogin = pop3.Login("Oden.Wan@comm100.com", "cmljyggwcswhwlsz", MailBee.AuthenticationMethods.Auto
                        //, MailBee.AuthenticationOptions.PreferSimpleMethods, null);
                    //bool ifLogin = pop3.Login("fengchufu@126.com", "Aa000000");
                    bool ifLogin = pop3.Login("fengchufu@outlook.com", "Weisheng0409", MailBee.AuthenticationMethods.Auto
                        , MailBee.AuthenticationOptions.PreferSimpleMethods, null);
                }
                int total = pop3.InboxMessageCount;
                var ids = pop3.GetMessageUids();
                Console.WriteLine("Ids length: " + ids.Length);
               
                Console.WriteLine($"inbox count: {total}");
                //var lastMail = pop3.DownloadEntireMessage(-1);
                //Print(lastMail, 1);
                //Console.WriteLine($"*********************************************************");
                int number = 20;
                int count = total >= number ? number : total;  //如果少于100封信，则取全部，如果大于100封信，则取最新的100封。
                int beginIndex = count == number ? (total - number) : 1;
                MailBee.Mime.MailMessageCollection emails = pop3.DownloadEntireMessages(beginIndex, count);
                int index = 1;
                foreach (MailMessage mail in emails)
                {
                    Print(mail, index);
                    index += 1;
                }

            }
             catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public static void SendEmail()
        {
            MailBee.SmtpMail.Smtp smtp = new MailBee.SmtpMail.Smtp();
            MailBee.SmtpMail.SmtpServer server = new SmtpServer();
            server.Name = "smtp.126.com";
            server.Port = 25;
            server.AccountName = "frankfeng23";
            server.Password = "Aa00000000";
            server.AuthMethods = MailBee.AuthenticationMethods.SaslLogin;
            server.SslMode = MailBee.Security.SslStartupMode.UseStartTlsIfSupported;
            smtp.SmtpServers.Add(server);

            MailMessage message = new MailMessage();
            message.Subject = "test edited sender";
            message.To.Add("fengchufu@163.com");
            message.From.Email = "frankfeng23@126.com";
            message.BodyHtmlText = "test return path";
            message.Sender = "frankfeng23@example.com";
            smtp.Message = message;
            smtp.Send();

        }

        public static void Print(MailBee.Mime.MailMessage email, int index)
        {
            if (email != null)
            {
                string ss = $"Index:{index}, From:{email.From.Email}";
                //string ss = $"Index:{index}, Time:{email.DateReceived.ToShortDateString()}, Subject:{email.Subject}, From:{email.From.Email}";
                Console.WriteLine(ss);

                //ss +=  "\n";
                //System.IO.File.AppendAllText(@"subjects.txt", ss);
                Console.WriteLine($"UidOnServer: {email.UidOnServer}");
                Console.WriteLine($"MessageId: {email.MessageID}");
                //Console.WriteLine("**********************Headers：*****************************");
                //foreach (MailBee.Mime.Header header in email.Headers)
                //{
                //    Console.WriteLine(header.Name + ": " + header.Value);
                //}
                //Console.WriteLine("**********************Attachments：*****************************");
                //foreach (MailBee.Mime.Attachment att in email.Attachments)
                //{
                //    Console.WriteLine("attachment contentId: " + att.ContentID);
                //    Console.WriteLine("attachment ContentType: " + att.ContentType);
                //    Console.WriteLine("attachment Filename: " + att.Filename);
                //    Console.WriteLine("attachment FilenameOriginal: " + att.FilenameOriginal);
                //    Console.WriteLine("attachment IsInline: " + att.IsInline);
                //    Console.WriteLine("attachment AsMimePart: " + att.AsMimePart);
                //    Console.WriteLine("attachment ContentLocation: " + att.ContentLocation);
                //}
                //Console.WriteLine("*************************Body：**************************");
                //Console.WriteLine(email.BodyHtmlText);
                Console.WriteLine("-----------------------------------------------------------");
            }
        }

    }
}
