package main

import (
	"encoding/json"
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
	// Get the network interface
	iface, err := net.InterfaceByName("eth0")
	if err != nil {
		panic(err)
	}

	// Get the interface's hardware address
	srcMac := iface.HardwareAddr

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
		var packetData PacketData
		err = json.NewDecoder(conn).Decode(&packetData)
		if err != nil {
			panic(err)
		}

		// Create a new Ethernet packet
		ethPacket := gopacket.NewSerializeBuffer()
		ethLayer := &layers.Ethernet{
			SrcMAC:       srcMac,
			DstMAC:       net.HardwareAddr{0xff, 0xff, 0xff, 0xff, 0xff, 0xff}, // Broadcast address
			EthernetType: layers.EthernetTypeIPv4,
		}

		// Create a new IP packet
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

		// Serialize the layers into the Ethernet packet
		err = gopacket.SerializeLayers(ethPacket, gopacket.SerializeOptions{},
			ethLayer, ipLayer, tcpLayer, gopacket.Payload(payload))
		if err != nil {
			panic(err)
		}

		// Send the packet
		rawConn, err := net.ListenPacket("eth0", layers.EthernetTypeIPv4.String())
		if err != nil {
			panic(err)
		}
		defer rawConn.Close()

		_, err = rawConn.WriteTo(ethPacket.Bytes(), &net.IPAddr{})
		if err != nil {
			panic(err)
		}
	}
}
