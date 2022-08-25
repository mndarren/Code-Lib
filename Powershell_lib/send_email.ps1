# Send notification email to DVT team when publishing EXE to QA
# Reference: https://dzone.com/articles/how-to-send-an-email-with-attachement-powershell-sendgrid-api
Function SendGridMailWithAttachment {
  param (
      [cmdletbinding()]
      [parameter()] [string] $ToAddress,
      [parameter()] [string] $FromAddress,
      [parameter()] [string] $Subject,
      [parameter()] [string] $Body,
      [parameter()] [string] $APIKey,
      [parameter()] [string] $FileName,
      [parameter()] [string] $FileNameWithFilePath,
      [parameter()] [string] $AttachementType
   )

  #Convert File to Base64
  $FileContent = get-content $FileNameWithFilePath
  $ConvertToBytes = [System.Text.Encoding]::UTF8.GetBytes($FileContent)
  $EncodedFile = [System.Convert]::ToBase64String($ConvertToBytes)
  
  # Body with attachement for SendGrid
  $SendGridBody = @{
      "personalizations" = @(
          @{
              "to"= @(
                            @{
                                 "email" = $ToAddress
                             }
               )
              "subject" = $Subject
           }
        )
              "content"= @(
                            @{
                                  "type" = "text/html"
                                  "value" = $Body
                             }
               )
              "from"  = @{
                          "email" = $FromAddress
                         }
              "attachments" = @(
                                  @{
                                      "content"=$EncodedFile
                                      "filename"=$FileName
                                      "type"= $AttachementType
                                      "disposition"="attachment"
                                   }
             )
  }
  $BodyJson = $SendGridBody | ConvertTo-Json -Depth 4

  #Header for SendGrid API
  $Header = @{
      "authorization" = "Bearer $APIKey"
  }

  #Send the email through SendGrid API
  $Parameters = @{
      Method      = "POST"
      Uri         = "https://api.sendgrid.com/v3/mail/send"
      Headers     = $Header
      ContentType = "application/json"
      Body        = $BodyJson
  }
  Invoke-RestMethod @Parameters
}

# Use the Email Sending Function
$Recipients = @(
    "user1@daikinapplied.com",
    "user2@gmail.com"
)

$Parameters = @{
  ToAddress   = "PlaceHolder"
  FromAddress = "user@daikinapplied.com"
  Subject     = "The AutoSim new version is available now."
  Body        = "The version is ready. Please download it. Please find the attachement."
  APIKey      = ""
  FileName         = "Token.txt"
  FileNameWithFilePath = "C:\Users\XXX\Token.txt"
  AttachementType  = "text/html"
}

for ($i=0; $i -lt $Recipients.Length; $i++) {
    $Parameters.ToAddress = $($Recipients[$i])
    SendGridMailWithAttachment @Parameters
}
