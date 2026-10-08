//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


using Microsoft.SharePoint;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace ConvertWebPageToPDFLinkWebPart.VisualWebPart1
{
    public partial class VisualWebPart1UserControl : UserControl
    {
        public SPWeb CurrentWeb { get; set; }

        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        string API_KEY = Utils.API_KEY;

        // Destination PDF file name
        const string DestinationFile = "result.pdf";
        const string DestinationLibName = "Shared Documents";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void StartButton_Click(object sender, EventArgs e)
        {
            // Create standard .NET web client instance
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();
            var SourceUrl = UrlTextBox.Text;

            if (String.IsNullOrWhiteSpace(SourceUrl))
            {
                LogTextBox.Text += "Enter valid web page url \n";
                return;
            }

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);
            
            // URL for `Web Page to PDF` API call
            string url = "https://api.pdf.co/v1/pdf/convert/from/url";

            // Prepare requests params as JSON
            Dictionary<string, object> requestBody = new Dictionary<string, object>();
            requestBody.Add("name", DestinationFile);
            requestBody.Add("url", SourceUrl);

            // Convert dictionary of params to JSON
            string jsonPayload = JsonConvert.SerializeObject(requestBody);

            try
            {
                // Execute POST request
                var response = webClient.UploadString(url, "POST", jsonPayload);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL of generated PDF file
                    string resultFileUrl = json["url"].ToString();

                    // Download PDF file
                    var retData = webClient.DownloadData(resultFileUrl);

                    //Upload file to SharePoint document library
                    //Read create stream
                    using (MemoryStream stream = new MemoryStream(retData))
                    {
                        //Get handle of library
                        SPFolder spLibrary = CurrentWeb.Folders[DestinationLibName];

                        //Replace existing file
                        var replaceExistingFile = true;

                        //Upload document to library
                        SPFile spfile = spLibrary.Files.Add(DestinationFile, stream, replaceExistingFile);
                        spLibrary.Update();
                    }

                    LogTextBox.Text += String.Format("Generated PDF document saved as \"{0}\\{1}\" file. \n", DestinationLibName, DestinationFile);
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
    }
}
