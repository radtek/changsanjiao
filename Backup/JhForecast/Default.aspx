<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="JhForecast_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <style type="text/css" >
        .MenuStyle{height:100%;font-size: 12px;}
        .paramItem
        {
            height: 25px;
            margin: 5px;
            width: 320px;
            border-bottom: solid 1px #ccc;
        }

    </style>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
 </head>
<body>
    <table class = "MenuStyle">
    <tr>
    <td>
            <div class="paramItem">
        日期： <input id="startTime"  style="width:110px" type="text" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
               <img onclick="WdatePicker({el:'startTime',dateFmt:'yyyy-MM-dd'})" src="../DatePicker/skin/datePicker.gif" width="16" height="22" align="absmiddle"/>
        </div>
        <div class="paramItem" id="panModes">
            模式：
            <input id="radNAQPMS2008"  name="mode" checked="checked" type="radio" /><label
                for="radNAQPMS2008">NAQPMS</label>
            <input id="radCMAQ4" name="mode" type="radio" /><label for="radCMAQ4">CMAQ</label>
            <input id="radCAMx"  name="mode" type="radio" /><label for="radCAMx">CAMx</label>
            <input id="radWRF" name="mode" type="radio" /><label for="radWRF">WRF-Chem</label>
        </div>
        
        <div class="paramItem">
            区域：
            <input id="radD1" onclick="loadPage(this.id)" name="area" checked="checked" type="radio" /><label
                for="radD1">全国</label>
            <input id="radD2" onclick="loadPage(this.id)" name="area" type="radio" /><label for="radD2">华东</label>
            <input id="radD3" onclick="loadPage(this.id)" name="area" type="radio" /><label for="radD3">长三角</label>
            <input id="radD4" onclick="loadPage(this.id)" name="area" type="radio" /><label for="radD4">上海</label>
        </div>

    </td>

    <td style="width:100%;">
        <iframe id="iframePage" src="NAQPMS_PM25/D1/pm25.htm" height="630px"  width="100%" frameborder="0">
        </iframe>

    </td>
    </tr>
    </table>

  
</body>
</html>
 <script language="javascript" type="text/javascript">
    startTime.value = "<%= m_DateStart %>";
    function loadPage(inputID){
            iframePage.location.href = "NAQPMS_PM25/" + inputID.substring(3,5) +"/pm25.htm";;
            
    }
</script> 