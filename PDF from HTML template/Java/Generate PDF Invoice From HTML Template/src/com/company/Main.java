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
import com.google.gson.JsonPrimitive;
import okhttp3.*;

import java.io.*;
import java.net.*;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

public class Main
{
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    final static String API_KEY = "***********************************";


    public static void main(String[] args) throws IOException
    {
        /* 
            Please follow below steps to create your own HTML Template and get "templateId". 
            1. Add new html template in app.pdf.co/templates/html
            2. Copy paste your html template code into this new template. Sample HTML templates can be found at "https://github.com/bytescout/pdf-co-api-samples/tree/master/PDF%20from%20HTML%20template/TEMPLATES-SAMPLES"
            3. Save this new template
            4. Copy it’s ID to clipboard
            5. Now set ID of the template into “templateId” parameter
        */

        // HTML template using built-in template
        // see https://app.pdf.co/templates/html/2/edit
        final String templateId = "2";
        
        // Data to fill the template
        final String templateData = new String(Files.readAllBytes(Paths.get(".\\invoice_data.json")));
        // Destination PDF file name
        final Path destinationFile = Paths.get(".\\result.pdf");

        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // Prepare URL for `HTML to PDF` API call
        String query = String.format(
                "https://api.pdf.co/v1/pdf/convert/from/html?name=%s",
                destinationFile.getFileName());

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

        // Prepare request body in JSON format
        JsonObject jsonBody = new JsonObject();
        jsonBody.add("templateId", new JsonPrimitive(templateId));
        jsonBody.add("templateData", new JsonPrimitive(templateData));

        RequestBody body = RequestBody.create(MediaType.parse("application/json"), jsonBody.toString());

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
                // Get URL of generated PDF file
                String resultFileUrl = json.get("url").getAsString();

                // Download PDF file
                downloadFile(webClient, resultFileUrl, destinationFile.toFile());

                System.out.printf("Generated PDF file saved as \"%s\" file.", destinationFile.toString());
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
