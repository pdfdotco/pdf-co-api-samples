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

    ' HTML input
    Dim inputSample As String = File.ReadAllText(".\sample.html")

    ' Destination PDF file name
    Const DestinationFile As String = ".\result.pdf"

    Sub Main()

        ' Create standard .NET web client instance
        Dim webClient As WebClient = New WebClient()

        ' Set API Key
        webClient.Headers.Add("x-api-key", API_KEY)

        ' Set JSON content type
        webClient.Headers.Add("Content-Type", "application/json")

        Try

            ' Prepare requests params as JSON
            ' See documentation: https : //apidocs.pdf.co/?#1-json-pdfconvertfromhtml
            Dim parameters As New Dictionary(Of String, Object)


            ' Input HTML code to be converted. Required. 
            parameters.Add("html", inputSample)

            ' Name of resulting file
            parameters.Add("name", Path.GetFileName(DestinationFile))

            ' Set to css style margins Like 10 px Or 5px 5px 5px 5px.
            parameters.Add("margins", "5px 5px 5px 5px")

            ' Can be Letter, A4, A5, A6 Or custom size Like 200x200
            parameters.Add("paperSize", "Letter")

            ' Set to Portrait Or Landscape. Portrait by default.
            parameters.Add("orientation", "Portrait")

            ' true by default. Set to false to disbale printing of background.
            parameters.Add("printBackground", True)

            ' If large input document, process in async mode by passing true
            parameters.Add("async", False)

            ' Set to HTML for header to be applied on every page at the header.
            parameters.Add("header", "")

            ' Set to HTML for footer to be applied on every page at the bottom.
            parameters.Add("footer", "")


            ' Convert dictionary of params to JSON
            Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)


            ' Prepare URL for `HTML to PDF` API call
            Dim url As String = "https://api.pdf.co/v1/pdf/convert/from/html"

            ' Execute POST request with JSON payload
            Dim response As String = webClient.UploadString(url, jsonPayload)

            ' Parse JSON response
            Dim json As JObject = JObject.Parse(response)

            If json("error").ToObject(Of Boolean) = False Then

                ' Get URL of generated PDF file
                Dim resultFileUrl As String = json("url").ToString()

                webClient.Headers.Remove("Content-Type") ' remove the header required for only the previous request

                ' Download the PDF file
                webClient.DownloadFile(resultFileUrl, DestinationFile)

                Console.WriteLine("Generated PDF document saved as ""{0}"" file.", DestinationFile)

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
