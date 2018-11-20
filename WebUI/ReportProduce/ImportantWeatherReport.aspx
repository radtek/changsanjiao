<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportantWeatherReport.aspx.cs" Inherits="ReportProduce_ImportantWeatherReport" %>
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
    <script src="../AQI/js/ImportantWeatherReport.js" type="text/javascript"></script>
    <link href="../css/ImportantWeatherReport.css" rel="stylesheet" type="text/css" />
        <script src="../AQI/js/main.js" type="text/javascript"></script>

    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>

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
            </tr>
        </table>
      </div>
      
   </div>  
  <div class="topBtns">
      <table>
         <tr>
         <td class="historyTd"><div id="getHihstory" class="hidtoryBtn">历史记录</div></td>
         <td class="typeTd"><div>产品类型：<span id="productType"></span></div></td>
         <td class="timeTd"><div><span>查询日期：</span><input   id="searchDate" onclick=" WdatePicker({dateFmt:'yyyy年MM月dd日'});"  onchange="changeDate(this)"  type="text" readonly="readonly" value=""/></div></td>
         <td class="btnTd">
           <div>
           <div class="wordBtn" id="clickQuery">一键查询</div>
           <div class="wordBtn" id="readMOdel">读取模板</div>
          </div>
          </td>
         </tr>
      </table>                      
  </div>
  </div>
        <div class="wordContent">    
         <div class="docTitle">华东区域气象中心专报</div>
         <div class="subTitle">—环境气象专报</div>
         <div class="issue" id="PO_docIssue"><input id="PO_year" class="issue" value="2016" /><span>年第</span><input id="PO_issueNum"class="issue" value="40" /><span>期</span></div>
         <div class="titleAndOra">
         <div id="docFullDate" class="docDate"><input id="PO_docDate" class="docDate" value="2016年1月27日16时" /></div>
         <div class="ora"><span >签发人：</span><input type="text" id="PO_Sing" value="雷小途" /></div><div style="clear:both"></div>
         </div>
         <div class="contentMainTitle"><textarea type="text" id="PO_TodayDay">15日-17日上午华东中、北部霾天气和PM2.5重污染过程</textarea></div>
         <div class="firstTitle">一、预报结论</div>
         <div class="contentText"><textarea name="PO_SHWeatherSys" id="PO_SHWeatherSys" class="contentTextBox">
    受静稳天气和冷空气输送影响，15日-17日上午，华东中部、北部地区将出现中度霾及重度污染过程，其中华东北部、中部地区的重污染时段分别出现在15日傍晚-16日上午及16日傍晚-夜间，PM2.5峰值浓度分别为300ug/m3和250ug/m3，17日上午随着冷空气主体南下，本次霾及污染过程结束。
具体预报如下：
15日-16日上午，山东大部，安徽北部、江苏、上海有轻度-中度霾，局部重度霾；AQI为中度-重度污染，局部短时严重污染，首要污染物为PM2.5。16日中午-17日早晨，江苏中南部、安徽、上海、浙江有轻度-中度霾，局部重度霾；AQI为中度-重度污染，局部短时严重污染，首要污染物为PM2.5。17日上午华东地区霾及污染天气逐渐结束。具体落区预报见图1、图2，华东重点城市预报详见表1。

    </textarea></div>
         
       </div>
       <div class="wordContent">
        <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><img id="PO_TomHazeDropZone" runat="server" class="scrollImg Img_024" src="../css/images/noImg.GIF" alt="" /></div>            
             <div class="imgDiv"><div class="selImg"></div><img id="PO_AfterHazeDropZone" runat="server" class="scrollImg Img_048" src="../css/images/noImg.GIF" alt="" /></div>
          </div>
          <div class="annoDiv"><input type="text" class="anno" id="PO_ImgAnno1" value="图1   1月16日-17日华东区域霾等级落区预报" /></div>
          <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><img id="PO_TomAQIDropZone" runat="server" class="scrollImg Img_024" src="../css/images/noImg.GIF" alt="" /></div>
             <div class="imgDiv"><div class="selImg"></div><img id="PO_AfterAQIDropZone" runat="server" class="scrollImg Img_048" src="../css/images/noImg.GIF" alt="" /></div>      
          </div>
          <div class="annoDiv"><input class="anno" type="text" id="PO_ImgAnno2" value="图2  1月16日-17日华东区域AQI（PM2.5）落区预报" /></div>
          </div>
       <div class="wordContent">
       <div class="tableTitle"><input type="text" id="PO_TableTitle1" value="表1 1月16日华东区域重点城市环境气象预报" /></div>
    <div class="contentText">
          <table class="contentTable firstTable">
          <tr class="titleBottomLine"><td></td><td colspan="3">AQI</td><td rowspan="2">空气污染气象条件等级</td><td rowspan="2">霾</td></tr>
            <tr class="titleBottomLine"><td></td><td>污染等级</td><td>首要污染物</td><td>AQI指数</td></tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>上海</span></div></td>
                <td><input id="PO_PoLevel_Shanghai" value="良" /></td>
                <td><input id="PO_FirstItem_Shanghai" value="PM2.5" /></td>
                <td><input id="PO_AQI_Shanghai"  value="65" /></td>
                <td><input id="PO_AirPolLevel_Shanghai" value="二级" /></td>
                <td><input id="PO_Haze_Shanghai" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>南京</span></div></td>
                <td><input id="PO_PoLevel_Nanjing" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Nanjing" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Nanjing" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Nanjing" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Nanjing" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>苏州</span></div></td>
                <td><input id="PO_PoLevel_Suzhou" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Suzhou" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Suzhou" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Suzhou" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Suzhou" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>杭州</span></div></td>
                <td><input id="PO_PoLevel_Hangzhou" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Hangzhou" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Hangzhou" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Hangzhou" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Hangzhou" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>宁波</span></div></td>
                <td><input id="PO_PoLevel_Ningbo" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Ningbo" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Ningbo" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Ningbo" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Ningbo" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>合肥</span></div></td>
                <td><input id="PO_PoLevel_Hefei" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Hefei" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Hefei" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Hefei" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Hefei" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>福州</span></div></td>
                <td><input id="PO_PoLevel_Fuzhou" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Fuzhou" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Fuzhou" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Fuzhou" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Fuzhou" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>厦门</span></div></td>
                <td><input id="PO_PoLevel_Xiamen" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Xiamen" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Xiamen" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Xiamen" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Xiamen" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>南昌</span></div></td>
                <td><input id="PO_PoLevel_Nanchang" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Nanchang" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Nanchang" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Nanchang" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Nanchang" name="PO_WindSpeed1" value="无" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>济南</span></div></td>
                <td><input id="PO_PoLevel_Jinan" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Jinan" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Jinan" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Jinan" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Jinan" name="PO_WindSpeed1" value="无" /></td>
            </tr> 
            <tr>
                <td class="cityTd"><div class="cityCell"><span>青岛</span></div></td>
                <td><input id="PO_PoLevel_Qingdao" name="PO_PoLevel1" value="良" /></td>
                <td><input id="PO_FirstItem_Qingdao" name="PO_PoLevel1" value="PM2.5" /></td>
                <td><input id="PO_AQI_Qingdao" name="PO_PoLevel1" value="65" /></td>
                <td><input id="PO_AirPolLevel_Qingdao" name="PO_Weather1" value="二级" /></td>
                <td><input id="PO_Haze_Qingdao" name="PO_WindSpeed1" value="无" /></td>
            </tr>          
          </table>
          </div>
          <div class="firstTitle">二、预报理由简述</div>
          <div class="contentText"><textarea name="PO_reportFocus" id="PO_reportFocus" class="contentTextBox_Rich">
      14日下午开始，华东北部受静稳天气形势控制，有利于污染物累积，14日PM2.5浓度普遍超过115ug/m3，15日08时山东西北部PM2.5浓度超过200ug/m3，局地超过300ug/m3，（图3）。16日05时，随着北方冷空气扩散南下，受上游输送影响，预计15日夜间-16日上午济南PM2.5峰值浓度超过300ug/m3，上海16日夜间-17日早晨PM2.5峰值浓度将超过250ug/m3。17日上午，随着冷空气主体南下，华东地区霾天气和污染天气过程将逐渐结束。16日06时、17日00时华东区域PM2.5浓度及地面风分布预报见图4。
          </textarea></div>
       </div>

       <div class="lastPage">
          <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><img id="PO_ImgTwoLeft" runat="server" class="scrollImg Img_024" src="../css/images/noImg.GIF" alt="" /></div>
             <div class="imgDiv"><div class="selImg"></div><img id="PO_ImgTwoRight" runat="server" class="scrollImg Img_048" src="../css/images/noImg.GIF" alt="" /></div>
          </div>
          <div class="annoDiv"><textarea class="anno" type="text" id="PO_ImgAnno3">图3 15日08时华东地区PM2.5质量浓度（环保数据）、天气现象和相对湿度（气象数据）实况监测（图左），华东地区14日AQI实况分布（图右</textarea></div>

          <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><img id="PO_ImgThreeLeft" runat="server" class="scrollImg Img_024" src="../css/images/noImg.GIF" alt="" /></div>
             <div class="imgDiv"><div class="selImg"></div><img id="PO_ImgThreeRight" runat="server" class="scrollImg Img_048" src="../css/images/noImg.GIF" alt="" /></div> 
          </div>
           <div class="annoDiv"><textarea type="text" class="anno" id="PO_ImgAnno4">图4  16日06时（左）、17日00时（右）华东区域PM2.5质量浓度及地面风分布</textarea></div>
             <div class="contentText"><textarea  id="PO_reportTo" class="contentTextBox_Rich">
      报：中国气象局郑国光局长，许小峰、宇如聪、沈晓农、矫梅燕副局长，刘实纪检组长，于新文副局长（发布时需请示领导，年中文件未包括该范围）
          </textarea></div>
          
             <div class="contentText"><textarea  id="PO_SendOras" class="contentTextBox_Rich">
      送：中国气象局值班室、预报与网络司、应急减灾与公共服务司、综合观测司、国家气象中心、公共气象服务中心、山东气象局、江苏气象局、安徽气象局、上海气象局、浙江气象局、江西气象局、福建气象局、青岛市气象局、宁波市气象局、厦门市气象局
          </textarea></div>
       <div class="bottomEditor"><div class="editor"><span>主编：</span><input id="PO_editor" name="PO_editor" class="editorText" type="text" value="耿福海"/></div><div class="reporter"><span>责任编辑：</span><input id="PO_reporter" name="PO_reporter" class="reporterText" type="text" value="许建明、毛卓成、马井会"/></div><div style="clear:"></div></div>
        <div class="bottomEditor">编制单位：上海市环境气象中心联系电话：021－53896141</div>
       </div>     
       </div>              
<div class="btnArea">
       <div class="btns">
       <div id="foreSave" class="button_Bottom" >保存</div> 
       <div id="forePreview" style="display:none;" class="button_Bottom"><a id="previewLink" href="<%=PageOfficeLink.OpenWindow("http://222.66.83.21:8282/PEMFCShare/AQI/PageOfficePreview/PolWeatherAnalysisPreview.aspx?filePath=ImportWeather.doc&ProductName=ImportWeather","width=1200px;height=700px;")%>" >预览</a></div>
        <div id="forePub" class="button_Bottom" style="display:none;">发布</div>           
       </div>
   </div>
   <div style="display:none;">
   <div id="upLoadFileArea">
   <form id="Form1" runat="server">      
          <div id="recImg" class="recImg">
              <img id="sourceImg" class="sourceImg" src="../css/images/noImg.GIF" alt="">
          </div>
          <div class="selImgContent"><img id="selDisImg" class="selDisImg" alt="">
             <div class="fileSelect">
              <asp:FileUpload ID="FileUpload1" class="upFile" runat="server"  onchange="preViewImg(this)"/>
              </div>
              <div class="fileSelect">
              <asp:Button ID="BtnUpload" class="upBtn" runat="server" Text="替 换" OnClick="BtnUpload_Click"  />
              </div>
      </div>
   
   <input type="hidden" runat="server" id="SelImgID" value="" />
    </form>
    </div>
    </div>
    <input name="txtHideHostName" type="hidden" id="txtHideHostName" value="http://localhost:21765/WebUI/"/>
     <!--FTP地址集合 -->
    <input type="hidden" id="FtpCollection" value="ImporWeaReport1,华东区域环境气象专报YYYYMMDD.doc;ImporWeaReport2,华东区域环境气象专报-YYYY年第N期-MMDD.pdf" />
</body>

</html>
