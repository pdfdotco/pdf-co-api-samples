## How to parse invoice information for document parser API in SharePoint using PDF.co Web API What is PDF.co Web API? It is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://bytescout.zendesk.com/hc/en-us/requests/new?subject=PDF.co%20Web%20API%20Question)

or just send email to [support@bytescout.com](mailto:support@bytescout.com?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)
[Sign Up For Online Training](https://academy.bytescout.com/)


## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://apidocs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **Utils.cs:**
    
```
namespace ParseSimpleDocumentWebPart.VisualWebPart1
{
    public class Utils
    {
        public static string API_KEY = "--ADD-YOUR-PDF-CO-API-KEY-HERE--";
    }
}

```

<!-- code block end -->    

<!-- code block begin -->

##### **VisualWebPart1.cs:**
    
```
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace ParseSimpleDocumentWebPart.VisualWebPart1
{
    [ToolboxItemAttribute(false)]
    public class VisualWebPart1 : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/ParseSimpleDocumentWebPart/VisualWebPart1/VisualWebPart1UserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}

```

<!-- code block end -->    

<!-- code block begin -->

##### **VisualWebPart1UserControl.ascx:**
    
```
<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualWebPart1UserControl.ascx.cs" Inherits="ParseSimpleDocumentWebPart.VisualWebPart1.VisualWebPart1UserControl" %>
Chose source file<br />
<asp:FileUpload ID="FileUpload1" runat="server" Width="600px" />
<br />
<br />
Template<br />
<asp:TextBox ID="TemplateTextBox" runat="server" Height="80px" TextMode="MultiLine" Width="600px"></asp:TextBox>
<br />
<br />
<asp:Button ID="StartButton" runat="server" OnClick="StartButton_Click" Text="Convert to CSV" style="width: 610px; padding-left: 0px; margin-left: 0px; padding-right: 0px; padding-right: 0px;"/>
<br />
<br />
Log<br />
<asp:TextBox ID="LogTextBox" runat="server" Height="80px" TextMode="MultiLine" Width="600px"></asp:TextBox>
<br />
<br />
Result<br />
<asp:TextBox ID="ResultTextBox" runat="server" Height="500px" TextMode="MultiLine" Width="600px"></asp:TextBox>
```

<!-- code block end -->    

<!-- code block begin -->

##### **VisualWebPart1UserControl.ascx.cs:**
    
```
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

```

<!-- code block end -->    

<!-- code block begin -->

##### **VisualWebPart1UserControl.ascx.designer.cs:**
    
```
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated. 
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParseSimpleDocumentWebPart.VisualWebPart1
{


    public partial class VisualWebPart1UserControl
    {

        /// <summary>
        /// FileUpload1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.FileUpload FileUpload1;

        /// <summary>
        /// TemplateTextBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox TemplateTextBox;

        /// <summary>
        /// StartButton control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button StartButton;

        /// <summary>
        /// LogTextBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox LogTextBox;

        /// <summary>
        /// ResultTextBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox ResultTextBox;
    }
}

```

<!-- code block end -->