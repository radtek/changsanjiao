<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewingQuery.aspx.cs" Inherits="Viewing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="css/Viewing.css?v=2017090111" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
     <script src="JS/bootstrap.min.js" type="text/javascript"></script>  
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="js/ViewingQuery.js?V=2017090111" type="text/javascript"></script>
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.css?v=1" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"></script>
</head>
<body>
<form id="form1" runat="server" method="post">
<div class="tableTop">
      <div id="topInfo" class="titleContent">
       <table style=" width:93%">
            <tr><th class="attrName">预报员：</th>
            <td class="attrValue" id="forecaster"></td>
            <th class="attrName">起报时间：</th>
                 <td><input name="H00" type="text" id="H00" runat="server" class="selectDateFormStyle" onchange="DateChange(this)" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" value="2016-09-08"/></td>
            <td id="forecastTime" class="attrValue"></td>
            <th class="attrName">预报时次：</th>
            <td id="forecastTimeLevels" class="attrValue">  <div id="p08" class="radioChecked"><a href="javascript:radioClickModule('p08');">08时</a></div>
                                                            <div id="p20" class="radioUnChecked"><a href="javascript:radioClickModule('p20');">20时</a></div></td>
            <td class="btnTd" style="width:425px;margin-right:-20px;">
            <%--<div id="autoLoad" class="button_Bottom" style="width:48px" onclick="AutoLoad()">调取历史</div>--%>
            <div id="Div53" class="button_Bottom" style="width:48px; display:none" onclick="window.open('ViewingQuery.aspx');">调取历史</div>          
            <div id="foreSave" class="button_Bottom" style=" display:none">保存</div>
            <%--<div id="Div51" class="button_Bottom" style="width:48px" onclick="window.open('ColdWithCityQuery.aspx');">产品查询</div>  --%>
            <div id="Div16" class="button_Bottom" style="width:48px; display:none" onclick='publish()'>发布</div>
            
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
                <span class="partTitle">观景指数等级预报</span> 
                <div id="dd1er" runat="server" class="singleHazeLevel singleHazeLevel_Selected" style="margin-right:73%; margin-top: -2px;" onclick="radioClickDate('dd1er')"><%=DateTime.Now.ToString("MM月dd日") %></div>
                <div id="dd2er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style="margin-right:66%; margin-top: -32px;" onclick="radioClickDate('dd2er')"><%=DateTime.Now.AddDays(1).ToString("MM月dd日") %></div>
                <div id="dd3er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style="margin-right:59%; margin-top:-32px;" onclick="radioClickDate('dd3er')"><%=DateTime.Now.AddDays(2).ToString("MM月dd日") %></div>
             </div>                                    
               <div id="aqiAreaTable" class="tableContent">
                  <table class="aqiAreaTable">
                  <tr>
                    <th scope="col" style="width:45px;">序号</th>
                    <th scope="col"  style="width:75px;">分类</th>
                    <th scope="col"  style="width:175px;">单位名称</th>
                    <th scope="col" style="width:180px;">观景等级</th>
                    <th scope="col" style="width:200px;">简短提示</th>
                    <th scope="col" style="width:40.5%;">详细提示</th>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">1</div></td>
                  <td class="regionOrder" style=" width:120px"  rowspan='4'><div class="areaOrder">森林类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海辰山植物园</div></td>
                  <td class="aqiLevelTd">
                    <div id="58367_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="selectID" class="dateDiv">
                          <div class="firstPolText" id="58367_Item">适宜</div>
                          <div id="selIcon" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58367" class="firstPolUl hide">
                          <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="58367_ColorNo" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd" style=" border-right-width:0px;">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea9"  class="aqiValue" onchange="valign(this)" ></textarea></div></div>
                  </td>
                  <td class="hazeTd" style="border-right-width:0px;">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Text10"  class="aqiValueII" onchange="valign(this)" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">2</div></td>
                 
                  <td class="c_RegionName"><div class="areaName">上海海湾国家森林公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="58368_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="selectIDs" class="dateDiv">
                          <div class="firstPolText" id="58368_Item">适宜</div>
                          <div id="selIcons" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58368" class="firstPolUl hide">
                               <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="58368_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea10"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="58370_AQI"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">3</div></td>
           
                  <td class="c_RegionName"><div class="areaName">东平国家森林公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="58369_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div2" class="dateDiv">
                          <div class="firstPolText" id="58369_Item">不适宜</div>
                          <div id="Div4" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58369" class="firstPolUl hide">
                           <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="58369_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea11"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea1"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">4</div></td>
             
                  <td class="c_RegionName"><div class="areaName">上海明珠湖·西沙湿地景区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="58370_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div3" class="dateDiv">
                          <div class="firstPolText" id="58370_Item">非常适宜</div>
                          <div id="Div6" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58370" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="58370_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea12"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea2"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">5</div></td>
                  <td class="regionOrder" style=" width:120px" rowspan='2' ><div class="areaOrder">古镇类</div></td>
                  <td class="c_RegionName"><div class="areaName">枫泾古镇</div></td>
                  <td class="aqiLevelTd">
                    <div id="58371_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div5" class="dateDiv">
                          <div class="firstPolText" id="58371_Item">非常适宜</div>
                          <div id="Div8" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58371" class="firstPolUl hide">
                              <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="58371_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea13"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea3"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">6</div></td>

                  <td class="c_RegionName"><div class="areaName">朱家角古镇</div></td>
                  <td class="aqiLevelTd">
                    <div id="58372_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div7" class="dateDiv">
                          <div class="firstPolText" id="58372_Item">非常适宜</div>
                          <div id="Div10" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58372" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="58372_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea14"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea4"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">7</div></td>
                    <td class="regionOrder" style=" width:120px"  rowspan='3'><div class="areaOrder">游乐场类</div></td>
                  <td class="c_RegionName"><div class="areaName">锦江乐园</div></td>
                  <td class="aqiLevelTd">
                       <div id="58373_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div9" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">非常适宜</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea15"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea5"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">8</div></td>
            
                  <td class="c_RegionName"><div class="areaName">上海欢乐谷</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">非常适宜</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                              <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea16"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea6"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">9</div></td>
      
                  <td class="c_RegionName"><div class="areaName">迪士尼度假区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">非常适宜</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                                <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea17"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea7"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">10</div></td>
                    <td class="regionOrder" style=" width:120px"  rowspan='6'><div class="areaOrder">花、海边、水果采摘类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海鲜花港</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">非常适宜</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                            <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea18"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea8"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">11</div></td>
     
                  <td class="c_RegionName"><div class="areaName">上海马陆葡萄艺术村</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div1" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div14" class="dateDiv">
                          <div class="firstPolText" id="Div20">非常适宜</div>
                          <div id="Div21" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                               <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div22" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea20"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">12</div></td>
         
                  <td class="c_RegionName"><div class="areaName">廊下生态园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div23" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div24" class="dateDiv">
                          <div class="firstPolText" id="Div25">非常适宜</div>
                          <div id="Div26" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div27" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea22"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">13</div></td>
                    
                  <td class="c_RegionName"><div class="areaName">金山城市沙滩景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div28" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div29" class="dateDiv">
                          <div class="firstPolText" id="Div30">非常适宜</div>
                          <div id="Div31" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div32" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea24"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">14</div></td>
                  
                  <td class="c_RegionName"><div class="areaName">上海市青少年校外活动营地—东方绿舟</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div33" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div34" class="dateDiv">
                          <div class="firstPolText" id="Div35">非常适宜</div>
                          <div id="Div36" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div37" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea26"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">15</div></td>
               
                  <td class="c_RegionName"><div class="areaName">碧海金沙景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div38" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div39" class="dateDiv">
                          <div class="firstPolText" id="Div40">非常适宜</div>
                          <div id="Div41" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                            <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div42" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea28"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">16</div></td>
                    <td class="regionOrder" style=" width:120px"  rowspan='3'><div class="areaOrder">一般类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海野生动物园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div43" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div44" class="dateDiv">
                          <div class="firstPolText" id="Div45">非常适宜</div>
                          <div id="Div46" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                             <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div47" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea30"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">17</div></td>
                   
                  <td class="c_RegionName"><div class="areaName">上海世纪公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div48" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div49" class="dateDiv">
                          <div class="firstPolText" id="Div50">非常适宜</div>
                          <div id="Div52" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div54" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea32"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">18</div></td>
  
                  <td class="c_RegionName"><div class="areaName">上海金罗店美兰湖景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div120" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div121" class="dateDiv">
                          <div class="firstPolText" id="Div122">非常适宜</div>
                          <div id="Div123" class="selIcon"></div>  
                      </div>
                      <ul id="Ul20" class="firstPolUl hide">
                          <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div124" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea57"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea58"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div  class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('1')">复制观景指数</div>
                      <div  class="button" onclick="TempSave('综合观景指数','dd1er')">暂存</div>
                   
           </div>
           </div>
                
               <div id="aqiAreaTableII-ertong" class="tableContent" style=" display:none"> 
                  <table class="aqiAreaTable">
                  <tr>
                    <th scope="col" style="width:45px;">序号</th>
                    <th scope="col"  style="width:75px;">分类</th>
                    <th scope="col"  style="width:175px;">单位名称</th>
                    <th scope="col" style="width:180px;">观景等级</th>
                    <th scope="col" style="width:200px;">简短提示</th>
                    <th scope="col" style="width:40.5%;">详细提示</th>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">1</div></td>
                   <td class="regionOrder" style=" width:120px"  rowspan='4'><div class="areaOrder">森林类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海辰山植物园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div51" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div55" class="dateDiv">
                          <div class="firstPolText" id="Div56">适宜</div>
                          <div id="Div57" class="selIcon"></div>  
                      </div>
                      <ul id="Ul8" class="firstPolUl hide">
                                    <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div58" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd" style=" border-right-width:0px;">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea33"  class="aqiValue" onchange="valign(this)" ></textarea></div></div>
                  </td>
                  <td class="hazeTd" style="border-right-width:0px;">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea34"  class="aqiValueII" onchange="valign(this)" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">2</div></td>
       
                  <td class="c_RegionName"><div class="areaName">上海海湾国家森林公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div59" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div60" class="dateDiv">
                          <div class="firstPolText" id="Div61">适宜</div>
                          <div id="Div62" class="selIcon"></div>  
                      </div>
                      <ul id="Ul9" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div63" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea35"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea36"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">3</div></td>
              
                  <td class="c_RegionName"><div class="areaName">东平国家森林公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div64" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div65" class="dateDiv">
                          <div class="firstPolText" id="Div66">不适宜</div>
                          <div id="Div67" class="selIcon"></div>  
                      </div>
                      <ul id="Ul10" class="firstPolUl hide">
                        <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div68" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea37"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea38"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">4</div></td>
              
                  <td class="c_RegionName"><div class="areaName">上海明珠湖·西沙湿地景区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div69" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div70" class="dateDiv">
                          <div class="firstPolText" id="Div71">非常适宜</div>
                          <div id="Div72" class="selIcon"></div>  
                      </div>
                      <ul id="Ul11" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div73" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea39"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea40"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">5</div></td>
           <td class="regionOrder" style=" width:120px" rowspan='2' ><div class="areaOrder">古镇类</div></td>
                  <td class="c_RegionName"><div class="areaName">枫泾古镇</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div74" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div75" class="dateDiv">
                          <div class="firstPolText" id="Div76">非常适宜</div>
                          <div id="Div77" class="selIcon"></div>  
                      </div>
                      <ul id="Ul12" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div78" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea41"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea42"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">6</div></td>
               
                  <td class="c_RegionName"><div class="areaName">朱家角古镇</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div79" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div80" class="dateDiv">
                          <div class="firstPolText" id="Div81">非常适宜</div>
                          <div id="Div82" class="selIcon"></div>  
                      </div>
                      <ul id="Ul13" class="firstPolUl hide">
                                <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div83" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea43"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea44"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">7</div></td>
                 <td class="regionOrder" style=" width:120px"  rowspan='3'><div class="areaOrder">游乐场类</div></td>
                  <td class="c_RegionName"><div class="areaName">锦江乐园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div84" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div85" class="dateDiv">
                          <div class="firstPolText" id="Div86">非常适宜</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea15"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea5"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">8</div></td>

                  <td class="c_RegionName"><div class="areaName">上海欢乐谷</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">非常适宜</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea16"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea6"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">9</div></td>
       
                  <td class="c_RegionName"><div class="areaName">迪士尼度假区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">非常适宜</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea17"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea7"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">10</div></td>
                         <td class="regionOrder" style=" width:120px"  rowspan='6'><div class="areaOrder">花、海边、水果采摘类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海鲜花港</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">非常适宜</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea18"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea8"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">11</div></td>
         
                  <td class="c_RegionName"><div class="areaName">上海马陆葡萄艺术村</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div1" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div14" class="dateDiv">
                          <div class="firstPolText" id="Div20">非常适宜</div>
                          <div id="Div21" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                                <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div22" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea20"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">12</div></td>
          
                  <td class="c_RegionName"><div class="areaName">廊下生态园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div23" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div24" class="dateDiv">
                          <div class="firstPolText" id="Div25">非常适宜</div>
                          <div id="Div26" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                                <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div27" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea22"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">13</div></td>
            
                  <td class="c_RegionName"><div class="areaName">金山城市沙滩景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div28" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div29" class="dateDiv">
                          <div class="firstPolText" id="Div30">非常适宜</div>
                          <div id="Div31" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div32" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea24"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">14</div></td>
            
                  <td class="c_RegionName"><div class="areaName">上海市青少年校外活动营地—东方绿舟</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div33" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div34" class="dateDiv">
                          <div class="firstPolText" id="Div35">非常适宜</div>
                          <div id="Div36" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div37" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea26"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">15</div></td>
                 
                  <td class="c_RegionName"><div class="areaName">碧海金沙景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div38" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div39" class="dateDiv">
                          <div class="firstPolText" id="Div40">非常适宜</div>
                          <div id="Div41" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div42" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea28"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">16</div></td>
             <td class="regionOrder" style=" width:120px"  rowspan='3'><div class="areaOrder">一般类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海野生动物园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div43" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div44" class="dateDiv">
                          <div class="firstPolText" id="Div45">非常适宜</div>
                          <div id="Div46" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                           <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div47" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea30"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">17</div></td>

                  <td class="c_RegionName"><div class="areaName">上海世纪公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div48" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div49" class="dateDiv">
                          <div class="firstPolText" id="Div50">非常适宜</div>
                          <div id="Div52" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div54" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea32"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">18</div></td>

                  <td class="c_RegionName"><div class="areaName">上海金罗店美兰湖景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div120" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div121" class="dateDiv">
                          <div class="firstPolText" id="Div122">非常适宜</div>
                          <div id="Div123" class="selIcon"></div>  
                      </div>
                      <ul id="Ul20" class="firstPolUl hide">
                         <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div124" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea57"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea58"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('2')">复制观景指数</div>
                      <div  class="button" onclick="TempSave('综合观景指数','dd2er')">暂存</div>
           </div>
           </div>

               <div id="aqiAreaTableIII-ertong" class="tableContent" style=" display:none"> 
                  <table class="aqiAreaTable">
                    <tr>
                    <th scope="col" style="width:45px;">序号</th>
                    <th scope="col"  style="width:75px;">分类</th>
                    <th scope="col"  style="width:175px;">单位名称</th>
                    <th scope="col" style="width:180px;">观景等级</th>
                    <th scope="col" style="width:200px;">简短提示</th>
                    <th scope="col" style="width:40.5%;">详细提示</th>
                  </tr>
                  <tr>
                  <td class="regionOrder"><div class="areaOrder">1</div></td>
                         <td class="regionOrder" style=" width:120px"  rowspan='4'><div class="areaOrder">森林类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海辰山植物园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div87" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div88" class="dateDiv">
                          <div class="firstPolText" id="Div89">适宜</div>
                          <div id="Div90" class="selIcon"></div>  
                      </div>
                      <ul id="Ul14" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div91" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd" style=" border-right-width:0px;">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea45"  class="aqiValue" onchange="valign(this)" ></textarea></div></div>
                  </td>
                  <td class="hazeTd" style="border-right-width:0px;">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea46"  class="aqiValueII" onchange="valign(this)" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">2</div></td>
         
                  <td class="c_RegionName"><div class="areaName">上海海湾国家森林公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div92" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div93" class="dateDiv">
                          <div class="firstPolText" id="Div94">适宜</div>
                          <div id="Div95" class="selIcon"></div>  
                      </div>
                      <ul id="Ul15" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div96" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea47"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea48"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">3</div></td>
              
                  <td class="c_RegionName"><div class="areaName">东平国家森林公园</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div97" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div98" class="dateDiv">
                          <div class="firstPolText" id="Div99">不适宜</div>
                          <div id="Div100" class="selIcon"></div>  
                      </div>
                      <ul id="Ul16" class="firstPolUl hide">
                             <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div101" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea49"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea50"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">4</div></td>

                  <td class="c_RegionName"><div class="areaName">上海明珠湖·西沙湿地景区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div102" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div103" class="dateDiv">
                          <div class="firstPolText" id="Div104">非常适宜</div>
                          <div id="Div105" class="selIcon"></div>  
                      </div>
                      <ul id="Ul17" class="firstPolUl hide">
                              <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div106" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea51"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea52"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">5</div></td>
           <td class="regionOrder" style=" width:120px" rowspan='2' ><div class="areaOrder">古镇类</div></td>
                  <td class="c_RegionName"><div class="areaName">枫泾古镇</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div107" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div108" class="dateDiv">
                          <div class="firstPolText" id="Div109">非常适宜</div>
                          <div id="Div110" class="selIcon"></div>  
                      </div>
                      <ul id="Ul18" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div111" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea53"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea54"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">6</div></td>
             
                  <td class="c_RegionName"><div class="areaName">朱家角古镇</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div112" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div113" class="dateDiv">
                          <div class="firstPolText" id="Div114">非常适宜</div>
                          <div id="Div115" class="selIcon"></div>  
                      </div>
                      <ul id="Ul19" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                    <div id="Div116" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea55"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea56"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">7</div></td>
               <td class="regionOrder" style=" width:120px"  rowspan='3'><div class="areaOrder">游乐场类</div></td>
                  <td class="c_RegionName"><div class="areaName">锦江乐园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div117" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div118" class="dateDiv">
                          <div class="firstPolText" id="Div119">非常适宜</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                              <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea15"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea5"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">8</div></td>
    
                  <td class="c_RegionName"><div class="areaName">上海欢乐谷</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">非常适宜</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea16"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea6"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">9</div></td>
     
                  <td class="c_RegionName"><div class="areaName">迪士尼度假区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">非常适宜</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea17"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea7"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">10</div></td>
                 <td class="regionOrder" style=" width:120px"  rowspan='6'><div class="areaOrder">花、海边、水果采摘类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海鲜花港</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">非常适宜</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                              <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea18"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea8"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">11</div></td>
   
                  <td class="c_RegionName"><div class="areaName">上海马陆葡萄艺术村</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div1" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div14" class="dateDiv">
                          <div class="firstPolText" id="Div20">非常适宜</div>
                          <div id="Div21" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div22" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea20"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">12</div></td>
           
                  <td class="c_RegionName"><div class="areaName">廊下生态园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div23" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div24" class="dateDiv">
                          <div class="firstPolText" id="Div25">非常适宜</div>
                          <div id="Div26" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                            <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div27" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea22"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">13</div></td>
                
                  <td class="c_RegionName"><div class="areaName">金山城市沙滩景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div28" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div29" class="dateDiv">
                          <div class="firstPolText" id="Div30">非常适宜</div>
                          <div id="Div31" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                                  <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div32" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea24"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">14</div></td>
                   
                  <td class="c_RegionName"><div class="areaName">上海市青少年校外活动营地—东方绿舟</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div33" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div34" class="dateDiv">
                          <div class="firstPolText" id="Div35">非常适宜</div>
                          <div id="Div36" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div37" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea26"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">15</div></td>
                
                  <td class="c_RegionName"><div class="areaName">碧海金沙景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div38" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div39" class="dateDiv">
                          <div class="firstPolText" id="Div40">非常适宜</div>
                          <div id="Div41" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                                 <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div42" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea28"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">16</div></td>
               <td class="regionOrder" style=" width:120px"  rowspan='3'><div class="areaOrder">一般类</div></td>
                  <td class="c_RegionName"><div class="areaName">上海野生动物园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div43" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div44" class="dateDiv">
                          <div class="firstPolText" id="Div45">非常适宜</div>
                          <div id="Div46" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                                <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div47" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea30"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">17</div></td>
                 
                  <td class="c_RegionName"><div class="areaName">上海世纪公园</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div48" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div49" class="dateDiv">
                          <div class="firstPolText" id="Div50">非常适宜</div>
                          <div id="Div52" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                                   <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div54" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea32"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="regionOrder"><div class="areaOrder">18</div></td>
  
                  <td class="c_RegionName"><div class="areaName">上海金罗店美兰湖景区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div120" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div121" class="dateDiv">
                          <div class="firstPolText" id="Div122">非常适宜</div>
                          <div id="Div123" class="selIcon"></div>  
                      </div>
                      <ul id="Ul20" class="firstPolUl hide">
                         <li><div>五星</div></li>
                          <li><div>四星</div></li>
                          <li><div>三星</div></li>
                          <li><div>二星</div></li>
                       </ul>
                 </div>
                       <div id="Div124" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea57"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd" style="border-right-width:0px;">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea58"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('3')">复制观景指数</div>
                      <div  class="button" onclick="TempSave('综合观景指数','dd3er')">暂存</div>
           </div>
           </div>

                       
               <div id="map" class="mpnew" style="float:right;  left: 0px;">
                 <div class="mapTitleNew">
                  <span>观景指数等级预报</span>
                 </div>
                 <div class="mapDate"><span>预报时间：</span><span id="mapDate"><%= DateTime.Now.ToString("yyyy年MM月dd日") %></span></div>
                       <div class="mps" id="mps">   
                         <div id="ChenShan" style="background-size: contain;"  title="上海辰山植物园" data-html="true" data-toggle="tooltip" data-placement="top"></div> 
                         <div id="HaiWan" style="background-size: contain;"  title="上海海湾国家森林公园" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="DongPing" style="background-size: contain;"  title="东平国家森林公园" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="XiSha" style="background-size: contain;"  title="上海明珠湖·西沙湿地景区" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="FengJing" style="background-size: contain;"  title="枫泾古镇" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="ZhuJia" style="background-size: contain;"  title="朱家角古镇" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="JinJiang" style="background-size: contain;" title="锦江乐园" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="HuanLe" style="background-size: contain;"  title="上海欢乐谷" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="DSN"  class="DSN" style="background-size: contain;"  title="迪士尼度假区" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="XianHua" style="background-size: contain;" title="上海鲜花港" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="MaLu" style="background-size: contain;" title="上海马陆葡萄艺术村" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="LangXia" style="background-size: contain;"  title="廊下生态园" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="JinShan" style="background-size: contain;"  title="金山城市沙滩景区" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="DongFang" style="background-size: contain;"  title="东方绿舟" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="BiHai" style="background-size: contain;"  title="碧海金沙景区" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="YeSheng" style="background-size: contain;"  title="上海野生动物园" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="ShiJi" style="background-size: contain;" title="上海世纪公园" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                         <div id="JinLuo" style="background-size: contain;" title="上海金罗店美兰湖景区" data-html="true" data-toggle="tooltip" data-placement="top"></div>
                       <%--<div class="mapMask"></div>--%>
                       </div>
                       <div class="pubTime">上海中心气象台<span id="pubTime">2016年09月13日</span><span id="pubHour">20</span>时发布</div> 
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
