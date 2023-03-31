# Setup LabView Pipeline
=====================================
1. Install NI LabVIEW 2023 Q1, 32 bit
2. In VI Package Manager, install NI Modbus library & OpenG Toolkit library
3. Install Daikin DevTool.msi
4. Setup Window security to Daikin Applied Certificate
5. Generate Token
```
1) Click Personal access tokens on DevOps Profile
2) Create New Token, by token name, organization, expiration
3) Copy the Token
```
6. Setup local agent
```
Ref: https://social.technet.microsoft.com/wiki/contents/articles/53386.azure-devops-create-configure-agent.aspx
1) Create New Agent Pool on DevOps, called DAA_Controls
2) Create New Agent, called DAA_Controls_Local_Agent
3) Download agent zip package to machine
4) Create agent folder and extract files
  PS C:\> mkdir agent ; cd agent  
  PS C:\agent> Add-Type -AssemblyName System.IO.Compression.FileSystem ; [System.IO.Compression.ZipFile]::ExtractToDirectory("$HOME\Downloads\vsts-agent-win-x64-2.159.2.zip", "$PWD")
5) Configure the Agent
  Enter server URL: https://dev.azure.com/daikinapplied
  Enter authentication type (press enter for PAT):
  Enter personal access token: copy paste token
  Enter agent pool name: DAA_Controls
  Enter agent name: DAA_Controls_Local_Agent
  Enter work folder (press enter for _work):
  Enter run agent as service? (Y/N) (press enter for N) : Y
  Enter User account to use for the service (press enter for NT AUTHORITY\NETWORK SERVICE) : Y
```
6. Run the Agent: PS C:\agent> .\run.cmd
7. Configure VI Server, Tools->Options->VI Server->Machine Access, Add 127.0.0.1 -> OK
8. Setup startup
```
Windows Logo + R
shell:startup
# create a run_agent.bat file with the following content
START /MIN C:\Users\XieZ\Tools\vsts-agent-win-x64-2.217.2\run.cmd
```
