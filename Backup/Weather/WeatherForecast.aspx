<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeatherForecast.aspx.cs" Inherits="WeatherForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>天气形势</title>
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
        <div style="margin-right: auto;	margin-left: auto; width: 900px;">
         
            <div style="height: 36px;	width: 582px;	font-size: 24px;	line-height: 36px;	font-weight: bold;	text-align: center;	font-family: 微软雅黑;">天气形势预报</div>
         
    
    <div style="width: 582px; height: 70px; border-right: green 1px solid; border-top: green 1px solid; border-left: green 1px solid; color: black; border-bottom: green 1px solid; font-family: 微软雅黑; font-size:13px;  margin-top: 10px; line-height: 20px; padding-left: 5px;">
        <br />
     &nbsp;&nbsp;&nbsp;&nbsp; 模式每天起报一次(北京20时)，预报时效为78小时，天气形势场和大气成分水平格距为6km，沙尘为24km。产品时间间隔1小时，图形中的时间（Init为起报时间，Valid为预报时间）均为UTC。
        <br />
    </div>
        <br />
        <br />
    <h1></h1> 
   
<TABLE class="tablekuang"  cellSpacing="0" cellPadding="0" width="582px" border="0"><TBODY><TR><TD class="tabletitle3" style="width:25%">
    类别</TD><TD class="tabletitle2" style="width:25%">
    层次</TD><TD class="tabletitle3" style="width:50%">
    说明</TD></TR><TR><TD class="tablerow3">
    2m气温与10m风</TD><TD class="tablerow3">
            <span><a href="#" onclick="donghua('ed76d585a11d8370e371f77ee5d70c65',717,1095)"
                style="color: blue"><span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('ed76d585a11d8370e371f77ee5d70c65',717,1095)" style="color: blue"><span
                        style="text-decoration: underline"> </span></span></A></TD><TD 
class="tablerow3">
        2m高度温度和10m高度风
    </TD></TR><TR><TD class="tablerow3">
    相对湿度</TD><TD class="tablerow3">
            <span><a href="#" onclick="donghua('c1f9c13f253a8a57309d6e6f319cf226',717,1095)"
                style="color: blue"><span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('ed76d585a11d8370e371f77ee5d70c65',717,1095)" style="color: blue"><span
                        style="text-decoration: underline"> </span></span></a></TD><TD 
class="tablerow3">
        2m高度相对湿度（%）
    </TD></TR><TR><TD class="tablerow3">
    露点温度与海平面气压 </TD><TD class="tablerow3">
            <span><a href="#" onclick="donghua('aabdde8ee643b5b0c9e31016fdddf3ec',717,1095)"
                style="color: blue"><span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('ed76d585a11d8370e371f77ee5d70c65',717,1095)" style="color: blue"><span
                        style="text-decoration: underline"> </span></span></A></TD><TD 
class="tablerow3">
        2m高度露点温度和海平面气压（hPa）
    </TD></TR><TR><TD class="tablerow3">
    小时降水</TD><TD class="tablerow3">
            <span><a href="#" onclick="donghua('967111975574d32b8c7c1646d3be542f',717,1095)"
                style="color: blue"><span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('ed76d585a11d8370e371f77ee5d70c65',717,1095)" style="color: blue"><span
                        style="text-decoration: underline"> </span></span></A></TD><TD 
class="tablerow3">
        前一小时至当前小时的降水量（mm/h）</TD></TR><TR><TD class="tablerow3">
    温度、位势高度与风</TD><TD class="tablerow3">
            <span><a href="#" onclick="donghua('f7a67fa63005a24be4dec21ad932a582',717,1095)"
                style="color: blue"><span style="text-decoration: underline">500hPa</span></a><a href="#" onclick="" style="color: blue"><span style="text-decoration: underline"> </span>
                </a> <a href="#" onclick="donghua('55179eacb0f96a1f1f3330b8f7b169a7',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">850hPa</span></a><a href="#" onclick="" style="color: blue"><span style="text-decoration: underline"> </span>
            </span></A></TD><TD 
class="tablerow3">
        不同标准层上的温度、位势高度与风
    </TD></TR></TBODY></TABLE>
    </div>


    </form>
</body>
</html>
