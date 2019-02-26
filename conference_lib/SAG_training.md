# Software AG Training
================================
0. Notes
```
   1) 32 bit environments are not supported, so use 64-bit OS;
   2) supported OS: Windows Server 2012, 2016; 
                    Suse Linux Enterprise Server(12 SPx), Red Hat Enterprise Linux Server(7.x), 
                    Solaris(11), HP-UX(11v3), AIX(7.1&7.2), MacOS(10.13)
   3) Linux: If using Installer GUI in Linux or MacOS, must install X11 display server.
   4) If IS shutdown improperly.
      MUST delete 2 files (.lock and wrapper.anchor) in <SAG dir>\profiles\IS_<name>\bin before restarting the services.
      The reason is a Tanuki Java service wrapper will be left bebind, will prevent IS server restart.
   5) Linux: if non-root user installing products, always use the same user shutdown at the same dirctory;
             if afterInstallAsRootLog.txt existing in installation dir, run ./bin/beforeInstallAsRoot script first
   6) License Keys: IS keys: for Integration Platform --> IS production license file (only 30 minutes run without key)
                             for Trading Networks server --> Trading Networks server license file
                             as part of Designer Workstation --> development license file
                             Mediator only --> Mediator license file
                             CloudStreams only --> Cloudstreams license
                    UM can be installed without key, but 90 days trial (key file in <SAG dir>\UniversalMessaging\server\<realmName>\)
                    Terracotta license required if using BigMemory Max or Terracotta DB
   7) Linux/MacOS: # of file descriptors at least 1024 for IS
                   # of coredump, data, file, memory and threads at least 32768 for MWS
                   # of nofiles to 8192 for MWS
   8) DB: at least one DB instance to run webMethods products. 
          For better performance, multiple DB instances running on multiple servers
          Oracle, SQL server and DB2 are supported.
          In labs, create 2 DBs (WEBMDB for Archive, WEBMARCHIVE for others)
          In real environment, recommend to create one DB/schema for each (IS, TN, MWS, BPM, Optimize, Archive) P2-56
          DB setting can be exported to and inported from .xml file 
   9) MEM: at lease 1 GB available, at least 100 MB free disk in system temp before running installation.
   10) SPM: at least one SPM is required per installation (in labs, 2 SPMs used)
   11) Jar install: if installer.exe hang or fail, go to use jar installer because of firewall
   12) 3 ways to start/shutdown IS: windows service, in IS admin UI, run files in bin/

   20) MUST: internal user must be in a group.
             Groups must be associated with ACLs.
   21) OAuth: IS 9.6+ supports OAuth 2.0, IS can be an OAuth client, an authorization server, or a resource server
              Enable OAuth configuration feature (require a license of SAG)
              External authorization servers must support RFC 7662, OAuth 2.0 token Introspection including Okta and Ping Identity
   22) JDK 1.8, not 1.9 version
   23) Great tool: BareTail can be used to monitor IS startup log (IS\instances\default\logs\server.log)
   24) Lab VM config: Windows server 2016, Intel Duo CPU 2.4 GHz, 16 GB memory, 100 GB disk, QEMU Standard PC (i440FX +PIIX, 1996)
   25) 20 labs list: (1) Install integration platform
                     (2) Configure Database via DCC
                     (3) Software AG Update Manager (SUM)
                     (4) Install CC using Bootstrap Installer
                     (5) Configure Logging
                     (6) Logging and Monitoring
                     (7) IS Security: Users and Groups
                     (8) IS Service Security
                     (9) Configure LDAP
                     (10) UM Install and Configuration
                     (11) Performance Management
                     (12) Using TSA to support caching and IS clustering
                     (13) IS Diagnostic Utility
                     (14) Resolving IS Service Issue
                     (15) Day to day Operation
                     (16) Installing Git and Configuring Designer
                     (17) A Development Workflow
                     (18) Asset Build Environment
                     (19) webMethods Deployer
                     (20) Multi-Instance IS
   26) Never allow sample or demo packages into production (e.g. PSUtilities, WmSamples, WmFlatFileSamples)
       Disable unneeded default packages in IS: WmARTExtDC, WmAssetPublisher, WmFlatFile ... P12-13,14
   27) MUST select "Client Prefix is Shared" if IS in a cluster or in load balancing fashion; 
       Connection alias is used by loggers on ISs to write or read logs from UM Queue
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
ESB: Enterprise Services Bus
OAuth: Open Standard Authorization Framework
IETF: Internet Engineering Task Force
OAuth 2.0: IETF standards: RFC 6749 and 6750
JNDI: Java Namespace and Directory Interface
ABE: Asset Build Environment
TN: Trading Network
NHP: Network Host PC
SUM: SAG Update Manager
XSLT: Extensible Stylesheet Language Transformations
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
   SPM: In lab: 9092 (http), 9093 (https); default: 8092(http), 8093(https)
   CC: 8090 (http) 8091 (https)
```
9. DB connection: DataDirect JDBC drivers
```
How to fix typo URL, ID or Password of DB?
   MWS cannot start up. IS can start up, but cannot connect DB.
   MWS to fix it by modify the file: <SAG dir>\MWS\server\default\config\mws.db.xml
   IS to fix it by editing JDBC pools in IS admin UI.
```
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
   MWS: service.bat in <SAG dir>\profiles\MWS_<instance_name>\bin, and then start service in Windows service
        mws.bat -option [start|stop|restart]  # didn't try it.
```
14. IS
```
   IS: from services, select 'After all client sessions end...' and enter a value in Max wait time.
       This'll notify connected clients.
   IS instance stored under <SAG dir>\IntegrationServer\instances.
   IS config file in <SAG dir>\profiles\IS_<instance_name>\configuration\custom_wrapper.conf
      change server logging level to trace: wrapper.app.parameter.n=-debug
                                            wrapper.app.parameter.n=trace
```
15. Quiesce mode (temporarily disable access to the IS)
```
What happens?
   Requests in progress are permitted to finish
   New requests blocked;
   Outbound connection (JDBC pools, LDAP) remain open.
   Scheduled system tasks continue;
   Audit logging continue
   Enterprise Gateway(if acting as IS, not TN): disconnect all connection
How to enter/exit Quiesce mode?
   Use the link in IS admin UI to enter the mode (max wait time setting up).
   the Quiesce port can be set in settings/Quiesce
What to do in Quiesce mode?
   Change ports: except diagnostic port and Quiesce port
   JMS messaging: disable connection, suspend JMS triggers, endpoint
   webMethods messaging: disable connection, suspend doc retrieval&processing from trigger
   package diable: except WmRoot and WmPublic
   scheduled user tasks: permit already running user scheduled tasks
   guaranteed delivery: shutdown inbound outbound delivery
   clustering: shutdown cache manager.
```
16. CC
```
Connect installation -> View installation (products, instances, status, perform lifecycle actions, jobs)
There's another class to introduce CC, including:
    installation and setup
    repositories and license management
    managed installation setup
    managed installation administration and configuration
    templated-based provisioning
    automation and scripting
    security
    DevOps overview
```
17. MWS
```
MWS: Portal server that hosts browser-based UI for monitoring, configuring and user interaction for
     many webMethods products, including ESB, BPMS, User Tasks and Optimize
Configure ESB (ESB = IS + UM)
    Verify license
       if IS key expired, IS will enter demo mode (only run 30 minutes)
       Settings/Licensing/Licensing Details
    perform OS-specific tasks
    IS, UM, TN and Monitor
    Internal DB settings
    Proxies and Extended Settings
```
18. UM (webMethods Messaging, JMS messaging, Broker is deprecated) Chapter 5 and 10
```
Windows Service: Should Create Windows service if on Windows since installer does not create this.
    Start/Software AG/Realm Server Command Prompt -> run registerService.bat
Enterprise Manager:
    Change JNDI URL: File/Open Profile to realm.cfg -> close Enterprise Manager 
                     -> Edit realm.cfg (localhost to sagbase) -> open Enterprise Manager to confirm
    Queue Delivery Persistence Policy: NoPersist/NoSync (not recommanded, doc can be lost)
                     Persist/NoSync -- using cluster (default)
                     Persist/Sync -- single UM node
                     Config tab/Show Advanced Config button/Advanced Connection/Event Storage/QueueDeliveryPersistencePolicy
    Connection Factories, Topics, Queues in JNDI tab (details in lab book)
Connect IS to UM realms: IS can publish/subscribe
                     Messaging Connection alias can be set (by developer) in the IS Document Type properties.
                       IS admin -> Settings/Messaging/JNDI Settings/Create JNDI Provider Alias (if cluster, provider URL contain comma-separated list)
                                   Settings/Messaging/JMS Settings/Create JMS Connection Alias
                     Native msg Connection (system generated): IS_UM_CONNECTION (should confirm url) | IS_DES_CONNECTION | IS_LOCAL_CONNECTION (DES Digital Event Services)
                     Sync IS Doc Types to Messaging Provider
```
19. Configuration
```
IS & webMethods Monitor: display data of process of IS and Optimize, in package: WmMonitor 
Allows MWS to connect TN: Administration/My webMethods/System Settings/TN Servers/Add TN Configuration (P5-44)
JDBC Connection Pool Alias: the default Pool Alias Definitions auto-created. TN and Archive need separate alias pools(good practice).
Web Service Alias: P5-50
Some settings in <SAG dir>\IntegrationServer\instances\<IS instance>\config\server.cnf
System parameters changed in Extended
```
20. Packages (logical IS container for a set of services, doc types, triggers and other related files)
```
Dir: <SAG dir>\IntegrationServer\packages # Also in each instance dir
Namespaces: package name not in it. pub.client:http (right) WmPublic.pub.client:http (Wrong)
Supported Types of Services: .NET, Adapter, C, Flow (SAG), Java, Map, OData, REST Resource Descriptor(Consumer), XSLT
Cannot Disabled: WmRoot (IS core)
Cannot Changed can disabled: WmFlatFile, WmMonitor, WmOptimize, WmPRT
Archived packages: replicate\outbound
Install an Archived package: put zip file in replicate\inbound -> IS admin/Packages/Management/Install Inbound Releases
                                            check "Archive upon installation" if want to archive it to replicate\archive
Lock Mode: (none, system, full)
    In lab, set system because installing local service development. full: enable user lock and show system lock.
    In full locking mode, locking is a feature in Designer. 
                          types: System Lock, Locked by you, Locked by Someone else, No lock.
                          Developer must lock an asset before any change;
                          Developer mush unlock assets before deployment.
Delete package: Disable it first to check if any complaint about it, and then Safe Delete
Loading package: "watt.server.package.parallel.threads" (2-10) default is 6
```
21. IS Management
```
Script <SAG dir>\IntegrationServer\instances\is_instance.bat can create/delete/update IS instance
IS installer only install default instance. core packages defined in is_core_packages.properties
instance.bat only get core packages for the new instance. 'is_instance update' to add additional packages. P7-10,13
SUM only can update fixes for SAG root dir, cannot update for one instance. (make 2 root dir if need)
```
22. Logging and auditing
```
Including: IS core Audit Log and Process Audit Log
Messaging, Server and UM Client Loggers can only log to File System;
Process Logger should log to DB for monitoring in MWS; Most loggers log to DB.
Async/Sync: if logger uses Asynchronous, Transient Queue is used; if Synchronous, directly to DB.
In safe mode: Document and Process Engine loggers are not available.
Server log: format "watt.debut.layout=" legacy(old format) new(new format)
If Service Audit Logger set to 'perSvc', then Developer can set logging in service properties.
MWS logging, UM logging skip here
```
23. Securing the Infrastructure
```
4 pillars: Authentication, Authorization, Confidentiality, Integrity
    Authentication is a prerequisite of authorization. (user IDs, client certificates, Security token: Kerberos, SAML)
    Authorization: access privileges granted to a user, program or process. (Groups, ACLs, OAuth, PortAccess Mode, IP access, .access)
    Integrity: ensuring information non-repudiation and authenticity. (digital signatures)
    Confidentiality: Server certi, message encryption, https/ftps ports
Points: 
    Users in denied group always be denied even also in allowed group;
    Best practice: Lowest level of access;
    .access: Restricts pub folder files to specific ACLs
    OAuth 2.0: OpenID from Salesforce allowed to access IS port
Ports:
    Access Mode: Port allows a set of services to be invoked
    IP Access: Port must be called by IP group
Encryption: S/MINE or PGP
Secure transport: https(SSL), ftps(SSL), sftp(SSH)
User ID: internal user must be in a group. Name and password are case-sensitive.
How to manage users and groups?
    IS Admin UI -> Security -> User Management -> create groups (one per line) 
                -> create users (one per line) and assign users to groups
```
24. Security: ACLs, OAuth, Port Access Mode, IP Access, .access, Certificate, HTTPS
```
ACLs: ACLs control access to IS packages, folders, files, and services.
      ACLs identifies Allowed groups and Denied groups
      4 types: List (metadata,input, output), Read(source code & metadata), Write (modify elements), Execute(services)
      Deployer can deploy ACLs.
How to deploy ACLs?
    Create ACLs: IS Admin UI -> Security -> ACLs -> Add and Remove ACLs (create ACL and add members)
    Assign ACLs to service: IS Admin UI -> Package -> Management -> Browser Folders -> adminSupport -> svcs -> customWriteToLog
                            -> Execute ACL -> select the related ACL -> test it with the triangle button
How to manage user in Designer?
    Software AG Designer -> Window -> Preferences -> Integration Servers -> Edit -> modify user or others
How to run service or test functionality?
    Software AG Designer -> Window ->Perspective -> Open Perspective -> service Development 
                         -> AdminSupport/svcs/customWriteToLog -> right click/run as Flow service
OAuth: details in lecture
```
25. LDAP
```
Details in lecture
```
23. Performance Tuning: A thankless Task
```
Basic ideas:
    1) it's rarely a short-term task (establish baseline->small change->new baseline->another small change...)
    2) it rarely shows huge return (one change may undo anohter or many, Test many times concurrently)
    3) it's about Reducing latency (time of a procedure to complete) and Increasing throughput (# of procedures completed per time period)
    4) there's always a tradeoff between performance and cost (investment)
    5) it's a shared responsibility (Architecture, Hardware and OS, enhanced or damaged by devs, monitored and tuned by admins)
    6) Pareto principle (80/20 rule) 50% savings in processes give almost 40% overall improvement.
Hardware and OS:
    1) Set wrapper.java.initmemory and wrapper.java.maxmemory the same value to avoid memory allocation overhead if incoming load is varying and unexpected
       Set initmemory 1/3rd of maxmemory if load is constant. benchmarking is the only way to determine ideal settings.
Developers:
    1) Code for performance and maintainability from the beginning;
    2) DROP unneeded pipeling variables
    3) Use Java services in place of high CPU overhead flow services
    4) Understand adapter service usage best practices, such as batch activity
    5) Avoid deeply nested looping
    6) Limit process logging level and service auditing levels
    7) Work carefully with optimize locally, express pipeline, and corelation
    8) Use service caching: Caching with Big Memory (TSA) P12-37-46
    9) Define services as stateless - this is the default
    10) Eliminate "dead" services and documents in packages
Admin:
    1) Package, port, server thread pool, logging management
       Turn off non-essential listeners; 
       Enable port thread pool if inbound requests are critical AND IS may be close to using the entire global thread pool;
       Server thread pool and trigger thread pool are the most critical thread pools.
       Only testing can determine the best size for a thread pool
       In most cases, less logging less auditing = better performance (Disable all unneeded logging)
       Set default log level to FATAL or ERROR (at least in PROD)
       Set Auditing 'Guaranteed' to 'No' to keep the transient entries in RAM
    2) Server parameter settings
    3) IS dispatcher, UM tuning
       UM Connection Buffer Size: default is 21 MB. message>default => error
       Small buffer + large msg = High CPU
       Large buffer + small msg = inefficient memory usage.
    4) IS clustering
    5) Large document configuration

Packages:
   1) unnecessary packages should be disabled or deleted, save boot time and free memory;
   2) Samples packages are not alive in production machine (WmTNExtDC, WmVCS)

```
24. UM 
```
How to install UM?
    1) Start -> Software AG -> Realm Server Command Prompt
    2) run registerService.bat # SAG UM service installed
    3) from windows service to start the service
How to connect IS to UM?
    1) Start -> SAG -> Enterprise Manager
    2) Realms -> umserver -> JNDI (check url: nsp://localhost:9000)
    3) File -> Open Profile -> find the realms.cfg
    4) change url: open the realms.cfg file with notepad++, change localhost into sagbase -> save
    5) restart Enterprise Manager
```
25. Trouble Shooting
```
empower website, logs, service usage, statistics, diagnostic port
Diagnostic utility service (recommanded) (sagbase:9999/invoke/vm.server.admin/getDiagnosticData)
Thread Dump: a track trace for each running thread in a java process. Can determine a port or service has become nonresponsive.
      c:\SoftwareAG\common\bin\wrapper-3.5.32.exe -d c:\SoftwareAG\profiles\IS_<Instance name>\configuration\wrapper.conf
      # this command can be got from Windows Services: Double click the service -> copy 'Path to executable' -> change -s to -d
      #The dump file locates <SAG dir>\profiles\IS_default\logs\wrapper.log
How to get in and out the Safe Mode?
    In <SAG dir>\profiles\IS_default\configuration\custom_wrapper.conf,
    Change line: wrapper.app.parameter.2=4 (from 4 to 5)
    Add line after the line parameter.6: wrapper.app.parameter.7=-safeboot
    Save the file.
    Run the bat: <SAG dir>\profiles\IS_default\bin\startup.bat/
    In the Safe Mode, you can find the packages cannot be activated in Packages/Management/Activate Inactive Packages
    After done action, shutdown IS, Change back the custom_wrapper.conf file (better way is to make a copy before the 1st change)
How to generate Thread Dump and Cancel thread from IS Admin UI?
    Server/Statistics/Usage/System Threads, click Current #, click Generate JVM Thread Dump
    Select the checkbox "Show threads that can be cancelled or ...", then Cancel the bad threads
How to remove a package from startup services list?
    Go into the package: <SAG dir>\IntegrationServer\instances\default\packages\StartupPackage\manifest.v3
    Change <record name="startup_services" ....<record> into <null name="startup_services"/>
    Also, this can be done in Designer: Right click package -> Properties -> Startup/Shutdown Services -> move available services to selected services
```
26. Email Notification
```
Start Email server -> Configure IS -> Configure MWS -> Create a scheduled task
How to configure Email Notification in IS?
    Settings/Resources/Edit Resource Settings: SMTP Server(sagbase),Internal Email(Administrator@company.com), Service Email(Customer@company.com)
    Extended: change watt.server.email.from=Administrator@company.com
How to configure Email Notification in MWS?
    Applications -> Administration -> My webMethods -> E-mail Servers: SMTP hosts(sagbase), SMTP username and password(empty)
                                                       From Email: Administrator@company.com, the same to Admin Email Address
    
```