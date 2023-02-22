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
		SrcMAC  net.HardwareAddr `json:"srcMAC"`
		DstMAC  net.HardwareAddr `json:"dstMAC"`
		SrcIP   net.IP           `json:"srcIP"`
		DstIP   net.IP           `json:"dstIP"`
		SrcPort uint16           `json:"srcPort"`
		DstPort uint16           `json:"dstPort"`
		Payload []byte           `json:"payload"`
		TTL     uint8            `json:"ttl"`
	}
	err := decoder.Decode(&packetInfo)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Received packet info: %+v", packetInfo)

	// Construct the IP packet using gopacket
	ethLayer := &layers.Ethernet{
		SrcMAC:       packetInfo.SrcMAC,
		DstMAC:       packetInfo.DstMAC,
		EthernetType: layers.EthernetTypeIPv4,
	}
	ipLayer := &layers.IPv4{
		Version:  4,
		TTL:      packetInfo.TTL,
		Protocol: layers.IPProtocolTCP,
		SrcIP:    packetInfo.SrcIP,
		DstIP:    packetInfo.DstIP,
	}
	tcpLayer := &layers.TCP{
		SrcPort: layers.TCPPort(packetInfo.SrcPort),
		DstPort: layers.TCPPort(packetInfo.DstPort),
	}
	if len(packetInfo.Payload) > 0 {
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		tcpLayer.Payload = packetInfo.Payload
	}

	buffer := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		FixLengths:       true,
		ComputeChecksums: true,
	}

	err = gopacket.SerializeLayers(buffer, opts, ethLayer, ipLayer, tcpLayer)
	if err != nil {
		log.Println(err)
		return
	}

	// Send the IP packet
	packetData := buffer.Bytes()
	conn.SetWriteDeadline(time.Now().Add(5 * time.Second)) // Set a timeout for the write
	_, err = conn.Write(packetData)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Sent IP packet: % X", packetData)
}
