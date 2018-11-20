<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HazeForecast.aspx.cs" Inherits="ReportProduce_HazeForecast" %>

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
    <script src="../AQI/js/HazeForecast.js?v=2016060811" type="text/javascript"></script>
    <link href="../css/HazeForecast.css" rel="stylesheet" type="text/css" />
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div id="content">
    <div class="tableTop">
          <div id="topInfo" class="titleContent">
            <table>
                <tr><th class="thTab"></th><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td><td></td><th class="thButton"></th></tr>
                <tr>
                   <td colspan="8" rowspan="2" class="tdtable">
                        <table border="0" cellpadding="0" cellspacing="5" class="report">
                            <tbody><tr>
                            <th class="thTab"></th>
                            <th scope="row">时段：</th>
                            <td class="tdtext">
                            <select name="ddlTime" id="ddlTime">
	                            <option id="opTomorrow"><%=strToday %></option>
	                            <option id="opAfter1"><%=strTomorrow %></option>
	                            <option id="opAfter2"><%=strAfter%></option>
                            </select>
                            </td>
                            <th scope="row" class="style1">霾：</th>
                            <td class="tdtext"><select name="ddlHazeLevel" id="ddlHazeLevel">
	                            <option value="1">无霾</option>
	                            <option value="2">轻微霾</option>
	                            <option value="3">轻度霾</option>
	                            <option value="4">中度霾</option>
	                            <option value="5">重度霾</option>
	                            <option value="6">严重霾</option>
                    </select></td>
                                <th scope="row" class="style2"><span class="VisibilityMin" style="display: inline;">24小时最小能见度：</span></th>
                            <td class="tdtext"><span class="VisibilityMin" style="display: inline;"><input name="txtVisibilityMin" type="text" id="txtVisibilityMin" maxlength="10" style="width:107px;" class="fontColor input">km</span></td>
                            <th class="thButton"> <input type="button" id="btnLast" class="button" value="历史记录" /></th>
                            </tr>
     
                        </tbody></table></td>
                </tr>
            </table>
          </div>
    </div>
    <div class="contentDiv"><textarea id="hazeContent"></textarea></div>
    <br/>
    <div class="submit">
        <input id="btnSave" class="submitbutton" type="button" value="保存" />
        <input id="btnPublish" class="submitbutton" type="button" value="发布" />
        
    </div>
    <textarea name="txtHideTempleteContent05" id="txtHideTempleteContent05" cols="20" rows="2" style="display:none;">&lt;p&gt;
	    &amp;nbsp;&lt;/p&gt;
    &lt;p&gt;
	    &amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; 上海市霾的预报&lt;br /&gt;
	    &amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; (2015年11月22日17时发布)&lt;/p&gt;
    &lt;p&gt;
	    <%=strToday %>：{TodayContent}。&lt;br /&gt;
	    <%=strTomorrow%>：{TomorrowContent}。&lt;br /&gt;
	    <%=strAfter%>：{AfterDayContent}。&lt;/p&gt;
    &lt;p&gt;
	    &lt;br /&gt;
	    &amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; 上海市城市环境气象中心&lt;/p&gt;
    </textarea>
    <textarea name="txtHideTempleteContent17" id="txtHideTempleteContent17" cols="20" rows="2" style="display:none;">&lt;p&gt;
	    &amp;nbsp;&lt;/p&gt;
    &lt;p&gt;
	    &amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; 上海市霾的预报&lt;br /&gt;
	    &amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; (2015年11月22日17时发布)&lt;/p&gt;
    &lt;p&gt;
	    2015年11月23日：{TomorrowContent}。&lt;br /&gt;
	    2015年11月24日：{AfterDayContent}。&lt;br /&gt;
	    2015年11月25日：{AfterDay2Content}。&lt;/p&gt;
    &lt;p&gt;
	    &lt;br /&gt;
	    &amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp; 上海市城市环境气象中心&lt;/p&gt;
    </textarea>
</div>
</body>
</html>
