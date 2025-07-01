//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


const orderedUrls = [];
const allInputs = $input.all();

// Debug: see what we have
console.log('Total inputs:', allInputs.length);
allInputs.forEach((item, index) => {
  console.log(`Input ${index}:`, Object.keys(item.json));
});

let splitUrls = [];
let deliveryUrl = '';
let projectUrl = '';

// Process each input item
allInputs.forEach(item => {
  if (item.json.body && Array.isArray(item.json.body)) {
    // This has the split URLs
    splitUrls = item.json.body;
    console.log('Found split URLs:', splitUrls.length);
  } else if (item.json.url) {
    if (item.json.url.includes('Delivery_Confirmation')) {
      deliveryUrl = item.json.url;
      console.log('Found delivery URL');
    } else if (item.json.url.includes('Project_Summary')) {
      projectUrl = item.json.url;
      console.log('Found project URL');
    }
  }
});

// Create the ordered sequence
if (splitUrls.length >= 3) {
  orderedUrls.push(splitUrls[0]);     // Invoice Part 1
  orderedUrls.push(projectUrl);       // Project
  orderedUrls.push(splitUrls[1]);     // Invoice Part 2  
  orderedUrls.push(deliveryUrl);      // Delivery
  orderedUrls.push(splitUrls[2]);     // Invoice Part 3
}

console.log('Final ordered URLs:', orderedUrls.length);
return [{ json: { urls: orderedUrls } }];
