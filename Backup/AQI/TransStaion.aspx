<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransStaion.aspx.cs" Inherits="AQI_TransStaion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>上海市交通路边站空气质量日报</title>
 <script language="javascript" type="text/javascript">
 </script>
 <link href="images/css/TransStation.css?v=2014091901" rel="stylesheet" type="text/css" />
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
  <script language="javascript" type="text/javascript" src="js/TransStation.js"></script>
</head>
<body style="-webkit-overflow-scrolling:touch; overflow: auto;">
    <form id="form1" runat="server">
   <asp:HiddenField id="Element" runat="server" />
   <div class="content" style="width: 100%">
   <div class="contentNone" style="margin-top: 10px">
      <div id="tool_duibi">
        <div id="tool_text">选择日期：</div>
         <div class="selectNewDate"><input id="H00" runat="server" type="text" class="selectDateFormStyle" onchange="queryData()"   onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
         <div id="tool_btn">
          <button type="button" class="normal-btn input-btnQuery" style="width:100px;"  id="btnQuery"   onclick="CreateDayProduct()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text" style="width:66px;">日报加工</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="ExportDayData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
         </button>
          <button type="button" class="normal-btn input-btnQuery"  id="Print"  onclick=" printit()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-print"></span>
            <span class="select-text">打印</span>
         </button>
         </div>
       </div>
     </div>
     <div class="line"></div>
     <div id="printArea">
     <!--startprint-->
     <div  class="title">上海市交通路边站空气质量日报</div>
     <div class="dateTime" id="titleDatetime">日期：<%=m_ForecastDate %></div>
       <div class="contentNone">
       <table id="forecastTable"  align="center" width="100%" border="0" cellpadding="0" cellspacing="0" class="tableku"  runat="server" >
       <tr>
       <td rowspan="3"  style="width:120px; padding-left:30px;" class="tableTitleName"  >监测站名称</td>
       <td  colspan="6" class="tableTitleName" style="padding-left:180px; ">常规污染物</td>
       <td  colspan="7" class="tableTitleName" style="padding-left:190px; " >苯系物</td>
       <td  colspan="2" class="tableTitleName" style="padding-left:35px; ">碳氢化合物</td>
       <td class="tableTitleNameRight" >黑碳</td>
       </tr>
       <tr>
       <td class="tableT"  style="font-weight: bold">NO</td>
       <td class="tableT"  style="font-weight: bold">NO<sub>2</sub></td>
       <td class="tableT"  style="font-weight: bold">NO<sub>x</sub></td>
       <td class="tableT"  style="font-weight: bold">CO</td>
       <td class="tableT"  style="font-weight: bold">PM<sub>2.5</sub></td>
       <td class="tableT"  style="font-weight: bold">PM<sub>10</sub></td>
       <td class="tableT"  style="font-weight: bold">苯</td>
       <td class="tableT"  style="font-weight: bold">甲苯</td>
       <td class="tableT" style="font-weight: bold">乙苯</td>
       <td class="tableT"  style="font-weight: bold">苯乙烯</td>
       <td class="tableT"  style="font-weight: bold">间对二甲苯</td>
       <td class="tableT"  style="font-weight: bold">邻二甲苯</td>
       <td class="tableT"  style="font-weight: bold">1,3丁二烯</td>
       <td class="tableT"  style="font-weight: bold">甲烷</td>
       <td class="tableT"  style="font-weight: bold">非甲烷总烃</td>
       <td class="tableTRight"  style="font-weight: bold">平均</td>
       </tr>
       <tr>
       <td class="tableT" >ug/m3</td>
       <td class="tableT" >ug/m3</td>
       <td class="tableT" >ug/m3</td>
       <td class="tableT" >mg/m3</td>
       <td class="tableT" >ug/m3</td>
       <td class="tableT" >ug/m3</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppb</td>
       <td class="tableT" >ppm</td>
       <td class="tableT" >ppm</td>
       <td class="tableTRight" >ng/m3</td>
       </tr>
       </table>
       <div>
       </div>
      </div>
      <!--endprint-->
      </div>
  </div>
        <asp:Button ID="ExportBut" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility"/>
</form>
</body>
</html>
