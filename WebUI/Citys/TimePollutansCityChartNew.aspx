<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimePollutansCityChartNew.aspx.cs" Inherits="TimePollutansCityChartNew" %>
<%@ Register Src="~/Citys/WebUserControl.ascx"TagName="WebUserControl"TagPrefix="uc1" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>主要城市</title>
  
    <link href="css/css.css?v=20161228" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
      <link rel="stylesheet" type="text/css" href="css/jquery.citypicker.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="js/TimePollutansCityChartNew.js?v=20161228112"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs --> 
  
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="js/histrock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>  
    <script type="text/javascript" src="JS/gridLine.js"></script>
<%-- <script src="js/Popt.js"></script>
        <script src="js/cityJson.js"></script>
        <script src="js/citySet.js"></script>--%> 


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
                 <div class="checkLable">时间范围：</div>
                 <input id="H00" type="text" class="selectDateFormStyle" value="<%= m_FromDate%>"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
            </div>
            <div class="checkStyleII">
                 <div class="checkLable">至</div>
                 <input id="H01" type="text" class="selectDateFormStyle" value="<%= m_ToDate%>" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日',minDate:'#F{$dp.$D(\'H00\')}'})"/>
                 &nbsp;&nbsp
              
                     <input type="text" id="city" style=" width:120px; height:20px; display:none;" value="上海-上海市"/>

                 &nbsp;&nbsp 分辨率：
                 <input id="hour" type="radio" class="radioBtn" name="resolution" checked="checked" onclick="ChangleType(this)" style="vertical-align: -1px;"/>
<label for="hour">小时</label>
<input id="day" class="radioBtn" type="radio" name="resolution" onclick="ChangleType(this)" style="vertical-align: -1px;"/>
<label for="day">日       |   时效：48</label>
            </div>
            <div id="tool_btn_area">
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="clickQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
            <%--<input type="button" id="btnQuery"  onclick="doQueryChart()" class="query defaultQueryButton"  onmouseover="this.className = 'query overQueryButton';" onmouseout ="this.className ='query defaultQueryButton';"   />--%>
         </div>
        </div>
        <div class="checkStyle" style=" width:1056px; margin-top:10px;  display:none ">
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
                       <div id="container1" style="width: 1000px; height: 350px; margin:0 0 0 0;"></div>
                       <div style="  border: solid 1px rgba(173, 160, 160,.5); border-radius: 5px; width:760px; height:28px; margin-left:140px; padding-top:10px;">
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',0,this)"  style=" width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#00FF7F; font-size:15px;vertical-align:middle; margin-left:4px;">实况</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',1,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#8A2BE2; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',2,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#E3170D; font-size:15px;vertical-align:middle;margin-left:4px;">WRF-CHEM</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',3,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#191970; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',4,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF8C00; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ10天</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',5,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF44AA; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE9km</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container1',6,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:black; font-size:15px;vertical-align:middle;margin-left:4px;">多模式最优集成</span>
                       </div>
                       &nbsp;
                       </td>
                </tr>
                <tr>
                    <td>
                    <div style="height:230px;"><table id="tblpm25" class="calTbl" border="0" cellpadding="0" cellspacing="0"></table></div>
                    </td>
                </tr>
                <tr>
                    <td>
                       <div id="container2" style="width: 1000px; height:350px; margin: 0 0 0 0 0;"></div>
                       <div style="  border: solid 1px rgba(173, 160, 160,.5); border-radius: 5px; width:760px; height:28px; margin-left:140px; padding-top:10px;">
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container2',0,this)"  style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#00FF7F; font-size:15px;vertical-align:middle; margin-left:4px;">实况</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container2',1,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#8A2BE2; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container2',2,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#E3170D; font-size:15px;vertical-align:middle;margin-left:4px;">WRF-CHEM</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container2',3,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#191970; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container2',4,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF8C00; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ10天</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container2',5,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF44AA; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE9km</span>
                                                <input type="checkbox" checked="checked"  onclick="toogleChart('container2',6,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:black; font-size:15px;vertical-align:middle;margin-left:4px;">多模式最优集成</span>
                       </div>
                       &nbsp;
                       </td>
                </tr>
                   <tr>
                    <td>
                       <div id="container3" style="width: 1000px; height:350px; margin: 0 0 0 0 0;"></div>
                       <div style="  border: solid 1px rgba(173, 160, 160,.5); border-radius: 5px; width:760px; height:28px; margin-left:140px; padding-top:10px;">
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container3',0,this)"  style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#00FF7F; font-size:15px;vertical-align:middle; margin-left:4px;">实况</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container3',1,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#8A2BE2; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container3',2,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#E3170D; font-size:15px;vertical-align:middle;margin-left:4px;">WRF-CHEM</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container3',3,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#191970; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container3',4,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF8C00; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ10天</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container3',5,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF44AA; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE9km</span>
                                                <input type="checkbox" checked="checked"  onclick="toogleChart('container3',6,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:black; font-size:15px;vertical-align:middle;margin-left:4px;">多模式最优集成</span>
                       </div>
                       &nbsp;
                       </td>
                </tr>
                  <tr>
                    <td>
                    <div style="height:230px;"><table id="tblo3" class="calTbl" border="0" cellpadding="0" cellspacing="0"></table></div>
                    </td>
                </tr>
                   <tr>
                    <td>
                       <div id="container4" style="width: 1000px; height:350px; margin: 0 0 0 0 0;"></div>
                       <div style="  border: solid 1px rgba(173, 160, 160,.5); border-radius: 5px; width:760px; height:28px; margin-left:140px; padding-top:10px;">
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container4',0,this)"  style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#00FF7F; font-size:15px;vertical-align:middle; margin-left:4px;">实况</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container4',1,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#8A2BE2; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container4',2,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#E3170D; font-size:15px;vertical-align:middle;margin-left:4px;">WRF-CHEM</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container4',3,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#191970; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container4',4,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF8C00; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ10天</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container4',5,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF44AA; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE9km</span>
                                                <input type="checkbox" checked="checked"  onclick="toogleChart('container4',6,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:black; font-size:15px;vertical-align:middle;margin-left:4px;">多模式最优集成</span>
                       </div>
                       &nbsp;
                       </td>
                </tr>
                      <tr>
                    <td>
                       <div id="container5" style="width: 1000px; height:350px; margin: 0 0 0 0 0;"></div>
                       <div style="  border: solid 1px rgba(173, 160, 160,.5); border-radius: 5px; width:760px; height:28px; margin-left:140px; padding-top:10px;">
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container5',0,this)"  style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#00FF7F; font-size:15px;vertical-align:middle; margin-left:4px;">实况</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container5',1,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#8A2BE2; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container5',2,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#E3170D; font-size:15px;vertical-align:middle;margin-left:4px;">WRF-CHEM</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container5',3,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#191970; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container5',4,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF8C00; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ10天</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container5',5,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF44AA; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE9km</span>
                                                <input type="checkbox" checked="checked"  onclick="toogleChart('container5',6,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:black; font-size:15px;vertical-align:middle;margin-left:4px;">多模式最优集成</span>
                       </div>
                       &nbsp;
                       </td>
                </tr>
                      <tr>
                    <td>
                       <div id="container6" style="width: 1000px; height:350px; margin: 0 0 0 0 0;"></div>
                       <div style="  border: solid 1px rgba(173, 160, 160,.5); border-radius: 5px; width:760px; height:28px; margin-left:140px; padding-top:10px;">
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container6',0,this)"  style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#00FF7F; font-size:15px;vertical-align:middle; margin-left:4px;">实况</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container6',1,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#8A2BE2; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container6',2,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#E3170D; font-size:15px;vertical-align:middle;margin-left:4px;">WRF-CHEM</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container6',3,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#191970; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container6',4,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF8C00; font-size:15px;vertical-align:middle;margin-left:4px;">CMAQ10天</span>
                        <input type="checkbox" checked="checked"  onclick="toogleChart('container6',5,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:#FF44AA; font-size:15px;vertical-align:middle;margin-left:4px;">CUACE9km</span>
                                                <input type="checkbox" checked="checked"  onclick="toogleChart('container6',6,this)" style="  width:15px; height:15px;  margin-left:15px;vertical-align:middle;" /><span style="color:black; font-size:15px;vertical-align:middle;margin-left:4px;">多模式最优集成</span>
                       </div>
                       &nbsp;
                       </td>
                </tr>
            </table>
         
    </div>
       <div style=" height:200px; position: relative;float: right;color: black; width: 220px; margin-right: 20px; margin-top: -2480px;  z-index: 9999;">
            <uc1:WebUserControl ID="WebUserControl1" runat="server"  />  
       </div>
</body>
</html>
