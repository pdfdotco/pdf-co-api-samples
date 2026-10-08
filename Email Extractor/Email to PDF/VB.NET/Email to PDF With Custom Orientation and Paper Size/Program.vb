'*******************************************************************************************'
'                                                                                           '
' Get API Key https://app.pdf.co/signup                                                     '
' API Documentation: https://developer.pdf.co/api/index.html                                '
'                                                                                           '
' Note: Replace placeholder values in the code with your API Key                            '
' and file paths (if applicable)                                                            '
'                                                                      '
'*******************************************************************************************'


Imports System
Imports RestSharp

Namespace HelloWorldApplication

    Class HelloWorld

        Private Shared Sub Main(ByVal args As String())

            Dim client = New RestClient("https://api.pdf.co/v1/pdf/convert/from/email")
            client.Timeout = -1

            Dim request = New RestRequest(Method.POST)
            request.AddHeader("Content-Type", "application/json")
            request.AddHeader("x-api-key", "{{x-api-key}}")

            ' You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/   
            request.AddParameter("application/json", "{" & vbLf & "    ""url"": ""https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/email-to-pdf/sample.eml""," & vbLf & "    ""embedAttachments"": true," & vbLf & "    ""name"": ""email-with-attachments""," & vbLf & "    ""async"": false," & vbLf & "    ""encrypt"": false," & vbLf & "    ""profiles"": ""{\""orientation\"": \""landscape\"", \""paperSize\"": \""letter\"" }""" & vbLf & "}", ParameterType.RequestBody)

            Dim response As IRestResponse = client.Execute(request)

            Console.WriteLine(response.Content)

        End Sub

    End Class

End Namespace
