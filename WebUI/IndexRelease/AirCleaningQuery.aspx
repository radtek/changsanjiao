<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirCleaningQuery.aspx.cs" Inherits="IndexRelease_AirCleaningQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <meta name="renderer" content="webkit">
    <title></title>
    
    <link href="css/AirCleaningQuery.css?v=201601429" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="js/AirCleaningQuery.js?V=20160310" type="text/javascript"></script>
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"></script>
</head>
<body>
<form id="form1" runat="server" method="post">
    <div class="tableTop">
        <div id="topInfo" class="titleContent">
            <table style=" width:93%">
                <tr><th class="attrName">预报员：</th>
                    <td class="attrValueII" id="forecaster"></td>
                    <th class="attrName">起报时间：</th>
                    <td><input name="H00" type="text" id="H00" runat="server" class="selectDateFormStyle" onchange="DateChange(this)" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" value="2016-09-08"/></td>
                    <td id="forecastTime" class="attrValue" style=" display:none"></td>
                    <th class="attrName">预报时次：</th>
                    <td id="forecastTimeLevels" class="attrValue">  <div id="p08" class="radioChecked"><a href="javascript:radioClickModule('p08');">08时</a></div>
                                                                    <div id="p20" class="radioUnChecked"><a href="javascript:radioClickModule('p20');">20时</a></div></td>
                    <td class="btnTd" style="width:427px;">
                    <div id="Div52" class="button_Bottom" style="width:60px; display:none">自动生成</div>          
                    <div id="foreSave" class="button_Bottom" style="display:none">保存</div>
                    <div id="Div51" class="button_Bottom" style="width:60px; display:none">获取历史</div>  
                    <div id="Div16" class="button_Bottom" style="width:60px; display:none">要素查询</div>
                    </td>
                </tr>
            </table>
      </div>
    </div>

    <div class="outLine" >
    <div class="totalContent" id="c1">
        <div class="content" id="rightContent" style="float:left;">
           <div class="tablePart">
               <div class="editLabelET"> 
                <div class="titlePoint"></div>
                <span class="partTitle">空气清洁度指数风险预报</span> 
                <div id="dd1er" runat="server" class="singleHazeLevel singleHazeLevel_Selected" style="margin-right:73%; margin-top: -2px;" onclick="radioClickDate('dd1er')">8月31日</div>
                <div id="dd2er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style="margin-right:66%; margin-top: -32px;" onclick="radioClickDate('dd2er')">9月1日</div>
                <div id="dd3er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style="margin-right:59%; margin-top:-32px;" onclick="radioClickDate('dd3er')">9月2日</div>
             </div>                                    
               <div id="aqiAreaTable" class="tableContent">
                  <table class="aqiAreaTable">
                  <tr>
                    <th scope="col" style="width:45px;">序号</th>
                    <th scope="col"  style="width:175px;">预报景区</th>
                    <th scope="col" style="width:180px;">风险等级</th>
                    <th scope="col" style="width:200px;">简短提示</th>
                    <th scope="col" style="width:40.5%;">详细提示</th>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">1</div></td>
                  <td class="c_RegionName"><div class="areaName">上海野生动物园</div></td>
                  <td class="aqiLevelTd">
                    <div id="58367_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="selectID" class="dateDiv">
                          <div class="firstPolText" id="58367_Item">一般</div>
                          <div id="selIcon" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58367" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="58367_ColorNo" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea9"  class="aqiValue" onchange="valign(this)" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Text10"  class="aqiValueII" onchange="valign(this)" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">2</div></td>
                  <td class="c_RegionName"><div class="areaName">上海世纪公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="58368_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="selectIDs" class="dateDiv">
                          <div class="firstPolText" id="58368_Item">不清洁</div>
                          <div id="selIcons" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58368" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="58368_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea10"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="58370_AQI"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">3</div></td>
                  <td class="c_RegionName"><div class="areaName">上海鲜花港</div></td>
                  <td class="aqiLevelTd">
                    <div id="58369_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div2" class="dateDiv">
                          <div class="firstPolText" id="58369_Item">非常不清洁</div>
                          <div id="Div4" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58369" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="58369_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea11"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea1"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">4</div></td>
                  <td class="c_RegionName"><div class="areaName">锦江乐园</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="58370_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div3" class="dateDiv">
                          <div class="firstPolText" id="58370_Item">清洁</div>
                          <div id="Div6" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58370" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="58370_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea12"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea2"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">5</div></td>
                  <td class="c_RegionName"><div class="areaName">上海金罗店美兰湖景区</div></td>
                  <td class="aqiLevelTd">
                    <div id="58371_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div5" class="dateDiv">
                          <div class="firstPolText" id="58371_Item">清洁</div>
                          <div id="Div8" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58371" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="58371_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea13"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea3"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">6</div></td>
                  <td class="c_RegionName"><div class="areaName">上海马陆葡萄艺术村</div></td>
                  <td class="aqiLevelTd">
                    <div id="58372_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div7" class="dateDiv">
                          <div class="firstPolText" id="58372_Item">清洁</div>
                          <div id="Div10" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58372" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="58372_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea14"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea4"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">7</div></td>
                  <td class="c_RegionName"><div class="areaName">上海欢乐谷</div></td>
                  <td class="aqiLevelTd">
                       <div id="58373_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div9" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">清洁</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea15"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea5"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">8</div></td>
                  <td class="c_RegionName"><div class="areaName">上海辰山植物园</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">清洁</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea16"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea6"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">9</div></td>
                  <td class="c_RegionName"><div class="areaName">枫泾古镇</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">清洁</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea17"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea7"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">10</div></td>
                  <td class="c_RegionName"><div class="areaName">金山城市沙滩景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">清洁</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea18"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea8"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">11</div></td>
                  <td class="c_RegionName"><div class="areaName">廊下生态园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div1" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div14" class="dateDiv">
                          <div class="firstPolText" id="Div20">清洁</div>
                          <div id="Div21" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div22" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea20"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">12</div></td>
                  <td class="c_RegionName"><div class="areaName">朱家角古镇</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div23" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div24" class="dateDiv">
                          <div class="firstPolText" id="Div25">清洁</div>
                          <div id="Div26" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div27" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea22"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">13</div></td>
                  <td class="c_RegionName"><div class="areaName">上海市青少年校外活动营地—东方绿舟</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div28" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div29" class="dateDiv">
                          <div class="firstPolText" id="Div30">清洁</div>
                          <div id="Div31" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div32" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea24"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">14</div></td>
                  <td class="c_RegionName"><div class="areaName">碧海金沙景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div33" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div34" class="dateDiv">
                          <div class="firstPolText" id="Div35">清洁</div>
                          <div id="Div36" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div37" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea26"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">15</div></td>
                  <td class="c_RegionName"><div class="areaName">上海海湾国家森林公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div38" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div39" class="dateDiv">
                          <div class="firstPolText" id="Div40">清洁</div>
                          <div id="Div41" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div42" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea28"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">16</div></td>
                  <td class="c_RegionName"><div class="areaName">东平国家森林公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div43" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div44" class="dateDiv">
                          <div class="firstPolText" id="Div45">清洁</div>
                          <div id="Div46" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div47" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea30"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">17</div></td>
                  <td class="c_RegionName"><div class="areaName">上海明珠湖·西沙湿地景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div48" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div49" class="dateDiv">
                          <div class="firstPolText" id="Div50">清洁</div>
                          <div id="Div1" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div54" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea32"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div  class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('1')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('空气清洁度指数','dd1er')">暂存</div>
                   
           </div>
           </div>
                
               <div id="aqiAreaTableII-ertong" class="tableContent" style=" display:none">
                  
                  <table class="aqiAreaTable">
                  <tr>
                    <th scope="col" style="width:45px;">序号</th>
                    <th scope="col"  style="width:175px;">预报景区</th>
                    <th scope="col" style="width:180px;">风险等级</th>
                    <th scope="col" style="width:200px;">简短提示</th>
                    <th scope="col" style="width:40.5%;">详细提示</th>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">1</div></td>
                  <td class="c_RegionName"><div class="areaName">上海野生动物园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div55" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div56" class="dateDiv">
                          <div class="firstPolText" id="Div57">一般</div>
                          <div id="Div58" class="selIcon"></div>  
                      </div>
                      <ul id="Ul8" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div59" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea33"  class="aqiValue" onchange="valign(this)" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea34"  class="aqiValueII" onchange="valign(this)" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">2</div></td>
                  <td class="c_RegionName"><div class="areaName">上海世纪公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div60" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div61" class="dateDiv">
                          <div class="firstPolText" id="Div62">不清洁</div>
                          <div id="Div63" class="selIcon"></div>  
                      </div>
                      <ul id="Ul9" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div64" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea35"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea36"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">3</div></td>
                  <td class="c_RegionName"><div class="areaName">上海鲜花港</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div65" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div66" class="dateDiv">
                          <div class="firstPolText" id="Div67">非常不清洁</div>
                          <div id="Div68" class="selIcon"></div>  
                      </div>
                      <ul id="Ul10" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div69" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea37"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea38"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">4</div></td>
                  <td class="c_RegionName"><div class="areaName">锦江乐园</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div70" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div71" class="dateDiv">
                          <div class="firstPolText" id="Div72">清洁</div>
                          <div id="Div73" class="selIcon"></div>  
                      </div>
                      <ul id="Ul11" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div74" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea39"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea40"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">5</div></td>
                  <td class="c_RegionName"><div class="areaName">上海金罗店美兰湖景区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div75" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div76" class="dateDiv">
                          <div class="firstPolText" id="Div77">清洁</div>
                          <div id="Div78" class="selIcon"></div>  
                      </div>
                      <ul id="Ul12" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div79" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea41"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea42"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">6</div></td>
                  <td class="c_RegionName"><div class="areaName">上海马陆葡萄艺术村</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div80" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div81" class="dateDiv">
                          <div class="firstPolText" id="Div82">清洁</div>
                          <div id="Div83" class="selIcon"></div>  
                      </div>
                      <ul id="Ul13" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div84" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea43"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea44"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">7</div></td>
                  <td class="c_RegionName"><div class="areaName">上海欢乐谷</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div85" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div86" class="dateDiv">
                          <div class="firstPolText" id="Div119">清洁</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea15"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea5"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">8</div></td>
                  <td class="c_RegionName"><div class="areaName">上海辰山植物园</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">清洁</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea16"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea6"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">9</div></td>
                  <td class="c_RegionName"><div class="areaName">枫泾古镇</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">清洁</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea17"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea7"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">10</div></td>
                  <td class="c_RegionName"><div class="areaName">金山城市沙滩景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">清洁</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea18"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea8"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">11</div></td>
                  <td class="c_RegionName"><div class="areaName">廊下生态园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div1" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div14" class="dateDiv">
                          <div class="firstPolText" id="Div20">清洁</div>
                          <div id="Div21" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div22" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea20"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">12</div></td>
                  <td class="c_RegionName"><div class="areaName">朱家角古镇</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div23" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div24" class="dateDiv">
                          <div class="firstPolText" id="Div25">清洁</div>
                          <div id="Div26" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div27" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea22"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">13</div></td>
                  <td class="c_RegionName"><div class="areaName">上海市青少年校外活动营地—东方绿舟</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div28" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div29" class="dateDiv">
                          <div class="firstPolText" id="Div30">清洁</div>
                          <div id="Div31" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div32" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea24"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">14</div></td>
                  <td class="c_RegionName"><div class="areaName">碧海金沙景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div33" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div34" class="dateDiv">
                          <div class="firstPolText" id="Div35">清洁</div>
                          <div id="Div36" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div37" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea26"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">15</div></td>
                  <td class="c_RegionName"><div class="areaName">上海海湾国家森林公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div38" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div39" class="dateDiv">
                          <div class="firstPolText" id="Div40">清洁</div>
                          <div id="Div41" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div42" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea28"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">16</div></td>
                  <td class="c_RegionName"><div class="areaName">东平国家森林公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div43" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div44" class="dateDiv">
                          <div class="firstPolText" id="Div45">清洁</div>
                          <div id="Div46" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div47" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea30"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">17</div></td>
                  <td class="c_RegionName"><div class="areaName">上海明珠湖·西沙湿地景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div48" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div49" class="dateDiv">
                          <div class="firstPolText" id="Div50">清洁</div>
                          <div id="Div1" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div54" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea32"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('2')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('空气清洁度指数','dd2er')">暂存</div>
           </div>
           </div>

               <div id="aqiAreaTableIII-ertong" class="tableContent" style=" display:none">
                  
                  <table class="aqiAreaTable">
                  <tr>
                    <th scope="col" style="width:45px;">序号</th>
                    <th scope="col"  style="width:175px;">预报景区</th>
                    <th scope="col" style="width:180px;">风险等级</th>
                    <th scope="col" style="width:200px;">简短提示</th>
                    <th scope="col" style="width:40.5%;">详细提示</th>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">1</div></td>
                  <td class="c_RegionName"><div class="areaName">上海野生动物园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div53" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div87" class="dateDiv">
                          <div class="firstPolText" id="Div88">一般</div>
                          <div id="Div89" class="selIcon"></div>  
                      </div>
                      <ul id="Ul14" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div90" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea45"  class="aqiValue" onchange="valign(this)" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea46"  class="aqiValueII" onchange="valign(this)" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">2</div></td>
                  <td class="c_RegionName"><div class="areaName">上海世纪公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div91" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div92" class="dateDiv">
                          <div class="firstPolText" id="Div93">不清洁</div>
                          <div id="Div94" class="selIcon"></div>  
                      </div>
                      <ul id="Ul15" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div95" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea47"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea48"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">3</div></td>
                  <td class="c_RegionName"><div class="areaName">上海鲜花港</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div96" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div97" class="dateDiv">
                          <div class="firstPolText" id="Div98">非常不清洁</div>
                          <div id="Div99" class="selIcon"></div>  
                      </div>
                      <ul id="Ul16" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div100" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea49"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea50"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">4</div></td>
                  <td class="c_RegionName"><div class="areaName">锦江乐园</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div101" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div102" class="dateDiv">
                          <div class="firstPolText" id="Div103">清洁</div>
                          <div id="Div104" class="selIcon"></div>  
                      </div>
                      <ul id="Ul17" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div105" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea51"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea52"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">5</div></td>
                  <td class="c_RegionName"><div class="areaName">上海金罗店美兰湖景区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div106" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div107" class="dateDiv">
                          <div class="firstPolText" id="Div108">清洁</div>
                          <div id="Div109" class="selIcon"></div>  
                      </div>
                      <ul id="Ul18" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div110" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea53"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea54"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">6</div></td>
                  <td class="c_RegionName"><div class="areaName">上海马陆葡萄艺术村</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div111" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div112" class="dateDiv">
                          <div class="firstPolText" id="Div113">清洁</div>
                          <div id="Div114" class="selIcon"></div>  
                      </div>
                      <ul id="Ul19" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                    <div id="Div115" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea55"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea56"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">7</div></td>
                  <td class="c_RegionName"><div class="areaName">上海欢乐谷</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div116" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div117" class="dateDiv">
                          <div class="firstPolText" id="Div118">清洁</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea15"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea5"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">8</div></td>
                  <td class="c_RegionName"><div class="areaName">上海辰山植物园</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">清洁</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea16"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea6"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">9</div></td>
                  <td class="c_RegionName"><div class="areaName">枫泾古镇</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">清洁</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea17"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea7"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">10</div></td>
                  <td class="c_RegionName"><div class="areaName">金山城市沙滩景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">清洁</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea18"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea8"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">11</div></td>
                  <td class="c_RegionName"><div class="areaName">廊下生态园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div1" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div14" class="dateDiv">
                          <div class="firstPolText" id="Div20">清洁</div>
                          <div id="Div21" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div22" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea20"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">12</div></td>
                  <td class="c_RegionName"><div class="areaName">朱家角古镇</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div23" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div24" class="dateDiv">
                          <div class="firstPolText" id="Div25">清洁</div>
                          <div id="Div26" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div27" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea22"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">13</div></td>
                  <td class="c_RegionName"><div class="areaName">上海市青少年校外活动营地—东方绿舟</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div28" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div29" class="dateDiv">
                          <div class="firstPolText" id="Div30">清洁</div>
                          <div id="Div31" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div32" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea24"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">14</div></td>
                  <td class="c_RegionName"><div class="areaName">碧海金沙景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div33" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div34" class="dateDiv">
                          <div class="firstPolText" id="Div35">清洁</div>
                          <div id="Div36" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div37" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea26"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">15</div></td>
                  <td class="c_RegionName"><div class="areaName">上海海湾国家森林公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div38" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div39" class="dateDiv">
                          <div class="firstPolText" id="Div40">清洁</div>
                          <div id="Div41" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div42" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea28"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">16</div></td>
                  <td class="c_RegionName"><div class="areaName">东平国家森林公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div43" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div44" class="dateDiv">
                          <div class="firstPolText" id="Div45">清洁</div>
                          <div id="Div46" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div47" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea30"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>

                  <tr><td class="regionOrder"><div class="areaOrder">17</div></td>
                  <td class="c_RegionName"><div class="areaName">上海明珠湖·西沙湿地景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div48" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div49" class="dateDiv">
                          <div class="firstPolText" id="Div50">清洁</div>
                          <div id="Div1" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                          <li><div>清洁</div></li>
                          <li><div>一般</div></li>
                          <li><div>不清洁</div></li>
                          <li><div>非常不清洁</div></li>
                       </ul>
                 </div>
                       <div id="Div54" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea32"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('3')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('空气清洁度指数','dd3er')">暂存</div>
           </div>
           </div>

                       
               <div id="map" class="mpnew" style="float:right;  left: 0px;">
                 <div class="mapTitleNew">
                  <span>空气清洁度风险预报</span>
                 </div>
                 <div class="mapDate"><span>预报时间：</span><span id="mapDate">2016年9月14日</span></div>
                       <div class="mps" id="mps">    
                       <div id="YeSheng" style="background-size: contain; " title="上海野生动物园"></div>
                       <div id="ShiJi"  title="上海世纪公园"></div>
                       <div id="XianHua" title="上海鲜花港"></div>
                       <div id="JinJiang" title="锦江乐园"></div>
                       <div id="JinLuo" title="上海金罗店美兰湖景区"></div>
                       <div id="MaLu" title="上海马陆葡萄艺术村"></div>
                       <div id="HuanLe" title="上海欢乐谷"></div>
                       <div id="ChenShan" title="上海辰山植物园"></div>
                       <div id="FengJing" title="枫泾古镇"></div>
                       <div id="JinShan" title="金山城市沙滩景区"></div>
                       <div id="LangXia" title="廊下生态园"></div>
                       <div id="ZhuJia" title="朱家角古镇"></div>
                       <div id="DongFang" title="东方绿舟"></div>
                       <div id="BiHai" title="碧海金沙景区"></div>
                       <div id="HaiWan" title="上海海湾国家森林公园"></div>
                       <div id="DongPing" title="东平国家森林公园"></div>
                       <div id="XiSha" title="上海明珠湖·西沙湿地景区"></div>
                       <%--<div class="mapMask"></div>--%>
                       </div>
                       <div class="pubTime">上海中心气象台<span id="pubTime">2016年09月13日</span><span id="pubHour">20</span>时发布</div>
                  <%--        <div class="mapTitle">
               <div class="titlePoint"></div>
               <span>上海感冒气象风险等级分区预报</span>
               </div>
                 <div class="mapControl" id="mapControl">
                   <div class="districtArea" id="districtArea">        
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
                   <div class="mapMask"></div>
               </div>
               </div>--%> 
              </div>       
           </div>              
           </div>  
        <div style="clear:both"></div>
    </div>
</div>
</form>
</body>
</html>
