# Basic Concepts
1. Socket: `net.node.port`
2. Packet: Five layers 
   ```
   layer 1 (physical layer)
   layer 2 (link layer) (Ethernet/Frame Header): 14 B
   layer 3 (internet layer)   (IP Header): 20 B
   layer 4 (transport layer)  (TCP/UDP/ICMP Header) xx/8/8 B
   layer 5 (data/payload layer)
   ```
   ![alt text]()
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
   TCP header:
   UDP header:
   ICMP header:
6. Linux structure
   Kernel -> Module -> Daemon -> Applications
   ![alt text](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/Linux_structure.PNG)
7. 