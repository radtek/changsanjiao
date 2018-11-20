<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EnvForeQualityScoreReport.aspx.cs"
    Inherits="ReportProduce_EnvForeQualityScoreReport" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228"
    Namespace="PageOffice" TagPrefix="po" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>WeatherForecastQualityReport</title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/Chart/highstock.js" type="text/javascript"></script>
    <script src="../JS/Chart/modules/exporting.js" type="text/javascript"></script>
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../AQI/js/EnvForeQualityScoreReport.js" type="text/javascript"></script>
    <link href="../css/WeatherForecastQualityReport.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <%--<form id="form1" action="../EvaluateHtml/ExportChart.ashx" runat="server" method="post">--%>
    <form id="form1" runat="server">
    
    <div class="total">
        <div class="topFixed">
            <div class="tableTop">
                <div id="topInfo" class="titleContent">
                    <table>
                        <tr>
                            <th class="attrName">
                                预报员：
                            </th>
                            <td class="attrValue" id="forecaster">
                            </td>
                            <th class="attrName">
                                预报时间：
                            </th>
                            <td id="forecastTime" class="attrValue">
                            </td>
                            <th class="attrName">
                                预报时次：
                            </th>
                            <td id="forecastTimeLevel" class="attrValue">
                            </td>
                           <%-- <th class="attrName">
                                期数：
                            </th>
                            <td class="attrValue">
                                <input id="issueSet" type="text" value="" />
                            </td>--%>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="topBtns">
                <table>
                    <tr>
                        <td class="historyTd">
                            <div id="getHihstory" class="hidtoryBtn">
                                历史记录</div>
                        </td>
                        <td class="typeTd">
                            <div>
                                产品类型：<span id="productType">环境气象预报质量评定通报</span></div>
                        </td>
                        <td class="timeTd">
                            <div>
                                <span>预报时间：</span></div>
                        </td>
                        <td class="btnTd">
                            <div>
                            <input id="H00" type="text" class="selectDateFormStyle" runat="server" onclick="WdatePicker({dateFmt:'yyyy年MM月'})"/>
                            <input type="button" style=" float:left;  margin-left:20px;" id="ScanBack" class="button" value="查询" onclick="InitContent()" />
                                <%--<div id="clickQuery" class="wordBtn">
                                    一键查询</div>
                                <div id="readMOdel" class="wordBtn">
                                    读取模板</div>--%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="wordContent">
            <div class="docTitle">
                <div class="docTitleText">
                    <input type="text" id="PO_year" class="inputYear" value="2016" /><span>年</span><input
                        type="text" id="PO_month" class="inputMonth" value="2" />月环境气象预报质量评定通报
                </div>
            </div>
            <%--<div class="issue" id="PO_docIssue">
                <input type="text" id="PO_year" class="issue" value="2016" />
                <span>年</span><input type="text" id="PO_month" class="issue" value="2" />月</div>--%>
            <%--<div class="titleAndOra">
                <div id="docFullDate" class="docDate">
                    <input type="text" id="PO_docDate" class="docDate" value="2015年10月27日17时" /></div>
                <div class="ora">
                    <span>长三角环境气象预报预警中心</span></div>
                <div style="clear: both">
                </div>
            </div>--%>
            <div class="contentText">
                <div class="firstTitle">
                    一、评定内容<br />
                </div>
                <p class="contentTextBox">
                    评定城市环境气象预报质量。主要内容包括：<br />
                    —24小时霾天气预报评分；<br />
                    —空气质量24小时空气质量等级、首要污染物及AQI预报评分；<br />
                    —空气质量24小时内分时段空气质量等级、首要污染物及AQI预报评分；<br />
                    —紫外线预报评分；<br />
                    —两家预报对比(气象部门：气象局主观预报结果；环保部门：环保局主观预报结果；两家合作：气象局和环保局联合预报结果)。<br />
                </p>
            </div>
            <div class="contentText">
                <div class="firstTitle">
                    二、评定结果<br />
                </div>
                <p class="secondTitle">
                    1、24小时霾天气预报质量</p>
                <textarea id="PO_SHWeatherReport" class="contentTextBox_Rich" rows="3">   2月份，上海出现6个霾日，其中上旬出现4天，中旬出现1天，下旬出现1天（霾日的统计时效为20-20时）。预报质量见表1。</textarea>
                <p class="tableTitle">
                    表1:24小时霾天气预报质量</p>
                <table class="dataTable1">
                    <tbody>
                        <tr>
                            <th colspan="2">
                            </th>
                            <th>
                                无霾
                            </th>
                            <th>
                                有霾
                            </th>
                        </tr>
                        <tr>
                            <td rowspan="6" width="20%">
                                05时（08-20）
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td width="20%">
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze05Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze05Fact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                准确预报
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze05Correct" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze05Correct" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                空报
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze05Null" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze05Null" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                漏报
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze05Miss" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze05Miss" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                预报评分
                            </td>
                            <td colspan="2">
                                <input type="text" id="PO_Haze05Score" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6" width="20%">
                                17时（20-20）
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze17Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze17Fact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                准确预报
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze17Correct" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze17Correct" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                空报
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze17Null" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze17Null" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                漏报
                            </td>
                            <td>
                                <input type="text" id="PO_noHaze17Miss" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_Haze17Miss" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                预报评分
                            </td>
                            <td colspan="2">
                                <input type="text" id="PO_Haze17Score" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="contentText">
                <p class="secondTitle">
                    2、24小时空气质量预报质量(20-20时)</p>
                <p class="tableTitle">
                    表2：空气质量等级预报情况</p>
                <table class="dataTable">
                    <tbody>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                优
                            </td>
                            <td>
                                良
                            </td>
                            <td>
                                轻度
                            </td>
                            <td>
                                中度
                            </td>
                            <td>
                                重度
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_GoodFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_FineFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_LightFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_MediumFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_HeavyFact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_GoodWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_FineWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_LightWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_MediumWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_HeavyWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                主观预报
                            </td>
                            <td>
                                <input type="text" id="PO_GoodSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_FineSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_LightSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_MediumSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_HeavySub" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <p class="tableTitle">
                    表3：首要污染物预报情况</p>
                <table class="dataTable">
                    <tbody>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                PM<sub>2.5</sub>
                            </td>
                            <td>
                                PM<sub>10</sub>
                            </td>
                            <td>
                                NO<sub>2</sub>
                            </td>
                            <td>
                                O<sub>3</sub>
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_PM25Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_PM10Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_NO2Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_O3Fact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_O3WRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                主观预报
                            </td>
                            <td>
                                <input type="text" id="PO_PM25Sub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_PM10Sub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_NO2Sub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_O3Sub" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <p class="tableTitle">
                    表4：AQI主客观预报准确率及综合评分</p>
                <table class="dataTable4">
                    <tbody>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                首要污染物正物确性评分
                            </td>
                            <td>
                                AQI预报级别正确性评分
                            </td>
                            <td>
                                AQI预报数值误差评分
                            </td>
                            <td>
                                综合评分
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_MainWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_CorrectWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_ErrorWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_MultiWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                主观预报
                            </td>
                            <td>
                                <input type="text" id="PO_MainSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_CorrectSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_ErrorSub" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_MultiSub" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="contentText">
                <p class="secondTitle">
                    3、分时段空气质量预报质量</p>
                <p class="tableTitle">
                    表5：空气质量等级预报情况</p>
                <table class="dataTable">
                    <tbody>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                优
                            </td>
                            <td>
                                良
                            </td>
                            <td>
                                轻度
                            </td>
                            <td>
                                中度
                            </td>
                            <td>
                                重度
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                夜间
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_night_GoodFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_FineFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_LightFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_MediumFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_HeavyFact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_night_GoodMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_FineMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_LightMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_MediumMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_HeavyMA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_night_GoodEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_FineEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_LightEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_MediumEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_HeavyEP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_night_GoodCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_FineCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_LightCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_MediumCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_HeavyCo" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_night_GoodWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_FineWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_LightWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_MediumWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_HeavyWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                上午
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_am_GoodFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_FineFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_LightFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_MediumFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_HeavyFact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_am_GoodMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_FineMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_LightMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_MediumMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_HeavyMA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_am_GoodEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_FineEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_LightEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_MediumEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_HeavyEP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_am_GoodCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_FineCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_LightCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_MediumCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_HeavyCo" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_am_GoodWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_FineWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_LightWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_MediumWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_HeavyWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                下午
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_pm_GoodFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_FineFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_LightFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_MediumFact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_HeavyFact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_pm_GoodMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_FineMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_LightMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_MediumMA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_HeavyMA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_pm_GoodEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_FineEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_LightEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_MediumEP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_HeavyEP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_pm_GoodCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_FineCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_LightCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_MediumCo" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_HeavyCo" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_pm_GoodWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_FineWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_LightWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_MediumWRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_HeavyWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <p class="tableTitle">
                    表6：首要污染物预报情况</p>
                <table class="dataTable">
                    <tbody>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                PM<sub>2.5</sub>
                            </td>
                            <td>
                                PM<sub>10</sub>
                            </td>
                            <td>
                                NO<sub>2</sub>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                夜间
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM25Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM10Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_NO2Fact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM25MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM10MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_NO2MA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM25EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM10EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_NO2EP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM25Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM10Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_NO2Co" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_night_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                上午
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM25Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM10Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_NO2Fact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM25MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM10MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_NO2MA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM25EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM10EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_NO2EP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM25Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM10Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_NO2Co" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_am_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                下午
                            </td>
                        </tr>
                        <tr class="blueRow">
                            <td>
                                实况
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM25Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM10Fact" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_NO2Fact" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM25MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM10MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_NO2MA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM25EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM10EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_NO2EP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM25Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM10Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_NO2Co" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_pm_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <p class="tableTitle">
                    表7：IAQI主客观预报准确率及综合评分</p>
                <table class="dataTable">
                    <tbody>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                PM<sub>2.5</sub>
                            </td>
                            <td>
                                PM<sub>10</sub>
                            </td>
                            <td>
                                NO<sub>2</sub>
                            </td>
                            <td>
                                综合评分
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="5">
                                夜间
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM25MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM10MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_NO2MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_MultiMA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM25EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM10EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_NO2EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_MultiEP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM25Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM10Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_NO2Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_MultiCo" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_night_MultiWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="5">
                                上午
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM25MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM10MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_NO2MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_MultiMA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM25EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM10EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_NO2EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_MultiEP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM25Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM10Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_NO2Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_MultiCo" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_am_MultiWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="5">
                                下午
                            </td>
                        </tr>
                        <tr class="purpleRow">
                            <td>
                                气象部门
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM25MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM10MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_NO2MA" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_MultiMA" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                环保部门
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM25EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM10EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_NO2EP" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_MultiEP" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="pinkRow">
                            <td>
                                两家合作
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM25Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM10Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_NO2Co" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_MultiCo" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                        <tr class="grayRow">
                            <td>
                                WRF-chem
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM25WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_PM10WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_NO2WRF" class="contentTextBox_Data" value="0" />
                            </td>
                            <td>
                                <input type="text" id="PO_IAQI_pm_MultiWRF" class="contentTextBox_Data" value="0" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="dataImg">
                    <img id="PO_imgPM2.5" src="#" style="display: none;" />
                    <div id="container0">
                    </div>
                </div>
                <div class="dataImg">
                    <img id="PO_imgPM10" src="#" style="display: none;" />
                    <div id="container1">
                    </div>
                </div>
                <div class="dataImg">
                    <img id="PO_imgNO2" src="#" style="display: none;" />
                    <div id="container2">
                    </div>
                </div>
                <p class="tableTitle">
                    图1：污染物分要素主客观预报与实况对比
                </p>
                <div class="contentText">
                    <p class="secondTitle">
                        4、紫外线预报评分
                    </p>
                    <p>
                        <textarea id="PO_uvForecast" class="contentTextBox_Rich" rows="auto">   2月紫外线预报评分98.6。其中2级出现2天，3级出现4天，4级出现8天，5级出现15天，未出现1级。预报质量见表8。</textarea></p>
                    <p class="tableTitle">
                        表8：紫外线预报情况
                    </p>
                    <table class="dataTable">
                        <tbody>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    2级
                                </td>
                                <td>
                                    3级
                                </td>
                                <td>
                                    4级
                                </td>
                                <td>
                                    5级
                                </td>
                            </tr>
                            <tr class="purpleRow">
                                <td>
                                    实况
                                </td>
                                <td>
                                    <input type="text" id="PO_uv2Fact" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv3Fact" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv4Fact" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv5Fact" class="contentTextBox_Data" value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    预报（16时）
                                </td>
                                <td>
                                    <input type="text" id="PO_uv2Forecast16" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv3Forecast16" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv4Forecast16" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv5Forecast16" class="contentTextBox_Data" value="0" />
                                </td>
                            </tr>
                            <tr class="purpleRow">
                                <td>
                                    预报（10时）
                                </td>
                                <td>
                                    <input type="text" id="PO_uv2Forecast10" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv3Forecast10" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv4Forecast10" class="contentTextBox_Data" value="0" />
                                </td>
                                <td>
                                    <input type="text" id="PO_uv5Forecast10" class="contentTextBox_Data" value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    评分
                                </td>
                                <td colspan="4">
                                    <input type="text" id="PO_uvForecastScore" class="contentTextBox_Data" value="0" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="description">
                    <div class="secondTitle">
                        <br />
                        <p>
                            评分说明：
                        </p>
                    </div>
                    <div class="stableText">
                        <p class="contentDescription">
                            一、霾的预报按照上海市11工观测站记录为准，有2个及2个以上站点记录有霾，则按照有霾记录，否则按照无霾记录；如果预报、实况均为有霾，则认为预报正确，其中05时预报与实况数据统计采用08-20时（白天），17时预报与实况数据统计采用20-20时（全天）；其中不考虑霾程度的偏差；预报评分S为：S=100*预报正确天数/预报天数。
                        </p>
                        <p class="contentDescription">
                            二、24小时空气质量评分参照《上海市城市空气质量预报检验评估和考核办法(试行)》<br />
                            1、目的<br />
                            &nbsp;&nbsp;为进一步规范城市空气质量预报业务服务工作，充分调动业务技术人员的积极性，提高重点城市空气质量预报业务水平,更好地满足气象服务的需要，制定本办法。<br />
                            2、考核对象<br />
                            &nbsp;&nbsp;本办法负责考核和管理上海市每日20时起报的24小时空气质量预报，包含AQI等级、AQI指数和首要污染物等。<br />
                            3、考核内容、标准及方法<br />
                            &nbsp;&nbsp;上海市空气质量预报检验考核内容分空气质量指数（AQI）等级考核和首要污染物考核两种。检验时段为每日20时起报的24小时AQI等级和首要污染物预报。AQI的定义、计算方法和级别划分依据《环境空气质量指数（AQI）技术规定（试行）》（HJ
                            633-2012）。 AQI逐小时实况以环保部全国城市空气质量实时发布显示的上海市10个国控点的AQI为实况。首要污染物判定为上海市10个国控站观测首要污染物中出现站点最多的污染物，若有两个或多个污染物为相等数量国控站观测的首要污染物，那么这些污染物并列为该城市的首要污染物。20-20时检验时段的AQI等级实况参照国标的日值的计算方法，先计算该时段污染物浓度及空气质量分指数（IAQI)，最后认定AQI数值。<br />
                            （1）空气质量预报精确度评分（S）<br />
                            空气质量预报精确度评分按以下统计模型进行评定：<br />
                        </p>
                        <p class="contentFormula">
                            S=0.2f1+0.5f2+0.3f3<br />
                        </p>
                        <p class="contentDescription">
                            其中:S为预报精确度评分（取1位小数）,f1为预报首要污染物正确性评分,f2为AQI预报级别正确性评分,f3为AQI预报数值误差评分。<br />
                        </p>
                        <ul>
                            <li>首要污染物正确性评分(f1)<br />
                                若预报的首要污染物与实况一致，则判定为首要污染物预报正确,否则为错误。首要污染物预报正确性评分按100分计算,首要污染物预报正确得100分，错误得0分。 若有两种或多种污染物并列为首要污染物，预报出其中一种即判定为首要污染物预报正确。</li>
                            <li>AQI预报级别正确性评分（f2）
                                <br />
                                每日AQI级别正确性按下表评分:<br />
                                <div class="contentImg">
                                    <img width="90%" class="dataImg" src="../css/images/ReportProduce/f2.png" /></div>
                            </li>
                            <li>AQI预报数值误差评分（f3）<br />
                                AQI预报数值误差评分（f3）按下表计算:
                                <div class="contentImg">
                                    <img class="dataImg" alt="f3_table" width="90%" src="../css/images/ReportProduce/f3_table.png" /></div>
                                <!-- <table class="dataTable">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    预报值与实况误差
                                                </td>
                                                <td>
                                                    0-25
                                                </td>
                                                <td>
                                                    26-50
                                                </td>
                                                <td>
                                                    51-100
                                                </td>
                                                <td>
                                                    101-150
                                                </td>
                                                <td>
                                                    151-500
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    评分
                                                </td>
                                                <td>
                                                    100
                                                </td>
                                                <td>
                                                    80
                                                </td>
                                                <td>
                                                    60
                                                </td>
                                                <td>
                                                    30
                                                </td>
                                                <td>
                                                    0
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>-->
                            </li>
                        </ul>
                        <p class="contentDescription">
                            （2）特殊考核<br />
                            特殊考核衡量各城市空气质量预报对于高浓度污染（AQI五、六级）的预报能力，参照降水TS评分计算方法，暂不记入总考核评分。<br />
                            例如5级AQI的预报TS评分公式：<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;TS= AC/（AF+AO-AC）<br />
                            AC为评分时段内预报正确（即AQI预报与实况均为5级）的天数，AF为评分时段内AQI预报为5级的总天数，AO为实际出现5级AQI的天数。若AF+AO-AC=0,则T=0。<br />
                            （3）空气质量预报月、季、年评分<br />
                            空气质量月、季、年评分由逐日预报质量评分求平均计算得出。<br />
                            （4）评分结果表现形式<br />
                            以数据文档或图表形式展示AQI等级预报评分结果，每月10日前通报上月评分结果。<br />
                            4、质量管理<br />
                            （1）空气质量预报的质量按空气质量预报质量每月评分和年评分进行考核，合格分为60分。<br />
                            （2）空气质量预报作为一项基本气象业务，其质量目标值应纳入城环中心业务进行工作目标管理。<br />
                            （3）本办法由上海市观测与预报处负责解释。<br />
                            （4）本办法从下发之日起执行。<br />
                        </p>
                        <p class="contentDescription">
                            三、空气质量预报AQI评分参照《上海市环境空气质量AQI预报考核评价方法》， 具体如下：<br />
                            （一）考核原则<br />
                        </p>
                        <ol>
                            <li>1. 根据《上海市环境空气质量预报技术规范》，本考核办法主要从技术层面对预报员的预报能力进行考核，同时结合预报的公众服务功能，对预报的发布内容进行考核。</li>
                            <li>2. 预报时段为上半夜（20:00-次日00:00）、下半夜（次日00:00-06:00）、次日上午（次日06:00-12:00）、次日下午（次日13:00-20:00）、次日上半夜（次日20:00-第三日0:00），发布和考评时段为夜间（20:00-次日06:00）、次日上午（次日06:00-12:00）、次日下午（次日13:00-20:00），次日的日AQI预报根据分段AQI的预报结果进行计算和考核。24-48小时预报仅作为内部参考，暂不纳入考核范围。</li>
                            <li>3. 重点关注分段预报结果的准确性，结合实时空气质量指数及公众感官的影响。</li>
                            <li>4. 本考核办法主要针对本市已对公众发布的24小时分段AQI预报，并对次日的日AQI预报结果进行统计。今后，随着本市空气质量预报工作的发展和预报技术的提高，将逐步拓展到针对48小时预报和模式客观预报的考核和评价。</li></ol>
                        <p class="contentDescription">
                            （二）考核方法<br />
                            1.关于臭氧的特殊规定<br />
                            （1）由于臭氧浓度在冬半年较低，故本规定仅对每年夏半年（3月16日至11月15日）的下午时段进行臭氧的预报和考核工作。冬半年（11月16日至次年3月15日）臭氧不作为考核指标。<br />
                            （2） 由于臭氧日最大浓度主要出现在下午时段，臭氧iAQI预报只参与下午时段的考核。但若上午臭氧1小时的iAQI超过100且成为首要污染物，则需要增加对臭氧1小时iAQI的考核。夜间不对臭氧进行考核。次日下午的臭氧8小时预报结果仅作为计算日AQI预报结果的依据，不参与该时段预报及考核的计算。<br />
                            （3） 预报员在夏半年的上午时段可以根据空气质量变化状况预判，选择是否填报臭氧：<br />
                        </p>
                        <ul>
                            <li>若填报，则该项指数和其他污染物并列计入各项考核内容；</li>
                            <li>若未填报且该时段臭氧实况未达到考核条件（第2条），则臭氧不计入考核；</li>
                            <li>若未填报且该时段臭氧实况达到考核条件（第2条），则该项指数以预报值为“0”记录和考核。</li></ul>
                        <p class="contentDescription">
                            （4） 分段预报仅以臭氧1小时浓度参照对应标准计算AQI和首要污染物并进行评价，日报则以臭氧8小时浓度参照对应标准计算AQI和首要污染物并进行评价。
                        </p>
                        <p class="contentDescription">
                            2.主观预报考核规定<br />
                            （1） 预报时段为上半夜（20:00-次日00:00）、下半夜（次日00:00-06:00）、次日上午（次日06:00-12:00）、次日下午（次日13:00-20:00）、次日上半夜（次日20:00-第三日0:00），发布和考评时段为夜间（20:00-次日06:00）、次日上午（次日06:00-12:00）、次日下午（次日13:00-20:00），次日的日AQI预报根据分段AQI的预报结果进行计算和考核。<br />
                            （2） 预报考核时段和发布时段一致，为夜间、次日上午、次日下午3个时间段，此外还包括次日的日AQI预报结果，每个时间段的预报满分均100分，总分的计算方式如下：<br />
                        </p>
                        <div class="contentFormula">
                            f<sub>总分</sub>= f<sub>夜间</sub>× 0.3 + f<sub>上午</sub> × 0.3 + f<sub>下午</sub>× 0.3
                            + f<sub>次日</sub>× 0.1<br />
                        </div>
                        <p class="contentDescription">
                            （3） 在计算首要污染物的时候，二氧化氮分段预报指数的计算参照小时浓度标准，而日AQI则参照日均浓度标准。<br />
                            （4） 各时段得分统计方法：
                        </p>
                        <ul>
                            <li>当实况各指标均为优等级时：<br />
                                <div class="contentFormula">
                                    F = 0.3 × f1 + 0.7× f4 + f0<br />
                                </div>
                                其中：f1为级别正确性评分，f4为所有参与考核指标的精度评分的平均，f0为污染附加分。 </li>
                            <li>当实况各指标有非有等级时：<br />
                                <div class="contentFormula">
                                    F = 0.1 × f1 +0.2 × f2+ 0.3 × f3 + 0.4 × f4 + f0<br />
                                </div>
                                其中：f1为首要污染物正确性评分，f2为级别准确性评分，f3为首要污染物（以实况为准）iAQI精度评分，f4为其他指标的iAQI精度评分的平均，f0为污染附加分。</li></ul>
                        <p class="contentDescription">
                            （5） 各项指标评分方法：<br />
                            ① f1（首要污染物正确分）：
                        </p>
                        <ul>
                            <li>仅在实况为非优级的情况下对首要污染物的准确性进行评价；</li>
                            <li>如果预报首要污染物和实况完全相同，得100分；</li>
                            <li>如果预报首要污染物和实况完全不同，得0分；</li>
                            <li>如果预报出现2个或以上首要污染物，实况首要污染物为其中一项，得分为f1=100 × 1 / N预报（N预报为预报污染物个数）；
                                <li>如果实况出现2个或以上首要污染物，预报为其中一项，得100分； </li>
                        </ul>
                        <p class="contentDescription">
                            ② f2（级别准确分）：</p>
                        <ul>
                            <li>级别准确性的考核适用于所有时段，根据分时段的预报发布内容和该时段实时空气质量指数变化的符合程度进行评价，发布内容分为基本级别描述和变化趋势描述。</li>
                            <li>对基本级别描述的评价分为完全准确、部分准确（跨一级）和不准确（跨两级以上），实况的基本级别判定为该时间段内出现频次最多的相邻两个级别（或一个级别）。</li>
                            <ul class="secondUl">
                                <li>如果实况的基本级别完全被预报的基本级别覆盖，则判定为完全准确，得100分；</li>
                                <li>如果实况的基本级别部分被预报的基本级别覆盖，其他时间和预报基本级别之间有一级的差别，则判定为部分准确，得50分；</li>
                                <li>如果实况的基本级别完全没有被预报的基本级别覆盖，和预报基本级别之间有两级（含）以上的差别，则判定为不准确，得0分；</li>
                            </ul>
                            <li>级别准确性得分最高为100分，最低为0分，得分高于100分/低于0分则以100分/0分计算。</li></ul>
                        <p class="contentDescription">
                            ③ f3（首要污染物iAQI精度）：<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;首要污染物iAQI精度根据预报值与实况值之间的差别计算得到。如果差别过大出现得分为负数的情况，则以0分代替；如果实况为优级天，则分母以优级天的最大AQI（50）代替。具体计算公式如下：<br />
                        </p>
                        <div class="contentFormula">
                            <img src="../css/images/ReportProduce/f3.png" /></div>
                        <p class="contentDescription">
                            ④ f4（其他污染物iAQI精度）：<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;其他污染物iAQI精度与首要污染物的计算方法基本一致，具体计算公式如下：<br />
                        </p>
                        <div class="contentFormula">
                            <img src="../css/images/ReportProduce/f4.png" /></div>
                        <br />
                        <p class="contentDescription">
                            其中i为纳入评价范围的各项污染物。而f4的得分为所有f4i的平均值<br />
                            ⑤ f0（污染预报加分项）：<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;当实况或预报出现轻度及以上污染时，进行AQI附加分（f0）评定，并加入各段的总评分。实况和预报等级对应得分（f0）见下表。<br />
                        </p>
                        <div class="contentImg">
                            <img width="90%" alt="f0" class="dataImg" src="../css/images/ReportProduce/f0.png" /></div>
                        <p class="contentDescription">
                            三、紫外线预报评分公式如下：<br />
                        </p>
                        <p class="contentFormula">
                            R=S<sub>前一日</sub>+S<sub>当日</sub>+0.4*J<sub>前一日</sub>+0.3*J<sub>当日</sub></p>
                        <p class="contentDescription">
                            &nbsp;&nbsp;&nbsp;&nbsp;其中Si为传输时效评分，有效得15分，传输不及时则以0分计；Ji为预报准确率评分，预报与实况相符得100分，相差一级得50分，相差二级及以上不得分。<br />
                        </p>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="btnArea">
        <div class="btns">
            <input id="svgContent" name="svgContent" type="hidden" />
            <input id="wordTempContent" name="wordTempContent" type="hidden" />
            <input id="productName" name="productName" type="hidden" />
            <%--<input id="svgSubmit" class="button_BottomInput" name="expSub" type="submit" value="保存" />--%>
            <input id="svgSubmit" class="button_BottomInput" name="expSub" type="button" value="保存" />
            <div id="forePreview" class="button_Bottom" >
                <a id="previewLink" href="<%=PageOfficeLink.OpenWindow("http://222.66.83.21:8282/PEMFCShare/AQI/PageOfficePreview/PolWeatherAnalysisPreview.aspx?filePath=EnvForeScore.doc&ProductName=EnvForeScore","width=1200px;height=700px;")%>">
                    预览</a></div>
            <div id="forePub" class="button_Bottom">
                发布</div>
        </div>
    </div>
    <div style="display: none;">
    </div>
    </form>
</body>
</html>
