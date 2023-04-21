using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SharpPcap;
using SharpPcap.LibPcap;

namespace GUI
{
    public partial class FirstPage : Form
    {
        // Set up the listener socket
        TcpListener listener = new TcpListener(IPAddress.Any, 8485);
        public FirstPage()
        {
            InitializeComponent();
        }

        private void FirstPage_Load(object sender, EventArgs e)
        {
            listener.Start();
            Random random = new Random();

            // Get all network interfaces
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();


            // Filter out WSL, VirtualBox Host-Only Network, and virtual interfaces
            interfaces = interfaces.Where(i =>
                !i.Name.Contains("WSL") &&
                !i.Description.Contains("Virtual") &&
                !i.Description.Contains("OpenVPN") &&
                !i.NetworkInterfaceType.ToString().Contains("Virtual")).ToArray();

            // Filter out inactive interfaces
            //interfaces = interfaces.Where(i => i.OperationalStatus == OperationalStatus.Up).ToArray();


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
            }
            catch (Exception exe)
            {

            }

            // Get host IP address
            var ipaddr = GetIpAddressByInterfaceName(interfaceName);

            // Run Docker container in user-defined network
            string dockerCommand = $"docker run -dit --rm --name PacketGenerator -p 8484:8484 -p 8485:8485 --env VAR1={ipaddr} --network {networkName} test_1";
            ExecuteDockerCommand(dockerCommand);

            for (int i = 0; i < interfaces.Length; i++)
            {
                int_box.Items.Add(interfaces[i].Name);
            }
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
        private void button1_Click(object sender, EventArgs e)
        {
                int flag = 0;
                string command = choose__box.SelectedItem.ToString();

            switch (command)
                {
                    case "ICMPv4":
                        flag = 1;
                        var ICMPvForm = new ICMPv4Page();
                        ICMPvForm.ShowDialog();
                        break;
                    case "IP":
                        flag = 1;
                        var IPForm = new IPPage();
                        IPForm.ShowDialog();
                        break;
                    case "TCP":
                        flag = 1;
                        var TCPForm = new TCPPage();
                        TCPForm.ShowDialog();
                        break;
                    case "UDP":
                        flag = 1;
                        var UDPForm = new UDPPage();
                        UDPForm.ShowDialog();
                        break;
                    case "Edit Pcap":
                        flag = 1;
                        var PcapForm = new PCAPPage();
                        PcapForm.ShowDialog();
                        break;
                    default:
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

                string sending_interface_Name = int_box.SelectedItem.ToString();

                var target_desc = GetNeworkInterfaceDescriptionByName(sending_interface_Name);

                // Get the list of available network devices
                CaptureDeviceList devices = CaptureDeviceList.Instance;

                ICaptureDevice device = null;

                for (int i = 0; i < devices.Count; i++)
                {
                    if (devices[i] is LibPcapLiveDevice liveDevice &&
                        liveDevice.Interface.Description.Contains(target_desc, StringComparison.OrdinalIgnoreCase))
                    {
                        device = devices[i];
                        break;
                    }
                }

                IInjectionDevice injectionDevice = null;

                for (int i = 0; i < devices.Count; i++)
                {
                    if (devices[i] is LibPcapLiveDevice liveDevice &&
                        liveDevice.Interface.Description.Contains(target_desc, StringComparison.OrdinalIgnoreCase))
                    {
                        injectionDevice = devices[i];
                        break;
                    }
                }

                device.Open();
                    try
                    {
                        injectionDevice.SendPacket(packetdata);
                    }
                    catch (Exception ex)
                    { 
                    }
                    device.Close();
                // Clean up
                client.Close();
            }
        }

        public static string GetNeworkInterfaceDescriptionByName(string name)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            string interface_description = null;

            foreach (NetworkInterface netInterface in interfaces)
            {
                if (netInterface.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    interface_description = netInterface.Description;
                    return interface_description;
                }
            }
            return "Error";
        }
    }
}