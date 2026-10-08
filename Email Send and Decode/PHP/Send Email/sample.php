<?php

// Please refer to our knowledge base at (https://apidocs.pdf.co/kb/Email%20Send%20(email-send)/index) for SMTP related information

$curl = curl_init();

curl_setopt_array($curl, array(
		CURLOPT_URL => 'https://api.pdf.co/v1/email/send',
		CURLOPT_RETURNTRANSFER => true,
		CURLOPT_ENCODING => '',
		CURLOPT_MAXREDIRS => 10,
		CURLOPT_TIMEOUT => 0,
		CURLOPT_FOLLOWLOCATION => true,
		CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
		CURLOPT_CUSTOMREQUEST => 'POST',
		CURLOPT_POSTFIELDS =>'{
    "url": "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-info/sample.pdf",
    "from": "John Doe <john@example.com>",
    "to": "Partner <partner@example.com>",
    "subject": "Check attached sample pdf",
    "bodytext": "Please check the attached pdf",
    "bodyHtml": "Please check the attached pdf",
    "smtpserver": "smtp.gmail.com",
    "smtpport": "587",
    "smtpusername": "my@gmail.com", 
    "smtppassword": "app specific password created as https://support.google.com/accounts/answer/185833",
    "async": false
}',
		CURLOPT_HTTPHEADER => array(
				'Content-Type: application/json',
				'x-api-key: ADD_YOUR_PDFco_KEY_HERE'
		),
));

$response = json_decode(curl_exec($curl));

curl_close($curl);
echo "<h2>Output:</h2><pre>", var_export($response, true), "</pre>";
