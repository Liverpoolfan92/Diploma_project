package icmpv4

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

type Packet struct {
	Packet []byte `json:"packet"`
}

type PacketICMPv4 struct {
	ICMPType int    `json:"icmpType"`
	ICMPCode int    `json:"icmpCode"`
	SrcIP    string `json:"srcIP"`
	DstIP    string `json:"dstIP"`
	SrcMAC   string `json:"srcMAC"`
	DstMAC   string `json:"dstMAC"`
}

func Handle_icmp(packeticmpv4 PacketICMPv4) {

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
	fmt.Println("JSON: ", outgoingPacket)

	ip_host := os.Getenv("VAR1")

	// Connect to TCP server at localhost:8485
	conn8485, err := net.Dial("tcp", ip_host+":8485")
	fmt.Println("IP:", ip_host)
	if err != nil {
		log.Fatal(err)
	}
	defer conn8485.Close()

	// Create JSON object containing packet bytes
	packet := Packet{Packet: outgoingPacket}
	jsonPacket, err := json.Marshal(packet)
	if err != nil {
		log.Fatal(err)
	}

	// Send JSON object to server
	_, err = conn8485.Write(jsonPacket)
	if err != nil {
		log.Fatal(err)
	}

	// Write the packet to the network interface
	err = handle.WritePacketData(outgoingPacket)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error sending packet: %v\n", err)
		os.Exit(1)
	}

	fmt.Println("Packet sent successfully!")
}
