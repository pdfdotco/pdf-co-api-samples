//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


function convertSerialDate(serial) {
  const base = new Date(Date.UTC(1899, 11, 30));
  const ms = serial * 24 * 60 * 60 * 1000;
  return new Date(base.getTime() + ms).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
}

const groupedQuotes = {};

for (const item of items) {
  const row = item.json;
  const quoteId = row["Quote ID"];
  
  if (!groupedQuotes[quoteId]) {
    groupedQuotes[quoteId] = {
      quote_accepted: false,
      quote_expired: false,
      quote_id: quoteId,
      quote_date: convertSerialDate(row["Quote Date"]),
      quote_validUntil: convertSerialDate(row["Valid Until"]),
      sales_rep: row["Sales Rep"],
      client_name: row["Client Name"],
      client_company: row["Client Company"],
      client_address: row["Client Address"],
      client_email: row["Client Email"],
      client_phone: row["Client Phone"],
      discount: parseFloat(row["Discount"]) || 0,
      tax: parseFloat(row["Tax"]) || 0,
      issuer_name: "Elena Rodriguez",
      issuer_company: "FutureTech Solutions Inc.",
      issuer_address: "2847 Innovation Drive, Austin, TX 78746",
      issuer_email: "quotes@futuretech-solutions.com",
      issuer_phone: "+1 (512) 555-0199",
      issuer_website: "www.futuretech-solutions.com",
      terms: "Payment terms: Net 30 days from invoice date.",
      note: "Thank you for considering our proposal.",
      items: []
    };
  }
  
  groupedQuotes[quoteId].items.push({
    name: row["Item Name"],
    description: row["Item Description"],
    quantity: parseInt(row["Quantity"]) || 1,
    price: parseFloat(row["Price"]) || 0
  });
}

// Return just the quote object, not wrapped in another array
const quote = Object.values(groupedQuotes)[0]; // Get the first (and likely only) quote
return [{ json: quote }];
