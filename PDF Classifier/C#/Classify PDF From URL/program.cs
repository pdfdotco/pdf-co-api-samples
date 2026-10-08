//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


using System;
using RestSharp;
namespace HelloWorldApplication {
		class HelloWorld {
				static void Main(string[] args) {
						var client = new RestClient("https://api.pdf.co/v1/pdf/classifier");
						client.Timeout = -1;
						var request = new RestRequest(Method.POST);
						request.AddHeader("Content-Type", "application/json");
						request.AddHeader("x-api-key", "YOUR_PDFCO_API_KEY");
						var body = @"{" + "\n" +
						@"    ""url"": ""https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/document-parser/sample-invoice.pdf""," + "\n" +
						@"    ""async"": false," + "\n" +
						@"    ""encrypt"": ""false""," + "\n" +
						@"    ""inline"": ""true""," + "\n" +
						@"    ""password"": """"," + "\n" +
						@"    ""profiles"": """"" + "\n" +
						@"} ";
						request.AddParameter("application/json", body,  ParameterType.RequestBody);
						IRestResponse response = client.Execute(request);
						Console.WriteLine(response.Content);
				}
		}
}
