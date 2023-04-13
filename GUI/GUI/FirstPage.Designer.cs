namespace GUI
{
    partial class FirstPage
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
            this.choose__box = new System.Windows.Forms.ComboBox();
            this.int_box = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // choose__box
            // 
            this.choose__box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.choose__box.FormattingEnabled = true;
            this.choose__box.Items.AddRange(new object[] {
            "ICMPv4",
            "IP",
            "UDP",
            "TCP",
            "Edit Pcap"});
            this.choose__box.Location = new System.Drawing.Point(12, 12);
            this.choose__box.Name = "choose__box";
            this.choose__box.Size = new System.Drawing.Size(260, 23);
            this.choose__box.TabIndex = 0;
            // 
            // int_box
            // 
            this.int_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.int_box.FormattingEnabled = true;
            this.int_box.Location = new System.Drawing.Point(278, 12);
            this.int_box.Name = "int_box";
            this.int_box.Size = new System.Drawing.Size(292, 23);
            this.int_box.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(576, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FirstPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.int_box);
            this.Controls.Add(this.choose__box);
            this.Name = "FirstPage";
            this.Text = "FirtsPage";
            this.Load += new System.EventHandler(this.FirstPage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox choose__box;
        private ComboBox int_box;
        private Button button1;
    }
}