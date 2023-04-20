FROM ubuntu

RUN apt-get update && apt-get install -y git libpcap-dev golang gcc g++

RUN git clone https://github.com/Liverpoolfan92/Diploma_project.git

RUN git clone https://github.com/google/gopacket.git 

WORKDIR /Diploma_project/Docker/

ENTRYPOINT ["go", "run", "receive.go"]