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
using System.Net;

namespace ByteScoutWebApiExample
{
	class Program
	{
		// The authentication key (API Key).
		// Get your own by registering at https://app.pdf.co
		const String API_KEY = "********************************";

		// Source PDF file to split
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
		const string SourceFileUrl = @"https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-split/sample.pdf";
		// Comma-separated list of page numbers (or ranges) to process. Example: '1,3-5,7-'.
		const string Pages = "1-2,3-";
		
		static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			try
			{
				// URL for `Split PDF` API call
				string url = "https://api.pdf.co/v1/pdf/split";

				// Prepare requests params as JSON
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("pages", Pages);
				parameters.Add("url", SourceFileUrl);

				// Convert dictionary of params to JSON
				string jsonPayload = JsonConvert.SerializeObject(parameters);

				// Execute POST request with JSON payload
				string response = webClient.UploadString(url, jsonPayload);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["error"].ToObject<bool>() == false)
				{
					// Download generated PDF files
					int part = 1;
					foreach (JToken token in json["urls"])
					{
						string resultFileUrl = token.ToString();
						string localFileName = String.Format(@".\part{0}.pdf", part);

						webClient.DownloadFile(resultFileUrl, localFileName);

						Console.WriteLine("Downloaded \"{0}\".", localFileName);
						part++;
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
