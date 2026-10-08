//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Net;

// Visit Knowledgebase for adding Text Macros to PDF 
// https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

namespace ByteScoutWebApiExample
{
    class Program
	{
		// The authentication key (API Key).
		// Get your own by registering at https://app.pdf.co
		const String API_KEY = "*****************************************";

        // Direct URL of source PDF file.
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
        const string SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf";
		// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
		const string Pages = "";
		// PDF document password. Leave empty for unprotected documents.
		const string Password = "";

        // Destination PDF file name
		const string DestinationFile = @".\result.pdf";

        // Text annotation params
        const string Type = "annotation";
        const int X = 400;
        const int Y = 600;
        const string Text = "APPROVED";
        const string FontName = "Times New Roman";
        const float FontSize = 24;
        const string FontColor = "FF0000";


		static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// * Add text annotation *

			// Prepare requests params as JSON
			// See documentation: https://apidocs.pdf.co/04-pdf-add-text-signatures-and-images-to-pdf
            string jsonPayload = $@"{{
    ""name"": ""{Path.GetFileName(DestinationFile)}"",
    ""url"": ""{SourceFileUrl}"",
    ""password"": ""{Password}"",
    ""annotations"": [
        {{
            ""x"": {X},
            ""y"": {Y},
            ""text"": ""{Text}"",
            ""fontname"": ""{FontName}"",
            ""size"": ""{FontSize.ToString(CultureInfo.InvariantCulture)}"",
            ""color"": ""{FontColor}"",
            ""pages"": ""{Pages}""
        }}
    ]
}}"; ;

			try
			{
                // URL of "PDF Edit" endpoint
                string url = "https://api.pdf.co/v1/pdf/edit/add";

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
