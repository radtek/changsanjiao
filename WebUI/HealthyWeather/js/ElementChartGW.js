// JScript 文件
var oldE = "c7";
var oldEII = "x1";
var oldENameII = "24"

var oldEIII = "sw1";
var oldENameIII = "上午"
var oldName = "PM25";
var curType = "气象";
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    $("#c1").addClass('radioChecked');
    doQueryChart("PM25", '24');
  }
)

function doQueryChart(E, P) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    curType = "环境";
    var yLine = "(ug/m³)";
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeatherGW', 'GetChartElements'),
        //url: "ElementChartGW.aspx/GetChartElements",
        timeout: 10000000,
        params: { fromDate: fromDate, toDate: toDate, eName: E, type: curType, Period: P, duration: oldENameIII },
        success: function (response) {
            if (response.responseText != "") {
                //console.log(response.responseText);
                var sps = response.responseText.toString().split("&");
                RenderChart(sps, E);
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
        if (floatValue > 0)
            return floatValue;
        else
            return null;
    }
    else
        return null;

}

function RenderChart(sps, eName) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;

    // var chartName = lineName + '(' + fromDate + '-' + toDate + ')' + eName + '数据';
    var yLINE = "ug/m3";
    if (eName == "O3") {
        yLINE = "PPB";
    }
    var LineName = eName;
    var dblMax = 0;
    var dblMin = 10000;
    var sites=new Array(15);

    var index = 0;
    for (var v in sps) {
        if (sps[index] != "") {
            var result = Ext.util.JSON.decode(sps[index]);
    
            //=============================== 
            if (result != null) {
                var arrayTmp = new Array();
                sites[index] = new Array();
                if (result['0'] != null && result['0'] != "*") {
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
                            sites[index][i] = tmp;
                        }
                    }
                }
                else
                    sites[index] = arrayTmp;
             }
            //===============================
           }
             index++;
       }

        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        console.log("{"+sites[0].toString()+"}");
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
      //  Highcharts.stockChart('container1',{
        $("#container1").highcharts({
            chart: {
                type: 'spline'
            },
            credits: { enabled: false },
            title: {
                text: "大气自动站"+LineName+"监测数据",
                style: {
                    fontSize: 18,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
            exporting: {
                url: 'http://export.hcharts.cn',
                filename: "自动站数据",
                width: 1200
            },
            global: { useUTC: false },
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%Y-%m-%d %H:%M'
            },

            xAxis: {
                type: 'datetime',
                showLastLabel:false,
               // tickInterval: 60 * 60 * 1000,//间隔值 
                tickPixelInterval:60,
                labels: {
                    formatter: function () {
                        var tipMessage = "";
                        tipMessage = Highcharts.dateFormat('%Y-%m-%d,%H', this.value)
                        return tipMessage;
                    },
                    rotation: 30,
                    x: 20,
                    y:30
                },
                offset: 0,
                lineColor: '#473C8B',
                lineWidth: 2,
                gridLineWidth: 0,
                //minorGridLineWidth: 0,
                //minorTickWidth: 2,
                //minorTickLength: 10
            },

            legend: {

                borderWidth: 0
            },
            yAxis: { // Primary yAxis
                lineColor: '#473C8B',
                gridLineWidth: 0,
                lineWidth: 2,
                style: {
                    color: '#473C8B'
                },
                min: 0,
                title: {
                    text: yLINE,
                    style: {
                        color: '#080808'
                    }
                },
                minorGridLineWidth: 0,
                minorTickWidth: 1,
                minorTickLength: 10
                //showEmpty: false
            },

            plotOptions: {
                series: {
                    marker: {
                        radius: 1  //曲线点半径，默认是4
                       // symbol: 'diamond' //曲线点类型："circle", "square", "diamond", "triangle","triangle-down"，默认是"circle"
                    },
                    turboThreshold: 2000
                }
            },
            series: [
            {
                name: '徐家汇',
                type: 'spline',
                data: sites[0],
                color: '#ff00ff'
            }, {
                name: '宝山',
                type: 'spline',
                data: sites[1],
                color: '#00ffff'
            }, {
                name: '金山',
                type: 'spline',
                data: sites[2],
                color: '#00ff00'
            }, {
                name: '崇明',
                type: 'spline',
                data: sites[3],
                color: '#e06666'
            }, {
                name: '浦东',
                type: 'spline',
                data: sites[4],
                color: '#ffff00'
            }, {
                name: '东滩',
                type: 'spline',
                data: sites[5],
                color: '#3d85c6'
            }, {
                name: '佘山岛',
                type: 'spline',
                data: sites[6],
                color: '#c27ba0'
            }, {
                name: '小洋山',
                type: 'spline',
                data: sites[7],
                color: '#45818e'
            }, {
                name: '佘山天文台',
                type: 'spline',
                data: sites[8],
                color: '#7f6000'
            }, {
                name: '世博园',
                type: 'spline',
                data: sites[9],
                color: '#000000'
            }, {
                name: '临港',
                type: 'spline',
                data: sites[10],
                color: '#3d85c6'
            }, {
                name: '迪斯尼',
                type: 'spline',
                data: sites[11],
                color: '#6fa8dc'
            }, {
                name: '中心大厦',
                type: 'spline',
                data: sites[12],
                color: '#7f6000'
            }, {
                name: '嘉定',
                type: 'spline',
                data: sites[13],
                color: '#8e7cc3'
            }, {
                name: '奉贤',
                type: 'spline',
                data: sites[14],
                color: '#274e13'
            }
           ]
        });


    //  alert("没有满足条件的信息!");
}

function clickQuery() {
    doQueryChart(oldName, '24');
}

function radioClickModule(id, name) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked") {
        $(".radioChecked").removeClass("radioChecked");
        el.className = "radioChecked";
        doQueryChart(name, oldENameII);
        if (oldE != "") {
            var oldObj = Ext.getDom(oldE);
            oldObj.className = "radioUnChecked";
            oldE = id;
            oldName = name;
        }
    }
}

function radioClickModuleII(id, name) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        doQueryChart(oldEName, name);
        if (oldE != "") {
            var oldObj = Ext.getDom(oldEII);
            oldObj.className = "radioUnChecked";
            oldEII = id;
            oldENameII = name;
        }
    }
}

function radioClickModuleIII(id, name) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
       
        if (oldEIII != "") {
            var oldObj = Ext.getDom(oldEIII);
            oldObj.className = "radioUnChecked";
            oldEIII = id;
            oldENameIII = name;
        }
        doQueryChart(oldEName, oldENameII);
    }
}