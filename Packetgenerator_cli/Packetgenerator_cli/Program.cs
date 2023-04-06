using System;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Packetgenerator_cli.protocols;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {

            Random random = new Random();

            // Get all network interfaces
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Sort the interfaces so that Ethernet interfaces come first,
            // then Wi-Fi interfaces, and finally all other interfaces
            interfaces = interfaces.OrderByDescending(i =>
            {
                if (i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    return 2;
                }
                else if (i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }).ToArray();

            // Select a random interface name
            string interfaceName = interfaces[random.Next(interfaces.Length)].Name;
            // Create user-defined network
            string networkName = "packet_network";

            try
            {
                ExecuteDockerCommand($"docker network create {networkName} --opt parent={interfaceName}");
            }catch(Exception e)
            {

            }

            // Get host IP address
            var ipaddr = GetIpAddressByInterfaceName(interfaceName);

            // Run Docker container in user-defined network
            string dockerCommand = $"docker run -dit --rm --name PacketGenerator -p 8484:8484 -p 8485:8485 --env VAR1={ipaddr} --network {networkName} test_1 bash";
            ExecuteDockerCommand(dockerCommand);

            string comand = Console.ReadLine();

            if (comand == "end")
            {
                break;
            }

            switch (comand)
            {
                case "--help":
                    Console.WriteLine("send(TYPE OF THE PACKET) to create a file \nedit() to load form pcap filee \nend to stop the program");
                    break;
                case "send(icmp)":
                    My_Icmp.send_icmp();
                    break;
                case "send(ip)":
                    My_Ip.send_ip();
                    break;
                case "send(tcp)":
                    My_Tcp.send_tcp();
                    break;
                case "send(udp)":
                    My_Udp.send_udp();
                    break;
                case "edit()":
                    Console.Write("Path to file: ");
                    string file_path = Console.ReadLine();
                    My_Pcap.send_pcap(file_path);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    public static void ExecuteDockerCommand(string command)
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C " + command;
        process.StartInfo = startInfo;
        process.Start();
    }

    public static IPAddress GetIpAddressByInterfaceName(string interfaceName)
    {
        foreach (var iface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (iface.Name == interfaceName)
            {
                foreach (var addrInfo in iface.GetIPProperties().UnicastAddresses)
                {
                    if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return addrInfo.Address;
                    }
                }
            }
        }

        return null;
    }
}



