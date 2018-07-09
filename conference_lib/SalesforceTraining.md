# Training of Salesforce
==================================================
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

```
7. SOQL and SOSL
```
SOQL Ex:
	public class ContactSearch {
	
	    public static List<Contact> searchForContacts(String lastName, String pc){
	        List<Contact> contacts = [SELECT ID, Name FROM CONTACT 
	                              	  WHERE LastName =: lastName and MailingPostalCode =: pc];
	        return contacts;
	        
	    }
	}

SOSL Ex:
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
	        
	        List<List<sObject>> searchList = [FIND target IN ALL FIELDS 
	                                          RETURNING Contact(FirstName,LastName), Lead(FirstName,LastName)];
			return searchList;
	    }
	}
```