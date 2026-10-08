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
    Const SourceFileUrl As String = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf"

    ' PDF document password. Leave empty for unprotected documents.
    Const Password As String = ""

    ' File name for generated output. Must be a String
    Const FileName As String = "newDocument"

    ' Destination PDF file name
    Const DestinationFile As String = ".\result.pdf"

    Sub Main()

        ' Create standard .NET web client instance
        Dim webClient As WebClient = New WebClient()

        ' Set API Key
        webClient.Headers.Add("x-api-key", API_KEY)

        ' Set JSON content type
        webClient.Headers.Add("Content-Type", "application/json")

        ' Controls to be added
        Dim annotations = New List(Of Object) From {
            New With {Key .text = "sample prefilled text", Key .x = 10, Key .y = 30, Key .size = 12, Key .pages = "0-", Key .type = "TextField", Key .id = "textfield1"},
            New With {Key .x = 100, Key .y = 150, Key .size = 12, Key .pages = "0-", Key .type = "Checkbox", Key .id = "checkbox2"},
            New With {Key .x = 100, Key .y = 170, Key .size = 12, Key .pages = "0-", Key .link = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png", Key .type = "CheckboxChecked", Key .id = "checkbox3"}
        }

        ' If enabled, Runs processing asynchronously. Returns Use JobId that you may use with /job/check to check state of the processing (possible states working,
        Dim async As Boolean = False

        ' Prepare URL of "PDF Edit" endpoint
        Dim url As String = "https://api.pdf.co/v1/pdf/edit/add"

        ' Prepare requests params as JSON
        ' See documentation: https : //apidocs.pdf.co
        Dim parameters As New Dictionary(Of String, Object)
        parameters.Add("name", Path.GetFileName(DestinationFile))
        parameters.Add("password", Password)
        parameters.Add("url", SourceFileUrl)
        parameters.Add("async", async)
        parameters.Add("annotations", annotations)

        ' Convert dictionary of params to JSON
        Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)

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

End Module
