// JScript 文件
var oldE = "c1";
var oldEName = "PM25"
var curType = "环境";
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    if (typeName == "儿童感冒") {
        radioClickModuleII("c1", typeName);
    }
    else if (typeName == "成人感冒") {
        radioClickModuleII("c2", typeName);
    }
    else if (typeName == "老人感冒") {
        radioClickModuleII("c3", typeName);
    }
    else if (typeName == "儿童哮喘") {
        radioClickModuleII("c4", typeName);
    }
    else if (typeName == "COPD") {
        radioClickModuleII("c5", typeName);
    }
    else if (typeName == "中暑") {
        radioClickModuleII("c6", typeName);
    }
    else if (typeName == "重污染") {
        radioClickModuleII("c7", typeName);
    }
}
)

function doQueryChart(E) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var yLine = "";
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    var forecaster=logResult["Alias"];
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'GetChartLevels'),
        params: { fromDate: fromDate, toDate: toDate, type: typeName, forecaster: forecaster },
        success: function (response) {
            if (response.responseText != "") {
                var sps = response.responseText.toString().split("&");
                var i = 0;
                for (var v in sps) {
                    if (sps[i] != "") {
                        var result = Ext.util.JSON.decode(sps[i]);
                         RenderChart(result, i, "#container" + (i + 1) + "",yLine);
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
   // try {
        if (x.toString() != "null" || x.toString() != "NULL") {
            var floatTemp = parseFloat(x).toFixed(1);
            var floatValue = parseFloat(floatTemp);
            if (floatValue > 0)
                return floatValue;
            else
                return null;
        }
        else
            return null;
   // } catch (exception) {
    //    return null;
   // }

}

function RenderChart(result, lineName, contonter,yLINE) {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var chartName = "";
    if (lineName == 0)
        lineName = "中心城区";
    if (lineName == 1)
        lineName = "浦东新区";
    if (lineName == 2)
        lineName = "闵行区";
    if (lineName == 3)
        lineName = "宝山区";
    if (lineName == 4)
        lineName = "松江区";
    if (lineName == 5)
        lineName = "金山区";
    if (lineName == 6)
        lineName = "青浦区";
    if (lineName == 7)
        lineName = "奉贤区";
    if (lineName == 8)
        lineName = "嘉定区";
    if (lineName == 9)
        lineName = "崇明";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var CuaceArray = new Array(); var ChemArray = new Array();
    var CmaqArray = new Array(); var AqiArray = new Array();
    var CMAQ10Array = new Array();

    var indexData = 3;
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
            credits: { enabled: false },
            title: {
                text: LineName,
                style: {
                    fontSize: 18,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
            exporting: {
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
                    text: yLINE,
                    style: {
                        color: '#080808'
                    }
                },
                showEmpty: false,
                max:5,
                min: 0,
                tickInterval:1
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
                        radius: 3,
                        symbol: 'circle',
                        lineWidth: 1
                    }
                }, column: {
                    borderWidth: 0,
                    pointWidth: 15, //柱子宽度
                    dataLabels: {
                        style: {
                            fontSize: 11
                        },

                        enabled: false
                    }
                }

            },
            series: [

            {
                name: '预报等级',
                type: 'column',
                data: ChemArray,
                color: '#66cdaa'
            },
            {
                name: '实况等级',
                type: 'spline',
                data: CuaceArray,
                color: 'blue'
            }
           ]
        });
    }
    else { }
    //  alert("没有满足条件的信息!");
}

function clickQuery() {

    doQueryChart(typeName);
}

function radioClickModule(id, name) {
    var el = Ext.getDom(id);
    typeName = name;
    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        doQueryChart(name);
        if (oldE != "") {
            var oldObj = Ext.getDom(oldE);
            oldObj.className = "radioUnChecked";
            oldE = id;
            oldEName = name;
        }
    }
}

function radioClickModuleII(id, name) {
    var el = Ext.getDom(id);
    typeName = name;
    el.className = "radioChecked";
    doQueryChart(name);
    if (id != "c1") {
        var oldObj = Ext.getDom("c1");
        oldObj.className = "radioUnChecked";
        oldE = id;
        oldEName = name;
    }
}