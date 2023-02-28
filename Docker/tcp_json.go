package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net"
	"strconv"
	"strings"

	"github.com/google/gopacket"
	"github.com/google/gopacket/layers"
)

type PacketInfo struct {
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
	Payload    string `json:"payload"`
}

func main() {
	// Listen on port 8484 for JSON packets
	ln, err := net.Listen("tcp", ":8484")
	if err != nil {
		log.Fatal(err)
	}
	defer ln.Close()

	// Loop forever, processing incoming packets
	for {
		conn, err := ln.Accept()
		if err != nil {
			log.Println(err)
			continue
		}
		go handleConnection(conn)
	}
}

func handleConnection(conn net.Conn) {
	// Read the incoming JSON packet
	decoder := json.NewDecoder(conn)
	var packetInfo PacketInfo
	err := decoder.Decode(&packetInfo)
	if err != nil {
		log.Println(err)
		return
	}

	// Parse the source and destination MAC addresses
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

	// Parse the source and destination IP addresses
	srcIP := net.ParseIP(packetInfo.SrcIPStr)
	if srcIP == nil {
		log.Println("Invalid source IP address:", packetInfo.SrcIPStr)
		return
	}
	dstIP := net.ParseIP(packetInfo.DstIPStr)
	if dstIP == nil {
		log.Println("Invalid destination IP address:", packetInfo.DstIPStr)
		return
	}

	// Parse the source and destination ports
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

	// Parse the TTL, sequence number, acknowledgement number, flags, and window size
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
	flagsStr := strings.ToLower(packetInfo.FlagsStr)
	flags := 0
	if strings.Contains(flagsStr, "ack") {
		flags |= 1 << 4
	}
	if strings.Contains(flagsStr, "psh") {
		flags |= 1 << 3
	}
	if strings.Contains(flagsStr, "rst") {
		flags |= 1 << 2
	}
	if strings.Contains(flagsStr, "syn") {
		flags |= 1 << 1
	}
	if strings.Contains(flagsStr, "fin") {
		flags |= 1
	}
	winSize, err := strconv.Atoi(packetInfo.WinSizeStr)
	if err != nil {
		log.Println(err)
		return
	}

	// Construct the Ethernet layer
	ethLayer := &layers.Ethernet{
		SrcMAC:       srcMac,
		DstMAC:       dstMac,
		EthernetType: layers.EthernetTypeIPv4,
	}

	// Construct the IP layer
	ipLayer := &layers.IPv4{
		Version:  4,
		TTL:      uint8(ttl),
		Protocol: layers.IPProtocolTCP,
		SrcIP:    srcIP,
		DstIP:    dstIP,
	}

	// Construct the TCP layer
	tcpLayer := &layers.TCP{
		SrcPort: layers.TCPPort(srcPort),
		DstPort: layers.TCPPort(dstPort),
		Seq:     uint32(seqNum),
		Ack:     uint32(ackNum),
		Window:  uint16(winSize),
		Flags:   layers.TCPFlags(flags),
	}

	// Set the TCP layer's payload
	if packetInfo.Payload != "" {
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		payload := []byte(packetInfo.Payload)
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		err = tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		if err != nil {
			log.Println(err)
			return
		}
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
		tcpLayer.SetNetworkLayerForChecksum(ipLayer)
	}

	// Construct the packet with all layers
	buffer := gopacket.NewSerializeBuffer()
	opts := gopacket.SerializeOptions{
		FixLengths:       true,
		ComputeChecksums: true,
	}
	err = gopacket.SerializeLayers(buffer, opts,
		ethLayer,
		ipLayer,
		tcpLayer,
		gopacket.Payload([]byte(packetInfo.Payload)),
	)
	if err != nil {
		log.Println(err)
		return
	}

	// Send the packet over the wire
	rawPacketData := buffer.Bytes()
	conn, err := net.Dial("raw", "eth0")
	if err != nil {
		log.Println(err)
		return
	}
	defer conn.Close()
	_, err = conn.Write(rawPacketData)
	if err != nil {
		log.Println(err)
		return
	}

	// Print a confirmation message
	fmt.Println("Sent packet:")
	fmt.Println(ethLayer)
	fmt.Println(ipLayer)
	fmt.Println(tcpLayer)
}
