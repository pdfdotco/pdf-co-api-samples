## PDF to excel API in PowerShell and PDF.co Web API

### Learn PDF to excel API in PowerShell

Every ByteScout tool includes samplePowerShell source codes that you can find here or in the folder with installed ByteScout product. PDF.co Web API helps with PDF to excel API in PowerShell. PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

The below SDK samples describe how to quickly make your application do PDF to excel API in PowerShell with the help of PDF.co Web API. If you want to implement this functionality, you should copy and paste code below into your app using code editor. Then compile and run your application. Enjoy writing a code with ready-to-use sample PowerShell codes to implement PDF to excel API using PDF.co Web API.

Visit our website to get a free trial version of PDF.co Web API. Free trial contains many of source code samples to help you with your PowerShell project.

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

##### **ConvertPdfToXlsFromUploadedFile.ps1:**
    
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
# Destination XLS file name
$DestinationFile = ".\result.xls"

# Some of advanced options available through profiles:
# (JSON can be single/double-quoted and contain comments.)
# {
#     "profiles": [
#         {
#             "profile1": {
#                 "NumberDecimalSeparator": "", // Allows to customize decimal separator in numbers.
#                 "NumberGroupSeparator": "", // Allows to customize thousands separator.
#                 "AutoDetectNumbers": true, // Whether to detect numbers. Values: true / false
#                 "RichTextFormatting": true, // Whether to keep text style and fonts. Values: true / false
#                 "PageToWorksheet": true, // Whether to create separate worksheet for each page of PDF document. Values: true / false
#                 "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
#                 "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
#                 "LineGroupingMode": "None", // Values: "None", "GroupByRows", "GroupByColumns", "JoinOrphanedRows"
#                 "ColumnDetectionMode": "ContentGroupsAndBorders", // Values: "ContentGroupsAndBorders", "ContentGroups", "Borders", "BorderedTables"
#                 "Unwrap": false, // Unwrap grouped text in table cells. Values: true / false
#                 "ShrinkMultipleSpaces": false, // Shrink multiple spaces in table cells that affect column detection. Values: true / false
#                 "DetectNewColumnBySpacesRatio": 1, // Spacing ratio that affects column detection.
#                 "CustomExtractionColumns": [ 0, 50, 150, 200, 250, 300 ], // Explicitly specify columns coordinates for table extraction.
#                 "CheckPermissions": true, // Ignore document permissions. Values: true / false
#             }
#         }
#     ]
# }

# Sample profile that sets advanced conversion options
# Advanced options are properties of XLSExtractor class from ByteScout PDF Extractor SDK used in the back-end:
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
            
            # 3. CONVERT UPLOADED PDF FILE TO XLS

            # Prepare URL for `PDF To XLS` API call
            $query = "https://api.pdf.co/v1/pdf/convert/to/xls"

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
                # Get URL of generated XLS file
                $resultFileUrl = $jsonResponse.url;
                
                # Download XLS file
                Invoke-WebRequest -Headers @{ "x-api-key" = $API_KEY } -OutFile $DestinationFile -Uri $resultFileUrl

                Write-Host "Generated XLS file saved as `"$($DestinationFile)`" file."
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

powershell -NoProfile -ExecutionPolicy Bypass -Command "& .\ConvertPdfToXlsFromUploadedFile.ps1"
echo Script finished with errorlevel=%errorlevel%

pause
```

<!-- code block end -->