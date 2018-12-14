<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllData.aspx.cs" Inherits="EvaluateHtml_AllData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="DatePicker/skin/WdatePicker.css" rel="stylesheet" />
    <link href="css/Evaluate.css" rel="stylesheet" />

    <script src="js/jquery-1.9.1.js"></script>
    <script src="js/jquery.table2excel.js"></script>

    <script src="js/Ext/adapter/ext/ext-base.js"></script>
    <script src="js/Ext/ext-all1.js"></script>
    <script src="js/Ext/ext-lang-zh_CN.js"></script>

    <script src="js/Utility.js"></script>
    <script src="js/highlight-active-input.js"></script>

    <script src="DatePicker/WdatePicker.js"></script>

    <script src="js/vue.js"></script>
    <script src="js/vue-resource.js"></script>
    <script src="js/AllData.js"></script>
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
