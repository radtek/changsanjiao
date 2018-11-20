<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForecastFilter.aspx.cs" Inherits="AQI_ForecastFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>预报查询</title>
 <script language="javascript" type="text/javascript">
 var lastTab = "<%=m_FirstTab %>";//当前选中的污染物
 </script>
 <link href="images/css/forecastFilter.css" rel="stylesheet" type="text/css" />
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/ForecastFilter.js"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body style="-webkit-overflow-scrolling:touch; overflow: auto;">
    <form id="form1" runat="server">
    <asp:HiddenField id="Element" runat="server" />
   <div class="content" style="width: 100%">
   <div id="contentNone" style="margin-top: 10px">
      <div id="tool_duibi">
        <div id="tool_text">起止时间：</div>
         <div class="selectNewDate"><input id="H00" runat="server" type="text" class="selectDateFormStyle"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
         <div  class="selectNewDate"><input id="H01" runat="server" type="text" class="selectDateFormStyle" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
         <div id="zonghe_duibi">
         <div id="name_duibi">综合预报：</div>
         <div id="rd1" class="radioChecked"><a href="javascript:radioClick('rd1');">24h</a></div>
         <div id="rd2" class="radioUnChecked"><a href="javascript:radioClick('rd2');">48h</a></div>
         </div>
         <div id="tool_btn">
          <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="queryData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="queryExportData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
         </button>
         <%-- <input type="button"  id="btnQuery" class="normal-btn input-btnQuery" value="查询" onclick="queryData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>
          <input type="button" id="btnExport" class="normal-btn input-btnQuery" value="导出" onclick="queryExportData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>--%>
            <%--<input type="button" id="btnQuery" class="queryButton defaultQueryButton"  onmouseover="this.className = 'queryButton overQueryButton';" onmouseout ="this.className ='queryButton defaultQueryButton';"  onclick="queryData()"  />                
            <input type="button" id="btnExport" class="queryButton defaultExportButton"  onmouseover="this.className = 'queryButton overExportButton';" onmouseout ="this.className ='queryButton defaultExportButton';"  onclick="queryExportData()"  />                --%>
         </div>
       </div>
       
       <div id="type_select1row">
       <label>预报时段：</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" checked ="checked"  id="noon" value ="2" />上午</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" checked ="checked"  id="afternoon"  value ="3" />下午</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" checked ="checked"  id="night" value ="6" />夜晚</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" id="shangNight"  value ="4" />上半夜</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" id="xiaNight"  value ="1" />下半夜</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" id="dayAve"  value ="7" />日平均</label>
       </div>
       <div id="type_select2row">
       <label>数据类型：</label>
       <label class="forecast"><input type="checkbox" name="dataType"   checked ="checked" id="shiCe"  value ="1" />实测</label>
       <label class="forecast"><input type="checkbox" name="dataModule" checked ="checked"  id="manual" value ="manual" />综合预报</label>
       <label class="forecast"><input type="checkbox" name="dataModule" id="CMAQ"  value ="CMAQ" />CMAQ </label>
       <label class="forecast"><input type="checkbox" name="dataModule" id="WRF"  value ="WRF" />WRF-CHEM</label>
       </div>
     </div>
       <div id="tabbtn" style="width: 1046px">
           <ul id="tabItem"  runat="server">
           </ul>
       </div>
       <div id="content">
           <div id="Tb0" class="hidden">
           
           </div>
            <div id="Tb1" >
            
           </div>
           <div id="Tb2" class="hidden">
           
           </div>
            <div id="Tb3" class="hidden">
            
           </div>
            <div id="Tb4" class="hidden">
            
           </div>
            <div id="Tb5" class="hidden">
            
           </div>
      </div>
  
  </div>
    <asp:Button ID="SearchBut" runat="server" onclick="Button1_Click" Text="Button"  CssClass="inVisibility"/>
</form>
</body>
</html>
