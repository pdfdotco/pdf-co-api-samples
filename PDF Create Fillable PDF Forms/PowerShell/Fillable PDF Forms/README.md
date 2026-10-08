## How to PDF create fillable PDF forms for fillable PDF forms in PowerShell and PDF.co Web API PDF.co Web API: the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

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

##### **FillablePDFForms.ps1:**
    
```
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$headers.Add("x-api-key", "{{x-api-key}}")

$body = "{`n    `"async`": false,`n    `"encrypt`": true,`n    `"name`": `"newDocument`",`n    `"url`": `"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf`",`n    `"annotations`":[        `n       {`n            `"text`":`"sample prefilled text`",`n            `"x`": 10,`n            `"y`": 30,`n            `"size`": 12,`n            `"pages`": `"0-`",`n            `"type`": `"TextField`",`n            `"id`": `"textfield1`"`n        },`n        {`n            `"x`": 100,`n            `"y`": 150,`n            `"size`": 12,`n            `"pages`": `"0-`",`n            `"type`": `"Checkbox`",`n            `"id`": `"checkbox2`"`n        },`n        {`n            `"x`": 100,`n            `"y`": 170,`n            `"size`": 12,`n            `"pages`": `"0-`",`n            `"link`": `"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png`",`n            `"type`": `"CheckboxChecked`",`n            `"id`":`"checkbox3`"`n        }          `n        `n    ],`n    `n    `"images`": [`n        {`n            `"url`": `"bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png`",`n            `"x`": 200,`n            `"y`": 250,`n            `"pages`": `"0`",`n            `"link`": `"www.pdf.co`"`n        }`n        `n    ]`n}"

$response = Invoke-RestMethod 'https://api.pdf.co/v1/pdf/edit/add' -Method 'POST' -Headers $headers -Body $body
$response | ConvertTo-Json
```

<!-- code block end -->