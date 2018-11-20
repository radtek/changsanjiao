<%@ Page Language="C#" AutoEventWireup="true" CodeFile="text.aspx.cs" Inherits="text" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title></title>
    <script language="javascript" type="text/javascript">
        var id = "<%=id %>";
    </script>
    <script language="javascript" type="text/javascript"></script>    
    <link href="css/PublicAspx.css" rel="stylesheet" type="text/css" />
<%--    <link href="css/icons.css" rel="stylesheet" type="text/css" />--%>
    <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
     <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
    <script language="javascript" type="text/javascript" src="JS/text.js"></script>
        <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageFrameText.js"></script>
    <script language ="javascript" type="text/javascript" src="JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="DatePicker/WdatePicker.js"></script>
</head>
<body id="Body1" runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;" >
    <div class="contentNone1" >
    <div id="contentImg" runat="server" style=" float:left; width:84%">
    <div  id="text" >
    <div class="title" id="title" ></div>
    <div  class="author" id="author"></div>
    <hr  color="red"/>
    <div  class="writing" id="writing"></div>
    </div>
    </div>
    <div id="selectTime" class="panel" style=" float:right; width:15%"   >
    </div>
    </div>
</body>
</html>
