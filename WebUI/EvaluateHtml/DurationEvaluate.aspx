<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DurationEvaluate.aspx.cs" Inherits="EvaluateHtml_DurationEvaluate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>分段评分</title>
 <link href="css/Evaluate.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script language="javascript" type="text/javascript" src="JS/DurationEvaluate.js"></script>
   <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body>
    <div style=" width:90%; margin-left:auto; margin-right:auto;">
    <div class="divTop" >
    <div>
        <div class="checkStyle">
            <div class="checkLable" style=" margin-top:4px;">评分时间</div>
            <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="InitTable()" onclick="WdatePicker({dateFmt:'yyyy年MM月'})"/>
             <input type="button" style=" float:left;  margin-left:20px;" id="ScanBack" class="button" value="查询" onclick="InitTable()" />
             <input type="button" style=" float:left;  margin-left:20px;" id="Button1" class="button" value="导出" onclick="OutTable()" />
        </div>
    </div>
    </div>
        <div style=" clear:both;"></div>
    <div id="leftTable" class="leftTable">
    <div class="label">空气质量等级预报情况（日数/单位：天）</div>
    <div id="coutTable0" style=" padding-right:50px;"></div>
    </div>
        <div id="Div2" class="rightTable">
    <div class="label" style=" margin-left:50px;">首要污染物预报情况（日数/单位：天）</div>
    <div id="coutTable1" style=" padding-left:50px;"></div>
    </div>
    <div id="rightTable" class="leftTable">
    <div class="label" style=" margin-top:8px;">空气质量等级预报情况（准确率/单位：百分比）</div>
    <div id="coutTable2" style=" padding-right:50px;"></div>
    </div>
     <div id="Div3" class="leftTable">
    <div class="label" style=" margin-top:8px;margin-left:50px;">首要污染物预报情况（准确率/单位：百分比）</div>
    <div id="coutTable3" style=" padding-left:50px;"></div>
    </div>
    <div id="Div1" class="leftTable">
    <div class="label" style=" margin-top:8px;">IAQI主客观预报准确率及综合评分(准确率/单位：百分比）</div>
    <div id="coutTable4" style=" padding-right:50px;"></div>
    </div>

    </div>
            <form id="form1" runat="server">
     <asp:HiddenField id="Element" runat="server" />
    <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility" />
      
    </form>
</body>
</html>
