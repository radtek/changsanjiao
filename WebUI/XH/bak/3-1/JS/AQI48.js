var m_width, m_height, m_infoes = { top: 0.173, left: 0.1906, perH: 0.089, perW: 0.14 };
$(function () {
    InitSvg(); GetDatas(new Date()); // ShowChart(); 
})
//window.onresize = function () {
//    InitSvg();
//}
function InitSvg() {
    m_height = $("#chartImg").height();
    m_width = $("#chartImg").width();
    $("#divSvg").height(m_height);
}

function GetDatas(dt) {
    $.ajax({
        url: "AQI48.aspx/GetDatas",
        type: "POST",
        contentType: "application/json",
        data: "{date:'" + formatate(dt) + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d) ShowChart( results.d);
        },
        error: function (ex) {
            if (formatate(dt) == formatate(new Date())) GetDatas(AddDay(dt, -1));
            else alert("今天、昨天的数据都没到！");
        }
    });
}
function ShowChart(datas) {
    var date = new Date(), color, html = "", pathHtml = '<polyline points="',textHtml="";
    var x, y,value;
    for (var i = 0; i < datas.length; i++) {
        value = datas[i].split(':');
        color = GetColor(value[1]);
        x = m_width * (m_infoes.left + i * m_infoes.perW + m_infoes.perW / 2) - 5;
        y = m_height * m_infoes.top + (350 - value[1]) * (m_infoes.perH * m_height / 50) - 5;
        pathHtml += x + "," + y + " ";
        html += '<circle title="当前指数: ' + value[1] + '" cx="' + x.toFixed(1) + '" cy="' + y.toFixed(1) + '" r="10" stroke-width="0" fill="' + color + '"/>';
        if (value[2]){ 
            var plus=value[2].split(",");
            textHtml += '<text x="' + (x + 5).toFixed(1) + '" y="' + (y+30).toFixed(1) + '" text-anchor="middle" fill="white">';
            for (var j = 0; j < plus.length; j++) { 
                textHtml += '<tspan dy="'+(j?'-20':'0')+'" x="' + (x + 5).toFixed(1) + '">' + plus[j] + '</tspan>';
            }
            textHtml += '</text>';
        }
        textHtml += '<text x="' + x.toFixed(1) + '" y="' + (m_height * m_infoes.top + 350 * (m_infoes.perH * m_height / 50) + 15).toFixed(1) + '" text-anchor="middle" fill="white"><tspan dy="16" x="' + x + '">' + value[0].split("&")[0] + '</tspan><tspan dy="26" x="' + x + '">' + value[0].split("&")[1] + '</tspan></text>';
    }
    pathHtml += "\" style=\"fill:transparent;stroke:white;stroke-width:2\"/>";
    $("#divSvg").html('<svg id="chartSvg" version="1.1" xmlns="http://www.w3.org/2000/svg">' + pathHtml + html + textHtml + '</svg>');
    $("#chartSvg circle").hover(function () {
        this.r.baseVal.value = 12;
        $("#divInfoes").html($(this).attr("title")).css({ display: "block", top: this.cy.baseVal.value+5, left: this.cx.baseVal.value+5 });
    }, function () {
        this.r.baseVal.value = 10;
        $("#divInfoes").css({ display: "none"});
    });
}
