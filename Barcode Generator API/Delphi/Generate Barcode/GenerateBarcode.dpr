program GenerateBarcode;

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
    // Result file name
    RESULT_FILE_NAME: string = 'barcode.png';
	// Barcode type. See valid barcode types in the documentation https://secure.bytescout.com/cloudapi.html#api-Default-barcodeGenerateGet
    BARCODE_TYPE: string = 'Code128';
    // Barcode value
	BARCODE_VALUE: string = 'qweasd123456';

var
    query: string;
    file_name: string;
    waiting_any_key: char;

begin
    try
        // Prepare URL for `Barcode Generator` API call
        query := TIdURI.URLEncode(Format('https://api.pdf.co/v1/barcode/generate' +
            '?name=%s&type=%s&value=%s',
            [ExtractFileName(RESULT_FILE_NAME), BARCODE_TYPE, BARCODE_VALUE]));

        if (WebAPIExec(query, API_KEY, file_name)) then
            Writeln(Format('Generated barcode saved to "%s" file."', [file_name]));
    finally
        Writeln('Press any key to continue...');
        Readln(waiting_any_key);
    end;
end.
