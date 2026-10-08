//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


var apiKey, uploadedFile1Url, uploadedFile2Url;

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

    var formData1 = $("#form input[type=file]")[0].files[0] // file to upload
    var formData2 = $("#form input[type=file]")[1].files[0] // file to upload

    var uploadFile2CallbackFn = function (uploadedUrl) {
        uploadedFile2Url = uploadedUrl;

        // Perform Merge
        MergeFiles(uploadedFile1Url, uploadedFile2Url);
    }

    var uploadFile1CallbackFn = function (uploadedUrl) {
        uploadedFile1Url = uploadedUrl;

        // Upload File - 2
        uploadFile(formData2, uploadFile2CallbackFn);
    }

    // Upload File - 1
    uploadFile(formData1, uploadFile1CallbackFn);

});

function uploadFile(formData, callbackFunction) {
    $.ajax({
        url: 'https://api.pdf.co/v1/file/upload/get-presigned-url?name=test.pdf&contenttype=application/pdf&encrypt=true',
        type: 'GET',
        headers: { 'x-api-key': apiKey }, // passing our api key
        success: function (result) {

            if (result['error'] === false) {
                console.log(result);
                var presignedUrl = result['presignedUrl']; // reading provided presigned url to put our content into
                var uploadedUrl = result['url']; // Uploaded URL

                $("#status").html('Uploading File ... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

                $.ajax({
                    url: presignedUrl, // no api key is required to upload file
                    type: 'PUT',
                    headers: { 'content-type': 'application/pdf' }, // setting to pdf type as we are uploading pdf file
                    data: formData,
                    processData: false,
                    success: function (result) {
                        $("#status").html('File Uploaded');

                        if (typeof callbackFunction === "function") {
                            callbackFunction(uploadedUrl);
                        }
                    },
                    error: function () {
                        $("#status").text('error');
                    }
                });
            }
        }
    });
}

function MergeFiles(url1, url2) {
   
    var oBody = {
        "url": `${url1},${url2}`,
        "name": "result.pdf"
    };

    $("#status").html('Processing Merge... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

    $.ajax({
        url: 'https://api.pdf.co/v1/pdf/merge',
        type: 'POST',
        headers: { 'x-api-key': apiKey, 'Content-Type': 'application/json' },
        data: JSON.stringify(oBody),
        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
        processData: false, // NEEDED, DON'T OMIT THIS
        success: function (result) {
            $("#status").text('Success!');

            $("#resultBlock").show();
            $("#inlineOutput").html('<iframe style="width:100%; height:500px;" src="' + result.url + '" />');
        },
        error: function () {
            $("#status").text('error');
        }
    });
}
