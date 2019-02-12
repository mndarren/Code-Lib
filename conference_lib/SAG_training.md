# Software AG Training
================================
0. Notes
```
   1) 32 bit environments are not supported;
   2) supported OS: Windows Server 2012, 2016; 
                    Suse Linux Enterprise Server(12 SPx), Red Hat Enterprise Linux Server(7.x), 
                    Solaris(11), HP-UX(11v3), AIX(7.1&7.2), MacOS(10.13)
   3) Linux: If using Installer GUI in Linux or MacOS, must install X11 display server.
   4) If stopping service (SAG, MWS or others) in Microsoft service, not in CC, and shutdown IS improperly.
      MUST delete 2 files (.lock and wrapper.anchor) in <SAG dir>\profiles\IS_<name>\bin before restarting the services.
      The reason is a Tanuki Java service wrapper will be left bebind, will prevent IS server restart.
   5) Linux: if non-root user installing products, always use the same user shutdown at the same dirctory;
             if afterInstallAsRootLog.txt existing in installation dir, run ./bin/beforeInstallAsRoot script first
   6) License Keys: IS keys: for Integration Platform --> IS production license file
                             for Trading Networks server --> Trading Networks server license file
                             as part of Designer Workstation --> development license file
                             Mediator only --> Mediator license file
                             CloudStreams only --> Cloudstreams license
                    UM can be installed without key, but 90 days trial
                    Terracotta license required if using BigMemory Max or Terracotta DB
   7) Linux/MacOS: # of file descriptors at least 1024 for IS
                   # of coredump, data, file, memory and threads at least 32768 for MWS
                   # of nofiles to 8192 for MWS
   8) DB: at least one DB instance to run webMethods products. 
          For better performance, multiple DB instances running on multiple servers
          Oracle, SQL server and DB2 are supported.
   9) MEM: at lease 1 GB available, at least 100 MB free disk in system temp before running installation.
   10) 
```
1. Abbrs about webMethods Integration Platform
```
IS -- Integration Server
MWS -- My webMethods Server
BPM: Business Processes Management
CC: Command Central (web UI or CLI)
UM: Universal Messaging (webMethods Messaging, JMS)
TSA: Terracotta Server Array
SAP: IBM ERP system
HIP: Hybrid Integration Platform
DCC: Database Component Configurator
SDC: Software Download Center
SPM: SAG Platform Manager
DBP: Digital Business Platform
     Analytics & Decisions (streaming analytics & AI, In-Memory Data) by Terracotta & Apama
     Process & Applications (Dynamic process automation, Low-code application) by webMethods
     Integration & API (Hybrid integration, API management) by webMethods (***)
     Devices (Device Connectivity, Device management) by Cumulocity
features: IS, MWS, CC, UM, TSA
what to cover in this workshop: IS, MWS, CC, UM, Terracotta BigMemory Max, Asset Build Env and Deployer
```
2. What is HIP?
```
Integrated combination of: 
    a. On-premises
       application/data
       integration platforms
    b. iPaaS/iSaaS
    c. API management
Connect everything! API gateway.
Integration Platform includes Cloud IN, Mobile IN, API management, Big Data IN, IoT IN, 
                              Application IN(***), B2B IN, File IN, Process IN, Data IN.
```
3. CC tool includes web UI and CLI.
4. UM: 2 types webMethods Messaging and JMS (Java Message Service)
5. Terracotta Big Memory: 2 types (Enterprise Ehcache in memory caching, TSA for clustering and distributed caches)
6. Lifecycle Management
```
   Designer and Mobile Designer;
   Installer, Update Manger and Deployer
   Command Central and MWS
```
7. Install HIP
```
Download installer https://empower.softwareag.com/Products/DownloadProducts/default.asp
    Software AG Installer, Update Manager, Command Central Bootstrapper
Download licensed webMethods products using SAG installer: http://empower.softwareag.com
Install products via SAG installer
Troubleshoot installation issues
Use DCC (Database Component Configuration)
```
8. Ports
```
   IS (5555 for admin, 9999 for common);
   MWS: 8585(http), 8443(https), 10998(RMI), 10999(RMI Registry), 5001(Java debug and SOAP monitor), 5002(JMX), 8009(AJP13)
   UM: 9000 (realm server interface port)
```
9. DB connection: DataDirect JDBC drivers
10. Process to install products: Download Intaller -> Download and create local image -> install image.
11. Script file: silent installation (product installation, image generation)
12. Login (default: Administrator | manage)
```
     IS http login: http://hostname:port (http://localhost:5555)
     MWS login: http://hostname:port (http://localhost:8585)
     CC UI login: http://hostname:port/cce/web (http://localhost:8090/cce/web, https://localhost:8091/cce/web)
     CLI: <SAG dir>\CommandCentral\client
```
13. Start/Stop service
```
   CC Start/Stop: startup.bat shutdown.bat in <SAG dir>\profiles\CCE\bin
   SPM: same files in <SAG dir>\profiles\SPM\bin
   # if SPM shutdown, CC cannot manage Installations.
   UM: nserver.bat nstopserver.bat <SAG dir>\UniversalMessaging\server\umserver\bin
   IS: start.bat shutdown.bat in <SAG dir>\profiles\IS_<instance_name>\bin
   IS: from services, select 'After all client sessions end...' and enter a value in Max wait time.
       This'll notify connected clients.
   IS instance stored under <SAG dir>\IntegrationServer\instances.
   IS config file in <SAG dir>\profiles\IS_<instance_name>\configuration\custom_wrapper.conf
```