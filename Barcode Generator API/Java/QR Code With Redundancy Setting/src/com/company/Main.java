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
    final static String API_KEY = "********************************";

    // Result file name
    final static Path ResultFile = Paths.get(".\\barcode.png");
    // Barcode type. See valid barcode types in the documentation https://apidocs.pdf.co/#barcode-generator
    final static String BarcodeType = "QRCode";
    // Barcode value
    final static String BarcodeValue = "QR123456\\nhttps://pdf.co\\nhttps://bytescout.com";

        
    public static void main(String[] args) throws IOException
    {
        // Create HTTP client instance
        OkHttpClient webClient = new OkHttpClient();

        /*
		 Valid error correction levels:
		 ----------------------------------
		 Low - [default] Lowest error correction level. (Approx. 7% of codewords can be restored).
		 Medium - Medium error correction level. (Approx. 15% of codewords can be restored).
		 Quarter - Quarter error correction level (Approx. 25% of codewords can be restored).
		 High - Highest error correction level (Approx. 30% of codewords can be restored).
		*/

        // Set "Custom Profiles" parameter
        String Profiles = "{ \\\"profiles\\\": [ { \\\"profile1\\\": { \\\"Options.QRErrorCorrectionLevel\\\": \\\"Quarter\\\" } } ] }";

        // Prepare URL for `Barcode Generator` API call
        String query = "https://api.pdf.co/v1/barcode/generate";

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
        String jsonPayload = String.format("{\"name\": \"%s\", \"type\": \"%s\", \"value\": \"%s\", \"profiles\": \"%s\"}",
            ResultFile.getFileName(),
            BarcodeType,
            BarcodeValue,
            Profiles);

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
                // Get URL of generated barcode image file
                String resultFileUrl = json.get("url").getAsString();

                // Download the image file
                downloadFile(webClient, resultFileUrl, ResultFile);

                System.out.printf("Generated barcode saved to \"%s\" file.", ResultFile.toString());
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
