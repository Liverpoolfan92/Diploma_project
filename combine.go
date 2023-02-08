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

	fmt.Println("Select a network interface:")
	for _, i := range interfaces {
		fmt.Println(i.HardwareAddr)
	}

	var interfaceName string
	fmt.Print("Enter the name of the interface you want to use: ")
	fmt.Scan(&interfaceName)

	var selectedInterface *net.Interface

	for _, i := range interfaces {
		if i.Name == interfaceName {
			selectedInterface = &i
			break
		}
	}

	if selectedInterface == nil {
		fmt.Println("Invalid selection.")
		return
	}

	fmt.Print("Enter IP address to ping or arp: ")

	var ipAddress string
	fmt.Scan(&ipAddress)

	var input1 string
	fmt.Println("Enter ping or arp")
	fmt.Scanf("%s", &input1)

	switch input1 {
	case "ping":
		ping(ipAddress, interfaceName)
	case "arp":
		arp(ipAddress, interfaceName)
	default:
		fmt.Println("Invalid input.")
	}
}
