## How to PDF text search API in PowerShell using PDF.co Web API

### How to PDF text search API in PowerShell

The coding tutorials are designed to help you test the features without need to write your own code. What is PDF.co Web API? It is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf. It can help you to PDF text search API in your PowerShell application.

You will save a lot of time on writing and testing code as you may just take the PowerShell code from PDF.co Web API for PDF text search API below and use it in your application. In order to implement the functionality, you should copy and paste this code for PowerShell below into your code editor with your app, compile and run your application. You can use these PowerShell sample examples in one or many applications.

PDF.co Web API free trial version is available on our website. PowerShell and other programming languages are supported.

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

##### **PDFTextSearchFromUploadedFileAsync.ps1:**
    
```
# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "***********************************"

# Source file name
$SourceFile = ".\sample.pdf"

# Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
$Pages = ""

# PDF document password. Leave empty for unprotected documents.
$Password = ""

# Search string. 
$SearchString = '\d{1,}\.\d\d' #Regular expression to find numbers like '100.00'

# Enable regular expressions (Regex) 
$RegexSearch = 'True'

# (!) Make asynchronous job
$Async = $true

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
            
            # 3. TEXT SEARCH FROM UPLOADED FILE

            # Prepare URL for PDF text search API call.
            # See documentation: https://apidocs.pdf.co
            $query = "https://api.pdf.co/v1/pdf/find"

            # Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
            # See documentation: https://apidocs.pdf.co
            $body = @{
                "password" = $Password
                "pages" = $Pages
                "url" = $uploadedFileUrl
                "searchString" = $SearchString
                "regexSearch" = $RegexSearch
                "async" = $Async
            } | ConvertTo-Json

            try {
                # Execute request
                $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query

                $jsonResponse = $response.Content | ConvertFrom-Json
            
                if ($jsonResponse.error -eq $false) {
                    # Asynchronous job ID
                    $jobId = $jsonResponse.jobId
            
                    # URL of generated JSON file with search result that will available after the job completion
                    $resultFileUrl = $jsonResponse.url
            
                    # Check the job status in a loop. 
                    # If you don't want to pause the main thread you can rework the code 
                    # to use a separate thread for the status checking and completion.
                    do {
                        $statusCheckUrl = "https://api.pdf.co/v1/job/check?jobid=" + $jobId
                        $jsonStatus = Invoke-RestMethod -Method Get -Headers @{ "x-api-key" = $API_KEY } -Uri $statusCheckUrl
            
                        # Display timestamp and status (for demo purposes)
                        Write-Host "$(Get-date): $($jsonStatus.status)"
            
                        if ($jsonStatus.status -eq "success") {
                            # Get JSON for search result
                            $jsonSearchResult = Invoke-RestMethod -Method Get -Headers @{ "x-api-key" = $API_KEY } -Uri $resultFileUrl
                            
                            # Display found result in console
                            foreach ($item in $jsonSearchResult)
                            {
                                Write-Host "Found text $($item.text) at coordinates $($item.left), $($item.top)"
                            }
                            break
                        }
                        elseif ($jsonStatus.status -eq "working") {
                            # Pause for a few seconds
                            Start-Sleep -Seconds 3
                        }
                        else {
                            Write-Host $jsonStatus.status
                            break
                        }
                    }
                    while ($true)
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

powershell -NoProfile -ExecutionPolicy Bypass -Command "& .\PDFTextSearchFromUploadedFileAsync.ps1"
echo Script finished with errorlevel=%errorlevel%

pause
```

<!-- code block end -->