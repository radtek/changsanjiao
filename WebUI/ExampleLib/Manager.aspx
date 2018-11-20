<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Manager.aspx.cs" Inherits="ExampleLib_Manager" %>

<!doctype html>
<html class="no-js" lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>个例库管理</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="css/plugins/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/plugins/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/utility.css?v=20161228" rel="stylesheet" type="text/css" />
            
</head>
<body class="container-fluid">
    <div class="form-group" style="margin-left:18px;">
        <label for="queryTime" class="control-label">案例时间：</label>
        <input type="text" class="form-control" id="querySTime" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'});"/>&nbsp--&nbsp<input type="text" class="form-control" id="queryETime" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'});"/>
        <button type="button" class="btn btn-default" id="btnQuery">开始查询</button>
    </div>
    <div class="form-group">
        <input type="radio" name="queryType" value="rdoDay" />
        <div id="pnlDay" class="inline">
            <label class="control-label">连续天数：</label>
            <input type="number" min="1" max="50" id="theDays" class="form-control" style="width:70px;" disabled="disabled" value="1" />
        </div>
    </div>
    <div class="form-group">
        <input type="radio" name="queryType" value="rdoTypes"  checked/>
        <div id="pnlTypes" class="inline">
            <label class="control-label">污染等级：</label>
            <select class="form-control" id="sltLvl"></select>
            <label class="control-label">季节：</label>
            <select class="form-control" id="sltSeason" onchange="CreatePollute(this)"></select>
            <label class="control-label">天气类型：</label>
            <select class="form-control" id="sltWeather"></select>
        </div>
    </div>
    <div id="divTable"></div>
  
    <ul class="nav nav-tabs" role="tablist" id="typeTab">
        <li role="presentation" class="active"><a href="#divImg" aria-controls="stationRain" role="tab" data-toggle="tab">图片展示</a></li>
        <li role="presentation"><a href="#divChart" aria-controls="extrameRain" role="tab" data-toggle="tab">小时数据</a></li>
    </ul>
    <div class="tab-content form-horizontal">
        <div role="tabpanel" class="tab-pane active" id="divImg">
        </div>
        <div role="tabpanel" class="tab-pane" id="divChart">
            <div id="chartBack" style=" width:100%; height:300px;">
            </div>
        </div>
    </div>

    <script src="js/plugins/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="js/plugins/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/plugins/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="../JS/highmap/js/highcharts.src.js" type="text/javascript"></script>
    <script src="js/Manager.js?v=20161228" type="text/javascript"></script>
</body>
</html>
