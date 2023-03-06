package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net"
	"os"

	"github.com/google/gopacket"
	"github.com/google/gopacket/layers"
	"github.com/google/gopacket/pcap"
)

type PacketUdp struct {
	SrcIP   string `json:"srcIP"`
	DstIP   string `json:"dstIP"`
	SrcPort int    `json:"srcPort"`
	DstPort int    `json:"dstPort"`
	SrcMAC  string `json:"srcMAC"`
	DstMAC  string `json:"dstMAC"`
	Payload string `json:"payload"`
}

func main() {
	// Listen on port 8484 for incoming connections
	listener, err := net.Listen("tcp", ":8484")
	if err != nil {
		panic(err)
	}
	defer listener.Close()

	for {
		// Wait for a client to connect
		conn, err := listener.Accept()
		if err != nil {
			panic(err)
		}

		// Parse the JSON data sent by the client
		var packetudp PacketUdp
		err = json.NewDecoder(conn).Decode(&packetudp)
		if err != nil {
			panic(err)
		}

		// Parse the source and destination IP addresses
		srcIP := net.ParseIP(packetudp.SrcIP)
		if srcIP == nil {
			log.Println("Invalid source IP address:", packetudp.SrcIP)
			return
		}
		dstIP := net.ParseIP(packetudp.DstIP)
		if dstIP == nil {
			log.Println("Invalid destination IP address:", packetudp.DstIP)
			return
		}

		// Parse the source and destination MAC addresses
		srcMAC, err := net.ParseMAC(packetudp.SrcMAC)
		if err != nil {
			fmt.Println("Error parsing source MAC address:", err)
			return
		}
		dstMAC, err := net.ParseMAC(packetudp.DstMAC)
		if err != nil {
			fmt.Println("Error parsing destination MAC address:", err)
			return
		}

		// Open device for sending packets
		handle, err := pcap.OpenLive("eth0", 65535, true, pcap.BlockForever)
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
			TTL:      64,
			SrcIP:    srcIP,
			DstIP:    dstIP,
			Protocol: layers.IPProtocolUDP,
		}

		// Create UDP layer
		udp := &layers.UDP{
			SrcPort: layers.UDPPort(packetudp.SrcPort),
			DstPort: layers.UDPPort(packetudp.DstPort),
		}

		// Create packet with all the layers
		buffer := gopacket.NewSerializeBuffer()
		opts := gopacket.SerializeOptions{
			ComputeChecksums: true,
			FixLengths:       true,
		}
		err = gopacket.SerializeLayers(buffer, opts, eth, ip, udp)
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
}
