using System;
using System.Net;
using System.Net.Sockets;
using Packetgenerator_cli.protocols;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            string comand = Console.ReadLine();

            if (comand == "end")
            {
                break;
            }

            switch (comand)
            {
                case "--help":
                    Console.WriteLine("send(TYPE OF THE PACKET) to create a file \nedit() to load form pcap filee \nend to stop the program");
                    break;
                case "send(icmp)":
                    My_Icmp.send_icmp();
                    break;
                case "send(ip)":
                    My_Ip.send_ip();
                    break;
                case "send(tcp)":
                    My_Tcp.send_tcp();
                    break;
                case "send(udp)":
                    My_Udp.send_udp();
                    break;
                case "edit()":
                    Console.Write("Path to file: ");
                    string file_path = Console.ReadLine();
                    My_Pcap.send_pcap(file_path);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}



