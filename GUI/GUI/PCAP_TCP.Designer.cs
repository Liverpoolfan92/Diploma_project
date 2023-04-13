namespace GUI
{
    partial class PCAP_TCP
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
            this.pcap_tcp_payload = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pcap_tcp_ackNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pcap_tcp_winSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pcap_tcp_flags = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pcap_tcp_seqNum = new System.Windows.Forms.TextBox();
            this.ippayload = new System.Windows.Forms.Label();
            this.pcap_tcp_ttl = new System.Windows.Forms.TextBox();
            this.ttl = new System.Windows.Forms.Label();
            this.tcp_button = new System.Windows.Forms.Button();
            this.pcap_tcp_dstPort = new System.Windows.Forms.TextBox();
            this.destinationport = new System.Windows.Forms.Label();
            this.sourceport = new System.Windows.Forms.Label();
            this.pcap_tcp_dstIp = new System.Windows.Forms.TextBox();
            this.pcap_tcp_srcMac = new System.Windows.Forms.TextBox();
            this.pcap_tcp_dstMac = new System.Windows.Forms.TextBox();
            this.pcap_tcp_srcPort = new System.Windows.Forms.TextBox();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.sourceIP = new System.Windows.Forms.Label();
            this.pcap_tcp_srcIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pcap_tcp_payload
            // 
            this.pcap_tcp_payload.Location = new System.Drawing.Point(173, 338);
            this.pcap_tcp_payload.Name = "pcap_tcp_payload";
            this.pcap_tcp_payload.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_payload.TabIndex = 79;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 338);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 78;
            this.label4.Text = "Payload:";
            // 
            // pcap_tcp_ackNum
            // 
            this.pcap_tcp_ackNum.Location = new System.Drawing.Point(173, 251);
            this.pcap_tcp_ackNum.Name = "pcap_tcp_ackNum";
            this.pcap_tcp_ackNum.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_ackNum.TabIndex = 77;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 15);
            this.label3.TabIndex = 76;
            this.label3.Text = "Acknowledgement number:";
            // 
            // pcap_tcp_winSize
            // 
            this.pcap_tcp_winSize.Location = new System.Drawing.Point(173, 280);
            this.pcap_tcp_winSize.Name = "pcap_tcp_winSize";
            this.pcap_tcp_winSize.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_winSize.TabIndex = 75;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 74;
            this.label2.Text = "Window size:";
            // 
            // pcap_tcp_flags
            // 
            this.pcap_tcp_flags.Location = new System.Drawing.Point(173, 309);
            this.pcap_tcp_flags.Name = "pcap_tcp_flags";
            this.pcap_tcp_flags.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_flags.TabIndex = 73;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 72;
            this.label1.Text = "Flags:";
            // 
            // pcap_tcp_seqNum
            // 
            this.pcap_tcp_seqNum.Location = new System.Drawing.Point(173, 222);
            this.pcap_tcp_seqNum.Name = "pcap_tcp_seqNum";
            this.pcap_tcp_seqNum.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_seqNum.TabIndex = 71;
            // 
            // ippayload
            // 
            this.ippayload.AutoSize = true;
            this.ippayload.Location = new System.Drawing.Point(12, 222);
            this.ippayload.Name = "ippayload";
            this.ippayload.Size = new System.Drawing.Size(106, 15);
            this.ippayload.TabIndex = 70;
            this.ippayload.Text = "Sequence number:";
            // 
            // pcap_tcp_ttl
            // 
            this.pcap_tcp_ttl.Location = new System.Drawing.Point(173, 193);
            this.pcap_tcp_ttl.Name = "pcap_tcp_ttl";
            this.pcap_tcp_ttl.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_ttl.TabIndex = 69;
            // 
            // ttl
            // 
            this.ttl.AutoSize = true;
            this.ttl.Location = new System.Drawing.Point(12, 193);
            this.ttl.Name = "ttl";
            this.ttl.Size = new System.Drawing.Size(71, 15);
            this.ttl.TabIndex = 68;
            this.ttl.Text = "Time to live:";
            // 
            // tcp_button
            // 
            this.tcp_button.Location = new System.Drawing.Point(12, 380);
            this.tcp_button.Name = "tcp_button";
            this.tcp_button.Size = new System.Drawing.Size(323, 58);
            this.tcp_button.TabIndex = 67;
            this.tcp_button.Text = "Send";
            this.tcp_button.UseVisualStyleBackColor = true;
            this.tcp_button.Click += new System.EventHandler(this.tcp_button_Click);
            // 
            // pcap_tcp_dstPort
            // 
            this.pcap_tcp_dstPort.Location = new System.Drawing.Point(173, 164);
            this.pcap_tcp_dstPort.Name = "pcap_tcp_dstPort";
            this.pcap_tcp_dstPort.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_dstPort.TabIndex = 66;
            // 
            // destinationport
            // 
            this.destinationport.AutoSize = true;
            this.destinationport.Location = new System.Drawing.Point(12, 164);
            this.destinationport.Name = "destinationport";
            this.destinationport.Size = new System.Drawing.Size(95, 15);
            this.destinationport.TabIndex = 65;
            this.destinationport.Text = "Destination Port:";
            // 
            // sourceport
            // 
            this.sourceport.AutoSize = true;
            this.sourceport.Location = new System.Drawing.Point(12, 135);
            this.sourceport.Name = "sourceport";
            this.sourceport.Size = new System.Drawing.Size(71, 15);
            this.sourceport.TabIndex = 64;
            this.sourceport.Text = "Source Port:";
            // 
            // pcap_tcp_dstIp
            // 
            this.pcap_tcp_dstIp.Location = new System.Drawing.Point(173, 48);
            this.pcap_tcp_dstIp.Name = "pcap_tcp_dstIp";
            this.pcap_tcp_dstIp.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_dstIp.TabIndex = 63;
            // 
            // pcap_tcp_srcMac
            // 
            this.pcap_tcp_srcMac.Location = new System.Drawing.Point(173, 77);
            this.pcap_tcp_srcMac.Name = "pcap_tcp_srcMac";
            this.pcap_tcp_srcMac.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_srcMac.TabIndex = 62;
            // 
            // pcap_tcp_dstMac
            // 
            this.pcap_tcp_dstMac.Location = new System.Drawing.Point(173, 106);
            this.pcap_tcp_dstMac.Name = "pcap_tcp_dstMac";
            this.pcap_tcp_dstMac.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_dstMac.TabIndex = 61;
            // 
            // pcap_tcp_srcPort
            // 
            this.pcap_tcp_srcPort.Location = new System.Drawing.Point(173, 135);
            this.pcap_tcp_srcPort.Name = "pcap_tcp_srcPort";
            this.pcap_tcp_srcPort.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_srcPort.TabIndex = 60;
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 106);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 59;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 77);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 58;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 48);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 57;
            this.destinationIP.Text = "Destination IP:";
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 19);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 56;
            this.sourceIP.Text = "Source IP:";
            // 
            // pcap_tcp_srcIp
            // 
            this.pcap_tcp_srcIp.Location = new System.Drawing.Point(173, 19);
            this.pcap_tcp_srcIp.Name = "pcap_tcp_srcIp";
            this.pcap_tcp_srcIp.Size = new System.Drawing.Size(172, 23);
            this.pcap_tcp_srcIp.TabIndex = 55;
            // 
            // PCAP_TCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 450);
            this.Controls.Add(this.pcap_tcp_payload);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pcap_tcp_ackNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pcap_tcp_winSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pcap_tcp_flags);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pcap_tcp_seqNum);
            this.Controls.Add(this.ippayload);
            this.Controls.Add(this.pcap_tcp_ttl);
            this.Controls.Add(this.ttl);
            this.Controls.Add(this.tcp_button);
            this.Controls.Add(this.pcap_tcp_dstPort);
            this.Controls.Add(this.destinationport);
            this.Controls.Add(this.sourceport);
            this.Controls.Add(this.pcap_tcp_dstIp);
            this.Controls.Add(this.pcap_tcp_srcMac);
            this.Controls.Add(this.pcap_tcp_dstMac);
            this.Controls.Add(this.pcap_tcp_srcPort);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.pcap_tcp_srcIp);
            this.Name = "PCAP_TCP";
            this.Text = "PCAP_TCP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox pcap_tcp_payload;
        private Label label4;
        private TextBox pcap_tcp_ackNum;
        private Label label3;
        private TextBox pcap_tcp_winSize;
        private Label label2;
        private TextBox pcap_tcp_flags;
        private Label label1;
        private TextBox pcap_tcp_seqNum;
        private Label ippayload;
        private TextBox pcap_tcp_ttl;
        private Label ttl;
        private Button tcp_button;
        private TextBox pcap_tcp_dstPort;
        private Label destinationport;
        private Label sourceport;
        private TextBox pcap_tcp_dstIp;
        private TextBox pcap_tcp_srcMac;
        private TextBox pcap_tcp_dstMac;
        private TextBox pcap_tcp_srcPort;
        private Label destinationMAC;
        private Label SourceMAC;
        private Label destinationIP;
        private Label sourceIP;
        private TextBox pcap_tcp_srcIp;
    }
}