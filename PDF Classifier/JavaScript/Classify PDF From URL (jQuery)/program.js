//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


var settings = {
    "url": "https://api.pdf.co/v1/pdf/classifier",
    "method": "POST",
    "timeout": 0,
    "headers": {
            "Content-Type": "application/json",
            "x-api-key": "YOUR_PDFCO_API_KEY"
    },
    "data": JSON.stringify({
      "url": "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/document-parser/sample-invoice.pdf",
      "async": false,
      "encrypt": "false",
      "inline": "true",
      "password": "",
      "profiles": ""
    }),
};

$.ajax(settings).done(function (response) {
    console.log(response);
});
