package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net"
	"strconv"
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
		SrcMacStr  string `json:"srcMacStr"`
		DstMacStr  string `json:"dstMacStr"`
		SrcIPStr   string `json:"srcIPStr"`
		DstIPStr   string `json:"dstIPStr"`
		SrcPortStr string `json:"srcPortStr"`
		DstPortStr string `json:"dstPortStr"`
		TTLStr     string `json:"ttlStr"`
		SeqNumStr  string `json:"seqNumStr"`
		AckNumStr  string `json:"ackNumStr"`
		FlagsStr   string `json:"flagsStr"`
		WinSizeStr string `json:"winSizeStr"`
		Payload    []byte `json:"payload"`
	}
	err := decoder.Decode(&packetInfo)
	if err != nil {
		log.Println(err)
		return
	}
	log.Printf("Received packet info: %+v", packetInfo)

	// Parse the packet information from the JSON message
	srcMac, err := net.ParseMAC(packetInfo.SrcMacStr)
	if err != nil {
		log.Println(err)
		return
	}
	dstMac, err := net.ParseMAC(packetInfo.DstMacStr)
	if err != nil {
		log.Println(err)
		return
	}
	srcIP := net.ParseIP(packetInfo.SrcIPStr)
	if srcIP == nil {
		log.Printf("Invalid source IP address: %s", packetInfo.SrcIPStr)
		return
	}
	dstIP := net.ParseIP(packetInfo.DstIPStr)
	if dstIP == nil {
		log.Printf("Invalid destination IP address: %s", packetInfo.DstIPStr)
		return
	}
	srcPort, err := strconv.Atoi(packetInfo.SrcPortStr)
	if err != nil {
		log.Println(err)
		return
	}
	dstPort, err := strconv.Atoi(packetInfo.DstPortStr)
	if err != nil {
		log.Println(err)
		return
	}
	ttl, err := strconv.Atoi(packetInfo.TTLStr)
	if err != nil {
		log.Println(err)
		return
	}
	seqNum, err := strconv.Atoi(packetInfo.SeqNumStr)
	if err != nil {
		log.Println(err)
		return
	}
	ackNum, err := strconv.Atoi(packetInfo.AckNumStr)
	if err != nil {
		log.Println(err)
		return
	}
	flags, err := strconv.Atoi(packetInfo.FlagsStr)
	if err != nil {
		log.Println(err)
		return
	}
	winSize, err := strconv.Atoi(packetInfo.WinSizeStr)
	if err != nil {
		log.Println(err)
		return
	}

	// Create a TCP packet with the parsed information
	ipLayer := &layers.IPv4{
		SrcIP:    srcIP,
		DstIP:    dstIP,
		Version:  4,
		TTL:      uint8(ttl),
		Protocol: layers.IPProtocolTCP,
	}
	tcpLayer := &layers.TCP{
		SrcPort: layers.TCPPort(srcPort),
		DstPort: layers.TCPPort(dstPort),
		Seq:     uint32(seqNum),
		Ack:     uint32(ackNum),
		Window:  uint16(winSize),
		ACK:     flags&0x10 != 0,
		SYN:     flags&0x02 != 0,
		FIN:     flags&0x01 != 0,
		RST:     flags&0x04 != 0,
	}
	tcpLayer.SetNetworkLayerForChecksum(ipLayer)
	payload := gopacket.Payload(packetInfo.Payload)
	packet := gopacket.NewSerializeBuffer()
	err = gopacket.SerializeLayers(packet, gopacket.SerializeOptions{},
		ipLayer, tcpLayer, payload)
	if err != nil {
		log.Println(err)
		return
	}

	// Send the packet
	log.Println("Sending packet")
	conn, err = net.Dial("ip4:tcp", dstIP.String())
	if err != nil {
		log.Println(err)
		return
	}
	defer conn.Close()
	_, err = conn.Write(packet.Bytes())
	if err != nil {
		log.Println(err)
		return
	}
	log.Println("Packet sent")
}
