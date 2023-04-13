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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using System.Reflection.Emit;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace GUI
{
    public partial class ICMPv4Page : Form
    {
        public ICMPv4Page()
        {
            InitializeComponent();
        }

        private void icmp_button_Click(object sender, EventArgs e)
        {
            int flag = 0;

            string srcIp_Text = icmp_srcIP.Text;
            if (!IsValidIpAddress(srcIp_Text))
            {
                icmp_srcIP.Text = "";
                flag++;
            }
            string dstIp_Text = icmp_dstIP.Text;
            if (!IsValidIpAddress(dstIp_Text))
            {
                icmp_dstIP.Text = "";
                flag++;
            }
            string srcMac_Text = icmp_srcMac.Text;
            if (!IsValidMacAddress(srcMac_Text))
            {
                icmp_srcMac.Text = "";
                flag++;
            }
            string dstMac_Text = icmp_dstMac.Text;
            if (!IsValidMacAddress(dstMac_Text))
            {
                icmp_dstMac.Text = "";
                flag++;
            }
            string type_Text = icmp_type.Text;
            if (!IsValidIcmpType(type_Text))
            {
                icmp_type.Text = "";
                flag++;
            }
            string code_Text = icmp_code.Text;
            if (!IsValidIcmpCode(type_Text, code_Text))
            {
                icmp_type.Text = "";
                flag++;
            }

            if(flag == 0) {
                // Create a JSON object from the input
                var data = new
                {
                    ICMPType = Convert.ToInt32(icmp_type.Text),
                    ICMPCode = Convert.ToInt32(icmp_code.Text),
                    SrcMac = icmp_srcMac.Text,
                    DstMac = icmp_dstMac.Text,
                    SrcIP = icmp_srcIP.Text,
                    DstIP = icmp_dstIP.Text
                };

                var data_json = JsonConvert.SerializeObject(data);
                var send_object = new
                {
                    Type = "icmp",
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
