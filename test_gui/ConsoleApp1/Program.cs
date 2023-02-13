using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GetHostIpRange
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the host IP address range
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var unicastAddresses = ipProperties.GetUnicastAddresses();
            var ipv4Addresses = unicastAddresses
                .Where(address => address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToList();

            // Choose a random free IP address
            var random = new Random();
            IPAddress randomIpAddress = null;
            while (randomIpAddress == null)
            {
                var randomIndex = random.Next(ipv4Addresses.Count);
                var randomAddress = ipv4Addresses[randomIndex];
                var network = new IPAddressRange(randomAddress.Address, randomAddress.IPv4Mask);
                randomIpAddress = network.GetAllAddresses().Where(address => !IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().Any(conn => conn.LocalEndPoint.Address.Equals(address))).OrderBy(x => random.Next()).FirstOrDefault();
            }

            // Print the random free IP address
            Console.WriteLine("Random free IP address: " + randomIpAddress);
        }
    }

    public static class IPAddressRange
    {
        readonly AddressFamily addressFamily;
        readonly byte[] lowerBytes;
        readonly byte[] upperBytes;

        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            // Assert that lower.AddressFamily == upper.AddressFamily
            this.addressFamily = lower.AddressFamily;
            this.lowerBytes = lower.GetAddressBytes();
            this.upperBytes = upper.GetAddressBytes();
        }

        public IEnumerable<IPAddress> GetAllAddresses()
        {
            if (this.addressFamily == AddressFamily.InterNetworkV6)
            {
                throw new NotSupportedException("The IPv6 address family is not supported.");
            }

            var current = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                current[i] = (byte)(this.lowerBytes[i]);
            }

            var upperBound = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                upperBound[i] = (byte)(this.upperBytes[i]);
            }

            while (current[0] <= upperBound[0] && current[1] <= upperBound[1] && current[2] <= upperBound[2] && current[3] <= upperBound[3])
            {
                yield return new IPAddress(current);

                current[3]++;
                for (var i = 3; i >= 0; i--)
                {
                    if (current[i] > upperBound[i])
                    {
                        if (i == 0)
                        {
                            yield break;
                        }

                        current[i - 1]++;
                        current[i] = 0;
                    }
                }
            }
        }
    }
}
