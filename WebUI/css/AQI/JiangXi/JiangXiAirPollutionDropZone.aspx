<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JiangXiAirPollutionDropZone.aspx.cs" Inherits="AQI_JiangXi_JiangXiAirPollutionDropZone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../../JS/Utility.js" type="text/javascript"></script>
    <script src="../../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../../JS/Utility.js" type="text/javascript"></script>
    <script src="JS/JiangXiAirPollutionDropZone.js" type="text/javascript"></script>
    <link href="css/JiangXiAirPollutionDropZone.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
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


    <tr class="trimg">
    <th colspan="6">
    <div id="change" class="tabs-container">
    <div class="tabs-wrap" style="margin-left:0px;left:0px;width:100%;">
    <ul class="tabs">
    <li id="PM25Li">
      <a class="tabs-inner">
        <span class="tabs-title">图1</span>
        <span class="tabs-icon"></span></a></li>
    <li id="PM10Li">
    <a class="tabs-inner">
    <span class="tabs-title">图2</span>
    <span class="tabs-icon"></span></a></li>
   </ul></div>
    <div class="tabs-panels" style="height:auto;width:auto;">
    <div class="panel" style="display:block;width:auto;">
    <div class="showimages" >
    <img id="02" src="../../css/images/noImg.GIF" /></div></div>
    <div class="panel" style="display:none;width:auto;">
    <div class="showimages">
    <img id="03" src="../../css/images/noImg.GIF" /></div></div>

    </div></div></th></tr>

    </tbody></table>
    </div>
    
    <div class="submit">
    <input class="button" type="button" value="保存" /></input>
    <input class="button" type="button" style="display:none" value="审核" /></input>
    <input class="button" type="button" style="display:none" value="发布" /></input>
    </div></div>
    </form>
</body>
</html>
