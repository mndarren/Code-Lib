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