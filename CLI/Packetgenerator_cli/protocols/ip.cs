using Newtonsoft.Json;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Packetgenerator_cli.protocols
{
    internal class My_Ip
    { 
        public static void send_ip()
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
            var data_json = JsonConvert.SerializeObject(data);
            var send_object = new
            {
                Type = "ip",
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
        }
    }
}
