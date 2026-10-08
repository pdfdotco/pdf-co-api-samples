//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


// Get all attachment keys
const attachmentKeys = Object.keys($binary).filter(key => key.startsWith('attachment_'));

// Create one item per attachment
const items = [];
attachmentKeys.forEach(key => {
  items.push({
    json: {
      ...$json,  // Use $json instead of $$input.item(0).json
      attachment_key: key,
      filename: $binary[key].fileName
    },
    binary: {
      file: $binary[key]  // Rename to 'file' for consistency
    }
  });
});

return items;
