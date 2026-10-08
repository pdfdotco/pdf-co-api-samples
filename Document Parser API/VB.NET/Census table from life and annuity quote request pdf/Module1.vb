'*******************************************************************************************'
'                                                                                           '
' Get API Key https://app.pdf.co/signup                                                     '
' API Documentation: https://developer.pdf.co/api/index.html                                '
'                                                                                           '
' Note: Replace placeholder values in the code with your API Key                            '
' and file paths (if applicable)                                                            '
'                                                                      '
'*******************************************************************************************'


Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports Newtonsoft.Json.Linq

Module Module1

    ' The authentication key (API Key).
    ' Get your own by registering at https://app.pdf.co
    Const API_KEY As String = "***************************************"

    ' Sample document containing life and annuity quote request form
    Const SourceFile As String = ".\SampleGroupDisabilityForm.pdf"

    ' PDF document password. Leave empty for unprotected documents.
    Const Password As String = ""

    ' Destination TXT file name
    Const DestinationFile As String = ".\result.json"

    ' (!) Make asynchronous job
    Const Async As Boolean = True

    Sub Main()

        ' Sample template
        Dim templateText As String = File.ReadAllText("SampleGroupDisabilityForm.yml")

        If ProcessAndGetJsonFields(templateText) Then

            ' Get json strings
            Dim jsonString As String = File.ReadAllText(DestinationFile)

            ' Parse to Json Fields
            Dim oRet = ParseJsonFields(jsonString)

            ' Display some of data to console
            Console.WriteLine($"Parsing Details:{Environment.NewLine}------------------------")

            Console.WriteLine($"Contact Person: {oRet.ContactPerson}")
            Console.WriteLine($"Business Name: {oRet.BusinessName}")
            Console.WriteLine($"Business Address: {oRet.BusinessAddress}")
            Console.WriteLine($"Business Type: {oRet.BusinessType}")
            Console.WriteLine($"Phone: {oRet.Phone}")
            Console.WriteLine($"Email: {oRet.Email}")

            ' Export list of census to csv format
            Dim csvOutputFile As String = "result.csv"

            Dim csvString = ConvertToCsv(oRet.lstCensusTable)
            File.WriteAllText(csvOutputFile, csvString)

            Console.WriteLine($"{Environment.NewLine}{csvOutputFile} generated successfully!")

        End If

        Console.WriteLine()
        Console.WriteLine("Press any key...")
        Console.ReadKey()

    End Sub

    ''' <summary>
    '''  Process Input File and Get Json Fields
    ''' </summary>
    Function ProcessAndGetJsonFields(ByVal templateText As String) As Boolean

        ' Create standard .NET web client instance
        Dim webClient As WebClient = New WebClient()

        ' Set API Key
        webClient.Headers.Add("x-api-key", API_KEY)

        ' 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
        ' * If you already have a direct file URL, skip to the step 3.

        ' Prepare URL for `Get Presigned URL` API call
        Dim query As String = Uri.EscapeUriString(String.Format(
            "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
            Path.GetFileName(SourceFile)))

        Try
            ' Execute request
            Dim response As String = webClient.DownloadString(query)

            ' Parse JSON response
            Dim json As JObject = JObject.Parse(response)

            If json("error").ToObject(Of Boolean) = False Then
                ' Get URL to use for the file upload
                Dim uploadUrl As String = json("presignedUrl").ToString()
                ' Get URL of uploaded file to use with later API calls
                Dim uploadedFileUrl As String = json("url").ToString()

                ' 2. UPLOAD THE FILE TO CLOUD.
                webClient.Headers.Add("content-type", "application/octet-stream")
                webClient.UploadFile(uploadUrl, "PUT", SourceFile) ' You can use UploadData() instead if your file is byte array or Stream

                ' 3. PARSE UPLOADED PDF DOCUMENT

                ' URL for `Document Parser` API call
                query = Uri.EscapeUriString(
                    String.Format("https://api.pdf.co/v1/pdf/documentparser?url={0}&async={1}", uploadedFileUrl, Async)
                    )

                Dim requestBody As New NameValueCollection()
                requestBody.Add("template", templateText)

                ' Execute request
                Dim responseBytes As Byte() = webClient.UploadValues(query, "POST", requestBody)
                response = Encoding.UTF8.GetString(responseBytes)

                ' Parse JSON response
                json = JObject.Parse(response)

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

                            Console.WriteLine("Generated JSON file saved as ""{0}"" file.", DestinationFile)

                            Return True

                        ElseIf status = "working" Then

                            ' Pause for a few seconds
                            Thread.Sleep(3000)

                        Else

                            Console.WriteLine(status)
                            Exit Do

                        End If

                    Loop

                Else
                    Console.WriteLine(json("message").ToString())
                End If

            End If

        Catch ex As WebException
            Console.WriteLine(ex.ToString())
        End Try

        webClient.Dispose()

        Return False

    End Function

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

    ''' <summary>
    ''' Parse Json Fields
    ''' </summary>
    Function ParseJsonFields(ByVal jsonInput As String) As FormVM

        Dim jsonObj As JObject = JObject.Parse(jsonInput)

        Dim oRet As New FormVM()

        oRet.ContactPerson = jsonObj.TraverseJObject("ContactPerson", "field")?.Value(Of String)("value")
        oRet.BusinessName = jsonObj.TraverseJObject("BusinessName", "field")?.Value(Of String)("value")
        oRet.BusinessAddress = jsonObj.TraverseJObject("BusinessAddress", "field")?.Value(Of String)("value")
        oRet.BusinessType = jsonObj.TraverseJObject("BusinessType", "field")?.Value(Of String)("value")
        oRet.Phone = jsonObj.TraverseJObject("Phone", "field")?.Value(Of String)("value")
        oRet.Email = jsonObj.TraverseJObject("Email", "field")?.Value(Of String)("value")

        Dim censusTable = jsonObj.TraverseJObject("CencusTable", "table")

        If Not censusTable Is Nothing Then

            Dim rows = censusTable("rows")

            If Not rows Is Nothing And rows.Count() > 0 Then

                For i As Int32 = 1 To rows.Count() - 1
                    Dim oCensus As New CensusTableVm()

                    ' Parse individual data
                    oCensus.SrNo = rows.ElementAt(i).ElementAt(0).ElementAt(0).Value(Of String)("value")
                    oCensus.DOB = rows.ElementAt(i).ElementAt(1).ElementAt(0).Value(Of String)("value")
                    oCensus.Gender = rows.ElementAt(i).ElementAt(2).ElementAt(0).Value(Of String)("value")
                    oCensus.Occupation = rows.ElementAt(i).ElementAt(3).ElementAt(0).Value(Of String)("value")
                    oCensus.Salary = rows.ElementAt(i).ElementAt(4).ElementAt(0).Value(Of String)("value")
                    oCensus.IsShortTermDisability = rows.ElementAt(i).ElementAt(5).ElementAt(0).Value(Of String)("value")
                    oCensus.IsLongTernDisability = rows.ElementAt(i).ElementAt(6).ElementAt(0).Value(Of String)("value")
                    oCensus.LifeInsuCoverAmt = rows.ElementAt(i).ElementAt(7).ElementAt(0).Value(Of String)("value")

                    oRet.lstCensusTable.Add(oCensus)
                Next

            End If

        End If

        Return oRet

    End Function

    ''' <summary>
    ''' Convert to Csv Format
    ''' </summary>
    Function ConvertToCsv(ByVal lstCensusTable As List(Of CensusTableVm)) As String

        Dim oRet As New StringBuilder(String.Empty)

        ' Get Header Row
        oRet.AppendLine(getCsvTitleRow())

        ' Put Child Rows
        For Each item In lstCensusTable
            oRet.AppendLine(item.ToString())
        Next

        Return oRet.ToString()

    End Function

    Class FormVM

        Public Property ContactPerson As String
        Public Property BusinessName As String
        Public Property BusinessAddress As String
        Public Property BusinessType As String
        Public Property Phone As String
        Public Property Email As String

        Public Property lstCensusTable As List(Of CensusTableVm) = New List(Of CensusTableVm)()

    End Class

    Class CensusTableVm

        Public Property SrNo As String
        Public Property DOB As String
        Public Property Gender As String
        Public Property Occupation As String
        Public Property Salary As String
        Public Property IsShortTermDisability As String
        Public Property IsLongTernDisability As String
        Public Property LifeInsuCoverAmt As String

        Public Overrides Function ToString() As String
            Return $"{SrNo},{DOB},{Gender},{Occupation},{Salary},{IsShortTermDisability},{IsLongTernDisability},{LifeInsuCoverAmt}"
        End Function

    End Class

    Public Function getCsvTitleRow() As String
        Return "SrNo,DOB,Gender,Occupation,Salary,IsShortTermDisability,IsLongTermDisability,LifeInsuCoverAmt"
    End Function

End Module

''' <summary>
''' Extension method container for JObject
''' </summary>
Module JObjectExtensionMethods

    <Extension()>
    Public Function TraverseJObject(ByVal jsonObj As JObject, ByVal propertyName As String, ByVal objectType As String) As JToken
        Return jsonObj("objects").Where(Function(x) x.Value(Of String)("name").Equals(propertyName) AndAlso x.Value(Of String)("objectType").Equals(objectType)).FirstOrDefault()
    End Function

End Module
