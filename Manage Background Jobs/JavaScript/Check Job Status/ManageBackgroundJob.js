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
  'method': 'POST',
  'url': 'https://api.pdf.co/v1/job/check',
  'headers': {
    'x-api-key': '{{x-api-key}}'
  },
  formData: {
    'jobid': '123'
  }
};
request(options, function (error, response) {
  if (error) throw new Error(error);
  console.log(response.body);
});
