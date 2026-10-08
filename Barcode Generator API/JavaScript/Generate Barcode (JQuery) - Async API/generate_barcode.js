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

$(document).ready(function () {
    $("#resultBlock").hide();
    $("#errorBlock").hide();
});

$(document).on("click", "#submit", function () {
    apiKey = $("#apiKey").val().trim(); //Get your API key by registering at https://apidocs.pdf.co

    var url = "https://api.pdf.co/v1/barcode/generate";

    var oData = {
        name: 'barcode.png',
        type: $("#barcodeType").val(), // Set barcode type (symbology)
        value: $("#inputValue").val(), // Set barcode value
        async: true
    };

    // Show loader
    $("#loader").show();

    $.ajax({
        url: url,
        data: oData,
        type: "GET",
        headers: {
            "x-api-key": apiKey
        },
    })
        .done(function (data, textStatus, jqXHR) {

            if (data.error) {
                $("#errorBlock").show();
                $("#error").html(data.message);
                $("#loader").hide();
            }
            else {
                checkIfJobIsCompleted(data.jobId, data.url);
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $("#errorBlock").show();
            $("#error").html("Request failed. Please check you use the correct API key.");
            $("#loader").hide();
        });
});

function checkIfJobIsCompleted(jobId, resultFileUrl) {
    $.ajax({
        url: 'https://api.pdf.co/v1/job/check?jobid=' + jobId,
        type: 'POST',
        headers: { 'x-api-key': apiKey }, // passing our api key
        success: function (jobResult) {

            $("#status").html(jobResult.status + ' &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

            if (jobResult.status == "working") {
                // Check again after 3 seconds
                setTimeout(function(){
                    checkIfJobIsCompleted(jobId, resultFileUrl);
                }, 3000);
            }
            else if (jobResult.status == "success") {
                $("#resultBlock").show();
                $("#image").attr("src", resultFileUrl);
            }

            $("#loader").hide();
        },
        error: function(){
            $("#errorBlock").show();
            $("#error").html("Request failed. Please check you use the correct API key.");
            $("#loader").hide();
        }
    });
}
