//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


const axios = require('axios');
var https = require("https");
var path = require("path");
var fs = require("fs");

// Set your API key
const API_KEY = "****************************";

// Base64 file data
const fileData = 'data:image/gif;base64,R0lGODlhEAAQAPUtACIiIScnJigoJywsLDIyMjMzMzU1NTc3Nzg4ODk5OTs7Ozw8PEJCQlBQUFRUVFVVVVhYWG1tbXt7fInDRYvESYzFSo/HT5LJVJPJVJTKV5XKWJbKWZbLWpfLW5jLXJrMYaLRbaTScKXScKXScafTdIGBgYODg6alprLYhbvekr3elr3el9Dotf///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAAAAAAAIf8LSW1hZ2VNYWdpY2sNZ2FtbWE9MC40NTQ1NQAsAAAAABAAEAAABpJAFGgkKhpFIRHpw2qBLJiLdCrNTFKt0wjD2Xi/G09l1ZIwRJeNZs3uUFQtEwCCVrM1bnhJYHDU73ktJQELBH5pbW+CAQoIhn94ioMKB46HaoGTB5WPaZmMm5wOIRcekqChliIZFXqoqYYkE2SaoZuWH1gmAgsIvr8ICQUPTRIABgTJyskFAw1ZDBAO09TUDw0RQQA7';

// API endpoint for uploading base64 image
const uploadUrl = 'https://api.pdf.co/v1/file/upload/base64';

// Payload for the request
const uploadPayload = {
    file: fileData,
    name: 'uploaded_image.gif', // You can change the name as required
};

// Upload the base64 image
axios.post(uploadUrl, uploadPayload, {
    headers: {
        'x-api-key': API_KEY,
    },
})
.then(uploadResponse => {
    console.log('Image uploaded successfully:', uploadResponse.data);

    // Once the image is uploaded, use the returned URL to convert to PDF
    const imageUrl = uploadResponse.data.url; // Get the URL of the uploaded image

    // Prepare request to convert the uploaded image to PDF
    const destinationFile = './result.pdf';

    const queryPath = '/v1/pdf/convert/from/image';

    const jsonPayload = JSON.stringify({
        name: path.basename(destinationFile),
        url: imageUrl, // Use the uploaded image URL
        async: true,
    });

    const reqOptions = {
        host: 'api.pdf.co',
        method: 'POST',
        path: queryPath,
        headers: {
            'x-api-key': API_KEY,
            'Content-Type': 'application/json',
            'Content-Length': Buffer.byteLength(jsonPayload, 'utf8'),
        },
    };

    // Send request to convert image to PDF
    const postRequest = https.request(reqOptions, (response) => {
        response.on('data', (d) => {
            const data = JSON.parse(d);

            if (data.error === false) {
                console.log(`Job #${data.jobId} has been created!`);
                checkIfJobIsCompleted(data.jobId, data.url);
            } else {
                console.log(data.message); // Error message
            }
        });
    }).on('error', (e) => {
        console.log(e); // Handle request error
    });

    // Write request data
    postRequest.write(jsonPayload);
    postRequest.end();
})
.catch(error => {
    console.error('Error uploading image:', error.response ? error.response.data : error.message);
});

// Function to check if the job has been completed and download the PDF
function checkIfJobIsCompleted(jobId, resultFileUrl) {
    const queryPath = '/v1/job/check';

    const jsonPayload = JSON.stringify({
        jobid: jobId,
    });

    const reqOptions = {
        host: 'api.pdf.co',
        path: queryPath,
        method: 'POST',
        headers: {
            'x-api-key': API_KEY,
            'Content-Type': 'application/json',
            'Content-Length': Buffer.byteLength(jsonPayload, 'utf8'),
        },
    };

    const postRequest = https.request(reqOptions, (response) => {
        response.on('data', (d) => {
            const data = JSON.parse(d);
            console.log(`Checking Job #${jobId}, Status: ${data.status}, Time: ${new Date().toLocaleString()}`);

            if (data.status === 'working') {
                // Check again after 3 seconds
                setTimeout(() => {
                    checkIfJobIsCompleted(jobId, resultFileUrl);
                }, 3000);
            } else if (data.status === 'success') {
                // Download PDF file
                const file = fs.createWriteStream('./result.pdf');
                https.get(resultFileUrl, (response2) => {
                    response2.pipe(file).on('close', () => {
                        console.log('Generated PDF file saved as "result.pdf".');
                    });
                });
            } else {
                console.log(`Operation ended with status: "${data.status}".`);
            }
        });
    });

    // Write request data
    postRequest.write(jsonPayload);
    postRequest.end();
}
