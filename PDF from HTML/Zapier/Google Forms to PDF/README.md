## google forms to PDF in Zapier using PDF.co Web API PDF.co Web API is the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

## REQUEST FREE TECH SUPPORT

[Click here to get in touch](https://bytescout.zendesk.com/hc/en-us/requests/new?subject=PDF.co%20Web%20API%20Question)

or just send email to [support@bytescout.com](mailto:support@bytescout.com?subject=PDF.co%20Web%20API%20Question) 

## ON-PREMISE OFFLINE SDK 

[Get Your 60 Day Free Trial](https://bytescout.com/download/web-installer?utm_source=github-readme)
[Explore Documentation](https://bytescout.com/documentation/index.html?utm_source=github-readme)
[Explore Source Code Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/)
[Sign Up For Online Training](https://academy.bytescout.com/)


## ON-DEMAND REST WEB API

[Get your API key](https://app.pdf.co/signup?utm_source=github-readme)
[Security](https://pdf.co/security)
[Explore Web API Documentation](https://apidocs.pdf.co?utm_source=github-readme)
[Explore Web API Samples](https://github.com/bytescout/ByteScout-SDK-SourceCode/tree/master/PDF.co%20Web%20API)

## VIDEO REVIEW

[https://www.youtube.com/watch?v=NEwNs2b9YN8](https://www.youtube.com/watch?v=NEwNs2b9YN8)




<!-- code block begin -->

##### **Tutorial.txt:**
    
```
1. In this sample we're using "Job application" google form as shown below.    
[Screenshot_1.png]

2. Google form can be associated with spreadsheet in backend to hold all data. We'll use this spreadsheet data to generate output PDF. Whenever new row is added to spreadsheet, it'll be used as trigger to process further actions. 
[Screenshot_2.png]

3. In first step to create zap, we've provided “Google Sheets” as input app and "New Spreadsheet Row" as trigger. Further also provided specific "Spreadsheet" and "Worksheet" to lookup on.
[Screenshot_3.png]"

4. After all configuration is ready, it's time to test trigger and analyze output data. Prior to testing, do add new google form input. This will add new row in spreadsheet which we’ve configured as trigger for this zap. Data will look like below.
[Screenshot_4.png]

5. Now that input application and trigger configuration is completed, let's move to next step where we proceed with PDF creation. Let's select "PDF.co" as an App and "Anything to PDF Converter" as an Event.
[Screenshot_5.png]

6. "Source Type" parameter value should be "Raw HTML code to convert to PDF". Also provide "Input" field value with html. Input html will contain data fields from spreadsheet such as “Name”, “Email”, “Phone Number”, etc. Finally provide some meaningful output PDF name in "Name" field.
[Screenshot_6.png]

7. Prior to sending data to PDF.co for PDF output, Let's review generated input HTML along with target PDF file name.
[Screenshot_7.png]

8. Here's response from PDF.co. It contains fields such as URL of output pdf, total page count of generated document, etc.
[Screenshot_8.png]

9. If we review generated PDF file, it'll look like following.
[Screenshot_9.png]
```

<!-- code block end -->