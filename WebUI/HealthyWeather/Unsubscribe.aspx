<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Unsubscribe.aspx.cs" Inherits="HealthyWeather_Unsubscribe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>取消订阅</title>

    <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="css/Unsubscribe.css" />

    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/Unsubscribe.js?V=1129"></script>
</head>
<body class="container-fluid">
    <form action="" class="form-inline" style="margin: 10px auto;">
        <div class="form-group">
            <label for="sd" class="col-md-2 control-label" style="text-align: right">申请时间：</label>
            <div class="col-sm-3">
                <input type="text" class="form-control" runat="server" id="startDate" onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd' });" />
            </div>
            <label for="endDate" class="col-md-2 control-label" style="text-align: right">结束时间：</label>
            <div class="col-sm-3">
                <input type="text" class="form-control" runat="server" id="endDate" onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd' });" />
            </div>


        </div>
        <div class="form-group">
            <label for="selUserGroup" class="col-md-2 control-label" style="text-align: right">用户组：</label>
            <div class="col-sm-3">
                <select class="form-control" id="selUserGroup"></select></div>
            <label for="selName" class="col-md-2 col-md-offset-2 control-label" style="text-align: right">用户名：</label>
            <div class="col-md-3">
                <select class="form-control" id="selName" onchange="Query()">
                    <option>全部</option>
                </select></div>
        </div>

        <div class="form-group relative">
            <label for="applyType" class="col-md-2 control-label" style="text-align: right">申请类型：</label>
            <div class="col-sm-3">
                <select class="form-control" id="applyType" onchange="Query()">
                    <option>全部</option>
                    <option>邮件</option>
                    <option>短信</option>
                </select></div>
            <!--王斌  2017.5.16-->
            <label for="diseaseType" class="col-md-2 col-md-offset-2 control-label hidden" style="text-align: right">疾病类型：</label>
            <div class="col-md-3">
                <select class="form-control hidden" id="diseaseType" onchange="Query()"></select></div>
            <button type="button" class="btn btn-default cx" onclick="Query()">查询</button>
            <button type="button" class="btn btn-default ty" onclick="Agree()">同意</button>
            <button type="button" class="btn btn-default ty" onclick="Del()">删除</button>
        </div>

        <div class="tab" style="margin: 0px auto; margin-top: 40px; width: 95%;">
            <table class="display dataTable no-footer" id="cancelTab" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
            </table>

        </div>
    </form>

</body>
</html>
