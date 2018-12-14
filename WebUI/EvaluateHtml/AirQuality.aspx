<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirQuality.aspx.cs" Inherits="EvaluateHtml_AirQuality" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>空气质量</title>
     <script language="javascript" type="text/javascript">
         var lastTab = "<%=m_FirstTab %>"; //当前选中的污染物
     </script>
    
    <link href="css/airQuality.css" rel="stylesheet" type="text/css" />
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="JS/AirQuality.js"></script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>
<body id="Body1" onclick="bodyClick(event)" runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;">
    <form id="form1" runat="server">
<asp:HiddenField id="Element" runat="server" />
    <div class="content">
      <div style="float:right;">
        浓度单位：微克/立方米（CO毫克/立方米）
      </div>
      <div class="querytool">
        <div id="tool_text">时间：</div>
         <div class="selectDate" style="width:150px"><input id="H00" runat="server" type="text" class="selectDateFormStyle" style="width:150px"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日 HH时'})"/></div>
        <div id="tool_text2">到</div>
         <div  class="selectDate" style="width:150px"><input id="H01" runat="server" type="text" class="selectDateFormStyle" style="width:150px" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日 HH时'})"/></div>
         <div id="tool_btn_area">
         <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="querySiteData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="exportSiteData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
         </button>
         </div>
        
       </div>
       <div id="Add_popup" runat="server"/>
       <div id="tabbtn">
         <ul id="tabItem" runat="server">
            <li id="addTab" style="vertical-align:bottom;text-align:left;"><img src="css/image/Add_btn_g.png" style="cursor:pointer;" onmouseover="this.src='css/image/Add_btn_h.png'" onmouseout="this.src='css/image/Add_btn_g.png'" onclick="addTabClick(this);"/></li>
         </ul>
       </div>
       <div id="content" style="width: 886px">
       </div>
    </div>
      <asp:Button ID="btnExport" runat="server" onclick="Button1_Click1" 
          Text="Button" CssClass="inVisibility"/>
</form>
</body>
</html>

