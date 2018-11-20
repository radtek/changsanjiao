<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainCityForecast.aspx.cs" Inherits="ReportProduce_MainCityForecast" %>
<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228"
    Namespace="PageOffice" TagPrefix="po" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../AQI/js/MainCityForecast.js" type="text/javascript"></script>
    <link href="../css/MainCityForecast.css" rel="stylesheet" type="text/css" />

    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
<div class="total">
<div class="topFixed">
  <div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table>
            <tr><th class="attrName">预报员：</th>
            <td class="attrValue" id="forecaster"></td>
            <th class="attrName">预报时间：</th>
            <td id="forecastTime" class="attrValue"></td>
            <th class="attrName">预报时次：</th>
            <td id="forecastTimeLevel" class="attrValue"></td>
            <th class="attrName">期数：</th>
            <td  class="attrValue"><input id="issueSet" type="text" value=""/></td>
            </tr>
        </table>
      </div>      
   </div>  
  <div class="topBtns">
      <table>
         <tr>
         <td class="historyTd"><div id="getHihstory" class="hidtoryBtn">历史记录</div></td>
         <td class="typeTd"><div>产品类型：<span id="productType"></span></div></td>
         <td class="timeTd"><div><span>查询日期：</span><input  id="searchDate" onclick=" WdatePicker({dateFmt:'yyyy年MM月dd日'});"  onchange="changeDate(this)"  type="text" readonly="readonly" value=""/></div></td>
         <td class="btnTd">
           <div>
           <div class="wordBtn"  id="clickQuery">一键查询</div>
           <div id="readMOdel" class="wordBtn">读取模板</div>
          </div>
          </td>
         </tr>
      </table>                      
  </div>
  </div>
  <div class="docContent">
        <div class="wordContent">    
         <div class="docTitle">华东区域重点城市预报</div>
         <div class="issue" style="display:none;" id="PO_docIssue"><input id="PO_year" value="2016" /><span>年第</span><input type="text" id="PO_issueNum" value="40" /><span>期</span></div>
         
         <div class="titleAndOra">
         <div id="docFullDate" class="docDate"><input type="text" id="PO_docDate" class="docDate" value="2015年10月27日14:30发布" /></div>
         <div class="ora"><span >长三角环境气象预报预警中心</span></div><div style="clear:both"></div>
         </div>
         
         <div class="firstTitle"><input type="text" id="Po_TodayDate" value="2015年10月28日20时～29日20时预报:" /></div>
         <div class="contentText"><textarea  name="PO_ForeText" id="PO_ForeText" class="contentTextBox">
29日，华东北部受冷空气影响，空气质量总体为PM2.5轻度污染，短时中度污染，短时轻微霾；华东中部受低涡东移影响，有明显降水过程，空气质量总体为PM2.5优-良或良，无霾。
    </textarea></div>    
    <div class="contentText">
          <table class="contentTable firstTable">
          <tr class="titleBottomLine"><td></td><td colspan="3"></td><td rowspan="2">空气污染气象条件等级</td><td rowspan="2">霾</td></tr>
            <tr class="titleBottomLine"><td></td><td>污染等级</td><td>首要污染物</td><td>AQI指数</td></tr>
            <tr>
                <td><div class="cityCell"><span>上海</span></div></td>
                <td><input id="PO_PoLevel_Shanghai" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Shanghai" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Shanghai" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Shanghai" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Shanghai" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>南京</span></div></td>
                <td><input id="PO_PoLevel_Nanjing" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Nanjing" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Nanjing" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Nanjing" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Nanjing" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>苏州</span></div></td>
                <td><input id="PO_PoLevel_Suzhou" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Suzhou" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Suzhou" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Suzhou" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Suzhou" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>杭州</span></div></td>
                <td><input id="PO_PoLevel_Hangzhou" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Hangzhou" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Hangzhou" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Hangzhou" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Hangzhou" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>宁波</span></div></td>
                <td><input id="PO_PoLevel_Ningbo" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Ningbo" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Ningbo" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Ningbo" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Ningbo" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>合肥</span></div></td>
                <td><input id="PO_PoLevel_Hefei" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Hefei" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Hefei" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Hefei" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Hefei" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>福州</span></div></td>
                <td><input id="PO_PoLevel_Fuzhou" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Fuzhou" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Fuzhou" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Fuzhou" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Fuzhou" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>厦门</span></div></td>
                <td><input id="PO_PoLevel_Xiamen" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Xiamen" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Xiamen" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Xiamen" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Xiamen" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>南昌</span></div></td>
                <td><input id="PO_PoLevel_Nanchang" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Nanchang" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Nanchang" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Nanchang" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Nanchang" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td><div class="cityCell"><span>济南</span></div></td>
                <td><input id="PO_PoLevel_Jinan" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Jinan" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Jinan" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Jinan" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Jinan" name="PO_WindSpeed1" value="无" /></td>
            </tr> 
            <tr>
                <td><div class="cityCell"><span>青岛</span></div></td>
                <td><input id="PO_PoLevel_Qingdao" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Qingdao" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Qingdao" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Qingdao" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Qingdao" name="PO_WindSpeed1" value="无" /></td>
            </tr>          
          </table>
          </div>
            <div class="bottomEditor"><div class="editor"><span>制作：</span><input id="PO_editor" name="PO_editor" class="editorText" type="text" value="毛卓成"/></div><div class="phone"><span>签发：</span><input id="PO_Sign" name="PO_editor" class="editorText" type="text" value="耿福海"/></div><div style="clear:"></div></div>
          </div>
        
    </div>
        <div style="clear:both"></div>
       </div>       
    <div class="btnArea">
       <div class="btns">                   
          
         <div id="foreSave" class="button_Bottom" >保存</div> 
          <div class="button_Bottom"  style="display:none;" id="preview"><a id="previewLink"  href="<%=PageOfficeLink.OpenWindow("http://222.66.83.21:8282/PEMFCShare/AQI/PageOfficePreview/FutureTenDaysPreview.aspx?filePath=MainCityForecast.doc&ProductName=MainCityForecast","width=1200px;height=700px;")%>" >预览</a></div>                   
          <div id="forePub" style="display:none;" class="button_Bottom">发布</div>            
       </div>
   </div>
   <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
    </div>
    <div id="closePreview" class="closeBtn"></div>
    </form>
    <input name="txtHideHostName" type="hidden" id="txtHideHostName" value="http://localhost:21765/WebUI/"/>
     <!--FTP地址集合 -->
    <input type="hidden" id="FtpCollection" value="MainCityForecast1,华东区域重点城市预报第N期.doc;MainCityForecast2,华东区域重点城市预报-YYYY年第N期-MMDD.pdf" />
</body>

</html>
