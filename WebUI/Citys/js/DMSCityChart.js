// JScript 文件
var oldCity = "c1";
var oldCityName="上海市"
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doQueryChart("上海市");
    $(window).scroll(function() { 
         var top = $(window).scrollTop(); 
         $("#citypicker_container").css({ top: top + "px" }); 
     });
     Bind();


     $("#clos").click(function () {
         if ($("#citypicker_pro").is(":hidden")) {

             $("#clos").css('background-image', 'url(images/arrow-up.png)');
             $("#citypicker_pro").css('display', 'block');
             $("#citypicker_city").css('display', 'block');
             $("#citypicker_container").animate({ height: "348px" }, "slow", function () {
                 $("#citypicker_pro").css('display', 'block');
                 $("#citypicker_city").css('display', 'block');

             });
         }
         else {
             $("#citypicker_container").animate({ height: "25px" }, "slow", function () {
                 $("#citypicker_pro").css('display', 'none');
                 $("#citypicker_city").css('display', 'none');

             });
             $("#clos").css('background-image', 'url(images/arrow-down.png)');
         }
     })
 }
)

function doQueryChart(citys) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var cityName= Ext.getDom("city").value;
    var duration="10";
    if(Ext.getDom("day").checked)
         duration="7";
        
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.CitysForecast', 'GetAQIChartDBII'),
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
    var CMAQ10Array = new Array(); var CUACE9Array = new Array();var blk = new Array();

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


        if (result['4'] != null && result['4'] != "*") {
            indexData = 4;
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
                    AqiArray[i] = tmp;
                }
            }
        }
        else
            AqiArray = arrayTmp;




        if (result['5'] != null && result['5'] != "*") {
            indexData = 5;
            var blk = result['5'].split('*');
            if (blk.length == 2) {
                var Pdx = blk[0].split('|');
                var Pdy = blk[1].split('|');
                for (var i = 0; i < Pdx.length; i++) {
                    var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pdy[i]))
                        dblMax = ConvertToFloat(Pdy[i]);

                    if (dblMin > ConvertToFloat(Pdy[i]))
                        dblMin = ConvertToFloat(Pdy[i]);
                    blk[i] = tmp;
                }
            }
        }
        else
            blk = arrayTmp;

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
            colors: ['#8A2BE2', '#A39480', '#f814e2' ],
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

            borderWidth: 0
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
                        color: '#080808',
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
                  lineWidth: 3,
                  states: {
                      hover: {
                          lineWidth: 1
                      }
                  },
                  marker: {
                      enabled: true,
                      radius: 2,
                      symbol: 'circle',
                      lineWidth:1
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
                color: '#FF8C00'
           } ,{
                 name: 'CUACE',
                 type: 'line',
                 data: CuaceArray
            }
            ,{
                 name: 'CUACE9km',
                 type: 'spline',
                 data: AqiArray,
                 color: '#FF44AA'
             }
            , {
                name: '多模式最优集成',
                type: 'spline',
                data: blk,
                color: 'black',
                lineWidth:5
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
    var CMAQ10Array = new Array(); var blk = new Array();

    var indexData = 5;
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


        if (result['4'] != null && result['4'] != "*") {
            indexData = 4;
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
                    AqiArray[i] = tmp;
                }
            }
        }
        else
            AqiArray = arrayTmp;


        if (result['5'] != null && result['5'] != "*") {
            indexData = 5;
            var blk = result['5'].split('*');
            if (blk.length == 2) {
                var Pdx = blk[0].split('|');
                var Pdy = blk[1].split('|');
                for (var i = 0; i < Pdx.length; i++) {
                    var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pdy[i]))
                        dblMax = ConvertToFloat(Pdy[i]);

                    if (dblMin > ConvertToFloat(Pdy[i]))
                        dblMin = ConvertToFloat(Pdy[i]);
                    blk[i] = tmp;
                }
            }
        }
        else
            blk = arrayTmp;

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
            colors: ['#8A2BE2', '#A39480', '#f814e2' ],
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

            borderWidth: 0
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
                        color: '#080808',
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
                    lineWidth: 3,
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
                color: '#FF8C00'
            }, {
                name: 'CUACE9km',
                type: 'spline',
                data: AqiArray,
                color: '#FF44AA'
            }, {
                name: '多模式最优集成',
                type: 'spline',
                data: blk,
                color: 'black',
                lineWidth: 5
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