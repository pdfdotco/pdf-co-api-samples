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
using System.Threading;

namespace ByteScoutWebApiExample
{
    class Program
	{
		// The authentication key (API Key).
		// Get your own by registering at https://app.pdf.co
		const String API_KEY = "***********************************";

		// Source Email file to convert
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
		const string SourceFileUrl = @"https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/email-to-pdf/sample.eml";

		// Ouput file path
		const string DestinationFile = @"output.pdf";

		// (!) Make asynchronous job
		const bool Async = true;


		static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			try
			{
				// URL for `PDF FROM Email` API call
				var url = "https://api.pdf.co/v1/pdf/convert/from/email";

				// Prepare requests params as JSON
				Dictionary<string, object> parameters = new Dictionary<string, object>();

				// Link to input EML or MSG file to be converted.
				// You can pass link to file from Google Drive, Dropbox or another online file service that can generate shareable links. 
				// You can also use built-in PDF.co cloud storage located at https://app.pdf.co/files or upload your file as temporary file right before making this API call (see Upload and Manage Files section for more details on uploading files via API).
				parameters.Add("url", SourceFileUrl);

				// True by default. 
				// Set to true to automatically embeds all attachments from original input email MSG or EML fileas files into final output PDF. 
				// Set to false if you don’t want to embed attachments so it will convert only the body of input email.
				parameters.Add("embedAttachments", true);

				// true by default. 
				// Converts attachments that are supported by API (doc, docx, html, png, jpg etc) into PDF and merges into output final PDF. 
				// Non-supported file types are added as PDF attachments (Adobe Reader or another viewer maybe required to view PDF attachments).
				// Set to false if you don’t want to convert attachments from original email and want to embed them as original files (as embedded pdf attachments). 
				parameters.Add("convertAttachments", true);

				// Can be Letter, A4, A5, A6 or custom size like 200x200
				parameters.Add("paperSize", "Letter");

				// Name of output PDF
				parameters.Add("name", "email-with-attachments");

				// Set to true to run as async job in background (recommended for heavy documents).
				parameters.Add("async", Async);

				// Convert dictionary of params to JSON
				string jsonPayload = JsonConvert.SerializeObject(parameters);

				// Execute POST request with JSON payload
				string response = webClient.UploadString(url, jsonPayload);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["error"].ToObject<bool>() == false)
				{
					// Asynchronous job ID
					string jobId = json["jobId"].ToString();
					// URL of generated JSON file available after the job completion; it will contain URLs of result PDF files.
					string resultFileUrl = json["url"].ToString();

					// Check the job status in a loop. 
					// If you don't want to pause the main thread you can rework the code 
					// to use a separate thread for the status checking and completion.
					do
					{
						string status = CheckJobStatus(jobId); // Possible statuses: "working", "failed", "aborted", "success".

						// Display timestamp and status (for demo purposes)
						Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status);

						if (status == "success")
						{
							// Download output file
							webClient.DownloadFile(resultFileUrl, DestinationFile);

							Console.WriteLine("Generated PDF file saved as \"{0}\" file.", DestinationFile);
							break;
						}
						else if (status == "working")
						{
							// Pause for a few seconds
							Thread.Sleep(3000);
						}
						else
						{
							Console.WriteLine(status);
							break;
						}
					}
					while (true);
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
		/// Checks Job Status
		/// </summary>
		static string CheckJobStatus(string jobId)
		{
			using (WebClient webClient = new WebClient())
			{
				// Set API Key
                webClient.Headers.Add("x-api-key", API_KEY);
				
				string url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

				string response = webClient.DownloadString(url);
				JObject json = JObject.Parse(response);

				return Convert.ToString(json["status"]);
			}
		}
	}
}
