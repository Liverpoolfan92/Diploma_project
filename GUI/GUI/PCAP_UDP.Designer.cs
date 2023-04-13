namespace GUI
{
    partial class PCAP_UDP
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
            this.pcap_udp_payload = new System.Windows.Forms.TextBox();
            this.udppayload = new System.Windows.Forms.Label();
            this.udp_button = new System.Windows.Forms.Button();
            this.pcap_udp_dstPort = new System.Windows.Forms.TextBox();
            this.destinationport = new System.Windows.Forms.Label();
            this.sourceport = new System.Windows.Forms.Label();
            this.pcap_udp_dstIp = new System.Windows.Forms.TextBox();
            this.pcap_udp_srcMac = new System.Windows.Forms.TextBox();
            this.pcap_udp_dstMac = new System.Windows.Forms.TextBox();
            this.pcap_udp_srcPort = new System.Windows.Forms.TextBox();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.sourceIP = new System.Windows.Forms.Label();
            this.pcap_udp_srcIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pcap_udp_payload
            // 
            this.pcap_udp_payload.Location = new System.Drawing.Point(163, 193);
            this.pcap_udp_payload.Name = "pcap_udp_payload";
            this.pcap_udp_payload.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_payload.TabIndex = 61;
            // 
            // udppayload
            // 
            this.udppayload.AutoSize = true;
            this.udppayload.Location = new System.Drawing.Point(12, 193);
            this.udppayload.Name = "udppayload";
            this.udppayload.Size = new System.Drawing.Size(52, 15);
            this.udppayload.TabIndex = 60;
            this.udppayload.Text = "Payload:";
            // 
            // udp_button
            // 
            this.udp_button.Location = new System.Drawing.Point(12, 320);
            this.udp_button.Name = "udp_button";
            this.udp_button.Size = new System.Drawing.Size(323, 58);
            this.udp_button.TabIndex = 59;
            this.udp_button.Text = "Send";
            this.udp_button.UseVisualStyleBackColor = true;
            this.udp_button.Click += new System.EventHandler(this.udp_button_Click);
            // 
            // pcap_udp_dstPort
            // 
            this.pcap_udp_dstPort.Location = new System.Drawing.Point(163, 164);
            this.pcap_udp_dstPort.Name = "pcap_udp_dstPort";
            this.pcap_udp_dstPort.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_dstPort.TabIndex = 58;
            // 
            // destinationport
            // 
            this.destinationport.AutoSize = true;
            this.destinationport.Location = new System.Drawing.Point(12, 164);
            this.destinationport.Name = "destinationport";
            this.destinationport.Size = new System.Drawing.Size(95, 15);
            this.destinationport.TabIndex = 57;
            this.destinationport.Text = "Destination Port:";
            // 
            // sourceport
            // 
            this.sourceport.AutoSize = true;
            this.sourceport.Location = new System.Drawing.Point(12, 135);
            this.sourceport.Name = "sourceport";
            this.sourceport.Size = new System.Drawing.Size(71, 15);
            this.sourceport.TabIndex = 56;
            this.sourceport.Text = "Source Port:";
            // 
            // pcap_udp_dstIp
            // 
            this.pcap_udp_dstIp.Location = new System.Drawing.Point(163, 48);
            this.pcap_udp_dstIp.Name = "pcap_udp_dstIp";
            this.pcap_udp_dstIp.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_dstIp.TabIndex = 55;
            // 
            // pcap_udp_srcMac
            // 
            this.pcap_udp_srcMac.Location = new System.Drawing.Point(163, 77);
            this.pcap_udp_srcMac.Name = "pcap_udp_srcMac";
            this.pcap_udp_srcMac.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_srcMac.TabIndex = 54;
            // 
            // pcap_udp_dstMac
            // 
            this.pcap_udp_dstMac.Location = new System.Drawing.Point(163, 106);
            this.pcap_udp_dstMac.Name = "pcap_udp_dstMac";
            this.pcap_udp_dstMac.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_dstMac.TabIndex = 53;
            // 
            // pcap_udp_srcPort
            // 
            this.pcap_udp_srcPort.Location = new System.Drawing.Point(163, 135);
            this.pcap_udp_srcPort.Name = "pcap_udp_srcPort";
            this.pcap_udp_srcPort.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_srcPort.TabIndex = 52;
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 106);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 51;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 77);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 50;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 48);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 49;
            this.destinationIP.Text = "Destination IP:";
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 19);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 48;
            this.sourceIP.Text = "Source IP:";
            // 
            // pcap_udp_srcIp
            // 
            this.pcap_udp_srcIp.Location = new System.Drawing.Point(163, 19);
            this.pcap_udp_srcIp.Name = "pcap_udp_srcIp";
            this.pcap_udp_srcIp.Size = new System.Drawing.Size(172, 23);
            this.pcap_udp_srcIp.TabIndex = 47;
            // 
            // PCAP_UDP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 450);
            this.Controls.Add(this.pcap_udp_payload);
            this.Controls.Add(this.udppayload);
            this.Controls.Add(this.udp_button);
            this.Controls.Add(this.pcap_udp_dstPort);
            this.Controls.Add(this.destinationport);
            this.Controls.Add(this.sourceport);
            this.Controls.Add(this.pcap_udp_dstIp);
            this.Controls.Add(this.pcap_udp_srcMac);
            this.Controls.Add(this.pcap_udp_dstMac);
            this.Controls.Add(this.pcap_udp_srcPort);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.pcap_udp_srcIp);
            this.Name = "PCAP_UDP";
            this.Text = "PCAP_UDP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox pcap_udp_payload;
        private Label udppayload;
        private Button udp_button;
        private TextBox pcap_udp_dstPort;
        private Label destinationport;
        private Label sourceport;
        private TextBox pcap_udp_dstIp;
        private TextBox pcap_udp_srcMac;
        private TextBox pcap_udp_dstMac;
        private TextBox pcap_udp_srcPort;
        private Label destinationMAC;
        private Label SourceMAC;
        private Label destinationIP;
        private Label sourceIP;
        private TextBox pcap_udp_srcIp;
    }
}