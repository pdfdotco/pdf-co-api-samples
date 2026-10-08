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
var path = require("path");
var fs = require("fs");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "***********************************";


// Direct URL of source PDF file.
// You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
const SourceFileUrl = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf";

// Destination PDF file name
const DestinationFile = "./protected.pdf";

// Passwords to protect PDF document
// The owner password will be required for document modification.
// The user password only allows to view and print the document.
const OwnerPassword = "123456";
const UserPassword = "654321";

// Encryption algorithm. 
// Valid values: "RC4_40bit", "RC4_128bit", "AES_128bit", "AES_256bit".
const EncryptionAlgorithm = "AES_128bit";

// Allow or prohibit content extraction for accessibility needs.
const AllowAccessibilitySupport = true;

// Allow or prohibit assembling the document.
const AllowAssemblyDocument = true;

// Allow or prohibit printing PDF document.
const AllowPrintDocument = true;

// Allow or prohibit filling of interactive form fields (including signature fields) in PDF document.
const AllowFillForms = true;

// Allow or prohibit modification of PDF document.
const AllowModifyDocument = true;

// Allow or prohibit copying content from PDF document.
const AllowContentExtraction = true;

// Allow or prohibit interacting with text annotations and forms in PDF document.
const AllowModifyAnnotations = true;

// Allowed printing quality.
// Valid values: "HighResolution", "LowResolution"
const PrintQuality = "HighResolution";

// Runs processing asynchronously. 
// Returns Use JobId that you may use with /job/check to check state of the processing (possible states: working, failed, aborted and success).
const async = false;


// Prepare request to `PDF Security` API endpoint
var queryPath = `/v1/pdf/security/add`;

// JSON payload for api request
var jsonPayload = JSON.stringify({
    name: path.basename(DestinationFile),
    url: SourceFileUrl,
    ownerPassword: OwnerPassword,
    userPassword: UserPassword,
    encryptionAlgorithm: EncryptionAlgorithm,
    allowAccessibilitySupport: AllowAccessibilitySupport,
    allowAssemblyDocument: AllowAssemblyDocument,
    allowPrintDocument: AllowPrintDocument,
    allowFillForms: AllowFillForms,
    allowModifyDocument: AllowModifyDocument,
    allowContentExtraction: AllowContentExtraction,
    allowModifyAnnotations: AllowModifyAnnotations,
    printQuality: PrintQuality,
    async: async
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
            // Download PDF file
            var file = fs.createWriteStream(DestinationFile);
            https.get(data.url, (response2) => {
                response2.pipe(file)
                    .on("close", () => {
                        console.log(`Generated PDF file saved as "${DestinationFile}" file.`);
                    });
            });
        }
        else {
            // Service reported error
            console.log(data.message);
        }
    });
}).on("error", (e) => {
    // Request error
    console.log(e);
});

// Write request data
postRequest.write(jsonPayload);
postRequest.end();
