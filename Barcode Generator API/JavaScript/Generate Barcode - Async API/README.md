## How to generate barcode async API for barcode generator API in JavaScript using PDF.co Web API

### How to generate barcode async API for barcode generator API in JavaScript: How To Tutorial

Today we will explain the steps and algorithm of how to generate barcode async API and how to make it work in your application. PDF.co Web API helps with barcode generator API in JavaScript. PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

The SDK samples like this one below explain how to quickly make your application do barcode generator API in JavaScript with the help of PDF.co Web API. For implimentation of this functionality, please copy and paste code below into your app using code editor. Then compile and run your app. You can use these JavaScript sample examples in one or many applications.

PDF.co Web API - free trial version is on available our website. Also, there are other code samples to help you with your JavaScript application included into trial version.

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

##### **generate_barcode.js:**
    
```
var apiKey = "";

function generateBarcode() {
    // Hide result blocks
    document.getElementById("errorBlock").style.display = "none";
    document.getElementById("resultBlock").style.display = "none";

    // Get API Key
    apiKey = document.getElementById("apiKey").value.trim();
    if (apiKey == "") {
        alert("API Key should not be empty.");
        return false;
    }
    // Get barcode type
    var barcodeType = document.getElementById("barcodeType").value;
    // Get barcode value
    var inputValue = document.getElementById("inputValue").value.trim();
    if (inputValue == null || inputValue == "") {
        alert("Barcode Value should not be empty.");
        return false;
    }

    //show loader
    showLoader(true);

    // Prepare URL
    var url = "https://api.pdf.co/v1/barcode/generate?name=barcode.png";
    url += "&type=" + barcodeType; // Set barcode type (symbology)
    url += "&value=" + inputValue; // Set barcode value
    url += "&async=True"; // Set async api

    // Prepare request
    var httpRequest = new XMLHttpRequest();
    httpRequest.open("POST", url, true);
    httpRequest.setRequestHeader("x-api-key", apiKey); // set API Key
    // Asynchronous response handler
    httpRequest.onreadystatechange = function () {
        if (httpRequest.readyState == 4) {
            // If OK
            if (httpRequest.status == 200) {
                var result = JSON.parse(httpRequest.responseText);
                checkIfJobIsCompleted(result.jobId, result.url);
            }
            // Else display error
            else {
                document.getElementById("errorBlock").style.display = "block"; // show hidden errorBlock
                document.getElementById("error").innerHTML = "Request failed. Please check you use the correct API key.";

                // Hide loader
                showLoader(false);
            }
        }
    }
    // Send request
    httpRequest.send();

    return true;
}

function checkIfJobIsCompleted(jobId, resultFileUrl) {

    var url = 'https://api.pdf.co/v1/job/check?jobid=' + jobId;

    // Prepare request
    var httpRequest = new XMLHttpRequest();
    httpRequest.open("POST", url, true);
    httpRequest.setRequestHeader("x-api-key", apiKey); // set API Key
    // Asynchronous response handler
    httpRequest.onreadystatechange = function () {
        if (httpRequest.readyState == 4) {
            // If OK
            if (httpRequest.status == 200) {
                var jobResult = JSON.parse(httpRequest.responseText);

                if (jobResult.status == "working") {
                    // Check again after 3 seconds
                    setTimeout(function(){
                        checkIfJobIsCompleted(jobId, resultFileUrl);
                    }, 3000);
                }
                else if (jobResult.status == "working") {
                    document.getElementById("resultBlock").style.display = "block"; // show hidden resultBlock
                    document.getElementById("image").setAttribute("src", resultFileUrl); // Set image link to display

                    // Hide loader
                    showLoader(false);
                }
            }
            // Else display error
            else {
                document.getElementById("errorBlock").style.display = "block"; // show hidden errorBlock
                document.getElementById("error").innerHTML = "Request failed. Please check you use the correct API key.";

                // Hide loader
                showLoader(false);
            }
        }
    }

    // Send request
    httpRequest.send();
}


function showLoader(isDisplay) {
    var loader = document.getElementById("loader");

    if (isDisplay) {
        loader.style.display = "";
    }
    else {
        loader.style.display = "none";
    }
}
```

<!-- code block end -->