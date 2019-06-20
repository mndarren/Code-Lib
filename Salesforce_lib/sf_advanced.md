# SF Trailhead Advanced
============================<br>
1. Injection Vulnerability Prevention
```
   # Security Review by SF security expert
   . Not just for Admins, but also for Develpers
   . Security Review -> Release AppExchange
   . Tool: Kingdom Management
   # XSS - Cross-Site Scripting
   # Types of XSS: 
   Stored XSS: malicious input permanently stored on a srever
   Reflected XSS: reflected back to the user on response page
   DOM-based XSS: modifying the web page's document object model (DOM)
   # Impact of XSS
   . Arbitrary requests: get victim to the web server
   . Malware download
   . Log keystrokes
   # Defending
   . Input Filtering: Blacklisting, Whitelisting (more secure)
   . Output Encoding
```
2. XSS
```
<a onmouseover="alert(\'XSS Attack\')"> any text</a>
   # How to get the challenge done?
   Manage my hands-on orgs -> connect the Kingdom Management Org -> done
   # Built-in XSS protections
   . Automatic HTML Encoding: '<script>' => &lt;script&gt;
   . Disable Automatic HTML encoding for some special use case
<apex:outputText escape="false">
    Hello {!$CurrentPage.parameters.userName}
</apex:outputText>
   # not fully sufficient for built-in HTML encoding
   # Example 1 ( multiple parsing contexts)
<div onclick=”console.log(‘{!$CurrentPage.parameters.userInput}’)”>Click me!</div> 
   # Example 2 (inserting merge fields into JavaScrip)
<script>
    var x = ‘{!$CurrentPage.parameters.userInput}’; // vulnerable to XSS
</script>
   # Example 3 (older browsers inserting merge field into CSS)
<style>
    .foo {
        Color: #{!CurrentPage.parameters.userInput}; // vulnerable to XSS
    }
</style>
   # Prevent XSS in Lightning Platform App FOR THE 3 ISSUES
   # 3 Encoding methods: JSENCODE(), HTMLENCODE(), JSINHTMLENCODE() OR JSENCODE(HTMLENCODE()
   # Example for JSENCODE()
<script>
    var x = '{!JSENCODE($CurrentPage.parameters.userInput)}';
</script>
   # Example 2 HTMLENCODE()
<apex:outputText escape="false" value="<i>Hello {!HTMLENCODE(Account.Name)}</i>" />
<apex:outputText value="Welcome, <b>{!HTMLENCODE($CurrentPage.Parameters.user)}</b>!" escape="false"/>
   # Example 3 for JS and HTML
<div id="test"></div>
<script>
    document.querySelector('#test').innerHTML='Howdy ' + '{!JSENCODE(HTMLENCODE(Account.Name))}';
</script>
   # Issue code
<div onclick="console.log('{!JSINHTMLENCODE(Account.Name)}')">Click me!</div>
   # solution: automatically run HTMLEncode()
<div onclick="console.log('{!JSENCODE(Account.Name)}')">Click me!</div>
   # Using a wrong ENCODE() can be dangerous!
   # encoding within the controller is strongly discouraged. Should in VF. 
   # Lightning ESAPI package can be used for more security encoding.
   # Example for ESAPI
Title: <apex:outputText value="{!title}" escape="false" /><br/>
title = '<b>' + ESAPI.encoder().SFDC_HTMLENCODE(person.Title__c) +'</b>';  // in Apex
<script>
    document.write('{!JSINHTMLENCODE(sampleMergeField4)}');
</script>
```
3. SOQL Injection
```
   # Diff between SOQL and SQL
   . No INSERT, UPDATE or DELETE statements, only SELECT
   . No command execution
   . No wild cards for fields; all fields must be explicitly typed
   . No JOIN statement; however, you can include information from parent objects like Select name, phone, account.name from contact
   . No UNION operator
   . Queries cannot be chained together
   # SOQL Injection Examples 1
whereClause += 'Title__c like  \'%'+textualTitle+'%\' ';
whereclause_records = database.query(query+' where'+whereClause);

%' and Performance_rating__c<2 and name like '% 
Title__c like '% %' and Performance_rating__c<2 and name like '% %'';

%' and name like 'Amanda% 
Title__c like '%'+textualTitle+'%'
   # Example 2
if(textualAge!=null){
    whereClause+='Age__c >'+textualAge+'';
    whereclause_records = database.query(query+' where'+whereClause);
}
22 limit 1
   # Exercise input
%' and Nobles_Only__c=true and name like '%
   # SOQL Injection Prevention
   . Static queries with bind variables
   . String.escapeSingleQuotes()
   . Type casting
   . Replacing characters
   . Whitelisting
   # Static Query
String query = ‘select id from contact where firstname =\’’+var+’\’’;
queryResult = Database.execute(query);
queryResult = [select id from contact where firstname =:var];
   # Static Query limitation
   . The search string in FIND clauses.
   . The filter literals in WHERE clauses.
   . The value of the IN or NOT IN operator in WHERE clauses, enabling filtering on a dynamic set of values. Note that this is of particular use with a list of IDs or strings, though it works with lists of any type.
   . The division names in WITH DIVISION clauses.
   . The numeric value in LIMIT clauses.
   . The numeric value in OFFSET clauses.
   # Typecasting with issue
public String textualAge {get; set;}
[...]
whereClause+='Age__c >'+textualAge+'';
whereclause_records = database.query(query+' where '+whereClause);
   # solution
whereClause+='Age__c >'+string.valueOf(textualAge)+'';
   # Escaping Single Quotes with issue
whereClause += 'Title__c like  \'%'+textualTitle+'%\' ';
whereclause_records = database.query(query+' where '+whereClause);
   # solution
whereClause += 'Title__c like  \'%'+ string.escapeSingleQuotes(textualTitle)+'%\' ';
   # Replacing Characters with issue
String query = ‘select id from user where isActive=‘+var;
   # attack
true AND ReceivesAdminInfoEmails=true
   # solution
String query = 'select id from user where isActive='+var.replaceAll('[^\w]','');
   # Whitelist with issue
public String objectName {get;set;}
[...]
string obj = ApexPages.currentPage().getParameters().get('object');
if(obj != null){ 
    string query = 'select id, name from '+obj+' limit 10';
    [...]
}
   # solution
if(obj=='...'||obj =='...'||obj =='...'){
   # Exercise Code
public class Prevent_SOQL_Injection_Challenge {

    public string textOne {get; set;}
    public string textTwo {get; set;}
    public string comparator {get; set;}
    public Integer numberOne {get; set;}

    public List<Supply__c> whereclause_records {get; set;}


    public PageReference stringSearchOne(){
        string query = 'SELECT Id,Name,Quantity__c,Storage_Location__c,Storage_Location__r.Castle__c,Type__c FROM Supply__c';
        string whereClause = '';

        if(textOne != null && textOne!=''){
                whereClause += 'name like  \'%'+string.escapeSingleQuotes(textOne)+'%\' ';
        }

        if(whereClause != ''){
            whereclause_records = database.query(query+' where '+whereClause+' Limit 10');
        }

        return null;
    }


    public PageReference stringSearchTwo(){
        string query = 'SELECT Id,Name,Quantity__c,Storage_Location__c,Storage_Location__r.Castle__c,Type__c FROM Supply__c';
        string whereClause = '';

        if(textTwo != null && textTwo!=''){
                whereClause += 'Storage_Location__r.name like  \'%'+string.escapeSingleQuotes(textTwo)+'%\' ';
        }

        if(whereClause != ''){
            whereclause_records = database.query(query+' where '+whereClause+' Limit 10');
        }

        return null;
    }


    public PageReference numberSearchOne(){
        string query = 'SELECT Id,Name,Quantity__c,Storage_Location__c,Storage_Location__r.Castle__c,Type__c FROM Supply__c';
        string whereClause = '';

        if(numberOne != null && comparator != null && (comparator == '<' || comparator == '>' || comparator == '=')){
            whereClause += 'Quantity__c '+comparator+' '+string.valueOf(numberOne)+' ';
        }

        if(whereClause != ''){
            whereclause_records = database.query(query+' where '+whereClause+' Limit 10');
        }

        return null;
    }

}
```
4. Open Redirect (Arbitrary redirect)
```
   # Example
https://www.vulnerable-site.com?startURL=https://www.good-site.com
https://www.vulnerable-site.com?startURL=https://www.evil-hacker.com
/apex/open_redirect_basics_demo?onSave=https://www.youtube.com/watch?v=dQw4w9WgXcQ
https://c.na3.visual.force.com/apex/open_redirect_basics_demo?onSave=https://www.youtube.com/watch?p=dQw4w9WgXcQ
   # redirect to a fake login page (lost credential)
   # Exercise Code
   public PageReference save(){
        PageReference savePage;
        if (Schema.SObjectType.Personnel__c.isUpdateable()){
            try{
                update unassigned;
                String completion = ApexPages.currentPage().getParameters().get('completion');
                //completion = 'https://www.google.com';
                savePage = new PageReference('https://www.google.com');
                savePage.setRedirect(true);
                validate(savePage);
                return savePage;
            }catch (exception e){
                ApexPages.addmessage(new ApexPages.message(ApexPages.severity.ERROR, 'Unable to update requisitions.  Exception: ' + e.getMessage()));
                return null;
            } 
        }else{
            ApexPages.addmessage(new ApexPages.message(ApexPages.severity.ERROR, 'You do not have permission to update requisitions'));
            return null;
        }
    }
   # Sf Uses Redirects
   . startURL: reload page
   . retURL: Back button
   . saveURL: Save Button
   . cancelURL: Cancel Button
   # SF protection is case-sensitive. (retURL -> returl => works) but ERROR
   Apex is case INSENSITIVE
   # Exercise Code
   Change Apex retURL to returl
https://amazing-appy-355953-dev-ed.lightning.force.com/lightning/n/Standard_Redirect_Protections_Challenge?returl=https%3A%2F%2Fwww.google.com
   # Strategies for Mitigating Open Redirect
   . Hardcode Redirects
   . Force only local redirects
   . Whitelist redirects
   # Hardcode Redirects Example
savePage = new PageReference(‘/home/home.jsp’);
savePage.setRedirect(true);
return savePage;
   # Force local redirects only Example
   public PageReference save(){
    PageReference savePage;
    if (Schema.SObjectType.Personnel__c.isUpdateable()){
    try{
        update unassigned;
        String completion = ApexPages.currentPage().getParameters().get('onSave');
        if(completion.startsWith('/')){
            completion=completion.replaceFirst('/','');
        }
        savePage = new PageReference('/'+completion);
        savePage.setRedirect(true);
        return savePage;
        return savePage;

            }catch (exception e){
                ApexPages.addmessage(new ApexPages.message(ApexPages.severity.ERROR, 'Unable to update requisitions.  Exception: ' + e.getMessage()));
                return null;
            } 
        }else{
            ApexPages.addmessage(new ApexPages.message(ApexPages.severity.ERROR, 'You do not have permission to update requisitions'));
            return null;
        }
    }
	# https://c.[yourinstance].visual.force.com/www.google.com does not exist
	# Whitelist Redirects (for not local domains) Example
	public PageReference save(){
    PageReference savePage;
    if (Schema.SObjectType.Requisition__c.isUpdateable()){
    try{
        update requisitions;
        String onsave = ApexPages.currentPage().getParameters().get('onSave');
                        
        URL currentURL = New URL('https://' + ApexPages.currentPage().getUrl());
        Set<String> whiteListedDomains = new Set<String>();
        whiteListedDomains.add(currentURL.getHost());
        whiteListedDomains.add('www.salesforce.com');
        whiteLIstedDomains.add('www.google.com');
        if( onsave == NULL || !whiteListedDomains.contains(New URL(onsave).getHost())){
            onsave = '/home/home.jsp';
        }
        savePage = new PageReference(onsave);
        savePage.setRedirect(true);
        return savePage;
            }catch (exception e){
                ApexPages.addmessage(new ApexPages.message(ApexPages.severity.ERROR, 'Unable to update requisitions.  Exception: ' + e.getMessage()));
                return null;
            } 
        }else{
            ApexPages.addmessage(new ApexPages.message(ApexPages.severity.ERROR, 'You do not have permission to update requisitions'));
            return null;
        }
    }
	
```
5. CSRF (Cross-Site Request Forgery
```
   # Open one website to attack another
   # prevent it using a unique random token (per request per user)
   # SF enabled CSRF protection by default
   # SF CSRF Example code
 
<apex:page controller="CSRF_Demo" sidebar="false" action="{!performInitAction}" tabStyle="CSRF_Demo__tab"> 
 
public void performInitAction() {
    try {
        String id = ApexPages.currentPage().getParameters().get('UserId');
        [...]
        Personnel__c obj = [select id, Name FROM Personnel__c WHERE id = :id];
        Resource_Type__c rt= [select id from Resource_Type__c where Name='Knight'][0];
        if(Personnel__c.sObjectType.getDescribe().isUpdateable()) {
            obj.Type__c=rt.id; update obj;
        }
    
    [...]
    } 
   # Reason: This action executes before the rest of the page loads, it bypasses the default CSRF protections
   # solution: remove any state-changing operations from apex:page action handlers
<apex:page controller="CSRF_Mitigation_Demo"sidebar="false" tabStyle="CSRF_Mitigation_Demo__tab">
# replace
<apex:outputLink value="/apex/CSRF_Demo?UserID={!person.Id}"> Knight This Squire </apex:outputLink>
# with
<apex:commandLink value=”Knight This Squire” action=”{!knightSquire}”>
    <apex:param name="accId" value="{!person.id}" assignTo="{!currPerId}"/>
</apex:commandLink>
   # Exercise Code
<apex:commandLink value="Approve Request" action="{!approveReqNOCSRF}">
    <apex:param name="accId" value=" {!req.id}" assignTo="{!approve}"/>
</apex:commandLink>
```
6. Clickjacking (Cursorjacking)
```
   # click to one object, but to another
   . Trick user enabling webcam and microphone through Flash
   . Trick user public profile
   . Download and run malware to control computer
   . Make user to follow someone on Twitter/Facebook/play YouTube video/Sharing and liking links on Facebook
   . Click Google AdSense ads to generate pay-per-click revenue
   # Clickjacking prevention
   . Use Frame-Busting Script (including script on every site page)
if (top != self) top.location = self.location;
if (top.location != location) top.location = self.location;
if (top.location != location) top.location.href = document.location.href;
   . Use X-Frame Options (using HTTP header included in IE8)
     DENY: Preventing page from loading in a frame completely
	 SAMEORIGIN: Allow framing only if the origin is the same as the content
	 ALLOW-FROM: only from a specific URL
   . SF clickjacking protection (frame-busting scripts and X-frame Options header as standard in SF)
```
7. Insecure Remote Resource Interaction
```
   # Break trust
   . Including remote resources
     * mixed content vulnerabilities (both HTTP (insecure) and HTTPS)
	 * include external resources safely (SF "static resource": archives(.zip and .jar), images, style sheets, javascript)
   . Sending data to remote resources
   # How to use Static Resources
   # Issue for including remote resources
<apex:image value="http://www.castles.org/images/sd2_small.jpg"/>
   # solution reference the file locally
<apex:image url=”{!$Resource.castle}”/>
   # leaks: Web server and proxy logs, Browser, URL referrer headers, Printed PDF
   . Tip: use POST always, not GET, and put all sensitive info in body of request
   # Exercise Code - replace all URL in code
<apex:image url="{!URLFOR($Resource.Challenge_Resources, 'Challenge_Resources/Castle.png')}" />                                                                                                                                    
```
8. Shield Platform Encryption (Dev Edition has it, but other Editions should buy)
```
   Field-level security to decide who can access which data at what time
   Tool Security Health Check and Event Monitoring can monitor the activities.
   # Tenant Secrets and Master Secrets are the keys for keys
   # Mater Secret will be generated 3 times / year. SF security officer access the HSM to generate new Master secret per SF release.
   # HSM - Hardware Security Module
   # Tenant secret can be created as often as you want. Both are for encode and decode your data.
   # Diff between Classic encryption and Shield Platform Encryption
   . Classic encryption: included in SF license; 128-bit AES key, 
   . Shield Platform Encryption: 256 AES key, additional fee.
   # How to enable it
   View Setup and Configuration for the first Admin
   second Admin: not for these 2, but others permissions
   # How to assign permission and generate Tenant Key
   Setup -> Permission Sets -> new, Label (Key Manager) -> Save -> system Permissions -> 
            Edit(enable Customize Application and Manage Encryption Keys) -> save ->
			Setup -> users -> choose user -> Permission Set Assignments -> Edit Assignments -> Key manager -> save
   Setup -> Platform Encryption -> Key Management -> Data in SF and Generate Tenant Secret
   Setup -> Platform Encryption -> Encryption Policy
   # Encrypt Only where Necessary
   . Define a threat model for the org
   . Not all data is sensitive
   . Create a data classification scheme early
   *** Create a strategy early for backing up and archiving keys and data
   *** Grant "Manage Encryption Keys" permission to authorized users only.
   **  Understand that encryption applies to all users, regardless of permissions
   # Other security features
   . Assign non-encryption related permissions to control who sees what info
   . Use roles and profiles to control access to sensitive data
   . Use field-level security settings, page layout settings and validation rules to control who see which data
```
9. Transaction Security
```
   # Using Transaction Security requires purchasing a SF shield or SF shield Event Monitoring add-on subscription.
   *** Allow user to have multiple login sessions can be a security risk
   Available - any policy you create
   Enabled - Admin turned on
   Triggered - a policy that's been activated.
   # Transaction Security consists of events, notifications, and actions.
   . Lock out specific geographical areas
   . Securely access confidential data
   # Enable it
   Setup -> Transaction Security Policies -> enable
   # interface for Transaction Security =  PolicyCondition
   # Example Code
global class MyLeadExportCondition implements TxnSecurity.PolicyCondition {
  public boolean evaluate(TxnSecurity.Event e) {
    // The event data is a Map<String, String>.
    // We need to call the valueOf() method on appropriate data types to use them in our logic.
    Integer numberOfRecords = Integer.valueOf(e.data.get('NumberOfRecords'));
    Long executionTimeMillis = Long.valueOf(e.data.get('ExecutionTime'));
    String entityName = e.data.get('EntityName');

    // Trigger the policy only for an export on leads, where we are downloading
    // more than 2000 records or it took more than 1 second (1000ms).
    if('Lead'.equals(entityName)){
      if(numberOfRecords > 2000 || executionTimeMillis > 1000){
        return true;
      }
    }

    // For everything else don't trigger the policy.
    return false;
  }
}
   # Test Policy Code
/**
 * Test for default MyDataLoaderLeadExportCondition provided by Salesforce
 * 
*/
@isTest
public class MyDataLoaderLeadExportConditionTest {
    /*
     * Test to check if policy gets triggered when user exports 750 Leads
    */
  public static testMethod void testLeadExportTriggered() {
    Map<String, String> eventMap = getEventMap(String.valueOf(750));
    Organization org = getOrganization();
    User user = createUser();
      
    TxnSecurity.Event e = createTransactionSecurityEvent(org, user, eventMap) ;
    /* We are unit testing a PolicyCondition that triggers
       when an event is generated due to high NumberOfRecords */
    MyDataLoaderLeadExportCondition dataLoaderLeadExportCondition = new MyDataLoaderLeadExportCondition();
    /* Assert that the condition is triggered */
    System.assertEquals(true, dataLoaderLeadExportCondition.evaluate(e));
   }
    /*
     * Test to check if policy doesn't get triggered when user exports 200 Leads
    */
  public static testMethod void testLeadExportNotTriggered() {
    Map<String, String> eventMap = getEventMap(String.valueOf(200));
    Organization org = getOrganization();
    User user = createUser();
       
    TxnSecurity.Event e = createTransactionSecurityEvent(org, user, eventMap) ;
    /* We are unit testing a PolicyCondition that does not trigger
       an event due to low NumberOfRecords  */
    MyDataLoaderLeadExportCondition dataLoaderLeadExportCondition = new MyDataLoaderLeadExportCondition();
      
    /* Assert that the condition is NOT triggered */
    System.assertEquals(false, dataLoaderLeadExportCondition.evaluate(e));
  }
    /*
     * Create an example user
     */
    private static User createUser(){
        Profile p = [select id from profile where name='System Administrator'];
        String ourSysAdminString = p.Id; 
        
        User u = new User(alias = 'test', email='user@salesforce.com',
            emailencodingkey='UTF-8', lastname='TestLastname', languagelocalekey='en_US',
            localesidkey='en_US', profileid = p.Id, country='United States',
            timezonesidkey='America/Los_Angeles', username=generateRandomUsername(7));
        insert u;
        return u;
    }
    /**
     * Generates a random username of the form “[random]@[random].com”, where the 
     * length of the random string is the given Integer argument. For example, with 
     * an argument of 3, the following random username may be produced: 
     * “ajZ@ajZ.com”.
     */ 
    private static String generateRandomUsername(Integer len) {
        final String chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
        String randStr = '';
        while (randStr.length() < len) {
           Integer idx = Math.mod(Math.abs(Crypto.getRandomInteger()), chars.length());
           randStr += chars.substring(idx, idx+1);
        }
        return randStr + '@' + randStr + '.com'; 
    }

    /*
     * Returns current organization
     */
    private static Organization getOrganization(){
        return [SELECT id, Name FROM Organization];
    }
      
    /*
     * Adds NumberOfRecords and example ExecutionTime
     */ 
    private static Map<String, String> getEventMap(String numberOfRecords){
        Map<String, String> eventMap = new Map<String, String>();
        eventMap.put('NumberOfRecords', numberOfRecords);
        eventMap.put('ExecutionTime', '30');
        eventMap.put('EntityName', 'Lead');
        return eventMap;
    }
    
    /*
     * Create a TxnSecurity.Event instance
     */ 
    private static TxnSecurity.Event createTransactionSecurityEvent(Organization org, User user, Map<String, String> eventMap){
        TxnSecurity.Event e = new TxnSecurity.Event(
             org.Id /* organizationId */,
             user.Id /* userId */,
             'Lead' /* entityName */ ,
             'DataExport' /* action */,
             'Lead' /* resourceType */,
             '000000000000000' /* entityId */,
              Datetime.newInstance(2016, 9, 22) /* timeStamp */,
              eventMap /* data - Map containing information about the event */ );
        return e;
    }
}
```
10. Large Data Volume (LDV) PERFORMANCE
```
   # why? LDV => sluggish performance, slower queries, slower search, slower sandbox refreshing.
   # solution: accommodating LDV up front, designing your data model to build scalability in from the get-go
   # Data Skew: carefully architecting record ownership to avoid data skew
     . 3 types: account data skew, ownership skew, lookup skew
   # Account Data Skew (Example: lots of unassigned Contacts parked under one Account named "Unassigned"
     . Reasons: Record locking (update Contacts related one Account), Sharing Issues (Change owner of Account will check all Contacts) 
   # Ownership Skew (one user has lots of same type objects)
     . Reason: change ownership of object must remove sharing from all old owners and parents and shared users
	 . solution: if cannot avoid ownership skew -> take user and related records away from the role hierarchy and sharing rules
   # Lookup Skew (lots of records associated with a single record in the lookup object)
   *** Strategy 1: design a data model for your org (<10000 children/parent)
   *** Strategy 2: Using external objects (avoid both storing large data in your org and performance issue)
                   SF Connect for external data source; File Connect for third-party content systems
   # External Object lookup (standard lookup relationship for external object with recordID; 2 special types of lookup relationships)
     . 2 special lookup: external lookups & indirect lookups
	 . external parent => External Lookup, internal parent => Lookup (external obj with record ID) OR Indirect Look
   # Data Search
     . indexed most test fields. For data to be searched, it must first be indexed.
	 . the searched result set used to query the related records
   # Queries solution: query building. to Design selective list views, reports, and SOQL queries, and to understand query optimization
     . SOQL is for query; SOSL is for search.
	 . SOSL limit is 2,000; SOQL is 50,000
	 . Use SOSL when a specific distinct term that you know exists within a field
	 . Query Optimizer is diff from DB system optimizer. 
	 . Query Optimizer maintains a table of statistics about distribution of data in each index.
   # Batch Apex: the best way to query a large data sets (50 M records asynchronously) not work for synchronous
   # Bulk Query: can retrieve up to 15 GB of data (divided into fifiteen 1 GB files
     . Bulk API support query and queryAll operations. queryAll includes deleted records, archived Task and Event records
	 . 3 types for Content-Type in the header: text/csv, application/xml, application/json.
	 . 2 minutes timeout limit to job fails with QUERY_TIMEOUT error
	 . 15 attempts to retrieve the result data (if exceeding 1 GB file limit or >10 minutes) to fail with Retrieve > 15 times Error
	   solution: PK Chunking header
	 . 7 days stored the successful bulk query result
   # Skinny Tables (Example: Account Standard fields Table + Account Custom Fields Table ~= Account skinny Table)
     . why faster? date 01/01/2018-12/31/2018 -> year=2018, exclude soft-deleted records
	 . SF platform synchronizes base objects and skinny table
	 . SF platform determine if using skinny table
	 . Most useful for millions of records.
	 . Can be created on custom objects, Account, Contact, Opportunity, Lead, and Case objects.
	 . side effects: might restrict or burden your business processes
	 . recreate skinny tables when adding one field to query, or alter field type.
	 . don't get copied to Sandbox from Production
	 . enable it - contact SF Customer Support
   # Loading Lean: only the data and configuration you need to meet your business-critical operations
     . Organization-wide sharing defaults: load data with Public Read/Write sharing model
	   if with a Private sharing model, system calculates sharing as records being added
	 . Complex object relationships. The more lookups the more check. so Load data then establish those relationships
	 . Sharing rules. ownership-based sharing and criteria-based sharing: the more the more sharing calculation
	   so load data and then set up sharing rules
	 . Workflow rules, validation rules and triggers. similar to the previous 2 items
   # NOT too Lean
     . Parent records with master-detail children (won't load children without parent)
	 . Record owners (owners should exist before load data
	 . Role hierarchy (with it, faster when loading portal accounts)
   # Bulk API vs. SOAP API
     . SOAP is for real-time client applications with not large data, bite-size chunks
	 . Bulk is for large data operation
   # Increase Speed by Suspending Events (Disable validation rules, Workflow rules, triggers)
     . Analyzing and preparing Data (clean data with rules before data loading, adding lookup relationships post-loading)
	 . Disable Events for loading (Edit status to inactive for Validation, Lead and Case assignment rules, Territory assignment rules
	 . To disable trigger, create a Custom Setting & a checkbox field and the related code
trigger setDefaultValues on Account (before insert, before update){
   Load_Settings__c s = Load_Settings__c.getInstance(UserInfo.GetUserID());
   if (s.Load_Lean__c) return; // skip trigger
   for (Account oAccount : trigger.new){
      oAccount.Status = 'Stage 1';
   }
}
     . Post-Processing
	   * Add lookup relationships between objects, roll-up summary fields to parent records, others
	   * Enhance records with foreign keys or other data to integration
	   * Enable triggers
	   * Turn validation, workflow and assignment rules back on
   # Data Delete and Extract
     . soft delete will stay in Recycle bin for 15 days
	 . hard delete can be enabled by SF admin
   # Chunking data: 100,000 record chunks by default, Max 250,000 (careful for Bulk API batch size)
   # using PK Chunking to handle extra-large data set extracts
     . Enable PK chunking when querying > 10 M records. it's a feature of Bulk API
	 . Sforce-Enable-PKChunking: chunkSize=50000 # in header
   # Truncation: a fast way to permanently remove all the records form a custom object
     . Truncation will permanently delete records, not move to Recycle Bin
	 . Cannot truncate standard objects and custom objects with lookup relationship
```
11. Apex Metadata API
```
   # Apex Metadata API -> Custom Setup UI (also can be used to create a Setup Wizard to guild to change Setup UI)
   # 2 top-level metaday types: page layouts and records of custom metadata types
   # Example code
Metadata.CustomMetadata customMetadata =  new Metadata.CustomMetadata();
customMetadata.fullName = 'MyNamespace__MetadataTypeName.MetadataRecordName';
Metadata.CustomMetadataValue customField = new Metadata.CustomMetadataValue();
customField.field = 'customField__c';
customField.value = 'New value';
customMetadata.values.add(customField);
   # limitations
   . only 2 metadata types
   . deleting metadata not supported
   . no API to track the status of deployment. can set up a callback to get status
   # Automated Metadata Updates for Multi-Org Deployment (Apex post install script)
   # Retrieve metadata synchronously, but deploy metadata asynchronously
   # Example code to update metadata
public class UpdatePageLayout {
    // Add custom field to page layout
    
    public Metadata.Layout buildLayout() {
        
        // Retrieve Account layout and section 
        List<Metadata.Metadata> layouts = 
            Metadata.Operations.retrieve(Metadata.MetadataType.Layout, 
            new List<String> {'Account-Account Layout'});
        Metadata.Layout layoutMd = (Metadata.Layout) layouts.get(0);
        Metadata.LayoutSection layoutSectionToEdit = null;
        List<Metadata.LayoutSection> layoutSections = layoutMd.layoutSections;
        for (Metadata.LayoutSection section : layoutSections) {
            
            if (section.label == 'Account Information') {
                layoutSectionToEdit = section;
                break;
            }
        }
        
        // Add the field under Account info section in the left column
        List<Metadata.LayoutColumn> layoutColumns = layoutSectionToEdit.layoutColumns;     
        List<Metadata.LayoutItem> layoutItems = layoutColumns.get(0).layoutItems;
        
        // Create a new layout item for the custom field
        Metadata.LayoutItem item = new Metadata.LayoutItem();
        item.behavior = Metadata.UiBehavior.Edit;
        item.field = 'AMAPI__Apex_MD_API_sample_field__c';  // with you namespace and field name
        layoutItems.add(item);
        
        return layoutMd;
    }
}
   # Callback example
public class PostInstallCallback implements Metadata.DeployCallback {
  
    public void handleResult(Metadata.DeployResult result,
        Metadata.DeployCallbackContext context) {
        
        if (result.status == Metadata.DeployStatus.Succeeded) {
            // Deployment was successful, take appropriate action.
            System.debug('Deployment Succeeded!');
        } else {
            // Deployment wasn’t successful, take appropriate action.
	    System.debug('Deployment Failed!');
        }
    }
}
   # Deployment Example code
public class DeployMetadata {
 
    // Create metadata container 
    public Metadata.DeployContainer constructDeploymentRequest() {
        
        Metadata.DeployContainer container = new Metadata.DeployContainer();
        
        // Add components to container         
        Metadata.Layout layoutMetadata = new UpdatePageLayout().buildLayout();
        container.addMetadata(layoutMetadata);
        return container;
    }
    
    // Deploy metadata
    public void deploy(Metadata.DeployContainer container) {
        // Create callback. 
        PostInstallCallback callback = new PostInstallCallback();
        
        // Deploy the container with the new components. 
        Id asyncResultId = Metadata.Operations.enqueueDeployment(container, callback);
    }
}
   # Deploy metadata script async
     *** the script is run automatically after the package is installed or updated.
   # Example code
public class PostInstallScript implements InstallHandler {
    
    // Deploy post-install metadata  
    public void onInstall(InstallContext context) {
        DeployMetadata deployUtil = new DeployMetadata();
        Metadata.DeployContainer container = deployUtil.constructDeploymentRequest();
        deployUtil.deploy(container);
    }
}
   # Exercise code
public class UpdateContactPageLayout {
	// Add custom field to page layout
    
    public Metadata.Layout addLayoutItem () {
        
        // Retrieve Account layout and section 
        List<Metadata.Metadata> layoutsList = 
            Metadata.Operations.retrieve(Metadata.MetadataType.Layout, 
            new List<String> {'Contact-Contact Layout'});
        Metadata.Layout layoutMetadata = (Metadata.Layout) layoutsList.get(0);
        Metadata.LayoutSection contactLayoutSection = null;
        List<Metadata.LayoutSection> layoutSections = layoutMetadata.layoutSections;
        for (Metadata.LayoutSection section : layoutSections) {
            
            if (section.label == 'Additional Information') {
                contactLayoutSection = section;
                break;
            }
        }
        
        // Add the field under Account info section in the left column
        List<Metadata.LayoutColumn> contactColumns = contactLayoutSection.layoutColumns;     
        List<Metadata.LayoutItem> contactLayoutItems = contactColumns.get(0).layoutItems;
        
        // Create a new layout item for the custom field
        Metadata.LayoutItem newField = new Metadata.LayoutItem();
        newField.behavior = Metadata.UiBehavior.Edit;
        newField.field = 'AMAPI__Apex_MD_API_Twitter_name__c';
        contactLayoutItems.add(newField);
        
        return layoutMetadata;
    }
}
   # layers: LayoutList -> SectionList -> Columns -> Fields
   # Automate Configuration Change (Great Thing)
   # Create a Custom Metadata Type by Setup (also we can do it by the previous way)
   Setup -> Custom Metadata Types -> new ...(VAT Rates) -> save
   Manage VAT Rates -> new -> Create VAT Rate Records
   # Create Controller and VF page
public class VATController {
    
    public final List<VAT_Rate__mdt> VATs {get;set;}
    final Map<String, VAT_Rate__mdt> VATsByApiName {get; set;}
    
    public VATController() { 
        VATs = new List<VAT_Rate__mdt>();
        VATsByApiName = new Map<String, Vat_Rate__mdt>();
        for (VAT_Rate__mdt v : [SELECT QualifiedApiName, MasterLabel, Default__c, Rate__c
                                FROM VAT_Rate__mdt]) { 
                                    VATs.add(v);
                                    VATsByApiName.put(v.QualifiedApiName, v);
                                }
    }
    
    public PageReference save() {        
        
        // Create a metadata container.
        Metadata.DeployContainer container = new Metadata.DeployContainer();
        List<String> vatFullNames = new List<String>();
        for (String recordName : VATsByApiName.keySet()) {
            vatFullNames.add('VAT_Rate.' + recordName);
        }
        
        List<Metadata.Metadata> records = 
            Metadata.Operations.retrieve(Metadata.MetadataType.CustomMetadata, 
                                         vatFullNames);
        
        for (Metadata.Metadata record : records) {
            Metadata.CustomMetadata vatRecord = (Metadata.CustomMetadata) record;
            String vatRecordName = vatRecord.fullName.substringAfter('.');
            VAT_Rate__mdt vatToCopy = VATsByApiName.get(vatRecordName);
            
            for (Metadata.CustomMetadataValue vatRecValue : vatRecord.values) {
                vatRecValue.value = vatToCopy.get(vatRecValue.field);
            }
            
            // Add record to the container.
            container.addMetadata(vatRecord);
        }   
        
        // Deploy the container with the new components. 
        Id asyncResultId = Metadata.Operations.enqueueDeployment(container, null);
        
        return null;
    }
}
<apex:page controller="VATController">
    <apex:form >
        <apex:pageBlock title="VAT Rates" mode="edit">
            <apex:pageMessages />
            
            <apex:pageBlockButtons >
                <apex:commandButton action="{!save}" value="Save"/>
            </apex:pageBlockButtons>
            
            <apex:pageBlockTable value="{!VATs}" var="v">
                <apex:column value="{!v.MasterLabel}"/>
                <apex:column headerValue="Rate">
                    <apex:inputText value="{!v.Rate__c}"/>
                </apex:column>
                <apex:column headerValue="Default">
                    <apex:inputCheckbox value="{!v.Default__c}"/>
                </apex:column>
            </apex:pageBlockTable>
        </apex:pageBlock>
    </apex:form>
</apex:page>
   # Exercise Code
public class MetadataExample {
    public void updateMetadata(){
        Metadata.CustomMetadata customMetadata = new Metadata.CustomMetadata();
        customMetadata.fullName = 'MyNamespace__MyMetadataTypeName.MyMetadataRecordName';
        
        Metadata.CustomMetadataValue customField = new Metadata.CustomMetadataValue();
        customField.field = 'customField__c';
        customField.value = 'New value';
        
        //add custom field to metadata
        customMetadata.values.add(customField);
        
        Metadata.DeployContainer deployContainer = new Metadata.DeployContainer();
        deployContainer.addMetadata(customMetadata);
        Id asyncResultId  = Metadata.Operations.enqueueDeployment(deployContainer, null);
    }
}
   # Test Container and Callback
   # Example code
@IsTest
public class DeploymentTest {
    @IsTest
    static void testDeployment() {
        DeployMetadata deployMd = new DeployMetadata();
        
        Metadata.DeployContainer container = deployMd.constructDeploymentRequest();
        List<Metadata.Metadata> contents = container.getMetadata();
        System.assertEquals(1, contents.size());
        Metadata.Layout md = (Metadata.Layout) contents.get(0);
       
        // Perform various assertions the layout metadata.
        System.assertEquals('Account-Account Layout', md.fullName);
    }
}
@IsTest
public class MyDeploymentCallbackTest {
    @IsTest
    static void testMyCallback() {
        
        // Instantiate the callback.
        Metadata.DeployCallback callback = new PostInstallCallback();
       
        // Create test result and context objects.
        Metadata.DeployResult result = new Metadata.DeployResult();
        result.numberComponentErrors = 1;
        Metadata.DeployCallbackContext context = new Metadata.DeployCallbackContext();
        
        // Invoke the callback's handleResult method.
        callback.handleResult(result, context);
    }
}
// DeployCallbackContext subclass for testing that returns myJobId.
public class TestingDeployCallbackContext extends Metadata.DeployCallbackContext {
    private Id myJobId = null; // Set to a fixed ID you can use for testing.
    public override Id getCallbackJobId() {
        return myJobId;
    }
}
   # security
     . only 2 types
	 . only 3 scenarios
   # Setup | Apex Settings can allows uncertified managed packages to execute metadata deployments.
   # Setup audit trail log in Setup | View Setup Audit Trail
   # Keep points in mind: A managed package Apex can
     . Update any publish subscripber-controlled metadata in same/different managed package, or subscriber org
	 . Update protected subscriber-controlled metadata in its own namespace.
	 . Update developer-controlled metadata only if it’s in the namespace of the subscriber org
```