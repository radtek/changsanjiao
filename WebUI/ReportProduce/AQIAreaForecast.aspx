<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQIAreaForecast.aspx.cs" Inherits="ReportProduce_AQIAreaForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/AQIArea.css" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIArea.js" type="text/javascript"></script>
    <script src="../AQI/js/main.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
       
</head>
<body>
<form id="form1" runat="server" method="post">
<div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table><tr><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td></tr></table>
      </div>
   </div>
<div class="outLine">
<div class="totalContent">
    <div id="map" class="map">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>上海地区AQI分区预报</span>
           </div>
        <div class="districtArea">        
           <div id="XuHui"></div>
           <div id="PuDong"></div>
           <div id="MinHang" ></div>
           <div id="BaoShanArea" ></div>
           <div id="SongJiang" ></div>
           <div id="JinShan" ></div>
           <div id="QingPu" ></div>
           <div id="FengXian" ></div>
           <div id="JiaDing" ></div>
           <div id="ChongMing" ></div>
        </div>
    </div>
    <div class="content">
       <div class="tablePart">
           <div class="editLabel">
       <div class="titlePoint"></div>
       <span class="partTitle">AQI分区指导预报</span>
       </div>                                    
       <div id="aqiAreaTable" class="tableContent">
          <table class="aqiAreaTable">
              <tr>
                  <th scope="col">预报区域</th>
                  <th scope="col">污染等级</th>
                  <th scope="col">首要污染物</th>
                  <th scope="col">AQI指数</th>
                  <th scope="col">霾预报</th>
              </tr>
              <tr>
              <td class="c_RegionName">中心城区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" type="text" name="aqiLevel" id="58367_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="58367_FirstPol">
                  <div id="selectID" class="dateDiv">
                      <div class="firstPolText" id="58367_Item">PM2.5</div>
                      <div id="selIcon" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58367_AQI" class="aqiValue"/></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div27">
                  <div id="Div28" class="hazeDiv">
                      <div class="hazeLevelText" id="58367_Haze">无霾</div>
                      <div id="Div29" class="selIcon"></div>
                  </div>
                  <ul id="Ul10" class="hazeLevelUl hide">
                      <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">浦东新区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58370_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="58370_FirstPol">
                  <div id="Div2" class="dateDiv">
                      <div class="firstPolText"  id="58370_Item">PM2.5</div>
                      <div id="Div3" class="selIcon"></div>
                  </div>
                  <ul id="Ul1" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58370_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div30">
                  <div id="Div31" class="hazeDiv">
                      <div class="hazeLevelText"  id="58370_Haze">无霾</div>
                      <div id="Div32" class="selIcon"></div>
                  </div>
                  <ul id="Ul11" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">闵行区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58361_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div1">
                  <div id="Div4" class="dateDiv">
                      <div class="firstPolText"  id="58361_Item">PM2.5</div>
                      <div id="Div5" class="selIcon"></div>
                  </div>
                  <ul id="Ul2" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd"> 
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58361_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div33">
                  <div id="Div34" class="hazeDiv">
                      <div class="hazeLevelText"  id="58361_Haze">无霾</div>
                      <div id="Div35" class="selIcon"></div>
                  </div>
                  <ul id="Ul12" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">宝山区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58362_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div6">
                  <div id="Div7" class="dateDiv">
                      <div class="firstPolText"  id="58362_Item">PM2.5</div>
                      <div id="Div8" class="selIcon"></div>
                  </div>
                  <ul id="Ul3" class="firstPolUl hide">
                     <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58362_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div36">
                  <div id="Div37" class="hazeDiv">
                      <div class="hazeLevelText"  id="58362_Haze">无霾</div>
                      <div id="Div38" class="selIcon"></div>
                  </div>
                  <ul id="Ul13" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">松江区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58462_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div9">
                  <div id="Div10" class="dateDiv">
                      <div class="firstPolText"  id="58462_Item">PM2.5</div>
                      <div id="Div11" class="selIcon"></div>
                  </div>
                  <ul id="Ul4" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58462_AQI" class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div39">
                  <div id="Div40" class="hazeDiv">
                      <div class="hazeLevelText"  id="58462_Haze">无霾</div>
                      <div id="Div41" class="selIcon"></div>
                  </div>
                  <ul id="Ul14" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">金山区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58460_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div12">
                  <div id="Div13" class="dateDiv">
                      <div class="firstPolText"  id="58460_Item">PM2.5</div>
                      <div id="Div14" class="selIcon"></div>
                  </div>
                  <ul id="Ul5" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58460_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div42">
                  <div id="Div43" class="hazeDiv">
                      <div class="hazeLevelText"  id="58460_Haze">无霾</div>
                      <div id="Div44" class="selIcon"></div>
                  </div>
                  <ul id="Ul15" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">青浦区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58461_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div15">
                  <div id="Div16" class="dateDiv">
                      <div class="firstPolText"  id="58461_Item">PM2.5</div>
                      <div id="Div17" class="selIcon"></div>
                  </div>
                  <ul id="Ul6" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58461_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div45">
                  <div id="Div46" class="hazeDiv">
                      <div class="hazeLevelText"  id="58461_Haze">无霾</div>
                      <div id="Div47" class="selIcon"></div>
                  </div>
                  <ul id="Ul16" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">奉贤区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58463_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div18">
                  <div id="Div19" class="dateDiv">
                      <div class="firstPolText"  id="58463_Item">PM2.5</div>
                      <div id="Div20" class="selIcon"></div>
                  </div>
                  <ul id="Ul7" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58463_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div48">
                  <div id="Div49" class="hazeDiv">
                      <div class="hazeLevelText"  id="58463_Haze">无霾</div>
                      <div id="Div50" class="selIcon"></div>
                  </div>
                  <ul id="Ul17" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">嘉定区</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58365_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div21">
                  <div id="Div22" class="dateDiv">
                      <div class="firstPolText"  id="58365_Item">PM2.5</div>
                      <div id="Div23" class="selIcon"></div>
                  </div>
                  <ul id="Ul8" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58365_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div51">
                  <div id="Div52" class="hazeDiv">
                      <div class="hazeLevelText"  id="58365_Haze">无霾</div>
                      <div id="Div53" class="selIcon"></div>
                  </div>
                  <ul id="Ul18" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
              <tr><td class="c_RegionName">崇明</td>
              <td class="aqiLevelTd"><input readonly="readonly" class="aqiLevelInput" name="aqiLevel" type="text" id="58366_Level"/></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div24">
                  <div id="Div25" class="dateDiv">
                      <div class="firstPolText"  id="58366_Item">PM2.5</div>
                      <div id="Div26" class="selIcon"></div>
                  </div>
                  <ul id="Ul9" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3-1小时</div></li>
                      <li><div>O3-8小时</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2小时</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58366_AQI"  class="aqiValue" /></div><div class="levelColor levelColor_1"></div></div>
              </td>
              <td class="hazeTd">
              <div class="hazeLevelSelect" id="Div54">
                  <div id="Div55" class="hazeDiv">
                      <div class="hazeLevelText"  id="58366_Haze">无霾</div>
                      <div id="Div56" class="selIcon"></div>
                  </div>
                  <ul id="Ul19" class="hazeLevelUl hide">
                       <li><div>无霾</div></li>
                      <li><div>轻微霾</div></li>
                      <li><div>轻度霾</div></li>
                      <li><div>中度霾</div></li>
                      <li><div>重度霾</div></li>
                      <li><div>严重霾</div></li>
                  </ul>
              </div>
              
              </td>
              </tr>
          </table>
       </div>                 
       <div class="editBtns">                                             
          <div id="copyHaze" class="button">复制霾</div>
          <div id="copyFirstPol" class="button">复制首要污染物</div>
          <div id="tempSave" class="button">暂存</div>
          <div id="getHistory" class="button">获取历史</div>  
          <div id="autoCreate" class="button">自动生成</div>          
       </div> 
       </div>         
       <div class="textPart">
       <div class="editLabel">
           <div class="titlePoint"></div>
           <span class="partTitle">空气质量预报数据文件</span>
         </div>
        <div id="aqiText" class="textContent">
        <textarea id="dataFileContent">        
        </textarea>
        </div>
       <div class="editBtns2">   
         <input type="button" value="暂存" id="btnTxtSave" class="button" />              
          <input type="button" value="自动生成" id="Button1" class="button" />        
       </div>
       <div class="btnArea">
           <div class="btns">
              <input id="forePreview" type="button" class="foreBtn" style="display:none;" value="预览"/>
              <input id="foreSave"  type="button" class="foreBtn" value="保存"/>
              <input id="forePublish"  type="button" class="foreBtn" style="display:none;" value="发布"/>
           </div>
       </div>
       </div>                                     
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

<input type="hidden" id="FtpWordCollection" value="32Down,AQI_SHQXJ_YYYYMMDDHH_024.doc;62WebSite,scuem_AQI-SHQXJ_YYYYMMDD1402.doc" />
<input type="hidden" id="FtpTextCollection" value="NationalOffice,Z_SEVP_C_BCSH_YYYYMMddhhmmss_P_MSP3_SH-MO_ENVAQFC_AIR_L88_SH_YYYYMMDDHHmm_00000-07200.TXT" />
</form>
</body>
</html>
