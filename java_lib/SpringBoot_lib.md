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
1) Install zip: choco install zip
2) Install sdkman(in Git Bash): 
	curl -s "https://get.sdkman.io" | bash
	source "$HOME/.sdkman/bin/sdkman-init.sh"
	sdk version
3) Install Spring Boot (not work in Windows)
	sdk install springboot
	spring --version
3) Install Spring Boot: manually download spring-boot-cli-2.3.0.BUILD-SNAPSHOT-bin.zip from https://docs.spring.io/spring-boot/docs/current-SNAPSHOT/reference/htmlsingle/#getting-started-manual-cli-installation
```
3. Create Hello World Spring project
```
1) Generate project by spring initializr: start.spring.io
2) 
```