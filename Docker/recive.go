package main

import (
	"encoding/json"
	"fmt"
	"github.com/Liverpoolfan92/go.pkt/eth"
	"github.com/Liverpoolfan92/go.pkt/ip"
	"net"
)

type PacketInfo struct {
	SrcMAC  string `json:"srcMAC"`
	DstMAC  string `json:"dstMAC"`
	SrcIP   string `json:"srcIP"`
	DstIP   string `json:"dstIP"`
	SrcPort int    `json:"srcPort"`
	DstPort int    `json:"dstPort"`
	Payload string `json:"payload"`
	TTL     int    `json:"ttl"`
}

func main() {
	fmt.Println("Listening on port 8484 for incoming JSON packets...")
	addr, err := net.ResolveTCPAddr("tcp", ":8484")
	if err != nil {
		fmt.Println("Error resolving address:", err)
		return
	}

	ln, err := net.ListenTCP("tcp", addr)
	if err != nil {
		fmt.Println("Error listening:", err)
		return
	}
	defer ln.Close()

	for {
		conn, err := ln.Accept()
		if err != nil {
			fmt.Println("Error accepting connection:", err)
			continue
		}
		go handleConnection(conn)
	}
}

func handleConnection(conn net.Conn) {
	// Read incoming JSON
	buf := make([]byte, 1024)
	n, err := conn.Read(buf)
	if err != nil {
		fmt.Println("Error reading:", err)
		return
	}

	var packet PacketInfo
	err = json.Unmarshal(buf[:n], &packet)
	if err != nil {
		fmt.Println("Error parsing JSON:", err)
		return
	}

	// Create IP packet
	srcMAC, err := net.ParseMAC(packet.SrcMAC)
	if err != nil {
		fmt.Println("Error parsing source MAC:", err)
		return
	}

	dstMAC, err := net.ParseMAC(packet.DstMAC)
	if err != nil {
		fmt.Println("Error parsing destination MAC:", err)
		return
	}

	srcIP := net.ParseIP(packet.SrcIP)
	if srcIP == nil {
		fmt.Println("Error parsing source IP:", err)
		return
	}

	dstIP := net.ParseIP(packet.DstIP)
	if dstIP == nil {
		fmt.Println("Error parsing destination IP:", err)
		return
	}

	ipPacket := ip.Packet{
		Version:  4,
		IHL:      5,
		TTL:      uint8(packet.TTL),
		Protocol: ip.TCP,
		SrcAddr:  srcIP,
		DstAddr:  dstIP,
	}

	// Create Ethernet frame
	ethFrame := eth.Frame{
		Src:  srcMAC,
		Dst:  dstMAC,
		Type: eth.IPv4,
		Data: ipPacket.Marshal(),
	}

	fmt.Println("Sending packet:", ethFrame)

	// Send packet
	rawConn, err := net.ListenPacket("raw", "eth0")
	if err != nil {
		fmt.Println("Error creating raw socket:", err)
		return
	}
	defer rawConn.Close()

	_, err = rawConn.WriteTo(ethFrame.Marshal(), &net.Interface{Index: 1})
	if err != nil {
		fmt.Println("Error sending packet:", err)
		return
	}

	fmt.Println("Packet sent successfully!")
}
