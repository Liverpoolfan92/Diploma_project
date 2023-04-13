namespace GUI
{
    partial class ICMPv4Page
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
            this.icmp_srcIP = new System.Windows.Forms.TextBox();
            this.sourceIP = new System.Windows.Forms.Label();
            this.destinationIP = new System.Windows.Forms.Label();
            this.SourceMAC = new System.Windows.Forms.Label();
            this.destinationMAC = new System.Windows.Forms.Label();
            this.icmp_type = new System.Windows.Forms.TextBox();
            this.icmp_dstMac = new System.Windows.Forms.TextBox();
            this.icmp_srcMac = new System.Windows.Forms.TextBox();
            this.icmp_dstIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.icmp_code = new System.Windows.Forms.TextBox();
            this.icmp_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // icmp_srcIP
            // 
            this.icmp_srcIP.Location = new System.Drawing.Point(163, 12);
            this.icmp_srcIP.Name = "icmp_srcIP";
            this.icmp_srcIP.Size = new System.Drawing.Size(172, 23);
            this.icmp_srcIP.TabIndex = 0;
            // 
            // sourceIP
            // 
            this.sourceIP.AutoSize = true;
            this.sourceIP.Location = new System.Drawing.Point(12, 12);
            this.sourceIP.Name = "sourceIP";
            this.sourceIP.Size = new System.Drawing.Size(59, 15);
            this.sourceIP.TabIndex = 1;
            this.sourceIP.Text = "Source IP:";
            // 
            // destinationIP
            // 
            this.destinationIP.AutoSize = true;
            this.destinationIP.Location = new System.Drawing.Point(12, 41);
            this.destinationIP.Name = "destinationIP";
            this.destinationIP.Size = new System.Drawing.Size(83, 15);
            this.destinationIP.TabIndex = 2;
            this.destinationIP.Text = "Destination IP:";
            // 
            // SourceMAC
            // 
            this.SourceMAC.AutoSize = true;
            this.SourceMAC.Location = new System.Drawing.Point(12, 70);
            this.SourceMAC.Name = "SourceMAC";
            this.SourceMAC.Size = new System.Drawing.Size(76, 15);
            this.SourceMAC.TabIndex = 3;
            this.SourceMAC.Text = "Source MAC:";
            // 
            // destinationMAC
            // 
            this.destinationMAC.AutoSize = true;
            this.destinationMAC.Location = new System.Drawing.Point(12, 99);
            this.destinationMAC.Name = "destinationMAC";
            this.destinationMAC.Size = new System.Drawing.Size(100, 15);
            this.destinationMAC.TabIndex = 4;
            this.destinationMAC.Text = "Destination MAC:";
            // 
            // icmp_type
            // 
            this.icmp_type.Location = new System.Drawing.Point(163, 128);
            this.icmp_type.Name = "icmp_type";
            this.icmp_type.Size = new System.Drawing.Size(172, 23);
            this.icmp_type.TabIndex = 5;
            // 
            // icmp_dstMac
            // 
            this.icmp_dstMac.Location = new System.Drawing.Point(163, 99);
            this.icmp_dstMac.Name = "icmp_dstMac";
            this.icmp_dstMac.Size = new System.Drawing.Size(172, 23);
            this.icmp_dstMac.TabIndex = 6;
            // 
            // icmp_srcMac
            // 
            this.icmp_srcMac.Location = new System.Drawing.Point(163, 70);
            this.icmp_srcMac.Name = "icmp_srcMac";
            this.icmp_srcMac.Size = new System.Drawing.Size(172, 23);
            this.icmp_srcMac.TabIndex = 7;
            // 
            // icmp_dstIP
            // 
            this.icmp_dstIP.Location = new System.Drawing.Point(163, 41);
            this.icmp_dstIP.Name = "icmp_dstIP";
            this.icmp_dstIP.Size = new System.Drawing.Size(172, 23);
            this.icmp_dstIP.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Code:";
            // 
            // icmp_code
            // 
            this.icmp_code.Location = new System.Drawing.Point(163, 157);
            this.icmp_code.Name = "icmp_code";
            this.icmp_code.Size = new System.Drawing.Size(172, 23);
            this.icmp_code.TabIndex = 11;
            // 
            // icmp_button
            // 
            this.icmp_button.Location = new System.Drawing.Point(12, 201);
            this.icmp_button.Name = "icmp_button";
            this.icmp_button.Size = new System.Drawing.Size(323, 58);
            this.icmp_button.TabIndex = 12;
            this.icmp_button.Text = "Send";
            this.icmp_button.UseVisualStyleBackColor = true;
            this.icmp_button.Click += new System.EventHandler(this.icmp_button_Click);
            // 
            // ICMPv4Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 450);
            this.Controls.Add(this.icmp_button);
            this.Controls.Add(this.icmp_code);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.icmp_dstIP);
            this.Controls.Add(this.icmp_srcMac);
            this.Controls.Add(this.icmp_dstMac);
            this.Controls.Add(this.icmp_type);
            this.Controls.Add(this.destinationMAC);
            this.Controls.Add(this.SourceMAC);
            this.Controls.Add(this.destinationIP);
            this.Controls.Add(this.sourceIP);
            this.Controls.Add(this.icmp_srcIP);
            this.Name = "ICMPv4Page";
            this.Text = "ICMPv4Page";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox icmp_srcIP;
        private Label sourceIP;
        private Label destinationIP;
        private Label SourceMAC;
        private Label destinationMAC;
        private TextBox icmp_type;
        private TextBox icmp_dstMac;
        private TextBox icmp_srcMac;
        private TextBox icmp_dstIP;
        private Label label1;
        private Label label2;
        private TextBox icmp_code;
        private Button icmp_button;
    }
}