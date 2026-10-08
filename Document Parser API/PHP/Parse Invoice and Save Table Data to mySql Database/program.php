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

$data_parsed = GetFormattedInvoiceData($data);

// var_dump($data_parsed);
echo '<hr/><p><strong><u>Saving to Database</u></strong></p>';
// echo $data_parsed->invId;

$sql = "INSERT INTO invoice (invId, dateIssued, total, subTotal, taxRate, tax, companyName, billTo, billingId, currency)
VALUES ('$data_parsed->invId','$data_parsed->dateIssued','$data_parsed->total','$data_parsed->subTotal','$data_parsed->taxRate','$data_parsed->tax','$data_parsed->companyName','$data_parsed->billTo','$data_parsed->billingId','$data_parsed->currency')";

if ($conn->query($sql) === TRUE) {
  echo "<p>New Invoice record added successfully!</p>";
} else {
  echo "<p>Error: " . $sql . "<br>" . $conn->error . "</p>";
}

foreach ($data_parsed->invItems as $itmInv) {
    $sql_itm = "INSERT INTO invoice_items (invId, description, intervl, quantity, amount)
    VALUES ('$data_parsed->invId','$itmInv->description','$itmInv->interval','$itmInv->quantity','$itmInv->amount')";

    if ($conn->query($sql_itm) === TRUE) {
        echo "<p>New Invoice Item record added successfully!</p>";
    } else {
        echo "<p>Error: " . $sql_itm . "<br>" . $conn->error . "</p>";
    }
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
                        echo "<div><a href='" . $resultFileUrl . "' target='_blank'>" . $resultFileUrl . "</a></div>";
						
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

class InvoiceData{
    public $invId;
    public $dateIssued;
    public $total;
    public $subTotal;
    public $taxRate;
    public $tax;
    public $companyName;
    public $billTo;
    public $billingId;
    public $currency;

    public $invItems = array();
}

class InvoiceItem{
    public $description;
    public $quantity;
    public $amount;
    public $interval;
}

function GetFormattedInvoiceData($inpJson){

    $oRet = new InvoiceData();

    $oRet->invId = FindJsonNodeValue($inpJson, 'invoiceId');
    $oRet->dateIssued = FindJsonNodeValue($inpJson, 'dateIssued');
    $oRet->total = FindJsonNodeValue($inpJson, 'total');
    $oRet->subTotal = FindJsonNodeValue($inpJson, 'subTotal');
    $oRet->taxRate = FindJsonNodeValue($inpJson, 'taxRate');
    $oRet->tax = FindJsonNodeValue($inpJson, 'tax');
    $oRet->companyName = FindJsonNodeValue($inpJson, 'companyName');
    $oRet->billingId = FindJsonNodeValue($inpJson, 'billingId');
    $oRet->billTo = FindJsonNodeValue($inpJson, 'billTo');
    $oRet->currency = FindJsonNodeValue($inpJson, 'currency');

    $invoiceItems = FindJsonNodeValue($inpJson, 'table1');
    if($invoiceItems != NULL){
        foreach ($invoiceItems as $keyRow => $itmRow) {

            $invItm = new InvoiceItem();

            foreach ($itmRow as $key => $value) {

                if($key == "description"){
                    $invItm->description = $value["value"];
                }

                if($key == "interval"){
                    $invItm->interval = $value["value"];
                }

                if($key == "quantity"){
                    $invItm->quantity = $value["value"];
                }

                if($key == "amount"){
                    $invItm->amount = $value["value"];
                }
            }

            $oRet->invItems[] = $invItm;
        }
    }

    // echo '<p>----</p>';
    //echo var_dump($oRet);

    return $oRet;
}

function FindJsonNodeValue($inpJson, $propName){
    $elmRet = array_values(array_filter($inpJson['objects'], function($obj) use ($propName){
        return ($obj['name'] == $propName);
    }));

    if(count($elmRet) > 0){

        if($elmRet[0]["objectType"] == "field"){
            return $elmRet[0]["value"];
        }
        else if($elmRet[0]["objectType"] == "table"){
            return $elmRet[0]["rows"];
        }
    }
    return NULL;
}

?>

</body>
</html>