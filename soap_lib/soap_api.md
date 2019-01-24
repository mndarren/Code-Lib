# How to convert wsdl to java code
=======================================
1. WSDLToJava Error: Rpc/encoded wsdls are not supported with CXF, if use="encoding" in soap body, not work for CXF
2. How to install axis2 in Ubuntu?
```
# Download Apache Axis2 first
cd /opt/
sudo unzip ~/Downloads/axis2-1.7.8-bin.zip -d .
sudo mv axis2-1.7.8/ axis2/
sudo chmod -R 777 axis2/
sudo gedit /etc/environment
AXIS2_HOME="/opt/axis2"
# add the above line in the file
/opt/axis2/bin
# add the above to the PATH
source /etc/environment
# in /opt/axis2/bin
axis2server.sh  # for test
```
3. How to setup SoapUI?
```
# Download SF WSDL
generate WSDL: login SF -> setup -> api -> generate WSDL -> generate
# Create a new SoapUI project with WSDL file path and project name
Preferences -> tools -> axis2 -> /opt/axis2
Tools -> Axis 2 artifact -> fill up WSDL, output dir, databinding method: xmlbeans, check sync style -> Generate
# If error, copy command to run in terminal (in /opt/axis2/bin)
./wsdl2java.sh -uri /path/to/xxx.wsdl -o /path/to/output -d xmlbeans -ns2p
```
