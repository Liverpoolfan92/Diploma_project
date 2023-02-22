using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt the user for input
            Console.WriteLine("Enter the source MAC address:");
            string srcMacStr = Console.ReadLine();

            Console.WriteLine("Enter the destination MAC address:");
            string dstMacStr = Console.ReadLine();

            Console.WriteLine("Enter the source IP address:");
            string srcIPStr = Console.ReadLine();

            Console.WriteLine("Enter the destination IP address:");
            string dstIPStr = Console.ReadLine();

            Console.WriteLine("Enter the source port:");
            int srcPort = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the destination port:");
            int dstPort = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the TTL:");
            int ttl = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the sequence number:");
            int seqNum = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the acknowledgement number:");
            int ackNum = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the flags:");
            string flagsStr = Console.ReadLine();

            Console.WriteLine("Enter the window size:");
            int winSize = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the payload:");
            string payload = Console.ReadLine();

            // Create a JSON object from the input
            JObject json = new JObject(
                new JProperty("srcMacStr", srcMacStr),
                new JProperty("dstMacStr", dstMacStr),
                new JProperty("srcIPStr", srcIPStr),
                new JProperty("dstIPStr", dstIPStr),
                new JProperty("srcPort", srcPort),
                new JProperty("dstPort", dstPort),
                new JProperty("ttl", ttl),
                new JProperty("seqNum", seqNum),
                new JProperty("ackNum", ackNum),
                new JProperty("flagsStr", flagsStr),
                new JProperty("winSize", winSize),
                new JProperty("payload", payload)
            );

            // Convert the JSON object to a string
            string jsonString = json.ToString();

            // Send the JSON string to the specified TCP port on the localhost
            using (TcpClient client = new TcpClient())
            {
                client.Connect("localhost", 8484);
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(jsonString);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("JSON sent successfully.");
                }
            }

            Console.ReadLine();
        }
    }
}
