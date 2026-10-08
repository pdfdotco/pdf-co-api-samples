//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


/*jshint esversion: 6 */

var https = require("https");
var path = require("path");
var fs = require("fs");

// `request` module is required for file upload.
// Use "npm install request" command to install.
var request = require("request");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "*********************************";

// Source DOC or DOCX file
const SourceFile = "./sample.docx";
// Destination PDF file name
const DestinationFile = "./result.pdf";

// Input file Base64
const SourceFileBase64 = fs.readFileSync(SourceFile, { encoding: 'base64' });

// 1. Get Uploaded File Url from Base64 Source
uploadBase64ToPDFco(SourceFileBase64, 'sample.docx')
    .then((respBase64FileUrl) => {
        // 2. CONVERT UPLOADED DOC (DOCX) FILE TO PDF
        convertDocToPdf(API_KEY, respBase64FileUrl, DestinationFile);
    })
    .catch(e => {
        console.log(e);
    });;


function uploadBase64ToPDFco(SourceFileBase64, inputFileName) {
    return new Promise(resolve => {
        var options = {
            'method': 'POST',
            'url': 'https://api.pdf.co/v1/file/upload/base64',
            'headers': {
                'x-api-key': API_KEY
            },
            formData: {
                'file': SourceFileBase64,
                'name': inputFileName
            }
        };

        request(options, function (err, res, body) {
            if (!err) {
                var data = JSON.parse(res.body)
                resolve(data.url);
            }
            else {
                console.log("uploadFile() request error: " + e);
            }
        });
    });
}

function convertDocToPdf(apiKey, uploadedFileUrl, destinationFile) {
    // Prepare URL for `DOC To PDF` API call
    let queryPath = `/v1/pdf/convert/from/doc`;

    // JSON payload for api request
    var jsonPayload = JSON.stringify({
        name: path.basename(destinationFile), url: uploadedFileUrl, async: true
    });

    var reqOptions = {
        host: "api.pdf.co",
        method: "POST",
        path: queryPath,
        headers: {
            "x-api-key": apiKey,
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
            if (data.error == false) {
                console.log(`Job #${data.jobId} has been created!`);
                checkIfJobIsCompleted(data.jobId, data.url, destinationFile);
            }
            else {
                // Service reported error
                console.log("convertDocToPdf(): " + data.message);
            }
        });
    })
        .on("error", (e) => {
            // Request error
            console.log("convertDocToPdf(): " + e);
        });

    // Write request data
    postRequest.write(jsonPayload);
    postRequest.end();
}

function checkIfJobIsCompleted(jobId, resultFileUrl, destinationFile) {
    let queryPath = `/v1/job/check`;

    // JSON payload for api request
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

            if (data.status == "working") {
                // Check again after 3 seconds
                setTimeout(function () { checkIfJobIsCompleted(jobId, resultFileUrl, destinationFile); }, 3000);
            }
            else if (data.status == "success") {
                // Download PDF file
                var file = fs.createWriteStream(destinationFile);
                https.get(resultFileUrl, (response2) => {
                    response2.pipe(file)
                        .on("close", () => {
                            console.log(`Generated PDF file saved as "${destinationFile}" file.`);
                        });
                });
            }
            else {
                console.log(`Operation ended with status: "${data.status}".`);
            }
        })
    });

    // Write request data
    postRequest.write(jsonPayload);
    postRequest.end();
}
