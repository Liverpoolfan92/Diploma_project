using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Create user-defined network
        Process.Start("docker", "network create packetgenerator");

        // Start Docker container in network
        Process.Start("docker", "run --network=packetgenerator --name=PacketGenerator -p 8484:8484 -p 8485:8485 packet_image");

        // Send host IP to container via JSON
        await SendHostIpToDocker();

        // Listen for JSON data on port 8484
        await ListenForJson();
    }

    static async Task SendHostIpToDocker()
    {
        var client = new HttpClient();
        var ip = GetHostIp();
        var content = new StringContent(JsonSerializer.Serialize(new { hostIp = ip }), System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://my_container:80/updateHostIp", content);
    }

    static async Task ListenForJson()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8484/");

        listener.Start();

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;

            if (request.ContentType == "application/json")
            {
                using (var streamReader = new System.IO.StreamReader(request.InputStream))
                {
                    var body = await streamReader.ReadToEndAsync();
                    var json = JsonSerializer.Deserialize<JsonElement>(body);

                    Console.WriteLine($"Received JSON data: {json}");
                }
            }

            context.Response.Close();
        }
    }

    static string GetHostIp()
    {
        // Replace with actual method for getting host IP address
        return "192.168.0.100";
    }
}
