<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirQualityDenAndIndexTable.aspx.cs" Inherits="ReportProduce_AirQualityDenAndIndexTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/WorkSchedule.css" rel="stylesheet" type="text/css" />
    <link href="../css/WorkSchedule_2.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="tabs_middle1_left" style="width:99%">
       <div class="contenttitle">
     <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>空气质量浓度和指数换算表</span>
           </div>
       <div class="contenttitle2">
     <table style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;" border="0" cellspacing="1" >
  <tbody><tr>
    <td rowspan="2" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">空气质量分指(IAQI)</td>
    <td colspan="10" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">污染物项目浓度限值</td>
    </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">二氧化硫(SO<sub>2</sub>)24小时平均/(µg/m<sup>3</sup>)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">二氧化硫(SO<sub>2</sub>)1小时平均/(µg/m<sup>3</sup>)<sup>(1)</sup></td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">二氧化氮(NO<sub>2</sub>)24小时平均/(µg/m<sup>3</sup>)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">二氧化硫(NO<sub>2</sub>)1小时平均/(µg/m<sup>3</sup>)<sup>(1)</sup></td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">颗粒物(粒径小于等于10um)24小时平均/(µg/m<sup>3</sup>)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">一氧化碳(CO)24小时平均/(mg/m<sup>3</sup>)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">一氧化碳(CO)1小时平均/(mg/m<sup>3</sup>)<sup>(1)</sup></td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">臭氧(O<sub>3</sub>)1小时平均/(µg/m<sup>3</sup>)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">臭氧(O<sub>3</sub>)8小时滑动平均/(µg/m<sup>3</sup>)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut">颗粒物(粒径小于等于2.5um)24小时平均/(µg/m<sup>3</sup>)</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">50</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">52</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">150</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">40</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">100</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">50</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">2</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">5</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">160</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">100</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">35</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">100</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">150</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">500</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">80</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">200</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">150</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">4</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">10</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">200</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">160</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">75</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">150</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">475</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">650</td>
    <td height="24" align="center" bgcolor="#FFFFFF"  class="tablerowOut">180</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >700</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >250</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >14</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">35</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">300</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">215</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">115</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">200</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >800</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >800</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >280</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >1200</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >350</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >24</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >60</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >400</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">265</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">150</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">300</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >1600</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >(2)</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >565</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >2340</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >420</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >36</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >90</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >800</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >800</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >250</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">400</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >2100</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >(2)</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >750</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >3090</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >500</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >48</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >120</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >1000</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >(3)</td>
    <td height="24" align="center" bgcolor="#FFFFFF"class="tablerowOut" >350</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">500</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >2620</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >(2)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >940</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >3840</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >600</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >60</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >150</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >1200</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >(3)</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut" >500</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">说明：</td>
    <td colspan="10" height="24" bgcolor="#FFFFFF"class="tablerowOut" >(1)二氧化硫(SO<sub>2</sub>)、二氧化氮(NO<sub>2</sub>)和一氧化碳(CO)的一小时平均浓度限值仅用于实时报，在日报中需使用相应污染物的24小时平均浓度限值<br>
    (2)二氧化硫(SO<sub>2</sub>)1小时平均浓度值高于800µg/m<sup>3</sup>的，不再进行其空气质量份指数计算，二氧化硫(SO<sub>2</sub>)空气质量分指数按24小时平均浓度计算的分指数预报<br>
    (3)臭氧(O<sub>3</sub>)8小时平均浓度值高于800µg/m<sup>3</sup>的，不再进行其空气质量分指数计算，臭氧(O<sub>3</sub>)空气质量分指数按1小时平均浓度计算的分指数预报
    </td>
    </tr>
    <tr>
    <td colspan="11"><img src="../AQI/img/1.png" alt="" width="500px"></td>
    </tr>
    
</tbody></table>
      </div>
     </div>
    </div>
</body>
</html>
