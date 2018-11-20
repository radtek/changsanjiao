<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBCityChart.aspx.cs" Inherits="DBCityChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>东北预报评估</title>
    <link href="css/DBcss.css?v=2016" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="JS/DBCityChart.js?v=20160812219"></script>
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
                       <div id="c5" class="radioCheckedMax"><a href="javascript:radioClickModule('c5','朝阳市','54324');">朝阳市</a></div>
                      <div id="c7" class="radioUnChecked"><a href="javascript:radioClickModule('c7','丹东市','54497');">丹东市</a></div>
                      <div id="c6" class="radioUnChecked"><a href="javascript:radioClickModule('c6','阜新市','54237');">阜新市</a></div>
                      <div id="c2" class="radioUnChecked"><a href="javascript:radioClickModule('c2','佳木斯市','50873');">佳木斯市</a></div>
                      <div id="c4" class="radioUnChecked"><a href="javascript:radioClickModule('c4','锦州市','54337');">锦州市</a></div>
                      <div id="c8" class="radioUnChecked"><a href="javascript:radioClickModule('c8','辽阳市','54347');">辽阳市</a></div>
                      <div id="c1" class="radioUnCheckedMax"><a href="javascript:radioClickModule('c1','齐齐哈尔市','50745');">齐齐哈尔市</a></div>
                      <div id="c3" class="radioUnChecked"><a href="javascript:radioClickModule('c3','葫芦岛市','54453');">葫芦岛市</a></div>
                      <div id="c9" class="radioUnChecked"><a href="javascript:radioClickModule('c9','牡丹江市','54094');">牡丹江市</a></div>
                      <div id="c10" class="radioUnChecked"><a href="javascript:radioClickModule('c10','大庆市','50850');">大庆市</a></div>
                     <div id="c11" class="radioUnChecked"><a href="javascript:radioClickModule('c11','绥化市','50853');">绥化市</a></div>
                     <div id="c12" class="radioUnChecked"><a href="javascript:radioClickModule('c12','泰安市','54827');">泰安市</a></div>
             
      </div>
              <table align="center" cellpadding="0" cellspacing="0" class="hct">
                 <tr>
                    <td>
                       <div id="container7" style="width: 1005px; height:350px; margin-left:27px;"></div></td>
                </tr>
                <tr>
                    <td>
                       <div id="container1" style="width: 1025px; height: 350px; "></div></td>
                </tr>
                <tr>
                    <td>
                       <div id="container2" style="width: 1025px; height:350px; margin-left:-5px;"></div></td>
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
                       <div id="container6" style="width: 1045px; height:350px; margin-left:-20px;"></div></td>
                </tr>
            </table>

    </div>
</body>
</html>
