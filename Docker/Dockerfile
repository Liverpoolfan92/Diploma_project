FROM ubuntu

RUN apt-get update -yy 

RUN apt-get install git -yy 

RUN apt-get install libpcap-dev -yy 
    
RUN apt-get install golang -yy 
RUN apt-get install gcc && apt-get install g++
RUN git clone https://github.com/Liverpoolfan92/Diploma_project.git
RUN git clone https://github.com/google/gopacket.git 
RUN cd Diploma_project/Docker/ 
RUN go run receive.go