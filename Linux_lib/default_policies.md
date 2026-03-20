Default Policies:
==========================
1. Pipeline default timeout is **1 hour (60 minutes)**.
2. Azure DevOps will keep the Pipeline built artifacts for **one month (30 days)** by default.
3. AutoSim Private key will be expired in **10 years (Sep. 2032)**.
4. The EXE files in AutoSim Dev folder and Service Tools Dev folder will be auto deleted in **one week (7 days)**. The cleanup action will perform on each weekend.
5. The Application Team email is ControlsApplicationTeam@daikinapplied.com, which is used to auto send notification email to team members. For example, when a new version is ready to test, or when the RT Pipeline is done and the test result is ready to check out, the Pipeline will auto send notification emails to the DVT team.
6. The **SendGrid** account is registered by Controls Application Team email. Since it's a free account, **the max number of sending emails per day is 100**.
7. The Application Team Service Account is SA_DAAControlsPipeline@daikinapplied.com, which is used to login the Pipeline servers, including Test Machine, LabVIEW online VM, and RT Pipeline servers.
8. The SharePoint API, **Tahoe API**, is used to download and upload actions with the SharePoint. **The URL default folder is Staging** folder.
