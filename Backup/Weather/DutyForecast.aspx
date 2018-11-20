<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DutyForecast.aspx.cs" Inherits="Weather_DutyForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>沙尘</title>
<link href="../AQI/images/css/css.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
function donghua(jflag,width,height){
	var path= "http://222.66.83.20:801//index.php?controller=listpic&action=carton&d=RDpcd3d3L3B1YmxpYy9jYWNoZS8%3D"+"&j="+jflag+"&psize="+width+","+height;
	height +=48;
	window.open (path,'',"width="+width+"px,height="+height+"px,resizable=no,scrollbars=no,status=no");
}

</script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-right: auto;	margin-left: auto; width: 900px; padding-left: 5px;">
         
            <div style="height: 36px;	width: 582px;	font-size: 24px;	line-height: 36px;	font-weight: bold;	text-align: center;	font-family: 微软雅黑;">
                沙尘预报</div>
         
    
    <div style="width: 582px; height: 70px; border-right: green 1px solid; border-top: green 1px solid; border-left: green 1px solid; color: black; border-bottom: green 1px solid; font-family: 微软雅黑; font-size:12px;  margin-top: 10px; line-height: 20px; padding-left: 5px;">
        <br />
     &nbsp;&nbsp;&nbsp;&nbsp; 模式每天起报一次(北京20时)，预报时效为78小时，天气形势场和大气成分水平格距为6km，沙尘为24km。产品时间间隔1小时，图形中的时间（Init为起报时间，Valid为预报时间）均为UTC。
        <br />
    </div>
        <br />
        <br />
    <h1></h1> 
   
     <table width="582px" border="0" cellpadding="0" cellspacing="0" class="tablekuang" >
          <tr>
            <td class="tabletitle2" style="width:25%">
                类别</td>
            <td class="tabletitle3" style="width:25%">
                层次</td>
            <td class="tabletitle3" style="width:50%">
                说明</td>
          </tr>
          <tr>
            <td class="tablerow3">
                沙尘PM10
            </td>
            <td class="tablerow3">
                    <span><a href="#" onclick="donghua('dc3152a739aa32ac46047f310a19482e',717,1095)"
                        style="color: blue"><span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('e39c1708f0ea54d806412ca9e96a1b0b',717,1095)" style="color: blue"><span
                                style="text-decoration: underline"> </span></span></A></td>
            <td class="tablerow3">
                近地面沙尘PM10浓度（ug/m3）
            </td>
          </tr>
          <tr>
            <td class="tablerow3">
                整层沙尘PM10</td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('aac9acf78a88b314cee85a1e507d59e9',717,1095)"
                        style="color: blue"><span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('076812b697b518307d9126429d954812',717,1095)" style="color: blue"><span
                                style="text-decoration: underline"> </span></span></A></td>
            <td class="tablerow3">
                整层（垂直积分）沙尘PM10浓度（mg/m2）
            </td>
          </tr>
          
        </table>
    </div>

    </form>
</body>
</html>
