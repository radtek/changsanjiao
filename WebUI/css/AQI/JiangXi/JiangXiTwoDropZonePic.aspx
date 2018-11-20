<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JiangXiTwoDropZonePic.aspx.cs" Inherits="AQI_JiangXi_JiangXiTwoDropZonePic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../../JS/Utility.js" type="text/javascript"></script>
    <script src="../../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../../JS/Utility.js" type="text/javascript"></script>
    <script src="JS/JiangXiAirPollutionDropZone.js" type="text/javascript"></script>

    <script src="JS/JiangXiAirPollutionSingleDropZone.js" type="text/javascript"></script>
    <link href="css/JJiangXiTwoDropZone.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="totalContent">
<div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table><tbody><tr><th class="attrName">预报员：</th><td class="attrValue" id="forecaster">江西</td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue">2016-01-04 22:57:9</td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue">17时</td></tr></tbody></table>
      </div>
   </div>
    <div class="imgContent">
   <div class="all all-Left" id="all-Left">
    <div class="two">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span class="titleText">污染气象条件预报</span>
           </div>
        <div class="autoGet">
         <div class="button">自动获取</div>
       </div>
    <div id="change" class="tabs-container">
    <div class="tabs-wrap" style="margin-left:0px;left:0px;width:100%;">
    <ul class="tabs">
    <li id="PM25Li">
      <a class="tabs-inner">
        <span class="tabs-title">站点</span>
        <span class="tabs-icon"></span></a></li>
    <li id="PM10Li">
    <a class="tabs-inner">
    <span class="tabs-title">落区</span>
    <span class="tabs-icon"></span></a></li>
   </ul></div>
    <div class="tabs-panels" style="height:auto;width:auto;">
    <div class="panel" style="display:block;width:auto;">
    <div class="imgArea_Left" >
    <img id="02" src="../../css/images/noImg.GIF" class="disImg"/></div></div>
    <div class="panel" style="display:none;width:auto;">
    <div class="imgArea_Left">
    <img id="03" src="../../css/images/noImg.GIF" class="disImg"/></div></div>

    </div></div>
    </div>
    
    <div class="submit">
    <div class="button bottomBtn">保 存</div>
    <div class="button bottomBtn" style="display:none">审 核</div>
    <div class="button bottomBtn"  style="display:none">发 布</div>
    </div></div>
   <div class="all all-Right" id="all-Right">

    <div class="two">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span class="titleText">污染天气预警</span>
           </div>
       <div class="autoGet">
         <div class="button">自动获取</div>
       </div>
       <div class="imgArea">
           <img src="../../css/images/noImg.GIF" class="disImg" id="hazeDropImg"/>
       </div>
    </div>

    <div class="submit">
    <div class="button bottomBtn">保 存</div>
    <div class="button bottomBtn" style="display:none">审 核</div>
    <div class="button bottomBtn"  style="display:none">发 布</div>
    </div>

    </div>
    </div>
    </div>
</body>
</html>
