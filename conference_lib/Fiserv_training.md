# Fiserv Training
================================
1. Abbr and Concepts
```
ESF -- Enterprise Services Framework
EFX -- Enterprise Financial Exchange msg framework
BIAN -- Banking Industry Architecture Network
CoA -- Communicator Advantage
SOA -- Service Oriented Architecture
SAG -- Software AG (150 banks, 70 countries)
UDDI-- Universal Description Discovery and Integration
UDDI registry service -- a web service managing info about service provider, service implementation and service metadata.
WSD -- Web Service Descriptor
WSC -- Web Service Connector (a flow service with an input and output signature
       that corresponds to the input and output messages of the web service operation.)
MTOM-- Message Transmission Optimization Mechanism (reason: Text Encoding using base 64 format which inflates 30% in msg)
       Binary Encoding: 3 options can be chosen (TCP/Pipe/MSMQ). Need to test them on your environment and take a decision.
       MTOM is related to Bandwidth, while Streaming is related to Buffering.
```
2. Premier Services (BIAN)
```
Account Management: acct reconciliation, acct receivable processing, acct inquiry, acct open,
                    broader consolidated position management, fraud detection facilities;
Cards: card activation, card limit validation, card current status, PIN change, card transaction history,
       PIN reset, modify card limit
Party: add a customer, customer inquiry
Lending: ability to board a loan
Payments: connecting to payment network, central cash management, check processing, funds transfer, memo post
Servicing: Fulfilment capabilities for various customers servicing support functions (retrieve enumeration value) 
           in particular customer case resolution and analysis, and customer orders.
```
3. Training topics
```
Group 1: 
    High level BA Overview
    Mapping sheet walkthrough
    Building of Schema and WSDL
    Package creation, importing WSDL
Group 2:
    Q&A from previous session
    High level BA Overview
    Walkthrough of ESF façade
    Creating façade flow service
    Set up - Service, operation, onboarding configuration
    Set up - Provider, Organization
Group 3:
    Configure – Parameters, xref rules, Status code, routing rules
    Onboarding, Logs
    Creating implementation service using mapping sheet
    Example with Parameters, xref rule and status code
    Unit testing
Group 4:
    Extract build using deployer
    Extract ESF script using migration module
    Deployment using deployer and migration module
    Artifacts created by Dev: unit test suite, design document, configuration sheet, deployment document
Group 5:
    Importing WSDL in SOAP UI and creating a test suite
    Defining the endpoint and credentials
    Creating a SOAP request
    Running the service and validating the request and response
    Validating the provider logs, how to check logs in ESF studio
```
4. No 3rd party consumer application and no 3rd party provider. Add requirement, add cost!
5. Keep using the packages that Fiserv provides. The new package/service can be built by clients also.
6. Designer: Developer tool for Software AG; ESF Studio: Developer tool for Fiserv.
7. ESF Studio functionalities (localhost:8023/ESF)
```
    Validating data (WS req & res)
    Data Mask to mask data value (create operation)
    Configuration: providers and related versions (Core Premier Provider, Signature Provider)
        ESF Rq  -- Provider request (POST)
        ESF Rs  -- Provider Req

        1234[5678] -- data mask
        Add provider package: Example HostSystem_v1_PRMCoreDemo_v1 (service name + provider name) name rule
    Setup: provider
        Implementation Service
        Status code: 0 -- success
        endpoint: name PRM, SOAP endpoint: URL http://localhost:8088/
    Configuration: parameters, status code (operation uses status code how many times) 0  Info   Success
        XRef Rules (detail) Examples
        ESF rq  |  OrgID   | Provider Rq  |  balance Type
        ---------------------------------------------------
        SDA     | DemoBank | SAV          |  22            1-*
        SDA     |  ABC     | 11           |  23            *-1

        Maintainence System -- MS
        Inquary System      -- IS

        Source field name (SystemName_ESF): Maintainence System   Inquary System
        Target field name (SystemName_Prv): MS                    IS
        # Routing Rules (Service no dropdown)
        # Data Center (can be used to add 3rd party provider) ***
    Onboarding: configuration, Log config, Detail Payload (immediate), Data Masking (add masking)
    Routing Rules: 1) Create it for each operation
                   2) Routing to remote data center (3rd party data center)
                   3) Endpoints
                   4) XPath expression (//ExpeditedInd='true')
```
8. Designer
```
    1) Global var in IS settings (localhost:5555)
    2) operation code, package -> getOperationCode from ESF
    3) Acct_v8, userEvent, EFXAPi, 2 types of error status
    4) CoA Training/Trainin Artifacts/wsdl/providers
    5) How to create a package: create package -> PRMCoreDemo_v1_connector -> create folder (same name)
                                -> create another folder (WSD) -> create web service descripter (consumer, getHandshake_core)
                                MAP (Extract EFXApi document)
                                MAP (MAP FiHeader)
                                MAP (MAP FiRequest)
                                LOOP endpoint
                                    LOOP endpoint properties
                                        Branch on key (Label: ServiceName) ServiceName and Version should be same to ESF Studio
                                In mapping detail: pattern: should be one format that provider accept.
                                                   Date Time: current datatime
                                                   UUID
                                                   pipeline in  | Service in      |  service out    |  pipeline out
                                                                  SystemName_ESF  | SystemName_Prv

```
9. Difference between provider WSD (Web Service Descriptor) and customer WSD
```
    Provider WSD is created from one or more IS services or from a single WSDL document, 
                 and is designed to allow the IS services to be invoked as web services over a network. 
                 2 types: a service first provider web service descriptor, 
                          a WSDL first provider web service descriptor.
                 How to create a service first provider WSD? Page 21 in SAG developer guide
    Customer WSD defines an external web service, which contains all the data from WSDL doc that defines the web service, 
                 as well as data needed for certain IS run-time properties.
                 IS creates a WSC and a response service (v9.0 and later) for each operation 
                 -> IS executes the WSC, which call a operation of a WS
```
10. Debug
```
    Use SOAP UI to test created services, including provider WSD and customer WSD.
    Before running test suites:
        1) check service in ESF studio -> setup -> organization (OrgId)
        2) set pipeline header = true in Designer
        3) Auth to be set to 'Basic' in Designer permission
        4) ExecuteACL: Application. Can be found in IS UI 5555 -> Security -> ACL
        5) set pipeline debug = Save in Designer
        6) others in SOAP UI: venderIdent: ESF, TrnId: UUID (123456789), channelName: NOW, SystemName: InquirySystem
    Debug:
        1) set pipeline debug: Restore (Override);
        2) check log tab in ESF Studio
        3) check vars from Designer, and record implementation service name
        4) can setup breakpoint in Designer
```
11. More Debug & Deployment
```
    Added transformer mapping for previous debug.
    # code for random UUID
    Random trnId: ${*java.util.UUID.randomUUID()}
    Create Service/Package -> Unit Test using SOUP UI -> Debug -> Deployment
    # Deployment
    1) localhost:5555 -> Deployer
    2) ESF Studio -> maintenance -> Export Data: (can be service, routing rule, onboarding)
       -> service -> version (8.0) -> Generate Migration Script -> Save (specify the script file name)
       # can import data using the Migration Script file
    3) ESF Studio -> symmetric OnBoarding (No-Yes-Yes, No means not same configuration)
                  -> Add Org (download template - excel org ids)
                  -> Customize OnBoarding (manage config)
                  -> Add service provider (just add org) -> populate 
                  # Apply to Routing point Yes, manage routing endpoint - Yes
                  - org endpoint update URL
                  - add org user (no use here, used to validate user)
                  - manage Xref rule -> check/add -> save (download template)
                  - add Xref Rule values (service / provider) -> submit (download script)

```
