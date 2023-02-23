package main

import (
	"encoding/hex"
	"fmt"
	"net"
	"os"

	"github.com/google/gopacket/layers"
)

func main() {
	// Ask user for ICMP type and code
	var icmpType, icmpCode uint8
	fmt.Print("Enter ICMP type (0-255): ")
	fmt.Scanln(&icmpType)
	fmt.Print("Enter ICMP code (0-255): ")
	fmt.Scanln(&icmpCode)

	// Ask user for source and destination IP addresses
	var srcIP, dstIP string
	fmt.Print("Enter source IP address: ")
	fmt.Scanln(&srcIP)
	fmt.Print("Enter destination IP address: ")
	fmt.Scanln(&dstIP)

	// Parse IP addresses
	srcAddr := net.ParseIP(srcIP)
	if srcAddr == nil {
		fmt.Printf("Error parsing source IP address: %s\n", srcIP)
		os.Exit(1)
	}

	dstAddr := net.ParseIP(dstIP)
	if dstAddr == nil {
		fmt.Printf("Error parsing destination IP address: %s\n", dstIP)
		os.Exit(1)
	}

	// Ask user for payload
	var payload string
	fmt.Print("Enter payload (hex string): ")
	fmt.Scanln(&payload)

	// Parse payload
	payloadBytes, err := hex.DecodeString(payload)
	if err != nil {
		fmt.Printf("Error decoding payload: %s\n", err)
		os.Exit(1)
	}

	// Create IP header
	ipHeader := ip.Header{
		Version:  4,
		Len:      20,
		TOS:      0,
		TotalLen: 20 + icmpv4.HeaderLen + len(payloadBytes),
		ID:       0,
		Flags:    2,
		FragOff:  0,
		TTL:      64,
		Proto:    1,
		Src:      srcAddr,
		Dst:      dstAddr,
		Options:  nil,
	}

	// Create ICMPv4 header
	icmpHeader := icmpv4.Header{
		Type: icmpType,
		Code: icmpCode,
		ID:   0,
		Seq:  0,
	}

	// Create ICMPv4 packet
	icmpPacket := icmpv4.NewPacket(&ipHeader, &icmpHeader, payloadBytes)

	// Send ICMPv4 packet
	conn, err := net.DialIP("ip4:icmp", &net.IPAddr{IP: srcAddr}, &net.IPAddr{IP: dstAddr})
	if err != nil {
		fmt.Printf("Error opening connection: %s\n", err)
		os.Exit(1)
	}
	defer conn.Close()

	if _, err := conn.Write(icmpPacket); err != nil {
		fmt.Printf("Error sending packet: %s\n", err)
		os.Exit(1)
	}

	fmt.Println("Packet sent successfully!")
}
