## How to PDF create fillable PDF forms for fillable PDF forms in Python with PDF.co Web API PDF.co Web API: the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

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

##### **FillablePDFForms.py:**
    
```
import os
import requests # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "**************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"


def main(args = None):
    fillPDFForm()


def fillPDFForm():
    """Fillable PDF form using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    payload = "{\n    \"async\": false,\n    \"encrypt\": true,\n    \"name\": \"newDocument\",\n    \"url\": \"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf\",\n    \"annotations\":[        \n       {\n            \"text\":\"sample prefilled text\",\n            \"x\": 10,\n            \"y\": 30,\n            \"size\": 12,\n            \"pages\": \"0-\",\n            \"type\": \"TextField\",\n            \"id\": \"textfield1\"\n        },\n        {\n            \"x\": 100,\n            \"y\": 150,\n            \"size\": 12,\n            \"pages\": \"0-\",\n            \"type\": \"Checkbox\",\n            \"id\": \"checkbox2\"\n        },\n        {\n            \"x\": 100,\n            \"y\": 170,\n            \"size\": 12,\n            \"pages\": \"0-\",\n            \"link\": \"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png\",\n            \"type\": \"CheckboxChecked\",\n            \"id\":\"checkbox3\"\n        }          \n        \n    ],\n    \n    \"images\": [\n        {\n            \"url\": \"bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png\",\n            \"x\": 200,\n            \"y\": 250,\n            \"pages\": \"0\",\n            \"link\": \"www.pdf.co\"\n        }\n        \n    ]\n}"
    # Prepare URL for 'Fillable PDF' API request
    url = "{}/pdf/edit/add".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=payload, headers={"x-api-key": API_KEY, 'Content-Type': 'application/json'})
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

if __name__ == '__main__':
    main()
```

<!-- code block end -->