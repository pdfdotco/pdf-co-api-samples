public class FillPDFFormSimplified{
    
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    String API_KEY = 'ABCDE';
    // Destination File Name
    string DestinationFile = 'result.pdf';
    
    // Direct URL of source PDF file.
    // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
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