//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


const inputData = $input.all();

// Function to process Document Parser results
function processParserResult(parserResult) {
    const data = Array.isArray(parserResult) ? parserResult[0] : parserResult;
    
    if (!data?.body?.objects) {
        console.log("Invalid parser result structure");
        return { invoiceFields: {}, tableItems: [] };
    }
    
    const invoiceFields = {};
    let tableObject = null;
    
    // Single pass through objects to extract both fields and table
    for (const obj of data.body.objects) {
        if (obj.objectType === "field") {
            invoiceFields[obj.name] = obj.value;
        } else if (obj.objectType === "table" && !tableObject) {
            tableObject = obj;
        }
    }
    
    if (!tableObject?.rows) {
        return { invoiceFields, tableItems: [] };
    }
    
    // Process table rows with optimized column detection
    const tableItems = tableObject.rows.map((row, index) => {
        const item = { itemNumber: index + 1 };
        
        // Process all columns in the row
        for (const [key, columnData] of Object.entries(row)) {
            if (key.startsWith('column') && columnData?.value !== undefined) {
                const value = columnData.value;
                // Smart type conversion: check if it's a valid number
                const numValue = Number(value);
                item[key] = (!isNaN(numValue) && value !== '' && value !== null) ? numValue : value;
            }
        }
        
        return item;
    });
    
    return { invoiceFields, tableItems };
}

// Process all inputs efficiently
const results = inputData.flatMap(item => {
    const { invoiceFields, tableItems } = processParserResult(item.json);
    
    // Return array of results (one per table item)
    return tableItems.map(tableItem => ({
        json: { ...invoiceFields, ...tableItem }
    }));
});

return results;
