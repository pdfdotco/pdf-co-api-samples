## How to generate PDF from large HTML file for HTML to PDF API in Java using PDF.co Web API PDF.co Web API: the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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

##### **Main.java:**
    
```
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
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;


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
        jsonBody.add("async", new JsonPrimitive(true));

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

                    String status = CheckJobStatus(webClient, jobId); // Possible statuses: "working", "failed", "aborted", "success"

                    System.out.println("Job#" + jobId + ": " + status + " - " + dtf.format(LocalDateTime.now()));

                    if (status.compareToIgnoreCase("success") == 0) {

                        // Download PDF file
                        downloadFile(webClient, resultFileUrl, destinationFile.toFile());

                        System.out.printf("Generated PDF file saved as \"%s\" file.", destinationFile.toString());

                        break;

                    } else if (status.compareToIgnoreCase("working") == 0) {
                        // Pause for a few seconds
                        try {
                            Thread.sleep(3000);
                        } catch (InterruptedException ex) {
                            Thread.currentThread().interrupt(); // restore interrupted status
                        }
                    } else {
                        System.out.println(status);
                        break;
                    }
                } while (true);
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

    // Check Job Status
    private static String CheckJobStatus(OkHttpClient webClient, String jobId) throws IOException {

        String url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

        // Prepare request
        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", API_KEY) // (!) Set API Key
                .build();

        // Execute request
        Response response = webClient.newCall(request).execute();

        if (response.code() == 200) {
            // Parse JSON response
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();

            return json.get("status").getAsString();
        } else {
            // Display request error
            System.out.println(response.code() + " " + response.message());
        }

        return "Failed";
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

```

<!-- code block end -->