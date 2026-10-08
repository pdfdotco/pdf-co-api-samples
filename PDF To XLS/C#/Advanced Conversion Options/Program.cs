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
		// Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
		const string Pages = "";
		// PDF document password. Leave empty for unprotected documents.
		const string Password = "";
		// Destination XLS file name
		const string DestinationFile = @".\result.xls";

		/*
		Some of advanced options available through profiles:
		(JSON can be single/double-quoted and contain comments.)
		{
		    "profiles": [
		        {
		            "profile1": {
		                "NumberDecimalSeparator": "", // Allows to customize decimal separator in numbers.
		                "NumberGroupSeparator": "", // Allows to customize thousands separator.
		                "AutoDetectNumbers": true, // Whether to detect numbers. Values: true / false
		                "RichTextFormatting": true, // Whether to keep text style and fonts. Values: true / false
		                "PageToWorksheet": true, // Whether to create separate worksheet for each page of PDF document. Values: true / false
		                "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
		                "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
		                "LineGroupingMode": "None", // Values: "None", "GroupByRows", "GroupByColumns", "JoinOrphanedRows"
		                "ColumnDetectionMode": "ContentGroupsAndBorders", // Values: "ContentGroupsAndBorders", "ContentGroups", "Borders", "BorderedTables"
		                "Unwrap": false, // Unwrap grouped text in table cells. Values: true / false
		                "ShrinkMultipleSpaces": false, // Shrink multiple spaces in table cells that affect column detection. Values: true / false
		                "DetectNewColumnBySpacesRatio": 1, // Spacing ratio that affects column detection.
		                "CustomExtractionColumns": [ 0, 50, 150, 200, 250, 300 ], // Explicitly specify columns coordinates for table extraction.
		                "CheckPermissions": true, // Ignore document permissions. Values: true / false
		            }
		        }
		    ]
		}
		*/
        // Sample profile that sets advanced conversion options.
        // Advanced options are properties of XLSExtractor class from ByteScout PDF Extractor SDK used in the back-end:
        // https://cdn.bytescout.com/help/BytescoutPDFExtractorSDK/html/2712c05b-9674-5253-df76-2a31ed055afd.htm
        static string Profiles = File.ReadAllText("profile.json");

        static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// URL for `PDF To XLS` API call
			string url = "https://api.pdf.co/v1/pdf/convert/to/xls";

			// Prepare requests params as JSON
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("name", Path.GetFileName(DestinationFile));
			parameters.Add("password", Password);
			parameters.Add("pages", Pages);
			parameters.Add("url", SourceFileUrl);
			parameters.Add("profiles", Profiles);

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
					// Get URL of generated XLS file
					string resultFileUrl = json["url"].ToString();

					// Download XLS file
					webClient.DownloadFile(resultFileUrl, DestinationFile);

					Console.WriteLine("Generated XLS file saved as \"{0}\" file.", DestinationFile);
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
