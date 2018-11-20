<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimePollutansChart.aspx.cs" Inherits="TimePollutansChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>污染物随时间变化</title>
    <link href="css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="TimePollutansChart.js?v=20160819"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs -->
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>
    <script language="javascript" type="text/javascript">
        var lastTab = "<%=m_FirstTab %>";//当前选中的污染物
        var station="<%=m_Station %>"
    </script>
</head>
<body>
    <div id="tabbtn">
       <ul id="tabItem" runat="server">
       </ul>
    </div>
    <div id="contentNone">
        <div id="tool">
            <div class="checkStyle">
                 <div class="checkLable">开始时间：</div>
                 <input id="H00" type="text" class="selectDateFormStyle" value="<%= m_FromDate%>"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日',maxDate:'#F{$dp.$D(\'H01\')}'})"/>
            </div>
            <div class="checkStyleII">
                 <div class="checkLable">结束时间：</div>
                 <input id="H01" type="text" class="selectDateFormStyle" value="<%= m_ToDate%>" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日',minDate:'#F{$dp.$D(\'H00\')}'})"/>
                  &nbsp;&nbsp;时效：48
            </div>
            <div id="tool_btn_area">
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="doQueryChart()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
            <%--<input type="button" id="btnQuery"  onclick="doQueryChart()" class="query defaultQueryButton"  onmouseover="this.className = 'query overQueryButton';" onmouseout ="this.className ='query defaultQueryButton';"   />--%>
         </div>
        </div>
          <table align="center" cellpadding="0" cellspacing="0" class="hct">
                <tr>
                    <td>
                       <div id="container1" style="width: 1050px; height: 350px; margin: 0 0 0 0;"></div></td>
                </tr>
                <tr>
                    <td>
                       <div id="container2" style="width: 1050px; height:350px; margin: 0 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container3" style="width: 1050px; height:350px; margin: 0 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container4" style="width: 1050px; height:350px; margin: 0 0 0 0 0;"></div></td>
                </tr>
            </table>

    </div>
</body>
</html>
