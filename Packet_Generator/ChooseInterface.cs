using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Packet_Generator
{
    public partial class Form2 : Form
    {
        private Button choose_interface;

        public Form2()
        {
            InitializeComponent();
        }
        private void choose_interface_Click(object sender, EventArgs e)
        {

            string hostName = Dns.GetHostName();
            
            int localPort = 8484;
            int targetPort = 8485;
            IPAddress localAddress = IPAddress.Any;
            IPAddress targetAddress = IPAddress.Parse("172.31.176.1");

            try
            {
                var listener = new TcpListener(localAddress, localPort);
                listener.Start();

                //Console.WriteLine($"Listening on {localAddress}:{localPort}");

                TcpClient client1 = listener.AcceptTcpClient();
                //Console.WriteLine("Received connection");

                var networkStream = client1.GetStream();
                var buffer = new byte[1024];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                string incomingData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                dynamic s = JsonConvert.DeserializeObject(incomingData);
                var items = s.interfaces;

                string[] json = ((Newtonsoft.Json.Linq.JArray)items).Select(jv => jv.ToString()).ToArray();
                //Console.WriteLine("Received interfaces:");
                /*for (int i = 0; i < json.Length; i++)
                {
                    Console.WriteLine($"{i}: {json[i]}");
                }

                Console.WriteLine("Choose an interface by its number:");
                int choice = int.Parse(Console.ReadLine());*/

                for (int i = 0; i < json.Length; i++)
                {
                    Button btn = new Button();
                    btn.Text = json[i];
                    btn.Name = "Button" + i.ToString();
                    btn.Click += new EventHandler(Button_Click);
                    btn.Location = new System.Drawing.Point(10, 10 + i * 50);
                    btn.Size = new System.Drawing.Size(100, 30);
                    this.Controls.Add(btn);
                }

               /* var chosenData = new Chosen { Interface = json[choice] };
                var jsonBytes = JsonConvert.SerializeObject(chosenData);

                TcpClient targetClient = new TcpClient();
                targetClient.Connect(targetAddress, targetPort);
                Console.WriteLine($"Connected to {targetAddress}:{targetPort}");

                var targetStream = targetClient.GetStream();
                var test = Encoding.UTF8.GetBytes(jsonBytes);
                targetStream.Write(test, 0, test.Length);
                Console.WriteLine("Sent data");

                targetClient.Close();*/
                listener.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int choice = Int32.Parse(Regex.Match(btn.Name, @"\d+").Value);
            Console.WriteLine("You chose " + btn.Text + " with index " + choice);
        }
        private void InitializeComponent()
        {
            this.choose_interface = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // choose_interface
            // 
            this.choose_interface.Location = new System.Drawing.Point(60, 58);
            this.choose_interface.Name = "choose_interface";
            this.choose_interface.Size = new System.Drawing.Size(170, 27);
            this.choose_interface.TabIndex = 0;
            this.choose_interface.Text = "Choose Interface";
            this.choose_interface.UseVisualStyleBackColor = true;
            this.choose_interface.Click += new System.EventHandler(this.choose_interface_Click);
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.choose_interface);
            this.Name = "Form2";
            this.ResumeLayout(false);

        }
    }

    class Interfaces
    {
        public string[] InterfaceList { get; set; }
    }

    class Chosen
    {
        public string Interface { get; set; }
    }
}