## pipedrive CRM to PDF in Zapier using PDF.co Web API PDF.co Web API: the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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
1. Select "Pipedrive" as an application. In this demo we'll create PDF report for new Deals created, hence "New Deal" will be selected as an event.
[Screenshot_1.png]

2. This is how new deal is created in Pipedrive.
[Screenshot_2.png]

3. Now that new deal is created, it’s time to Test trigger.
[Screenshot_3.png]"

4. Test trigger data contains all deals related information such as organization name, contact name, etc. 
[Screenshot_4.png]

5. In order to create PDF report, we'll use "PDF.co" as app and "Anything to PDF Converter" as an event. Method "Anything to PDF Converter" can convert most of formats such as HTML, document, spreadsheet, presentation or even URL to PDF output type. 
[Screenshot_5.png]

6. Now we need to provide all event parameters such as source type, input, etc. Select "Raw HTML code to convert to PDF" as "Source Type". In "Input" field specify basic html report which covers deal information as shown in screenshot. Lastly, we've provided output file name as deal name with PDF extension.
[Screenshot_6.png]

7. Here's final input request which will be sent to PDF.co for PDF output generation.
[Screenshot_7.png]

8. Output received from PDF.co is in json format and contains fields such as url, pagecount, error, etc. Here URL is referring to PDF output report.
[Screenshot_8.png]

9. Here's how PDF output for deal report looked like when opened.
[Screenshot_9.png]
```

<!-- code block end -->