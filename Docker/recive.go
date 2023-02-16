package main

import (
	"encoding/json"
	"fmt"
	"net"
)

// Struct to hold incoming interface names
type Interfaces struct {
	Interfaces []string `json:"interfaces"`
}

// Struct to hold the chosen interface name
type chosen struct {
	Interface string `json:"interface"`
}

func main() {
	// Start a listener on port 8484
	listener, err := net.Listen("tcp", value+":8484")
	if err != nil {
		fmt.Printf("Error starting listener: %s\n", err.Error())
		return
	}
	defer listener.Close()
	fmt.Println("Listening on port 8484...")

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

		// Unmarshal the json data into the Interfaces struct
		var incomingInterfaces Interfaces
		err = json.Unmarshal(buffer[:n], &incomingInterfaces)
		if err != nil {
			fmt.Printf("Error unmarshalling json data: %s\n", err.Error())
			continue
		}

		for _, i := range incomingInterfaces.Interfaces {
			fmt.Println(i)
		}

		var interfacenum int
		fmt.Println("Choose from 0 to 3\n")
		fmt.Scan(&interfacenum)
		// Choose an interface from the received array
		chosenInterface := incomingInterfaces.Interfaces[interfacenum]

		// Connect to port 8485
		conn, err = net.Dial("tcp", value+":8485")
		if err != nil {
			fmt.Printf("Error connecting to port 8485: %s\n", err.Error())
			continue
		}
		defer conn.Close()

		// Send the chosen interface name to port 8485 as a JSON string
		chosenData := chosen{Interface: chosenInterface}
		data, err := json.Marshal(chosenData)
		if err != nil {
			fmt.Printf("Error marshalling chosen data: %s\n", err.Error())
			continue
		}

		_, err = conn.Write(data)
		if err != nil {
			fmt.Printf("Error sending data: %s\n", err.Error())
			continue
		}
	}
}
