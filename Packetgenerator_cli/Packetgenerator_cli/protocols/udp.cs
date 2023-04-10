using Newtonsoft.Json;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace Packetgenerator_cli.protocols
{
    internal class My_Udp
    {
        public static void send_udp()
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
                Payload = payload
            };

            // Serialize the data to JSON
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
        }
    }
}
