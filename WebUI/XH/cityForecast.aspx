<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cityForecast.aspx.cs" Inherits="Comforecast_AllCityForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/css.css?v=20161011211111" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="JS/cityForecast.js?v=20170311011111111"></script>
   
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

 <link rel="stylesheet" type="text/css" href="themes/default/easyui.css" />
	<link rel="stylesheet" type="text/css" href="themes/icon.css" />
	<link rel="stylesheet" type="text/css" href="css/demo.css" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
	<script type="text/javascript" src="js/jquery.easyui.min.js"></script>
</head>
<body>
    <div style=" width:98%; height:100%; margin-left:auto; margin-right:auto;  margin-bottom:38px;">
    <div class="divTop" >
        <div class="checkStyle">
            <div class="checkLable" >起报时间</div>
            <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="DateChange(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
                                   <input type="button" style="display:block;" id="history" class="button_BottomII"   value=" 调取历史"  onclick="HistoryForecast()"/>
                        <input type="button" style="display:block; margin-left:300px;     margin-top: -25px;" id="Button2" class="button_BottomII"   value=" 导出word"  onclick="exportWord()"/>
   <input type="button" style="display:block; margin-left:395px;     margin-top: -25px;" id="Button3" class="button_BottomII"   value=" 上传FTP"  onclick="uploadWord()"/>
        </div>
    </div>
    <div id="comforecast">
     <table id="comforecastTable" width="100%" border="0" cellpadding="0" cellspacing="0"  runat="server">
     <tr>
     <td class=" tabletitle2" style=" width:10%;">站点</td>
     <td class=" tabletitle2" style=" width:20%;">时间</td>
     <td class=" tabletitle" style=" width:20%;">AQI</td>
     <td class=" tabletitle" >首要污染物</td>
     <td class=" tabletitle1" style=" width:25%;">空气质量</td>
     </tr>
     </table>
    </div>

    </div>
       <div class="btnArea">
       <div class="btns">
           <input type="button" id="foreSave" class="button_Bottom"  style=" margin-right:40px;"  value=" 保存"  onclick="SaveForecast()"/>
            <input type="button" id="Button1" class="button_Bottom" style="display:none;"   value=" 发布"  onclick="PublishForecast()"/>
       </div>
       </div>
       <form runat="server" id="Word_Form">
        <asp:HiddenField ID="WordInfo" runat="server" />
        <asp:Button runat="server" ID="DownloadWord" OnClick="DownloadWord_Click" style="display:none;" />
    </form>   
</body>
</html>
