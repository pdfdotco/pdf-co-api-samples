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
		

		// Result file name
		const string ResultFileName = @".\barcode.png";
		// Barcode type. See valid barcode types in the documentation https://secure.bytescout.com/cloudapi.html#api-Default-barcodeGenerateGet
		const string BarcodeType = "Code128";
		// Barcode value
		const string BarcodeValue = "qweasd123456";


		static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/#barcode-generator
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", Path.GetFileName(ResultFileName));
            parameters.Add("type", BarcodeType);
            parameters.Add("value", BarcodeValue);
            // Convert dictionary of params to JSON
            string jsonPayload = JsonConvert.SerializeObject(parameters);
			
			try
			{
                // URL of "Barcode Generator" endpoint
                string url = "https://api.pdf.co/v1/barcode/generate";

                // Execute POST request with JSON payload
                string response = webClient.UploadString(url, jsonPayload);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["error"].ToObject<bool>() == false)
				{
					// Get URL of generated barcode image file
					string resultFileURI = json["url"].ToString();
					
					// Download generated image file
					webClient.DownloadFile(resultFileURI, ResultFileName);

					Console.WriteLine("Generated barcode saved to \"{0}\" file.", ResultFileName);
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
	}
}
