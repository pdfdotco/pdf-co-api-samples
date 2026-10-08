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
    # You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
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