//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


// Visit Knowledgebase for adding Text Macros to PDF 
// https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

var https = require("https");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "**************************";

// Direct URL of source PDF file.
// You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
const SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf";

// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
const Pages = "";

// PDF document password. Leave empty for unprotected documents.
const Password = "";

// Destination PDF file name
const DestinationFile = "./result.pdf";

// Text annotation params
const Type = "annotation";
const X = 400;
const Y = 600;
const Text = "APPROVED";
const FontName = "Times New Roman";
const FontSize = 24;
const Color = "FF0000";

// * Add Text *
// Prepare request to `PDF Edit` API endpoint
var queryPath = `/v1/pdf/edit/add`;

// JSON payload for api request
var jsonPayload = JSON.stringify({
    name: 'result.pdf',
    url: SourceFileUrl,
    password: Password,
    annotations:[
        {
            pages: Pages,
            x: X,
            y: Y,
            text: Text,
            fontname: FontName,
            size: FontSize,
            color: Color
        }
    ]
});

var reqOptions = {
    host: "api.pdf.co",
    method: "POST",
    path: encodeURI(queryPath),
    headers: {
        "x-api-key": API_KEY,
        "Content-Type": "application/json",
        "Content-Length": Buffer.byteLength(jsonPayload, 'utf8')
    }
};


exports.handler = async (event) => {

    let dataString = '';
    const promise_response = await new Promise((resolve, reject) => {
        
        // Send request
        var postRequest = https.request(reqOptions, (response) => {
            response.on('data', chunk => {
                dataString += chunk;
            });
            
            response.on('end', () => {
                resolve({
                    statusCode: 200,
                    body: JSON.stringify(JSON.parse(dataString), null, 4)
                });
            });
              
        }).on("error", (e) => {
            reject({
                statusCode: 500,
                body: 'Something went wrong!'
            });
        });
        
        // Write request data
        postRequest.write(jsonPayload);
        postRequest.end();
    });

    return promise_response;
};
