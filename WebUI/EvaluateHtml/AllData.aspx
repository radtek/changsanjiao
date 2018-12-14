<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllData.aspx.cs" Inherits="EvaluateHtml_AllData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <link href="css/Evaluate2.css" rel="stylesheet" type="text/css" />
        <script src="JS/jquery-1.9.1.js"></script>
    <script src="JS/jquery.table2excel.js"></script>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>

    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="JS/vue.js" type="text/javascript"></script>
    <script src="JS/vue-resource.js" type="text/javascript"></script>
    <script src="JS/AllData.js?V=20180424"></script>
    <title></title>
</head>
<body>
    <div style="margin-left:30px">
    <div><input id="H00" type="text" class="selectDateFormStyle" runat="server" onclick="WdatePicker({ dateFmt: 'yyyy年MM月' })" />
        <button class="button" style="margin-left:20px" onclick="InitTable()">查询</button>
        <button  class="button" id="Button1" style="margin-left:20px"
        onclick="method1('forecastTable')"">导出</button>
    </div>
            <div id="leftTable" class="score">
            <div id="coutTable0" class="chinaTable" >
            </div>
        </div>
        </div>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
