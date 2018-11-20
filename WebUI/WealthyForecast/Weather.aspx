<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Weather.aspx.cs" Inherits="WealthyForecast_Wealthy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="themes/default/easyui.css" />
    <link href="css/Weather.css?v=20170904" rel="stylesheet" type="text/css" />
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-datetimepicker.js" type="text/javascript"></script>
    <script src="js/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="js/Weather.js?v=20170908" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="head">
        <div class="_head">
           <span class="site" style="text-align:right;margin-left:5%;">选择订正站点</span>
           <select id="site" onchange="refresh(this)"><option></option></select>
           <a href="#" style="text-decoration:none;color:#000"><span class="calculate" onclick="reCalculate()">&nbsp &nbsp &nbsp 重新计算</span></a>
           <button type="button" class="btn btn-default col-sm-offset-1"style="height:27px;line-height:14px;" onclick="save()">保存数据</button>
           <button type="button" class="btn btn-default col-sm-offset-1"style="height:27px;line-height:14px;" onclick="Update()" title="调取上一次数据">调取数据</button>
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
        <label for="MaxTemperature" class="col-sm-2 col-sm-offset-1"  style=' display:none'>最高气温（日）:</label>
        <input type="text" name="text" class="MaxTemperature col-sm-2" style="text-align:left; display:none"/>
        <label for="MinTemperature" class="col-sm-2 col-sm-offset-1" style=' display:none'>最低气温（日）:</label>
        <input type="text" name="text" class="MinTemperature col-sm-2" style="text-align:left;display:none"/>
        
        <label for="MaxTemperatureM" class="col-sm-2 col-sm-offset-1"  style=' display:none'>最高气温（上午）:</label>
        <input type="text" name="text" class="MaxTemperatureM col-sm-2"style="text-align:left;display:none"/>
        <label for="MinTemperatureA" class="col-sm-2 col-sm-offset-1"  style=' display:none'>最低气温（下午）:</label>
        <input type="text" name="text" class="MinTemperatureA col-sm-2" style="text-align:left;display:none"/>

        <label for="ReaHumidity" class="col-sm-2 col-sm-offset-1"  style=' display:none'>相对湿度（日）:</label>
        <input type="text" name="text" class="ReaHumidity col-sm-2"style="text-align:left;display:none"/>
        <label for="ReaHumidityN" class="col-sm-2 col-sm-offset-1"  style=' display:none'>最低气温（晚上）:</label>
        <input type="text" name="text" class="ReaHumidityN col-sm-2" style="text-align:left;display:none"/>

        <label for="ReaHumidityM" class="col-sm-2 col-sm-offset-1"  style=' display:none'>相对湿度（上午）:</label>
        <input type="text" name="text" class="ReaHumidityM col-sm-2"style="text-align:left;display:none"/>
        <label for="ReaHumidityA" class="col-sm-2 col-sm-offset-1"  style=' display:none'>最低气温（下午）:</label>
        <input type="text" name="text" class="ReaHumidityA col-sm-2" style="text-align:left;display:none"/>

        <label for="MeanPressure" class="col-sm-2 col-sm-offset-1"  style=' display:none'>日平均气压:</label>
        <input type="text" name="text" class="MeanPressure col-sm-2"style="text-align:left;display:none"/>
        <label for="MeanSpeed" class="col-sm-2 col-sm-offset-1"  style=' display:none'>日平均风速:</label>
        <input type="text" name="text" class="MeanSpeed col-sm-2" style="text-align:left;display:none"/>

        <label for="WeaPhenomenaM" class="col-sm-2 col-sm-offset-1">天气现象（白天）:<span style="color:red">*</span></label>
        <select class="WeaPhenomenaM col-sm-2"name="text" style="text-align:left"><option></option></select>
        <label for="WeaPhenomenaN" class="col-sm-2 col-sm-offset-1">天气现象（晚上）:<span style="color:red">*</span></label>
        <select class="WeaPhenomenaN col-sm-2"name="text"style="text-align:left"><option></option></select>

        <label for="HazeM" class="col-sm-2 col-sm-offset-1">霾等级（日）:<span style="color:Red">*</span></label>
        <select  class="HazeM col-sm-2" name="text" style="text-align:left" id='MDJ1'>
        <option>无霾</option>
        <option>轻微</option>
        <option>轻度</option>
        <option>中度</option>
        <option>重度</option>
        <option>严重</option>
        </select>
        <label for="HazeA" class="col-sm-2 col-sm-offset-1 hidden">霾等级（下午）:<span style="color:Red">*</span></label>
        <select class="HazeN col-sm-2 hidden" name="text" style="text-align:left">
        <option>无霾</option>
        <option>轻微</option>
        <option>轻度</option>
        <option>中度</option>
        <option>重度</option>
        <option>严重</option>
        </select>

        <label for="rainM" class="col-sm-2 col-sm-offset-1 ">降雨量（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="rainM col-sm-2" style="text-align:left" id='JYL1'/>
        <label for="rainA" class="col-sm-2 col-sm-offset-1 hidden">降雨量（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="rainA col-sm-2 hidden" style="text-align:left"/>

        <label for="temperatureM" class="col-sm-2 col-sm-offset-1">平均温度（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="temperatureM col-sm-2" style="text-align:left" id='PJWD1'/>
        <label for="temperatureA" class="col-sm-2 col-sm-offset-1 hidden">平均温度(下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="temperatureA col-sm-2 hidden" style="text-align:left"/>

        <%--王斌  2017.5.2--%>
        <label for="speed" class="col-sm-2 col-sm-offset-1 ">风速：<span>*</span></label>
        <select class="speed col-sm-2" id="speed1" name="speed" style="text-align:left;"><option></option></select>
        <label for="speedM" class="col-sm-2 col-sm-offset-1 hidden">平均风速（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="speedM col-sm-2 hidden" style="text-align:left" id='PJFS1'/>
        <label for="speedA" class="col-sm-2 col-sm-offset-1 hidden">平均风速（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="speedA col-sm-2 hidden" style="text-align:left"/>

        <div style="float:left; width:100%">
        <label for="cloudM" class="col-sm-2 col-sm-offset-1" style=" float:left;">云量（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="cloudM col-sm-2" style="text-align:left;  float:left" id='YL1'/>
       </div>

        <label for="cloudA" class="col-sm-2 col-sm-offset-1 hidden" >云量（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text" class="cloudA col-sm-2 hidden" style="text-align:left"/>

         <label for="SixWindM" class="col-sm-2 col-sm-offset-1"  style=' display:none'>六级阵风等级（上午）:</label>
        <select  class="HazeM col-sm-2"name="text"style="text-align:left;display:none">
        <option>没有</option>
        <option>有</option>
        </select>
        <label for="SixWindA" class="col-sm-2 col-sm-offset-1"  style=' display:none'>六级阵风等级（下午）:</label>
        <select class="HazeN col-sm-2"name="text" style="text-align:left;display:none">
        <option>没有</option>
        <option>有</option>
        </select>

        <label for="UVRankM" class="col-sm-2 col-sm-offset-1"  style=' display:none'>UV等级（上午）:</label>
        <select class="UVRankM col-sm-2"name="text"style="text-align:left;display:none">
        <option>1级</option>
        <option>2级</option>
        <option>3级</option>
        <option>4级</option>
        <option>5级</option>
        </select>
        <label for="UVRankA" class="col-sm-2 col-sm-offset-1"  style=' display:none'>UV等级（下午）:</label>
        <select class="UVRankA col-sm-2"name="text" style="text-align:left;display:none">
        <option>1级</option>
        <option>2级</option>
        <option>3级</option>
        <option>4级</option>
        <option>5级</option>
        </select>
        
        <label for="O3_1HM" class="col-sm-2 col-sm-offset-1"  style=' display:none'>O3_1h（上午）:</label>
        <input type="text" name="text" class="O3_1HM col-sm-2"style="text-align:left;display:none"/>
        <label for="O3_1HA" class="col-sm-2 col-sm-offset-1"  style=' display:none'>O3_1h（下午）:</label>
        <input type="text" name="text" class="O3_1HA col-sm-2" style="text-align:left;display:none"/>
        
        <label for="O3_8HM" class="col-sm-2 col-sm-offset-1"  style=' display:none'>O3_8h（上午）:</label>
        <input type="text" name="text" class="O3_8HM col-sm-2"style="text-align:left;display:none"/>
        <label for="O3_8HA" class="col-sm-2 col-sm-offset-1"  style=' display:none'>O3_8h（下午）:</label>
        <input type="text" name="text" class="O3_8HA col-sm-2" style="text-align:left;display:none"/>
        
        <label for="PM2.5M" class="col-sm-2 col-sm-offset-1"  style=' display:none'>PM2.5（上午）:</label>
        <input type="text" name="text" class="PM2.5M col-sm-2"style="text-align:left;display:none"/>
        <label for="PM2.5A" class="col-sm-2 col-sm-offset-1"  style=' display:none'>PM2.5（下午）:</label>
        <input type="text" name="text" class="PM2.5A col-sm-2" style="text-align:left;display:none"/>
        
        <label for="PM10M" class="col-sm-2 col-sm-offset-1"  style=' display:none'>PM10（上午）:</label>
        <input type="text" name="text" class="PM10M col-sm-2"style="text-align:left;display:none"/>
        <label for="PM10A" class="col-sm-2 col-sm-offset-1"  style=' display:none'>PM10（下午）:</label>
        <input type="text" name="text" class="PM10A col-sm-2" style="text-align:left;display:none"/>

        <div style="width:100%;margin-top:10px;float:left">
        
            <div class="pollM col-sm-offset-1 col-sm-5" ">
                <label style="margin-left:5%;font-weight:normal;line-height:25px;" class="col-sm-4 ">首要污染物（日）:<span style="color:Red">*</span></label>
                   
                    <input class="easyui-combobox" id="pollutantM" "
                    data-options="
			            url:'combobox_data1.json',
		                method:'get',
                        width :100,
                        marginTop:1000,
			            valueField:'id',
			            textField:'text',
			            multiple:true,
			            panelHeight:'auto' "/>
                 
                <label style="line-height:25px;font-weight:normal;margin-left:9px;"for="aqi" class="col-sm-offset-1">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi" style="width:15%;" id='AQI1'/>
            </div>
          
            <div class="pollA col-sm-5 hidden">
                <label  class="col-sm-4 " style="margin-left:5%;line-height:25px;font-weight:normal;">首要污染物下午:<span style="color:Red">*</span></label>
               
                <input class="easyui-combobox"  id="pollutantA"
                 data-options="
			        url:'combobox_data1.json',
		            method:'get',
                    width :100,
			        valueField:'id',
			        textField:'text',
			        multiple:true,
			        panelHeight:'auto' "/>
        
                <label for="aqi" class="col-sm-offset-1"style="font-weight:normal;margin-left:9px;">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi" style="width:15%;"/>
            
            </div>
          
        </div>
    </div>
    
    <div class="content" id="table2" style="display:none">
        <label for="MaxTemperature" class="col-sm-2 col-sm-offset-1" style=" display:none">最高气温（日）:</label>
        <input type="text" name="text2" class="MaxTemperature col-sm-2"style="text-align:left; display:none"/>
        <label for="MinTemperature" class="col-sm-2 col-sm-offset-1" style=" display:none">最低气温（日）:</label>
        <input type="text" name="text2" class="MinTemperature col-sm-2" style="text-align:left; display:none"/>
        
        <label for="MaxTemperatureM" class="col-sm-2 col-sm-offset-1" style=" display:none">最高气温（上午）:</label>
        <input type="text" name="text2" class="MaxTemperatureM col-sm-2"style="text-align:left; display:none"/>
        <label for="MinTemperatureA" class="col-sm-2 col-sm-offset-1" style=" display:none">最低气温（下午）:</label>
        <input type="text" name="text2" class="MinTemperatureA col-sm-2" style="text-align:left; display:none"/>

        <label for="ReaHumidity" class="col-sm-2 col-sm-offset-1" style=" display:none">相对湿度（日）:</label>
        <input type="text" name="text2" class="ReaHumidity col-sm-2"style="text-align:left; display:none"/>
        <label for="ReaHumidityN" class="col-sm-2 col-sm-offset-1" style=" display:none">最低气温（晚上）:</label>
        <input type="text" name="text2" class="ReaHumidityN col-sm-2" style="text-align:left; display:none"/>

        <label for="ReaHumidityM" class="col-sm-2 col-sm-offset-1" style=" display:none">相对湿度（上午）:</label>
        <input type="text" name="text2" class="ReaHumidityM col-sm-2"style="text-align:left; display:none"/>
        <label for="ReaHumidityA" class="col-sm-2 col-sm-offset-1" style=" display:none">最低气温（下午）:</label>
        <input type="text" name="text2" class="ReaHumidityA col-sm-2" style="text-align:left; display:none"/>

        <label for="MeanPressure" class="col-sm-2 col-sm-offset-1" style=" display:none">日平均气压:</label>
        <input type="text" name="text2" class="MeanPressure col-sm-2"style="text-align:left; display:none"/>
        <label for="MeanSpeed" class="col-sm-2 col-sm-offset-1" style=" display:none">日平均风速:</label>
        <input type="text" name="text2" class="MeanSpeed col-sm-2" style="text-align:left; display:none"/>

        <label for="WeaPhenomenaM" class="col-sm-2 col-sm-offset-1">天气现象（白天）:<span style="color:red">*</span></label>
        <select class="WeaPhenomenaM col-sm-2" name="text2"style="text-align:left"><option></option></select>
        <label for="WeaPhenomenaN" class="col-sm-2 col-sm-offset-1">天气现象（晚上）:<span style="color:red">*</span></label>
        <select class="WeaPhenomenaN col-sm-2" name="text2" style="text-align:left"><option></option></select>

        <label for="HazeM" class="col-sm-2 col-sm-offset-1">霾等级（日）:<span style="color:Red">*</span></label>
        <select  class="HazeM col-sm-2" name="text2" style="text-align:left" id='MDJ2'>
        <option>无霾</option>
        <option>轻微</option>
        <option>轻度</option>
        <option>中度</option>
        <option>重度</option>
        <option>严重</option>
        </select>
        <label for="HazeA" class="col-sm-2 col-sm-offset-1 hidden">霾等级（下午）:<span style="color:Red">*</span></label>
        <select  class="HazeN col-sm-2 hidden" name="text2" style="text-align:left">
        <option>无霾</option>
        <option>轻微</option>
        <option>轻度</option>
        <option>中度</option>
        <option>重度</option>
        <option>严重</option>
        </select>

        <label for="rainM" class="col-sm-2 col-sm-offset-1">降雨量（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="rainM col-sm-2" style="text-align:left" id='JYL2'/>
        <label for="rainA" class="col-sm-2 col-sm-offset-1 hidden">降雨量（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="rainA col-sm-2 hidden" style="text-align:left"/>

        <label for="temperatureM" class="col-sm-2 col-sm-offset-1">平均温度（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="temperatureM col-sm-2" style="text-align:left" id='PJWD2'/>
        <label for="temperatureA" class="col-sm-2 col-sm-offset-1 hidden">平均温度（上午）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="temperatureA col-sm-2 hidden" style="text-align:left"/>

         <%--王斌  2017.5.2--%>
        <label for="speed" class="col-sm-2 col-sm-offset-1 ">风速：<span>*</span></label>
        <select class="speed col-sm-2" id="speed2" name="speed" style="text-align:left;"><option></option></select>
        <label for="speedM" class="col-sm-2 col-sm-offset-1 hidden">平均风速（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="speedM col-sm-2 hidden" style="text-align:left" id='PJFS2'/>
        <label for="speedA" class="col-sm-2 col-sm-offset-1 hidden">平均风速（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="speedA col-sm-2 hidden" style="text-align:left"/>
         <div style="float:left; width:100%">
           <label for="cloudM" class="col-sm-2 col-sm-offset-1">云量（日）：<span style="color:Red">*</span></label>
           <input type="text" name="text2" class="cloudM col-sm-2" style="text-align:left" id='YL2'/>
        </div>
        <label for="cloudA" class="col-sm-2 col-sm-offset-1 hidden">云量（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text2" class="cloudA col-sm-2 hidden" style="text-align:left"/>

         <label for="SixWindM" class="col-sm-2 col-sm-offset-1" style=" display:none">六级阵风等级（上午）:</label>
        <select class="HazeM col-sm-2" name="text2" style="text-align:left; display:none">
        <option>没有</option>
        <option>有</option>
        </select>
        <label for="SixWindA" class="col-sm-2 col-sm-offset-1" style=" display:none">六级阵风等级（下午）:</label>
        <select class="HazeN col-sm-2"name="text2" style="text-align:left; display:none">
        <option>没有</option>
        <option>有</option>
        </select>

        <label for="UVRankM" class="col-sm-2 col-sm-offset-1" style=" display:none">UV等级（上午）:</label>
        <select class="UVRankM col-sm-2" name="text2" style="text-align:left; display:none">
        <option>1级</option>
        <option>2级</option>
        <option>3级</option>
        <option>4级</option>
        <option>5级</option>
        </select>
        <label for="UVRankA" class="col-sm-2 col-sm-offset-1" style=" display:none">UV等级（下午）:</label>
        <select class="UVRankA col-sm-2"name="text2" style="text-align:left; display:none">
        <option>1级</option>
        <option>2级</option>
        <option>3级</option>
        <option>4级</option>
        <option>5级</option>
        </select>
        
        <label for="O3_1HM" class="col-sm-2 col-sm-offset-1" style=" display:none">O3_1h（上午）:</label>
        <input type="text" name="text2" class="O3_1HM col-sm-2"style="text-align:left; display:none"/>
        <label for="O3_1HA" class="col-sm-2 col-sm-offset-1" style=" display:none">O3_1h（下午）:</label>
        <input type="text" name="text2" class="O3_1HA col-sm-2" style="text-align:left; display:none"/>
        
        <label for="O3_8HM" class="col-sm-2 col-sm-offset-1" style=" display:none">O3_8h（上午）:</label>
        <input type="text" name="text2" class="O3_8HM col-sm-2"style="text-align:left; display:none"/>
        <label for="O3_8HA" class="col-sm-2 col-sm-offset-1" style=" display:none">O3_8h（下午）:</label>
        <input type="text" name="text2" class="O3_8HA col-sm-2" style="text-align:left; display:none"/>
        
        <label for="PM2.5M" class="col-sm-2 col-sm-offset-1" style=" display:none">PM2.5（上午）:</label>
        <input type="text" name="text2" class="PM2.5M col-sm-2"style="text-align:left; display:none"/>
        <label for="PM2.5A" class="col-sm-2 col-sm-offset-1" style=" display:none">PM2.5（下午）:</label>
        <input type="text" name="text2" class="PM2.5A col-sm-2" style="text-align:left; display:none"/>
        
        <label for="PM10M" class="col-sm-2 col-sm-offset-1" style=" display:none">PM10（上午）:</label>
        <input type="text" name="text2" class="PM10M col-sm-2"style="text-align:left; display:none"/>
        <label for="PM10A" class="col-sm-2 col-sm-offset-1" style=" display:none">PM10（下午）:</label>
        <input type="text" name="text2" class="PM10A col-sm-2" style="text-align:left; display:none"/>

         <div style="width:100%;margin-top:10px;float:left">
        
            <div class="pollM2 col-sm-offset-1 col-sm-5" ">
                <label style="margin-left:5%;font-weight:normal;line-height:25px;" class="col-sm-4 ">首要污染物（日）:<span style="color:Red">*</span></label>
                   
                    <input class="easyui-combobox" id="pollutantM2"
                    data-options="
			            url:'combobox_data1.json',
		                method:'get',
                        width :100,
                        marginTop:1000,
			            valueField:'id',
			            textField:'text',
			            multiple:true,
			            panelHeight:'auto' "/>
                 
                <label style="line-height:25px;font-weight:normal;margin-left:9px;"for="aqi2" class="col-sm-offset-1">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi2" style="width:15%;" id='AQI2'/>
            </div>
          
            <div class="pollA2 col-sm-5 hidden">
                <label  class="col-sm-4 " style="margin-left:5%;line-height:25px;font-weight:normal;">首要污染物下午:<span style="color:Red">*</span></label>
               
                <input class="easyui-combobox"  id="pollutantA2"
                 data-options="
			        url:'combobox_data1.json',
		            method:'get',
                    width :100,
			        valueField:'id',
			        textField:'text',
			        multiple:true,
			        panelHeight:'auto' "/>
        
                <label for="aqi2" class="col-sm-offset-1"style="font-weight:normal;margin-left:9px;">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi2" style="width:15%;"/>
            
            </div>
          
        </div>
    </div>

    <div class="content" id="table3" style="display:none">
        <label for="MaxTemperature" class="col-sm-2 col-sm-offset-1"  style=" display:none">最高气温（日）:</label>
        <input type="text" name="text3" class="MaxTemperature col-sm-2"style="text-align:left; display:none"/>
        <label for="MinTemperature" class="col-sm-2 col-sm-offset-1"  style=" display:none">最低气温（日）:</label>
        <input type="text" name="text3" class="MinTemperature col-sm-2" style="text-align:left; display:none"/>
        
        <label for="MaxTemperatureM" class="col-sm-2 col-sm-offset-1"  style=" display:none">最高气温（上午）:</label>
        <input type="text" name="text3" class="MaxTemperatureM col-sm-2"style="text-align:left; display:none"/>
        <label for="MinTemperatureA" class="col-sm-2 col-sm-offset-1"  style=" display:none">最低气温（下午）:</label>
        <input type="text" name="text3" class="MinTemperatureA col-sm-2" style="text-align:left; display:none"/>

        <label for="ReaHumidity" class="col-sm-2 col-sm-offset-1"  style=" display:none">相对湿度（日）:</label>
        <input type="text" name="text3" class="ReaHumidity col-sm-2"style="text-align:left; display:none"/>
        <label for="ReaHumidityN" class="col-sm-2 col-sm-offset-1"  style=" display:none">最低气温（晚上）:</label>
        <input type="text" name="text3" class="ReaHumidityN col-sm-2" style="text-align:left; display:none"/>

        <label for="ReaHumidityM" class="col-sm-2 col-sm-offset-1"  style=" display:none">相对湿度（上午）:</label>
        <input type="text" name="text3" class="ReaHumidityM col-sm-2"style="text-align:left; display:none"/>
        <label for="ReaHumidityA" class="col-sm-2 col-sm-offset-1"  style=" display:none">最低气温（下午）:</label>
        <input type="text" name="text3" class="ReaHumidityA col-sm-2" style="text-align:left; display:none"/>

        <label for="MeanPressure" class="col-sm-2 col-sm-offset-1"  style=" display:none">日平均气压:</label>
        <input type="text" name="text3" class="MeanPressure col-sm-2"style="text-align:left; display:none"/>
        <label for="MeanSpeed" class="col-sm-2 col-sm-offset-1"  style=" display:none">日平均风速:</label>
        <input type="text" name="text3" class="MeanSpeed col-sm-2" style="text-align:left; display:none"/>

        <label for="WeaPhenomenaM" class="col-sm-2 col-sm-offset-1">天气现象（白天）:<span style="color:red">*</span></label>
        <select class="WeaPhenomenaM col-sm-2" name="text3" style="text-align:left"><option></option></select>
        <label for="WeaPhenomenaN" class="col-sm-2 col-sm-offset-1">天气现象（晚上）:<span style="color:red">*</span></label>
        <select class="WeaPhenomenaN col-sm-2" name="text3" style="text-align:left"><option></option></select>

        <label for="HazeM" class="col-sm-2 col-sm-offset-1">霾等级（日）:<span style="color:Red">*</span></label>
        <select  class="HazeM col-sm-2" name="text3" style="text-align:left" id='MDJ3'>
        <option>无霾</option>
        <option>轻微</option>
        <option>轻度</option>
        <option>中度</option>
        <option>重度</option>
        <option>严重</option>
        </select>
        <label for="HazeA" class="col-sm-2 col-sm-offset-1 hidden">霾等级（下午）:<span style="color:Red">*</span></label>
        <select class="HazeN col-sm-2 hidden"name="text3" style="text-align:left">
        <option>无霾</option>
        <option>轻微</option>
        <option>轻度</option>
        <option>中度</option>
        <option>重度</option>
        <option>严重</option>
        </select>

        <label for="rainM" class="col-sm-2 col-sm-offset-1">降雨量（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="rainM col-sm-2" style="text-align:left" id='JYL3'/>
        <label for="rainA" class="col-sm-2 col-sm-offset-1 hidden">降雨量（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="rainA col-sm-2 hidden" style="text-align:left"/>

        <label for="temperatureM" class="col-sm-2 col-sm-offset-1">平均温度（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="temperatureM col-sm-2" style="text-align:left" id='PJWD3'/>
        <label for="temperatureA" class="col-sm-2 col-sm-offset-1 hidden">平均温度（上午）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="temperatureA col-sm-2 hidden" style="text-align:left"/>

         <%--王斌  2017.5.2--%>
        <label for="speed" class="col-sm-2 col-sm-offset-1 ">风速：<span>*</span></label>
        <select class="speed col-sm-2" id="speed3" name="speed" style="text-align:left;"><option></option></select>
        <label for="speedM" class="col-sm-2 col-sm-offset-1 hidden">平均风速（日）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="speedM col-sm-2 hidden" style="text-align:left" id='PJFS3'/>
        <label for="speedA" class="col-sm-2 col-sm-offset-1 hidden">平均风速（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="speedA col-sm-2 hidden" style="text-align:left"/>

         <div style="float:left; width:100%">
         <label for="cloudM" class="col-sm-2 col-sm-offset-1">云量（日）：<span style="color:Red">*</span></label>
         <input type="text" name="text3" class="cloudM col-sm-2" style="text-align:left" id='YL3'/>
        </div>

        <label for="cloudA" class="col-sm-2 col-sm-offset-1 hidden">云量（下午）：<span style="color:Red">*</span></label>
        <input type="text" name="text3" class="cloudA col-sm-2 hidden" style="text-align:left"/>

        <label for="SixWindM" class="col-sm-2 col-sm-offset-1"  style=" display:none">六级阵风等级（上午）:</label>
        <select  class="HazeM col-sm-2" name="text3" style="text-align:left; display:none">
        <option>没有</option>
        <option>有</option>
        </select>
        <label for="SixWindA" class="col-sm-2 col-sm-offset-1"  style=" display:none">六级阵风等级（下午）:</label>
        <select class="HazeN col-sm-2"name="text3" style="text-align:left; display:none">
        <option>没有</option>
        <option>有</option>
        </select>

        <label for="UVRankM" class="col-sm-2 col-sm-offset-1"  style=" display:none">UV等级（上午）:</label>
        <select class="UVRankM col-sm-2" name="text3" style="text-align:left; display:none">
        <option>1级</option>
        <option>2级</option>
        <option>3级</option>
        <option>4级</option>
        <option>5级</option>
        </select>
        <label for="UVRankA" class="col-sm-2 col-sm-offset-1"  style=" display:none">UV等级（下午）:</label>
        <select class="UVRankA col-sm-2"name="text3" style="text-align:left; display:none">
        <option>1级</option>
        <option>2级</option>
        <option>3级</option>
        <option>4级</option>
        <option>5级</option>
        </select>
        
        <label for="O3_1HM" class="col-sm-2 col-sm-offset-1"  style=" display:none">O3_1h（上午）:</label>
        <input type="text"name="text3" class="O3_1HM col-sm-2"style="text-align:left; display:none"/>
        <label for="O3_1HA" class="col-sm-2 col-sm-offset-1"  style=" display:none">O3_1h（下午）:</label>
        <input type="text" name="text3" class="O3_1HA col-sm-2" style="text-align:left; display:none"/>
        
        <label for="O3_8HM" class="col-sm-2 col-sm-offset-1" style=" display:none">O3_8h（上午）:</label>
        <input type="text" name="text3" class="O3_8HM col-sm-2"style="text-align:left; display:none"/>
        <label for="O3_8HA" class="col-sm-2 col-sm-offset-1" style=" display:none">O3_8h（下午）:</label>
        <input type="text" name="text3" class="O3_8HA col-sm-2" style="text-align:left; display:none"/>
        
        <label for="PM2.5M" class="col-sm-2 col-sm-offset-1" style=" display:none">PM2.5（上午）:</label>
        <input type="text" name="text3" class="PM2.5M col-sm-2"style="text-align:left; display:none"/>
        <label for="PM2.5A" class="col-sm-2 col-sm-offset-1" style=" display:none">PM2.5（下午）:</label>
        <input type="text" name="text3" class="PM2.5A col-sm-2" style="text-align:left; display:none"/>
        
        <label for="PM10M" class="col-sm-2 col-sm-offset-1" style=" display:none">PM10（上午）:</label>
        <input type="text" name="text3" class="PM10M col-sm-2"style="text-align:left; display:none"/>
        <label for="PM10A" class="col-sm-2 col-sm-offset-1" style=" display:none">PM10（下午）:</label>
        <input type="text" name="text3" class="PM10A col-sm-2" style="text-align:left; display:none"/>

        
           <%-- <div class="pollM" style="width:534px;float:left;margin-top:5px;margin-left:149px;">
                <label style="font-weight:normal;line-height:25px;" class="col-sm-3">首要污染物上午:<span style="color:Red">*</span></label>
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; 
                <input class="easyui-combobox" id="pollutantM3"
                data-options="
			        url:'combobox_data1.json',
		            method:'get',
                    width :100,
                    marginTop:1000,
			        valueField:'id',
			        textField:'text',
			        multiple:true,
			        panelHeight:'auto' "/>
        
                <label style="line-height:25px;font-weight:normal;margin-left:9px;"for="aqi" class="col-sm-offset-1">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi3" style="width:80px;"/>
            </div>
            <div class="pollA" style="width:600px;margin-top:-27px;margin-right:48px;float:right">
                <label  class="col-sm-3 " style="line-height:17px;font-weight:normal;margin-left:25px;">首要污染物下午:<span style="color:Red">*</span></label>
                &nbsp;&nbsp; &nbsp; &nbsp;
                <input class="easyui-combobox"  id="pollutantA3"
                 data-options="
			        url:'combobox_data1.json',
		            method:'get',
                    width :100,
			        valueField:'id',
			        textField:'text',
			        multiple:true,
			        panelHeight:'auto' "/>
        
                <label for="aqi" class="col-sm-offset-1"style="font-weight:normal;margin-left:10px;">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi3" style="width:80px;"/>
            </div>--%>
        <div style="width:100%;margin-top:10px;float:left">
        
            <div class="pollM3 col-sm-offset-1 col-sm-5" ">
                <label style="margin-left:5%;font-weight:normal;line-height:25px;" class="col-sm-4 ">首要污染物（日）:<span style="color:Red">*</span></label>
                   
                    <input class="easyui-combobox" id="pollutantM3" 
                    data-options="
			            url:'combobox_data1.json',
		                method:'get',
                        width :100,
                        marginTop:1000,
			            valueField:'id',
			            textField:'text',
			            multiple:true,
			            panelHeight:'auto' "/>
                 
                <label style="line-height:25px;font-weight:normal;margin-left:9px;"for="aqi3" class="col-sm-offset-1">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi3" style="width:15%;" id='AQI3'/>
            </div>
          
            <div class="pollA3 col-sm-5 hidden ">
                <label  class="col-sm-4 " style="margin-left:5%;line-height:25px;font-weight:normal;">首要污染物下午:<span style="color:Red">*</span></label>
               
                <input class="easyui-combobox"  id="pollutantA3"
                 data-options="
			        url:'combobox_data1.json',
		            method:'get',
                    width :100,
			        valueField:'id',
			        textField:'text',
			        multiple:true,
			        panelHeight:'auto' "/>
        
                <label for="aqi3" class="col-sm-offset-1"style="font-weight:normal;margin-left:9px;">AQI:<span style="color:Red">*</span></label>
                <input type="text" name="aqi3" style="width:15%;"/>
            
            </div>
          
        </div>
    </div>


    <input type="hidden" id="date" name="date" />
    </form>
</body>
</html>
