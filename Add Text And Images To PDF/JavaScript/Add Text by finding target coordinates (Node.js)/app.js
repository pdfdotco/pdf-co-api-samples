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

// Visit Knowledgebase for adding Text Macros to PDF 
// https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "*****************************";

// Direct URL of source PDF file.
// You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
const SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf";

// Search string. 
const SearchString = 'Notes';

// Prepare URL for PDF text search API call.
// See documentation: https://apidocs.pdf.co/07-pdf-search-text
var queryFindText = `/v1/pdf/find`;

// JSON payload for find text
var jsonPayload_findText = JSON.stringify({ url: SourceFileUrl, searchString: SearchString });

let reqOptionsFindText = {
    host: "api.pdf.co",
    path: queryFindText,
    method: "POST",
    headers: {
        "x-api-key": API_KEY,
        "Content-Type": "application/json",
        "Content-Length": Buffer.byteLength(jsonPayload_findText, 'utf8')
    }
};

let chunks = [];

// Send request
var postRequest_FindText = https.request(reqOptionsFindText, (response_findText) => {
    response_findText.on("data", (data) => {
        chunks.push(data);
    }).on("end", function () {
        let d_findText = Buffer.concat(chunks);

        // Parse JSON response
        let dataFindText = JSON.parse(d_findText);
        if (dataFindText.body.length > 0) {
            var element = dataFindText.body[0];
            console.log("Found text " + element["text"] + " at coordinates " + element["left"] + ", " + element["top"]);

            // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
            const Pages = "";

            // PDF document password. Leave empty for unprotected documents.
            const Password = "";

            // Destination PDF file name
            const DestinationFile = "./result.pdf";

            // Text annotation params
            const X = +element["left"];
            const Y = +element["top"] + 25;
            const Text = "Some notes will go here... Some notes will go here.... Some notes will go here.....";
            const FontName = "Times New Roman";
            const FontSize = 12;
            const Color = "FF0000";

            // * Add Text *
            // Prepare request to `PDF Edit` API endpoint
            var queryPath = `/v1/pdf/edit/add`;

            // JSON payload for api request
            var jsonPayload = JSON.stringify({
                name: path.basename(DestinationFile),
                password: Password,
                url: SourceFileUrl,
                annotations: [{
                    text: Text,
                    x: X,
                    y: Y,
                    pages: Pages,
                    fontname: FontName,
                    size: FontSize,
                    color: Color
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
                        var file = fs.createWriteStream(DestinationFile);
                        https.get(data.url, (response2) => {
                            response2.pipe(file).on("close", () => {
                                console.log(`Generated PDF file saved to '${DestinationFile}' file.`);
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

        } else {
            console.error("No result found.");
        }
    }).on("error", (e) => {
        console.error("Error: ", error);
    })
});

// Write request data
postRequest_FindText.write(jsonPayload_findText);
postRequest_FindText.end();
