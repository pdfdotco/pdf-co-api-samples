//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


/**
 * IMPORTANT: Add Service reference for "Drive". Go to Services > Locate "Drive (drive API)" > Add Reference of it 
 */

// Add Your PDF.co API Key here
const pdfCoAPIKey = 'ADD_YOUR_PDFco_API_KEY_HERE';

// Get the active spreadsheet and the active sheet
ss = SpreadsheetApp.getActiveSpreadsheet();
ssid = ss.getId();

// Look in the same folder the sheet exists in. For example, if this template is in
// My Drive, it will return all of the files in My Drive.
var ssparents = DriveApp.getFileById(ssid).getParents();

// Store File-Ids/PermissionIds used for merging
let filePermissions = [];

/**
 * Note: Here, we're getting current folder where spreadsheet is residing.
 * But we can certainly pick any folder of our like by using Folder related functions.
 * For example:
  var allFolders = DriveApp.getFoldersByName("Folder_Containing_PDF_Files");
  while (allFolders.hasNext()) {
    var folder = allFolders.next();
    Logger.log(folder.getName());
  }
 */
// Loop through all the files and add the values to the spreadsheet.
var folder = ssparents.next();

/**
 * Add PDF.co Menus in Google Spreadsheet
 */
function onOpen() {
  var menuItems = [
    {name: 'Read QRCode from All PDF Files Of Current Folder', functionName: 'readQRCodeFromCurrentFolderPDFs'} 
  ];
  ss.addMenu('PDF.co', menuItems);
}

function readQRCodeFromCurrentFolderPDFs(){
  var allFileNameUrls = getPDFFilesFromCurFolder();
  var allResp = [];

  if(allFileNameUrls && allFileNameUrls.length > 0){
    for(let i = 0; i < allFileNameUrls.length; i++){
      const elmCurFileNameUrl = allFileNameUrls[i];

      var oResp = readQRCodeFromPDF(elmCurFileNameUrl.url, elmCurFileNameUrl.fileName);
      allResp.push(oResp);
    }
  }

  //console.log(JSON.stringify(allResp));

  // Write all resp
  const outputIndex = 2;
  for(let i = 0; i < allResp.length; i++){
    ss.getRange(`A${outputIndex + i}`).setValue(allResp[i].file);
    if(allResp[i].data){
      ss.getRange(`B${outputIndex + i}`).setValue(JSON.stringify(allResp[i].data));
    }
  }
}

/**
 * Get all PDF files from current folder
 */
function getPDFFilesFromCurFolder() {
  var files = folder.getFiles();
  var allFileNameUrls = [];

  while (files.hasNext()) {
    var file = files.next();

    var fileName = file.getName();
    if(fileName.endsWith(".pdf")){
      // Create Pre-Signed URL from PDF.co
      var respPresignedUrl = getPDFcoPreSignedURL(fileName)

      if(!respPresignedUrl.error){
        var fileData = file.getBlob();
        if(uploadFileToPresignedURL(respPresignedUrl.presignedUrl, fileData)){
          // Add Url
          allFileNameUrls.push({url: respPresignedUrl.url, fileName: fileName});
        }
      }
    }
  }

  return allFileNameUrls;
}

/**
 * Reads QRCode from PDF URL using PDF.co
 */
function readQRCodeFromPDF(pdfUrl, pdfFileName) {
  // Prepare Payload
  var data = {
    "url": pdfUrl,
    "types": 'QRCode',
    "async": false
  };

  // Prepare Request Options
  var options = {
    'method' : 'post',
    'contentType': 'application/json',
    'headers': {
      "x-api-key": pdfCoAPIKey
    },
    // Convert the JavaScript object to a JSON string.
    'payload' : JSON.stringify(data)
  };
  
  // Get Response
  // https://developers.google.com/apps-script/reference/url-fetch
  var pdfCoResponse = UrlFetchApp.fetch('https://api.pdf.co/v1/barcode/read/from/url', options);

  var pdfCoRespContent = pdfCoResponse.getContentText();
  var pdfCoRespJson = JSON.parse(pdfCoRespContent);

  // Display Result
  if(!pdfCoRespJson.error && pdfCoRespJson.barcodes && pdfCoRespJson.barcodes.length > 0){
    return {file: pdfFileName, data: pdfCoRespJson.barcodes};
  }
  else{
    return {file: pdfFileName, data: null};
  }
}

/**
 * Gets PDF.co Presigned URL
 */
function getPDFcoPreSignedURL(fileName){
  // Prepare Request Options
  var options = {
    'method' : 'GET',
    'contentType': 'application/json',
    'headers': {
      "x-api-key": pdfCoAPIKey
    }
  };

  var apiUrl = `https://api.pdf.co/v1/file/upload/get-presigned-url?name=${fileName}`;
  
  // Get Response
  // https://developers.google.com/apps-script/reference/url-fetch
  var pdfCoResponse = UrlFetchApp.fetch(apiUrl, options);

  var pdfCoRespContent = pdfCoResponse.getContentText();
  var pdfCoRespJson = JSON.parse(pdfCoRespContent);

  return pdfCoRespJson;
}

/**
 * Uploads File to PDF.co PreSigned URL
 */
function uploadFileToPresignedURL(presignedUrl, fileContent){
  // Prepare Request Options
  var options = {
    'method' : 'PUT',
    'contentType': 'application/octet-stream',
    'headers': {
      "x-api-key": pdfCoAPIKey
    },
    // Convert the JavaScript object to a JSON string.
    'payload' : fileContent
  };
  
  // Get Response
  // https://developers.google.com/apps-script/reference/url-fetch
  var pdfCoResponse = UrlFetchApp.fetch(presignedUrl, options);

  if(pdfCoResponse.getResponseCode() === 200){
    return true;
  }
  else{
    return false;
  }
}
