# SF Tips (key: CRM)
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
   __r - Relationship
   __b - Big Object
   __mdt - Custom Metadata object
   ISV - Independent Software Vendor
   SLDS - Salesforce Lightning Design System
```
2. Refreshing Sandbox
```
   If you didn’t select Auto Activate while refreshing your sandbox, 
   you must activate your sandbox before you can use it.
```
3. Error: Log File Buffer Size is too big
```
   SELECT Id FROM ApexLog
   Then delete rows to clear log buffer
```
4. about validation rule in configuration
```
   there is a hidden 'gotcha' about test classes.
   They only "must" be run when deploying Apex into the org.
   They are not required to run when a configuration change is made. 
   This gotcha causes lots of common problems.
```
5. Trigger test example
```
static testMethod void Trigger_LockinCheckAcctForStartUpPricingInsert3()
    {   
//
//Insert a Test Account with a Boolean field for validation parameters   
//
    test.startTest();
        try{
       
        Account a = new Account();
       
        a.Name = 'Test Lockin Account3';
        a.Start_Up_Pricing_Consumed__c = true;
       
    insert a;
       
//Insert child custom object with Status != Active or Approved
//but Parent Account pricing already consumed 
//     
    LockIns__c l = new LockIns__c();
       
        l.Account_for_lock_in__c = a.Id;
        l.Status__c = 'New';
        l.Start_Date__c = System.today();
            
    insert l;   

//Update the custom object record's "Status" to "Active" which will provide Validation Error
//because the Parent Account is already set to TRUE.
//       
        l.Status__c = 'Active';
       
    update l;
        }
        catch(Exception e){
            System.Assert(e.getMessage().contains('Validation Rule exception message goes here'));
        }
        test.stopTest();
    }
```
6. write a validation rule using apex and triggers
```
trigger validphonemust on Account(before insert)
{
  for(Account a : trigger.new)
   {
    if(a.phone=='' || a.phone==null﻿)
    {
      a.addError('Phone number must');
    }
   }
}
//VF
<apex:page controller="Sample" sidebar="false" >
<apex:pagemessages />
<apex:form >
    <apex:pageblock >    
        <apex:pageBlocksection >
            <apex:pageblockSectionItem >Name:</apex:pageblockSectionItem>
            <apex:pageblockSectionItem ><apex:inputtext value="{!nam}" /></apex:pageblockSectionItem>        
            <apex:pageblockSectionItem >Age:</apex:pageblockSectionItem>        
            <apex:pageblockSectionItem ><apex:inputtext value="{!age}" /></apex:pageblockSectionItem>        
        </apex:pageBlocksection>      
        <apex:pageblockButtons >
            <apex:commandButton value="Submit" action="{!submit}" reRender=""/>
        </apex:pageblockButtons>
    </apex:pageblock>
</apex:form>
</apex:page>
// Controller
public class Sample
{
    public String nam {get;set;}
    public Decimal age {get;set;}
 
    public void submit()
    {
        try
        {
            Member__c m = new Member__c();
            m.Name = nam;
            m.Age__c = age;
            insert m;
        }
        catch(Exception e)
        {
            String error = e.getMessage();
            ApexPages.addMessage(new ApexPages.Message(ApexPages.Severity.ERROR,error));
        }
    }
}
```
7. With sharing, without sharing, inherited sharing
```
   # Apex without a sharing declaration is insecure by default.
   # with/without sharing turn on/off sharing rules enforcement
   with sharing - enforce rules on the current user
   # reason: Apex code running in system context (in which code can access 
     all objects and fields— object permissions, field-level security, sharing rules)
	 but not applied for the current user
   # key points
     . "without sharing" class calls "with sharing" class method => enforce sharing rules in the method
	 . "with sharing" class calls "without sharing" class => enforce rules to the "without sharing" class
	 . inner class not inherit the sharing rules from container class
	 . child class inherit sharing rules from parent class
   # inherited sharing keyword
   inherited sharing class runs as with sharing with:
    . An Aura component controller
	. A Visualforce controller
	. An Apex REST service
	. Any other entry point to an Apex transaction
   # inherited sharing kind of always good
   
```
8. Change set transferring
```
   Setup -> Deployment Settings -> choose Sandbox -> allow inbound changes
   # in Dev box choose UAT allowed
   # in UAT box choose Dev allowed
```
9. Formula field
```
   . user cannot change formula field values
   . lots of functions can be used in formula field
```
10. Speed up to sync code
```
Sometimes, pulling the entire project (Apex, Visualforce, Objects, Workflows, etc.) from the sandbox to IntelliJ can take a long time. Here is an alternative strategy which can speed up this process:
1.	If your sandbox was refreshed, in Source Control, verify your sandbox branch is at the same commit as the sandbox from which your sandbox was refreshed.
2.	In IntelliJ, instead of pulling down everything from the server, pull down only Apex Classes. This will be much faster than retrieving everything.
3.	In Source Control, you will see many deletions/removed files, since you didn't pull down anything but Apex Classes.
4.	In Source Control, reset your branch to the latest commit. This will add/recover the files you didn't pull down to IntelliJ.
5.	If you need to pull down individual files from the server, do so.
```
11. Custom Metadata Type usage
```
   . Mappings—Create associations between different objects, such as a custom metadata type that assigns cities, states, or provinces to particular regions in a country.
   . Business rules—Combine configuration records with custom functionality. Use custom metadata types along with some Apex code to route payments to the correct endpoint.
   . Master data—Let’s say that your org uses a standard accounting app. Create a custom metadata type that defines custom charges, like duties and VAT rates. If you include this type as part of an extension package, subscriber orgs can reference the master data.
   . Whitelists—Manage lists, such as approved donors and pre-approved vendors.
   . Secrets—Store information, like API keys, in your protected custom metadata types within a package.
```
12. Install Chrome extension for salesforce
```
https://chrome.google.com/webstore/detail/salesforce-change-set-hel/gdjfanbphogooppaefebaaoohdcigpoi?hl=en-US 
```
13. Jetforcer back from v2.0 to v1.4
```
JetForcer 1.4.x (Download link at right): https://plugins.jetbrains.com/plugin/9238-jetforcer--the-smartest-force-com-ide/versions 

No need to uninstall 2.x. 
Go to Settings > Plugins in IDEA. Click the gear icon and select 'Install Plugin from Disk'. Navigate to the Zip file.
Restart IDEA. 
```
14. Oddity to fixed
```
   # what is it?
   Running the above code in IntelliJ returns null, and yet running the same code 
   in the Developer Console successfully returns the Custom Setting values. Am I missing something?
   # solution
   1.Running the same code in the Salesforce Developer Console,
   2.Running the same code in an Apex test method/class, and
   3.Manually reviewing the values in the list in the Setup method
```
15. Relationship object call/query
```
   1) Parent calling Children by SOQL: "From Children__r"
   2) Children calling Parent by SOQL: "SELECT Children__r.fieldName"
```
16. IntelliJ wiredo
```
   Always run unit test by right click on the file name, otherwise probably part unit test run.
```
17. How to avoid getting SOQL Limits
```
   1) Don’t do Queries in Loops;
   2) use Describes instead of SOQL if we can;
      System.assert(s.getsObjectType() == Account.sObjectType);
	  Schema.DescribeSObjectResult dsr = Account.sObjectType.getDescribe();
	  Schema.DescribeFieldResult dfr = Schema.sObjectType.Account.fields.Name;
      System.assert(dfr.getSObjectField() == Account.Name);
   3) using static variables or custom settings when you know the data is unlikely to change much if/ever (Caching)
   4) Using Salesforce APIs instead of doing it within Salesforce if your processing will be very time consuming.
   5) Avoid using Process Builder on the Account, Opportunity, lead or contact since it doesn’t scale very well because its not bulkified.
   6) Use Test.startTest() and Test.stopTest() to get a fresh set of governor limits
   7) Avoid using Seealldata=true if possible. 
   8) Combine your SOQL queries
   9) Combine your DML statements by using update on a list instead of individual records.
   10)Separate your test class into multiple test classes.
```
18. Before update/delete in Trigger
```
   Once update/delete failed, SF will rollback all changes. No worries about that.
```
19. How to use switch
```
public enum Season {WINTER, SPRING, SUMMER, FALL}

switch on season {
   when WINTER {
       System.debug('boots');
   }
   when SPRING, SUMMER {
       System.debug('sandals');
   }
   when else {
       System.debug('none of the above');
```
20. When inserting a list of sObjects
```
   Note: if we insert new objects together, we should use __r and the correct order,
		 OR we can use __c if insert them 1 by 1.
		 if you know the object already exists in the SF you can use __c as well.
```
21. Types thinking
```
   component and page are similar with html/js;
   object, layout, report, profile and metadata are similar with xml/config.
```
22. Difference between Administration task and Development task
```
   # Admin task
    . Developing email templates
	. Creating or editing users
	. Creating or editing permission sets and profiles
   # Development task: any change to a field, an object, a tab...
```
23. Test a Validation Rule
```
    @IsTest
    static void testPurchase_Price_Rule(){
        
        CustomObject__c col = new CustomObject__c();
        col.RecordTypeId = CustomObjectClass.EQUIPMENT_ID;
        col.New_Purchase_Price__c = 6.6;
        
        try {
            Database.insert(new List<sObject>{col});
			System.assert(false, 'expected update failure');
        } catch (System.DmlException e) {
            System.assert(e.getMessage().contains('New Purchase Price is not applicable for any CustomObject'));
        }
    }
```
24. Object Type change will change both Object_Type__c and Object RecordType
```
   # Notes:
     1. Double check the related Record Type. Sometimes when you changed the order of picklist, 
	    the Record Type will not be updated as expected.
     2. When creating "outbound change set", we have to take case of both since they are the same.
```
25. FieldPath over Label in VF
```
   In VF page, we should use FieldPath to check the field, not Label because Label can be changed by user,
   while the FieldPath is fixed.
```
26. Difference between Master-Detail and Lookup relationships
```
   Master-Detail: Max 2 on one object; parent field is required; Delete parent -> auto delete children;
   Lookup: Max 25 on one object; parent field is not required; Delete parent will not auto delete children.
```
27. Debug Tip: Once creating a new field, we should change the VF page as well for the query field to compare values.
28. SOQL code conflicts with Trigger. If so, bypass() the Trigger
29. About Unit Test
```
   1. System.debug(); // will not show up always. When debug msg too much
      Try to use System.assertNotEqual() or System.assertEqual();
   2. Id of sObjects will be generated after insertion action.
   3. insert sObject one by one will not create relationships between them.
      Try to use helper class to do that.
```