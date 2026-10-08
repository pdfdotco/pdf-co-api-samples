## document parser API in Salesforce using PDF.co Web API PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

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

##### **DocumentParserOutputAsJSON.cls:**
    
```
public class DocumentParserOutputAsJSON {
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    static String API_KEY = '***********************';
    
    // Direct URL of source PDF file.
    static string SourceFileUrl = 'https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/document-parser/sample-invoice.pdf';
    // PDF document password. Leave empty for unprotected documents.
    static string Password = '';

    @TestVisible
    String jsonOutput;
    public void parseDocumentAsJSON()
    {        
        try
        {
            // Create HTTP client instance
            Http http = new Http();
            HttpRequest request = new HttpRequest();
            // Set API Key
            request.setHeader('x-api-key', API_KEY);
            
            Boolean async = false;
            String inline = 'true';
            String profiles = '';
            Boolean encrypt = false;
            Boolean storeResult = false;
            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co
            // Create JSON payload
            JSONGenerator gen = JSON.createGenerator(true);
            gen.writeStartObject();
            gen.writeStringField('url', SourceFileUrl);
            gen.writeStringField('outputFormat', 'JSON');
            gen.writeStringField('templateId', '1');
            gen.writeBooleanField('async', async);
            gen.writeBooleanField('encrypt', encrypt);
            gen.writeStringField('inline', inline);
            gen.writeStringField('password', password);
            gen.writeStringField('profiles', profiles);
            gen.writeBooleanField('storeResult', false);
            
            gen.writeEndObject();
            // Convert dictionary of params to JSON
            String jsonPayload = gen.getAsString();
        
            // URL of 'PDF Edit' endpoint
            string url = 'https://api.pdf.co/v1/pdf/documentparser';
            request.setEndpoint(url);            
            request.setHeader('Content-Type', 'application/json;charset=UTF-8');
            request.setMethod('POST');
            request.setBody(jsonPayload);
            // Execute request
            HttpResponse response =  http.send(request);                
            
            if(response.getStatusCode() == 200)
            {
                // Parse JSON response
                Map<String, Object> json = (Map<String, Object>)JSON.deserializeUntyped(response.getBody());
                if ((Boolean)json.get('error') == false)
                {
                    jsonOutput = response.getBody();
                    System.debug('JSON '+ jsonOutput);    
                }
            }
            else
            {
                System.debug('Error Response ' + response.getBody());
                System.Debug(' Status ' + response.getStatus());
                System.Debug(' Status Code' + response.getStatusCode());
                System.Debug(' Response String' + response.toString());
            }
        }
        catch (Exception ex)
        {
            String errorBody = 'Message: ' + ex.getMessage() + ' -- Cause: ' + ex.getCause() + ' -- Stacktrace: ' + ex.getStackTraceString();
            System.Debug(errorBody);
        }
    }
}
```

<!-- code block end -->    

<!-- code block begin -->

##### **DocumentParserOutputAsJSONTest.cls:**
    
```
@isTest
public class DocumentParserOutputAsJSONTest
{
    private  testmethod static void testparseDocumentAsJSON()
    {
        Test.setMock(HttpCalloutMock.class, new DocumentParserOutputAsJSONTest.DocumentCreationMock());
        DocumentParserOutputAsJSON docParse = new DocumentParserOutputAsJSON();
        docParse.parseDocumentAsJSON();
        System.assert(docParse.jsonOutput.contains('John A. Doe'));
    }

    private  testmethod static void testparseDocumentAsJSONException()
    {
        DocumentParserOutputAsJSON docParse = new DocumentParserOutputAsJSON();
        docParse.parseDocumentAsJSON();
        System.assert(null == docParse.jsonOutput);
    }    
   
    public class DocumentCreationMock implements HttpCalloutMock {
        public HTTPResponse respond(HTTPRequest req) {
            HttpResponse res = new HttpResponse();
            String testBody = '{"hash":"John A. Doe","url":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c336bfcef1a473d98492bda25d8da03/newDocument.pdf?X-Amz-Expires=3600&x-amz-security-token=FwoGZXIvYXdzEO7%2F%2F%2F%2F%2F%2F%2F%2F%2F%2FwEaDHWK1dY4d4lOgsheliKBATwE%2FZewASPTEnPxTn%2BOdYhP4h3gljAJfqbRvQptDX7wdWLmrBS7Tg4qTU6pAbxIdXChGPjBWpSbtiADJKmqkmyhkUmE8GSM1%2FGtJO6bga2pgzvFLXmzxjTf3%2BFNqwYOvbyApIZdVLoPpEKY6PlCflQtLTd30dhelm6xpB8pitbdhSjdz8KCBjIobVy%2Fjwybwp6OQgB%2FT6QkIo2dU07gtFREdn5jhRyvnS5lkccweBV1%2Bw%3D%3D&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=ASIA4NRRSZPHMV5P3JOS/20210316/us-west-2/s3/aws4_request&X-Amz-Date=20210316T124309Z&X-Amz-SignedHeaders=host;x-amz-security-token&X-Amz-Signature=95287bf3c007fed4c2c5aeea1ce75c846cc6c68b22aaf35175ebe41a105f54e1","pageCount":1,"error":false,"status":200,"name":"newDocument","remainingCredits":9913694,"credits":3}';
            res.setHeader('Content-Type', 'application/json');
            res.setBody(testBody);
            res.setStatusCode(200);
            return res;
        }
    }
}
```

<!-- code block end -->