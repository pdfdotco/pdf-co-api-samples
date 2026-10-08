# PDF.co Document Parser templates

Explore the latest set of PDF.co Document Parser templates at
[https://github.com/bytescout/pdf-co-api-samples/tree/master/Document%20Parser%20API/TEMPLATES-SAMPLES](https://github.com/bytescout/pdf-co-api-samples/tree/master/Document%20Parser%20API/TEMPLATES-SAMPLES)

## How to convert PDF invoice to google sheet for document parser API in GoogleAppScript and PDF.co Web API What is PDF.co Web API? It is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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
/**
 * Initial Declaration and References
 */

// Get UI
const ui = SpreadsheetApp.getUi();

// Get the active spreadsheet and the active sheet
const ss = SpreadsheetApp.getActiveSpreadsheet();
const ssid = ss.getId();

/**
 * Add PDF.co Menus in Google Spreadsheet
 */
function onOpen() {
  var menuItems = [
    {name: 'Get Invoice Information', functionName: 'getInvoiceInformation'} 
  ];
  ss.addMenu('PDF.co', menuItems);
}


/**
 * Function which gets Invoice Information using PDF.co
 */
function getInvoiceInformation() {
  
  let invoiceUrlPromptResp = ui.prompt("Please Provide Invoice URL:");
  let invoiceUrl = invoiceUrlPromptResp.getResponseText();
  
  if(invoiceUrlPromptResp.getSelectedButton() == ui.Button.OK && invoiceUrl && invoiceUrl.trim() !== ""){
      // Prepare Payload
      var data = {
        "url": invoiceUrl, //"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/document-parser/sample-invoice.pdf",
        "outputFormat": "JSON",
        "templateId": "1",
        "async": false,
        "encrypt": "false",
        "inline": "true",
        "password": "",
        "profiles": "",
        "storeResult": false
      };

      // Prepare Request Options
      var options = {
        'method' : 'post',
        'contentType': 'application/json',
        'headers': {
          "x-api-key": "--enter-your-pdf-co-api-key-here--"
        },
        // Convert the JavaScript object to a JSON string.
        'payload' : JSON.stringify(data)
      };
      
      // Get Response
      // https://developers.google.com/apps-script/reference/url-fetch
      var pdfCoResponse = UrlFetchApp.fetch('https://api.pdf.co/v1/pdf/documentparser', options);

      var pdfCoRespText = pdfCoResponse.getContentText();
      var pdfCoRespJson = JSON.parse(pdfCoRespText);

      // Display Result
      if(!pdfCoRespJson.error){
        // Upload file to Google Drive
        showInvoiceResult(pdfCoRespJson.body);    
      }
      else{
        resultUrlCell.setValue(pdfCoRespJson.message);    
      }
  }
  else{
    ui.alert("Please Provide Invoice URL");
  }
}

/**
 * Render Invoice Data to Spreadsheet
 */
function showInvoiceResult(invResultBody){
  var cmpName = getObjectValue(invResultBody, "companyName");
  var invName = getObjectValue(invResultBody, "companyName2");
  var invoiceId = getObjectValue(invResultBody, "invoiceId");
  var issuedDate = getObjectValue(invResultBody, "dateIssued");
  var dueDate = getObjectValue(invResultBody, "dateDue");
  var bankAccount = getObjectValue(invResultBody, "bankAccount");
  var total = getObjectValue(invResultBody, "total");
  var subTotal = getObjectValue(invResultBody, "subTotal");
  var tax = getObjectValue(invResultBody, "tax");

  var tableData = getTableData(invResultBody, "table");

  var cellIndex = 1;

  if(cmpName && cmpName !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Company Name");
    ss.getRange(`B${cellIndex}`).setValue(cmpName);
    cellIndex++;
  }

  if(invName && invName !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Invoice Name");
    ss.getRange(`B${cellIndex}`).setValue(invName);
    cellIndex++;
  }

  if(invoiceId && invoiceId !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Invoice #");
    ss.getRange(`B${cellIndex}`).setValue(invoiceId);
    cellIndex++;
  }

  if(issuedDate && issuedDate !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Issued Date");
    ss.getRange(`B${cellIndex}`).setValue(issuedDate);
    cellIndex++;
  }

  if(dueDate && dueDate !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Due Date");
    ss.getRange(`B${cellIndex}`).setValue(dueDate);
    cellIndex++;
  }

  if(bankAccount && bankAccount !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Bank Account");
    ss.getRange(`B${cellIndex}`).setValue(bankAccount);
    cellIndex++;
  }

  if(total && total !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Total");
    ss.getRange(`B${cellIndex}`).setValue(total);
    cellIndex++;
  }

  if(subTotal && subTotal !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Sub Total");
    ss.getRange(`B${cellIndex}`).setValue(subTotal);
    cellIndex++;
  }

  if(tax && tax !== ""){
    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Tax");
    ss.getRange(`B${cellIndex}`).setValue(tax);
    cellIndex++;
  }

  // Render Table
  if(tableData && tableData.length > 0){
    cellIndex++;

    ss.getRange(`A${cellIndex}`).setFontWeight("bold").setValue("Product Name");
    ss.getRange(`B${cellIndex}`).setFontWeight("bold").setValue("Item Price");
    ss.getRange(`C${cellIndex}`).setFontWeight("bold").setValue("Qty");
    ss.getRange(`D${cellIndex}`).setFontWeight("bold").setValue("Total Price");
    cellIndex++;

    for(var iTableData = 0; iTableData < tableData.length; iTableData++){
      ss.getRange(`A${cellIndex}`).setValue(tableData[iTableData].prodName);
      ss.getRange(`B${cellIndex}`).setValue(tableData[iTableData].itmPrice);
      ss.getRange(`C${cellIndex}`).setValue(tableData[iTableData].qty);
      ss.getRange(`D${cellIndex}`).setValue(tableData[iTableData].totalPrice);
      cellIndex++;
    }
  }
}

/**
 * Get Json Object Value
 */
function getObjectValue(jsonBody, fieldName){
  var oRet = "";
  if(jsonBody && jsonBody.objects && jsonBody.objects.length > 0){
    var findObjField = jsonBody.objects.filter(x => x.name === fieldName && x.objectType === "field");
    if(findObjField && findObjField.length > 0){
      oRet = findObjField[0].value;
    }
  }

  return oRet;
}

/**
 * Get Table formatted data from input Json
 */
function getTableData(jsonBody, fieldName){
  var oRet = [];

  if(jsonBody && jsonBody.objects && jsonBody.objects.length > 0){
    var findObjTable = jsonBody.objects.filter(x => x.name === fieldName && x.objectType === "table");
    if(findObjTable && findObjTable.length > 0 && findObjTable[0].rows && findObjTable[0].rows.length > 0){
      var tableRows = findObjTable[0].rows;

      for(var iRow = 0; iRow < tableRows.length; iRow++){
        var qty = tableRows[iRow].column1.value;
        var prodName = tableRows[iRow].column2.value;
        var itmPrice = tableRows[iRow].column3.value;
        var totalPrice = tableRows[iRow].column4.value;

        oRet.push({ qty: qty, prodName: prodName, itmPrice: itmPrice, totalPrice: totalPrice });
      }
    }
  }

  return oRet;
}



```

<!-- code block end -->