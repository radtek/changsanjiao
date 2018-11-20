<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisFore.aspx.cs" Inherits="HealthyWeather_VisFore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="css/AirEleFore.css" rel="stylesheet" />


    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script src="../JS/jquery-1.10.2.js"></script>
    <%--<script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>--%>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/bootstrap-select.min.js"></script>
   <%-- <script src="js/highcharts.js"></script>--%><script src="js/highcharts.src.js"></script>
    <script src="js/VisFore.js"></script>
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
            <div class="form-group col-sm-4 f">
                <label for="site" class="col-sm-2 control-label" style="padding-right: 0;">站点:</label>
                <div class="col-sm-8">
                    <select class="selectpicker" multiple id="site">
                        <%--<option value="58460">金山</option>
                        <option value="58370">浦东</option>
                        <option value="58463">奉贤</option>--%>
                    </select>

                </div>
            </div>

            <input type="button" class="btn btn-default" onclick="query()" value="查询" />
        </div>
        <div id="container" style="width: 70%; margin: 0 auto;padding-top: 40px;"></div>
        <div class="table">
            
        </div>
    </form>
</body>
</html>
