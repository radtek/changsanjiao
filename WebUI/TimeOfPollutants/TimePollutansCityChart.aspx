<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimePollutansCityChart.aspx.cs" Inherits="TimePollutansCityChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>污染物随时间变化</title>
    <link href="css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="TimePollutansCityChart.js?v=20160819"></script>
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
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="clickQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
            <%--<input type="button" id="btnQuery"  onclick="doQueryChart()" class="query defaultQueryButton"  onmouseover="this.className = 'query overQueryButton';" onmouseout ="this.className ='query defaultQueryButton';"   />--%>
         </div>
        </div>
        <div class="checkStyle" style=" width:1056px; margin-top:10px; ">
                     <div class="checkLable" style="width:75px">重点城市：</div>
                      <div id="c1" class="radioChecked"><a href="javascript:radioClickModule('c1','上海市');">上海市</a></div>
                      <div id="c2" class="radioUnChecked"><a href="javascript:radioClickModule('c2','济南市');">济南市</a></div>
                      <div id="c3" class="radioUnChecked"><a href="javascript:radioClickModule('c3','青岛市');">青岛市</a></div>
                      <div id="c4" class="radioUnChecked"><a href="javascript:radioClickModule('c4','合肥市');">合肥市</a></div>
                      <div id="c5" class="radioUnChecked"><a href="javascript:radioClickModule('c5','南京市');">南京市</a></div>
                      <div id="c6" class="radioUnChecked"><a href="javascript:radioClickModule('c6','苏州市');">苏州市</a></div>
                      <div id="c7" class="radioUnChecked"><a href="javascript:radioClickModule('c7','杭州市');">杭州市</a></div>
                      <div id="c8" class="radioUnChecked"><a href="javascript:radioClickModule('c8','宁波市');">宁波市</a></div>
                      <div id="c9" class="radioUnChecked"><a href="javascript:radioClickModule('c9','南昌市');">南昌市</a></div>
                      <div id="c10" class="radioUnChecked"><a href="javascript:radioClickModule('c10','福州市');">福州市</a></div>
                      <div id="c11" class="radioUnChecked"><a href="javascript:radioClickModule('c11','厦门市');">厦门市</a></div>
             
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
