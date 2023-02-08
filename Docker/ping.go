package main

import (
	"fmt"
	"net"

	"github.com/ghedo/go.pkt/packet"
)

func pingWithGoPkt(host string, interfaceName string) {
	// Open a handle to the network interface
	handle, err := pcap.OpenLive(interfaceName, 65536, true, pcap.BlockForever)
	if err != nil {
		fmt.Printf("Error opening handle: %s\n", err)
		return
	}
	defer handle.Close()

	// Create a new ICMP packet
	icmp := packet.NewICMP()
	icmp.Type = packet.ICMPEchoRequest
	icmp.Code = 0
	icmp.icmp.Checksum = 0
	icmp.Identifier = 0
	icmp.SequenceNumber = 0
	icmp.SetPayload([]byte("Hello, World!"))

	// Create a new IP packet
	ip := packet.NewIP()
	ip.Version = 4
	ip.IHL = 5
	ip.TOS = 0
	ip.Length = 0
	ip.Id = 0
	ip.Flags = 0
	ip.FragmentOffset = 0
	ip.TTL = 64
	ip.Protocol = packet.IPProtocolICMP
	ip.Checksum = 0
	ip.SrcIP = net.IPv4zero
	ip.DstIP = net.ParseIP(host)
	ip.SetPayload(icmp)

	// Write the packet to the network interface
	if err := handle.WritePacketData(ip.Bytes()); err != nil {
		fmt.Printf("Error sending packet: %s\n", err)
		return
	}
}
