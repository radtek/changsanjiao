$(function () {
    getTime();
})
function getTime() {
    $.ajax({
        url: "ECGroundEle.aspx/GetTime",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d;
            if (data != "error") {
                $("#time").val(data);
                query();
            }
        }

    });
}
function query() {
    let time = $("#time").val();
    $.ajax({
        url: "ECGroundEle.aspx/Query",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        data: "{time:'" + time + "'}",
        success: function (results) {
            var dataStr = results.d;
            if (dataStr.indexOf("error") < 0) {
                //var data = eval('(' + dataStr + ')');

                RenderChart(dataStr);
            }

        }

    });
}

function getWindPath(speedArr, directArr) {
    var windArr = new Array();
    speedArr.forEach(function (item, index) {
        var speed = item[1];
        var direct = directArr[index][1];
        var y = 50;
        if (item[1] == null) {
            y = "null";
        }
        var point = {
            x: item[0],
            y: y,
            marker: {
                symbol: 'windarrow',
                lineColor: '#f00',
                lineWidth: 2,
                speed: ConvertToFloat(speed),
                direct: ConvertToFloat(direct),
                radius: 4
            }
        }
        windArr[index] = point;
    })
    return windArr;
}
function RenderChart(data) {
    var dataArr = data.split("&");
    var windArr = dataArr.slice(2, 4);   //风速，风向
    dataArr = dataArr.slice(0, 2);   //相对湿度、温度
    var windInfoArr = "";
    var RH = '', tem = '';
    if (windArr.length > 1) {
        var windSpeed = eval('(' + windArr[1].split('#')[0] + ')');
        var windDir = eval('(' + windArr[0].split('#')[0] + ')');
        windInfoArr = getWindPath(windSpeed, windDir);
        RH = eval("(" + dataArr[0].split('#')[0] + ")");
        tem = eval("(" + dataArr[1].split('#')[0] + ")");
    }
    Highcharts.setOptions({
        lang: {
            rangeSelectorFrom: '从',
            rangeSelectorTo: '到',
            rangeSelectorZoom: '范围：'
        }
    });
    Highcharts.stockChart('container', {
        // $('#container').highcharts('Chart', {
        //rangeSelector: {

        //},
        chart: {
            height: 600
        },
        rangeSelector: {
            selected: 2,
            buttons: [{
                type: 'day',
                count: 3,
                text: '3天'
            }, {
                type: 'week',
                count: 1,
                text: '7天'
            }, {
                type: 'day',
                count: 15,
                text: '15天'
            }, {
                type: 'all',
                text: 'All'
            }],
            buttonTheme: {
                width: 50
            },
            inputDateFormat: '%m-%d,%H',
            inputEditDateFormat: '%m-%d,%H',
        },
        navigator: {
            baseSeries: 2,
            //series: {
            //    type: 'spline',
            //    color: '#4572A7',
            //    fillOpacity: 0.4,
            //    //dataGrouping: {
            //    //    smoothed: true,
            //    //},
            //    lineWidth: 1,
            //    marker: {
            //        enabled: false
            //    },
            //    shadow: false
            //},
            xAxis: {
                tickWidth: 0,
                lineWidth: 0,
                gridLineWidth: 1,
                tickPixelInterval: 100,
                labels: {
                    align: 'center',
                    x: 0,
                    y: 0,
                    formatter: function () {
                        return Highcharts.dateFormat('%m-%d,%H', this.value);
                    }
                }
            }
        },
        credits: { enabled: false },
        zoomType: 'xy',
        global: { useUTC: false },
        title: {
            text: '地面多要素',
            style: {
                fontSize: "28px"
            }
        },
        exporting: {
            enabled: false
        },
        tooltip: {
            shared: true, //受data[type]影响
            crosshairs: false,
            formatter: function () {
                var c_tipTimeFormat = '%Y-%m-%d %H';
                var _time_val = Highcharts.dateFormat(c_tipTimeFormat, this.points[0].x);
                var tipMessage = _time_val;
                for (var i = 0; i < this.points.length; i++) {
                    if (this.points[i].series.name == "风速风向" && this.points[i].point.marker.speed != "null" && this.points[i].point.marker.speed != null) {
                        tipMessage = tipMessage + '<br/><span style="color:#ff0000">风速</span>：' + this.points[i].point.marker.speed.toFixed(2);
                        tipMessage = tipMessage + '<br/><span style="color:#ff0000">风向</span>：' + this.points[i].point.marker.direct.toFixed(2);
                    }
                    else
                        tipMessage = tipMessage + '<br/><span style="color:' + this.points[i].series.color + '">' + this.points[i].series.name + '</span>：' + this.points[i].y.toFixed(2);

                    tipMessage = tipMessage + "<br/>";
                }
                return tipMessage;
            }

        },
        xAxis: {
            type: 'datetime',
            labels: {
                formatter: function () {
                    return Highcharts.dateFormat('%m-%d,%H', this.value);
                },
                rotation: 35
            },
            tickPixelInterval: 60,
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
                        formatter: function () {
                            return this.value;
                        },
                        style: {
                            color: '#990033'
                        }
                    },
                    title: {
                        text: "降水量(mm)",
                        style: {
                            color: '#990033'
                        }
                    },

                    lineWidth: 2,
                    showEmpty: false,
                    opposite: false

                }, {
                    title: {
                        text: "温度T(℃)",
                        style: {
                            color: '#FF0000'
                        }
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        },
                        style: {
                            color: '#FF0000'
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
            series: {
                dataGrouping: {
                    enabled: false
                }
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true,
                        allowOverlap: false
                    }
                }
            },

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
            y: 0,
            floating: true,
            borderWidth: 1,
            backgroundColor: '#FFFFFF'

        },
        series:
                [{
                         type: 'column',
                         name: "降水量",
                         data: RH,
                         //data:windArrtest,
                         yAxis: 0,
                         visible: true,
                         color: 'green'

                  }, {
                         type: 'spline',
                         name: "温度",
                         yAxis: 1,
                         data: tem,
                         visible: true
                  }, {
                    type: 'spline',
                    name: "风速风向",
                    yAxis: 0,
                    data: windInfoArr,
                    color: 'white',
                    visible: true
                }
                ]
    });
}

Highcharts.SVGRenderer.prototype.symbols.windarrow = function (x, y, w, h, ee) {
    x = x + w / 2, y = y + h / 2;
    var path;
    var speed = ee.speed, direct = ee.direct;

    if (speed != 0) {
        var radius = (parseFloat(speed / 3.6) + 4) * 4,
                  _r = 4,
                  _angle = 200 / radius,
                  rotateWay = "";
        var apoint = getRotatePoints2([x, y], radius, parseFloat(direct), rotateWay);
        var bpoint = getRotatePoints2([x, y], radius - _r, parseFloat(direct) + _angle, rotateWay);
        var cpoint = getRotatePoints2([x, y], radius - _r, parseFloat(direct) - _angle, rotateWay);

        path = ['M', x, y, 'L', apoint[0][0], apoint[0][1], bpoint[0][0], bpoint[0][1], apoint[0][0], apoint[0][1], cpoint[0][0], cpoint[0][1], apoint[0][0], apoint[0][1]];
    }

    else {
        path = ['M', x - 1, y, 'L', x + 1, y];
    }
    return path;
};
if (Highcharts.VMLRenderer) {
    Highcharts.VMLRenderer.prototype.symbols.windarrow = Highcharts.SVGRenderer.prototype.symbols.windarrow;
}
//getRotatePoints2(起始点[x,y], 半径(px), 角度(℃), 方向(rotateWay为空表示顺时针))
function getRotatePoints2(center, radius, angle, rotateWay) {
    var sin;
    var cos;
    var x;
    var y;
    var _rotateWay = "";

    var points = new Array();
    sin = Math.sin(angle * Math.PI / 180);
    cos = Math.cos(angle * Math.PI / 180);
    if (rotateWay === "")
        x = center[0] - radius * sin;
    else
        x = center[0] + radius * sin;
    y = center[1] + radius * cos;
    points[0] = [x, y];
    return points;
}
function ConvertToFloat(x) {
    if (x != "null") {
        var floatTemp = parseFloat(x).toFixed(2);
        var floatValue = parseFloat(floatTemp);
        if (floatValue > 0)
            return floatValue;
        else
            return null;
    }
    else
        return null;

}