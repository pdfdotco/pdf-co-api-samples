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
    """Fill PDF form using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    # You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
    payload = "{\n    \"async\": false,\n    \"encrypt\": false,\n    \"name\": \"f1040-filled\",\n    \"url\": \"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf\",\n    \"fields\": [\n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1]\",\n            \"pages\": \"1\",\n            \"text\": \"True\"\n        },\n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].f1_02[0]\",\n            \"pages\": \"1\",\n            \"text\": \"John A.\"\n        },        \n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].f1_03[0]\",\n            \"pages\": \"1\",\n            \"text\": \"Doe\"\n        },        \n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0]\",\n            \"pages\": \"1\",\n            \"text\": \"123456789\"\n        },\n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]\",\n            \"pages\": \"1\",\n            \"text\": \"Joan B.\"\n        },\n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]\",\n            \"pages\": \"1\",\n            \"text\": \"Joan B.\"\n        },\n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_06[0]\",\n            \"pages\": \"1\",\n            \"text\": \"Doe\"\n        },\n        {\n            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_07[0]\",\n            \"pages\": \"1\",\n            \"text\": \"987654321\"\n        }     \n\n\n\n    ],\n    \"annotations\":[\n        {\n            \"text\":\"Sample Filled with PDF.co API using /pdf/edit/add. Get fields from forms using /pdf/info/fields\",\n            \"x\": 10,\n            \"y\": 10,\n            \"size\": 12,\n            \"pages\": \"0-\",\n            \"color\": \"FFCCCC\",\n            \"link\": \"https://pdf.co\"\n        }\n    ],    \n    \"images\": [        \n    ]\n}"

    # Prepare URL for 'Fill PDF' API request
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