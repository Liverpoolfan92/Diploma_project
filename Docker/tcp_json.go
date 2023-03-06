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

type PacketTCP struct {
	SrcMac  string `json:"SrcMac"`
	DstMac  string `json:"DstMac"`
	SrcIP   string `json:"SrcIP"`
	DstIP   string `json:"DstIP"`
	SrcPort int    `json:"SrcPort"`
	DstPort int    `json:"DstPort"`
	TTL     int    `json:"TTL"`
	SeqNum  int    `json:"SeqNum"`
	AckNum  int    `json:"AckNum"`
	Flags   string `json:"Flags"`
	WinSize int    `json:"WinSize"`
	Payload string `json:"Payload"`
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
		var packettcp PacketTCP
		err = json.NewDecoder(conn).Decode(&packettcp)
		if err != nil {
			panic(err)
		}

		// Parse the source and destination IP addresses
		srcIP := net.ParseIP(packettcp.SrcIP)
		if srcIP == nil {
			log.Println("Invalid source IP address:", packettcp.SrcIP)
			return
		}
		dstIP := net.ParseIP(packettcp.DstIP)
		if dstIP == nil {
			log.Println("Invalid destination IP address:", packettcp.DstIP)
			return
		}

		// Parse the source and destination MAC addresses
		srcMAC, err := net.ParseMAC(packettcp.SrcMac)
		if err != nil {
			fmt.Println("Error parsing source MAC address:", err)
			return
		}
		dstMAC, err := net.ParseMAC(packettcp.DstMac)
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
			TTL:      uint8(packettcp.TTL),
			SrcIP:    srcIP,
			DstIP:    dstIP,
			Protocol: layers.IPProtocolTCP,
		}

		// Create TCP layer
		tcp := &layers.TCP{
			SrcPort: layers.TCPPort(packettcp.SrcPort),
			DstPort: layers.TCPPort(packettcp.DstPort),
			Seq:     uint32(packettcp.SeqNum),
			Ack:     uint32(packettcp.AckNum),
			Window:  uint16(packettcp.WinSize),
			SYN:     packettcp.Flags == "SYN",
			ACK:     packettcp.Flags == "ACK",
			RST:     packettcp.Flags == "RST",
			PSH:     packettcp.Flags == "PSH",
			URG:     packettcp.Flags == "URG",
		}
		tcp.SetNetworkLayerForChecksum(ip)

		// Create packet with all the layers
		buffer := gopacket.NewSerializeBuffer()
		opts := gopacket.SerializeOptions{
			ComputeChecksums: true,
			FixLengths:       true,
		}
		err = gopacket.SerializeLayers(buffer, opts, eth, ip, tcp)
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
