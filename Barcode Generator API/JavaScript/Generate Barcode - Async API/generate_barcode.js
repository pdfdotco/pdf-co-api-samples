//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


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
