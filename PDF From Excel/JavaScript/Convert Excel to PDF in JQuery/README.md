## How to convert excel to PDF in jquery for excel to PDF API in JavaScript with PDF.co Web API

### Step By Step Tutorial: how to convert excel to PDF in jquery for excel to PDF API in JavaScript

Writing of the code to convert excel to PDF in jquery in JavaScript can be done by developers of any level using PDF.co Web API. PDF.co Web API was made to help with excel to PDF API in JavaScript. PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

JavaScript code samples for JavaScript developers help to speed up the application's code writing when using PDF.co Web API. For implimentation of this functionality, please copy and paste code below into your app using code editor. Then compile and run your app. Enjoy writing a code with ready-to-use sample JavaScript codes to implement excel to PDF API using PDF.co Web API.

Trial version of ByteScout is available for free download from our website. This and other source code samples for JavaScript and other programming languages are available.

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

    var apiKey = $("#apiKey").val().trim(); //Get your API key at https://app.pdf.co

    var formData = $("#form input[type=file]")[0].files[0]; // file to upload
    var toType = $("#convertType").val(); // output type
    var isInline = $("#outputType").val() == "inline"; // if we need output as inline content or link to output file

    $("#status").html('Requesting presigned url for upload... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

    $.ajax({
        url: 'https://api.pdf.co/v1/file/upload/get-presigned-url?name=test.pdf&encrypt=true',
        type: 'GET',
        headers: { 'x-api-key': apiKey }, // passing our api key
        success: function (result) {

            if (result['error'] === false) {
                var presignedUrl = result['presignedUrl']; // reading provided presigned url to put our content into
                var accessUrl = result['url']; // reading output url that will indicate uploaded file

                $("#status").html('Uploading... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

                $.ajax({
                    url: presignedUrl, // no api key is required to upload file
                    type: 'PUT',
                    data: formData,
                    processData: false,
                    success: function (result) {

                        $("#status").html('Processing... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

                        $.ajax({
                            url: 'https://api.pdf.co/v1/xls/convert/to/' + toType + '?url=' + presignedUrl + '&encrypt=true&inline=' + isInline,
                            type: 'POST',
                            headers: { 'x-api-key': apiKey },
                            success: function (result) {

                                $("#status").text('done converting.');

                                // console.log(JSON.stringify(result));

                                $("#resultBlock").show();

                                if (isInline) {
                                    $("#inlineOutput").text(result['body']);
                                }
                                else {
                                    $("#result").attr("href", result['url']).html(result['url']);
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


```

<!-- code block end -->