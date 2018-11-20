<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JiangXiAQIArea.aspx.cs" Inherits="AQI_JiangXi_JiangXiAQIArea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../../JS/Utility.js" type="text/javascript"></script>
    <script src="../../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../../JS/Utility.js" type="text/javascript"></script>
    <link href="css/JiangXiAQIArea.css" rel="stylesheet" type="text/css" />
    <script src="JS/JiangXiAQIArea.js" type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server" method="post">

   <div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table><tr><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td></tr></table>
      </div>
   </div>
    <div class="content"> 
    <div class="all all-Left" id="all-Left">    
    <div class="two">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span class="titleText">预报图片</span>
           </div>
        <div class="button button_Right" >自动获取</div>    
        <img src="../../css/images/noImg.GIF" id="hazeDropImg"/>
    </div>
    <div class="btns">
        <div class="button button_Bottom">保存</div>
        <div class="button button_Bottom" style="display:none" >审核</div>
        <div class="button button_Bottom" style="display:none" >发布</div>
    </div>

    </div>
    <div class="all all-Right" id="all-Right">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span class="titleText">预报文本</span>
           </div>
 
   <div class="editBtns2">   
         <div id="btnTxtSave" class="button button_Right" >暂存</div>              
      <div id="Button1" class="button button_Right" >自动生成</div>
   </div>                       
   <div style="clear:both;"></div>
   <div id="aqiText" class="textContent">
      <textarea id="dataFileContent">        
      </textarea>
   </div>

    <div class="btns">
        <div id="forePreview"  class="button button_Bottom">预览</div>
        <div id="foreSave"   class="button button_Bottom">保存</div>
    </div>

   </div>
</div>
<input name="txtHideModelUrlDown" type="hidden" id="txtHideModelUrl32Down" value="AQI\WordModel\AQI-SHQXJ_2015102214_024.doc"/>
<input name="txtHideModelUrl" type="hidden" id="txtHideModelUrl32" value="AQI\WordModel\scuem_AQI-SHQXJ_201510221402.doc"/>
<input name="txtHideDocNamePrefixDown" type="hidden"  value="AQI_SHQXJ_"/>
<input name="txtHideDocNameSufix32" type="hidden"  value="1402"/>
<input name="txtHideDocNameSufixDown" type="hidden"  value="14_024"/>
<input name="txtHideDocNamePrefix32" type="hidden"  value="scuem_AQI-SHQXJ_"/>
<input name="txtHideWordProductPath" type="hidden"  value="AQI\WordProduct\"/>
<input name="txtHideWordTempProductPath" type="hidden"  value="AQI\WordProduct\"/>
</form>
</body>
</html>
