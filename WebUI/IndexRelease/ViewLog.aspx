<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewLog.aspx.cs" Inherits="IndexRelease_ViewLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="css/ViewLog.css" rel="stylesheet" />

    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.10.2.js"></script>
    <script src="../JS/jquery.dataTables.js"></script>
    <script src="../DatePicker/WdatePicker.js"></script>
    <script src="js/Utility.js"></script>
    <script src="js/ViewLog.js"></script>

</head>
<body>
    <form class="form-horizontal" role="form" runat="server">
        <div class="header">
            <div class="form-group" style="margin-top:15px;">
                <label for="" class="col-sm-1 control-label">开始时间:</label>
                <div class="col-sm-2">
                    <input type="text" class="form-control" id="startTime" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' });" />
                </div>
                <label for="" class="col-sm-1 control-label">结束时间:</label>
                <div class="col-sm-2">
                    <input type="text" class="form-control" id="endTime" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' });" />
                </div>
            </div>
            <div class="form-group" style="margin-top:15px;">
                <label for="people" class="col-sm-1 control-label">人员:</label>
                <div class="col-sm-2">
                    <select id="people" class="form-control" style="height:35px;"></select>
                </div>
                <label for="module" class="col-sm-1 control-label">模块:</label>
                <div class="col-sm-2">
                    <select id="module" class="form-control" style="height:35px;">
                        <option>全部</option>
                        <option>观景指数等级预报</option>
                        <option>天气订正</option>
                    </select>
                </div>
                <label for="module" class="col-sm-1 control-label">状态:</label>
                <div class="col-sm-1">
                    <select id="sucess" class="form-control" style="height:35px;">
                        <option>全部</option>
                        <option>成功</option>
                        <option>失败</option>
                    </select>
                </div>
                <div class="col-sm-1">
                   <input type="button" class="btn-default btn" style="margin-left:60px" value="查询" onclick="query()" />
                </div>
            </div>
        </div>
        <div class="content" style="width:98%;margin:0 auto;">
            <table class="display dataTable no-footer test" id="table" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
            </table>
        </div>
    </form>
</body>
</html>
