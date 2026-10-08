# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
$API_KEY = "********************************"

# Source PDF file
$SourceFile = ".\sample-rotated.pdf"
# Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
$Pages = ""
# PDF document password. Leave empty for unprotected documents.
$Password = ""
# Destination XML file name
$DestinationFile = ".\result.xml"

# Some of advanced options available through profiles:
# (it can be single/double-quoted and contain comments.)
# {
# 	"profiles": [
# 		{
# 			"profile1": {
# 				"SaveImages": "None", // Whether to extract images. Values: "None", "Embed".
# 				"ImageFormat": "PNG", // Image format for extracted images. Values: "PNG", "JPEG", "GIF", "BMP".
# 				"SaveVectors": false, // Whether to extract vector objects (vertical and horizontal lines). Values: true / false
# 				"ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
# 				"ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
# 				"LineGroupingMode": "None", // Values: "None", "GroupByRows", "GroupByColumns", "JoinOrphanedRows"
# 				"ColumnDetectionMode": "ContentGroupsAndBorders", // Values: "ContentGroupsAndBorders", "ContentGroups", "Borders", "BorderedTables"
# 				"Unwrap": false, // Unwrap grouped text in table cells. Values: true / false
# 				"ShrinkMultipleSpaces": false, // Shrink multiple spaces in table cells that affect column detection. Values: true / false
# 				"DetectNewColumnBySpacesRatio": 1, // Spacing ratio that affects column detection.
# 				"CustomExtractionColumns": [ 0, 50, 150, 200, 250, 300 ], // Explicitly specify columns coordinates for table extraction.
# 				"CheckPermissions": true, // Ignore document permissions. Values: true / false
# 			}
# 		}
# 	]
# }

# Sample profile that sets advanced conversion options
# Advanced options are properties of XMLExtractor class from ByteScout PDF Extractor SDK used in the back-end:
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
            
            # 3. CONVERT UPLOADED PDF FILE TO XML

            # Prepare URL for `PDF To XML` API call
            $query = "https://api.pdf.co/v1/pdf/convert/to/xml"

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
                # Get URL of generated XML file
                $resultFileUrl = $jsonResponse.url;
                
                # Download XML file
                Invoke-WebRequest -Headers @{ "x-api-key" = $API_KEY } -OutFile $DestinationFile -Uri $resultFileUrl

                Write-Host "Generated XML file saved as `"$($DestinationFile)`" file."
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
