## email extractor in VB.NET using PDF.co Web API PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://bytescout.zendesk.com/hc/en-us/requests/new?subject=PDF.co%20Web%20API%20Question)

or just send email to [support@bytescout.com](mailto:support@bytescout.com?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)
[Sign Up For Online Training](https://academy.bytescout.com/)


## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://apidocs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **Program.vb:**
    
```
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

            request.AddParameter("application/json", "{" & vbLf & "    ""url"": ""https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/email-to-pdf/sample.eml""," & vbLf & "    ""embedAttachments"": true," & vbLf & "    ""name"": ""email-with-attachments""," & vbLf & "    ""async"": false," & vbLf & "    ""encrypt"": false," & vbLf & "    ""profiles"": ""{\""orientation\"": \""landscape\"", \""paperSize\"": \""letter\"" }""" & vbLf & "}", ParameterType.RequestBody)

            Dim response As IRestResponse = client.Execute(request)

            Console.WriteLine(response.Content)

        End Sub

    End Class

End Namespace
```

<!-- code block end -->