//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


// Input: PDF.co barcode reader output
console.log("Full input data:", JSON.stringify($input.item.json, null, 2));

// Check if it's an array or direct object
const inputData = $input.item.json;
let barcodeResults;

if (Array.isArray(inputData)) {
    barcodeResults = inputData;
} else if (inputData.results && Array.isArray(inputData.results)) {
    barcodeResults = inputData.results;
} else {
    barcodeResults = [inputData]; // Wrap single object in array
}

console.log("Barcode results:", JSON.stringify(barcodeResults, null, 2));

// Check if we have results
if (!barcodeResults || barcodeResults.length === 0) {
    return {
        json: {
            error: "No barcode results found",
            rawInput: $input.item.json
        }
    };
}

const bcbpString = barcodeResults[0].Value;

function parseBCBP(bcbp) {
    // Extract each field by position
    const passengerName = bcbp.substring(2, 22).trim();        // SMITH/JOHNATHAN
    const confirmationCode = bcbp.substring(23, 30).trim();    // ABC123
    const originAirport = bcbp.substring(30, 33);              // NYC
    const destinationAirport = bcbp.substring(33, 36);         // CHG
    const airlineCode = bcbp.substring(36, 39).trim();         // SL
    const flightNumber = bcbp.substring(39, 44).trim();        // 4257
    const julianDate = bcbp.substring(44, 47);                 // 074
    const classCode = bcbp.substring(47, 48);                  // Y
    const seatNumber = bcbp.substring(48, 52).trim();          // 14F
    
    // Convert Julian date to readable format
    const currentYear = new Date().getFullYear();
    const julianDay = parseInt(julianDate);
    const flightDate = new Date(currentYear, 0, julianDay);
    
    // Format date as YYYY-MM-DD
    const formattedDate = flightDate.toISOString().split('T')[0];
    
    // Format date as readable (March 15, 2024)
    const readableDate = flightDate.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
    
    // Format passenger name properly
    const nameParts = passengerName.split('/');
    const lastName = nameParts[0] || '';
    const firstName = nameParts[1] || '';
    const formattedName = firstName && lastName ? `${firstName} ${lastName}` : passengerName;
    
    return {
        clientName: formattedName,
        flightNumber: `${airlineCode}${flightNumber}`,
        flightDateFormatted: formattedDate,        // 2024-03-15
        flightDateReadable: readableDate,          // March 15, 2024
        julianDate: julianDate,                    // 074 (if you want original)
        route: `${originAirport}-${destinationAirport}`,
        seat: seatNumber,
        confirmationCode: confirmationCode,
        class: classCode
    };
}

// Parse the data
const parsed = parseBCBP(bcbpString);

// Return for Google Sheets
return {
    json: {
        clientName: parsed.clientName,
        flightNumber: parsed.flightNumber,
        travelDate: parsed.flightDateFormatted,
        route: parsed.route,
        seat: parsed.seat,
        processedDate: new Date().toISOString().split('T')[0],
        confirmationCode: parsed.confirmationCode
    }
};
