Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();

    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");
    $("#PO_year").val(new Date().getFullYear());

    $("#PO_docDate").val(getFormatDate("") + "17时");
    $("#PO_month").val(new Date().getMonth());
    InitContent();
    $("#productName").val("EnvForeScore");

    $("#svgSubmit").click(function () {
        var chart_PM25 = $("#container0").highcharts();
        var chart_PM10 = $("#container1").highcharts();
        var chart_NO2 = $("#container2").highcharts();
        //        var svg1 = chart_PM25.getSVG();
        //        var svg2 = chart_PM10.getSVG();
        //        var svg3 = chart_NO2.getSVG();
        var svg1 = chart_PM25.getSVG().replace(/</g, '$').replace(/>/g, '@');
        var svg2 = chart_PM10.getSVG().replace(/</g, '$').replace(/>/g, '@');
        var svg3 = chart_NO2.getSVG().replace(/</g, '$').replace(/>/g, '@');
        var svg = svg1 + "%" + svg2 + "%" + svg3;
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveEnvForeScoreReport'),
            params: { wordTempContent: getWordContent(), productName: "EnvForeScore", svgs: svg },
            success: function (response) {
                if (response.responseText == "success") {
                    myMask.hide();
                    alert("保存成功");
                    $("#forePreview").show();
                    $("#forePub").show();
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    })
});


function getWordContent() {
    var pageContent = "";
    for (var i = 0; i < $("textarea").length; i++) {
        if ($("textarea")[i].id.indexOf("PO") > -1) {
            pageContent += $("textarea")[i].id + "=" + $($("textarea")[i]).val() + "&";
        }
    }

    for (var j = 0; j < $("input").length; j++) {
        if ($("input")[j].id.indexOf('PO') > -1) {
            pageContent += $("input")[j].id + "=" + $($("input")[j]).val() + "&";
        }
    }
    return pageContent.substring(0, pageContent.length - 1);
}

function InitChart() {
    var dateTime = Ext.getDom("H00").value;
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'IAQIChart'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                try {
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        if (obj == "0" || obj == "1" || obj == "2") {
                            RenderChart(result[obj], obj);
                        }
                    }
                    var chart_PM25 = $("#container0").highcharts();
                    var chart_PM10 = $("#container1").highcharts();
                    var chart_NO2 = $("#container2").highcharts();
                    var svg = chart_PM25.getSVG() + "&" + chart_PM10.getSVG() + "&" + chart_NO2.getSVG();
                    $("#svgContent").val(svg);
                    myMask.hide();
                }
                catch (err) {
                    $("#container0").html("");
                    $("#container1").html("");
                    $("#container2").html("");
                    myMask.hide();
                }

            }
            else {
                myMask.hide();
                Ext.Msg.alert("提示", "没有满足条件的信息。");
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionReport', 'GetEnvReportAllContent'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    $("#" + obj).val(result[obj]);
                }
                $("#wordTempContent").val(getWordContent());
            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}


function InitContent() {
    for (var j = 0; j < $("input").length; j++) {
        if ($("input")[j].id.indexOf('PO') > -1) {
            $($("input")[j]).val("0");
        }
    }

    var month = new Date().getMonth() + 1;
    $("#PO_year").val(new Date().getFullYear());
    if (Ext.getDom("H00").value != "") {
        var dateTime = Ext.getDom("H00").value;
        month = dateTime.substring(5, 7)[0] == "0" ? dateTime.substring(6, 7) : dateTime.substring(5, 7);
        $("#PO_year").val(dateTime.substring(0, 4));
        $("#PO_month").val(month);
    }
    InitChart();
    InitTable();    
}

function Export() {
    var dateTime = Ext.getDom("H00").value;
    var chart = $('#container0').highcharts();
    chart.exportChart({
        url: 'chartExplorerss.ashx?ItemName=PM25&dateTime=' + dateTime,
        width: 1400,
        sourceWidth: 1400,   //导出的文件宽度
        filename: 'MyChart',
        type: 'image/png'
    });
}
function RenderChart(result, obj) {
    var yTitle = "AQI";
    var title = "";
    var fileName = "";
    if (obj == 0) {
        title = "PM2.5";
        fileName = "PM25";
    }
    if (obj == 1) {
        title = "PM10";
        fileName = "PM10";
    }
    if (obj == 2) {
        title = "NO2";
        fileName = "NO2";
    }
    if (obj == 3) {
        title = "O3-1h";
        fileName = "O31";
    }
    if (obj == 4) {
        title = "O3-8h";
        fileName = "O38";
    }

    var ArrayData = new Array();
    for (var j = 0; j < 3; j++) {
        var arrayTmp = new Array();
        if (result[j] != null && result[j] != "**") {
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
    //    var dateTime = Ext.getDom("H00").value;
    var dateTime = "2016年03月";
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
        width:'1050px',
        title: {
            text: title
        },
        global: { useUTC: false },
        exporting: {
            url: 'chartExplorerss.ashx?ItemName=' + fileName + '&dateTime=' + dateTime,
            width: 1400,
            sourceWidth: 1400,   //导出的文件宽度
            filename: 'MyChart',
            enabled: true
        },
        tooltip: {
            shared: true,
            crosshairs: true,
            formatter: function () {
                var dt = new Date(parseInt(this.points[0].x) - 8 * 3600 * 1000);
                var hour = dt.getHours();
                var tipHour = "上午";
                if (hour == "08")
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
                return tipMessage;
            }
        },
        xAxis: {
            type: 'datetime',
            tickInterval: 8 * 3600 * 1000,

            labels: {
                style: {
                    fontSize: '9px'
                },
                formatter: function () {
                    var hour = Highcharts.dateFormat('%H', parseInt(this.value) - 8 * 3600 * 1000);
                    var tipMessage = "";
                    var tipHour = "上午";
                    if (obj == 3 || obj == 4) {
                        if (hour == "08") {
                            tipMessage = Highcharts.dateFormat('%m月', this.value) + "<br/>" + Highcharts.dateFormat('%d日', this.value);
                        }
                    }
                    else {

                        if (hour == "08") {

                            tipHour = "下<br/>午";
                            tipMessage = tipHour + "<br/>" + Highcharts.dateFormat('%m月', this.value) + "<br/>" + Highcharts.dateFormat('%d日', this.value);
                        }
                        else if (hour == "16") {

                            tipHour = "夜<br/>间";
                            tipMessage = tipHour + "<br/>";
                        }
                        else if (hour == "00") {
                            tipHour = "上<br/>午";
                            tipMessage = tipHour + "<br/>"
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
                name: 'WRF-Chem',
                data: ArrayData[1]
            }, {
                type: 'line',
                name: '主观预报',
                data: ArrayData[2]
            }]
    });

}
