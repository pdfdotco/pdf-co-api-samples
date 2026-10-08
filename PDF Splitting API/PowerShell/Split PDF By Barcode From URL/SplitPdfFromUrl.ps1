# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "***********************************"

# Source PDF file to split
$SourceFileUrl = "https://bytescout-com.s3.us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-split/split_by_barcode.pdf"
# Split by qr code or datamatrix with value search with regex
$SplitText = "[[barcode:qrcode,datamatrix /bytescout\\.com/]]"


# Prepare URL for `Split PDF By Barcode` API call
$query = "https://api.pdf.co/v1/pdf/split2"

# Prepare request body (will be auto-converted to JSON by Invoke-RestMethod)
# See documentation: https://apidocs.pdf.co
$body = @{
    "searchString" = $SplitText
    "url" = $SourceFileUrl
} | ConvertTo-Json

try {
    # Execute request
    $response = Invoke-WebRequest -Method Post -Headers @{ "x-api-key" = $API_KEY; "Content-Type" = "application/json" } -Body $body -Uri $query

    $jsonResponse = $response.Content | ConvertFrom-Json

    if ($jsonResponse.error -eq $false) {
        # Download generated PDF files
        $part = 1;
        foreach ($url in $jsonResponse.urls) {
            $localFileName = ".\part$($part).pdf"

            # Download PDF file
            Invoke-WebRequest -Headers @{ "x-api-key" = $API_KEY } -OutFile $localFileName -Uri $url

            Write-Host "Downloaded `"$($localFileName)`""
            $part++
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
