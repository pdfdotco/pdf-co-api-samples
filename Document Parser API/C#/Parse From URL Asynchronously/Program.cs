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
using System.Net;
using System.Threading;

// Cloud API asynchronous "Document Parser" job example.
// Allows to avoid timeout errors when processing huge or scanned PDF documents.

namespace ByteScoutWebApiExample
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const string API_KEY = "***********************************";

        // Source PDF file
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
        const string SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/document-parser/MultiPageTable.pdf";

        // Destination TXT file name
        const string DestinationFile = @".\result.json";

        // (!) Make asynchronous job
        const bool Async = true;


        static void Main(string[] args)
        {
            // Template text. Use Document Parser (https://pdf.co/document-parser, https://app.pdf.co/document-parser)
            // to create templates.
            // Read template from file:
            String templateText = File.ReadAllText(@".\MultiPageTable-template1.yml");

            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            try
            {
                // PARSE UPLOADED PDF DOCUMENT

                // URL of `Document Parser` API call
                string url = "https://api.pdf.co/v1/pdf/documentparser";

                Dictionary<string, object> requestBody = new Dictionary<string, object>();
                requestBody.Add("template", templateText);
                requestBody.Add("name", Path.GetFileName(DestinationFile));
                requestBody.Add("url", SourceFileUrl);
                requestBody.Add("async", Async);

                // Convert dictionary of params to JSON
                string jsonPayload = JsonConvert.SerializeObject(requestBody);

                // Execute request
                string response = webClient.UploadString(url, "POST", jsonPayload);

                // Parse response
                JObject json = JObject.Parse(response);

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
                    Console.WriteLine(json["message"].ToString());
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }

            webClient.Dispose();

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static string CheckJobStatus(WebClient webClient, string jobId)
        {
            string url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

            string response = webClient.DownloadString(url);
            JObject json = JObject.Parse(response);

            return Convert.ToString(json["status"]);
        }
    }
}
