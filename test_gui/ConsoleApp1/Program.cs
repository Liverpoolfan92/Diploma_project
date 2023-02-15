using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

class Program
{
    static void Main(string[] args)
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface ni in interfaces)
        {
            if(ni.Name.EndsWith("(Default Switch)"))
            {
                Console.WriteLine("Interface Name: {0}", ni.Name);

                IPInterfaceProperties ipProps = ni.GetIPProperties();
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    Console.WriteLine("\tIP Address: {0}", addr.Address);
                }
            }
        }

        Console.ReadLine();
    }
}
