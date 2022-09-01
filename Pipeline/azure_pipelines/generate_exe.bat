REM Setup ExeFilename dynamically based on different environment
echo off
echo IS_DEV is %IS_DEV%
echo DEV_ExeFilename is %DEV_ExeFilename%
IF %IS_QA% neq True IF %IS_PROD% neq True (SET ExeFilename=%DEV_ExeFilename%)
IF %IS_QA% equ True (SET ExeFilename=%QA_ExeFilename%)
IF %IS_PROD% equ True (SET ExeFilename=%PROD_ExeFilename%)
echo ExeFilename is %ExeFilename%

:: Generate EXE file
cd "%src%\%wixExeDirProjectName%"
"%WIX%\bin\candle" -d"DevEnvDir=%DevEnvDir%\" -dSolutionDir=%src%\ -dSolutionExt=.sln -dSolutionFileName=%solutionName%.sln -dSolutionName=%solutionName% -dSolutionPath=%src%\%solutionName%.sln -dConfiguration=%buildConfiguration% -dOutDir=bin\%buildConfiguration%\ -dPlatform=x86 -dProjectDir=%src%\%wixExeDirProjectName%\ -dProjectExt=.wixproj -dProjectFileName=%wixExeDirProjectName%.wixproj -dProjectName=%wixExeDirProjectName% -dProjectPath=%src%\%wixExeDirProjectName%\%wixExeDirProjectName%.wixproj -dTargetDir=%src%\%wixExeDirProjectName%\bin\%buildConfiguration%\ -dTargetExt=.exe -dTargetFileName=%ExeFilename%.exe -dTargetName=%ExeFilename% -dTargetPath=%src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%ExeFilename%.exe -d%wixBuildDirProjectName%.Configuration=%buildConfiguration% -d"%wixBuildDirProjectName%.FullConfiguration=%buildConfiguration%|x86" -d%wixBuildDirProjectName%.Platform=x86 -d%wixBuildDirProjectName%.ProjectDir=%src%\%wixBuildDirProjectName%\ -d%wixBuildDirProjectName%.ProjectExt=.wixproj -d%wixBuildDirProjectName%.ProjectFileName=%wixBuildDirProjectName%.wixproj -d%wixBuildDirProjectName%.ProjectName=%wixBuildDirProjectName% -d%wixBuildDirProjectName%.ProjectPath=%src%\%wixBuildDirProjectName%\%wixBuildDirProjectName%.wixproj -d%wixBuildDirProjectName%.TargetDir=%src%\%wixBuildDirProjectName%\bin\%buildConfiguration%\ -d%wixBuildDirProjectName%.TargetExt=.msi -d%wixBuildDirProjectName%.TargetFileName=%wixBuildDirProjectName%.msi -d%wixBuildDirProjectName%.TargetName=%wixBuildDirProjectName% -d%wixBuildDirProjectName%.TargetPath=%src%\%wixBuildDirProjectName%\bin\%buildConfiguration%\%wixBuildDirProjectName%.msi -out obj\%buildConfiguration%\ -arch x86 -ext "%WIX%\bin\\WixUtilExtension.dll" -ext "%WIX%\bin\\WixNetFxExtension.dll" -ext "%WIX%\bin\\WixBalExtension.dll" Bundle.wxs
"%WIX%\bin\light" -out %src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%ExeFilename% -pdbout %src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%wixExeDirProjectName%.wixpdb -ext "%WIX%\bin\\WixUtilExtension.dll" -ext "%WIX%\bin\\WixNetFxExtension.dll" -ext "%WIX%\bin\\WixBalExtension.dll" -loc MyBootstrapperTheme.wxl -contentsfile obj\%buildConfiguration%\%wixExeDirProjectName%.wixproj.BindContentsFileList.txt -outputsfile obj\%buildConfiguration%\%wixExeDirProjectName%.wixproj.BindOutputsFileList.txt -builtoutputsfile obj\%buildConfiguration%\%wixExeDirProjectName%.wixproj.BindBuiltOutputsFileList.txt -wixprojectfile %src%\%wixExeDirProjectName%\%wixExeDirProjectName%.wixproj obj\%buildConfiguration%\Bundle.wixobj
"%WIX%\bin\insignia" -ib %src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%ExeFilename% -out %src%\%wixExeDirProjectName%\obj\%buildConfiguration%\%ExeFilename%
"%SIGNTOOL%" sign /sm /n "%SIGNName%" /t %SIGNTime% "%src%\%wixExeDirProjectName%\obj\%buildConfiguration%\%ExeFilename%"
"%WIX%\bin\insignia" -ab %src%\%wixExeDirProjectName%\obj\%buildConfiguration%\%ExeFilename% %src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%ExeFilename% -out %src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%ExeFilename%
"%SIGNTOOL%" sign /sm /n "%SIGNName%" /t %SIGNTime% "%src%\%wixExeDirProjectName%\bin\%buildConfiguration%\%ExeFilename%"