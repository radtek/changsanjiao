$(function () {
    getDate();
})

function getDate() {
    $.ajax({
        url: "VisFore.aspx/Getdate",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d;
            if (data != "error") {
                $("#time").val(data);
                getSelVal();
            }
        }

    });
}
function getSelVal() {
    $.ajax({
        url: "VisFore.aspx/GetSelVal",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d.rows;
            if (data != "error") {
                //$("#time").val(data);
                var html = "";
                for (let i = 0; i < data.length; i++) {
                    html += "<option value='" + data[i].siteid + "'>" + data[i].sitename + "</option>"
                }
                $("#site").html(html);
                $('#site').selectpicker('refresh');
                $('#site').selectpicker('val', '' + data[0].siteid + '');
                query();
            }
        }

    });
}
function query() {
    let sites = $("#site").val();
    let time = $("#time").val();
    $.ajax({
        url: "VisFore.aspx/GetChart",
        type: "POST",
        data: "{sites:'" + sites + "',time:'" + time + "'}",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d;
            if (data != "error") {
                let foreStr = data.split("@")[0], shiKStr = data.split("@")[1];
                let arrFore = foreStr.split("#");
                let arrShiK = shiKStr.split("#");
                for (let i = 0; i < arrFore.length; i++) {
                    let fore = eval("(" + arrFore[i].split('&')[0] + ")");
                    let shiK = eval("(" + arrShiK[i].split('&')[0] + ")");
                    let name = arrFore[i].split('&')[1];
                    renderChart(fore,shiK, name, i);
                }
                getTable();
            }
        }

    });
}
function getTable() {
    $.ajax({
        url: "VisFore.aspx/GetTable",
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
function renderChart(fore,shiK, name,num ) {
    $("#container").append("<div class=c" + num + "></div>");
    $("#container .c" + num + "").highcharts({
        chart: {
            type: 'spline'
        },
        credits: { enabled: false },
        title: {
            text: name+"能见度预报",
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
       // global: { useUTC: false },  要设置在setoption
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
                if (this.points.length > 1) {
                    tipMessage += '<br/><span style="color:blue">差值</span>:' + (this.points[0].y - this.points[1].y).toFixed(2)
                }
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
            opposite: false,
            min: -1,
            max:5,
            title: {
                text: ""
            },
            minorGridLineWidth: 1,
            minorTickWidth: 1,
            minorTickLength: 0,
            tickInterval:1
            //showEmpty: false
        },

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
            name: '预报',
            type: 'spline',
            data: fore,
            color: '#ff00ff',
        },
        {
            name: '实况',
            type: 'column',
            data: shiK,
            color: 'green',
        }
        ]
    });
}