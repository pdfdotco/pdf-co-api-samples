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

    var formData = $("#form input[type=file]")[0].files[0]; // file to upload
    var toType = $("#convertType").val(); // output type
    var isInline = $("#outputType").val() == "inline"; // if we need output as inline content or link to output file

    $("#status").html('Requesting presigned url for upload... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');

    $.ajax({
        url: `https://api.pdf.co/v1/file/upload/get-presigned-url?name=test.${$("#inputFile").val().split('.').pop().toLowerCase()}&encrypt=true`,
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
                    success: function () {

                        $("#status").html('Processing... &nbsp;&nbsp;&nbsp; <img src="ajax-loader.gif" />');
                        $.ajax({
                            url: 'https://api.pdf.co/v1/xls/convert/to/' + toType,
                            type: 'POST',
                            headers: { 'x-api-key': apiKey, "Content-Type": "application/json" },
                            data: JSON.stringify({
                                "url": accessUrl,
                                "encrypt": true,
                                "inline": isInline
                            }),
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

