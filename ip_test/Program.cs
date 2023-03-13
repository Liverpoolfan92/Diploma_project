using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace JsonSender
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the user input from the console
            Console.Write("SrcMac: ");
            var srcMac = Console.ReadLine();
            Console.Write("DstMac: ");
            var dstMac = Console.ReadLine();
            Console.Write("SrcIp: ");
            var srcIp = Console.ReadLine();
            Console.Write("DstIp: ");
            var dstIp = Console.ReadLine();
            Console.Write("SrcPort: ");
            var srcPort = int.Parse(Console.ReadLine());
            Console.Write("DstPort: ");
            var dstPort = int.Parse(Console.ReadLine());
            Console.Write("Payload: ");
            var payload = Console.ReadLine();
            Console.Write("TTL: ");
            var ttl = int.Parse(Console.ReadLine());

            // Create an object to hold the data
            var data = new
            {
                SrcMac = srcMac,
                DstMac = dstMac,
                SrcIp = srcIp,
                DstIp = dstIp,
                SrcPort = srcPort,
                DstPort = dstPort,
                Payload = payload,
                Ttl = ttl
            };

            // Serialize the data to JSON
            var json = JsonConvert.SerializeObject(data);

            // Create a TCP client and connect to port 8484 on the local host
            using (var client8484 = new TcpClient())
            {
                var endpoint = new IPEndPoint(IPAddress.Loopback, 8484);
                client8484.Connect(endpoint);

                // Get a network stream for the client
                var stream8485 = client8484.GetStream();

                // Convert the JSON to bytes and write it to the stream
                var bytes = Encoding.UTF8.GetBytes(json);
                stream8485.Write(bytes, 0, bytes.Length);
            }

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
                string json8485 = Encoding.UTF8.GetString(buffer, 0, bytesRead);


                // Parse the JSON into an object
                RAWPACKET obj = JsonConvert.DeserializeObject<RAWPACKET>(json8485);

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
