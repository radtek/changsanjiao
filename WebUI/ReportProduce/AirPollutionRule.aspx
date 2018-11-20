<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirPollutionRule.aspx.cs" Inherits="ReportProduce_AirPollutionRule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
                <link href="../css/WorkSchedule.css" rel="stylesheet" type="text/css" />
    <link href="../css/WorkSchedule_2.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
<script src="../AQI/js/CalclulateHeight.js" type="text/javascript"></script>
</head>
<body>
<div class="tabs_middle1_left" style="width:99%">
       <div class="contenttitle">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>空气污染气象条件预报等级</span>
           </div>
       <div class="contenttitle2">
           <table style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;" border="0" cellspacing="1">
  <tbody><tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>等级</strong></td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>评价</strong></td>
    <td height="24" align="left" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>描述</strong></td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">一级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">好</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">非常有利于空气污染物稀释、扩散和清除</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">二级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">较好</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">较有利于空气污染物稀释、扩散和清除</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">三级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">一般</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">对空气污染物稀释、扩散和清除无明显影响</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">四级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">较差</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">不利于空气污染物稀释、扩散和清除</td>
    </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">五级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">差</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">很不利于空气污染物稀释、扩散和清除</td>
  </tr>
    <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">六级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">极差</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">极不利于空气污染物稀释、扩散和清除</td>
  </tr>
</tbody></table>
       </div>
    </div>
       </div>
</body>
</html>
