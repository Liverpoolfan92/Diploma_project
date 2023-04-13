namespace GUI
{
    partial class IPPage
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
            this.ip_button = new System.Windows.Forms.Button();
            this.ip_dstPort = new System.Windows.Forms.TextBox();
            this.destinationport = new System.Windows.Forms.Label();
            this.sourceport = new System.Windows.Forms.Label();
            this.ip_dstIp = new System.Windows.Forms.TextBox();
            this.ip_srcMac = new System.Windows.Forms.TextBox();
            this.ip_dstMac = new System.Windows.Forms.TextBox();
            this.ip_srcPort = new System.Windows.Forms.TextBox();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.sourceIP = new System.Windows.Forms.Label();
            this.ip_srcIp = new System.Windows.Forms.TextBox();
            this.ttl = new System.Windows.Forms.Label();
            this.ip_ttl = new System.Windows.Forms.TextBox();
            this.ippayload = new System.Windows.Forms.Label();
            this.ip_payload = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ip_button
            // 
            this.ip_button.Location = new System.Drawing.Point(12, 319);
            this.ip_button.Name = "ip_button";
            this.ip_button.Size = new System.Drawing.Size(323, 58);
            this.ip_button.TabIndex = 25;
            this.ip_button.Text = "Send";
            this.ip_button.UseVisualStyleBackColor = true;
            this.ip_button.Click += new System.EventHandler(this.ip_button_Click);
            // 
            // ip_dstPort
            // 
            this.ip_dstPort.Location = new System.Drawing.Point(163, 163);
            this.ip_dstPort.Name = "ip_dstPort";
            this.ip_dstPort.Size = new System.Drawing.Size(172, 23);
            this.ip_dstPort.TabIndex = 24;
            // 
            // destinationport
            // 
            this.destinationport.AutoSize = true;
            this.destinationport.Location = new System.Drawing.Point(12, 163);
            this.destinationport.Name = "destinationport";
            this.destinationport.Size = new System.Drawing.Size(95, 15);
            this.destinationport.TabIndex = 23;
            this.destinationport.Text = "Destination Port:";
            // 
            // sourceport
            // 
            this.sourceport.AutoSize = true;
            this.sourceport.Location = new System.Drawing.Point(12, 134);
            this.sourceport.Name = "sourceport";
            this.sourceport.Size = new System.Drawing.Size(71, 15);
            this.sourceport.TabIndex = 22;
            this.sourceport.Text = "Source Port:";
            // 
            // ip_dstIp
            // 
            this.ip_dstIp.Location = new System.Drawing.Point(163, 47);
            this.ip_dstIp.Name = "ip_dstIp";
            this.ip_dstIp.Size = new System.Drawing.Size(172, 23);
            this.ip_dstIp.TabIndex = 21;
            // 
            // ip_srcMac
            // 
            this.ip_srcMac.Location = new System.Drawing.Point(163, 76);
            this.ip_srcMac.Name = "ip_srcMac";
            this.ip_srcMac.Size = new System.Drawing.Size(172, 23);
            this.ip_srcMac.TabIndex = 20;
            // 
            // ip_dstMac
            // 
            this.ip_dstMac.Location = new System.Drawing.Point(163, 105);
            this.ip_dstMac.Name = "ip_dstMac";
            this.ip_dstMac.Size = new System.Drawing.Size(172, 23);
            this.ip_dstMac.TabIndex = 19;
            // 
            // ip_srcPort
            // 
            this.ip_srcPort.Location = new System.Drawing.Point(163, 134);
            this.ip_srcPort.Name = "ip_srcPort";
            this.ip_srcPort.Size = new System.Drawing.Size(172, 23);
            this.ip_srcPort.TabIndex = 18;
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 105);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 17;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 76);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 16;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 47);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 15;
            this.destinationIP.Text = "Destination IP:";
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 18);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 14;
            this.sourceIP.Text = "Source IP:";
            // 
            // ip_srcIp
            // 
            this.ip_srcIp.Location = new System.Drawing.Point(163, 18);
            this.ip_srcIp.Name = "ip_srcIp";
            this.ip_srcIp.Size = new System.Drawing.Size(172, 23);
            this.ip_srcIp.TabIndex = 13;
            // 
            // ttl
            // 
            this.ttl.AutoSize = true;
            this.ttl.Location = new System.Drawing.Point(12, 192);
            this.ttl.Name = "ttl";
            this.ttl.Size = new System.Drawing.Size(71, 15);
            this.ttl.TabIndex = 26;
            this.ttl.Text = "Time to live:";
            // 
            // ip_ttl
            // 
            this.ip_ttl.Location = new System.Drawing.Point(163, 192);
            this.ip_ttl.Name = "ip_ttl";
            this.ip_ttl.Size = new System.Drawing.Size(172, 23);
            this.ip_ttl.TabIndex = 27;
            // 
            // ippayload
            // 
            this.ippayload.AutoSize = true;
            this.ippayload.Location = new System.Drawing.Point(12, 221);
            this.ippayload.Name = "ippayload";
            this.ippayload.Size = new System.Drawing.Size(52, 15);
            this.ippayload.TabIndex = 28;
            this.ippayload.Text = "Payload:";
            // 
            // ip_payload
            // 
            this.ip_payload.Location = new System.Drawing.Point(163, 221);
            this.ip_payload.Name = "ip_payload";
            this.ip_payload.Size = new System.Drawing.Size(172, 23);
            this.ip_payload.TabIndex = 29;
            // 
            // IPPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 450);
            this.Controls.Add(this.ip_payload);
            this.Controls.Add(this.ippayload);
            this.Controls.Add(this.ip_ttl);
            this.Controls.Add(this.ttl);
            this.Controls.Add(this.ip_button);
            this.Controls.Add(this.ip_dstPort);
            this.Controls.Add(this.destinationport);
            this.Controls.Add(this.sourceport);
            this.Controls.Add(this.ip_dstIp);
            this.Controls.Add(this.ip_srcMac);
            this.Controls.Add(this.ip_dstMac);
            this.Controls.Add(this.ip_srcPort);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.ip_srcIp);
            this.Name = "IPPage";
            this.Text = "IPPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ip_button;
        private TextBox ip_dstPort;
        private Label destinationport;
        private Label sourceport;
        private TextBox ip_dstIp;
        private TextBox ip_srcMac;
        private TextBox ip_dstMac;
        private TextBox ip_srcPort;
        private Label destinationMAC;
        private Label SourceMAC;
        private Label destinationIP;
        private Label sourceIP;
        private TextBox ip_srcIp;
        private Label ttl;
        private TextBox ip_ttl;
        private Label ippayload;
        private TextBox ip_payload;
    }
}