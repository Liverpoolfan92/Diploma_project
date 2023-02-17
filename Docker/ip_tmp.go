package main

import (
    "fmt"
    "log"
    "net"
    "os"

    "github.com/google/gopacket"
    "github.com/google/gopacket/layers"
    "github.com/google/gopacket/pcap"
)

func main() {
    // Get device name and other user inputs
    device := "eth0" // replace with the user input
    srcMAC, _ := net.ParseMAC("00:11:22:33:44:55") // replace with the user input
    dstMAC, _ := net.ParseMAC("66:77:88:99:aa:bb") // replace with the user input
    srcIP := net.ParseIP("192.168.0.2") // replace with the user input
    dstIP := net.ParseIP("192.168.0.3") // replace with the user input
    srcPort := layers.TCPPort(1234) // replace with the user input
    dstPort := layers.TCPPort(5678) // replace with the user input
    payload := []byte("Hello, world!") // replace with the user input
    ttl := uint8(64) // replace with the user input

    // Open device for sending packets
    handle, err := pcap.OpenLive(device, 65535, true, pcap.BlockForever)
    if err != nil {
        log.Fatal(err)
    }
    defer handle.Close()

    // Create Ethernet layer
    eth := &layers.Ethernet{
        SrcMAC:       srcMAC,
        DstMAC:       dstMAC,
        EthernetType: layers.EthernetTypeIPv4,
    }

    // Create IP layer
    ip := &layers.IPv4{
        Version:  4,
        TTL:      ttl,
        SrcIP:    srcIP,
        DstIP:    dstIP,
        Protocol: layers.IPProtocolTCP,
    }

    // Create TCP layer
    tcp := &layers.TCP{
        SrcPort: srcPort,
        DstPort: dstPort,
        Seq:     100,
        SYN:     true,
    }
    tcp.SetNetworkLayerForChecksum(ip)

    // Create payload layer
    payloadLayer := &gopacket.Payload{
        Payload: payload,
    }

    // Create packet with all the layers
    buffer := gopacket.NewSerializeBuffer()
    opts := gopacket.SerializeOptions{
        ComputeChecksums: true,
        FixLengths:       true,
    }
    err = gopacket.SerializeLayers(buffer, opts, eth, ip, tcp, payloadLayer)
    if err != nil {
        log.Fatal(err)
    }
    outgoingPacket := buffer.Bytes()

    // Write the packet to the network interface
    err = handle.WritePacketData(outgoingPacket)
    if err != nil {
        fmt.Fprintf(os.Stderr, "Error sending packet: %v\n", err)
        os.Exit(1)
    }

    fmt.Println("Packet sent successfully!")
}
