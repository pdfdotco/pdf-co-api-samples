import os
import requests  # pip install requests
import time
import datetime


# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "*************************************"


# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"


# Source PDF file
SourceFile = "./sample.pdf"  # Path to your local file




def main(args=None):
    # Step 1: Upload the local file to PDF.co storage
    uploadedFileUrl = uploadFile(SourceFile)


    if uploadedFileUrl:
        # Step 2: Process the uploaded file
        getParsedInvoice(uploadedFileUrl)
    else:
        print("File upload failed.")




def uploadFile(fileName):
    """Uploads file to the cloud"""


    # 1. RETRIEVE PRESIGNED URL TO UPLOAD FILE.


    # Prepare URL for 'Get Presigned URL' API request
    url = "{}/file/upload/get-presigned-url?contenttype=application/octet-stream&name={}".format(
        BASE_URL, os.path.basename(fileName))


    # Execute request and get response as JSON
    response = requests.get(url, headers={"x-api-key": API_KEY})
    if response.status_code == 200:
        json = response.json()


        if json["error"] == False:
            # URL to use for file upload
            uploadUrl = json["presignedUrl"]
            # URL for future reference
            uploadedFileUrl = json["url"]


            # 2. UPLOAD FILE TO CLOUD.
            with open(fileName, 'rb') as file:
                requests.put(uploadUrl, data=file, headers={"x-api-key": API_KEY, "content-type": "application/octet-stream"})


            return uploadedFileUrl
        else:
            # Show service reported error
            print(json["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")


    return None




def getParsedInvoice(uploadedFileUrl):
    """AI Invoice Parser using PDF.co Web API"""


    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    parameters = {}
    parameters["url"] = uploadedFileUrl


    # Prepare URL for 'AI Invoice Parser' API request
    url = "{}/ai-invoice-parser".format(BASE_URL)


    # Execute request and get response as JSON
    response = requests.post(url, data=parameters, headers={"x-api-key": API_KEY})
    if response.status_code == 200:
        json = response.json()


        if json["error"] == False:
            # Asynchronous job ID
            jobId = json["jobId"]


            # Check the job status in a loop.
            # If you don't want to pause the main thread, you can rework the code
            # to use a separate thread for the status checking and completion.
            while True:
                status = checkJobStatus(jobId)  # Possible statuses: "working", "failed", "aborted", "success".


                # Display timestamp and status (for demo purposes)
                print(datetime.datetime.now().strftime("%H:%M.%S") + ": " + status)


                if status == "success":
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
    if response.status_code == 200:
        json = response.json()


        if json["status"]:
            print("** Response **")
            print(json)


        return json["status"]
    else:
        print(f"Request error: {response.status_code} {response.reason}")


    return None




if __name__ == '__main__':
    main()
