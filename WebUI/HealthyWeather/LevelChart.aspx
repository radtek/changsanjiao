<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LevelChart.aspx.cs" Inherits="LevelChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>等级查询</title>
    <link href="css/ELEcss.css?v=2017" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="JS/LevelChart.js?v=2017071"></script>
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
        var station = "<%=m_Station %>";
        var typeName = "<%=typeName %>";
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
            <div class="checkStyleII" style=" width:500px;">
                 <div class="checkLable">结束时间：</div>
                 <input id="H01" type="text" class="selectDateFormStyle" value="<%= m_ToDate%>" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日',minDate:'#F{$dp.$D(\'H00\')}'})"/>
                
            </div>
            <div id="tool_btn_area">
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="clickQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
            <%--<input type="button" id="btnQuery"  onclick="doQueryChart()" class="query defaultQueryButton"  onmouseover="this.className = 'query overQueryButton';" onmouseout ="this.className ='query defaultQueryButton';"   />--%>
         </div>
        </div>
        <div class="checkStyle" style=" width:1056px; margin-top:10px; display:block ">
                     <div class="checkLable" style="width:75px">产品类型：</div>
                       <div id="c1" class="radioChecked"><a href="javascript:radioClickModule('c1','儿童感冒','54324');">儿童感冒</a></div>
                      <div id="c2" class="radioUnChecked"><a href="javascript:radioClickModule('c2','成人感冒','54497');">成人感冒</a></div>
                      <div id="c3" class="radioUnChecked"><a href="javascript:radioClickModule('c3','老人感冒','54237');">老人感冒</a></div>
                      <div id="c4" class="radioUnChecked"><a href="javascript:radioClickModule('c4','儿童哮喘','50873');">儿童哮喘</a></div>
                      <div id="c5" class="radioUnChecked"><a href="javascript:radioClickModule('c5','COPD','54337');">COPD</a></div>
                      <div id="c6" class="radioUnChecked"><a href="javascript:radioClickModule('c6','中暑','54347');">中暑</a></div>
                      <div id="c7" class="radioUnChecked"><a href="javascript:radioClickModule('c7','重污染','54347');">重污染</a></div>
      </div>
      <div style=" display:none">
                      <div id="c78" class="radioUnChecked" style=" margin-left:90px;"><a href="javascript:radioClickModule('c7','平均温度','50745');">平均温度</a></div>
                      <div id="c10" class="radioUnChecked"><a href="javascript:radioClickModule('c10','最大温度','54094');">最大温度</a></div>
                      <div id="c11" class="radioUnChecked"><a href="javascript:radioClickModule('c11','最小温度','54094');">最小温度</a></div>
                      <div id="c8" class="radioUnChecked"><a href="javascript:radioClickModule('c8','风力','54453');">风力</a></div>
                      <div id="c9" class="radioUnChecked"><a href="javascript:radioClickModule('c9','湿度','54094');">湿度</a></div>
                  </div>    
             
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
