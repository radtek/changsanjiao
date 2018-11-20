<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weekTable.aspx.cs" Inherits="AQI_weekTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>一周展望</title>
 <script language="javascript" type="text/javascript"></script>
 
    <link href="images/css/weekTable.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    
    
    <script language="javascript" type="text/javascript" src="js/weekTable.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>
<body>
    <div id="title">
        <div id="title1h">预报会商</div>
        <div id="titleDate"><%=m_ForecastDate %></div>
        <input id="nowDateTime" type="hidden" />
    </div>
<div class="content">
      <div class="querytool">
        <div id="tool_text">时间：</div>
         <div class="selectDate" style="width:150px"><input id="H00" runat="server" type="text" class="selectDateFormStyle" style="width:150px"  onchange="changeDate(this)"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
         <div id="tool_btn_area">
              <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="queryDayComment()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
                <span class="select-Query"></span>
                <span class="select-text">查询</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="exportSiteData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
         </button>
         </div>
        
       </div>
       <div id="content" style="width: 986px; margin-top: 10px;">
     
       </div>
    </div>
</body>
</html>
