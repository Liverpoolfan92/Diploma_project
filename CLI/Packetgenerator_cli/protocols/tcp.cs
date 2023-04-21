using Newtonsoft.Json;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using SharpPcap.LibPcap;
using System.Globalization;

namespace Packetgenerator_cli.protocols
{
    internal class My_Tcp
    {
        public static void send_tcp()
        {
            string srcIP, dstIP, srcMac, dstMac, flags;
            int srcPort, dstPort, ttl, seqNum, ackNum, winSize;
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
            while (true)
            {
                Console.Write("TTL: ");
                ttl = int.Parse(Console.ReadLine());
                if (IsValidTTL(ttl.ToString())) { break; }
            }
            while (true)
            {
                Console.Write("SeqNum: ");
                seqNum = Convert.ToInt32(Console.ReadLine());
                if (IsValidSeqNum(seqNum.ToString())) { break; }
            }
            while(true)
            {
                Console.Write("AckNum: ");
                ackNum = Convert.ToInt32(Console.ReadLine());
                if(IsValidAckNum(ackNum.ToString())) { break; }
            }
            while (true)
            {
                Console.Write("WinSize: ");
                winSize = Convert.ToInt32(Console.ReadLine());
                if(IsValidWinSize(winSize.ToString())) { break; }

            }
            while (true)
            {
                Console.Write("Flags: ");
                flags = Console.ReadLine();
                if (IsValidTCPFlags(flags)) { break; }
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
                SrcPort = srcPort,
                DstPort = dstPort,
                Payload = payload,
                Ttl = ttl,
                SeqNum = seqNum,
                AckNum = ackNum,
                WinSize = winSize,
                FlagsStr = ConvertFlagsToInt(flags),
            };

            // Serialize the data to JSON
            var data_json = JsonConvert.SerializeObject(data);
            var send_object = new
            {
                Type = "tcp",
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

        public static bool IsValidTCPFlags(string tcpFlags)
        {
            // Define the valid TCP flags as an array
            string[] validFlags = { "FIN", "SYN", "RST", "PSH", "ACK", "URG", "ECE", "CWR" };

            // Split the input string into individual flags
            string[] flags = tcpFlags.Split(',');

            foreach (var flag in flags)
            {
                // Trim the flag and convert to uppercase for comparison
                string trimmedFlag = flag.Trim().ToUpper();

                // Check if the trimmed flag is a valid TCP flag
                if (!Array.Exists(validFlags, f => f == trimmedFlag))
                {
                    return false;
                }
            }

            return true;
        }

        static int ConvertFlagsToInt(string flagsString)
        {
            string[] flagArray = flagsString.Split(','); // Split input string by commas to get individual flags
            int result = 0;

            foreach (string flag in flagArray)
            {
                string flagTrimmed = flag.Trim().ToUpper(); // Trim and convert flag to uppercase for case-insensitive comparison

                // Add flag value to result based on flag string
                switch (flagTrimmed)
                {
                    case "FIN":
                        result |= 1 << 0;
                        break;
                    case "SYN":
                        result |= 1 << 1;
                        break;
                    case "RST":
                        result |= 1 << 2;
                        break;
                    case "PSH":
                        result |= 1 << 3;
                        break;
                    case "ACK":
                        result |= 1 << 4;
                        break;
                    case "URG":
                        result |= 1 << 5;
                        break;
                    case "ECE":
                        result |= 1 << 6;
                        break;
                    case "CWR":
                        result |= 1 << 7;
                        break;
                    default:
                        Console.WriteLine("Invalid flag: " + flagTrimmed);
                        break;
                }
            }

            return result;
        }

        public static bool IsValidSeqNum(string seqNum)
        {
            uint seqNumValue;
            if (UInt32.TryParse(seqNum, out seqNumValue))
            {
                // Check if the sequence number is within the valid range (0 - UInt32.MaxValue)
                if (seqNumValue <= UInt32.MaxValue)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidAckNum(string ackNum)
        {
            uint ackNumValue;
            if (UInt32.TryParse(ackNum, out ackNumValue))
            {
                // Check if the acknowledgment number is within the valid range (0 - UInt32.MaxValue)
                if (ackNumValue <= UInt32.MaxValue)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidWinSize(string winSize)
        {
            ushort winSizeValue;
            if (UInt16.TryParse(winSize, out winSizeValue))
            {
                // Check if the window size is within the valid range (0 - UInt16.MaxValue)
                if (winSizeValue <= UInt16.MaxValue)
                {
                    return true;
                }
            }

            return false;
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
        public static bool IsValidTTL(string ttlString)
        {
            if (int.TryParse(ttlString, out int ttl))
            {
                // Check if the TTL is within the valid range (1 - 255)
                if (ttl >= 1 && ttl <= 255)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
