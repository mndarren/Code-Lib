param ($uploadFileFrom, $uploadLocation, $uploadFilename, $ssoTokenUrl, $ssoClientId, $ssoClientSecret, $ssoScope, $ssoGrantType, $spoDocId)
Write-Host
Write-Host "Retrieve Access Token..."

$ssoHeaders = @{
    "Content-Type" = "application/x-www-form-urlencoded"
    "User-Agent" = "PowerShell"
    "Accept" = "*/*"
};
$ssoCreds = @{
    "client_id" = $ssoClientId
    "client_secret" = $ssoClientSecret
    "grant_type" = $ssoGrantType
    "scope" = $ssoScope
};

Write-Host "SSO Endpoint: $ssoTokenUrl"
$ssoResponse = Invoke-RestMethod $ssoTokenUrl -Method Post -Body $ssoCreds -Headers $ssoHeaders;
$token = $ssoResponse.access_token;
Write-Host "Token: $token";

Write-Host;
Write-Host "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

Write-Host;
Write-Host "Make WebAPI Call...";
$url = 'https://tahoestg.dev.daikinapplied.com/api/reports/uploadcontrol'
$fileContentEncoded = [System.Convert]::ToBase64String([System.IO.File]::ReadAllBytes($uploadFileFrom))

$apiHeader = @{"authorization" = "bearer $token"};
$data = ConvertTo-Json @{ 
           SPODocId = $spoDocId
           FileContent = $fileContentEncoded
           FileName = $uploadFilename
           FolderName = $uploadLocation
          }

$response = Invoke-RestMethod -Uri $url -Method Post -Body $data -Header $apiHeader -ContentType "application/json"
Write-Host "File upload result:" 
Write-Host $response
