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
    final static String API_KEY = "****************************";

    // Direct URL of source PDF file.
    final static String SourceFileUrl = "bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-form/f1040.pdf";
    // PDF document password. Leave empty for unprotected documents.
	final static String Password = "";

    // Destination PDF file name
	final static Path ResultFile = Paths.get(".\\result.pdf");

    public static void main(String[] args) throws IOException
    {
        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // Prepare URL for `PDF Edit` API call
        String query = "https://api.pdf.co/v1/pdf/edit/add";

        // Prepare form filling data
        String fields = "[\n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].FilingStatus[0].c1_01[1]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"True\"\n" +
                "        },\n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].f1_02[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"John A.\"\n" +
                "        },        \n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].f1_03[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"Doe\"\n" +
                "        },        \n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_04[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"123456789\"\n" +
                "        },\n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"Joan B.\"\n" +
                "        },\n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_05[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"Joan B.\"\n" +
                "        },\n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_06[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"Doe\"\n" +
                "        },\n" +
                "        {\n" +
                "            \"fieldName\": \"topmostSubform[0].Page1[0].YourSocial_ReadOrderControl[0].f1_07[0]\",\n" +
                "            \"pages\": \"1\",\n" +
                "            \"text\": \"987654321\"\n" +
                "        }     \n" +
                "    ]";

        // Asynchronous Job
        String async = "false";

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
        String jsonPayload = String.format("{\n" +
                "    \"url\": \"%s\",\n" +
                "    \"async\": %s,\n" +
                "    \"encrypt\": false,\n" +
                "    \"name\": \"f1040-filled\",\n" +
                "    \"fields\": %s"+
                "}", SourceFileUrl, async, fields);

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
                // Get URL of generated output file
                String resultFileUrl = json.get("url").getAsString();

                // Download the image file
                downloadFile(webClient, resultFileUrl, ResultFile);

                System.out.printf("Generated file saved to \"%s\" file.", ResultFile.toString());
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

    public static void downloadFile(OkHttpClient webClient, String url, Path destinationFile) throws IOException
    {
        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .build();
        // Execute request
        Response response = webClient.newCall(request).execute();

        byte[] fileBytes = response.body().bytes();

        // Save downloaded bytes to file
        OutputStream output = new FileOutputStream(destinationFile.toFile());
        output.write(fileBytes);
        output.flush();
        output.close();

        response.close();
    }
}
