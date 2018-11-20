<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryFilter.aspx.cs" Inherits="EvaluateHtml_QueryFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>月查询检索功能</title>
 <link href="css/Evaluate.css" rel="stylesheet" type="text/css" />
  <link href="css/QueryFilter.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-1.9.1.js"></script>
 <script language="javascript" type="text/javascript" src="JS/QueryFilter.js"></script>
   <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
     <!-- stockjs -->
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>
</head>
<body>
  <div style=" width:80%; margin-left:auto; margin-right:auto; ">
    <div class="divTop" >
    <div>
       <div  class="dateSelect" id="yearSelect" runat="server">

      </div>
       <div  style=" float:left; margin-left:10px;" id="listTable">
            <div id="rd1" class="radioChecked"><a href="javascript:radioClick('rd1');">列表</a></div>
            <div id="rd2" class="radioUnChecked"><a href="javascript:radioClick('rd2');">图标</a></div>
       </div>
       <input type="button" style=" float:left;  margin-left:20px;" id="ScanBack" class="button" value="查询" onclick="InitTable()" />
       <input type="button" style=" float:left;  margin-left:20px;" id="Button1" class="button" value="导出" onclick="ExportTable()" />
    </div>
    </div>
    <div id="leftTable" class="score" style="width: 100%; overflow:hidden;">
    <div id="coutTable0" ></div>
   <div id="container" style="width: 100%; height:450px; margin: 0 auto; display:none;"></div>
    </div>
        <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
        <div  id="personMonth" class="OnlyOne">
        </div>
    </div>
        <div id="showEveryDay" class="hidden">
        <div  id="personDay" class="OnlyOne">
        </div>
    </div>
    </div>
    <form id="form1" runat="server">
     <asp:HiddenField id="Element" runat="server" />
   
     <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility"/>
    </form>
</body>
</html>

