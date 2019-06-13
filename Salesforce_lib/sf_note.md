# SF Notes (key: CRM)
=======================<br>
0. HTTP methods and suffix
```
   GET    - Select
   POST   - Create
   PATCH  - Update
   PUT    - Upsert
   DELETE - Delete
   __c - Customer Object
   __e - Platform Event object
   __x - External Object
```
1. Limitation of Batch (Bulk API)
```
1) Max 10k batches/day
2) Max 7 days keeping for batches and jobs
3) One Batch size: <=10MB, <=10k records, <=10M chars
4) One field: <=32k chars
5) One record: <=5k fields, <=400k chars
6) Batch processing in chunks: 1 chunk =200 records for >=V21.0; 1 chunk =100 records for <=V20.0
7) Timeout: 5 min/chunk, 10 min/batch, 10 times back queue -> mark Failed permenantly for batch
```
2. Points to be taken care of
```
1) Fields names of SF Objects are case sensitive, otherwise Error!
2) the field "SomethingDate__c" of Contact cannot be assigned empty string 
   because it's Date type, but can be null value in json.
   "RecordType" field cannot be included in Contact object.
3) Fields names of SF object for Bulk API are case sensitive;
4) the field related SSN for SF Bulk API should be correct format, like: 123-45-6789
5) "No content to map to Object" Error message means there's some field(s) not in SF Object.
6) We don't need to send null fields or empty fields to SF, 
   so just delete them from the list of SF object.
```
3. csv format requirement
```
1) Only comma.
2) If a comma, a new line, or a double quote in value,must be contained within double quotes: 
   for example, "Director of Operations, Western Region".
3) If double quote in value, double double quotes, e.g. "This is the ""gold"" standard"
4) A space before or after a double quote generates an error for the row. For example, 
   John,Smith is valid; John, Smith is valid, but the second value is " Smith"; ."John", "Smith" is not valid.
5) Empty field values are ignored when you update records. 
   To set a field value to null, use a field value of #N/A.
6) Fields with a double data type can include fractional values. 
   Values can be stored in scientific notation 
   if the number is large enough (or, for negative numbers, small enough)
```
4. batch status
![alt text](https://github.com/mndarren/Code-Lib/blob/master/conference_lib/references/batch_status.png)
5. SF has multiple applications
```
   Sales, Service, Marketing, Community, Analytics, Platform
```
6. SOAP UI endpoint explanation
```
   # https://test.salesforce.com/services/Soap/c/45.0/0DF190000000CEd
   login.salesforce.com/test.salesforce.com - Top level domain for login request;
   /service/Soap - Specify SOAP API request
   /c - Enterprise WSDL (/u : partner WSDL)
   /45.0 - API version
   /0DF190000000CEd - Package version
```
7. What to know about APIs 
```
   # API first in SF, UI will be built on APIs
   REST: support XML & JSON; can call all SF functionalities (CRUD), Sync, for Mobile&Web App
   SOAP: only XML supported; using WSDL, so it's great for server to server integration, Sync
   BULK: 1.0 and 2.0 versions. 2.0 easy to use. Since internally using IP authentication, cannot use OAuth approach. Async
   Streaming: pub/sub model, reducing # of rolling. usually used to do notification event. Async
   # API limits (2 types)
   . Concurrent Limits cap: Trailhead Playground 5 long running calls/once; Sandbox org 25/once
   . Total Limits cap(24 hrs): Enterprise Edition (1000 with SF license, 200 with SF Light App license, +4000 with unlimited Apps pack)
   . How to check limits: setup -> system overview / API usage notification
   # other API
   . UI API used to CRUD records, list views, actions, dependent picklists
   . Chatter API for building UI for Chatter, communities or recommendations. (in Mobile App)
   . Metadata API to migrate Changes from a sandbox to production environment (tools: Force.com IDE/ANT Migration)
   . Analytics API for datasets, lenses, dashboards
   . Apex REST API: expose Apex classes and methods. (support OAuth and Session ID)
   . Apex SOAP API: expose Apex methods as SOAP WS
   . Tooling API: to integrate SF Metadata with other systems. 
                  manage and deploy working copies of Apex classes & triggers and VF pages and components.
```
8. REST API
```
   # link to query SOQL
   /services/data/v45.0/query/?q=SELECT+Name+From+Account+WHERE+ShippingCity='San+Francisco'
   # post an account creation
   /services/data/v45.0/sobjects/account
   # Node.js using Nforce
var nforce = require('nforce');
// create the connection with the Salesforce connected app
var org = nforce.createConnection({
  clientId: process.env.CLIENT_ID,
  clientSecret: process.env.CLIENT_SECRET,
  redirectUri: process.env.CALLBACK_URL,
  mode: 'single'
});
// authenticate and return OAuth token
org.authenticate({
  username: process.env.USERNAME,
  password: process.env.PASSWORD+process.env.SECURITY_TOKEN
}, function(err, resp){
  if (!err) {
    console.log('Successfully logged in! Cached Token: ' + org.oauth.access_token);
    // execute the query
    org.query({ query: 'select id, name from account limit 5' }, function(err, resp){
      if(!err && resp.records) {
        // output the account names
        for (i=0; i<resp.records.length;i++) {
          console.log(resp.records[i].get('name'));
        }
      }
    });
  }
  if (err) console.log(err);
});
```
9. SOAP API
```
   # 2 typws of WSDL: Enterprise (1 org) and partner (* org)
   # How to find security token
   <your name > My Settings > Personal  >  Reset My Security Token
   # Login request
   get username from Users list in SF
   get token from previous step, password + token as passwork in SOAP UI
   # copy the instance name (the part before .salesforce.com)
   # copy the session ID
   # Create sObject
   URL: change instance name and remove the version ID (the last part)
   Header: remove all others but session ID, paste the session ID
   Body: replace <urn:sObjects> with <urn:sObjects xsi:type="urn1:Account" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
         <Name>Bluebeards Grog House</Name>
         <Description>It is better than Blackbeards.</Description>
   # copy the result with ID
```
10. Streaming API
```
   # keep your external source in sync with your SF data with PushTopic events and Change Data Capture events
   # PushTopic
     a sObject containing the criteria of events you want to listen to. 
	 Criteria is the SOQL query
	 Supported objects: Account, Contact, Opportunity.
	 # code
	 PushTopic pushTopic = new PushTopic();
     pushTopic.Name = 'AccountUpdates';
     pushTopic.Query = 'SELECT Id, Name, Phone FROM Account WHERE BillingCity=\'San Francisco\'';
     pushTopic.ApiVersion = 37.0;
     insert pushTopic;
	 # change which fields trigger notifications
	 pushTopic.NotifyForFields = [All, Referenced (default), Select, Where)
	 # Notification
	 pushTopic.NotifyForOperationCreate = true;
     pushTopic.NotifyForOperationUpdate = true;
     pushTopic.NotifyForOperationUndelete = true;
     pushTopic.NotifyForOperationDelete = true;
	 # Channel: /topic/PushTopicName
	 # query
	   The SELECT statement’s field list must include Id.
       Only one object per query is allowed.
       The object must be valid for the specified API version.
	   aggregate queries and semi-joins not supported
   # Custom Notifications with Platform Events
     # Define platform event (Need: Create platform event and adding fields)
	 API name suffix with __e, like Order_Event__e
	 A channel name is provided automatically, like /event/Order_Event__e
	 # Publishing Platform Events
	   Process Builder using the Create a Record action
       Flow using a Create Records element
       Apex EventBus.publish() method
       REST API sobjects resource
       SOAP API create() call
	 # Subscribing the Platform Events
	 # How? Setup -> search "Platform Events"
   # Custom Notification with Generic Streaming
     StreamingChannel: sObject, can be created by Apex, REST or SOAP.
	 channel name: /u/ChannelName
	 # Apex Code
	 StreamingChannel ch = new StreamingChannel();
     ch.Name = '/u/Broadcast';
     insert ch;
	 # How? Setup -> search "User Interface" -> Enable Dynamic Streaming Channel Creation
   # Generate events by REST
     . get channel ID: SOQL(SELECT Id, Name FROM StreamingChannel)
	 . /services/data/vXX.0/sobjects/StreamingChannel/Streaming Channel ID/push
	 . Body: # userIds: specify a list of subscribed users
	   { 
         "pushEvents": [
              { 
                 "payload": "Broadcast message to all subscribers", 
                 "userIds": [] 
               } 
            ] 
        }
   # Retrieve Past Notification
     before version 37.0, only keep message for 24 hours
	 From version 37.0, set up a ReplayId, any time replay it.
	 "-1": all new events after subscription
	 "-2": All events
```
11. UI API: Lightning
```
   # Records and Layouts
   /ui-api/record-ui/{recordIds}
   /ui-api/layout/{objectApiName}
   /ui-api/object-info/{objectApiName}
   /ui-api/records/{recordId}
   /ui-api/record-defaults/create/{objectApiName}
   /ui-api/record-defaults/clone/{recordId}
   /ui-api/object-info/{objectApiName}/picklist-values/{recordTypeId}
   # List Views
   /ui-api/list-ui/${listViewId}
   /ui-api/list-ui/${objectApiName}/${listViewApiName}
   /ui-api/list-info/${listViewId}
   /ui-api/list-info/${objectApiName}/${listViewApiName}
   /ui-api/list-records/${listViewId}
   /ui-api/list-records/${objectApiName}/${listViewApiName}
   # Actions
   /ui-api/actions/global
   /ui-api/actions/record/${recordIds}
   /ui-api/actions/record/${recordId}/record-edit
   /ui-api/actions/record/${recordId}/related-list/${relatedListIds}
   // There are more actions resources! Check the User Interface API Developer Guide!
   # Favorites
   /ui-api/favorites
   /ui-api/favorites/${favoriteId}
   /ui-api/favorites/batch
   /ui-api/favorites/${favoriteId}/usage
   # Lookups
   /ui-api/lookups/{objectApiName}/{fieldApiName}
   /ui-api/lookups/{objectApiName}/{fieldApiName}/{targetApiName}

```
12. Apex Integration (REST)
```
   # Rule: every Callout from SF should be approved first.
   # How
   Setup -> Remote Site Setting -> New Remote Site -> name, url, desc -> save
   # How to test code
   Developer Console -> Debug -> Open Execute Anonymous Window -> copy/paste code -> Open log -> execute -> Debug only
   # Example code
Http http = new Http();
HttpRequest request = new HttpRequest();
request.setEndpoint('https://th-apex-http-callout.herokuapp.com/animals');
request.setMethod('GET');
HttpResponse response = http.send(request);
// If the request is successful, parse the JSON response.
if (response.getStatusCode() == 200) {
    // Deserialize the JSON string into collections of primitive data types.
    Map<String, Object> results = (Map<String, Object>) JSON.deserializeUntyped(response.getBody());
    // Cast the values in the 'animals' key as a list
    List<Object> animals = (List<Object>) results.get('animals');
    System.debug('Received the following animals:');
    for (Object animal: animals) {
        System.debug(animal);
    }
}
   # JSON2Apex Tool: https://json2apex.herokuapp.com/
   # example code for Post
Http http = new Http();
HttpRequest request = new HttpRequest();
request.setEndpoint('https://th-apex-http-callout.herokuapp.com/animals');
request.setMethod('POST');
request.setHeader('Content-Type', 'application/json;charset=UTF-8');
// Set the body as a JSON object
request.setBody('{"name":"mighty moose"}');
HttpResponse response = http.send(request);
// Parse the JSON response
if (response.getStatusCode() != 201) {
    System.debug('The status code returned was not expected: ' +
        response.getStatusCode() + ' ' + response.getStatus());
} else {
    System.debug(response.getBody());
}
   # Test Callouts (Apex not supported, but can make Mock
   # create Apex class first
public class AnimalsCallouts {
    public static HttpResponse makeGetCallout() {
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        request.setEndpoint('https://th-apex-http-callout.herokuapp.com/animals');
        request.setMethod('GET');
        HttpResponse response = http.send(request);
        // If the request is successful, parse the JSON response.
        if (response.getStatusCode() == 200) {
            // Deserializes the JSON string into collections of primitive data types.
            Map<String, Object> results = (Map<String, Object>) JSON.deserializeUntyped(response.getBody());
            // Cast the values in the 'animals' key as a list
            List<Object> animals = (List<Object>) results.get('animals');
            System.debug('Received the following animals:');
            for (Object animal: animals) {
                System.debug(animal);
            }
        }
        return response;
    }
    public static HttpResponse makePostCallout() {
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        request.setEndpoint('https://th-apex-http-callout.herokuapp.com/animals');
        request.setMethod('POST');
        request.setHeader('Content-Type', 'application/json;charset=UTF-8');
        request.setBody('{"name":"mighty moose"}');
        HttpResponse response = http.send(request);
        // Parse the JSON response
        if (response.getStatusCode() != 201) {
            System.debug('The status code returned was not expected: ' +
                response.getStatusCode() + ' ' + response.getStatus());
        } else {
            System.debug(response.getBody());
        }
        return response;
    }        
}
   # Create Mock Test 
   Developer Console -> File/New/Static Resource -> MIME type (text/plain, if using JSON)
   # example Mock test return: {"animals": ["pesky porcupine", "hungry hippo", "squeaky squirrel"]}
   # create Apex Test class
   File/New/Apex Class
   # Example code for GET
@isTest
private class AnimalsCalloutsTest {
    @isTest static  void testGetCallout() {
        // Create the mock response based on a static resource
        StaticResourceCalloutMock mock = new StaticResourceCalloutMock();
        mock.setStaticResource('GetAnimalResource');
        mock.setStatusCode(200);
        mock.setHeader('Content-Type', 'application/json;charset=UTF-8');
        // Associate the callout with a mock response
        Test.setMock(HttpCalloutMock.class, mock);
        // Call method to test
        HttpResponse result = AnimalsCallouts.makeGetCallout();
        // Verify mock response is not null
        System.assertNotEquals(null,result,
            'The callout returned a null response.');
        // Verify status code
        System.assertEquals(200,result.getStatusCode(),
          'The status code is not 200.');
        // Verify content type   
        System.assertEquals('application/json;charset=UTF-8',
          result.getHeader('Content-Type'),
          'The content type value is not expected.');  
        // Verify the array contains 3 items     
        Map<String, Object> results = (Map<String, Object>) 
            JSON.deserializeUntyped(result.getBody());
        List<Object> animals = (List<Object>) results.get('animals');
        System.assertEquals(3, animals.size(),
          'The array should only contain 3 items.');          
    }   
}
   # Note: Test/Always Run Asynchronously -> Test/New Run
   # Test POST with HttpCalloutMock: Test.setMock(HttpCalloutMock.class, new AnimalsHttpCalloutMock());
   # create AnimalsHttpCalloutMock class first
@isTest
global class AnimalsHttpCalloutMock implements HttpCalloutMock {
    // Implement this interface method
    global HTTPResponse respond(HTTPRequest request) {
        // Create a fake response
        HttpResponse response = new HttpResponse();
        response.setHeader('Content-Type', 'application/json');
        response.setBody('{"animals": ["majestic badger", "fluffy bunny", "scary bear", "chicken", "mighty moose"]}');
        response.setStatusCode(200);
        return response; 
    }
}
   # Add method into the previous AnimalsCalloutsTest class
@isTest static void testPostCallout() {
    // Set mock callout class 
    Test.setMock(HttpCalloutMock.class, new AnimalsHttpCalloutMock()); 
    // This causes a fake response to be sent
    // from the class that implements HttpCalloutMock. 
    HttpResponse response = AnimalsCallouts.makePostCallout();
    // Verify that the response received contains fake values
    String contentType = response.getHeader('Content-Type');
    System.assert(contentType == 'application/json');
    String actualValue = response.getBody();
    System.debug(response.getBody());
    String expectedValue = '{"animals": ["majestic badger", "fluffy bunny", "scary bear", "chicken", "mighty moose"]}';
    System.assertEquals(actualValue, expectedValue);
    System.assertEquals(200, response.getStatusCode());
}
   # Note: callouts can be in triggers and in asynchronous Apex (2 ways to do async run callout)
   @future(callout=true) OR Queueable Apex  # callout running in separate thread
   Trigger's callout should use "@future(callout=true)"
   ## Exercise work
public class AnimalLocator { 
    public class Animal {
		public Integer id;
		public String name;
		public String eats;
		public String says;
	}
    public Animal animal;
    
    public static string getAnimalNameById(integer id){
        string str;
        string URL='https://th-apex-http-callout.herokuapp.com/animals/'+id;
        
        http http=new http();
        httprequest Req=new httprequest();
        req.setEndpoint(URL);
        req.setMethod('GET');
        httpResponse Response=http.send(req);
        
        system.debug('Response Code: '+response.getStatusCode());
        system.debug('Response Body: '+response.getBody());
        //type ResultType= type.forName('Animals');
        //system.debug('Type: '+ ResultType);
        AnimalLocator obj= new AnimalLocator();
        obj=(AnimalLocator) System.JSON.deserialize(response.getBody(), AnimalLocator.class);
        System.debug('Obj: '+obj.animal.name );
        str=obj.animal.name;
        System.debug('Name: '+str );
        return str;
    }
}

@istest
public class AnimalLocatorTest {
   testmethod static void  Restcallout(){
        Test.setMock(HttpCalloutMock.class, new AnimalLocatorMock());
        string s=AnimalLocator.getAnimalNameById(1);
    }
}}

@istest
public class AnimalLocatorMock implements HttpCalloutMock {
    public httpresponse respond(httprequest req){
                
        httpresponse Response=new httpresponse();
        response.setStatusCode(200);
        response.setBody('{"animal":{"id":1,"name":"chicken","eats":"chicken food","says":"cluck cluck"}}');
         
        return response;
    }
}}
   Note: https://opfocus.com/json-deserialization-techniques-in-salesforce/
   ***We should use the JSON.deserialize, not JSON.deserializeUntyped()
      2 ways work: 
	  Type resultType = Type.forName('CustomersResponse');
      CustomersResponse deserializeResults = (CustomersResponse)JSON.deserialize(response, resultType);
	  # OR put the Apex classes as internal class
public with sharing class CustomersResponse {
    public List<Customer> Customers;
    public Integer Count;
    public CustomersResponse() {
}
public with sharing class Customer {
    public String Status;
    public String Message;
    public String FirstName;
    public String LastName;
    public String Email;
    public String Phone;
    public Customer() {
    }
}
   # Rules for this point about JSON
   1. Data member names must exactly match (case-insensitive) the name of the attribute 
      or element in the JSON object.
   2. All data members that are being mapped must have public access.
   3. All data members must be typed correctly to match the element type in the JSON object, 
      otherwise a TypeException is thrown. So in our example, if Count is defined to be a String, 
	  an Exception is  thrown as it should be defined as an Integer.
   4. NOTE: Interestingly enough, you do not have to define a default no-arg constructor. 
      In fact, if you define a private no-arg Constructor , it still parses your object successfully.
   5. If you do not define a data member for one of the JSON object properties, 
      then it will not be mapped. It will be silently ignored.
```
13. Apex Integration (SOAP)
```
   # WSDL2Apex
   Setup -> Apex Classes search -> Generate from WSDL -> Choose file -> Parse WSDL -> Generate Apex code
   Only manually code the Apex classes with calling WS or HTTP
   # Not work for testing WS Callouts, using Mock to solve it (WebServiceMock, Test.setMock)
   # Process for SOAP WS
   Generate Apex from WSDL -> Remote Site Settings (add link) -> Create Apex class, test class, mock class
   # Code example
public class ParkLocator {
    public static String[] country(String c) {
        ParkService.ParksImplPort park = 
            new ParkService.ParksImplPort ();
        return park.byCountry(c);
    }
}
@isTest
global class ParkServiceMock implements WebServiceMock {
	global void doInvoke(
           Object stub,
           Object request,
           Map<String, Object> response,
           String endpoint,
           String soapAction,
           String requestName,
           String responseNS,
           String responseName,
           String responseType) {
        // start - specify the response you want to send
        ParkService.byCountryResponse response_x = 
            new ParkService.byCountryResponse ();
               response_x.return_x = new String[]{'Germany', 'India', 'Japan', 'United States'};
        // end
        response.put('response_x', response_x); 
   }
}
@isTest
public class ParkLocatorTest {
	@isTest static void testParkLocator() {              
        // This causes a fake response to be generated
        Test.setMock(WebServiceMock.class, new ParkServiceMock());
        // Call the method that invokes a callout
        String x = 'India';
        String[] result = ParkLocator.country(x);
        // Verify that a fake result is returned
        System.assertEquals(new String[]{'Germany', 'India', 'Japan', 'United States'}, result); 
    }
}
```
14. Export REST or SOAP Service from Apex (Workbench & cURL)
```
   # How to do the REST? class as global, methods as global static, annotations
   # Code example
@RestResource(urlMapping='/Cases/*')
global with sharing class CaseManager {
    @HttpGet
    global static Case getCaseById() {
        RestRequest request = RestContext.request;
        // grab the caseId from the end of the URL
        String caseId = request.requestURI.substring(
          request.requestURI.lastIndexOf('/')+1);
        Case result =  [SELECT CaseNumber,Subject,Status,Origin,Priority
                        FROM Case
                        WHERE Id = :caseId];
        return result;
    }
    @HttpPost
    global static ID createCase(String subject, String status,
        String origin, String priority) {
        Case thisCase = new Case(
            Subject=subject,
            Status=status,
            Origin=origin,
            Priority=priority);
        insert thisCase;
        return thisCase.Id;
    }   
    @HttpDelete
    global static void deleteCase() {
        RestRequest request = RestContext.request;
        String caseId = request.requestURI.substring(
            request.requestURI.lastIndexOf('/')+1);
        Case thisCase = [SELECT Id FROM Case WHERE Id = :caseId];
        delete thisCase;
    }     
    @HttpPut
    global static ID upsertCase(String subject, String status,
        String origin, String priority, String id) {
        Case thisCase = new Case(
                Id=id,
                Subject=subject,
                Status=status,
                Origin=origin,
                Priority=priority);
        // Match case by Id, if present.
        // Otherwise, create new case.
        upsert thisCase;
        // Return the case ID.
        return thisCase.Id;
    }
    @HttpPatch
    global static ID updateCaseFields() {
        RestRequest request = RestContext.request;
        String caseId = request.requestURI.substring(
            request.requestURI.lastIndexOf('/')+1);
        Case thisCase = [SELECT Id FROM Case WHERE Id = :caseId];
        // Deserialize the JSON string into name-value pairs
        Map<String, Object> params = (Map<String, Object>)JSON.deserializeUntyped(request.requestbody.tostring());
        // Iterate through each parameter field and value
        for(String fieldName : params.keySet()) {
            // Set the field and value on the Case sObject
            thisCase.put(fieldName, params.get(fieldName));
        }
        update thisCase;
        return thisCase.Id;
    }    
}
   # URL: https://yourInstance.salesforce.com/services/apexrest/Account/*
   # @HttpGet, @HttpPost, @HttpDelete, @HttpPut, @HttpPatch
   # Recommendation: versioning your API endpoints, like /Cases/v1/*  /Cases/v2/*
   # How to do the SOAP? Class as global, method as webservice static
   # Example
global with sharing class MySOAPWebService {
    webservice static Account getRecord(String id) {
        // Add your code
    }
}
   # App Manager: set up Authorization https://developer.salesforce.com/docs/atlas.en-us.218.0.chatterapi.meta/chatterapi/CR_quickstart_oauth.htm
   App Manager -> new connected app -> Enable OAuth Settings -> name, email, callback url, Access and manage your data(api) -> save
   # How to use curl to connect SF
   setup App permission(previous step) -> get Client ID, customer secret, userId and password+token (reset password), sandbox ID (xxxx.my.)
   -> command: curl -v https://login.salesforce.com/services/oauth2/token -d "grant_type=password" -d "client_id=<your_consumer_key>" -d "client_secret=<your_consumer_secret>" -d "username=<your_username>" -d "password=<your_password_and_security_token>" -H 'X-PrettyPrint:1'
   curl https://yourInstance.salesforce.com/services/apexrest/Cases/<Record_ID> -H 'Authorization: Bearer <your_session_id>' -H 'X-PrettyPrint:1'
   # the session_id is the previous step token id.
   # Note: Using double quote for Authorization: Bearer part because of ! => \! Single Quote not work!
   # Test Code
@IsTest
private class CaseManagerTest {
    @isTest static void testGetCaseById() {
        Id recordId = createTestRecord();
        // Set up a test request
        RestRequest request = new RestRequest();
        request.requestUri =
            'https://yourInstance.salesforce.com/services/apexrest/Cases/'
            + recordId;
        request.httpMethod = 'GET';
        RestContext.request = request;
        // Call the method to test
        Case thisCase = CaseManager.getCaseById();
        // Verify results
        System.assert(thisCase != null);
        System.assertEquals('Test record', thisCase.Subject);
    }
    @isTest static void testCreateCase() {
        // Call the method to test
        ID thisCaseId = CaseManager.createCase(
            'Ferocious chipmunk', 'New', 'Phone', 'Low');
        // Verify results
        System.assert(thisCaseId != null);
        Case thisCase = [SELECT Id,Subject FROM Case WHERE Id=:thisCaseId];
        System.assert(thisCase != null);
        System.assertEquals(thisCase.Subject, 'Ferocious chipmunk');
    }
    @isTest static void testDeleteCase() {
        Id recordId = createTestRecord();
        // Set up a test request
        RestRequest request = new RestRequest();
        request.requestUri =
            'https://yourInstance.salesforce.com/services/apexrest/Cases/'
            + recordId;
        request.httpMethod = 'GET';
        RestContext.request = request;
        // Call the method to test
        CaseManager.deleteCase();
        // Verify record is deleted
        List<Case> cases = [SELECT Id FROM Case WHERE Id=:recordId];
        System.assert(cases.size() == 0);
    }
    @isTest static void testUpsertCase() {
        // 1. Insert new record
        ID case1Id = CaseManager.upsertCase(
                'Ferocious chipmunk', 'New', 'Phone', 'Low', null);
        // Verify new record was created
        System.assert(Case1Id != null);
        Case case1 = [SELECT Id,Subject FROM Case WHERE Id=:case1Id];
        System.assert(case1 != null);
        System.assertEquals(case1.Subject, 'Ferocious chipmunk');
        // 2. Update status of existing record to Working
        ID case2Id = CaseManager.upsertCase(
                'Ferocious chipmunk', 'Working', 'Phone', 'Low', case1Id);
        // Verify record was updated
        System.assertEquals(case1Id, case2Id);
        Case case2 = [SELECT Id,Status FROM Case WHERE Id=:case2Id];
        System.assert(case2 != null);
        System.assertEquals(case2.Status, 'Working');
    }    
    @isTest static void testUpdateCaseFields() {
        Id recordId = createTestRecord();
        RestRequest request = new RestRequest();
        request.requestUri =
            'https://yourInstance.salesforce.com/services/apexrest/Cases/'
            + recordId;
        request.httpMethod = 'PATCH';
        request.addHeader('Content-Type', 'application/json');
        request.requestBody = Blob.valueOf('{"status": "Working"}');
        RestContext.request = request;
        // Update status of existing record to Working
        ID thisCaseId = CaseManager.updateCaseFields();
        // Verify record was updated
        System.assert(thisCaseId != null);
        Case thisCase = [SELECT Id,Status FROM Case WHERE Id=:thisCaseId];
        System.assert(thisCase != null);
        System.assertEquals(thisCase.Status, 'Working');
    }  
    // Helper method
    static Id createTestRecord() {
        // Create test record
        Case caseTest = new Case(
            Subject='Test record',
            Status='New',
            Origin='Phone',
            Priority='Medium');
        insert caseTest;
        return caseTest.Id;
    }          
}
   # Supported Data Type for Apex REST
   . Blob not supported
   . Only maps with String keys are supported
   . other Apex primitives
   # Namespace in Apex REST Endpoints
   https://instance.salesforce.com/services/apexrest/packageNamespace/MyMethod/
   # Exercise Code
@restresource(urlMapping='/Accounts/*')
global with sharing class AccountManager {
	@HttpGet
    global static Account getAccount() {
        RestRequest request = RestContext.request;
        // grab the acctId from the end of the URL
        String acctId = request.requestURI.substringBetween('Accounts/','/contacts');
        Account result =  [SELECT Id, Name, (SELECT Id, Name FROM Contacts)
                        FROM Account
                        WHERE Id = :acctId];
        return result;
    }
}
@isTest
public class AccountManagerTest {
	 @isTest static void testGetAccount() {
        Account Acc=new Account();
         acc.Name='Darren';
         insert acc;
         Contact con=new Contact();
         con.LastName='Xie';
         con.FirstName='Zhao';
         con.AccountId=acc.Id;
         insert con;
        // Set up a test request
        RestRequest request = new RestRequest();
        request.requestUri =
            'https://curious-bear-9nzbf3-dev-ed.my.salesforce.com/services/apexrest/Accounts/'
            + acc.Id + '/contacts';
        request.httpMethod = 'GET';
        RestContext.request = request;
        // Call the method to test
        Account thisCase = AccountManager.getAccount();
        // Verify results
        System.assert(thisCase != null);
         System.debug('Account = ' + thisCase);
        //System.assertEquals('Test record', thisCase.Subject);
    }
}
```
15. Asynchronous processing
```
   # benefits
   User efficiency | Scalability | Higher Limits | Higher Governor (100 queries to 200 queries for SOQL)
   Future Methods, Batch Apex, Queueable Apex, Scheduled Apex
   # Future Methods
   @future | Must static void | only primitive data type as paramters.
   when to use? 
   . Callouts to external WS in a trigger
   . Operations you want to run in their own thread.
   . sometimes synchronous sometimes asynchronous
   # Code example
public class SMSUtils {
    // Call async from triggers, etc, where callouts are not permitted.
    @future(callout=true)
    public static void sendSMSAsync(String fromNbr, String toNbr, String m) {
        String results = sendSMS(fromNbr, toNbr, m);
        System.debug(results);
    }
    // Call from controllers, etc, for immediate processing
    public static String sendSMS(String fromNbr, String toNbr, String m) {
        // Calling 'send' will result in a callout
        String results = SmsMessage.send(fromNbr, toNbr, m);
        insert new SMS_Log__c(to__c=toNbr, from__c=fromNbr, msg__c=results);
        return results;
    }
}
@isTest
global class SMSCalloutMock implements HttpCalloutMock {
    global HttpResponse respond(HttpRequest req) {
        // Create a fake response
        HttpResponse res = new HttpResponse();
        res.setHeader('Content-Type', 'application/json');
        res.setBody('{"status":"success"}');
        res.setStatusCode(200);
        return res; 
    }
}
@IsTest
private class Test_SMSUtils {
  @IsTest
  private static void testSendSms() {
    Test.setMock(HttpCalloutMock.class, new SMSCalloutMock());
    Test.startTest();
      SMSUtils.sendSMSAsync('111', '222', 'Greetings!');
    Test.stopTest();
    // runs callout and check results
    List<SMS_Log__c> logs = [select msg__c from SMS_Log__c];
    System.assertEquals(1, logs.size());
    System.assertEquals('success', logs[0].msg__c);
  }
}
   # Test future method and remember
   . startTest and stopTest to run asynchronous processes.
   . Cannot be used in Visualforce controllers in getMethodName(), seMethodName()
   . Cannot call future method from another future method
   . Cannot put getContent() and getContentAsPDF() in future method
   . 50 futuren calls per Apex invocation.
   # Best practices
   . Ensuer that future methods execute as fast as possible.
   . For WS callouts, try to bundle all callouts together
   . < 200 records
   . Consider using Batch Apex to process large # of records.
   # Exercise Code
public class AccountProcessor {
	 @future
    public static void countContacts(Set<id> ids) {
        List<Account> accounts_l = [select Id, Number_of_Contacts__c, (select Id from contacts) from account where Id in :ids];
        for (Account acc : accounts_l){
            List<Contact> contacts_l = acc.contacts;
            acc.Number_of_Contacts__c = contacts_l.size();
        }
        Database.update(accounts_l);
    }
}
@isTest
public class AccountProcessorTest {
	@IsTest
  private static void testAccountProcessor() {
      Account acc = new Account();
      acc.Name = 'Darren1';
      Database.insert(acc);
      Account acc1 = new Account();
      acc1.Name = 'Darren2';
      Database.insert(acc1);
      
      Contact con = new Contact();
      con.LastName = 'Xie';
      con.AccountId = acc.Id;
      Database.insert(con);
      Contact con1 = new Contact();
      con1.LastName = 'Xie';
      con1.AccountId = acc1.Id;
      Database.insert(con1);
      Contact con2 = new Contact();
      con2.LastName = 'Xie';
      con2.AccountId = acc1.Id;
      Database.insert(con2);
    Test.startTest();
      AccountProcessor.countContacts(new Set<id>{acc.Id, acc1.Id});
    Test.stopTest();
    
    System.assertEquals(1, acc.Number_of_Contacts__c);
      System.assertEquals(2, acc1.Number_of_Contacts__c);
  }
}
```
16. Batch Apex
```
   # Usage: data cleaning or archiving
   # benefits
   . each transaction starts with a new set of governor limits
   . one batch failing not affect other batches
   # Syntax: start() execute(), finish() | Database.Stateful to maintain state across all transactions
global class MyBatchClass implements Database.Batchable<sObject> {
    global (Database.QueryLocator | Iterable<sObject>) start(Database.BatchableContext bc) {
        // collect the batches of records or objects to be passed to execute
    }
    global void execute(Database.BatchableContext bc, List<P> records){
        // process each batch of records
    }    
    global void finish(Database.BatchableContext bc){
        // execute any post-processing operations
    }    
}
MyBatchClass myBatchObject = new MyBatchClass(); 
Id batchId = Database.executeBatch(myBatchObject);
Id batchId = Database.executeBatch(myBatchObject, 100);
AsyncApexJob job = [SELECT Id, Status, JobItemsProcessed, TotalJobItems, NumberOfErrors FROM AsyncApexJob WHERE ID = :batchId ];
   # Example Code
global class UpdateContactAddresses implements 
    Database.Batchable<sObject>, Database.Stateful {
    
    // instance member to retain state across transactions
    global Integer recordsProcessed = 0;
    global Database.QueryLocator start(Database.BatchableContext bc) {
        return Database.getQueryLocator(
            'SELECT ID, BillingStreet, BillingCity, BillingState, ' +
            'BillingPostalCode, (SELECT ID, MailingStreet, MailingCity, ' +
            'MailingState, MailingPostalCode FROM Contacts) FROM Account ' + 
            'Where BillingCountry = \'USA\''
        );
    }
    global void execute(Database.BatchableContext bc, List<Account> scope){
        // process each batch of records
        List<Contact> contacts = new List<Contact>();
        for (Account account : scope) {
            for (Contact contact : account.contacts) {
                contact.MailingStreet = account.BillingStreet;
                contact.MailingCity = account.BillingCity;
                contact.MailingState = account.BillingState;
                contact.MailingPostalCode = account.BillingPostalCode;
                // add contact to list to be updated
                contacts.add(contact);
                // increment the instance member counter
                recordsProcessed = recordsProcessed + 1;
            }
        }
        update contacts;
    }    
    global void finish(Database.BatchableContext bc){
        System.debug(recordsProcessed + ' records processed. Shazam!');
        AsyncApexJob job = [SELECT Id, Status, NumberOfErrors, 
            JobItemsProcessed,
            TotalJobItems, CreatedBy.Email
            FROM AsyncApexJob
            WHERE Id = :bc.getJobId()];
        // call some utility to send email
        EmailUtils.sendMessage(job, recordsProcessed);
    }    
}
@isTest
private class UpdateContactAddressesTest {
    @testSetup 
    static void setup() {
        List<Account> accounts = new List<Account>();
        List<Contact> contacts = new List<Contact>();
        // insert 10 accounts
        for (Integer i=0;i<10;i++) {
            accounts.add(new Account(name='Account '+i, 
                billingcity='New York', billingcountry='USA'));
        }
        insert accounts;
        // find the account just inserted. add contact for each
        for (Account account : [select id from account]) {
            contacts.add(new Contact(firstname='first', 
                lastname='last', accountId=account.id));
        }
        insert contacts;
    }
    static testmethod void test() {        
        Test.startTest();
        UpdateContactAddresses uca = new UpdateContactAddresses();
        Id batchId = Database.executeBatch(uca);
        Test.stopTest();
        // after the testing stops, assert records were updated properly
        System.assertEquals(10, [select count() from contact where MailingCity = 'New York']);
    }
    
}
   # Note: Test Batch Apex only test one batch, so should < 200 records in the test method
   . if < 200 records to run, use Queueable Apex.
   . Tune any SOQL query to gather the records to execute as quickly as possible
   . Minimize the number of asynchronous requests created to minimize the chance of delays
   . Use extreme care if you are planning to invoke a batch job from a trigger.
   # Exercise Code
global class LeadProcessor implements 
    Database.Batchable<sObject>, Database.Stateful{
	// instance member to retain state across transactions
    global Integer recordsProcessed = 0;
    global Database.QueryLocator start(Database.BatchableContext bc) {
        return Database.getQueryLocator(
            'SELECT ID, LeadSource FROM Lead'
        );
    }
    global void execute(Database.BatchableContext bc, List<Lead> scope){
        // process each batch of records
        for (Lead lead : scope) {
            lead.LeadSource = 'Dreamforce';
        }
        Database.update(scope);
    }    
    global void finish(Database.BatchableContext bc){
        System.debug(recordsProcessed + ' records processed. Shazam!');
        AsyncApexJob job = [SELECT Id, Status, NumberOfErrors, 
            JobItemsProcessed,
            TotalJobItems, CreatedBy.Email
            FROM AsyncApexJob
            WHERE Id = :bc.getJobId()];
        // call some utility to send email
        //EmailUtils.sendMessage(job, recordsProcessed);
    }   
}
@isTest
public class LeadProcessorTest {
	@testSetup 
    static void setup() {
        List<Lead> leads = new List<Lead>();
        // insert 10 accounts
        for (Integer i=0;i<200;i++) {
            leads.add(new Lead(LastName='Lead '+i, 
                Company='Company'+i, Status='Status'+i));
        }
        insert leads;
    }
    static testmethod void test() {        
        Test.startTest();
        LeadProcessor lp = new LeadProcessor();
        Id batchId = Database.executeBatch(lp);
        Test.stopTest();
        // after the testing stops, assert records were updated properly
        System.assertEquals(200, [select count() from lead]);
        System.assertEquals('Dreamforce', [select LeadSource from lead][0].LeadSource);
    }
}
```
17. Queueable Apex: Superset of future method (50 jobs limit)
```
   # benifits
   . Non-primitive types
   . Monitoring: invoking the System.enqueuJob to return Id of AsyncApexJob record.
   . Chaining jobs
   # syntax
public class SomeClass implements Queueable { 
    public void execute(QueueableContext context) {
        // awesome code here
    }
}
   # Example code
public class SomeClass implements Queueable { 
    public void execute(QueueableContext context) {
        // awesome code here
    }
}

// find all accounts in ‘NY’
List<Account> accounts = [select id from account where billingstate = ‘NY’];
// find a specific parent account for all records
Id parentId = [select id from account where name = 'ACME Corp'][0].Id;
// instantiate a new instance of the Queueable class
UpdateParentAccount updateJob = new UpdateParentAccount(accounts, parentId);
// enqueue the job for processing
ID jobID = System.enqueueJob(updateJob);
SELECT Id, Status, NumberOfErrors FROM AsyncApexJob WHERE Id = :jobID

@isTest
public class UpdateParentAccountTest {
    @testSetup 
    static void setup() {
        List<Account> accounts = new List<Account>();
        // add a parent account
        accounts.add(new Account(name='Parent'));
        // add 100 child accounts
        for (Integer i = 0; i < 100; i++) {
            accounts.add(new Account(
                name='Test Account'+i
            ));
        }
        insert accounts;
    }
    
    static testmethod void testQueueable() {
        // query for test data to pass to queueable class
        Id parentId = [select id from account where name = 'Parent'][0].Id;
        List<Account> accounts = [select id, name from account where name like 'Test Account%'];
        // Create our Queueable instance
        UpdateParentAccount updater = new UpdateParentAccount(accounts, parentId);
        // startTest/stopTest block to force async processes to run
        Test.startTest();        
        System.enqueueJob(updater);
        Test.stopTest();        
        // Validate the job ran. Check if record have correct parentId now
        System.assertEquals(100, [select count() from account where parentId = :parentId]);
    }
    
}
   # Chaining Jobs (no jobs limitation, but 5 jobs limit for Developer Edition)
public class FirstJob implements Queueable { 
    public void execute(QueueableContext context) { 
        // Awesome processing logic here    
        // Chain this job to next job by submitting the next job
        System.enqueueJob(new SecondJob());
    }
}
   # Exercise Code
public class AddPrimaryContact implements Queueable{
	private Contact cont;
    private String stat;
    
    public AddPrimaryContact(Contact cont, String stat) {
        this.cont = cont;
        this.stat = stat;
    }
    public void execute(QueueableContext context) {
        List<Account> acct_l = [select Id, BillingState from Account where BillingState = :stat];
        List<Contact> cont_l = new List<Contact>();
            for(Account acc : acct_l){
                Contact temp_cont = cont.clone(false, false, false, false);
                temp_cont.AccountId = acc.Id;
                cont_l.add(temp_cont);
            }
        update cont_l;
    }
}
@isTest
public class AddPrimaryContactTest {
	@testSetup 
    static void setup() {
        List<Account> accounts = new List<Account>();
        // add 100 child accounts
        for (Integer i = 0; i < 100; i++) {
            if (i<50){
                accounts.add(new Account(
                name='Name'+i, BillingState = 'NY'
            ));
            }else{
                accounts.add(new Account(
                name='Name'+i, BillingState = 'CA'
            ));
            }
        }
        insert accounts;
    }
    
    static testmethod void testQueueable() {
        // query for test data to pass to queueable class
        Contact cont = new Contact();
        List<Account> accounts = [select id, name from account where name like 'Test Account%'];
        // Create our Queueable instance
        AddPrimaryContact updater = new AddPrimaryContact(cont, 'CA');
        // startTest/stopTest block to force async processes to run
        Test.startTest();        
        System.enqueueJob(updater);
        Test.stopTest();        
        // Validate the job ran. Check if record have correct parentId now
        System.assertEquals(50, [select count() from account where BillingState = :'CA']);
    }
}
```
18. Apex Scheduler
```
   # Example Code
global class RemindOpptyOwners implements Schedulable {
    global void execute(SchedulableContext ctx) {
        List<Opportunity> opptys = [SELECT Id, Name, OwnerId, CloseDate 
            FROM Opportunity 
            WHERE IsClosed = False AND 
            CloseDate < TODAY];
        // Create a task for each opportunity in the list
        TaskUtils.remindOwners(opptys);
    }
    
}
@isTest
private class RemindOppyOwnersTest {
    // Dummy CRON expression: midnight on March 15.
    // Because this is a test, job executes
    // immediately after Test.stopTest().
    public static String CRON_EXP = '0 0 0 15 3 ? 2022';
    static testmethod void testScheduledJob() {
        // Create some out of date Opportunity records
        List<Opportunity> opptys = new List<Opportunity>();
        Date closeDate = Date.today().addDays(-7);
        for (Integer i=0; i<10; i++) {
            Opportunity o = new Opportunity(
                Name = 'Opportunity ' + i,
                CloseDate = closeDate,
                StageName = 'Prospecting'
            );
            opptys.add(o);
        }
        insert opptys;
        
        // Get the IDs of the opportunities we just inserted
        Map<Id, Opportunity> opptyMap = new Map<Id, Opportunity>(opptys);
        List<Id> opptyIds = new List<Id>(opptyMap.keySet());
        Test.startTest();
        // Schedule the test job
        String jobId = System.schedule('ScheduledApexTest',
            CRON_EXP, 
            new RemindOpptyOwners());         
        // Verify the scheduled job has not run yet.
        List<Task> lt = [SELECT Id 
            FROM Task 
            WHERE WhatId IN :opptyIds];
        System.assertEquals(0, lt.size(), 'Tasks exist before job has run');
        // Stopping the test will run the job synchronously
        Test.stopTest();
        
        // Now that the scheduled job has executed,
        // check that our tasks were created
        lt = [SELECT Id 
            FROM Task 
            WHERE WhatId IN :opptyIds];
        System.assertEquals(opptyIds.size(), 
            lt.size(), 
            'Tasks were not created');
    }
}
RemindOpptyOwners reminder = new RemindOpptyOwners();
// Seconds Minutes Hours Day_of_month Month Day_of_week optional_year
String sch = '20 30 8 10 2 ?';
String jobID = System.schedule('Remind Opp Owners', sch, reminder);
   # OR another way: Setup -> Apex Classes -> Schedule Apex -> name, *, weekly, dates,-> Save
   # remember
   . 100 scheduled Apex jobs/time
   . Extreme care for adding scheduler in a trigger (limit)
   . sync WS callouts not supported from scheduled Apex
   # Exercise Code
global class DailyLeadProcessor implements Schedulable {
	global void execute(SchedulableContext ctx) {
        List<Lead> leads_l = [SELECT Id, LeadSource FROM Lead WHERE LeadSource = :'' Limit 200];
        for (Lead l : leads_l){
            l.LeadSource = 'Dreamforce';
        }
        update leads_l;
    }
}
@isTest
public class DailyLeadProcessorTest {
	@testSetup 
    static void setup() {
        List<Lead> leads = new List<Lead>();
        for (Integer i = 0; i < 200; i++) {
            
                leads.add(new Lead(
                LastName='Lead '+i, 
                Company='Company'+i, Status='Status'+i
            ));
            
        }
        insert leads;
    }
    static testmethod void testScheduledJob() {
         String CRON_EXP = '0 0 0 15 3 ? 2022';
        Test.startTest();
        // Schedule the test job
        String jobId = System.schedule('ScheduledApexTest',
            CRON_EXP, 
            new DailyLeadProcessor());         
        // Verify the scheduled job has not run yet.
        
        Test.stopTest();
        System.assertEquals(200, 
            [select Id, LeadSource from Lead where LeadSource = :'Dreamforce'].size());
    }
}
```
19. Monitoring Apex jobs
```
   Setup -> Apex jobs/Apex Flex Queue
   # SOQL to monitor queued jobs
   AsyncApexJob jobInfo = [SELECT Status, NumberOfErrors
    FROM AsyncApexJob WHERE Id = :jobID]; //jobID from System.enqueueJob
   # Flex Queue -> Batch Job Queue -> running 5 jobs at the same time when enough resource
   # Monitoring Scheduled jobs
   CronTrigger ct = [SELECT TimesTriggered, NextFireTime FROM CronTrigger WHERE Id = :jobID]; //jobID from System.schedule
global class DoAwesomeStuff implements Schedulable {
    global void execute(SchedulableContext sc) {
        // some awesome code
        CronTrigger ct = [SELECT TimesTriggered, NextFireTime FROM CronTrigger WHERE Id = :sc.getTriggerId()];
    }
}
CronTrigger job = [SELECT Id, CronJobDetail.Id, CronJobDetail.Name, CronJobDetail.JobType FROM CronTrigger ORDER BY CreatedDate DESC LIMIT 1];
CronJobDetail ctd = [SELECT Id, Name, JobType FROM CronJobDetail WHERE Id = :job.CronJobDetail.Id];
SELECT COUNT() FROM CronTrigger WHERE CronJobDetail.JobType = '7’
```
20. Event-driven
```
   Platform event record: a sObject, not viewable, cannot be edited, but not deleted
   # 3 types: 
   . Platform Event: like generic event, but can send record data
   . PushTopic event (client receiving msg for record change), 
   . generic event: arbitrary message, not have to tie to record
   # Platform event usage
   . send & receive custom event data (Define event schema as typed fields)
   . pub/sub in Apex
   . about SF platform
   . Publish declaratively using Process Builder and Flow Builder
   # How to set up/publish a platform event?
   1) Define&publish event: Setup -> Platform events -> New Platform Event ->save->Custom Fields&Relationships New;
   2) Sf Platform events stored for 24 hours. Retrieve Stored events in API CometD, not in Apex; API name __e
   3) Publish event by Apex/Process Builder/Flow Builder if using it internal; external usage by SF APIs published
      Using SF API: /services/data/v40.0/sobjects/Cloud_News__e/
	     body: {
                  "Location__c" : "Mountain City",
                  "Urgent__c" : true,
                  "News_Content__c" : "Lake Road is closed due to mudslides."
               }
   
// Create an instance of the event and store it in the newsEvent variable
Cloud_News__e newsEvent = new Cloud_News__e(
           Location__c='Mountain City', 
           Urgent__c=true, 
           News_Content__c='Lake Road is closed due to mudslides.');
// Call method to publish events
Database.SaveResult sr = EventBus.publish(newsEvent);
// Inspect publishing result 
if (sr.isSuccess()) {
    System.debug('Successfully published event.');
} else {
    for(Database.Error err : sr.getErrors()) {
        System.debug('Error returned: ' +
                     err.getStatusCode() +
                     ' - ' +
                     err.getMessage());
    }
}
   # multiple events published
// List to hold event objects to be published.
List<Cloud_News__e> newsEventList = new List<Cloud_News__e>();
// Create event objects.
Cloud_News__e newsEvent1 = new Cloud_News__e(
           Location__c='Mountain City', 
           Urgent__c=true, 
           News_Content__c='Lake Road is closed due to mudslides.');
Cloud_News__e newsEvent2 = new Cloud_News__e(
           Location__c='Mountain City', 
           Urgent__c=false, 
           News_Content__c='Small incident on Goat Lane causing traffic.');
// Add event objects to the list.
newsEventList.add(newsEvent1);
newsEventList.add(newsEvent2);
// Call method to publish events.
List<Database.SaveResult> results = EventBus.publish(newsEventList);
// Inspect publishing result for each event
for (Database.SaveResult sr : results) {
    if (sr.isSuccess()) {
        System.debug('Successfully published event.');
    } else {
        for(Database.Error err : sr.getErrors()) {
            System.debug('Error returned: ' +
                        err.getStatusCode() +
                        ' - ' +
                        err.getMessage());
        }
    }       
}
   # limits
   . The allOrNoneHeader API header is ignored when you publish platform events through the API
   . The Apex setSavepoint() and rollback() Database methods aren’t supported with platform events.
   
   # Subscribe Events (Triggers, processes, flows)
   # Subscribe Events with Trigger
   . not need listen to the channel
   . only support after insert Trigger
   . not like on other objects, trigger on event only execute once for the same transaction
   . Automated Process entity as a user, Developer Console can not see unless adding a trace flag entry
   # How to add a flag entry for Automated Process entity
   Setup -> Debug Logs -> New -> Automated Process type, choose dates, * search level -> save
   # Notes about platform event trigger
   . a trigger can receive a batch of events at once, order in the batch
   . Asynchronous trigger execution. might delay
   . Automated Process System user, not a common user
   . Apex Governor limits & Apex Trigger Limitations
   # Test Event Trigger Example code
// Trigger for listening to Cloud_News events.
trigger CloudNewsTrigger on Cloud_News__e (after insert) {    
    // List to hold all cases to be created.
    List<Case> cases = new List<Case>();
    
    // Get queue Id for case owner
    Group queue = [SELECT Id FROM Group WHERE Name='Regional Dispatch' AND Type='Queue'];
       
    // Iterate through each notification.
    for (Cloud_News__e event : Trigger.New) {
        if (event.Urgent__c == true) {
            // Create Case to dispatch new team.
            Case cs = new Case();
            cs.Priority = 'High';
            cs.Subject = 'News team dispatch to ' + 
                event.Location__c;
            cs.OwnerId = queue.Id;
            cases.add(cs);
        }
   }
    
    // Insert all cases corresponding to events received.
    insert cases;
}
@isTest
public class PlatformEventTest {
    @isTest static void test1() {
        // Create test event instance
        Cloud_News__e newsEvent = new Cloud_News__e(
            Location__c='Mountain City', 
            Urgent__c=true, 
            News_Content__c='Test message.');
        
        Test.startTest();
        // Call method to publish events
        Database.SaveResult sr = EventBus.publish(newsEvent);
        
        Test.stopTest();
        
        // Perform validation here
        // Verify that the publish was successful
        System.assertEquals(true, sr.isSuccess());
        // Check that the case that the trigger created is present.
        List<Case> cases = [SELECT Id FROM Case];
        // Validate that this case was found.
        // There is only one test case in test context.
        System.assertEquals(1, cases.size());
    }
}
   # Also, like publishing Event, we can use Process and Flow to subscribe Event
   # subscribe platform event with CometD (scalable HTTP-based event routing bus)
   # EMP connector to access CometD (AJAX push pattern with protocol Comet)
// Connect to the CometD endpoint
    cometd.configure({
               url: 'https://<Salesforce_URL>/cometd/45.0/',
               requestHeaders: { Authorization: 'OAuth <Session_ID>'}
    });
{
  "data": {
    "schema": "_2DBiqh-utQNAjUH78FdbQ", 
    "payload": {
      "CreatedDate": "2017-04-27T16:50:40Z", 
      "CreatedById": "005D0000001cSZs", 
      "Location__c": "San Francisco", 
      "Urgent__c": true, 
      "News_Content__c": "Large highway is closed due to asteroid collision."
    }, 
    "event": {
      "replayId": 2
    }
  }, 
  "channel": "/event/Cloud_News__e"
}
   # GET request: /vXX.X/event/eventSchema/Schema_ID
   # Retrieve the event schema: /vXX.X/sobjects/Platform_Event_Name__e/eventSchema
   # Exercise Code
trigger OrderEventTrigger on Order_Event__e (after insert) {
	// List to hold all cases to be created.
    List<Task> cases = new List<Task>();
       
    // Iterate through each notification.
    for (Order_Event__e event : Trigger.New) {
        if (event.Has_Shipped__c == true) {
            Task cs = new Task();
            cs.Priority = 'Medium';
            cs.Subject = 'Follow up on shipped order ' + event.Order_Number__c;
            cs.OwnerId = event.CreatedById;
            cases.add(cs);
        }
   }
    
    // Insert all cases corresponding to events received.
    insert cases;
}
```
21. SF connect
```
   # when to use SF connect
   . a large amount of data don't want to be copied into SF
   . small data at any one time
   . need real-time access to the latest data
   . store data in cloud ro back-office system, but want display or access data
   # 3 types
   OData 2.0 adapter / OData 4.0 adapter
   Cross-org adapter: use the standard Lightning Platform REST API.
   Custom adapter created via Apex Connector Framework
   # How to set up Ext Integration with SF Connect
   1) Create the external data source
   2) Create the external objects and fields
   3) Define relationships for the external objects
   4) Enable user access to external objects and fields
   5) Set up user authentication (Named Principal, Per User)
   # install Test Package (Admins only) -> Set Customer IDs
   https://login.salesforce.com/packaging/installPackage.apexp?p0=04tE00000001aqG
   # Create external data source
   Setup -> External Data Source -> New -> name, OData 2.0, URL -> save
   # Create external object
   Setup -> External Data Source -> Validate and Sync -> choose fields -> Sync
   __x External Object suffix
   # Create custom tab to access external object
   Setup -> Tabs -> New -> change * and save
   # 3 types of External Objects relationships
   , Lookup: child object(standard/custom/external), parent obj(standard/custom), ext data contain SF ID (Yes)
   , External lookup: child obj(standard/custom/external), parent obj(external), No
   , Indirect lookup: child obj(external), parent obj(standard/custom), No
   # configure Indirect lookup relationship (External Order to Account)
   # Enable Chatter for External Data
   Setup/Feed Tracking/Feed Tracking/Orders/Enable Feed Tracking/Save (The Follow button for Chatter feed)
```
22. SF Development 
```
   # 3 types SF development
   . Change set development
   . Org development
   . Package development
   # ALM process (ALM: Application Lifecycle Management)
   1) Plan Release
   2) Develop
   3) Test
   4) Build release
   5) Test Release
   6) Release
   # Release Management process (Patch, Minor, Major)
   # Limitation for Change sets: only cannot delete fields, but adding fields
   # Package development: can include many project in one container
   # Prepare Release Environment: Develop and test steps -> Build release -> Test release -> Release
   *** have to manually migrate a change if the changed component not supported in Metadata API yet.
   # Authorize a Deployment Connection
   Setup -> Deployment Settings -> Edit -> Allow Inbound Changes -> Save
   # How to modify contents of uploaded change set?
   Clone the change set -> Modify the clone -> upload modified clone
   # Test integration Environment and deploy changes
   Clone a change set, validate a change set, Deploy a change set
   # Whole process current
   Dev sandbox -> Dev Pro sandbox -> Full sandbox -> production
   # Org development: pain point - error since difference between different environments
   Tools: Change list, 
   Deployment run list (non deployed metadata, like profile and permission set assignments),
   Project management tools: Agile accelerator, Jira
   # SFDX command to retrieve the new custom object and custom field
   sfdx force:source:retrieve --metadata CustomObject:Language_Course_Instructor__c,CustomField:Language_Course__c.Course_Instructor__c
   # Deploy change to Sandbox
   sfdx force:source:deploy --metadata CustomObject:Language_Course_Instructor__c, \
      CustomField:Language_Course__c.Course_Instructor__c
```