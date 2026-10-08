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
		const String API_KEY = "******************";

		static void Main(string[] args)
		{
			// --TemplateID--
			/* 
				Please follow below steps to create your own HTML Template and get "templateId". 
				1. Add new html template in app.pdf.co/templates/html
				2. Copy paste your html template code into this new template. Sample HTML templates can be found at "https://github.com/bytescout/pdf-co-api-samples/tree/master/PDF%20from%20HTML%20template/TEMPLATES-SAMPLES"
				3. Save this new template
				4. Copy it’s ID to clipboard
				5. Now set ID of the template into “templateId” parameter
			*/

			// HTML template using built-in template
			// see https://app.pdf.co/templates/html/2/edit
			var templateId = 478; // Replace with your templateId

			// Data to fill the template
			string templateData = File.ReadAllText(@".\quote_data.json");
			// Destination PDF file name
			string destinationFile = @".\result.pdf";

			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			webClient.Headers.Add("Content-Type", "application/json");

			try
			{
                // URL for `HTML to PDF` API call
				string url = Uri.EscapeUriString(string.Format(
					"https://api.pdf.co/v1/pdf/convert/from/html?name={0}", 
					Path.GetFileName(destinationFile)));

				// Prepare requests params as JSON
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				parameters.Add("name", Path.GetFileName(destinationFile));
				parameters.Add("templateId", templateId);
				parameters.Add("templateData", templateData);

				// Convert dictionary of params to JSON
				string jsonPayload = JsonConvert.SerializeObject(parameters);

                // Execute request
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
