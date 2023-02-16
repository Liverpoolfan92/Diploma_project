package main

import (
	"fmt"
	"os/exec"
)

func main() {
	cmd := exec.Command("ping", "-c", "4", "8.8.8.8") // pinging 8.8.8.8 with 4 packets
	output, err := cmd.CombinedOutput()
	if err != nil {
		fmt.Println(err)
	}
	fmt.Println(string(output))
}
