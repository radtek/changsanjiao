<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ColdWithCity.aspx.cs" Inherits="ColdWithCity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="css/ColdWithCity.css?v=201708211116" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css?v=201604129" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="js/ColdWithCity.js" type="text/javascript"></script>
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
      <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
     <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
</head>
<body>
<form id="form1" runat="server" method="post">
<div class="tableTop">
      <div id="topInfo" class="titleContent">
        <table style=" width:93%">
            <tr><th class="attrName">预报员：</th>
            <td class="attrValue" id="forecaster"></td>
            <th class="attrName">发布时间：</th>
<%--            <td id="forecastTime" class="attrValue"></td>--%>
            <td><input name="H00" type="text" id="forecastTime" runat="server" class="selectDateFormStyle" onchange="DateChange(this)" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" value="2016-09-08"/></td>
            <th class="attrName">发布时次：</th>
            <td id="forecastTimeLevels" class="attrValue">  <div id="p10" class="radioChecked"><a href="javascript:radioClickModule('p10');">10时</a></div>
                                                            <div id="p17" class="radioUnChecked"><a href="javascript:radioClickModule('p17');">17时</a></div></td>
            <td class="btnTd" style="width:335px;margin-right:-60px;">
            <div id="autoLoad" class="button_Bottom" style="width:48px" onclick="AutoLoad()">自动生成</div>          
            <div id="foreSave" class="button_Bottom">保存</div>
                  <div id="Div16" class="button_Bottom" style="width:48px" onclick="window.open('ElementChart.aspx');">要素查询</div>
            <div id="Div53" class="button_Bottom" style="width:48px" onclick="window.open('LevelChart.aspx?type=儿童感冒');">等级查询</div>
            <div id="Div51" class="button_Bottom" style=" visibility:hidden; width:48px" onclick="window.open('ColdWithCityQuery.aspx');">产品查询</div>  
      
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
                <span class="partTitle" style=" font-family:微软雅黑">儿童感冒气象风险预报</span>
<%--                <div id="dd1er" runat="server" class="singleHazeLevel singleHazeLevel_Selected" style="float:left; margin-top: -5px; margin-left:15px;" onclick="radioClickDate('dd1er')"></div>
                <div id="dd2er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style="float:left; margin-top: -5px;margin-left:5px;" onclick="radioClickDate('dd2er')"></div>--%>

                <div id="dd1er" runat="server" class="radioChecked" style=" font-size:13px; width:80px; float:left; margin-left:15px; cursor:pointer" onclick="radioClickDate('dd1er')"></div>
                <div id="dd2er" runat="server" class="radioUnChecked" style=" font-size:13px; width:80px; float:left; cursor:pointer" onclick="radioClickDate('dd2er')"></div>

                <span style="  margin-left:45px;float:left; color:blue" id='dzrq'> &nbsp</span>
             </div>                                    
               <div id="aqiAreaTable" class="tableContent">
                  <table class="aqiAreaTable">
                  <tr>
                      <th scope="col"  style="width:100px;">预报区域</th>
                      <th scope="col" style="width:160px;">风险等级</th>
                      <th scope="col" style="width:200px;">防范人群</th>
                      <th scope="col" style="width:49.5%;">预防措施</th>
              
                  </tr>
                  <tr>
                  <td class="c_RegionName"><div class="areaName">中心城区</div></td>
                  <td class="aqiLevelTd">
                    <div id="58367_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="selectID" class="dateDiv">
                          <div class="firstPolText" id="58367_Item">低</div>
                          <div id="selIcon" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58367" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="58367_ColorNo" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea9"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Text10"  class="aqiValueII" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">浦东新区</div></td>
                  <td class="aqiLevelTd">
                    <div id="58368_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="selectIDs" class="dateDiv">
                          <div class="firstPolText" id="58368_Item">低</div>
                          <div id="selIcons" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58368" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">闵行区</div></td>
                  <td class="aqiLevelTd">
                    <div id="58369_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div2" class="dateDiv">
                          <div class="firstPolText" id="58369_Item">低</div>
                          <div id="Div4" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58369" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">宝山区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="58370_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div3" class="dateDiv">
                          <div class="firstPolText" id="58370_Item">低</div>
                          <div id="Div6" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58370" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">松江区</div></td>
                  <td class="aqiLevelTd">
                    <div id="58371_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div5" class="dateDiv">
                          <div class="firstPolText" id="58371_Item">低</div>
                          <div id="Div8" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58371" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">金山区</div></td>
                  <td class="aqiLevelTd">
                    <div id="58372_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div7" class="dateDiv">
                          <div class="firstPolText" id="58372_Item">低</div>
                          <div id="Div10" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58372" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">青浦区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58373_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div9" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">低</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">奉贤区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">低</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">嘉定区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">低</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
                  <tr><td class="c_RegionName"><div class="areaName">崇明</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">低</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
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
              </table>
                      <div  class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('1')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('儿童感冒','dd1er')">暂存</div>
                   
           </div>
           </div>
               <div id="aqiAreaTableII-ertong" class="tableContent" style=" display:none">
                  <table class="aqiAreaTable">
                  <tr>
                      <th scope="col"  style="width:100px;">预报区域</th>
                      <th scope="col" style="width:160px;">风险等级</th>
                      <th scope="col" style="width:200px;">防范人群</th>
                      <th scope="col" style="width:49.5%;">预防措施</th>
              
                  </tr>
                  <tr>
                  <td class="c_RegionName"><div class="areaName">中心城区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div14" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div20" class="dateDiv">
                          <div class="firstPolText" id="Div21">低</div>
                          <div id="Div22" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div23" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea19"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea63"  class="aqiValueII" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">浦东新区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div24" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div25" class="dateDiv">
                          <div class="firstPolText" id="Div26">低</div>
                          <div id="Div27" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div28" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea20"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea64"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">闵行区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div29" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div30" class="dateDiv">
                          <div class="firstPolText" id="Div31">低</div>
                          <div id="Div32" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div33" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea21"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea65"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">宝山区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div34" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div35" class="dateDiv">
                          <div class="firstPolText" id="Div36">低</div>
                          <div id="Div37" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div38" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea22"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea66"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">松江区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div39" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div40" class="dateDiv">
                          <div class="firstPolText" id="Div41">低</div>
                          <div id="Div42" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div43" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea23"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea67"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">金山区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div44" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div45" class="dateDiv">
                          <div class="firstPolText" id="Div46">低</div>
                          <div id="Div47" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div48" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea24"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea68"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">青浦区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div49" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div50" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">低</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea25"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea69"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">奉贤区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">低</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea26"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea70"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">嘉定区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">低</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea27"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea71"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">崇明</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">低</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea28"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea72"  class="aqiValueII" ></textarea></div></div>
                  </td>
    
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('2')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('儿童感冒','dd2er')">暂存</div>
           </div>
           </div>        
               <div id="map" class="mpnew" style="float:right;  left: 0px;">
                 <div class="mapTitleNew">
                  <span>儿童感冒气象风险预报</span><span id="mpContent1" class="export" title="导出这张图片" onclick="cutDiv('儿童感冒气象风险预报','mapDate','pubTime','mps','pubHour')"></span>
                 </div>
                 <div class="mapDate"><span>预报时间：</span><span id="mapDate">2016年9月14日</span></div>
                       <div class="mps" id="mps">    
                       <div id="XuHui" ></div>
                       <div id="PuDong" ></div>
                       <div id="MinHang"  ></div>
                       <div id="BaoShanArea" ></div>
                       <div id="SongJiang" ></div>
                       <div id="JinShan"></div>
                       <div id="QingPu" ></div>
                       <div id="FengXian"  ></div>
                       <div id="JiaDing" ></div>
                       <div id="ChongMing"  ></div>
                       <div class="mapMask"></div>
                       </div>
                       <div class="pubTime">长三角环境气象预报预警中心<span id="pubTime">2016年09月13日</span><span id="pubHour">20</span>时发布</div>
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
    <div class="totalContentII" id="c2">
        <div class="content" id="Div1" style="float:left;">
           <div class="tablePart">
               <div class="editLabel">
           <div class="titlePoint"></div>
           <span class="partTitle">青少年和成年人感冒气象风险预报</span> 
             <div id="dd3er" runat="server" class="singleHazeLevel singleHazeLevel_Selected" style=" visibility:hidden; margin-right:73%; margin-top: -2px;" onclick="radioClickDateII('dd3er')">8月31日</div>
             <div id="dd4er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style="  visibility:hidden; margin-right:66%; margin-top: -32px;" onclick="radioClickDateII('dd4er')">9月1日</div>
           </div>                                    
               <div id="aqiAreaTableQN" class="tableContent">
                  <table class="aqiAreaTable">
                  <tr>
                      <th scope="col"  style="width:100px;">预报区域</th>
                      <th scope="col" style="width:160px;">风险等级</th>
                      <th scope="col" style="width:200px;">防范人群</th>
                      <th scope="col" style="width:49.5%;">预防措施</th>
                  </tr>
                  <tr>
                  <td class="c_RegionName"><div class="areaName">中心城区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div54" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div55" class="dateDiv">
                          <div class="firstPolText" id="Div56">低</div>
                          <div id="Div57" class="selIcon"></div>  
                      </div>
                      <ul id="Ul7" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div58" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea29"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea73"  class="aqiValueII" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">浦东新区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div59" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div60" class="dateDiv">
                          <div class="firstPolText" id="Div61">低</div>
                          <div id="Div62" class="selIcon"></div>  
                      </div>
                      <ul id="Ul8" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div63" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea30"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea74"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">闵行区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div64" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div65" class="dateDiv">
                          <div class="firstPolText" id="Div66">低</div>
                          <div id="Div67" class="selIcon"></div>  
                      </div>
                      <ul id="Ul9" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div68" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea31"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea75"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">宝山区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div69" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div70" class="dateDiv">
                          <div class="firstPolText" id="Div71">低</div>
                          <div id="Div72" class="selIcon"></div>  
                      </div>
                      <ul id="Ul10" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div73" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea32"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea76"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">松江区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div74" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div75" class="dateDiv">
                          <div class="firstPolText" id="Div76">低</div>
                          <div id="Div77" class="selIcon"></div>  
                      </div>
                      <ul id="Ul11" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div78" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea33"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea77"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">金山区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div79" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div80" class="dateDiv">
                          <div class="firstPolText" id="Div81">低</div>
                          <div id="Div82" class="selIcon"></div>  
                      </div>
                      <ul id="Ul12" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div83" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea34"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea78"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">青浦区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58373_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div9" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">低</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea35"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea79"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">奉贤区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">低</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea36"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea80"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">嘉定区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">低</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea37"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea81"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">崇明</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">低</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea38"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea82"  class="aqiValueII" ></textarea></div></div>
                  </td>
    
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('3')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('青年感冒','dd3er')">暂存</div>
               <%--       <div id="getHistory" class="button">获取历史</div>  
                      <div id="autoCreate" class="button">自动生成</div>    --%>      
           </div>
           </div>
               <div id="aqiAreaTableQNII" class="tableContent" style=" display:none">
                  <table class="aqiAreaTable">
                  <tr>
                      <th scope="col"  style="width:100px;">预报区域</th>
                      <th scope="col" style="width:160px;">风险等级</th>
                      <th scope="col" style="width:200px;">防范人群</th>
                      <th scope="col" style="width:49.5%;">预防措施</th>
              
                  </tr>
                  <tr>
                  <td class="c_RegionName"><div class="areaName">中心城区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div14" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div20" class="dateDiv">
                          <div class="firstPolText" id="Div21">低</div>
                          <div id="Div22" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div23" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea39"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea83"  class="aqiValueII" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">浦东新区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div24" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div25" class="dateDiv">
                          <div class="firstPolText" id="Div26">低</div>
                          <div id="Div27" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div28" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea40"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea84"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">闵行区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div29" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div30" class="dateDiv">
                          <div class="firstPolText" id="Div31">低</div>
                          <div id="Div32" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div33" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea41"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea85"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">宝山区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div34" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div35" class="dateDiv">
                          <div class="firstPolText" id="Div36">低</div>
                          <div id="Div37" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div38" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea42"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea86"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">松江区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div39" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div40" class="dateDiv">
                          <div class="firstPolText" id="Div41">低</div>
                          <div id="Div42" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div43" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea43"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea87"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">金山区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div44" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div45" class="dateDiv">
                          <div class="firstPolText" id="Div46">低</div>
                          <div id="Div47" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div48" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea44"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea88"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">青浦区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div49" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div50" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">低</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea45"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea89"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">奉贤区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">低</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea46"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea90"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">嘉定区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">低</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea47"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea91"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">崇明</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">低</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea48"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea92"  class="aqiValueII" ></textarea></div></div>
                  </td>
    
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('4')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('青年感冒','dd4er')">暂存</div>
        <%--              <div id="getHistory-ert" class="button">获取历史</div>  
                      <div id="autoCreate-ert" class="button">自动生成</div>      --%>    
           </div>
           </div>        
           <div id="mpII" class="mpnewII" style="float:right;  left: 0px;">
                 <div class="mapTitleNew">
                  <span>青少年和成年人感冒气象风险预报</span><span class="export" title="导出这张图片" onclick="cutDiv('青少年和成年人感冒气象风险预报','mapDateII','pubTimeII','mpsII','pubHourII')"></span>
                 </div>
                 <div class="mapDate"><span>预报时间：</span><span id="mapDateII">2016年9月14日</span></div>
                         
                     <div class="mpsII" id="mpsII">    
                       <div id="XuHuiII" ></div>
                       <div id="PuDongII" ></div>
                       <div id="MinHangII"  ></div>
                       <div id="BaoShanAreaII" ></div>
                       <div id="SongJiangII" ></div>
                       <div id="JinShanII"></div>
                       <div id="QingPuII" ></div>
                       <div id="FengXianII"  ></div>
                       <div id="JiaDingII" ></div>
                       <div id="ChongMingII"  ></div>
                       <div class="mapMask"></div>
                      </div>
                   <div class="pubTime">长三角环境气象预报预警中心<span id="pubTimeII">2016年09月13日</span><span id="pubHourII">10</span>时发布</div>
              </div>            
           </div>        
           </div>  
        <div style="clear:both"></div>
    </div>
    <div class="totalContentIII" id="c3">
        <div class="content" id="Div84" style="float:left;">
           <div class="tablePart">
               <div class="editLabel">
           <div class="titlePoint"></div>
           <span class="partTitle">老年人感冒气象风险预报</span> 
             <div id="dd5er" runat="server" class="singleHazeLevel singleHazeLevel_Selected" style=" visibility:hidden; margin-right:73%; margin-top: -2px;" onclick="radioClickDateIII('dd5er')">8月31日</div>
             <div id="dd6er" runat="server" class="singleHazeLevel singleHazeLevel_UnSelected" style=" visibility:hidden; margin-right:66%; margin-top: -32px;" onclick="radioClickDateIII('dd6er')">9月1日</div>
           </div>                                    
               <div id="aqiAreaTableLR" class="tableContent">
                  <table class="aqiAreaTable">
                  <tr>
                      <th scope="col"  style="width:100px;">预报区域</th>
                      <th scope="col" style="width:160px;">风险等级</th>
                      <th scope="col" style="width:200px;">防范人群</th>
                      <th scope="col" style="width:49.5%;">预防措施</th>
              
                  </tr>
                  <tr>
                  <td class="c_RegionName"><div class="areaName">中心城区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div88" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div89" class="dateDiv">
                          <div class="firstPolText" id="Div90">低</div>
                          <div id="Div91" class="selIcon"></div>  
                      </div>
                      <ul id="Ul13" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div92" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea49"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea93"  class="aqiValueII" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">浦东新区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div93" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div94" class="dateDiv">
                          <div class="firstPolText" id="Div95">低</div>
                          <div id="Div96" class="selIcon"></div>  
                      </div>
                      <ul id="Ul14" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div97" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea50"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea95"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">闵行区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div98" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div99" class="dateDiv">
                          <div class="firstPolText" id="Div100">低</div>
                          <div id="Div101" class="selIcon"></div>  
                      </div>
                      <ul id="Ul15" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div102" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea51"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea96"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">宝山区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div103" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div104" class="dateDiv">
                          <div class="firstPolText" id="Div105">低</div>
                          <div id="Div106" class="selIcon"></div>  
                      </div>
                      <ul id="Ul16" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div107" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea52"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea97"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">松江区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div108" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div109" class="dateDiv">
                          <div class="firstPolText" id="Div110">低</div>
                          <div id="Div111" class="selIcon"></div>  
                      </div>
                      <ul id="Ul17" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div112" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea53"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea98"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">金山区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div113" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div114" class="dateDiv">
                          <div class="firstPolText" id="Div115">低</div>
                          <div id="Div116" class="selIcon"></div>  
                      </div>
                      <ul id="Ul18" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div117" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea54"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea99"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">青浦区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58373_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div9" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">低</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea55"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea100"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">奉贤区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">低</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea56"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea101"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">嘉定区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">低</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea57"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea102"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">崇明</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">低</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea58"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea103"  class="aqiValueII" ></textarea></div></div>
                  </td>
    
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('5')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('老年感冒','dd5er')">暂存</div>
              <%--        <div id="getHistory" class="button">获取历史</div>  
                      <div id="autoCreate" class="button">自动生成</div>    --%>      
           </div>
           </div>
               <div id="aqiAreaTableLRII" class="tableContent" style=" display:none">
                  <table class="aqiAreaTable">
                  <tr>
                      <th scope="col"  style="width:100px;">预报区域</th>
                      <th scope="col" style="width:160px;">风险等级</th>
                      <th scope="col" style="width:200px;">防范人群</th>
                      <th scope="col" style="width:49.5%;">预防措施</th>
              
                  </tr>
                  <tr>
                  <td class="c_RegionName"><div class="areaName">中心城区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div14" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div20" class="dateDiv">
                          <div class="firstPolText" id="Div21">低</div>
                          <div id="Div22" class="selIcon"></div>  
                      </div>
                      <ul id="Ul1" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div23" class="levelColor levelColor_1 aqiInputs"> </div>  
                  </td>
                  <td class="aqiValueTd">
                  <div class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea59"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="hazeTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea104"  class="aqiValueII" ></textarea></div></div>
                   </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">浦东新区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div24" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div25" class="dateDiv">
                          <div class="firstPolText" id="Div26">低</div>
                          <div id="Div27" class="selIcon"></div>  
                      </div>
                      <ul id="Ul2" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div28" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea60"  class="aqiValue" ></textarea></div></div> 
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea106"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">闵行区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div29" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div30" class="dateDiv">
                          <div class="firstPolText" id="Div31">低</div>
                          <div id="Div32" class="selIcon"></div>  
                      </div>
                      <ul id="Ul3" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div33" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea61"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd"> 
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea107"  class="aqiValueII" ></textarea></div></div>
                  </td>
     
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">宝山区</div></td>
                  <td class="aqiLevelTd"> 
                    <div id="Div34" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div35" class="dateDiv">
                          <div class="firstPolText" id="Div36">低</div>
                          <div id="Div37" class="selIcon"></div>  
                      </div>
                      <ul id="Ul4" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div38" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea62"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea108"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">松江区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div39" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div40" class="dateDiv">
                          <div class="firstPolText" id="Div41">低</div>
                          <div id="Div42" class="selIcon"></div>  
                      </div>
                      <ul id="Ul5" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div43" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                       <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea94"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea109"  class="aqiValueII" ></textarea></div></div>
                  </td>
          
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">金山区</div></td>
                  <td class="aqiLevelTd">
                    <div id="Div44" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div45" class="dateDiv">
                          <div class="firstPolText" id="Div46">低</div>
                          <div id="Div47" class="selIcon"></div>  
                      </div>
                      <ul id="Ul6" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                    <div id="Div48" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd"> 
                    <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea105"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea110"  class="aqiValueII" ></textarea></div></div>
                  </td>
      
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">青浦区</div></td>
                  <td class="aqiLevelTd">
                       <div id="Div49" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div50" class="dateDiv">
                          <div class="firstPolText" id="58373_Item">低</div>
                          <div id="Div15" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58373" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58373_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea115"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea111"  class="aqiValueII" ></textarea></div></div>
                  </td>
   
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">奉贤区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58374_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div11" class="dateDiv">
                          <div class="firstPolText" id="583734_Item">低</div>
                          <div id="Div17" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58374" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58374_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                      <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea116"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea112"  class="aqiValueII" ></textarea></div></div>
                  </td>
         
                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">嘉定区</div></td>
                  <td class="aqiLevelTd">
                       <div id="58375_FirstPol" class="dateSelect" style=" margin-top:4px;">
                      <div id="Div12" class="dateDiv">
                          <div class="firstPolText" id="583735_Item">低</div>
                          <div id="Div18" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58375" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                       </ul>
                 </div>
                       <div id="58375_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">  
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea117"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea113"  class="aqiValueII" ></textarea></div></div>
                  </td>

                  </tr>
                  <tr><td class="c_RegionName"><div class="areaName">崇明</div></td>
                  <td class="aqiLevelTd">
                       <div id="58376_FirstPol" class="dateSelect" style=" margin-top:4px;">
                        <div id="Div13" class="dateDiv">
                          <div class="firstPolText" id="583736_Item">低</div>
                          <div id="Div19" class="selIcon"></div>  
                      </div>
                      <ul id="firstPolUl_58376" class="firstPolUl hide">
                          <li><div>低</div></li>
                          <li><div>轻微</div></li>
                          <li><div>中等</div></li>
                          <li><div>较高</div></li>
                          <li><div>高</div></li>
                      </ul>
                 </div>
                       <div id="58376_ColorNo" class="levelColor levelColor_1 aqiInputs"></div>  
                  </td>
                  <td class="aqiItemTd">
                     <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1" id="Textarea118"  class="aqiValue" ></textarea></div></div>
                  </td>
                  <td class="aqiValueTd">
                  <div  class="aqiContent"><div class="aqiInput"><textarea name="AQI"  rows="2" cols="1"  id="Textarea114"  class="aqiValueII" ></textarea></div></div>
                  </td>
                  </tr>
              </table>
                      <div class="editBtns">                                             
                      <div  class="button" onclick="copyFirstPol('6')">复制风险等级</div>
                      <div  class="button" onclick="TempSave('老年感冒','dd6er')">暂存</div>
     <%--                 <div id="getHistory-ert" class="button">获取历史</div>  
                      <div id="autoCreate-ert" class="button">自动生成</div>   --%>       
           </div>
           </div>        
              <div id="mpIII" class="mpnewII" style="float:right;  left: 0px;">
                 <div class="mapTitleNew">
                  <span>老年人感冒气象风险预报</span><span class="export" title="导出这张图片" onclick="cutDiv('老年人感冒气象风险预报','mapDateIII','pubTimeIII','mpsIII','pubHourIII')"></span>
                 </div>
                 <div class="mapDate"><span>预报时间：</span><span id="mapDateIII">2016年9月14日</span></div>
                    <div class="mpsIII" id="mpsIII">    
                       <div id="XuHuiIII" ></div>
                       <div id="PuDongIII" ></div>
                       <div id="MinHangIII"  ></div>
                       <div id="BaoShanAreaIII" ></div>
                       <div id="SongJiangIII" ></div>
                       <div id="JinShanIII"></div>
                       <div id="QingPuIII" ></div>
                       <div id="FengXianIII"  ></div>
                       <div id="JiaDingIII" ></div>
                       <div id="ChongMingIII"  ></div>
                       <div class="mapMask"></div>
                      </div>
                       <div class="pubTime">长三角环境气象预报预警中心<span id="pubTimeIII">2016年09月13日</span><span id="pubHourIII">10</span>时发布</div>
              </div>       
           </div>          
           </div>  
        <div style="clear:both"></div>
    </div>
</div>
      <input type="hidden" id="serverTime" value=""  runat="server"/>
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
