<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQICompare.aspx.cs" Inherits="AQI_AQICompare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>污染物曲线</title>

<script language="javascript" type="text/javascript">
//        <%if (m_UnLogin) {%>
//            alert('您登录已过期或者还未登录，请先登录！');
//            top.location.href = "../Default.aspx";
//        <%} %>
        
        var lastTab = "<%=m_FirstTab %>";//当前选中的污染物
        var partID="<%=m_PartID %>";
    </script>
    
    <link href="images/css/forecastFilter.css" rel="stylesheet" type="text/css" />
    <link href="images/css/css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    
    
    <script language="javascript" type="text/javascript" src="js/AQICompare.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs -->
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>

</head>
<body runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;">
    <form id="form1" runat="server">
        <asp:HiddenField id="Element" runat="server" />
    <div id="tabbtn">
       <ul id="tabItem" runat="server">
       </ul>
    </div>
    <div id="contentNone">
        <div id="tool" style="width: 886px;">
            <div class="checkStyle">
                 <div class="checkLable">开始时间：</div>
                 <input id="H00" type="text" class="selectDateFormStyle" value="<%= m_FromDate%>"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日',maxDate:'#F{$dp.$D(\'H01\')}'<%=m_MaxFrom%>})"/>
            </div>
            <div class="checkStyle">
                 <div class="checkLable">结束时间：</div>
                 <input id="H01" type="text" class="selectDateFormStyle" value="<%= m_ToDate%>" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日',minDate:'#F{$dp.$D(\'H00\')}'})"/>
            </div>
             <div id="zonghe_duibi">
                 <div class="checkLable">预报时效：</div>
                 <div id="rd1" class="radioChecked"><a href="javascript:radioClick('rd1');">24h</a></div>
                 <div id="rd2" class="radioUnChecked"><a href="javascript:radioClick('rd2');">48h</a></div>
            </div>
            <div id="tool_btn_area">
             <%-- <input type="button"  id="btnQuery" class="normal-btn input-btnQuery" value="查询" onclick="doCompareChart()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>
              <input type="button" id="ExportData" class="normal-btn input-btnQuery" value="导出" onclick="exportSiteData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>--%>
         <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="doCompareChart()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="exportSiteData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
         </button>
            </div>
        </div>
            <div id="type_select1row" style="width: 886px;">
                <div class="checkLable" style="margin-left: 12px">预报参数：</div>
                    <div  class="shortdan_highlight" id="AQI"><a href="javascript:tabClick('AQI')">AQI</a></div>
                    <div class="shortdan" id="item1"><a href="javascript:tabClick('item1')">PM2.5</a></div>
                    <div class="shortdan" id="item2"><a href="javascript:tabClick('item2')">PM10</a></div>
                    <div class="shortdan" id="item3"><a href="javascript:tabClick('item3')">NO2</a></div>
                    <div class="shortdan" id="item4"><a href="javascript:tabClick('item4')">03-1h</a></div>
                    <div class="shortdan" id="item5"><a href="javascript:tabClick('item5')">03-8h</a></div>
                </div>
        <div id="container" style="width: 850px; height:450px; margin: 0 auto;"></div>
    </div>
        <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility"/>
</form>
</body>
</html>
