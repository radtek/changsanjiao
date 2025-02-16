﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElementChart.aspx.cs" Inherits="DBCityChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>要素查询</title>
    <link href="css/ELEcss.css?v=20170416" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ElementChart.js?v=20170523"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs -->
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>
        <script type="text/javascript" src="JS/gridLine.js"></script>
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
            </div>
              <div class="checkLable" style="width:75px; margin-left:-10px; ">时效：</div>
                      <div id="x1" class="radioChecked"  style=" margin-left:-30px;"><a href="javascript:radioClickModuleII('x1','24');">24</a></div>
                      <div id="x2" class="radioUnChecked" style=" display:none"><a href="javascript:radioClickModuleII('x2','48');">48</a></div>
                      <div id="x3" class="radioUnChecked"  style=" display:none"><a href="javascript:radioClickModuleII('x3','72');">72</a></div>
              
                      <div id="sw1" class="radioChecked"  style=" margin-left:10px;"><a href="javascript:radioClickModuleIII('sw1','上午');">上午</a></div>
                      <div id="sw2" class="radioUnChecked"><a href="javascript:radioClickModuleIII('sw2','下午');">下午</a></div>
                     
            <div id="tool_btn_area">
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="clickQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
            <%--<input type="button" id="btnQuery"  onclick="doQueryChart()" class="query defaultQueryButton"  onmouseover="this.className = 'query overQueryButton';" onmouseout ="this.className ='query defaultQueryButton';"   />--%>
         </div>
        </div>
        <div class="checkStyle" style=" width:1056px; margin-top:10px; ">
                     <div class="checkLable" style="width:75px">选择要素：</div>
                      <div id="c7" class="radioChecked" ><a href="javascript:radioClickModule('c7','平均温度','50745');">平均温度</a></div>
                      <div id="c10" class="radioUnChecked"><a href="javascript:radioClickModule('c10','最高温度','54094');">最高温度</a></div>
                      <div id="c11" class="radioUnChecked"><a href="javascript:radioClickModule('c11','最低温度','54094');">最低温度</a></div>
                      <div id="c8" class="radioUnChecked"><a href="javascript:radioClickModule('c8','风速','54453');">风速</a></div>
                      <div id="c9" class="radioUnChecked"><a href="javascript:radioClickModule('c9','湿度','54094');">湿度</a></div>
                      <div id="c12" class="radioUnChecked"><a href="javascript:radioClickModule('c12','气压','54094');">气压</a></div>

                    
      </div>
                     
                        <div id="c1" class="radioUnChecked" style=" margin-left:90px;"><a href="javascript:radioClickModule('c1','PM25','54324');">PM<sub>2.5</sub></a></div>
                      <div id="c2" class="radioUnChecked"><a href="javascript:radioClickModule('c2','PM10','54497');">PM<sub>10</sub></a></div>
                      <div id="c3" class="radioUnChecked"><a href="javascript:radioClickModule('c3','O38H','54237');">0<sub>3</sub>-8h</a></div>
                      <div id="c4" class="radioUnChecked"><a href="javascript:radioClickModule('c4','NO2','50873');">NO<sub>2</sub></a></div>
                      <div id="c5" class="radioUnChecked"><a href="javascript:radioClickModule('c5','SO2','54337');">SO<sub>2</sub></a></div>
                      <div id="c6" class="radioUnChecked"><a href="javascript:radioClickModule('c6','CO','54347');">CO</a></div>
             
              <table align="center" cellpadding="0" cellspacing="0" class="hct">
                 <tr>
                    <td>
                       <div id="container1" style="width: 1025px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                <tr>
                    <td>
                       <div id="container2" style="width: 1025px; height: 350px;margin: 0 0 0 0; "></div></td>
                </tr>
                <tr>
                    <td>
                       <div id="container3" style="width: 1025px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container4" style="width: 1025px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container5" style="width: 1025px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                 <tr>
                    <td>
                       <div id="container6" style="width: 1025px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                 <tr>
                    <td>
                       <div id="container7" style="width: 1045px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container8" style="width: 1045px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container9" style="width: 1045px; height:350px; margin: 0 0 0 0;"></div></td>
                </tr>
                   <tr>
                    <td>
                       <div id="container10" style="width: 1045px; height:350px;margin: 0 0 0 0;"></div></td>
                </tr>
            </table>

    </div>
</body>
</html>
