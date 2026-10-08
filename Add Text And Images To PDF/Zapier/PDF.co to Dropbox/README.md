## PDF in Zapier using PDF.co Web API PDF.co Web API is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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
1. For this demo we already have input source configured. It can be anything based on requirement. We have selected PDF.co as input app and selected "PDF Filler" as preferred event. This "PDF Filler" event can add multiple text or images PDF document.
[Screenshot_1.png]

2. As we can see in screenshot, we have provided various PDF.co configuration options. In "Direct Link to Source PDF", we have provided input file which will be processed. In "Text Objects" we have provided input text in pre-defined format such as X location, Y location, page index, input text, font size, font family, etc. After that we've provided output file name.
[Screenshot_2.png]

3. Now we're ready to send input request to PDF.co. We're shown input request data here. If it looks good then we'll proceed with "TEST & REVIEW".
[Screenshot_3.png]"

4. Once response received from PDF.co, we can see output contains URL which contains processed output file. 
[Screenshot_4.png]

5. This is how output file looks like. In top-left corner it contains text "Hello World!" like we've specified in input request. 
[Screenshot_5.png]

6. Let's proceed with saving resulting file to Dropbox. Here we've selected input app as "Dropbox" and event as "Upload File".
[Screenshot_6.png]

7. With Dropbox account is added, it's time to configure upload details. In field for "Folder", we've specified output Dropbox folder name. In "File" field, we've specified output file URL from PDF.co response. Then we've provided input fields like whether overwrite enabled, output file name and lastly extension of output file.
[Screenshot_7.png]

8. We're ready send request for save file to Dropbox. We’re presented with request data before sending to Dropbox API. If it looks good then we can proceed with "TEST & REVIEW".
[Screenshot_8.png]

9. Here's how successful Dropbox file save request’s response data looks like. 
[Screenshot_9.png]

10. In Dropbox output folder we can find new output file successfully saved.
[Screenshot_10.png]
```

<!-- code block end -->