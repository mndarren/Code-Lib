# Fiserv Training
================================
1. Abbr
```
ESF -- Enterprise Services Framework
EFX -- Enterprise Financial Exchange msg framework
BIAN -- Banking Industry Architecture Network
CoA -- Communicator Advantage
SOA -- Service Oriented Architecture
SAG -- Software AG (150 banks, 70 countries)
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
