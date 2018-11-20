<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComForecastResee.aspx.cs" Inherits="AQI_ComForecastResee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>预报回顾</title>
 <script language="javascript" type="text/javascript">
        var lastTab = "<%=m_FirstTab %>";//当前选中的污染物
        var userJson = "<%=m_UserJson %>";
        var peopleJson="<%=m_PeopleJson %>";
        var userLimit="<%=Limits %>";
        var userName="<%=LoginName %>";
        
 </script>
 
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/ComForecastReSee.js"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>
<body onclick="hide(event)" style="-webkit-overflow-scrolling:touch; overflow: auto;">
        <div id="title">
            <div id="title1h">上海市空气质量预报</div>
            <div id="titleDate"><%=m_ForecastDate %></div>
            <input id="nowDateTime" type="hidden" />
        </div>
    <div id="comForecast" class="show content">
       <div id="tabbtn" style="width: 1030px">
           <ul id="tabItem" runat="server">
           </ul>
         <div class="table-select">
         <button type="button" class="normal-btn table-select-btn"  id="yesDay"  onclick="changeDateSelect(-1)" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-back"></span>
            <span class="select-text">上一天</span>
         </button>
         <button type="button" class="normal-btn table-select-btn"  id="toDay"  onclick="today()" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-now"></span>
            <span class="select-text">今天</span>
         </button>
         <button type="button" class="normal-btn table-select-btn" id="tomoDay"  onclick="changeDateSelect(1)" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-text">下一天</span>
            <span class="select-next"></span>
         </button>
      </div>
        </div>  
        <div id="content" style="width: 1046px">
          <div id="tool" style="width: 1038px">
                <div class="selectDate"><input id="H00" runat="server" type="text" class="selectDateFormStyle" onchange="changeDate(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
                <div class="checkStyle">
                     <div class="checkLable">综合预报：</div>
                     <div id="rd1" class="radioChecked"><a href="javascript:radioClick('rd1');">24h</a></div>
                     <div id="rd2" class="radioUnChecked"><a href="javascript:radioClick('rd2');">48h</a></div>
                </div>
                <div class="checkStyle">
                     <div class="checkLable">数值模式：</div>
                     <div id="rd3" class="radioUnChecked"><a href="javascript:radioClick('rd3');">CMAQ</a></div>
                     <div id="rd4"class="radioChecked"><a href="javascript:radioClick('rd4');">WRF</a></div>
                </div>
                  <div id="filter" class="lay" style="display:none">
                  </div>
                <div class="user">
                     <ul >
                         <li>主班：<input id="H02" type="text" class="onlyInput"  readonly="readonly"/></li>
                         <li>副班：<input id="H01" type="text" class ="onlyInput" readonly="readonly"/></li>
                     </ul>
                </div>
                <div class="user">
                     <ul>
                         <li>气象主班：<input id="H04" type="text" class="onlyInput"  readonly="readonly" /></li>
                         <li>气象副班：<input id="H03" type="text" class="onlyInput" readonly="readonly"/></li>
                     </ul>
                </div>
           </div>
           <div class="tableS">
          <table id="forecastTable" width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang"  runat="server" >
        </table>
           </div>
        </div>
        <div  class="bottomDiv">
           <div class="tableS">
               <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang">
          <tr>
            <td class="tabletitle2">预报时段</td>
            <td class="tabletitle3">24小时预报</td>
            <td class="tabletitle3">48小时预报</td>
          </tr>
          <tr>
            <td class="tablerow3">气象条件分析</td>
            <td class="tablerow3">
              <textarea id="H05" cols="45" rows="3" class="textarea"  readonly="readonly"></textarea></td>
            <td class="tablerow3">
             <textarea id="H06" cols="45" rows="3" class="textarea" readonly="readonly" ></textarea></td>
          </tr>
          <tr>
            <td class="tablerow3">气象条件回顾</td>
            <td class="tablerow3">
              <textarea id="H13" cols="45" rows="3" class="textarea"></textarea></td>
            <td class="tablerow3">
             <textarea id="H14" cols="45" rows="3" class="textarea"></textarea></td>
          </tr>
          <tr>
            <td class="tablerow3">污染过程分析</td>
            <td class="tablerow3" ><textarea id="H07" cols="45" rows="3" class="textarea" readonly="readonly"></textarea></td>
            <td class="tablerow3"><textarea id="H08" cols="45" rows="3" class="textarea" readonly="readonly"></textarea></td>
          </tr>
          <tr>
            <td class="tablerow3">污染过程回顾</td>
            <td class="tablerow3" ><textarea id="H15" cols="45" rows="3" class="textarea"></textarea></td>
            <td class="tablerow3"><textarea id="H16" cols="45" rows="3" class="textarea"></textarea></td>
          </tr>
        </table>
          </div>
        </div>
        <div class="btnarea">
              <input type="button"  id="btnSave" class="normal-btn input-btn" value="保存" onclick="SaveTextArea()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'"  onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'">
              <input type="button" id="btnSubmit" class="normal-btn input-btn" value="清空" onclick="clearTextArea()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'">
<%--             <input type="button" id="btnSave"  class="save defaultButton" value="保存"  onclick="SaveTextArea();" onmouseover="this.className = 'save overButton';" onmouseout ="this.className ='save defaultButton';" />  
            <div id="btnSubmit" class="save defaultButton"><a href="javascript:clearTextArea();">清空</a></div>--%>
        </div>
    </div>
    </body>
</html>
