using PacketDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpPcap;

namespace GUI
{
    public partial class PCAP_ICMPv4 : Form
    {
        private Packet selectedPacket;
        public PCAP_ICMPv4(Packet selectedPacket)
        {
            InitializeComponent();
            this.selectedPacket = selectedPacket;
        }

        private void icmp_button_Click(object sender, EventArgs e)
        {
            int flag = 0;

            string srcIp_Text = pcap_icmp_srcIP.Text;
            if (!IsValidIpAddress(srcIp_Text))
            {
                pcap_icmp_srcIP.Text = "";
                flag++;
            }
            string dstIp_Text = pcap_icmp_dstIP.Text;
            if (!IsValidIpAddress(dstIp_Text))
            {
                pcap_icmp_dstIP.Text = "";
                flag++;
            }
            string srcMac_Text = pcap_icmp_srcMac.Text;
            if (!IsValidMacAddress(srcMac_Text))
            {
                pcap_icmp_srcMac.Text = "";
                flag++;
            }
            string dstMac_Text = pcap_icmp_dstMac.Text;
            if (!IsValidMacAddress(dstMac_Text))
            {
                pcap_icmp_dstMac.Text = "";
                flag++;
            }
            string type_Text = pcap_icmp_type.Text;
            if (!IsValidIcmpType(type_Text))
            {
                pcap_icmp_type.Text = "";
                flag++;
            }
            string code_Text = pcap_icmp_code.Text;
            if (!IsValidIcmpCode(type_Text, code_Text))
            {
                pcap_icmp_code.Text = "";
                flag++;
            }

            if(flag == 0)
            {
                if (selectedPacket is EthernetPacket)
                {
                    var ethPacket = (EthernetPacket)selectedPacket;

                    if (ethPacket.PayloadPacket is IPv4Packet)
                    {
                        var ipPacket = (IPv4Packet)ethPacket.PayloadPacket;

                        if (ipPacket.PayloadPacket is IcmpV4Packet)
                        {
                            var icmpPacket = (IcmpV4Packet)ipPacket.PayloadPacket;

                            // Update packet
                            ethPacket.SourceHardwareAddress = PhysicalAddress.Parse(pcap_icmp_srcMac.Text);
                            ethPacket.DestinationHardwareAddress = PhysicalAddress.Parse(pcap_icmp_dstMac.Text);
                            ipPacket.SourceAddress = IPAddress.Parse(pcap_icmp_srcIP.Text);
                            ipPacket.DestinationAddress = IPAddress.Parse(pcap_icmp_dstIP.Text);
                            byte icmpTypeCode = (byte)((byte.Parse(pcap_icmp_type.Text) << 8) | byte.Parse(pcap_icmp_code.Text));
                            icmpPacket.TypeCode = (IcmpV4TypeCode)icmpTypeCode;

                            // Recalculate checksums
                            icmpPacket.Checksum = icmpPacket.CalculateIcmpChecksum();
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

        private bool IsValidIcmpType(string inputText)
        {
            int icmpType;
            if (int.TryParse(inputText, out icmpType))
            {
                return icmpType >= 0 && icmpType <= 255;
            }
            return false;
        }

        private bool IsValidIcmpCode(string inputType, string inputCode)
        {
            int icmpType;
            int icmpCode;
            if (int.TryParse(inputType, out icmpType) && int.TryParse(inputCode, out icmpCode))
            {
                switch (icmpType)
                {
                    case 0:
                    case 8:
                        // ICMP Echo Request (Ping) and Echo Reply (Ping) - valid codes are 0-255
                        return icmpCode >= 0 && icmpCode <= 255;
                    case 3:
                        // ICMP Destination Unreachable - valid codes are 0-15
                        return icmpCode >= 0 && icmpCode <= 15;
                    case 4:
                        // ICMP Source Quench - valid code is 0
                        return icmpCode == 0;
                    case 5:
                        // ICMP Redirect - valid codes are 0-3
                        return icmpCode >= 0 && icmpCode <= 3;
                    case 11:
                        // ICMP Time Exceeded - valid codes are 0-1
                        return icmpCode >= 0 && icmpCode <= 1;
                    case 12:
                        // ICMP Parameter Problem - valid codes are 0-2
                        return icmpCode >= 0 && icmpCode <= 2;
                    case 13:
                        // ICMP Timestamp Request - valid code is 0
                        return icmpCode == 0;
                    case 14:
                        // ICMP Timestamp Reply - valid code is 0
                        return icmpCode == 0;
                    case 15:
                        // ICMP Information Request - valid code is 0
                        return icmpCode == 0;
                    case 16:
                        // ICMP Information Reply - valid code is 0
                        return icmpCode == 0;
                    case 17:
                        // ICMP Address Mask Request - valid code is 0
                        return icmpCode == 0;
                    case 18:
                        // ICMP Address Mask Reply - valid code is 0
                        return icmpCode == 0;
                    case 43:
                        return icmpCode >= 0 && icmpCode <= 4;
                    default:
                        // Invalid ICMP type, return false
                        return false;
                }
            }
            return false;
        }
    }
}
