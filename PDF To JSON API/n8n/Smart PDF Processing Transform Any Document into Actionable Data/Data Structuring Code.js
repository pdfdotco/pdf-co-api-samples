//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


// Parse the OpenAI JSON response - it's a single object, not an array
const openaiResponse = $input.first().json;
const content = openaiResponse.message.content;  // Remove the [0] since it's not an array

// Extract JSON from the markdown code block
const jsonMatch = content.match(/```json\n(.*?)\n```/s);
const medicalData = JSON.parse(jsonMatch[1]);

// Structure for Google Sheets
return [{
  json: {
    patient_name: medicalData.patient_demographics.name,
    patient_dob: medicalData.patient_demographics.dob,
    patient_id: medicalData.patient_demographics.patient_id,
    age: medicalData.patient_demographics.age,
    sex: medicalData.patient_demographics.sex,
    specimen_type: medicalData.test_details.specimen_type,
    collection_date: medicalData.test_details.collection_date_time,
    ordering_physician: medicalData.test_details.ordering_physician,
    
    // Key lab values
    wbc_value: medicalData.lab_values["Complete Blood Count"]["White Blood Cell (WBC)"].result,
    wbc_flag: medicalData.lab_values["Complete Blood Count"]["White Blood Cell (WBC)"].flag || "Normal",
    
    rbc_value: medicalData.lab_values["Complete Blood Count"]["Red Blood Cell (RBC)"].result,
    rbc_flag: medicalData.lab_values["Complete Blood Count"]["Red Blood Cell (RBC)"].flag || "Normal",
    
    hemoglobin_value: medicalData.lab_values["Complete Blood Count"]["Hemoglobin (HB/Hgb)"].result,
    hemoglobin_flag: medicalData.lab_values["Complete Blood Count"]["Hemoglobin (HB/Hgb)"].flag || "Normal",
    
    hematocrit_value: medicalData.lab_values["Complete Blood Count"]["Hematocrit (HCT)"].result,
    hematocrit_flag: medicalData.lab_values["Complete Blood Count"]["Hematocrit (HCT)"].flag || "Normal",
    
    platelet_value: medicalData.lab_values["Complete Blood Count"]["Platelet count"].result,
    platelet_flag: medicalData.lab_values["Complete Blood Count"]["Platelet count"].flag || "Normal",
    
    // Critical alerts
    has_critical_values: medicalData.critical_values_and_notifications.critical_values ? "YES" : "NO",
    critical_hemoglobin: medicalData.critical_values_and_notifications.critical_values["Hemoglobin (HB/Hgb)"] || "",
    critical_hematocrit: medicalData.critical_values_and_notifications.critical_values["Hematocrit (HCT)"] || "",
    
    // Notifications
    notified_physician: medicalData.critical_values_and_notifications.notifications.physician,
    notification_time: medicalData.critical_values_and_notifications.notifications.time_notified,
    notified_by: medicalData.critical_values_and_notifications.notifications.notified_by,
    
    // Clinical assessment
    clinical_assessment: medicalData.clinical_assessment.assessment,
    
    // Metadata
    processing_timestamp: new Date().toISOString(),
    report_date: new Date().toISOString().split('T')[0]  // Just the date part
  }
}];
