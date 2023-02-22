package main

import (
	"fmt"
	"net"

	"github.com/google/gopacket/ipv4"
	"github.com/google/gopacket/packet"
	"github.com/google/gopacket/pcap"
	"github.com/google/gopacket/udp"
)

func main() {
	// Ask the user for the source and destination IP addresses
	fmt.Print("Source IP address: ")
	srcIP := net.ParseIP(readString())

	fmt.Print("Destination IP address: ")
	dstIP := net.ParseIP(readString())

	// Ask the user for the source and destination port numbers
	fmt.Print("Source port: ")
	srcPort := uint16(readInt())

	fmt.Print("Destination port: ")
	dstPort := uint16(readInt())

	// Ask the user for the source and destination MAC addresses
	fmt.Print("Source MAC address: ")
	srcMAC, err := net.ParseMAC(readString())
	if err != nil {
		panic(err)
	}

	fmt.Print("Destination MAC address: ")
	dstMAC, err := net.ParseMAC(readString())
	if err != nil {
		panic(err)
	}

	// Ask the user for the payload
	fmt.Print("Payload: ")
	payload := readString()

	// Open a handle to the network device
	handle, err := pcap.OpenLive("eth0", 65535, true, pcap.BlockForever)
	if err != nil {
		panic(err)
	}
	defer handle.Close()

	// Create an IPv4 packet
	ipPkt := ipv4.Make()
	ipPkt.SetVersion(4)
	ipPkt.SetHeaderLength(5)
	ipPkt.SetLength(20 + 8 + len(payload))
	ipPkt.SetProtocol(packet.IPProtocolUDP)
	ipPkt.SetSourceAddress(srcIP)
	ipPkt.SetDestinationAddress(dstIP)

	// Create a UDP packet
	udpPkt := udp.Make()
	udpPkt.SetSourcePort(srcPort)
	udpPkt.SetDestinationPort(dstPort)
	udpPkt.SetLength(8 + len(payload))

	// Set the payload of the UDP packet
	udpPkt.SetPayload([]byte(payload))

	// Calculate the UDP checksum
	udpPkt.CalculateChecksum(ipPkt)

	// Serialize the UDP packet and add it as payload of the IPv4 packet
	ipPkt.SetPayload(udpPkt.Marshal())

	// Set the source and destination MAC addresses of the Ethernet frame
	ethPkt := packet.MakeEthernetII()
	ethPkt.SetSourceAddress(srcMAC)
	ethPkt.SetDestinationAddress(dstMAC)
	ethPkt.SetPayload(ipPkt.Marshal())

	// Write the Ethernet frame to the network
	if err := handle.WritePacketData(ethPkt.Marshal()); err != nil {
		panic(err)
	}
}

func readString() string {
	var s string
	fmt.Scanln(&s)
	return s
}

func readInt() int {
	var i int
	fmt.Scanln(&i)
	return i
}
