using System;
using System.Collections.Generic;
using SharpPcap;
using PacketDotNet;
using SharpPcap.LibPcap;
using System.Net.NetworkInformation;

namespace PcapSender
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the list of available network interfaces
            var devices = CaptureDeviceList.Instance;
            // Print out the available network interfaces
            Console.WriteLine("Available Interfaces:");
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {devices[i].Description}");
            }

            // Prompt the user to select a network interface
            Console.Write("Enter the number of the interface to use: ");
            int interfaceIndex = int.Parse(Console.ReadLine()) - 1;

            // Open the selected network interface for capturing
            var device = devices[interfaceIndex];
            device.Open();

            // Load the pcap file
            var packetList = new List<Packet>();
            var fileReader = new CaptureFileReaderDevice(args[0]);
            fileReader.Open();

            // Create a PacketCommunicator from the CaptureFileReaderDevice
            var communicator = new LibPcapLiveDevice(fileReader.Name).Open();

            // Loop through the packets in the pcap file
            RawCapture rawPacket;
            Packet packet;
            while (communicator.ReceivePacket(out rawPacket) == SharpPcap.Common.PacketCommunicatorReceiveResult.Ok)
            {
                packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                packetList.Add(packet);
            }

            // Print out the packets in the pcap file
            Console.WriteLine("Packets in file:");
            for (int i = 0; i < packetList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {packetList[i].ToString()}");
            }

            // Prompt the user to select a packet to send
            Console.Write("Enter the number of the packet to send: ");
            int packetIndex = int.Parse(Console.ReadLine()) - 1;
            var selectedPacket = packetList[packetIndex];

            // Send the packet
            device.SendPacket(selectedPacket);

            // Close the network interface
            device.Close();

            Console.WriteLine("Packet sent successfully.");
        }
    }
}