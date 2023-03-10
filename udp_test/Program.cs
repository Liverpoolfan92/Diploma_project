using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt the user for input
            Console.Write("SrcMAC: ");
            var srcMacStr = Console.ReadLine();

            Console.Write("DstMAC: ");
            var dstMacStr = Console.ReadLine();

            Console.Write("SrcIP: ");
            var srcIPStr = Console.ReadLine();

            Console.Write("DstIP: ");
            var dstIPStr = Console.ReadLine();

            Console.Write("SrcPort: ");
            var srcPortStr = Console.ReadLine();

            Console.Write("DstPort: ");
            var dstPortStr = Console.ReadLine();

            Console.Write("Payload: ");
            var payload = Console.ReadLine();

            // Create a JSON object from the input
            var data = new
            {
                SrcMac = srcMacStr,
                DstMac = dstMacStr,
                SrcIp = srcIPStr,
                DstIp = dstIPStr,
                SrcPort = Convert.ToInt32(srcPortStr),
                DstPort = Convert.ToInt32(dstPortStr),
                Payload = payload,
            };

            // Serialize the data to JSON
            var send_json = JsonConvert.SerializeObject(data);

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

            Console.WriteLine("Data sent successfully.");

            var ip_host = GetNetworkInterfaceIPAddress("eth0");
            
                // Set up the listener socket
                TcpListener listener = new TcpListener(ip_host, 8485);
                listener.Start();

                Console.WriteLine("Listening on port 8485...");

                while (true)
                {
                    // Wait for a connection
                    TcpClient client = listener.AcceptTcpClient();

                // Read the JSON payload
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);


                // Parse the JSON into an object
                RAWPACKET obj = JsonConvert.DeserializeObject<RAWPACKET>(json);

                    // Do something with the object
                    Console.WriteLine($"Received object with property1={obj.BYTES}");

                    // Clean up
                    client.Close();
                }
            }
        public static IPAddress GetNetworkInterfaceIPAddress(string interfaceName)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface iface in interfaces)
            {
                if (iface.Name == interfaceName && iface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in iface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }
                    }
                }
            }

            throw new ArgumentException($"No IP address found for network interface {interfaceName}");
        }
    }

        class RAWPACKET
        {
            public string BYTES { get; set; }
        }
    }
