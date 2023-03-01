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

type PacketData struct {
	SrcMac  string `json:"SrcMac"`
	DstMac  string `json:"DstMac"`
	SrcIp   string `json:"SrcIp"`
	DstIp   string `json:"DstIp"`
	SrcPort int    `json:"SrcPort"`
	DstPort int    `json:"DstPort"`
	Payload string `json:"Payload"`
	TTL     int    `json:"Ttl"`
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
		var packetData PacketData
		err = json.NewDecoder(conn).Decode(&packetData)
		if err != nil {
			panic(err)
		}

		// Parse the source and destination IP addresses
		srcIP := net.ParseIP(packetData.SrcIp)
		if srcIP == nil {
			log.Println("Invalid source IP address:", packetData.SrcIp)
			return
		}
		dstIP := net.ParseIP(packetData.DstIp)
		if dstIP == nil {
			log.Println("Invalid destination IP address:", packetData.DstIp)
			return
		}

		// Parse the source and destination MAC address
		srcmac, err := net.ParseMAC(packetData.SrcMac)
		if err != nil {
			fmt.Println("Error parsing MAC address:", err)
			return
		}

		dstmac, err := net.ParseMAC(packetData.DstMac)
		if err != nil {
			fmt.Println("Error parsing MAC address:", err)
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
			SrcMAC:       srcmac,
			DstMAC:       dstmac,
			EthernetType: layers.EthernetTypeIPv4,
		}

		// Create IP layer
		ip := &layers.IPv4{
			Version:  4,
			TTL:      uint8(packetData.TTL),
			SrcIP:    srcIP,
			DstIP:    dstIP,
			Protocol: layers.IPProtocolTCP,
		}

		// Create TCP layer
		tcp := &layers.TCP{
			SrcPort: layers.TCPPort(packetData.SrcPort),
			DstPort: layers.TCPPort(packetData.DstPort),
			Seq:     100,
			SYN:     true,
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
