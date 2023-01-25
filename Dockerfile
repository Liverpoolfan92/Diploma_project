FROM ubuntu

RUN apt update -yy && \
    apt install git -yy && \
    apt install libpcap-dev -yy && \
    apt install golang -yy && \
    apt-get install gcc && apt-get install g++ && \
    git clone https://github.com/Liverpoolfan92/Diploma_project.git && \
    git clone https://github.com/ghedo/go.pkt.git && \
    cd go.pkt && \
    go run examples/ping/main.go 8.8.8.8
