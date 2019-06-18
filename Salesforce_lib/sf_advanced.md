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
   
```