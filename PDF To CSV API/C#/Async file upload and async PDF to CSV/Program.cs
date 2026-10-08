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
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ByteScoutWebApiExample
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "**************************************";

        // Source PDF file
        const string SourceFile = @".\sample.pdf";
        // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
        const string Pages = "";
        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";
        // Destination CSV file name
        const string DestinationFile = @".\result.csv";
        // (!) Make asynchronous job
        const bool Async = true;


        static void Main(string[] args)
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

                if (json["status"].ToString() != "error")
                {
                    // Get URL to use for the file upload
                    string uploadUrl = json["presignedUrl"].ToString();
                    string uploadedFileUrl = json["url"].ToString();

                    // 2. UPLOAD THE FILE TO CLOUD.

                    webClient.Headers.Add("content-type", "application/octet-stream");
                    webClient.UploadFile(uploadUrl, "PUT", SourceFile); // You can use UploadData() instead if your file is byte[] or Stream
                    webClient.Headers.Remove("content-type");

                    // 3. CONVERT UPLOADED PDF FILE TO CSV

                    // URL for `PDF To CSV` API call
                    var url = "https://api.pdf.co/v1/pdf/convert/to/csv";

                    // Prepare requests params as JSON
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("name", Path.GetFileName(DestinationFile));
                    parameters.Add("password", Password);
                    parameters.Add("pages", Pages);
                    parameters.Add("url", uploadedFileUrl);
                    parameters.Add("async", Async);

                    // Convert dictionary of params to JSON
                    string jsonPayload = JsonConvert.SerializeObject(parameters);

                    try
                    {
                        // Execute POST request with JSON payload
                        response = webClient.UploadString(url, jsonPayload);

                        // Parse JSON response
                        json = JObject.Parse(response);

                        if (json["status"].ToString() != "error")
                        {
                            // Asynchronous job ID
                            string jobId = json["jobId"].ToString();
                            // URL of generated CSV file that will available after the job completion
                            string resultFileUrl = json["url"].ToString();

                            // Check the job status in a loop. 
                            // If you don't want to pause the main thread you can rework the code 
                            // to use a separate thread for the status checking and completion.
                            do
                            {
                                string status = CheckJobStatus(jobId); // Possible statuses: "working", "failed", "aborted", "success".

                                // Display timestamp and status (for demo purposes)
                                Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status);

                                if (status == "success")
                                {
                                    // Download CSV file
                                    webClient.DownloadFile(resultFileUrl, DestinationFile);

                                    Console.WriteLine("Generated CSV file saved as \"{0}\" file.", DestinationFile);
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


        /// <summary>
        /// Check job status
        /// </summary>
        static string CheckJobStatus(string jobId)
        {
            using (WebClient webClient = new WebClient())
            {
                // Set API Key
                webClient.Headers.Add("x-api-key", API_KEY);

                string url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

                string response = webClient.DownloadString(url);
                JObject json = JObject.Parse(response);

                return Convert.ToString(json["status"]);
            }
        }

    }
}
