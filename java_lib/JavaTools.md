# Java Tools
===============================
1. JAXB (Java Architecture XML Binding)  
   [JAXB Example](https://github.com/mndarren/Code-Lib/tree/master/java_lib/jaxb)

2. JMS (Java Message Service)  


3. ESB (Enterprise Service Bus)
   ```
   1) Advantages: Single Point of Access, Transaction Manager, Security Manager,
                  Service Proxy (Java and .Net), Gateway to the world
   2) Disadvantages: Single point failure risk, extra level indirection to decreased performance.
   ```
   ![ESB](https://github.com/mndarren/Code-Lib/blob/master/java_lib/resource/ESB.PNG)
4. EJB (Enterprise Java Beans)

5. JSP (Java Server Pages)

6. SOAP vs. REST
   ```
   No.   SOAP                                      REST
   ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
   1    XML-based message protocol                Architectural style
   2    Use WSDL for communication                Use SML or JSON
   3    Invoke services by calling RPC method     Simple call services via URL path
   4    no return human readable result           readable return with XML or JSON
   5    Transfer over HTTP, SMTP or FTP           only HTTP
   6    JavaScript can call SOAP, but difficult   easy to call from JavaScript
   7    Performance is not great compared to REST  Better performance
   ```