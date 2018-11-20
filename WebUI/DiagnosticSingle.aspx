<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DiagnosticSingle.aspx.cs" Inherits="SingleProvince_DiagnosticSingle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>江西省环境气象监测预报服务平台（1.0版）</title>
    <link href="css/main_JiangXi.css" rel="stylesheet" type="text/css" />
        <link href="css/icons_JX.css?v=20160510" rel="stylesheet" type="text/css" />
    <link href="css/imageViewer.css" rel="stylesheet" type="text/css" />
    <link href="css/ext-patch.css" rel="stylesheet" type="text/css" />   
    <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>
    <link href="Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="Ext/ext-all.js" type="text/javascript"></script>
        <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>

         <script language="javascript" type="text/javascript" src="JS/Login.js"></script>
     <!-- 系统主界面 --> 
    <script language="javascript" type="text/javascript" src="JS/GISToolbarSingleProvince.js"></script>
   
    <script language="javascript" type="text/javascript" src="JS/OtherViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fuHe.js"></script>
    
    <script language="javascript" type="text/javascript" src="JS/Password.js"></script>

    <!-- 海洋气象 --> 
    <script language="javascript" type="text/javascript" src="JS/TreeMenu.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageProduct.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ForecastPanel.js"></script>
    <script language="javascript" type="text/javascript" src="JS/FlexGrid.js"></script>
    <script language="javascript" type="text/javascript" src="JS/PatrolCheck.js"></script>
    
    <!-- 图片浏览 --> 
    <script type="text/javascript" src="JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="JS/Chart/modules/exporting.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageFrame.js"></script>
    <script language="javascript" type="text/javascript" src="JS/tableQuxian.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
    
<%--    <script src="../JS/ProductPrev.js" type="text/javascript"></script>--%>
    <style> 
        a{ text-decoration:none} 
        a{ color:#0000FF} 
       html,body{ margin:0px; height:100%;} 
    </style> 
    
    <script src="SingleProvince/JS/tomainContentSingle.js" type="text/javascript"></script>
     <script type="text/javascript">
         Ext.onReady(function () {
             tomainviewer('diagnostic');
         });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
