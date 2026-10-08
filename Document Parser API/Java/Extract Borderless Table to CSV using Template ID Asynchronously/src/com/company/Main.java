//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//




import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.JsonPrimitive;
import okhttp3.*;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Main {
    // Get your own API Key by registering at https://app.pdf.co
    final static String API_KEY = "****************************************";

    public static void main(String[] args) throws IOException, InterruptedException {
        // Source PDF file
        final String SourceFileUrl = "https://pdfco-test-files.s3.us-west-2.amazonaws.com/document-parser/sample-invoice.pdf";
        // PDF document password. Leave empty for unprotected documents.
        final String Password = "";
        // Destination CSV file name
        final Path DestinationFile = Paths.get(".\\result2.csv");
        final String outputFormat = "CSV";
        // Template text. Use Document Parser to create templates.
        final String templateId = "1";

        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // Initiate async document parsing
        initiateAsyncDocumentParsing(webClient, API_KEY, DestinationFile, Password, SourceFileUrl, outputFormat, templateId);
    }

    public static void initiateAsyncDocumentParsing(OkHttpClient webClient, String apiKey, Path destinationFile,
                                                    String password, String uploadedFileUrl, String outputFormat, String templateId) throws IOException, InterruptedException {
        // Prepare POST request body in JSON format
        JsonObject jsonBody = new JsonObject();
        jsonBody.add("url", new JsonPrimitive(uploadedFileUrl));
        jsonBody.add("outputFormat", new JsonPrimitive(outputFormat));
        jsonBody.add("templateId", new JsonPrimitive(templateId));
        jsonBody.add("async", new JsonPrimitive(true)); // Enable async mode

        RequestBody body = RequestBody.create(MediaType.parse("application/json"), jsonBody.toString());

        // Prepare request to `Document Parser` API
        Request request = new Request.Builder()
                .url("https://api.pdf.co/v1/pdf/documentparser")
                .addHeader("x-api-key", apiKey) // (!) Set API Key
                .addHeader("Content-Type", "application/json")
                .post(body)
                .build();

        // Execute request
        Response response = webClient.newCall(request).execute();
        if (response.code() == 200) {
            // Parse JSON response
            JsonObject json = JsonParser.parseString(response.body().string()).getAsJsonObject();
            boolean error = json.get("error").getAsBoolean();
            if (!error) {
                // Asynchronous job ID
                String jobId = json.get("jobId").getAsString();
                System.out.println("Job#" + jobId + ": has been created.");
                // Poll job status
                pollJobStatus(webClient, apiKey, jobId, destinationFile);
            } else {
                // Display service reported error
                System.out.println(json.get("message").getAsString());
            }
        } else {
            // Display request error
            System.out.println(response.code() + " " + response.message());
        }
    }

    private static void pollJobStatus(OkHttpClient webClient, String apiKey, String jobId, Path destinationFile) throws IOException, InterruptedException {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("MM/dd/yyyy HH:mm:ss");
        String jobStatusUrl = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

        while (true) {
            // Prepare request to check job status
            Request request = new Request.Builder()
                    .url(jobStatusUrl)
                    .addHeader("x-api-key", apiKey) // (!) Set API Key
                    .build();

            // Execute request
            Response response = webClient.newCall(request).execute();
            if (response.code() == 200) {
                // Parse JSON response
                JsonObject json = JsonParser.parseString(response.body().string()).getAsJsonObject();
                String status = json.get("status").getAsString();

                System.out.println("Job#" + jobId + ": " + status + " - " + dtf.format(LocalDateTime.now()));

                if (status.equalsIgnoreCase("success")) {
                    // Get result file URL
                    String resultFileUrl = json.get("url").getAsString();
                    downloadFile(webClient, resultFileUrl, destinationFile.toFile());
                    System.out.printf("Generated file saved to \"%s\" file.%n", destinationFile.toString());
                    break;
                } else if (status.equalsIgnoreCase("failed") || status.equalsIgnoreCase("aborted")) {
                    System.out.println("Job failed with status: " + status);
                    break;
                }

                // Wait for a few seconds before checking again
                Thread.sleep(3000);
            } else {
                // Display request error
                System.out.println(response.code() + " " + response.message());
                break;
            }
        }
    }

    public static void downloadFile(OkHttpClient webClient, String url, File destinationFile) throws IOException {
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
