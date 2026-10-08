## How to read barcode from uploaded file for barcode reader API in PowerShell with PDF.co Web API

### See how to read barcode from uploaded file to have barcode reader API in PowerShell

Source code documentation samples provide quick and easy way to add a required functionality into your application. Barcode reader API in PowerShell can be implemented with PDF.co Web API. PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

Fast application programming interfaces of PDF.co Web API for PowerShell plus the instruction and the code below will help to learn how to read barcode from uploaded file. Follow the instruction and copy - paste code for PowerShell into your project's code editor. Tutorials are available along with installed PDF.co Web API if you'd like to dive deeper into the topic and the details of the API.

Our website provides free trial version of PDF.co Web API that includes source code samples to help with your PowerShell project.

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

##### **ReadBarcodeFromUploadedFile.ps1:**
    
```
# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "***********************************"

# Source file name
$SourceFile = ".\sample.pdf"
# Comma-separated list of barcode types to search. 
# See valid barcode types in the documentation https://apidocs.pdf.co
$BarcodeTypes = "Code128,Code39,Interleaved2of5,EAN13"
# Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
$Pages = ""


# 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
# * If you already have a direct file URL, skip to the step 3.

# Prepare URL for `Get Presigned URL` API call
$query = "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=" + `
    [System.IO.Path]::GetFileName($SourceFile)
$query = [System.Uri]::EscapeUriString($query)

try {
    # Execute request
    $jsonResponse = Invoke-RestMethod -Method Get -Headers @{ "x-api-key" = $API_KEY } -Uri $query
    
    if ($jsonResponse.error -eq $false) {
        # Get URL to use for the file upload
        $uploadUrl = $jsonResponse.presignedUrl
        # Get URL of uploaded file to use with later API calls
        $uploadedFileUrl = $jsonResponse.url

        # 2. UPLOAD THE FILE TO CLOUD.

        $r = Invoke-WebRequest -Method Put -Headers @{ "x-api-key" = $API_KEY; "content-type" = "application/octet-stream" } -InFile $SourceFile -Uri $uploadUrl
        
        if ($r.StatusCode -eq 200) {
            
            # 3. READ BARCODES FROM UPLOADED FILE

            # Prepare URL for `Barcode Reader` API call
            $query = "https://api.pdf.co/v1/barcode/read/from/url"

            # Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
            # See documentation: https://apidocs.pdf.co
            $body = @{
                "types" = $BarcodeTypes
                "pages" = $Pages
                "url" = $uploadedFileUrl
            } | ConvertTo-Json
            
            # Execute request
            $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query
            
            $jsonResponse = $response.Content | ConvertFrom-Json
            
            if ($jsonResponse.error -eq $false) {
                # Display found barcodes in console
                foreach ($barcode in $jsonResponse.barcodes)
                {
                    Write-Host "Found barcode:"
                    Write-Host "  Type: $($barcode.TypeName)"
                    Write-Host "  Value: $($barcode.Value)"
                    Write-Host "  Document Page Index: $($barcode.Page)"
                    Write-Host "  Rectangle: $($barcode.Rect)"
                    Write-Host "  Confidence: $($barcode.Confidence)"
                    Write-Host ""
                }
            }
            else {
                # Display service reported error
                Write-Host $jsonResponse.message
            }
        }
        else {
            # Display request error status
            Write-Host $r.StatusCode + " " + $r.StatusDescription
        }
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

powershell -NoProfile -ExecutionPolicy Bypass -Command "& .\ReadBarcodeFromUploadedFile.ps1"
echo Script finished with errorlevel=%errorlevel%

pause
```

<!-- code block end -->