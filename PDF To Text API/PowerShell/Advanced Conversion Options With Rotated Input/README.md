## PDF to text API in PowerShell with PDF.co Web API

### Build PDF to text API in PowerShell: ### Step-by-step instructions on how to do PDF to text API in PowerShell

This page helps you to learn from code samples for programming in PowerShell. PDF to text API in PowerShell can be applied with PDF.co Web API. PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

PowerShell code snippet like this for PDF.co Web API works best when you need to quickly implement PDF to text API in your PowerShell application.  Just copy and paste this PowerShell sample code to your PowerShell application's code editor, add a reference to PDF.co Web API (if you haven't added yet) and you are ready to go! Check these PowerShell sample code examples to see if they acknowledge to your needs and requirements for the project.

PDF.co Web API free trial version is available for download from our website. Free trial also includes programming tutorials along with source code samples.

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

##### **ConvertPdfToTxtFromUploadedFile.ps1:**
    
```
# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "********************************"

# Source PDF file
$SourceFile = ".\sample-rotated.pdf"
# Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
$Pages = ""
# PDF document password. Leave empty for unprotected documents.
$Password = ""
# Destination TXT file name
$DestinationFile = ".\result.txt"

# Some of advanced options available through profiles:
# (JSON can be single/double-quoted and contain comments.)
# {
#     "profiles": [
#         {
#             "profile1": {                
#                 "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
#                 "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
#                 "ExtractAnnotations": true, // Whether to extract PDF annotations.
#                 "CheckPermissions": true, // Ignore document permissions. Values: true / false
#                 "DetectNewColumnBySpacesRatio": 1.2, // A ratio affecting number of spaces between words. 
#             }
#         }
#     ]
# }

# Sample profile that sets advanced conversion options
# Advanced options are properties of CSVExtractor class from ByteScout PDF Extractor SDK used in the back-end:
# https://cdn.bytescout.com/help/BytescoutPDFExtractorSDK/html/87ce5fa6-3143-167d-abbd-bc7b5e160fe5.htm

# Valid RotationAngle values:
# 0 - no rotation
# 1 - 90 degrees
# 2 - 180 degrees
# 3 - 270 degrees
$Profiles = '{ "profiles": [{ "profile1": { "RotationAngle": 1 } } ] }'


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
            
            # 3. CONVERT UPLOADED PDF FILE TO TXT

            # Prepare URL for `PDF To TXT` API call
            $query = "https://api.pdf.co/v1/pdf/convert/to/text"

            # Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
            # See documentation: https://apidocs.pdf.co
            $body = @{
                "name" = $(Split-Path $DestinationFile -Leaf)
                "password" = $Password
                "pages" = $Pages
                "url" = $uploadedFileUrl
                "profiles" = $Profiles
            } | ConvertTo-Json
            
            # Execute request
            $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query
            
            $jsonResponse = $response.Content | ConvertFrom-Json
            
            if ($jsonResponse.error -eq $false) {
                # Get URL of generated TXT file
                $resultFileUrl = $jsonResponse.url;
                
                # Download TXT file
                Invoke-WebRequest -Headers @{ "x-api-key" = $API_KEY } -OutFile $DestinationFile -Uri $resultFileUrl

                Write-Host "Generated TXT file saved as `"$($DestinationFile)`" file."
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

powershell -NoProfile -ExecutionPolicy Bypass -Command "& .\ConvertPdfToTxtFromUploadedFile.ps1"
echo Script finished with errorlevel=%errorlevel%

pause
```

<!-- code block end -->