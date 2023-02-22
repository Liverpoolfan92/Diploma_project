package main

import (
	"encoding/json"
	"log"
	"net"
	"time"

	"github.com/google/gopacket"
	"github.com/google/gopacket/layers"
)

func main() {
	// Listen on port 8484 for incoming connections
	addr, err := net.ResolveTCPAddr("tcp", "0.0.0.0:8484")
	if err != nil {
		log.Fatal(err)
	}
	listener, err := net.ListenTCP("tcp", addr)
	if err != nil {
		log.Fatal(err)
	}
	log.Printf("Listening on %s", addr)

	// Loop forever, handling incoming connections
	for {
		conn, err := listener.Accept()
		if err != nil {
			log.Println(err)
			continue
		}
		go handleConnection(conn)
	}
}

func handleConnection(conn net.Conn) {
	defer conn.Close()

	// Read the incoming JSON message from the connection
	decoder := json.NewDecoder(conn)
	var packetInfo struct {
		ICMPType uint8  `json:"icmpType"`
		ICMPCode uint8  `json:"icmpCode"`
		SrcIP    net.IP `json:"srcIP"`
		DstIP    net.IP `json:"dstIP"`
	}
	err := decoder.Decode(&packetInfo)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Received packet info: %+v", packetInfo)

	// Construct the ICMPv4 packet using go.pkt
	ethLayer := &layers.Ethernet{
		SrcMAC:       net.HardwareAddr{0x00, 0x11, 0x22, 0x33, 0x44, 0x55},
		DstMAC:       net.HardwareAddr{0x00, 0x11, 0x22, 0x33, 0x44, 0x66},
		EthernetType: layers.EthernetTypeIPv4,
	}
	ipLayer := &layers.IPv4{
		Version:  4,
		Protocol: layers.IPProtocolICMPv4,
		SrcIP:    packetInfo.SrcIP,
		DstIP:    packetInfo.DstIP,
		TTL:      64,
	}
	icmpLayer := &layers.ICMPv4{
		TypeCode: layers.CreateICMPv4TypeCode(packetInfo.ICMPType, packetInfo.ICMPCode),
	}
	icmpLayer.SetNetworkLayerForChecksum(ipLayer)

	buffer := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		FixLengths:       true,
		ComputeChecksums: true,
	}

	err = gopacket.SerializeLayers(buffer, opts, ethLayer, ipLayer, icmpLayer)
	if err != nil {
		log.Println(err)
		return
	}

	// Send the ICMPv4 packet
	packetData := buffer.Bytes()
	conn.SetWriteDeadline(time.Now().Add(5 * time.Second)) // Set a timeout for the write
	_, err = conn.Write(packetData)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Sent ICMPv4 packet: % X", packetData)
}
