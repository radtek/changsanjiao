<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQISiteGuideReport.aspx.cs" Inherits="ReportProduce_AQISiteGuideReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIGuideReport.js" type="text/javascript"></script>
    <link href="../css/AQISiteGuideReport.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div style="position: absolute; left: 0px; top: 0px; right: 0px;">
        <div class="panelbox">
            <div class="contenttitle">
                <div class="ct_left">
                </div>
                <div class="ct_mid">
                    <div class="ct_icon">
                        <img src="../images/aqi/aqi.png" width="16" height="16" alt="">
                    </div>
                    华东AQI站点指导预报
                </div>
                <div class="ct_right">
                </div>
            </div>
            <div class="content">
                <div class="contentB" style="width: 100%; height: 50px;">
                    <div class="content">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="default_table2">
                            <tbody><tr>
                                <th width="10%" align="right">
                                    预报员：
                                </th>
                                <td width="7%" align="left">
                                    <span id="forecaster"></span>
                                </td>
                                <th width="10%" align="right">
                                    预报时间：
                                </th>
                                <td width="7%" align="left">
                                    <input name="txtForecastTime" type="text" id="forecastTime" class="input_no_border" readonly="readonly" onclick="WdatePicker({ skin: 'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'});" onfocus="WdatePicker({ skin: 'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'});" value="2015-12-07 17:38:30">
                                </td>
                                <th width="10%" align="right">
                                    预报时次：
                                </th>
                                <td width="7%" align="left">
                                    <span id="forecastTimeLevel">10:30时</span>
                                </td>
                            </tr>
                        </tbody></table>
                    </div>
                </div>
            </div>
        </div>
        <div class="panelbox">
            <br>
            <div class="box">
                <div class="label">
                    <span>空气质量预报数据文件</span></div>
                <input type="button" value="自动生成" class="button" style="position: absolute; right: 100px;" onclick="AutoCreate512StsAllData();">
            </div>
            <div class="tabs_container">
                <div id="DownLinkDiv" style="text-align: center;">
                    <br>
                   <input type="button" id="btnDownFile" value="下载站点预报文件" class="button">
                    <br><br>
                 </div>
            </div>
        </div>
        <br>
        <div class="submit">
            <input id="btnSaveAQI" type="button" value="保存" class="submitbutton">
            <input id="btnAuthProduct" class="submitbutton" type="button" value="审核" style="display: none;">
            <input id="btnPush" class="submitbutton" type="button" value="发布" style="">
        </div>
    </div>
</body>
</html>
