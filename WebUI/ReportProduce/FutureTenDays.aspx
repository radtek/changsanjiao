<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FutureTenDays.aspx.cs" Inherits="ReportProduce_FutureTenDays" %>
<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228"
    Namespace="PageOffice" TagPrefix="po" %>

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
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../AQI/js/FutureTenDays.js" type="text/javascript"></script>
    <link href="../css/FutureTenDays.css" rel="stylesheet" type="text/css" />
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
//        function InsideMessageAdd() {
//            //window.open()得到子窗体
//            tip = window.openModalDialog("<%=PageOfficeLink.OpenWindow("../AQI/PageOfficePreview/FutureTenDaysPreview.aspx?filePath=FutureTenDays.doc&ProductName=FutureTenDays","width=1200px;height=700px;")%>", "", "dialogWidth=400px;dialogHeight=300px;help=no;status:no")
//            //启动timer，判断子窗体是否关闭
//            timer = window.setInterval("IfWindowClosed()", 500);
//        }
//        var timer;
//        var tip;
//        function IfWindowClosed() {
//            //判断子窗体是否关闭
//            if (tip.closed == true) {
//                alert("close");
//                window.clearInterval(timer);
//            }
//        }
        
    </script>
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
           <div class="wordBtn" id="clickQuery">一键查询</div>
           <div id="readMOdel" class="wordBtn">读取模板</div>
           <div id="readForeData" class="autoGet">自动获取</div>
          </div>
          </td>
         </tr>
      </table>                      
  </div>
  </div>
  <div class="docContent">
        <div class="wordContent">    
         <div class="docTitle">未来10天本市空气污染潜势预测</div>
         <div class="issue" id="PO_docIssue"><input type="text" id="PO_year" value="2015" /><span>年第</span><input type="text" id="PO_issueNum" value="40" /><span>期</span></div>
         <div class="organization">上海市气象局</div>
         <div class="titleAndOra">
         <div id="docFullDate" class="docDate"><input id="PO_docDate" class="docDate" value="2015年10月27日17时" /></div>
         <div class="ora"><span >签发人：</span><input type="text" id="PO_Signer" value="陈振林"/></div><div style="clear:both"></div>
         </div>
         
         <div class="firstTitle">一、未来10天本市空气污染潜势预测</div>
         <div class="contentText"><textarea  name="PO_TenDaysAirFore" id="PO_TenDaysAirFore" class="contentTextBox">
    26日-27日中午前，本市天气静稳，扩散条件较差，预计将出现轻度-中度污染。26日夜里可能出现短时重度污染，需加强关注。
    </textarea></div>    
    <div  class="tableTitle">未来10天本市空气污染潜势预测结论如下：</div>
    <div class="contentText">
          <table class="contentTable firstTable">
            <tr class="titleBottomLine"><td></td><td>空气污染潜势</td><td>天气现象</td><td>风向风力</td></tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date1">10月30日</span></div></td>
                <td><input id="PO_PoLevel1" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather1" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed1" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date2">10月30日</span></div></td>
                <td><input id="PO_PoLevel2" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather2" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed2"  value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date3">10月30日</span></div></td>
                <td><input id="PO_PoLevel3" value="" /></td>
                <td><input id="PO_Weather3" value="" /></td>
                <td><input id="PO_WindSpeed3" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date4">10月30日</span></div></td>
                <td><input id="PO_PoLevel4" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather4" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed4" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date5">10月30日</span></div></td>
                <td><input id="PO_PoLevel5" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather5" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed5" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date6">10月30日</span></div></td>
                <td><input id="PO_PoLevel6" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather6" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed6" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date7">10月30日</span></div></td>
                <td><input id="PO_PoLevel7" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather7" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed7" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date8">10月30日</span></div></td>
                <td><input id="PO_PoLevel8" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather8" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed8" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date9">10月30日</span></div></td>
                <td><input id="PO_PoLevel9" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather9" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed9" name="PO_WindSpeed1" value="" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_Date10">10月30日</span></div></td>
                <td><input id="PO_PoLevel10" name="PO_PoLevel1" value="" /></td>
                <td><input id="PO_Weather10" name="PO_Weather1" value="" /></td>
                <td><input id="PO_WindSpeed10" name="PO_WindSpeed1" value="" /></td>
            </tr>            
          </table>
          </div>
          <div class="contentText">
          <div id="Textarea1" class="contentTextBox">
    污染潜势：“低”：空气质量以优为主；“中等”：空气质量以良为主；“较高”：以轻度污染为主；“高”：以中度污染为主；“很高”：可能出现重度及以上污染。
    </div>  
    </div> 
          </div>
        <div class="secondPage">
        <div class="tableTitle">未来7天主要工业区空气污染潜势预测结论如下：</div>
          <div class="contentText">
          <table class="contentTable secondTable">
            <tr class="titleBottomLine"><td></td><td>高桥、老港</td><td>金山石化</td><td>奉贤化工</td><td>宝钢、吴淞</td><td>闵行吴泾</td></tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven1">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao1" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan1" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian1" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang1" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang1" name="PO_WindSpeed" value="低" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven2">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao2" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan2" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian2" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang2" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang2" name="PO_WindSpeed" value="低" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven3">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao3" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan3" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian3" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang3" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang3" name="PO_WindSpeed" value="低" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven4">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao4" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan4" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian4" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang4" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang4" name="PO_WindSpeed" value="低" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven5">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao5" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan5" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian5" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang5" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang5" name="PO_WindSpeed" value="低" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven6">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao6" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan6" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian6" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang6" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang6" name="PO_WindSpeed" value="低" /></td>
            </tr>
            <tr>
                <td><div class="dateCell"><span id="PO_DateSeven7">10月30日</span></div></td>
                <td><input id="PO_Gaoqiao7" name="PO_PoLevel1" value="低" /></td>
                <td><input id="PO_Jinshan7" name="PO_FirstItem" value="低" /></td>
                <td><input id="PO_Fenxian7" name="PO_Weather" value="低" /></td>
                <td><input id="PO_Baogang7" name="PO_WindSpeed" value="低" /></td>
                <td><input id="PO_Minhang7" name="PO_WindSpeed" value="低" /></td>
            </tr>
          
          </table>
          </div>
          <div class="firstTitle">二、未来3天影响分析和调控建议</div>
          <div class="contentText">
          <textarea name="PO_SHWeatherSys" id="PO_FutureThreDays" class="contentTextBox">
    26日-27日中午前，本市天气静稳，扩散条件较差，出现污染天气的概率较高。此次污染过程PM2.5来源以本地积聚为主，预计贡献率可达50-60%。闵行、宝山工业区排放对中心城区的影响比较明显。
    </textarea>          
          <div class="bottomText">
          <div class="contentText_Bottom">抄报：</div>
          <textarea class="contentTextBox_Bottom" id="PO_mainFocus" name="PO_mainFocus">蒋卓庆副市长、黄融副秘书长</textarea>
          <div class="contentText_Bottom">抄送：</div>
          <textarea class="contentTextBox_Bottom" id="Textarea2" name="PO_mainFocus">上海市环保局</textarea>
          </div>
          
          </div>
          <div class="bottomEditor"><div class="editor"><span>制作：</span><input id="PO_editor" name="PO_editor" class="editorText" type="text" value="陈镭"/></div><div class="reporter"><span>审核：</span><input id="PO_reporter" name="PO_reporter" class="reporterText" type="text" value="毛卓成"/></div><div class="phone"><span>联系电话：</span><input id="phone" name="PO_editor" class="editorText" type="text" value="53896141"/></div><div style="clear:"></div></div>
          
       </div>
    </div>
        <div style="clear:both"></div>
       </div>       
    <div class="btnArea">
       <div class="btns">                   
          
          <div id="foreSave" class="button_Bottom" >保存</div> 
          <div class="button_Bottom"  style="display:none;" id="preview"><a id="previewLink"  href="<%=PageOfficeLink.OpenWindow("http://222.66.83.21:8282/PEMFCShare/AQI/PageOfficePreview/FutureTenDaysPreview.aspx?filePath=FutureTenDays.doc&ProductName=FutureTenDays","width=1200px;height=1200px;")%>" >预览</a></div>
          <div id="forePub" style="display:none;" class="button_Bottom">发布</div>            
       </div>
   </div>
   <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
    </div>
    <div id="closePreview" class="closeBtn"></div>
    <input type="hidden" id="FtpCollection" value="FutureTenDays,未来10天全市空气污染潜势预测专报YYYY年第N期.doc" />
    </form>
</body>
</html>
