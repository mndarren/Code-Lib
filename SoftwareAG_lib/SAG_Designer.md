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
6. How to create XML Schema?
```
   Tool: Altova XML Spy Enterprise version 2011 Rel 2 sp1
   Mapping: HostSystemSel/SystemName - Allowed value: TellerSystem, EFTSystem,HSPSystem,CheckingSystem,
            SavingSystem,LoanSystem,MortgageSystem,ConusmerSystem,InquirySystem,MaintenanceSystem,
			SpecificationSystem,MemoPostSystem
   # Schema Example HostSystemSvc_v9_0.xsd
   IFX_Type: HostSystemInqRq - HostSystemSel: ChannelName, 
                                              SystemName (value: InquirySystem,MaintenanceSystem,MemoPostSystem)
             HostSystemInqRs - Status (StatusCode, StatusDesc, Severity, SvcProviderName, ServerStatusCode, ..)
			                   HostSystemRec: HostSystemInfo - HostSystemData: SystemName, SystemStatus
							                  HostSystemStatus: HostSystemStatusCode, EffDt
   # WSDL Example xxx.wsdl
   PortTypes: HostSystemServicePortType - PingOper, HostSystemInqOper
   Bindings: HostSystemService_SoapHTTPBinding - transport to the same Operations
   Services: HostSystemService: HostSystemServicePort
   # From WSDL file WSDL tab to Text tab
   Define operations
   EFXHeader
   # Example for xxxHdr1_2.xsd
   EFXHdr - version,
            service: SvcName, Version
			client: Organization - OrgId, CharterId, Environment,  
			        VendorIdent, ClientAppIdent, Version, Channel, ClientDt
			Credentials:
			Tracking: OriginatingTrnId, TrnId, ParentTrnId
			Delivery
			Context
```
7. How to use Designer to develop Web ?
```
   - Window -> Perspective -> Open Perspective -> Service Development
   - Add and Modify IS button -> Add IS server -> verify the server
   Then we can create package
```