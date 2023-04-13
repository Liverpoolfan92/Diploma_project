namespace GUI
{
    partial class UDPPage
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
            this.udp_payload = new System.Windows.Forms.TextBox();
            this.udppayload = new System.Windows.Forms.Label();
            this.udp_button = new System.Windows.Forms.Button();
            this.udp_dstPort = new System.Windows.Forms.TextBox();
            this.destinationport = new System.Windows.Forms.Label();
            this.sourceport = new System.Windows.Forms.Label();
            this.udp_dstIp = new System.Windows.Forms.TextBox();
            this.udp_srcMac = new System.Windows.Forms.TextBox();
            this.udp_dstMac = new System.Windows.Forms.TextBox();
            this.udp_srcPort = new System.Windows.Forms.TextBox();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.sourceIP = new System.Windows.Forms.Label();
            this.udp_srcIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // udp_payload
            // 
            this.udp_payload.Location = new System.Drawing.Point(163, 186);
            this.udp_payload.Name = "udp_payload";
            this.udp_payload.Size = new System.Drawing.Size(172, 23);
            this.udp_payload.TabIndex = 46;
            // 
            // udppayload
            // 
            this.udppayload.AutoSize = true;
            this.udppayload.Location = new System.Drawing.Point(12, 186);
            this.udppayload.Name = "udppayload";
            this.udppayload.Size = new System.Drawing.Size(52, 15);
            this.udppayload.TabIndex = 45;
            this.udppayload.Text = "Payload:";
            // 
            // udp_button
            // 
            this.udp_button.Location = new System.Drawing.Point(12, 313);
            this.udp_button.Name = "udp_button";
            this.udp_button.Size = new System.Drawing.Size(323, 58);
            this.udp_button.TabIndex = 42;
            this.udp_button.Text = "Send";
            this.udp_button.UseVisualStyleBackColor = true;
            this.udp_button.Click += new System.EventHandler(this.udp_button_Click);
            // 
            // udp_dstPort
            // 
            this.udp_dstPort.Location = new System.Drawing.Point(163, 157);
            this.udp_dstPort.Name = "udp_dstPort";
            this.udp_dstPort.Size = new System.Drawing.Size(172, 23);
            this.udp_dstPort.TabIndex = 41;
            // 
            // destinationport
            // 
            this.destinationport.AutoSize = true;
            this.destinationport.Location = new System.Drawing.Point(12, 157);
            this.destinationport.Name = "destinationport";
            this.destinationport.Size = new System.Drawing.Size(95, 15);
            this.destinationport.TabIndex = 40;
            this.destinationport.Text = "Destination Port:";
            // 
            // sourceport
            // 
            this.sourceport.AutoSize = true;
            this.sourceport.Location = new System.Drawing.Point(12, 128);
            this.sourceport.Name = "sourceport";
            this.sourceport.Size = new System.Drawing.Size(71, 15);
            this.sourceport.TabIndex = 39;
            this.sourceport.Text = "Source Port:";
            // 
            // udp_dstIp
            // 
            this.udp_dstIp.Location = new System.Drawing.Point(163, 41);
            this.udp_dstIp.Name = "udp_dstIp";
            this.udp_dstIp.Size = new System.Drawing.Size(172, 23);
            this.udp_dstIp.TabIndex = 38;
            // 
            // udp_srcMac
            // 
            this.udp_srcMac.Location = new System.Drawing.Point(163, 70);
            this.udp_srcMac.Name = "udp_srcMac";
            this.udp_srcMac.Size = new System.Drawing.Size(172, 23);
            this.udp_srcMac.TabIndex = 37;
            // 
            // udp_dstMac
            // 
            this.udp_dstMac.Location = new System.Drawing.Point(163, 99);
            this.udp_dstMac.Name = "udp_dstMac";
            this.udp_dstMac.Size = new System.Drawing.Size(172, 23);
            this.udp_dstMac.TabIndex = 36;
            // 
            // udp_srcPort
            // 
            this.udp_srcPort.Location = new System.Drawing.Point(163, 128);
            this.udp_srcPort.Name = "udp_srcPort";
            this.udp_srcPort.Size = new System.Drawing.Size(172, 23);
            this.udp_srcPort.TabIndex = 35;
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 99);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 34;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 70);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 33;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 41);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 32;
            this.destinationIP.Text = "Destination IP:";
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 12);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 31;
            this.sourceIP.Text = "Source IP:";
            // 
            // udp_srcIp
            // 
            this.udp_srcIp.Location = new System.Drawing.Point(163, 12);
            this.udp_srcIp.Name = "udp_srcIp";
            this.udp_srcIp.Size = new System.Drawing.Size(172, 23);
            this.udp_srcIp.TabIndex = 30;
            // 
            // UDPPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 450);
            this.Controls.Add(this.udp_payload);
            this.Controls.Add(this.udppayload);
            this.Controls.Add(this.udp_button);
            this.Controls.Add(this.udp_dstPort);
            this.Controls.Add(this.destinationport);
            this.Controls.Add(this.sourceport);
            this.Controls.Add(this.udp_dstIp);
            this.Controls.Add(this.udp_srcMac);
            this.Controls.Add(this.udp_dstMac);
            this.Controls.Add(this.udp_srcPort);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.udp_srcIp);
            this.Name = "UDPPage";
            this.Text = "UDPPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox udp_payload;
        private Label udppayload;
        private Button udp_button;
        private TextBox udp_dstPort;
        private Label destinationport;
        private Label sourceport;
        private TextBox udp_dstIp;
        private TextBox udp_srcMac;
        private TextBox udp_dstMac;
        private TextBox udp_srcPort;
        private Label destinationMAC;
        private Label SourceMAC;
        private Label destinationIP;
        private Label sourceIP;
        private TextBox udp_srcIp;
    }
}