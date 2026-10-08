## How to add text and images to PDF in Delphi with PDF.co Web API What is PDF.co Web API? It is the Rest API that provides set of data extraction functions, tools for documents manipulation, splitting and merging of pdf files. Includes built-in OCR, images recognition, can generate and read barcodes from images, scans and pdf.

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

##### **AddImagesToExistingPDF.dpr:**
    
```
program AddImagesToExistingPDF;

//*******************************************************************************************//
//                                                                                           //
// Download Free Evaluation Version From: https://bytescout.com/download/web-installer       //
//                                                                                           //
// Also available as Web API! Get Your Free API Key: https://app.pdf.co/signup               //
//                                                                                           //
// Copyright (c) 2017-2020 ByteScout, Inc. All rights reserved.                                //
// https://www.bytescout.com                                                                 //
// https://pdf.co                                                                            //
//                                                                                           //
//*******************************************************************************************//

{$APPTYPE CONSOLE}

{$R *.res}

uses
    System.SysUtils,
    Classes,
    IdURI,
    ByteScoutWebApiExec in 'ByteScoutWebApiExec.pas';

var
    query: string;
    file_name: string;
    waiting_any_key: char;

const
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    API_KEY: string = '********************************************'
    // Direct URL of source PDF file.
    SOURCE_FILE_URL: string = 'https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/sample.pdf';
    // Comma-separated list of page indices (or ranges) to process. Leave empty for all pages. Example: '0,2-5,7-'.
    PAGES: string = '';
    // PDF document password. Leave empty for unprotected documents.
    PASSWORD: string = '';

    // Destination PDF file name
    DESTINATION_FILE: string = 'result.pdf';

    // Image params
    TYPE1: string = 'image';
    X1: integer = 400;
    Y1: integer = 20;
    WIDTH1: integer = 119;
    HEIGHT1: integer = 32;
    IMAGE_URL: string = 'https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-edit/logo.png';

begin
    try
        // Prepare URL for `PDF Edit` API call
        query := TIdURI.URLEncode(Format('https://api.pdf.co/v1/pdf/edit/add' +
            '?name=%s&password=%s&pages=%s' +
            '&url=%s&type=%s&x=%d&y=%d&width=%d&height=%d&urlimage=%s',
            [ExtractFileName(DESTINATION_FILE), PASSWORD, PAGES,
            SOURCE_FILE_URL, TYPE1, X1, Y1, WIDTH1, HEIGHT1, IMAGE_URL]));

        if (WebAPIExec(query, API_KEY, file_name)) then
            Writeln(Format('Generated file saved to "%s" file."', [file_name]));
    finally
        Writeln('Press any key to continue...');
        Readln(waiting_any_key);
    end;
end.

```

<!-- code block end -->    

<!-- code block begin -->

##### **ByteScoutWebApiExec.pas:**
    
```
unit ByteScoutWebApiExec;

interface

function WebAPIExec(query, api_key: string; var file_name: string): boolean;

implementation

uses
    System.SysUtils,
    Classes,
    IdHTTP,
    IdURI,
    IdSSL,
    IdSSLOpenSSL,
    System.JSON;

function WebAPIExec(query, api_key: string; var file_name: string): boolean;
var
    http: TIdHTTP;
    http_file_downloader: TIdHTTP;
    response_stream: TStringStream;
    response: string;
    io_handler: TIdSSLIOHandlerSocketOpenSSL;
    response_json: TJSONObject;
    is_error: TJSONBool;
    file_url: string;
    file_stream: TFileStream;
    error_message: TJSONString;
begin
    Result := false;
    file_name := '';
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
    http_file_downloader := nil;
    try
        try
            // Set API Key
            http.Request.CustomHeaders.AddValue('x-api-key', api_key);
            // Execute request
            http.Get(query, response_stream);

            // Parse JSON response
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

                Result := true;
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
        if (Assigned(http_file_downloader)) then
            http_file_downloader.Free();
    end;
end;

end.

```

<!-- code block end -->