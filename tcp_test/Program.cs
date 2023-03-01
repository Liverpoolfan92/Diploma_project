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
            Console.Write("SrcMAC: ");
            string srcMacStr = Console.ReadLine();

            Console.Write("DstMAC: ");
            string dstMacStr = Console.ReadLine();

            Console.Write("SrcIP: ");
            string srcIPStr = Console.ReadLine();

            Console.Write("DstIP: ");
            string dstIPStr = Console.ReadLine();

            Console.Write("SrcPort: ");
            int srcPort = Convert.ToInt32(Console.ReadLine());

            Console.Write("DstPort: ");
            int dstPort = Convert.ToInt32(Console.ReadLine());

            Console.Write("TTL: ");
            int ttl = Convert.ToInt32(Console.ReadLine());

            Console.Write("SeqNum: ");
            int seqNum = Convert.ToInt32(Console.ReadLine());

            Console.Write("AckNum: ");
            int ackNum = Convert.ToInt32(Console.ReadLine());

            Console.Write("Flags: ");
            string flagsStr = Console.ReadLine();

            Console.Write("WinSize: ");
            int winSize = Convert.ToInt32(Console.ReadLine());

            Console.Write("Payload: ");
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
