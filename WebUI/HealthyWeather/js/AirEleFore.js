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
    $.ajax({
        url: "AirEleFore.aspx/GetChart",
        type: "POST",
        data: "{pollName:'" + pollName + "',time:'"+time+"'}",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            let data = results.d;
            if (data != "error") {
                var yLINE = "ug/m3";
                if (pollName == "O3") {
                    yLINE = "PPB";
                }
                let series = eval("(" + data.split('&')[0] + ")");;
                let name = data.split('&')[1];
                renderChart(series, name, yLINE);
            }
        }

    });
}

function renderChart(series, name, yLINE) {
    
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
            shared: true,
            crosshairs: true,
            xDateFormat: '%Y-%m-%d %H:%M'
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
            name: name,
            type: 'spline',
            data: series,
            color: '#ff00ff'
        }
        ]
    });
}