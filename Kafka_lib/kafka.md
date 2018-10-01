# How install Kafka in CentOS
==========================================
1. Install packages: https://www.digitalocean.com/community/tutorials/how-to-install-apache-kafka-on-centos-7
2. Open ports on firewall  # no route to host centos
```
firewall-cmd --zone=public --list-all
firewall-cmd --permanent --zone=public --add-port=9092/tcp
firewall-cmd --reload
firewall-cmd --zone=public --list-all
```
