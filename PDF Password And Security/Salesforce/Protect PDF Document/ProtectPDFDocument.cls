public class ProtectPDFDocument{
    
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    String API_KEY = '**************************';
    // Destination File Name
    string DestinationFile = 'result.pdf';
    
    // Direct URL of source PDF file.
    // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
    string SourceFileUrl = 'https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf';

    // Passwords to protect PDF document
    // The owner password will be required for document modification.
    // The user password only allows to view and print the document.
    string OwnerPassword = '123456';
    string UserPassword = '654321';

    // Encryption algorithm. 
    // Valid values: 'RC4_40bit', 'RC4_128bit', 'AES_128bit', 'AES_256bit'.
    string EncryptionAlgorithm = 'AES_128bit';

    // Allow or prohibit content extraction for accessibility needs.
    Boolean AllowAccessibilitySupport = true;

    // Allow or prohibit assembling the document.
    Boolean AllowAssemblyDocument = true;

    // Allow or prohibit printing PDF document.
    Boolean AllowPrintDocument = true;

    // Allow or prohibit filling of interactive form fields (including signature fields) in PDF document.
    Boolean AllowFillForms = true;

    // Allow or prohibit modification of PDF document.
    Boolean AllowModifyDocument = true;

    // Allow or prohibit copying content from PDF document.
    Boolean AllowContentExtraction = true;

    // Allow or prohibit interacting with text annotations and forms in PDF document.
    Boolean AllowModifyAnnotations = true;

    // Allowed printing quality.
    // Valid values: 'HighResolution', 'LowResolution'
    string PrintQuality = 'HighResolution';

    public void protectPDFDocument()
    {        
        try
        {
            // Create HTTP client instance
            Http http = new Http();
            HttpRequest request = new HttpRequest();
            // Set API Key
            request.setHeader('x-api-key', API_KEY);
                        
            // If enabled, Runs processing asynchronously. Returns Use JobId that you may use with /job/check to check state of the processing (possible states: working, 
            Boolean async = false;
            
            // Prepare requests params as JSON
            // See documentation: https://apidocs.pdf.co
            // Create JSON payload
            JSONGenerator gen = JSON.createGenerator(true);
            gen.writeStartObject();
            gen.writeStringField('url', SourceFileUrl);
            gen.writeStringField('name', DestinationFile);
            gen.writeStringField('ownerPassword', OwnerPassword);
            gen.writeStringField('userPassword', UserPassword);
            gen.writeStringField('encryptionAlgorithm', EncryptionAlgorithm);
            gen.writeBooleanField('allowAccessibilitySupport', AllowAccessibilitySupport);
            gen.writeBooleanField('allowAssemblyDocument', AllowAssemblyDocument);
            gen.writeBooleanField('allowPrintDocument', AllowPrintDocument);
            gen.writeBooleanField('allowFillForms', AllowFillForms);
            gen.writeBooleanField('allowModifyDocument', AllowModifyDocument);
            gen.writeBooleanField('allowContentExtraction', AllowContentExtraction);
            gen.writeBooleanField('allowModifyAnnotations', AllowModifyAnnotations);
            gen.writeStringField('printQuality', PrintQuality);
            gen.writeBooleanField('async', async);

            gen.writeEndObject();

            // Convert dictionary of params to JSON
            String jsonPayload = gen.getAsString();
            
            // URL of 'PDF Security' endpoint
            string url = 'https://api.pdf.co/v1/pdf/security/add';
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