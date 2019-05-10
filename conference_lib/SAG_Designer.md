# Link SF to SAG by CloudStreams Adapter
==================================================
1. Package structure
```
-Package_One
  |-Package_One (folder)
     |-adapters (folder)
     |-jms (folder)
     |-services (folder)
     |-webServices (folder)
         |-providers (folder)
         |-consumers (folder)
```
2. Create Web Service provider (Provide a WSDL)
```
   using WSD to do the operation
   choose SOAP type so that after creating it we can test it by SOAP UI (maybe good for REST too)
   web service will use your specific service.
   On tab 'Operations', copy WSDL URL, and then test it by SOAP UI
   # summary: web service is to convert service to WSDL format.
```
3. Create Web Service consumer (Consume a WSDL)
```
   using WSD tool
   test it in Designer, to run Flow Service from connectors.
   in jdbc window tree tab, add connector and link input (City to inCity)
```
4. 
