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
            Console.WriteLine("Enter the ICMP type:");
            int icmpType = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the ICMP code:");
            int icmpCode = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the source IP address:");
            string srcIP = Console.ReadLine();

            Console.WriteLine("Enter the destination IP address:");
            string dstIP = Console.ReadLine();

            // Create a JSON object from the input
            JObject json = new JObject(
                new JProperty("icmpType", icmpType),
                new JProperty("icmpCode", icmpCode),
                new JProperty("srcIP", srcIP),
                new JProperty("dstIP", dstIP)
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
