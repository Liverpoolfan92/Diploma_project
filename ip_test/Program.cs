using System;
using System.Net;
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
            using (var client = new TcpClient())
            {
                var endpoint = new IPEndPoint(IPAddress.Loopback, 8484);
                client.Connect(endpoint);

                // Get a network stream for the client
                var stream = client.GetStream();

                // Convert the JSON to bytes and write it to the stream
                var bytes = Encoding.UTF8.GetBytes(json);
                stream.Write(bytes, 0, bytes.Length);
            }

            // Output the user input values to the console
            Console.WriteLine($"SrcMac: {srcMac}");
            Console.WriteLine($"DstMac: {dstMac}");
            Console.WriteLine($"SrcIp: {srcIp}");
            Console.WriteLine($"DstIp: {dstIp}");
            Console.WriteLine($"SrcPort: {srcPort}");
            Console.WriteLine($"DstPort: {dstPort}");
            Console.WriteLine($"Payload: {payload}");
            Console.WriteLine($"TTL: {ttl}");
        }
    }
}
