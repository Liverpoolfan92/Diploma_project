package main

import (
	"encoding/json"
	"fmt"
	"log"
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
	// Open a TCP client connection to the server on port 8484
	conn, err := net.Dial("tcp", "localhost:8484")
	if err != nil {
		log.Fatal(err)
	}
	defer conn.Close()

	// Receive the JSON data from the server
	decoder := json.NewDecoder(conn)
	var packetData PacketData
	err = decoder.Decode(&packetData)
	if err != nil {
		log.Fatal(err)
	}

	// Use the parsed data to construct an Ethernet frame with an IP layer and a TCP layer
	ethLayer := &layers.Ethernet{
		SrcMAC:       net.HardwareAddr{0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff},
		DstMAC:       net.HardwareAddr{0x00, 0x11, 0x22, 0x33, 0x44, 0x55},
		EthernetType: layers.EthernetTypeIPv4,
	}
	ipLayer := &layers.IPv4{
		Version:  4,
		TTL:      uint8(packetData.Ttl),
		Protocol: layers.IPProtocolTCP,
		SrcIP:    net.ParseIP(packetData.SrcIp),
		DstIP:    net.ParseIP(packetData.DstIp),
	}
	tcpLayer := &layers.TCP{
		SrcPort: layers.TCPPort(packetData.SrcPort),
		DstPort: layers.TCPPort(packetData.DstPort),
		SYN:     true,
	}

	// Set the payload of the TCP layer to the payload specified in the JSON data
	payload := []byte(packetData.Payload)
	tcpLayer.SetNetworkLayerForChecksum(ipLayer)
	tcpLayer.SetNetworkLayerForChecksum(ipLayer)
	tcpLayer.SetNetworkLayerForChecksum(ipLayer)
	tcpLayer.Payload = payload

	// Serialize the Ethernet frame with the IP and TCP layers
	buffer := gopacket.NewSerializeBuffer()
	options := gopacket.SerializeOptions{
		FixLengths:       true,
		ComputeChecksums: true,
	}
	err = gopacket.SerializeLayers(buffer, options, ethLayer, ipLayer, tcpLayer)
	if err != nil {
		log.Fatal(err)
	}

	// Send the constructed packet over the network
	err = conn.SetDeadline(time.Now().Add(5 * time.Second))
	if err != nil {
		log.Fatal(err)
	}
	_, err = conn.Write(buffer.Bytes())
	if err != nil {
		log.Fatal(err)
	}
	fmt.Println("Packet sent successfully!")
}
