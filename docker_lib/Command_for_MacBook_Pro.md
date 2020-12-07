# Command for MacBook Pro
===============================
## Check Mac Product Version
```
$ sw_vers -productVersion
```
## Where is Info.plist for java
```
/Library/Java/JavaVirtualMachines/jdk-11.0.9.jdk/Contents 
```  
## Where is java
```
/Library/Java/JavaVirtualMachines/jdk-11.0.9.jdk/Contents/Home/bin/java 
```  
## Where is Java VM Framework
```
/system/library/frameworks/javaVM.framework/Versions
```
## eclipse.ini
```
Eclipse.app -> Right click -> Show Package Content -> MacOS
```
## Docker install sonar   
```
docker pull docker.elastic.co/elasticsearch/elasticsearch:7.10.0
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.10.0
docker pull sonarqube
docker run -d --name sonarqube -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true -p 9000:9000 sonarqube:latest

# Check containers process status
docker ps -a
docker parse <container id>

# Remove image
docker image ls
docker rmi <image id>

# Stop & remove container first
docker stop <container id>
docker rm <container id>

# Run Sonar Scanning
./gradlew sonarqube \
  -Dsonar.projectKey=algorithm_k \
  -Dsonar.host.url=http://localhost:9000 \
  -Dsonar.login=7132e2e2106b79130162645a16c8fede38000326

--Install Elasticsearch with Docker
https://www.elastic.co/guide/en/elasticsearch/reference/current/docker.html#docker
https://docs.sonarqube.org/latest/setup/get-started-2-minutes/
--sanar image
https://hub.docker.com/_/sonarqube/
```
