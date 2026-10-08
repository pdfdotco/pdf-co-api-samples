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

// Import axios for making HTTP requests
const axios = require("axios");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "************************";

// Source PDF file
const SourceFile = "./sample.pdf";
// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
const Pages = "";
// PDF document password. Leave empty for unprotected documents.
const Password = "";
// Destination XML file name
const DestinationFile = "./result.xml";

// 1. RETRIEVE PRESIGNED URL TO UPLOAD FILE.
getPresignedUrl(API_KEY, SourceFile)
    .then(([uploadUrl, uploadedFileUrl]) => {
        // 2. UPLOAD THE FILE TO CLOUD.
        uploadFile(API_KEY, SourceFile, uploadUrl)
            .then(() => {
                // 3. CONVERT UPLOADED PDF FILE TO XML
                convertPdfToXml(API_KEY, uploadedFileUrl, Password, Pages, DestinationFile);
            })
            .catch(e => {
                console.log(e);
            });
    })
    .catch(e => {
        console.log(e);
    });

function getPresignedUrl(apiKey, localFile) {
    return new Promise((resolve, reject) => {
        // Prepare request to `Get Presigned URL` API endpoint
        let queryPath = `/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=${path.basename(SourceFile)}`;
        let reqOptions = {
            host: "api.pdf.co",
            path: encodeURI(queryPath),
            headers: { "x-api-key": API_KEY }
        };
        // Send request
        https.get(reqOptions, (response) => {
            response.on("data", (d) => {
                let data = JSON.parse(d);
                if (data.error === false) {
                    // Return presigned url we received
                    resolve([data.presignedUrl, data.url]);
                } else {
                    // Service reported error
                    console.log("getPresignedUrl(): " + data.message);
                    reject(data.message);
                }
            });
        })
            .on("error", (e) => {
                // Request error
                console.log("getPresignedUrl(): " + e);
                reject(e);
            });
    });
}

function uploadFile(apiKey, localFile, uploadUrl) {
    return new Promise((resolve, reject) => {
        fs.readFile(SourceFile, (err, data) => {
            if (err) {
                console.log("Error reading file: ", err);
                reject(err);
                return;
            }

            // Use axios to upload the file
            axios.put(uploadUrl, data, {
                headers: {
                    "Content-Type": "application/octet-stream"
                }
            })
                .then(() => {
                    resolve();
                })
                .catch((err) => {
                    console.log("uploadFile() error: " + err);
                    reject(err);
                });
        });
    });
}

function convertPdfToXml(apiKey, uploadedFileUrl, password, pages, destinationFile) {
    // Prepare request to `PDF To XML` API endpoint
    var queryPath = `/v1/pdf/convert/to/xml`;

    // JSON payload for api request
    var jsonPayload = JSON.stringify({
        name: path.basename(destinationFile), password: password, pages: pages, url: uploadedFileUrl, async: true
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
            response.setEncoding("utf8");
            // Parse JSON response
            let data = JSON.parse(d);
            if (data.error === false) {
                // Process returned job
                console.log(`Job #${data.jobId} has been created!`);
                checkIfJobIsCompleted(data.jobId, data.url, destinationFile);
            } else {
                // Service reported error
                console.log("convertPdfToXml(): " + data.message);
            }
        });
    })
        .on("error", (e) => {
            // Request error
            console.log("convertPdfToXml(): " + e);
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

            if (data.status === "working") {
                // Check again after 3 seconds
                setTimeout(function () {
                    checkIfJobIsCompleted(jobId, resultFileUrl, destinationFile);
                }, 3000);
            } else if (data.status === "success") {
                // Download XML file
                var file = fs.createWriteStream(destinationFile);
                https.get(resultFileUrl, (response2) => {
                    response2.pipe(file)
                        .on("close", () => {
                            console.log(`Generated XML file saved as "${destinationFile}" file.`);
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
