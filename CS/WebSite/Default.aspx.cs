using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxUploadControl;
using System.IO;
using DevExpress.XtraRichEdit;
using System.Web.Caching;
using DevExpress.Web.ASPxCallback;
using DevExpress.XtraRichEdit.Internal;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        if(!String.IsNullOrEmpty(Request["loadFile"]))
            WritePdfToResponse(Request["loadFile"]);
    }
    void WritePdfToResponse(string fileName) {
        object uploadedFileBytes = Page.Session["UploadedFile"];
        if(uploadedFileBytes == null)
            return;
        MemoryStream stream = MemoryStreamHelper.FromBytes(uploadedFileBytes);
        Page.Session["UploadedFile"] = null;
        if(stream == null)
            return;
        stream.WriteTo(Page.Response.OutputStream);
        Page.Response.ContentType = "application/pdf";
        Page.Response.HeaderEncoding = System.Text.Encoding.UTF8;
        Page.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.pdf", Path.GetFileNameWithoutExtension(fileName)));
        Page.Response.End();
    }
    protected void ASPxUploadControl_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e) {
        ASPxUploadControl uploadControl = (ASPxUploadControl)sender;
        UploadedFile uploadedFile = uploadControl.UploadedFiles[0];
        e.CallbackData = Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) + String.Format("?loadFile={0}", uploadedFile.FileName);
        try {
            Stream pdfStream = ConvertToPdf(uploadedFile.FileContent, uploadedFile.FileName);
            Session["UploadedFile"] = MemoryStreamHelper.ToBytes(pdfStream);
        }
        catch(Exception ex) {
            e.ErrorText = ex.Message;
            e.IsValid = false;
        }
    }
    Stream ConvertToPdf(Stream stream, string fileName) {
        RichEditDocumentServer server = new RichEditDocumentServer();
        server.LoadDocument(stream, FileExtensionHelper.GetDocumentFormat(fileName));
        MemoryStream memoryStream = new MemoryStream();
        server.ExportToPdf(memoryStream);
        return memoryStream;
    }    
}
public static  class MemoryStreamHelper {
    public static byte[] ToBytes(Stream stream) {
        stream.Position = 0;
        byte[] buf = new byte[stream.Length];
        stream.Read(buf, 0, (int)stream.Length);
        return buf;
    }
    public static MemoryStream FromBytes(object bytes) {
        byte[] buf = bytes as byte[];
        MemoryStream stream = new MemoryStream(buf);
        return stream;
    }
}
public static class FileExtensionHelper {
    
    static Dictionary<string, DocumentFormat> useFormat = new Dictionary<string,DocumentFormat>();

    static FileExtensionHelper () {
        useFormat.Add("txt", DocumentFormat.PlainText);
        useFormat.Add("docx", DocumentFormat.OpenXml);
        useFormat.Add("doc", DocumentFormat.Doc);
        useFormat.Add("rtf", DocumentFormat.Rtf);
        useFormat.Add("odt", DocumentFormat.OpenDocument);
        useFormat.Add("htm", DocumentFormat.Html);
        useFormat.Add("mht", DocumentFormat.Mht);
        useFormat.Add("epub", DocumentFormat.ePub);
    }     

     static public DocumentFormat GetDocumentFormat(string fileName)
    {
        DocumentFormat format;
        string extension = Path.GetExtension(fileName).TrimStart('.').ToLower();
        if (String.IsNullOrEmpty(extension))
            return DocumentFormat.Undefined;
        if (useFormat.TryGetValue(extension, out format)) return format;
        return DocumentFormat.Undefined;
    }
}
