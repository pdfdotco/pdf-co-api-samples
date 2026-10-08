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

	' Source PDF files
	Dim SourceFiles as String() = { ".\sample1.pdf", ".\sample2.pdf" }
	' Destination PDF file name
	const DestinationFile as string = ".\result.pdf"

	Sub Main()

		' Create standard .NET web client instance
		Dim webClient As WebClient = New WebClient()

		' Set API Key
		webClient.Headers.Add("x-api-key", API_KEY)

		' 1. UPLOAD FILES TO CLOUD

		Dim uploadedFiles As List(Of String) = New List(Of String)()

		Try

			For Each pdfFile As String In SourceFiles

				' 1a. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.

				' Prepare URL for `Get Presigned URL` API call
				Dim query As string = Uri.EscapeUriString(string.Format(
					"https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
					Path.GetFileName(pdfFile)))

				' Execute request
				Dim response As string = webClient.DownloadString(query)

				' Parse JSON response
				Dim json As JObject = JObject.Parse(response)

				If json("error").ToObject(Of Boolean) = False Then

					' Get URL to use for the file upload
					Dim uploadUrl As string = json("presignedUrl").ToString()
					' Get URL of uploaded file to use with later API calls
					Dim uploadedFileUrl As string = json("url").ToString()

					' 1b. UPLOAD THE FILE TO CLOUD.

					webClient.Headers.Add("content-type", "application/octet-stream")
					webClient.UploadFile(uploadUrl, "PUT", pdfFile) ' You can use UploadData() instead if your file is byte array or Stream

					uploadedFiles.Add(uploadedFileUrl)

				Else 
					Console.WriteLine(json("message").ToString())
				End If
				
			Next

			if uploadedFiles.Count > 0 Then

				' Set JSON content type
				webClient.Headers.Add("Content-Type", "application/json")

				' 2. MERGE UPLOADED PDF DOCUMENTS

				' Prepare URL for `Merge PDF` API call
				Dim url As String = "https://api.pdf.co/v1/pdf/merge"

				' Prepare requests params as JSON
				' See documentation: https : //apidocs.pdf.co
				Dim parameters As New Dictionary(Of String, Object)
				parameters.Add("name", Path.GetFileName(DestinationFile))
				parameters.Add("url", string.Join(",", uploadedFiles))

				' Convert dictionary of params to JSON
				Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)

				' Execute POST request with JSON payload
				Dim response As String = webClient.UploadString(url, jsonPayload)

				' Parse JSON response
				Dim json As JObject = JObject.Parse(response)

				If json("error").ToObject(Of Boolean) = False Then
				
					' Get URL of generated PDF file
					Dim resultFileUrl As string = json("url").ToString()

					' Download PDF file
					webClient.DownloadFile(resultFileUrl, DestinationFile)

					Console.WriteLine("Generated PDF file saved as ""{0}"" file.", DestinationFile)

				Else 
					Console.WriteLine(json("message").ToString())
				End If

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
