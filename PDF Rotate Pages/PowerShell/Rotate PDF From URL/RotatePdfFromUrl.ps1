# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "***********************************"

# Direct URL of source PDF file.
$SourceFileURL = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-optimize/sample.pdf"

# Angle in degrees. Supported values are 90, 180, 270.
$Angle = 90

# Comma-separated list of page indices (or ranges) to process. Example: '0,3-5,7-'. For ALL pages just leave this param empty
$Pages = "0-2,4"

# Destination PDF file name
$DestinationFile = ".\result.pdf"


# Prepare URL for `Rotate PDF` API call
$query = "https://api.pdf.co/v1/pdf/edit/rotate"

# Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
# See documentation: https://apidocs.pdf.co
$body = @{
    "url" = $SourceFileURL
    "name" = $(Split-Path $DestinationFile -Leaf)
    "angle" = $Angle
    "pages" = $Pages
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
