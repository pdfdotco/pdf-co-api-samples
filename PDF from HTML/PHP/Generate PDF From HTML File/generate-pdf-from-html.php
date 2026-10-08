<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>HTML to PDF Result</title>
</head>
<body>

<?php 

// Get submitted form data
$apiKey = $_POST["apiKey"]; // The authentication key (API Key). Get your own by registering at https://app.pdf.co

// HTML input
$inputHtml = file_get_contents("./inputHtml.html");

// Prepare URL for HTML to PDF API call
$url = "https://api.pdf.co/v1/pdf/convert/from/html";


// Prepare requests params
// See documentation: https://apidocs.pdf.co/?#1-json-pdfconvertfromhtml
$parameters = array();

// Input HTML code to be converted. Required. 
$parameters["html"] = utf8_encode($inputHtml);

// Name of resulting file
$parameters["name"] = "result.pdf";

// Set to css style margins like 10 px or 5px 5px 5px 5px.
$parameters["margins"] =  "5px 5px 5px 5px";

// Can be Letter, A4, A5, A6 or custom size like 200x200
$parameters["paperSize"] = "Letter";

// Set to Portrait or Landscape. Portrait by default.
$parameters["orientation"] = "Portrait";

// true by default. Set to false to disbale printing of background.
$parameters["printBackground"] = true;

// If large input document, process in async mode by passing true
$parameters["async"] = false;

// Set to HTML for header to be applied on every page at the header.
$parameters["header"] = "";

// Set to HTML for footer to be applied on every page at the bottom.
$parameters["footer"] = "";


// Create Json payload
$data = json_encode($parameters);

// Create request
$curl = curl_init();
curl_setopt($curl, CURLOPT_HTTPHEADER, array("x-api-key: " . $apiKey, "Content-type: application/json"));
curl_setopt($curl, CURLOPT_URL, $url);
curl_setopt($curl, CURLOPT_POST, true);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
curl_setopt($curl, CURLOPT_POSTFIELDS, $data);

// Execute request
$result = curl_exec($curl);

if (curl_errno($curl) == 0)
{
    $status_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);
    
    if ($status_code == 200)
    {
        $json = json_decode($result, true);
        
        if (isset($json["error"]) && $json["error"] == true)
        {
            // Display service reported error
            echo "<p>Error: " . $json["message"] . "</p>"; 
        }
        else
        {
            $resultFileUrl = $json["url"];
            
            // Display link to the file with conversion results
            echo "<div><h2>Conversion Result:</h2><a href='" . $resultFileUrl . "' target='_blank'>" . $resultFileUrl . "</a></div>";
        }
    }
    else
    {
        // Display request error
        echo "<p>Status code: " . $status_code . "</p>"; 
        echo "<p>" . $result . "</p>";
    }
}
else
{
    // Display CURL error
    echo "Error: " . curl_error($curl);
}

// Cleanup
curl_close($curl);

?>

</body>
</html>