//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


var canvas, context, timer;

var constraints = window.constraints = {
    audio: false,
    video: { facingMode: "environment" } // change to "user" for front camera (see https://developer.mozilla.org/en-US/docs/Web/API/MediaTrackConstraints/facingMode )
};

//  (HTML5 based camera only) this portion of code will be used when browser supports navigator.getUserMedia  *********     */
window.addEventListener("DOMContentLoaded", function () {
    canvas = document.getElementById("canvasU"),
        context = canvas.getContext("2d"),
        video = document.getElementById("video"),
        videoObj = { "video": true },
        errBack = function (error) {
            console.log("Video capture error: ", error.code);
        };

    // check if we can use HTML5 based camera (through mediaDevices.getUserMedia() function)
    if (navigator.mediaDevices.getUserMedia) { // Standard browser
        // display HTML5 camera
        document.getElementById("userMedia").style.display = '';
        // adding click event to take photo from webcam
        document.getElementById("snap").addEventListener("click", function () {
            context.drawImage(video, 0, 0, 640, 480);
        });

        navigator.mediaDevices
            .getUserMedia(constraints)
            .then(handleSuccess)
            .catch(handleError);
    }
    // check if we can use HTML5 based camera (through .getUserMedia() function in Webkit based browser)
    else if (navigator.webkitGetUserMedia) { // WebKit-prefixed for Google Chrome
        // display HTML5 camera
        document.getElementById("userMedia").style.display = '';
        // adding click event to take photo from webcam
        document.getElementById("snap").addEventListener("click", function () {
            context.drawImage(video, 0, 0, 640, 480);
        });
        navigator.webkitGetUserMedia(videoObj, function (stream) {
            video.src = window.webkitURL.createObjectURL(stream);
            video.play();
        }, errBack);
    }
    // check if we can use HTML5 based camera (through .getUserMedia() function in Firefox based browser)
    else if (navigator.mozGetUserMedia) { // moz-prefixed for Firefox 
        // display HTML5 camera
        document.getElementById("userMedia").style.display = '';
        // adding click event to take photo from webcam
        document.getElementById("snap").addEventListener("click", function () {
            context.drawImage(video, 0, 0, 640, 480);
        });
        navigator.mozGetUserMedia(videoObj, function (stream) {
            video.mozSrcObject = stream;
            video.play();
        }, errBack);
    }
    
}, false);

function handleSuccess(stream) {
    var video = document.querySelector('video');
    var videoTracks = stream.getVideoTracks();
    console.log('Got stream with constraints:', constraints);
    console.log(`Using video device: ${videoTracks[0].label}`);
    window.stream = stream; // make variable available to browser console
    video.srcObject = stream;
}

function handleError(error) {
    if (error.name === 'ConstraintNotSatisfiedError') {
        var v = constraints.video;
        errorMsg(`The resolution ${v.width.exact}x${v.height.exact} px is not supported by your device.`);
    } else if (error.name === 'PermissionDeniedError') {
        errorMsg('Permissions have not been granted to use your camera and ' +
            'microphone, you need to allow the page access to your devices in ' +
            'order for the demo to work.');
    }
    errorMsg(`getUserMedia error: ${error.name}`, error);
}

function errorMsg(msg, error) {
    var errorElement = document.querySelector('#errorMsg');
    errorElement.innerHTML += `<p>${msg}</p>`;
    if (typeof error !== 'undefined') {
        console.error(error);
    }
}


// (HTML5 based camera only)
// uploads the image to the server 
function UploadToCloud() {

    if($.trim($("#txtPDFcoAPIKey").val()) === ""){
        alert("PDF.co API Key can not be empty!");
        return;
    }

    if($.trim($("#txtAreaBarcodeTypes").val()) === ""){
        alert("PDF.co Barcode Types can not be empty!");
        return;
    }

    $('#report').html("scanning the current frame......");
    context.drawImage(video, 0, 0, 640, 480);
    var img = canvas.toDataURL('image/jpeg', 0.9);//.split(',')[1];

    readBarcodeFromPDF(img);

    timer = setTimeout(UploadToCloud, 3000);  // will capture new image to detect barcode after 3000 mili second
}

// (flash based camera only) stop the capturing 
function stopCall() {
    $('#report').html("STOPPED Scanning.");
    clearTimeout(timer);
}

function readBarcodeFromPDF(img) {

    const PDFcoAPIKey = $("#txtPDFcoAPIKey").val();
    const barcodeTypes = $("#txtAreaBarcodeTypes").val();

    var form = new FormData();
    form.append("file", img);
    form.append("name", `Frame_${new Date().getTime()}.jpg`);

    var settings_base64Upload = {
        "url": "https://api.pdf.co/v1/file/upload/base64",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "x-api-key": PDFcoAPIKey
        },
        "processData": false,
        "mimeType": "multipart/form-data",
        "contentType": false,
        "data": form
    };

    // Upload Base64 to PDF.co Cloud 
    $.ajax(settings_base64Upload).done(function (resp_upload) {

        var json_resp_upload = JSON.parse(resp_upload);

        var oData_barcodeRead = {
            "url": json_resp_upload.url,
            "types": barcodeTypes,
            "encrypt": false,
            "async": false
        };

        //console.log(oData_barcodeRead);

        var settings_barcode_read = {
            "url": "https://api.pdf.co/v1/barcode/read/from/url",
            "method": "POST",
            "timeout": 0,
            "headers": {
                "x-api-key": PDFcoAPIKey,
                "Content-Type": "application/json"
            },
            "data": JSON.stringify(oData_barcodeRead),
        };

        $.ajax(settings_barcode_read).done(function (response_barcode) {

            if(response_barcode.barcodes && response_barcode.barcodes.length > 0){

                for (let i = 0; i < response_barcode.barcodes.length; i++) {
                    const elmBarcode = response_barcode.barcodes[i];

                    let htmlSelect = document.getElementById('OutListBoxHTML5');
                    htmlSelect.value = elmBarcode.Value + "\r\n" + htmlSelect.value;
                }

            }
        });
    });

}
