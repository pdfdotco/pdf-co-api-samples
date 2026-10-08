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
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

// Cloud API asynchronous "Delete Text from PDF" job example.

namespace ByteScoutWebApiExample
{
	class Program
	{
		// The authentication key (API Key).
		// Get your own by registering at https://app.pdf.co
		const String API_KEY = "YOUR_PDFco_API_KEY_HERE";

		// Source PDF file
		const string SourceFile = @".\sample.pdf";

		// PDF document password. Leave empty for unprotected documents.
		const string Password = "";

		// Regex Enabled Search
		const bool RegexEnabled = true;

		// Destination PDF file name
		const string DestinationFile = @".\result.pdf";


		static void Main(string[] args)
		{
			// Upload file to the cloud
			string uploadedFileUrl = UploadFile(SourceFile);

			if (string.IsNullOrEmpty(uploadedFileUrl))
			{
				Console.WriteLine("File upload error.");
				return;
			}

			// Regular Expression for Credit Card
			string regex_creditCard = @"[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}";

			// Regular expression CVV number
			string regex_cvv = @"\b[0-9]{3}\b";

			// Regular expression mm/year
			string regex_month_year = @"[0-9]{2}(\/|-)[0-9]{4}";

			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// Prepare requests params as JSON
			// See documentation: https://apidocs.pdf.co/#pdf-search-and-delete-text-from-pdf
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("name", Path.GetFileName(DestinationFile));
			parameters.Add("password", Password);
			parameters.Add("url", uploadedFileUrl);
			parameters.Add("searchStrings", new string[] { regex_creditCard, regex_cvv, regex_month_year });
			parameters.Add("regex", RegexEnabled);
			parameters.Add("async", true); // ! Making Async request

			// Convert dictionary of params to JSON
			string jsonPayload = JsonConvert.SerializeObject(parameters);

			// URL of `Delete Text from PDF` API call
			string url = "https://api.pdf.co/v1/pdf/edit/delete-text";

			try
			{
				// Execute POST request with JSON payload
				string response = webClient.UploadString(url, jsonPayload);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["error"].ToObject<bool>() == false)
				{
					// Asynchronous job ID
					string jobId = json["jobId"].ToString();
					// URL of generated PDF file that will available after the job completion
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
							// Download PDF file
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
		/// Uploads file to the cloud and return URL of uploaded file to use in further API calls.
		/// </summary>
		/// <param name="file">Source file name (path).</param>
		/// <returns>URL of uploaded file</returns>
		static string UploadFile(string file)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			try
			{
				// 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
				// * If you already have a direct file URL, skip to the step 3.

				// Prepare URL for `Get Presigned URL` API call
				string query = Uri.EscapeUriString(string.Format(
					"https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
					Path.GetFileName(file)));

				// Execute request
				string response = webClient.DownloadString(query);

				// Parse JSON response
				JObject json = JObject.Parse(response);

				if (json["error"].ToObject<bool>() == false)
				{
					// Get URL to use for the file upload
					string uploadUrl = json["presignedUrl"].ToString();
					// Get URL of uploaded file to use with later API calls
					string uploadedFileUrl = json["url"].ToString();

					// 2. UPLOAD THE FILE TO CLOUD.

					webClient.Headers.Add("content-type", "application/octet-stream");
					webClient.UploadFile(uploadUrl, "PUT", file); // You can use UploadData() instead if your file is in byte[] or Stream

					return uploadedFileUrl;
				}
				else
				{
					// Display service reported error
					Console.WriteLine(json["message"].ToString());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			finally
			{
				webClient.Dispose();
			}

			return null;
		}

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
