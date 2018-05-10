namespace EmailSendTester
{
    partial class EmailSender
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_emailaddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_timer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_message = new System.Windows.Forms.Label();
            this.txb_subject = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txb_content = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_stop = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ltb_emaillist = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txb_sender = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txb_pwd = new System.Windows.Forms.TextBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Send an email";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(339, 387);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(161, 316);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(93, 23);
            this.btn_start.TabIndex = 1;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Receive Email Address:";
            // 
            // txb_emailaddress
            // 
            this.txb_emailaddress.Location = new System.Drawing.Point(224, 36);
            this.txb_emailaddress.Name = "txb_emailaddress";
            this.txb_emailaddress.Size = new System.Drawing.Size(246, 21);
            this.txb_emailaddress.TabIndex = 4;
            this.txb_emailaddress.Text = "fengchufu@126.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "sender timer interval(s): ";
            // 
            // txb_timer
            // 
            this.txb_timer.Location = new System.Drawing.Point(224, 85);
            this.txb_timer.Name = "txb_timer";
            this.txb_timer.Size = new System.Drawing.Size(246, 21);
            this.txb_timer.TabIndex = 6;
            this.txb_timer.Text = "5";
            this.txb_timer.TextChanged += new System.EventHandler(this.txb_timer_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "message:";
            // 
            // lbl_message
            // 
            this.lbl_message.AutoSize = true;
            this.lbl_message.Location = new System.Drawing.Point(196, 265);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(65, 12);
            this.lbl_message.TabIndex = 8;
            this.lbl_message.Text = "no content";
            // 
            // txb_subject
            // 
            this.txb_subject.Location = new System.Drawing.Point(224, 132);
            this.txb_subject.Name = "txb_subject";
            this.txb_subject.Size = new System.Drawing.Size(246, 21);
            this.txb_subject.TabIndex = 10;
            this.txb_subject.Text = "email subject";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "subject:";
            // 
            // txb_content
            // 
            this.txb_content.Location = new System.Drawing.Point(224, 174);
            this.txb_content.MaxLength = 327670;
            this.txb_content.Multiline = true;
            this.txb_content.Name = "txb_content";
            this.txb_content.Size = new System.Drawing.Size(246, 77);
            this.txb_content.TabIndex = 12;
            this.txb_content.Text = "test email content";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(159, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "content:";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(323, 316);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(101, 23);
            this.btn_stop.TabIndex = 13;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ltb_emaillist);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(522, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 229);
            this.panel1.TabIndex = 14;
            // 
            // ltb_emaillist
            // 
            this.ltb_emaillist.FormattingEnabled = true;
            this.ltb_emaillist.ItemHeight = 12;
            this.ltb_emaillist.Location = new System.Drawing.Point(5, 27);
            this.ltb_emaillist.Name = "ltb_emaillist";
            this.ltb_emaillist.Size = new System.Drawing.Size(219, 196);
            this.ltb_emaillist.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "sender email list:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(495, 287);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "sender email:";
            // 
            // txb_sender
            // 
            this.txb_sender.Location = new System.Drawing.Point(581, 284);
            this.txb_sender.Name = "txb_sender";
            this.txb_sender.Size = new System.Drawing.Size(165, 21);
            this.txb_sender.TabIndex = 16;
            this.txb_sender.Text = "frankfeng24@126.com";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(519, 321);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "password:";
            // 
            // txb_pwd
            // 
            this.txb_pwd.Location = new System.Drawing.Point(581, 318);
            this.txb_pwd.Name = "txb_pwd";
            this.txb_pwd.PasswordChar = '*';
            this.txb_pwd.Size = new System.Drawing.Size(165, 21);
            this.txb_pwd.TabIndex = 18;
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(563, 368);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 19;
            this.btn_add.Text = "Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // EmailSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 526);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.txb_pwd);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txb_sender);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.txb_content);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txb_subject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txb_timer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_emailaddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.close);
            this.Controls.Add(this.button1);
            this.Name = "EmailSender";
            this.Text = "EmailSender";
            this.Load += new System.EventHandler(this.EmailSender_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_emailaddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_timer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.TextBox txb_subject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txb_content;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox ltb_emaillist;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txb_sender;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txb_pwd;
        private System.Windows.Forms.Button btn_add;
    }
}

