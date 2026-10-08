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
const API_KEY = "***********************************";


// Source PDF file
const SourceFile1 = "./sample1.pdf";
const SourceFile2 = "./sample2.pdf";

// Destination PDF file name
const DestinationFile = "./result.pdf";


// Upload File-1: - 1. RETRIEVE PRESIGNED URL TO UPLOAD FILE.
getPresignedUrl(SourceFile1)
    .then(([uploadUrl1, uploadedFileUrl1]) => {

        // Upload File-1: - 2. UPLOAD THE FILE TO CLOUD.
        uploadFile(SourceFile1, uploadUrl1)
            .then(() => {

                // Upload File-2: - 1. RETRIEVE PRESIGNED URL TO UPLOAD FILE.
                getPresignedUrl(SourceFile2)
                    .then(([uploadUrl2, uploadedFileUrl2]) => {

                        // Upload File-2: - 2. UPLOAD THE FILE TO CLOUD.
                        uploadFile(SourceFile2, uploadUrl2)
                            .then(() => {

                                const SourceFiles = [
                                    uploadedFileUrl1,
                                    uploadedFileUrl2
                                ];

                                // Perform Merge PDF Documents
                                mergePDFDocuments(SourceFiles, DestinationFile);
                            })
                            .catch(e => {
                                console.log(e);
                            });
                    })
                    .catch(e => {
                        console.log(e);
                    });
            })
            .catch(e => {
                console.log(e);
            });
    })
    .catch(e => {
        console.log(e);
    });


function getPresignedUrl(localFile) {
    return new Promise(resolve => {
        // Prepare request to `Get Presigned URL` API endpoint
        let queryPath = `/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=${path.basename(localFile)}`;
        let reqOptions = {
            host: "api.pdf.co",
            path: encodeURI(queryPath),
            headers: { "x-api-key": API_KEY }
        };
        // Send request
        https.get(reqOptions, (response) => {
            let str_resp = '';
            response.on('data', function (chunk) {
                str_resp += chunk;
              });
            
            response.on("end", () => {
                let data = JSON.parse(str_resp);
                if (data.error == false) {
                    // Return presigned url we received
                    resolve([data.presignedUrl, data.url]);
                }
                else {
                    // Service reported error
                    console.log("getPresignedUrl(): " + data.message);
                }
            });
        })
            .on("error", (e) => {
                // Request error
                console.log("getPresignedUrl(): " + e);
            });
    });
}

function uploadFile(sourceFile, uploadUrl) {
    return new Promise(resolve => {
        fs.readFile(sourceFile, (err, data) => {
            request({
                method: "PUT",
                url: uploadUrl,
                body: data,
                headers: {
                    "Content-Type": "application/octet-stream"
                }
            }, (err, res, body) => {
                if (!err) {
                    resolve();
                }
                else {
                    console.log("uploadFile() request error: " + e);
                }
            });
        });
    });
}

function mergePDFDocuments(SourceFiles, DestinationFile) {
    // Prepare request to `Merge PDF` API endpoint
    var queryPath = `/v1/pdf/merge`;

    // JSON payload for api request
    var jsonPayload = JSON.stringify({
        name: path.basename(DestinationFile), url: SourceFiles.join(",")
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
}

