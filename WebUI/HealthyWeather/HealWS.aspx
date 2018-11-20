<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HealWS.aspx.cs" Inherits="HealthyWeather_HealWS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/HealWS.css" rel="stylesheet" />

    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../JS/Utility.js"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
   <%-- <script src="js/bootstrap.min.js"></script>--%>
    <script src="js/HealWS.js"></script>
</head>
<body>
    <div class="box">
        <div class="a"><label>服务地址：</label><a href="http://222.66.83.21:8282/HeathWS/WebService.asmx">http://222.66.83.21:8282/HeathWS/WebService.asmx</a></div>
        <div class="content">
            <label class="lf">密钥：</label>
            <input type="text" class="w lf form-control" id="key" />
            <button class="btn-default btn lm" onclick="confirm()">提交</button>
        </div>
        <span id="xmll" class="xml"></span>
    </div>
</body>
</html>
