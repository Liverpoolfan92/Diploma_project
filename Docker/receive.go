package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net"

	"github.com/Liverpoolfan92/Diploma_project/icmpv4"
	"github.com/Liverpoolfan92/Diploma_project/ip"
	"github.com/Liverpoolfan92/Diploma_project/tcp"
	"github.com/Liverpoolfan92/Diploma_project/udp"
)

type Response struct {
	Type string `json:"type"`
	Data string `json:"data"`
}

func main() {
	// Listen on port 8484 for incoming connections
	listener, err := net.Listen("tcp", ":8484")
	if err != nil {
		log.Fatal(err)
	}
	defer listener.Close()

	// Wait for a client to connect
	conn, err := listener.Accept()
	if err != nil {
		log.Fatal(err)
	}

	// Parse the JSON data sent by the client
	var response Response
	err = json.NewDecoder(conn).Decode(&response)
	if err != nil {
		log.Fatal(err)
	}

	switch response.Type {
	case "tcp":
		var packet tcp.PacketTCP
		err = json.Unmarshal([]byte(response.Data), &packet)
		if err != nil {
			log.Fatal(err)
		}
		tcp.Handle_tcp(packet)
		fmt.Println(packet)
		break
	case "udp":
		var packet udp.PacketUDP
		err = json.Unmarshal([]byte(response.Data), &packet)
		if err != nil {
			log.Fatal(err)
		}
		udp.Handle_udp(packet)
		fmt.Println(packet)
		break
	case "ip":
		var packet ip.PacketIP
		err = json.Unmarshal([]byte(response.Data), &packet)
		if err != nil {
			log.Fatal(err)
		}
		ip.Handle_ip(packet)
		fmt.Println(packet)
		break
	case "icmp":
		var packet icmpv4.PacketICMPv4
		err = json.Unmarshal([]byte(response.Data), &packet)
		if err != nil {
			log.Fatal(err)
		}
		icmpv4.Handle_icmp(packet)
		fmt.Println(packet)
		break
	}

	fmt.Println(response.Type)
}
