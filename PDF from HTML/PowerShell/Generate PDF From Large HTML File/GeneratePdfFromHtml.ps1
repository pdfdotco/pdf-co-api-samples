# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "**************************************"

# HTML input
$HtmlInput = [IO.File]::ReadAllText(".\sample.html")
# Destination PDF file name
$DestinationFile = ".\result.pdf"

# Prepare URL for HTML to PDF API call
$query = "https://api.pdf.co/v1/pdf/convert/from/html"
$query = [System.Uri]::EscapeUriString($query)

# Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
# See documentation: https://apidocs.pdf.co
$body = @{
    "html" = $HtmlInput
    "name" = $(Split-Path $DestinationFile -Leaf)
    "async" = $true # Make asychronous job
} | ConvertTo-Json

try {
    # Execute request
    $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query

    if ($response.StatusCode -eq 200) {
        
        $jsonResponse = $response.Content | ConvertFrom-Json

        if ($jsonResponse.error -eq $false) {
            # Asynchronous job ID
            $jobId = $jsonResponse.jobId
            # URL of generated PDF file that will available after the job completion
            $resultFileUrl = $jsonResponse.url

            # Check the job status in a loop. 
            do {
                $statusCheckUrl = "https://api.pdf.co/v1/job/check?jobid=" + $jobId
                $jsonStatus = Invoke-RestMethod -Method Get -Headers @{ "x-api-key" = $API_KEY } -Uri $statusCheckUrl

                # Display timestamp and status (for demo purposes)
                Write-Host "$(Get-date): $($jsonStatus.status)"

                if ($jsonStatus.status -eq "success") {
                    # Download PDF file
                    Invoke-WebRequest -Headers @{ "x-api-key" = $API_KEY } -OutFile $DestinationFile -Uri $resultFileUrl
                    Write-Host "Generated PDF file saved as `"$($DestinationFile)`" file."
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
    else {
        # Display request error status
        Write-Host $response.StatusCode + " " + $response.StatusDescription
    }
}
catch {
    # Display request error
    Write-Host $_.Exception
}
