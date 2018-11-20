$(function () {
    getDate();
})

function getDate() {
    $.ajax({
        url: "AirEleFore.aspx/Getdate",
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
function radioClickModule(id, name) {
    var el = $('#' + id);
    if (el.hasClass("radioUnChecked")) {
        el.removeClass("radioUnChecked");
        $(".radioChecked").addClass("radioUnChecked");
        $(".radioChecked").removeClass("radioChecked");
        el.addClass("radioChecked");
        query();
    }
}
function query() {
    let pollName = $(".radioChecked a").text();
    let time = $("#time").val();
    let sites = $("#site").val();
    $.ajax({
        url: "AirEleFore.aspx/GetChart",
        type: "POST",
        data: "{pollName:'" + pollName + "',time:'" + time + "',sites:'" + sites + "'}",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d;
            var yLINE = "ug/m3";
            if (pollName == "O3") {
                yLINE = "PPB";
            }
            if (pollName == 'CO') {
                yLINE = "mg/m3";
            }
            if (data != "error") {
                let foreStr = data.split("@")[0], shiKStr = data.split("@")[1];
                let arrFore = foreStr.split("#");
                let arrShiK = shiKStr.split("#");
                for (let i = 0; i < arrFore.length; i++) {
                    let fore = eval("(" + arrFore[i].split('&')[0] + ")");
                    let shiK = eval("(" + arrShiK[i].split('&')[0] + ")");
                    let name = arrFore[i].split('&')[1];
                    let dValue = calDValue(fore,shiK);
                    renderChart(fore, shiK,dValue, name,yLINE);
                }
                getTable();
            }
        }

    });
}
function calDValue(fore,shiK) {
    var arr = [];
    for (var i = 0; i < shiK.length; i++) {
        var pointer = new Array();
        var time = fore[i][0];
        var val = '';
        if (shiK[i][1] == null) {
            val = null;
        } else {
            val = parseFloat((fore[i][1] - shiK[i][1]).toFixed(2));
        }
        pointer.push(time, val);
        arr.push(pointer);
    }
    
    return arr;
}
function getTable() {
    $.ajax({
        url: "AirEleFore.aspx/GetTable",
        type: "POST",
        //data: "{sites:'" + sites + "',time:'" + time + "'}",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d;
            if (data != 'error') {
                $('.table').empty();
                $('.table').html(data);
            }
        }
    });
}
function renderChart(fore,shiK,dValue, name, yLINE) {
    
    $("#container").highcharts({
        chart: {
            type: 'spline'
        },
        credits: { enabled: false },
        title: {
            text: "大气成分预报",
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
            shared: true, //受data[type]影响
            crosshairs: false,
            formatter: function () {
                var c_tipTimeFormat = '%Y-%m-%d %H';
                var _time_val = Highcharts.dateFormat(c_tipTimeFormat, this.points[0].x);
                var tipMessage = _time_val;
                for (var i = 0; i < this.points.length; i++) {
                    tipMessage = tipMessage + '<br/><span style="color:' + this.points[i].series.color + '">' + this.points[i].series.name + '</span>：' + this.points[i].y.toFixed(2);
                    tipMessage = tipMessage + "<br/>";
                }
                //if (this.points.length > 1) {
                //    tipMessage += '<br/><span style="color:blue">差值</span>:' + (this.points[1].y - this.points[0].y).toFixed(2)
                //}
                return tipMessage;
            }
        },

        xAxis: {
            type: 'datetime',
            showLastLabel: false,
            // tickInterval: 60 * 60 * 1000,//间隔值 
            tickPixelInterval: 50,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%Y-%m-%d,%H', this.value)
                    return tipMessage;
                },
                rotation: 30,
                x: 20,
                y: 30
            },
            offset: 0,
            lineColor: '#473C8B',
            lineWidth: 2,
            gridLineWidth: 0
        },

        legend: {

            borderWidth: 0
        },
        yAxis: [{ // Primary yAxis
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
            minorGridLineWidth: 1,
            minorTickWidth: 1,
            minorTickLength: 0
            //showEmpty: false
        },
        {
            gridLineWidth: 0,
            lineWidth: 2,
            title: {
                text: yLINE
            },
            minorGridLineWidth: 1,
            minorTickWidth: 1,
            minorTickLength: 0,
            opposite: true
            //showEmpty: false
        }],

        plotOptions: {
            series: {
                marker: {
                    radius: 1  //曲线点半径，默认是4
                },
                turboThreshold: 2000
            }
        },
        series: [
        {
            name: '实况',
            type: 'column',
            data: shiK,
            color: 'green',
            yAxis: 0
        },
        {
            name: '预报',
            type: 'spline',
            data: fore,
            color: '#ff00ff',
            yAxis:0
        },
        {
            name: '差值',
            type: 'spline',
            data: dValue,
            yAxis:1
        }
        ]
    });
}