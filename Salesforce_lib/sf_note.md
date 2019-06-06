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