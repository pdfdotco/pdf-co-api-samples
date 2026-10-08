import os
import requests
import json
import time
import datetime

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "**************************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Direct URL of source PDF file.
SourceFileUrl = "https://www.irs.gov/pub/irs-pdf/f1099msc.pdf"
# PDF document password. Leave empty for unprotected documents.
Password = ""

# Destination PDF file name
DestinationFile = ".result.pdf"

# Runs processing asynchronously. Returns Use JobId that you may use with /job/check to check state of the processing (possible states: working, failed, aborted and success). Must be one of: true, false.
Async = "True"

# Values to fill out pdf fields with built-in pdf form filler.
# To fill fields in PDF form, use the following format page;fieldName;value for example: 0;editbox1;text is here. To fill checkbox, use true, for example: 0;checkbox1;true. To separate multiple objects, use | separator. To get the list of all fillable fields in PDF form please use /pdf/info/fields endpoint.

FieldsStrings = "1;topmostSubform[0].CopyA[0].CopyAHeader[0].c1_1[0];true|1;topmostSubform[0].CopyA[0].LeftColumn[0].f1_2[0];Iris|1;topmostSubform[0].CopyA[0].LeftColumn[0].f1_3[0];123456789|1;topmostSubform[0].CopyA[0].LeftColumn[0].f1_5[0];HelloWorld"

def main(args = None):
   fillPDFForm(SourceFileUrl, DestinationFile)

def fillPDFForm(uploadedFileUrl, destinationFile):
   """Converts HTML to PDF using PDF.co Web API"""

   # Prepare requests params as JSON
   # See documentation: https://apidocs.pdf.co
   parameters = {}
   parameters["name"] = os.path.basename(destinationFile)
   parameters["url"] = uploadedFileUrl
   parameters["fieldsString"] = FieldsStrings
   parameters["async"] = Async

   # Prepare URL for 'Fill PDF' API request
   url = "{}/pdf/edit/add".format(BASE_URL)

   # Execute request and get response as JSON
   response = requests.post(url, data=parameters, headers={ "x-api-key": API_KEY })
   if response.status_code == 200:
       json_response = response.json()

       # If there are no errors, proceed to poll for job status
       if not json_response.get("error", False):
           # Extract the jobId from the response
           jobId = json_response.get("jobId")
           if jobId:
               print(f"Job ID: {jobId}")  # Output job ID

               # Poll the job status
               while True:
                   status = checkJobStatus(jobId)
                   print(datetime.datetime.now().strftime("%H:%M.%S") + ": " + status)

                   if status == "success":
                       # Extract the result URL to the final PDF
                       resultJsonUrl = json_response["url"]
                       print(f"Result JSON URL: {resultJsonUrl}")  # Debug print to check the result URL
                       
                       # Fetch the result JSON to get the PDF URL
                       fetchAndDownloadPdf(resultJsonUrl, destinationFile)
                       break
                   elif status == "working":
                       # Pause for a few seconds before polling again
                       time.sleep(3)
                   else:
                       print(status)
                       break
       else:
           print(f"Error: {json_response.get('message', 'Unknown error')}")
   else:
       print(f"Request error: {response.status_code} {response.reason}")


def fetchAndDownloadPdf(resultJsonUrl, destinationFile):
    """Fetch result JSON to get the final PDF URL and download the PDF"""
    # Fetch the result JSON file that contains the final PDF URL
    result_json_response = requests.get(resultJsonUrl, headers={"x-api-key": API_KEY})
    
    if result_json_response.status_code == 200:
        result_json = result_json_response.json()
        
        # Extract the actual PDF URL from the JSON response
        finalPdfUrl = result_json["url"]
        print(f"Final PDF URL: {finalPdfUrl}")  # Debug print to check the final PDF URL
        
        # Download the actual PDF
        pdf_response = requests.get(finalPdfUrl, headers={"x-api-key": API_KEY}, stream=True)
        if pdf_response.status_code == 200:
            with open(destinationFile, 'wb') as file:
                for chunk in pdf_response.iter_content(chunk_size=8192):
                    file.write(chunk)
            print(f"Result PDF saved as \"{destinationFile}\".")  # Final result
        else:
            print(f"Error downloading PDF: {pdf_response.status_code} {pdf_response.reason}")
    else:
        print(f"Error fetching result JSON: {result_json_response.status_code} {result_json_response.reason}")


def checkJobStatus(jobId):
    """Checks server job status"""
    url = f"{BASE_URL}/job/check?jobid={jobId}"

    response = requests.get(url, headers={"x-api-key": API_KEY})
    if response.status_code == 200:
        json_response = response.json()
        return json_response["status"]
    else:
        print(f"Request error: {response.status_code} {response.reason}")

    return None


if __name__ == '__main__':
    main()
