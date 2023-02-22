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
		SrcIP   net.IP           `json:"srcIP"`
		DstIP   net.IP           `json:"dstIP"`
		SrcPort uint16           `json:"srcPort"`
		DstPort uint16           `json:"dstPort"`
		SrcMAC  net.HardwareAddr `json:"srcMAC"`
		DstMAC  net.HardwareAddr `json:"dstMAC"`
		Payload []byte           `json:"payload"`
	}
	err := decoder.Decode(&packetInfo)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Received packet info: %+v", packetInfo)

	// Construct the UDP packet using gopacket
	ethLayer := &layers.Ethernet{
		SrcMAC:       packetInfo.SrcMAC,
		DstMAC:       packetInfo.DstMAC,
		EthernetType: layers.EthernetTypeIPv4,
	}
	ipLayer := &layers.IPv4{
		Version:  4,
		Protocol: layers.IPProtocolUDP,
		SrcIP:    packetInfo.SrcIP,
		DstIP:    packetInfo.DstIP,
	}
	udpLayer := &layers.UDP{
		SrcPort: layers.UDPPort(packetInfo.SrcPort),
		DstPort: layers.UDPPort(packetInfo.DstPort),
	}
	if len(packetInfo.Payload) > 0 {
		udpLayer.SetNetworkLayerForChecksum(ipLayer)
		udpLayer.Payload = packetInfo.Payload
	}

	buffer := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		FixLengths:       true,
		ComputeChecksums: true,
	}

	err = gopacket.SerializeLayers(buffer, opts, ethLayer, ipLayer, udpLayer)
	if err != nil {
		log.Println(err)
		return
	}

	// Send the UDP packet
	packetData := buffer.Bytes()
	conn.SetWriteDeadline(time.Now().Add(5 * time.Second)) // Set a timeout for the write
	_, err = conn.Write(packetData)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Sent UDP packet: % X", packetData)
}
