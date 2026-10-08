## How to merge PDF documents from urls for PDF merging API in Delphi and PDF.co Web API PDF.co Web API: the flexible Web API that includes full set of functions from e-signature requests to data extraction, OCR, images recognition, pdf splitting and pdf splitting. Can also generate barcodes and read barcodes from images, scans and pdf.

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

<!-- code block begin -->

##### **MergePDFDocumentsFromURLs.dpr:**
    
```
program MergePDFDocumentsFromURLs;

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
  IdURI,
  ByteScoutWebApiExec in 'ByteScoutWebApiExec.pas';

const
    // The authentication key (API Key).
    // Get your own by registering at https://app.pdf.co
    API_KEY: string = '********************************************'
	// Destination PDF file name
	DESTINATION_FILE: string = 'result.pdf';
    // Direct URLs of PDF files to merge
	SOURCE_FILES: array [0..1] of string = (
        'https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample1.pdf',
        'https://bytescout-com.s3.amazonaws.com/files/demo-files/cloud-api/pdf-merge/sample2.pdf'
    );
var
    query: string;
    file_name: string;
    waiting_any_key: char;
    files: string;
    i: integer;

begin
    try
        // Prepare URL for `Merge PDF` API call
        files := '';
        for i := 0 to High(SOURCE_FILES) do begin

            if (Length(files) > 0) then
                files := files + ',';
            files := files + SOURCE_FILES[i];
        end;
		query := TIdURI.URLEncode(Format('https://api.pdf.co/v1/pdf/merge' +
            '?name=%s&url=%s',
            [ExtractFileName(DESTINATION_FILE), files]));

        if (WebAPIExec(query, API_KEY, file_name)) then
            Writeln(Format('Generated PDF file saved as "%s" file.', [file_name]));
    finally
        Writeln('Press any key to continue...');
        Readln(waiting_any_key);
    end;
end.

```

<!-- code block end -->