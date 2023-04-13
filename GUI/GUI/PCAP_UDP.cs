using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class PCAP_UDP : Form
    {
        private Packet selectedPacket;
        public PCAP_UDP(Packet selectedPacket)
        {
            InitializeComponent();
            this.selectedPacket = selectedPacket;
        }

        private void udp_button_Click(object sender, EventArgs e)
        {
            int flag = 0;

            string srcIp_Text = pcap_udp_srcIp.Text;
            if (!IsValidIpAddress(srcIp_Text))
            {
                pcap_udp_srcIp.Text = "";
                flag++;
            }
            string dstIp_Text = pcap_udp_dstIp.Text;
            if (!IsValidIpAddress(dstIp_Text))
            {
                pcap_udp_dstIp.Text = "";
                flag++;
            }
            string srcMac_Text = pcap_udp_srcMac.Text;
            if (!IsValidMacAddress(srcMac_Text))
            {
                pcap_udp_srcMac.Text = "";
                flag++;
            }
            string dstMac_Text = pcap_udp_dstMac.Text;
            if (!IsValidMacAddress(dstMac_Text))
            {
                pcap_udp_dstMac.Text = "";
                flag++;
            }
            string srcPort_Text = pcap_udp_srcPort.Text;
            if (!IsValidPort(srcPort_Text))
            {
                pcap_udp_srcPort.Text = "";
                flag++;
            }
            string dstPort_Text = pcap_udp_dstPort.Text;
            if (!IsValidPort(dstPort_Text))
            {
                pcap_udp_dstPort.Text = "";
                flag++;
            }

            if (flag == 0)
            {
                if (selectedPacket is EthernetPacket)
                {
                    var ethPacket = (EthernetPacket)selectedPacket;

                    if (ethPacket.PayloadPacket is IPv4Packet)
                    {
                        var ipPacket = (IPv4Packet)ethPacket.PayloadPacket;

                        if (ipPacket.PayloadPacket is UdpPacket)
                        {
                            var udpPacket = (UdpPacket)ipPacket.PayloadPacket;

                            // Update packet
                            ethPacket.SourceHardwareAddress = PhysicalAddress.Parse(pcap_udp_srcMac.Text);
                            ethPacket.DestinationHardwareAddress = PhysicalAddress.Parse(pcap_udp_dstMac.Text);
                            ipPacket.SourceAddress = IPAddress.Parse(pcap_udp_srcIp.Text);
                            ipPacket.DestinationAddress = IPAddress.Parse(pcap_udp_dstIp.Text);
                            udpPacket.SourcePort = (ushort)Convert.ToInt32(pcap_udp_srcPort.Text);
                            udpPacket.DestinationPort = (ushort)Convert.ToInt32(pcap_udp_dstPort.Text);
                            ipPacket.PayloadData = System.Text.Encoding.ASCII.GetBytes(pcap_udp_payload.Text);

                            // Recalculate checksums
                            udpPacket.Checksum = udpPacket.CalculateUdpChecksum();
                            ipPacket.Checksum = ipPacket.CalculateIPChecksum();
                            // Re-serialize packet and print updated packet info
                            var updatedPacket = ethPacket.Bytes;

                            sendPacket(updatedPacket);
                        }
                    }
                }
                // Close the window
                this.Close();
            }
        }

        public static void sendPacket(byte[] packet)
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine("{0}.{1}\n", i, devices[i]);
                Console.WriteLine("Choose a device");
            }
            int dev = 0;
            if ((!Int32.TryParse(Console.ReadLine(), out dev)) || (dev > devices.Count))
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
                injectionDevice.SendPacket(packet);
                Console.WriteLine("-- Packet send successfuly.");
            }
            catch (Exception e)
            {
                Console.WriteLine("--" + e.Message);
            }
            device.Close();
        }

        private bool IsValidIpAddress(string inputText)
        {
            if (IPAddress.TryParse(inputText, out IPAddress parsedIpAddress))
            {
                // Check if the parsed IP address is IPv4 or IPv6
                if (parsedIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ||
                    parsedIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsValidMacAddress(string inputText)
        {
            if (PhysicalAddress.TryParse(inputText, out PhysicalAddress parsedMacAddress))
            {
                return true;
            }

            return false;
        }

        private bool IsValidPort(string portNumberString)
        {
            if (int.TryParse(portNumberString, out int portNumber))
            {
                // Check if the port number is within the valid range (0 - 65535)
                if (portNumber >= 0 && portNumber <= 65535)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
