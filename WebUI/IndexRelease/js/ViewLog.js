$(function () {
    //动态改变select框与input框的宽度一样，input有padding值
    var rqWidth = $("#startTime").width();
    $("#module").width(rqWidth);
    $("#people").width(rqWidth);
    var dNow = new Date().Format("yyyy-MM-dd");
    $("#startTime").val(dNow);
    $("#endTime").val(dNow);
    getPeople();
})

//获取人员
function getPeople() {
    $.ajax({
        url: "ViewLog.aspx/GetPeople",
        type: "POST",
        contentType: "application/json",
        dataType: "JSON",
        success: function (results) {
            var data = results.d.rows;
            var html = "<option value='全部'>全部</option>";
            for (i = 0; i < data.length; i++) {
                html += "<option value='" + data[i].Alias + "'>" + data[i].Alias + "</option>";
            }
            $("#people").html(html);
            query();
        }
    });
}

//查询
function query() {
    var startTime = $("#startTime").val() + " 00:00:00";
    var endTime = $("#endTime").val() + " 23:59:59";
    var operator = $("#people").val();
    var module = $("#module").val();
    var status=$("#sucess").val();
    $('#table').empty();
    var oTable = $('#table').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true,
        "bSortClasses": false,
        "bPaginate": false,
        "searching": false,
        "bAutoWidth": false,
        "ordering":false,
        "info": true,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.IndexRelease&method=QueryLog&startTime="
            + encodeURI(startTime) + "&endTime=" + encodeURI(endTime) + "&people=" + encodeURI(operator) + "&fun=" + encodeURI(module) + "&status=" + encodeURI(status) + "",
        "columns": [
            { "title": "操作人","class":"center" },
            { "title": "操作时间", "class": "center" },
            { "title": "功能模块", "class": "center" },
            { "title": "操作说明", "class": "center" },
            { "title": "是否成功", "class": "center" }
        ],
        "aoColumnDefs": [{
        }
        ],
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
        },
        "fnInitComplete": function (oSettings, json) {
            
        },
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>",
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sInfoEmpty": "当前显示 0 到 0 条记录，共 0 条",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "用户搜索",
            "oPaginate": {
                "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页"
            }
        }
    });
}
