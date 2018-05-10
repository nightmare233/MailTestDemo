using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace EmailSendTester
{
    public partial class EmailSender : Form
    {
        public static readonly int intervalSeconds = 4;
        public static string bodyCotent = "Frank test send an email";
        public int currentCount = 0;
        public static string[] emailList = new string[] {
            //"frankfeng23@126.com/Aa00000000",
            //""
        };
        public Dictionary<string, string> emailDic = new Dictionary<string, string> { };
        public EmailSender()
        {
            InitializeComponent();
        }

        public static void SendMail126()
        {
            string account = "frankfeng23";
            string password = "Aa00000000";

            MailMessage message = new MailMessage();
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

        public static void SendMail(string receiveEmailAddress, string subject, string body, string senderEmail, string password)
        {  
            MailMessage message = new MailMessage();
            message.From = new MailAddress(senderEmail);
            message.To.Add(new MailAddress(receiveEmailAddress));

            message.Subject = subject;
            message.BodyEncoding = System.Text.UnicodeEncoding.Unicode;
            //message.Body = body + Environment.NewLine + body;
            message.Body = "##-Please type your reply above this line-##" + "\n\r" + body;

            SmtpClient client = new SmtpClient("smtp."+ senderEmail.Split('@')[1], 25);
            string account = senderEmail.Split('@')[0];
            client.Credentials = new System.Net.NetworkCredential(account, password);
            client.EnableSsl = true;
            client.Send(message);
            //await client.SendMailAsync(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!CheckSenderList())
            {
                return;
            }
            SendAmailbyFirstSender();
        }

        private void SendAmailbyFirstSender()
        { 
            string email = this.ltb_emaillist.SelectedItem.ToString().Split('/')[0];
            string password = this.ltb_emaillist.SelectedItem.ToString().Split('/')[1];
            currentCount++;
            SendMail(this.txb_emailaddress.Text.Trim(), this.txb_subject.Text + " - " + currentCount, this.txb_content.Text, email, password);
            this.lbl_message.Text = "The first mail has been sent successfully!";
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckSenderList())
                {
                    return;
                }

                if (this.txb_timer.Text == string.Empty || int.Parse(this.txb_timer.Text.Trim()) <= intervalSeconds)
                {
                    this.lbl_message.Text = "timer interval should be larger than "+ intervalSeconds + "(s).";
                    return;
                }
                this.timer1.Enabled = true;
                this.timer1.Interval = int.Parse(this.txb_timer.Text) * 1000; //default 20s
                this.timer1.Start();
                this.lbl_message.Text = "started.";
                SendAmailbyFirstSender(); //立即发第一封信。
            }
            catch (Exception ex)
            {
                this.lbl_message.Text = ex.Message;
            }
        }
        private void btn_stop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.lbl_message.Text = "stoped.";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string email = this.ltb_emaillist.SelectedItem.ToString().Split('/')[0];
                string password = this.ltb_emaillist.SelectedItem.ToString().Split('/')[1];
                //循环是用发信人列表发信，避免频繁发信被封号。
                if (currentCount % ltb_emaillist.Items.Count == 0)
                {
                    this.ltb_emaillist.SelectedIndex = 0;
                }
                else
                {
                    this.ltb_emaillist.SelectedIndex++;
                }
                this.currentCount++;
                SendMail(this.txb_emailaddress.Text.Trim(), this.txb_subject.Text + " - " + currentCount, this.txb_content.Text, email, password);
                
                this.lbl_message.Text = "Already sent email count: " + currentCount;
            }
            catch (Exception ex)
            {
                this.lbl_message.Text = ex.Message;
            }
        }

        private void EmailSender_Load(object sender, EventArgs e)
        {
            CheckSenderList();
        }

        private bool CheckSenderList()
        {
            //发信人列表： 
            //this.emailDic.Add("frankfeng24@126.com", "Aa00000000");
            //foreach (KeyValuePair<string, string> item in emailDic)
            //{
            //    this.ltb_emaillist.Items.Add(item.Key + "/" + item.Value);

            //}
            if (this.ltb_emaillist.Items.Count < 1)
            {
                lbl_message.Text = "Please add sender email first at the right panel.";
                return false;
            }
            else
            {
                this.ltb_emaillist.SelectedIndex = 0;
                return true;
            }
        }

        private void txb_timer_TextChanged(object sender, EventArgs e)
        {
            if (this.txb_timer.Text == string.Empty || int.Parse(this.txb_timer.Text.Trim()) <= intervalSeconds)
            {
                this.lbl_message.Text = "timer interval should be larger than "+ intervalSeconds + "(s).";
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        { 
            if (txb_sender.Text.Trim() != string.Empty && txb_pwd.Text.Trim() != string.Empty)
            {
                this.ltb_emaillist.Items.Add(txb_sender.Text.Trim() + "/" + txb_pwd.Text.Trim());
            }
            this.ltb_emaillist.Refresh();
            txb_sender.Text = string.Empty;
            txb_pwd.Text = string.Empty;
        }
    }
}
