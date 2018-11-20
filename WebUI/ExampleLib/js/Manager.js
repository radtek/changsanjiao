var m_Info = { nowSeason: "", weatherType: null };
$(function () {
    var dtNow = new Date();
    $("#queryETime").val(GetDateStr(dtNow,0));
    $("#querySTime").val(GetDateStr(dtNow,-1));
    $("#btnQuery").click(Query);
    InitControl();

    $("input[name='queryType']").change(function () {
        if ($("input[name='queryType']:checked").val() == "rdoDay") {
            $("#pnlTypes").find(".form-control").attr("disabled", true);
            $("#pnlDay").find(".form-control").attr("disabled", false);
        }
        else {
            $("#pnlDay").find(".form-control").attr("disabled", true);
            $("#pnlTypes").find(".form-control").attr("disabled", false);
        }
    });
});
function GetDateStr(dt,year) {
    return (dt.getFullYear() + year) + "-" + (dt.getMonth() + 1) + "-" + dt.getDate();
}
function InitControl() {
    $.ajax({
        url: "Manager.aspx/InitControl",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d;
            var strHtml = "<option>--全部--</option>", tbl = datas[0].rows;
            for (var i = 0; i < tbl.length; i++) {
                strHtml += "<option>" + tbl[i].MC + "</option>";
            }
            $("#sltLvl").html(strHtml);
            m_Info.weatherType = datas[2].rows;
            strHtml = "<option value=''>--全部--</option>", tbl = datas[1].rows;
            for (i = 0; i < tbl.length; i++) {
                strHtml += "<option value='" + tbl[i].DM + "_" + tbl[i].PERIOD + "' >" + tbl[i].MC + "</option>";
            }
            $("#sltSeason").html(strHtml);
            CreatePollute($("#sltSeason")[0]);
            Query();

        },
        error: function (ex) {
//            alert("异常，" + ex.responseText + "！");
        }
    });
}

function CreatePollute(obj) {
    var selectedOption = obj.options[obj.selectedIndex];
    m_Info.nowSeason = selectedOption.value.split("_");
    var strHtml = "<option>--全部--</option>";
    for (var i = 0; i < m_Info.weatherType.length; i++) {
        if (m_Info.nowSeason[0] === "" || m_Info.weatherType[i].SEASON.indexOf(m_Info.nowSeason[0]) > -1) strHtml += "<option>" + m_Info.weatherType[i].MC + "</option>";
    }
    $("#sltWeather").html(strHtml);
}

function Query() {
    var sdate = $("#querySTime").val(), edate = $("#queryETime").val();
    if (sdate == "" || edate == "") { alert("开始或结束的时间不能为空！");return;}
    var url = "PatrolHandler.do?provider=MMShareBLL.DAL.Example&method=QueryManager&sDdate=" + sdate
    + "&eDdate=" + edate;
    if ($("input[name='queryType']:checked").val() == "rdoDay") url += "&type=day&strWhere=" + $("#theDays").val();
    else {
        url += "&type=else&strWhere=";
        var lvl = $("#sltLvl").val();
        if (lvl != "--全部--") url += lvl;
        url += "*";
        if (m_Info.nowSeason[1]) url += m_Info.nowSeason[1];
        url += "*";
        var weather = $("#sltWeather").val();
        if (weather != "--全部--") url +=  weather;
    }
    $('#divTable').html('<table class="display dataTable no-footer" id="resultTbl" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align:center"></table>');
    var tbl = $('#resultTbl').DataTable({
        "bProcessing": true, "sScrollY": 300, "bFilter": true,
        "bPaginate": false, "bInfo": false, "bDestroy": true, "bRetrieve": true,
        "sAjaxSource": url,
        "fnDrawCallback": function (a, b) {
            var obj = $("#resultTbl tbody tr")[0];
            if (obj.innerText.indexOf("对不起") == -1) obj.click(); 
            if (a.jqXHR) {
                //                $("#resultTbl tbody").html('<tr role="row" class="odd"><td class="center sorting_1">2015-12-30</td><td class=" center">92</td><td class=" center">轻度污染</td><td class=" center">高压楔</td><td class=" center"></td><td class=" center"></td><td class=" center"></td><td class=" center"></td><td class=" center"></td><td class="center"></td></tr><tr><td colspan="10">asdsfgdfh</td></tr>');
            }
        },
        "columns": [
            { "title": "日期", "class": "center" },
            { "title": "PM2.5浓度", "class": "center" },
            { "title": "等级", "class": "center" },
            { "title": "天气类型", "class": "center" },
            { "title": "地面形势场", "class": "center", width: 200 },
            { "title": "主导风向(00-24时)", "class": "center" },
            { "title": "平均风速(00-24时)", "class": "center" },
            { "title": "能见度", "class": "center" },
            { "title": "天气现象", "class": "center" },
            { "title": "云量", "class": "center"}],
        "oLanguage": {
            "sProcessing": "<img src='../images/loading.gif?v=2'/>正在查询...",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sSearch": "表内搜索：",
            "sEmptyTable": "表中无数据存在！"
        }
    });

    $('#resultTbl tbody').on('click', 'tr', function () {
        $('#resultTbl .selected').removeClass("selected");
        $(this).addClass("selected");
        var data = tbl.row(this).data();
        QueryChart(data[0]);
        GetImg(data[0]);
    });
}

function GetImg(date) {
    $.ajax({
        url: "Manager.aspx/GetImg",
        data: "{date:'" + date + " 20:00:00'}",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows;
            var strHtml = "";
            for (var i = 0; i < datas.length; i++) {
                strHtml += "<img src='../" + datas[i].FOLDER + "/" + datas[i].NAME + "'>";
            }
            $("#divImg").html(strHtml);

        },
        error: function (ex) {
//            alert("异常，" + ex.responseText + "！");
        }
    });
}

function QueryChart(strDate) {
    $.ajax({
        url: "Retrieval.aspx/QueryHour",
        type: "POST",
        contentType: "application/json",
        data: "{date:'" + strDate + "',field:'PM2_5'}",
        dataType: 'json',
        success: function (results) {
            var series = JSON.parse(results.d);
            $("#chartBack").highcharts({
                chart: { type: 'spline'},
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
