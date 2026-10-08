## How to add text and images to PDF for visual web part 1 in SharePoint using PDF.co Web API PDF.co Web API: the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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
namespace AddTextAndImagesToPDFWebPart.VisualWebPart1
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

namespace AddTextAndImagesToPDFWebPart.VisualWebPart1
{
    [ToolboxItemAttribute(false)]
    public class VisualWebPart1 : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/AddTextAndImagesToPDFWebPart/VisualWebPart1/VisualWebPart1UserControl.ascx";

        protected override void CreateChildControls()
        {
            var control = (VisualWebPart1UserControl)Page.LoadControl(_ascxPath);
            Controls.Add(control);

            control.CurrentWeb = SPContext.Current.Web;
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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisualWebPart1UserControl.ascx.cs" Inherits="AddTextAndImagesToPDFWebPart.VisualWebPart1.VisualWebPart1UserControl" %>
<br />
<asp:Button ID="StartButton" runat="server" OnClick="StartButton_Click" Text="Start" style="width: 610px; padding-left: 0px; margin-left: 0px; padding-right: 0px; padding-right: 0px;"/>
<br />
<br />
Log<br />
<asp:TextBox ID="LogTextBox" runat="server" Height="300px" TextMode="MultiLine" Width="600px"></asp:TextBox>
<br />

```

<!-- code block end -->    

<!-- code block begin -->

##### **VisualWebPart1UserControl.ascx.cs:**
    
```
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

namespace AddTextAndImagesToPDFWebPart.VisualWebPart1
{


    public partial class VisualWebPart1UserControl
    {

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
    }
}

```

<!-- code block end -->