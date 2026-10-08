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
const SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-to-excel/sample.pdf";
// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
const Pages = "";
// PDF document password. Leave empty for unprotected documents.
const Password = "";
// Destination XLS file name
const DestinationFile = "./result.xls";

/*
Some of advanced options available through profiles:
(JSON can be single/double-quoted and contain comments.)
{
    "profiles": [
        {
            "profile1": {
                "NumberDecimalSeparator": "", // Allows to customize decimal separator in numbers.
                "NumberGroupSeparator": "", // Allows to customize thousands separator.
                "AutoDetectNumbers": true, // Whether to detect numbers. Values: true / false
                "RichTextFormatting": true, // Whether to keep text style and fonts. Values: true / false
                "PageToWorksheet": true, // Whether to create separate worksheet for each page of PDF document. Values: true / false
                "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
                "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
                "LineGroupingMode": "None", // Values: "None", "GroupByRows", "GroupByColumns", "JoinOrphanedRows"
                "ColumnDetectionMode": "ContentGroupsAndBorders", // Values: "ContentGroupsAndBorders", "ContentGroups", "Borders", "BorderedTables"
                "Unwrap": false, // Unwrap grouped text in table cells. Values: true / false
                "ShrinkMultipleSpaces": false, // Shrink multiple spaces in table cells that affect column detection. Values: true / false
                "DetectNewColumnBySpacesRatio": 1, // Spacing ratio that affects column detection.
                "CustomExtractionColumns": [ 0, 50, 150, 200, 250, 300 ], // Explicitly specify columns coordinates for table extraction.
                "CheckPermissions": true, // Ignore document permissions. Values: true / false
            }
        }
    ]
}
*/
// Sample profile that sets advanced conversion options.
// Advanced options are properties of XLSExtractor class from ByteScout PDF Extractor SDK used in the back-end:
// https://cdn.bytescout.com/help/BytescoutPDFExtractorSDK/html/2712c05b-9674-5253-df76-2a31ed055afd.htm
const Profiles = '{ "profiles": [ { "profile1": { "RichTextFormatting": false, "PageToWorksheet": false } } ] }';

// Prepare request to `PDF To XLS` API endpoint
var queryPath = `/v1/pdf/convert/to/xls`;

// JSON payload for api request
var jsonPayload = JSON.stringify({
    name: path.basename(DestinationFile), password: Password, pages: Pages, url: SourceFileUrl, profiles: Profiles, async: true
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
            console.log(`Job #${data.jobId} has been created!`);
            checkIfJobIsCompleted(data.jobId, data.url);
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

function checkIfJobIsCompleted(jobId, resultFileUrl) {
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
                setTimeout(function () { checkIfJobIsCompleted(jobId, resultFileUrl); }, 3000);
            }
            else if (data.status == "success") {
                // Download XLS file
                var file = fs.createWriteStream(DestinationFile);
                https.get(resultFileUrl, (response2) => {
                    response2.pipe(file)
                        .on("close", () => {
                            console.log(`Generated XLS file saved as "${DestinationFile}" file.`);
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
