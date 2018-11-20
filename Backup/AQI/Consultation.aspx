<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Consultation.aspx.cs" Inherits="AQI_Consultation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title></title>
 <script language="javascript" type="text/javascript">
        var userJson = "<%=m_UserJson %>";
        var userLimit="<%=Limits %>";
        var userName="<%=LoginName %>";
 </script>
  <link href="images/css/forecastFilter.css" rel="stylesheet" type="text/css" />
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/Consultation.js"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>
    <body onclick="hide(event)" style="-webkit-overflow-scrolling:touch; overflow: auto;">
    <div id="HuiS">
        <div id="titleHui">
            <div id="title1h">上海市空气质量预报会商</div>
            <div id="titleDate"><%=m_ForecastDate %></div>
            <input id="nowDateTime" type="hidden" />
        </div>
<%--        <div  class="dataTimeHS">
             <div class="dateShowHS" id="yesDay"><a href="javascript:changeDateSelect(-1);">上一天</a></div>
             <div class="dateShowHS" id="toDay"><a href="javascript:today();">今天</a></div>
             <div class="dateShowHS" id="tomoDay"><a href="javascript:changeDateSelect(1);">下一天</a></div>
        </div>--%>
     <div class="dataTimeHS" >
         <button type="button" class="normal-btn table-select-btn"  id="yesDay"  onclick="changeDateSelect(-1)" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-back"></span>
            <span class="select-text">上一天</span>
         </button>
         <button type="button" class="normal-btn table-select-btn"  id="toDay"  onclick="today()" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">

            <span class="select-now"></span>
            <span class="select-text">今天</span>
         </button>
         <button type="button" class="normal-btn table-select-btn" id="tomoDay"  onclick="changeDateSelect(1)" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-text">下一天</span>
            <span class="select-next"></span>
         </button>
      </div>
   </div>
     <div id="comForecast">
       <div id="content">
          <div id="tool" >
              <div class="userT" >
                  <ul>
                     <li>起报日期</li>
                  </ul>
              </div>
                <div class="selectDate">
                     <input id="H00" runat="server" type="text" class="selectDateFormStyle" onchange="changeDate(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
               </div>

                <div class="userT" style="width: 95px; padding-left: 20px;"> 市环境监测中心</div>
                <div class="user" >
                     <ul >
                         <li>主班：<input id="H02" type="text" class="onlyInput"/></li>
                         <li>副班：<input id="H01" type="text" class ="onlyInput"/></li>
                     </ul>
                </div>
                <div class="userT" style="padding-left: 60px"> 气象局</div>
                <div class="user">
                      <div id="filter" class="lay" style="display:none"></div>
                     <ul>
                         <li>主班：<input id="H04" type="text" class="onlyInput" onclick = "Select(this,202)" onkeyup="Select(this,202)"/></li>
                         <li>副班：<input id="H03" type="text" class="onlyInput" onclick = "Select(this,201)" onkeyup="Select(this,201)"/></li>
                     </ul>
                </div>
          </div>
           <div id="table">
           <table id="pTable" width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang" runat="server">
              <tr>
                <td class="tabletitle">预报时效</td>
                <td class="tabletitleDate">日期</td>
                <td class="tabletitle">时段</td>
                <td class="tabletitle">PM<sub>2.5</sub></td>
                <td class="tabletitle">PM<sub>10</sub></td>
                <td class="tabletitle">NO<sub>2</sub></td>
                <td class="tabletitle">O3<sub>-1h</sub></td>
                <td class="tabletitle">O3<sub>-8h</sub></td>
                <td class="tabletitle">AQI</td>
                </tr>
            </table>
           </div>
           
           
            <div  class="bottomDiv" style="width: 1030px">
           <div class="tableS" style="width: 1030px">
                <table width="100%" border="0"  cellpadding="0" cellspacing="0" class="tablekuang">
              <tr>
                <td class="tabletitle2">项目</td>
                <td class="tabletitle3">24小时</td>
                <td class="tabletitle3">48小时</td>
              </tr>
              <tr>
                <td class="tablerow3">气象条件分析</td>
                <td class="tablerow3" >
                  <textarea id="H05" cols="45" rows="3" class="textarea" style="width: 100%"></textarea></td>
                <td class="tablerow3">
                 <textarea id="H06" cols="45" rows="3" class="textarea"  style="width:  100%"></textarea></td>
              </tr>
              <tr>
                <td class="tablerow3">预报预览</td>
                <td class="tablerow3"><textarea id="H09" cols="45" rows="3" class="textarea" 
                        readonly="readonly" style="width: 100%"></textarea></td>
                <td class="tablerow3"><textarea id="H10" cols="45" rows="3" class="textarea" 
                        readonly="readonly" style="width: 100%"></textarea></td>
              </tr>
            </table>
          </div>
        </div>
        <div  class="bottomDiv" style="width: 1030px">
           <div class="tableS" style="width: 1030px">
               <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang">
                    <tr>
                        <td class="tablerow6">短信<br />
                          发布<br />
                          内容</td>
                        <td class="tablerow7" style="padding-top: 0px">
                            <textarea id="PH09" cols="100" rows="2"  class="textarea" readonly="readonly"  style="height: 60px; width: 100%;"></textarea>
                        </td>
                        
                        </tr>
              </table>
          </div>
        </div>
        <div class="btnarea">
          <input type="button"  id="sendSM" class="normal-btn input-btn" value="提交" onclick="doSubmit();Broadcast(['H05','H06','H03','H04']);" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'">
              <input type="button" id="goBack" class="normal-btn input-btn" value="清空" onclick="clear()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'">
        </div>
     </div>
  </div>
     </body>
</html>
