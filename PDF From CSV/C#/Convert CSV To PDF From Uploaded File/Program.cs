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
        const String API_KEY = "***********************************";
        

        // Source CSV file
        const string SourceFile = @".\sample.csv";

        // Destination PDF file
        const string DestinationFile = @".\result.pdf";


        static void Main(string[] args)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);


            // Upload file to the cloud
            string uploadedFileUrl = UploadFile(SourceFile);

            if (string.IsNullOrEmpty(uploadedFileUrl))
            {
                Console.WriteLine("File upload error.");
                return;
            }

            // CONVERT UPLOADED CSV FILE TO PDF

            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/#1-json-pdfconvertfromcsv
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("url", uploadedFileUrl);
            parameters.Add("name", Path.GetFileName(DestinationFile));
            // Convert dictionary of params to JSON
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            try
            {
                // URL of "CSV to PDF" endpoint
                string url = "https://api.pdf.co/v1/pdf/convert/from/csv";

                // Execute POST request with JSON payload
                string response = webClient.UploadString(url, jsonPayload);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL of generated PDF file
                    string resultFileUrl = json["url"].ToString();

                    // Download PDF file
                    webClient.DownloadFile(resultFileUrl, DestinationFile);

                    Console.WriteLine("Generated PDF file saved as \"{0}\" file.", DestinationFile);
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
            finally
            {
                webClient.Dispose();
            }


            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        /// <summary>
        /// Uploads file to the cloud and return URL of uploaded file to use in further API calls.
        /// </summary>
        /// <param name="file">Source file name (path).</param>
        /// <returns>URL of uploaded file</returns>
        static string UploadFile(string file)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            try
            {
                // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
                // * If you already have a direct file URL, skip to the step 3.

                // Prepare URL for `Get Presigned URL` API call
                string query = Uri.EscapeUriString(string.Format(
                    "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                    Path.GetFileName(file)));

                // Execute request
                string response = webClient.DownloadString(query);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL to use for the file upload
                    string uploadUrl = json["presignedUrl"].ToString();
                    // Get URL of uploaded file to use with later API calls
                    string uploadedFileUrl = json["url"].ToString();

                    // 2. UPLOAD THE FILE TO CLOUD.

                    webClient.Headers.Add("content-type", "application/octet-stream");
                    webClient.UploadFile(uploadUrl, "PUT", file); // You can use UploadData() instead if your file is in byte[] or Stream

                    return uploadedFileUrl;
                }
                else
                {
                    // Display service reported error
                    Console.WriteLine(json["message"].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                webClient.Dispose();
            }

            return null;
        }
    }
}
