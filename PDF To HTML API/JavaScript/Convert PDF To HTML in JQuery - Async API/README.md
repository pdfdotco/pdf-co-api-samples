## How to convert PDF to HTML in jquery async API for PDF to HTML API in JavaScript with PDF.co Web API

### How to convert PDF to HTML in jquery async API for PDF to HTML API in JavaScript: Step By Step Tutorial

Here you may find thousands pre-made source code pieces for easy implementation in your own programming projects. PDF.co Web API was made to help with PDF to HTML API in JavaScript. PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

Fast application programming interfaces of PDF.co Web API for JavaScript plus the instruction and the code below will help to learn how to convert PDF to HTML in jquery async API. Sample code in JavaScript is all you need. Copy-paste it to your the code editor, then add a reference to PDF.co Web API and you are ready to try it! Enjoy writing a code with ready-to-use sample JavaScript codes to add PDF to HTML API functions using PDF.co Web API in JavaScript.

ByteScout free trial version is available for FREE download from our website. Programming tutorials along with source code samples are included.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://bytescout.zendesk.com/hc/en-us/requests/new?subject=PDF.co%20Web%20API%20Question)

or just send email to [support@bytescout.com](mailto:support@bytescout.com?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)
[Sign Up For Online Training](https://academy.bytescout.com/)


## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://apidocs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **converter.js:**
    
```
var apiKey, formData, toType, isInline;

$(document).ready(function () {
    $("#resultBlock").hide();
    $("#errorBlock").hide();
    $("#result").attr("href", '').html('');
});
 
$(document).on("click", "#submit", function () {
    $("#resultBlock").hide();
    $("#errorBlock").hide();
    $("#inlineOutput").text(''); // inline output div
    $("#status").text(''); // status div
 
    apiKey = $("#apiKey").val().trim(); //Get your API key at https://app.pdf.co
 
    formData = $("#form input[type=file]")[0].files[0]; // file to upload
    toType = $("#convertType").val(); // output type
    isInline = $("#outputType").val() == "inline"; // if we need output as inline content or link to output file

    $("#status").html('Requesting presigned url for upload... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

    $.ajax({
        url: 'https://api.pdf.co/v1/file/upload/get-presigned-url?name=test.pdf&contenttype=application/pdf&encrypt=true',
        type: 'GET',
        headers: {'x-api-key': apiKey}, // passing our api key
        success: function (result) {    

            if (result['error'] === false) {

                var presignedUrl = result['presignedUrl']; // reading provided presigned url to put our content into
                $("#status").html('Uploading... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

                $.ajax({
                    url: presignedUrl, // no api key is required to upload file
                    type: 'PUT',
                    headers: {'content-type': 'application/pdf'}, // setting to pdf type as we are uploading pdf file
                    data: formData,
                    processData: false,
                    success: function (result) {                               
                        
                        $("#status").html('Processing... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

                        $.ajax({
                            url: 'https://api.pdf.co/v1/pdf/convert/to/'+toType+'?url='+ presignedUrl + '&encrypt=true&inline=' + isInline + '&async=True',
                            type: 'POST',
                            headers: {'x-api-key': apiKey},
                            success: function (result) { 
                                if (result.error) {
                                    $("#status").text('Error uploading file.');
                                }
                                else {
                                    checkIfJobIsCompleted(result.jobId, result.url);
                                }
                            }
                        });
                    },
                    error: function () {
                        $("#status").text('error');
                    }
                  });                

                }
            }
        });
});
 
function checkIfJobIsCompleted(jobId, resultFileUrl) {
    $.ajax({
        url: 'https://api.pdf.co/v1/job/check?jobid=' + jobId,
        type: 'GET',
        headers: { 'x-api-key': apiKey }, // passing our api key
        success: function (jobResult) {

            $("#status").html(jobResult.status + ' &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

            if (jobResult.status == "working") {
                // Check again after 3 seconds
                setTimeout(checkIfJobIsCompleted(jobId, resultFileUrl), 3000)
            }
            else if (jobResult.status == "success") {

                $("#status").text('Done converting.');

                $("#resultBlock").show();

                if (isInline && toType != "xls" && toType != "xlsx") {
                    $.ajax({
                        url: resultFileUrl,
                        dataType: 'text',
                        success: function (respText) {
                            $("#inlineOutput").text(respText);
                        }
                    });
                }
                else {
                    $("#result").attr("href", resultFileUrl).html(resultFileUrl);
                }
            }
        }
    });
}

```

<!-- code block end -->