<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportImgTest.aspx.cs" Inherits="ReportProduce_ExportImgTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Chart/highstock.js" type="text/javascript"></script>
    <script src="../JS/Chart/modules/exporting.js" type="text/javascript"></script>
    <script src="../JS/ExportImg.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" action="../EvaluateHtml/ExportChart.ashx" runat="server" method="post">
    
    <input id="svgContent" name="svgContent" type="hidden" />
    <input id="expSub" name="expSub" type="submit" value="导出图片" />
    <%--<div id="exportBtn" style="width:50px;height:30px;background-color:Blue;"><a id="exportUrl">导出图片</a></div>--%>
    </form>
    <div id="container0"></div>
    <div id="container1"></div>
    <div id="container2"></div>
</body>
</html>
