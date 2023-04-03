using System;
using System.Net;
using System.Net.Sockets;
using Packetgenerator_cli.protocols;

class Program
{
    static void Main(string[] args)
    {
        string comand = Console.ReadLine();


        switch (comand)
        {
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
                string file_path = Console.ReadLine();
                My_Pcap.send_pcap(file_path);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}



