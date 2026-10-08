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
Imports System.Threading
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
            parameters.Add("async", True)

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

                ' Asynchronous job ID
                Dim jobId As String = json("jobId").ToString()
                ' URL of generated PDF file that will available after the job completion
                Dim resultFileUrl As String = json("url").ToString()

                ' Check the job status in a loop. 
                ' If you don't want to pause the main thread you can rework the code 
                ' to use a separate thread for the status checking and completion.
                Do
                    Dim status As String = CheckJobStatus(jobId) ' Possible statuses: "working", "failed", "aborted", "success".

                    ' Display timestamp and status (for demo purposes)
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status)

                    If status = "success" Then

                        ' Download PDF file
                        webClient.DownloadFile(resultFileUrl, DestinationFile)

                        Console.WriteLine("Generated PDF file saved as ""{0}"" file.", DestinationFile)
                        Exit Do

                    ElseIf status = "working" Then
                        ' Pause for a few seconds
                        Thread.Sleep(3000)
                    Else
                        Console.WriteLine(status)
                        Exit Do
                    End If
                Loop
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
    ''' Check Async Task Job Status
    ''' </summary>
    Function CheckJobStatus(jobId As String) As String

        Using webClient As WebClient = New WebClient()

            ' Set API Key
            webClient.Headers.Add("x-api-key", API_KEY)

            Dim url As String = "https://api.pdf.co/v1/job/check?jobid=" + jobId

            Dim response As String = webClient.DownloadString(url)
            Dim json As JObject = JObject.Parse(response)

            Return Convert.ToString(json("status"))

        End Using

    End Function


End Module
