namespace Packet_Generator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Choose_Interface = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Choose_Interface
            // 
            this.Choose_Interface.Location = new System.Drawing.Point(320, 70);
            this.Choose_Interface.Name = "Choose_Interface";
            this.Choose_Interface.Size = new System.Drawing.Size(148, 64);
            this.Choose_Interface.TabIndex = 0;
            this.Choose_Interface.Text = "Interface";
            this.Choose_Interface.UseVisualStyleBackColor = true;
            this.Choose_Interface.Click += new System.EventHandler(this.Choose_Interface_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Choose_Interface);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button Choose_Interface;
    }
}