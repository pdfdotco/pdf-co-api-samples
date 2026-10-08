//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


var https = require("https");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "***********************************";


// Direct URL of source file to search barcodes in.
// You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
const SourceFileUrl = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/encryption/barcode_encrypted_aes128.png";

// Comma-separated list of barcode types to search. 
// See valid barcode types in the documentation https://apidocs.pdf.co
const BarcodeTypes = "QRCode";

// Refer to documentations for more info. https://apidocs.pdf.co/32-1-user-controlled-data-encryption-and-decryption
const Profiles = "{ 'DataDecryptionAlgorithm': 'AES128', 'DataDecryptionKey': 'Qweasd1234567890', 'DataDecryptionIV': '0mDI&qLv*ivTCd$*' }";

// Prepare request to `Barcode Reader` API endpoint
var queryPath = `/v1/barcode/read/from/url`;

// JSON payload for api request
var jsonPayload = JSON.stringify({
    types: BarcodeTypes,
    profiles: Profiles,
    url: SourceFileUrl
});

var reqOptions = {
    host: "api.pdf.co",
    method: "POST",
    path: queryPath,
    headers: {
        "x-api-key": API_KEY,
        "Content-Type": "application/json",
        "Content-Length": Buffer.byteLength(jsonPayload, 'utf8')
    }
};

// Send request
var postRequest = https.request(reqOptions, (response) => {
    response.on("data", (d) => {
        // Parse JSON response
        var data = JSON.parse(d);

        if (data.error == false) {
            // Display found barcodes in console
            data.barcodes.forEach((element) => {
                console.log("Found barcode:");
                console.log("  Type: " + element.TypeName);
                console.log("  Value: " + element.Value);
                console.log("  Document Page Index: " + element.Page);
                console.log("  Rectangle: " + element.Rect);
                console.log("  Confidence: " + element.Confidence);
                console.log("");
            }, this);
        }
        else {
            // Service reported error
            console.log(data.message);
        }
    });
}).on("error", (e) => {
    // Request error
    console.error(e);
});

// Write request data
postRequest.write(jsonPayload);
postRequest.end();
