using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

class Program
{
    static void Main(string[] args)
    {
        string fileName = "C:\\Users\\zhele\\Documents\\GitHub\\Diploma_project\\pcap_test\\example.pcapng";
        var device = new CaptureFileReaderDevice(fileName);
        device.Open();

        device.OnPacketArrival += (sender, e) =>
        {
            var packet = PacketDotNet.Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
            if (packet is EthernetPacket)
            {
                var ethPacket = (EthernetPacket)packet;

                if (ethPacket.PayloadPacket is IPv4Packet)
                {
                    var ipPacket = (IPv4Packet)ethPacket.PayloadPacket;
                    Console.WriteLine($"protocol: {ipPacket.PayloadPacket.ToString()}" +" "+ ipPacket.ToString());
                };
            }
        };

        device.OnPacketArrival += (sender, e) =>
        {
            var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);

            if (packet is EthernetPacket)
            {
                var ethPacket = (EthernetPacket)packet;

                if (ethPacket.PayloadPacket is IPv4Packet)
                {
                    var ipPacket = (IPv4Packet)ethPacket.PayloadPacket;

                    if (ipPacket.PayloadPacket is TcpPacket)
                    {
                        var tcpPacket = (TcpPacket)ipPacket.PayloadPacket;

                        Console.WriteLine($"Source MAC: {ethPacket.SourceHardwareAddress}");
                        Console.WriteLine($"Destination MAC: {ethPacket.DestinationHardwareAddress}");
                        Console.WriteLine($"Source IP: {ipPacket.SourceAddress}");
                        Console.WriteLine($"Destination IP: {ipPacket.DestinationAddress}");
                        Console.WriteLine($"Source Port: {tcpPacket.SourcePort}");
                        Console.WriteLine($"Destination Port: {tcpPacket.DestinationPort}");
                        Console.WriteLine($"Sequence Number: {tcpPacket.SequenceNumber}");
                        Console.WriteLine($"Acknowledgment Number: {tcpPacket.AcknowledgmentNumber}");
                        Console.WriteLine($"Window Size: {tcpPacket.WindowSize}");
                        Console.WriteLine($"TCP Flags: {tcpPacket.Flags}");
                        Console.WriteLine($"TTL: {ipPacket.TimeToLive}");

                        Console.Write("Enter new source MAC address: ");
                        PhysicalAddress srcMAC = PhysicalAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new destination MAC address: ");
                        PhysicalAddress dstMAC = PhysicalAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new source IP address: ");
                        IPAddress srcIP = IPAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new destination IP address: ");
                        IPAddress dstIP = IPAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new source port: ");
                        ushort srcPort = ushort.Parse(Console.ReadLine());
                        Console.Write("Enter new destination port: ");
                        ushort dstPort = ushort.Parse(Console.ReadLine());
                        /*Console.Write("Enter new TCP flags: ");
                        TcpFlags flags = (TcpFlags)Enum.Parse(typeof(TcpFlags), Console.ReadLine(), true);*/
                        Console.Write("Enter new sequence number: ");
                        uint seqNum = uint.Parse(Console.ReadLine());
                        Console.Write("Enter new acknowledgment number: ");
                        uint ackNum = uint.Parse(Console.ReadLine());
                        Console.Write("Enter new window size: ");
                        ushort windowSize = ushort.Parse(Console.ReadLine());
                        Console.Write("Enter new TTL: ");
                        byte ttl = byte.Parse(Console.ReadLine());
                        Console.Write("Enter new payload data (in hex format): ");
                        byte[] payload = Encoding.UTF8.GetBytes(Console.ReadLine());
                        

                        // Update packet
                        ethPacket.SourceHardwareAddress = srcMAC;
                        ethPacket.DestinationHardwareAddress = dstMAC;
                        ipPacket.SourceAddress = srcIP;
                        ipPacket.DestinationAddress = dstIP;
                        tcpPacket.SourcePort = srcPort;
                        tcpPacket.DestinationPort = dstPort;
                        //tcpPacket.Flags = flags;
                        tcpPacket.SequenceNumber = seqNum;
                        tcpPacket.AcknowledgmentNumber = ackNum;
                        tcpPacket.WindowSize = windowSize;
                        ipPacket.TimeToLive = ttl;

                        // Recalculate checksums
                        tcpPacket.Checksum = tcpPacket.CalculateTcpChecksum();
                        ipPacket.Checksum = ipPacket.CalculateIPChecksum();
                        // Re-serialize packet and print updated packet info
                        var updatedPacket = ethPacket.Bytes;

                        sendPacket(updatedPacket);

                        Console.WriteLine($"Updated packet: {BitConverter.ToString(updatedPacket)}");
                        Console.WriteLine($"New Source MAC: {ethPacket.SourceHardwareAddress}");
                        Console.WriteLine($"New Destination MAC: {ethPacket.DestinationHardwareAddress}");
                        Console.WriteLine($"New Source IP: {ipPacket.SourceAddress}");
                        Console.WriteLine($"New Destination IP: {ipPacket.DestinationAddress}");
                        Console.WriteLine($"New Source Port: {tcpPacket.SourcePort}");
                        Console.WriteLine($"New Destination Port: {tcpPacket.DestinationPort}");
                        Console.WriteLine($"New Sequence Number: {tcpPacket.SequenceNumber}");
                        Console.WriteLine($"New Acknowledgment Number: {tcpPacket.AcknowledgmentNumber}");
                        Console.WriteLine($"New Window Size: {tcpPacket.WindowSize}");
                        Console.WriteLine($"New TCP Flags: {tcpPacket.Flags}");
                        Console.WriteLine($"New TTL: {ipPacket.TimeToLive}");
                    }

                    if (ipPacket.PayloadPacket is UdpPacket)
                    {
                        var udpPacket = (UdpPacket)ipPacket.PayloadPacket;

                        Console.WriteLine($"Source MAC: {ethPacket.SourceHardwareAddress}");
                        Console.WriteLine($"Destination MAC: {ethPacket.DestinationHardwareAddress}");
                        Console.WriteLine($"Source IP: {ipPacket.SourceAddress}");
                        Console.WriteLine($"Destination IP: {ipPacket.DestinationAddress}");
                        Console.WriteLine($"Source Port: {udpPacket.SourcePort}");
                        Console.WriteLine($"Destination Port: {udpPacket.DestinationPort}");
                        Console.WriteLine($"TTL: {ipPacket.TimeToLive}");

                        Console.Write("Enter new source MAC address: ");
                        PhysicalAddress srcMAC = PhysicalAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new destination MAC address: ");
                        PhysicalAddress dstMAC = PhysicalAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new source IP address: ");
                        IPAddress srcIP = IPAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new destination IP address: ");
                        IPAddress dstIP = IPAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new source port: ");
                        ushort srcPort = ushort.Parse(Console.ReadLine());
                        Console.Write("Enter new destination port: ");
                        ushort dstPort = ushort.Parse(Console.ReadLine());
                        Console.Write("Enter new TTL: ");
                        byte ttl = byte.Parse(Console.ReadLine());
                        Console.Write("Enter new payload data (in hex format): ");
                        byte[] payload = Encoding.UTF8.GetBytes(Console.ReadLine());

                        // Update packet
                        ethPacket.SourceHardwareAddress = srcMAC;
                        ethPacket.DestinationHardwareAddress = dstMAC;
                        ipPacket.SourceAddress = srcIP;
                        ipPacket.DestinationAddress = dstIP;
                        udpPacket.SourcePort = srcPort;
                        udpPacket.DestinationPort = dstPort;
                        ipPacket.TimeToLive = ttl;

                        // Recalculate checksums
                        udpPacket.Checksum = udpPacket.CalculateUdpChecksum();
                        ipPacket.Checksum = ipPacket.CalculateIPChecksum();
                        // Re-serialize packet and print updated packet info
                        var updatedPacket = ethPacket.Bytes;
                        
                        sendPacket(updatedPacket);
                        
                        Console.WriteLine($"Updated packet: {BitConverter.ToString(updatedPacket)}");
                        Console.WriteLine($"New Source MAC: {ethPacket.SourceHardwareAddress}");
                        Console.WriteLine($"New Destination MAC: {ethPacket.DestinationHardwareAddress}");
                        Console.WriteLine($"New Source IP: {ipPacket.SourceAddress}");
                        Console.WriteLine($"New Destination IP: {ipPacket.DestinationAddress}");
                        Console.WriteLine($"New Source Port: {udpPacket.SourcePort}");
                        Console.WriteLine($"New Destination Port: {udpPacket.DestinationPort}");;
                        Console.WriteLine($"New TTL: {ipPacket.TimeToLive}");
                    }

                    if(ipPacket.PayloadPacket is IcmpV4Packet)
                    {
                        var icmpPacket = (IcmpV4Packet)ipPacket.PayloadPacket;

                        Console.WriteLine($"Source MAC: {ethPacket.SourceHardwareAddress}");
                        Console.WriteLine($"Destination MAC: {ethPacket.DestinationHardwareAddress}");
                        Console.WriteLine($"Source IP: {ipPacket.SourceAddress}");
                        Console.WriteLine($"Destination IP: {ipPacket.DestinationAddress}");
                        Console.WriteLine($"ICMPType: {icmpPacket.TypeCode}");

                        Console.Write("Enter new source MAC address: ");
                        PhysicalAddress srcMAC = PhysicalAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new destination MAC address: ");
                        PhysicalAddress dstMAC = PhysicalAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new source IP address: ");
                        IPAddress srcIP = IPAddress.Parse(Console.ReadLine());
                        Console.Write("Enter new destination IP address: ");
                        IPAddress dstIP = IPAddress.Parse(Console.ReadLine());
                        Console.WriteLine("Enter new Type: ");
                        int icmptype = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter new Code: ");
                        int icmpCode = Convert.ToInt32(Console.ReadLine());

                        // Update packet
                        ethPacket.SourceHardwareAddress = srcMAC;
                        ethPacket.DestinationHardwareAddress = dstMAC;
                        ipPacket.SourceAddress = srcIP;
                        ipPacket.DestinationAddress = dstIP;
                        byte icmpTypeCode = (byte)((icmptype << 8) | icmpCode);
                        icmpPacket.TypeCode = (IcmpV4TypeCode)icmpTypeCode;

                        // Recalculate checksums
                        icmpPacket.Checksum = icmpPacket.CalculateIcmpChecksum();
                        ipPacket.Checksum = ipPacket.CalculateIPChecksum();
                        // Re-serialize packet and print updated packet info
                        var updatedPacket = ethPacket.Bytes;

                        sendPacket(updatedPacket);

                        Console.WriteLine($"Updated packet: {BitConverter.ToString(updatedPacket)}");
                        Console.WriteLine($"New Source MAC: {ethPacket.SourceHardwareAddress}");
                        Console.WriteLine($"New Destination MAC: {ethPacket.DestinationHardwareAddress}");
                        Console.WriteLine($"New Source IP: {ipPacket.SourceAddress}");
                        Console.WriteLine($"New Destination IP: {ipPacket.DestinationAddress}");
                        Console.WriteLine($"ICMPType: {icmpPacket.TypeCode}");
                    }
                }
            }
        };

        device.Capture();
    }
    public static void sendPacket(byte[] packet)
    {
        CaptureDeviceList devices = CaptureDeviceList.Instance;
        for (int i = 0; i < devices.Count; i++)
        {
            Console.WriteLine("{0}.{1}\n", i, devices[i]);
            Console.WriteLine("Choose a device");
        }
        int dev = 0;
        if ((!Int32.TryParse(Console.ReadLine(), out dev)) || (dev > devices.Count))
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
            injectionDevice.SendPacket(packet);
            //device.SendPacket(obj.BYTES);
            Console.WriteLine("-- Packet send successfuly.");
        }
        catch (Exception e)
        {
            Console.WriteLine("--" + e.Message);
        }
        device.Close();
    }
}
