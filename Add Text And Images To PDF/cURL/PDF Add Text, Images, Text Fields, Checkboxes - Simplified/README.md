## How to add text and images to PDF in cURL and PDF.co Web API PDF.co Web API: the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://support.pdf.co/en)

or just send email to [support@pdf.co](mailto:support@pdf.co?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)



## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://docs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **AddImageToPDF_Simplified.cmd:**
    
```
curl --location --request POST 'https://api.pdf.co/v1/pdf/edit/add' \
--header 'Content-Type: application/json' \
--header 'x-api-key: {{x-api-key}}' \
--data-raw '{
    "async": false,
    "encrypt": false,
    "name": "f1040-form-filled",
    "url": "bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf",
    "annotationsString": "20;20;0-;PDF form filled with PDF.co API;24;Arial;FF0000;www.pdf.co;true",
    "imagesString": "100;180;0-;bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png|400;180;0-;bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png;www.pdf.co;200;200",
    "fieldsString": "1;topmostSubform[0].Page1[0].f1_02[0];John A. Doe|1;topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1];true|1;topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0];123456789"
}'
```

<!-- code block end -->