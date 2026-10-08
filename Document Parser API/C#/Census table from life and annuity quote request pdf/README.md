## document parser API in C# and PDF.co Web API PDF.co Web API: the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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

##### **LifeAndAnnuityQuoteRequest.csproj:**
    
```
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="12.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{1E1C2C34-017E-4605-AE2B-55EA3313BE51}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>LifeAndAnnuityQuoteRequest</RootNamespace>
    <AssemblyName>LifeAndAnnuityQuoteRequest</AssemblyName>
    <TargetFrameworkVersion>v4.6</TargetFrameworkVersion>
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
    <Reference Include="Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed, processorArchitecture=MSIL">
      <HintPath>packages\Newtonsoft.Json.10.0.3\lib\net40\Newtonsoft.Json.dll</HintPath>
      <Private>True</Private>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Program.cs" />
  </ItemGroup>
  <ItemGroup>
    <None Include="SampleGroupDisabilityForm.yml">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Include="SampleGroupDisabilityForm.pdf">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Include="packages.config" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name="BeforeBuild">
  </Target>
  <Target Name="AfterBuild">
  </Target>
  -->
</Project>
```

<!-- code block end -->    

<!-- code block begin -->

##### **LifeAndAnnuityQuoteRequest.sln:**
    
```

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2013
VisualStudioVersion = 12.0.40629.0
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LifeAndAnnuityQuoteRequest", "LifeAndAnnuityQuoteRequest.csproj", "{1E1C2C34-017E-4605-AE2B-55EA3313BE51}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{1E1C2C34-017E-4605-AE2B-55EA3313BE51}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1E1C2C34-017E-4605-AE2B-55EA3313BE51}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1E1C2C34-017E-4605-AE2B-55EA3313BE51}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1E1C2C34-017E-4605-AE2B-55EA3313BE51}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal

```

<!-- code block end -->    

<!-- code block begin -->

##### **Program.cs:**
    
```
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace LifeAndAnnuityQuoteRequest
{
    class Program
    {
        // The authentication key (API Key).
        // Get your own by registering at https://app.pdf.co
        const String API_KEY = "***********************************";

        // Sample document containing life and annuity quote request form
        const string SourceFile = @".\SampleGroupDisabilityForm.pdf";

        // PDF document password. Leave empty for unprotected documents.
        const string Password = "";

        // Destination TXT file name
        const string DestinationFile = @".\result.json";

        // (!) Make asynchronous job
        const bool Async = true;


        static void Main(string[] args)
        {
            // Sample template
            String templateText = File.ReadAllText(@".\SampleGroupDisabilityForm.yml");

            if (ProcessAndGetJsonFields(templateText))
            {
                // Get json strings
                string jsonString = File.ReadAllText(DestinationFile);

                // Parse to Json Fields
                var oRet = ParseJsonFields(jsonString);

                // Display some of data to console
                Console.WriteLine("Parsing Details:\n------------------------");

                Console.WriteLine($"Contact Person: {oRet.ContactPerson}");
                Console.WriteLine($"Business Name: {oRet.BusinessName}");
                Console.WriteLine($"Business Address: {oRet.BusinessAddress}");
                Console.WriteLine($"Business Type: {oRet.BusinessType}");
                Console.WriteLine($"Phone: {oRet.Phone}");
                Console.WriteLine($"Email: {oRet.Email}");

                // Export list of census to csv format
                var csvOutputFile = "result.csv";

                var csvString = ConvertToCsv(oRet.lstCensusTable);
                File.WriteAllText(csvOutputFile, csvString);

                Console.WriteLine($"\n{csvOutputFile} generated successfully!");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        /// <summary>
        /// Process Input File and Get Json Fields
        /// </summary>
        static bool ProcessAndGetJsonFields(string templateText)
        {
            // Create standard .NET web client instance
            WebClient webClient = new WebClient();

            // Set API Key
            webClient.Headers.Add("x-api-key", API_KEY);

            // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
            // * If you already have a direct file URL, skip to the step 3.

            // Prepare URL for `Get Presigned URL` API call
            string query = Uri.EscapeUriString(string.Format(
                "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                Path.GetFileName(SourceFile)));

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
                    webClient.UploadFile(uploadUrl, "PUT", SourceFile); // You can use UploadData() instead if your file is byte[] or Stream
                    webClient.Headers.Remove("content-type");

                    // 3. PARSE UPLOADED PDF DOCUMENT

                    // URL of `Document Parser` API call
                    string url = "https://api.pdf.co/v1/pdf/documentparser";

                    // Prepare requests params as JSON
                    Dictionary<string, object> requestBody = new Dictionary<string, object>();
                    requestBody.Add("template", templateText);
                    requestBody.Add("name", Path.GetFileName(DestinationFile));
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
                            string status = CheckJobStatus(webClient, jobId); // Possible statuses: "working", "failed", "aborted", "success".

                            // Display timestamp and status (for demo purposes)
                            Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status);

                            if (status == "success")
                            {
                                // Download JSON file
                                webClient.DownloadFile(resultFileUrl, DestinationFile);

                                Console.WriteLine("Generated JSON file saved as \"{0}\" file.", DestinationFile);

                                return true;
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

            return false;
        }


        /// <summary>
        /// Check job status
        /// </summary>
        static string CheckJobStatus(WebClient webClient, string jobId)
        {
            string url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

            string response = webClient.DownloadString(url);
            JObject json = JObject.Parse(response);

            return Convert.ToString(json["status"]);
        }


        /// <summary>
        /// Parser Json Fields
        /// </summary>
        static FormVM ParseJsonFields(string jsonInput)
        {
            JObject jsonObj = JObject.Parse(jsonInput);

            var oRet = new FormVM();

            oRet.ContactPerson = jsonObj.TraverseJObject("ContactPerson", "field")?.Value<string>("value") ?? "";
            oRet.BusinessName = jsonObj.TraverseJObject("BusinessName", "field")?.Value<string>("value") ?? "";
            oRet.BusinessAddress = jsonObj.TraverseJObject("BusinessAddress", "field")?.Value<string>("value") ?? "";
            oRet.BusinessType = jsonObj.TraverseJObject("BusinessType", "field")?.Value<string>("value") ?? "";
            oRet.Phone = jsonObj.TraverseJObject("Phone", "field")?.Value<string>("value") ?? "";
            oRet.Email = jsonObj.TraverseJObject("Email", "field")?.Value<string>("value") ?? "";

            var censusTable = jsonObj.TraverseJObject("CencusTable", "table");
            if (censusTable != null)
            {
                var rows = censusTable["rows"];

                if (rows != null && rows.Count() > 1)
                {

                    for (int i = 1; i < rows.Count(); i++)
                    {
                        var oCensus = new CensusTableVm();

                        // Parser individual data
                        oCensus.SrNo = rows.ElementAt(i).ElementAt(0).ElementAt(0).Value<string>("value");
                        oCensus.DOB = rows.ElementAt(i).ElementAt(1).ElementAt(0).Value<string>("value");
                        oCensus.Gender = rows.ElementAt(i).ElementAt(2).ElementAt(0).Value<string>("value");
                        oCensus.Occupation = rows.ElementAt(i).ElementAt(3).ElementAt(0).Value<string>("value");
                        oCensus.Salary = rows.ElementAt(i).ElementAt(4).ElementAt(0).Value<string>("value");
                        oCensus.IsShortTermDisability = rows.ElementAt(i).ElementAt(5).ElementAt(0).Value<string>("value");
                        oCensus.IsLongTernDisability = rows.ElementAt(i).ElementAt(6).ElementAt(0).Value<string>("value");
                        oCensus.LifeInsuCoverAmt = rows.ElementAt(i).ElementAt(7).ElementAt(0).Value<string>("value");

                        oRet.lstCensusTable.Add(oCensus);
                    }
                }
            }

            return oRet;
        }

        /// <summary>
        /// Convert to Csv Format
        /// </summary>
        static string ConvertToCsv(List<CensusTableVm> lstCensusTable)
        {
            var oRet = new StringBuilder(string.Empty);

            // Get Header Row
            oRet.AppendLine(CensusTableVm.getCsvTitleRow());

            // Put Child Rows
            foreach (var item in lstCensusTable)
            {
                oRet.AppendLine(item.ToString());
            }

            return oRet.ToString();
        }

    }

    class FormVM
    {
        public string ContactPerson { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<CensusTableVm> lstCensusTable { get; set; } = new List<CensusTableVm>();
    }

    class CensusTableVm
    {
        public string SrNo { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string Salary { get; set; }
        public string IsShortTermDisability { get; set; }
        public string IsLongTernDisability { get; set; }
        public string LifeInsuCoverAmt { get; set; }

        public override string ToString()
        {
            return $"{SrNo},{DOB},{Gender},{Occupation},{Salary},{IsShortTermDisability},{IsLongTernDisability},{LifeInsuCoverAmt}";
        }

        public static string getCsvTitleRow()
        {
            return $"SrNo,DOB,Gender,Occupation,Salary,IsShortTermDisability,IsLongTermDisability,LifeInsuCoverAmt";
        }
    }

    /// <summary>
    /// Extension method container for JObject
    /// </summary>
    public static class JObjectExtensionMethods
    {
        /// <summary>
        /// Traverse JObject
        /// </summary>
        public static JToken TraverseJObject(this JObject jsonObj, string propertyName, string objectType)
        {
            return jsonObj["objects"].Where(x => x.Value<string>("name") == propertyName && x.Value<string>("objectType") == objectType)
                .FirstOrDefault();
        }
    }


}

```

<!-- code block end -->    

<!-- code block begin -->

##### **SampleGroupDisabilityForm.yml:**
    
```
templateName: Untitled document kind
templateVersion: 4
templatePriority: 0
detectionRules:
  keywords: []
objects:
- name: CencusTable
  objectType: table
  tableProperties:
    start:
      y: 324.75
      pageIndex: 0
    end:
      y: 683.25
      pageIndex: 0
    left: 27
    right: 581.25
    rowMergingRule: byBorders
- name: ContactPerson
  objectType: field
  fieldProperties:
    fieldType: rectangle
    rectangle:
    - 178.5
    - 111
    - 76.5
    - 15
    pageIndex: 0
- name: BusinessName
  objectType: field
  fieldProperties:
    fieldType: rectangle
    rectangle:
    - 177.75
    - 126
    - 105
    - 17.25
    pageIndex: 0
- name: BusinessAddress
  objectType: field
  fieldProperties:
    fieldType: rectangle
    rectangle:
    - 183
    - 144.75
    - 187.5
    - 24
    pageIndex: 0
- name: BusinessType
  objectType: field
  fieldProperties:
    fieldType: rectangle
    rectangle:
    - 183.75
    - 206.25
    - 77.25
    - 19.5
    pageIndex: 0
- name: Phone
  objectType: field
  fieldProperties:
    fieldType: rectangle
    rectangle:
    - 181.5
    - 236.25
    - 86.25
    - 15
    pageIndex: 0
- name: Email
  objectType: field
  fieldProperties:
    fieldType: rectangle
    rectangle:
    - 182.25
    - 250.5
    - 112.5
    - 17.25
    pageIndex: 0


```

<!-- code block end -->    

<!-- code block begin -->

##### **packages.config:**
    
```
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Newtonsoft.Json" version="10.0.3" targetFramework="net40" />
</packages>
```

<!-- code block end -->