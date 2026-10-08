//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


package com.company;

import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import okhttp3.*;

import java.io.*;
import java.net.*;
import java.nio.file.Path;
import java.nio.file.Paths;

public class Main
{
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    final static String API_KEY = "***********************************";

    // Direct URL of source PDF file.
    // You can also upload your own file into PDF.co and use it as url. Check "Upload File" samples for code snippets: https://github.com/bytescout/pdf-co-api-samples/tree/master/File%20Upload/    
    final static String SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-to-text/sample.pdf";
    // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
    final static String Pages = "";
    // PDF document password. Leave empty for unprotected documents.
    final static String Password = "";
    // Destination TXT file name
    final static Path DestinationFile = Paths.get(".\\result.txt");


    public static void main(String[] args) throws IOException
    {
        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        /*
        Some of advanced options available through profiles:
        (JSON can be single/double-quoted and contain comments.)
        {
            "profiles": [
                {
                    "profile1": {                
                        "ExtractInvisibleText": true, // Invisible text extraction. Values: true / false
                        "ExtractShadowLikeText": true, // Shadow-like text extraction. Values: true / false
                        "ExtractAnnotations": true, // Whether to extract PDF annotations.
                        "CheckPermissions": true, // Ignore document permissions. Values: true / false
                        "DetectNewColumnBySpacesRatio": 1.2, // A ratio affecting number of spaces between words. 
                    }
                }
            ]
        }
        */
        // Sample profile that sets advanced conversion options
        // Advanced options are properties of TextExtractor class from ByteScout Text Extractor SDK used in the back-end:
        // https://cdn.bytescout.com/help/BytescoutPDFExtractorSDK/html/8a2bae5a-346f-8338-b5aa-6f3522dca0d4.htm
        String profiles = "{ 'profiles': [ { 'profile1': { 'TrimSpaces': 'False', 'PreserveFormattingOnTextExtraction': 'True', 'Unwrap': 'True' } } ] }";

        // Prepare URL for `PDF To TXT` API call
        String query = "https://api.pdf.co/v1/pdf/convert/to/text";

        // Make correctly escaped (encoded) URL
        URL url = null;
        try
        {
            url = new URI(null, query, null).toURL();
        }
        catch (URISyntaxException e)
        {
            e.printStackTrace();
        }

        // Create JSON payload
		String jsonPayload = String.format("{\"name\": \"%s\", \"password\": \"%s\", \"pages\": \"%s\", \"url\": \"%s\", \"profiles\": \"%s\"}",
                DestinationFile.getFileName(),
                Password,
                Pages,
                SourceFileUrl,
                profiles);

        // Prepare request body
        RequestBody body = RequestBody.create(MediaType.parse("application/json"), jsonPayload);
        
        // Prepare request
        Request request = new Request.Builder()
            .url(url)
            .addHeader("x-api-key", API_KEY) // (!) Set API Key
            .addHeader("Content-Type", "application/json")
            .post(body)
            .build();
        
        // Execute request
        Response response = webClient.newCall(request).execute();
        
        if (response.code() == 200)
        {
            // Parse JSON response
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();

            boolean error = json.get("error").getAsBoolean();
            if (!error)
            {
                // Get URL of generated TXT file
                String resultFileUrl = json.get("url").getAsString();

                // Download TXT file
                downloadFile(webClient, resultFileUrl, DestinationFile.toFile());

                System.out.printf("Generated TXT file saved as \"%s\" file.", DestinationFile.toString());
            }
            else
            {
                // Display service reported error
                System.out.println(json.get("message").getAsString());
            }
        }
        else
        {
            // Display request error
            System.out.println(response.code() + " " + response.message());
        }
    }

    public static void downloadFile(OkHttpClient webClient, String url, File destinationFile) throws IOException
    {
        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .build();
        // Execute request
        Response response = webClient.newCall(request).execute();

        byte[] fileBytes = response.body().bytes();

        // Save downloaded bytes to file
        OutputStream output = new FileOutputStream(destinationFile);
        output.write(fileBytes);
        output.flush();
        output.close();

        response.close();
    }
}
