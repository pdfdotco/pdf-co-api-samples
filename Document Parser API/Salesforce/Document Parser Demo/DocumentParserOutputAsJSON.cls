public class DocumentParserOutputAsJSON {
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    static String API_KEY = '***********************';
    
    // Direct URL of source PDF file.
    // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
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