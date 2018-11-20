<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Retrieval.aspx.cs" Inherits="Example_Retrieval" %>

<!doctype html>

<html class="no-js" lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>个例库检索分析</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="css/plugins/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/plugins/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/Retrieval.css" rel="stylesheet" type="text/css" />
    <link href="css/utility.css?v=20161228" rel="stylesheet" type="text/css" />
    <script>

    </script>
        
</head>
<body class="container-fluid">
    <div class="form-group">
        <label for="queryTime" class="control-label">匹配时间：</label>
        <input type="text" class="form-control" id="queryTime" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'});"/>

        <label class="control-label">匹配要素：</label>
        <div class='queryFeature checkBox checked' tag="03">海平面气压</div>
        <div class='queryFeature checkBox checked' tag="01">500百帕高度</div>
        <div class='queryFeature checkBox checked' tag="02">850百帕温度</div>
        <div class='checkBox checked' id="chkAll" style="margin-left:15px;">全选</div>

        <button type="button" class="btn btn-default" id="btnQuery">开始分析</button>
    </div>
    
    <div id="divTable"></div>

    <ul class="nav nav-tabs" role="tablist" id="typeTab">
        <li role="presentation" class="active"><a href="#divImg" aria-controls="stationRain" role="tab" data-toggle="tab">图片展示</a></li>
        <li role="presentation"><a href="#divChart" aria-controls="extrameRain" role="tab" data-toggle="tab">小时数据</a></li>
    </ul>
    <div class="tab-content form-horizontal">
        <div role="tabpanel" class="tab-pane active" id="divImg">
            <div></div>
            <div></div>
        </div>
        <div role="tabpanel" class="tab-pane" id="divChart">
            <div id="chartBack" style=" width:100%; height:300px;">
            </div>
        </div>
    </div>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="js/plugins/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/plugins/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="../JS/highmap/js/highcharts.src.js" type="text/javascript"></script>
    <script src="js/Retrieval.js?v=201612281" type="text/javascript"></script>
</body>
</html>
