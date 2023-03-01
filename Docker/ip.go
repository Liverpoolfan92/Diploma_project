package main

import (
	"encoding/json"
	"fmt"
	"net"

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
	// Dial an Ethernet connection
	conn, err := net.Dial("unix", "eth0")
	if err != nil {
		panic(err)
	}
	defer conn.Close()

	for {
		// Wait for a client to connect
		listener, err := net.Listen("tcp", ":8484")
		if err != nil {
			panic(err)
		}
		defer listener.Close()

		clientConn, err := listener.Accept()
		if err != nil {
			panic(err)
		}

		// Parse the JSON data sent by the client
		var packetData PacketData
		err = json.NewDecoder(clientConn).Decode(&packetData)
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

		// Add an Ethernet layer
		ethLayer := &layers.Ethernet{
			SrcMAC:       net.HardwareAddr(packetData.SrcMac),
			DstMAC:       net.HardwareAddr(packetData.DstMac),
			EthernetType: layers.EthernetTypeIPv4,
		}

		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		err = gopacket.SerializeLayers(ipPacket, gopacket.SerializeOptions{},
			ethLayer, ipLayer, tcpLayer, gopacket.Payload(payload))
		if err != nil {
			panic(err)
		}

		// Send the packet
		_, err = conn.Write(ipPacket.Bytes())
		if err != nil {
			panic(err)
		}

		fmt.Println("Packet sent")

	}
}
