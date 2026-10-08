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

' Visit Knowledgebase for adding Text Macros to PDF 
' https://apidocs.pdf.co/kb/Fill%20PDF%20and%20Add%20Text%20or%20Images%20(pdf-edit-add)/macros

Module Module1

    ' The authentication key (API Key).
    ' Get your own by registering at https://app.pdf.co
    Const API_KEY As String = "***********************************"

    ' Direct URL of source PDF file.
    ' You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/   
    Const SourceFileUrl As String = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf"

    ' Comma-separated list of page indices (Or ranges) to process. Leave empty for all pages. Example '0,2-5,7-'.
    Const Pages As String = ""

    ' PDF document password. Leave empty for unprotected documents.
    Const Password As String = ""

    ' Destination PDF file name
    Const DestinationFile As String = ".\result.pdf"

    ' Text params

    Private Const Type As String = "annotation"
    Private Const Text As String = "Some notes will go here... Some notes will go here.... Some notes will go here....."
    Private Const FontName As String = "Times New Roman"
    Private Const FontSize As Decimal = 12
    Private Const Color As String = "FF0000"

    Sub Main()

        ' Create standard .NET web client instance
        Dim webClient As WebClient = New WebClient()

        ' Set API Key
        webClient.Headers.Add("x-api-key", API_KEY)

        ' Find Text coordinates to add Annotation
        Dim oCoordinates = FindCoordinates(API_KEY, SourceFileUrl, "Notes")

        Dim X As Integer = oCoordinates.x
        Dim Y As Integer = oCoordinates.y + 25

        ' Set JSON content type
        webClient.Headers.Add("Content-Type", "application/json")

        ' * Add text annotations *
        ' Prepare URL for `PDF Edit` API call
		Dim url As String = "https://api.pdf.co/v1/pdf/edit/add"

        ' Prepare requests params as JSON
        ' See documentation: https://apidocs.pdf.co/04-pdf-add-text-signatures-and-images-to-pdf
        Dim jsonPayload As String = $"{{
    ""name"": ""{Path.GetFileName(DestinationFile)}"",
    ""url"": ""{SourceFileUrl}"",
    ""password"": ""{Password}"",
    ""annotations"": [
        {{
            ""text"": ""{Text}"",
            ""x"": {X},
            ""y"": {Y},
            ""fontname"": ""{FontName}"",
            ""size"": ""{FontSize}"",
            ""color"": ""{Color}"",
            ""pages"": ""{Pages}""
        }}
    ]
}}"

        Try
            ' Execute POST request with JSON payload
            Dim response As String = webClient.UploadString(url, jsonPayload)

            ' Parse JSON response
            Dim json As JObject = JObject.Parse(response)

            If json("error").ToObject(Of Boolean) = False Then

                ' Get URL of generated PDF file
                Dim resultFileUrl As String = json("url").ToString()

                ' Download PDF file
                webClient.DownloadFile(resultFileUrl, DestinationFile)

                Console.WriteLine("Generated PDF file saved as ""{0}"" file.", DestinationFile)

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

    ''' <summary>
    ''' Find result coordinates
    ''' </summary>
    Function FindCoordinates(ByVal API_KEY As String, ByVal SourceFileUrl As String, ByVal SearchString As String) As ResultCoOrdinates

        Dim oResult As New ResultCoOrdinates()

        ' Create standard .NET web client instance
        Dim webClient As WebClient = New WebClient()

        ' Set API Key
        webClient.Headers.Add("x-api-key", API_KEY)

        ' Set JSON content type
        webClient.Headers.Add("Content-Type", "application/json")

        ' Prepare URL for PDF text search API call.
        ' See documentation: https://apidocs.pdf.co/07-pdf-search-text
        Dim url As String = "https://api.pdf.co/v1/pdf/find"

        ' Prepare requests params as JSON
        ' See documentation: https : //apidocs.pdf.co
        Dim parameters As New Dictionary(Of String, Object)
		parameters.Add("url", SourceFileUrl)
		parameters.Add("searchString", SearchString)

        ' Convert dictionary of params to JSON
        Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)

        Try
            ' Execute POST request with JSON payload
            Dim response As String = webClient.UploadString(url, jsonPayload)

            ' Parse JSON response
            Dim json As JObject = JObject.Parse(response)

            If json("error").ToObject(Of Boolean) = False Then

                Dim item = json("body")(0)

                oResult.x = item("left")
                oResult.y = item("top")
                oResult.width = item("width")
                oResult.height = item("height")

            End If

        Catch ex As WebException
            Console.WriteLine(ex.ToString())
        End Try

        webClient.Dispose()

        Return oResult

    End Function

    Class ResultCoOrdinates

        Public x As Integer
        Public y As Integer
        Public width As Integer
        Public height As Integer

    End Class


End Module
