using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpPcap;

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
                    injectionDevice.SendPacket(packetdata);
                    //device.SendPacket(obj.BYTES);
                    Console.WriteLine("-- Packet send successfuly.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("--" + e.Message);
                }
                device.Close();
                Console.WriteLine("-- Device closed. ");
                // Do something with the object
                Console.WriteLine($"Received object with property1={obj.packet}");

                // Clean up
                client.Close();
            }
        }
    }

    class RAWPACKET
    {
        public string packet { get; set; }
    }
}
