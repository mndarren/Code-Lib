# Link SF to SAG by CloudStreams Adapter
==================================================
0. Main points
```
   1) For each SAG service, we should build a related service in ESF
   2) In ESF Studio, each service can have multiple operations.
      Each provider can have multiple Provider Implementation Services.
   3) import service -> copy service to provider folder for reusable purpose -> invoke service 
      -> create business logic by copy template
   4) Each WS has a different Provider Implementation Service (including Signature, PRM)
```
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
9. How to Design service by Designer
```
   Copy and paste service from WS folder to Provider folder
   ***WS service Insert/Invoke service in provider folder -> choose service and then see the mapping tree
   Enable the service by ESF framework feature (login feature, transaction) call framework services
   # in AthenaClient/ExecutionControl/Pub/V1_0/operationFacadeTemplate
   use to template to figure out how to write services
   Copy template content to service in provider folder (after solved conflict)
   Copy all to the service that you are working on -> (Version conflict at that time)
```
10. How Designer template to decode in ESF Studio (link them together) Setup/Service
```
   # Everything go thru the template.
   Each Service in Designer template -- related Service in ESF studio
   Each service has its own operation in ESF studio 
   Once defining each service, ESF Studio generates operation Code. (plug in, plug out)
   # In ESF Studio
   - Create Service in Setup/Service (Type: Atomic) Service name should be XXXSystem, not XXXSystemInq
     Service name: XXXSystem is different from XXXSystemInq
   - Create operation (operation code will be generated by DB SQL code in Server)
     operation name: XXXSystemInq. Enable Framework
	 * Parameter: version, 1.0, header version (parameters can be found in Designer related service)
	 * Status code: 100, Error, Error Status
	   Status Codes can also be defined in configure tab as universal status code.
	 * Data Masking (about payload) PayloadType: Request/Response/Intermediate
	   Intermediate means to try to send to your provider
	   * ESF Rq generates Provider request -> Post WS Connector -> Provider Response as ESF Rs
	     Both provider request and provider response can be logged as intermediate logs
	   * Request, Xpath: copy SystemName from Designer, stateCharacter/endCharacter (0-4)LeftToRight
```
11. ESF Studio Setup/Provider
```
   # create Provider in ESF
   Provider: XXXCoreDemo, Provider version: 1.0 (required for configuration)
   Each core provider has to be written an provider implementation service
   # create Provider Implementation Service in Designer
   New package (XXXSystem_v1_YYYCoreDemo_v1)/Pub
   Create Flow Service (XXXSytemInqBizSvc)
   # in ESF Studio
   - Provider Implementation Service: XXXSystem_v1_YYYCoreDemo_v1.Pub:XXXSytemInqBizSvc
     All other service should add implementation service here (partyAcct, ...)
   - Status: without configuration definition, drop down shows empty.
     Default: ESF (0-success, 100-Error), Signature (200-Error), PRM (300-Error)
   - Endpoint: Mock for now
     YYYCoreDemo_GetHandshake, SOAP Endpoint, SOAP, http://localhost:8088/mockGetHandshakeService_SoapHTTPBinding
	 K/V: {Version: 1.0, ServiceName: GetHandshake}
```
12. ESF Setup/Organization
```
   # create organization
   Organization: DemoOrg, Org ID: 1, ID Source: DUNS/FDIC/FISERV (DUNS for safe option)
```
13. ESF Configure
```
   - Parameters: the parameters created on Setup will automatically show here.
   - XRef Rules: Example: it can be used Account Type (ESF Rq: SDA, Provider Rq: SAV)
     OR SystemName translation
     Translation can be 1-*, 1-1, *-1, *-*
	 Create new XRef rule: Name/ID/Service (which service will use this rule)
   - Status Code (can be used in operation) can link operation for specific status code (conflict for now)
   - Routing Rules, create new rule (used to link service, operation, organization, implementation service, provider)
     ID, Organization, Service (not work for now)
	 Add Endpoint: once choose a provider, we have to choose which endpoint to go to since each provider can have multiple endpoints.
	 The Routing Rule ID can be used to do debugging with logs
```
14. ESF Onboarding
```
   Onboard organization: link Org to Service
   Multiple Orgs can use the same Service.
   Open configuration: we can enable "Validate Request" and enable "Validate Response"
         Log configuration: can apply to all operations
		 Data Masking: 
```
15. ESF Logs
```
   Transaction from to date time
   Advanced Search: Transaction Id, Organization, Service, Operation, Routing Rule ID, Status
```
16. ESF Administration
```
   - User management: SuperAdmin (create user, System user/Active Directory )
       Usually give SuperAdmin/ServiceAdmin to developers since creating/modifying configuration items
   - Group Management: no use
   - Role Management: create roles
   - Session Management: no use
   - Settings: used to when connecting to 80
```
17. ESF Maintenance
```
   Migration: import data / export data
   we have to define all configuration for a service, after that. we can move it from one environment to another.
   Maybe use "Export data" feature to do the above
```
18. Designer to design Business logic by Facade Template
```
   # WS service revoke Provider service; copy Facade service from Acct package to Provider service
   # global variables come from IS settings/Global Variables
   Update operationCD in preProcessor step
   Change pipeline input mapping and pipeline output mapping
   MAP (Build EFXApi Doc for implementation service) just for implementation service input fields
```
19. Designer implementation service Biz logic
```
   - Copy EFXApi from Provider service to Provider Implementation service input Doc
     (RoutingInfo, CPStack, Header, Request)
   - Copy Response Doc from Provider service output to provider implementation service output Doc
     in Response sub-level
   - Build Biz logic
     . Map (Extract EFXApi documents) copy input fields to pipeline output
	 . in ESF Studio/Setup/Provider/Endpoint/Key.Value {ServiceName: GetHandshake, Version: 1.0) check it
	   The Endpoints can be multiple (different URL, ServiceName and version values should be different
	 . Loop RoutingInfo/Endpoints
	      Loop Endpoints/properties
		     Branch property/key == "ServiceName"
			    Map property/value to fi:fiHeader/fi:Service/@Name (copy fi:fiAPI from connector to pipeline output first)
     . Map Header other fields
	   Version (hard code 1.0)
	   fi:Service/fi:DateTime = GetCurrentDateString (pattern = yyyy-MM-dd'T'HH.mm.ssZ
	   fi:UUID maps to efxhdr:TrnId (firstly fixed Provider Service biz logic mapping EFXHearder, then copy the EFXHeader to Implementation Service first MAP)
	   # fi:Client/@Version can also to create K/V in ESF studio/Provider/Endpoints/k.v
	   Hard code for now (@Version=1.0, fi:VendorID=ESF, fi:AppID=ESF, fi:OrgID=DemoOrg)
     . Mapping fi:Request (map GroupName, and hard code others for now)
	   Copy XXXSystemInqRq from Provider package WS pineline in to this Map pipeline in/request
	   # direction: 0-Left2Right, 1-Right2Left
	   Invoke XRef from AthenaClient package (ruleCD = XRef Rule ID from ESF, direction=0)
	      *** ns1:SystemName - Keys/SystemName_ESF, Values/SystemName_Pro - fi:GroupName
	 . Mapping connector (Copy connector to the biz logic)
	   SOAPPEP/url - _url (since we use SOAP type in ESF provider endpoint config)
	   # If username and password config in ESF, we should map them as well
	   Mapping Response
```
20. Designer to design Handshake package (Consumer visitor)
```
   # YYYCoreDemo_v1_connector
   Import WSDL to create a WS Consumer visitor.
   
```