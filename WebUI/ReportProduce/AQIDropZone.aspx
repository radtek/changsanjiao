<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQIDropZone.aspx.cs" Inherits="ReportProduce_AQIDropZone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <link href="../css/AQIDropZone.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../AQI/js/AQIDropZone.js?v=20160429" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form2" runat="server">
      <div class="tableTop">
          <div id="topInfo" class="titleContent">
            <table><tr><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td></tr></table>
          </div>
      
       </div>
    <div class="two" id="leftArea">    
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>落区图</span>
           </div>
    <div class="tabs-panels" id="outLine">
        <div id="totalArea" class="totalArea">
        <div class="showimages" >
            <div class="topCheck"><input id="putPM25" class="topCheckBox" type="checkbox" /><span class="checkText"> PM2.5</span></div>
            <div class="imgArea"><img id="pm25" src="../css/images/noImg.GIF" class="disImg" onclick="showOne(this)"/></div>
        </div>

        <div class="showimages">
         <div class="topCheck"><input id="putPM10" class="topCheckBox" type="checkbox" /><span class="checkText"> PM10</span></div>
        <div class="imgArea"><img id="pm10" src="../css/images/noImg.GIF" class="disImg" onclick="showOne(this)"/></div></div>
 
        <div class="showimages">
         <div class="topCheck"><input id="put031" class="topCheckBox" type="checkbox" /><span class="checkText"> O3_1h</span></div>
        <div class="imgArea"><img id="o3_1h" src="../css/images/noImg.GIF" class="disImg" onclick="showOne(this)"/></div></div>

        <div class="showimages">
         <div class="topCheck"><input id="put038" class="topCheckBox" type="checkbox" /><span class="checkText"> O3_8h</span></div>
        <div class="imgArea"><img id="o3_8h" src="../css/images/noImg.GIF" class="disImg" onclick="showOne(this)"/></div></div>
  
        <div class="showimages">
         <div class="topCheck"><input id="putNO2" class="topCheckBox" type="checkbox" /><span class="checkText"> NO2</span></div>
        <div class="imgArea"><img id="no2" src="../css/images/noImg.GIF" class="disImg" onclick="showOne(this)"/></div></div>
        <div class="rightCorner" id="rightCorner">
        <div class="autoArea"><div class="button" id="autoGet">自动获取</div></div>
        <div class="allCheck"><input type="checkbox" id="selectAll" /><span class="allSelText">全选</span></div>
        <div class="bottomBtns">
        <div class="btns">       
            <div class="button_Bottom" id="btnSave">保 存</div>
            <div class="button_Bottom" id="btnCheck" style="display:none;">审 核</div> 
            <div class="button_Bottom" id="btnPublish" style="display:none;">发 布</div> 
             </div>
            </div>           
    </div>
    </div>        
    </div>       
    </div>
    <div class="guideReport" id="guideReport">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>站点指导预报</span>
           </div>
    <div class="textdownload" id="textdownload">
      <div id="downloadIcon" class="downloadIcon"></div>
      <div id="downloadText" class="downloadText">
         
      </div>
      <div class="autoGetOutLine"><div class="txtAutoGet" id="txtAutoGet">自动获取</div></div>
      <div class="downloadBottom" id="downloadTxt"><a id="downloadUrl" href="">下载站点预报文件</a></div>
    </div>
    <div class="btns_Bottom">
    <div class="innerBtns">
            <div id="btnSaveAQI" class="button_Bottom">保 存</div>
            <div id="btnAuthProduct" class="button_Bottom" style="display:none;">审 核</div>
            <div id="btnPush" class="button_Bottom" style="display:none;">发 布</div>
            </div>
    </div>
    </div>  
     <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden detailViewDiv"><img id="detailView" class="detailView" /><div id="closeBtn" class="closeBtn"></div>          
     <!--FTP地址集合 -->
    <input type="hidden" id="FtpCollection" value="DropZone1,YYYYMMDDHH_要素_mmdd.GIF;port21,YYYYMMDDHH_要素_mmdd.GIF" />
    <input type="hidden" id="SiteGuideReportFtp" value="SiteGuideReport,Z_SEVP_C_BCSH_YYYYMMddhhmmss_P_MSP3_SH-MO_ENVAQFC_AIR_L88_ENC_YYYYMMDDHHMM_00000-07200.TXT" />
    </form>
</body>
</html>
