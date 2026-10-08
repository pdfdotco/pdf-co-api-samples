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
const API_KEY = "***************************";

// HTML Input
const inputHtml = "./sample.html";
// Destination PDF file name
const DestinationFile = "./result.pdf";

// Prepare requests params as JSON
// See documentation: https://apidocs.pdf.co/?#1-json-pdfconvertfromhtml
var parameters = {};

// Input HTML code to be converted. Required.
parameters["html"] = fs.readFileSync(inputHtml, "utf8");

// Name of resulting file
parameters["name"] = path.basename(DestinationFile);

// Set to css style margins like 10 px or 5px 5px 5px 5px.
parameters["margins"] = "5px 5px 5px 5px";

// Can be Letter, A4, A5, A6 or custom size like 200x200
parameters["paperSize"] = "Letter";

// Set to Portrait or Landscape. Portrait by default.
parameters["orientation"] = "Portrait";

// true by default. Set to false to disbale printing of background.
parameters["printBackground"] = true;

// If large input document, process in async mode by passing true
parameters["async"] = false;

// Set to HTML for header to be applied on every page at the header.
parameters["header"] = "";

// Set to HTML for footer to be applied on every page at the bottom.
parameters["footer"] = "";


// Convert JSON object to string
var jsonPayload = JSON.stringify(parameters);

// Prepare request to `HTML To PDF` API endpoint
var url = '/v1/pdf/convert/from/html';
var reqOptions = {
    host: "api.pdf.co",
    path: url,
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
