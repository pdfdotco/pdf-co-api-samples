## How to generate PDF from large HTML file for HTML to PDF API in Python with PDF.co Web API What is PDF.co Web API? It is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

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

##### **GeneratePdfFromHtml.py:**
    
```
import os
import time
import datetime
import requests # pip install requests


# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "*************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# HTML template
file_read = open(".\\sample.html", mode='r', encoding= 'utf-8')
SampleHtml = file_read.read()
file_read.close()

# Destination PDF file name
DestinationFile = ".\\result.pdf"


def main(args = None):
    GeneratePDFFromHtml(SampleHtml, DestinationFile)


def GeneratePDFFromHtml(SampleHtml, destinationFile):
    """Converts HTML to PDF using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co/?#1-json-pdfconvertfromhtml
    parameters = {}

    # Input HTML code to be converted. Required.
    parameters["html"] = SampleHtml

    #  Name of resulting file
    parameters["name"] = os.path.basename(destinationFile)

    # Set to css style margins like 10 px or 5px 5px 5px 5px.
    parameters["margins"] = "5px 5px 5px 5px"

    # Can be Letter, A4, A5, A6 or custom size like 200x200
    parameters["paperSize"] = "Letter"

    # Set to Portrait or Landscape. Portrait by default.
    parameters["orientation"] = "Portrait"

    # true by default. Set to false to disable printing of background.
    parameters["printBackground"] = "true"

    # If large input document, process in async mode by passing true
    # ! Making Async Request
    parameters["async"] = "true"

    # Set to HTML for header to be applied on every page at the header.
    parameters["header"] = ""

    # Set to HTML for footer to be applied on every page at the bottom.
    parameters["footer"] = ""

    # Prepare URL for 'HTML To PDF' API request
    url = "{}/pdf/convert/from/html".format(
        BASE_URL
    )

    # Execute request and get response as JSON

    response = requests.post(url, data=parameters, headers={ "x-api-key": API_KEY })
    if (response.status_code == 200):
        json = response.json()

        if json["error"] == False:
            # Asynchronous job ID
            jobId = json["jobId"]
            #  URL of the result file
            resultFileUrl = json["url"]

            # Check the job status in a loop.
            # If you don't want to pause the main thread you can rework the code
            # to use a separate thread for the status checking and completion.
            while True:
                status = checkJobStatus(jobId)  # Possible statuses: "working", "failed", "aborted", "success".

                # Display timestamp and status (for demo purposes)
                print(datetime.datetime.now().strftime("%H:%M.%S") + ": " + status)

                if status == "success":
                    # Download result file
                    r = requests.get(resultFileUrl, stream=True)
                    if (r.status_code == 200):
                        with open(destinationFile, 'wb') as file:
                            for chunk in r:
                                file.write(chunk)
                        print(f"Result file saved as \"{destinationFile}\" file.")
                    else:
                        print(f"Request error: {response.status_code} {response.reason}")
                    break
                elif status == "working":
                    # Pause for a few seconds
                    time.sleep(3)
                else:
                    print(status)
                    break
        else:
            # Show service reported error
            print(json["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")


def checkJobStatus(jobId):
    """Checks server job status"""

    url = f"{BASE_URL}/job/check?jobid={jobId}"

    response = requests.get(url, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        json = response.json()
        return json["status"]
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return None


if __name__ == '__main__':
    main()
```

<!-- code block end -->