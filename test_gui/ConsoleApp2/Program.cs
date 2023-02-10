using System;
using System.Net;
using System.Diagnostics;

namespace DockerRunScript
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostIP = GetLocalIPAddress();
            string dockerCommand = "docker run -dit --rm --network host --name PacketGenerator --env VAR1=" + hostIP + " test_1 bash";

            ExecuteDockerCommand(dockerCommand);
        }

        static string GetLocalIPAddress()
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

        static void ExecuteDockerCommand(string command)
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
