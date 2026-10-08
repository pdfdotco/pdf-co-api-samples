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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Cloud API asynchronous "Barcode Reader" job example.
// Allows to avoid timeout errors when processing huge or scanned PDF documents.


namespace ByteScoutWebApiExample
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "***********************************";

        // Direct URL of source file (image or PDF) to search barcodes in.
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
        const string SourceFileURL = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/barcode-reader/sample.pdf";
        
        // Comma-separated list of barcode types to search. 
        // See valid barcode types in the documentation https://apidocs.pdf.co
        const string BarcodeTypes = "Code128,Code39,Interleaved2of5,EAN13";
        
        // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
        const string Pages = "";
        
        // (!) Make asynchronous job
        const bool Async = true;


        static void Main(string[] args)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/?#barcode-reader
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("url", SourceFileURL);
            parameters.Add("type", BarcodeTypes);
            parameters.Add("pages", Pages);
            parameters.Add("async", Async.ToString());
            // Convert dictionary of params to JSON
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            try
            {
                // URL of "Barcode Reader" endpoint
                string url = "https://api.pdf.co/v1/barcode/read/from/url";

                // Execute POST request with JSON payload
                string response = webClient.UploadString(url, jsonPayload);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Asynchronous job ID
                    string jobId = json["jobId"].ToString();
                    // URL of generated JSON file with decoded barcodes that will available after the job completion
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
                            // Download JSON results file as string
                            string jsonFileString = webClient.DownloadString(resultFileUrl);

                            JArray jsonFoundBarcodes = JArray.Parse(jsonFileString);
                            
                            // Display found barcodes in console
                            foreach (JToken token in jsonFoundBarcodes)
                            {
                                Console.WriteLine("Found barcode:");
                                Console.WriteLine("  Type: " + token["TypeName"]);
                                Console.WriteLine("  Value: " + token["Value"]);
                                Console.WriteLine("  Document Page Index: " + token["Page"]);
                                Console.WriteLine("  Rectangle: " + token["Rect"]);
                                Console.WriteLine("  Confidence: " + token["Confidence"]);
                                Console.WriteLine();
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
                    // Display service reported error
                    Console.WriteLine(json["message"].ToString());
                }
            }
            catch (WebException e)
            {
                // Display request error
                Console.WriteLine(e.ToString());
            }
            finally
            {
                webClient.Dispose();
            }


            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

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
