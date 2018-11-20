<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UVTable.aspx.cs" Inherits="ReportProduce_UVTable" %>

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
           <span>紫外线表</span>
           </div>
       <div class="contenttitle2">
           <table  style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;" border="0" cellspacing="1">
  <tbody><tr>
    <td height="24" align="center" bgcolor="#FFFFFF"  class="tabletitleDateOut"><strong>级别</strong></td>
    <td height="24" align="center" bgcolor="#FFFFFF"  class="tabletitleDateOut"><strong>辐射量 w/m2</strong></td>
    <td height="24" align="center" bgcolor="#FFFFFF"  class="tabletitleDateOut"><strong>紫外线指数</strong></td>
    <td height="24" align="center" bgcolor="#FFFFFF"  class="tabletitleDateOut"><strong>紫外线辐射强度</strong></td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>对人体可能影响皮肤晒红时间（分钟）</strong></td>
    <td height="24" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>须采取的防护措施</strong></td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">一级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">&lt;5</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">0,1,2</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">最弱</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">100-180</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">不需要采取防护措施</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">二级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">5-10</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">3,4</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">弱</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">60-100</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">可以适当采取一些防护措施，如：涂擦防护霜等</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">三级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">10-15</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">5,6</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">中等</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">30-60</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">外出时戴好遮阳帽、太阳镜和太阳伞等，涂擦SPF指数大于15的防护霜</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">四级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">15-30</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">7,8,9</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">强</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">20-24</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">除上述防护措施外，上午十点至下午四点时段避免外出，或尽可能在遮荫处</td>
  </tr>
  <tr>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">五级</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">&gt;=30</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">&gt;=10</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">很强</td>
    <td height="24" align="center" bgcolor="#FFFFFF" class="tablerowOut">&lt;20</td>
    <td height="24" bgcolor="#FFFFFF" class="tablerowOut">尽可能不在室外活动，必须外出时，要采取各种有效的防护措施</td>
  </tr>
</tbody></table>
       </div>
    </div>
       </div>
</body>
</html>
