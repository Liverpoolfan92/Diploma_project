package main

import (
	"encoding/json"
	"fmt"
	"net"
	"time"

	"github.com/google/gopacket"
	"github.com/google/gopacket/layers"
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
	// Connect to the TCP server running on the localhost
	conn, err := net.Dial("tcp", "localhost:8484")
	if err != nil {
		panic(err)
	}
	defer conn.Close()

	// Read the JSON data from the TCP stream
	var packetData PacketData
	err = json.NewDecoder(conn).Decode(&packetData)
	if err != nil {
		panic(err)
	}

	// Create a new IP packet
	ipPacket := gopacket.NewSerializeBuffer()
	ipLayer := &layers.IPv4{
		SrcIP:    net.ParseIP(packetData.SrcIp),
		DstIP:    net.ParseIP(packetData.DstIp),
		Version:  4,
		Length:   20,
		TTL:      uint8(packetData.Ttl),
		Protocol: layers.IPProtocolTCP,
	}
	// Add a TCP layer
	tcpLayer := &layers.TCP{
		SrcPort: layers.TCPPort(packetData.SrcPort),
		DstPort: layers.TCPPort(packetData.DstPort),
		SYN:     true,
		Window:  14600,
	}
	// Set the TCP payload
	payload := []byte(packetData.Payload)
	tcpLayer.SetNetworkLayerForChecksum(ipLayer)
	err = gopacket.SerializeLayers(ipPacket, gopacket.SerializeOptions{},
		ipLayer, tcpLayer, gopacket.Payload(payload))
	if err != nil {
		panic(err)
	}

	// Write the packet to the console
	fmt.Printf("%v\n", ipPacket)

	// Wait for a second
	time.Sleep(time.Second)
}
