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
    public partial class UDPPage : Form
    {
        public UDPPage()
        {
            InitializeComponent();
        }

        private void udp_button_Click(object sender, EventArgs e)
        {
            int flag = 0;

            string srcIp_Text = udp_srcIp.Text;
            if (!IsValidIpAddress(srcIp_Text))
            {
                udp_srcIp.Text = "";
                flag++;
            }
            string dstIp_Text = udp_dstIp.Text;
            if (!IsValidIpAddress(dstIp_Text))
            {
                udp_dstIp.Text = "";
                flag++;
            }
            string srcMac_Text = udp_srcMac.Text;
            if (!IsValidMacAddress(srcMac_Text))
            {
                udp_srcMac.Text = "";
                flag++;
            }
            string dstMac_Text = udp_dstMac.Text;
            if (!IsValidMacAddress(dstMac_Text))
            {
                udp_dstMac.Text = "";
                flag++;
            }
            string srcPort_Text = udp_srcPort.Text;
            if (!IsValidPort(srcPort_Text))
            {
                udp_srcPort.Text = "";
                flag++;
            }
            string dstPort_Text = udp_dstPort.Text;
            if (!IsValidPort(dstPort_Text))
            {
                udp_dstPort.Text = "";
                flag++;
            }

            if(flag == 0)
            {
                // Create a JSON object from the input
                var data = new
                {
                    SrcMac = udp_srcMac.Text,
                    DstMac = udp_dstMac.Text,
                    SrcIp = udp_srcIp.Text,
                    DstIp = udp_dstIp.Text,
                    SrcPort = Convert.ToInt32(udp_srcPort.Text),
                    DstPort = Convert.ToInt32(udp_dstPort.Text),
                    Payload = udp_payload.Text,
                };

                var data_json = JsonConvert.SerializeObject(data);
                var send_object = new
                {
                    Type = "udp",
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
