<?php

// Visit Knowledgebase for adding Text Macros to PDF 
// https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

$curl = curl_init();

curl_setopt_array($curl, array(
	CURLOPT_URL => 'https://api.pdf.co/v1/pdf/edit/add',
	CURLOPT_RETURNTRANSFER => true,
	CURLOPT_ENCODING => '',
	CURLOPT_MAXREDIRS => 10,
	CURLOPT_TIMEOUT => 0,
	CURLOPT_FOLLOWLOCATION => true,
	CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
	CURLOPT_CUSTOMREQUEST => 'POST',
	CURLOPT_POSTFIELDS =>'{
    "async": false,
    "encrypt": false,
    "inline": true,
    "name": "f1040-form-filled",
    "url": "bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf",
    "annotationsString": "250;20;0-;PDF form filled with PDF.co API;24+bold+italic+underline+strikeout;Arial;FF0000;www.pdf.co;true",
    "imagesString": "100;180;0-;bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png|400;180;0-;bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png;www.pdf.co;200;200",
    "fieldsString": "1;topmostSubform[0].Page1[0].f1_02[0];John A. Doe|1;topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1];true|1;topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0];123456789"
}',
	CURLOPT_HTTPHEADER => array(
		'Content-Type: application/json',
		'x-api-key: __YOUR_PDF_CO_API_KEY_HERE__'
	),
));

$response = json_decode(curl_exec($curl));

curl_close($curl);

echo "URL: " . $response -> url . "\n";
echo "Page Count: " . $response -> pageCount . "\n"; 
echo "Error: " . $response -> error . "\n"; 
echo "Status: " . $response -> status . "\n"; 
echo "Name: " . $response -> name ;

