// JScript 文件
var oldCity = "c5";
var oldCityName="朝阳市"
var olcCityCode="54324";
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doQueryChart("朝阳市",olcCityCode);
   }
)

function doQueryChart(citys,siteID) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.TimePollutansForecast', 'GetAQIChartDBForecast'),
        params: { fromDate: fromDate, toDate: toDate, city: citys,siteID:siteID },
        success: function (response) {
            if (response.responseText != "") {
                doQueryChartAQI(citys,siteID);
                var sps=response.responseText.toString().split("&");
                var i=0;
                for(var v in sps){
                    if(sps[i]!=""){
                      var result = Ext.util.JSON.decode(sps[i]);
                         if((i+1)!=7){
                              RenderChart(result, i, "#container"+(i+1)+"");
                          }
                    }
                    i++;
                }
            }
            else
              Ext.Msg.alert("提示", "没有满足条件的信息。");

              myMask.hide();
        },
        failure: function (response) {
           myMask.hide();
           Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function doQueryChartAQI(citys,siteID) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.TimePollutansForecast', 'GetAQIChartDBForecastAQI'),
        params: { fromDate: fromDate, toDate: toDate, city: citys,siteID:siteID },
        success: function (response) {
            if (response.responseText != "") {
                var sps=response.responseText.toString().split("&");
                var i=0;
                    if(sps[i]!=""){
                      var result = Ext.util.JSON.decode(sps[i]);
                       RenderChartAQI(result, i, "#container"+(7)+""); 
                    }
                    i++;
            }
            else
              Ext.Msg.alert("提示", "没有满足条件的信息。");

              myMask.hide();
        },
        failure: function (response) {
           myMask.hide();
           Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function ConvertToFloat(x) {
  if (x != "null") {
        var floatTemp = parseFloat(x).toFixed(1);
        var floatValue = parseFloat(floatTemp);
        if(floatValue>0)
            return floatValue;
        else
            return null;
    }
    else
        return null;
}



function RenderChartAQI(result,lineName,contonter) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var lineName = "AQI";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var PM25Array = new Array(); var PM10Array = new Array();
    var O38Array = new Array();  var SO2Array = new Array(); 
    var indexData = 4
    if (result != null) {
        var arrayTmp = new Array();
        if (result['0'] != null && result['0'] != "*") {
            indexData = 0;
            var Pb = result['0'].split('*');
            if (Pb.length == 3) {
                var Pbx = Pb[0].split('|');
                var Pby = Pb[1].split('|');
                var Pbz = Pb[2].split('|');
                for (var i = 0; i < Pbx.length; i++) {
                    var tmp = { x: Pbx[i] * 1000, y: ConvertToFloat(Pby[i]),z: Pbz[i]};
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pby[i]))
                        dblMax = ConvertToFloat(Pby[i]);

                    if (dblMin > ConvertToFloat(Pby[i]))
                        dblMin = ConvertToFloat(Pby[i]);
                    PM25Array[i] = tmp;
                }
            }
        }
        else
            PM25Array = arrayTmp;

        if (result['1'] != null && result['1'] != "*") {
            indexData = 1;
            var Fe = result['1'].split('*');
            if (Fe.length == 3) {
                var Fex = Fe[0].split('|');
                var Fey = Fe[1].split('|');
                var Fez = Fe[2].split('|');
                for (var i = 0; i < Fex.length; i++) {
                    var tmp = { x: Fex[i] * 1000, y: ConvertToFloat(Fey[i]),z: Fez[i]};
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Fey[i]))
                        dblMax = ConvertToFloat(Fey[i]);

                    if (dblMin > ConvertToFloat(Fey[i]))
                        dblMin = ConvertToFloat(Fey[i]);
                    PM10Array[i] = tmp;
                }
            }
        }
        else
            PM10Array = arrayTmp;


        if (result['2'] != null && result['2'] != "*") {
            indexData = 2;
            var Pd = result['2'].split('*');
            if (Pd.length == 3) {
                var Pdx = Pd[0].split('|');
                var Pdy = Pd[1].split('|');
                var Pdz = Pd[2].split('|');
                for (var i = 0; i < Pdx.length; i++) {
                    var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]),z:Pdz[i] };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pdy[i]))
                        dblMax = ConvertToFloat(Pdy[i]);

                    if (dblMin > ConvertToFloat(Pdy[i]))
                        dblMin = ConvertToFloat(Pdy[i]);
                    O38Array[i] = tmp;
                }
            }
        }
        else
            O38Array = arrayTmp;


       if (result['3'] != null && result['3'] != "*") {
            indexData = 3;
            var SO2 = result['3'].split('*');
            if (SO2.length == 3) {
                var SO2x = SO2[0].split('|');
                var SO2y = SO2[1].split('|');
                var SO2z = SO2[2].split('|');
                for (var i = 0; i < SO2x.length; i++) {
                    var tmp = { x: SO2x[i] * 1000, y: ConvertToFloat(SO2y[i]) ,z:SO2z[i]};
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(SO2y[i]))
                        dblMax = ConvertToFloat(SO2y[i]);

                    if (dblMin > ConvertToFloat(SO2y[i]))
                        dblMin = ConvertToFloat(SO2y[i]);
                    SO2Array[i] = tmp;
                }
            }
        }
        else
            SO2Array = arrayTmp;


       
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
        // create the chart
          Highcharts.setOptions({
          lang: {
            printChart: "打印图表",
            downloadJPEG: "下载JPEG 图片",
            downloadPDF: "下载PDF文档",
            downloadPNG: "下载PNG 图片",
            downloadSVG: "下载SVG 矢量图",
            exportButtonTitle: "导出图片"
          }
        });
        $(contonter).highcharts({
            chart: {
               type: 'spline'
            },
            colors: ['#00FF7F','#8A2BE2', '#A39480', '#f814e2' ],
            credits: { enabled: false },
            title: {
                text: LineName,
                style: {
                    fontSize: 18,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
             exporting:{
               url: 'http://export.hcharts.cn'
            },
            global: { useUTC: false },
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%Y-%m-%d',//鼠标移动到趋势线上时显示的日期格式 
                pointFormat:'<div style="color:{series.color}">  {series.name}:  {point.y} <h1 style="color:black">首要污染物：{point.z}</h1></div>',
                useHTML: true
            },
       
          xAxis: {
            type: 'datetime',
            tickInterval: 24 * 3600 * 1000,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%m月', this.value) + "<br/>" + Highcharts.dateFormat('%d日', this.value);
                    return tipMessage;
                }
            },
            offset: 0,
            lineColor: '#473C8B',
            lineWidth: 2,
            minorGridLineWidth: 0,
            minorTickInterval: 6 * 3600 * 1000,
            minorTickWidth: 1,
            minorTickLength: 5
        },
       legend: {
            backgroundColor: 'rgba(0,0,0,0)',
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
        },
         yAxis: { // Primary yAxis
                lineColor: '#473C8B',
                lineWidth: 2,
                style: {
                    color: '#473C8B'
                },
                title: {
                    text: '',
                    style: {
                        color: '#080808'
                    }
                },
                showEmpty: false
            },

            plotOptions: {
             series: {
                  connectNulls: true
                },
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
                    pointWidth: 8,
                    borderWidth: 0
                },
                line: {
                    stacking: 'normal',
                    pointWidth: 3,
                    borderWidth: 0,
                    lineWidth:0
                }
            },
            series: [
             {
                 name: '实况',
                 type: 'column',
                 data: SO2Array
             },{
                 name: '预报24时效',
                 type: 'spline',
                 data: PM25Array,
                 color:'green'
            }, {
                name: '预报48时效',
                type: 'spline',
                data: PM10Array,
                color:'#E3170D'
           },{
                name: '预报72时效',
                type: 'spline',
                data: O38Array,
                color:'yellow'
           }]
        });
    }
    else{}
      //  alert("没有满足条件的信息!");
}

function RenderChart(result,lineName,contonter) {
      var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var chartName = "";
    if (lineName == 0)
        lineName = "PM2.5";
    if (lineName == 1)
        lineName = "PM10";
    if (lineName == 2)
        lineName = "O3-8h";
     if (lineName ==3) 
        lineName = "SO2";
    if (lineName == 4) 
        lineName = "NO2";
    if (lineName == 5) 
        lineName = "CO";
    if (lineName == 6) 
        lineName = "AQI";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var PM25Array = new Array(); var PM10Array = new Array();
    var O38Array = new Array();  var SO2Array = new Array(); 
    var indexData = 4
    if (result != null) {
        var arrayTmp = new Array();
        if (result['0'] != null && result['0'] != "*") {
            indexData = 0;
            var Pb = result['0'].split('*');
            if (Pb.length == 2) {
                var Pbx = Pb[0].split('|');
                var Pby = Pb[1].split('|');
                for (var i = 0; i < Pbx.length; i++) {
                    var tmp = { x: Pbx[i] * 1000, y: ConvertToFloat(Pby[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pby[i]))
                        dblMax = ConvertToFloat(Pby[i]);

                    if (dblMin > ConvertToFloat(Pby[i]))
                        dblMin = ConvertToFloat(Pby[i]);
                    PM25Array[i] = tmp;
                }
            }
        }
        else
            PM25Array = arrayTmp;

        if (result['1'] != null && result['1'] != "*") {
            indexData = 1;
            var Fe = result['1'].split('*');
            if (Fe.length == 2) {
                var Fex = Fe[0].split('|');
                var Fey = Fe[1].split('|');
                for (var i = 0; i < Fex.length; i++) {
                    var tmp = { x: Fex[i] * 1000, y: ConvertToFloat(Fey[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Fey[i]))
                        dblMax = ConvertToFloat(Fey[i]);

                    if (dblMin > ConvertToFloat(Fey[i]))
                        dblMin = ConvertToFloat(Fey[i]);
                    PM10Array[i] = tmp;
                }
            }
        }
        else
            PM10Array = arrayTmp;


        if (result['2'] != null && result['2'] != "*") {
            indexData = 2;
            var Pd = result['2'].split('*');
            if (Pd.length == 2) {
                var Pdx = Pd[0].split('|');
                var Pdy = Pd[1].split('|');
                for (var i = 0; i < Pdx.length; i++) {
                    var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pdy[i]))
                        dblMax = ConvertToFloat(Pdy[i]);

                    if (dblMin > ConvertToFloat(Pdy[i]))
                        dblMin = ConvertToFloat(Pdy[i]);
                    O38Array[i] = tmp;
                }
            }
        }
        else
            O38Array = arrayTmp;


       if (result['3'] != null && result['3'] != "*") {
            indexData = 3;
            var SO2 = result['3'].split('*');
            if (SO2.length == 2) {
                var SO2x = SO2[0].split('|');
                var SO2y = SO2[1].split('|');
                for (var i = 0; i < SO2x.length; i++) {
                    var tmp = { x: SO2x[i] * 1000, y: ConvertToFloat(SO2y[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(SO2y[i]))
                        dblMax = ConvertToFloat(SO2y[i]);

                    if (dblMin > ConvertToFloat(SO2y[i]))
                        dblMin = ConvertToFloat(SO2y[i]);
                    SO2Array[i] = tmp;
                }
            }
        }
        else
            SO2Array = arrayTmp;


       
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
        // create the chart
          Highcharts.setOptions({
          lang: {
            printChart: "打印图表",
            downloadJPEG: "下载JPEG 图片",
            downloadPDF: "下载PDF文档",
            downloadPNG: "下载PNG 图片",
            downloadSVG: "下载SVG 矢量图",
            exportButtonTitle: "导出图片"
          }
        });
        $(contonter).highcharts({
            chart: {
               type: 'spline'
            },
            colors: ['#00FF7F','#8A2BE2', '#A39480', '#f814e2' ],
            credits: { enabled: false },
            title: {
                text: LineName,
                style: {
                    fontSize: 18,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
             exporting:{
               url: 'http://export.hcharts.cn'
            },
            global: { useUTC: false },
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%Y-%m-%d'//鼠标移动到趋势线上时显示的日期格式 
            },
       
          xAxis: {
            type: 'datetime',
            tickInterval: 24 * 3600 * 1000,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%m月', this.value) + "<br/>" + Highcharts.dateFormat('%d日', this.value);

                    return tipMessage;
                }
            },
            offset: 0,
            lineColor: '#473C8B',
            lineWidth: 2,
            minorGridLineWidth: 0,
            minorTickInterval: 6 * 3600 * 1000,
            minorTickWidth: 1,
            minorTickLength: 5
        },
       legend: {
            backgroundColor: 'rgba(0,0,0,0)',
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
        },
         yAxis: { // Primary yAxis
                lineColor: '#473C8B',
                lineWidth: 2,
                style: {
                    color: '#473C8B'
                },
                title: {
                    text: '（ug/m3）',
                    style: {
                        color: '#080808'
                    }
                },
                showEmpty: false
            },

            plotOptions: {
             series: {
                  connectNulls: true
                },
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
                    pointWidth: 8,
                    borderWidth: 0
                },
                line: {
                    stacking: 'normal',
                    pointWidth: 3,
                    borderWidth: 0,
                    lineWidth:0
                }
            },
            series: [
             {
                 name: '实况',
                 type: 'column',
                 data: SO2Array
             },{
                 name: '预报24时效',
                 type: 'spline',
                 data: PM25Array,
                 color:'green'
            }, {
                name: '预报48时效',
                type: 'spline',
                data: PM10Array,
                color:'#E3170D'
           },{
                name: '预报72时效',
                type: 'spline',
                data: O38Array,
                color:'yellow'
           }]
        });
    }
    else{}
      //  alert("没有满足条件的信息!");
}

function clickQuery(){
       doQueryChart(oldCityName,olcCityCode);
}

function radioClickModule(id,name,siteID) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked" ||el.className == "radioUnCheckedMax" ) {
        if(id=="c1"){
           el.className = "radioCheckedMax";
        }
       else{
           el.className = "radioChecked";
        }
        doQueryChart(name,siteID);
        if (oldCity != "") {
            var oldObj = Ext.getDom(oldCity);
            if(id=="c1"){
               oldObj.className = "radioUnChecked";
            }else{
              oldObj.className = "radioUnCheckedMax";
            }
            oldCity = id;
            oldCityName=name;
            olcCityCode=siteID;
        }
    }
}