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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace PDFcoCodeSample
{
    class Program
    {
        const String API_KEY = "***********************************";
        const string DestinationFile = @".\newDocument.pdf";
        const bool Async = true; // Enable asynchronous processing


        static async Task Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-api-key", API_KEY);


                string url = "https://api.pdf.co/v1/pdf/convert/from/html";


                // Prepare requests params as JSON
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "templateId", 1 },
                    { "name", Path.GetFileName(DestinationFile) },
                    { "margins", "40px 20px 20px 20px" },
                    { "paperSize", "Letter" },
                    { "orientation", "Portrait" },
                    { "header", "" },
                    { "printBackground", true },
                    { "footer", "" },
                    { "async", Async }, // Enable asynchronous processing
                    { "encrypt", false },
                    { "templateData", "{\"paid\": true,\"invoice_id\": \"0021\",\"invoice_date\": \"August 29, 2041\",\"invoice_dateDue\": \"September 29, 2041\",\"issuer_name\": \"Sarah Connor\",\"issuer_company\": \"T-800 Research Lab\",\"issuer_address\": \"435 South La Fayette Park Place, Los Angeles, CA 90057\",\"issuer_website\": \"www.example.com\",\"issuer_email\": \"info@example.com\",\"client_name\": \"Cyberdyne Systems\",\"client_company\": \"Cyberdyne Systems\",\"client_address\": \"18144 El Camino Real, Sunnyvale, California\",\"client_email\": \"sales@example.com\",\"items\": [    {    \"name\": \"T-800 Prototype Research\", \"price\": 1000.00    },   {     \"name\": \"T-800 Cloud Sync Setup\", \"price\": 300.00     }  ],\"discount\": 100,\"tax\": 87,\"total\": 1287,\"note\": \"Thank you for your support of advanced robotics.\"}" }
                };


                // Convert dictionary of params to JSON
                string jsonPayload = JsonConvert.SerializeObject(parameters);


                try
                {
                    // Send POST request asynchronously
                    var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);


                    // Read response asynchronously
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);


                    if (json["error"].ToObject<bool>() == false)
                    {
                        // Get Job ID for asynchronous processing
                        string jobId = json["jobId"].ToString();
                        Console.WriteLine($"Job ID: {jobId}");


                        // Check job status in a loop
                        bool isJobCompleted = false;
                        while (!isJobCompleted)
                        {
                            string status = await CheckJobStatusAsync(httpClient, jobId);
                            Console.WriteLine($"Job Status: {status}");


                            if (status == "success")
                            {
                                // Get URL of generated PDF file
                                string resultFileUrl = json["url"].ToString();


                                // Download PDF file asynchronously
                                byte[] fileBytes = await httpClient.GetByteArrayAsync(resultFileUrl);
                                await File.WriteAllBytesAsync(DestinationFile, fileBytes);


                                Console.WriteLine("Generated PDF file saved as \"{0}\" file.", DestinationFile);
                                isJobCompleted = true;
                            }
                            else if (status == "working")
                            {
                                // Wait for a few seconds before checking again
                                await Task.Delay(3000);
                            }
                            else
                            {
                                Console.WriteLine($"Job failed or aborted. Status: {status}");
                                isJobCompleted = true;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(json["message"].ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }


            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }


        static async Task<string> CheckJobStatusAsync(HttpClient httpClient, string jobId)
        {
            string url = $"https://api.pdf.co/v1/job/check?jobid={jobId}";


            // Send GET request to check job status
            HttpResponseMessage response = await httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);


            return json["status"].ToString();
        }
    }
}
