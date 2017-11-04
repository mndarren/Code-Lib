# Install Java 8 or 9 on Linux Debian
=========================================
```
1. Install Default JRE/JDK
   $sudo apt-get update
   $sudo apt-get install default-jre
   $sudo apt-get install default-jdk
2. Install Oracle JDK
   $sudo apt-get install software-properties-common
   $sudo add-apt-repository "deb http://ppa.launchpad.net/webupd8team/java/ubuntu xenial main"
   $sudo apt-get update
3. Install JDK 8 or 9
   $sudo apt-get install oracle-java8-installer
   $sudo apt-get install oracle-java9-installer
   $javac -version
4. Manage Java version
   $sudo update-alternatives --config java
   $sudo nano /etc/environment
        JAVA_HOME="/usr/lib/jvm/java-8-oracle"
   $source /etc/environment
   $echo $JAVA_HOME
```