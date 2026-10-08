## How to generate barcode (jquery) for barcode generator API in JavaScript and PDF.co Web API

### Step By Step Tutorial: how to generate barcode (jquery) for barcode generator API in JavaScript

These source code samples are listed and grouped by their programming language and functions they use. PDF.co Web API helps with barcode generator API in JavaScript. PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

This rich sample source code in JavaScript for PDF.co Web API includes the number of functions and options you should do calling the API to implement barcode generator API. Open your JavaScript project and simply copy & paste the code and then run your app! This basic programming language sample code for JavaScript will do the whole work for you in implementing barcode generator API in your app.

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
$(document).ready(function () {
    $("#resultBlock").hide();
    $("#errorBlock").hide();
});

$(document).on("click", "#submit", function () {
    var apiKey = $("#apiKey").val().trim(); // Get your API key by registering at https://apidocs.pdf.co

    var url = "https://api.pdf.co/v1/barcode/generate";

    var oData = {
        name: 'barcode.png',
        type: $("#barcodeType").val(), // Set barcode type (symbology)
        value: $("#inputValue").val() // Set barcode value
    };

    $.ajax({
        url: url,
        type: "POST",
        data: oData,
        headers: {
            "x-api-key": apiKey
        },
    })
    .done (function(data, textStatus, jqXHR) { 
        if (data.error == false) {
            $("#resultBlock").show();
            $("#image").attr("src", data.url);
        }
        else {
            $("#errorBlock").show();
            $("#error").html(data.message);
        }
    })
    .fail (function(jqXHR, textStatus, errorThrown) { 
        $("#errorBlock").show();
        $("#error").html("Request failed. Please check you use the correct API key.");
    });
});

```

<!-- code block end -->