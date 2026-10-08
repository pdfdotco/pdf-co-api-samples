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

namespace ByteScoutWebApiExample
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "***********************************";
        

        // Source PDF file
        const string SourceFile = @".\sample.pdf";
        
        // Destination PDF file name
        const string DestinationFile = @".\protected.pdf";

        // Passwords to protect PDF document
        // The owner password will be required for document modification.
        // The user password only allows to view and print the document.
        const string OwnerPassword = "123456";
        const string UserPassword = "654321";

        // Encryption algorithm. 
        // Valid values: "RC4_40bit", "RC4_128bit", "AES_128bit", "AES_256bit".
        const string EncryptionAlgorithm = "AES_128bit";

        // Allow or prohibit content extraction for accessibility needs.
        const bool AllowAccessibilitySupport = true;

        // Allow or prohibit assembling the document.
        const bool AllowAssemblyDocument = true;

        // Allow or prohibit printing PDF document.
        const bool AllowPrintDocument = true;

        // Allow or prohibit filling of interactive form fields (including signature fields) in PDF document.
        const bool AllowFillForms = true;

        // Allow or prohibit modification of PDF document.
        const bool AllowModifyDocument = true;

        // Allow or prohibit copying content from PDF document.
        const bool AllowContentExtraction = true;

        // Allow or prohibit interacting with text annotations and forms in PDF document.
        const bool AllowModifyAnnotations = true;

        // Allowed printing quality.
        // Valid values: "HighResolution", "LowResolution"
        const string PrintQuality = "HighResolution";

        
        static void Main(string[] args)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);


            // Upload file to the cloud
            string uploadedFileUrl = UploadFile(SourceFile);


            // PROTECT UPLOADED PDF DOCUMENT

            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/?#pdf-security
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name", Path.GetFileName(DestinationFile));
            parameters.Add("url", uploadedFileUrl);
            parameters.Add("ownerPassword", OwnerPassword);
            parameters.Add("userPassword", UserPassword);
            parameters.Add("encryptionAlgorithm", EncryptionAlgorithm);
            parameters.Add("allowAccessibilitySupport", AllowAccessibilitySupport);
            parameters.Add("allowAssemblyDocument", AllowAssemblyDocument);
            parameters.Add("allowPrintDocument", AllowPrintDocument);
            parameters.Add("allowFillForms", AllowFillForms);
            parameters.Add("allowModifyDocument", AllowModifyDocument);
            parameters.Add("allowContentExtraction", AllowContentExtraction);
            parameters.Add("allowModifyAnnotations", AllowModifyAnnotations);
            parameters.Add("printQuality", PrintQuality);

            // Convert dictionary of params to JSON
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            try
            {
                // URL of "PDF Security" endpoint
                string url = "https://api.pdf.co/v1/pdf/security/add";

                // Execute POST request with JSON payload
                string response = webClient.UploadString(url, jsonPayload);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL of generated PDF file
                    string resultFileUrl = json["url"].ToString();

                    // Download generated PDF file
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

            webClient.Dispose();


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
