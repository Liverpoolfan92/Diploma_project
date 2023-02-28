package main

import (
	"fmt"
	"net"
	"os"
	"strconv"

	"github.com/google/gopacket"
	"github.com/google/gopacket/layers"
)

func main() {
	// Get input from user
	var srcMacStr, dstMacStr, srcIPStr, dstIPStr, srcPortStr, dstPortStr, ttlStr, seqNumStr, ackNumStr, flagsStr, winSizeStr string
	var payload []byte

	fmt.Print("Enter source MAC address (e.g. 00:11:22:33:44:55): ")
	fmt.Scanln(&srcMacStr)

	fmt.Print("Enter destination MAC address (e.g. 00:11:22:33:44:55): ")
	fmt.Scanln(&dstMacStr)

	fmt.Print("Enter source IP address: ")
	fmt.Scanln(&srcIPStr)

	fmt.Print("Enter destination IP address: ")
	fmt.Scanln(&dstIPStr)

	fmt.Print("Enter source port number: ")
	fmt.Scanln(&srcPortStr)

	fmt.Print("Enter destination port number: ")
	fmt.Scanln(&dstPortStr)

	fmt.Print("Enter TTL: ")
	fmt.Scanln(&ttlStr)

	fmt.Print("Enter sequence number: ")
	fmt.Scanln(&seqNumStr)

	fmt.Print("Enter acknowledgment number: ")
	fmt.Scanln(&ackNumStr)

	fmt.Print("Enter TCP flags (e.g. SYN, ACK, FIN): ")
	fmt.Scanln(&flagsStr)

	fmt.Print("Enter window size: ")
	fmt.Scanln(&winSizeStr)

	fmt.Print("Enter payload: ")
	fmt.Scanln(&payload)

	// Parse input values
	srcMac, err := net.ParseMAC(srcMacStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse source MAC address: %v", err)
		return
	}

	dstMac, err := net.ParseMAC(dstMacStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse destination MAC address: %v", err)
		return
	}

	srcIP := net.ParseIP(srcIPStr)
	if srcIP == nil {
		fmt.Fprintf(os.Stderr, "Failed to parse source IP address: %v", err)
		return
	}

	dstIP := net.ParseIP(dstIPStr)
	if dstIP == nil {
		fmt.Fprintf(os.Stderr, "Failed to parse destination IP address: %v", err)
		return
	}

	srcPort, err := strconv.Atoi(srcPortStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse source port number: %v", err)
		return
	}

	dstPort, err := strconv.Atoi(dstPortStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse destination port number: %v", err)
		return
	}

	ttl, err := strconv.Atoi(ttlStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse TTL: %v", err)
		return
	}

	seqNum, err := strconv.Atoi(seqNumStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse sequence number: %v", err)
		return
	}

	ackNum, err := strconv.Atoi(ackNumStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse acknowledgment number: %v", err)
		return
	}

	var flags layers.TCPFlag
	switch flagsStr {
	case "FIN":
		flags = layers.TCPFlagFin
	case "SYN":
		flags = layers.TCPFlagSyn
	case "RST":
		flags = layers
	case "PSH":
		flags = layers.TCPFlagPsh
	case "ACK":
		flags = layers.TCPFlagAck
	case "URG":
		flags = layers.TCPFlagUrg
	case "ECE":
		flags = layers.TCPFlagECE
	case "CWR":
		flags = layers.TCPFlagCWR
	default:
		fmt.Fprintf(os.Stderr, "Invalid TCP flags")
		return
	}

	winSize, err := strconv.Atoi(winSizeStr)
	if err != nil {
		fmt.Fprintf(os.Stderr, "Failed to parse window size: %v", err)
		return
	}

	// Create packet
	buf := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		FixLengths:       true,
		ComputeChecksums: true,
	}

	// Ethernet layer
	ethLayer := &layers.Ethernet{
		SrcMAC:       srcMac,
		DstMAC:       dstMac,
		EthernetType: layers.EthernetTypeIPv4,
	}
	if err := ethLayer.SerializeTo(buf, opts); err != nil {
		fmt.Fprintf(os.Stderr, "Failed to serialize Ethernet layer: %v", err)
		return
	}

	// IP layer
	ipLayer := &layers.IPv4{
		Version:  4,
		TTL:      uint8(ttl),
		SrcIP:    srcIP,
		DstIP:    dstIP,
		Protocol: layers.IPProtocolTCP,
	}
	if err := ipLayer.SerializeTo(buf, opts); err != nil {
		fmt.Fprintf(os.Stderr, "Failed to serialize IP layer: %v", err)
		return
	}

	// TCP layer
	tcpLayer := &layers.TCP{
		SrcPort: layers.TCPPort(srcPort),
		DstPort: layers.TCPPort(dstPort),
		Seq:     uint32(seqNum),
		Ack:     uint32(ackNum),
		Window:  uint16(winSize),
		Flags:   flags,
	}
	if err := tcpLayer.SetNetworkLayerForChecksum(ipLayer); err != nil {
		fmt.Fprintf(os.Stderr, "Failed to set network layer for TCP checksum: %v", err)
		return
	}
	if err := tcpLayer.SerializeTo(buf, opts); err != nil {
		fmt.Fprintf(os.Stderr, "Failed to serialize TCP layer: %v", err)
		return
	}

	// Payload layer
	if len(payload) > 0 {
		if err := gopacket.SerializeLayers(buf, opts, gopacket.Payload(payload)); err != nil {
			fmt.Fprintf(os.Stderr, "Failed to serialize payload layer: %v", err)
			return
		}
	}

	// Send packet
	packetData := buf.Bytes()
	fmt.Printf("Sending packet with length %d\n", len(packetData))
	fmt.Printf("Packet data: %v\n", packetData)
}
