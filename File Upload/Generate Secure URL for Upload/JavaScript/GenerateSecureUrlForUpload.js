//*******************************************************************************************//
//                                                                                           //
// Get Your API Key: https://app.pdf.co/signup                                               //
// API Documentation: https://developer.pdf.co/api/index.html                                //
//                                                                                           //
// Note: Replace placeholder values in the code with your API Key                            //
// and file paths (if applicable)                                                            //
//                                                                                           //
//*******************************************************************************************//


var request = require('request');
var options = {
  'method': 'GET',
  'url': 'https://api.pdf.co/v1/file/upload/get-presigned-url?name=test.pdf&encrypt=true',
  'headers': {
    'x-api-key': '{{x-api-key}}'
  },
  formData: {

  }
};
request(options, function (error, response) {
  if (error) throw new Error(error);
  let data = JSON.parse(response.body);
  console.log(data);
});
