var chart = { perTop: 0.09, perLeft: 0.05, eachPerHt: 0.11, eachPerWh: 0.14, html: "", type: "",
    yLbl: [[0, "00DD18", "一级：优"], [50, "FDF001", "二级：良"], [100, "F69603", "三级：轻度污染"], [150, "FB0102", "四级：中度污染"]
        , [200, "980146", "五级：重度污染"], [250, "980146", "五级：重度污染"], [300, "640604", "六级：严重污染"], [350, "", ""]],
    GetHtml: function () { return this.html; },
    ToFixed: function (data, length) { return parseFloat(data.toFixed((length ? length : 1))); },
    GetEachY: function (i, middle) { return this.ToFixed(this.top + this.perHeight * (this.yLbl.length - (middle ? 2 : 1) - i)); },
    GetBack: function () {
        this.chartWidth = this.width - 2 * (this.left + 20), this.chartHeight = this.perHeight * (this.yLbl.length - 1);
        var strResult1 = '<rect  x="' + (this.left + 40) + '" y="' + (this.top - 8) + '" width="' + this.chartWidth + '" height="' + this.chartHeight
        + '"style="fill:rgba(255,255,255,0.1);"/><g text-anchor="middle" fill="white">',
        strResult2 = '<g style="stroke-width:0;" text-anchor="left" fill="white">';
        for (var i = 0; i < this.yLbl.length; i++) {
            strResult1 += '<text  x="' + this.left + '" y="' + this.GetEachY(i) + '">' + this.yLbl[i][0] + '</text>';
            if (this.yLbl[i][1]) {
                strResult2 += '<rect x="' + (this.left + 30) + '" y="' + (this.GetEachY(i, true) - 8) + '" width="15" height="' + this.perHeight + '"style="fill:#' + this.yLbl[i][1] + ';"/>';
                strResult1 += '<polyline points="' + (this.left + 60) + ',' + (this.GetEachY(i) - 8) + ' ' + (this.left + this.chartWidth) + ','
                + (this.GetEachY(i) - 8) + '" style="fill:transparent;stroke:rgba(255,255,255,0.2);stroke-width:2"/>';
//                if (this.yLbl[i + 1] && this.yLbl[i + 1][2] == this.yLbl[i][2])
//                    strResult2 += '<text x="' + (this.left + 60) + '" y="' + this.ToFixed(this.top + this.perHeight * (this.yLbl.length - 2 - i)) + '" style="font-size:0.9em;">' + this.yLbl[i][2] + '</text>';
//                else if (this.yLbl[i - 1] && this.yLbl[i - 1][2] == this.yLbl[i][2]) continue;
//                else
//                    strResult2 += '<text x="' + (this.left + 60) + '" y="' + this.ToFixed(this.top + this.perHeight * (this.yLbl.length - 2 - i + 0.5)) + '" style="font-size:0.9em;">' + this.yLbl[i][2] + '</text>';
            }
        }
        strResult1 += '</g>';
        strResult2 += '</g>';
        return strResult1 + strResult2;
    },
    GetActive: function () {
        var lineHtml = ['<polyline points="', ''], coloumnHtml = [''], textHtml = '<g text-anchor="middle" fill="white">';
        var x, y, value, color;
        for (var i = 0; i < this.datas.length; i++) {
            value = this.datas[i].split(':');
            if (value[1]) {
                color = GetColor(value[1]);
                x = this.ToFixed(this.left*2+ (i + 0.5) * this.perWidth - 5); //+ 170
                yLine = this.ToFixed(this.top + (350 - value[1]) * this.perHeight / 50 - 5);
                h = this.ToFixed(value[1] * this.perHeight / 50);
                yColumn = this.ToFixed(this.top + 7 * this.perHeight - h - 8);
                lineHtml[0] += x + "," + yLine + " ";
                lineHtml[1] += '<circle class="point" title="当前指数: ' + value[1] + '" cx="' + x + '" cy="' + yLine + '" r="10" stroke-width="0" fill="' + color + '"/>';
                coloumnHtml[0] += '<rect class="point" title="当前指数: ' + value[1] + '" x="' + x + '" y="' + yColumn + '" width="20" height="' + h + '"style="fill:' + color + ';stroke-width:0;"/>';
            }
            if (value[2]) {
                var plus = value[2].split(",");
                textHtml += '<text x="' + (x + 5) + '" y="' + (this.type == "line" ? (yLine + 30) : (yColumn - 5)) + '">';
                for (var j = 0; j < plus.length; j++) {
                    textHtml += '<tspan dy="' + (j ? '-20' : '0') + '" x="' + (x + (this.type == "line" ? 5 : 10)).toFixed(1) + '">' + plus[j] + '</tspan>';
                }
                textHtml += '</text>';
            }
            textHtml += '<text x="' + (x + (this.type == "line" ? 5 : 10)).toFixed(1) + '" y="' + this.ToFixed(this.top + 350 * this.perHeight / 50 + 15) + '"><tspan dy="16" x="' + (x + (this.type == "line" ? 5 : 10)).toFixed(1) + '">' + value[0].split("&")[0] + '</tspan><tspan dy="26" x="' + (x + (this.type == "line" ? 5 : 10)).toFixed(1) + '">' + value[0].split("&")[1] + '</tspan></text>';
        }
        lineHtml[0] += "\" style=\"fill:transparent;stroke:white;stroke-width:2\"/>";
        textHtml += "</g>"
        return (this.type == "line" ? lineHtml.join() : coloumnHtml.join()) + textHtml;
    },
    InitChart: function (options) {//options.datas格式：20时—06时&01-01:55:PM2.5,PM10
        if (!options.container) options.container = $("body");
        if (!options.type) options.type = "line";
        this.type = options.type;
        this.datas = options.datas;
        this.width = options.container.width(), this.height = options.container.height();
        this.eachPerWh = this.ToFixed((1 - this.perLeft * 4+0.02) / this.datas.length, 4);
        this.left = this.ToFixed(this.width * this.perLeft), this.top = this.ToFixed(this.height * this.perTop);
        this.perWidth = this.ToFixed(this.width * this.eachPerWh), this.perHeight = this.ToFixed(this.height * this.eachPerHt);
        this.html = this.GetBack() + this.GetActive();
        options.container.html('<svg id="chartSvg" version="1.1" xmlns="http://www.w3.org/2000/svg">' + this.html + '</svg><div id="divInfoes"></div>');

        $("svg .point").hover(function (e) {
            var x1 = e.pageX || (e.clientX), y1 = e.pageY || (e.clientY);
            try {
                this.r.baseVal.value = 12;
            } catch (ex) { }
            $("#divInfoes").html($(this).attr("title")).css({ display: "block", top: y1 + 10, left: x1 + 10 });
        }, function () {
            try {
                this.r.baseVal.value = 10;
            } catch (ex) { }
            $("#divInfoes").css({ display: "none" });
        });
    }
};
var m_option;
$(function () {
    GetDatas(new Date());
});
window.onresize = function () {
    if (m_option)
        chart.InitChart(m_option);
}
function GetDatas(dt) {
    $.ajax({
        url: (m_entity == "aqi" ? "AQI48" : "cityForeChart") + ".aspx/GetDatas",
        type: "POST",
        contentType: "application/json",
        data: "{date:'" + formatate(dt) + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d) {
                m_option = { type: (m_entity == "aqi" ?'line':'coloumn'), datas: results.d };
                chart.InitChart(m_option);
            }
            //else alert("没有找到数据！");
        },
        error: function (ex) {
            //alert("今天、昨天的数据都没到！");
        }
    });
}

function LeftPad(value, length, padChar) {
    var len = value.toString().length;
    while (len < length) {
        value = (padChar == undefined ? "0" : padChar) + value.toString();
        len++;
    }
    return value;
}

function formatate(format) {
    var year = format.getFullYear();
    var month = format.getMonth() + 1;
    var day = format.getDate();
    formatStr = year + "-" + LeftPad(month, 2) + "-" + LeftPad(day, 2);
    return formatStr;
}
function GetColor(val) {
    if (val < 0) alert("空气质量指数不能为负数！");
    else if (val < 50) return "#00DD18";
    else if (val < 100) return "#FDF001";
    else if (val < 150) return "#F69603";
    else if (val < 200) return "#FB0102";
    else if (val < 300) return "#980146";
    else return "#640604";
}
