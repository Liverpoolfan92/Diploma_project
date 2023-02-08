package main

import (
	"encoding/json"
	"fmt"
	"net"
	"os/exec"
	"strings"
)

// Struct to hold all interfaces names
type Interfaces struct {
	Interfaces []string `json:"interfaces"`
}

// Struct to hold the chosen interface name
type chosen struct {
	Interface string `json:"interface"`
}

func main() {
	// Get a list of all network interfaces
	interfaces, err := net.Interfaces()
	if err != nil {
		fmt.Printf("Error getting network interfaces: %s\n", err.Error())
		return
	}

	var interfaceNames []string
	for _, i := range interfaces {
		interfaceNames = append(interfaceNames, i.Name)
	}

	// Send the interface names to port 8484
	conn, err := net.Dial("tcp", " 172.19.144.1:8484")
	if err != nil {
		fmt.Printf("Error connecting to port 8484: %s\n", err.Error())
		return
	}
	defer conn.Close()

	interfacesData := Interfaces{Interfaces: interfaceNames}
	data, err := json.Marshal(interfacesData)
	if err != nil {
		fmt.Printf("Error marshalling interface data: %s\n", err.Error())
		return
	}

	_, err = conn.Write(data)
	if err != nil {
		fmt.Printf("Error sending data: %s\n", err.Error())
		return
	}

	// Start a listener on port 8485
	listener, err := net.Listen("tcp", ":8485")
	if err != nil {
		fmt.Printf("Error starting listener: %s\n", err.Error())
		return
	}
	defer listener.Close()
	fmt.Println("Listening on port 8485...")

	for {
		// Wait for an incoming connection
		conn, err := listener.Accept()
		if err != nil {
			fmt.Printf("Error accepting connection: %s\n", err.Error())
			continue
		}
		defer conn.Close()

		// Read JSON data from the incoming connection
		buffer := make([]byte, 1024)
		n, err := conn.Read(buffer)
		if err != nil {
			fmt.Printf("Error reading from connection: %s\n", err.Error())
			continue
		}

		// Unmarshal the json data into the chosen struct
		var chosenInterface chosen
		err = json.Unmarshal(buffer[:n], &chosenInterface)
		if err != nil {
			fmt.Printf("Error unmarshalling json data: %s\n", err.Error())
			continue
		}

		// Print the chosen interface name
		fmt.Printf("Chosen interface: %s\n", chosenInterface.Interface)

		// ping 8.8.8.8 using the chosen interface
		ping("8.8.8.8", chosenInterface.Interface)
	}
}

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
