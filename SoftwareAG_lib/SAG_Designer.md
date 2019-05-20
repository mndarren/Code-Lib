# Link SF to SAG by CloudStreams Adapter
==================================================
1. Package structure
```
-Package_One
  |-Package_One (folder)
     |-adapters (folder)
     |-jms (folder)
     |-services (folder)
     |-webServices (folder)
         |-providers (folder)
         |-consumers (folder)
LaserPro: a document-assembly system used by loan officer for generating loan documentation.
LexisNexis: a provider providing sensitive personal information to certain customers who have completed a rigorous account activation process.
M-Files:  intelligent information management software helps you easily store, organize and access all kinds of documents and information.
          based on what it is instead of where it's been stored.
```
2. Create Web Service provider (Provide a WSDL) Inbound Request
```
   using WSD to do the operation
   choose SOAP type so that after creating it we can test it by SOAP UI (maybe good for REST too)
   web service will use your specific service.
   On tab 'Operations', copy WSDL URL, and then test it by SOAP UI
   # summary: web service is to convert service to WSDL format.
```
3. Create Web Service consumer (Consume a WSDL) Outbound Request
```
   using WSD tool
   test it in Designer, to run Flow Service from connectors.
   in jdbc window tree tab, add connector and link input (City to inCity)
```
4. API Gateway
```
   API Governance: API and API related assets creation and lifecycle governance.
   API Portal: Solution for API providers to maintain relationship with API developers and users
   API Gateway: API request mediation, protection and access control
   # 4 modules
   APIs, Policies, Applications, APIPackages
```
5. How to create, protect and publish an API with webMethods.io API
```
   Start -> Define customer journeys -> Identify resources -> Design APIs
         -> Prototype and Mock APIs -> Publish APIs -> Develop APIs
         -> Automated CI/CD -> Apply policies and deploy -> Measure and analyse APIs -> End
   # About API Perspective
   - We live in a heavily API Connected World.
   - APIs are valuable business assets.
     * Leverage your unique data & services
	 * Create new products & services
	 * Become easier to do business with
	 * Support digital transformation initiatives
   # API management platform (3 parts)
     * API Portal: Engage & Support developers
	 * API Gateway: Protect access & Monetize usage
	 * Microgateway: Manage Microservices architecture
```
6. How to create XML Schema, WSDL, and Header?
```
   Tool: Altova XML Spy Enterprise version 2011 Rel 2 sp1
   Mapping: xxxSystemSel/SystemName - Allowed value: TellerSystem, EFTSystem,HSPSystem,CheckingSystem,
            SavingSystem,LoanSystem,MortgageSystem,ConusmerSystem,InquirySystem,MaintenanceSystem,
			SpecificationSystem,MemoPostSystem
   # Schema Example xxxSystemSvc_v9_0.xsd
   IFX_Type: xxxSystemInqRq - xxxSystemSel: ChannelName, 
                                              SystemName (value: InquirySystem,MaintenanceSystem,MemoPostSystem)
             xxxSystemInqRs - Status (StatusCode, StatusDesc, Severity, SvcProviderName, ServerStatusCode, ..)
			                   xxxSystemRec: xxxSystemInfo - xxxSystemData: SystemName, SystemStatus
							                  xxxSystemStatus: xxxSystemStatusCode, EffDt
   # WSDL Example xxx.wsdl
   PortTypes: xxxSystemServicePortType - PingOper, xxxSystemInqOper
   Bindings: xxxSystemService_SoapHTTPBinding - transport to the same Operations
   Services: xxxSystemService: xxxSystemServicePort
   # From WSDL file WSDL tab to Text tab
   Define operations
   ZZZHeader
   # Example for xxxHdr1_2.xsd
   ZZZHdr - version,
            service: SvcName, Version
			client: Organization - OrgId, CharterId, Environment,  
			        VendorIdent, ClientAppIdent, Version, Channel, ClientDt
			Credentials:
			Tracking: OriginatingTrnId, TrnId, ParentTrnId
			Delivery
			Context
```
7. How to use Designer to develop Web Services?
```
   - Window -> Perspective -> Open Perspective -> Service Development
   - Add and Modify IS button -> Add IS server -> verify the server
   Then we can create package
   Create a WS by import a WSDL file, then docTypes and services will be generated automatically
```
8. XXXSystemInquiry User Guide
```
   Message Header
   Message Status
   XXXSystemInq Mapping Specification: object (XXXSystem), Service (XXXSystemService), operation(XXXSystemInqOper)
   # XXXSystemService operation | Message                        | YYY Object
   XXXSystemInqOper             | XXXSystemInqRq/XXXSystemInqRs  | XXXSystem
   # Instructions
   Required: Name of YYY object (i.e. Party, Acct, PartyAcctRel)
   Required: Operation (i.e. Add, Mod, Del, Inq, Rev, Can)
   Optional: Channel (i.e. NOW, BusinessOnline)
   Required: Object Version (i.e. 1.0, 1.1, 2.0)
   Optional: Extended Object (i.e. PersonParty, OrgParty, LoanAcct, DepAcct)
   Optional: Special Operation (enter 'List' if this is a "List Inq" operation)
   # YYYHdr Schema
   # XPATH                      | Usage     | DataType  | Allowed Values   | YYY Allowed Values
    Version                       Optional    NC-T2                          1.2
	Service!SvcName               Required    C-32        XXXSystemSvc
	Service!Version               Optional    NC-T2
	Client!Organization!OrgId     Required    C-32
	Credentials!SessKey           Required    C-64
	Tracking!TrnId                Required    UUID
   # Status Schema (Required)
   Status/StatusCode, Status/StatusDesc, Status/Severity
   Status/SvcProviderName, Status/AdditionalStatus/StatusCode (same to previous)
   # XXXSystemInq Schema (Required)
   Status (Aggregate type)
   XXXSystemSel (Aggregate)
   XXXSystemSel/SystemName (value: InquirySystem,MaintenanceSystem,MemoPostSystem)
   XXXSystemRec
   XXXSystemRec/XXXSystemInfo
   XXXSystemRec/XXXSystemInfo/XXXSystemData
   XXXSystemRec/XXXSystemInfo/XXXSystemData/SystemName
   XXXSystemRec/XXXSystemInfo/XXXSystemData/SystemStatus (List table: SystemStatus)
   XXXSystemRec/XXXSystemStatus (Rule: None)
   XXXSystemRec/XXXSystemStatus/XXXSystemStatus/XXXSystemStatusCode (Set to 'valid')
   XXXSystemRec/XXXSystemStatus/EffDt (Set to serverDate (yyyy-mm-ddThh:mm:ss:micro))
   # How to SET XXXSystemStatus
   Both Server Response and Host Response are OK => Active
   Otherwise any is Failure => Inactive
```
9. Design service by Designer
```
   Copy and paste service from WS folder to Provider folder
   Insert/Invoke -> choose service and then see the mapping tree
   Enable the service by ESF framework feature (login feature, transaction) call framework services
   # in AthenaClient package, Service template can be found in ExecutionControl/Pub/V1_0/operationFacadeTemplate
   use to template to figure out how to write services
   Copy all to the service that you are working on -> Version conflict
```
10. How Designer template to decode in ESF Studio (link them together)
```
   # Everything go thru the template.
   Each Service in Designer template -- related Service in ESF studio
   Each service has its own operation in ESF studio 
   Once defining each service, ESF Studio generates operation Code. (plug in, plug out)
   # In ESF Studio
   Create Service in Setup/Service (Type: Atomic)
```