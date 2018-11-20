<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirPollutionDropZone.aspx.cs" Inherits="ReportProduce_AirPollutionDropZone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <link href="../css/AirPollutionDropZone.css" rel="stylesheet" type="text/css" />
    <script src="../AQI/js/AirPollutionDropZone.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="all">
    <div class="top">
    <table class="table1" width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody>
    <tr>
    <th class="thset">预报员:</th>
    <td class="tdset" id="forecaster"></td>
    <th class="thset">预报时间:</th>
    <td class="tdset" id="forecastTime"></td>
    <th class="thset"></th>
    <td class="tdset"></td>
    </tr></tbody></table></div><br />
    
    <div class="two">
    <table class="table2" width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody>
    <tr>
    <th class="thset">华东区域落区预报图</th>
    <th class="thset" colspan="3"></th>
    <th class="thset"><input class="button" type="button" value="自动获取" /></input></th>
    <th class="thset"><div id="btnupdown" class="buttonup"></div></th></tr>
    <tr class="trimg">
    <td id="tdimg" colspan="6"><img src="../css/images/noImg.GIF" id="airPolImg"/></td>
    </tr>
    </tbody></table></div>

    <div class="submit">
    <input class="button" type="button" value="保存" /></input>
    <input class="button" type="button" style="display:none" value="审核" /></input>
    <input class="button" type="button" style="display:none" value="发布" /></input>
    </div>

    </div>
    </form>
</body>
</html>
