// JScript 文件
var oldE = "c7";
var oldEName = "平均温度"

var oldEII = "x1";
var oldENameII = "24"

var oldEIII = "sw1";
var oldENameIII = "上午"

var curType = "气象";
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doQueryChart("平均温度", '24');

}
)

function doQueryChart(E, P) {

    //alert(oldENameIII);
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    curType = "环境";
    var yLine = "(ug/m³)";
    if (E == "平均温度" ||
        E == "最高温度" ||
        E == "最低温度" ||
        E == "风速" ||
        E == "湿度" ||
        E == "气压") {
        curType = "气象";
    }

    if (E == "平均温度" ||
          E == "最高温度" ||
          E == "最低温度") {
        yLine = "(℃)";
    }

    if (E == "风速") {
        yLine = "(m/s)";
    }
    if (E == "湿度") {
        yLine = "(%)";
    }
    if (E == "气压") {
        yLine = "(Pa)";
    }

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'GetChartElements'),
        params: { fromDate: fromDate, toDate: toDate, eName: E, type: curType, Period: P, duration: oldENameIII },
        success: function (response) {
            if (response.responseText != "") {
                var sps = response.responseText.toString().split("&");
                var i = 0;
                for (var v in sps) {
                    if (sps[i] != "") {
                        var result = Ext.util.JSON.decode(sps[i]);
                        RenderChart(result, i, "#container" + (i + 1) + "", yLine, E);
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
        if (floatValue > 0)
            return floatValue;
        else
            return null;
    }
    else
        return null;

}

function RenderChart(result, lineName, contonter, yLINE, eName) {
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

    var chartName = lineName + '(' + fromDate + '-' + toDate + ')' + eName + '数据';
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
                url: 'http://export.hcharts.cn',
                filename: chartName,
                width: 1200
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
                        radius: 3,
                        symbol: 'circle',
                        lineWidth: 1
                    }
                }
            },
            series: [
            {
                name: '实况',
                type: 'spline',
                data: CuaceArray
            }, {
                name: '模式预报',
                type: 'spline',
                data: ChemArray,
                color: '#E3170D'
            }, {
                name: '人工预报',
                type: 'spline',
                data: CmaqArray,
                color: '#191970'
            }
           ]
        });

    }
    else { }
    //  alert("没有满足条件的信息!");
}

function clickQuery() {

    doQueryChart(oldEName,'24');
}

function radioClickModule(id, name) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        doQueryChart(name, oldENameII);
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