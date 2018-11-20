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
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'CompareChart'),
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
    var hour = 8;
    var title = "";
    if (obj == 0)
        title = "PM2.5";
    if (obj == 1)
        title = "PM10";
    if (obj == 2)
        title = "NO2";
    if (obj == 3)
        title = "O3-1h";
    if (obj == 4)
        title = "O3-8h";
    if (obj == 3 || obj == 4)
        hour = 24;
    var ArrayData = new Array();
    for (var j = 0; j <4; j++) {
        var arrayTmp = new Array();
        if (result[j] != null && result[j] != "**") {
            var cl = result[j].split('*');
            if (cl.length == 2) {
                var clx = cl[0].split('|');
                var cly = cl[1].split('|');
                for (var k = 0; k < clx.length; k++) {
                    var yValue = null;
                    if (cly[k] != "null" && cly[k] != "") {
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
            rangeSelectorFrom: '从',
            rangeSelectorTo: '到',
            rangeSelectorZoom: ''
        }
    });
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
        colors: ['#DBB0AF', '#66229C', '#9BBB59', '#558ED5'],
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
            formatter: function () {
                var dt;
                if (obj == 3 || obj == 4) {
                    dt = new Date(parseInt(this.points[0].x) - 24 * 3600 * 1000).add('d', 1) ;
                    var tipMessage = dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate() + "<br/>";
                    $.each(this.points, function () {
                        tipMessage = tipMessage + '<span style="color:' + this.series.color + '">' + this.series.name + '</span>:' + ":" + this.y;
                        tipMessage = tipMessage + "<br/>";
                    });
                }
                else {
                    dt = new Date(parseInt(this.points[0].x) - 8 * 3600 * 1000);

                    var hour = dt.getHours();
                    var tipHour = "上午";
                    if (hour == "06")
                        tipHour = "上午";
                    else if (hour == "16")
                        tipHour = "下午";
                    else if (hour == "23")
                        tipHour = "夜间";
                    var tipMessage = dt.getFullYear() + "-" + (dt.getMonth() + 1) + "-" + dt.getDate() + " " + tipHour + "<br/>";
                    $.each(this.points, function () {
                        tipMessage = tipMessage + '<span style="color:' + this.series.color + '">' + this.series.name + '</span>:' + ":" + this.y;
                        tipMessage = tipMessage + "<br/>";
                    });
                }
                return tipMessage;
            }
        },
        xAxis: {
            type: 'datetime',
            tickInterval: hour * 3600 * 1000,

            labels: {
                style: {
                    fontSize: '9px'
                },
                formatter: function () {
                    var hour;
                    if (obj == 3 || obj == 4) {
                        var startDate = new Date(this.value);
                        startDate1 = startDate.add('d', 0);
                        tipMessage = Highcharts.dateFormat('%m月', startDate1) + "<br/>" + Highcharts.dateFormat('%d日', startDate1);
                    }
                    else {
                        hour = Highcharts.dateFormat('%H', parseInt(this.value) - 8 * 3600 * 1000);
                        var tipMessage = "";
                        var tipHour = "上午";

                        if (hour == "08") {
                            if (this.isFirst) {
                                return "";
                            }
                            else {
                                tipHour = "下<br/>午";
                                tipMessage = tipHour + "<br/>";
                            }
                        }
                        else if (hour == "16") {
                            if (this.isLast) {
                                return "";
                            }
                            else {
                                tipHour = "夜<br/>间";
                                tipMessage = tipHour + "<br/>";
                            }
                        }
                        else if (hour == "00") {
                            tipHour = "上<br/>午";
                            var startDate = new Date(this.value);

                            startDate1 = startDate.add('d', 0)
                            
                            tipMessage = tipHour + "<br/>" + Highcharts.dateFormat('%m月', startDate1) + "<br/>" + Highcharts.dateFormat('%d日', startDate1);
                        }
                    }


                    return tipMessage;
                }
            }
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
                name: '气象部门',
                data: ArrayData[1]
            }, {
                type: 'line',
                name: '环保部门',
                data: ArrayData[2]
            }, {
                type: 'line',
                name: '两家合作',
                data: ArrayData[3]
            }]
    });

}