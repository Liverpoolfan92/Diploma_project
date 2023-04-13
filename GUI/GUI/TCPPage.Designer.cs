namespace GUI
{
    partial class TCPPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcp_seqNum = new System.Windows.Forms.TextBox();
            this.ippayload = new System.Windows.Forms.Label();
            this.tcp_ttl = new System.Windows.Forms.TextBox();
            this.ttl = new System.Windows.Forms.Label();
            this.tcp_button = new System.Windows.Forms.Button();
            this.tcp_dstPort = new System.Windows.Forms.TextBox();
            this.destinationport = new System.Windows.Forms.Label();
            this.sourceport = new System.Windows.Forms.Label();
            this.tcp_dstIp = new System.Windows.Forms.TextBox();
            this.tcp_srcMac = new System.Windows.Forms.TextBox();
            this.tcp_dstMac = new System.Windows.Forms.TextBox();
            this.tcp_srcPort = new System.Windows.Forms.TextBox();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.sourceIP = new System.Windows.Forms.Label();
            this.tcp_srcIp = new System.Windows.Forms.TextBox();
            this.tcp_flags = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcp_winSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tcp_ackNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tcp_payload = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tcp_seqNum
            // 
            this.tcp_seqNum.Location = new System.Drawing.Point(173, 222);
            this.tcp_seqNum.Name = "tcp_seqNum";
            this.tcp_seqNum.Size = new System.Drawing.Size(172, 23);
            this.tcp_seqNum.TabIndex = 46;
            // 
            // ippayload
            // 
            this.ippayload.AutoSize = true;
            this.ippayload.Location = new System.Drawing.Point(12, 222);
            this.ippayload.Name = "ippayload";
            this.ippayload.Size = new System.Drawing.Size(106, 15);
            this.ippayload.TabIndex = 45;
            this.ippayload.Text = "Sequence number:";
            // 
            // tcp_ttl
            // 
            this.tcp_ttl.Location = new System.Drawing.Point(173, 193);
            this.tcp_ttl.Name = "tcp_ttl";
            this.tcp_ttl.Size = new System.Drawing.Size(172, 23);
            this.tcp_ttl.TabIndex = 44;
            // 
            // ttl
            // 
            this.ttl.AutoSize = true;
            this.ttl.Location = new System.Drawing.Point(12, 193);
            this.ttl.Name = "ttl";
            this.ttl.Size = new System.Drawing.Size(71, 15);
            this.ttl.TabIndex = 43;
            this.ttl.Text = "Time to live:";
            // 
            // tcp_button
            // 
            this.tcp_button.Location = new System.Drawing.Point(12, 380);
            this.tcp_button.Name = "tcp_button";
            this.tcp_button.Size = new System.Drawing.Size(323, 58);
            this.tcp_button.TabIndex = 42;
            this.tcp_button.Text = "Send";
            this.tcp_button.UseVisualStyleBackColor = true;
            this.tcp_button.Click += new System.EventHandler(this.tcp_button_Click);
            // 
            // tcp_dstPort
            // 
            this.tcp_dstPort.Location = new System.Drawing.Point(173, 164);
            this.tcp_dstPort.Name = "tcp_dstPort";
            this.tcp_dstPort.Size = new System.Drawing.Size(172, 23);
            this.tcp_dstPort.TabIndex = 41;
            // 
            // destinationport
            // 
            this.destinationport.AutoSize = true;
            this.destinationport.Location = new System.Drawing.Point(12, 164);
            this.destinationport.Name = "destinationport";
            this.destinationport.Size = new System.Drawing.Size(95, 15);
            this.destinationport.TabIndex = 40;
            this.destinationport.Text = "Destination Port:";
            // 
            // sourceport
            // 
            this.sourceport.AutoSize = true;
            this.sourceport.Location = new System.Drawing.Point(12, 135);
            this.sourceport.Name = "sourceport";
            this.sourceport.Size = new System.Drawing.Size(71, 15);
            this.sourceport.TabIndex = 39;
            this.sourceport.Text = "Source Port:";
            // 
            // tcp_dstIp
            // 
            this.tcp_dstIp.Location = new System.Drawing.Point(173, 48);
            this.tcp_dstIp.Name = "tcp_dstIp";
            this.tcp_dstIp.Size = new System.Drawing.Size(172, 23);
            this.tcp_dstIp.TabIndex = 38;
            // 
            // tcp_srcMac
            // 
            this.tcp_srcMac.Location = new System.Drawing.Point(173, 77);
            this.tcp_srcMac.Name = "tcp_srcMac";
            this.tcp_srcMac.Size = new System.Drawing.Size(172, 23);
            this.tcp_srcMac.TabIndex = 37;
            // 
            // tcp_dstMac
            // 
            this.tcp_dstMac.Location = new System.Drawing.Point(173, 106);
            this.tcp_dstMac.Name = "tcp_dstMac";
            this.tcp_dstMac.Size = new System.Drawing.Size(172, 23);
            this.tcp_dstMac.TabIndex = 36;
            // 
            // tcp_srcPort
            // 
            this.tcp_srcPort.Location = new System.Drawing.Point(173, 135);
            this.tcp_srcPort.Name = "tcp_srcPort";
            this.tcp_srcPort.Size = new System.Drawing.Size(172, 23);
            this.tcp_srcPort.TabIndex = 35;
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 106);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 34;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 77);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 33;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 48);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 32;
            this.destinationIP.Text = "Destination IP:";
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 19);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 31;
            this.sourceIP.Text = "Source IP:";
            // 
            // tcp_srcIp
            // 
            this.tcp_srcIp.Location = new System.Drawing.Point(173, 19);
            this.tcp_srcIp.Name = "tcp_srcIp";
            this.tcp_srcIp.Size = new System.Drawing.Size(172, 23);
            this.tcp_srcIp.TabIndex = 30;
            // 
            // tcp_flags
            // 
            this.tcp_flags.Location = new System.Drawing.Point(173, 309);
            this.tcp_flags.Name = "tcp_flags";
            this.tcp_flags.Size = new System.Drawing.Size(172, 23);
            this.tcp_flags.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 47;
            this.label1.Text = "Flags:";
            // 
            // tcp_winSize
            // 
            this.tcp_winSize.Location = new System.Drawing.Point(173, 280);
            this.tcp_winSize.Name = "tcp_winSize";
            this.tcp_winSize.Size = new System.Drawing.Size(172, 23);
            this.tcp_winSize.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 49;
            this.label2.Text = "Window size:";
            // 
            // tcp_ackNum
            // 
            this.tcp_ackNum.Location = new System.Drawing.Point(173, 251);
            this.tcp_ackNum.Name = "tcp_ackNum";
            this.tcp_ackNum.Size = new System.Drawing.Size(172, 23);
            this.tcp_ackNum.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 15);
            this.label3.TabIndex = 51;
            this.label3.Text = "Acknowledgement number:";
            // 
            // tcp_payload
            // 
            this.tcp_payload.Location = new System.Drawing.Point(173, 338);
            this.tcp_payload.Name = "tcp_payload";
            this.tcp_payload.Size = new System.Drawing.Size(172, 23);
            this.tcp_payload.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 338);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 53;
            this.label4.Text = "Payload:";
            // 
            // TCPPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 450);
            this.Controls.Add(this.tcp_payload);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tcp_ackNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tcp_winSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tcp_flags);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tcp_seqNum);
            this.Controls.Add(this.ippayload);
            this.Controls.Add(this.tcp_ttl);
            this.Controls.Add(this.ttl);
            this.Controls.Add(this.tcp_button);
            this.Controls.Add(this.tcp_dstPort);
            this.Controls.Add(this.destinationport);
            this.Controls.Add(this.sourceport);
            this.Controls.Add(this.tcp_dstIp);
            this.Controls.Add(this.tcp_srcMac);
            this.Controls.Add(this.tcp_dstMac);
            this.Controls.Add(this.tcp_srcPort);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.tcp_srcIp);
            this.Name = "TCPPage";
            this.Text = "TCPPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tcp_seqNum;
        private Label ippayload;
        private TextBox tcp_ttl;
        private Label ttl;
        private Button tcp_button;
        private TextBox tcp_dstPort;
        private Label destinationport;
        private Label sourceport;
        private TextBox tcp_dstIp;
        private TextBox tcp_srcMac;
        private TextBox tcp_dstMac;
        private TextBox tcp_srcPort;
        private Label destinationMAC;
        private Label SourceMAC;
        private Label destinationIP;
        private Label sourceIP;
        private TextBox tcp_srcIp;
        private TextBox tcp_flags;
        private Label label1;
        private TextBox tcp_winSize;
        private Label label2;
        private TextBox tcp_ackNum;
        private Label label3;
        private TextBox tcp_payload;
        private Label label4;
    }
}