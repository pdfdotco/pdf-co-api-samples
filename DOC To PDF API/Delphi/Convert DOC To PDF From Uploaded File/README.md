## How to convert DOC to PDF from uploaded file for DOC to PDF API in Delphi and PDF.co Web API PDF.co Web API: the Web API with a set of tools for documents manipulation, data conversion, data extraction, splitting and merging of documents. Includes image recognition, built-in OCR, barcode generation and barcode decoders to decode bar codes from scans, pictures and pdf.

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

##### **ConvertDOCToPDFFromUploadedFile.dpr:**
    
```
program ConvertDOCToPDFFromUploadedFile;

//*******************************************************************************************//
//                                                                                           //
// Download Free Evaluation Version From: https://bytescout.com/download/web-installer       //
//                                                                                           //
// Also available as Web API! Get Your Free API Key: https://app.pdf.co/signup               //
//                                                                                           //
// Copyright � 2017-2020 ByteScout, Inc. All rights reserved.                                //
// https://www.bytescout.com                                                                 //
// https://pdf.co                                                                            //
//                                                                                           //
//*******************************************************************************************//

{$APPTYPE CONSOLE}

{$R *.res}

uses
    System.SysUtils,
    Classes,
    IdHTTP,
    IdURI,
    IdSSL,
    IdSSLOpenSSL,
    System.JSON,
    IdMultipartFormData;

const
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    API_KEY: string = '********************************************'
    // Source DOC or DOCX file
	SOURCE_FILE: string = 'sample.docx';
    // Destination PDF file name
	DESTINATION_FILE: string = 'result.pdf';

var
    http: TIdHTTP;
    http_file_downloader: TIdHTTP;
    http_file_uploader: TIdHTTP;
    response_stream: TStringStream;
    response: string;
    io_handler: TIdSSLIOHandlerSocketOpenSSL;
    response_json: TJSONObject;
    is_error: TJSONBool;
    file_url: string;
    file_stream: TFileStream;
    error_message: TJSONString;
    presigned_url: string;
    uploaded_file_url: string;
    query: string;
    waiting_any_key: char;
    multi_part_form_data_stream: TIdMultiPartFormDataStream;
    file_name: string;

begin
    // Put the necessary libeay32.dll & ssleay32.dll library versions in the
    // program folder
    io_handler := TIdSSLIOHandlerSocketOpenSSL.Create(nil);
    io_handler.SSLOptions.Method := sslvSSLv23;
    io_handler.SSLOptions.SSLVersions := [sslvSSLv23];

    http := TIdHTTP.Create(nil);
    http.HTTPOptions := http.HTTPOptions + [hoForceEncodeParams];
    http.AllowCookies := true;
    http.HandleRedirects := true;
    http.Request.Connection := 'keep-alive';
    http.Request.ContentType := 'application/json; charset=utf-8';
    http.Request.UserAgent := 'User-Agent:Mozilla/5.0 (Windows NT 6.1; ) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36';
    http.IOHandler := io_handler;

    response_stream := TStringStream.Create();
    file_stream := nil;
    http_file_uploader := nil;
    multi_part_form_data_stream := nil;
    http_file_downloader := nil;
    try
        try
            // 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
			// * If you already have a direct file URL, skip to the step 3.

			// Prepare URL for `Get Presigned URL` API call
			query := TIdURI.URLEncode(Format('https://api.pdf.co/v1/file/upload/get-presigned-url' +
                '?contenttype=application/octet-stream&name=%s',
				[ExtractFileName(SOURCE_FILE)]));

            // Set API Key
            http.Request.CustomHeaders.AddValue('x-api-key', API_KEY);
            // Execute request
            http.Get(query, response_stream);
            // Parse JSON response
            response_json := TJSONObject.ParseJSONValue(response_stream.DataString, false) as TJSONObject;
            is_error := response_json.Values['error'] as TJSONBool;
            if (not is_error.AsBoolean) then begin

                // Get URL to use for the file upload
                presigned_url := TJSONString(response_json.Values['presignedUrl']).ToString();
                presigned_url := StringReplace(presigned_url, '"', '', [rfReplaceAll]);
				// Get URL of uploaded file to use with later API calls
				uploaded_file_url := TJSONString(response_json.Values['url']).ToString();
                uploaded_file_url := StringReplace(uploaded_file_url, '"', '', [rfReplaceAll]);

                // 2. UPLOAD THE FILE TO CLOUD.
                http_file_uploader := TIdHTTP.Create(nil);
                http_file_uploader.AllowCookies := true;
                http_file_uploader.HandleRedirects := true;
                http_file_uploader.Request.CustomHeaders.AddValue('x-api-key', API_KEY);
                http_file_uploader.Request.CustomHeaders.AddValue('content-type', 'application/octet-stream');
                file_stream := TFileStream.Create(SOURCE_FILE, fmOpenRead);
                response := http_file_uploader.Put(presigned_url, file_stream);
                file_stream.Free();

                // 3. CONVERT UPLOADED DOC (DOCX) FILE TO PDF

                // Prepare URL for `DOC To PDF` API call
                query := TIdURI.URLEncode(Format('https://api.pdf.co/v1/pdf/convert/from/doc' +
                    '?name=%s&url=%s',
                    [ExtractFileName(DESTINATION_FILE), uploaded_file_url]));

                // Execute request
                response_stream.Clear();
                http.Get(query, response_stream);
                response_json := TJSONObject.ParseJSONValue(response_stream.DataString, false) as TJSONObject;
                is_error := response_json.Values['error'] as TJSONBool;
                if (not is_error.AsBoolean) then begin

                    file_url := TJSONString(response_json.Values['url']).ToString();
                    file_url := StringReplace(file_url, '"', '', [rfReplaceAll]);
                    file_name := ExtractFileName(StringReplace(file_url, '/', '\', [rfReplaceAll]));
                    file_name := IncludeTrailingPathDelimiter(ExtractFilePath(ParamStr(0))) + file_name;
                    http_file_downloader := TIdHTTP.Create(nil);

                    // Download generated file
                    file_stream := TFileStream.Create(file_name, fmCreate);
                    http_file_downloader.Get(file_url, file_stream);

                    Writeln(Format('Generated PDF file saved as "%s" file."', [file_name]));
                end else begin

                    error_message := response_json.Values['message'] as TJSONString;
                    raise Exception.Create(error_message.ToString);
                end;

            end else begin

                error_message := response_json.Values['message'] as TJSONString;
                raise Exception.Create(error_message.ToString);
            end;
        except
            on E: Exception do begin

                response := http.ResponseText;
                Writeln(E.ClassName, ': ', E.Message);
            end;
        end;
    finally
        response_stream.Free();
        http.Free();
        if (Assigned(file_stream)) then
            file_stream.Free();
        if (Assigned(multi_part_form_data_stream)) then
            multi_part_form_data_stream.Free();
        if (Assigned(http_file_uploader)) then
            http_file_uploader.Free();
        if (Assigned(http_file_downloader)) then
            http_file_downloader.Free();
        Writeln('Press any key to continue...');
        Readln(waiting_any_key);
    end;
end.

```

<!-- code block end -->