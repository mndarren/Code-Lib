# Basic Concepts
=================
1. Socket: `net.node.port`
2. Packet: Five layers 
   ```
   layer 1 (physical layer)
   layer 2 (link layer) (Ethernet/Frame Header): 14 B
   layer 3 (network layer)   (IP Header): 20 B
   layer 4 (transport layer)  (TCP/UDP/ICMP Header) xx/8/8 B
   layer 5 (data/payload layer)
   ```
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/packet_layers.png)
3. IP 5 classes
   ```
   class A: 1-127 (N.H.H.H) (127.0.0.0/8 for loopback)
   class B: 128-191 (N.N.H.H)
   class C: 192-223 (N.N.N.H)
   class D: 224-239 (Reserved for Multicasting)
   class E: 240-254 (Experimental/used for research)
   ```
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/IP_classes.PNG)
4. Private IP addresses
   ```
   class A: 10.0.0.0/8
   class B: 172.16.0.0/16 -> 172.31.0.0/16
   class C: 192.168.0.0/16
   ```
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/private_IP.PNG)
5. Headers structure  
   IP header:  
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/IP_header.PNG)
   TCP header:  
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/TCP_header.PNG)
   UDP header:  
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/UDP_header.PNG)
   ICMP header:  
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/ICMP_header.PNG)
6. Linux structure
   Kernel -> Module -> Daemon -> Applications  
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/Linux_structure.PNG)
7. /usr/sbin: Unix System Resources/System binary
8. TCP flags
   ```
   U -URG --Urgent       ---Unskilled
   A -ACK --Ackownlege   ---Attacker
   P -PSH --Push         ---Pester
   R -RST --Reset        ---Real
   S -SYN --Synchronize  ---Security
   F -FIN --Final        ---Folks
   ```
9. Important config files
   ```
   /etc/fstab     # accessible fs
   /etc/motd      # message of today
   /etc/hosts     # names to ip addresses
   /etc/networks  # network names to ip addresses
   /etc/protocols # contain all protocols
   /etc/services  # contain all services
   /etc/pid#/maps # view process in memory (rwsp, read|write|shared|protected)
   ```
10. Port types
   ```
   encrypted port (Ex: 22)
   redirected port (Ex: ncat -v -c 'echo this is a test' -l -p 1234 -t)
   status port (Ex: rpcinfo -p 10.18.59.34)
   rogue port (Ex: you don't know)
   ```
11. Digital Signature (used to confirm Data Integration)
   ```
   Type    |  Hex  |  bits
   sum        5       20
   cksum      10      40
   md5sum     32      128
   sha128sum  32      128
   sha512sum  124     512
   ```
12. Top 20 ports
   ```
   Echo        7         TCP/UDP
   FTP         20/21     TCP
   SFTP        115       TCP      --encrypted
   FTPS        989/990   TCP      --encrypted
   SSH/SCP     22        TCP      --encrypted
   Telnet      23        TCP
   SMTP        25        TCP      --used for sending email
   SMTPS       465       TCP      --encrypted
   POP3        110       TCP      --used for receiving email
   POP3S       995       TCP      --encrypted
   DNS         53        TCP
   DHCP        67/68     UDP
   HTTP        80        TCP
   HTTPS       443       TCP      --encrypted
   Kerberos    88        TCP/UDP  --464 for Kerberos v5
   sunrpc      111   
   SNMP        161/162
   LDAP        389   
   LDAPS       636       TCP/UDP  --encrypted
   NFS         2049
   ```   
13. Terms
   ```
   MAC  --Media Access Control
   LLC  --Logical Link Control
   DNS  --Domain Name System
   TCP  --Transission Control Protocol (6)
   UDP  --User Datagram Protocol (17)
   ICMP --Internet Control Message Protocol (1)
   ARP  --Address Resolution Protocol
   LDAP --Lightweight Directory Access Protocol
   DHCP --Dynamic Host Configuration Protocol
   HTTP --Hypertext Transfer Protocol
   SNMP --Simple Network Management Protocol
   CSMA/CD --Carrier Sensor Multiple Access/Collision Detection
   UUCP --Unix to Unix Copy Protocol
   ```
14. 7 file types
   ```
   -  --file
   d  --directory
   p  --pipe
   c  --character
   l  --link
   b  --block
   s  --socket
   ```
15. TTL default length
   ```
   64   --Linux default
   128  --Windows default
   255  --Cisco default
   ```
16. Diff between Reject connection and Drop connection   
   1) Reject: close server -> client tried to connect server -> kernel reply refused for safety
   2) Drop: 
17. **English Domain** --(DNS)--> **Number IP address** --(ARP)--> **Data Link(MAC)**
18. 