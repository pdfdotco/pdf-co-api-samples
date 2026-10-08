## How to parse invoice for document parser API in PHP with PDF.co Web API PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

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

##### **Google Invoice.json:**

```
{
  "templateName": "Google Invoice",
  "templateVersion": 4,
  "templatePriority": 0,
  "detectionRules": {
    "keywords": [
      "Google",
      "77-0493581",
      "Invoice"
    ]
  },
  "objects": [
    {
      "name": "invoiceId",
      "objectType": "field",
      "fieldProperties": {
        "expression": "Invoice number:{{Spaces}}({{Digits}})",
        "regex": true
      },
      "id": 0
    },
    {
      "name": "dateIssued",
      "objectType": "field",
      "fieldProperties": {
        "expression": "Issue date:{{Spaces}}({{SmartDate}})",
        "regex": true,
        "dataType": "date",
        "dateFormat": "MMM d, yyyy"
      },
      "id": 1
    },
    {
      "name": "total",
      "objectType": "field",
      "fieldProperties": {
        "expression": "Amount due in USD:{{Spaces}}{{Number}}",
        "regex": true,
        "dataType": "decimal"
      },
      "id": 2
    },
    {
      "name": "subTotal",
      "objectType": "field",
      "fieldProperties": {
        "expression": "Subtotal in USD:{{Spaces}}{{Number}}",
        "regex": true,
        "dataType": "decimal"
      },
      "id": 3
    },
    {
      "name": "taxRate",
      "objectType": "field",
      "fieldProperties": {
        "expression": "State sales tax {{OpeningParenthesis}}{{Digits}}{{Percent}}{{ClosingParenthesis}}",
        "regex": true,
        "dataType": "integer"
      },
      "id": 4
    },
    {
      "name": "tax",
      "objectType": "field",
      "fieldProperties": {
        "expression": "State sales tax{{Anything}}{{Number}}{{LineEnd}}",
        "regex": true,
        "dataType": "decimal"
      },
      "id": 5
    },
    {
      "name": "companyName",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "static",
        "expression": "Google LLC",
        "regex": true
      },
      "id": 6
    },
    {
      "name": "billTo",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "rectangle",
        "regex": true,
        "rectangle": [
          0,
          152,
          280,
          72
        ],
        "pageIndex": 0
      },
      "id": 7
    },
    {
      "name": "billingId",
      "objectType": "field",
      "fieldProperties": {
        "expression": "Billing ID:{{Spaces}}({{DigitsOrSymbols}})",
        "regex": true
      },
      "id": 8
    },
    {
      "name": "currency",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "static",
        "expression": "USD",
        "regex": true
      },
      "id": 9
    },
    {
      "name": "table1",
      "objectType": "table",
      "tableProperties": {
        "start": {
          "expression": "Description{{Spaces}}Interval{{Spaces}}Quantity{{Spaces}}Amount",
          "regex": true
        },
        "end": {
          "expression": "Subtotal in USD",
          "regex": true
        },
        "row": {
          "expression": "{{LineStart}}{{Spaces}}(?<description>{{SentenceWithSingleSpaces}}){{Spaces}}(?<interval>{{3Letters}}{{Space}}{{Digits}}{{Space}}{{Minus}}{{Space}}{{3Letters}}{{Space}}{{Digits}}){{Spaces}}(?<quantity>{{Digits}}){{Spaces}}(?<amount>{{Number}})",
          "regex": true
        },
        "columns": [
          {
            "name": "quantity",
            "dataType": "integer"
          },
          {
            "name": "amount",
            "dataType": "decimal"
          }
        ]
      },
      "id": 10
    }
  ],
  "culture": "en-US",
  "description": "",
  "options": {
    "ocrMode": "auto",
    "ocrLanguage": "eng",
    "ocrResolution": 300,
    "ocrImageFilters": "",
    "ocrWhiteList": "",
    "ocrBlackList": ""
  }
}
```

<!-- code block end -->    

<!-- code block begin -->

##### **program.php:**

```
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Document Parse Results</title>
</head>
<body>

<?php 

// Get submitted form data
$apiKey = $_POST["apiKey"]; // The authentication key (API Key). Get your own by registering at https://app.pdf.co/documentation/api


// 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
// * If you already have the direct PDF file link, go to the step 3.

// Create URL
$url = "https://api.pdf.co/v1/file/upload/get-presigned-url" . 
    "?name=" . $_FILES["fileInput"]["name"] .
    "&contenttype=application/octet-stream";
    
// Create request
$curl = curl_init();
curl_setopt($curl, CURLOPT_HTTPHEADER, array("x-api-key: " . $apiKey));
curl_setopt($curl, CURLOPT_URL, $url);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);

// Execute request
$result = curl_exec($curl);

if (curl_errno($curl) == 0)
{
    $status_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);
    
    if ($status_code == 200)
    {
        $json = json_decode($result, true);
        
        // Get URL to use for the file upload
        $uploadFileUrl = $json["presignedUrl"];
        // Get URL of uploaded file to use with later API calls
        $uploadedFileUrl = $json["url"];
        
        // 2. UPLOAD THE FILE TO CLOUD.
        
        $localFile = $_FILES["fileInput"]["tmp_name"];
        $fileHandle = fopen($localFile, "r");
        
        curl_setopt($curl, CURLOPT_URL, $uploadFileUrl);
        curl_setopt($curl, CURLOPT_HTTPHEADER, array("content-type: application/octet-stream"));
        curl_setopt($curl, CURLOPT_PUT, true);
        curl_setopt($curl, CURLOPT_INFILE, $fileHandle);
        curl_setopt($curl, CURLOPT_INFILESIZE, filesize($localFile));

        // Execute request
        curl_exec($curl);
        
        fclose($fileHandle);
        
        if (curl_errno($curl) == 0)
        {
            $status_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);
            
            if ($status_code == 200)
            {
                // Read all template texts
                $templateText = file_get_contents($_FILES["fileTemplate"]["tmp_name"]);

                // 3. PARSE UPLOADED PDF DOCUMENT
                ParseDocument($apiKey, $uploadedFileUrl, $templateText);
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
    }
    else
    {
        // Display service reported error
        echo "<p>Status code: " . $status_code . "</p>"; 
        echo "<p>" . $result . "</p>"; 
    }
    
    curl_close($curl);
}
else
{
    // Display CURL error
    echo "Error: " . curl_error($curl);
}

// Insert parsed output to db
function InsertToDb($resultFileUrl)
{
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "sample_db";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

//read the json file contents
$jsondata = file_get_contents($resultFileUrl);

$data = json_decode(preg_replace('/[\x00-\x1F\x80-\xFF]/', '', $jsondata), true);

//var_dump($data);

$invoiceno = $data['objects'][0]['value'];
$invoicedate = $data['objects'][1]['value'];
$invoicetotal = $data['objects'][2]['value'];

// echo "Invoice No: " . $invoiceno;
// echo "Invoice Date: " . $invoicedate;
// echo "Invoice Total: " . $invoicetotal;

$sql = "INSERT INTO sample_table (inv_no, inv_date, inv_total)
VALUES ('$invoiceno', '$invoicedate', '$invoicetotal')";

if ($conn->query($sql) === TRUE) {
  echo "New record created successfully";
} else {
  echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
}

function ParseDocument($apiKey, $uploadedFileUrl, $templateText) 
{
    // (!) Make asynchronous job
    $async = TRUE;

    // Prepare URL for Document parser API call.
    // See documentation: https://apidocs.pdf.co/?#1-pdfdocumentparser
    $url = "https://api.pdf.co/v1/pdf/documentparser";

    // Prepare requests params
    $parameters = array();
    $parameters["url"] = $uploadedFileUrl;
    $parameters["template"] = $templateText;
    $parameters["async"] = $async;

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
    echo $result . "<br/>";

    if (curl_errno($curl) == 0)
    {
        $status_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);
        
        if ($status_code == 200)
        {
            $json = json_decode($result, true);
        
            if (!isset($json["error"]) || $json["error"] == false)
            {
                // URL of generated JSON file that will available after the job completion
                $resultFileUrl = $json["url"];
				
                // Asynchronous job ID
                $jobId = $json["jobId"];
                
                // Check the job status in a loop
                do
                {
                    $status = CheckJobStatus($jobId, $apiKey); // Possible statuses: "working", "failed", "aborted", "success".
                    
                    // Display timestamp and status (for demo purposes)
                    echo "<p>" . date(DATE_RFC2822) . ": " . $status . "</p>";
                    
                    if ($status == "success")
                    {
                        // Display link to JSON file with information about parsed fields
                        echo "<div><h2>Parsing Result:</h2><a href='" . $resultFileUrl . "' target='_blank'>" . $resultFileUrl . "</a></div>";
						
						//Calls InsertToDb function to get key value pair from JSON file 
						InsertToDb($resultFileUrl);
                        break;
                    }
                    else if ($status == "working")
                    {
                        // Pause for a few seconds
                        sleep(3);
                    }
                    else 
                    {
                        echo $status . "<br/>";
                        break;
                    }
                }
                while (true);
            }
            else
            {
                // Display service reported error
                echo "<p>Error: " . $json["message"] . "</p>"; 
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
}

function CheckJobStatus($jobId, $apiKey)
{
    $status = null;
    
	// Create URL
    $url = "https://api.pdf.co/v1/job/check";
    
    // Prepare requests params
    $parameters = array();
    $parameters["jobid"] = $jobId;

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

            $status = $json["status"];

            if ($json["status"] == "failed")
            {
                // Display service reported error
                echo "<p>Error: " . $json["message"] . "</p>"; 
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
    
    return $status;
}

?>

</body>
</html>
```

<!-- code block end -->