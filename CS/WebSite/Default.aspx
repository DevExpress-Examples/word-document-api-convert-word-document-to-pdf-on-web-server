<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v12.1, Version=12.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v12.1, Version=12.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v12.1, Version=12.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v12.1, Version=12.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxed" %>
<%@ Register Assembly="DevExpress.Web.v12.1, Version=12.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dxcb" %>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8">
</head>
<body>

    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>

    <form id="Form1" runat="server">
        <div class="page">
            <div class="header">
                <div class="title">
                    <h1>
                        Convert to PDF via RichEdit document server
                    </h1>
                </div>
            </div>
            <div class="main">
                <asp id="BodyContent" runat="server">
    <script type="text/javascript">
    <!--
        function Uploader_TextChanged() {
            
            uploader.UploadFile();
        }
        function Uploader_FileUploadComplete(s, e) {
            if (e.isValid)
                window.location.href = e.callbackData;
        }
//--></script>
    <dxuc:ASPxUploadControl ID="ASPxUploadControl1" runat="server" OnFileUploadComplete="ASPxUploadControl_FileUploadComplete"
        ClientInstanceName="uploader">
        <ValidationSettings MaxFileSize="2048576">
        </ValidationSettings>
        <ClientSideEvents TextChanged="function(s, e) { Uploader_TextChanged(); }" FileUploadComplete="function(s, e) {Uploader_FileUploadComplete(s, e); }"/>
    </dxuc:ASPxUploadControl>
</asp>
            </div>
    </form>
</body>
</html>
