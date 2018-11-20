$(function () {
    InitDisease();
    var dtNow = new Date();
    $("#iptEnd").val(dtNow.getFullYear() + "-" + (dtNow.getMonth() + 1) + "-" + dtNow.getDate());
    $("#iptStart").val(dtNow.getFullYear() + "-" + (dtNow.getMonth()+1) + "-" + dtNow.getDate());
//    $("#chkSendAll").change(function () {
//        if ($(this).is(':checked'))
//            $("#selDiseaseType").attr("disabled", true);
//        else
//            $("#selDiseaseType").removeAttr("disabled");
//    });
})

function InitDisease() {
    $.ajax({
        url: "SendLog.aspx/GetHealthyType",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows, html = "<option>全部</option>";
            for (var i = 0; i < datas.length; i++) {
                html += "<option>" + datas[i].MC + "</option>";
            }
            $("#selDiseaseType").html(html);
            Query();
        },
        error: function (ex) {
            //alert("异常，" + ex.responseText + "！");
        }
    });    
}

function Query() {
    //var Alias = "<%=m_alias %>"
//    var loginParams = getCookie("UserInfo");
//    var logResult = loginParams.split(","); //{Alias:'管理员'
//    var Alias = logResult[0].replace("{Alias:'", "").replace("'", "");
    var start = $("#iptStart").val();
    var end = $("#iptEnd").val();
    var isAll = $("#chkSendAll").is(':checked') ? 1 : 0;
    if (!start || !end) { alert("开始或结束时间不能为空！"); return; }
    var sendType1 = $("#sendType").val();
    //    var healthyType = isAll ? "" : $("#selDiseaseType").val();
    var healthyType = $("#selDiseaseType").val();
    var sendStatus = $("#selSendType").val();
    $('#logTbl').empty();
    var tbl = $('#logTbl').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "order": [[7, "desc"]],
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.HealthyWeather&method=QueryLog&IsAll=" + isAll + "&healthyType="
        + encodeURI(healthyType) + "&sendStatus=" + sendStatus + "&start=" + start + "&end=" + end + "&sendType=" + sendType1 + "&Alias=" + Alias,
        "columns": [
            { "title": "发送人", "class": "center" },
            { "title": "接收人", "class": "center" },
            { "title": "发送类型", "class": "center" },
            { "title": "疾病类型", "class": "center" },
            { "title": "邮件地址", "class": "center" },
            { "title": "电话号码", "class": "center" },
            { "title": "发送状态", "class": "center" },
            { "title": "发送时间", "class": "center", "width":150},
            { "title": "是否一键发送", "class": "center" }
            ],
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>",
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sInfoEmpty": "当前显示 0 到 0 条记录，共 0 条",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "<span style='float:left; margin-left:15px;'>搜索&nbsp</span>",
            "oPaginate": { "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页"
            }
        }
    });
}