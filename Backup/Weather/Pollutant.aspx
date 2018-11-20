<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pollutant.aspx.cs" Inherits="Weather_Pollutant" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <title>污染物浓度</title>
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
         
            <div style="height: 36px;	width: 582px;	font-size: 24px;	line-height: 36px;	font-weight: bold;	text-align: center;	font-family: 微软雅黑;">污染物浓度预报</div>
         
    
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
                臭氧</td>
            <td class="tablerow3">
                <a href="#" onclick="donghua('e39c1708f0ea54d806412ca9e96a1b0b',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
              </td>
            <td class="tablerow3">
                近地面臭氧浓度（ppb）</td>
          </tr>
          <tr>
            <td class="tablerow3">
                一氧化碳
            </td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('076812b697b518307d9126429d954812',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3">
                近地面一氧化碳浓度（ppb）
            </td>
          </tr>
          <tr>
            <td class="tablerow3">
                一氧化氮</td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('d99cc24e99ac6edccffda2b6c0893a59',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3" >
                近地面一氧化氮浓度（ppb）
            </td>
          </tr>          
          <tr>
            <td class="tablerow3">
                二氧化氮
            </td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('02d70ce61966d636742df3366cbb1c7b',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3" >
                近地面二氧化氮浓度（ppb）
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                氮氧化物</td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('bd29be7a168ad7eefc47b4968119e391',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3" >
                近地面氮氧化物（NO+NO2）浓度（ppb）
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                二氧化硫
            </td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('472b0de0746ed2a427034ca0b8c4eb87',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3" >
                近地面二氧化硫浓度（ppb）
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                PM2.5&nbsp;
            </td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('f9435c3db9268a45c96c957afe2f8cd9',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3" >
                近地面PM2.5浓度（ug/m3）
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                PM10
            </td>
            <td class="tablerow3" >
                <a href="#" onclick="donghua('6b952f0e2807eef5693e59c67b206944',717,1095)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
            </td>
            <td class="tablerow3" >
                近地面PM10浓度（ug/m3）
            </td>
          </tr>
        </table>
    </div>

    </form>
</body>
</html>
