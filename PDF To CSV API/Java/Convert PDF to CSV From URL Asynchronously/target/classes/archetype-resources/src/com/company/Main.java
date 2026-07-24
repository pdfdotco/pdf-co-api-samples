//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api                                     //
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

//
// PDF.co Web API example for processing large documents using asynchronous API.
//
public class Main
{
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    final static String API_KEY = "nnamdionyemaobi@gmail.com_Fcz589ZrRNt66ufvgsZZaMOHs918OOJ75F6p5sHyIZAUVpnjM1e3cVrq3jFs5gDi";

    // Source PDF URL
    final static String SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-to-csv/sample.pdf"; // <-- Replace with your PDF URL
    // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
    final static String Pages = "";
    // PDF document password. Leave empty for unprotected documents.
    final static String Password = "";
    // Destination CSV file name
    final static Path DestinationFile = Paths.get(".\\resul8.csv");
    // (!) Create asynchronous job
    final static boolean Async = true;
    final static String rect = "29,309,270,135";

    public static void main(String[] args) throws IOException
    {
        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // Directly call the PDF to CSV conversion since the file is already available at a URL
        PdfToCsv(webClient, API_KEY, DestinationFile, Password, Pages, SourceFileUrl);
    }

    public static void PdfToCsv(OkHttpClient webClient, String apiKey, Path destinationFile,
                                String password, String pages, String sourceFileUrl) throws IOException
    {
        // Prepare URL for `PDF To CSV` API call
        String query = "https://api.pdf.co/v1/pdf/convert/to/csv";

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
        String jsonPayload = String.format("{\"name\": \"%s\", \"password\": \"%s\", \"pages\": \"%s\", \"url\": \"%s\", \"async\": \"%s\", \"rect\": \"%s\"}",
                destinationFile.getFileName(),
                password,
                pages,
                sourceFileUrl,   // <-- Use the source URL here
                Async,
                rect);

        // Prepare request body
        RequestBody body = RequestBody.create(MediaType.parse("application/json"), jsonPayload);

        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", apiKey) // (!) Set API Key
                .addHeader("Content-Type", "application/json")
                .post(body)
                .build();

        // Execute request
        Response response = webClient.newCall(request).execute();

        if (response.code() == 200)
        {
            // Parse JSON response
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();

            String status = json.get("status").getAsString();
            if (!status.equals("error"))
            {
                // Asynchronous job ID
                String jobId = json.get("jobId").getAsString();
                // Get URL of generated CSV file
                String resultFileUrl = json.get("url").getAsString();

                // Check the job status in a loop.
                // If you don't want to pause the main thread you can rework the code
                // to use a separate thread for the status checking and completion.
                do
                {
                    status = checkJobStatus(webClient, apiKey, jobId); // Possible statuses: "working", "failed", "aborted", "success".

                    // Display timestamp and status (for demo purposes)
                    System.out.println(java.time.LocalDateTime.now() + ": " + status);

                    if (status.equals("success"))
                    {
                        // Download CSV file
                        downloadFile(webClient, resultFileUrl, destinationFile.toFile());

                        System.out.printf("Generated CSV file saved as \"%s\" file.", destinationFile.toString());
                        break;
                    }
                    else if (status.equals("working"))
                    {
                        // Pause for a few seconds
                        try {
                            Thread.sleep(3000);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                    else
                    {
                        System.out.printf(status);
                        break;
                    }
                }
                while (true);
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

    public static String checkJobStatus(OkHttpClient webClient, String apiKey, String jobId) throws IOException
    {
        // Prepare URL for `PDF To CSV` API call
        String url = String.format("https://api.pdf.co/v1/job/check?jobid=%s", jobId);

        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", apiKey) // (!) Set API Key
                .build();
        // Execute request
        Response response = webClient.newCall(request).execute();

        // Parse JSON response
        JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();

        return json.get("status").getAsString();
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
    }
}
