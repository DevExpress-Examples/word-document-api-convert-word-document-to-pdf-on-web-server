Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxUploadControl
Imports System.IO
Imports DevExpress.XtraRichEdit
Imports System.Web.Caching
Imports DevExpress.Web.ASPxCallback
Imports DevExpress.XtraRichEdit.Internal

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        If Not String.IsNullOrEmpty(Request("loadFile")) Then
            WritePdfToResponse(Request("loadFile"))
        End If
    End Sub
    Private Sub WritePdfToResponse(ByVal fileName As String)
        Dim uploadedFileBytes As Object = Page.Session("UploadedFile")
        If uploadedFileBytes Is Nothing Then
            Return
        End If
        Dim stream As MemoryStream = MemoryStreamHelper.FromBytes(uploadedFileBytes)
        Page.Session("UploadedFile") = Nothing
        If stream Is Nothing Then
            Return
        End If
        stream.WriteTo(Page.Response.OutputStream)
        Page.Response.ContentType = "application/pdf"
        Page.Response.HeaderEncoding = System.Text.Encoding.UTF8
        Page.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.pdf", Path.GetFileNameWithoutExtension(fileName)))
        Page.Response.End()
    End Sub
    Protected Sub ASPxUploadControl_FileUploadComplete(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs)
        Dim uploadControl As ASPxUploadControl = DirectCast(sender, ASPxUploadControl)
        Dim uploadedFile As UploadedFile = uploadControl.UploadedFiles(0)
        e.CallbackData = Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) & String.Format("?loadFile={0}", uploadedFile.FileName)
        Try
            Dim pdfStream As Stream = ConvertToPdf(uploadedFile.FileContent, uploadedFile.FileName)
            Session("UploadedFile") = MemoryStreamHelper.ToBytes(pdfStream)
        Catch ex As Exception
            e.ErrorText = ex.Message
            e.IsValid = False
        End Try
    End Sub
    Private Function ConvertToPdf(ByVal stream As Stream, ByVal fileName As String) As Stream

        Dim server_Renamed As New RichEditDocumentServer()
        server_Renamed.LoadDocument(stream, FileExtensionHelper.GetDocumentFormat(fileName))
        Dim memoryStream As New MemoryStream()
        server_Renamed.ExportToPdf(memoryStream)
        Return memoryStream
    End Function
End Class
Public NotInheritable Class MemoryStreamHelper

    Private Sub New()
    End Sub

    Public Shared Function ToBytes(ByVal stream As Stream) As Byte()
        stream.Position = 0
        Dim buf(stream.Length - 1) As Byte
        stream.Read(buf, 0, CInt(stream.Length))
        Return buf
    End Function
    Public Shared Function FromBytes(ByVal bytes As Object) As MemoryStream
        Dim buf() As Byte = TryCast(bytes, Byte())
        Dim stream As New MemoryStream(buf)
        Return stream
    End Function
End Class
Public NotInheritable Class FileExtensionHelper

    Private Sub New()
    End Sub


    Private Shared useFormat As New Dictionary(Of String, DocumentFormat)()

    Shared Sub New()
        useFormat.Add("txt", DocumentFormat.PlainText)
        useFormat.Add("docx", DocumentFormat.OpenXml)
        useFormat.Add("doc", DocumentFormat.Doc)
        useFormat.Add("rtf", DocumentFormat.Rtf)
        useFormat.Add("odt", DocumentFormat.OpenDocument)
        useFormat.Add("htm", DocumentFormat.Html)
        useFormat.Add("mht", DocumentFormat.Mht)
        useFormat.Add("epub", DocumentFormat.ePub)
    End Sub

     Public Shared Function GetDocumentFormat(ByVal fileName As String) As DocumentFormat
        Dim format As DocumentFormat = Nothing
        Dim extension As String = Path.GetExtension(fileName).TrimStart("."c).ToLower()
        If String.IsNullOrEmpty(extension) Then
            Return DocumentFormat.Undefined
        End If
        If useFormat.TryGetValue(extension, format) Then
            Return format
        End If
        Return DocumentFormat.Undefined
     End Function
End Class
