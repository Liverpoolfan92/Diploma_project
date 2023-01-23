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

	fmt.Println("Select a network interface:")
	fmt.Println("1. Wired")
	fmt.Println("2. Wireless")

	var interfaceNum int
	fmt.Scan(&interfaceNum)

	var interfaceName string

	if interfaceNum == 1 {
		// wired interface
		for _, i := range interfaces {
			if i.Flags&net.FlagUp != 0 &&
				i.Flags&net.FlagLoopback == 0 &&
				!strings.HasPrefix(i.Name, "docker") &&
				!strings.HasPrefix(i.Name, "wlan") {
				interfaceName = i.Name
				break
			}
		}
	} else if interfaceNum == 2 {
		// wireless interface
		for _, i := range interfaces {
			if i.Flags&net.FlagUp != 0 &&
				i.Flags&net.FlagLoopback == 0 &&
				!strings.HasPrefix(i.Name, "docker") &&
				strings.HasPrefix(i.Name, "wlan") {
				interfaceName = i.Name
				break
			}
		}
	} else {
		fmt.Println("Invalid selection. Only 1 or 2 is allowed.")
		return
	}

	if interfaceName == "" {
		fmt.Println("No such interface found.")
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
