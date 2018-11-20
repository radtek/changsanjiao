<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HazeDropZone.aspx.cs" Inherits="ReportProduce_HazeDropZone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../AQI/js/HazeDropZone.js" type="text/javascript"></script>
    <script src="../AQI/js/AirPollutionDropZone.js" type="text/javascript"></script>
    <link href="../css/HazeAndAirPolDropZone.css?v=20160510" rel="stylesheet" type="text/css" />
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>

    <div class="totalContent">
    <div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table><tr><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td></tr></table>
      </div>
      
   </div>
    
    <div class="imgContent">
    <div class="all_Left" id="all_Left">    
        <div class="two">
        <div class="mapTitle">
           <div class="titlePoint"></div>
           <span class="titleText">霾</span>
           </div>
           <img src="../css/images/noImg.GIF" id="hazeDropImg" class="displayImg" alt="" />
        </div>
        <div class="btns">
            <div class="button" id="autoGetHaze">获取图片</div>
            <div class="button" id="btnHazeSave">保存</div>
            <div class="button" id="btnHazeCheck" style="display:none;">审核</div>
            <div class="button" id="btnHazePublish" style="display:none;">发布</div>
        </div>
    </div>
    <div class="all_Right" id="all_Right">    
    <div class="two">
        <div class="mapTitle">
           <div class="titlePoint"></div>
           <span class="titleText">空气污染气象条件</span>
         </div>
         <img src="../css/images/noImg.GIF" id="airPolImg" class="displayImg" alt=""/></div>
    <div class="btns">
       <div class="button" id="autoGetAirPol">获取图片</div>
       <div class="button" id="btnAirPolSave">保存</div>
         <div class="button" id="btnAirCheck" style="display:none;">审核</div>
        <div class="button" id="btnAirPolPub" style="display:none;">发布</div>
    </div>
    </div>
    </div>
    </div>
    <input type="hidden" id="FtpHazeCollection" value="DropZone1,YYYYMMDDHH_haze_mmdd.GIF;port21,YYYYMMDDHH_haze_mmdd.GIF" />
    <input type="hidden" id="FtpAirPolCollection" value="DropZone1,YYYYMMDDHH_diffusion_mmdd.GIF;port21,YYYYMMDDHH_diffusion_mmdd.GIF" />
</body>
</html>
