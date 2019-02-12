# Software AG Training
================================
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
DCC: Database Component Configuration
SDC: Software Download Center
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