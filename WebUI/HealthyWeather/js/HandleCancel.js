$(function () {
    Query();
})

function Query() {
    $('#tblHandle').empty();
    var tbl = $('#tblHandle').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true, "bPaginate": false,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.HealthyWeather&method=GetCancel",
        "columns": [
            { "visible": false },
            { "visible": false },
            { "title": "用户姓名", "class": "center" },
            { "title": "电话", "class": "center" },
            { "title": "邮件", "class": "center" },
            { "title": "申请类型", "class": "center" },
            { "title": "申请时间", "class": "center"},
            { "title": "同意", "class": "center", "orderable": false,"width": 10 },
            { "title": "否决", "class": "center", "orderable": false,"width": 10 }],
        "columnDefs": [{
            "targets": 7, "data": null,
            "defaultContent": "<img src=\"images/success.png\" title=\"同意\" style=\"cursor: pointer\"></img>"
        }, {
            "targets":8, "data": null,
            "defaultContent": "<img src=\"images/fail.png\" title=\"否决\" style=\"cursor: pointer\"></img>"
        }],
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>",
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sInfoEmpty": "当前显示 0 到 0 条记录，共 0 条",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "用户搜索",
            "oPaginate": { "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页"
            }
        }
    });

    $('#tblHandle tbody').on('click', 'img', function () {
        var data = tbl.row($(this).parents('tr')).data();
        if (this.title == "同意") {
            if (confirm("确认要以后不再发送" + data[5] + "到这个用户么？")) Agree(data[0], data[1], data[5] == "邮件" ? "EMAIL" : "MESSAGE");
        }
        else {
            if (confirm("确认要拒绝这个用户的申请么？")) Reject(data[0]);
        }
    });
}

function Agree(id, userId, type) {
    $.ajax({
        url: "HandleCancel.aspx/AgreeRequest",
        type: "POST",
        contentType: "application/json",
        data: "{id:'" + id + "',userId:'"+userId+"',type:'"+type+"'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "1") Query();
            else alert("操作失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}

function Reject(id) {
    $.ajax({
        url: "HandleCancel.aspx/Reject",
        type: "POST",
        contentType: "application/json",
        data: "{id:'" + id + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "1") Query();
            else alert("操作失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}
