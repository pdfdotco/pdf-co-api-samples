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
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Main {
    // Get your own API Key by registering at https://app.pdf.co
    final static String API_KEY = "*************";

    public static void main(String[] args) throws IOException, InterruptedException {
        // Source PDF file
        final String SourceFileUrl = "https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/document-parser/MultiPageTable.pdf";
        // PDF document password. Leave empty for unprotected documents.
        final String Password = "";
        // Destination JSON file name
        final Path DestinationFile = Paths.get(".\\result.csv");
        final String outputFormat = "CSV";
        // Template text
        String templateText = new String(Files.readAllBytes(Paths.get(".\\MultiPageTable-template1.json")), StandardCharsets.UTF_8);

        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        // Parse the uploaded PDF document
        ParseDocumentAsync(webClient, API_KEY, DestinationFile, Password, SourceFileUrl, templateText, outputFormat);
    }

    public static void ParseDocumentAsync(OkHttpClient webClient, String apiKey, Path destinationFile,
                                          String password, String uploadedFileUrl, String templateText, String outputFormat) throws IOException, InterruptedException {

        // Prepare POST request body in JSON format
        JsonObject jsonBody = new JsonObject();
        jsonBody.add("url", new JsonPrimitive(uploadedFileUrl));
        jsonBody.add("template", new JsonPrimitive(templateText));
        jsonBody.add("outputFormat", new JsonPrimitive(outputFormat));
        jsonBody.add("async", new JsonPrimitive(true)); // Enable asynchronous processing

        RequestBody body = RequestBody.create(MediaType.parse("application/json"), jsonBody.toString());

        // Prepare request to `Document Parser` API
        Request request = new Request.Builder()
                .url("https://api.pdf.co/v1/pdf/documentparser")
                .addHeader("x-api-key", apiKey)
                .addHeader("Content-Type", "application/json")
                .post(body)
                .build();

        // Execute request
        Response response = webClient.newCall(request).execute();
        if (response.code() == 200) {
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();
            boolean error = json.get("error").getAsBoolean();
            if (!error) {
                String jobId = json.get("jobId").getAsString();
                System.out.println("Job#" + jobId + ": has been created.");

                // Check job status in a loop
                while (true) {
                    String status = CheckJobStatus(webClient, apiKey, jobId);
                    DateTimeFormatter dtf = DateTimeFormatter.ofPattern("MM/dd/yyyy HH:mm:ss");
                    System.out.println("Job#" + jobId + ": " + status + " - " + dtf.format(LocalDateTime.now()));

                    if (status.equalsIgnoreCase("success")) {
                        String resultFileUrl = json.get("url").getAsString();
                        // Download the file
                        downloadFile(webClient, resultFileUrl, destinationFile.toFile());
                        System.out.printf("Generated file saved to \"%s\" file.", destinationFile.toString());
                        break;
                    } else if (status.equalsIgnoreCase("working")) {
                        Thread.sleep(3000); // Pause for a few seconds before retrying
                    } else {
                        System.out.println("Job failed with status: " + status);
                        break;
                    }
                }
            } else {
                System.out.println(json.get("message").getAsString());
            }
        } else {
            System.out.println(response.code() + " " + response.message());
        }
    }

    // Check Job Status
    private static String CheckJobStatus(OkHttpClient webClient, String apiKey, String jobId) throws IOException {
        String url = "https://api.pdf.co/v1/job/check?jobid=" + jobId;

        Request request = new Request.Builder()
                .url(url)
                .addHeader("x-api-key", apiKey)
                .build();

        Response response = webClient.newCall(request).execute();
        if (response.code() == 200) {
            JsonObject json = new JsonParser().parse(response.body().string()).getAsJsonObject();
            return json.get("status").getAsString();
        } else {
            System.out.println(response.code() + " " + response.message());
        }
        return "Failed";
    }

    public static void downloadFile(OkHttpClient webClient, String url, File destinationFile) throws IOException {
        Request request = new Request.Builder()
                .url(url)
                .build();

        Response response = webClient.newCall(request).execute();
        byte[] fileBytes = response.body().bytes();
        OutputStream output = new FileOutputStream(destinationFile);
        output.write(fileBytes);
        output.flush();
        output.close();
        response.close();
    }
}
