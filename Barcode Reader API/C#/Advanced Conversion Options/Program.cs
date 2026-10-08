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
        
        // Direct URL of source file (image or PDF) to search barcodes in.
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
        const string SourceFileURL = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/barcode-reader/sample.pdf";

        // Comma-separated list of barcode types to decode. 
        // See the documentation: https://apidocs.pdf.co/?#barcode-reader
        const string BarcodeTypes = "Code39,Code128,Interleaved2of5,EAN13";

        // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
        const string Pages = "";

        /*
        Some of advanced options available through profiles:
        (JSON can be single/double-quoted and contain comments.)
        {
            "profiles": [
                {
                    "profile1": {
                        "ScanArea": "WholePage", // Values: "TopLeftQuarter", "TopRightQuarter", "BottomRightQuarter", "BottomLeftQuarter", "TopHalf", "BottomHalf", "WholePage".
                        "RequireQuietZones": true, // Whether the quite zone is obligatory for 1D barcodes. Values: true / false
                        "MaxNumberOfBarcodesPerPage": 0, // 0 - unlimited.
                        "MaxNumberOfBarcodesPerDocument": 0, // 0 - unlimited.
                        "ScanStep": 1, // Scan interval for linear (1-dimensional) barcodes.
                        "MinimalDataLength": 0, // Minimal acceptable length of decoded data.
                        "FastMode": true // Faster decoding mode for good quality documents
                    }
                }
            ]
        }
        */

        // Sample profile that sets advanced conversion options
        // Advanced options are properties of Reader class from Bytescout BarCodeReader used in the back-end:
        // https://cdn.bytescout.com/help/BytescoutBarCodeReaderSDK/html/T_Bytescout_BarCodeReader_Reader.htm
        static string Profiles = File.ReadAllText(@".\profile.json");


        static void Main(string[] args)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/?#barcode-reader
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("pages", Pages);
            parameters.Add("url", SourceFileURL);
            parameters.Add("type", BarcodeTypes);
            parameters.Add("profiles", Profiles);
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
                    // Display found barcodes in console
                    foreach (JToken token in json["barcodes"])
                    {
                        Console.WriteLine("Found barcode:");
                        Console.WriteLine("  Type: " + token["TypeName"]);
                        Console.WriteLine("  Value: " + token["Value"]);
                        Console.WriteLine("  Document Page Index: " + token["Page"]);
                        Console.WriteLine("  Rectangle: " + token["Rect"]);
                        Console.WriteLine("  Confidence: " + token["Confidence"]);
                        Console.WriteLine();
                    }
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
    }
}
