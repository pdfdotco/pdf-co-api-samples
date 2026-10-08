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
var fs = require('fs');
var options = {
  'method': 'POST',
  'url': 'https://api.pdf.co/v1/file/upload',
  'headers': {
    'x-api-key': '{{x-api-key}}'
  },
  formData: {
    'file': {
      'value': fs.createReadStream('/path/to/file'),
      'options': {
        'filename': 'filename'
        'contentType': null
      }
    }
  }
};
request(options, function (error, response) {
  if (error) throw new Error(error);
  let data = JSON.parse(response.body);
  console.log(data);
});
