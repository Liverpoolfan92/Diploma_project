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
    internal class My_Tcp
    {
        public static void send_tcp()
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
                Type = "tcp",
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
            var json8484 = JsonConvert.SerializeObject(data);

            // Create a TCP client and connect to port 8484 on the local host
            using (var client8484 = new TcpClient())
            {
                var endpoint = new IPEndPoint(IPAddress.Loopback, 8484);
                client8484.Connect(endpoint);

                // Get a network stream for the client
                var stream8484 = client8484.GetStream();

                // Convert the JSON to bytes and write it to the stream
                var bytes = Encoding.UTF8.GetBytes(json8484);
                stream8484.Write(bytes, 0, bytes.Length);
            }

            // Set up the listener socket
            TcpListener listener = new TcpListener(IPAddress.Any, 8485);
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
                byte[] packetdata = Convert.FromBase64String(obj.packet);

                CaptureDeviceList devices = CaptureDeviceList.Instance;
                for (int i = 0; i < devices.Count; i++)
                {
                    Console.WriteLine("{0}.{1}\n", i, devices[i]);
                    Console.WriteLine("Choose a device");
                }
                int dev = 0;
                if (!int.TryParse(Console.ReadLine(), out dev) || dev > devices.Count)
                {
                    Console.WriteLine("Incorrect device number");
                    Console.ReadLine();
                    return;
                }
                IInjectionDevice injectionDevice = devices[dev];
                ICaptureDevice device = devices[dev];
                device.Open();
                try
                {
                    injectionDevice.SendPacket(packetdata);
                    Console.WriteLine("-- Packet send successfuly.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("--" + e.Message);
                }
                device.Close();

                // Clean up
                client.Close();
            }
        }
    }
}
