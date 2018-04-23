﻿Imports Microsoft.VisualBasic
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
		If (Not String.IsNullOrEmpty(Request("loadFile"))) Then
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
		Dim uploadControl As ASPxUploadControl = CType(sender, ASPxUploadControl)
		Dim uploadedFile As UploadedFile = uploadControl.UploadedFiles(0)
		e.CallbackData = Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) + String.Format("?loadFile={0}", uploadedFile.FileName)
		Try
			Dim pdfStream As Stream = ConvertToPdf(uploadedFile.FileContent, uploadedFile.FileName)
			Session("UploadedFile") = MemoryStreamHelper.ToBytes(pdfStream)
		Catch ex As Exception
			e.ErrorText = ex.Message
			e.IsValid = False
		End Try
	End Sub
	Private Function ConvertToPdf(ByVal stream As Stream, ByVal fileName As String) As Stream
		Dim server As New RichEditDocumentServer()
		server.LoadDocument(stream, GetDocumentFormat(server, fileName))
		Dim memoryStream As New MemoryStream()
		server.ExportToPdf(memoryStream)
		Return memoryStream
	End Function
	Private Function GetDocumentFormat(ByVal server As RichEditDocumentServer, ByVal fileName As String) As DocumentFormat
		Dim importers As List(Of IImporter(Of DocumentFormat, Boolean)) = GetImporters(server)
		Dim extension As String = Path.GetExtension(fileName).TrimStart("."c).ToLower()
		If String.IsNullOrEmpty(extension) Then
			Return DocumentFormat.Undefined
		End If
		Dim count As Integer = importers.Count
		For i As Integer = 0 To count - 1
			Dim extensions As FileExtensionCollection = importers(i).Filter.Extensions
			If extensions.IndexOf(extension) >= 0 Then
				Return importers(i).Format
			End If
		Next i
		Return DocumentFormat.Undefined
	End Function
	Private Function GetImporters(ByVal server As RichEditDocumentServer) As List(Of IImporter(Of DocumentFormat, Boolean))
		Dim importManagerService As IDocumentImportManagerService = CType(server.GetService(GetType(IDocumentImportManagerService)), IDocumentImportManagerService)
		Return importManagerService.GetImporters()
	End Function
End Class
Public NotInheritable Class MemoryStreamHelper
	Private Sub New()
	End Sub
	Public Shared Function ToBytes(ByVal stream As Stream) As Byte()
		stream.Position = 0
		Dim buf(stream.Length - 1) As Byte
		stream.Read(buf, 0, CInt(Fix(stream.Length)))
		Return buf
	End Function
	Public Shared Function FromBytes(ByVal bytes As Object) As MemoryStream
		Dim buf() As Byte = TryCast(bytes, Byte())
		Dim stream As New MemoryStream(buf)
		Return stream
	End Function
End Class
