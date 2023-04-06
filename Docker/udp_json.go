package udp

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

type PacketUDP struct {
	SrcIP   string `json:"srcIP"`
	DstIP   string `json:"dstIP"`
	SrcPort int    `json:"srcPort"`
	DstPort int    `json:"dstPort"`
	SrcMAC  string `json:"srcMAC"`
	DstMAC  string `json:"dstMAC"`
	Payload string `json:"payload"`
}

type Packet struct {
	Packet []byte `json:"packet"`
}

func handle_udp(packetudp PacketUDP) {

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
	udp.SetNetworkLayerForChecksum(ip)

	// Create packet with all the layers
	buffer := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		ComputeChecksums: true,
		FixLengths:       true,
	}
	err = gopacket.SerializeLayers(buffer, opts, eth, ip, udp, gopacket.Payload([]byte(packetudp.Payload)))
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
