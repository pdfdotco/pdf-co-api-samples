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

	' --TemplateID--
	' Please follow below steps to create your own HTML Template and get "templateId". 
	' 1. Add new html template in app.pdf.co/templates/html
	' 2. Copy paste your html template code into this new template. Sample HTML templates can be found at "https://github.com/bytescout/pdf-co-api-samples/tree/master/PDF%20from%20HTML%20template/TEMPLATES-SAMPLES"
	' 3. Save this new template
	' 4. Copy it’s ID to clipboard
	' 5. Now set ID of the template into “templateId” parameter

	' HTML template using built-in template
	' see https://app.pdf.co/templates/html/3/edit
	Dim template_id = 3

	' Data to fill the template
	Dim templateData As String = File.ReadAllText(".\invoice_data.json")
	' Destination PDF file name
	Const DestinationFile As String = ".\result.pdf"

	Sub Main()

		' Create standard .NET web client instance
		Dim webClient As WebClient = New WebClient()

		' Set API Key
		webClient.Headers.Add("x-api-key", API_KEY)

		Try
			' Prepare URL for HTML to PDF API call
			Dim request As String = Uri.EscapeUriString(String.Format(
				"https://api.pdf.co/v1/pdf/convert/from/html?name={0}",
				Path.GetFileName(DestinationFile)))

			' Prepare request body in JSON format
			Dim jsonObject As JObject = New JObject(
				New JProperty("templateId", template_id),
				New JProperty("templateData", templateData))

			webClient.Headers.Add("Content-Type", "application/json")

			' Execute request
			Dim response As String = webClient.UploadString(request, jsonObject.ToString())

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
