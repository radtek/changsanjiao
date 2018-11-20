<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarningMap.aspx.cs" Inherits="ReportProduce_WarningMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../AQI/js/WarningMap.js" type="text/javascript"></script>
    <link href="../css/WarningMap.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="mapOutline" class="mapOutline">
       <img id="baseMap" class="mapImg" src="../AQI/WarningMap/J_PM25_WHF_201603311400_000.png"/>
    </div>
    </form>
</body>
</html>
