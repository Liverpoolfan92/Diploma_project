using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PacketSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter source MAC address:");
            string srcMacStr = Console.ReadLine();
            Console.WriteLine("Enter destination MAC address:");
            string dstMacStr = Console.ReadLine();
            Console.WriteLine("Enter source IP address:");
            string srcIPStr = Console.ReadLine();
            Console.WriteLine("Enter destination IP address:");
            string dstIPStr = Console.ReadLine();
            Console.WriteLine("Enter source port:");
            string srcPortStr = Console.ReadLine();
            Console.WriteLine("Enter destination port:");
            string dstPortStr = Console.ReadLine();
            Console.WriteLine("Enter payload:");
            string payload = Console.ReadLine();

            JObject json = new JObject();
            json.Add("srcMac", srcMacStr);
            json.Add("dstMac", dstMacStr);
            json.Add("srcIP", srcIPStr);
            json.Add("dstIP", dstIPStr);
            json.Add("srcPort", srcPortStr);
            json.Add("dstPort", dstPortStr);
            json.Add("payload", payload);

            string jsonStr = json.ToString();

            TcpClient client = new TcpClient("localhost", 8484);
            StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
            writer.Write(jsonStr);
            writer.Flush();
            client.Close();
        }
    }
}
