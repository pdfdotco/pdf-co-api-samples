// Visit Knowledgebase for adding Text Macros to PDF 
// https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

// Handle PDF.co Menu on Google Sheet Open Event
function onOpen() {
  var spreadsheet = SpreadsheetApp.getActive();
  var menuItems = [
    {name: 'CreatePDF', functionName: 'fillPDFForm'} 
  ];
  spreadsheet.addMenu('PDF.co', menuItems);
}
  
/**
 * A function that adds headers and some initial data to the spreadsheet.
 */
function fillPDFForm() {
  var spreadsheet = SpreadsheetApp.getActive();

  // Get PDF.co API Key Cell
  let pdfCoAPIKeyCell = spreadsheet.getRange("B1");

  // Get Cells for Input/Output
  let pdfUrlCell = spreadsheet.getRange("A4");
  let familyNameCell = spreadsheet.getRange("A6");
  let givenNameCell = spreadsheet.getRange("B6");
  let middleNameCell = spreadsheet.getRange("C6");
  let resultUrlCell =  spreadsheet.getRange("C4");

  let pdfCoAPIKey = pdfCoAPIKeyCell.getValue();
  let pdfUrl = pdfUrlCell.getValue();
  let familyName = familyNameCell.getValue();
  let givenName = givenNameCell.getValue();
  let middleName = middleNameCell.getValue();
  
  // Prepare Payload
  var data = {
    "async": false,
    "encrypt": false,
    "inline": true,
    "name": "sample_result",
    "url": pdfUrl,
    "fieldsString": `0;form1[0].#subform[0].Line1_FamilyName[0];${familyName}|0;form1[0].#subform[0].Line1_GivenName[0];${givenName}|0;form1[0].#subform[0].Line1_MiddleName[0];${middleName}`
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
  var pdfCoResponse = UrlFetchApp.fetch('https://api.pdf.co/v1/pdf/edit/add', options);

  var pdfCoRespContent = pdfCoResponse.getContentText();
  var pdfCoRespJson = JSON.parse(pdfCoRespContent);

  // Display Result
  if(!pdfCoRespJson.error){
    resultUrlCell.setValue(pdfCoRespJson.url);
  }
  else{
    resultUrlCell.setValue(pdfCoRespJson.message);
  }
}
  