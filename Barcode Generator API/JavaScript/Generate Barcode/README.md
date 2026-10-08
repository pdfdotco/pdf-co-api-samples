## How to generate barcode for barcode generator API in JavaScript with PDF.co Web API

### Learn how to generate barcode to have barcode generator API in JavaScript

These source code samples are listed and grouped by their programming language and functions they use. PDF.co Web API helps with barcode generator API in JavaScript. PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

JavaScript code snippet like this for PDF.co Web API works best when you need to quickly implement barcode generator API in your JavaScript application. Follow the instruction and copy - paste code for JavaScript into your project's code editor. Test JavaScript sample code examples whether they respond your needs and requirements for the project.

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

##### **generate_barcode.js:**
    
```
function generateBarcode()
{
    // Hide result blocks
    document.getElementById("errorBlock").style.display = "none";
    document.getElementById("resultBlock").style.display = "none";

    // Get API Key
    var apiKey = document.getElementById("apiKey").value.trim();
    if (apiKey == "") {
        alert("API Key should not be empty.");
        return false;
    }
    // Get barcode type
    var barcodeType=document.getElementById("barcodeType").value;
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

    // Prepare request
    var httpRequest = new XMLHttpRequest();
    httpRequest.open("POST", url, true);
    httpRequest.setRequestHeader("x-api-key", apiKey); // set API Key
    // Asynchronous response handler
    httpRequest.onreadystatechange = function() {
        if (httpRequest.readyState == 4) {
            // If OK
            if (httpRequest.status == 200) {
                var result = JSON.parse(httpRequest.responseText);
                if (result.error == false) {
                    document.getElementById("resultBlock").style.display = "block"; // show hidden resultBlock
                    document.getElementById("image").setAttribute("src", result.url); // Set image link to display
                }
                else {
                    document.getElementById("errorBlock").style.display = "block"; // show hidden errorBlock
                    document.getElementById("error").innerHTML = result.message;
                }
            }
            // Else display error
            else {
                document.getElementById("errorBlock").style.display = "block"; // show hidden errorBlock
                document.getElementById("error").innerHTML = "Request failed. Please check you use the correct API key.";
            }

            // Hide loader
            showLoader(false);
        }
    }
    // Send request
    httpRequest.send();

    return true;
}

function showLoader(isDisplay){
    var loader = document.getElementById("loader");

    if(isDisplay){
        loader.style.display = "";
    }
    else{
        loader.style.display="none";
    }
}
```

<!-- code block end -->