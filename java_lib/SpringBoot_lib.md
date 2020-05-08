# Setup Spring Boot
=================================<br/>
1. Install maven and gradle in Windows
```
1) Run cmd line as administrator;
2) Install chololatey by run the command:
@"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin".
3) Install maven: choco install maven
4) Install gradle: choco install gradle
```
2. Install Spring Boot
```
0) Install Spring Boot CLI: manually download spring-boot-cli-2.3.0.BUILD-SNAPSHOT-bin.zip from https://docs.spring.io/spring-boot/docs/current-SNAPSHOT/reference/htmlsingle/#getting-started-manual-cli-installation, and then setup environment variables
1) Install zip: choco install zip
2) Install sdkman(in Git Bash): 
	curl -s "https://get.sdkman.io" | bash
	source "$HOME/.sdkman/bin/sdkman-init.sh"
	sdk version
3) Install Spring Boot (not work in Windows)
	sdk install springboot
	spring --version
4) Install Spring Boot in Eclipse: Help -> Eclipse Marketplace -> spring tools -> install
5) Create a Spring Boot project: Create new other project -> spring starter project -> run as spring boot
	Error: Datasource
	Solved: @SpringBootApplication(exclude= {DataSourceAutoConfiguration.class}) OR add DB info
```
3. Create and build jar by CLI
```
1) Generate project by spring initializr: start.spring.io
2) In the project folder, to run the following command: 
	gradle clean build
	mvn clean install (OR)
	# in the target folder
	java -jar xxxxxx.jar
	# JAVA_HOME has to be a JDK, not a JRE
```
4. Build war file by mvn
```
1) extends SpringBootServletInitializer # in main class
2) <packaging>war</packaging> # in pom.xml
mvn package
grdle clean build
# war file in the /target
```
5. Run application by Maven
```
mvn spring-boot:run
```
6. Autowired beans
```
# @Autowired @Bean @ComponentScan
@SpringBootApplication
public class DemoApplication {
@Autowired
   RestTemplate restTemplate;
   
   public static void main(String[] args) {
      SpringApplication.run(DemoApplication.class, args);
   }
   @Bean
   public RestTemplate getRestTemplate() {
      return new RestTemplate();   
   }
}
```
7. Application Runner & Command Line Runner
```
# implements ApplicationRunner, implements CommandLineRunner
@SpringBootApplication
public class DemoApplication implements ApplicationRunner {
   public static void main(String[] args) {
      SpringApplication.run(DemoApplication.class, args);
   }
   @Override
   public void run(ApplicationArguments arg0) throws Exception {
      System.out.println("Hello World from Application Runner");
   }
}
@SpringBootApplication
public class DemoApplication implements CommandLineRunner {
   public static void main(String[] args) {
      SpringApplication.run(DemoApplication.class, args);
   }
   @Override
   public void run(String... arg0) throws Exception {
      System.out.println("Hello world from Command Line Runner");
   }
}
```
8. Properties
```
# Command Line
java -jar demo-0.0.1-SNAPSHOT.jar --server.port=9090

# application.properties
server.port=9090
spring.application.name=demoservice

# application.yml
spring:
	application:
		name: demoservice
	server:
		port: 9090

# Externalized properties
java -jar -Dspring.config.location = C:\application.properties demo-0.0.1-SNAPSHOT.jar

# Read value from properties
@SpringBootApplication
@RestController
public class DemoApplication {
   @Value("${spring.application.name:demoservice}") // :default value to avoid cannot find issue
   private String name;
   public static void main(String[] args) {
      SpringApplication.run(DemoApplication.class, args);
   }
   @RequestMapping(value = "/")
   public String name() {
      return name;
   }
}   

# Diff environments
## application.properties
server.port = 8080
## application-dev.properties
server.port = 9090
## application-prod.properties
server.port = 4431
# Command Line
java -jar demo-0.0.1-SNAPSHOT.jar --spring.profiles.active=dev
java -jar demo-0.0.1-SNAPSHOT.jar --spring.profiles.active=prod

# Diff yml
spring:
   application:
      name: demoservice
server:
   port: 8080

---
spring:
   profiles: dev
   application:
      name: demoservice
server:
   port: 9090

---
spring: 
   profiles: prod
   application:
      name: demoservice
server: 
   port: 4431
```
9. Log 
```
java -jar demo.jar --debug
# application.properties
debug = true
logging.path = /var/tmp/
logging.file = /var/tmp/mylog.log
logging.level.root = WARN  ## TRACE,DEBUG, INFO,WARN,ERROR, FATAL, OFF

# logback config in logback.xml
<?xml version = "1.0" encoding = "UTF-8"?>
<configuration>
   <appender name = "STDOUT" class = "ch.qos.logback.core.ConsoleAppender">
      <encoder>
         <pattern>[%d{yyyy-MM-dd'T'HH:mm:ss.sss'Z'}] [%C] [%t] [%L] [%-5p] %m%n</pattern>
      </encoder>
   </appender>
   
   <appender name = "FILE" class = "ch.qos.logback.core.FileAppender">
      <File>/var/tmp/mylog.log</File>
      <encoder>
         <pattern>[%d{yyyy-MM-dd'T'HH:mm:ss.sss'Z'}] [%C] [%t] [%L] [%-5p] %m%n</pattern>
      </encoder>
   </appender>
   
   <root level = "INFO">
      <appender-ref ref = "FILE"/>
      <appender-ref ref = "STDOUT"/> 
   </root>
</configuration>

# Used in main class
@SpringBootApplication
public class DemoApplication {
   private static final Logger logger = LoggerFactory.getLogger(DemoApplication.class);
   
   public static void main(String[] args) {
      logger.info("this is a info message");
      logger.warn("this is a warn message");
      logger.error("this is a error message");
      SpringApplication.run(DemoApplication.class, args);
   }
}
```