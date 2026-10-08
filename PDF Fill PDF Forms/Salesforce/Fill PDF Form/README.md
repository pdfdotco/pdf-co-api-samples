## PDF fill PDF forms in Salesforce with PDF.co Web API What is PDF.co Web API? It is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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

##### **FillPDFFormSimplified.cls:**
    
```
public class FillPDFFormSimplified{
    
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    String API_KEY = 'ABCDE';
    // Destination File Name
    string DestinationFile = 'result.pdf';
    
    // Direct URL of source PDF file.
    string SourceFileUrl = 'https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf';
    // PDF document password. Leave empty for unprotected documents.
    string Password = '';
    // File name for generated output. Must be a String
    string FileName = 'f1040-form-filled';
    
    public void fillPDFForm()
    {
        
        try
        {
            // Create HTTP client instance
            Http http = new Http();
            HttpRequest request = new HttpRequest();
            // Set API Key
            request.setHeader('x-api-key', API_KEY);
            
            // Values to fill out pdf fields with built-in pdf form filler
            // To fill fields in PDF form, use the following format page;fieldName;value for example: 0;editbox1;text is here. 
            // To fill checkbox, use true, for example: 0;checkbox1;true. 
            // To separate multiple objects, use | separator. 
            // To get the list of all fillable fields in PDF form please use /pdf/info/fields endpoint.
            String fieldString = '1;topmostSubform[0].Page1[0].f1_02[0];John A. Doe|1;topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1];true|1;topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0];123456789';
            
            // If enabled, Runs processing asynchronously. Returns Use JobId that you may use with /job/check to check state of the processing (possible states: working, 
            Boolean async = false;
            
            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co
            // Create JSON payload
            JSONGenerator gen = JSON.createGenerator(true);
            gen.writeStartObject();
            gen.writeStringField('url', SourceFileUrl);
            gen.writeStringField('name', FileName);
            gen.writeBooleanField('async', async);
            gen.writeStringField('password', password);
            gen.writeBooleanField('encrypt', false);
            gen.writeStringField('fieldsString', fieldString);
            gen.writeEndObject();
            // Convert dictionary of params to JSON
            String jsonPayload = gen.getAsString();
            
            // URL of 'PDF Edit' endpoint
            string url = 'https://api.pdf.co/v1/pdf/edit/add';
            request.setEndpoint(url);            
            request.setHeader('Content-Type', 'application/json;charset=UTF-8');
            request.setMethod('POST');
            request.setBody(jsonPayload);
            // Execute request
            HttpResponse response =  http.send(request);                
            
            // Parse JSON response
            Map<String, Object> json = (Map<String, Object>)JSON.deserializeUntyped(response.getBody());
            
            if(response.getStatusCode() == 200) 
            {
                if ((Boolean)json.get('error') == false)
                {
                    // Get URL of generated PDF file
                    String resultFileUrl =(String)json.get('url');
                    
                    // Download generated PDF file
                    downloadFile(resultFileUrl, DestinationFile);
                    
                    System.debug('Generated PDF file saved as \'{0}\' file.'+ DestinationFile);
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
    
    @TestVisible
    private static void downloadFile(String extFileUrl, String DestinationFile)
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

##### **FillPDFFormSimplifiedTest.cls:**
    
```
@isTest
public class FillPDFFormSimplifiedTest
{
    private  testmethod static void testfillPDFForm()
    {
        Test.setMock(HttpCalloutMock.class, new FillPDFFormSimplifiedTest.DocumentCreationMock());
        (new FillPDFFormSimplified()).fillPDFForm();
        List<ContentVersion> cv = [select Id, VersionData from ContentVersion];
        System.assertEquals(1, cv.size());
        String strt1 = cv[0].VersionData.toString();
        System.assert(strt1.contains('John A. Doe'));
    }

    private  testmethod static void testfillPDFFormException()
    {
        (new FillPDFFormSimplified()).fillPDFForm();
        List<ContentVersion> cv = [select Id from ContentVersion];
        System.assertEquals(0, cv.size());
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