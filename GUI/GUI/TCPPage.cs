using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace GUI
{
    public partial class TCPPage : Form
    {
        public TCPPage()
        {
            InitializeComponent();
        }

        private void tcp_button_Click(object sender, EventArgs e)
        {
            int flag = 0;

            string srcIp_Text = tcp_srcIp.Text;
            if (!IsValidIpAddress(srcIp_Text))
            {
                tcp_srcIp.Text = "";
                flag++;
            }
            string dstIp_Text = tcp_dstIp.Text;
            if (!IsValidIpAddress(dstIp_Text))
            {
                tcp_dstIp.Text = "";
                flag++;
            }
            string srcMac_Text = tcp_srcMac.Text;
            if (!IsValidMacAddress(srcMac_Text))
            {
                tcp_srcMac.Text = "";
                flag++;
            }
            string dstMac_Text = tcp_dstMac.Text;
            if (!IsValidMacAddress(dstMac_Text))
            {
                tcp_dstMac.Text = "";
                flag++;
            }
            string srcPort_Text = tcp_srcPort.Text;
            if (!IsValidPort(srcPort_Text))
            {
                tcp_srcPort.Text = "";
                flag++;
            }
            string dstPort_Text = tcp_dstPort.Text;
            if (!IsValidPort(dstPort_Text))
            {
                tcp_dstPort.Text = "";
                flag++;
            }
            string ttl_Text = tcp_ttl.Text;
            if (!IsValidTTL(ttl_Text))
            {
                tcp_ttl.Text = "";
                flag++;
            }
            string seq_Text = tcp_seqNum.Text;
            if (!IsValidSeqNum(seq_Text))
            {
                tcp_seqNum.Text = "";
                flag++;
            }
            string ack_Text = tcp_ackNum.Text;
            if (!IsValidAckNum(ack_Text))
            {
                tcp_ackNum.Text = "";
                flag++;
            }
            string win_Text = tcp_winSize.Text;
            if (!IsValidWinSize(win_Text))
            {
                tcp_winSize.Text = "";
                flag++;
            }
            string flag_Text = tcp_flags.Text;
            if (!IsValidTCPFlags(flag_Text))
            {
                tcp_flags.Text = "";
                flag++;
            }

            if(flag== 0)
            {
                // Create a JSON object from the input
                var data = new
                {
                    SrcMac = tcp_srcMac.Text,
                    DstMac = tcp_dstMac.Text,
                    SrcIp = tcp_srcIp.Text,
                    DstIp = tcp_dstIp.Text,
                    SrcPort = (ushort)Convert.ToInt32(tcp_srcPort.Text),
                    DstPort = (ushort)Convert.ToInt32(tcp_dstPort.Text),
                    Ttl = Convert.ToByte(tcp_ttl.Text),
                    SeqNum = (ushort)Convert.ToInt32(tcp_seqNum.Text),
                    AckNum = (ushort)Convert.ToInt32(tcp_ackNum.Text),
                    FlagsStr = ConvertFlagsToInt(tcp_flags.Text),
                    WinSize = (ushort)Convert.ToInt32(tcp_winSize.Text),
                    Payload = tcp_payload.Text,
                };

                var data_json = JsonConvert.SerializeObject(data);
                var send_object = new
                {
                    Type = "tcp",
                    Data = data_json

                };
                var send_json = JsonConvert.SerializeObject(send_object);

                // Create a TCP client and connect to port 8484 on the local host
                using (var client8484 = new TcpClient())
                {
                    var endpoint = new IPEndPoint(IPAddress.Loopback, 8484);
                    client8484.Connect(endpoint);

                    // Get a network stream for the client
                    var stream8484 = client8484.GetStream();

                    // Convert the JSON to bytes and write it to the stream
                    var bytes = Encoding.UTF8.GetBytes(send_json);
                    stream8484.Write(bytes, 0, bytes.Length);
                }

                // Close the window
                this.Close();
            }

        }

        static int ConvertFlagsToInt(string flagsString)
        {
            string[] flagArray = flagsString.Split(','); // Split input string by commas to get individual flags
            int result = 0;

            foreach (string flag in flagArray)
            {
                string flagTrimmed = flag.Trim().ToUpper(); // Trim and convert flag to uppercase for case-insensitive comparison

                // Add flag value to result based on flag string
                switch (flagTrimmed)
                {
                    case "FIN":
                        result |= 1 << 0;
                        break;
                    case "SYN":
                        result |= 1 << 1;
                        break;
                    case "RST":
                        result |= 1 << 2;
                        break;
                    case "PSH":
                        result |= 1 << 3;
                        break;
                    case "ACK":
                        result |= 1 << 4;
                        break;
                    case "URG":
                        result |= 1 << 5;
                        break;
                    case "ECE":
                        result |= 1 << 6;
                        break;
                    case "CWR":
                        result |= 1 << 7;
                        break;
                    default:
                        Console.WriteLine("Invalid flag: " + flagTrimmed);
                        break;
                }
            }

            return result;
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
