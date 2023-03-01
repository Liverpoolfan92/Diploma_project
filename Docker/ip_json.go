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
	Ttl     int    `json:"Ttl"`
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

		// Open device for sending packets
		handle, err := pcap.OpenLive("eth0", 65535, true, pcap.BlockForever)
		if err != nil {
			log.Fatal(err)
		}
		defer handle.Close()

		// Create Ethernet layer
		eth := &layers.Ethernet{
			SrcMAC:       net.HardwareAddr(packetData.SrcMac),
			DstMAC:       net.HardwareAddr(packetData.DstMac),
			EthernetType: layers.EthernetTypeIPv4,
		}

		// Create IP layer
		ip := &layers.IPv4{
			Version:  4,
			TTL:      uint8(packetData.Ttl),
			SrcIP:    net.IP(packetData.SrcIp),
			DstIP:    net.IP(packetData.DstIp),
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
