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
using System.IO;
using System.Net;

namespace ByteScoutWebApiExample
{
    class Program
	{
		// The authentication key (API Key).
		// Get your own by registering at https://app.pdf.co
		const String API_KEY = "**************************************";
		
		// Source PDF file
		const string SourceFile = @".\sample.pdf";
		 
		static void Main(string[] args)
		{
			// Upload Input File
			var UploadedFileURL = UploadFileAndGetUrl(SourceFile);

			Console.WriteLine("Uploaded File URL: " + UploadedFileURL);

			Console.WriteLine();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}

		/// <summary>
		/// Upload Input File and get Uploaded URL
		/// </summary>
		static string UploadFileAndGetUrl(string SourceFile)
        {
			var uploadedFileUrl = "";

			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
			// * If you already have a direct file URL, skip to the step 3.

			// Prepare URL for `Get Presigned URL` API call
			string query = Uri.EscapeUriString(string.Format(
				"https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
				Path.GetFileName(SourceFile)));

			try
			{
				// Execute request
				string response = webClient.DownloadString(query);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["status"].ToString() != "error")
				{
					// Get URL to use for the file upload
					string uploadUrl = json["presignedUrl"].ToString();
					uploadedFileUrl = json["url"].ToString();

					// 2. UPLOAD THE FILE TO CLOUD.
					webClient.Headers.Add("content-type", "application/octet-stream");
					webClient.UploadFile(uploadUrl, "PUT", SourceFile); // You can use UploadData() instead if your file is byte[] or Stream
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

			return uploadedFileUrl;
		}
	}
}
