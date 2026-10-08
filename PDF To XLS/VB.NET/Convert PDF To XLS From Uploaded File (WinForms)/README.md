## How to convert PDF to XLS from uploaded file (winforms) for PDF to excel API in VB.NET using PDF.co Web API

### Tutorial: how to convert PDF to XLS from uploaded file (winforms) for PDF to excel API in VB.NET

Writing of the code to convert PDF to XLS from uploaded file (winforms) in VB.NET can be done by developers of any level using PDF.co Web API. PDF to excel API in VB.NET can be implemented with PDF.co Web API. PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

VB.NET code snippet like this for PDF.co Web API works best when you need to quickly implement PDF to excel API in your VB.NET application. This VB.NET sample code should be copied and pasted into your project. After doing this just compile your project and click Run. You can use these VB.NET sample examples in one or many applications.

Our website provides free trial version of PDF.co Web API that includes source code samples to help with your VB.NET project.

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

##### **Form1.Designer.vb:**
    
```
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmbConvertTo = New System.Windows.Forms.ComboBox()
        Me.label4 = New System.Windows.Forms.Label()
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.cmbOutputAs = New System.Windows.Forms.ComboBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.btnSelectInputFile = New System.Windows.Forms.Button()
        Me.txtInputPDFFileName = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.txtCloudAPIKey = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'cmbConvertTo
        '
        Me.cmbConvertTo.FormattingEnabled = True
        Me.cmbConvertTo.Items.AddRange(New Object() {"XLS", "XLSX"})
        Me.cmbConvertTo.Location = New System.Drawing.Point(116, 101)
        Me.cmbConvertTo.Name = "cmbConvertTo"
        Me.cmbConvertTo.Size = New System.Drawing.Size(430, 24)
        Me.cmbConvertTo.TabIndex = 19
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(22, 101)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(78, 17)
        Me.label4.TabIndex = 18
        Me.label4.Text = "Convert To"
        '
        'btnConvert
        '
        Me.btnConvert.Location = New System.Drawing.Point(25, 184)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(194, 43)
        Me.btnConvert.TabIndex = 17
        Me.btnConvert.Text = "Convert"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'cmbOutputAs
        '
        Me.cmbOutputAs.FormattingEnabled = True
        Me.cmbOutputAs.Items.AddRange(New Object() {"URL to output file", "inline content"})
        Me.cmbOutputAs.Location = New System.Drawing.Point(116, 141)
        Me.cmbOutputAs.Name = "cmbOutputAs"
        Me.cmbOutputAs.Size = New System.Drawing.Size(430, 24)
        Me.cmbOutputAs.TabIndex = 16
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(22, 141)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(71, 17)
        Me.label3.TabIndex = 15
        Me.label3.Text = "Output As"
        '
        'btnSelectInputFile
        '
        Me.btnSelectInputFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectInputFile.Location = New System.Drawing.Point(420, 54)
        Me.btnSelectInputFile.Name = "btnSelectInputFile"
        Me.btnSelectInputFile.Size = New System.Drawing.Size(126, 36)
        Me.btnSelectInputFile.TabIndex = 14
        Me.btnSelectInputFile.Text = "Select input File"
        Me.btnSelectInputFile.UseVisualStyleBackColor = True
        '
        'txtInputPDFFileName
        '
        Me.txtInputPDFFileName.Location = New System.Drawing.Point(124, 56)
        Me.txtInputPDFFileName.Name = "txtInputPDFFileName"
        Me.txtInputPDFFileName.Size = New System.Drawing.Size(290, 22)
        Me.txtInputPDFFileName.TabIndex = 13
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(22, 59)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(96, 17)
        Me.label2.TabIndex = 12
        Me.label2.Text = "Input PDF File"
        '
        'txtCloudAPIKey
        '
        Me.txtCloudAPIKey.Location = New System.Drawing.Point(193, 22)
        Me.txtCloudAPIKey.Name = "txtCloudAPIKey"
        Me.txtCloudAPIKey.Size = New System.Drawing.Size(353, 22)
        Me.txtCloudAPIKey.TabIndex = 11
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(22, 21)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(165, 17)
        Me.label1.TabIndex = 10
        Me.label1.Text = "ByteScout Cloud API Key"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 251)
        Me.Controls.Add(Me.cmbConvertTo)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.cmbOutputAs)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.btnSelectInputFile)
        Me.Controls.Add(Me.txtInputPDFFileName)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.txtCloudAPIKey)
        Me.Controls.Add(Me.label1)
        Me.Name = "Form1"
        Me.Text = "Cloud API: PDF to Excel Conversion"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents cmbConvertTo As ComboBox
    Private WithEvents label4 As Label
    Private WithEvents btnConvert As Button
    Private WithEvents cmbOutputAs As ComboBox
    Private WithEvents label3 As Label
    Private WithEvents btnSelectInputFile As Button
    Private WithEvents txtInputPDFFileName As TextBox
    Private WithEvents label2 As Label
    Private WithEvents txtCloudAPIKey As TextBox
    Private WithEvents label1 As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class

```

<!-- code block end -->    

<!-- code block begin -->

##### **Form1.vb:**
    
```
Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class Form1

#Region "File Selection"

    Private Sub btnSelectInputFile_Click(sender As Object, e As EventArgs) Handles btnSelectInputFile.Click

        OpenFileDialog1.ShowDialog()

    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        txtInputPDFFileName.Text = OpenFileDialog1.FileName

    End Sub


#End Region

#Region "Convert PDF to Excel"

    ''' <summary>
    ''' Perform convert PDF to Excel
    ''' </summary>
    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click

        Try

            If (ValidateInputs()) Then

                ' Comma-separated list of page indices (Or ranges) to process. Leave empty for all pages. Example '0,2-5,7-'.
                Const Pages As String = ""

                ' PDF document password. Leave empty for unprotected documents.
                Const Password As String = ""


                ' Checks whether convert to as inline content.
                Dim isInline As Boolean = Convert.ToString(cmbOutputAs.SelectedItem).ToLower() = "inline content"

                ' Destination file name
                Dim DestinationFile As String = String.Format(".\{0}.{1}",
                                                              Path.GetFileNameWithoutExtension(txtInputPDFFileName.Text),
                                                              Convert.ToString(cmbConvertTo.SelectedItem).ToLower())

                ' Create standard .NET web client instance
                Dim webClient As WebClient = New WebClient()

                ' Set API Key
                webClient.Headers.Add("x-api-key", txtCloudAPIKey.Text.Trim())

                ' 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
                ' * If you already have a direct file URL, skip to the step 3.

                ' Prepare URL for `Get Presigned URL` API call
                Dim query As String = Uri.EscapeUriString(
                    String.Format("https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                                  Path.GetFileName(txtInputPDFFileName.Text)))

                ' Execute request
                Dim response As String = webClient.DownloadString(query)

                ' Parse JSON response
                Dim json As JObject = JObject.Parse(response)

                If json("error").ToObject(Of Boolean)() = False Then

                    ' Get URL to use for the file upload
                    Dim uploadUrl As String = json("presignedUrl").ToString()
                    Dim uploadedFileUrl As String = json("url").ToString()

                    ' 2. UPLOAD THE FILE TO CLOUD.
                    webClient.Headers.Add("content-type", "application/octet-stream")
                    webClient.UploadFile(uploadUrl, "PUT", txtInputPDFFileName.Text)
                    webClient.Headers.Remove("content-type")

                    ' 3. CONVERT UPLOADED PDF FILE TO Excel

                    ' Prepare URL for `PDF To Excel` API call
                    query = Uri.EscapeUriString(
                        String.Format("https://api.pdf.co/v1/pdf/convert/to/{4}?name={0}&password={1}&pages={2}&url={3}&encrypt=true&inline={5}",
                                      Path.GetFileName(DestinationFile),
                                      Password,
                                      Pages,
                                      uploadedFileUrl,
                                      Convert.ToString(cmbConvertTo.SelectedItem).ToLower(),
                                      isInline))

                    ' Execute request
                    response = webClient.DownloadString(query)

                    ' Parse JSON response
                    json = JObject.Parse(response)

                    If json("error").ToObject(Of Boolean)() = False Then

                        ' Get URL of generated Excel file
                        Dim resultFileUrl As String = json("url").ToString()

                        ' Download Excel output file
                        webClient.DownloadFile(resultFileUrl, DestinationFile)

                        MessageBox.Show($"Generated XLS file saved as {DestinationFile} file.", "Success!")

                        ' Open Downloaded output file
                        Process.Start(DestinationFile)
                    Else
                        MessageBox.Show(json("message").ToString())
                    End If
                Else
                    MessageBox.Show(json("message").ToString())
                End If

                webClient.Dispose()

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        End Try

    End Sub

    ''' <summary>
    ''' Validates form inputs
    ''' </summary>
    ''' <returns></returns>
    Private Function ValidateInputs() As Boolean

        If (String.IsNullOrEmpty(txtCloudAPIKey.Text)) Then
            Throw New Exception("Cloud API key cannot be empty")
        End If

        If (String.IsNullOrEmpty(txtInputPDFFileName.Text)) Then
            Throw New Exception("Input PDf file must be selected/entered.")
        End If

        If (Not System.IO.File.Exists(txtInputPDFFileName.Text)) Then
            Throw New Exception("Input file does not exits.")
        End If

        If (System.IO.Path.GetExtension(txtInputPDFFileName.Text).ToLower() <> ".pdf") Then
            Throw New Exception("Input file must be PDF")
        End If

        If (String.IsNullOrEmpty(Convert.ToString(cmbConvertTo.SelectedItem))) Then
            Throw New Exception("Please select convert to option.")
        End If

        If (String.IsNullOrEmpty(Convert.ToString(cmbOutputAs.SelectedItem))) Then
            Throw New Exception("Please select output-as option")
        End If

        Return True

    End Function


#End Region

End Class

```

<!-- code block end -->    

<!-- code block begin -->

##### **PdfToExcelFromUploadedFile.vbproj:**
    
```
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{24EF098A-6CEA-410F-A81A-DD6AF0F90B56}</ProjectGuid>
    <OutputType>WinExe</OutputType>
    <StartupObject>PdfToExcelFromUploadedFile.Form1</StartupObject>
    <RootNamespace>PdfToExcelFromUploadedFile</RootNamespace>
    <AssemblyName>PdfToExcelFromUploadedFile</AssemblyName>
    <FileAlignment>512</FileAlignment>
    <MyType>WindowsForms</MyType>
    <TargetFrameworkVersion>v4.6</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <DefineDebug>true</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <OutputPath>bin\Debug\</OutputPath>
    <DocumentationFile>PdfToExcelFromUploadedFile.xml</DocumentationFile>
    <NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <DebugType>pdbonly</DebugType>
    <DefineDebug>false</DefineDebug>
    <DefineTrace>true</DefineTrace>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DocumentationFile>PdfToExcelFromUploadedFile.xml</DocumentationFile>
    <NoWarn>42016,41999,42017,42018,42019,42032,42036,42020,42021,42022</NoWarn>
  </PropertyGroup>
  <PropertyGroup>
    <OptionExplicit>On</OptionExplicit>
  </PropertyGroup>
  <PropertyGroup>
    <OptionCompare>Binary</OptionCompare>
  </PropertyGroup>
  <PropertyGroup>
    <OptionStrict>Off</OptionStrict>
  </PropertyGroup>
  <PropertyGroup>
    <OptionInfer>On</OptionInfer>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="Newtonsoft.Json, Version=11.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL">
      <HintPath>..\packages\Newtonsoft.Json.11.0.2\lib\net40\Newtonsoft.Json.dll</HintPath>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.Data" />
    <Reference Include="System.Deployment" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
  </ItemGroup>
  <ItemGroup>
    <Import Include="Microsoft.VisualBasic" />
    <Import Include="System" />
    <Import Include="System.Collections" />
    <Import Include="System.Collections.Generic" />
    <Import Include="System.Data" />
    <Import Include="System.Drawing" />
    <Import Include="System.Diagnostics" />
    <Import Include="System.Windows.Forms" />
    <Import Include="System.Linq" />
    <Import Include="System.Xml.Linq" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Form1.vb">
      <SubType>Form</SubType>
    </Compile>
    <Compile Include="Form1.Designer.vb">
      <DependentUpon>Form1.vb</DependentUpon>
      <SubType>Form</SubType>
    </Compile>
  </ItemGroup>
  <ItemGroup>
    <EmbeddedResource Include="Form1.resx">
      <DependentUpon>Form1.vb</DependentUpon>
    </EmbeddedResource>
  </ItemGroup>
  <ItemGroup>
    <None Include="packages.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.VisualBasic.targets" />
</Project>
```

<!-- code block end -->    

<!-- code block begin -->

##### **packages.config:**
    
```
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Newtonsoft.Json" version="11.0.2" targetFramework="net40" />
</packages>
```

<!-- code block end -->