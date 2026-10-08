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
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ByteScoutWebApiExample
{
	class Program
	{
		// The authentication key (API Key).
		// Get your own by registering at https://app.pdf.co
		const String API_KEY = "***********************************";
		
		// Source PDF files
		static string[] SourceFiles = new string[] { @".\sample1.pdf", @".\sample2.pdf" };
		// Destination PDF file name
		const string DestinationFile = @".\result.pdf";
        // (!) Make asynchronous job
        const bool Async = true;

        static void Main(string[] args)
		{
			// Create standard .NET web client instance
			WebClient webClient = new WebClient();

			// Set API Key
			webClient.Headers.Add("x-api-key", API_KEY);

			// 1. UPLOAD FILES TO CLOUD

			List<string> uploadedFiles = new List<string>();

			try
			{
				foreach (string pdfFile in SourceFiles)
				{
					// 1a. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
					
					// Prepare URL for `Get Presigned URL` API call
					string query = Uri.EscapeUriString(string.Format(
						"https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
						Path.GetFileName(pdfFile)));

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

						// 1b. UPLOAD THE FILE TO CLOUD.

						webClient.Headers.Add("content-type", "application/octet-stream");
						webClient.UploadFile(uploadUrl, "PUT", pdfFile); // You can use UploadData() instead if your file is byte[] or Stream
						
						uploadedFiles.Add(uploadedFileUrl);
					}
					else
					{
						Console.WriteLine(json["message"].ToString());
					}
				}

				if (uploadedFiles.Count > 0)
				{
                    // 2. MERGE UPLOADED PDF DOCUMENTS

                    // URL for `Merge PDF` API call
                    string url = "https://api.pdf.co/v1/pdf/merge";

                    // Prepare requests params as JSON
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("name", Path.GetFileName(DestinationFile));
                    parameters.Add("url", string.Join(",", uploadedFiles));
                    parameters.Add("async", Async);

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
        /// Check job status
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
