using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Auth;
using Microsoft.Exchange.WebServices.Autodiscover;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Threading;

namespace MailDemo
{
    public class ExchangeEmailServer
    {
        private ExchangeService _service;
        private readonly string username = "support@comm100.com"; //"frank@comm100.com";
        //private readonly string password = "gvhldhhkdxtcnpvc"; //frank
        private readonly string password = "nfzhpbdsdvdbzryf"; //support
        //private readonly string password = "bssqhswzsqptjkfy"; //oden
        private readonly string domain = "";
        private readonly string serverUrl = "https://outlook.office365.com/EWS/Exchange.asmx"; 
       
        public ExchangeEmailServer()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
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

        public void ReceiveEmail()
        { 
            string syncState = "";
            ChangeCollection<ItemChange> itemChanges = null;
            //PropertySet properties = new PropertySet();
            //properties.();
            int count = 0;
            do
            {
                itemChanges = _service.SyncFolderItems(new FolderId(WellKnownFolderName.Inbox), PropertySet.IdOnly, null, 50, SyncFolderItemsScope.NormalItems, syncState);
                syncState = itemChanges.SyncState;
                count += itemChanges.Count;
                Console.WriteLine("itemchanges count : " + count);
                //Console.WriteLine("syncState: " + syncState);

                foreach (ItemChange itemChange in itemChanges)
                {
                    try
                    {
                        if (itemChange.ChangeType == ChangeType.Create)
                        {
                            EmailMessage emailMessage = EmailMessage.Bind(_service, itemChange.ItemId);
                            Console.WriteLine("create time: " + emailMessage.DateTimeCreated + ": " + emailMessage.Subject + ", from: " + emailMessage.From.Address);
                            //Console.WriteLine(emailMessage.Body);
                            //Console.WriteLine(count + ": " + itemChange.ItemId.UniqueId);
                        }
                        else
                        {
                            Console.WriteLine("type:" + itemChange.ChangeType.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            while (itemChanges.MoreChangesAvailable && count < 50);
           
        }

        public void GetMailsFromArchive(DateTime date, int count)
        {
            count = 10;
            SearchFilter.IsLessThanOrEqualTo filter = new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeReceived, date);

            if (_service != null)
            {
                FindFoldersResults aFolders = _service.FindFolders(WellKnownFolderName.MsgFolderRoot, new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "Archive"), new FolderView(10));
                if (aFolders.Folders.Count == 1)
                {
                    FindItemsResults<Item> findResults = _service.FindItems(aFolders.Folders[0].Id, filter, new ItemView(count));
                    Console.WriteLine("count: " + findResults.TotalCount);
                    int countTemp = 1;
                    foreach (Item item in findResults)
                    {
                        //item.Move(WellKnownFolderName.ArchiveMsgFolderRoot);
                        EmailMessage message = EmailMessage.Bind(_service, item.Id);
                        Console.WriteLine(countTemp++ + "creattime: " + message.DateTimeCreated + ": " + message.Subject + ", from: " + message.From);
                        //message.Move(WellKnownFolderName.ArchiveRoot);
                    }
                } 
               
            } 
        }

        /// <summary>
        /// 从当前日期开始往前move
        /// </summary>
        /// <param name="date"></param>
        /// <param name="count"></param>
        public void MoveBeforeDateArchive(DateTime date, int count)
        { 
            try
            {
                SearchFilter.IsLessThanOrEqualTo filter = new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeReceived, date);
                SearchFilter.IsGreaterThanOrEqualTo filter2 = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date.AddDays(-1));
                SearchFilter.SearchFilterCollection searchFilterCollection = new SearchFilter.SearchFilterCollection(LogicalOperator.And, filter,filter2); 
                if (_service != null)
                {
                    FindFoldersResults aFolders = _service.FindFolders(WellKnownFolderName.MsgFolderRoot, new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "Archive"), new FolderView(10));
                    if (aFolders.Folders.Count == 1)
                    {
                        FolderId archiveFolderId = aFolders.Folders[0].Id;
                        FindItemsResults<Item> findResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilterCollection, new ItemView(count));
                        Console.WriteLine($"date: {date.ToShortDateString()}, count: {findResults.TotalCount}");
                        List<ItemId> itemids = new List<ItemId>();
                        foreach (var item in findResults)
                        {
                            itemids.Add(item.Id);
                        }
                        if (itemids.Count > 0)
                        {
                            _service.MoveItems(itemids, archiveFolderId);
                            Console.WriteLine("move successful!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            } 
        }
        /// <summary>
        ///从当前日期开始往后move
        /// </summary>
        /// <param name="date"></param>
        /// <param name="count"></param>
        public void MoveAfterDateArchive(DateTime date, int count, int days)
        {
            try
            {
                SearchFilter.IsLessThanOrEqualTo filter = new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeReceived, date.AddDays(days));
                SearchFilter.IsGreaterThanOrEqualTo filter2 = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);
                SearchFilter.SearchFilterCollection searchFilterCollection = new SearchFilter.SearchFilterCollection(LogicalOperator.And, filter, filter2);
                if (_service != null)
                {
                    FindFoldersResults aFolders = _service.FindFolders(WellKnownFolderName.MsgFolderRoot, new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "Archive"), new FolderView(10));
                    if (aFolders.Folders.Count == 1)
                    {
                        FolderId archiveFolderId = aFolders.Folders[0].Id;

                        FindItemsResults<Item> findResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilterCollection, new ItemView(count));
                        Console.WriteLine($"date: {date.ToShortDateString()}, count: {findResults.TotalCount}");

                        List<ItemId> itemids = new List<ItemId>();
                        foreach (var item in findResults)
                        {
                            itemids.Add(item.Id);
                        }
                        if (itemids.Count > 0)
                        {
                            Console.WriteLine("moving ... ...");
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            _service.MoveItems(itemids, archiveFolderId);
                            sw.Stop();
                            //new Timer(new TimerCallback(timer_Elapsed), null, 0, 30000);

                            Console.WriteLine($"move successful after {sw.ElapsedMilliseconds} ms, then sleep 30s ...");

                            System.Threading.Thread.Sleep(30000); //30s
                        }

                    }
                }
            }
            catch (ServiceRequestException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("try again...");
                MoveAfterDateArchive(date, count, days);
            }
            catch(ServerBusyException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("sleep 120s ...");
                System.Threading.Thread.Sleep(120000); //120s
                Console.WriteLine("try again...");
                MoveAfterDateArchive(date, count, days);
            }
           
        }

        private static void timer_Elapsed(object sender)
        {
            int n = 1;
            if (n <= 30)
            {
                Console.Write(" " + n );
                n++;
            }
        }

        public void ReceiveEmailByFindItems(DateTime date, int count)
        { 
            SearchFilter.IsLessThanOrEqualTo filter = new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeReceived, date);
            if (_service != null)
            {
                FindItemsResults<Item> findResults = _service.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(count));
                int countTemp = 1;
                if (findResults.TotalCount > 0)
                {
                    foreach (Item item in findResults)
                    {
                        EmailMessage message = EmailMessage.Bind(_service, item.Id);
                        Console.WriteLine(countTemp++ + ": creattime: " + message.DateTimeCreated + ": " + message.Subject + ", from: " + message.From);
                    }
                }
                else
                {
                    Console.WriteLine("no email.");
                }
                
            }
        }

        public void GetOneEmail()
        {
            string unid = "AAMkAGU4NmMxODVjLTJlOWMtNGQyYS04ODYyLWJiNGM0MDIzZjEzZABGAAAAAADLfNrNVa5vR7EYJNNU6Av7BwBZkVbuY3S+RITdHy52vtjmAAAAAAEMAABZkVbuY3S+RITdHy52vtjmAAGKRZdzAAA=";
            var email =  EmailMessage.Bind(_service, unid);
            Console.WriteLine(email.Subject);
            Console.WriteLine(email.DateTimeCreated);
            Console.WriteLine(email.From);
            Console.WriteLine(email.Size);
            Console.WriteLine("body: " + email.Body.Text);
            if (email.Body.BodyType == BodyType.HTML)
            {
                PropertySet properties = new PropertySet(BasePropertySet.FirstClassProperties);
                properties.RequestedBodyType = BodyType.Text;
                EmailMessage textMessage = EmailMessage.Bind(_service, unid, properties);
                Console.WriteLine("text message: " + textMessage.Body.Text); 
            } 
        }

        public void SendEmail()
        {

        }

        public void Test()
        {
            Folder.Bind(_service, WellKnownFolderName.Inbox);
            Console.WriteLine("test success.");
        }
    }
}