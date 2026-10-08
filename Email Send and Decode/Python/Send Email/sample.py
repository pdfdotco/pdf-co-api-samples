import requests
import json

url = "https://api.pdf.co/v1/email/send"

# Please refer to our knowledge base at (https://apidocs.pdf.co/kb/Email%20Send%20(email-send)/index) for SMTP related information

payload = json.dumps({
  "url": "https://bytescout-com.s3-us-west-2.amazonaws.com/files/demo-files/cloud-api/pdf-info/sample.pdf",
  "from": "John Doe <john@example.com>",
  "to": "Partner <partner@example.com>",
  "subject": "Check attached sample pdf",
  "bodytext": "Please check the attached pdf",
  "bodyHtml": "Please check the attached pdf",
  "smtpserver": "smtp.gmail.com",
  "smtpport": "587",
  "smtpusername": "my@gmail.com",
  "smtppassword": "app specific password created as https://support.google.com/accounts/answer/185833",
  "async": False
})
headers = {
		'Content-Type': 'application/json',
		'x-api-key': 'ADD_YOUR_API_KEY'
}

response = requests.request("POST", url, headers=headers, data=payload)

print(response.text)
