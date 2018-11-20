<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQIPeriod06.aspx.cs" Inherits="ReportProduce_AQIPeriod48" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/EnvReportPro48.css?v=0604" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/OutAQI_06.js?v=2017082111" type="text/javascript"></script>
<%--    <script src="../AQI/js/TextAreaChar.js" type="text/javascript"></script>--%>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        if ((typeof Range !== "undefined") && !Range.prototype.createContextualFragment) {
            Range.prototype.createContextualFragment = function (html) {
                var frag = document.createDocumentFragment(),
                        div = document.createElement("div");
                frag.appendChild(div);
                div.outerHTML = html;
                return frag;
            };
        }
    </script>
</head>
<body>

<div class="content">
   <div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table><tr><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td></tr></table>
      </div>
   </div>
   <div class="outLine" id="outLine">
   
   <div class="mapTitle">
           <div class="titlePoint"></div>
           <div class="titleText"><span>AQI分时段预报表格</span></div>
           <div class="importBtns">    
           <div id="refreshData" class="importBtn">数据重载</div>           
               <div id="modelImport" class="importBtn">模式预报</div> 
               <div id="subjImport" class="importBtn">主观预报</div> 
               <div id="historyData" class="importBtn">历史数据</div> 
           </div>   
           </div>
   <div id="forecastTable" class="forecastTable">    
   <table id="contentTable" cellpadding="5"       cellspacing="0" runat="server" >
   <tr>
		<td class="tabletitleOutPeiod" rowspan="2">预报时效</td>
		<td class="tabletitleDateOut" rowspan="2">日期</td>
		<td class="tabletitleOutP" rowspan="2">时段</td>
		<td class="tabletitleOut" colspan="2">PM<sub>2.5</sub></td>
		<td class="tabletitleOut" colspan="2">PM<sub>10</sub></td>
		<td class="tabletitleOut" colspan="2">NO<sub>2</sub></td>
		<td class="tabletitleOut" colspan="2">O3<sub>-1h</sub></td>
		<td class="tabletitleOut" colspan="2">O3<sub>-8h</sub></td>
		<td class="tabletitleOut" colspan="2">AQI</td>
	</tr>
    <tr>
		<td class="tabletitleOut">监测中心</td>
		<td class="tabletitleOut">气象局</td>
		<td class="tabletitleOut">监测中心</td>
		<td class="tabletitleOut">气象局</td>
		<td class="tabletitleOut">监测中心</td>
		<td class="tabletitleOut">气象局</td>
		<td class="tabletitleOut">监测中心</td>
		<td class="tabletitleOut">气象局</td>
		<td class="tabletitleOut">监测中心</td>
		<td class="tabletitleOut">气象局</td>
		<td class="tabletitleOut">监测中心</td>
		<td class="tabletitleOut">气象局</td>
	</tr>   
   </table>
   
   </div>
   
   </div>
   
</div>
<div class="btnArea">
       <div class="btns">
          <div id="forePreview" class="button_Bottom">预览</div>
          <div id="foreSave" class="button_Bottom" >保存</div>
          <div id="foreCheck" class="button_Bottom" >审核</div>
          <div id="forePub" class="button_Bottom" style="display:none;" >发布</div>
       </div>
   </div>
<textarea  name="txtHidePeriodAQITxtTemplete" id="txtHidePeriodAQITxtTemplete" style="display:none;">
上海市空气质量预报			
(	{PublishDate}	{Hour}发布）
时段	AQI	空气质量等级	首要污染物
明天上午（06时—12时）	{RangeTomMorning}	{LevelTomMorning}	{AQIItemTomMorning}
明天下午（12时—20时）	{RangeTomAfternoon}	{LevelTomAfternoon}	{AQIItemTomAfternoon}
明天夜间（20时—06时）  {RangeTomNight}	{LevelTomNight}	{AQIItemTomNight}
后天      （00时—24时）  {RangeAfterDay}	{LevelAfterDay}	{AQIItemAfterDay}
第三日    （00时—24时）  {RangeThirdDay}	{LevelThirdDay}	{AQIItemThirdDay}
			
上海中心气象台
上海市环境监测中心
联合发布
</textarea>
<textarea  name="txtHidePeriodAQITxtTemplete" id="txtHidePeriodAQITxtTwoLines" style="display:none;">
上海市空气质量预报			
(	{PublishDate}	{Hour}发布）
时段	AQI	空气质量等级	首要污染物
今天上午（06时—12时）	{RangeTomMorning}	{LevelTomMorning}	{AQIItemTomMorning}
今天下午（12时—20时）	{RangeTomAfternoon}	{LevelTomAfternoon}	{AQIItemTomAfternoon}
今天夜间（20时—06时）  {RangeTomNight}	{LevelTomNight}	{AQIItemTomNight}
明天白天（06时—20时）  {RangeAfterDay}	{LevelAfterDay}	{AQIItemAfterDay}
			
上海中心气象台
上海市环境监测中心
联合发布
</textarea>
<textarea  name="txtHidePeriodAQITxtTemplete" id="txtHidePeriodAQITxtOneLine" style="display:none;">
上海市空气质量预报			
(	{PublishDate}	{Hour}发布）
时段	AQI	空气质量等级	首要污染物
今天下午（12时—20时）	{RangeTomAfternoon}	{LevelTomAfternoon}	{AQIItemTomAfternoon}
今天夜间（20时—06时）  {RangeTomNight}	{LevelTomNight}	{AQIItemTomNight}
明天白天（06时—20时）  {RangeAfterDay}	{LevelAfterDay}	{AQIItemAfterDay}
			
上海中心气象台
上海市环境监测中心
联合发布
</textarea>
<textarea name="txtHidePeriodAQIMsgTemplete" id="txtHidePeriodAQIMsgTemplete" cols="20" rows="2" style=" display:none;">中心气象台和监测中心{PublishDate}{Hour}联合发布的上海市空气质量预报：{TodayDay}夜间{LevelNight}，AQI为{RangeTonight}；{TomorrowDay}上午{LevelTomMorning}，AQI为{RangeTomMorning}；下午{LevelTomAfternoon}，AQI为{RangeTomAfternoon}；夜间{LevelTomNight}，AQI为{RangeTomNight}；{AfterDay}{LevelAfterDay}，AQI为{RangeAfterDay}；{third}{LevelthirdDay}，AQI为{RangethirdDay}。</textarea>
<textarea name="txtHidePeriodAQIMsgTemplete" id="txtHidePeriodAQIMsgTempleteTwoLines" cols="20" rows="2" style=" display:none;">中心气象台和监测中心{PublishDate}{Hour}联合发布的上海市空气质量预报：{TodayDay}上午{LevelTomMorning}，AQI为{RangeTomMorning}；下午{LevelTomAfternoon}，AQI为{RangeTomAfternoon}；夜间{LevelTomNight}，AQI为{RangeTomNight}；{TomorrowDay}{LevelAfterDay}，AQI为{RangeAfterDay}；{secondDay}{LevelsecondDay}，AQI为{RangesecondDay}。</textarea>
<textarea name="txtHidePeriodAQIMsgTemplete" id="txtHidePeriodAQIMsgTempleteOneLine" cols="20" rows="2" style=" display:none;">中心气象台和监测中心{PublishDate}{Hour}联合发布的上海市空气质量预报：{TodayDay}下午{LevelTomAfternoon}，AQI为{RangeTomAfternoon}；夜间{LevelTomNight}，AQI为{RangeTomNight}；{TomorrowDay}{LevelAfterDay}，AQI为{RangeAfterDay}；{secondDay}{LevelsecondDay}，AQI为{RangesecondDay}。</textarea>
<!--FTP地址集合 -->
<input type="hidden" id="FtpCollection" value="InfoCenterFtpII,AQI_SH_YYYYMMDD0600.txt;SciServiceCenterII,AQI_SH_YYYYMMDD0600.txt;AQILocalII,AQI_SH_YYYYMMDD0600.txt;AQILocal62II,scuem_air_YYYYMMDD0600.txt;62WebSiteII,scuem_air_YYYYMMDD0600.txt" />
</body>
</html>
