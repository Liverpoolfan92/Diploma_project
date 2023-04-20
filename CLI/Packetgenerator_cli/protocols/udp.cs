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
using System.Globalization;

namespace Packetgenerator_cli.protocols
{
    internal class My_Udp
    {
        public static void send_udp()
        {
            string srcIP, dstIP, srcMac, dstMac;
            int srcPort, dstPort;

            // Prompt the user for input
            while (true)
            {
                Console.Write("SrcIP: ");
                srcIP = Console.ReadLine();
                if (IsValidIPv4Address(srcIP)) { break; }
            }
            while (true)
            {
                Console.Write("DstIP: ");
                dstIP = Console.ReadLine();
                if (IsValidIPv4Address(dstIP)) { break; }
            }
            while (true)
            {
                Console.Write("SrcMAC: ");
                srcMac = Console.ReadLine();
                if (IsValidMACAddress(srcMac)) { break; }
            }
            while (true)
            {
                Console.Write("DstMAC: ");
                dstMac = Console.ReadLine();
                if (IsValidMACAddress(dstMac)) { break; }
            }
            while (true)
            {
                Console.Write("SrcPort: ");
                srcPort = int.Parse(Console.ReadLine());
                if (IsValidPort(srcPort.ToString())) { break; }
            }
            while (true)
            {
                Console.Write("SrcPort: ");
                dstPort = int.Parse(Console.ReadLine());
                if (IsValidPort(dstPort.ToString())) { break; }
            }
            Console.Write("Payload: ");
            var payload = Console.ReadLine();

           
            
            // Create a JSON object from the input
            var data = new
            {
                SrcMac = srcMac,
                DstMac = dstMac,
                SrcIp = srcIP,
                DstIp = dstIP,
                SrcPort = Convert.ToInt32(srcPort),
                DstPort = Convert.ToInt32(dstPort),
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

        public static bool IsValidPort(string portNumberString)
        {
            if (int.TryParse(portNumberString, out int portNumber))
            {
                // Check if the port number is within the valid range (0 - 65535)
                if (portNumber >= 0 && portNumber <= 65535)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidMACAddress(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            // Check if it contains exactly 5 colons
            int colonCount = input.Count(c => c == ':');
            if (colonCount != 5)
            {
                return false;
            }

            // Check if it is a valid MAC address
            string[] octets = input.Split(':');
            if (octets.Length != 6)
            {
                return false;
            }

            foreach (string octet in octets)
            {
                if (!byte.TryParse(octet, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte result))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidIPv4Address(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            // Check if it contains exactly 3 periods
            int periodCount = input.Count(c => c == '.');
            if (periodCount != 3)
            {
                return false;
            }

            // Check if it is a valid IPv4 address
            string[] octets = input.Split('.');
            if (octets.Length != 4)
            {
                return false;
            }

            foreach (string octet in octets)
            {
                if (!byte.TryParse(octet, out byte result))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
