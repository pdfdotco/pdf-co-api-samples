## barcode generator API in PowerShell using PDF.co Web API What is PDF.co Web API? It is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://bytescout.zendesk.com/hc/en-us/requests/new?subject=PDF.co%20Web%20API%20Question)

or just send email to [support@bytescout.com](mailto:support@bytescout.com?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)
[Sign Up For Online Training](https://academy.bytescout.com/)


## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://apidocs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **GenerateBarcode.ps1:**
    
```
# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "****************************"

# Result file name
$ResultFile = ".\barcode.png"
# Barcode type. See valid barcode types in the documentation https://apidocs.pdf.co
$BarcodeType = "QRCode"
# Barcode value
$BarcodeValue = "QR123456\nhttps://pdf.co\nhttps://bytescout.com"

# Valid error correction levels:
# ----------------------------------
# Low - [default] Lowest error correction level. (Approx. 7% of codewords can be restored).
# Medium - Medium error correction level. (Approx. 15% of codewords can be restored).
# Quarter - Quarter error correction level (Approx. 25% of codewords can be restored).
# High - Highest error correction level (Approx. 30% of codewords can be restored).

# Set "Custom Profiles" parameter
$Profiles = "{ ""profiles"": [ { ""profile1"": { ""Options.QRErrorCorrectionLevel"": ""Quarter"" } } ] }"

$resultFileName = [System.IO.Path]::GetFileName($ResultFile)
$query = "https://api.pdf.co/v1/barcode/generate"

# Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
# See documentation: https://apidocs.pdf.co
$body = @{
    "name" = $resultFileName
    "type" = $BarcodeType
    "value" = $BarcodeValue
    "profiles" = $Profiles
} | ConvertTo-Json

try {
    # Execute request
    $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query

    $jsonResponse = $response.Content | ConvertFrom-Json

    if ($jsonResponse.error -eq $false) {
        # Get URL of generated barcode image file
        $resultFileUrl = $jsonResponse.url
        
        # Download the image file
        Invoke-WebRequest -Uri $resultFileUrl -OutFile $ResultFile

        Write-Host "Generated barcode saved to '$($ResultFile)' file."
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

```

<!-- code block end -->    

<!-- code block begin -->

##### **run.bat:**
    
```
@echo off

powershell -NoProfile -ExecutionPolicy Bypass -Command "& .\GenerateBarcode.ps1"
echo Script finished with errorlevel=%errorlevel%

pause
```

<!-- code block end -->