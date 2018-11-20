<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OzoneForecast.aspx.cs" Inherits="ReportProduce_OzoneForecast" %>

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
    <script src="../AQI/js/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/OzoneForecast.js" type="text/javascript"></script>
    <link href="../css/OzoneForecast.css" rel="stylesheet" type="text/css" />
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div id="content">
    <div class="tableTop">
          <div id="topInfo" class="titleContent">
            <table>
                <tr><th class="thTab"></th><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td><td></td><th class="thButton"></th></tr>
            </table>
          </div>
          <div class="valueArea">
             <table class="displayTable">
                <tr>
                <td class="valueContent">
                  <table border="0" cellpadding="0" cellspacing="5" class="report">
                        <tr>
                            <th class="tdName" scope="row">1小时平均浓度：</th>
                        <td  class="tdValue" >
                            <input name="txtO3_1H" type="text" id="txtO3_1H" maxlength="5" style="width:107px; margin-bottom: 0px;">μg/m<sup>3</sup></td><input type="hidden" value="">
                        <th class="tdName" scope="row">8小时滑动平均浓度：</th>
                        <td class="tdValue">
                            <input name="txtO3_8H" type="text" id="txtO3_8H" maxlength="5" style="width:107px;">μg/m<sup>3</sup></td><input type="hidden" value="">
                        </tr>
                        <tr>
                            <th class="tdName" scope="row">1小时平均最大浓度范围：</th>
                        <td class="tdValue">
                            <input name="txtO3_1H_AVG" type="text" id="txtO3_1H_AVG" maxlength="10" style="width:107px;" data-options="required:true">ppb</td>
                        <th class="tdName" scope="row">8小时滑动平均最大浓度范围：</th>
                        <td class="tdValue">
                            <input name="txtO3_8H_AVG" type="text" id="txtO3_8H_AVG" maxlength="10" style="width:107px;" data-options="required:true">ppb</td>
                        </tr>
                        <tr>
                        <th class="tdName" scope="row">出现时段：</th>
                        <td class="tdValue">
                            <input name="txtO3_1H_Time" type="text" id="txtO3_1H_Time" maxlength="10" style="width:107px;" value="12-16"></td>
                        <th class="tdName" scope="row">出现时段：</th>
                        <td class="tdValue"><input name="txtO3_8H_Time" type="text" id="txtO3_8H_Time" maxlength="10" style="width:107px;" value="下午到傍晚"></td>
                        </tr>
                    </table>
                </td>
                <td class="btnContent"><input id="autoGet" class="button" type="button" value="获取实况"/><input id="history" class="button" type="button" value="历史数据"/></td>
                </tr>
             </table>
          </div>
    </div>
    <div class="contentDiv"><textarea id="ozoneContent"></textarea></div>
    <br/>
    <div class="submit">
        <input id="btnSave" class="submitbutton" type="button" value="保存" />
        <input id="btnPublish" class="submitbutton" type="button" value="发布" />
        
    </div>
</div>
<textarea name="txtHideTempleteContent" id="txtHideTempleteContent" cols="20" rows="2" style="display:none;">&lt;p&gt;
	11月24日上海市区臭氧1小时平均浓度最大为{O3_1h}ppb，出现在{O3_1h_Time_Rang}时，8小时滑动平均浓度最大为{O3_8h}ppb，出现在{O3_8h_Time_Rang}。&lt;/p&gt;
&lt;p&gt;
	&lt;br /&gt;
	&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; 2015年11月23日17时&lt;/p&gt;
</textarea>
</body>
</html>
