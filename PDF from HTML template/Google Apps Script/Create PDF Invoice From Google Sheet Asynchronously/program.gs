/**
 * Google Apps Script: Asynchronous PDF Invoice Generation with PDF.co
 */


// Replace with your PDF.co API Key
const PDFCO_API_KEY = "YOUR_PDFCO_API_KEY";


// Spreadsheet references
const ui = SpreadsheetApp.getUi();
const ss = SpreadsheetApp.getActiveSpreadsheet();


/**
 * Adds a custom menu on spreadsheet open.
 */
function onOpen() {
  ss.addMenu('PDF.co', [
    { name: 'Get PDF Invoice', functionName: 'getPDFInvoice' }
  ]);
}


/**
 * Submits an asynchronous request to PDF.co to generate a PDF invoice from HTML (using a template).
 * Retrieves the final URL if successful and places it in cell H1.
 */
function getPDFInvoice() {
  // Generate the invoice data JSON
  const invoiceDataJson = JSON.stringify(generateInvoiceJson());


  // Prepare the POST payload for PDF.co
  const payload = {
    templateId: 2,
    templateData: invoiceDataJson,
    async: true
  };


  const options = {
    method: 'post',
    contentType: 'application/json',
    headers: {
      "x-api-key": PDFCO_API_KEY
    },
    payload: JSON.stringify(payload)
  };


  // STEP 1) Initiate the async PDF generation job
  const initResponse = UrlFetchApp.fetch('https://api.pdf.co/v1/pdf/convert/from/html', options);
  const initJson = JSON.parse(initResponse.getContentText());


  // Handle immediate errors
  if (initJson.error) {
    ui.alert("Error initiating PDF.co job: " + initJson.message);
    return;
  }


  // Extract jobId for async polling
  const jobId = initJson.jobId;
  if (!jobId) {
    ui.alert("No jobId received for the asynchronous job.");
    return;
  }


  // STEP 2) Poll the job status until success, failure, or time out
  const maxAttempts = 30;     // Up to 30 tries
  const pollIntervalMs = 2000; // 2 seconds
  let attemptCount = 0;
  let jobStatus = "working";  // Possible values: "working", "success", "failed"


  // Endpoint for checking job status
  const jobCheckUrl = "https://api.pdf.co/v1/job/check?jobid=" + jobId;


  while (attemptCount < maxAttempts && jobStatus === "working") {
    Utilities.sleep(pollIntervalMs);
    attemptCount++;


    // Poll job status
    const statusOptions = {
      method: 'post',
      headers: {
        "x-api-key": PDFCO_API_KEY
      }
    };
    const jobCheckResponse = UrlFetchApp.fetch(jobCheckUrl, statusOptions);
    const jobCheckJson = JSON.parse(jobCheckResponse.getContentText());


    // Handle polling errors
    if (jobCheckJson.error) {
      ui.alert("Error checking job status: " + jobCheckJson.message);
      return;
    }


    jobStatus = jobCheckJson.status;


    // If job succeeded, retrieve the result URL
    if (jobStatus === "success") {
      const resultUrl = jobCheckJson.url;
      if (!resultUrl) {
        ui.alert("Job completed but no result URL found.");
        return;
      }


      // Optionally, check for errors retrieving result
      if (jobCheckJson.error) {
        ui.alert("Error retrieving async result: " + jobCheckJson.message);
        return;
      }


      // Write the result URL into cell H1
      ss.getRange("H1").setValue(resultUrl);
      return;


    } else if (jobStatus === "failed") {
      ui.alert("Async job failed: " + jobCheckJson.message);
      return;
    }
  }


  // If the loop ends without success or fail, we timed out
  if (jobStatus !== "success") {
    ui.alert("Job did not complete in time. (Last status: " + jobStatus + ")");
  }
}


/**
 * Returns invoice data in JSON format for PDF generation.
 * Adjust the fields as needed (some sample data is provided).
 */
function generateInvoiceJson() {
  // Example data for an invoice:
  // (You can also dynamically pull this from the spreadsheet if you prefer)
  return {
    "paid": true,
    "invoice_id": "0021",
    "invoice_date": "August 29, 2041",
    "invoice_dateDue": "September 29, 2041",
    "issuer_name": "Sarah Connor",
    "issuer_company": "T-800 Research Lab",
    "issuer_address": "435 South La Fayette Park Place, Los Angeles, CA 90057",
    "issuer_website": "www.example.com",
    "issuer_email": "info@example.com",
    "client_name": "Cyberdyne Systems",
    "client_company": "Cyberdyne Systems",
    "client_address": "18144 El Camino Real, Sunnyvale, California",
    "client_email": "sales@example.com",
    "items": [
      { "name": "T-800 Prototype Research", "price": 1000.0 },
      { "name": "T-800 Cloud Sync Setup", "price": 300.0 }
    ],
    "discount": 0.1,
    "tax": 0.0725,
    "note": "Thank you for your support of advanced robotics."
  };
}


/**
 * Example method to collect items from specific rows in the spreadsheet.
 * Currently unused. If you enable it, integrate within generateInvoiceJson().
 */
function getInvoiceItemsJson() {
  const invoiceItems = [];
  let index = 9; // Starting row for items


  // Check if there's data in columns A:B on each row
  while (ss.getRange(`A${index}:B${index}`).getValue() !== "") {
    invoiceItems.push({
      name: ss.getRange(`A${index}:B${index}`).getValue(),
      price: ss.getRange(`C${index}`).getValue(),
      quantity: ss.getRange(`D${index}`).getValue()
    });
    index++;
  }


  return invoiceItems;
}
