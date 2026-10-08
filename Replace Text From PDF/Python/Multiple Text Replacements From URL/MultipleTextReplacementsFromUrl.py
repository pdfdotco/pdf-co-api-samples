import os
import requests 
import json

API_KEY = "******************************"
BASE_URL = "https://api.pdf.co/v1"
SourceFileURL = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-to-text/sample.pdf"
Password = ""
DestinationFile = ".\\result.pdf"

def main(args = None):
    replaceStringFromPdf(SourceFileURL, DestinationFile)

search_strings = ["Your Company Name", "Client Name", "Item"]
replace_strings = ["XYZ LLC", "ACME", "SKU"]

def replaceStringFromPdf(uploadedFileUrl, destinationFile):
    """Replace Text from PDF using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    parameters = {
        "name": os.path.basename(destinationFile),
        "password": Password,
        "url": uploadedFileUrl,
        "searchStrings": search_strings,
        "replaceStrings": replace_strings,
        "replacementLimit": 1
    }

    # Prepare URL for 'Replace Text from PDF' API request
    url = "{}/pdf/edit/replace-text".format(BASE_URL)

    # Convert parameters dictionary to JSON string
    payload = json.dumps(parameters)

    # Execute request and get response as JSON
    response = requests.post(url, data=payload, headers={ "x-api-key": API_KEY })

    if (response.status_code == 200):
        json_data = response.json()

        if json_data["error"] == False:
            #  Get URL of result file
            resultFileUrl = json_data["url"]            
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
            print(json_data["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")

if __name__ == '__main__':
    main()
