## How to convert document to PDF for DOC to PDF API in Salesforce and PDF.co Web API PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

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

##### **DOCTOPDFConvertor.cls:**
    
```
public class DOCTOPDFConvertor {
    
    String API_KEY = '"*****************************************';
    @TestVisible
    String fileName = 'sample'; // File name "sample.docx" which is available in Files.
    string DestinationFile =  'result.pdf'; //This is the Desitantion File Name.
    public void convertDocxToPdf()
    {
        ContentVersion cv = [select Title, VersionData from ContentVersion where Title = :fileName limit 1];
        Blob SourceFile  = cv.VersionData;
        
        try
        {
            //1. Prepare URL for "Get Presigned URL" API call
            string url = 'https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=:fileName'; 
            HttpRequest req = new HttpRequest();
            req.setHeader('x-api-key', API_KEY);
            req.setEndpoint(url);
            req.setMethod('GET');
            req.setTimeout(60000);
            Http http = new Http();
            HTTPResponse res = http.send(req);
            if(res.getStatusCode() == 200) 
            {
                System.Debug('res ' + res);
                Map<String, Object> deserializedBody =  (Map<String, Object>)JSON.deserializeUntyped(res.getBody());
                Boolean isError = Boolean.ValueOf(deserializedBody.get('error'));
                if(isError == false)
                {
                    // Get URL to use for the file upload
                    String uploadUrl = String.ValueOf(deserializedBody.get('presignedUrl'));
                    // Get URL of uploaded file to use with later API calls
                    String uploadedFileUrl = String.ValueOf(deserializedBody.get('url'));
                    // 2. UPLOAD THE FILE TO CLOUD.
                    if(uploadFile(API_KEY, uploadUrl, SourceFile))
                    {
                        // 3. CONVERT UPLOADED DOC (DOCX) FILE TO PDF and download.
                        ConvertDocToPdf(API_KEY, DestinationFile, uploadedFileUrl);
                    }
                }
            }
            else
            {
                System.debug('Error Response ' + res.getBody());
                System.Debug(' Status ' + res.getStatus());
                System.Debug(' Status Code' + res.getStatusCode());
                System.Debug(' Response String' + res.toString());
            }
            
        }
        catch(Exception ex)
        {
            String errorBody = 'Message: ' + ex.getMessage() + ' -- Cause: ' + ex.getCause() + ' -- Stacktrace: ' + ex.getStackTraceString();
            System.Debug(errorBody);
        }
    }
    
    @TestVisible
    public static boolean uploadFile(String API_KEY, String url, Blob sourceFile)
    {
        HttpRequest req = new HttpRequest();
        req.setHeader('x-api-key', API_KEY);
        req.setHeader('Content-Type', 'application/octet-stream');
        req.setEndpoint(url);
        req.setMethod('PUT');
        req.setTimeout(60000);
        req.setBodyAsBlob(sourceFile);
        Http http = new Http();
        HTTPResponse res = http.send(req);
        if(res.getStatusCode() == 200) 
        {
            System.Debug(res);
            return true;
        }
        else
        {
            System.debug('Error Response ' + res.getBody());
            System.Debug(' Status ' + res.getStatus());
            System.Debug(' Status Code' + res.getStatusCode());
            System.Debug(' Response String' + res.toString());
            return false;
        }
    }
    
    public static void ConvertDocToPdf(String API_KEY, String DestinationFile, String uploadedFileUrl)
    {
        Map<string, object> parameters = new Map<string, object>();
        parameters.put('name', DestinationFile);
        parameters.put('url', uploadedFileUrl);
        string jsonPayload = Json.serialize(parameters);
        String url = 'https://api.pdf.co/v1/pdf/convert/from/doc';
        
        HttpRequest req = new HttpRequest();
        req.setHeader('x-api-key', API_KEY);
        req.setHeader('Content-Type', 'application/json');
        req.setEndpoint(url);
        req.setMethod('POST');
        req.setTimeout(60000);
        req.setBody(jsonPayload);
        Http http = new Http();
        HTTPResponse res = http.send(req);
        if(res.getStatusCode() == 200) 
        {
            System.Debug(res);
            Map<String, Object> deserializedBody =  (Map<String, Object>)JSON.deserializeUntyped(res.getBody());
            Boolean isError = Boolean.ValueOf(deserializedBody.get('error'));
            if(isError == false)
            {
                String resultFileUrl = String.ValueOf(deserializedBody.get('url'));
                downloadPDFAndStore(resultFileUrl, DestinationFile);
            }
        }
        else
        {
            System.debug('Error Response ' + res.getBody());
            System.Debug(' Status ' + res.getStatus());
            System.Debug(' Status Code' + res.getStatusCode());
            System.Debug(' Response String' + res.toString());
        }
    }
    
    @TestVisible
    private static void downloadPDFAndStore(String extFileUrl, String DestinationFile)
    {
        Http h = new Http(); 
        HttpRequest req = new HttpRequest(); 
        extFileUrl = extFileUrl.replace(' ', '%20'); 
        req.setEndpoint(extFileUrl); 
        req.setMethod('GET'); 
        req.setHeader('Content-Type', 'application/pdf');
        req.setCompressed(true); 
        req.setTimeout(60000); 
        //Now Send HTTP Request
        HttpResponse res  = h.send(req); 
        if(res.getStatusCode() == 200) 
        {
            blob fileContent = res.getBodyAsBlob();
            ContentVersion conVer = new ContentVersion();
            conVer.ContentLocation = 'S'; // to use S specify this document is in Salesforce, to use E for external files
            conVer.PathOnClient = DestinationFile + '.pdf'; // The files name, extension is very important here which will help the file in preview.
            conVer.Title = DestinationFile; // Display name of the files
            conVer.VersionData = fileContent;
            insert conVer;
            System.Debug('Success');
        }
        else
        {
            System.debug('Error Response ' + res.getBody());
            System.Debug(' Status ' + res.getStatus());
            System.Debug(' Status Code' + res.getStatusCode());
            System.Debug(' Response String' + res.toString());
        }
    }
}
```

<!-- code block end -->    

<!-- code block begin -->

##### **DOCTOPDFConvertorTest.cls:**
    
```
@isTest
private class DOCTOPDFConvertorTest
{
    @TestSetup
    static void makeData(){
        ContentVersion conVer = new ContentVersion();
        conVer.ContentLocation = 'S'; // to use S specify this document is in Salesforce, to use E for external files
        conVer.PathOnClient = 'DestinationFile.pdf'; // The files name, extension is very important here which will help the file in preview.
        conVer.Title = 'Sample'; // Display name of the files
        conVer.VersionData = Blob.ValueOf('fileContent');
        insert conVer;
    }
    
    private  testmethod static void testconvertDocxToPdf()
    {
        Test.setMock(HttpCalloutMock.class, new DOCTOPDFConvertorTest.DocumentCreationMock());
        DOCTOPDFConvertor dc = new DOCTOPDFConvertor();
        dc.fileName = 'Sample';
        Test.startTest();
        dc.convertDocxToPdf();
        Test.stopTest();
        List<ContentVersion> cv = [select Id from ContentVersion];
        System.assertEquals(2, cv.size());
    }
    
    private  testmethod static void testuploadFile()
    {
        Test.setMock(HttpCalloutMock.class, new DOCTOPDFConvertorTest.DocumentCreationErrorMock());
        Test.startTest();
        Boolean result = DOCTOPDFConvertor.uploadFile('abc', 'https://www.google.com', Blob.ValueOf('fileContent'));
        Test.stopTest();
        System.assertEquals(false, result);
    }
    
    private  testmethod static void testconvertDocxToPdfError()
    {
        DOCTOPDFConvertor dc = new DOCTOPDFConvertor();
        dc.fileName = 'Sample';
        Test.startTest();
        dc.convertDocxToPdf();
        Test.stopTest();
        List<ContentVersion> cv = [select Id from ContentVersion];
        System.assertEquals(1, cv.size());
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
    
    public class DocumentCreationErrorMock implements HttpCalloutMock {
        public HTTPResponse respond(HTTPRequest req) {
            HttpResponse res = new HttpResponse();
            String testBody = '{"presignedUrl":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c72bf56341142ba83c8f98b47f14d62/test.pdf?X-Amz-Expires=900&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIZJDPLX6D7EHVCKA/20200302/us-west-2/s3/aws4_request&X-Amz-Date=20200302T143951Z&X-Amz-SignedHeaders=host&X-Amz-Signature=8650913644b6425ba8d52b78634698e5fc8970157d971a96f0279a64f4ba87fc","url":"https://pdf-temp-files.s3-us-west-2.amazonaws.com/0c72bf56341142ba83c8f98b47f14d62/test.pdf?X-Amz-Expires=3600&x-amz-security-token=FwoGZXIvYXdzEGgaDA9KaTOXRjkCdCqSTCKBAW9tReCLk1fVTZBH9exl9VIbP8Gfp1pE9hg6et94IBpNamOaBJ6%2B9Vsa5zxfiddlgA%2BxQ4tpd9gprFAxMzjN7UtjU%2B2gf%2FKbUKc2lfV18D2wXKd1FEhC6kkGJVL5UaoFONG%2Fw2jXfLxe3nCfquMEDo12XzcqIQtNFWXjKPWBkQEvmii4tfTyBTIot4Na%2BAUqkLshH0R7HVKlEBV8btqa0ctBjwzwpWkoU%2BF%2BCtnm8Lm4Eg%3D%3D&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=ASIA4NRRSZPHEGHTOA4W/20200302/us-west-2/s3/aws4_request&X-Amz-Date=20200302T143951Z&X-Amz-SignedHeaders=host;x-amz-security-token&X-Amz-Signature=243419ac4a9a315eebc2db72df0817de6a261a684482bbc897f0e7bb5d202bb9","error":false,"status":200,"name":"test.pdf","remainingCredits":98145}';
            res.setHeader('Content-Type', 'application/json');
            res.setBody(testBody);
            res.setStatusCode(201);
            return res;
        }
    }
}
```

<!-- code block end -->