using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var srcPort = Convert.ToInt32(Console.ReadLine());

            Console.Write("DstPort: ");
            var dstPort = Convert.ToInt32(Console.ReadLine());

            Console.Write("TTL: ");
            var ttl = Convert.ToInt32(Console.ReadLine());

            Console.Write("SeqNum: ");
            var seqNum = Convert.ToInt32(Console.ReadLine());

            Console.Write("AckNum: ");
            var ackNum = Convert.ToInt32(Console.ReadLine());

            Console.Write("Flags: ");
            var flagsStr = Console.ReadLine();

            Console.Write("WinSize: ");
            var winSize = Convert.ToInt32(Console.ReadLine());

            Console.Write("Payload: ");
            var payload = Console.ReadLine();

            // Create a JSON object from the input
            var data = new
            {
                SrcMac = srcMacStr,
                DstMac = dstMacStr,
                SrcIp = srcIPStr,
                DstIp = dstIPStr,
                SrcPort = srcPort,
                DstPort = dstPort,
                Payload = payload,
                Ttl = ttl,
                SeqNum = seqNum,
                AckNum = ackNum,
                WinSize = winSize,
                FlagsStr = flagsStr,
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
            Console.ReadLine();
        }
    }
}
