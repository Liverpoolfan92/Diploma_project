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

type PacketICMPv4 struct {
	ICMPType int    `json:"icmpType"`
	ICMPCode int    `json:"icmpCode"`
	SrcIP    string `json:"srcIP"`
	DstIP    string `json:"dstIP"`
	SrcMAC   string `json:"srcMAC"`
	DstMAC   string `json:"dstMAC"`
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
		var packeticmpv4 PacketICMPv4
		err = json.NewDecoder(conn).Decode(&packeticmpv4)
		if err != nil {
			panic(err)
		}

		// Parse the source and destination IP addresses
		srcIP := net.ParseIP(packeticmpv4.SrcIP)
		if srcIP == nil {
			log.Println("Invalid source IP address:", packeticmpv4.SrcIP)
			return
		}
		dstIP := net.ParseIP(packeticmpv4.DstIP)
		if dstIP == nil {
			log.Println("Invalid destination IP address:", packeticmpv4.DstIP)
			return
		}

		// Parse the source and destination MAC addresses
		srcMAC, err := net.ParseMAC(packeticmpv4.SrcMAC)
		if err != nil {
			fmt.Println("Error parsing source MAC address:", err)
			return
		}
		dstMAC, err := net.ParseMAC(packeticmpv4.DstMAC)
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
			Protocol: layers.IPProtocolICMPv4,
		}
		//fix the next 15 lines?
		// next 13 lines are godlike//no idea what it does
		// Create ICMPv4 layer
		icmpv4 := &layers.ICMPv4{
			TypeCode: layers.CreateICMPv4TypeCode(uint8(packeticmpv4.ICMPType), uint8(packeticmpv4.ICMPCode)),
		}

		// Create packet with all the layers
		buffer := gopacket.NewSerializeBuffer()
		opts := gopacket.SerializeOptions{
			ComputeChecksums: true,
			FixLengths:       true,
		}
		err = gopacket.SerializeLayers(buffer, opts, eth, ip, icmpv4)
		if err != nil {
			log.Fatal(err)
		}
		outgoingPacket := buffer.Bytes()
		fmt.Println("%+v", buffer.Bytes())

		// Write the packet to the network interface
		err = handle.WritePacketData(outgoingPacket)
		if err != nil {
			fmt.Fprintf(os.Stderr, "Error sending packet: %v\n", err)
			os.Exit(1)
		}

		fmt.Println("Packet sent successfully!")
	}
}
