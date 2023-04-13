namespace GUI
{
    partial class PCAP_ICMPv4
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
            this.icmp_button = new System.Windows.Forms.Button();
            this.pcap_icmp_code = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pcap_icmp_dstIP = new System.Windows.Forms.TextBox();
            this.pcap_icmp_srcMac = new System.Windows.Forms.TextBox();
            this.pcap_icmp_dstMac = new System.Windows.Forms.TextBox();
            this.pcap_icmp_type = new System.Windows.Forms.TextBox();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.sourceIP = new System.Windows.Forms.Label();
            this.pcap_icmp_srcIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // icmp_button
            // 
            this.icmp_button.Location = new System.Drawing.Point(12, 203);
            this.icmp_button.Name = "icmp_button";
            this.icmp_button.Size = new System.Drawing.Size(323, 58);
            this.icmp_button.TabIndex = 25;
            this.icmp_button.Text = "Send";
            this.icmp_button.UseVisualStyleBackColor = true;
            this.icmp_button.Click += new System.EventHandler(this.icmp_button_Click);
            // 
            // pcap_icmp_code
            // 
            this.pcap_icmp_code.Location = new System.Drawing.Point(163, 159);
            this.pcap_icmp_code.Name = "pcap_icmp_code";
            this.pcap_icmp_code.Size = new System.Drawing.Size(172, 23);
            this.pcap_icmp_code.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Code:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "Type:";
            // 
            // pcap_icmp_dstIP
            // 
            this.pcap_icmp_dstIP.Location = new System.Drawing.Point(163, 43);
            this.pcap_icmp_dstIP.Name = "pcap_icmp_dstIP";
            this.pcap_icmp_dstIP.Size = new System.Drawing.Size(172, 23);
            this.pcap_icmp_dstIP.TabIndex = 21;
            // 
            // pcap_icmp_srcMac
            // 
            this.pcap_icmp_srcMac.Location = new System.Drawing.Point(163, 72);
            this.pcap_icmp_srcMac.Name = "pcap_icmp_srcMac";
            this.pcap_icmp_srcMac.Size = new System.Drawing.Size(172, 23);
            this.pcap_icmp_srcMac.TabIndex = 20;
            // 
            // pcap_icmp_dstMac
            // 
            this.pcap_icmp_dstMac.Location = new System.Drawing.Point(163, 101);
            this.pcap_icmp_dstMac.Name = "pcap_icmp_dstMac";
            this.pcap_icmp_dstMac.Size = new System.Drawing.Size(172, 23);
            this.pcap_icmp_dstMac.TabIndex = 19;
            // 
            // pcap_icmp_type
            // 
            this.pcap_icmp_type.Location = new System.Drawing.Point(163, 130);
            this.pcap_icmp_type.Name = "pcap_icmp_type";
            this.pcap_icmp_type.Size = new System.Drawing.Size(172, 23);
            this.pcap_icmp_type.TabIndex = 18;
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 101);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 17;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 72);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 16;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 43);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 15;
            this.destinationIP.Text = "Destination IP:";
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 14);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 14;
            this.sourceIP.Text = "Source IP:";
            // 
            // pcap_icmp_srcIP
            // 
            this.pcap_icmp_srcIP.Location = new System.Drawing.Point(163, 14);
            this.pcap_icmp_srcIP.Name = "pcap_icmp_srcIP";
            this.pcap_icmp_srcIP.Size = new System.Drawing.Size(172, 23);
            this.pcap_icmp_srcIP.TabIndex = 13;
            // 
            // PCAP_ICMPv4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 450);
            this.Controls.Add(this.icmp_button);
            this.Controls.Add(this.pcap_icmp_code);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pcap_icmp_dstIP);
            this.Controls.Add(this.pcap_icmp_srcMac);
            this.Controls.Add(this.pcap_icmp_dstMac);
            this.Controls.Add(this.pcap_icmp_type);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.pcap_icmp_srcIP);
            this.Name = "PCAP_ICMPv4";
            this.Text = "PCAP_ICMPv4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button icmp_button;
        private TextBox pcap_icmp_code;
        private Label label2;
        private Label label1;
        private TextBox pcap_icmp_dstIP;
        private TextBox pcap_icmp_srcMac;
        private TextBox pcap_icmp_dstMac;
        private TextBox pcap_icmp_type;
        private Label destinationMAC;
        private Label SourceMAC;
        private Label destinationIP;
        private Label sourceIP;
        private TextBox pcap_icmp_srcIP;
    }
}