<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendLog.aspx.cs" Inherits="HealthyWeather_SendLog" %>

<!doctype html>
<html class="no-js" lang="zh-cn">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/PubUserSet.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        var Alias = "<%=m_alias %>";
    </script>
</head>
<body class="container-fluid">
    <form class="form-horizontal" role="form">
        <div class="form-group">
            <label for="inputEmail3" class="col-sm-2 control-label">开始时间：</label>
            <div class="col-sm-3"><input type="text" class="form-control" id="iptStart" onclick=" WdatePicker({dateFmt:'yyyy-MM-dd'});"/></div>
            <label for="inputEmail3" class="col-sm-2 control-label">结束时间：</label>
            <div class="col-sm-3"><input type="text" class="form-control" id="iptEnd" onclick=" WdatePicker({dateFmt:'yyyy-MM-dd'});"/></div>
            <label class="col-sm-2 checkbox-inline text-left">
              <input type="checkbox" id="chkSendAll" checked="checked"/> 属于一键发送
            </label>
        </div>
        <div class="form-group">
            <label for="sendType" class="col-sm-2 control-label">发送类型：</label>
            <div class="col-sm-3">
                <select class="form-control" onchange="Query()" id="sendType"><option>全部</option><option>短信</option><option>邮件</option><option>FTP</option></select>
            </div>
            
            <label for="selDiseaseType" class="col-sm-2 control-label">疾病类型：</label>
            <div class="col-sm-3">
              <select class="form-control" onchange="Query()" id="selDiseaseType"></select>
            </div>
            <div class="col-sm-2 text-left fr"><button type="button" class="btn btn-default" onclick="Query()">查询</button></div>
        </div>
        
        <div class="form-group">
            <label for="selSendType" class="col-sm-2 control-label">发送状态：</label>
            <div class="col-sm-3">
              <select class="form-control"  onchange="Query()" id="selSendType"><option>全部</option><option>成功</option><option>失败</option></select>
            </div>
           
        </div>
    </form>
    <table class="display dataTable no-footer" id="logTbl" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
    </table>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    
    <script src="js/sendLog.js?v=20171207" type="text/javascript"></script>
    
</body>
</html>
