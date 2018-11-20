<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductScreening_JX.aspx.cs" Inherits="ReportProduce_ProductScreening_JX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
     <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/ProductScreening.js" type="text/javascript"></script>
    <link href="../css/ProductScreening.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="totalContent">
      <div class="outLine" id="outLine">
         <div class="shArea">
           <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>江西预报预警产品</span>
           </div>           
           <div class="displayItem" id="AQIImg">
           <div class="singleTitle">AQI预报图片</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem"  id="AQIText">
            <div class="singleTitle">AQI预报文本</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem" id="AirPolSiteImg">
           <div class="singleTitle">污染气象条件预报站点</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem"  id="AirPolDropImg">
           <div class="singleTitle">污染气象条件落区图</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>

           <div class="displayItem" id="AirPolWarning">
           <div class="singleTitle">污染天气预警图</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">05时预报</div>
               </div>
               
             </div>
           </div>

           <div class="displayItem" id="HazeForecast" style="display:none">
           <div class="singleTitle">霾预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition" id="HazeForecast_05">待完成</div>
                 <div class="pubTime">05时预报</div>
               </div>
               <div class="singleConDis">
                 <div class="condition" id="HazeForecast_17">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem"  id="UVForecast" style="display:none">
           <div class="singleTitle">UV预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition" id="UVForecast_05">待完成</div>
                 <div class="pubTime">10时预报</div>
               </div>
             <div class="singleConDis">
                 <div class="condition" id="UVForecast_17">待完成</div>
                 <div class="pubTime">16时预报</div>
               </div>             
             </div>
           </div>
           <div class="displayItem"  id="AirPollutionForecast" style="display:none">
           <div class="singleTitle">空气污染气象条件</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition" id="AirPollutionForecast_07">待完成</div>
                 <div class="pubTime">08时预报</div>
               </div>
             <div class="singleConDis">
                 <div class="condition" id="AirPollutionForecast_17">待完成</div>
                 <div class="pubTime">20时预报</div>
               </div>
             </div>
           </div>
         </div>
         <div class="eastChinaArea" id="AirPolFore" style="display:none;">

           <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>华东区域预报预警产品</span>
           </div>
           <div class="displayItem" id="guideReportFile">
            <div class="singleTitle">指导预报文件</div>
             <div class="innerContent">
               <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">10:30预报</div>
               </div>

             </div>
           </div>
           <div class="displayItem" id="AQIDropZone">
            <div class="singleTitle">AQI落区预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">14时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem" id="HazeDropZone">
            <div class="singleTitle">霾落区预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">14时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem" id="AirPollutionDropZone">
            <div class="singleTitle">空气污染气象条件落区预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">14时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem" id="eastChinaMainCity">
            <div class="singleTitle">华东区域重点城市预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">14时预报</div>
               </div>
             </div>
           </div>
         </div>
      </div>

    </div>
    </form>
</body>
</html>
