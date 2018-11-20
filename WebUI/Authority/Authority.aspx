<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Authority.aspx.cs" Inherits="Authority_Authority" %>

<!DOCTYPE html>

<html class="no-js" lang="zh-cn">
<head runat="server">
    <title>用户权限管理</title>
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../HealthyWeather/css/bootstrap.css"/>
    <link rel="stylesheet" type="text/css" href="css/easyui.css"/>
    <link rel="stylesheet" type="text/css" href="css/icon.css"/>
    <link rel="stylesheet" type="text/css" href="css/demo.css"/>
    <link rel="Stylesheet" type="text/css" href="css/Authority.css "/>
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.easyui.min.js" type="text/javascript" charset="gb2312"></script>
    <script src="js/Authority.js" type ="text/javascript"></script>
</head>
<body class="container-fluid">
    <div class="row">
        <div class="col-sm-3 col-md-3">
          <table class="display dataTable no-footer" id="User" role="grid" aria-describedby="example_info"
           border="0" cellspacing="0" cellpadding="0" style="text-align: center; height:90%"></table>
        </div>
        <div class="col-sm-9 col-md-9" style="padding-left:20px; ">
          <div class="row" id="edit" style="padding-left:20px ;margin-bottom:20px;margin-top:0px;">
            <%--<button type="button" class="btn btn-default" id="cancel" onclick="cancel()">取消</button>--%>
            <button type="button" class="btn btn-info" id="save" onclick="editAuthority()">保存</button>
          </div>
          <table id="tree" style="height:520px"></table>
        </div>
    </div>  
</body>
</html>
