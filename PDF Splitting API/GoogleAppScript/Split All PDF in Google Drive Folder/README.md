## How to split PDF in GoogleAppScript with PDF.co Web API PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://bytescout.zendesk.com/hc/en-us/requests/new?subject=PDF.co%20Web%20API%20Question)

or just send email to [support@bytescout.com](mailto:support@bytescout.com?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)
[Sign Up For Online Training](https://academy.bytescout.com/)


## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://apidocs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **program.gs:**
    
```
function splitPDFDocuments() {
  
  // Set Google Drive Folder ID
  var googleDriveFolderId='YOUR_FOLDER_ID';
  
  // Set PDF.co API Key
  var pdfCoAPIKey='YOUR_API_KEY';

  // Get Files from Google Drive Folder
  var files = DriveApp.getFolderById(googleDriveFolderId).getFiles()

  while (files.hasNext()) {
    var file = files.next();
    
    file.setSharing(DriveApp.Access.ANYONE_WITH_LINK, DriveApp.Permission.VIEW);
  };

  // Prepare Payload
  var data = {
    "async": false,
    "encrypt": false,
    "inline": true,
    "name": "result",
    "url": file.getDownloadUrl(),
    "pages": "*"
  };

  // Prepare Request Options
  var options = {
    'method': 'post',
    'contentType': 'application/json',
    'headers': {
      "x-api-key": pdfCoAPIKey
    },
	
	// Convert the JavaScript object to a JSON string
    'payload': JSON.stringify(data)
  };

  var pdfCoResponse = UrlFetchApp.fetch('https://api.pdf.co/v1/pdf/split', options);

  var pdfCoRespContent = pdfCoResponse.getContentText();
  var pdfCoRespJson = JSON.parse(pdfCoRespContent);

  var resultUrls = pdfCoRespJson.urls;
  
  // Save Split PDFs in Google Drive Folder
  for(let i=0;i<resultUrls.length;i++) {
    var splitFile = UrlFetchApp.fetch(resultUrls[i]).getBlob();

    DriveApp.getFolderById(googleDriveFolderId).createFile(splitFile);
  }
}
```

<!-- code block end -->