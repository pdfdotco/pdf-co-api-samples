//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


// N8N JavaScript Code - Extract Unique Items and Generate Redaction Configurations
// This processes the PDF.co Document Parser API output and creates redaction configs for each item

// Get the input data from the previous node (document parser output)
const inputData = $input.all();

// Array to store unique item values
let uniqueItems = [];
let seenValues = new Set();

// Process each input item to extract unique values
inputData.forEach(item => {
  // Navigate to the table rows
  const body = item.json.body || item.json[0].body;
  const objects = body.objects;
  
  // Find the table object
  const table = objects.find(obj => obj.objectType === "table");
  
  if (table && table.rows) {
    // Extract item values from each row
    table.rows.forEach(row => {
      if (row.item && row.item.value) {
        const itemValue = row.item.value;
        
        // Only add if we haven't seen this value before
        if (!seenValues.has(itemValue)) {
          seenValues.add(itemValue);
          uniqueItems.push(itemValue);
        }
      }
    });
  }
});

// Generate redaction configurations for each unique item
const redactionConfigs = [];

uniqueItems.forEach(currentItem => {
  // Get all items except the current one (these will be redacted)
  const itemsToRedact = uniqueItems.filter(item => item !== currentItem);
  
  // Create search strings for redaction
  const searchStrings = itemsToRedact.map(item => {
    // Escaped backslashes to produce literal \s+\d+\s+\d+\.\d+\s+\d+\.\d+ in output
    return item + '\\s+\\d+\\s+\\d+\\.\\d+\\s+\\d+\\.\\d+';
  });
  
  // Create simple data for the PDF.co node to use
  const config = {
    // Data for PDF.co node fields - output as proper array
    searchStrings: searchStrings,
    
    // Metadata for workflow management
    visibleItem: currentItem,
    outputFileName: `file_${currentItem.toLowerCase().replace(/\s+/g, '')}.pdf`,
    redactedItems: itemsToRedact
  };
  
  redactionConfigs.push(config);
});

// Return each configuration as a separate n8n output item
return redactionConfigs.map(config => ({
  json: config
}));
