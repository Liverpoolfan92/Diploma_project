using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpPcap;

namespace Packetgenerator_cli.protocols
{
    internal class My_Icmp
    {
        public static void send_icmp()
        {
            // Prompt the user for input

            Console.Write("SrcIP: ");
            var srcIP = Console.ReadLine();
            Console.Write("DstIP: ");
            var dstIP = Console.ReadLine();
            Console.Write("SrcMAC: ");
            var srcMac = Console.ReadLine();
            Console.Write("DstMAC: ");
            var dstMac = Console.ReadLine();
            Console.WriteLine("ICMP type:");
            int icmpType = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("ICMP code:");
            int icmpCode = Convert.ToInt32(Console.ReadLine());

            // Create a JSON object from the input
            var data = new
            {
                ICMPType = icmpType,
                ICMPCode = icmpCode,
                SrcMac = srcMac,
                DstMac = dstMac,
                SrcIP = srcIP,
                DstIP = dstIP
            };

            // Serialize the data to JSON
            var data_json = JsonConvert.SerializeObject(data);
            var send_object = new
            {
                Type = "icmp",
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
