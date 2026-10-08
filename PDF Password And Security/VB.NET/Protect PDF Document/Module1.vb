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
	Const API_KEY As String = "*****************************"

	' Direct URL of source PDF file.
    ' You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/   
	Const SourceFileUrl As String = "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf"

	' Destination PDF file name
	Const DestinationFile As String = ".\result.pdf"

	' Passwords to protect PDF document
	' The owner password will be required for document modification.
	' The user password only allows to view And print the document.
	Const OwnerPassword As String = "123456"
	Const UserPassword As String = "654321"

	' Encryption algorithm. 
	' Valid values: "RC4_40bit", "RC4_128bit", "AES_128bit", "AES_256bit".
	Const EncryptionAlgorithm As String = "AES_128bit"

	' Allow Or prohibit content extraction for accessibility needs.
	Const AllowAccessibilitySupport As Boolean = True

	' Allow Or prohibit assembling the document.
	Const AllowAssemblyDocument As Boolean = True

	' Allow Or prohibit printing PDF document.
	Const AllowPrintDocument As Boolean = True

	' Allow Or prohibit filling of interactive form fields (including signature fields) in PDF document.
	Const AllowFillForms As Boolean = True

	' Allow Or prohibit modification of PDF document.
	Const AllowModifyDocument As Boolean = True

	' Allow Or prohibit copying content from PDF document.
	Const AllowContentExtraction As Boolean = True

	' Allow Or prohibit interacting with text annotations And forms in PDF document.
	Const AllowModifyAnnotations As Boolean = True

	' Allowed printing quality.
	' Valid values: "HighResolution", "LowResolution"
	Const PrintQuality As String = "HighResolution"


	Sub Main()

		' Create standard .NET web client instance
		Dim webClient As WebClient = New WebClient()

		' Set API Key
		webClient.Headers.Add("x-api-key", API_KEY)

		' Set JSON content type
		webClient.Headers.Add("Content-Type", "application/json")

		' Prepare URL of "PDF Security" endpoint
		Dim url As String = "https://api.pdf.co/v1/pdf/security/add"

		' Prepare requests params as JSON
		' See documentation: https : //apidocs.pdf.co
		Dim parameters As New Dictionary(Of String, Object)
		parameters.Add("name", Path.GetFileName(DestinationFile))
		parameters.Add("url", SourceFileUrl)
		parameters.Add("ownerPassword", OwnerPassword)
		parameters.Add("userPassword", UserPassword)
		parameters.Add("encryptionAlgorithm", EncryptionAlgorithm)
		parameters.Add("allowAccessibilitySupport", AllowAccessibilitySupport)
		parameters.Add("allowAssemblyDocument", AllowAssemblyDocument)
		parameters.Add("allowPrintDocument", AllowPrintDocument)
		parameters.Add("allowFillForms", AllowFillForms)
		parameters.Add("allowModifyDocument", AllowModifyDocument)
		parameters.Add("allowContentExtraction", AllowContentExtraction)
		parameters.Add("allowModifyAnnotations", AllowModifyAnnotations)
		parameters.Add("printQuality", PrintQuality)

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
