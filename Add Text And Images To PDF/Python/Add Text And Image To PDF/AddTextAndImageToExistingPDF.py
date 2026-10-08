import os
import requests # pip install requests

# Visit Knowledgebase for adding Text Macros to PDF 
# https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "***************************************************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Direct URL of source PDF file.
# You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf"

#Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
Pages = ""

# PDF document password. Leave empty for unprotected documents.
Password = ""

# Destination PDF file name
DestinationFile = ".//result.pdf"

# Text annotation params
Type = "annotation"
I = 400
J = 600
Text = "APPROVED"
FontName = "Times New Roman"
FontSize = 24
Color = "FF0000"

# Image params
Type = "image"
X = 400
Y = 20
Width = 119
Height = 32
ImageUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png"

def main(args = None):
    addToExistingPDF(DestinationFile)

def addToExistingPDF(destinationFile):
    import json
    """Add Text using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    payload = json.dumps({
        "name": os.path.basename(destinationFile),
        "password": Password,
        "url": SourceFileUrl,
        "annotations": [{
            "text": Text,
            "x": I,
            "y": J,
            "fontname": FontName,
            "size": FontSize,
            "color": Color,
            "pages": Pages
        }],
        "images": [{
            "url": ImageUrl,
            "x": X,
            "y": Y,
            "width": Width,
            "height": Height,
            "pages": Pages
        }]
    })

    # Prepare URL for 'PDF Edit' API request
    url = "{}/pdf/edit/add".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=payload, headers={ "x-api-key": API_KEY })
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