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

	' Result file name
	Const ResultFileName As String = ".\barcode.png"
	' Barcode type. See valid barcode types in the documentation https://apidocs.pdf.co/#barcode-generator
	Const BarcodeType As String = "QRCode"
	' Barcode value
	Const BarcodeValue As String = "QR123456
https://pdf.co
https://bytescout.com"

	Sub Main()

		' Create standard .NET web client instance
		Dim webClient As WebClient = New WebClient()

		' Set API Key
		webClient.Headers.Add("x-api-key", API_KEY)

        ' Set JSON content type
        webClient.Headers.Add("Content-Type", "application/json")

		' Prepare URL for `Barcode Generator` API call
		Dim url As String = "https://api.pdf.co/v1/barcode/generate"


		' Valid error correction levels:
		' ----------------------------------
		' Low -[default] Lowest error correction level. (Approx. 7% of codewords can be restored).
		' Medium -Medium error correction level. (Approx. 15% of codewords can be restored).
		' Quarter -Quarter error correction level (Approx. 25% of codewords can be restored).
		' High -Highest error correction level (Approx. 30% of codewords can be restored).

		' Set "Custom Profiles" parameter
		Dim Profiles As String = "{ ""profiles"": [ { ""profile1"": { ""Options.QRErrorCorrectionLevel"": ""Quarter"" } } ] }"


		' Prepare requests params as JSON
		' See documentation: https : //apidocs.pdf.co
		Dim parameters As New Dictionary(Of String, Object)
		parameters.Add("name", Path.GetFileName(ResultFileName))
		parameters.Add("type", BarcodeType)
		parameters.Add("value", BarcodeValue)
		parameters.Add("profiles", Profiles)

		' Convert dictionary of params to JSON
		Dim jsonPayload As String = JsonConvert.SerializeObject(parameters)

		Try
			' Execute POST request with JSON payload
			Dim response As String = webClient.UploadString(url, jsonPayload)

			' Parse JSON response
			Dim json As JObject = JObject.Parse(response)

			If json("error").ToObject(Of Boolean) = False Then
				
				' Get URL of generated barcode image file
				Dim resultFileURI As string = json("url").ToString()

				' Download the image file
				webClient.DownloadFile(resultFileURI, ResultFileName)

				Console.WriteLine("Generated barcode saved to ""{0}"" file.", ResultFileName)

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
