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
const API_KEY = "***********************************";

// Direct URL of source PDF file.
// You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
const SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-to-text/sample.pdf";

// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
const Pages = "";

// PDF document password. Leave empty for unprotected documents.
const Password = "";

// Prepare URL for PDF Table Search API call.
// See documentation: https://apidocs.pdf.co
var query = `https://api.pdf.co/v1/pdf/find/table`;
let reqOptions = {
    uri: query,
    headers: { "x-api-key": API_KEY },
    formData: {
        password: Password,
        pages: Pages,
        url: SourceFileUrl
    }
};

// Send request
request.post(reqOptions, function (error, resp, body) {
    if (error) {
        return console.error("Error: ", error);
    }

    var jsonBody = JSON.parse(body);

    // Loop through all found tables, and get json data
    if (jsonBody.body.tables && jsonBody.body.tables.length > 0) {
        for (var i = 0; i < jsonBody.body.tables.length; i++) {
            getJSONFromCoordinates(SourceFileUrl, jsonBody.body.tables[i].PageIndex, jsonBody.body.tables[i].rect, `table_${i + 1}.json`);
        }
    }

});

/**
 * Get JSON from specific co-ordinates
 */
function getJSONFromCoordinates(fileUrl, pageIndex, rect, outputFileName) {

    // Prepare request to `PDF To JSON` API endpoint
    var jsonQueryPath = `https://api.pdf.co/v1/pdf/convert/to/json`;

    // Json Request 
    let jsonReqOptions = {
        uri: jsonQueryPath,
        headers: { "x-api-key": API_KEY },
        formData: {
            pages: pageIndex,
            url: fileUrl,
            rect: rect
        }
    };

    // Send request
    request.post(jsonReqOptions, function (error, resp, body) {
        if (error) {
            return console.error("Error: ", error);
        }

        var outputJsonUrl = JSON.parse(body).url;

        // Download JSON file
        var file = fs.createWriteStream(outputFileName);
        https.get(outputJsonUrl, (response2) => {
            response2.pipe(file)
                .on("close", () => {
                    console.log(`Generated JSON file saved as "${outputFileName}" file.`);
                });
        });

    });
}

