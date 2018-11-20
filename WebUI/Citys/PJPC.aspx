<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PJPC.aspx.cs" Inherits="PJPC" %>
<%@ Register Src="~/Citys/PJPCII.ascx"TagName="WebUserControl"TagPrefix="uc1" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>平均偏差</title>
    <link href="css/css.css?v=20161020" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
      <link rel="stylesheet" type="text/css" href="css/jquery.citypicker.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>

    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs --> 
    <script type="text/javascript" src="JS/gridLines.js"></script>
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>


</head>
<body>
    <div id="tabbtn">
       <ul id="tabItem" runat="server">
       </ul>
    </div>
    <div id="contentNone">
          <uc1:WebUserControl ID="WebUserControl" runat="server"  />  
    </div>
</body>
</html>
