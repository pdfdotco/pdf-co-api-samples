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
