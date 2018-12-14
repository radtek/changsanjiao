Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTable();
}
)
//获取鼠标按下时的值
function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'ChinaChart'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    RenderChart(result[obj], obj);
                }

            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}
function RenderChart(result, obj) {
    var yTitle = "AQI";
    var title = "";
    if (obj == 0)
        title = "PM2.5";
    if (obj == 1)
        title = "PM10";
    if (obj == 2)
        title = "NO2";
    if (obj == 3) {
        title = "O3 1小时";
    }
    if (obj == 4) {
        title = "O3";
    }

    var ArrayData = new Array();
    for (var j = 0; j < 3; j++) {
        var arrayTmp = new Array();
        if (result[j] != null && result[j] != "*") {
            var cl = result[j].split('*');
            if (cl.length == 2) {
                var clx = cl[0].split('|');
                var cly = cl[1].split('|');
                for (var k = 0; k < clx.length; k++) {
                    var yValue = null;
                    if (cly[k] != "null") {
                        if (cly[k] != null) {
                            if (cly[k] != " ")
                                yValue = parseFloat(cly[k]);
                        }
                        var tmp = { x: clx[k] * 1000, y: yValue };
                        arrayTmp[k] = tmp;
                    }

                }
                ArrayData[j] = arrayTmp;
            }
        }
        else {
            ArrayData[j] = arrayTmp;
        }
    }
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
    // create the chart
    $('#container' + obj).highcharts({
        colors: ['#DBB0AF', '#9BBB59', '#558ED5'],
        credits: { enabled: false },
        title: {
            text: title
        },
        global: { useUTC: false },
        exporting: {
            width: 1400,
            sourceWidth: 1400,   //导出的文件宽度
            enabled: true
        },
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
            offset: 0
        },
        yAxis:
            [{
                title: { text: yTitle },
                offset: 0,
                lineWidth: 2,
                min: 0
            }],
        plotOptions: {
            line: {
                lineWidth: 2,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                },
                marker: {
                    enabled: true,
                    radius: 3,
                    lineWidth: 1,
                    symbol: 'circle'
                }
            },
            column: {
                borderWidth: 0
            }

        },
        legend: {
            backgroundColor: '#FFFFFF',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false,
            enabled: true,
            align: 'right',
            verticalAlign: 'top',
            x: -50,
            y: 0,
            layout: 'vertical',
            floating: true,
            borderWidth: 1,
            backgroundColor: '#FFFFFF'

        },
        series:
            [{
                type: 'column',
                name: '实况',
                data: ArrayData[0]
            }, {
                type: 'line',
                name: '主观预报',
                data: ArrayData[1]
            }, {
                type: 'line',
                name: 'WRF-Chem',
                data: ArrayData[2]
            }]
    });

}