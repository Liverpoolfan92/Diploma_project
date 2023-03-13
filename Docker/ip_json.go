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

type PacketIP struct {
	SrcMac  string `json:"SrcMac"`
	DstMac  string `json:"DstMac"`
	SrcIp   string `json:"SrcIp"`
	DstIp   string `json:"DstIp"`
	SrcPort int    `json:"SrcPort"`
	DstPort int    `json:"DstPort"`
	Payload string `json:"Payload"`
	TTL     int    `json:"TTL"`
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
		var packetIp PacketIP
		err = json.NewDecoder(conn).Decode(&packetIp)
		if err != nil {
			panic(err)
		}

		// Parse the source and destination IP addresses
		srcIP := net.ParseIP(packetIp.SrcIp)
		if srcIP == nil {
			log.Println("Invalid source IP address:", packetIp.SrcIp)
			return
		}
		dstIP := net.ParseIP(packetIp.DstIp)
		if dstIP == nil {
			log.Println("Invalid destination IP address:", packetIp.DstIp)
			return
		}

		// Parse the source and destination MAC address
		srcmac, err := net.ParseMAC(packetIp.SrcMac)
		if err != nil {
			fmt.Println("Error parsing MAC address:", err)
			return
		}

		dstmac, err := net.ParseMAC(packetIp.DstMac)
		if err != nil {
			fmt.Println("Error parsing MAC address:", err)
			return
		}
		// Open device for sending packets
		handle, err := pcap.OpenLive("eth0", 65535, false, pcap.BlockForever)
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
		_ = eth
		// Create IP layer
		ip := &layers.IPv4{
			Version:  4,
			TTL:      uint8(packetIp.TTL),
			SrcIP:    srcIP,
			DstIP:    dstIP,
			Protocol: layers.IPProtocolTCP,
		}

		// Create TCP layer
		tcp := &layers.TCP{
			SrcPort: layers.TCPPort(packetIp.SrcPort),
			DstPort: layers.TCPPort(packetIp.DstPort),
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
		err = gopacket.SerializeLayers(buffer, opts, eth, ip, tcp, gopacket.Payload([]byte(packetIp.Payload)))
		if err != nil {
			log.Fatal(err)
		}
		outgoingPacket := buffer.Bytes()
		fmt.Println("JSON: ", outgoingPacket)

		ip_host := os.Getenv("VAR1")

		// Connect to TCP server at localhost:8485
		conn8485, err := net.Dial("tcp", ip_host+":8485")
		fmt.Println("IP:", ip_host)
		if err != nil {
			panic(err)
		}
		defer conn8485.Close()

		// Create JSON object containing packet bytes
		packet := Packet{Packet: outgoingPacket}
		jsonPacket, err := json.Marshal(packet)
		if err != nil {
			panic(err)
		}

		// Send JSON object to server
		_, err = conn8485.Write(jsonPacket)
		if err != nil {
			panic(err)
		}
		// Write the packet to the network interface
		err = handle.WritePacketData(outgoingPacket)
		if err != nil {
			fmt.Fprintf(os.Stderr, "Error sending packet: %v\n", err)
			os.Exit(1)
		}

		fmt.Println("Packet sent successfully!")
	}
}
