//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


// Simpler version - just extract text from the first item
const item = $input.first();
const pdfData = Array.isArray(item.json) ? item.json[0] : item.json;

let extractedText = "";
const rows = pdfData?.document?.page?.row || [];

rows.forEach(row => {
  if (row.column) {
    row.column.forEach(col => {
      if (col.text && col.text.text) {
        extractedText += col.text.text + " ";
      }
    });
    extractedText += "\n";
  }
});

return [{
  json: {
    extractedText: extractedText.trim()
  }
}];
