<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="AQI_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>华东一周污染回顾</title>
  <style type="text/css">
*{ margin:0; padding:0;}
ul, li{list-style:none}
img{ border:0}
html{
	font-family: "微软雅黑", "STHeiti", "Microsoft YaHei";
	font-size: 14px;
	line-height: 22px;
	color: #444444;
	background-color: #fff;
	height: 100%;
}
a{color:#444; text-decoration:none;cursor:pointer;}
a:hover{color:#444; text-decoration:none;cursor:pointer;}
.title, .tab, .chartarea{
	width: 1042px;
	margin-right: auto;
	margin-left: auto;
	overflow: hidden;
}
.tab li{
	float: left;
	margin-right: 6px;
	width: 90px;
	text-align: center;
	height: 40px;
}
.tab li a{color:#444; text-decoration:none;cursor:pointer; display: block;}
.tab li a:hover{color:#444; text-decoration:underline;cursor:pointer; display: block;}
.foucs{
	line-height: 32px;
	background-image: url(img/tab_foucs.png);
	background-repeat: no-repeat;
	height: 40px;
	width: 90px;
	color: #fff;
	cursor:pointer;
}
.line{
	line-height: 32px;
	border: 1px solid #cccccc;
	height: 32px;
	cursor:pointer;
}
.title{
	font-size: 22px;
	line-height: 36px;
	text-align: center;
	margin-top: 16px;
}
.chartarea li{
	float: left;
	height: 300px;
	width: 334px;
	overflow: hidden;
	margin-bottom: 20px;
}
.mt10{margin-top: 10px;}
.mt20{margin-top: 20px;}
.mr20{margin-right: 20px;}
.date{
	background-image: url(img/date_bg.png);
	background-repeat: no-repeat;
	height: 28px;
	width: 138px;
	float: left;
	margin-top: 4px;
}
.formstyle{
	border: 0;
	background-color:transparent;
	height: 28px;
	width: 110px;
	text-indent: 5px;
	cursor:pointer;
}
</style>
   <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/AQI.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
  <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>

<body>
<div class="title">华东一周污染回顾</div>
<div class="tab mt20">
     <ul>
         <li><p class="foucs" id="H07"  style=" cursor:pointer;" onclick="today()">今天</p></li>
         <li><p class="line" onclick="imageChange(this)" id="H00" runat="server"></p></li>
         <li><p class="line" onclick="imageChange(this)" id="H01" runat="server"></p></li>
         <li><p class="line" onclick="imageChange(this)" id="H02" runat="server"></p></li>
         <li><p class="line" onclick="imageChange(this)" id="H03" runat="server"></p></li>
         <li><p class="line" onclick="imageChange(this)" id="H04" runat="server"></p></li>
         <li><p class="line" onclick="imageChange(this)" id="H05" runat="server"></p></li>
     </ul>
     <div class="date"><input name="" type="text" id="H06" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" onchange="changeDate(this)" class="formstyle" runat="server"/></div>
</div>
<div class="chartarea mt10" id="img">
<ul>
     <li class="mr20"><img id="M0" /></li>
     <li class="mr20"><img id="M1"/></li>
     <li><img id="M2"/></li>
     <li class="mr20"><img id="M3"/></li>
     <li class="mr20"><img id="M4"/></li>
     <li><img id="M5" /></li>
     </ul>
</div>
</body>
</html>
