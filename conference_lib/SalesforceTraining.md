# Training of Salesforce
==================================================<br/>
0. introduction
```
	1) 2 cloud: Sales Cloud
	            Service Cloud
	2) keywords: Trust, 
	             Multitenancy, 
	             Metadata, 
	             API
	3) Develop without code (metadata-driven: object, field, record; declarative development)
	4) 3 builders: Lightening App Builder, 
	               Process Builder (if know CRUD),
	               Schema Builder (obj field relationship)
	5) 3 core things: Lightning Component Framework, 
	                  Apex (like Java), 
	                  Virtualforce (like HTML)
	6) SOQL -- like SQL; 
	   SLDS -- Salesforce Lightning Design System;
	   Developer Console -- Salesforce IDE;
	   Heroku -- Web Development platform;
	   API -- SOAP, REST, Metadata, Tooling, Marketing Cloud, Bulk, Streaming, Chatter REST, Mobile SDK.
	7) UI desktop 2 tools: Lightning Experience, 
	                       Salesforce Classic
```
1. Obj
```
	1) 5 types of Obj: standard obj, 
	                   customer obj, 
	                   external obj, 
	                   platform event, 
	                   BigObjects
	2) obj relationships types: lookup (1-1, 1-*), 
	                            master-detail (cascade), 
	                            hierarchical (only for user obj)
	3) How? setup -> object manager -> new -> create obj
```
2. Import / Export Data
```
	1) 2 methods: Data Import Wizard (50k/time)
	              Data Loader (5M/time, client app, obj wizard not support, schedule data load)
	2) Contraints: permission, type of data, data storage, type of obj
	3) Bulk API: large # of records simultaneously, faster than SOAP since parallel and fewer network round trip
	4) Before import: export data from your system
	                  clean up data for accuracy and consistency
	                  compare your data fields to SF fields
	                  make any configuration changes required in SF, like new value to picklist, new field, temp deactivate workflow rules
	5) Tips: test small file first;
	         Big Data Load Jobs
	6) Export data: Data Export Wizard (6 days weekly backup, 28 days monthly)
	                Data Loader (UI or Cli customer export process)
	                48 hrs deleted after Email
```
3. Security
```
	1) 4 levels : org (authored users, passwd, when where), obj, field, record (org-wide default, role hierarchies, sharing rules, Manual sharing)
	2) Auditing: Record modification fields
	             Login history (succeeded, failed past 6 months)
	             Field history tracking (only standard obj)
	             Setup audi trail (logs)
	3) org level: manage authorized users (create, deactivate)
	              set passwd policies
	              limiting when (profile login hours) and where (Network Access OR Profile -> name -> edit OR new -> IP Range)
	4) Each User: name and passwd
	            1 profile
	            * permission sets
	5) Rule of permission: A permission can only add permission, cannot remove permission by assigning a permission set to user.
	6) Obj level: How? Permission set -> clone -> label (user license) -> save -> manage assignment -> assign -> done
	7) Field level: Enable Ehanced Profile UI first!!!
	                How to Profile? setup -> Profile -> choose user -> obj settings -> edit -> save
	                How to permissionset? obj settings -> edit -> field permissions -> manage assignments
	8) Record level: Permission on a record always combines (obj, field and record levels) permissions;
	                 When conflict, most restrict win.
	9) Principles: baseline <- Profile
	               permission set assigned also set the baseline permission
	               no owned records set first set by org-wide defaults
	               if org-wide default < Public Read/Write -> can backup certain roles
	               using "Sharing Rules" to expand access to additional groups of users
	               Each record owner can manually share records to others
	10) 4 levels access: Private, 
	                     Public Read Only, 
	                     Public Read/Write, 
	                     Controlled by Parent
	11) Org-wide default never grant more than obj permission
	12) 3 Quesions: who is the most restricted user of this obj?   --> org-wide default
	                Possible to create instance of the obj that user cannot read?
	                Possible to create instance of the obj that user cannot edit?
	13) How to do org-wide default? Sharing Setting -> org-wide default -> each obj set default
	14) Note: Never set org-wide default for Review obj since always private.
	15) **Disable automatic access** Deselect "Grant Access Using Hierarchies" for any customer obj
	    ==> only owner and users that owner granted can access data
	16) Updating org-wide default -> auto recalculation -> Completed Email -> refresh Sharing Settings
	17) Apex code modify customer obj <- only users with "Modify All Data" permission allows.
	18) How to do Role Hierarchies? Roles -> create -> assign users to role -> add -> save
	19) Sharing Rules: extend access with Sharing Rules
	        Prerequisite: org-wide default -- Private/Public Read Only
	        3 components: Share which records? ownership OR criteria-based sharing (based on field value, not ownership)
	                      With which users? public group = individual users + roles + roles&subordinates + other public groups
	                      What access? Read-only OR R/W
	        How? Public Groups -> new -> settings -> save
	             Sharing Settings -> Manage Sharing Settings for -> rules new -> based on record owner -> settings -save
```
4. Calculations
```
	1) Formular Editor (white space no matter, case sensitive)
	    Power of One on obj (10 account * 3 opportunities = 30 records)
	    functions: LEN(), Hyperlink(), Round(), AND()
	    How? Manage Object -> Field -> new -> Formular
	2) Implement Roll up Summary Field (based on master-detail relationship)
	    Summary: COUNT, SUM, MAX, MIN
	    How? Obj Manager -> Fields -> new -> roll up summary (Ex: Account (summary obj) --> Opportunities (summerized obj))
	3) Create Validation Rules (to obj, fields, campaign members, case milestone)
	    How? obj manager -> Account -> validation rules -> new -> error message
	    Ex: AND (NOT (ISBLANK(AccountNumber)),
	             NOT (ISNUMBER(AccountNumber)))
	        YEAR(My_Date__c) <> YEAR (TODAY())  -- must be in current year
	        (Salary_Max__c - Salary_Min__c) > 20000  -- must within 20000
	        AND (RIGHT(Web_Site__c, 4) <> ".COM",
	             RIGHT(Web_Site__c, 4) <> ".com",
	             RIGHT(Web_Site__c, 4) <> ".ORG",
	             RIGHT(Web_Site__c, 4) <> ".org",
	             RIGHT(Web_Site__c, 4) <> ".NET",
	             RIGHT(Web_Site__c, 4) <> ".net")
```
5. Lightning Flow Process Automation (Process Builder (send), Cloud Flow Designer (send, retrieve))
```
	1) when to use which: Guide visual Experience -- Cloud Flow Designer
	                      Behind Screen Auto  -- Cloud Flow Designer, Process Builder, Apex
	                      Approval Auto  -- Approval
	   Process (record created, record updated, platform event occurs) 
	   --> Flows (guide automate experience, add more functionality, start process when click) 
	   --> Apex (more complex functionality)
	2) Process Builder 
	    3 components: trigger (when to run), 
	                  at lease 1 criteria node (whether or not execute action) (set filter, formular, always run),
	                  at lease 1 action(what to do) (immediate action, scheduled action)
	    3 types: Record change (create or edit)
	             Invacable (called by another)
	             Platform event (msg received)
	    How? setup -> process Builder -> Add trigger (add obj, choose obj, save) -> add criteria (add row, Advanced Yes -- Ignore)
	         -> Add schedule -> add action
	    Tip: Deactivate Validate Rules if conflict
    3) Cloud Flow Designer 
        3 blocks: Elements (drag it)
                  Connectors (set path next to do)
                  Resources (set start from)
        4 types of Element: Screen (adding fields, add Lightning components)
                            Logic (control flow create branches, update data)
                            Actions (Apex code)
                            Integration (Local Actions or Apex code) tie-in platform event
        How? setup -> Flows -> new flow -> add screen add fields -> create the record (Record Create Element, create new variable)
             -> create second screen -> finish flow -> connect element -> set start element -> save name -> activate flow (setup -> Flows -> activate)
        How to add Flow to Home? Lightning App Builder -> new -> Home -> clone default page -> drag -> Activate -> test -> add Run Flow
        How to enable Lightning? setup -> Process Automation Settings -> Enable Lightning Runtime for Flow
    4) Combine Flow and Process (Process cannot grab ID of existing record)
    	4 types of Flow variables: Variable (a value), sObject (a record), sObject collection (* record), Collection (set of same data type)
    5) Approval initial submission action (lock record),
                final rejection action (Rejected),
                final approval action (Approved, unlock record)
        How to create Email Template? setup -> Email Template -> new -> settings -> save
        How to approval? setup -> Approval Process -> new (jump start wizard) -> Configure -> Final      
```
6. Apex
```
	1) Hosted Apex, OOP, strong-typed, multitenant, integrated with DB, easy to use to test, versioned
	2) Diff: cloud development, triggers, DB query statement, transaction & rollback, global > public, versioning
	3) Data type: Date, Datetime, ID, sObject, collections(map, sObject collection), Enum
	    List<String> colors = new List<String>();  // Better since no # of elements. colors[0], colors.get(0), .size(), colors.add()
	    String[] colors = new String[];
	4) class -> save (auto compiled) Anonymous Apex -- run lines of code
	5) __c Customer, __r relationship
	6) sObject sobj1 = new Account(Name='Zhao');
	   Account acct = (Account) sobj1;
	   String name = acct.name;
	7) Tip: API names of object and fields can diff from their labels.
	8) DML: insert | update | upsert | delete | undelete | merge
	        insert acct; //ID auto assigned
	        DmlException try{} catch(){}
	9) DB methods: Database.insert() .update() .upsert() .delete() .undelete() .merge()
	          insert recordList;
	          Database.insert(recordList, false); // allOrNone false means partial no error
	          Database.SaveResult[] results = Database.insert(recordList, false); //success & fail info
	          Database.UpsertResult DeleteResult
	10) Apex Ex:
		public class EmailManager {
		    // Public method
		    public void sendMail(String address, String subject, String body) {
		        // Create an email message object
		        Messaging.SingleEmailMessage mail = new Messaging.SingleEmailMessage();
		        String[] toAddresses = new String[] {address};
		        mail.setToAddresses(toAddresses);
		        mail.setSubject(subject);
		        mail.setPlainTextBody(body);
		        // Pass this email message to the built-in sendEmail method 
		        // of the Messaging class
		        Messaging.SendEmailResult[] results = Messaging.sendEmail(
		                                 new Messaging.SingleEmailMessage[] { mail });
		        
		        // Call a helper method to inspect the returned results
		        inspectResults(results);
		    }
		    
		    // Helper method
		    private static Boolean inspectResults(Messaging.SendEmailResult[] results) {
		        Boolean sendResult = true;
		        
		        // sendEmail returns an array of result objects.
		        // Iterate through the list to inspect results. 
		        // In this class, the methods send only one email, 
		        // so we should have only one result.
		        for (Messaging.SendEmailResult res : results) {
		            if (res.isSuccess()) {
		                System.debug('Email sent successfully');
		            }
		            else {
		                sendResult = false;
		                System.debug('The following errors occurred: ' + res.getErrors());                 
		            }
		        }
		        
		        return sendResult;
		     }
		}
```
7. SOQL and SOSL
```
	1) ORDER BY name ASC  --DESC
	   LIMIT n    -- LIMIT 1
	   String targetDep = 'Wingo';
	   [WHERE Department =: targetDep]
	2) Query related record
		[SELECT Name, (SELECT FirstName, LastName FROM Contacts)
		 FROM Account WHERE Name = 'Darren'];
		[SELECT Account.Name FROM Contact WHERE FirstName = 'Darren'];
	
	3) SOQL Ex:
		public class ContactSearch {
		
		    public static List<Contact> searchForContacts(String lastName, String pc){
		        List<Contact> contacts = [SELECT ID, Name FROM CONTACT 
		                              	  WHERE LastName =: lastName and MailingPostalCode =: pc];
		        return contacts;
		    }
		}
	
	4) SOSL (Search fields across multiple objects ~ Apache Lucene)
		SOSL editor: FIND {Windo}
		in Apex:     FIND 'Windo'
		Syntax: FIND 'SearchQuery' [IN SearchGroup] [RETURNING ObjectsAndFields]
		SearchGroup: ALL FIELDS   by default
					 Name Fields
					 Email Fields
					 Phone Fields
					 Sidebar Fields
		SearchQuery: The Query    --  The SFDC Query (matched)
					 Wingo OR Man --  Carol Department 'Wingo' (matched)
					 1212         --  (415)555-1212 (matched)
					 Wing*        --  * means 0 or more char
					 Wing?        --  ? means 1 char
					 AND ()   can be used as well
		Order by Ex:
			FIND {Cloud Kicks} RETURNING Account (Name, Industry ORDER BY Name)
		Limit n Ex:
			FIND {Cloud Kicks} RETURNING Account (Name, Industry ORDER BY Name LIMIT 10)
		Offset n Ex:
			FIND {Cloud Kicks} RETURNING Account (Name, Industry ORDER BY Name LIMIT 10 OFFSET 25)
		WITH DIVIDION Ex:
		    FIND {Cloud Kicks} RETURNING Account (Name, Industry)
    	       WITH DIVISION = 'Global'
		WITH DATA CATEGORY Ex:
		    FIND {race} RETURNING KnowledgeArticleVersion
               (Id, Title WHERE PublishStatus='online' and language='en_US')
               WITH DATA CATEGORY Location__c AT America__c
        WITH NETWORK Ex:
            FIND {first place} RETURNING User (Id, Name),
               FeedItem (id, ParentId WHERE CreatedDate = THIS_YEAR Order by CreatedDate DESC)
               WITH NETWORK = '00000000000001'
        WITH PRICEBOOK Ex:
            Find {shoe} RETURNING Product2 WITH PricebookId = '01sxx0000002MffAAE'
		Ex:
		List<List<sObject>> searchList = [FIND 'Wingo OR SFDC' IN ALL FIELDS 
		                   RETURNING Account(Name),Contact(FirstName,LastName,Department)];
		Account[] searchAccounts = (Account[])searchList[0];
		Contact[] searchContacts = (Contact[])searchList[1];
		System.debug('Found the following accounts.');
		for (Account a : searchAccounts) {
		    System.debug(a.Name);
		}
		System.debug('Found the following contacts.');
		for (Contact c : searchContacts) {
		    System.debug(c.LastName + ', ' + c.FirstName);
		}
		public class ContactAndLeadSearch {
	
		    public static List<List<sObject>> searchContactsAndLeads(String target){
		        
		        List<List<sObject>> searchList = [FIND :target IN ALL FIELDS 
		                                          RETURNING Contact(FirstName,LastName), Lead(FirstName,LastName)];
				return searchList;
		    }
		}
```
8. Triggers
```
	1) events: before insert, before update, before delete
			   after insert, after update, after delete, after undelete  # the record that fire the after trigger, read only
	2) context var: Trigger.New Trigger.Old
					isExecuting  -- true if Apex code is a trigger
					isAfter, isBefore, isInsert, isUpdate, isDelete, isUndelete
					newMap  -- a map of IDs for before update, after insert, after updagte, after undelete
					odeMap  -- for update and delete
					size    -- both new and old
	3) Tip: never use DML operation, use Database.methods()
	4) Query limits: sync 100 SOQL
					 async 200 SOQL
					 SML statement 150 in trigger
	5) How to activate trigger? setup -> Apex Triggers -> Edit -> is active check -> save
	6) Trigger Ex:
	trigger ContextExampleTrigger on Account (before insert, after insert, after delete) {
	    if (Trigger.isInsert) {
	        if (Trigger.isBefore) {
	            // Process before insert
	            for (Account a : Trigger.New){  // bulk insert
	            	a.Description = 'New Description!'
	        	}
	        } else if (Trigger.isAfter) {
	            // Process after insert
	        }        
	    }
	    else if (Trigger.isDelete) {
	        // Process after delete
	    }
	}
	
	trigger AddRelatedRecord on Account(after insert, after update) {
	    List<Opportunity> oppList = new List<Opportunity>();
	    
	    // Get the related opportunities for the accounts in this trigger
	    Map<Id,Account> acctsWithOpps = new Map<Id,Account>(
	        [SELECT Id,(SELECT Id FROM Opportunities) FROM Account WHERE Id IN :Trigger.New]);
	    
	    // Add an opportunity for each account if it doesn't already have one.
	    // Iterate through each account.
	    for(Account a : Trigger.New) {
	        System.debug('acctsWithOpps.get(a.Id).Opportunities.size()=' + acctsWithOpps.get(a.Id).Opportunities.size());
	        // Check if the account already has a related opportunity.
	        if (acctsWithOpps.get(a.Id).Opportunities.size() == 0) {
	            // If it doesn't, add a default opportunity
	            oppList.add(new Opportunity(Name=a.Name + ' Opportunity',
	                                       StageName='Prospecting',
	                                       CloseDate=System.today().addMonths(1),
	                                       AccountId=a.Id));
	        }           
	    }
	    if (oppList.size() > 0) {
	        insert oppList;
	    }
	}

	//addError() Ex:
	trigger AccountDeletion on Account (before delete) {
	   
	    // Prevent the deletion of accounts if they have related opportunities.
	    for (Account a : [SELECT Id FROM Account
	                     WHERE Id IN (SELECT AccountId FROM Opportunity) AND
	                     Id IN :Trigger.old]) {
	        Trigger.oldMap.get(a.Id).addError(
	            'Cannot delete account with related opportunities.');
	    }
	    
	}

	//@future Ex:
	public class CalloutClass {
	    @future(callout=true)
	    public static void makeCallout() {
	        HttpRequest request = new HttpRequest();
	        // Set the endpoint URL.
	        String endpoint = 'http://yourHost/yourService';
	        request.setEndPoint(endpoint);
	        // Set the HTTP verb to GET.
	        request.setMethod('GET');
	        // Send the HTTP request and get the response.
	        HttpResponse response = new HTTP().send(request);
	    }
	}
	trigger CalloutTrigger on Account (before insert, before update) {
	    CalloutClass.makeCallout();
	}
	trigger AccountAddressTrigger on Account (before insert, before update) {
	    for(Account a : Trigger.new){
	        If (a.Match_Billing_Address__c == true && a.BillingPostalCode!=Null) {
	            a.ShippingPostalCode = a.BillingPostalCode;
	        }   
	    }
	}
	trigger SoqlTriggerBulk on Account(after update) {  
	    // Perform SOQL query once.    
	    // Get the accounts and their related opportunities.
	    List<Account> acctsWithOpps = 
	        [SELECT Id,(SELECT Id,Name,CloseDate FROM Opportunities) 
	         FROM Account WHERE Id IN :Trigger.New];
	  
	    // Iterate over the returned accounts    
	    for(Account a : acctsWithOpps) { 
	        Opportunity[] relatedOpps = a.Opportunities;  
	        // Do some other processing
	    }
	}
	trigger SoqlTriggerBulk on Account(after update) {  
	    // Perform SOQL query once.    
	    // Get the related opportunities for the accounts in this trigger.
	    List<Opportunity> relatedOpps = [SELECT Id,Name,CloseDate FROM Opportunity
	        WHERE AccountId IN :Trigger.New];
	  
	    // Iterate over the related opportunities    
	    for(Opportunity opp : relatedOpps) { 
	        // Do some other processing
	    }
	}
	trigger AddRelatedRecord on Account(after insert, after update) {
	    List<Opportunity> oppList = new List<Opportunity>();
	    
	    // Add an opportunity for each account if it doesn't already have one.
	    // Iterate over accounts that are in this trigger but that don't have opportunities.
	    for (Account a : [SELECT Id,Name FROM Account
	                     WHERE Id IN :Trigger.New AND
	                     Id NOT IN (SELECT AccountId FROM Opportunity)]) {
	        // Add a default opportunity for this account
	        oppList.add(new Opportunity(Name=a.Name + ' Opportunity',
	                                   StageName='Prospecting',
	                                   CloseDate=System.today().addMonths(1),
	                                   AccountId=a.Id)); 
	    }
	    
	    if (oppList.size() > 0) {
	        insert oppList;
	    }
	}
	
	trigger ClosedOpportunityTrigger on Opportunity (after insert, after update) {
	
	      List<Task> OpTasklist = new List<Task>();
	    
	    // Iterate over opportunities that are in this trigger and have a stage of "Closed Won"
	    for (Opportunity op: [SELECT id FROM Opportunity 
	                          WHERE Id IN :Trigger.New AND
	                          StageName =: 'Closed Won']) {
	                              
	                              if (((Trigger.IsUpdate) && (Trigger.oldMap.get(op.Id).StageName != 'Closed Won')) || 
	                                  (Trigger.IsInsert)) {
	                                      OpTaskList.add(new Task (Subject='Follow Up Test Task', 
	                                                               WhatId = op.Id)); }          
	                          }
	    
	    If (OpTaskList.size() > 0) { 
	        Insert OpTaskList ;
	    } 
	}
```
9. Apex Test (Apex Hammer -- run all test, at lease 75% coverage and all tests past)
```
	1) Syntax: @isTest static void testName(){}
	 		   static testMethod void testName(){}
	2) unit test -- private
	   data factory class -- public
	   == operator performs case insensitive
	3) Test Suite
		How? Developer Console -> Test|New Suite
		Rollback not stay in DB after test for test data
		Not best practice, but we can. @isTest (SeeAllData=true)
	4) Test Ex:
		public class TemperatureConverter {
		    // Takes a Fahrenheit temperature and returns the Celsius equivalent.
		    public static Decimal FahrenheitToCelsius(Decimal fh) {
		        Decimal cs = (fh - 32) * 5/9;
		        return cs.setScale(2);
		    }
		}
		@isTest
		private class TemperatureConverterTest {
		    @isTest static void testWarmTemp() {
		        Decimal celsius = TemperatureConverter.FahrenheitToCelsius(70);
		        System.assertEquals(21.11,celsius);
		    }
		    
		    @isTest static void testFreezingPoint() {
		        Decimal celsius = TemperatureConverter.FahrenheitToCelsius(32);
		        System.assertEquals(0,celsius);
		    }
		    @isTest static void testBoilingPoint() {
		        Decimal celsius = TemperatureConverter.FahrenheitToCelsius(212);        
		        System.assertEquals(100,celsius,'Boiling point temperature is not expected.');
		    } 
		    
		    @isTest static void testNegativeTemp() {
		        Decimal celsius = TemperatureConverter.FahrenheitToCelsius(-10);
		        System.assertEquals(-23.33,celsius);
		    }
		      
		}
		@isTest
		public class TestVerifyDate {
		
		    @isTest static void testDate(){
		        Date date1 = System.today();
		        Date date2 = date1.addDays(10);
		        Date date3 = date1.addDays(30);
		        Date date4 = date1.addDays(40);
		        Integer totalDays = Date.daysInMonth(date1.year(), date1.month());
		        Date lastDay = Date.newInstance(date1.year(), date1.month(), totalDays);
		        
		        Date d = VerifyDate.CheckDates(date1, date2);
		        System.assertEquals(date2, d);
		        
		        d = VerifyDate.CheckDates(date1, date3);
		        System.assertEquals(date3, d);
		        
		        d = VerifyDate.CheckDates(date1, date4);
		        System.assertEquals(lastDay, d);
		        
		        d = VerifyDate.CheckDates(date2, date1);
		        System.assertEquals(lastDay, d);
		    }
		}
		@isTest
		private class TestAccountDeletion {
		    @isTest static void TestDeleteAccountWithOneOpportunity() {
		        // Test data setup
		        // Create an account with an opportunity, and then try to delete it
		        Account acct = new Account(Name='Test Account');
		        insert acct;
		        Opportunity opp = new Opportunity(Name=acct.Name + ' Opportunity',
		                                       StageName='Prospecting',
		                                       CloseDate=System.today().addMonths(1),
		                                       AccountId=acct.Id);
		        insert opp;
		        
		        // Perform test
		        Test.startTest();
		        Database.DeleteResult result = Database.delete(acct, false);
		        Test.stopTest();
		        // Verify 
		        // In this case the deletion should have been stopped by the trigger,
		        // so verify that we got back an error.
		        System.assert(!result.isSuccess());
		        System.assert(result.getErrors().size() > 0);
		        System.assertEquals('Cannot delete account with related opportunities.',
		                             result.getErrors()[0].getMessage());
		    }
		    
		}
		@isTest
		public class TestRestrictContactByName {
		
		    @isTest static void TestInsertContactWithInvalidLastName() {
		        // Test data setup
		        // Create a contact with the last name INVALIDNAME
		        Contact cont = new Contact(FirstName = 'John ', LastName = 'INVALIDNAME');
		            
		        // Perform test
		        Test.startTest();
		        Database.SaveResult result = Database.insert(cont, false);
		        Test.stopTest();
		
		        // Verify 
		        // In this case the insert operation should have been stopped by the trigger,
		        // so verify that we got back an error.
		        System.assert(!result.isSuccess());
		        System.assert(result.getErrors().size() > 0);
		        System.assertEquals('The Last Name "INVALIDNAME" is not allowed for DML',
		                             result.getErrors()[0].getMessage());
		    
		    }
		   
		}
		//Create Test Data for Apex Test
		@isTest
		public class TestDataFactory {
		    public static List<Account> createAccountsWithOpps(Integer numAccts, Integer numOppsPerAcct) {
		        List<Account> accts = new List<Account>();
		        
		        for(Integer i=0;i<numAccts;i++) {
		            Account a = new Account(Name='TestAccount' + i);
		            accts.add(a);
		        }
		        insert accts;
		        
		        List<Opportunity> opps = new List<Opportunity>();
		        for (Integer j=0;j<numAccts;j++) {
		            Account acct = accts[j];
		            // For each account just inserted, add opportunities
		            for (Integer k=0;k<numOppsPerAcct;k++) {
		                opps.add(new Opportunity(Name=acct.Name + ' Opportunity ' + k,
		                                       StageName='Prospecting',
		                                       CloseDate=System.today().addMonths(1),
		                                       AccountId=acct.Id));
		            }
		        }
		        // Insert all opportunities for all accounts.
		        insert opps;
		        
		        return accts;
		    }
		}
		
		Account[] accts = TestDataFactory.createAccountsWithOpps(1,1);
```
10. Visualforce -- Web Development Framework
```
	1) 150 VF components
	2) global var     -- {!$user.FirstName}
	   VF Expressions -- {!Contact.FirstName}
	3) Controller: coarse-grained components <Apex:detail>
				   fine-grained components
	4) Add a page a valid record ID "&id=xxxxxxxx"
	5) VF Ex:
		<apex:page standardController="Contact" >
		    <apex:form >
		        
		        <apex:pageBlock title="Edit Contact">
		            <apex:pageBlockSection columns="1">
		                <apex:inputField value="{!Contact.FirstName}"/>
		                <apex:inputField value="{!Contact.LastName}"/>
		                <apex:inputField value="{!Contact.Email}"/>
		                <apex:inputField value="{!Contact.Birthdate}"/>
		            </apex:pageBlockSection>
		            <apex:pageBlockButtons >
		                <apex:commandButton action="{!save}" value="Save"/>
		            </apex:pageBlockButtons>
		        </apex:pageBlock>
		        
		    </apex:form>
		</apex:page>
		
		{! $User.FirstName & ' ' & $User.LastName }
		
		<p>The year today is {! YEAR(TODAY()) }</p>
		<p>Tomorrow will be day number  {! DAY(TODAY() + 1) }</p>
		<p>Let's find a maximum: {! MAX(1,2,3,4,5,6,5,4,3,2,1) } </p>
		<p>The square root of 49 is {! SQRT(49) }</p>
		<p>Is it true?  {! CONTAINS('salesforce.com', 'force.com') }</p>
		
		<p>{! IF( CONTAINS('salesforce.com','force.com'), 
		     'Yep', 'Nope') }</p>
		<p>{! IF( DAY(TODAY()) < 15, 
		     'Before the 15th', 'The 15th or after') }</p>
		({! IF($User.isActive, $User.Username, 'inactive') })
		
		<apex:page showHeader="false">
		    <apex:pageBlock title="User Status">
		        <apex:pageBlockSection columns="1">
		            
		            {!$User.FirstName } {!$User.LastName } ({!$User.Username })
		            
		        </apex:pageBlockSection>
		    </apex:pageBlock>
		</apex:page>
		
		//controller
		<apex:page standardController="Contact">
		    
		    <apex:pageBlock title="Contact Summary">
		        <apex:pageBlockSection>
		        	
		            First Name: {! Contact.FirstName } <br/>
		            Last Name: {! Contact.LastName } <br/>
		            Owner Email: {! Contact.Owner.Email} <br/>
		            
		        </apex:pageBlockSection>
		    </apex:pageBlock>
		    
		</apex:page>
		
		//OUtput components
		<apex:detail relatedList="false"/>  // too much to disable related list
		<apex:relatedList list="Opportunities" pageSize="5"/>  //display related record
		
		//instead of detail
		<apex:outputField value="{! Account.Name }"/>
		<apex:outputField value="{! Account.Phone }"/>
		<apex:outputField value="{! Account.Industry }"/>
		<apex:outputField value="{! Account.AnnualRevenue }"/>
		
		//Display a table
		<apex:pageBlock title="Contacts">
		   <apex:pageBlockTable value="{!Account.contacts}" var="contact">
		      <apex:column value="{!contact.Name}"/>
		      <apex:column value="{!contact.Title}"/>
		      <apex:column value="{!contact.Phone}"/>
		   </apex:pageBlockTable>
		</apex:pageBlock>
		//challenge
		<apex:page standardController="Opportunity">
		    <apex:outputField value="{! Opportunity.Name }"/>
			<apex:outputField value="{! Opportunity.Amount }"/>
			<apex:outputField value="{! Opportunity.CloseDate }"/>
			<apex:outputField value="{! Opportunity.Account.Name}"/>
		</apex:page>
		
		//form
		<apex:page standardController="Account">
		    <apex:form>
		    
		    <apex:pageBlock title="Edit Account">
		    <apex:pageMessages/>
		        <apex:pageBlockSection columns="1">
		    	<apex:inputField value="{! Account.Name }"/>
		    	<apex:inputField value="{! Account.Phone }"/>        
		    	<apex:inputField value="{! Account.Industry }"/>        
		    	<apex:inputField value="{! Account.AnnualRevenue }"/>
			</apex:pageBlockSection>
		        <apex:pageBlockButtons>
		            <apex:commandButton action="{! save }" value="Save" />        
		        </apex:pageBlockButtons>
		    </apex:pageBlock>
		
		    <apex:pageBlock title="Contacts">
		    	<apex:pageBlockTable value="{!Account.contacts}" var="contact">
		        	<apex:column>
		            	<apex:outputLink
		                	value="{! URLFOR($Action.Contact.Edit, contact.Id) }">
		                	Edit
		            	</apex:outputLink>
		            	&nbsp;
		            	<apex:outputLink
		                	value="{! URLFOR($Action.Contact.Delete, contact.Id) }">
		                	Del
		            	</apex:outputLink>
		        	</apex:column>
		        	<apex:column value="{!contact.Name}"/>
		        	<apex:column value="{!contact.Title}"/>
		        	<apex:column value="{!contact.Phone}"/>
		    	</apex:pageBlockTable>
			</apex:pageBlock>
		    
		    </apex:form>
		</apex:page>
		
		//list controller
		<apex:page standardController="Contact" recordSetVar="contacts">
		    <apex:pageBlock title="Contacts List" id="contacts_list">
		        
		            Filter: 
		            <apex:selectList value="{! filterId }" size="1">
		                <apex:selectOptions value="{! listViewOptions }"/>
		                <apex:actionSupport event="onchange" reRender="contacts_list"/>
		            </apex:selectList>
		        
		        <!-- Contacts List -->
		        <apex:pageBlockTable value="{! contacts }" var="ct">
		            <apex:column value="{! ct.FirstName }"/>
		            <apex:column value="{! ct.LastName }"/>
		            <apex:column value="{! ct.Email }"/>
		            <apex:column value="{! ct.Account.Name }"/>
		        </apex:pageBlockTable>
		        
		    </apex:pageBlock>
		</apex:page>
		
		<!-- Pagination -->
		<table style="width: 100%"><tr>
		    <td>
		        Page: <apex:outputText 
		    		value=" {!PageNumber} of {! CEILING(ResultSize / PageSize) }"/>
		    </td>            
		    <td align="center">
		        <!-- Previous page -->
				<!-- active -->
				<apex:commandLink action="{! Previous }" value="« Previous"
		     		rendered="{! HasPrevious }"/>
				<!-- inactive (no earlier pages) -->
				<apex:outputText style="color: #ccc;" value="« Previous"
		     		rendered="{! NOT(HasPrevious) }"/>
				&nbsp;&nbsp;  
				<!-- Next page -->
				<!-- active -->
				<apex:commandLink action="{! Next }" value="Next »"
		     		rendered="{! HasNext }"/>
				<!-- inactive (no more pages) -->
				<apex:outputText style="color: #ccc;" value="Next »"
		     		rendered="{! NOT(HasNext) }"/>
		    </td>
		    
		    <td align="right">
		        Records per page:
				<apex:selectList value="{! PageSize }" size="1">
		    		<apex:selectOption itemValue="5" itemLabel="5"/>
		    		<apex:selectOption itemValue="20" itemLabel="20"/>
		    		<apex:actionSupport event="onchange" reRender="contacts_list"/>
				</apex:selectList>
		    </td>
		</tr></table>
		
		//Challenge
		<apex:page standardController="Account" recordSetVar="Accounts" >
		   <apex:pageblock>
		       <apex:repeat var="a" value="{!Accounts}" rendered="true" id="account_list">
		           <li>
		               <apex:outputLink value="/{!a.ID}" >
		                   <apex:outputText value="{!a.Name}"/>
		               </apex:outputLink>
		           </li>
		       </apex:repeat>
		   </apex:pageblock>
		</apex:page>
		
		//static resource ($Resource | URLFOR())
		How? setup -> Static Resources -> new JQuery choose file public -> save
		{! $Resource.jQuery}
		<apex:page>
		    
		    <!-- Add the static resource to page's <head> -->
		    <apex:includeScript value="{! $Resource.jQuery }"/>
		    
		    <!-- A short bit of jQuery to test it's there -->
		    <script type="text/javascript">
		        jQuery.noConflict();
		        jQuery(document).ready(function() {
		            jQuery("#message").html("Hello from jQuery!");
		        });
		    </script>
		    
		    <!-- Where the jQuery message will appear -->
		    <h1 id="message"></h1>
		    
		</apex:page>
		
		Tip: when creating Static Resource with zip file, if > 5MB, remove /demo/ directory and upload a smaller one.
		<apex:page showHeader="false" sidebar="false" standardStylesheets="false">
		    
		    <!-- Add static resources to page's <head> -->
		    <apex:stylesheet value="{!
		          URLFOR($Resource.jQueryMobile,'jquery.mobile-1.4.5/jquery.mobile-1.4.5.css')}"/>
		    <apex:includeScript value="{! $Resource.jQuery }"/>
		    <apex:includeScript value="{!
		         URLFOR($Resource.jQueryMobile,'jquery.mobile-1.4.5/jquery.mobile-1.4.5.js')}"/>
		    <div style="margin-left: auto; margin-right: auto; width: 50%">
		        <!-- Display images directly referenced in a static resource -->
		        <h3>Images</h3>
		        <p>A hidden message:
		            <apex:image alt="eye" title="eye"
		                 url="{!URLFOR($Resource.jQueryMobile, 'jquery.mobile-1.4.5/images/icons-png/eye-black.png')}"/>
		            <apex:image alt="heart" title="heart"
		                 url="{!URLFOR($Resource.jQueryMobile, 'jquery.mobile-1.4.5/images/icons-png/heart-black.png')}"/>
		            <apex:image alt="cloud" title="cloud"
		                 url="{!URLFOR($Resource.jQueryMobile, 'jquery.mobile-1.4.5/images/icons-png/cloud-black.png')}"/>
		        </p>
		    <!-- Display images referenced by CSS styles,
		         all from a static resource. -->
		    <h3>Background Images on Buttons</h3>
		    <button class="ui-btn ui-shadow ui-corner-all
		         ui-btn-icon-left ui-icon-action">action</button>
		    <button class="ui-btn ui-shadow ui-corner-all
		         ui-btn-icon-left ui-icon-star">star</button>
		    </div>
		</apex:page>
		
		//challenge
		<apex:page showHeader="false" >
		    <!-- Add static resources to page's <head> -->
		    <apex:image value="{! URLFOR($Resource.vfimagetest,'cats/kitten1.jpg')}"/>
		</apex:page>
		
		//Customer Controller
		How? create controller class -> use it in VF page
		public class ContactsListController {
		    private String sortOrder = 'LastName';
		    
			public List<Contact> getContacts() {
		    
		    	List<Contact> results = Database.query(
		        	'SELECT Id, FirstName, LastName, Title, Email ' +
		        	'FROM Contact ' +
		        	'ORDER BY ' + sortOrder + ' ASC ' +
		        	'LIMIT 10'
		    	);
		    	return results;
			}
			public void sortByLastName() {
		    	this.sortOrder = 'LastName';
			}
		    
			public void sortByFirstName() {
		    	this.sortOrder = 'FirstName';
			}
		}
		<apex:page controller="ContactsListController">
		    <apex:form>
		        <apex:pageBlock title="Contacts List" id="contacts_list">
		            
		            <!-- Contacts List -->
					<apex:pageBlockTable value="{! contacts }" var="ct">
		    			<apex:column value="{! ct.FirstName }">
		    				<apex:facet name="header">
		        				<apex:commandLink action="{! sortByFirstName }" 
		            				reRender="contacts_list">First Name
		        				</apex:commandLink>
		    				</apex:facet>
						</apex:column>
						<apex:column value="{! ct.LastName }">
		    				<apex:facet name="header">
		       	 				<apex:commandLink action="{! sortByLastName }" 
		            				reRender="contacts_list">Last Name
		        				</apex:commandLink>
		    				</apex:facet>
						</apex:column>
		    			<apex:column value="{! ct.Title }"/>
		    			<apex:column value="{! ct.Email }"/>
		    
					</apex:pageBlockTable>
		        </apex:pageBlock>
		    </apex:form>
		</apex:page>
		
		//Apex properties
		public MyObject__c myVariable { get; set; }
		//challenge
		public class NewCaseListController {
		
		    public List<Case> getNewCases() {
		    
		    	List<Case> results = [Select Id, CaseNumber FROM Case WHERE status = 'New'];
		    	return results;
			}
		}
		<apex:page controller="NewCaseListController">
		  <apex:repeat var="case" value="{!NewCases}">
		  <li>
		  <apex:outputLink value="/{!case.id}">
		      {!case.id} {!case.CaseNumber}
		  </apex:outputLink>
		  </li>
		  </apex:repeat>
		</apex:page>
```
11. Developer Console
```
	1) Developer Console doesn't have version control or conflict resolution. Be careful to avoid overwrite other code
	2) How to setup Workspace? Workspace -> new
		When switching workspace, Workspace -> Workspace Manager Workspace -> Switch Workspace
	3) How to create a Lightning Component? File -> New -> Lightning Component -> submit -> save
	4) VF Page challenge
<apex:page sidebar="false">
   <h1>Station Status</h1>
   <apex:form id="stationReadinessChecklist">
      <apex:pageBlock title="Station Readiness Checklist">
         <!--Second Section-->
         <apex:pageBlockSection title="Fuel Tanks">
            <apex:inputCheckbox immediate="true"/>Tank 1
            <apex:inputCheckbox immediate="true"/>Tank 2
            <apex:inputCheckbox immediate="true"/>Tank 3
            <apex:inputCheckbox immediate="true"/>Tank 4
            <apex:inputCheckbox immediate="true"/>Tank 5
            <apex:inputCheckbox immediate="true"/>Tank 6
         </apex:pageBlockSection>
         <apex:pageBlockButtons>
            <!--Adding Save Button-->
            <apex:commandButton value="Save" action="{!save}"/>
         </apex:pageBlockButtons>
      </apex:pageBlock>
   </apex:form>
</apex:page>
	5) View debug log (System.debug())
		Log Inspector -- easier to view large logs. (View Log Panels)
			How? Debug -> view Log Panels
		Perspective Manager How? Debug -> Switch Perspectives
			How to create own Perspective? 
			Switch log level -> Execute Code (with open log checked) -> Debug -> View Log Panels -> choose -> Save Perspective As
		Log Categories (ApexCode, Database(DML, SOQL, SOSL))
			Log level: NONE, ERROR, WARN, INFO, DEBUG, FINE, FINER, FINEST (they are cumulative)
			Tip: log level depends on log event. ApexCode events start logging at INFO, if setting ERROR, get nothing
			How? Debug -> Change Log Levels
	6) Inspect Objects at Checkpoints (Heap, Symbols)
		How? setup breakpoint -> run code -> checkpoint tab -> double click
	7) SOSL challenge:
	List<List<Contact>> contacts = [FIND 'Mission Control' IN ALL FIELDS RETURNING Contact(FirstName, LastName, Phone, Email, Title)];
	Contact[] cons = (Contact[])contacts[0];
    for (Contact c : cons){
        System.debug(c.Lastname +', '+c.Firstname);
    }
```
12. Search Solution Basics (most-used SF feature)
```
	1) Create a record, the search engine comes along, make a copy of the data, and breaks up the content into smaller pieces called tokens. Store these tokens in the search index, along with a link back to the original record.
	2) APIs SOQL, SOSL
	   Query(REST) and query()(SOAP) - Executes a SOQL query
	   Search(REST) and search()(SOAP)- Executes a SOSL text strinig search
	3) SF Federated Search connector built by OpenSearch standard engine
	4) Single obj search, Multiple obj search, Custom obj search
	5) Optimize Search Result
	      How? setup -> Synonyms -> choose
```
