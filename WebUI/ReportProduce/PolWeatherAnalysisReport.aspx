<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PolWeatherAnalysisReport.aspx.cs" Inherits="ReportProduce_PolWeatherAnalysisReport" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228"
    Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
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
    <script src="../AQI/js/PolWeatherAnalysisReport.js" type="text/javascript"></script>
    <link href="../css/PolWeatherAnalysisReport.css" rel="stylesheet" type="text/css" />
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>    
</head>
<body>
<%--    <form id="form1" runat="server">--%>
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
         <td class="timeTd"><div><span>查询日期：</span><input   id="searchDate" onclick=" WdatePicker({dateFmt:'yyyy年MM月dd日'});"  onchange="changeDate(this)"  type="text" readonly="readonly" value=""/></div></td>
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
         <div class="docTitle">污染天气过程跟踪解析专报</div>
         <div class="issue" id="PO_docIssue"><input type="text" id="PO_year" class="issue" value="2016" /><span>年第</span><input type="text" id="PO_issueNum" class="issue" value="40" />期</div>
         <div class="titleAndOra">
         <div id="docFullDate" class="docDate"><input type="text" id="PO_docDate" class="docDate" value="2015年10月27日17时" /></div>
         <div class="ora"><span >长三角环境气象预报预警中心</span></div><div style="clear:both"></div>
         </div>         
         <div class="firstTitle">一、影响上海的天气系统及实况</div>
         <div class="contentText"><textarea name="PO_SHWeatherSys" id="PO_SHWeatherSys" class="contentTextBox">
    本市今日受南下冷空气的影响，主导风向为西北风，风力较大，存在一定的污染物输送，空气质量上午开始为PM2.5轻度污染。
    </textarea></div>
    <div class="firstTitle">二、主要预报结论</div>
    <div class="contentText">
          <table class="contentTable">
            <tr><td colspan="2" class="titleTd">日期</td><td class="titleTd">霾</td><td class="titleTd">空气污染等级</td></tr>
            <tr>
                <td class="titleTd"><input id="PO_tomDate" name="PO_tomDate" value="10月28日"/></td>
                <td class="titleTd"><input id="PO_tomWeekDay" name="PO_tomWeekDay" value="周三" /></td>
                <td><input id="PO_tomHaze" name="PO_tomHaze" value="上午以前轻微霾" /></td>
                <td><input id="PO_tomAirPol" name="PO_tomAirPol" value="上午PM2.5轻度-中度污染下午PM2.5良-轻度污染" />
                </td>
            </tr>
            <tr>
                <td class="titleTd"><input id="PO_afterDate" name="PO_afterDate" value="10月28日"/></td>
                <td class="titleTd"><input id="PO_afterWeekday" name="PO_afterWeekday" value="周三" /></td>
                <td><input id="PO_afterHaze" name="PO_afterHaze" value="上午以前轻微霾" /></td>
                <td><input id="PO_afterAirPol" name="PO_afterAirPol" value="上午PM2.5轻度-中度污染下午PM2.5良-轻度污染" />
                </td>
            </tr>
            <tr>
                <td class="titleTd"><input id="PO_afterDate2" name="PO_afterDate2" value="10月28日"/></td>
                <td class="titleTd"><input id="PO_afterWeekday2" name="PO_afterWeekday2" value="周三" /></td>
                <td><input id="PO_afterHaze2" name="PO_afterHaze2" value="上午以前轻微霾" /></td>
                <td><input id="PO_afterAirPol2" name="PO_afterAirPol2" value="上午PM2.5轻度-中度污染下午PM2.5良-轻度污染" />
                </td>
            </tr>

          </table>
          </div>
          <div class="firstTitle">三、预报关注点：</div>
          <div class="contentText"><textarea name="PO_reportFocus" id="PO_reportFocus" class="contentTextBox_Rich">
      
    27日夜间-28日上午污染过程：27日夜间-28日上午本市处于高压中心附近，大气扩散条件较差，且27日白天颗粒物浓度较高，预计28日上午空气污染等级将达到PM2.5轻度-中度污染，28日上午前有轻微霾；28日中午随着大气扩散条件转好，霾及污染天气过程结束。
    不确定性分析：由于污染积累存在不确定性，因此27日夜间-28日上午的颗粒物浓度和霾的程度均存在一定的不确定性。
          </textarea></div>
          <div class="bottomText">
          <div class="contentText_Bottom">关注重点：</div>
          <textarea class="contentTextBox_Bottom" id="PO_mainFocus" name="PO_mainFocus">27日夜间-28日上午上海地区PM2.5污染及霾天气过程。</textarea>
          </div>
          <div style="clear:both"></div>
          <div class="bottomEditor"><div class="editor"><span>编辑：</span><input id="PO_editor" name="PO_editor" class="editorText" type="text" value="甄新蓉"/></div><div class="reporter"><span>领班预报员：</span><input id="PO_reporter" name="PO_reporter" class="reporterText" type="text" value="毛卓成"/></div><div style="clear:both"></div></div>
          
         

       </div>
       <div class="secondPage">
       <div class="imgTitle">附图：华东地区三天霾等级预报(20时) </div>
          <div class="imgDocArea">
             <div class="imgDiv"><div class="selImg"></div><img id="PO_tomHazeImg" runat="server" class="scrollImg Img_024" src="../css/images/noImg.GIF" alt="" /></div>
             <div class="imgDiv"><div class="selImg"></div><img id="PO_aferHazeImg" runat="server" class="scrollImg Img_048" src="../css/images/noImg.GIF" alt="" /></div>           
             <div class="imgDiv"><div class="selImg"></div><img id="PO_afterHazeImg2" runat="server" class="scrollImg Img_072" src="../css/images/noImg.GIF" alt="" /></div>                           
          </div>
       </div>
    </div>
<div class="btnArea">
       <div class="btns">
        <div id="foreSave" class="button_Bottom" >保存</div>      
          <div id="forePreview" class="button_Bottom" style="display:block;"><a id="previewLink" href="<%=PageOfficeLink.OpenWindow("http://localhost:10310/WebUI/AQI/PageOfficePreview/PolWeatherAnalysisPreview.aspx?filePath=PolWeatherAnalysis.doc&ProductName=PolWeatherAnalysis","width=1200px;height=700px;")%>" >预览</a></div>         
           <div id="forePub" class="button_Bottom" style="display:block">发布</div>
       </div>
   </div>
   <div style="display:none;">
   <div id="upLoadFileArea">
   <form runat="server">      
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
    <input name="txtHideHostName" type="hidden" id="wordContent" value=""/>
    <input name="txtHideHostName" type="hidden" id="txtHideHostName" value="http://localhost:21765/WebUI/"/>
     <!--FTP地址集合 -->
    <input type="hidden" id="FtpCollection" value="ShPolWeaAnaReport,上海市污染天气过程跟踪解析专报YYYY年第N期.doc" />    
</body>
</html>
