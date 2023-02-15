using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Windows.Forms.DataFormats;

namespace Packet_Generator
{
    public partial class Form2 : Form
    {
        private Button Start_button;

        public Form2()
        {
            InitializeComponent();
        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            /*Form2 form2 = (Form2)Application.OpenForms["Form2"];
            string my_interface = form2.Parent_int;*/

            // Create user-defined network
            string networkName = "packet_network";
            ExecuteDockerCommand($"docker network create {networkName}");

            // Get host IP address
            string hostIP = GetLocalIPAddress();

            // Run Docker container in user-defined network
            string dockerCommand = $"docker run -dit --rm --name PacketGenerator --env VAR1={hostIP} --network {networkName} packetgenerator bash";
            ExecuteDockerCommand(dockerCommand);

            /*// Switch to another form
            Form2 form2 = new Form2();
            form2.Show();*/

            // Close the current form
            //this.Close();
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

        private void ExecuteDockerCommand(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            process.StartInfo = startInfo;
            process.Start();
        }


        private void InitializeComponent()
        {
            this.Start_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Start_button
            // 
            this.Start_button.Location = new System.Drawing.Point(62, 48);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(146, 44);
            this.Start_button.TabIndex = 0;
            this.Start_button.Text = "Start";
            this.Start_button.UseVisualStyleBackColor = true;
            this.Start_button.Click += new System.EventHandler(this.Start_button_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Start_button);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }
    }
}
