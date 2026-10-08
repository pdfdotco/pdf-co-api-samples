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
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace AddTextAndImagesToPDFWebPart.VisualWebPart1
{
    public partial class VisualWebPart1UserControl : UserControl
    {
        public SPWeb CurrentWeb { get; set; }

        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        string API_KEY = Utils.API_KEY;

        // Direct URL of source PDF file.
        // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/
        const string SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf";

        // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
        const string Pages = "";
        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";

        // Destination PDF file name
        const string DestinationFile = "result.pdf";
        const string DestinationFileImage = "result_image.pdf";
        const string DestinationLibName = "Shared Documents";

        // Text annotation params
        const string Type = "annotation";
        const int X = 400;
        const int Y = 600;
        const string Text = "APPROVED";
        const string FontName = "Times New Roman";
        const float FontSize = 24;
        const string FontColor = "FF0000";

        // Image params
        private const int X1 = 400;
        private const int Y1 = 20;
        private const int Width1 = 119;
        private const int Height1 = 32;
        private const string ImageUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void StartButton_Click(object sender, EventArgs e)
        {
            // Create standard .NET web client instance
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            // * Add text annotation *

            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co/04-pdf-add-text-signatures-and-images-to-pdf
            string jsonPayload = $@"{{
                                        ""name"": ""{DestinationFile}"",
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
                                        ],
                                        ""images"": [
                                                {{
                                                    ""url"": ""{ImageUrl}"",
                                                    ""x"": {X1},
                                                    ""y"": {Y1},
                                                    ""width"": {Width1},
                                                    ""height"": {Height1},
                                                    ""pages"": ""{Pages}""
                                                }}
                                            ]
                                    }}";

            try
            {
                // URL of "PDF Edit" endpoint
                string url = "https://api.pdf.co/v1/pdf/edit/add";

                // Execute POST request with JSON payload
                string response = webClient.UploadString(url, jsonPayload);

                LogTextBox.Text += String.Format("Add text \"{0}\" to pdf file \"{1}\". \n", Text, SourceFileUrl);
                LogTextBox.Text += String.Format("Add image \"{0}\" to pdf file. \n", ImageUrl);

                // Parse JSON response
                JObject json = JObject.Parse(response);

                if (json["error"].ToObject<bool>() == false)
                {
                    // Get URL of generated PDF file
                    string resultFileUrl = json["url"].ToString();

                    // Download generated PDF file
                    var retData = webClient.DownloadData(resultFileUrl);

                    //Upload file to SharePoint document linrary
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

                    LogTextBox.Text += String.Format("Generated PDF file saved as \"{0}\\{1}\" file. \n", DestinationLibName, DestinationFile);
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
            finally
            {
                webClient.Dispose();
            }

            LogTextBox.Text += "\n";
            LogTextBox.Text += "Done...\n";
        }
    }
}
