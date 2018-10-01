# How install Kafka in CentOS
==========================================
1. Install packages: https://www.digitalocean.com/community/tutorials/how-to-install-apache-kafka-on-centos-7
```
1) create user kafka
    sudo useradd kafka -m
    sudo passwd kafka
    sudo usermod -aG wheel kafka
    su -l kafka
2) download Kafka binaries
    mkdir ~/Downloads
    curl "http://www-eu.apache.org/dist/kafka/1.1.0/kafka_2.12-1.1.0.tgz" -o ~/Downloads/kafka.tgz
    mkdir ~/kafka && cd ~/kafka
    tar -xvzf ~/Downloads/kafka.tgz --strip 1  # strip 1 to extract itself ~/kafka/, not ~/kafka/kafka_2.12-1.1.0/
3) Configure Kafka server
    sudo nano ~/kafka/config/server.properties
    Add the line to allow us to delete topic
    delete.topic.enable = true
4) Create systemd Unit files
    sudo nano /etc/systemd/system/zookeeper.service
    # Add the following in the file
    [Unit]
    Requires=network.target remote-fs.target
    After=network.target remote-fs.target

    [Service]
    Type=simple
    User=kafka
    ExecStart=/home/kafka/kafka/bin/zookeeper-server-start.sh /home/kafka/kafka/config/zookeeper.properties
    ExecStop=/home/kafka/kafka/bin/zookeeper-server-stop.sh
    Restart=on-abnormal

    [Install]
    WantedBy=multi-user.target

    sudo nano /etc/systemd/system/kafka.service
    # Add the following lines in the file
    [Unit]
    Requires=zookeeper.service
    After=zookeeper.service

    [Service]
    Type=simple
    User=kafka
    ExecStart=/bin/sh -c '/home/kafka/kafka/bin/kafka-server-start.sh /home/kafka/kafka/config/server.properties > /home/kafka/kafka/kafka.log 2>&1'
    ExecStop=/home/kafka/kafka/bin/kafka-server-stop.sh
    Restart=on-abnormal

    [Install]
    WantedBy=multi-user.target
5) Start Kafka
    sudo systemctl start kafka
    journalctl -u kafka              # check logs
    sudo systemctl enable kafka      # enable Kafka every reboot
```
2. Open ports on firewall  # Error no route to host centos
```
firewall-cmd --zone=public --list-all
firewall-cmd --permanent --zone=public --add-port=9092/tcp
firewall-cmd --reload
firewall-cmd --zone=public --list-all
```
3. Set up listeners  # Error connection refused
```
# in ~/kafka/kafka/config/server.properties. default value of <my_ip> is empty
listeners=PLAINTEXT://<my_ip>:9092
sudo systemctl restart kafka
```
