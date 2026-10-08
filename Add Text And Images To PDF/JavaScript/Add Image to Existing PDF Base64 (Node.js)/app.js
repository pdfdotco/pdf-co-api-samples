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

// `request` module is required for file upload.
// Use "npm install request" command to install.
var request = require("request");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "*****************************************";

// Source PDF file.
const SourceFile = "./sample.pdf";

// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
const Pages = "";

// PDF document password. Leave empty for unprotected documents.
const Password = "";

// Destination PDF file name
const DestinationFile = "./result.pdf";

// Image params
const Type = "image";
const X = 400;
const Y = 20;
const Width = 119;
const Height = 32;

// You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
const ImageUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png";

// Input file Base64
const SourceFileBase64 = fs.readFileSync(SourceFile, { encoding: 'base64' });

// 1. Get Uploaded File Url from Base64 Source
uploadBase64ToPDFco(SourceFileBase64, 'sample.pdf')
    .then((respBase64FileUrl) => {
        // 2. Add Image to PDF
        addImageToPdf(respBase64FileUrl, DestinationFile);
    })
    .catch(e => {
        console.log(e);
    });;


function addImageToPdf(sourceFileUrl, destinationFile) {
    // * Add image *
    // Prepare request to `PDF Edit` API endpoint
    var queryPath = `/v1/pdf/edit/add`;

    // JSON payload for api request
    var jsonPayload = JSON.stringify(
        {
            name: path.basename(destinationFile),
            password: Password,
            url: sourceFileUrl,
            images: [
                {
                    url: ImageUrl,
                    x: X,
                    y: Y,
                    width: Width,
                    height: Height,
                    pages: Pages
                }]
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
                // Download the PDF file
                var file = fs.createWriteStream(destinationFile);
                https.get(data.url, (response2) => {
                    response2.pipe(file).on("close", () => {
                        console.log(`Generated PDF file saved to '${destinationFile}' file.`);
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
        console.error(e);
    });

    // Write request data
    postRequest.write(jsonPayload);
    postRequest.end();
}


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
