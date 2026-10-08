## How to read barcode from URL asynchronously for barcode reader API in PowerShell with PDF.co Web API

### Learn how to read barcode from URL asynchronously to have barcode reader API in PowerShell

Sample source codes below will show you how to cope with a difficult task, for example, barcode reader API in PowerShell. PDF.co Web API was made to help with barcode reader API in PowerShell. PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

This rich sample source code in PowerShell for PDF.co Web API includes the number of functions and options you should do calling the API to implement barcode reader API. This PowerShell sample code should be copied and pasted into your project. After doing this just compile your project and click Run. This basic programming language sample code for PowerShell will do the whole work for you in implementing barcode reader API in your app.

Trial version of ByteScout is available for free download from our website. This and other source code samples for PowerShell and other programming languages are available.

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

##### **ReadBarcodeFromUrlAsynchronously.ps1:**
    
```
# Cloud API asynchronous "Barcode Reader" job example.
# Allows to avoid timeout errors when processing huge or scanned PDF documents.

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "***********************************"

# Direct URL of source file to search barcodes in.
$SourceFileURL = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/barcode-reader/sample.pdf"
# Comma-separated list of barcode types to search. 
# See valid barcode types in the documentation https://apidocs.pdf.co
$BarcodeTypes = "Code128,Code39,Interleaved2of5,EAN13"
# Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
$Pages = ""
# (!) Make asynchronous job
$Async = $true


# Prepare URL for `Barcode Reader` API call
$query = "https://api.pdf.co/v1/barcode/read/from/url"

# Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
# See documentation: https://apidocs.pdf.co
$body = @{
    "types" = $BarcodeTypes
    "pages" = $Pages
    "url" = $SourceFileURL
    "async" = $Async
} | ConvertTo-Json

try {
    # Execute request
    $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query

    $jsonResponse = $response.Content | ConvertFrom-Json

    if ($jsonResponse.error -eq $false) {
        # Asynchronous job ID
        $jobId = $jsonResponse.jobId
        # URL of generated JSON file with decoded barcodes that will available after the job completion
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
                # Download JSON file with decoded barcodes
                $jsonFoundBarcodes = Invoke-RestMethod -Method Get -Headers @{ "x-api-key" = $API_KEY } -Uri $resultFileUrl
                
                # Display found barcodes in console
                foreach ($barcode in $jsonFoundBarcodes)
                {
                    Write-Host "Found barcode:"
                    Write-Host "  Type: " + $barcode.TypeName
                    Write-Host "  Value: " + $barcode."Value"
                    Write-Host "  Document Page Index: " + $barcode."Page"
                    Write-Host "  Rectangle: " + $barcode."Rect"
                    Write-Host "  Confidence: " + $barcode."Confidence"
                    Write-Host ""
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

```

<!-- code block end -->    

<!-- code block begin -->

##### **run.bat:**
    
```
@echo off

powershell -NoProfile -ExecutionPolicy Bypass -Command "& .\ReadBarcodeFromUrlAsynchronously.ps1"
echo Script finished with errorlevel=%errorlevel%

pause
```

<!-- code block end -->