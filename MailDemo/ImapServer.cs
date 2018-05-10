using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBee.ImapMail;
using MailBee.Mime;

namespace MailDemo
{
    public class IMAPServer
    {
        public static void TestReceiveEmail()
        {
            try
            {
                Imap imp = new Imap();
                imp.Connect("imap.126.com");
                imp.Login("frankfeng23@126.com", "Aa00000000");
                imp.SelectFolder("Inbox");
                MailMessageCollection msgs = imp.DownloadMessageHeaders(Imap.AllMessages, false);
                // POP3 version: msgs = pop.DownloadMessageHeaders();
                imp.Disconnect();

                foreach (MailMessage msg in msgs)
                {
                    Console.WriteLine("From: " + msg.From.ToString());
                    Console.WriteLine("To: " + msg.To.ToString());
                    Console.WriteLine("Subject: " + msg.Subject);
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                throw;    
            }
        }
    }
}
