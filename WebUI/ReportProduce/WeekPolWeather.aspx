<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeekPolWeather.aspx.cs" Inherits="ReportProduce_WeekPolWeather" %>
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
    <script src="../AQI/js/WeekPolWeather.js" type="text/javascript"></script>
    <link href="../css/WeekPolWeather.css" rel="stylesheet" type="text/css" />
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
         <td class="timeTd"><div><span>预报时间：</span><input   id="searchDate" onclick=" WdatePicker({dateFmt:'yyyy年MM月dd日'});"  onchange="changeDate(this)"  type="text" readonly="readonly" value=""/></div></td>
         <td class="btnTd">
           <div>
           <div id="clickQuery" class="wordBtn">一键查询</div>
           <div id="readMOdel" class="wordBtn">读取模板</div>
          </div>
          </td>
         </tr>
      </table>
      </div>                      
  </div>
        <div class="wordContent">    
         <div class="docTitle">长三角地区一周污染天气展望</div>
         <div class="issue" id="PO_docIssue"><input type="text" id="PO_year" class="issue" value="2016" /><span>年第</span><input type="text" id="PO_issueNum" class="issue" value="40" />期</div>
         <div class="titleAndOra">
         <div id="docFullDate" class="docDate"><input type="text" id="PO_docDate" class="docDate" value="2015年10月27日17时" /></div>
         <div class="ora"><span >签发人：</span><input type="text" id="PO_Sing" value="许建明" /><div style="clear:both"></div></div>
         </div>         
         <div class="firstTitle">一、上周污染天气回顾：</div>
         <div class="contentText"><textarea name="PO_SHWeatherSys" id="PO_SHWeatherSys" class="contentTextBox">
    上周华东地区PM2.5污染主要出现在山东以及苏皖中北部地区，其中山东西部部分地区连续一周均为污染日，上海及周边地区也出现了1-3天污染；O3污染主要出现在苏南和浙北部分地区，个别地区出现了6日污染。过去一周华东地区PM2.5和O3污染日数分布见图1：
    </textarea></div>

    <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><div id="TomHazeDropZone_Div" runat="server"><img id="PO_TomHazeDropZone" runat="server" class="scrollImg Img_024"  alt="" /></div></div>             
          </div>
          <div class="annoDiv"><input type="text" class="anno" id="PO_ImgAnno1" value="图1   过去一周（{PastStart}-{PastEnd}）华东地区PM2.5和O3污染日数分布图" /></div>
          </div>
          <div class="wordContent"> 
          <div class="firstTitle">二、未来一周长三角区域污染天气预报：</div>
          <div class="contentText"><textarea name="PO_reportFocus" id="PO_reportFocus" class="contentTextBox_Rich">
      10月28日长三角地区受高压控制，气压场较弱，空气质量总体为PM2.5良-轻度污染，北部地区有轻微霾；29日长三角地区受冷空气影响，北部地区空气质量为PM2.5良-轻度污染，局部地区有轻微霾，南部地区有降水，空气质量以良为主，无霾；11月1日长三角北部地区受高压控制，气压场较弱，空气质量为PM2.5良-轻度污染，局部地区有轻微霾，南部地区有降水，空气质量为优-良，无霾。其余时段，长三角地区空气质量总体为优-良，无霾。图2为长三角地区重点城市未来一周AQI逐日预报，表1为长三角地区重点城市未来一周霾预报。
          </textarea></div>
          <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><img id="PO_TomAQIDropZone" runat="server" class="scrollImg Img_024"  alt="" /></div>                        
          </div>
          <div class="annoDiv"><input class="anno" type="text" id="PO_ImgAnno2" value="图2  未来一周（{FutureStart}-{FutureEnd}）长三角重点城市空气质量预报，纵坐标为AQI指数，横坐标为时间（说明：绿色为优、黄色为良、橙色为轻度污染、红色为中度污染、紫色为重度污染、褐红色为严重污染）" /></div>
            <div class="tableTitle"><span>表1    长三角地区重点城市未来一周霾预报</span></div>
          <div class="contentText">
          <table class="contentTable firstTable">
          <tr class="titleBottomLine"><td class="firstCell"></td><td class="dateCell"><input type="text" id="PO_monWeekday" value="周一" /><br/><input type="text" id="PO_mondayDate" value="10月25日" /></td><td class="dateCell"><input type="text" id="PO_tueWeekday" value="周二" /><br/><input type="text" id="PO_tueDate" value="10月25日" /></td><td class="dateCell"><input type="text" id="PO_wedWeekday" value="周三" /><br/><input type="text" id="PO_wedDate" value="10月25日" /></td><td class="dateCell"><input type="text" id="PO_thurWeekday" value="周四" /><br/><input type="text" id="PO_thurDate" value="10月25日" /></td><td class="dateCell"><input type="text" id="PO_friWeekday" value="周五" /><br/><input type="text" id="PO_friDate" value="10月25日" /></td><td class="dateCell"><input type="text" id="PO_saturWeekday" value="周六" /><br/><input type="text" id="PO_saturDate" value="10月25日" /></td><td class="dateCell"><input type="text" id="PO_sunWeekday" value="周日" /><br/><input type="text" id="PO_sunDate" value="10月25日" /></td></tr>            
            <tr>
                <td class="cityTd"><div class="cityCell"><span>上海</span></div></td>
                <td><input id="PO_HazeMonday_Shanghai" value="无霾" /></td>
                <td><input id="PO_HazeTue_Shanghai" value="无霾" /></td>
                <td><input id="PO_HazeWed_Shanghai"  value="无霾" /></td>
                <td><input id="PO_HazeThur_Shanghai"  value="无霾" /></td>
                <td><input id="PO_HazeFriday_Shanghai"  value="无霾" /></td>
                <td><input id="PO_HazeSaturday_Shanghai"  value="无霾" /></td>
                <td><input id="PO_HazeSunday_Shanghai"  value="无霾" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>合肥</span></div></td>
                <td><input id="PO_HazeMonday_Hefei" value="无霾" /></td>
                <td><input id="PO_HazeTue_Hefei" value="无霾" /></td>
                <td><input id="PO_HazeWed_Hefei"  value="无霾" /></td>
                <td><input id="PO_HazeThur_Hefei"  value="无霾" /></td>
                <td><input id="PO_HazeFriday_Hefei"  value="无霾" /></td>
                <td><input id="PO_HazeSaturday_Hefei"  value="无霾" /></td>
                <td><input id="PO_HazeSunday_Hefei"  value="无霾" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>南京</span></div></td>
                <td><input id="PO_HazeMonday_Nanjing" value="无霾" /></td>
                <td><input id="PO_HazeTue_Nanjing" value="无霾" /></td>
                <td><input id="PO_HazeWed_Nanjing"  value="无霾" /></td>
                <td><input id="PO_HazeThur_Nanjing"  value="无霾" /></td>
                <td><input id="PO_HazeFriday_Nanjing"  value="无霾" /></td>
                <td><input id="PO_HazeSaturday_Nanjing"  value="无霾" /></td>
                <td><input id="PO_HazeSunday_Nanjing"  value="无霾" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>苏州</span></div></td>
                <td><input id="PO_HazeMonday_Suzhou" value="无霾" /></td>
                <td><input id="PO_HazeTue_Suzhou" value="无霾" /></td>
                <td><input id="PO_HazeWed_Suzhou"  value="无霾" /></td>
                <td><input id="PO_HazeThur_Suzhou"  value="无霾" /></td>
                <td><input id="PO_HazeFriday_Suzhou"  value="无霾" /></td>
                <td><input id="PO_HazeSaturday_Suzhou"  value="无霾" /></td>
                <td><input id="PO_HazeSunday_Suzhou"  value="无霾" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>杭州</span></div></td>
                <td><input id="PO_HazeMonday_Hangzhou" value="无霾" /></td>
                <td><input id="PO_HazeTue_Hangzhou" value="无霾" /></td>
                <td><input id="PO_HazeWed_Hangzhou"  value="无霾" /></td>
                <td><input id="PO_HazeThur_Hangzhou"  value="无霾" /></td>
                <td><input id="PO_HazeFriday_Hangzhou"  value="无霾" /></td>
                <td><input id="PO_HazeSaturday_Hangzhou"  value="无霾" /></td>
                <td><input id="PO_HazeSunday_Hangzhou"  value="无霾" /></td>
            </tr>
            <tr>
                <td class="cityTd"><div class="cityCell"><span>宁波</span></div></td>
                <td><input id="PO_HazeMonday_Ningbo" value="无霾" /></td>
                <td><input id="PO_HazeTue_Ningbo" value="无霾" /></td>
                <td><input id="PO_HazeWed_Ningbo"  value="无霾" /></td>
                <td><input id="PO_HazeThur_Ningbo"  value="无霾" /></td>
                <td><input id="PO_HazeFriday_Ningbo"  value="无霾" /></td>
                <td><input id="PO_HazeSaturday_Ningbo"  value="无霾" /></td>
                <td><input id="PO_HazeSunday_Ningbo"  value="无霾" /></td>
            </tr>
            </table>
            </div>
            <div class="contentText"><textarea  id="PO_SendOras" class="contentTextBox_Rich">
报：国家环境气象中心、长三角三省一市局领导、环境气象业务单位、业务单位主要负责人、分管职能处室主要负责人
          </textarea></div>
       <div class="bottomEditor"><div class="editor"><span>编辑：</span><input id="PO_editor" name="PO_editor" class="editorText" type="text" value="陈镭"/></div><div class="reporter"><span>审核：</span><input id="PO_reporter" name="PO_reporter" class="reporterText" type="text" value="毛卓成"/></div><div style="clear:"></div></div>
        <div class="bottomPhone"><span class="bottomLeftSpan">编制单位：长三角环境气象预报预警中心   </span><span class="bottomRightSpan">联系电话：021－53896141</span></div>
          <div style="clear:both"></div>                            
       </div>
    
    </div>
<div class="btnArea">
       <div class="btns">
        <div id="foreSave" class="button_Bottom" >保存</div>      
          <div id="forePreview" class="button_Bottom" style="display:none;"><a id="previewLink" href="<%=PageOfficeLink.OpenWindow("../AQI/PageOfficePreview/PolWeatherAnalysisPreview.aspx?filePath=WeekPolWeather.doc&ProductName=WeekPolWeather","width=1200px;height=700px;")%>" >预览</a></div>         
           <div id="forePub" class="button_Bottom" style="display:none">发布</div>
       </div>
   </div>
   <div style="display:none;">
   <div id="upLoadFileArea">
   <form id="Form1" runat="server">      
          <div id="recImg" class="recImg">
              <img id="sourceImg" class="sourceImg" src="../css/images/noImg.GIF" alt="">
          </div>
          <div class="selImgContent"><div id="selImgDiv" class="selImgDiv"><img id="selDisImg" runat="server" class="selDisImg" alt=""/></div>
             <div class="fileSelect">
              <asp:FileUpload ID="FileUpload1" class="upFile" runat="server"  onchange="preViewImg(this)"/>
              </div>
              <div class="fileSelect">
              <asp:Button ID="BtnUpload" class="upBtn" runat="server" Text="替 换" OnClick="BtnUpload_Click"  />
              <asp:Button ID="PDFProcess" style="display:none;" runat="server" OnClick="BtnPDF_Click"/>
              </div>
      </div>
   
   <input type="hidden" runat="server" id="SelImgID" value="" />
    </form>
    </div>
    </div>
    <input name="txtHideHostName" type="hidden" id="txtHideHostName" value="http://localhost:21765/WebUI/"/>
    <!--FTP地址集合 -->
    <input type="hidden" id="FtpCollection" value="WeekPolWeaForesee1,长三角地区一周污染天气展望YYYY年第N期.doc;WeekPolWeaForesee2,长三角地区一周污染天气展望-YYYY年第N期-MMDD.pdf" />
</body>
</html>
