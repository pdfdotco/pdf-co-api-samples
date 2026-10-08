# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "********************************"

# Direct URL of source PDF file.
$SourceFileURL = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf"
# Destination PDF file name
$DestinationFile = ".\protected.pdf"

# Passwords to protect PDF document
# The owner password will be required for document modification.
# The user password only allows to view and print the document.
$OwnerPassword = "123456"
$UserPassword = "654321"

# Encryption algorithm. 
# Valid values: "RC4_40bit", "RC4_128bit", "AES_128bit", "AES_256bit".
$EncryptionAlgorithm = "AES_128bit"

# Allow or prohibit content extraction for accessibility needs.
$AllowAccessibilitySupport = $True

# Allow or prohibit assembling the document.
$AllowAssemblyDocument = $True

# Allow or prohibit printing PDF document.
$AllowPrintDocument = $True

# Allow or prohibit filling of interactive form fields (including signature fields) in PDF document.
$AllowFillForms = $True

# Allow or prohibit modification of PDF document.
$AllowModifyDocument = $True

# Allow or prohibit copying content from PDF document.
$AllowContentExtraction = $True

# Allow or prohibit interacting with text annotations and forms in PDF document.
$AllowModifyAnnotations = $True

# Allowed printing quality.
# Valid values: "HighResolution", "LowResolution"
$PrintQuality = "HighResolution"

# Runs processing asynchronously. 
# Returns Use JobId that you may use with /job/check to check state of the processing (possible states: working, failed, aborted and success).
$async = $False


# Prepare URL for `PDF Security` API call
$query = "https://api.pdf.co/v1/pdf/security/add"

# Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
# See documentation: https://apidocs.pdf.co
$body = @{
    "name" = $(Split-Path $DestinationFile -Leaf)
    "url" = $SourceFileURL
    "ownerPassword" = $OwnerPassword
    "userPassword" = $UserPassword
    "encryptionAlgorithm" = $EncryptionAlgorithm
    "allowAccessibilitySupport" = $AllowAccessibilitySupport
    "allowAssemblyDocument" = $AllowAssemblyDocument
    "allowPrintDocument" = $AllowPrintDocument
    "allowFillForms" = $AllowFillForms
    "allowModifyDocument" = $AllowModifyDocument
    "allowContentExtraction" = $AllowContentExtraction
    "allowModifyAnnotations" = $AllowModifyAnnotations
    "printQuality" = $PrintQuality
    "async" = $async
} | ConvertTo-Json

try {
    # Execute request
    $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query

    $jsonResponse = $response.Content | ConvertFrom-Json

    if ($jsonResponse.error -eq $false) {
        # Get URL of generated PDF file
        $resultFileUrl = $jsonResponse.url;
        
        # Download PDF file
        Invoke-WebRequest -Headers @{ "x-api-key" = $API_KEY } -OutFile $DestinationFile -Uri $resultFileUrl

        Write-Host "Generated PDF file saved as `"$($DestinationFile)`" file."
    }
    else {
        # Display service reported error
        Write-Host $jsonResponse.message
    }
}
catch {
    # Display request error
    Write-Host $_.Exception
}