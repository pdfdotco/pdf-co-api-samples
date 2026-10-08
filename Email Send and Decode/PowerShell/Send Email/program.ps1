# Please refer to our knowledge base at (https://apidocs.pdf.co/kb/Email%20Send%20(email-send)/index) for SMTP related information


$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$headers.Add("x-api-key", "Add_Your_PDFco_Api_Key")

$body = "{`n    `"url`": `"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-info/sample.pdf`",`n    `"from`": `"John Doe <john@example.com>`",`n    `"to`": `"Partner <partner@example.com>`",`n    `"subject`": `"Check attached sample pdf`",`n    `"bodytext`": `"Please check the attached pdf`",`n    `"bodyHtml`": `"Please check the attached pdf`",`n    `"smtpserver`": `"smtp.gmail.com`",`n    `"smtpport`": `"587`",`n    `"smtpusername`": `"my@gmail.com`", `n    `"smtppassword`": `"app specific password created as https://support.google.com/accounts/answer/185833`",`n    `"async`": false`n}"

$response = Invoke-RestMethod 'https://api.pdf.co/v1/email/send' -Method 'POST' -Headers $headers -Body $body
$response | ConvertTo-Json