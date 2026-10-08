## How to convert PDF to XLS from uploaded file (winforms) for PDF to excel API in C# and PDF.co Web API

### How to convert PDF to XLS from uploaded file (winforms) in C# with easy ByteScout code samples to make PDF to excel API. Step-by-step tutorial

The documentation is designed to help you to implement the features on your side. PDF.co Web API helps with PDF to excel API in C#. PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

The SDK samples like this one below explain how to quickly make your application do PDF to excel API in C# with the help of PDF.co Web API. Follow the instruction and copy - paste code for C# into your project's code editor. Tutorials are available along with installed PDF.co Web API if you'd like to dive deeper into the topic and the details of the API.

ByteScout free trial version is available for FREE download from our website. Programming tutorials along with source code samples are included.

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

##### **Form1.Designer.cs:**
    
```
namespace PdfToExcelFrom
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtCloudAPIKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtInputPDFFileName = new System.Windows.Forms.TextBox();
            this.btnSelectInputFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOutputAs = new System.Windows.Forms.ComboBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbConvertTo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "ByteScout Cloud API Key";
            // 
            // txtCloudAPIKey
            // 
            this.txtCloudAPIKey.Location = new System.Drawing.Point(184, 14);
            this.txtCloudAPIKey.Name = "txtCloudAPIKey";
            this.txtCloudAPIKey.Size = new System.Drawing.Size(353, 22);
            this.txtCloudAPIKey.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Input PDF File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // txtInputPDFFileName
            // 
            this.txtInputPDFFileName.Location = new System.Drawing.Point(115, 48);
            this.txtInputPDFFileName.Name = "txtInputPDFFileName";
            this.txtInputPDFFileName.Size = new System.Drawing.Size(290, 22);
            this.txtInputPDFFileName.TabIndex = 3;
            // 
            // btnSelectInputFile
            // 
            this.btnSelectInputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectInputFile.Location = new System.Drawing.Point(411, 46);
            this.btnSelectInputFile.Name = "btnSelectInputFile";
            this.btnSelectInputFile.Size = new System.Drawing.Size(126, 36);
            this.btnSelectInputFile.TabIndex = 4;
            this.btnSelectInputFile.Text = "Select input File";
            this.btnSelectInputFile.UseVisualStyleBackColor = true;
            this.btnSelectInputFile.Click += new System.EventHandler(this.btnSelectInputFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output As";
            // 
            // cmbOutputAs
            // 
            this.cmbOutputAs.FormattingEnabled = true;
            this.cmbOutputAs.Items.AddRange(new object[] {
            "URL to output file",
            "inline content"});
            this.cmbOutputAs.Location = new System.Drawing.Point(107, 133);
            this.cmbOutputAs.Name = "cmbOutputAs";
            this.cmbOutputAs.Size = new System.Drawing.Size(430, 24);
            this.cmbOutputAs.TabIndex = 6;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(16, 176);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(194, 43);
            this.btnConvert.TabIndex = 7;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Convert To";
            // 
            // cmbConvertTo
            // 
            this.cmbConvertTo.FormattingEnabled = true;
            this.cmbConvertTo.Items.AddRange(new object[] {
            "XLS",
            "XLSX"});
            this.cmbConvertTo.Location = new System.Drawing.Point(107, 93);
            this.cmbConvertTo.Name = "cmbConvertTo";
            this.cmbConvertTo.Size = new System.Drawing.Size(430, 24);
            this.cmbConvertTo.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 255);
            this.Controls.Add(this.cmbConvertTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.cmbOutputAs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSelectInputFile);
            this.Controls.Add(this.txtInputPDFFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCloudAPIKey);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Cloud API: PDF to Excel Conversion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCloudAPIKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtInputPDFFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOutputAs;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnSelectInputFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbConvertTo;
    }
}


```

<!-- code block end -->    

<!-- code block begin -->

##### **Form1.cs:**
    
```
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
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
                        string response = webClient.UploadString(url, jsonPayload);

                        // Parse JSON response
                        json = JObject.Parse(response);

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

```

<!-- code block end -->    

<!-- code block begin -->

##### **PdfToExcelFromUploadedFile.csproj:**
    
```
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{4E476AFF-4982-4A1E-930E-6D25DEC3421A}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <RootNamespace>PdfToExcelFromUploadedFile</RootNamespace>
    <AssemblyName>PdfToExcelFromUploadedFile</AssemblyName>
    <TargetFrameworkVersion>v2.0</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="Newtonsoft.Json, Version=11.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL">
      <HintPath>..\packages\Newtonsoft.Json.11.0.2\lib\net20\Newtonsoft.Json.dll</HintPath>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Deployment" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Form1.cs">
      <SubType>Form</SubType>
    </Compile>
    <Compile Include="Form1.Designer.cs">
      <DependentUpon>Form1.cs</DependentUpon>
    </Compile>
    <Compile Include="Program.cs" />
  </ItemGroup>
  <ItemGroup>
    <EmbeddedResource Include="Form1.resx">
      <DependentUpon>Form1.cs</DependentUpon>
    </EmbeddedResource>
  </ItemGroup>
  <ItemGroup>
    <None Include="packages.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
```

<!-- code block end -->    

<!-- code block begin -->

##### **Program.cs:**
    
```
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PdfToExcelFrom
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

```

<!-- code block end -->    

<!-- code block begin -->

##### **packages.config:**
    
```
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Newtonsoft.Json" version="11.0.2" targetFramework="net20" />
</packages>
```

<!-- code block end -->