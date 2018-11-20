var  lastTab="58367";
var lastTabQx="183";
var  lastTabMulti="temperature";
var lastTabQxMulti="8";
var lastStation="单站点多要素_0";
var text;
var windSpeed =new Array();
var id="Q0";
Ext.onReady(function(){
    initInputHighlightScript();
    dataShareQuery();
})
function tabStationClick(id){
    var lastParts = lastStation.split("_");
    var lastID=lastParts[1];
    var curParts = id.split("_");
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastStation);
    lastEl.className = "";
    
    lastEl.innerHTML = curEl.innerHTML.replace(id,lastStation).replace(curParts[0],lastParts[0]);
    
    curEl.className = "tabHighlight";
    curEl.innerHTML = curParts[0];
    var currentID=curParts[1];
    
    lastStation = id; 
    if(id=="单站点多要素_0")
    {
        hideEl("multStation");
        showEl("simpleStation"); 
    } 
    else 
    {
        hideEl("simpleStation");
        showEl("multStation");
//        HuanJingMulti();
//        HuanJingQiMulti();
    }
}
function tabClick(id){
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTab);
    lastEl.className = "shortdan";
    curEl.className = "shortdan_highlight";
    lastTab = id;
    if(Ext.getDom("container").className=="show")
        HuanJing();
    else 
       tableQuery();
}
function tabClickQx(id){
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTabQx);
    lastEl.className = "shortdanH";
    curEl.className = "shortdan_highlightH";
    lastTabQx = id;
    if(Ext.getDom("qxcontainer").className=="show")
        Qixiang();
    else 
       tableQueryH();

}
function tabClickMulti(id)
{
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTabMulti);
    lastEl.className = "shortdan";
    curEl.className = "shortdan_highlight";
    lastTabMulti = id;
    if(Ext.getDom("MultiContainer").className=="show")
        HuanJingMulti();
    else 
       tableQueryQ();
}
function tabClickMultiQX(id)
{
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTabQxMulti);
    lastEl.className = "shortdanH";
    curEl.className = "shortdan_highlightH";
    lastTabQxMulti = id;
    if(Ext.getDom("Multiqxcontainer").className=="show")
        HuanJingQiMulti();
    else 
       tableQueryQH();

}
function HuanJingMulti()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("MultiStationQixi");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlight")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','DataShareCompareMulti'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
            var result = Ext.util.JSON.decode(response.responseText);
            RenderChartMulti(result);
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });

}
function HuanJingQiMulti()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("MUtiStationHuan");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlightH")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    if(itemID=="5")
        text="mg/m3";
    else 
        text="ug/m3";
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','DataShareCompareMultiQi'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
            var result = Ext.util.JSON.decode(response.responseText);
            RenderChartMultiQI(result);
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });

}
function Qixiang()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curE2 = Ext.getDom("zdian");
    var itemIDQX;
    for(var i=1;i<curE2.children.length;i++)  
    {
      if(curE2.children[i].className== "shortdan_highlightH")
      {
          itemIDQX=curE2.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','DataShareCompareQX'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemIDQX}, 
    success: function(response){
        if(response.responseText!=""){
            var result = Ext.util.JSON.decode(response.responseText);
            RenderChartH(result);
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
}
function HuanJing()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("type_select1row");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlight")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','DataShareCompare'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
            var result = Ext.util.JSON.decode(response.responseText);
            RenderChart(result);
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });

}
function dataShareQuery()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("type_select1row");
    var curE2 = Ext.getDom("zdian");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlight")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    var itemIDQX;
    for(var i=1;i<curE2.children.length;i++)  
    {
      if(curE2.children[i].className== "shortdan_highlightH")
      {
          itemIDQX=curE2.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','DataShareCompare'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
            var result = Ext.util.JSON.decode(response.responseText);
            RenderChart(result);
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
 Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','DataShareCompareQX'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemIDQX}, 
    success: function(response){
        if(response.responseText!=""){
            var result = Ext.util.JSON.decode(response.responseText);
            RenderChartH(result);
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
    HuanJingQiMulti();
    HuanJingMulti();
    tableQueryQHClick();
    tableQueryQClick();
    tableQueryHClick();
    tableQueryClick();
}
function ConvertToFloat(x)
{
 var floatTemp=parseFloat(x).toFixed(2);
 var floatValue=parseFloat(floatTemp); 
 return floatValue;
 
}
function RenderChart(result)
{
    var lineTilte="气象参数变化";
    var dblMax = 0;
    var dblMin = 10000;
    var tempMin=10000;
    var tempArray=new Array();var Wind_DirArray=new Array();
    var air_pressureArray=new Array();var rain_sumArray=new Array();var relativehumidityArray=new Array();
    if (result!=null)
    {
        var temp=result['0'].split('*');
        if (temp.length==2){
            var clx=temp[0].split('|');
            var cly=temp[1].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y: ConvertToFloat(cly[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(cly[i]))
                    dblMax = ConvertToFloat(cly[i]);
                
                if (dblMin > ConvertToFloat(cly[i]))
                    dblMin = ConvertToFloat(cly[i]);
                tempArray[i] = tmp;
            }
        }
        var air_pressure=result['2'].split('*');
        if (air_pressure.length==2){
            var airx=air_pressure[0].split('|');
            var airy=air_pressure[1].split('|');
            for (var i = 0; i < airx.length; i++) {
                var tmp = { x: airx[i]*1000, y: ConvertToFloat(airy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(airy[i]))
                    dblMax = ConvertToFloat(airy[i]);
                
                if (dblMin > ConvertToFloat(airy[i]))
                    dblMin = ConvertToFloat(airy[i]);
                if (tempMin > ConvertToFloat(airy[i]))
                    tempMin= ConvertToFloat(airy[i]);
                air_pressureArray[i] = tmp;
            }
        }
        var rain_sum=result['3'].split('*');
        if(rain_sum[1]!="")
        {
            if (rain_sum.length==2){
                var rain_sumx=rain_sum[0].split('|');
                var rain_sumy=rain_sum[1].split('|');
                for (var i = 0; i < rain_sumx.length; i++) {                
                    var tmp = { x: rain_sumx[i]*1000, y: ConvertToFloat(rain_sumy[i])<0?"":ConvertToFloat(rain_sumy[i])};
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(rain_sumy[i]))
                        dblMax = ConvertToFloat(rain_sumy[i]);
                    
                    if (dblMin > ConvertToFloat(rain_sumy[i]))
                        dblMin = ConvertToFloat(rain_sumy[i]);
                    rain_sumArray[i] = tmp;
                }
            }
        }
        var relativehumidity=result['4'].split('*');
        if(relativehumidity[1]!="")
        {
            if (relativehumidity.length==2){
                var relativehumidityx=relativehumidity[0].split('|');
                var relativehumidityy=relativehumidity[1].split('|');
                for (var i = 0; i < relativehumidityx.length; i++) {
                    var tmp = { x: relativehumidityx[i]*1000, y: ConvertToFloat(relativehumidityy[i])<0?"":ConvertToFloat(relativehumidityy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(relativehumidityy[i]))
                        dblMax = ConvertToFloat(relativehumidityy[i]);
                    
                    if (dblMin > ConvertToFloat(relativehumidityy[i]))
                        dblMin = ConvertToFloat(relativehumidityy[i]);
                    relativehumidityArray[i] = tmp;
                }
            } 
        }
        var Wind_Dir=result['1'].split('*');
        if(Wind_Dir[1]!="")
        {
            if (Wind_Dir.length==3){
                var nox=Wind_Dir[0].split('|');
                var noy=Wind_Dir[1].split('|');
                var speedy=Wind_Dir[2].split('|');
                for (var i = 0; i < nox.length; i++) {
                var tmp = { x: nox[i]*1000, y: ConvertToFloat(speedy[i])<0?"":ConvertToFloat(speedy[i]) };
                var point;
                var speedIndex=Math.ceil(ConvertToFloat(speedy[i]));
                if(speedIndex%2!=0)
                   speedIndex=speedIndex+1;
                var windDirection=parseInt((ConvertToFloat(noy[i])-11.25)/22.5+1)%16;
                    point = {
                        x: nox[i]*1000,
    //                    name:"<span style='color:#990033'>风向风速:</span>"+speedy[i],
                        y: tempMin,
                        marker: {
                            symbol: "url(images/wind/" + speedIndex+"/"+windDirection+".png)"
                        }
                    };
                    Wind_DirArray[i] = point;
                    windSpeed[i]=tmp;
                }
            }  
        }  
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
		
		Highcharts.setOptions({
            lang:{
                rangeSelectorFrom:'从',
	            rangeSelectorTo:'到',
	            rangeSelectorZoom:''
	        }
        });
        // create the chart
$('#container').highcharts('Chart', {
                rangeSelector:
                {
                    buttons: [{
                        type: 'day',
	                    count: 2,
	                    text: '天'
                    }, {
	                    type: 'week',
	                    count: 1,
	                    text: '星期'
                    }, {
	                    type: 'month',
	                    count: 1,
	                    text: '月'
	                }, {
	                    type: 'all',
	                    text: '全部'
                    }],
                    selected: 0,
                    inputDateFormat:'%Y-%m-%d'
                },
                colors: ['#FF0000', '', '#990033', '#3366FF', '#663300', '#996600', 'Teal', 'Purple','Aqua', 'Lime', 'Yellow', 'gray'],
                credits:{enabled : false},
                title: {
                    text: lineTilte
                },
                zoomType: 'xy',
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                      shared: true,
                      xDateFormat: '%Y-%m-%d %H',
                      crosshairs: true
 
                },
                navigator:{
                    baseSeries:2,
                    series: {
	                    type: 'spline',
	                    color: '#4572A7',
	                    fillOpacity: 0.4,
	                    dataGrouping: {
		                    smoothed: true,
		                    enabled:false
	                    },
	                    lineWidth: 1,
	                    marker: {
		                    enabled: false
	                    },
	                    shadow: false
                    },
                    xAxis: {
	                    tickWidth: 0,
	                    lineWidth: 0,
	                    gridLineWidth: 1,
	                    tickPixelInterval: 100,
	                    labels: {
		                    align: 'center',
		                    x: 0,
		                    y: 0,
		                    formatter: function() {
                                return  Highcharts.dateFormat('%m月%d', this.value);
                            }
	                    }
                    }
                },
                xAxis: {
                    type: 'datetime',
                    labels: {
                        formatter: function() {
                            return  Highcharts.dateFormat('%d日 %H时', this.value);
                        }
                    },
                    dateTimeLabelFormats: {
                        second: '%H',
                        minute: '%H',
                        hour: '%H',
                        day: '%d日<br/>%H时',
                        week: '%d',
                        month: '%m',
                        year: '%Y'
                    }


                },
                yAxis: 
                [{
                        labels: {
                        formatter: function() {
                            return this.value;
                        },
                        style: {
                            color: '#990033'
                        }
                    },
                    title: {
                        text: '气压(hPa)',
                        style: {
                            color: '#990033'
                        }
                    },
                    lineWidth: 2,
                    showEmpty: false

                 }, {                      
                    title: {
                        text: '温度(°C)',
                        style: {
                            color: '#FF0000'
                        }
                    },
                    labels: {
                        formatter: function() {
                            return this.value;
                        },
                        style: {
                            color: '#FF0000'
                        }
                    },
                    lineWidth: 2,
                    showEmpty: false,
                    opposite: true
                }, { 
                    title: {
                        text: '相对湿度( %)',
                        style: {
                            color: '#663300'
                        }
                    },
                    labels: {
                        formatter: function() {
                            return this.value;
                        },
                        style: {
                            color: '#663300'
                        }
                    },
                    lineWidth: 2,
                    showEmpty: false,
                    opposite: true
                }, {           
                      title: {
                        text: '降雨量(mm)',
                        style: {
                            color: '#3366FF'
                        }
                    },
                    labels: {
                        formatter: function() {
                            return this.value;
                        },
                        style: {
                            color: '#3366FF'
                        }                          
                    },
                    lineWidth: 2,
                    showEmpty: false,
                    opposite: true

                }],
                plotOptions: {
                    spline: {
                        lineWidth: 2,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        marker: {
                            enabled: true,
                            radius: 0,
                            lineWidth: 1
                        }
                    },
                    column: {
                        stacking: 'normal',
                        pointWidth: 10,
                        borderWidth: 0
                    },
                    series:{
                        dataGrouping:{
                            enabled:false
                        }
                    }
                    
                },
                legend: {
                    style: {
                        left: 'auto',
                        bottom: 'auto',
                        right: 'auto',
                        top: 'auto'
                    },
                    backgroundColor: '#FFFFFF',
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false,
                    enabled: true,
                    align: 'right',
                    verticalAlign: 'top',
                    x: -50,
                    y: 35,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: '#FFFFFF'

                },
                series:
                [{
                    type: 'spline',
                    name: '温度',
                    data: tempArray,
                    yAxis: 1,
                    visible:false

                },{
                    type: 'spline',
                    name: '风向风速',
                    yAxis: 0,
                    data: Wind_DirArray
                },{
                    type: 'spline',
                    name: '气压',
                    yAxis: 0,
                    data: air_pressureArray
                },{
                    type: 'column',
                    name: '降雨量',
                    yAxis:3,
                    data: rain_sumArray,
                    visible:false
                },{
                    type: 'spline',
                    name: '相对湿度',
                    yAxis:2,
                    data: relativehumidityArray,
                    visible:false 
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartH(result)
{
    var lineTilte="大气污染物浓度变化";
    var dblMax = 0;
    var dblMin = 10000;
    var PM25Array=new Array();var PM10Array=new Array();
    var NO2Array=new Array();var O31Array=new Array();
    var O38Array=new Array();var SO2Array=new Array();var COArray=new Array();
    //Cl,NO3,SO4,Na,NH4,K,Mg,Ca, OC(热学),EC(热学),OC(光学),EC(光学),[PM2#5(ug/m3)]
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==2){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y: ConvertToFloat(cly[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(cly[i]))
                    dblMax = ConvertToFloat(cly[i]);
                
                if (dblMin > ConvertToFloat(cly[i]))
                    dblMin = ConvertToFloat(cly[i]);
                PM25Array[i] = tmp;
            }
        }
        var so=result['1'].split('*');
        if (so.length==2){
            var sox=so[0].split('|');
            var soy=so[1].split('|');
            for (var i = 0; i < sox.length; i++) {
                var tmp = { x: sox[i]*1000, y: ConvertToFloat(soy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(soy[i]))
                    dblMax = ConvertToFloat(soy[i]);
                
                if (dblMin > ConvertToFloat(soy[i]))
                    dblMin = ConvertToFloat(soy[i]);
                PM10Array[i] = tmp;
            }
        }
        var nh=result['2'].split('*');
        if (nh.length==2){
            var nhx=nh[0].split('|');
            var nhy=nh[1].split('|');
            for (var i = 0; i < nhx.length; i++) {
                var tmp = { x: nhx[i]*1000, y: ConvertToFloat(nhy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nhy[i]))
                    dblMax = ConvertToFloat(nhy[i]);
                
                if (dblMin > ConvertToFloat(nhy[i]))
                    dblMin = ConvertToFloat(nhy[i]);
                NO2Array[i] = tmp;
            }
        }
        var k=result['3'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                O31Array[i] = tmp;
            }
        }
        var mg=result['4'].split('*');
        if (mg.length==2){
            var mgx=mg[0].split('|');
            var mgy=mg[1].split('|');
            for (var i = 0; i < mgx.length; i++) {
                var tmp = { x: mgx[i]*1000, y: ConvertToFloat(mgy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(mgy[i]))
                    dblMax = ConvertToFloat(mgy[i]);
                
                if (dblMin > ConvertToFloat(mgy[i]))
                    dblMin = ConvertToFloat(mgy[i]);
                O38Array[i] = tmp;
            }
        }
        var ca=result['5'].split('*');
        if (ca.length==2){
            var cax=ca[0].split('|');
            var cay=ca[1].split('|');
            for (var i = 0; i < cax.length; i++) {
                var tmp = { x: cax[i]*1000, y: ConvertToFloat(cay[i]) };
                 //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(cay[i]))
                    dblMax = ConvertToFloat(cay[i]);
                
                if (dblMin > ConvertToFloat(cay[i]))
                    dblMin = ConvertToFloat(cay[i]);
                SO2Array[i] = tmp;
            }
        }
        var oc=result['6'].split('*');
        if (oc.length==2){
            var ocx=oc[0].split('|');
            var ocy=oc[1].split('|');
            for (var i = 0; i < ocx.length; i++) {
                var tmp = { x: ocx[i]*1000, y: ConvertToFloat(ocy[i]) };
                 //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ocy[i]))
                    dblMax = ConvertToFloat(ocy[i]);
                
                if (dblMin > ConvertToFloat(ocy[i]))
                    dblMin = ConvertToFloat(ocy[i]);
                COArray[i] = tmp;
            }
        }
        
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
		
		Highcharts.setOptions({
            lang:{
                rangeSelectorFrom:'从',
	            rangeSelectorTo:'到',
	            rangeSelectorZoom:''
	        }
        });
        // create the chart
        $('#qxcontainer').highcharts('Chart', {
                rangeSelector:
                {
                    buttons: [{
                        type: 'day',
	                    count: 2,
	                    text: '天'
                    }, {
	                    type: 'week',
	                    count: 1,
	                    text: '星期'
                    }, {
	                    type: 'month',
	                    count: 1,
	                    text: '月'
	                }, {
	                    type: 'all',
	                    text: '全部'
                    }],
                    selected: 0,
                    inputDateFormat:'%Y-%m-%d'
                },
                colors: [ '#990033', '#009900','Purple', '#0000FF', 'Black', '#996600', 'Teal','#FF0000','Aqua', 'Lime', 'Yellow', 'gray'],
                credits:{enabled : false},
                title: {
                    text: lineTilte,
                    verticalAlign:"bottom"

                },
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                      shared: true,
                      xDateFormat: '%Y-%m-%d %H',
                      crosshairs: true 
                },
                navigator:{
                    baseSeries:2,
                    series: {
	                    type: 'spline',
	                    color: '#4572A7',
	                    fillOpacity: 0.4,
	                    dataGrouping: {
		                    smoothed: true,
		                    enabled:false
	                    },
	                    lineWidth: 1,
	                    marker: {
		                    enabled: false
	                    },
	                    shadow: false
                    },
                    xAxis: {
	                    tickWidth: 0,
	                    lineWidth: 0,
	                    gridLineWidth: 1,
	                    tickPixelInterval: 100,
	                    labels: {
		                    align: 'center',
		                    x: 0,
		                    y: 0,
		                    formatter: function() {
                                return  Highcharts.dateFormat('%m月%d', this.value);
                            }
	                    }
                    }
                },
                xAxis: {
                    type: 'datetime',
                    labels: {
                        formatter: function() {
                            return  Highcharts.dateFormat('%d日 %H时', this.value);
                        }
                    },
                    dateTimeLabelFormats: {
                        second: '%H',
                        minute: '%H',
                        hour: '%H',
                        day: '%d日<br/>%H时',
                        week: '%d',
                        month: '%m',
                        year: '%Y'
                    }
                },
                yAxis: 
                [{
                    title: {
                        text: '污染物(ug/m3)',
                        style: {
                            color: '#990033'
                        }
                    },
                    labels: {
                        formatter: function() {
                            return this.value;
                        },
                        style: {
                            color: '#990033'
                        }
                    },
                    lineWidth: 2,
                    showEmpty: false
                    
                }, { 
                      labels: {
                        formatter: function() {
                            return this.value;
                        },
                        style: {
                            color: '#990033'
                        }
                    },
                    title: {
                        text: 'CO(mg/m3)',
                        style: {
                            color: '#990033'
                        }
                    },
                    lineWidth: 2,
                    showEmpty: false,
                    opposite: true

                }],
                plotOptions: {
                    spline: {
                        lineWidth: 2,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        marker: {
                            enabled: true,
                            radius: 0,
                            lineWidth: 1
                        }
                    },
                    column: {
                        stacking: 'normal',
                        pointWidth: 10,
                        borderWidth: 0
                    },
                    series:{
                        dataGrouping:{
                            enabled:false
                        }
                    }
                    
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'top',
                    x: -10,
                    y: 5,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: '#FFFFFF'
                },
                series:
                [{
                    type: 'spline',
                    name: 'PM2.5',
                    yAxis:0,
                    data: PM25Array
                },{
                    type: 'spline',
                    name: 'PM10',
                    yAxis:0,
                    data: PM10Array
                },{
                    type: 'spline',
                    name: 'NO2',
                    yAxis:0,
                    data: NO2Array,
                    visible:false
                },{
                    type: 'spline',
                    name: 'O3-1h',
                    yAxis:0,
                    data: O31Array,
                    visible:false
                },{
                    type: 'spline',
                    name: 'O3-8h',
                    yAxis:0,
                    data: O38Array,
                    visible:false
                },{
                    type: 'spline',
                    name: 'SO2',
                    yAxis:0,
                    data: SO2Array,
                    visible:false
                },{
                    type: 'spline',
                    name: 'CO',
                    yAxis:1,
                    data: COArray,
                    visible:false  
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartMulti(result)
{
    var lineTilte="气象参数变化";
    var dblMax = 0;
    var dblMin = 10000;
    var xjhArray=new Array();var mhArray=new Array();var pdArray=new Array();
    var bsArray=new Array();var sjArray=new Array();var jdArray=new Array();
    var qpArray=new Array();var jsArray=new Array();var fxArray=new Array();
    var cmArray=new Array();
    //Cl,NO3,SO4,Na,NH4,K,Mg,Ca, OC(热学),EC(热学),OC(光学),EC(光学),[PM2#5(ug/m3)]
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==2){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y: ConvertToFloat(cly[i])<0?0.0:ConvertToFloat(cly[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(cly[i]))
                    dblMax = ConvertToFloat(cly[i]);
                
                if (dblMin > ConvertToFloat(cly[i]))
                    dblMin = ConvertToFloat(cly[i]);
                xjhArray[i] = tmp;
            }
        }
        var no=result['1'].split('*');
        if (no.length==2){
            var nox=no[0].split('|');
            var noy=no[1].split('|');
            for (var i = 0; i < nox.length; i++) {
                var tmp = { x: nox[i]*1000, y: ConvertToFloat(noy[i])<0?0.0:ConvertToFloat(noy[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(noy[i]))
                    dblMax = ConvertToFloat(noy[i]);
                
                if (dblMin > ConvertToFloat(noy[i]))
                    dblMin = ConvertToFloat(noy[i]);
                mhArray[i] = tmp;
            }
        }
        var so=result['2'].split('*');
        if (so.length==2){
            var sox=so[0].split('|');
            var soy=so[1].split('|');
            for (var i = 0; i < sox.length; i++) {
                var tmp = { x: sox[i]*1000, y: ConvertToFloat(soy[i])<0?0.0:ConvertToFloat(soy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(soy[i]))
                    dblMax = ConvertToFloat(soy[i]);
                
                if (dblMin > ConvertToFloat(soy[i]))
                    dblMin = ConvertToFloat(soy[i]);
                pdArray[i] = tmp;
            }
        }
        var na=result['3'].split('*');
        if (na.length==2){
            var nax=na[0].split('|');
            var nay=na[1].split('|');
            for (var i = 0; i < nax.length; i++) {
                var tmp = { x: nax[i]*1000, y: ConvertToFloat(nay[i])<0?0.0:ConvertToFloat(nay[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nay[i]))
                    dblMax = ConvertToFloat(nay[i]);
                
                if (dblMin > ConvertToFloat(nay[i]))
                    dblMin = ConvertToFloat(nay[i]);
                bsArray[i] = tmp;
            }
        }
        var nh=result['4'].split('*');
        if (nh.length==2){
            var nhx=nh[0].split('|');
            var nhy=nh[1].split('|');
            for (var i = 0; i < nhx.length; i++) {
                var tmp = { x: nhx[i]*1000, y: ConvertToFloat(nhy[i])<0?0.0:ConvertToFloat(nhy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nhy[i]))
                    dblMax = ConvertToFloat(nhy[i]);
                
                if (dblMin > ConvertToFloat(nhy[i]))
                    dblMin = ConvertToFloat(nhy[i]);
                sjArray[i] = tmp;
            }
        }
        var k=result['5'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])<0?0.0:ConvertToFloat(ky[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                jdArray[i] = tmp;
            }
        }
        var nh=result['6'].split('*');
        if (nh.length==2){
            var nhx=nh[0].split('|');
            var nhy=nh[1].split('|');
            for (var i = 0; i < nhx.length; i++) {
                var tmp = { x: nhx[i]*1000, y: ConvertToFloat(nhy[i])<0?0.0:ConvertToFloat(nhy[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nhy[i]))
                    dblMax = ConvertToFloat(nhy[i]);
                
                if (dblMin > ConvertToFloat(nhy[i]))
                    dblMin = ConvertToFloat(nhy[i]);
                qpArray[i] = tmp;
            }
        }
        var k=result['7'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])<0?0.0:ConvertToFloat(ky[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                jsArray[i] = tmp;
            }
        }
        var k=result['8'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])<0?0.0:ConvertToFloat(ky[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                fxArray[i] = tmp;
            }
        }
        var k=result['9'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])<0?0.0:ConvertToFloat(ky[i]) };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                cmArray[i] = tmp;
            }
        }
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
		
		Highcharts.setOptions({
            lang:{
                rangeSelectorFrom:'从',
	            rangeSelectorTo:'到',
	            rangeSelectorZoom:''
	        }
        });
        // create the chart
        $('#MultiContainer').highcharts('Chart', {
                rangeSelector:
                {
                    buttons: [{
                        type: 'day',
	                    count: 2,
	                    text: '天'
                    }, {
	                    type: 'week',
	                    count: 1,
	                    text: '星期'
                    }, {
	                    type: 'month',
	                    count: 1,
	                    text: '月'
	                }, {
	                    type: 'all',
	                    text: '全部'
                    }],
                    selected: 0,
                    inputDateFormat:'%Y-%m-%d'
                },
                colors: ['#0000FF', '#009900', '#990033', 'Black', '#FF0000', '#996600', 'Teal', 'Purple','Aqua', 'Lime', 'Yellow', 'gray'],
                credits:{enabled : false},
                title: {
                    text: lineTilte
//                    margin:30
                },
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                      shared: true,
                      xDateFormat: '%Y-%m-%d %H',
                      crosshairs: true  
                },
                navigator:{
                    baseSeries:2,
                    series: {
	                    type: 'spline',
	                    color: '#4572A7',
	                    fillOpacity: 0.4,
	                    dataGrouping: {
		                    smoothed: true,
		                    enabled:false
	                    },
	                    lineWidth: 1,
	                    marker: {
		                    enabled: false
	                    },
	                    shadow: false
                    },
                    xAxis: {
	                    tickWidth: 0,
	                    lineWidth: 0,
	                    gridLineWidth: 1,
	                    tickPixelInterval: 100,
	                    labels: {
		                    align: 'center',
		                    x: 0,
		                    y: 0,
		                    formatter: function() {
                                return  Highcharts.dateFormat('%m月%d', this.value);
                            }
	                    }
                    }
                },
                xAxis: {
                    type: 'datetime',
                    labels: {
                        formatter: function() {
                            return  Highcharts.dateFormat('%d日 %H时', this.value);
                        }
                    },
                    dateTimeLabelFormats: {
                        second: '%H',
                        minute: '%H',
                        hour: '%H',
                        day: '%d日<br/>%H时',
                        week: '%d',
                        month: '%m',
                        year: '%Y'
                    }
                },
                yAxis: 
                [{
                    title: {text: 'ug/m3'},
                    offset: 0,
                    lineWidth: 2
                }],
                plotOptions: {
                    spline: {
                        lineWidth: 2,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        marker: {
                            enabled: true,
                            radius: 0,
                            lineWidth: 1
                        }
                    },
                    column: {
                        stacking: 'normal',
                        pointWidth: 10,
                        borderWidth: 0
                    },
                    series:{
                        dataGrouping:{
                            enabled:false
                        }
                    }
                    
                },
                legend: {
                   align: 'right',
                    verticalAlign: 'top',
                    x: -10,
                    y: 35,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: '#FFFFFF'
                },
                series:
                [{
                    type: 'spline',
                    name: '徐家汇',
                    data: xjhArray
                },{
                    type: 'spline',
                    name: '闵行',
                    data: mhArray,
                    visible:false
                },{
                    type: 'spline',
                    name: '浦东',
                    data: pdArray
                },{
                    type: 'spline',
                    name: '宝山',
                    data: bsArray,
                    visible:false
                },{
                    type: 'spline',
                    name: '松江',
                    data: sjArray,
                    visible:false
                },{
                    type: 'spline',
                    name: '嘉定',
                    data: jdArray,
                    visible:false
                },{
                    type: 'spline',
                    name: '青浦',
                    data: qpArray
                },{
                    type: 'spline',
                    name: '金山',
                    data: jsArray,
                    visible:false
                },{
                    type: 'spline',
                    name: '奉贤',
                    data: fxArray,
                    visible:false 
               },{
                    type: 'spline',
                    name: '崇明',
                    data: cmArray   
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartMultiQI(result)
{
    var lineTilte="大气污染物浓度变化";
    var dblMax = 0;
    var dblMin = 10000;
    var jaArray=new Array();var lwArray=new Array();var pdcsArray=new Array();
    var pdzjArray=new Array();var ptArray=new Array();var qpArray=new Array();
    var xhArray=new Array();var ypArray=new Array();var hkArray=new Array();
    var pdArray=new Array();
    //Cl,NO3,SO4,Na,NH4,K,Mg,Ca, OC(热学),EC(热学),OC(光学),EC(光学),[PM2#5(ug/m3)]
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==2){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y: ConvertToFloat(cly[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(cly[i]))
                    dblMax = ConvertToFloat(cly[i]);
                
                if (dblMin > ConvertToFloat(cly[i]))
                    dblMin = ConvertToFloat(cly[i]);
                jaArray[i] = tmp;
            }
        }
        var no=result['1'].split('*');
        if (no.length==2){
            var nox=no[0].split('|');
            var noy=no[1].split('|');
            for (var i = 0; i < nox.length; i++) {
                var tmp = { x: nox[i]*1000, y: ConvertToFloat(noy[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(noy[i]))
                    dblMax = ConvertToFloat(noy[i]);
                
                if (dblMin > ConvertToFloat(noy[i]))
                    dblMin = ConvertToFloat(noy[i]);
                lwArray[i] = tmp;
            }
        }
        var so=result['2'].split('*');
        if (so.length==2){
            var sox=so[0].split('|');
            var soy=so[1].split('|');
            for (var i = 0; i < sox.length; i++) {
                var tmp = { x: sox[i]*1000, y: ConvertToFloat(soy[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(soy[i]))
                    dblMax = ConvertToFloat(soy[i]);
                
                if (dblMin > ConvertToFloat(soy[i]))
                    dblMin = ConvertToFloat(soy[i]);
                pdcsArray[i] = tmp;
            }
        }
        var na=result['3'].split('*');
        if (na.length==2){
            var nax=na[0].split('|');
            var nay=na[1].split('|');
            for (var i = 0; i < nax.length; i++) {
                var tmp = { x: nax[i]*1000, y: ConvertToFloat(nay[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nay[i]))
                    dblMax = ConvertToFloat(nay[i]);
                
                if (dblMin > ConvertToFloat(nay[i]))
                    dblMin = ConvertToFloat(nay[i]);
                pdzjArray[i] = tmp;
            }
        }
        var nh=result['4'].split('*');
        if (nh.length==2){
            var nhx=nh[0].split('|');
            var nhy=nh[1].split('|');
            for (var i = 0; i < nhx.length; i++) {
                var tmp = { x: nhx[i]*1000, y: ConvertToFloat(nhy[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nhy[i]))
                    dblMax = ConvertToFloat(nhy[i]);
                
                if (dblMin > ConvertToFloat(nhy[i]))
                    dblMin = ConvertToFloat(nhy[i]);
                ptArray[i] = tmp;
            }
        }
        var k=result['5'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                qpArray[i] = tmp;
            }
        }
        var nh=result['6'].split('*');
        if (nh.length==2){
            var nhx=nh[0].split('|');
            var nhy=nh[1].split('|');
            for (var i = 0; i < nhx.length; i++) {
                var tmp = { x: nhx[i]*1000, y: ConvertToFloat(nhy[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(nhy[i]))
                    dblMax = ConvertToFloat(nhy[i]);
                
                if (dblMin > ConvertToFloat(nhy[i]))
                    dblMin = ConvertToFloat(nhy[i]);
                xhArray[i] = tmp;
            }
        }
        var k=result['7'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                ypArray[i] = tmp;
            }
        }
        var k=result['8'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                hkArray[i] = tmp;
            }
        }
        var k=result['9'].split('*');
        if (k.length==2){
            var kx=k[0].split('|');
            var ky=k[1].split('|');
            for (var i = 0; i < kx.length; i++) {
                var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i])};
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(ky[i]))
                    dblMax = ConvertToFloat(ky[i]);
                
                if (dblMin > ConvertToFloat(ky[i]))
                    dblMin = ConvertToFloat(ky[i]);
                pdArray[i] = tmp;
            }
        }
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
		
		Highcharts.setOptions({
            lang:{
                rangeSelectorFrom:'从',
	            rangeSelectorTo:'到',
	            rangeSelectorZoom:''
	        }
        });
        // create the chart
        $('#Multiqxcontainer').highcharts('Chart', {
                rangeSelector:
                {
                    buttons: [{
                        type: 'day',
	                    count: 2,
	                    text: '天'
                    }, {
	                    type: 'week',
	                    count: 1,
	                    text: '星期'
                    }, {
	                    type: 'month',
	                    count: 1,
	                    text: '月'
	                }, {
	                    type: 'all',
	                    text: '全部'
                    }],
                    selected: 0,
                    inputDateFormat:'%Y-%m-%d'
                },
                colors: ['#0000FF', '#009900', '#990033', 'Black', '#FF0000', '#996600', 'Teal', 'Purple','Aqua', 'Lime', 'Yellow', 'gray'],
                credits:{enabled : false},
                title: {
                    text: ''
//                    verticalAlign:"bottom"
//                    margin:30
                },
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                      shared: true,
                      xDateFormat: '%Y-%m-%d %H',
                      crosshairs: true 
                },
                xAxis: {
                    type: 'datetime',
                    labels: {
                        formatter: function() {
                            return  Highcharts.dateFormat('%d日 %H时', this.value);
                        }
                    },
                    dateTimeLabelFormats: {
                        second: '%H',
                        minute: '%H',
                        hour: '%H',
                        day: '%d日<br/>%H时',
                        week: '%d',
                        month: '%m',
                        year: '%Y'
                    }
                },
                yAxis: 
                [{
                    title: {text:text},
                    offset: 0,
                    lineWidth: 2
                }],
                plotOptions: {
                    spline: {
                        lineWidth: 2,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        marker: {
                            enabled: true,
                            radius: 0,
                            lineWidth: 1
                        }
                    },
                    column: {
                        stacking: 'normal',
                        pointWidth: 10,
                        borderWidth: 0
                    },
                    series:{
                        dataGrouping:{
                            enabled:false
                        }
                    }
                    
                },
//                legend: {
//                   align: 'right',
//                    verticalAlign: 'top',
//                    x: -10,
//                    y: 5,
//                    floating: true,
//                    borderWidth: 1,
//                    backgroundColor: '#FFFFFF'
//                },
                series:
                [{
                    type: 'spline',
                    name: '静安监测站',
                    data: jaArray
                },{
                    type: 'spline',
                    name: '卢湾师专附小',
                    data: lwArray
                },{
                    type: 'spline',
                    name: '浦东川沙',
                    data: pdcsArray
                },{
                    type: 'spline',
                    name: '浦东张江',
                    data: pdzjArray
                },{
                    type: 'spline',
                    name: '普陀监测站',
                    data: ptArray
                },{
                    type: 'spline',
                    name: '青浦淀山湖',
                    data: qpArray 
                },{
                    type: 'spline',
                    name: '徐汇上师大',
                    data: xhArray
                },{
                    type: 'spline',
                    name: '杨浦四漂',
                    data: ypArray
                },{
                    type: 'spline',
                    name: '虹口凉城',
                    data: hkArray 
               },{
                    type: 'spline',
                    name: '浦东监测站',
                    data: pdArray   
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function tableQueryClick()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("type_select1row");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlight")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','tableSimpleQuery'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
           Ext.getDom("tablecontainer").innerHTML = response.responseText;
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
}
function tableQuery()
{  
    hideEl("container");
    showEl("tablecontainer");
    tableQueryClick();

}
function quxianQuery()
{
    hideEl("tablecontainer");
    showEl("container");
}
function tableQueryHClick()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("zdian");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlightH")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','tableSimpleQueryH'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
           Ext.getDom("qxcontainerTable").innerHTML = response.responseText;
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
}
function tableQueryH()
{  
    hideEl("qxcontainer");
    showEl("qxcontainerTable");
    tableQueryHClick();

}
function quxianQueryH()
{
    hideEl("qxcontainerTable");
    showEl("qxcontainer");
}
function tableQueryQClick()
{
   var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("MultiStationQixi");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlight")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','tableSimpleQueryQ'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
           Ext.getDom("MultiContainerQ").innerHTML = response.responseText;
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
}
function tableQueryQ()
{  
    hideEl("MultiContainer");
    showEl("MultiContainerQ");
    tableQueryQClick();

}
function quxianQueryQ()
{
    hideEl("MultiContainerQ");
    showEl("MultiContainer");
}
function tableQueryQHClick()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var curEl = Ext.getDom("MUtiStationHuan");
    var itemID;
    for(var i=1;i<curEl.children.length;i++)  
    {
      if(curEl.children[i].className== "shortdan_highlightH")
      {
          itemID=curEl.children[i].id;
          break;
      }
    }
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.SiteData','tableSimpleQueryQH'),
    params: { fromDate: fromDate,toDate:toDate,itemID:itemID}, 
    success: function(response){
        if(response.responseText!=""){
           Ext.getDom("MultiContainerQH").innerHTML = response.responseText;
        }
        else
            Ext.Msg.alert("提示", "没有满足条件的信息。"); 
    },
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });
}
function tableQueryQH()
{  
    hideEl("Multiqxcontainer");
    showEl("MultiContainerQH");
    tableQueryQHClick();

}
function quxianQueryQH()
{
    hideEl("MultiContainerQH");
    showEl("Multiqxcontainer");
}
function mouseOver(obj)
{
    if(obj!=null)
    {
       obj.style.backgroundColor = "#badbff";
    }
}
function mouseOut(obj)
{
 if(obj!=null)
    obj.style.backgroundColor = "#fff";
}
function menuClick(el)
{
  Ext.getDom(id).style.backgroundColor = "#fff";
  id=el;
  Ext.getDom(el).style.backgroundColor = "#badbff";
  var station=lastStation;
  var curEl;
  var itemID;
  var itemIndex;
  var fromDate = Ext.getDom("H00").value;
  var toDate = Ext.getDom("H01").value;
  
  if(station=="单站点多要素_0")
  {
    if(el=="Q0")
    {   
        itemIndex="1";
        curEl = Ext.getDom("type_select1row");
        for(var i=1;i<curEl.children.length;i++)  
        {
          if(curEl.children[i].className== "shortdan_highlight")
          {
              itemID=curEl.children[i].id;
              break;
          }
        }
    }
    else 
    {
        itemIndex="2";
        curEl = Ext.getDom("zdian");
        for(var i=1;i<curEl.children.length;i++)  
        {
          if(curEl.children[i].className== "shortdan_highlightH")
          {
              itemID=curEl.children[i].id;
              break;
          }
        }
    }
  }
  else 
  {
    if(el=="Q0")
    {
        itemIndex="3";
        curEl = Ext.getDom("MultiStationQixi");
        for(var i=1;i<curEl.children.length;i++)  
        {
          if(curEl.children[i].className== "shortdan_highlight")
          {
              itemID=curEl.children[i].id;
              break;
          }
        }
    }
    else 
    {
        itemIndex="4";
        curEl = Ext.getDom("MUtiStationHuan");
        for(var i=1;i<curEl.children.length;i++)  
        {
          if(curEl.children[i].className== "shortdan_highlightH")
          {
              itemID=curEl.children[i].id;
              break;
          }
        }
    }
  }
    var content=fromDate+"|"+toDate+"|"+itemID+"|"+itemIndex;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("btnExport").click();
    var obj =Ext.getDom("menu");
    obj.style.display= "none";
}
function exportSiteData()
{
  var station=lastStation;
  var obj =Ext.getDom("menu");
  var temp =Ext.getDom("ExportData");
  obj.style.left = getElementLeft(temp,obj.parent)-5+ "px";
  obj.style.top =  getElementTop(temp,obj.parent) + temp.offsetHeight+10+ "px";
  obj.style.display= "block";
  obj.style .zIndex =100;
}
