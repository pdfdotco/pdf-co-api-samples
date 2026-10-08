## How to parse PDF invoice for document parser API in Python with PDF.co Web API PDF.co Web API: the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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

##### **GenericInvoice-Template.json:**
    
```
{
  "templateName": "Generic Invoice [en]",
  "templateVersion": 4,
  "templatePriority": 999999,
  "objects": [
    {
      "name": "companyName",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "$funcFindCompany",
        "regex": true
      },
      "id": 0
    },
    {
      "name": "companyName2",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "$funcFindCompanyNext",
        "regex": true
      },
      "id": 1
    },
    {
      "name": "invoiceId",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?is)(?:\\bInvoice|\\bNumber).*?(?<value>(?-i)INV-[A-Z0-9]+)( |\\r?$)",
        "regex": true
      },
      "id": 2
    },
    {
      "name": "invoiceIdFallback",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:Invoice Number|Invoice #|Invoice No\\.?|Receipt No\\.?|CREDIT NOTE No\\.?|REF NO\\.?)\\s*:?\\s+(?<value>.*?)(  |\\r?$)",
        "regex": true,
        "coalesceWith": "invoiceId"
      },
      "id": 3
    },
    {
      "name": "invoiceIdFallback2",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:\\bInvoice|\\bNumber)\\s*:?\\s+(?<value>.*?)(  |\\r?$)",
        "regex": true,
        "coalesceWith": "invoiceId"
      },
      "id": 4
    },
    {
      "name": "dateIssued",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?is)(?:Invoice Date|Issue Date|Date Issued|Date of Issue|Billed On).*?(?<value>{{SmartDate}})",
        "regex": true,
        "dataType": "date"
      },
      "id": 5
    },
    {
      "name": "dateIssuedFallback",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "$funcFindMinDate",
        "regex": true,
        "dataType": "date",
        "coalesceWith": "dateIssued"
      },
      "id": 6
    },
    {
      "name": "dateDue",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?is)(?:Due Date|Due On|Payment Due).*?(?<value>{{SmartDate}})",
        "regex": true,
        "dataType": "date"
      },
      "id": 7
    },
    {
      "name": "dateDueFallback",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "$funcFindMaxDate",
        "regex": true,
        "dataType": "date",
        "coalesceWith": "dateDue"
      },
      "id": 8
    },
    {
      "name": "bankAccount",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:Account Number|Account #|Bank Account No\\.?|ACCT #|Checking Number)\\s*:?\\s*(?<value>[0-9\\p{Pd}]{4,20})(\\s|\\r?$)",
        "regex": true
      },
      "id": 9
    },
    {
      "name": "bankAccountFallback",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?is)(?:Account).*?(?<value>[0-9\\p{Pd}]{5,20})(\\s|\\r?$)",
        "regex": true,
        "coalesceWith": "bankAccount"
      },
      "id": 10
    },
    {
      "name": "total",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:Invoice Total|Total Due|Total Amount|Amount Due|Amount Payable|Total Invoice|Gross Amount|Total for this invoice|Total \\(Incl\\. VAT\\)).*?(?<value>{{Number}})(\\s|$)",
        "regex": true,
        "dataType": "decimal"
      },
      "id": 11
    },
    {
      "name": "totalFallback",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "$funcFindMaxNumber",
        "regex": true,
        "dataType": "decimal",
        "coalesceWith": "total"
      },
      "id": 12
    },
    {
      "name": "subTotal",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:Subtotal|Sub Total|Invoice Sub-total|Sub-total|Taxable Value|Net Amount|Total \\(Excl\\. VAT\\)).*?(?<value>{{Number}})(\\s|$)",
        "regex": true,
        "dataType": "decimal"
      },
      "id": 13
    },
    {
      "name": "tax",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:Total Tax|Tax Amount|Sales Tax|Total GST).*?(?<value>{{Number}})(\\s|$)",
        "regex": true,
        "dataType": "decimal"
      },
      "id": 14
    },
    {
      "name": "taxFallback",
      "objectType": "field",
      "fieldProperties": {
        "fieldType": "macros",
        "expression": "(?i)(?:\\bVAT|\\bTax|\\bGST).*?(?<value>{{Number}})(\\s|$)",
        "regex": true,
        "dataType": "decimal",
        "coalesceWith": "tax"
      },
      "id": 15
    },
    {
      "name": "table",
      "objectType": "table",
      "tableProperties": {
        "start": {
          "pageIndex": 0,
          "expression": "(?i)^.*?(Description|Quantity|Qty|Unit Price).*?$",
          "regex": true
        },
        "end": {
          "expression": "(?i)^.*?(Subtotal|Total|Amount).*?$",
          "regex": true
        }
      },
      "id": 16
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

##### **parsePdfInvoice.py:**
    
```
import os
import requests # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "***********************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Source PDF file
SourceFile = ".\\SampleInvoice.pdf"

# Destination JSON file name
DestinationFile = ".\\result.json"

# Template text. Use Document Parser (https://pdf.co/document-parser, https://app.pdf.co/document-parser)
# to create templates.
# Read template from file:
file_read = open(".\\GenericInvoice-Template.json", mode='r', encoding="utf-8",errors="ignore")
Template = file_read.read()
file_read.close()

def main(args = None):
    uploadedFileUrl = uploadFile(SourceFile)

    if (uploadedFileUrl != None):
        PerformDocumentParser(uploadedFileUrl, Template, DestinationFile)

def PerformDocumentParser(uploadedFileUrl, template, destinationFile):

    # Content
    data = {
        'url': uploadedFileUrl,
        'template': template
    }

    # Prepare URL for 'Document Parser' API request
    url = "{}/pdf/documentparser".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data= data, headers={ "x-api-key": API_KEY })

    if (response.status_code == 200):
        json = response.json()

        if json["error"] == False:
            #  Get URL of result file
            resultFileUrl = json["url"]            
            # Download result file
            r = requests.get(resultFileUrl, stream=True)
            if (r.status_code == 200):
                with open(destinationFile, 'wb') as file:
                    for chunk in r:
                        file.write(chunk)
                print(f"Result file saved as \"{destinationFile}\" file.")
            else:
                print(f"Request error: {response.status_code} {response.reason}")
        else:
            # Show service reported error
            print(json["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")


def uploadFile(fileName):
    """Uploads file to the cloud"""

    # 1. RETRIEVE PRESIGNED URL TO UPLOAD FILE.

    # Prepare URL for 'Get Presigned URL' API request
    url = "{}/file/upload/get-presigned-url?contenttype=application/octet-stream&name={}".format(
        BASE_URL, os.path.basename(fileName))

    # Execute request and get response as JSON
    response = requests.get(url, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        json = response.json()

        if json["error"] == False:
            # URL to use for file upload
            uploadUrl = json["presignedUrl"]
            # URL for future reference
            uploadedFileUrl = json["url"]

            # 2. UPLOAD FILE TO CLOUD.
            with open(fileName, 'rb') as file:
                requests.put(uploadUrl, data=file,
                             headers={"x-api-key": API_KEY, "content-type": "application/octet-stream"})

            return uploadedFileUrl
        else:
            # Show service reported error
            print(json["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return None


if __name__ == '__main__':
    main()
```

<!-- code block end -->