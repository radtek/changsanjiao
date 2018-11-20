<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScoreScreening.aspx.cs" Inherits="ReportProduce_ScoreScreening" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
     <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/ScoreScreening.js" type="text/javascript"></script>
    <link href="../css/ProductScreening.css" rel="stylesheet" type="text/css" />
    <link href="../css/ScoreScreening.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="totalContent">
      <div class="outLine" id="outLine">
         <div class="shArea">
           <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>预报评分结果监控</span>
           </div>           
           <div class="displayItem" id="AQIArea">
           <div class="singleTitle">国家局主观预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem"  id="AQIPeriod">
            <div class="singleTitle">气象局分时段预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>

           <div class="displayItem" id="HazeForecast">
           <div class="singleTitle">霾预报评分</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition" id="Haze24_05">待完成</div>
                 <div class="pubTime">05时预报</div>
               </div>
               <div class="singleConDis">
                 <div class="condition" id="Haze24_17">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
           <div class="displayItem"  id="UVForecast">
           <div class="singleTitle">紫外线预报评分</div>
             <div class="innerContent">             
             <div class="singleConDis">
                 <div class="condition" id="UVForecast_17">待完成</div>
                 <div class="pubTime">16时预报</div>
               </div>             
             </div>
           </div>
          
         </div>         
      </div>
    </div>
    </form>
</body>
</html>
