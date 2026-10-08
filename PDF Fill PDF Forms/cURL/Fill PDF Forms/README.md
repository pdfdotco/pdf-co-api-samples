## PDF fill PDF forms in cURL with PDF.co Web API PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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

##### **FillPDFForms.cmd:**
    
```
curl --location --request POST 'https://api.pdf.co/v1/pdf/edit/add' \
--header 'Content-Type: application/json' \
--header 'x-api-key: {{x-api-key}}' \
--data-raw '{
    "async": false,
    "encrypt": false,
    "name": "f1040-filled",
    "url": "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf",
    "fields": [
        {
            "fieldName": "topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1]",
            "pages": "1",
            "text": "True"
        },
        {
            "fieldName": "topmostSubform[0].Page1[0].f1_02[0]",
            "pages": "1",
            "text": "John A."
        },        
        {
            "fieldName": "topmostSubform[0].Page1[0].f1_03[0]",
            "pages": "1",
            "text": "Doe"
        },        
        {
            "fieldName": "topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0]",
            "pages": "1",
            "text": "123456789"
        },
        {
            "fieldName": "topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]",
            "pages": "1",
            "text": "Joan B."
        },
        {
            "fieldName": "topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]",
            "pages": "1",
            "text": "Joan B."
        },
        {
            "fieldName": "topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_06[0]",
            "pages": "1",
            "text": "Doe"
        },
        {
            "fieldName": "topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_07[0]",
            "pages": "1",
            "text": "987654321"
        }     



    ],
    "annotations":[
        {
            "text":"Sample Filled with PDF.co API using /pdf/edit/add. Get fields from forms using /pdf/info/fields",
            "x": 10,
            "y": 10,
            "size": 12,
            "pages": "0-",
            "color": "FFCCCC",
            "link": "https://pdf.co"
        }
    ],    
    "images": [        
    ]
}'
```

<!-- code block end -->