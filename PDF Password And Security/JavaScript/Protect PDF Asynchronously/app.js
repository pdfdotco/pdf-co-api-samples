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
const SourceFileUrl = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf";

// Destination PDF file name
const DestinationFile = "./protected.pdf";

// Passwords to protect PDF document
const OwnerPassword = "123456";
const UserPassword = "654321";

// Encryption algorithm. 
const EncryptionAlgorithm = "AES_128bit";

// Allow or prohibit various PDF permissions
const AllowAccessibilitySupport = true;
const AllowAssemblyDocument = true;
const AllowPrintDocument = true;
const AllowFillForms = true;
const AllowModifyDocument = true;
const AllowContentExtraction = true;
const AllowModifyAnnotations = true;

// Allowed printing quality.
const PrintQuality = "HighResolution";

// Runs processing asynchronously
const asyncProcessing = true;

// Prepare request to `PDF Security` API endpoint
var queryPath = `/v1/pdf/security/add`;

// JSON payload for API request
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
    async: asyncProcessing
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
        if (data.error === false) {
            console.log(`Job #${data.jobId} has been created!`);
            checkIfJobIsCompleted(data.jobId, data.url);
        } else {
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

function checkIfJobIsCompleted(jobId, resultFileUrl) {
    let queryPath = `/v1/job/check`;

    // JSON payload for API request
    let jsonPayload = JSON.stringify({
        jobid: jobId
    });

    let reqOptions = {
        host: "api.pdf.co",
        path: queryPath,
        method: "POST",
        headers: {
            "x-api-key": API_KEY,
            "Content-Type": "application/json",
            "Content-Length": Buffer.byteLength(jsonPayload, 'utf8')
        }
    };

    // Send request
    var postRequest = https.request(reqOptions, (response) => {
        response.on("data", (d) => {
            response.setEncoding("utf8");

            // Parse JSON response
            let data = JSON.parse(d);
            console.log(`Checking Job #${jobId}, Status: ${data.status}, Time: ${new Date().toLocaleString()}`);

            if (data.status === "working") {
                // Check again after 3 seconds
                setTimeout(() => { checkIfJobIsCompleted(jobId, resultFileUrl); }, 3000);
            } else if (data.status === "success") {
                // Download PDF file
                var file = fs.createWriteStream(DestinationFile);
                https.get(resultFileUrl, (response2) => {
                    response2.pipe(file)
                        .on("close", () => {
                            console.log(`Generated PDF file saved as "${DestinationFile}" file.`);
                        });
                });
            } else {
                console.log(`Operation ended with status: "${data.status}".`);
            }
        });
    });

    // Write request data
    postRequest.write(jsonPayload);
    postRequest.end();
}
