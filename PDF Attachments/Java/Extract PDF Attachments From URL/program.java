//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


import java.io.*;
import okhttp3.*;
public class main {
		public static void main(String []args) throws IOException{
				OkHttpClient client = new OkHttpClient().newBuilder()
						.build();
				MediaType mediaType = MediaType.parse("application/json");
				RequestBody body = RequestBody.create(mediaType, "{\n    \"url\": \"https://bytescout-com.s3.us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-attachments/attachments.pdf\",\n    \"inline\": true,\n    \"async\": false\n}");
				Request request = new Request.Builder()
						.url("https://api.pdf.co/v1/pdf/attachments/extract")
						.method("POST", body)
						.addHeader("Content-Type", "application/json")
						.addHeader("x-api-key", "__Replace_With_Your_PDFco_API_Key__")
						.build();
				Response response = client.newCall(request).execute();
				System.out.println(response.body().string());
		}
}
