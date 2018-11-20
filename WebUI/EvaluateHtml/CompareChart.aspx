<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompareChart.aspx.cs" Inherits="EvaluateHtml_CompareChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IAQI曲线</title>
 <link href="css/Evaluate.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script language="javascript" type="text/javascript" src="JS/CompareChart.js?v=20170818"></script>
   <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs -->
<script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
<script type="text/javascript" src="../JS/Chart/highstock.js"></script>
<script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>
</head>
<body>
    <div style=" width:90%; margin-left:auto; margin-right:auto;">
        <div class="divTop" >
    <div>
        <div class="checkStyle">
            <div class="checkLable" style=" margin-top:4px;">评分时间</div>
            <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="InitTable()" onclick="WdatePicker({dateFmt:'yyyy年MM月'})"/>
             <input type="button" style=" float:left;  margin-left:20px;" id="ScanBack" class="button" value="查询" onclick="InitTable()" />
        </div>
    </div>
    </div>
        <div style=" clear:both;"></div>
    <div id="leftTable" class="leftTable" style=" width:100%;">
    <div id="container0" style="width: 100%; height:450px; margin: 0 auto; float:left;"></div>
    <div id="container1" style="width: 100%; height:450px; margin: 0 auto;float:left;"></div>
    <div id="container2" style="width: 100%; height:450px; margin: 0 auto;float:left;"></div>
    <div id="container3" style="width: 100%; height:450px; margin: 0 auto;float:left;"></div>
    <div id="container4" style="width: 100%; height:450px; margin: 0 auto;float:left;"></div>
    </div>
    </div>
</body>
</html>
