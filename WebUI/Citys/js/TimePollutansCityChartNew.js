// JScript 文件
var oldCity = "c1";
var oldCityName="上海市"
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doQueryChart("上海市");
    $(window).scroll(function() { 
         var top = $(window).scrollTop(); 
         $("#citypicker_container").css({ top: (top-350) + "px" }); 
     }); 
     Bind();

     $("#clos").click(function(){

          if( $("#citypicker_pro").is(":hidden")){ 
		   
                    $("#clos").css('background-image','url(images/arrow-up.png)'); 
                       $("#citypicker_pro").css('display','block'); 
                           $("#citypicker_city").css('display','block'); 
                    $("#citypicker_container").animate({height:"348px"},"slow",function(){
                           $("#citypicker_pro").css('display','block'); 
                           $("#citypicker_city").css('display','block'); 
                        
                    }); 
                }
          else{
              $("#citypicker_container").animate({height:"25px"},"slow",function(){
                       $("#citypicker_pro").css('display','none'); 
                       $("#citypicker_city").css('display','none'); 
                        
               }); 
               $("#clos").css('background-image','url(images/arrow-down.png)');  
          }
	})
 }
)

function doQueryChart(citys) {
   //12-28 xuehui 
   $(":checkbox").each(function(){
      this.checked=true;
   });

    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var cityName= Ext.getDom("city").value;
    var duration="10";
    if(Ext.getDom("day").checked)
         duration="7";
        
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    //Ext.Ajax.timeout=900000; 
    Ext.Ajax.request({
        timeout: 600000,
        url: getUrl('MMShareBLL.DAL.CitysForecast', 'GetAQIChartDB'),
        params: { fromDate: fromDate, toDate: toDate, city: cityName,duration:duration },
        success: function (response) {
            if (response.responseText != "") {
                var sps=response.responseText.toString().split("&");
                var i=0;
                for(var v in sps){
                    if(sps[i]!=""){
                         var result = Ext.util.JSON.decode(sps[i]);
                         if(duration=="10")
                             RenderChart(result, i, "#container"+(i+1)+"");
                         else
                             RenderChartDay(result, i, "#container"+(i+1)+"");
                    }
                    i++;
                }
                RenderTable(0,Ext.util.JSON.decode(sps[0]));
                RenderTable(2,Ext.util.JSON.decode(sps[2]));//03
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

function RenderChart(result,lineName,contonter) {
      var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var chartName = "";
    if (lineName == 0)
        lineName = "PM2.5";
    if (lineName == 1)
        lineName = "PM10";
    if (lineName == 2)
        lineName = "O3";
    if (lineName == 3) 
        lineName = "NO2";
    if (lineName == 4) 
        lineName = "CO";
    if (lineName == 5) 
        lineName = "SO2";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var CuaceArray = new Array(); var ChemArray = new Array();
    var CmaqArray = new Array(); var AqiArray = new Array(); 
    var CMAQ10Array = new Array(); var Cuace9Array = new Array(); 
    var DMSArray = new Array(); 

    var indexData = 6;
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
                    CuaceArray[i] = tmp;
                }
            }
        }
        else
            CuaceArray = arrayTmp;

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
                    ChemArray[i] = tmp;
                }
            }
        }
        else
            ChemArray = arrayTmp;

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
                    CmaqArray[i] = tmp;
                }
            }
        }
        else
            CmaqArray = arrayTmp;

        if (result['3'] != null && result['3'] != "*") {
            indexData = 3;
            var Pd = result['3'].split('*');
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
                    CMAQ10Array[i] = tmp;
                }
            }
        }
        else
            CMAQ10Array = arrayTmp;


        if (result['5'] != null && result['5'] != "*") {
            indexData = 5;
            var Pd = result['5'].split('*');
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
                    AqiArray[i] = tmp;
                }
            }
        }
        else
            AqiArray = arrayTmp;


if (result['6'] != null && result['6'] != "*") {
            indexData = 5;
            var Pd = result['6'].split('*');
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
                    DMSArray[i] = tmp;
                }
            }
        }
        else
            DMSArray = arrayTmp;




            if (result['4'] != null && result['4'] != "*") {
            indexData = 6;
            var Pd = result['4'].split('*');
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
                    Cuace9Array[i] = tmp;
                }
            }
        }
        else
            Cuace9Array = arrayTmp;


            

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
                    fontSize: 22,
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
                xDateFormat: '%Y-%m-%d, %H'//鼠标移动到趋势线上时显示的日期格式 
            },
       
          xAxis: {
           title: {
           style: {
                   
                      fontSize: 22
                }
             },
            type: 'datetime',
            tickInterval: 24 * 3600 * 1000,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%m月', this.value) + "" + Highcharts.dateFormat('%d日', this.value);
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
            enabled: false,
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
                        color: '#080808' ,
                      fontSize: 22
                    }
                },
                showEmpty: false
            },

            plotOptions: {
             series: {
                  connectNulls: false
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
                },
                series: {
                    dataGrouping: {
                        enabled: false
                     },
                    marker: {
                     enabled: true,
                     fillColor:'#8A2BE2',
                     lineWidth:2,
                     lineColor:null
                  }
                }
            },
            series: [
             {
                 name: '实况',
                 type: 'column',
                 data: DMSArray
             },{
                 name: 'CUACE',
                 type: 'line',
                 data: CuaceArray
            }, {
                 name: 'WRF-CHEM',
                 type: 'spline',
                 data: ChemArray,
                 color:'#E3170D'
            }, {
                name: 'CMAQ',
                type: 'spline',
                data: CmaqArray,
                color:'#191970'
           }
           , {
                name: 'CMAQ10天',
                type: 'spline',
                data: CMAQ10Array,
                color:'#FF8C00'
           }, {
                name: 'CUACE9km',
                type: 'spline',
                data: Cuace9Array,
                color:'#FF44AA'
           }, {
                name: '多模式最优集成',
                type: 'spline',
                data: AqiArray,
                color:'black',
                lineWidth:4
           }]
        });
    }
    else{}
      //  alert("没有满足条件的信息!");
}



function RenderChartDay(result,lineName,contonter) {
      var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var chartName = "";
    if (lineName == 0)
        lineName = "PM2.5";
    if (lineName == 1)
        lineName = "PM10";
    if (lineName == 2)
        lineName = "O3";
    if (lineName == 3) 
        lineName = "NO2";
    if (lineName == 4) 
        lineName = "CO";
    if (lineName == 5) 
        lineName = "SO2";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var CuaceArray = new Array(); var ChemArray = new Array();
    var CmaqArray = new Array(); var AqiArray = new Array(); 
    var CMAQ10Array = new Array();  var Cuace9Array = new Array(); 
    var DMSArray = new Array(); 
    var indexData = 6;
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
                    CuaceArray[i] = tmp;
                }
            }
        }
        else
            CuaceArray = arrayTmp;

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
                    ChemArray[i] = tmp;
                }
            }
        }
        else
            ChemArray = arrayTmp;

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
                    CmaqArray[i] = tmp;
                }
            }
        }
        else
            CmaqArray = arrayTmp;

        if (result['3'] != null && result['3'] != "*") {
            indexData = 3;
            var Pd = result['3'].split('*');
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
                    CMAQ10Array[i] = tmp;
                }
            }
        }
        else
            CMAQ10Array = arrayTmp;


        if (result['5'] != null && result['5'] != "*") {
            indexData = 5;
            var Pd = result['5'].split('*');
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
                    AqiArray[i] = tmp;
                }
            }
        }
        else
            AqiArray = arrayTmp;



            if (result['6'] != null && result['6'] != "*") {
            indexData = 5;
            var Pd = result['6'].split('*');
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
                    DMSArray[i] = tmp;
                }
            }
        }
        else
            DMSArray = arrayTmp;

             if (result['4'] != null && result['4'] != "*") {
            indexData = 6;
            var Pd = result['4'].split('*');
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
                    Cuace9Array[i] = tmp;
                }
            }
        }
        else
            Cuace9Array = arrayTmp;

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
           scrollbar: {
        enabled: true
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
            enabled: false,
            align: 'right',
            verticalAlign: 'top',
            x: -50,
            y: 35,
            floating: true,
            borderWidth: 1
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
                  connectNulls: false
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
                 data: DMSArray
             },{
                 name: 'CUACE',
                 type: 'line',
                 data: CuaceArray
            }, {
                 name: 'WRF-CHEM',
                 type: 'spline',
                 data: ChemArray,
                 color:'#E3170D'
            }, {
                name: 'CMAQ',
                type: 'spline',
                data: CmaqArray,
                color:'#191970'
           }
           , {
                name: 'CMAQ10天',
                type: 'spline',
                data: CMAQ10Array,
                color:'#FF8C00'
           }, {
                name: 'CUACE9km',
                type: 'spline',
                data: Cuace9Array,
                color:'#FF44AA'
           }, {
                name: '多模式最优集成',
                type: 'spline',
                data: AqiArray,
                color:'black',
                lineWidth:4
           }]
        });
    }
    else{}
      //  alert("没有满足条件的信息!");
}

function clickQuery(){
       doQueryChart(oldCityName);
}

function radioClickModule(id,name) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        doQueryChart(name);
        if (oldCity != "") {
            var oldObj = Ext.getDom(oldCity);
            oldObj.className = "radioUnChecked";
            oldCity = id;
            oldCityName=name;
        }
    }
}

function toogleChart(obj,index,v) {
    var chart = $('#'+obj).highcharts();
    var series = chart.series[index];
    if (!v.checked)
        series.hide();
    else
        series.show();
}


function RenderTable(index,sps){


    var result = "<tr class='thTitle'><td>模式</td><td>平均偏差</td><td>均方根偏差</td><td>相关系数</td></tr>";
        if(sps["6"]){
        	var j=0;
        	for(var key in sps){
                if(key=="6") continue;
                if(parseInt(key)> j){
                    var tt=LoopNull("",j,parseInt(key)-1);
                    result += tt[1];
                    j=tt[0];
        			j++;
                }
        		if (key == j.toString() ) {
        			var data = FilterData(sps[6], sps[j]);
        			var skSelected = data.split("|")[0].split(",");
        			var mdSelected = data.split("|")[1].split(",");
        			result += "<tr><td>" + GetModel(j) + "</td><td>" + MeanBias(skSelected, mdSelected) + "</td><td>"  + RootMeanSqrError(skSelected, mdSelected) + "</td><td>"  + CorrelationCoe(skSelected, mdSelected) + "</td></tr>";
        			j++;
                }
               //else result +=GetNull("",result,j);

        	}
        }
        else{
        	for(var z=1;z<7;z++){
        		result +=GetNull("",z);
        	}
        }
   $("#tbl"+GetItem(index)).html(result);

}

function LoopNull(ret,j,compare){
    if(compare==j) return [j,GetNull(ret,j)];
    else return LoopNull(GetNull(ret,j),j+1,compare);
    
}
function GetNull(result,j){
    return result+"<tr><td>" + GetModel(j) + "</td><td>-</td><td>-</td><td>-</td></tr>";
}
function GetItem(itemNumber){
	var item;
	switch (itemNumber){
		case 0:
			item="pm25";
			break;
		case 1:
			item="pm10";
			break;
		case 2:
			item="o3";
			break;
		case 3:
			item="no2";
			break;
	}
	return item;
}

function GetModel(modelNumber){
	var model;
	switch (modelNumber){
	    case 0:
			model="cuace";
			break;
		case 1:
			model="wrf_chem";
			break;
		case 2:
			model="cmaq";
			break;
		case 3:
			model="cmaq10";
			break;
		case 4:
			model="cuace9km";
			break;
		case 5:
			model="多模式最优集成";
			break;
		case 6:
			model="实况";
			break;
	}
	return model;
}

//匹配实况数据与模式数据，data1实况数据，data2模式数据
function FilterData(data1, data2) {
    var time1 = data1.split("*")[0].toString().split("|");
    var value1 = data1.split("*")[1].toString().split("|");
    var time2 = data2.split("*")[0].toString().split("|");
    var value2 = data2.split("*")[1].toString().split("|");
	var sk=[];//实况
	var md=[];//模式
	for(var i=0;i<time1.length;i++){
		for(var j=0;j<time2.length;j++){
		    if (time1[i] == time2[j]) {
		        if (value1[i] != "NULL" && value2[j] != "NULL") {

		            sk.push(value1[i]);
		            md.push(value2[j]);
		        }
				break;
			}
        }

    }

if(sk.length>0){
    sk[sk.length - 1] = sk[sk.length - 1].replace("'", "");
}
if(md.length>0){
    md[md.length - 1] = md[md.length - 1].replace("'", "");
}
	return sk+"|"+md;
}

//字符串转float
function ConvertToFloat(x) {
  if (x != "NULL") {
        var floatValue = parseFloat(x);
        return floatValue;
    }
    else{
        return null;
    }
}

//Mean bias
function MeanBias(measured,predicted){
	var mea=measured;//实况数据，数组类型
	var pre=predicted;//预测数据，数组类型
	var sum=0.00;
	for(var a=0;a<mea.length;a++){
			sum+=(ConvertToFloat(pre[a])-ConvertToFloat(mea[a]));
	}
    var mb = sum / mea.length;
	return mb.toFixed(2);
}

//Root Mean Square Error
function RootMeanSqrError(measured,predicted){
	var mea=measured;//实况数据，数组类型
	var pre=predicted;//预测数据，数组类型
	var bias;
	var sum=0.00;
	var rmse;
	for(var b=0;b<mea.length;b++){
		bias=ConvertToFloat(pre[b])-ConvertToFloat(mea[b]);
		sum+=Math.pow(bias,2);
	}
	rmse=Math.pow(sum/mea.length,0.5);
	return rmse.toFixed(2);
}

//Correlation coefficient
function CorrelationCoe(measured,predicted){
	var mea=measured;//实况数据，数组类型
	var pre=predicted;//预测数据，数组类型
	var sumPre=0.00;
	var sumMea=0.00;
	var avgPre = 0.00;
	var avgMea = 0.00;
	var dvaluePre = 0.00;
	var dvalueMea = 0.00;
	var sumPreMea = 0.00;
	var sumSqrMea = 0.00;
	var sumSqrPre = 0.00;
	var sqrt;
	//实况和模式数据都存在，求平均
	for(var c=0;c<mea.length;c++){
		sumPre+=ConvertToFloat(pre[c]);
		sumMea+=ConvertToFloat(mea[c]);
	}
	avgPre=sumPre/mea.length;
	avgMea=sumMea/mea.length;
	//求相关系数
	for(var d=0;d<mea.length;d++){
		dvaluePre=(ConvertToFloat(pre[d])-avgPre);
        sumSqrPre+=Math.pow(dvaluePre,2);

		dvalueMea=(ConvertToFloat(mea[d])-avgMea);
		sumSqrMea+=Math.pow(dvalueMea,2);	
       
        sumPreMea+=(dvaluePre*dvalueMea);
	}
	sqrt=Math.pow(sumSqrPre*sumSqrMea,0.5);
	r = sumPreMea / sqrt;
	return r.toFixed(2);
}