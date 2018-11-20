<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForemanDaily.aspx.cs" Inherits="HealthyWeather_ForemanDaily" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="css/ForemanDaily.css" rel="stylesheet" />

    <script src="../JS/jquery-1.10.2.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="../Ext/adapter/ext/ext-base.js"></script>
    <script src="../Ext/ext-all.js"></script>
    <script src="../Ext/ext-lang-zh_CN.js"></script>
    <script src="../JS/Utility.js"></script>
    <script src="js/bootstrap-select.min.js"></script>
    <script src="js/ForemanDaily.js?v=20170801"></script>
    <script src="js/exporting.js"></script>
    <script src="js/highcharts.js"></script>
    <script src="js/highcharts-zh_CN.js"></script>
    <script src="js/no-data-to-display.js"></script>
    <script src="../My97DatePicker/WdatePicker.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="top" class="top">
            <div class="_top">
                <span id="attrName" class="attrName">预报员：</span>
                <span id="forecaster" class="forecaster font_color"></span>
                <span id="forecastTime" class="forecastTime">发布时间：</span>
                <span><input type="text" id="time"class="time"onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd' });"/></span>
                <input type="button" value="查询" id="query" onclick="clickQuery()" class="query" />
                <input type="button" value="保存" id="save" onclick="clickSave()" class="save" />
            </div>
        </div>
        <div class="content">
            <div class="table">
                <div class="titleText"><span>领班逐日预报</span></div>
                <div class="table-content" id="table-content">
                    <table style="width:100%;">
                        <tbody>
                            <tr >
                                <td class="title">预报员</td>
                                <td class="title">预报时间</td>
                                <td class="title">AQI</td>
                                <td class="title">等级</td>
                                <td class="title">首要污染物</td>
                            </tr>
                            <tr>
                                <td class="t-con" id="people">xxx</td>
                                <td class="t-con" id="foreTime"></td>
                                <td class="t-con" id="par_aqi" onclick="show(id)"><input type="text"class="aqi" style="border:none" id="aqi" /><span id="td_value"></span></td>
                                <td class="t-con" id="grade"></td>
                                <td class="t-con" id="par_poll" onclick="show(id)">
                                    <div id="poll"><select id="multSel" class="selectpicker show-tick" multiple data-live-search="false"></select></div>
                                    <span id="poll_val"></span></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div id="container"  style="width:65%;height:300px;margin:80px auto;"></div>
        </div>
    </form>
</body>
</html>
