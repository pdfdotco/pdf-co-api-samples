//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HL7CreationFromJson
{
    class JsonParserHelper
    {
        /// <summary>
        /// Parse Json Fileds in class format
        /// </summary>
        public static JsonHL7Fields ParseJsonHL7Fields(string jsonData)
        {
            // Get Object data from input file
            JObject jsonObj = JObject.Parse(jsonData);

            var dKeyVal = new Dictionary<string, string>();
            foreach (JObject prop in jsonObj["objects"])
            {
                dKeyVal.Add(Convert.ToString(prop["name"]), Convert.ToString(prop["value"]));
            }


            var oRet = new JsonHL7Fields();

            oRet.LabName = dKeyVal["LabName"];

            oRet.PatientLastName = dKeyVal["PatientLastName"];
            oRet.PatientFirstName = dKeyVal["PatientFirstName"];
            oRet.PatientSSN = dKeyVal["PatientSSN"];
            oRet.PatientDOB = dKeyVal["PatientDOB"];
            oRet.PatientPhoneHome = dKeyVal["PatientHomePhone"];
            oRet.PatientPhoneWork = dKeyVal["PatientWorkPhone"];
            oRet.PatientAddress = dKeyVal["PatientAddress"];
            oRet.PatientChartNo = dKeyVal["PatientChartNo"];

            string patGenderMaleSelectedVal = dKeyVal["PatientGenderMale"];
            string patGenderFemaleSelectedVal = dKeyVal["PatientGenderFemale"];

            if (!string.IsNullOrEmpty(patGenderMaleSelectedVal))
            {
                oRet.PatientGender = "M";
            }
            else if (!string.IsNullOrEmpty(patGenderFemaleSelectedVal))
            {
                oRet.PatientGender = "F";
            }

            oRet.PhysicianName = dKeyVal["PhysicianName"];
            oRet.PhysicianAccountNo = dKeyVal["PhysicianAccountName"];
            oRet.PhysicianNpi = dKeyVal["PhysicianNPI"];

            oRet.InsuranceName = dKeyVal["InsuranceName"];
            oRet.InsurancePolicy = dKeyVal["InsurancePolicy"];
            oRet.InsuranceGroup = dKeyVal["InsuranceGroup"];
            oRet.InsuredName = dKeyVal["InsuredName"];
            oRet.InsuredSSN = dKeyVal["InsuredSSN"];
            oRet.InsuredDob = dKeyVal["InsuredDOB"];

            string relToPatIsSelf = dKeyVal["RelationToPatientIsSelf"];
            string relToPatIsSpouse = dKeyVal["RelationToPatientIsSpouse"];
            string relToPatIsDependent = dKeyVal["RelationToPatientIsDependent"];

            if (!string.IsNullOrEmpty(relToPatIsSelf))
            {
                oRet.RelationToPatient = "Self";
            }
            else if (!string.IsNullOrEmpty(relToPatIsSpouse))
            {
                oRet.RelationToPatient = "Spouse";
            }
            else if (!string.IsNullOrEmpty(relToPatIsDependent))
            {
                oRet.RelationToPatient = "Dependent";
            }

            // Add Collection Date/Time
            string colDate = dKeyVal["CollectionDate"];
            string colTime = dKeyVal["CollectionTime"];
            string colTimeIsAm = dKeyVal["CollectionTimeIsAM"];
            string colTimeIsPm = dKeyVal["CollectionTimeIsPM"];

            string colTimeAmPm = "";
            if (!string.IsNullOrEmpty(colTimeIsAm))
            {
                colTimeAmPm = "AM";
            }
            else if (!string.IsNullOrEmpty(colTimeIsPm))
            {
                colTimeAmPm = "PM";
            }

            oRet.CollectionDateTime = $"{colDate} {colTime} {colTimeAmPm}";


            // Add ICD Codes
            string IcdCodes = dKeyVal["ICD10DxCodes"];
            if (!string.IsNullOrEmpty(IcdCodes))
            {
                var arrIcdCodes = IcdCodes.Split(',');

                foreach (var itmIcd in arrIcdCodes)
                {
                    oRet.Icd10Codes.Add(itmIcd.Trim());
                }
            }

            // Add Question/Answers
            string Ques_ClinicalHistoryIsRoutinePap = string.IsNullOrEmpty(dKeyVal["ClinicalHistoryIsRoutinePap"]) ? "No" : "Yes";
            string Ques_ClinicalHistoryIsAbnormalBleeding = string.IsNullOrEmpty(dKeyVal["ClinicalHistoryIsAbnormalBleeding"]) ? "No" : "Yes";

            oRet.QuestionAnswer.Add(new KeyValuePair<string, string>("Is Routine PAP?", Ques_ClinicalHistoryIsRoutinePap));
            oRet.QuestionAnswer.Add(new KeyValuePair<string, string>("Is Abnormal Bleeding?", Ques_ClinicalHistoryIsAbnormalBleeding));

            return oRet;
        }

    }
}
