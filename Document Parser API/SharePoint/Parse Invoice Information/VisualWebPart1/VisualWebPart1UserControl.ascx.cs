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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace ParseSimpleDocumentWebPart.VisualWebPart1
{
    public partial class VisualWebPart1UserControl : UserControl
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        string API_KEY = Utils.API_KEY;
        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";

        // (!) Make asynchronous job
        const bool Async = true;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void StartButton_Click(object sender, EventArgs e)
        {
            // Template text. Use Document Parser (https://pdf.co/document-parser, https://app.pdf.co/document-parser)
            // to create templates.

            // Create standard .NET web client instance
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();

            if (!FileUpload1.HasFile && String.IsNullOrWhiteSpace(TemplateTextBox.Text))
            {
                LogTextBox.Text += "Select file and template \n";
                return;
            }

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
            // * If you already have a direct file URL, skip to the step 3.

            // Prepare URL for `Get Presigned URL` API call
            string query = Uri.EscapeUriString(string.Format(
                "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                FileUpload1.FileName));

            try
            {
                // Execute request
                string response = webClient.DownloadString(query);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL to use for the file upload
                    string uploadUrl = json["presignedUrl"].ToString();
                    string uploadedFileUrl = json["url"].ToString();

                    // 2. UPLOAD THE FILE TO CLOUD.

                    webClient.Headers.Add("content-type", "application/octet-stream");
                    webClient.UploadData(uploadUrl, "PUT", FileUpload1.FileBytes);
                    webClient.Headers.Remove("content-type");

                    // 3. PARSE UPLOADED PDF DOCUMENT

                    // URL of `Document Parser` API call
                    string url = "https://api.pdf.co/v1/pdf/documentparser";

                    Dictionary<string, object> requestBody = new Dictionary<string, object>();
                    requestBody.Add("template", TemplateTextBox.Text);
                    requestBody.Add("name", FileUpload1.FileName);
                    requestBody.Add("url", uploadedFileUrl);
                    requestBody.Add("async", Async);

                    // Convert dictionary of params to JSON
                    string jsonPayload = JsonConvert.SerializeObject(requestBody);

                    // Execute request
                    response = webClient.UploadString(url, "POST", jsonPayload);

                    // Parse JSON response
                    json = JObject.Parse(response);

                    if (json["error"].ToObject<bool>() == false)
                    {
                        // Asynchronous job ID
                        string jobId = json["jobId"].ToString();
                        // Get URL of generated JSON file
                        string resultFileUrl = json["url"].ToString();

                        // Check the job status in a loop. 
                        // If you don't want to pause the main thread you can rework the code 
                        // to use a separate thread for the status checking and completion.
                        do
                        {
                            string status = CheckJobStatus(jobId); // Possible statuses: "working", "failed", "aborted", "success".

                            // Display timestamp and status (for demo purposes)
                            LogTextBox.Text += DateTime.Now.ToLongTimeString() + ": " + status + "\n";

                            if (status == "success")
                            {
                                // Download JSON result
                                var result = webClient.DownloadString(resultFileUrl);

                                LogTextBox.Text += "Generated JSON.\n";
                                ResultTextBox.Text += result;
                                break;
                            }
                            else if (status == "working")
                            {
                                // Pause for a few seconds
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                LogTextBox.Text += status + " \n";
                                break;
                            }
                        }
                        while (true);
                    }
                    else
                    {
                        LogTextBox.Text += json["message"].ToString() + " \n";
                    }
                }
                else
                {
                    LogTextBox.Text += json["message"].ToString() + " \n";
                }
            }
            catch (Exception ex)
            {
                LogTextBox.Text += ex.ToString() + " \n";
            }

            webClient.Dispose();

            LogTextBox.Text += "\n";
            LogTextBox.Text += "Done...\n";
        }

        protected string CheckJobStatus(string jobId)
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
