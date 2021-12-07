using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data; 
using Microsoft.Exchange.WebServices.Autodiscover;
using System.Security.Cryptography.X509Certificates; 
using System.Net.Security;
using System.Diagnostics;
using System.Threading;
using System.Net.Mail;

namespace EmailSendTester
{
    public class EWSServerwith15
    { 
        private ExchangeService _service;
        private readonly string username = "frank.feng@comm100.com";//"letschat@comm100.com";"oden.wan@comm100.com";
        private readonly string password = "bchmywmyzkxvmcht";
            //"DeirkgeE4rgee"; //support dynamsoft
        //private readonly string password = "xpzyxfmcxlffpnkz"; //letschat
        //private readonly string password = "nfzhpbdsdvdbzryf"; //support
        //private readonly string password = "bssqhswzsqptjkfy"; //oden
        private readonly string domain = "";
        private readonly string serverUrl = "https://outlook.office365.com/EWS/Exchange.asmx";

        public EWSServerwith15()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                {
                    return true;
                }
                // If there are errors in the certificate chain, look at each error to determine the cause.
                if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                {
                    if (chain != null && chain.ChainStatus != null)
                    {
                        foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                        {
                            if ((certificate.Subject == certificate.Issuer) &&
                               (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                            {
                                // Self-signed certificates with an untrusted root are valid. 
                                continue;
                            }
                            else
                            {
                                if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                                {
                                    // If there are any other errors in the certificate chain, the certificate is invalid,
                                    // so the method returns false.
                                    return false;
                                }
                            }
                        }
                    }

                    // When processing reaches this line, the only errors in the certificate chain are 
                    // untrusted root errors for self-signed certificates. These certificates are valid
                    // for default Exchange server installations, so return true.
                    return true;
                }
                else
                {
                    return false;
                }
            };
            _service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            //_service = new ExchangeService();
            //_service.Credentials = new WebCredentials(username, password, domain);
            //_service.Credentials = new NetworkCredential(username, password);
            NetworkCredential networkCredential = new NetworkCredential(username, password);
            _service.Credentials = networkCredential;//new OAuthCredentials(networkCredential);
            _service.TraceEnabled = false;
            //_service.AutodiscoverUrl(username);
            if (string.IsNullOrEmpty(serverUrl))
            {
                _service.AutodiscoverUrl(username);
            }
            else
            {
                _service.Url = new Uri(serverUrl);
            }
            //string token = "";
            //OAuthCredentials oAuthCredentials = new OAuthCredentials(token); 
            //appIdentityToken.Validate()
        }


        public async Task<bool> SendEmail()
        {
            EmailMessage emailMessage = new EmailMessage(_service);
            string receiveEmailAddressList = "fengchufu@163.com";
            //@"fengchufu@126.com;
            //frank.feng@comm100.com;
            //oden.wan@comm100.com;
            //shelay.tao@comm100.com;
            //raymond.zhang@comm100.com;
            //shane.wang@comm100.com;
            //shelay111@163.com";

         
            
            if (!string.IsNullOrEmpty(receiveEmailAddressList))
            {
                var list = receiveEmailAddressList.Trim().Replace("\r", "").Replace("\n", "").Split(';');
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.Trim()))
                        emailMessage.ToRecipients.Add(new EmailAddress(item));
                }
            }
            emailMessage.Subject = $"performance test {DateTime.Now.ToShortDateString()}";
            emailMessage.Body = $"performance test content {DateTime.Now.ToShortDateString()}";
            emailMessage.Send();
            return true;
        }
         
      
        private static void timer_Elapsed(object sender)
        {
            int n = 1;
            if (n <= 30)
            {
                Console.Write(" " + n);
                n++;
            }
        } 
    }
}