//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


var $canvas, $signaturePad;

$(document).ready(function () {
    $canvas = $('canvas');

    $("#btnAddSignatureToPDF").click(resizeSignatureAndProceedWithAppendingToPDF);

    $signaturePad = $('.clsCanvasContainer').signaturePad({
        drawOnly: true,
        defaultAction: 'drawIt',
        validateFields: false,
        lineWidth: 0,
        output: null,
        sigNav: null,
        name: null,
        typed: null,
        clear: '#btnClear',
        typeIt: null,
        drawIt: null,
        typeItDesc: null,
        drawItDesc: null
    });
});

function resizeSignatureAndProceedWithAppendingToPDF() {

    const ctx = $canvas[0].getContext("2d");
    ctx.clearRect(0, 0, $canvas.width, $canvas.height);

    const originalImageFromCanvas = $canvas[0].toDataURL();

    const callbackFn = function (respImage) {
        addSignatureToPDF(respImage);
    }

    resizeCanvasImage(originalImageFromCanvas, 0.28, callbackFn);
}

function addSignatureToPDF(signatureImage) {

    $("#btnAddSignatureToPDF").prop("disabled", true).text("Please Wait...");

    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("x-api-key", $("#txtPDFcoAPIKey").val());

    // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
    var raw = JSON.stringify({
        "url": $("#txtInputPDFUrl").val(),
        "searchString": $("#txtTextToReplaceWith").val(),
        "caseSensitive": false,
        "replaceImage": signatureImage,//"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png",
        "async": false,
        "profiles": "{ 'AutoCropImages': true }"
    });

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    fetch("https://api.pdf.co/v1/pdf/edit/replace-text-with-image", requestOptions)
        .then(response => response.text())
        .then(result => handleSuccessResponse(result))
        .catch(error => {
            $("#btnAddSignatureToPDF").prop("disabled", false).text("Add to PDF");
            console.log('error', error);
        });
}

function handleSuccessResponse(result) {
    $("#btnAddSignatureToPDF").prop("disabled", true).text("Add to PDF");
    var oResult = JSON.parse(result);
    if (oResult.error) {
        $("#divError").show();
        $("#divError").html(oResult.message);
    }
    else {
        $("#aViewResult").show();
        $("#aViewResult").attr("href", oResult.url);
    }
    $("#btnAddSignatureToPDF").prop("disabled", false);
}

function resizeCanvasImage(sourceImage, resizePercentage, callbackFn) {

    // const canvas = document.getElementById("canvas");
    // const ctx = canvas.getContext("2d");
    const img = new Image();
    img.src = sourceImage;

    img.onload = function () {
        const oc = document.createElement('canvas');
        const octx = oc.getContext('2d');
        octx.clearRect(0, 0, oc.width, oc.height);

        // Set the width & height to 75% of image
        oc.width = img.width * resizePercentage;
        oc.height = img.height * resizePercentage;
        // step 2, resize to temporary size
        octx.drawImage(img, 0, 0, oc.width, oc.height);

        // Get canvas image
        const resizedImageFromCanvas = oc.toDataURL();

        callbackFn(resizedImageFromCanvas);
    }
}
