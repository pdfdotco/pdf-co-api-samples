## How to parse document and get CSV output for document parser API in Salesforce with PDF.co Web API PDF.co Web API: the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

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

##### **API.cls:**
    
```
public class API {
    
    public static String  API_KEY = '********************************';
    
    //Get Call
    public static HttpResponse  getCall(String endPointURL)
    {
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        //Set the key in header.
        request.setHeader('x-api-key', API_KEY);
        //Set the URL to invoke
        request.setEndpoint(endPointURL); 
        //Sets the type of method to be used for the HTTP request.
        request.setMethod('GET');
        /*
        The timeout is the maximum time to wait for establishing the HTTP connection.
        The same timeout is used for waiting for the request to start. When the request is executing, 
        such as retrieving or posting data, the connection is kept alive until the request finishes.
        */
        request.setTimeout(60000);
        HttpResponse response =  http.send(request);
        return response;
    }
    
    //Post Call
    public static HttpResponse  postCall(String endPointURL, String body, String contentType)
    {
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        //Set the key in header.
        request.setHeader('x-api-key', API_KEY);
        //Set the URL to invoke
        request.setEndpoint(endPointURL);            
        request.setHeader('Content-Type', contentType);
        //Sets the type of method to be used for the HTTP request.
        request.setMethod('POST');
        //Sets the contents of the body for this request.
        request.setBody(body);
        /*
        The timeout is the maximum time to wait for establishing the HTTP connection.
        The same timeout is used for waiting for the request to start. When the request is executing, 
        such as retrieving or posting data, the connection is kept alive until the request finishes.
        */
        request.setTimeout(60000); 
        HttpResponse response =  http.send(request);
        return response;
    }
    
    //PUT Call for Blob Body
    public static HttpResponse  putCall(String endPointURL, Blob body, String contentType)
    {
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        //Set the key in header.
        request.setHeader('x-api-key', API_KEY);
        //Set the URL to invoke
        request.setEndpoint(endPointURL);            
        request.setHeader('Content-Type', contentType);
        //Sets the type of method to be used for the HTTP request.
        request.setMethod('PUT');
        //Sets the contents of the body for this request using a Blob.
        request.setBodyAsBlob(body);
        /*
        The timeout is the maximum time to wait for establishing the HTTP connection.
        The same timeout is used for waiting for the request to start. When the request is executing, 
        such as retrieving or posting data, the connection is kept alive until the request finishes.
        */
        request.setTimeout(60000);
        HttpResponse response =  http.send(request);
        return response;
    }
}
```

<!-- code block end -->    

<!-- code block begin -->

##### **APITest.cls:**
    
```
@isTest
public class APITest {

    private  testmethod static void getCallTest()
    {
        Test.setMock(HttpCalloutMock.class, new APITest.APIMock());
        Test.StartTest();
        HttpResponse resp = API.getCall('https://www.google.com/');
        Test.stopTest();
        System.assertNotEquals(null, resp);
    }

    private  testmethod static void postCallTest()
    {
        Test.setMock(HttpCalloutMock.class, new APITest.APIMock());
        Test.StartTest();
        HTTPResponse res = API.postCall('https://www.google.com/', 'jsonPayload', 'application/json');
        Test.stopTest();
        System.assertNotEquals(null, res);
    }

    private  testmethod static void putCallTest()
    {
        Test.setMock(HttpCalloutMock.class, new APITest.APIMock());
        Test.StartTest();
        HTTPResponse res = API.putCall('https://www.google.com/', Blob.valueOf('sourceFile'),'application/octet-stream');
        Test.stopTest();
        System.assertNotEquals(null, res);
    }

    public class APIMock implements HttpCalloutMock {
        public HTTPResponse respond(HTTPRequest req) {
            HttpResponse res = new HttpResponse();
            String testBody = '{"presignedUrl":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c72bf56341142ba83c8f98b47f14d62/test.pdf?X-Amz-Expires=900&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIZJDPLX6D7EHVCKA/20200302/us-west-2/s3/aws4_request&X-Amz-Date=20200302T143951Z&X-Amz-SignedHeaders=host&X-Amz-Signature=8650913644b6425ba8d52b78634698e5fc8970157d971a96f0279a64f4ba87fc","url":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c72bf56341142ba83c8f98b47f14d62/test.pdf?X-Amz-Expires=3600&x-amz-security-token=FwoGZXIvYXdzEGgaDA9KaTOXRjkCdCqSTCKBAW9tReCLk1fVTZBH9exl9VIbP8Gfp1pE9hg6et94IBpNamOaBJ6%2B9Vsa5zxfiddlgA%2BxQ4tpd9gprFAxMzjN7UtjU%2B2gf%2FKbUKc2lfV18D2wXKd1FEhC6kkGJVL5UaoFONG%2Fw2jXfLxe3nCfquMEDo12XzcqIQtNFWXjKPWBkQEvmii4tfTyBTIot4Na%2BAUqkLshH0R7HVKlEBV8btqa0ctBjwzwpWkoU%2BF%2BCtnm8Lm4Eg%3D%3D&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=ASIA4NRRSZPHEGHTOA4W/20200302/us-west-2/s3/aws4_request&X-Amz-Date=20200302T143951Z&X-Amz-SignedHeaders=host;x-amz-security-token&X-Amz-Signature=243419ac4a9a315eebc2db72df0817de6a261a684482bbc897f0e7bb5d202bb9","error":false,"status":200,"name":"test.pdf","remainingCredits":98145}';
            res.setHeader('Content-Type', 'application/json');
            res.setBody(testBody);
            res.setStatusCode(200);
            return res;
        }
    }
}

```

<!-- code block end -->    

<!-- code block begin -->

##### **DocumentParserCSV.cls:**
    
```
public class DocumentParserCSV {
    
    public void startProcessing()
    {
        try
        {
            JSONGenerator gen = JSON.createGenerator(true);
            gen.writeStartObject();
            gen.writeStringField('url', 'https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/document-parser/sample-invoice.pdf');
            gen.writeStringField('templateId', '1');
            gen.writeStringField('outputFormat', 'CSV');
            gen.writeStringField('generateCsvHeaders', 'true');
            
            gen.writeBooleanField('async', false);
            gen.writeStringField('encrypt', 'false');
            gen.writeStringField('inline', 'true');
            gen.writeStringField('password', '');
            gen.writeBooleanField('storeResult', false);
            
            gen.writeEndObject();
            
            // Convert dictionary of params to JSON
            String jsonPayload = gen.getAsString();
            String url = 'https://api.pdf.co/v1/pdf/documentparser';
            HttpResponse response =  API.postCall(url, jsonPayload, 'application/json');    
            
            Map<String, Object> json = (Map<String, Object>)JSON.deserializeUntyped(response.getBody());
            
            if(response.getStatusCode() == 200) 
            {
                if ((Boolean)json.get('error') == false)
                {
                    // Get URL of generated PDF file
                    String jsonBody =(String)json.get('body');
                    SYstem.Debug(jsonBody);
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

##### **DocumentParserCSVTest.cls:**
    
```
@isTest
private class DocumentParserCSVTest
{
    private  testmethod static void testDocumentParserCSV()
    {
        Test.setMock(HttpCalloutMock.class, new DocumentParserCSVTest.DocumentCreationMock());
        DocumentParserCSV dc = new DocumentParserCSV();
        Test.startTest();
        dc.startProcessing();
        Test.stopTest();
    }
    
    private  testmethod static void testDocumentParserCSVError()
    {
        DocumentParserCSV dc = new DocumentParserCSV();
        Test.startTest();
        dc.startProcessing();
        Test.stopTest();
    }
    
    public class DocumentCreationMock implements HttpCalloutMock {
        public HTTPResponse respond(HTTPRequest req) {
            HttpResponse res = new HttpResponse();
            String testBody = '{"presignedUrl":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c72bf56341142ba83c8f98b47f14d62/test.pdf?X-Amz-Expires=900&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIZJDPLX6D7EHVCKA/20200302/us-west-2/s3/aws4_request&X-Amz-Date=20200302T143951Z&X-Amz-SignedHeaders=host&X-Amz-Signature=8650913644b6425ba8d52b78634698e5fc8970157d971a96f0279a64f4ba87fc","url":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c72bf56341142ba83c8f98b47f14d62/test.pdf?X-Amz-Expires=3600&x-amz-security-token=FwoGZXIvYXdzEGgaDA9KaTOXRjkCdCqSTCKBAW9tReCLk1fVTZBH9exl9VIbP8Gfp1pE9hg6et94IBpNamOaBJ6%2B9Vsa5zxfiddlgA%2BxQ4tpd9gprFAxMzjN7UtjU%2B2gf%2FKbUKc2lfV18D2wXKd1FEhC6kkGJVL5UaoFONG%2Fw2jXfLxe3nCfquMEDo12XzcqIQtNFWXjKPWBkQEvmii4tfTyBTIot4Na%2BAUqkLshH0R7HVKlEBV8btqa0ctBjwzwpWkoU%2BF%2BCtnm8Lm4Eg%3D%3D&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=ASIA4NRRSZPHEGHTOA4W/20200302/us-west-2/s3/aws4_request&X-Amz-Date=20200302T143951Z&X-Amz-SignedHeaders=host;x-amz-security-token&X-Amz-Signature=243419ac4a9a315eebc2db72df0817de6a261a684482bbc897f0e7bb5d202bb9","error":false,"status":200,"name":"test.pdf","remainingCredits":98145}';
            res.setHeader('Content-Type', 'application/json');
            res.setBody(testBody);
            res.setStatusCode(200);
            return res;
        }
    }
}
```

<!-- code block end -->