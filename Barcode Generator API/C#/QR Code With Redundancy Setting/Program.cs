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
		const String API_KEY = "************************************";
		

		// Result file name
		const string ResultFileName = @".\barcode.png";
		// Barcode type. See valid barcode types in the documentation https://apidocs.pdf.co/#barcode-generator
		const string BarcodeType = "QRCode";
		// Barcode value
		const string BarcodeValue = "QR123456\nhttps://pdf.co\nhttps://bytescout.com";



		static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			/*
			 Valid error correction levels:
			 ----------------------------------
			 Low - [default] Lowest error correction level. (Approx. 7% of codewords can be restored).
			 Medium - Medium error correction level. (Approx. 15% of codewords can be restored).
			 Quarter - Quarter error correction level (Approx. 25% of codewords can be restored).
			 High - Highest error correction level (Approx. 30% of codewords can be restored).
			 */

			// Set "Custom Profiles" parameter
			var Profiles = @"{ ""profiles"": [ { ""profile1"": { ""Options.QRErrorCorrectionLevel"": ""Quarter"" } } ] }";

			// Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/#barcode-generator
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", Path.GetFileName(ResultFileName));
            parameters.Add("type", BarcodeType);
            parameters.Add("value", BarcodeValue);
			parameters.Add("profiles", Profiles);

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
