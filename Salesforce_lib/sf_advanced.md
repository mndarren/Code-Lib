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

```