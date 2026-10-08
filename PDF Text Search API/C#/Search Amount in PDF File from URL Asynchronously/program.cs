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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ByteScoutWebApiExample
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "***********************************";


        // Direct URL of source PDF file.
        const string SourceFileUrl = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-split/multiple-invoices.pdf";


        // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
        const string Pages = "";


        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";


        // Search string.
        const string SearchString = @"(\p{Sc}) *([0-9.]*)"; // Regular expression to find numbers like '100.00'
                                             // Note: do not use `+` char in regex, but use `{1,}` instead.
                                             // `+` char is valid for URL and will not be escaped, and it will become a space char on the server side.


        // Enable regular expressions (Regex)
        const bool RegexSearch = true;


        // (!) Make asynchronous job
        const bool Async = true;


        static async Task Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Set API Key
                httpClient.DefaultRequestHeaders.Add("x-api-key", API_KEY);


                // Prepare URL for PDF text search API call.
                string url = "https://api.pdf.co/v1/pdf/find";


                // Prepare requests params as JSON
                var parameters = new Dictionary<string, object>
                {
                    { "password", Password },
                    { "pages", Pages },
                    { "url", SourceFileUrl },
                    { "searchString", SearchString },
                    { "regexSearch", RegexSearch },
                    { "async", Async }
                };


                // Convert dictionary of params to JSON
                string jsonPayload = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


                try
                {
                    // Execute POST request with JSON payload
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();


                    // Parse JSON response
                    JObject json = JObject.Parse(responseString);


                    if (json["error"]?.ToObject<bool>() == false)
                    {
                        // Asynchronous job ID
                        string jobId = json["jobId"]?.ToString();


                        // URL of generated json file that will available after the job completion
                        string resultFileUrl = json["url"]?.ToString();


                        if (string.IsNullOrEmpty(jobId) || string.IsNullOrEmpty(resultFileUrl))
                        {
                            Console.WriteLine("Job ID or Result File URL is missing.");
                            return;
                        }


                        // Check the job status in a loop.
                        do
                        {
                            string status = await CheckJobStatus(httpClient, jobId); // Possible statuses: "working", "failed", "aborted", "success".


                            // Display timestamp and status (for demo purposes)
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status);


                            if (status == "success")
                            {
                                // Execute request
                                string respFileJson = await httpClient.GetStringAsync(resultFileUrl);


                                // Parse JSON response
                                JArray jsonFoundInformation = JArray.Parse(respFileJson);


                                // Display found information in console
                                foreach (JToken item in jsonFoundInformation)
                                {
                                    Console.WriteLine($"Found text \"{item["text"]}\" at coordinates {item["left"]}, {item["top"]}");
                                }


                                break;
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
                        Console.WriteLine(json["message"]?.ToString());
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


        static async Task<string> CheckJobStatus(HttpClient httpClient, string jobId)
        {
            string url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;


            string response = await httpClient.GetStringAsync(url);
            JObject json = JObject.Parse(response);


            return json["status"]?.ToString() ?? "unknown";
        }
    }
}
