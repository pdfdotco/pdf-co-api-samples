## PDF forms info reader in Python with PDF.co Web API PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

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

##### **PDFFormInfoReader.py:**
    
```
import os
import requests # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "***************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Source PDF file url
SourceFileURL = "https://pdf-temp-files.s3.amazonaws.com/R2FBM39LFX1BFC860O06XU0TL613JTZ9/f1040-form-filled.pdf "
Async = "False"

# Destination PDF file name
DestinationFile = ".\\result.pdf"

parameters = {}
parameters["async"] = Async
parameters["name"] = os.path.basename(DestinationFile)
parameters["url"] = SourceFileURL
 
# Prepare URL for 'Info Fields' API request
url = "{}/pdf/info/fields".format(BASE_URL)

response = requests.post(url, data=parameters, headers={ "x-api-key": API_KEY })
 
if (response.status_code == 200):
    json = response.json()
for field in json["info"]["FieldsInfo"]["Fields"]:print(field["FieldName"] + "=>" + field["Value"])
```

<!-- code block end -->
