using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace JSON_Communicator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a TCP listener on port 8484
            TcpListener listener = new TcpListener(IPAddress.Parse("172.20.160.1"), 8484);
            listener.Start();
            Console.WriteLine("Listening on port 8484...");

            // Wait for an incoming connection
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Received incoming connection.");

            // Read the incoming data as a string
            byte[] buffer = new byte[1024];
            int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
            string incomingData = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Parse the incoming JSON data into an array
            string[] interfaces = JsonConvert.DeserializeObject<string[]>(incomingData);

            // Choose a random interface from the array
            Random random = new Random();
            int index = random.Next(0, interfaces.Length - 1);
            string chosenInterface = interfaces[index];

            // Send the chosen interface to port 8485
            TcpClient sendClient = new TcpClient("localhost", 8485);
            byte[] sendBuffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chosenInterface));
            sendClient.GetStream().Write(sendBuffer, 0, sendBuffer.Length);
            Console.WriteLine("Sent chosen interface to port 8485.");
        }
    }
}
