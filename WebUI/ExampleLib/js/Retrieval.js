
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
function AddDay(theDate, count) {
    if (typeof (count) != "number") {
        return null;
    }
    var result = new Date(theDate);
    result.setDate(result.getDate() + count);
    return result;
}
$(function () {
    var dtNow = new Date();
    $("#queryTime").val(formatate(AddDay(dtNow,-1)));
    $(".checkBox").click(function () {
        if (this.innerText == "全选") {
            if ($(this).attr("class").indexOf("unchecked") > -1) {
                $(".queryFeature").removeClass("unchecked").addClass("checked");
                $(this).removeClass("unchecked").addClass("checked");
            }
            else {
                $(".queryFeature").removeClass("checked").addClass("unchecked");
                $(this).removeClass("checked").addClass("unchecked");
            }
        }
        else {
            if ($(this).attr("class").indexOf("unchecked") != -1) {
                $(".queryFeature.checked").removeClass("checked").addClass("unchecked");
                $(this).removeClass("unchecked").addClass("checked");
            }
        }
    });
    $("#btnQuery").click(Query);
    Query();
});

function Query() {
    $('#divTable').html('<table class="display dataTable no-footer" id="resultTbl" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align:center"></table>');
    var url = "PatrolHandler.do?provider=MMShareBLL.DAL.Example&method=QueryDeparture&time=", queryTime = $("#queryTime").val();
    if (!queryTime) { alert("查询时间不能为空！"); return; }
    if ($("#chkAll").attr("class").indexOf("unchecked") > 0) {
        url += queryTime + "2000&element=" + $(".checked.queryFeature").attr("tag") + "&type=GetSimilarListByTimeAndElement";
    }
    else url += queryTime + "2000&element=&type=GetSimilarListByTime";
    var tbl = $('#resultTbl').DataTable({
        "bProcessing": true, "sScrollY": 300, "bFilter": true,
        "bPaginate": false, "bInfo": false, "bDestroy": true, "bRetrieve": true,
        "sAjaxSource": url,
        "fnDrawCallback": function (a, b) {
            var obj = $("#resultTbl tbody tr")[0];
            if (obj.innerText.indexOf("对不起") == -1) obj.click();
            if (a.jqXHR) {
            }
        },
        "columns": [
                { "title": "序号", "class": "center" },
                { "title": "日期", "class": "center" },
                { "title": "相似离度", "class": "center" },
        //            { "title": "湿度情况", "class": "center" },
        //            { "title": "风速情况", "class": "center" },
                {"title": "首要污染物", "class": "center" },
                { "title": "峰值浓度", "class": "center" },
                { "title": "最大AQI", "class": "center"}],
        "oLanguage": {
            "sProcessing": "<img src='../images/loading.gif?v=2'/>正在查询...",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sSearch": "表内搜索：",
            "sEmptyTable": "表中无数据存在！"
        }
    });

    $('#resultTbl tbody').on('click', 'tr', function (obj) {
        $('#resultTbl .selected').removeClass("selected");
        $(this).addClass("selected");
        var data = tbl.row(this).data();
        QueryChart(data[1], data[3]);
        GetImg(data[1]);
    });
}

function GetImg(date) {
    $.ajax({
        url: "Retrieval.aspx/GetImg",
        data: "{dateClick:'" + date + "',dateQuery:'" + $("#queryTime").val() + "'}",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.tables;
            var strHtml = "";
            for (var i = 0; i < datas[0].rows.length; i++) {
                strHtml += "<img style='width:100%' src='../" + datas[0].rows[i].FOLDER + "/" + datas[0].rows[i].NAME + "'>";
            }
            $($("#divImg div")[0]).html(strHtml);
            strHtml = "";
            for (var i = 0; i < datas[1].rows.length; i++) {
                strHtml += "<img style='width:100%' src='../" + datas[1].rows[i].FOLDER + "/" + datas[1].rows[i].NAME + "'>";
            }
            $($("#divImg div")[1]).html(strHtml);

        },
        error: function (ex) {
//            alert("异常，" + ex.responseText + "！");
        }
    });
}
function QueryChart(strDate, type) {
    var field = "";
    if (type.indexOf(10) > 0) field = "PM10";
    else field = "PM2_5";
    $.ajax({
        url: "Retrieval.aspx/QueryHour",
        type: "POST",
        contentType: "application/json",
        data: "{date:'" + strDate + "',field:'" + field + "'}",
        dataType: 'json',
        success: function (results) {
            var series = JSON.parse(results.d);
            $("#chartBack").highcharts({
                chart: { type: 'spline' },
                legend: { enabled: false },
                credits: { enabled: false },
                title: { text: "" },
                lang: { noData: "该时间点未查询到相关数据" },
                exporting: { enabled: false },
                tooltip: { xDateFormat: '%Y-%m-%d, %H' },
                xAxis: { type: 'datetime', dateTimeLabelFormats: { day: '%d日', hour: '%H时'} },
                yAxis: { title: { text: 'PM2.5浓度值'} },
                series: series
            });
        },
        error: function (ex) {
//            alert("异常，" + ex.responseText);
        }
    });
}