using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpPcap;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Security.Cryptography;

namespace Packetgenerator_cli.protocols
{
    internal class My_Icmp
    {
        public static void send_icmp()
        {
            string srcIP, dstIP, srcMac, dstMac;
            int icmpType, icmpCode;
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
                Console.WriteLine("ICMP type:");
                icmpType = Convert.ToInt32(Console.ReadLine());
                if (IsValidIcmpType(icmpType.ToString())) { break; }
            }
            while (true)
            {
                Console.WriteLine("ICMP code:");
                icmpCode = Convert.ToInt32(Console.ReadLine());
                if (IsValidIcmpCode(icmpType.ToString(), icmpCode.ToString())){ break; }
            }

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

        private static bool IsValidIcmpType(string inputText)
        {
            int icmpType;
            if (int.TryParse(inputText, out icmpType))
            {
                return icmpType >= 0 && icmpType <= 255;
            }
            return false;
        }

        private static bool IsValidIcmpCode(string inputType, string inputCode)
        {
            int icmpType;
            int icmpCode;
            if (int.TryParse(inputType, out icmpType) && int.TryParse(inputCode, out icmpCode))
            {
                switch (icmpType)
                {
                    case 0:
                    case 8:
                        // ICMP Echo Request (Ping) and Echo Reply (Ping) - valid codes are 0-255
                        return icmpCode >= 0 && icmpCode <= 255;
                    case 3:
                        // ICMP Destination Unreachable - valid codes are 0-15
                        return icmpCode >= 0 && icmpCode <= 15;
                    case 4:
                        // ICMP Source Quench - valid code is 0
                        return icmpCode == 0;
                    case 5:
                        // ICMP Redirect - valid codes are 0-3
                        return icmpCode >= 0 && icmpCode <= 3;
                    case 11:
                        // ICMP Time Exceeded - valid codes are 0-1
                        return icmpCode >= 0 && icmpCode <= 1;
                    case 12:
                        // ICMP Parameter Problem - valid codes are 0-2
                        return icmpCode >= 0 && icmpCode <= 2;
                    case 13:
                        // ICMP Timestamp Request - valid code is 0
                        return icmpCode == 0;
                    case 14:
                        // ICMP Timestamp Reply - valid code is 0
                        return icmpCode == 0;
                    case 15:
                        // ICMP Information Request - valid code is 0
                        return icmpCode == 0;
                    case 16:
                        // ICMP Information Reply - valid code is 0
                        return icmpCode == 0;
                    case 17:
                        // ICMP Address Mask Request - valid code is 0
                        return icmpCode == 0;
                    case 18:
                        // ICMP Address Mask Reply - valid code is 0
                        return icmpCode == 0;
                    case 43:
                        return icmpCode >= 0 && icmpCode <= 4;
                    default:
                        // Invalid ICMP type, return false
                        return false;
                }
            }
            return false;
        }
    }
}
