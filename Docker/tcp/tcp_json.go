package tcp

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
	Flags   int    `json:"Flags"`
	WinSize int    `json:"WinSize"`
	Payload string `json:"Payload"`
}

func Handle_tcp(packettcp PacketTCP) {

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

	fmt.Print(packettcp.Flags)

	// Create TCP layer
	tcp := &layers.TCP{
		SrcPort: layers.TCPPort(packettcp.SrcPort),
		DstPort: layers.TCPPort(packettcp.DstPort),
		Seq:     uint32(packettcp.SeqNum),
		Ack:     uint32(packettcp.AckNum),
		Window:  uint16(packettcp.WinSize),
		SYN:     (packettcp.Flags & (1 << 1)) != 0,
		ACK:     (packettcp.Flags & (1 << 4)) != 0,
		RST:     (packettcp.Flags & (1 << 2)) != 0,
		PSH:     (packettcp.Flags & (1 << 3)) != 0,
		URG:     (packettcp.Flags & (1 << 5)) != 0,
		ECE:     (packettcp.Flags & (1 << 6)) != 0,
		CWR:     (packettcp.Flags & (1 << 7)) != 0,
	}
	//calculate the checksum of a packet with eth, ip, tcp layers and payload
	tcp.SetNetworkLayerForChecksum(ip)

	// Create packet with all the layers
	buffer := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		ComputeChecksums: true,
		FixLengths:       true,
	}
	err = gopacket.SerializeLayers(buffer, opts, eth, ip, tcp, gopacket.Payload([]byte(packettcp.Payload)))
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
