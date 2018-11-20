<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadFile.aspx.cs" Inherits="AQI_PageOfficePreview_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <fieldset style="width: 290px">
                <legend class="mainTitle">FileUpload控件典型应用</legend>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="BtnUpload" runat="server" Text="上传" OnClick="BtnUpload_Click" />
                <hr />
                <asp:Label ID="LabMessage1" runat="server" ForeColor="red" /><br />
                <asp:Label ID="LabMessage2" runat="server" />
            </fieldset>
        </div>
    </form>
</body>
</html>
