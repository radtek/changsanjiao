<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimePM.aspx.cs" Inherits="TimePM" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>过去三天PM2.5变化</title>
  
<%--    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>--%>
    <script language="javascript" type="text/javascript" src="js/TimePM.js?v=18"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
   
    <!-- stockjs --> 
  
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>




  
</head>
<body style="background: rgb(1,50,83)">
     <div id="container1" style="width: 100%; height: 100%; margin:0 0 0 0; "></div>
       <script language="javascript" type="text/javascript">
           var lastTab = "<%=m_FirstTab %>"; //当前选中的污染物
           var station = "<%=m_Station %>"
           doQueryChart();
    </script>
</body>
</html>
