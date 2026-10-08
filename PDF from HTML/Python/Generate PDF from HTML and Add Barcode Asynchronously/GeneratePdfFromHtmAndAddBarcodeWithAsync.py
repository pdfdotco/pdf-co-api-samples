import os
import json
import time
import datetime
import requests  # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "******************************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

### STEP 1: GENERATE BARCODE ###

# Result file name
ResultFile = ".\\barcode.png"
# Barcode type. See valid barcode types in the documentation https://apidocs.pdf.co
BarcodeType = "Code128"
# Barcode value
BarcodeValue = "qweasd123456"

# Barcode Source File
SourceFile = ResultFile


def main(args=None):
    # Generate Barcode
    generateBarcode(ResultFile)

    # Upload Barcode to Cloud
    uploadedBarcodeUrl = uploadFile(SourceFile)

    # Convert HTML to PDF in async mode and get job ID
    jobId = GeneratePDFFromHtml(SampleHtml, DestinationFile)

    # Check job status using jobId
    if jobId:
        print(f"Job ID: {jobId} - Checking job status...")
        while True:
            status = checkJobStatus(jobId)
            if status == "success":
                print("Job completed successfully.")
                # Download the resulting PDF from the URL
                resultFileUrl = downloadPdfAfterJobCompletion(jobId)
                if resultFileUrl:
                    # Download the PDF file and save it locally
                    resultFilePath = downloadPdf(resultFileUrl)

                    # Upload the downloaded file to the cloud
                    uploadedPdfUrl = uploadFile(resultFilePath)

                    # Add barcode to the PDF
                    addImageToExistingPdf(uploadedPdfUrl, uploadedBarcodeUrl, DestinationFile)
                break
            elif status == "failed":
                print("Job failed.")
                return
            else:
                print("Job still in progress... Waiting...")
                time.sleep(5)  # Wait for 5 seconds before checking again
    else:
        print("Failed to start the PDF conversion job.")


def generateBarcode(destinationFile):
    """Generates Barcode using PDF.co Web API"""

    # Prepare requests params as JSON
    parameters = {}
    parameters["name"] = os.path.basename(destinationFile)
    parameters["type"] = BarcodeType
    parameters["value"] = BarcodeValue

    # Prepare URL for 'Barcode Generate' API request
    url = "{}/barcode/generate".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=parameters, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        json_response = response.json()

        if json_response["error"] == False:
            # Get URL of result file
            resultFileUrl = json_response["url"]
            # Download result file
            r = requests.get(resultFileUrl, stream=True)
            if (r.status_code == 200):
                with open(destinationFile, 'wb') as file:
                    for chunk in r:
                        file.write(chunk)
            else:
                print(f"Request error: {response.status_code} {response.reason}")
        else:
            print(json_response["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")


### STEP 2: UPLOAD FILE AS TEMPORARY FILE ###


def uploadFile(fileName):
    """Uploads file to the cloud"""

    # 1. RETRIEVE PRESIGNED URL TO UPLOAD FILE.

    # Prepare URL for 'Get Presigned URL' API request
    url = "{}/file/upload/get-presigned-url?contenttype=application/octet-stream&name={}".format(
        BASE_URL, os.path.basename(fileName))

    # Execute request and get response as JSON
    response = requests.get(url, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        json_response = response.json()

        if json_response["error"] == False:
            # URL to use for file upload
            uploadUrl = json_response["presignedUrl"]
            # URL for future reference
            uploadedFileUrl = json_response["url"]

            # 2. UPLOAD FILE TO CLOUD.
            with open(fileName, 'rb') as file:
                requests.put(uploadUrl, data=file, headers={"x-api-key": API_KEY, "content-type": "application/octet-stream"})

            return uploadedFileUrl
        else:
            print(json_response["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return None


### STEP 3: CONVERT HTML TO PDF ###


# HTML template
file_read = open(".\\sample.html", mode='r', encoding='utf-8')
SampleHtml = file_read.read()
file_read.close()

# Destination PDF file name
DestinationFile = ".\\result.pdf"


def GeneratePDFFromHtml(SampleHtml, destinationFile):
    """Converts HTML to PDF using PDF.co Web API with Async Mode"""

    # Prepare requests params as JSON
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
    parameters["async"] = "true"

    # Set to HTML for header to be applied on every page at the header.
    parameters["header"] = ""

    # Set to HTML for footer to be applied on every page at the bottom.
    parameters["footer"] = ""

    # Prepare URL for 'HTML To PDF' API request
    url = "{}/pdf/convert/from/html".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=parameters, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        json_response = response.json()

        if json_response["error"] == False:
            # Get Job ID for async status tracking
            jobId = json_response["jobId"]
            print(f"Job ID: {jobId}")
            return jobId
        else:
            print(json_response["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return None


def downloadPdf(resultFileUrl):
    """Downloads the result PDF file from the URL"""
    r = requests.get(resultFileUrl, stream=True)
    if r.status_code == 200:
        local_filename = DestinationFile  # Save it as DestinationFile
        with open(local_filename, 'wb') as file:
            for chunk in r:
                file.write(chunk)
        print(f"PDF downloaded and saved as {local_filename}")
        return local_filename
    else:
        print(f"Request error: {r.status_code} {r.reason}")
    return None


def downloadPdfAfterJobCompletion(jobId):
    """Downloads the result PDF after job completion"""

    # Prepare the URL to fetch the result after the job has completed
    url = f"{BASE_URL}/job/check?jobid={jobId}"

    # Execute the request to get the result file URL
    response = requests.get(url, headers={"x-api-key": API_KEY})
    if response.status_code == 200:
        json_response = response.json()

        # Print the full response for debugging
        print("Job status response:", json_response)

        # Check if the response contains expected keys and values
        if 'status' in json_response and json_response['status'] == 'success':
            resultFileUrl = json_response.get("url")
            if resultFileUrl:
                return resultFileUrl
            else:
                print("Result file URL not found in the response.")
        else:
            print(f"Job failed or status not found. Response: {json_response}")
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return None


def checkJobStatus(jobId):
    """Checks server job status"""

    url = f"{BASE_URL}/job/check?jobid={jobId}"

    response = requests.get(url, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        json_response = response.json()
        return json_response.get("status", "unknown")  # Use get() to avoid KeyError
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return "unknown"


### STEP 4: ADD BARCODE TO PDF ###


# Destination PDF file name
DestinationFile = ".//result.pdf"

# Image params
Type = "image"
X = 400
Y = 20
Width = 150
Height = 50


def addImageToExistingPdf(PdfUrl, barcodeUrl, destinationFile):
    import json
    """Add image using PDF.co Web API"""

    # Prepare requests params as JSON
    payload = json.dumps({
        "name": os.path.basename(destinationFile),
        "url": PdfUrl,
        "images": [{
            "url": barcodeUrl,
            "x": X,
            "y": Y,
            "width": Width,
            "height": Height
        }]
    })

    # Prepare URL for 'PDF Edit' API request
    url = "{}/pdf/edit/add".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=payload, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        try:
            json_response = response.json()

            if json_response["error"] == False:
                # URL of the result file
                resultFileUrl = json_response["url"]

                # Download the result file directly from the URL
                r = requests.get(resultFileUrl, stream=True)
                if (r.status_code == 200):
                    with open(destinationFile, 'wb') as file:
                        for chunk in r:
                            file.write(chunk)
                    print(f"Result file saved as \"{destinationFile}\" file.")
                else:
                    print(f"Request error: {response.status_code} {response.reason}")
            else:
                print(f"Error: {json_response['message']}")
        except ValueError as e:
            print(f"Error parsing the response: {e}")
            print("Response:", response.text)
    else:
        print(f"Request error: {response.status_code} {response.reason}")


if __name__ == '__main__':
    main()
