using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class PCAPPage : Form
    {
        public PCAPPage()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // Show the OpenFileDialog
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PCAP Files|*.pcap|All Files|*.*";
            openFileDialog1.Title = "Select a PCAP File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Load the selected PCAP file using SharpPcap
                var device = new SharpPcap.LibPcap.CaptureFileReaderDevice(openFileDialog1.FileName);
                device.Open();

                device.OnPacketArrival += (sender, e) =>
                {
                    var packet = PacketDotNet.Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
                    if (packet is EthernetPacket)
                    {
                        var ethPacket = (EthernetPacket)packet;

                        if (ethPacket.PayloadPacket is IPv4Packet)
                        {
                            comboBox1.Items.Add(packet);
                            var ipPacket = (IPv4Packet)ethPacket.PayloadPacket;
                            //txtPackets.AppendText($"protocol: {ipPacket.PayloadPacket.ToString()}" + " " + ipPacket.ToString() + "\n");
                        };
                    }
                };
                device.Capture();
                device.Close();
            }
        }

        private void select_button_Click(object sender, EventArgs e)
        {
            // Save the selected packet from the ComboBox in a variable
            var selectedPacket = comboBox1.SelectedItem;
            var ethPacket = (EthernetPacket)selectedPacket;
            var ipPacket = (IPv4Packet)ethPacket.PayloadPacket;

            switch (ipPacket.PayloadPacket)
            {
                case IcmpV4Packet:
                    var ICMPvForm = new ICMPv4Page();
                    ICMPvForm.ShowDialog();
                    break;
                case TcpPacket:
                    var TCPForm = new TCPPage();
                    TCPForm.ShowDialog();
                    break;
                case UdpPacket:
                    var UDPForm = new UDPPage();
                    UDPForm.ShowDialog();
                    break;
                default:
                    MessageBox.Show("Invalid packet.");
                    break;
            }
            // Close the window
            this.Close();
        }
    }
}
