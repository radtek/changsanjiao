var m_width, m_height, m_infoes = { top: 0.173, left: 0.1906, perH: 0.089, perW: 0.07 };
$(function () {
    InitSvg(); GetDatas();
})

function InitSvg() {
    m_height = $("#chartImg").height();
    m_width = $("#chartImg").width();
    $("#chartSvg").height(m_height);
}

function GetDatas() {
    $.ajax({
        url: "cityForeChart.aspx/GetDatas",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            if (results.d.rows) ShowChart(results.d.rows);
        }
    });
}

function ShowChart(datas) {
    var date = new Date(), color, html = "", textHtml = "";
    var x, y,h,item,plus;
    for (var i = 0; i < datas.length;i++ ) {
        x = m_width * (m_infoes.left + i * m_infoes.perW + m_infoes.perW / 2) - 5;
        h = (datas[i].AQI * (m_infoes.perH *m_height / 50)).toFixed(1);
        y = (m_height * m_infoes.top + 350 * (m_infoes.perH * m_height / 50) - h).toFixed(1);
        if (datas[i].AQI) {
            color = GetColor(datas[i].AQI);
            html += '<rect title="当前指数: ' + datas[i].AQI + '" x="' + x.toFixed(1) + '" y="' + y + '" width="20" height="' + h + '"style="fill:' + color + ';stroke-width:0;"/>';
        }
        item = datas[i].DATE.split("-");
        plus = datas[i].PLU.split(",");
        if (plus[0]) {
            textHtml += '<text x="' + (x + 5).toFixed(1) + '" y="' + (m_height * m_infoes.top + 350 * (m_infoes.perH * m_height / 50) -h-5).toFixed(1) + '" text-anchor="middle" fill="white">';
            for (var j = 0; j < plus.length; j++) { 
                textHtml += '<tspan dy="'+(j?'-20':'0')+'" x="' + (x + 5).toFixed(1) + '">' + plus[j] + '</tspan>';
            }
            textHtml += '</text>';
        }
        
        textHtml += '<text x="' + (x + 5).toFixed(1) + '" y="' + (m_height * m_infoes.top + 350 * (m_infoes.perH * m_height / 50) + 15).toFixed(1) + '" text-anchor="middle" fill="white"><tspan dy="16" x="' + (x + 5).toFixed(1) + '">' + item[1] + "-" + item[2] + '</tspan><tspan dy="26" x="' + (x + 5).toFixed(1) + '">' + item[0] + '</tspan></text>';
    }
    $("#chartSvg").html(html + textHtml);
    $("#chartSvg rect").hover(function () {
//        this.r.baseVal.value = 12;
        $("#divInfoes").html($(this).attr("title")).css({ display: "block", top: this.y.baseVal.value - 18, left: this.x.baseVal.value + 18 });
    }, function () {
//        this.r.baseVal.value = 10;
        $("#divInfoes").css({ display: "none" });
    });
}

