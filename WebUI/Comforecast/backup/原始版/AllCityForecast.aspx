<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllCityForecast.aspx.cs" Inherits="Comforecast_AllCityForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/css.css?v=201610121" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script language="javascript" type="text/javascript" src="JS/AllCityForecast.js?v=20160920001"></script>
   <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body>
    <div style=" width:98%; height:100%; margin-left:auto; margin-right:auto;  margin-bottom:38px;">
    <div class="divTop" >
        <div class="checkStyle">
            <div class="checkLable" >起报时间</div>
            <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="DateChange(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
              <input type="button" id="history" class="button_BottomII"   value=" 获取历史"  onclick="HistoryForecast()"/>
        </div>
    </div>
    <div id="comforecast">
     <table id="comforecastTable" width="100%" border="0" cellpadding="0" cellspacing="0"  runat="server">
     <tr>
     <td class=" tabletitle2" style=" width:6%;">站点</td>
     <td class=" tabletitle2">时间</td>
     <td class=" tabletitle">AQI</td>
     <td class=" tabletitle">首要污染物</td>
     <td class=" tabletitle1" >空气质量</td>
     <td class=" tabletitle">PM<sub>2.5</sub></td>
     <td class=" tabletitle">PM<sub>10</sub></td>
     <td class=" tabletitle">CO</td>
     <td class=" tabletitle">O<sub>3</sub><sub>-8h</sub></td>
     <td class=" tabletitle">SO<sub>2</sub></td>
     <td class=" tabletitle">NO<sub>2</sub></td>
     </tr>
     </table>
    </div>

    </div>
       <div class="btnArea">
       <div class="btns">
           <input type="button" id="foreSave" class="button_Bottom"  style=" margin-right:40px;"  value=" 保存"  onclick="SaveForecast()"/>
            <input type="button" id="Button1" class="button_Bottom"   value=" 发布"  onclick="PublishForecast()"/>
       </div>
       </div>
</body>
</html>
