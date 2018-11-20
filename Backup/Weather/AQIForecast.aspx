<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQIForecast.aspx.cs" Inherits="Weather_AQIForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
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
        <div style="margin-right: auto;	margin-left: auto; width: 900px; margin-top: 10px;">
         
            <div style="height: 36px;	width: 582px;	font-size: 24px;	line-height: 36px;	font-weight: bold;	text-align: center;	font-family: 微软雅黑;">
                AQI指数预报</div>
         
    
    <div style="width: 582px; height: 90px; border-right: green 1px solid; border-top: green 1px solid; border-left: green 1px solid; color: black; border-bottom: green 1px solid; font-family: 微软雅黑; font-size:12px; line-height: 20px; padding-left: 5px;">
        <br />
     &nbsp;&nbsp;&nbsp;&nbsp; 空气质量预报产品来源于华东区域化学天气数值预报系统（WRF-Chem），水平分辨率为6km，目前包括未来3天的日平均PM10、SO2、NO2日平均和8小时O3浓度。生活指数产品基于SMB-WARMS和WRF-CMAQ数值预报系统开发，气象要素场来自SMB-WARMS系统，大气成分要素由WRF-CMAQ系统提供。
        <br />
    </div>
        <br />
        <br />
    <h1></h1> 
   
     <table width="582px" border="0" cellpadding="0" cellspacing="0" class="tablekuang" >
          <tr>
            <td class="tabletitle2" style="width:25%">
                类别</td>
            <td class="tabletitle3" style="width:15%">
                层次</td>
            <td class="tabletitle3" style="width:60%">
                说明</td>
          </tr>
          <tr>
            <td class="tablerow3">
                AQI</td>
            <td class="tablerow3">
                <a href="#" onclick="donghua('d6977c49992b18830784ae9a867a3ba9',741,900)" style="color: blue">
                    <span style="text-decoration: underline">地面</span></a>
                        </td>
            <td class="tablerow3">
                空气质量指数</td>
          </tr>
          <tr>
            <td class="tablerow3" style="height: 26px">
                PM2.5日平均
            </td>
            <td class="tablerow3" style="height: 26px" >
                    <span><a href="#" onclick="donghua('a03af21aa6d0e0923083f9aa46ca6255',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('076812b697b518307d9126429d954812',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" style="height: 26px">
                日平均（北京时当日0时-23时）近地面PM2.5分指数&nbsp;
            </td>
          </tr>
          <tr>
            <td class="tablerow3">
                PM10日平均
            </td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('650f8ef01712ea0c94e11f39ac837de1',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('d99cc24e99ac6edccffda2b6c0893a59',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" >
                日平均（北京时当日0时-23时）近地面PM10分指数</td>
          </tr>          
          <tr>
            <td class="tablerow3">
                NO2日平均
            </td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('4507618d3c33f24e4fafb8a2e3ed2e71',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('02d70ce61966d636742df3366cbb1c7b',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" >
                日平均（北京时当日0时-23时）近地面PM2.5分指数&nbsp;
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                SO2日平均</td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('06fa234ab56d374959f559fa0357851c',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('bd29be7a168ad7eefc47b4968119e391',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" >
                日平均（北京时当日0时-23时）近地面二氧化硫分指数</td>
          </tr>
                    <tr>
            <td class="tablerow3">
                8小时最大O3
            </td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('49b4171f6238ffe383bd4e859e03c556',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('472b0de0746ed2a427034ca0b8c4eb87',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" >
                近地面日最大8小时滑动平均臭氧分指数
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                1小时最大O3
            </td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('86235588f736132b012803ac0d66191e',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('f9435c3db9268a45c96c957afe2f8cd9',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" >
                近地面日小时最大臭氧分指数
            </td>
          </tr>
                    <tr>
            <td class="tablerow3">
                日最大霾等级</td>
            <td class="tablerow3" >
                    <span><a href="#" onclick="donghua('56129ed19eede40414c7dac43cf4765b',741,900)" style="color: blue">
                        <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('6b952f0e2807eef5693e59c67b206944',717,1095)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                </td>
            <td class="tablerow3" >
                日最大霾霾等级（按霾预报等级计算），持续4小时及以上，持续时段内无降水且RH&lt;95
            </td>
          </tr>
           <tr>
            <td class="tablerow3">
                日平均CO
            </td>
            <td class="tablerow3" >
                    <span>
                        <span><a href="#" onclick="donghua('d37b2df1059668494a9eb440b3621a1b',741,900)" style="color: blue">
                            <span style="text-decoration: underline">地面</span></a><a href="#" onclick="donghua('56129ed19eede40414c7dac43cf4765b',741,900)" style="color: blue"><span style="text-decoration: underline"> </span></span>
                        </span></td>
            <td class="tablerow3" >
                近地面日平均一氧化碳分指数
            </td>
          </tr>
        </table>
    </div>

    </form>
</body>
</html>
