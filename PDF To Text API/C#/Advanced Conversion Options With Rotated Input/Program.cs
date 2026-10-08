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
        const String API_KEY = "*************************************";

        // Source PDF file
        const string SourceFile = @".\sample-rotated.pdf";
        // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
        const string Pages = "";
        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";
        // Destination TEXT file name
        const string DestinationFile = @".\result.txt";

        /*
        Some of advanced options available through profiles:
        (JSON can be single/double-quoted and contain comments.)
        {
            "profiles": [
                {
                    "profile1": {                
                        "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
                        "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
                        "ExtractAnnotations": true, // Whether to extract PDF annotations.
                        "CheckPermissions": true, // Ignore document permissions. Values: true / false
                        "DetectNewColumnBySpacesRatio": 1.2, // A ratio affecting number of spaces between words. 
                    }
                }
            ]
        }
        */

        // Sample profile that sets advanced conversion options
        // Advanced options are properties of CSVExtractor class from ByteScout PDF Extractor SDK used in the back-end:
        // https://cdn.bytescout.com/help/BytescoutPDFExtractorSDK/html/87ce5fa6-3143-167d-abbd-bc7b5e160fe5.htm

        /*
         Valid RotationAngle values:
            0 - no rotation
            1 - 90 degrees
            2 - 180 degrees
            3 - 270 degrees
        */
        static string Profiles = File.ReadAllText("profile.json");

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

                    // 3. CONVERT UPLOADED PDF FILE TO TEXT

                    // URL for `PDF To TEXT` API call
                    var url = "https://api.pdf.co/v1/pdf/convert/to/text";

                    // Prepare requests params as JSON
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("name", Path.GetFileName(DestinationFile));
                    parameters.Add("password", Password);
                    parameters.Add("pages", Pages);
                    parameters.Add("url", uploadedFileUrl);
                    parameters.Add("profiles", Profiles);

                    // Convert dictionary of params to JSON
                    string jsonPayload = JsonConvert.SerializeObject(parameters);

                    // Execute POST request with JSON payload
                    response = webClient.UploadString(url, jsonPayload);

                    // Parse JSON response
                    json = JObject.Parse(response);

                    if (json["status"].ToString() != "error")
                    {
                        // Get URL of generated TEXT file
                        string resultFileUrl = json["url"].ToString();

                        // Download TEXT file
                        webClient.DownloadFile(resultFileUrl, DestinationFile);

                        Console.WriteLine("Generated TEXT file saved as \"{0}\" file.", DestinationFile);
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

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
