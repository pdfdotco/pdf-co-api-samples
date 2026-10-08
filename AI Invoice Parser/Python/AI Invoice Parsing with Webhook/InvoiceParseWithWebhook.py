import os
import requests  # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "YOUR_API_KEY"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Direct URL of Source PDF file
# You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
SourceFileURL = "https://pdfco-test-files.s3.us-west-2.amazonaws.com/document-parser/sample-invoice.pdf"


# Provide your callback URL here
# To test you can temporary callback from sites like "https://webhook.site/"    
CallbackURL = "https://example.com/callback/url/you/provided"

def main(args=None):
    getParsedInvoice(SourceFileURL, CallbackURL)


def getParsedInvoice(uploadedFileUrl, callbackURL):
    """AI Invoice Parser using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    parameters = {}
    parameters["url"] = uploadedFileUrl
    parameters["callback"] = callbackURL

    # Prepare URL for 'AI Invoice Parser' API request
    url = "{}/ai-invoice-parser".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=parameters, headers={ "x-api-key": API_KEY })
    if (response.status_code == 200):
        json = response.json()

        if json["error"] == False:
            # Display information
            print("Result will be available on provided Webhook URL")
        else:
            # Show service reported error
            print(json["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")

if __name__ == '__main__':
    main()