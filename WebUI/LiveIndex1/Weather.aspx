<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Weather.aspx.cs" Inherits="WealthyForecast_Wealthy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="css/Weather.css" rel="stylesheet" />

    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../Ext/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="js/Utility.js" type="text/javascript"></script>
    <script src="js/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js"  type="text/javascript"></script>
    <script src="js/bootstrap-datetimepicker.js" type="text/javascript" ></script>
    <script src="js/Weather.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="head">
        <div class="_head">
           <span class="site" style="text-align:right;margin-left:5%;">选择订正站点</span>
           <select id="site" onchange="refresh(id)"></select>
           <button type="button" id="getEle" class="btn btn-default col-sm-offset-1"style="height:27px;line-height:14px;" onclick="refresh(this.id)">获取数据</button>
           <button type="button" class="btn btn-default col-sm-offset-1"style="height:27px;line-height:14px;" onclick="save()">保存数据</button>
            <button type="button" class="btn btn-default col-sm-offset-1"style="height:27px;line-height:14px;" onclick="correct()">订正</button>
        </div>
    </div>
    <br />
    <br />
    <div class="navigate">
        <ul class="nav nav-tabs">
            <li id="day1" role="presentation" class="select active" onclick="selectDates(id)"><a id="a1" href="#">今天：2017年3月7日预报</a></li>
            <li id="day2" role="presentation" class="unSelect" onclick="selectDates(id)"><a id="a2" href="#">明天：2017年3月8日预报</a></li>
            <li id="day3" role="presentation"class="unSelect"onclick="selectDates(id)"><a id="a3" href="#">后天：2017年3月9日预报</a></li>
        </ul>
    </div>
    <br />


    <div class="content" id="table1" style="display:block">
        <label for="MaxTemperature" class="col-sm-2 col-sm-offset-1 fac1" code="w2" >最高气温(日):</label>
        <input type="text" name="text" class="MaxTemperature col-sm-2"style="text-align:left"/>
        <label for="MinTemperature" class="col-sm-2 col-sm-offset-1 fac1" code="w3">最低气温(日):</label>
        <input type="text" name="text" class="MinTemperature col-sm-2" style="text-align:left"/>
        
        <label for="MaxTemperatureM" class="col-sm-2 col-sm-offset-1 fac1" code="w63">最高气温(上午):</label>
        <input type="text" name="text" class="MaxTemperatureM col-sm-2"style="text-align:left"/>
        <label for="MinTemperatureA" class="col-sm-2 col-sm-offset-1 fac1" code="w64">最高气温(下午):</label>
        <input type="text" name="text" class="MinTemperatureA col-sm-2" style="text-align:left"/>

        <label for="ReaHumidity" class="col-sm-2 col-sm-offset-1 fac1" code="w14">相对湿度(日):</label><!--平均-->
        <input type="text" name="text" class="ReaHumidity col-sm-2"style="text-align:left"/>
        <label for="ReaHumidityN" class="col-sm-2 col-sm-offset-1 fac1" code="w31">相对湿度(晚上):</label><!--平均-->
        <input type="text" name="text" class="ReaHumidityN col-sm-2" style="text-align:left"/>

        <label for="ReaHumidityM" class="col-sm-2 col-sm-offset-1 fac1" code="w29">相对湿度(上午):</label><!--平均-->
        <input type="text" name="text" class="ReaHumidityM col-sm-2"style="text-align:left"/>
        <label for="ReaHumidityA" class="col-sm-2 col-sm-offset-1 fac1" code="w30">相对湿度(下午):</label><!--平均-->
        <input type="text" name="text" class="ReaHumidityA col-sm-2" style="text-align:left"/>

        <label for="MeanPressure" class="col-sm-2 col-sm-offset-1 fac1" code="w41">平均气压(日):</label>
        <input type="text" name="text" class="MeanPressure col-sm-2"style="text-align:left"/>
        <label for="MeanSpeed" class="col-sm-2 col-sm-offset-1 fac1" code="w17">平均风速(日):</label>
        <input type="text" name="text" class="MeanSpeed col-sm-2" style="text-align:left"/>

        <label for="WeaPhenomenaM" class="col-sm-2 col-sm-offset-1 fac1" code="w1">天气现象(白天):</label>
        <select class="WeaPhenomenaM col-sm-2"name="text" style="text-align:left"><option></option></select>
        <label for="WeaPhenomenaN" class="col-sm-2 col-sm-offset-1 fac1" code="w51">天气现象(夜晚):</label>
        <select class="WeaPhenomenaN col-sm-2"name="text"style="text-align:left"><option></option></select>

        <label for="HazeM" class="col-sm-2 col-sm-offset-1 fac1" code="w71">霾(上午):</label>
        <select  class="HazeM col-sm-2" name="text" style="text-align:left" id='MDJ1'>
        <option value="1">无霾</option>
        <option value="2">轻微</option>
        <option value="3">轻度</option>
        <option value="4">中度</option>
        <option value="5">重度</option>
        <option value="6">严重</option>
        </select>
        <label for="HazeA" class="col-sm-2 col-sm-offset-1 fac1" code="w72">霾(下午):</label>
        <select class="HazeN col-sm-2" name="text" style="text-align:left">
        <option value="1">无霾</option>
        <option value="2">轻微</option>
        <option value="3">轻度</option>
        <option value="4">中度</option>
        <option value="5">重度</option>
        <option value="6">严重</option>
        </select>
         <label for="SixWindM" class="col-sm-2 col-sm-offset-1 fac1" code="w73">6级阵风(上午):</label>
        <select  class="HazeM col-sm-2"name="text"style="text-align:left">
        <option value="0">没有</option>
        <option value="1">有</option>
        </select>
        <label for="SixWindA" class="col-sm-2 col-sm-offset-1 fac1" code="w74">6级阵风(下午):</label>
        <select class="HazeN col-sm-2"name="text" style="text-align:left">
        <option value="0">没有</option>
        <option value="1">有</option>
        </select>

        <label for="UVRankM" class="col-sm-2 col-sm-offset-1 fac1" code="w75">UV(上午):</label>
        <select class="UVRankM col-sm-2"name="text"style="text-align:left">
        <option value="1">1级</option>
        <option value="2">2级</option>
        <option value="3">3级</option>
        <option value="4">4级</option>
        <option value="5">5级</option>
        </select>
        <label for="UVRankA" class="col-sm-2 col-sm-offset-1 fac1" code="w76">UV(下午):</label>
        <select class="UVRankA col-sm-2"name="text" style="text-align:left">
        <option value="1">1级</option>
        <option value="2">2级</option>
        <option value="3">3级</option>
        <option value="4">4级</option>
        <option value="5">5级</option>
        </select>
        
        <label for="O3_1HM" class="col-sm-2 col-sm-offset-1 fac1" code="w77">O3_1h最大(上午):</label>
        <input type="text" name="text" class="O3_1HM col-sm-2"style="text-align:left"/>
        <label for="O3_1HA" class="col-sm-2 col-sm-offset-1 fac1" code="w78">O3_1h最大(下午):</label>
        <input type="text" name="text" class="O3_1HA col-sm-2" style="text-align:left"/>
        
        <label for="O3_8HM" class="col-sm-2 col-sm-offset-1 fac1" code="w79">O3_8h最大 (上午):</label>
        <input type="text" name="text" class="O3_8HM col-sm-2"style="text-align:left"/>
        <label for="O3_8HA" class="col-sm-2 col-sm-offset-1 fac1" code="w80">O3_8h最大 (下午):</label>
        <input type="text" name="text" class="O3_8HA col-sm-2" style="text-align:left"/>
        
        <label for="PM2.5M" class="col-sm-2 col-sm-offset-1 fac1" code="w81">PM2.5(上午):</label>
        <input type="text" name="text" class="PM2.5M col-sm-2"style="text-align:left"/>
        <label for="PM2.5A" class="col-sm-2 col-sm-offset-1 fac1" code="w82">PM2.5(下午):</label>
        <input type="text" name="text" class="PM2.5A col-sm-2" style="text-align:left"/>
        
        <label for="PM10M" class="col-sm-2 col-sm-offset-1 fac1" code="w83">PM10(上午):</label>
        <input type="text" name="text" class="PM10M col-sm-2"style="text-align:left"/>
        <label for="PM10A" class="col-sm-2 col-sm-offset-1 fac1" code="w84">PM10(下午):</label>
        <input type="text" name="text" class="PM10A col-sm-2" style="text-align:left"/>

        <div class="cal col-sm-3" style="float:left;line-height:20px;">
             <div class="checkbox">
                 <label><input type="checkbox" id="check1" value="0"/>重新计算今天指数</label>
             </div>
        </div>
    </div>
    
    <div class="content" id="table2" style="display:none">
        <label for="MaxTemperature" class="col-sm-2 col-sm-offset-1 fac2" code="w2">最高气温(日):</label>
        <input type="text" name="text2" class="MaxTemperature col-sm-2"style="text-align:left"/>
        <label for="MinTemperature" class="col-sm-2 col-sm-offset-1 fac2" code="w3">最低气温(日):</label>
        <input type="text" name="text2" class="MinTemperature col-sm-2" style="text-align:left"/>
        
        <label for="MaxTemperatureM" class="col-sm-2 col-sm-offset-1 fac2" code="w63">最高气温(上午):</label>
        <input type="text" name="text2" class="MaxTemperatureM col-sm-2"style="text-align:left"/>
        <label for="MinTemperatureA" class="col-sm-2 col-sm-offset-1 fac2" code="w64">最高气温(下午):</label>
        <input type="text" name="text2" class="MinTemperatureA col-sm-2" style="text-align:left"/>

        <label for="ReaHumidity" class="col-sm-2 col-sm-offset-1 fac2" code="w14">相对湿度(日):</label>
        <input type="text" name="text2" class="ReaHumidity col-sm-2"style="text-align:left"/>
        <label for="ReaHumidityN" class="col-sm-2 col-sm-offset-1 fac2" code="w31">相对湿度(晚上):</label>
        <input type="text" name="text2" class="ReaHumidityN col-sm-2" style="text-align:left"/>

        <label for="ReaHumidityM" class="col-sm-2 col-sm-offset-1 fac2" code="w29">相对湿度(上午):</label>
        <input type="text" name="text2" class="ReaHumidityM col-sm-2"style="text-align:left"/>
        <label for="ReaHumidityA" class="col-sm-2 col-sm-offset-1 fac2" code="w30">相对湿度(下午):</label>
        <input type="text" name="text2" class="ReaHumidityA col-sm-2" style="text-align:left"/>

        <label for="MeanPressure" class="col-sm-2 col-sm-offset-1 fac2" code="w41">平均气压(日):</label>
        <input type="text" name="text2" class="MeanPressure col-sm-2"style="text-align:left"/>
        <label for="MeanSpeed" class="col-sm-2 col-sm-offset-1 fac2" code="w17">平均风速(日):</label>
        <input type="text" name="text2" class="MeanSpeed col-sm-2" style="text-align:left"/>

        <label for="WeaPhenomenaM" class="col-sm-2 col-sm-offset-1 fac2" code="w1">天气现象(白天):</label>
        <select class="WeaPhenomenaM col-sm-2" name="text2"style="text-align:left"><option></option></select>
        <label for="WeaPhenomenaN" class="col-sm-2 col-sm-offset-1 fac2" code="w51">天气现象(夜晚):</label>
        <select class="WeaPhenomenaN col-sm-2" name="text2" style="text-align:left"><option></option></select>

        <label for="HazeM" class="col-sm-2 col-sm-offset-1 fac2" code="w71">霾(上午):</label>
        <select  class="HazeM col-sm-2" name="text2" style="text-align:left" id='MDJ2'>
        <option value="1">无霾</option>
        <option value="2">轻微</option>
        <option value="3">轻度</option>
        <option value="4">中度</option>
        <option value="5">重度</option>
        <option value="6">严重</option>
        </select>
        <label for="HazeA" class="col-sm-2 col-sm-offset-1 fac2" code="w72">霾(下午):</label>
        <select  class="HazeN col-sm-2" name="text2" style="text-align:left">
        <option value="1">无霾</option>
        <option value="2">轻微</option>
        <option value="3">轻度</option>
        <option value="4">中度</option>
        <option value="5">重度</option>
        <option value="6">严重</option>
        </select>
         <label for="SixWindM" class="col-sm-2 col-sm-offset-1 fac2"code="w73">6级阵风(上午):</label>
        <select class="HazeM col-sm-2" name="text2" style="text-align:left">
        <option value="0">没有</option>
        <option value="1">有</option>
        </select>
        <label for="SixWindA" class="col-sm-2 col-sm-offset-1 fac2" code="w74">6级阵风(下午):</label>
        <select class="HazeN col-sm-2"name="text2" style="text-align:left">
        <option value="0">没有</option>
        <option value="1">有</option>
        </select>

        <label for="UVRankM" class="col-sm-2 col-sm-offset-1 fac2" code="w75">UV(上午):</label>
        <select class="UVRankM col-sm-2" name="text2" style="text-align:left">
        <option value="1">1级</option>
        <option value="2">2级</option>
        <option value="3">3级</option>
        <option value="4">4级</option>
        <option value="5">5级</option>
        </select>
        <label for="UVRankA" class="col-sm-2 col-sm-offset-1 fac2"code="w76">UV(下午):</label>
        <select class="UVRankA col-sm-2"name="text2" style="text-align:left">
        <option value="1">1级</option>
        <option value="2">2级</option>
        <option value="3">3级</option>
        <option value="4">4级</option>
        <option value="5">5级</option>
        </select>
        
        <label for="O3_1HM" class="col-sm-2 col-sm-offset-1 fac2" code="w77">O3_1h最大(上午):</label>
        <input type="text" name="text2" class="O3_1HM col-sm-2"style="text-align:left"/>
        <label for="O3_1HA" class="col-sm-2 col-sm-offset-1 fac2" code="w78">O3_1h最大(下午):</label>
        <input type="text" name="text2" class="O3_1HA col-sm-2" style="text-align:left"/>
        
        <label for="O3_8HM" class="col-sm-2 col-sm-offset-1 fac2"code="w79">O3_8h最大 (上午):</label>
        <input type="text" name="text2" class="O3_8HM col-sm-2"style="text-align:left"/>
        <label for="O3_8HA" class="col-sm-2 col-sm-offset-1 fac2" code="w80">O3_8h最大 (下午):</label>
        <input type="text" name="text2" class="O3_8HA col-sm-2" style="text-align:left"/>
        
        <label for="PM2.5M" class="col-sm-2 col-sm-offset-1 fac2" code="w81">PM2.5(上午):</label>
        <input type="text" name="text2" class="PM2.5M col-sm-2"style="text-align:left"/>
        <label for="PM2.5A" class="col-sm-2 col-sm-offset-1 fac2" code="w82">PM2.5(下午):</label>
        <input type="text" name="text2" class="PM2.5A col-sm-2" style="text-align:left"/>
        
        <label for="PM10M" class="col-sm-2 col-sm-offset-1 fac2" code="w83">PM10(上午):</label>
        <input type="text" name="text2" class="PM10M col-sm-2"style="text-align:left"/>
        <label for="PM10A" class="col-sm-2 col-sm-offset-1 fac2" code="w84">PM10(下午):</label>
        <input type="text" name="text2" class="PM10A col-sm-2" style="text-align:left"/>

        <div class="cal col-sm-3" style="float:left;line-height:20px;">
             <div class="checkbox">
                 <label><input type="checkbox" id="check2" value="0"/>重新计算明天指数</label>
             </div>
        </div>
    </div>

    <div class="content" id="table3" style="display:none">
        <label for="MaxTemperature" class="col-sm-2 col-sm-offset-1 fac3" code="w2">最高气温(日):</label>
        <input type="text" name="text3" class="MaxTemperature col-sm-2"style="text-align:left"/>
        <label for="MinTemperature" class="col-sm-2 col-sm-offset-1 fac3" code="w3">最低气温(日):</label>
        <input type="text" name="text3" class="MinTemperature col-sm-2" style="text-align:left"/>
        
        <label for="MaxTemperatureM" class="col-sm-2 col-sm-offset-1 fac3" code="w63">最高气温(上午):</label>
        <input type="text" name="text3" class="MaxTemperatureM col-sm-2"style="text-align:left"/>
        <label for="MinTemperatureA" class="col-sm-2 col-sm-offset-1 fac3" code="w64">最高气温(下午):</label>
        <input type="text" name="text3" class="MinTemperatureA col-sm-2" style="text-align:left"/>

        <label for="ReaHumidity" class="col-sm-2 col-sm-offset-1 fac3" code="w14">相对湿度(日):</label>
        <input type="text" name="text3" class="ReaHumidity col-sm-2"style="text-align:left"/>
        <label for="ReaHumidityN" class="col-sm-2 col-sm-offset-1 fac3" code="w31">相对湿度(晚上):</label>
        <input type="text" name="text3" class="ReaHumidityN col-sm-2" style="text-align:left"/>

        <label for="ReaHumidityM" class="col-sm-2 col-sm-offset-1 fac3" code="w29">相对湿度(上午):</label>
        <input type="text" name="text3" class="ReaHumidityM col-sm-2"style="text-align:left"/>
        <label for="ReaHumidityA" class="col-sm-2 col-sm-offset-1 fac3" code="w30">相对湿度(下午):</label>
        <input type="text" name="text3" class="ReaHumidityA col-sm-2" style="text-align:left"/>

        <label for="MeanPressure" class="col-sm-2 col-sm-offset-1 fac3" code="w41">平均气压(日):</label>
        <input type="text" name="text3" class="MeanPressure col-sm-2"style="text-align:left"/>
        <label for="MeanSpeed" class="col-sm-2 col-sm-offset-1 fac3" code="w17">平均风速(日):</label>
        <input type="text" name="text3" class="MeanSpeed col-sm-2" style="text-align:left"/>

        <label for="WeaPhenomenaM" class="col-sm-2 col-sm-offset-1 fac3" code="w1">天气现象(白天):</label>
        <select class="WeaPhenomenaM col-sm-2" name="text3" style="text-align:left"><option></option></select>
        <label for="WeaPhenomenaN" class="col-sm-2 col-sm-offset-1 fac3" code="w51">天气现象(夜晚):</label>
        <select class="WeaPhenomenaN col-sm-2" name="text3" style="text-align:left"><option></option></select>

        <label for="HazeM" class="col-sm-2 col-sm-offset-1 fac3"code="w71">霾(上午):</label>
        <select  class="HazeM col-sm-2" name="text3" style="text-align:left" id='MDJ3'>
        <option value="1">无霾</option>
        <option value="2">轻微</option>
        <option value="3">轻度</option>
        <option value="4">中度</option>
        <option value="5">重度</option>
        <option value="6">严重</option>
        </select>
        <label for="HazeA" class="col-sm-2 col-sm-offset-1 fac3" code="w72">霾(下午):</label>
        <select class="HazeN col-sm-2"name="text3" style="text-align:left">
        <option value="1">无霾</option>
        <option value="2">轻微</option>
        <option value="3">轻度</option>
        <option value="4">中度</option>
        <option value="5">重度</option>
        <option value="6">严重</option>
        </select>

        <label for="SixWindM" class="col-sm-2 col-sm-offset-1 fac3"code="w73">6级阵风(上午):</label>
        <select  class="HazeM col-sm-2" name="text3" style="text-align:left">
        <option value="0">没有</option>
        <option value="1">有</option>
        </select>
        <label for="SixWindA" class="col-sm-2 col-sm-offset-1 fac3" code="w74">6级阵风(下午):</label>
        <select class="HazeN col-sm-2"name="text3" style="text-align:left">
        <option value="0">没有</option>
        <option value="1">有</option>
        </select>

        <label for="UVRankM" class="col-sm-2 col-sm-offset-1 fac3" code="w75">UV(上午):</label>
        <select class="UVRankM col-sm-2" name="text3" style="text-align:left">
        <option value="1">1级</option>
        <option value="2">2级</option>
        <option value="3">3级</option>
        <option value="4">4级</option>
        <option value="5">5级</option>
        </select>
        <label for="UVRankA" class="col-sm-2 col-sm-offset-1 fac3"code="w76">UV(下午):</label>
        <select class="UVRankA col-sm-2"name="text3" style="text-align:left">
        <option value="1">1级</option>
        <option value="2">2级</option>
        <option value="3">3级</option>
        <option value="4">4级</option>
        <option value="5">5级</option>
        </select>
        
        <label for="O3_1HM" class="col-sm-2 col-sm-offset-1 fac3" code="w77">O3_1h最大(上午):</label>
        <input type="text"name="text3" class="O3_1HM col-sm-2"style="text-align:left"/>
        <label for="O3_1HA" class="col-sm-2 col-sm-offset-1 fac3" code="w78">O3_1h最大(下午):</label>
        <input type="text" name="text3" class="O3_1HA col-sm-2" style="text-align:left"/>
        
        <label for="O3_8HM" class="col-sm-2 col-sm-offset-1 fac3"code="w79">O3_8h最大 (上午):</label>
        <input type="text" name="text3" class="O3_8HM col-sm-2"style="text-align:left"/>
        <label for="O3_8HA" class="col-sm-2 col-sm-offset-1 fac3" code="w80">O3_8h最大 (下午):</label>
        <input type="text" name="text3" class="O3_8HA col-sm-2" style="text-align:left"/>
        
        <label for="PM2.5M" class="col-sm-2 col-sm-offset-1 fac3" code="w81">PM2.5(上午):</label>
        <input type="text" name="text3" class="PM2.5M col-sm-2"style="text-align:left"/>
        <label for="PM2.5A" class="col-sm-2 col-sm-offset-1 fac3" code="w82">PM2.5(下午):</label>
        <input type="text" name="text3" class="PM2.5A col-sm-2" style="text-align:left"/>
        
        <label for="PM10M" class="col-sm-2 col-sm-offset-1 fac3" code="w83">PM10(上午):</label>
        <input type="text" name="text3" class="PM10M col-sm-2 "style="text-align:left"/>
        <label for="PM10A" class="col-sm-2 col-sm-offset-1 fac3" code="w84">PM10(下午):</label>
        <input type="text" name="text3" class="PM10A col-sm-2" style="text-align:left"/>

         <div class="cal col-sm-3" style="float:left;line-height:20px;">
             <div class="checkbox">
                 <label><input type="checkbox" id="check3" value="0"/>重新计算后天指数</label>
             </div>
        </div>
    </div>


    <input type="hidden" id="date" name="date" />
    </form>
</body>
</html>
