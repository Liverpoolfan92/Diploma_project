using PacketDotNet;
using PacketDotNet.Ieee80211;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class PCAP_TCP : Form
    {
        private Packet selectedPacket;
        public PCAP_TCP(Packet selectedPacket)
        {
            InitializeComponent();
            this.selectedPacket = selectedPacket;
        }



        private void tcp_button_Click(object sender, EventArgs e)
        {
            int flag = 0;

            string srcIp_Text = pcap_tcp_srcIp.Text;
            if (!IsValidIpAddress(srcIp_Text))
            {
                pcap_tcp_srcIp.Text = "";
                flag++;
            }
            string dstIp_Text = pcap_tcp_dstIp.Text;
            if (!IsValidIpAddress(dstIp_Text))
            {
                pcap_tcp_dstIp.Text = "";
                flag++;
            }
            string srcMac_Text = pcap_tcp_srcMac.Text;
            if (!IsValidMacAddress(srcMac_Text))
            {
                pcap_tcp_srcMac.Text = "";
                flag++;
            }
            string dstMac_Text = pcap_tcp_dstMac.Text;
            if (!IsValidMacAddress(dstMac_Text))
            {
                pcap_tcp_dstMac.Text = "";
                flag++;
            }
            string srcPort_Text = pcap_tcp_srcPort.Text;
            if (!IsValidPort(srcPort_Text))
            {
                pcap_tcp_srcPort.Text = "";
                flag++;
            }
            string dstPort_Text = pcap_tcp_dstPort.Text;
            if (!IsValidPort(dstPort_Text))
            {
                pcap_tcp_dstPort.Text = "";
                flag++;
            }
            string ttl_Text = pcap_tcp_ttl.Text;
            if (!IsValidTTL(ttl_Text))
            {
                pcap_tcp_ttl.Text = "";
                flag++;
            }
            string seq_Text = pcap_tcp_seqNum.Text;
            if (!IsValidSeqNum(seq_Text))
            {
                pcap_tcp_seqNum.Text = "";
                flag++;
            }
            string ack_Text = pcap_tcp_ackNum.Text;
            if (!IsValidAckNum(ack_Text))
            {
                pcap_tcp_ackNum.Text = "";
                flag++;
            }
            string win_Text = pcap_tcp_winSize.Text;
            if (!IsValidWinSize(win_Text))
            {
                pcap_tcp_winSize.Text = "";
                flag++;
            }
            string flag_Text = pcap_tcp_flags.Text;
            if (!IsValidTCPFlags(flag_Text))
            {
                pcap_tcp_flags.Text = "";
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

                        if (ipPacket.PayloadPacket is TcpPacket)
                        {
                            var tcpPacket = (TcpPacket)ipPacket.PayloadPacket;
                            // Update packet
                            ethPacket.SourceHardwareAddress = PhysicalAddress.Parse(pcap_tcp_srcMac.Text);
                            ethPacket.DestinationHardwareAddress = PhysicalAddress.Parse(pcap_tcp_dstMac.Text);
                            ipPacket.SourceAddress = IPAddress.Parse(pcap_tcp_srcIp.Text);
                            ipPacket.DestinationAddress = IPAddress.Parse(pcap_tcp_dstIp.Text);
                            tcpPacket.SourcePort = (ushort)Convert.ToInt32(pcap_tcp_srcPort.Text);
                            tcpPacket.DestinationPort = (ushort)Convert.ToInt32(pcap_tcp_dstPort.Text);
                            tcpPacket.PayloadData = System.Text.Encoding.ASCII.GetBytes(pcap_tcp_payload.Text);
                            tcpPacket.Flags = ushort.Parse(pcap_tcp_flags.Text);
                            tcpPacket.SequenceNumber = (ushort)Convert.ToInt32(pcap_tcp_seqNum.Text);
                            tcpPacket.AcknowledgmentNumber = (ushort)Convert.ToInt32(pcap_tcp_ackNum.Text);
                            tcpPacket.WindowSize = (ushort)Convert.ToInt32(pcap_tcp_winSize);
                            ipPacket.TimeToLive = Convert.ToByte(pcap_tcp_ttl.Text);

                            // Recalculate checksums
                            tcpPacket.Checksum = tcpPacket.CalculateTcpChecksum();
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

        private bool IsValidTTL(string ttlString)
        {
            if (int.TryParse(ttlString, out int ttl))
            {
                // Check if the TTL is within the valid range (1 - 255)
                if (ttl >= 1 && ttl <= 255)
                {
                    return true;
                }
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

        static bool IsValidSeqNum(string seqNum)
        {
            uint seqNumValue;
            if (UInt32.TryParse(seqNum, out seqNumValue))
            {
                // Check if the sequence number is within the valid range (0 - UInt32.MaxValue)
                if (seqNumValue <= UInt32.MaxValue)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsValidAckNum(string ackNum)
        {
            uint ackNumValue;
            if (UInt32.TryParse(ackNum, out ackNumValue))
            {
                // Check if the acknowledgment number is within the valid range (0 - UInt32.MaxValue)
                if (ackNumValue <= UInt32.MaxValue)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsValidWinSize(string winSize)
        {
            ushort winSizeValue;
            if (UInt16.TryParse(winSize, out winSizeValue))
            {
                // Check if the window size is within the valid range (0 - UInt16.MaxValue)
                if (winSizeValue <= UInt16.MaxValue)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsValidTCPFlags(string tcpFlags)
        {
            // Define the valid TCP flags as an array
            string[] validFlags = { "FIN", "SYN", "RST", "PSH", "ACK", "URG", "ECE", "CWR" };

            // Split the input string into individual flags
            string[] flags = tcpFlags.Split(',');

            foreach (var flag in flags)
            {
                // Trim the flag and convert to uppercase for comparison
                string trimmedFlag = flag.Trim().ToUpper();

                // Check if the trimmed flag is a valid TCP flag
                if (!Array.Exists(validFlags, f => f == trimmedFlag))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
