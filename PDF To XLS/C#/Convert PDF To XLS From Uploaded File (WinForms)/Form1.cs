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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PdfToExcelFrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region File Selection 

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            txtInputPDFFileName.Text = openFileDialog1.FileName;
        }

        private void btnSelectInputFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        #endregion

        #region Convert PDF to Excel

        /// <summary>
        /// Perform convert PDF to Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
                    const string Pages = "";

                    // PDF document password. Leave empty for unprotected documents.
                    const string Password = "";

                    // Checks whether convert to as inline content.
                    bool isInline = Convert.ToString(cmbOutputAs.SelectedItem).ToLower() == "inline content";

                    // Destination file name
                    string DestinationFile = string.Format(@".\{0}.{1}",
                        Path.GetFileNameWithoutExtension(txtInputPDFFileName.Text),
                        Convert.ToString(cmbConvertTo.SelectedItem).ToLower());

                    // Create standard .NET web client instance
                    WebClient webClient = new WebClient();

                    // Set API Key
                    webClient.Headers.Add("x-api-key", txtCloudAPIKey.Text.Trim());

                    // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
                    // * If you already have a direct file URL, skip to the step 3.

                    // Prepare URL for `Get Presigned URL` API call
                    string query = Uri.EscapeUriString(string.Format(
                        "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                        Path.GetFileName(txtInputPDFFileName.Text)));

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
                        webClient.UploadFile(uploadUrl, "PUT", txtInputPDFFileName.Text); // You can use UploadData() instead if your file is byte[] or Stream
                        webClient.Headers.Remove("content-type");

                        // 3. CONVERT UPLOADED PDF FILE TO Excel

                        // Prepare URL for `PDF To Excel` API call
                        var url = string.Format("https://api.pdf.co/v1/pdf/convert/to/{0}",Convert.ToString(cmbConvertTo.SelectedItem).ToLower());

                        // Prepare requests params as JSON
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("name", Path.GetFileName(DestinationFile));
                        parameters.Add("password", Password);
                        parameters.Add("pages", Password);
                        parameters.Add("url", uploadedFileUrl);
                        parameters.Add("encrypt", true);
                        parameters.Add("inline", isInline);

                        // Convert dictionary of params to JSON
                        string jsonPayload = JsonConvert.SerializeObject(parameters);

                        // Execute POST request with JSON payload
                        string response_json = webClient.UploadString(url, jsonPayload);

                        // Parse JSON response
                        json = JObject.Parse(response_json);

                        if (json["error"].ToObject<bool>() == false)
                        {
                            // Get URL of generated Excel file
                            string resultFileUrl = json["url"].ToString();

                            // Download Excel output file
                            webClient.DownloadFile(resultFileUrl, DestinationFile);

                            MessageBox.Show($"Generated XLS file saved as {DestinationFile} file.", "Success!");

                            // Open Downloaded output file
                            Process.Start(DestinationFile);
                        }
                        else
                        {
                            MessageBox.Show(json["message"].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show(json["message"].ToString());
                    }
                    webClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        /// <summary>
        /// Validates form inputs
        /// </summary>
        /// <returns></returns>
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtCloudAPIKey.Text))
            {
                throw new Exception("Cloud API key cannot be empty");
            }

            if (string.IsNullOrEmpty(txtInputPDFFileName.Text))
            {
                throw new Exception("Input PDf file must be selected/entered.");
            }

            if (!System.IO.File.Exists(txtInputPDFFileName.Text))
            {
                throw new Exception("Input file does not exits.");
            }

            if (System.IO.Path.GetExtension(txtInputPDFFileName.Text).ToLower() != ".pdf")
            {
                throw new Exception("Input file must be PDF");
            }

            if (string.IsNullOrEmpty(Convert.ToString(cmbConvertTo.SelectedItem)))
            {
                throw new Exception("Please select convert to option.");
            }

            if (string.IsNullOrEmpty(Convert.ToString(cmbOutputAs.SelectedItem)))
            {
                throw new Exception("Please select output-as option");
            }

            return true;
        }

        #endregion

    }
}
