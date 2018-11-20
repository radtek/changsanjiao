<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ThreeGeneralReport.aspx.cs" Inherits="ReportProduce_ThreeGeneralReport" %>

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
    <script src="../AQI/js/HazeForecast.js?v=20160429" type="text/javascript"></script>
    <script src="../AQI/js/UVForecast.js?v=20160429" type="text/javascript"></script>
    <script src="../AQI/js/OzoneForecast.js?v=20160429" type="text/javascript"></script>

    <link href="../css/ThreeGeneralReport.css" rel="stylesheet" type="text/css" />
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>

  <div class="tableTop">
          <div id="topInfo" class="titleContent">
            <table>
                <tr><th class="thTab"></th><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td><td></td><th class="thButton"></th></tr>
            </table>
          </div>
    </div>
       <div class="hazeArea" id="hazeArea">
          <div class="areaTitle"><div class="titlePoint"></div><span>霾</span></div>
          <div class="selectArea">
            <div class="leftTitle">时段</div>
            <div class="selContent"><div class="singlePeriod" id="todayPeriod"><%=strToday %></div><div class="singlePeriod" id="tomorrowPeriod"><%=strTomorrow%></div><div class="singlePeriod" id="afterPeriod"><%=strAfter%></div></div>
          </div>
          <div class="selectArea">
              <div class="leftTitle">霾</div>
              <div class="selContent"><div class="singleHazeLevel">无霾</div><div class="singleHazeLevel">轻微霾</div><div class="singleHazeLevel">轻度霾</div><div class="singleHazeLevel">中度霾</div><div class="singleHazeLevel">重度霾</div><div class="singleHazeLevel">严重霾</div></div>
          </div>
          
          <div style="clear:both"></div>
          <div class="hazeOp">
              <div type="button" id="btnLast" class="hazeHistory">历史记录</div>    
              <div class="visibility"><span>24小时最小能见度：</span><input id="txtVisibilityMin" type="text" /><span>km</span></div>         
              <input name="txtPublishEndDate" id="txtPublishEndDate" class="dataPicker" onclick=" WdatePicker({dateFmt:'yyyy年MM月dd日 HH时'});"  onchange="changeDate(this)"  type="text" readonly="readonly" value="">
               
              <div class="pubDateSelLabel">发布日期：</div>             
          </div>
          
          <div class="txtArea_Haze" id="txtArea_Haze">
             <div class="hazeContentTitle">上海市霾的预报</div>
             <div class="hazeContentDate" id="hazeContentDate">{TodayDate}{Time}时发布</div>
             <div class="textTable">
                <table class="contentTable">                  
                   <tr><td class="tdDate" nowrap="nowrap"><%=strToday %></td><td class="tdHaze" nowrap="nowrap"><input type="text" class="hazeInput" id="todayLevel"/></td><td class="tdVis" nowrap="nowrap"><span class="visTag">最小能见度：</span><input class="hazeDis" id="visToday" type="text"/><span class="km">km</span></td></tr>
                   <tr><td class="tdDate" nowrap="nowrap"><%=strTomorrow%></td><td class="tdHaze" nowrap="nowrap"><input type="text" class="hazeInput" id="tomorrowLevel"/></td><td class="tdVis" nowrap="nowrap"><span class="visTag">最小能见度：</span><input class="hazeDis" type="text" id="visTom"/><span class="km">km</span></td></tr>
                   <tr><td class="tdDate" nowrap="nowrap"><%=strAfter%></td><td class="tdHaze" nowrap="nowrap"><input type="text" class="hazeInput" id="afterLevel"/></td><td class="tdVis" nowrap="nowrap"><span class="visTag">最小能见度：</span><input class="hazeDis" type="text" id="visAfter"/><span class="km">km</span></td></tr>
                </table>
             </div>
             <div class="pubOrganization">上海市城市环境气象中心</div>
              <div class="hazeContentTitleLeft">24小时霾预报</div>
             <div class="textTableLeft">
                <table class="contentTable_24">                                     
                   <tr>
                       <td class="tdDate" nowrap="nowrap"><%=strToday%></td>
                       <td class="tdHaze" nowrap="nowrap">
                        <div class="dateSelect" id="58367_FirstPol">
                          <div id="selectID" class="dateDiv">
                              <div class="firstPolText" id="58367_Item">无霾</div>
                              <div id="selIcon" class="selIcon"></div>
                          </div>
                              <ul id="firstPolUl" class="firstPolUl hide">
                                  <li><div>无霾</div></li>
                                  <li><div>轻微霾</div></li>
                                  <li><div>轻度霾</div></li>
                                  <li><div>中度霾</div></li>
                                  <li><div>重度霾</div></li>
                                  <li><div>严重霾</div></li>
                              </ul>
                           </div>
                       </td>
                       <td class="tdVis" nowrap="nowrap"><div id="saveHaze24" class="btnRight_24">保存</div></td>
                   </tr>                   
                </table>
             </div>
          </div>
          <div class="btnArea">
             <div id="hazePreview" class="button saveButton">预 览</div>
             <div id="hazeSave" class="button saveButton">保 存</div>
             <div id="hazeCheck" class="button checkButton" style="display:none;">审 核</div>
             <div id="hazePub" class="button pubButton" style="display:none;">发 布</div>
          </div>
       </div>
       <div class="rightArea" id="rightArea">
         <div class="uvArea">
           <div class="areaTitle"><div class="titlePoint"></div><span>紫外线</span></div>
           <div>
             <table class="report" cellspacing="5" cellpadding="0" border="0">
                <tr>
                    <th class="style2" scope="row">实况值：</th>
                        <td class="textbox" ime-mode: disabled;">
                        <input id="txtAvgUVAB" type="text" value="0" class="inputText" maxlength="4" name="inputID" />w/m <sup>2</sup></td>
                    <th scope="row"> 预报指数：</th>
                        <td class="textboxIndex"><input id="txtUVIndex" type="text" class="inputText" maxlength="3" value="0" name="txtUVIndex" /></td>
                    <th scope="row">紫外线等级：<input id="txtUVLevel" type="text" class="inputText" maxlength="3" value="0" name="txtUVLevel" /></th>
                        <td class="tdtext"></td>
                </tr>
            </table>
            <div class="rightBtnArea">
            <div id="btnLastOneUV" class="btnRight">历史记录</div>
            <div id="btnAutoGetUV" class="btnRight">获取实况</div>
            </div>
             
           </div>
           <div class="contentWithTitle">
                <div class="contentTitle">防护措施：</div>
                <div class="rightContent"><textarea id="uvProtectContent" class="uvProtectContent">不需要采取防护措施。</textarea></div>
                
           </div>
          <div class="contentWithTitle_Low">
             <div class="contentTitle">产品结果：</div>
             <div class="rightContent">
             <textarea id="txtToday16Result" class="uvProductContent"></textarea>
              <textarea id="tom10Result" class="uvProductContent"></textarea>
             </div>             
          </div>
          
           <div class="btnArea">
           <div id="uvSave" class="button saveButton">保 存</div>
           <div id="uvCheck" class="button saveButton" style="display:none;">审 核</div>
            <div id="uvPub" class="button pubButton" style="display:none;">发 布</div>
            </div>
         </div>
         <div class="ozoneArea" id="ozoneArea">
           <div class="areaTitle"><div class="titlePoint"></div><span>臭氧</span></div>
           <table class="displayTable" id="displayTable">
                <tr>
                <td class="valueContent">
                  <table border="0"class="report">
                        <tr>
                            <th class="tdName" scope="row">1小时平均浓度：</th>
                        <td  class="tdValue" >
                            <input name="txtO3_1H" type="text" id="txtO3_1H" value="0" maxlength="5" class="inputText">μg/m<sup>3</sup></td><input type="hidden" value="">
                        <th class="tdName_1" scope="row">8小时滑动平均浓度：</th>
                        <td class="tdValue">
                            <input name="txtO3_8H" type="text" id="txtO3_8H" maxlength="5" value="0" class="inputText">μg/m<sup>3</sup></td><input type="hidden" value="">
                        </tr>
                        <tr>
                            <th class="tdName" scope="row">1小时平均最大浓度范围：</th>
                        <td class="tdValue">
                            <input name="txtO3_1H_AVG" type="text" id="txtO3_1H_AVG" maxlength="10" value="0" class="inputText"/>ppb</td>
                        <th class="tdName_1" scope="row">8小时滑动平均最大浓度范围：</th>
                        <td class="tdValue">
                            <input name="txtO3_8H_AVG" type="text" id="txtO3_8H_AVG" value="0" maxlength="10"class="inputText"/>ppb</td>
                        </tr>
                        <tr>
                        <th class="tdName" scope="row">出现时段：</th>
                        <td class="tdValue">
                            <input name="txtO3_1H_Time" type="text" id="txtO3_1H_Time" maxlength="10" class="inputText" value="12-16"></td>
                        <th class="tdName_1" scope="row">出现时段：</th>
                        <td class="tdValue"><input name="txtO3_8H_Time" type="text" id="txtO3_8H_Time" maxlength="10" class="inputText" value="下午到傍晚"/></td>
                        </tr>
                    </table>
                </td>                
                </tr>
             </table>
              <div class="rightBtnArea">
              <div id="ozoneHistory" class="btnRight">历史数据</div>
             <div id="autoOzoneGet" class="btnRight">获取实况</div>
             </div>
           <div class="txtArea">
           <textarea id="ozoneContent" class="ozoneContent">
             {TomorrowDate}上海市区臭氧1小时平均浓度最大为{O3_1h}ppb，出现在{O3_1h_Time_Rang}时，8小时滑动平均浓度最大为{O3_8h}ppb，出现在{O3_8h_Time_Rang}。&lt;/p&gt;

                                               {TodayDate}17时
           </textarea>
           </div>
           <div class="btnArea">
           <div id="ozoneSave" class="button saveButton" >保 存</div>
           <div id="ozoneCheck" class="button saveButton" style="display:none;">审 核</div>
            <div id="oazonePub" class="button pubButton" style="display:none;">发 布</div>
            </div>
         </div>
       </div>

<textarea name="txtHideTempleteContent05" id="txtHideTempleteContent05" cols="20" rows="2" style="display:none;">

    

                   上海市霾的预报
               ({PubDateContent})

<%=strToday %>：{TodayContent}。
<%=strTomorrow%>：{TomorrowContent}。
<%=strAfter%>：{AfterDayContent}。


                               上海市城市环境气象中心


                    
    </textarea>
    <textarea id="txtHidePubDateTime" style="display:none;">{TodayDate}{Time}</textarea>
     <textarea name="txtHideTempleteContent" id="txtHideTempleteContent" cols="20" rows="2" style="display:none;">58362 1{UVLevel}000</textarea>
    <textarea name="txtHideTempleteContent_Tomorrow10" id="txtHideTempleteContent_Tomorrow10" cols="20" rows="2" style="display:none;">58362 1{UVLevel}000 20{10}</textarea>
    <textarea name="txtHideTempleteContent" id="txtHideOzoneTemplate" cols="20" rows="2" style="display:none;">{TomorrowDate}上海市区臭氧1小时平均浓度最大为{O3_1h}ppb，出现在{O3_1h_Time_Rang}时，8小时滑动平均浓度最大为{O3_8h}ppb，出现在{O3_8h_Time_Rang}。
    
                                                     {TodayDate}17时</textarea>
    <input type="hidden" id="hazeFtpCollection" value="InfoCenterFtp,hazeYYMMDDHH.txt;InfoCenterFtpIII,hazeYYMMDDHH.txt" />
    <input type="hidden" id="uvFtpCollection" value="Qbzq,SHMMDD07.URP" />
    <input type="hidden" id="uvFtpCollectionTom" value="Qbzq,SHMMDD01.URP" />
    <input type="hidden" id="ozoneFtpCollection" value="InfoCenterFtp,ozoneYYMMDD1700.txt;InfoCenterFtpIII,ozoneYYMMDD1700.txt;62WebSite,scuem_ozone_YYYYMMDD1600.txt" />
</body>
</html>