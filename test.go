package main

import (
	"fmt"
	"os/exec"
	"regexp"
	"strings"
)

func main() {
	fmt.Println("Select a network interface:")
	fmt.Println("1. Wired")
	fmt.Println("2. Wireless")

	var interfaceNum int
	fmt.Scan(&interfaceNum)

	var interfaceName string

	if interfaceNum == 1 {
		// wired interface
		output, _ := exec.Command("ip", "link").CombinedOutput()
		regex := regexp.MustCompile(`(?m)^[0-9]+: (.*?):.*?state UP`)
		matches := regex.FindAllStringSubmatch(string(output), -1)
		for _, match := range matches {
			if strings.Contains(match[1], "eth0") {
				interfaceName = match[1]
				break
			}
		}
	} else if interfaceNum == 2 {
		// wireless interface
		output, _ := exec.Command("ip", "link").CombinedOutput()
		regex := regexp.MustCompile(`(?m)^[0-9]+: (.*?):.*?state UP`)
		matches := regex.FindAllStringSubmatch(string(output), -1)
		for _, match := range matches {
			if strings.Contains(match[1], "wlan") {
				interfaceName = match[1]
				break
			}
		}
	} else {
		fmt.Println("Invalid selection. Only 1 or 2 is allowed.")
		return
	}

	if interfaceName == "" {
		fmt.Println("no such interface found")
		return
	}

	fmt.Print("Enter IP address to ping: ")

	var ipAddress string
	fmt.Scan(&ipAddress)

	pingCmd := exec.Command("ping", "-I", interfaceName, ipAddress)

	output, err := pingCmd.CombinedOutput()
	if err != nil {
		fmt.Printf("Command failed: %s\n", err)
		return
	}

	fmt.Println(strings.TrimSpace(string(output)))
}
