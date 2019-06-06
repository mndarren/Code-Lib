# SF Notes (key: CRM)
=======================<br>
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