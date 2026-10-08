## How to read barcode from URL for barcode reader API in Python with PDF.co Web API

### Learn in simple ways: How to read barcode from URL for barcode reader API in Python

This page displays the step-by-step instructions and algorithm of how to read barcode from URL and how to apply it in your application. PDF.co Web API helps with barcode reader API in Python. PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

The SDK samples displayed below below explain how to quickly make your application do barcode reader API in Python with the help of PDF.co Web API. Open your Python project and simply copy & paste the code and then run your app! Further improvement of the code will make it more robust.

Trial version of ByteScout is available for free download from our website. This and other source code samples for Python and other programming languages are available.

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

##### **ReadBarcodeFromUrl.py:**
    
```
import os
import requests  # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "******************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Direct URL of source file to search barcodes in.
SourceFileURL = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/barcode-reader/sample.pdf"

# Comma-separated list of barcode types to search.
# See valid barcode types in the documentation https://apidocs.pdf.co
BarcodeTypes = "Code128,Code39,Interleaved2of5,EAN13"

# Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
Pages = ""


def main(args=None):
    readBarcodes(SourceFileURL)

def readBarcodes(uploadedFileUrl):
    """Get Barcode Information using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    parameters = {}
    parameters["types"] = BarcodeTypes
    parameters["pages"] = Pages
    parameters["url"] = uploadedFileUrl

    # Prepare URL for 'Barcode Reader' API request
    url = "{}/barcode/read/from/url".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=parameters, headers={ "x-api-key": API_KEY })
    if (response.status_code == 200):
        json = response.json()

        if json["error"] == False:
            # Display information
            for barcode in json["barcodes"]:
                print("Found barcode:")
                print(f"  Type: {barcode['TypeName']}")
                print(f"  Value: {barcode['Value']}")
                print(f"  Document Page Index: {barcode['Page']}")
                print(f"  Rectangle: {barcode['Rect']}")
                print(f"  Confidence: {barcode['Confidence']}")
                print("")
        else:
            # Show service reported error
            print(json["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")


if __name__ == '__main__':
    main()
```

<!-- code block end -->