using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.Mime;
using MailBee.Pop3Mail;

namespace MailDemo
{
    public class Pop3Server
    {
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
