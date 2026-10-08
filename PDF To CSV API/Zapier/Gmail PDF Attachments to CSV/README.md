## gmail PDF attachments to CSV in Zapier and PDF.co Web API PDF.co Web API is the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

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
1. This is input Gmail email with PDF attachment. Upon receiving this email, we aim to convert PDF attachment to CSV file format.
[Screenshot_1.png]

2. PDF attachment is containing product information such as name, units and price.
[Screenshot_2.png]

3. We're creating zap with "Gmail" as input App and "New Email Matching Search" as preferred Event.
[Screenshot_3.png]

4. Next step is to provide source Gmail account with Search criteria. Here we have specified criteria like email should be from specific address and also should contain attachment. Only email with specific criteria will be considered for this zap.
[Screenshot_4.png]

5. Now Gmail source is configured, it’s time to test input data. Prior to hitting test trigger button, have one mail received which qualifies mail filter criteria. It's necessary for test trigger detection. If we review Test email data, It contains all email information along with attachment information like shown below.
[Screenshot_5.png]

6. In next step to proceed with "PDF to CSV" setup, we have "PDF.co" as an input app and selected "PDF to Anything Converter" as method. This method facilitates converting PDF to most of formats such as JPG, CSV, XML, JSON, etc.
[Screenshot_6.png]

7. Different parameter values for "Customize PDF to Anything" are filled in this step. As we intend to have output in “CSV” format, "Output Format" parameter is having value "CSV". In "PDF URL" field we have provided "All Attachment" as input value. Lastly for "Name" parameter which represents name of output file, we have provided fixed name "Output.csv".
[Screenshot_7.png]

8. Prior to sending request to PDF.co, we analyze input data which will be passed. 
[Screenshot_8.png]

9. After request is completed, it'll send json output data such as "url", "pageCount", "error", etc. Here "URL" field contains output csv file URL. 
[Screenshot_9.png]

10. If output csv file URL is opened/downloaded and file is opened, we can see it contains all PDF data in CSV format. With this we’ve achieved creating zap which will detect attachment from specific source, and process it with converting to output CSV. 
[Screenshot_10.png]

```

<!-- code block end -->