//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


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

    var signatureImageUrl = $("#signatureImageUrl").val();
    var destinationXCoordinate = $("#destinationXCoordinate").val();
    var destinationYCoordinate = $("#destinationYCoordinate").val();
    var destinationHeight = $("#destinationHeight").val();
    var destinationWidth = $("#destinationWidth").val();

    var formData = $("#SourceFile")[0].files[0]; // file to upload

    $("#status").html('Getting presigned url... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

    $.ajax({
        url: 'https://api.pdf.co/v1/file/upload/get-presigned-url?name=test.pdf&contenttype=application/pdf&encrypt=true',
        type: 'GET',
        headers: { 'x-api-key': apiKey }, // passing our api key
        success: function (result) {

            if (result['error'] === false) {
                console.log(result);
                var presignedUrl = result['presignedUrl']; // reading provided presigned url to put our content into
                var uploadedUrl = result['url']; // Uploaded URL

                $("#status").html('Uploading... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

                $.ajax({
                    url: presignedUrl, // no api key is required to upload file
                    type: 'PUT',
                    headers: { 'content-type': 'application/pdf' }, // setting to pdf type as we are uploading pdf file
                    data: formData,
                    processData: false,
                    success: function (result) {

                        // PDF.co API URL
                        var cUrl = 'https://api.pdf.co/v1/pdf/edit/add';

                        // Input data
                        var data = {
                            name: 'result.pdf',
                            url: uploadedUrl,
                            images: [
                                {
                                    url: signatureImageUrl,
                                    x: destinationXCoordinate,
                                    y: destinationYCoordinate,
                                    width: destinationWidth,
                                    height: destinationHeight
                                }
                            ]
                        };

                        console.log(data);

                        $("#status").html('Processing... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');
                        $.ajax({
                            url: cUrl,
                            type: 'POST',
                            headers: { 'x-api-key': apiKey, 'Content-Type': 'application/json' },
                            data: JSON.stringify(data),
                            processData: false,
                            contentType: false,
                            //data: oData,
                            success: function (result) {
                                $("#status").text('done processing.');

                                if (result.error) {
                                    $("#errorBlock").show();
                                    $("#errors").text(result.message);
                                } else {
                                    $("#resultBlock").show();
                                    $("#inlineOutput").text(JSON.stringify(result));
                                    $("#iframeResultPdf").prop("src", result.url);
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

