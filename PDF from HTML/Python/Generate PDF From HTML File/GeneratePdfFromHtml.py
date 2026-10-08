import os
import  json
import requests # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "**************************"

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
    parameters["async"] = "false"

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