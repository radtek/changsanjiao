<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataShare.aspx.cs" Inherits="AQI_DataShare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据共享</title>

    <script language="javascript" type="text/javascript"></script>
    
    <link href="images/css/ComforecastDataShare.css" rel="stylesheet" type="text/css" />
    <link href="images/css/css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    
    
    <script language="javascript" type="text/javascript" src="js/ComforecastDataShare.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <!-- stockjs -->
    <script type="text/javascript" src="../JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="../JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="../JS/Chart/modules/exporting.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField id="Element" runat="server" />
    <div id="contentShare">  
        <div id="tabbtnShare">
           <ul id="tabItem" runat="server">
               <li class='liTab'><span id="单站点多要素_0" class ="tabHighlight" onclick="tabStationClick('单站点多要素_0')">单站点多要素</span></li>
               <li class='liTab'><span id="多站点单要素_0"><a href="javascript:tabStationClick('多站点单要素_0')">多站点单要素</a></span></li>
           </ul>
        </div>
    </div>
    <div id="content" style="width: 1046px; margin-bottom: 10px;">
        <div id="tool" style="width: 886px;">
            <div class="checkStyleShare">
                 <div class="checkLable">开始时间：</div>
                 <input id="H00" type="text" class="selectDateFormStyle" style="width:150px" value="<%= m_FromDate%>"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日 HH时'})"/>
            </div>
            <div class="checkStyleShare">
                 <div class="checkLable">结束时间：</div>
                 <input id="H01" type="text" class="selectDateFormStyle"  style="width:150px" value="<%= m_ToDate%>" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日 HH时',minDate:'#F{$dp.$D(\'H00\')}'})"/>
            </div>
            <div id="tool_btn_area" style="width: 250px">
            
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="dataShareQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="exportSiteData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'" style="width: 116px">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
            <span class="select-but"></span>
         </button>
         <div id="menu" class="menu" style="display: none">
             <ul>
                 <li id="Q0" onmouseover="mouseOver(this)" class="menuSelect" onclick="menuClick('Q0')" onmouseout="mouseOut(this)">气象数据</li>
                 <li id="Q1" onmouseover="mouseOver(this)" class="menuNormal" onclick="menuClick('Q1')"  onmouseout="mouseOut(this)">环境数据</li>
             </ul>
         </div>
<%--          <label id="re" style="margin-left: 0px; text-decoration: underline; cursor: pointer; font-family: 微软雅黑; font-size: 17px; line-height: 20px; margin-bottom: 3px;" onclick="ExportStyle()">导出表</label> --%>
            </div>
        </div>
        <div id="simpleStation" class="show">
            <div id="type_select1row" style="width: 886px;">
                <div class="checkLable" style="margin-left: 12px; width: 40px;">站点：</div>
                    <div  class="shortdan_highlight" id="58367"><a href="javascript:tabClick('58367')">徐家汇</a></div>
                    <div class="shortdan" id="58361"><a href="javascript:tabClick('58361')">闵行</a></div>
                    <div class="shortdan" id="58370"><a href="javascript:tabClick('58370')">浦东</a></div>
                    <div class="shortdan" id="58362"><a href="javascript:tabClick('58362')">宝山</a></div>
                    <div class="shortdan" id="58462"><a href="javascript:tabClick('58462')">松江</a></div>
                    <div class="shortdan" id="58365"><a href="javascript:tabClick('58365')">嘉定</a></div>
                    <div class="shortdan" id="58461"><a href="javascript:tabClick('58461')">青浦</a></div>
                    <div class="shortdan" id="58460"><a href="javascript:tabClick('58460')">金山</a></div>
                    <div class="shortdan" id="58463"><a href="javascript:tabClick('58463')">奉贤</a></div>
                    <div class="shortdan" id="58366"><a href="javascript:tabClick('58366')">崇明</a></div>
                    <div class="buttonStyle">
                     <input type="button" id="table" class="tableButton defaulttableButton"  onmouseover="this.className = 'tableButton overtableButton';" onmouseout ="this.className ='tableButton defaulttableButton';"  onclick="tableQuery()"  />                
                     <input type="button" id="quxian" class="tableButton defaultQuxianButton" onclick="quxianQuery()" onmouseover="this.className = 'tableButton overQuxianButton';" onmouseout ="this.className ='tableButton defaultQuxianButton';"   />                
                    </div>
                </div>
        <div id="container" style="width: 1040px; height:450px; margin: 0 auto;"  class="show"></div>
        <div id="tablecontainer" style="width: 900px; height:450px; margin: 0 auto; overflow: auto;" class="hidden"></div>

        <div id="qxcontainer" style="width: 1040px; height:450px; margin: 0 auto;" class="show"></div>
                <div id="zdian" style="width: 1036px;" class="zdian">
                <div class="checkLable" style="margin-left: 12px; width: 40px;">站点：</div>
                    <div  class="shortdan_highlightH" id="183"><a href="javascript:tabClickQx('183')">静安监测站</a></div>
                    <div class="shortdanH" id="185"><a href="javascript:tabClickQx('185')">卢湾师专附小</a></div>
                    <div class="shortdanH" id="193"><a href="javascript:tabClickQx('193')">浦东川沙</a></div>
                    <div class="shortdanH" id="195"><a href="javascript:tabClickQx('195')">浦东张江</a></div>
                    <div class="shortdanH" id="201"><a href="javascript:tabClickQx('201')">普陀监测站</a></div>
                    <div class="shortdanH" id="203"><a href="javascript:tabClickQx('203')">青浦淀山湖</a></div>
                    <div class="shortdanH" id="207"><a href="javascript:tabClickQx('207')">徐汇上师大</a></div>
                    <div class="shortdanH" id="209"><a href="javascript:tabClickQx('209')">杨浦四漂</a></div>
                    <div class="shortdanH" id="215"><a href="javascript:tabClickQx('215')">虹口凉城</a></div>
                    <div class="shortdanH" id="228"><a href="javascript:tabClickQx('228')">浦东监测站</a></div>
                     <div class="buttonStyle">
                         <input type="button" id="tableH" class="tableButton defaulttableButton"  onmouseover="this.className = 'tableButton overtableButton';" onmouseout ="this.className ='tableButton defaulttableButton';"  onclick="tableQueryH()"  />                
                         <input type="button" id="quxianH" class="tableButton defaultQuxianButton" onclick="quxianQueryH()" onmouseover="this.className = 'tableButton overQuxianButton';" onmouseout ="this.className ='tableButton defaultQuxianButton';"   />                
                    </div>
         </div>
        <div id="qxcontainerTable" style="width: 900px; height:450px; margin: 0 auto; overflow: auto;" class="hidden"></div>
        </div>
        <div id="multStation" class="hidden">
            <div id="MultiStationQixi" style="width: 886px;" class="zdian">
                <div class="checkLable" style="margin-left: 12px; width: 65px;">气象条件：</div>
                    <div  class="shortdan_highlight" id="temperature"><a href="javascript:tabClickMulti('temperature')">温度</a></div>
                    <div class="shortdan" id="wind_direction"><a href="javascript:tabClickMulti('wind_direction')">风向</a></div>
                    <div class="shortdan" id="wind_speed"><a href="javascript:tabClickMulti('wind_speed')">风速</a></div>
                    <div class="shortdan" id="air_pressure"><a href="javascript:tabClickMulti('air_pressure')">气压</a></div>
                    <div class="shortdan" id="rain_sum"><a href="javascript:tabClickMulti('rain_sum')">降雨量</a></div>
                    <div class="shortdan" id="relativehumidity"><a href="javascript:tabClickMulti('relativehumidity')">相对湿度</a></div>
                     <div class="buttonStyle">
                         <input type="button" id="tableQ" class="tableButton defaulttableButton"  onmouseover="this.className = 'tableButton overtableButton';" onmouseout ="this.className ='tableButton defaulttableButton';"  onclick="tableQueryQ()"  />                
                         <input type="button" id="quxianQ" class="tableButton defaultQuxianButton" onclick="quxianQueryQ()" onmouseover="this.className = 'tableButton overQuxianButton';" onmouseout ="this.className ='tableButton defaultQuxianButton';"   />                
                    </div>
                </div>
        <div id="MultiContainer" style="width: 1040px; height:450px; margin: 0 auto;" class="show"></div>  
        <div id="MultiContainerQ" style="width: 1040px; height:450px; margin: 0 auto; overflow: auto;" class="hidden"></div> 

        <div id="Multiqxcontainer" style="width: 1040px; height:450px; margin: 0 auto;" class="show"></div>
        <div id="MUtiStationHuan" style="width: 886px;" class="zdian">
        <div class="checkLable" style="margin-left: 12px; width: 60px;">污染物：</div>
            <div  class="shortdan_highlightH" id="8"><a href="javascript:tabClickMultiQX('8')">PM2.5</a></div>
            <div class="shortdanH" id="3"><a href="javascript:tabClickMultiQX('3')">PM10</a></div>
            <div class="shortdanH" id="2"><a href="javascript:tabClickMultiQX('2')">NO2</a></div>
            <div class="shortdanH" id="6"><a href="javascript:tabClickMultiQX('6')">O3-1h</a></div>
            <div class="shortdanH" id="7"><a href="javascript:tabClickMultiQX('7')">O3-8h</a></div>
            <div class="shortdanH" id="1"><a href="javascript:tabClickMultiQX('1')">SO2</a></div>
            <div class="shortdanH" id="5"><a href="javascript:tabClickMultiQX('5')">CO</a></div>
             <div class="buttonStyle">
                 <input type="button" id="Button1" class="tableButton defaulttableButton"  onmouseover="this.className = 'tableButton overtableButton';" onmouseout ="this.className ='tableButton defaulttableButton';"  onclick="tableQueryQH()"  />                
                 <input type="button" id="Button2" class="tableButton defaultQuxianButton" onclick="quxianQueryQH()" onmouseover="this.className = 'tableButton overQuxianButton';" onmouseout ="this.className ='tableButton defaultQuxianButton';"   />                
            </div>
 </div>
        <div id="MultiContainerQH" style="width: 1040px; height:450px; margin: 0 auto; overflow: auto;" class="hidden"></div> 
        </div>
    </div>

<asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button"   CssClass="inVisibility"/>
</form>
</body>
</html>
