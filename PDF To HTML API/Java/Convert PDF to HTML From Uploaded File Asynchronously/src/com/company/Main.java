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

public class Main {
    // The authentication key (API Key).
    final static String API_KEY = "*******************************";

    // Source PDF file
    final static Path SourceFile = Paths.get(".\\sample.pdf");
    // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
    final static String Pages = "";
    // PDF document password. Leave empty for unprotected documents.
    final static String Password = "";
    // Destination HTML file name
    final static Path DestinationFile = Paths.get(".\\result.html");
    // Set to `true` to get simplified HTML without CSS. Default is the rich HTML keeping the document design.
    final static boolean PlainHtml = false;
    // Set to `true` if your document has the column layout like a newspaper.
    final static boolean ColumnLayout = false;
    // Enable async processing
    final static boolean Async = true;

    public static void main(String[] args) throws IOException {
        OkHttpClient webClient = new OkHttpClient();

        // Step 1: Retrieve presigned URL to upload the file
        String query = String.format(
                "https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name=%s",
                SourceFile.getFileName());

        Request request = new Request.Builder()
                .url(query)
                .addHeader("x-api-key", API_KEY)
                .build();

        Response response = webClient.newCall(request).execute();

        if (response.code() == 200) {
            JsonObject json = JsonParser.parseString(response.body().string()).getAsJsonObject();
            boolean error = json.get("error").getAsBoolean();

            if (!error) {
                String uploadUrl = json.get("presignedUrl").getAsString();
                String uploadedFileUrl = json.get("url").getAsString();

                if (uploadFile(webClient, uploadUrl, SourceFile)) {
                    // Step 2: Start asynchronous PDF to HTML conversion
                    PdfToHtml(webClient, uploadedFileUrl);
                }
            } else {
                System.out.println("Error: " + json.get("message").getAsString());
            }
        } else {
            System.out.println(response.code() + " " + response.message());
        }
    }

    public static void PdfToHtml(OkHttpClient webClient, String uploadedFileUrl) throws IOException {
        String query = "https://api.pdf.co/v1/pdf/convert/to/html";

        URL url = null;
        try {
            // Proper URI handling
            url = new URI(query).toURL();
        } catch (URISyntaxException e) {
            System.out.println("Invalid URI: " + e.getMessage());
            return; // Exit gracefully if the URI is invalid
        }

        // JSON payload with async flag
        String jsonPayload = String.format(
                "{\"name\": \"%s\", \"password\": \"%s\", \"pages\": \"%s\", \"simple\": \"%s\", \"columns\": \"%s\", \"url\": \"%s\", \"async\": \"%s\"}",
                DestinationFile.getFileName(),
                Password,
                Pages,
                PlainHtml,
                ColumnLayout,
                uploadedFileUrl,
                Async
        );

        RequestBody body = RequestBody.create(MediaType.parse("application/json"), jsonPayload);

        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", API_KEY)
                .addHeader("Content-Type", "application/json")
                .post(body)
                .build();

        Response response = webClient.newCall(request).execute();

        if (response.code() == 200) {
            JsonObject json = JsonParser.parseString(response.body().string()).getAsJsonObject();
            String status = json.get("status").getAsString();

            if (!"error".equals(status)) {
                String jobId = json.get("jobId").getAsString();
                pollJobStatus(webClient, jobId);
            } else {
                System.out.println("Error: " + json.get("message").getAsString());
            }
        } else {
            System.out.println(response.code() + " " + response.message());
        }
    }

    public static void pollJobStatus(OkHttpClient webClient, String jobId) throws IOException {
        String url = String.format("https://api.pdf.co/v1/job/check?jobid=%s", jobId);

        while (true) {
            Request request = new Request.Builder()
                    .url(url)
                    .addHeader("x-api-key", API_KEY)
                    .build();

            Response response = webClient.newCall(request).execute();
            JsonObject json = JsonParser.parseString(response.body().string()).getAsJsonObject();

            String status = json.get("status").getAsString();
            System.out.println(java.time.LocalDateTime.now() + ": " + status);

            if ("success".equals(status)) {
                String resultFileUrl = json.get("url").getAsString();
                downloadFile(webClient, resultFileUrl, DestinationFile.toFile());
                System.out.println("File downloaded to: " + DestinationFile.toString());
                break;
            } else if ("working".equals(status)) {
                try {
                    Thread.sleep(3000); // Wait for 3 seconds before polling again
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            } else {
                System.out.println("Job finished with status: " + status);
                break;
            }
        }
    }

    public static boolean uploadFile(OkHttpClient webClient, String url, Path sourceFile) throws IOException {
        RequestBody body = RequestBody.create(MediaType.parse("application/octet-stream"), sourceFile.toFile());

        Request request = new Request.Builder()
                .url(url)
                .addHeader("content-type", "application/octet-stream")
                .put(body)
                .build();

        Response response = webClient.newCall(request).execute();
        return response.code() == 200;
    }

    public static void downloadFile(OkHttpClient webClient, String url, File destinationFile) throws IOException {
        Request request = new Request.Builder().url(url).build();
        Response response = webClient.newCall(request).execute();

        byte[] fileBytes = response.body().bytes();

        try (OutputStream output = new FileOutputStream(destinationFile)) {
            output.write(fileBytes);
        }
    }
}
