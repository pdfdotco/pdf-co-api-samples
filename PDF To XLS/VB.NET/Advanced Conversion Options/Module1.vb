'*******************************************************************************************'
'                                                                                           '
' Get API Key https://app.pdf.co/signup                                                     '
' API Documentation: https://developer.pdf.co/api/index.html                                '
'                                                                                           '
' Note: Replace placeholder values in the code with your API Key                            '
' and file paths (if applicable)                                                            '
'                                                                      '
'*******************************************************************************************'


Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module Module1

    ' The authentication key (API Key).
    ' Get your own by registering at https://app.pdf.co
    Const API_KEY As String = "***********************************"

    ' Direct URL of source PDF file.
    ' You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/   
    Const SourceFileUrl As String = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-to-excel/sample.pdf"
    ' Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
    Const Pages As String = ""
    ' PDF document password. Leave empty for unprotected documents.
    Const Password As String = ""
    ' Destination XLS file name
    Const DestinationFile As String = ".\result.xls"

    ' Some of advanced options available through profiles:
    ' (JSON can be single/double-quoted and contain comments.)
    ' {
    '     "profiles": [
    '         {
    '             "profile1": {
    '                 "NumberDecimalSeparator": "", // Allows to customize decimal separator in numbers.
    '                 "NumberGroupSeparator": "", // Allows to customize thousands separator.
    '                 "AutoDetectNumbers": true, // Whether to detect numbers. Values: true / false
    '                 "RichTextFormatting": true, // Whether to keep text style and fonts. Values: true / false
    '                 "PageToWorksheet": true, // Whether to create separate worksheet for each page of PDF document. Values: true / false
    '                 "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
    '                 "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
    '                 "LineGroupingMode": "None", // Values: "None", "GroupByRows", "GroupByColumns", "JoinOrphanedRows"
    '                 "ColumnDetectionMode": "ContentGroupsAndBorders", // Values: "ContentGroupsAndBorders", "ContentGroups", "Borders", "BorderedTables"
    '                 "Unwrap": false, // Unwrap grouped text in table cells. Values: true / false
    '                 "ShrinkMultipleSpaces": false, // Shrink multiple spaces in table cells that affect column detection. Values: true / false
    '                 "DetectNewColumnBySpacesRatio": 1, // Spacing ratio that affects column detection.
    '                 "CustomExtractionColumns": [ 0, 50, 150, 200, 250, 300 ], // Explicitly specify columns coordinates for table extraction.
    '                 "CheckPermissions": true, // Ignore document permissions. Values: true / false
    '             }
    '         }
    '     ]
    ' }

    ' Sample profile that sets advanced conversion options.
    ' Advanced options are properties of XLSExtractor class from ByteScout PDF Extractor SDK used in the back-end:
    ' https//cdn.bytescout.com/help/BytescoutPDFExtractorSDK/html/2712c05b-9674-5253-df76-2a31ed055afd.htm
    Dim Profiles As String = File.ReadAllText("profile.json")


    Sub Main()

        ' Create standard .NET web client instance
        Dim webClient As WebClient = New WebClient()

        ' Set API Key
        webClient.Headers.Add("x-api-key", API_KEY)

        ' Set JSON content type
        webClient.Headers.Add("Content-Type", "application/json")

        ' Prepare URL for `PDF To XLS` API call
		Dim url As String = "https://api.pdf.co/v1/pdf/convert/to/xls"

        ' Prepare requests params as JSON
        ' See documentation: https : //apidocs.pdf.co
        Dim parameters As New Dictionary(Of String, Object)
		parameters.Add("name", Path.GetFileName(DestinationFile))
		parameters.Add("password", Password)
		parameters.Add("pages", Pages)
		parameters.Add("url", SourceFileUrl)
		parameters.Add("profiles", Profiles)

        ' Convert dictionary of params to JSON
        Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)

        Try
            ' Execute POST request with JSON payload
            Dim response As String = webClient.UploadString(url, jsonPayload)

            ' Parse JSON response
            Dim json As JObject = JObject.Parse(response)

            If json("error").ToObject(Of Boolean) = False Then

                ' Get URL of generated XLS file
                Dim resultFileUrl As String = json("url").ToString()

                ' Download XLS file
                webClient.DownloadFile(resultFileUrl, DestinationFile)

                Console.WriteLine("Generated XLS file saved as ""{0}"" file.", DestinationFile)

            Else
                Console.WriteLine(json("message").ToString())
            End If

        Catch ex As WebException
            Console.WriteLine(ex.ToString())
        End Try

        webClient.Dispose()

        Console.WriteLine()
        Console.WriteLine("Press any key...")
        Console.ReadKey()

    End Sub

End Module
