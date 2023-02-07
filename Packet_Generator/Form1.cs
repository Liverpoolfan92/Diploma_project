using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace Packet_Generator
{
    public partial class Form1 : Form
    {
        private List<string> interfaces;
        private TcpClient client;

        public Form1()
        {
            InitializeComponent();
            interfaces = new List<string>();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            ListenForData();
        }

        private void ListenForData()
        {
            // Listen for incoming JSON messages on port 8484
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 8484);
            listener.Start();
            client = listener.AcceptTcpClient();

            while (true)
            {
                NetworkStream stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    // Read the incoming JSON message
                    byte[] bytes = new byte[client.ReceiveBufferSize];
                    int data = stream.Read(bytes, 0, client.ReceiveBufferSize);
                    string incomingJson = System.Text.Encoding.ASCII.GetString(bytes, 0, data);

                    // Parse the JSON to extract the array of interface names
                    interfaces = JsonConvert.DeserializeObject<List<string>>(incomingJson);

                    // Create a button for each interface
                    foreach (string i in interfaces)
                    {
                        Button b = new Button();
                        b.Text = i;
                        b.Click += new EventHandler(this.interface_Click);
                        this.Controls.Add(b);
                    }
                }
            }
        }

        private void interface_Click(object sender, EventArgs e)
        {
            // Send the selected interface name as a JSON message on port 8485
            Button b = (Button)sender;
            string outgoingJson = JsonConvert.SerializeObject(b.Text);
            NetworkStream stream = client.GetStream();
            byte[] outBytes = System.Text.Encoding.ASCII.GetBytes(outgoingJson);
            stream.Write(outBytes, 0, outBytes.Length);
        }
    }
}
