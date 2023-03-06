using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PacketSender
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the pcap file
            string pcapFile = "example.pcap";
            CaptureFileReaderDevice captureFileReader = new CaptureFileReaderDevice(pcapFile);

            // Parse the captured packets
            List<Packet> packets = new List<Packet>();
            RawCapture rawCapture;
            do
            {
                rawCapture = captureFileReader.GetNextPacket();
                if (rawCapture != null)
                {
                    Packet packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
                    packets.Add(packet);
                }
            } while (rawCapture != null);

            // Display the packet data to the user
            Console.WriteLine($"Found {packets.Count} packets in {pcapFile}:");
            for (int i = 0; i < packets.Count; i++)
            {
                Console.WriteLine($"[{i}] {packets[i].ToString()}");
            }

            // Prompt the user to choose which packet to send
            Console.Write("Choose a packet to send (0-{0}): ", packets.Count - 1);
            int selectedIndex = int.Parse(Console.ReadLine());
            Packet selectedPacket = packets[selectedIndex];

            // Convert the selected packet to a JSON string
            string json = JsonConvert.SerializeObject(selectedPacket);

            // Send the JSON string over the network to port 8484 using a TcpClient
            using (TcpClient client = new TcpClient())
            {
                client.Connect("localhost", 8484);
                using (StreamWriter writer = new StreamWriter(client.GetStream()))
                {
                    writer.Write(json);
                }
            }

            Console.WriteLine("Packet sent successfully!");
            Console.ReadLine();
        }
    }
}
