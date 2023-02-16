package main

import (
	"fmt"
	"net"
	"os"
	"strconv"
	"strings"
	"time"

	"github.com/ghedo/go.pkt/eth"
	"github.com/ghedo/go.pkt/tcp"
)

func main() {
	// Prompt the user for various fields
	var srcMacStr, dstMacStr, srcIPStr, dstIPStr, srcPortStr, dstPortStr, payloadStr, ttlStr string

	fmt.Print("Enter the source MAC address: ")
	fmt.Scanln(&srcMacStr)

	fmt.Print("Enter the destination MAC address: ")
	fmt.Scanln(&dstMacStr)

	fmt.Print("Enter the source IP address: ")
	fmt.Scanln(&srcIPStr)

	fmt.Print("Enter the destination IP address: ")
	fmt.Scanln(&dstIPStr)

	fmt.Print("Enter the source port: ")
	fmt.Scanln(&srcPortStr)

	fmt.Print("Enter the destination port: ")
	fmt.Scanln(&dstPortStr)

	fmt.Print("Enter the payload: ")
	fmt.Scanln(&payloadStr)

	fmt.Print("Enter the TTL: ")
	fmt.Scanln(&ttlStr)

	// Parse the fields
	srcMac, err := net.ParseMAC(srcMacStr)
	if err != nil {
		fmt.Println("Error parsing source MAC address:", err)
		os.Exit(1)
	}

	dstMac, err := net.ParseMAC(dstMacStr)
	if err != nil {
		fmt.Println("Error parsing destination MAC address:", err)
		os.Exit(1)
	}

	srcIP := net.ParseIP(srcIPStr)
	if srcIP == nil {
		fmt.Println("Error parsing source IP address:", err)
		os.Exit(1)
	}

	dstIP := net.ParseIP(dstIPStr)
	if dstIP == nil {
		fmt.Println("Error parsing destination IP address:", err)
		os.Exit(1)
	}

	srcPort, err := strconv.Atoi(srcPortStr)
	if err != nil {
		fmt.Println("Error parsing source port:", err)
		os.Exit(1)
	}

	dstPort, err := strconv.Atoi(dstPortStr)
	if err != nil {
		fmt.Println("Error parsing destination port:", err)
		os.Exit(1)
	}

	payload := []byte(payloadStr)

	ttl, err := strconv.Atoi(ttlStr)
	if err != nil {
		fmt.Println("Error parsing TTL:", err)
		os.Exit(1)
	}

	// Create Ethernet packet
	ethHeader := eth.Header{
		SrcAddr: srcMAC,
		DstAddr: dstMAC,
		Type:    eth.EthertypeIPv4,
	}
	ethPacket, err := packet.NewPacket(&ethHeader, nil)
	if err != nil {
		log.Fatal(err)
	}

	// Create IPv4 packet
	ipHeader := ip.Header{
		Version:  4,
		Len:      20,
		TOS:      0,
		ID:       0,
		Flags:    ip.DontFragment,
		TTL:      uint8(ttl),
		Protocol: ip.ProtocolTCP,
		SrcAddr:  srcIP,
		DstAddr:  dstIP,
	}
	ipPacket, err := packet.NewPacket(&ipHeader, ethPacket)
	if err != nil {
		log.Fatal(err)
	}

	// Create TCP packet
	tcpHeader := tcp.Header{
		SrcPort: tcp.Port(srcPort),
		DstPort: tcp.Port(dstPort),
		SeqNum:  tcp.SeqNum(0),
		AckNum:  tcp.AckNum(0),
		Flags:   tcp.FlagSyn,
		WinSize: tcp.WindowSize(4096),
	}
	tcpPacket, err := packet.NewPacket(&tcpHeader, ipPacket)
	if err != nil {
		log.Fatal(err)
	}

	// Add payload to TCP packet
	err = tcpPacket.SetPayload(payload)
	if err != nil {
		log.Fatal(err)
	}

	// Calculate TCP checksum
	tcpHeader.CalculateChecksum(ipPacket)

	// Send packet on wire
	handle, err := pcap.OpenLive("eth0", 65535, true, 0)
	if err != nil {
		log.Fatal(err)
	}
	defer handle.Close()

	err = handle.SendPacket(tcpPacket.Bytes())
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("Packet sent!")
}
