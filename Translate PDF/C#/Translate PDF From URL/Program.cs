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
		
		// Direct URL of source PDF file.
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
		const string SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-to-excel/sample.pdf";

		// Destination PDF file name
		const string DestinationFile = @".\result.pdf";

		// Pages to translate. Default translates the 1st page only. Use 0- for all pages
		const string pagesToTranslate = "0-";

		// From language
		const string langfrom = "en";

		// To language 
		const string langTo = "es";

		static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// URL of `PDF Translate` API call
			string url = "https://api.pdf.co/v1/pdf/translate";

			// Prepare requests params as JSON
			// See documentation: https://apidocs.pdf.co/02-pdf-translate
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("name", Path.GetFileName(DestinationFile));
			parameters.Add("url", SourceFileUrl);
			parameters.Add("pages", pagesToTranslate);
			parameters.Add("langFrom", langfrom);
			parameters.Add("langto", langTo);


			// Convert dictionary of params to JSON
			string jsonPayload = JsonConvert.SerializeObject(parameters);

			try
			{
				// Execute POST request with JSON payload
				string response = webClient.UploadString(url, jsonPayload);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["error"].ToObject<bool>() == false)
				{
					// Get URL of generated output file
					string resultFileUrl = json["url"].ToString();

					// Download output file
					webClient.DownloadFile(resultFileUrl, DestinationFile);

					Console.WriteLine("Translated PDF file saved as \"{0}\" file.", DestinationFile);
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
