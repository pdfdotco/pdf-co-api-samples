//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


const axios = require("axios");
const path = require("path");
const fs = require("fs");

// The authentication key (API Key).
// Get your own by registering at https://app.pdf.co
const API_KEY = "*************************";

// Source PDF file
const SourceFile = "./sample.pdf";
// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
const Pages = "";
// PDF document password. Leave empty for unprotected documents.
const Password = "";
// OCR language. "eng", "fra", "deu", "spa" supported currently.
const Language = "eng";
// Destination PDF file name
const DestinationFile = "./result.pdf";

// 1. Retrieve presigned URL to upload file.
getPresignedUrl(API_KEY, SourceFile)
  .then(([uploadUrl, uploadedFileUrl]) => {
    // 2. Upload the file to the cloud.
    uploadFile(SourceFile, uploadUrl)
      .then(() => {
        // 3. Make uploaded PDF file searchable.
        makePdfSearchable(API_KEY, uploadedFileUrl, Password, Pages, Language, DestinationFile);
      })
      .catch((e) => {
        console.error("Error uploading file:", e);
      });
  })
  .catch((e) => {
    console.error("Error retrieving presigned URL:", e);
  });

async function getPresignedUrl(apiKey, localFile) {
  try {
    const queryPath = `/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=${path.basename(localFile)}`;
    const response = await axios.get(`https://api.pdf.co${queryPath}`, {
      headers: { "x-api-key": apiKey },
    });

    if (!response.data.error) {
      return [response.data.presignedUrl, response.data.url];
    } else {
      throw new Error(response.data.message);
    }
  } catch (error) {
    console.error("getPresignedUrl error:", error);
    throw error;
  }
}

async function uploadFile(localFile, uploadUrl) {
  try {
    const fileData = fs.readFileSync(localFile);
    await axios.put(uploadUrl, fileData, {
      headers: {
        "Content-Type": "application/octet-stream",
      },
    });
  } catch (error) {
    console.error("uploadFile error:", error);
    throw error;
  }
}

async function makePdfSearchable(apiKey, uploadedFileUrl, password, pages, language, destinationFile) {
  try {
    const queryPath = `/v1/pdf/makesearchable`;
    const payload = {
      name: path.basename(destinationFile),
      password: password,
      pages: pages,
      lang: language,
      url: uploadedFileUrl,
      async: true,
    };

    const response = await axios.post(`https://api.pdf.co${queryPath}`, payload, {
      headers: {
        "x-api-key": apiKey,
        "Content-Type": "application/json",
      },
    });

    if (!response.data.error) {
      console.log(`Job #${response.data.jobId} has been created!`);
      checkIfJobIsCompleted(response.data.jobId, response.data.url, destinationFile);
    } else {
      throw new Error(response.data.message);
    }
  } catch (error) {
    console.error("makePdfSearchable error:", error);
  }
}

async function checkIfJobIsCompleted(jobId, resultFileUrl, destinationFile) {
  try {
    const queryPath = `/v1/job/check`;
    const payload = { jobid: jobId };

    while (true) {
      const response = await axios.post(`https://api.pdf.co${queryPath}`, payload, {
        headers: {
          "x-api-key": API_KEY,
          "Content-Type": "application/json",
        },
      });

      console.log(
        `Checking Job #${jobId}, Status: ${response.data.status}, Time: ${new Date().toLocaleString()}`
      );

      if (response.data.status === "working") {
        await new Promise((resolve) => setTimeout(resolve, 3000));
      } else if (response.data.status === "success") {
        const fileResponse = await axios.get(resultFileUrl, {
          responseType: "stream",
        });

        const writer = fs.createWriteStream(destinationFile);
        fileResponse.data.pipe(writer);

        writer.on("finish", () => {
          console.log(`Generated PDF file saved as "${destinationFile}".`);
        });

        writer.on("error", (err) => {
          console.error("Error saving file:", err);
        });

        break;
      } else {
        console.log(`Operation ended with status: "${response.data.status}".`);
        break;
      }
    }
  } catch (error) {
    console.error("checkIfJobIsCompleted error:", error);
  }
}
