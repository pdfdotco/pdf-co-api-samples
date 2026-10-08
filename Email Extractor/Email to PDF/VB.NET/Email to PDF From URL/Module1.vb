'*******************************************************************************************'
'                                                                                           '
' Get API Key https://app.pdf.co/signup                                                     '
' API Documentation: https://developer.pdf.co/api/index.html                                '
'                                                                                           '
' Note: Replace placeholder values in the code with your API Key                            '
' and file paths (if applicable)                                                            '
'                                                                      '
'*******************************************************************************************'


Imports System.Net
Imports System.Threading
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


' Cloud API asynchronous "PDF To result" job example.
' Allows to avoid timeout errors when processing huge or scanned PDF documents.

Module Module1

	' The authentication key (API Key).
	' Get your own by registering at https://app.pdf.co
	Const API_KEY As String = "***********************************"

	' Direct URL of source PDF file.
    ' You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/   
	Const SourceFileUrl As String = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/email-to-pdf/sample.eml"

	' Ouput file path
	Const DestinationFile As String = "output.pdf"

	' (!) Make asynchronous job
	Const Async As Boolean = True


	Sub Main()

		' Create standard .NET web client instance
		Dim webClient As WebClient = New WebClient()

		' Set API Key
		webClient.Headers.Add("x-api-key", API_KEY)

		' Set JSON content type
		webClient.Headers.Add("Content-Type", "application/json")

		' Prepare URL for `PDF FROM Email` API call
		Dim url As String = "https://api.pdf.co/v1/pdf/convert/from/email"

		' Prepare requests params as JSON
		' See documentation: https : //apidocs.pdf.co
		Dim parameters As New Dictionary(Of String, Object)

		' Link to input EML Or MSG file to be converted.
		' You can pass link to file from Google Drive, Dropbox Or another online file service that can generate shareable links. 
		' You can also use built-in PDF.co cloud storage located at https:'app.pdf.co/files Or upload your file as temporary file right before making this API call (see Upload And Manage Files section for more details on uploading files via API).
		parameters.Add("url", SourceFileUrl)

		' True by default. 
		' Set to true to automatically embeds all attachments from original input email MSG Or EML fileas files into final output PDF. 
		' Set to false if you don't want to embed attachments so it will convert only the body of input email.
		parameters.Add("embedAttachments", True)

		' true by default. 
		' Converts attachments that are supported by API (doc, docx, html, png, jpg etc) into PDF And merges into output final PDF. 
		' Non-supported file types are added as PDF attachments (Adobe Reader Or another viewer maybe required to view PDF attachments).
		' Set to false if you don't want to convert attachments from original email and want to embed them as original files (as embedded pdf attachments). 
		parameters.Add("convertAttachments", True)

		' Can be Letter, A4, A5, A6 Or custom size Like 200x200
		parameters.Add("paperSize", "Letter")

		' Name of output PDF
		parameters.Add("name", "email-with-attachments")

		' Set to true to run as async job in background (recommended for heavy documents).
		parameters.Add("async", True)

		' Convert dictionary of params to JSON
		Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)

		Try
			' Execute POST request with JSON payload
			Dim response As String = webClient.UploadString(url, jsonPayload)

			' Parse JSON response
			Dim json As JObject = JObject.Parse(response)

			If json("error").ToObject(Of Boolean) = False Then

				' Asynchronous job ID
				Dim jobId As String = json("jobId").ToString()
				' URL of generated result file that will available after the job completion
				Dim resultFileUrl As String = json("url").ToString()

				' Check the job status in a loop. 
				' If you don't want to pause the main thread you can rework the code 
				' to use a separate thread for the status checking and completion.
				Do
					Dim status As String = CheckJobStatus(jobId) ' Possible statuses: "working", "failed", "aborted", "success".

					' Display timestamp and status (for demo purposes)
					Console.WriteLine(DateTime.Now.ToLongTimeString() + ": " + status)

					If status = "success" Then

						' Download result file
						webClient.DownloadFile(resultFileUrl, DestinationFile)

						Console.WriteLine("Generated result file saved as ""{0}"" file.", DestinationFile)
						Exit Do

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

		Catch ex As WebException
			Console.WriteLine(ex.ToString())
		End Try

		webClient.Dispose()


		Console.WriteLine()
		Console.WriteLine("Press any key...")
		Console.ReadKey()

	End Sub

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
