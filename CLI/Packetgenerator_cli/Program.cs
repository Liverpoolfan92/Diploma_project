using System;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;
using Packetgenerator_cli.protocols;
using SharpPcap;
using Docker.DotNet;
using Docker.DotNet.Models;

class Program
{
    static void Main(string[] args)
    {
        // Set up the listener socket
        TcpListener listener = new TcpListener(IPAddress.Any, 8485);
        listener.Start();

        Random random = new Random();

            // Get all network interfaces
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Filter out WSL, VirtualBox Host-Only Network, and virtual interfaces
            interfaces = interfaces.Where(i =>
                !i.Name.Contains("WSL") &&
                !i.Description.Contains("Virtual") &&
                !i.NetworkInterfaceType.ToString().Contains("Virtual")).ToArray();

        // Filter out inactive interfaces
        interfaces = interfaces.Where(i => i.OperationalStatus == OperationalStatus.Up).ToArray();  


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

        if (!CheckIfNetworkExists(networkName))
        {

            try
            {
                ExecuteDockerCommand($"docker network create {networkName} --opt parent={interfaceName}");
            }
            catch (Exception e)
            {

            }
        }

            // Get host IP address
            var ipaddr = GetIpAddressByInterfaceName(interfaceName);

            // Run Docker container in user-defined network
            string dockerCommand = $"docker run -dit --rm --name PacketGenerator -p 8484:8484 -p 8485:8485 --env VAR1={ipaddr} --network {networkName} test_1";
            ExecuteDockerCommand(dockerCommand);

        while (true)
        {
            int flag = 0;
            string comand = Console.ReadLine();

            if (comand == "end")
            {
                KillPacketDocker();
                break;
            }

            switch (comand)
            {
                case "help":
                    Console.WriteLine("send(TYPE OF THE PACKET) to create a file \nedit() to load form pcap filee \nend to stop the program");
                    break;
                case "send(icmp)":
                    My_Icmp.send_icmp();
                    flag = 1;
                    break;
                case "send(ip)":
                    My_Ip.send_ip();
                    flag = 1;
                    break;
                case "send(tcp)":
                    My_Tcp.send_tcp();
                    flag = 1;
                    break;
                case "send(udp)":
                    My_Udp.send_udp();
                    flag = 1;
                    break;
                case "edit()":
                    Console.Write("Path to file: ");
                    string file_path = Console.ReadLine();
                    flag = 1;
                    My_Pcap.send_pcap(file_path);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            if (flag == 1)
            {
                // Wait for a connection
                TcpClient client = listener.AcceptTcpClient();

                // Read the JSON payload
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
                string json8485 = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Parse the JSON into an object
                RAWPACKET obj = JsonConvert.DeserializeObject<RAWPACKET>(json8485);
                byte[] packetdata = Convert.FromBase64String(obj.packet);

                CaptureDeviceList devices = CaptureDeviceList.Instance;
                for (int i = 0; i < devices.Count; i++)
                {
                    Console.WriteLine("{0}.{1}\n", i, devices[i].Description.ToString());
                    Console.WriteLine("Choose a device");
                }
                int dev = 0;
                if (!int.TryParse(Console.ReadLine(), out dev) || dev > devices.Count)
                {
                    Console.WriteLine("Incorrect device number");
                    Console.ReadLine();
                    return;
                }
                IInjectionDevice injectionDevice = devices[dev];
                ICaptureDevice device = devices[dev];
                device.Open();
                try
                {
                    injectionDevice.SendPacket(packetdata);
                    Console.WriteLine("-- Packet send successfuly.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("--" + e.Message);
                }
                device.Close();

                // Clean up
                client.Close();
            }
        }
    }

    public static void KillPacketDocker()
    {
        using (var client = new DockerClientConfiguration().CreateClient())
        {
            client.Containers.StopContainerAsync("PacketGenerator", new ContainerStopParameters()).Wait();
        }
    }

    public static bool CheckIfNetworkExists(string networkName)
    {
        using (var client = new DockerClientConfiguration().CreateClient())
        {
            var networks = client.Networks.ListNetworksAsync().Result;
            return networks.Any(n => n.Name == networkName);
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

    class RAWPACKET
    {
        public string packet { get; set; }
    }
}



