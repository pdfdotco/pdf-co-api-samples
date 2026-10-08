## PDF fill PDF forms in PowerShell using PDF.co Web API What is PDF.co Web API? It is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

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

##### **FillPDFForms.ps1:**
    
```
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$headers.Add("x-api-key", "{{x-api-key}}")

$body = "{`n    `"async`": false,`n    `"encrypt`": false,`n    `"name`": `"f1040-filled`",`n    `"url`": `"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf`",`n    `"fields`": [`n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1]`",`n            `"pages`": `"1`",`n            `"text`": `"True`"`n        },`n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].f1_02[0]`",`n            `"pages`": `"1`",`n            `"text`": `"John A.`"`n        },        `n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].f1_03[0]`",`n            `"pages`": `"1`",`n            `"text`": `"Doe`"`n        },        `n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0]`",`n            `"pages`": `"1`",`n            `"text`": `"123456789`"`n        },`n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]`",`n            `"pages`": `"1`",`n            `"text`": `"Joan B.`"`n        },`n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]`",`n            `"pages`": `"1`",`n            `"text`": `"Joan B.`"`n        },`n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_06[0]`",`n            `"pages`": `"1`",`n            `"text`": `"Doe`"`n        },`n        {`n            `"fieldName`": `"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_07[0]`",`n            `"pages`": `"1`",`n            `"text`": `"987654321`"`n        }     `n`n`n`n    ],`n    `"annotations`":[`n        {`n            `"text`":`"Sample Filled with PDF.co API using /pdf/edit/add. Get fields from forms using /pdf/info/fields`",`n            `"x`": 10,`n            `"y`": 10,`n            `"size`": 12,`n            `"pages`": `"0-`",`n            `"color`": `"FFCCCC`",`n            `"link`": `"https://pdf.co`"`n        }`n    ],    `n    `"images`": [        `n    ]`n}"

$response = Invoke-RestMethod 'https://api.pdf.co/v1/pdf/edit/add' -Method 'POST' -Headers $headers -Body $body
$response | ConvertTo-Json
```

<!-- code block end -->