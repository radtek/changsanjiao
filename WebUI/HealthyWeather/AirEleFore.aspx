<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirEleFore.aspx.cs" Inherits="HealthyWeather_AirEleFore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/AirEleFore.css" rel="stylesheet" />


    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script src="js/highcharts.js"></script>
    <script src="js/AirEleFore.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal" role="form">
        <div class="top">
            <div class="form-group col-sm-4 f">
                <label for="time" class="col-sm-2 control-label" style="padding-right: 0;">时间:</label>
                <div class="col-sm-8">
                    <input type="text" class="form-control" id="time" onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd HH:00:00' });">
                </div>
            </div>
            <input type="button" class="btn btn-default" onclick="query()" value="查询"/>
        </div>
        <div class="ele">
            <div class="checkLable" style="width: 75px">选择要素：</div>
            <div id="c1" class="radioChecked" style="margin-left: 6px; margin-top: 20px;"><a href="javascript:radioClickModule('c1','PM2.5','54324');">PM2.5</a></div>
            <div id="c2" class="radioUnChecked" style="margin-top: 20px;"><a href="javascript:radioClickModule('c2','PM10','54497');">PM10</a></div>
            <div id="c4" class="radioUnChecked" style="margin-top: 20px;"><a href="javascript:radioClickModule('c4','SO','54497');">SO2</a></div>
            <div id="c3" class="radioUnChecked" style="margin-top: 20px;"><a href="javascript:radioClickModule('c3','O3','54237');">03</a></div>
            <div id="c5" class="radioUnChecked" style="margin-top: 20px;"><a href="javascript:radioClickModule('c5','NO2','54237');">NO2</a></div>
            <div id="c6" class="radioUnChecked" style="margin-top: 20px;"><a href="javascript:radioClickModule('c6','CO','54237');">CO</a></div>
        </div>
        <div id="container" style="width: 80%;margin:0 auto; height: 500px; padding-top: 80px;"></div>
    </form>
</body>
</html>
