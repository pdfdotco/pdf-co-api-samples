'*******************************************************************************************'
'                                                                                           '
' Get API Key https://app.pdf.co/signup                                                     '
' API Documentation: https://developer.pdf.co/api/index.html                                '
'                                                                                           '
' Note: Replace placeholder values in the code with your API Key                            '
' and file paths (if applicable)                                                            '
'                                                                      '
'*******************************************************************************************'


Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq

Public Class Form1

#Region "File Selection"

    Private Sub btnSelectInputFile_Click(sender As Object, e As EventArgs) Handles btnSelectInputFile.Click

        OpenFileDialog1.ShowDialog()

    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        txtInputPDFFileName.Text = OpenFileDialog1.FileName

    End Sub


#End Region

#Region "Convert PDF to Excel"

    ''' <summary>
    ''' Perform convert PDF to Excel
    ''' </summary>
    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click

        Try

            If (ValidateInputs()) Then

                ' Comma-separated list of page indices (Or ranges) to process. Leave empty for all pages. Example '0,2-5,7-'.
                Const Pages As String = ""

                ' PDF document password. Leave empty for unprotected documents.
                Const Password As String = ""


                ' Checks whether convert to as inline content.
                Dim isInline As Boolean = Convert.ToString(cmbOutputAs.SelectedItem).ToLower() = "inline content"

                ' Destination file name
                Dim DestinationFile As String = String.Format(".\{0}.{1}",
                                                              Path.GetFileNameWithoutExtension(txtInputPDFFileName.Text),
                                                              Convert.ToString(cmbConvertTo.SelectedItem).ToLower())

                ' Create standard .NET web client instance
                Dim webClient As WebClient = New WebClient()

                ' Set API Key
                webClient.Headers.Add("x-api-key", txtCloudAPIKey.Text.Trim())

                ' 1. RETRIEVE THE PRESIGNED URL TO UPLOAD THE FILE.
                ' * If you already have a direct file URL, skip to the step 3.

                ' Prepare URL for `Get Presigned URL` API call
                Dim query As String = Uri.EscapeUriString(
                    String.Format("https://api.pdf.co/v1/file/upload/get-presigned-url?contenttype=application/octet-stream&name={0}",
                                  Path.GetFileName(txtInputPDFFileName.Text)))

                ' Execute request
                Dim response As String = webClient.DownloadString(query)

                ' Parse JSON response
                Dim json As JObject = JObject.Parse(response)

                If json("error").ToObject(Of Boolean)() = False Then

                    ' Get URL to use for the file upload
                    Dim uploadUrl As String = json("presignedUrl").ToString()
                    Dim uploadedFileUrl As String = json("url").ToString()

                    ' 2. UPLOAD THE FILE TO CLOUD.
                    webClient.Headers.Add("content-type", "application/octet-stream")
                    webClient.UploadFile(uploadUrl, "PUT", txtInputPDFFileName.Text)
                    webClient.Headers.Remove("content-type")

                    ' 3. CONVERT UPLOADED PDF FILE TO Excel

                    ' Prepare URL for `PDF To Excel` API call
                    query = Uri.EscapeUriString(
                        String.Format("https://api.pdf.co/v1/pdf/convert/to/{4}?name={0}&password={1}&pages={2}&url={3}&encrypt=true&inline={5}",
                                      Path.GetFileName(DestinationFile),
                                      Password,
                                      Pages,
                                      uploadedFileUrl,
                                      Convert.ToString(cmbConvertTo.SelectedItem).ToLower(),
                                      isInline))

                    ' Execute request
                    response = webClient.DownloadString(query)

                    ' Parse JSON response
                    json = JObject.Parse(response)

                    If json("error").ToObject(Of Boolean)() = False Then

                        ' Get URL of generated Excel file
                        Dim resultFileUrl As String = json("url").ToString()

                        ' Download Excel output file
                        webClient.DownloadFile(resultFileUrl, DestinationFile)

                        MessageBox.Show($"Generated XLS file saved as {DestinationFile} file.", "Success!")

                        ' Open Downloaded output file
                        Process.Start(DestinationFile)
                    Else
                        MessageBox.Show(json("message").ToString())
                    End If
                Else
                    MessageBox.Show(json("message").ToString())
                End If

                webClient.Dispose()

            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        End Try

    End Sub

    ''' <summary>
    ''' Validates form inputs
    ''' </summary>
    ''' <returns></returns>
    Private Function ValidateInputs() As Boolean

        If (String.IsNullOrEmpty(txtCloudAPIKey.Text)) Then
            Throw New Exception("Cloud API key cannot be empty")
        End If

        If (String.IsNullOrEmpty(txtInputPDFFileName.Text)) Then
            Throw New Exception("Input PDf file must be selected/entered.")
        End If

        If (Not System.IO.File.Exists(txtInputPDFFileName.Text)) Then
            Throw New Exception("Input file does not exits.")
        End If

        If (System.IO.Path.GetExtension(txtInputPDFFileName.Text).ToLower() <> ".pdf") Then
            Throw New Exception("Input file must be PDF")
        End If

        If (String.IsNullOrEmpty(Convert.ToString(cmbConvertTo.SelectedItem))) Then
            Throw New Exception("Please select convert to option.")
        End If

        If (String.IsNullOrEmpty(Convert.ToString(cmbOutputAs.SelectedItem))) Then
            Throw New Exception("Please select output-as option")
        End If

        Return True

    End Function


#End Region

End Class
