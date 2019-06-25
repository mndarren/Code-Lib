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