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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ByteScoutWebApiExample
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "******************************";

        static void Main(string[] args)
        {
            // HTML input
            string inputSample = File.ReadAllText(@".\sample.html");

            // Destination PDF file name
            string destinationFile = @".\result.pdf";

            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);
            
            // Set JSON content type
            webClient.Headers.Add("Content-Type", "application/json");

            try
            {
                // Prepare requests params as JSON
                // See documentation: https://apidocs.pdf.co/?#1-json-pdfconvertfromhtml
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                // Input HTML code to be converted. Required. 
                parameters.Add("html", inputSample);

                // Name of resulting file
                parameters.Add("name", Path.GetFileName(destinationFile));

                // Set to css style margins like 10 px or 5px 5px 5px 5px.
                parameters.Add("margins", "5px 5px 5px 5px");

                // Can be Letter, A4, A5, A6 or custom size like 200x200
                parameters.Add("paperSize", "Letter");

                // Set to Portrait or Landscape. Portrait by default.
                parameters.Add("orientation", "Portrait");

                // true by default. Set to false to disbale printing of background.
                parameters.Add("printBackground", true);

                // If large input document, process in async mode by passing true
                parameters.Add("async", false);

                // Set to HTML for header to be applied on every page at the header.
                parameters.Add("header", "");

                // Set to HTML for footer to be applied on every page at the bottom.
                parameters.Add("footer", "");


                // Convert dictionary of params to JSON
                string jsonPayload = JsonConvert.SerializeObject(parameters);


                // Prepare URL for `HTML to PDF` API call
                string url = "https://api.pdf.co/v1/pdf/convert/from/html";

                // Execute POST request with JSON payload
                string response = webClient.UploadString(url, jsonPayload);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL of generated PDF file
                    string resultFileUrl = json["url"].ToString();

                    webClient.Headers.Remove("Content-Type"); // remove the header required for only the previous request

                    // Download the PDF file
                    webClient.DownloadFile(resultFileUrl, destinationFile);

                    Console.WriteLine("Generated PDF document saved as \"{0}\" file.", destinationFile);
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
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
