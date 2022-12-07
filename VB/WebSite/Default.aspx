<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<%@ Page Title="Home Page" Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxuc" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxed" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dxcb" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
	<title />
	<meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
</head>

<body>
	<script src="Scripts/jquery-1.4.1.js" type="text/javascript">
	</script>

	<form id="Form1" runat="server">
		<div class="page">
			<div class="header">
				<div class="title">
					<h1>Convert to PDF via RichEdit document server
					</h1>
				</div>
			</div>

			<div class="main">
				<script type="text/javascript">
					function Uploader_TextChanged() {
						uploader.UploadFile();
					}

					function Uploader_FileUploadComplete(s, e) {
						if (e.isValid)
							window.location.href = e.callbackData;
					}
				</script>

				<dxuc:ASPxUploadControl ID="ASPxUploadControl1" runat="server" OnFileUploadComplete="ASPxUploadControl_FileUploadComplete" ClientInstanceName="uploader">
					<ValidationSettings MaxFileSize="2048576" />
					<ClientSideEvents TextChanged="function(s, e) { Uploader_TextChanged(); }" FileUploadComplete="function(s, e) {Uploader_FileUploadComplete(s, e); }" />
				</dxuc:ASPxUploadControl>
			</div>
		</div>
	</form>
</body>
</html>