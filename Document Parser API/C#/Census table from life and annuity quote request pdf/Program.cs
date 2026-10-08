//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace LifeAndAnnuityQuoteRequest
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "***********************************";

        // Sample document containing life and annuity quote request form
        const string SourceFile = @".\SampleGroupDisabilityForm.pdf";

        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";

        // Destination TXT file name
        const string DestinationFile = @".\result.json";

        // (!) Make asynchronous job
        const bool Async = true;


        static void Main(string[] args)
        {
            // Sample template
            String templateText = File.ReadAllText(@".\SampleGroupDisabilityForm.yml");

            if (ProcessAndGetJsonFields(templateText))
            {
                // Get json strings
                string jsonString = File.ReadAllText(DestinationFile);

                // Parse to Json Fields
                var oRet = ParseJsonFields(jsonString);

                // Display some of data to console
                Console.WriteLine("Parsing Details:\n------------------------");

                Console.WriteLine($"Contact Person: {oRet.ContactPerson}");
                Console.WriteLine($"Business Name: {oRet.BusinessName}");
                Console.WriteLine($"Business Address: {oRet.BusinessAddress}");
                Console.WriteLine($"Business Type: {oRet.BusinessType}");
                Console.WriteLine($"Phone: {oRet.Phone}");
                Console.WriteLine($"Email: {oRet.Email}");

                // Export list of census to csv format
                var csvOutputFile = "result.csv";

                var csvString = ConvertToCsv(oRet.lstCensusTable);
                File.WriteAllText(csvOutputFile, csvString);

                Console.WriteLine($"\n{csvOutputFile} generated successfully!");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        /// <summary>
        /// Process Input File and Get Json Fields
        /// </summary>
        static bool ProcessAndGetJsonFields(string templateText)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
            // * If you already have a direct file URL, skip to the step 3.

            // Prepare URL for `Get Presigned URL` API call
            string query = Uri.EscapeUriString(string.Format(
                "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                Path.GetFileName(SourceFile)));

            try
            {
                // Execute request
                string response = webClient.DownloadString(query);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL to use for the file upload
                    string uploadUrl = json["presignedUrl"].ToString();
                    string uploadedFileUrl = json["url"].ToString();

                    // 2. UPLOAD THE FILE TO CLOUD.

                    webClient.Headers.Add("content-type", "application/octet-stream");
                    webClient.UploadFile(uploadUrl, "PUT", SourceFile); // You can use UploadData() instead if your file is byte[] or Stream
                    webClient.Headers.Remove("content-type");

                    // 3. PARSE UPLOADED PDF DOCUMENT

                    // URL of `Document Parser` API call
                    string url = "https://api.pdf.co/v1/pdf/documentparser";

                    // Prepare requests params as JSON
                    Dictionary<string, object> requestBody = new Dictionary<string, object>();
                    requestBody.Add("template", templateText);
                    requestBody.Add("name", Path.GetFileName(DestinationFile));
                    requestBody.Add("url", uploadedFileUrl);
                    requestBody.Add("async", Async);

                    // Convert dictionary of params to JSON
                    string jsonPayload = JsonConvert.SerializeObject(requestBody);

                    // Execute request
                    response = webClient.UploadString(url, "POST", jsonPayload);

                    // Parse JSON response
                    json = JObject.Parse(response);

                    if (json["error"].ToObject<bool>() == false)
                    {
                        // Asynchronous job ID
                        string jobId = json["jobId"].ToString();
                        // Get URL of generated JSON file
                        string resultFileUrl = json["url"].ToString();

                        // Check the job status in a loop. 
                        // If you don't want to pause the main thread you can rework the code 
                        // to use a separate thread for the status checking and completion.
                        do
                        {
                            string status = CheckJobStatus(webClient, jobId); // Possible statuses: "working", "failed", "aborted", "success".

                            // Display timestamp and status (for demo purposes)
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status);

                            if (status == "success")
                            {
                                // Download JSON file
                                webClient.DownloadFile(resultFileUrl, DestinationFile);

                                Console.WriteLine("Generated JSON file saved as \"{0}\" file.", DestinationFile);

                                return true;
                            }
                            else if (status == "working")
                            {
                                // Pause for a few seconds
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                Console.WriteLine(status);
                                break;
                            }
                        }
                        while (true);
                    }
                    else
                    {
                        Console.WriteLine(json["message"].ToString());
                    }
                }
                else
                {
                    Console.WriteLine(json["message"].ToString());
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }

            webClient.Dispose();

            return false;
        }


        /// <summary>
        /// Check job status
        /// </summary>
        static string CheckJobStatus(WebClient webClient, string jobId)
        {
            string url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

            string response = webClient.DownloadString(url);
            JObject json = JObject.Parse(response);

            return Convert.ToString(json["status"]);
        }


        /// <summary>
        /// Parser Json Fields
        /// </summary>
        static FormVM ParseJsonFields(string jsonInput)
        {
            JObject jsonObj = JObject.Parse(jsonInput);

            var oRet = new FormVM();

            oRet.ContactPerson = jsonObj.TraverseJObject("ContactPerson", "field")?.Value<string>("value") ?? "";
            oRet.BusinessName = jsonObj.TraverseJObject("BusinessName", "field")?.Value<string>("value") ?? "";
            oRet.BusinessAddress = jsonObj.TraverseJObject("BusinessAddress", "field")?.Value<string>("value") ?? "";
            oRet.BusinessType = jsonObj.TraverseJObject("BusinessType", "field")?.Value<string>("value") ?? "";
            oRet.Phone = jsonObj.TraverseJObject("Phone", "field")?.Value<string>("value") ?? "";
            oRet.Email = jsonObj.TraverseJObject("Email", "field")?.Value<string>("value") ?? "";

            var censusTable = jsonObj.TraverseJObject("CencusTable", "table");
            if (censusTable != null)
            {
                var rows = censusTable["rows"];

                if (rows != null && rows.Count() > 1)
                {

                    for (int i = 1; i < rows.Count(); i++)
                    {
                        var oCensus = new CensusTableVm();

                        // Parser individual data
                        oCensus.SrNo = rows.ElementAt(i).ElementAt(0).ElementAt(0).Value<string>("value");
                        oCensus.DOB = rows.ElementAt(i).ElementAt(1).ElementAt(0).Value<string>("value");
                        oCensus.Gender = rows.ElementAt(i).ElementAt(2).ElementAt(0).Value<string>("value");
                        oCensus.Occupation = rows.ElementAt(i).ElementAt(3).ElementAt(0).Value<string>("value");
                        oCensus.Salary = rows.ElementAt(i).ElementAt(4).ElementAt(0).Value<string>("value");
                        oCensus.IsShortTermDisability = rows.ElementAt(i).ElementAt(5).ElementAt(0).Value<string>("value");
                        oCensus.IsLongTernDisability = rows.ElementAt(i).ElementAt(6).ElementAt(0).Value<string>("value");
                        oCensus.LifeInsuCoverAmt = rows.ElementAt(i).ElementAt(7).ElementAt(0).Value<string>("value");

                        oRet.lstCensusTable.Add(oCensus);
                    }
                }
            }

            return oRet;
        }

        /// <summary>
        /// Convert to Csv Format
        /// </summary>
        static string ConvertToCsv(List<CensusTableVm> lstCensusTable)
        {
            var oRet = new StringBuilder(string.Empty);

            // Get Header Row
            oRet.AppendLine(CensusTableVm.getCsvTitleRow());

            // Put Child Rows
            foreach (var item in lstCensusTable)
            {
                oRet.AppendLine(item.ToString());
            }

            return oRet.ToString();
        }

    }

    class FormVM
    {
        public string ContactPerson { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<CensusTableVm> lstCensusTable { get; set; } = new List<CensusTableVm>();
    }

    class CensusTableVm
    {
        public string SrNo { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string Salary { get; set; }
        public string IsShortTermDisability { get; set; }
        public string IsLongTernDisability { get; set; }
        public string LifeInsuCoverAmt { get; set; }

        public override string ToString()
        {
            return $"{SrNo},{DOB},{Gender},{Occupation},{Salary},{IsShortTermDisability},{IsLongTernDisability},{LifeInsuCoverAmt}";
        }

        public static string getCsvTitleRow()
        {
            return $"SrNo,DOB,Gender,Occupation,Salary,IsShortTermDisability,IsLongTermDisability,LifeInsuCoverAmt";
        }
    }

    /// <summary>
    /// Extension method container for JObject
    /// </summary>
    public static class JObjectExtensionMethods
    {
        /// <summary>
        /// Traverse JObject
        /// </summary>
        public static JToken TraverseJObject(this JObject jsonObj, string propertyName, string objectType)
        {
            return jsonObj["objects"].Where(x => x.Value<string>("name") == propertyName && x.Value<string>("objectType") == objectType)
                .FirstOrDefault();
        }
    }


}
