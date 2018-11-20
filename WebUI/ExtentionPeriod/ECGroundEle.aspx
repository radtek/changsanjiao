<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ECGroundEle.aspx.cs" Inherits="ExtentionPeriod_ECGroundEle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/GroundEle.css" rel="stylesheet" />

    <script src="../JS/jquery-1.10.2.js"></script>
    <script src="js/bootstrap.min.js"></script>
  <%--  <script src="js/highcharts.js"></script>--%>
    <script src="https://cdn.hcharts.cn/highstock/highstock.js"></script>
    <script src="../My97DatePicker/WdatePicker.js"></script>
    <script src="js/ECGroundEle.js?v=2018071105"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="top">
            <div class="form-group col-sm-4 f">
                <label for="time" class="col-sm-3 control-label" style="padding-right: 0;">开始时间:</label>
                <div class="col-sm-8">
                    <input type="text" class="form-control" id="time" onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd HH:00:00' });"/>
                </div>
            </div>
            <input type="button" class="btn btn-default" onclick="query()" value="查询"/>
        </div>
        <div id="container" style="width:90%;margin:0 auto;"></div>
    </div>
    </form>
</body>
</html>
