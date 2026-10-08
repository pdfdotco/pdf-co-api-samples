import os
import requests # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "********************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Direct URL of source PDF file.
# You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
SourceFileURL = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf"

# Destination PDF file name
DestinationFile = ".\\protected.pdf"

# Passwords to protect PDF document
# The owner password will be required for document modification.
# The user password only allows to view and print the document.
OwnerPassword = "123456"
UserPassword = "654321"

# Encryption algorithm. 
# Valid values: "RC4_40bit", "RC4_128bit", "AES_128bit", "AES_256bit".
EncryptionAlgorithm = "AES_128bit"

# Allow or prohibit content extraction for accessibility needs.
AllowAccessibilitySupport = True

# Allow or prohibit assembling the document.
AllowAssemblyDocument = True

# Allow or prohibit printing PDF document.
AllowPrintDocument = True

# Allow or prohibit filling of interactive form fields (including signature fields) in PDF document.
AllowFillForms = True

# Allow or prohibit modification of PDF document.
AllowModifyDocument = True

# Allow or prohibit copying content from PDF document.
AllowContentExtraction = True

# Allow or prohibit interacting with text annotations and forms in PDF document.
AllowModifyAnnotations = True

# Allowed printing quality.
# Valid values: "HighResolution", "LowResolution"
PrintQuality = "HighResolution"

# Runs processing asynchronously. 
# Returns Use JobId that you may use with /job/check to check state of the processing (possible states: working, failed, aborted and success).
Async = False

def main(args = None):
    protectPDF(SourceFileURL, DestinationFile)


def protectPDF(uploadedFileUrl, destinationFile):
    """Protect PDF using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    parameters = {"name": os.path.basename(destinationFile), "url": uploadedFileUrl, "ownerPassword": OwnerPassword,
                  "userPassword": UserPassword, "encryptionAlgorithm": EncryptionAlgorithm,
                  "allowAccessibilitySupport": AllowAccessibilitySupport,
                  "allowAssemblyDocument": AllowAssemblyDocument, "allowPrintDocument": AllowPrintDocument,
                  "allowFillForms": AllowFillForms, "allowModifyDocument": AllowModifyDocument,
                  "allowContentExtraction": AllowContentExtraction, "allowModifyAnnotations": AllowModifyAnnotations,
                  "printQuality": PrintQuality, "async": Async}

    # Serializing json
    import json
    json_object = json.dumps(parameters, indent=4)

    # Prepare URL for 'PDF Security' API request
    url = "{}/pdf/security/add".format(BASE_URL)

    # Execute request and get response as JSON
    response = requests.post(url, data=json_object, headers={"x-api-key": API_KEY})
    if (response.status_code == 200):
        jsonResp = response.json()

        if jsonResp["error"] == False:
            #  Get URL of result file
            resultFileUrl = jsonResp["url"]
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
            print(jsonResp["message"])
    else:
        print(f"Request error: {response.status_code} {response.reason}")

if __name__ == '__main__':
    main()