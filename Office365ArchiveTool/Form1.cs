using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Auth;
using Microsoft.Exchange.WebServices.Autodiscover;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace Office365ArchiveTool
{
    public partial class Form1 : Form
    {
        private ExchangeService _service;
        private bool status = true;
        private readonly string serverUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
        private FolderId archiveFolderId;
        private string message = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            lbl_message.Text = "start to connect the server...";
            status = true;
            try
            {
                if (ExchangeEmailServer())
                {
                    Thread thread = new Thread(Archive);
                    thread.Start();
                }
                //if (ExchangeEmailServer())
                //{
                //    Archive();
                //} 
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
  
        private void btn_close_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        public void Archive()
        {
            if (!status)
            {
                message = "stoped.";
                return;
            }


            DateTime date = DateTime.Parse(txb_date.Text.Trim());
            int count = int.Parse(txb_count.Text.Trim());
            int days = int.Parse(txb_days.Text.Trim());
            while (date < DateTime.Now.AddDays(-90)) //3 months
            {
                message = "archive date: " + date.ToShortDateString();
                MoveBeforeDateArchive(date, count, days);
                date = date.AddDays(-1 * days);
            }
        }

        private bool ExchangeEmailServer()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                {
                    return true;
                }
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
            NetworkCredential networkCredential = new NetworkCredential(this.txb_email.Text, txb_pwd.Text);
            _service.Credentials = networkCredential;
            _service.TraceEnabled = false;
            if (string.IsNullOrEmpty(serverUrl))
            {
                _service.AutodiscoverUrl(txb_email.Text);
            }
            else
            {
                _service.Url = new Uri(serverUrl);
            }
            try
            {
                //var itemChanges = _service.SyncFolderItems(new FolderId(WellKnownFolderName.Inbox), PropertySet.IdOnly, null, 50, SyncFolderItemsScope.NormalItems, "");
                FindFoldersResults aFolders = _service.FindFolders(WellKnownFolderName.MsgFolderRoot, new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "Archive"), new FolderView(10));
                archiveFolderId = aFolders.Folders[0].Id;
                message = "connected.";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                message = "email or password wrong.";
                return false;
            }
            return true;
        }

        public void MoveBeforeDateArchive(DateTime date, int count, int days)
        {
            try
            {

                SearchFilter.IsLessThanOrEqualTo beginfilter = new SearchFilter.IsLessThanOrEqualTo(ItemSchema.DateTimeReceived, date);
                SearchFilter.IsGreaterThanOrEqualTo endfilter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date.AddDays(-1 * days));
                SearchFilter.SearchFilterCollection searchFilterCollection = new SearchFilter.SearchFilterCollection(LogicalOperator.And, beginfilter, endfilter);
                if (_service != null)
                {
                    FindItemsResults<Item> findResults = _service.FindItems(WellKnownFolderName.Inbox, searchFilterCollection, new ItemView(count));
                    message = $"date: {date.ToShortDateString()}, count: {findResults.TotalCount}" + (findResults.TotalCount > 0 ? ", moving..." : ", skip...");
                    List<ItemId> itemids = new List<ItemId>();
                    foreach (var item in findResults)
                    {
                        itemids.Add(item.Id);
                    }
                    if (itemids.Count > 0)
                    {
                        _service.MoveItems(itemids, archiveFolderId);
                        message = $"move successful, then sleep 10s ...";
                        Thread.Sleep(10000); //10s
                    }
                    else
                    {
                        Thread.Sleep(500); //1s
                    }
                }
                else
                {
                    message = "can't connect to server.";
                }
            }
            catch (ServiceRequestException ex)
            {
                message = ex.Message;
                message = "try again...";
                MoveBeforeDateArchive(date, count, days);
            }
            catch (ServerBusyException ex)
            {
                message = ex.Message;
                message = "pause 120s ...";
                Thread.Sleep(120000); //120s
                message = "try again...";
                MoveBeforeDateArchive(date, count, days);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                throw;
            }
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_message.Text = message;
        }
    }
}
