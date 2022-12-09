:: Unit Test Commands
REM MS Build
msbuild AAH_AutoSim.sln /t:Clean;Rebuild
REM Run Unit Tests and store result to JSON
dotnet test --no-build /p:CollectCoverage=true
REM Calculate Coverage of Unit Test
dotnet test --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Exclude="[xunit*]\*" /p:CoverletOutput="./TestResults/"
REM Generate Test Report
cd Tests
dotnet reportgenerator "-reports:TestResults\coverage.cobertura.xml" "-targetdir:TestResults\html" -reporttypes:HTML;
REM Open Test Report
cd TestResults\html
start index.html
cd ..\..\..
