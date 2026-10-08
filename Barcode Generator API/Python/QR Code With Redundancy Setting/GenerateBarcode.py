import os
import requests # pip install requests

# The authentication key (API Key).
# Get your own by registering at https://app.pdf.co
API_KEY = "*************************"

# Base URL for PDF.co Web API requests
BASE_URL = "https://api.pdf.co/v1"

# Result file name
ResultFile = ".\\barcode.png"
# Barcode type. See valid barcode types in the documentation https://apidocs.pdf.co/#barcode-generator
BarcodeType = "QRCode"
# Barcode value
BarcodeValue = "QR123456\nhttps://pdf.co\nhttps://bytescout.com"

# Valid error correction levels:
# ----------------------------------
# Low - [default] Lowest error correction level. (Approx. 7% of codewords can be restored).
# Medium - Medium error correction level. (Approx. 15% of codewords can be restored).
# Quarter - Quarter error correction level (Approx. 25% of codewords can be restored).
# High - Highest error correction level (Approx. 30% of codewords can be restored).

# Set "Custom Profiles" parameter
Profiles = "{ \"profiles\": [ { \"profile1\": { \"Options.QRErrorCorrectionLevel\": \"Quarter\" } } ] }"


def main(args = None):
    generateBarcode(ResultFile)

def generateBarcode(destinationFile):
    """Generates Barcode using PDF.co Web API"""

    # Prepare requests params as JSON
    # See documentation: https://apidocs.pdf.co
    parameters = {}
    parameters["name"] = os.path.basename(destinationFile)
    parameters["type"] = BarcodeType
    parameters["value"] = BarcodeValue
    parameters["profiles"] = Profiles

    # Prepare URL for 'Barcode Generate' API request
    url = "{}/barcode/generate".format(BASE_URL)

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