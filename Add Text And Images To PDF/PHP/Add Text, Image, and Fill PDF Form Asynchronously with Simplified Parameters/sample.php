
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Add Text and Image from Scratch</title>
</head>
<body>

<?php

// Main Script
$apiKey = "YOUR_PDFCO_API_KEY"; // Replace with your API key
$url = "https://api.pdf.co/v1/pdf/edit/add";

$parameters = array(
    "async" => true,
    "encrypt" => false,
    "inline" => true,
    "name" => "output",
    "url" => "https://pdfco-test-files.s3.us-west-2.amazonaws.com/pdf-form/f1040.pdf",
    "annotationsString" => "250;20;0-;PDF form filled with PDF.co API;24+bold+italic+underline+strikeout;Arial;FF0000;www.pdf.co;true",
    "imagesString" => "100;180;0-;bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png|400;180;0-;bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png;www.pdf.co;200;200",
    "fieldsString" => "1;topmostSubform[0].Page1[0].f1_02[0];John A. Doe|1;topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1];true|1;topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0];123456789"

);

$response = sendRequest($url, $parameters, $apiKey);
$json = json_decode($response["result"], true);

if ($response["error"]) {
    echo "<p>Error: {$response["error"]}</p>";
} elseif ($response["status_code"] != 200) {
    echo "<p>Status code: {$response["status_code"]}</p>";
    echo "<p>Error: {$response["result"]}</p>";
} elseif (isset($json["error"]) && $json["error"] == true) {
    echo "<p>Error: {$json["message"]}</p>";
} else {
    $jobId = $json["jobId"];
    CheckJobStatus($jobId, $apiKey);
}

// Function to send HTTP request
function sendRequest($url, $parameters, $apiKey) {
    $data = json_encode($parameters);

    $curl = curl_init();
    curl_setopt_array($curl, array(
        CURLOPT_URL => $url,
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_POST => true,
        CURLOPT_HTTPHEADER => array(
            "x-api-key: $apiKey",
            "Content-Type: application/json"
        ),
        CURLOPT_POSTFIELDS => $data
    ));

    $result = curl_exec($curl);
    $error = curl_error($curl);
    $status_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);

    curl_close($curl);

    return array("result" => $result, "error" => $error, "status_code" => $status_code);
}

// Function to check job status
function CheckJobStatus($jobId, $apiKey) {
    $url = "https://api.pdf.co/v1/job/check";

    do {
        $response = sendRequest($url, array("jobid" => $jobId), $apiKey);
        $json = json_decode($response["result"], true);

        if ($response["error"]) {
            echo "<p>Error: {$response["error"]}</p>";
            break;
        }

        if ($response["status_code"] != 200) {
            echo "<p>Status code: {$response["status_code"]}</p>";
            echo "<p>Error checking job status: {$response["result"]}</p>";
            break;
        }

        if (isset($json["error"]) && $json["error"] == true) {
            echo "<p>Error: {$json["message"]}</p>";
            break;
        }

        $status = $json["status"];
        if ($status === "success") {
            $resultFileUrl = $json["url"];
            echo "<p>Job completed successfully. Download result: <a href='$resultFileUrl' target='_blank'>$resultFileUrl</a></p>";
            break;
        } elseif ($status === "working") {
            sleep(3);
        } else {
            echo "<p>Job status: $status</p>";
            break;
        }
    } while (true);
}

?>

</body>
</html>
