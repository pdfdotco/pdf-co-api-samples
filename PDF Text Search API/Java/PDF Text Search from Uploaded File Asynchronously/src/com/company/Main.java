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

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import okhttp3.*;

import java.io.*;
import java.net.*;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Main
{
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    final static String API_KEY = "***********************************";

    // Source file name
    final static Path SourceFile = Paths.get(".\\sample.pdf");

    // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
    final static String Pages = "";

    // PDF document password. Leave empty for unprotected documents.
    final static String Password = "";

    // Search string.
    final static String SearchString = "\\d{1,}\\.\\d\\d"; // Regular expression to find numbers like '100.00'
    // Note: do not use `+` char in regex, but use `{1,}` instead.
    // `+` char is valid for URL and will not be escaped, and it will become a space char on the server side.

    // Enable regular expressions (Regex)
    final static boolean RegexSearch = true;

    // (!) Make asynchronous job
    final static  boolean Async = true;


    public static void main(String[] args) throws IOException
    {
        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
        // * If you already have a direct file URL, skip to the step 3.

        // Prepare URL for `Get Presigned URL` API call
        String query = String.format(
                "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=%s",
                SourceFile.getFileName());

        // Prepare request
        Request request = new Request.Builder()
                .url(query)
                .addHeader("x-api-key", API_KEY) // (!) Set API Key
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
                // Get URL to use for the file upload
                String uploadUrl = json.get("presignedUrl").getAsString();
                // Get URL of uploaded file to use with later API calls
                String uploadedFileUrl = json.get("url").getAsString();

                // 2. UPLOAD THE FILE TO CLOUD.

                if (uploadFile(webClient, uploadUrl, SourceFile.toFile()))
                {
                    // 3. SEARCH TEXT FROM UPLOADED PDF FILE

                    searchTextFromPDF(webClient, uploadedFileUrl);
                }
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

    public static void searchTextFromPDF(OkHttpClient webClient, String uploadedFileUrl) throws IOException {

        // Prepare URL for PDF text search API call.
        // See documentation: https://apidocs.pdf.co
        String query = "https://api.pdf.co/v1/pdf/find";

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
		String jsonPayload = String.format("{\"password\": \"%s\", \"pages\": \"%s\", \"url\": \"%s\", \"searchString\": \"%s\", \"regexSearch\": \"%s\", \"async\": \"%s\"}",
                Password,
                Pages,
                uploadedFileUrl,
                SearchString,
                RegexSearch,
                Async);
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
        
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("MM/dd/yyyy HH:mm:ss");

        if (response.code() == 200)
        {
            // Parse JSON response
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();

            boolean error = json.get("error").getAsBoolean();
            if (!error)
            {
                // Asynchronous job ID
                String jobId = json.get("jobId").getAsString();

                System.out.println("Job#" + jobId + ": has been created. - " + dtf.format(LocalDateTime.now()));

                // URL of generated json file that will available after the job completion
                String resultFileUrl = json.get("url").getAsString();

                // Check the job status in a loop.
                // If you don't want to pause the main thread you can rework the code
                // to use a separate thread for the status checking and completion.
                do {
                    String status = CheckJobStatus(webClient, jobId);  // Possible statuses: "working", "failed", "aborted", "success".

                    System.out.println("Job#" + jobId + ": " + status + " - " + dtf.format(LocalDateTime.now()));


                    if(status.compareToIgnoreCase("success") == 0){

                        // Build request for job url
                        request = new Request.Builder()
                                .url(resultFileUrl)
                                .build();

                        // Execute request
                        response =  webClient.newCall(request).execute();

                        // Parse JSON response
                        JsonArray jsonArray = new JsonParser().parse(response.body().string()).getAsJsonArray();

                        // Display found items in console
                        for (JsonElement element : jsonArray)
                        {
                            JsonObject item = (JsonObject) element;
                            System.out.println("Found text " + item.get("text") + " at coordinates " + item.get("left") + ", "+ item.get("top"));
                        }

                        break;
                    }
                    else if (status.compareToIgnoreCase("working") == 0){
                        // Pause for a few seconds
                        try{
                            Thread.sleep(3000);
                        }
                        catch (InterruptedException ex){
                            Thread.currentThread().interrupt(); // restore interrupted status
                        }
                    }
                    else{
                        System.out.println(status);
                        break;
                    }
                }while (true);
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

    public static boolean uploadFile(OkHttpClient webClient, String url, File sourceFile) throws IOException
    {
        // Prepare request body
        RequestBody body = RequestBody.create(MediaType.parse("application/octet-stream"), sourceFile);

        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", API_KEY) // (!) Set API Key
                .addHeader("content-type", "application/octet-stream")
                .put(body)
                .build();

        // Execute request
        Response response = webClient.newCall(request).execute();

        return (response.code() == 200);
    }

    // Check Job Status
    private  static String CheckJobStatus(OkHttpClient webClient, String jobId) throws IOException {

        String url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", API_KEY) // (!) Set API Key
                .build();

        // Execute request
        Response response = webClient.newCall(request).execute();

        if (response.code() == 200)
        {
            // Parse JSON response
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();

            return json.get("status").getAsString();
        }
        else
        {
            // Display request error
            System.out.println(response.code() + " " + response.message());
        }

        return "Failed";
    }

}
