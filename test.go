package main

import (
	"fmt"
	"net"
	"os/exec"
	"strings"
)

func ping(host string, interfaceName string) {
	// run the 'ping' command and capture the output
	out, err := exec.Command("ping", "-I", interfaceName, "-c", "5", host).Output()
	if err != nil {
		fmt.Printf("Error: %s\n", err.Error())
		return
	}

	// parse the output
	output := strings.TrimSpace(string(out))
	fmt.Printf("%s\n", output)
}

func arp(host string, interfaceName string) {
	// run the 'arp' command and capture the output
	out, err := exec.Command("arp", "-i", interfaceName, "-n", host).Output()
	if err != nil {
		fmt.Printf("Error: %s\n", err.Error())
		return
	}

	// parse the output
	output := strings.TrimSpace(string(out))
	fmt.Printf("%s\n", output)
}

func main() {

	interfaces, _ := net.Interfaces()
	var interfaceNames []string
	for _, i := range interfaces {
		interfaceNames = append(interfaceNames, i.Name)
	}
	var in string
	var inter string
	fmt.Println("Choose an interface:", interfaceNames)
	fmt.Scanf("%s", &in)
	switch in {
	case "lo":
		inter = interfaceNames[0]
	case "eth0":
		inter = interfaceNames[1]
	case "eth1":
		inter = interfaceNames[2]
	case "docker0":
		inter = interfaceNames[3]
	}
	var input1 string
	var input2 string
	fmt.Println("Enter ping or arp + ip address")
	fmt.Scanf("%s", &input1)
	fmt.Scanf("%s", &input2)

	switch input1 {
	case "ping":
		ping(input2, inter)
	case "arp":
		arp(input2, inter)
	default:
		fmt.Println("Invalid input.")
	}
}
