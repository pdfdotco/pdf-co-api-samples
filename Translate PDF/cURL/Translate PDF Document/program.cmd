curl --location --request POST 'https://api.pdf.co/v1/pdf/translate' \
--header 'x-api-key: your_pdfco_api_key' \
--header 'Content-Type: application/json' \
--data-raw '{
    "url": "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-to-csv/sample.pdf",
    "name": "result-translate-en-to-de",
    "langFrom": "en",
    "langto": "de",
    "async": false
}'