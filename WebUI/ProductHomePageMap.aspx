<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductHomePageMap.aspx.cs" Inherits="ProductHomePageMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/reset.css" rel="stylesheet" type="text/css" />
    <link href="css/examples.css" rel="stylesheet" type="text/css" />
    <link href="css/darktooltip.css" rel="stylesheet" type="text/css" />
     <script src="JS/jquery-1.10.2.js" type="text/javascript"></script>
    <link href="Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="Ext/ext-all.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
    <script src="JS/highmap/js/highmaps.js" type="text/javascript"></script>
    <script src="JS/highmap/js/modules/data.js" type="text/javascript"></script>
    <%--<link href="css/ProductHomePage.css" rel="stylesheet" type="text/css" />--%>
    <script src="http://code.highcharts.com/mapdata/custom/world.js"></script>
    <script src="JS/ProductHomePageMap.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="container" style="height: 500px; min-width: 310px; max-width: 800px; margin: 0 auto"></div>

    </div>
    </form>
</body>
</html>
