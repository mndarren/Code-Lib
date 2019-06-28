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
