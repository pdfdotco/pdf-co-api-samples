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
        // HTML input
        final String inputSample = new String(Files.readAllBytes(Paths.get(".\\sample.html")));
        // Destination PDF file name
        final Path destinationFile = Paths.get(".\\result.pdf");

        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // Prepare URL for `HTML to PDF` API call
        String apiUrl = "https://api.pdf.co/v1/pdf/convert/from/html";

        // Make correctly escaped (encoded) URL
        URL url = null;
        try
        {
            url = new URI(null, apiUrl, null).toURL();
        }
        catch (URISyntaxException e)
        {
            e.printStackTrace();
        }

        // Prepare request body in JSON format
        // See documentation: https://apidocs.pdf.co/?#1-json-pdfconvertfromhtml
        JsonObject jsonBody = new JsonObject();

        // Input HTML code to be converted. Required.
        jsonBody.add("html", new JsonPrimitive(inputSample));

        // Name of resulting file
        jsonBody.add("name", new JsonPrimitive(destinationFile.getFileName().toString()));

        // Set to css style margins like 10 px or 5px 5px 5px 5px.
        jsonBody.add("margins", new JsonPrimitive("5px 5px 5px 5px"));

        // Can be Letter, A4, A5, A6 or custom size like 200x200
        jsonBody.add("paperSize", new JsonPrimitive("Letter"));

        // Set to Portrait or Landscape. Portrait by default.
        jsonBody.add("orientation", new JsonPrimitive("Portrait"));

        // true by default. Set to false to disable printing of background.
        jsonBody.add("printBackground", new JsonPrimitive(true));

        // If large input document, process in async mode by passing true
        jsonBody.add("async", new JsonPrimitive(false));

        // Set to HTML for header to be applied on every page at the header.
        jsonBody.add("header", new JsonPrimitive(""));

        // Set to HTML for footer to be applied on every page at the bottom.
        jsonBody.add("footer", new JsonPrimitive(""));


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
