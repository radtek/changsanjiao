<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangjiangEcoZone.aspx.cs" Inherits="ReportProduce_ChangjiangEcoZone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ChangjiangEcoZone.css?v=20160429" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/ChangjiangEcoZone.js?v=20160530" type="text/javascript"></script>
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server" method="post">
<div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table>
            <tr><th class="attrName">预报员：</th>
            <td class="attrValue" id="forecaster"></td>
            <th class="attrName">预报时间：</th>
            <td id="forecastTime" class="attrValue"></td>
            <th class="attrName">预报时次：</th>
            <td id="forecastTimeLevel" class="attrValue"></td>
            <td class="btnTd">
            <div id="foreSave" class="button_Bottom">保存</div>
            <div id="downLoad" class="button_Bottom" style="display:none;"><a id="downLoadImg" href="">下载</a></div>
            <div id="foreCheck" class="button_Bottom" style="display:none;">审核</div>
            <div id="forePub" class="button_Bottom" style="display:none;">发布</div>
            </tr>
        </table>
      </div>      
   </div>
<div class="outLine">
<div class="totalContent" >
    <div id="map" class="map">
    <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>长江经济带AQI落区</span>
           </div>
    <div class="mapControl" id="mapControl">
        <div class="imgDiv">
            <img id="img_24" class="changjingIMG" src="../AQI/img/zw.png" />
            <img id="img_48" class="changjingIMG" style="display:none;" src="../AQI/img/zw.png" />
            <img id="img_72" class="changjingIMG" style="display:none;" src="../AQI/img/zw.png" />
            <div class="sitrPointValue" id="58367A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58468A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58457A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58259A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58349A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58245A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58238A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58334A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58321A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58424A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="58502A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="57494A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="57461A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="57516A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="56491A_Value"  onclick = "showInput(event,this)"></div>
            <div class="sitrPointValue" id="56289A_Value"  onclick = "showInput(event,this)"></div>
        </div>
    </div>
    
    </div>
    <div class="content" id="rightContent">
       <div class="tablePart">
       <div class="editLabel">
       <div class="titlePoint"></div>
       <span class="partTitle">AQI分区指导预报</span>
       </div>      
       <div class="selTab"><div id="tab1" class="tabItem selectedTab">21日20时-22日20时</div><div id="tab2" class="tabItem">22日20时-23日20时</div><div id="tab3" class="tabItem">23日20时-24日20时</div></div>                               
       <div id="aqiAreaTable" class="tableContent">
          <table class="aqiAreaTable">
              <tr>
                  <th scope="col" >预报区域</th>
                  <th scope="col">首要污染物</th>
                  <th scope="col">AQI指数</th>
              </tr>
              <tr>
              <td class="c_RegionName"><div class="areaName">徐家汇</div></td>             
              <td class="aqiItemTd">
              <div class="dateSelect" id="58367A_FirstPol">
                  <div id="selectID" class="dateDiv">
                      <div class="firstPolText" id="58367A_Item">O3</div>
                      <div id="selIcon" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58367A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                      <li><div>O3</div></li>
                      <li><div>O3-8h</div></li>                     
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58367A_AQI" class="aqiValue"/></div><div id="58367Av_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">余姚</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="58468A_FirstPol">
                  <div id="Div2" class="dateDiv">
                      <div class="firstPolText"  id="58468A_Item">PM2.5</div>
                      <div id="Div3" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58468A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58468A_AQI"  class="aqiValue" /></div><div id="58468A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">杭州</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div1">
                  <div id="Div4" class="dateDiv">
                      <div class="firstPolText"  id="58457A_Item">PM2.5</div>
                      <div id="Div5" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58457A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd"> 
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58457A_AQI"  class="aqiValue" /></div><div  id="58457A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">南通</div></td>             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div6">
                  <div id="Div7" class="dateDiv">
                      <div class="firstPolText"  id="58259A_Item">PM2.5</div>
                      <div id="Div8" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58259A" class="firstPolUl hide">
                     <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58259A_AQI"  class="aqiValue" /></div><div id="58259A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">吴中</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div9">
                  <div id="Div10" class="dateDiv">
                      <div class="firstPolText"  id="58349A_Item">PM2.5</div>
                      <div id="Div11" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58349A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58349A_AQI" class="aqiValue" /></div><div id="58349A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">扬州</div></td>
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div12">
                  <div id="Div13" class="dateDiv">
                      <div class="firstPolText"  id="58245A_Item">PM2.5</div>
                      <div id="Div14" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58245A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58245A_AQI"  class="aqiValue" /></div><div id="58245A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">南京</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div15">
                  <div id="Div16" class="dateDiv">
                      <div class="firstPolText"  id="58238A_Item">PM2.5</div>
                      <div id="Div17" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58238A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58238A_AQI"  class="aqiValue" /></div><div  id="58238A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">芜湖</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div18">
                  <div id="Div19" class="dateDiv">
                      <div class="firstPolText"  id="58334A_Item">PM2.5</div>
                      <div id="Div20" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58334A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58334A_AQI"  class="aqiValue" /></div><div id="58334A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">合肥</div></td>             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div21">
                  <div id="Div22" class="dateDiv">
                      <div class="firstPolText"  id="58321A_Item">PM2.5</div>
                      <div id="Div23" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58321A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58321A_AQI"  class="aqiValue" /></div><div id="58321A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">安庆</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div24">
                  <div id="Div25" class="dateDiv">
                      <div class="firstPolText"  id="58424A_Item">PM2.5</div>
                      <div id="Div26" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58424A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58424A_AQI"  class="aqiValue" /></div><div id="58424A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">九江</div></td>             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div57">
                  <div id="Div58" class="dateDiv">
                      <div class="firstPolText"  id="58502A_Item">PM2.5</div>
                      <div id="Div60" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_58502A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="58502A_AQI"  class="aqiValue" /></div><div id="58502A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">武汉</div></td>              
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div66">
                  <div id="Div67" class="dateDiv">
                      <div class="firstPolText"  id="57494A_Item">PM2.5</div>
                      <div id="Div69" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_57494A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="57494A_AQI"  class="aqiValue" /></div><div id="57494A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">宜昌</div></td>
             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div75">
                  <div id="Div76" class="dateDiv">
                      <div class="firstPolText"  id="57461A_Item">PM2.5</div>
                      <div id="Div78" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_57461A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="57461A_AQI"  class="aqiValue" /></div><div id="57461A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>
              <tr><td class="c_RegionName"><div class="areaName">沙坪坝</div></td>
             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div84">
                  <div id="Div85" class="dateDiv">
                      <div class="firstPolText"  id="57516A_Item">PM2.5</div>
                      <div id="Div87" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_57516A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="57516A_AQI"  class="aqiValue" /></div><div id="57516A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>

              <tr><td class="c_RegionName"><div class="areaName">宜宾</div></td>
             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div27">
                  <div id="Div28" class="dateDiv">
                      <div class="firstPolText"  id="56491A_Item">PM2.5</div>
                      <div id="Div30" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_56491A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="56491A_AQI"  class="aqiValue" /></div><div id="56491A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>

              <tr><td class="c_RegionName"><div class="areaName">双流</div></td>
             
              <td class="aqiItemTd">
              <div class="dateSelect" id="Div29">
                  <div id="Div31" class="dateDiv">
                      <div class="firstPolText"  id="56289A_Item">PM2.5</div>
                      <div id="Div33" class="selIcon"></div>
                  </div>
                  <ul id="firstPolUl_56289A" class="firstPolUl hide">
                      <li><div>PM2.5</div></li>
                      <li><div>PM10</div></li>
                       <li><div>O3</div></li>
                       <li><div>O3-8h</div></li>
                      <li><div>CO</div></li>
                      <li><div>SO2</div></li>
                      <li><div>NO2</div></li>
                  </ul>
              </div>
                  
              </td>
              <td class="aqiValueTd">
              <div  class="aqiContent"><div class="aqiInput"><input type="text" name="AQI" id="56289A_AQI"  class="aqiValue" /></div><div id="56289A_ColorNo" class="levelColor levelColor_1"></div></div>
              </td>
              
              </tr>
          </table>
       </div>       
       
                 
       <div class="editBtns">                                             
          <div id="copyHaze" class="button" style="display:none;">复制霾</div>
          <div id="copyFirstPol" class="button">复制首要污染物</div>
          <div id="tempSave" class="button">暂存</div>
          <div id="getHistory" class="button">获取历史</div>  
          <div id="autoCreate" class="button">自动生成</div>          
       </div> 
       </div>                                                     
       </div>  
       <div style="clear:both"></div>
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
<input type="hidden" id="wordModelName" value="AQI-SHQXJ_2015102214_024.doc" />
<input type="hidden" id="FtpCollection" value="NationalOffice,Z_SEVP_C_BCSH_YYYYMMddhhmmss_P_MSP3_SH-MO_ENVAQFC_AIR_L88_SH_YYYYMMDDHHmm_00000-07200.TXT;32Down,AQI_SHQXJ_YYYYMMDDHH_024.doc;62WebSite,scuem_AQI-SHQXJ_YYYYMMDD1402.doc" />
</form>
</body>
</html>