using System;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WinformsExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void runDockerButton_Click(object sender, EventArgs e)
        {
            string hostIP = GetLocalIPAddress();
            string dockerCommand = "docker run -dit --rm --network host --name PacketGenerator --env VAR1=" + hostIP + " --dns=212.39.90.52 test_1 bash";

            ExecuteDockerCommand(dockerCommand);
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
    }
}
