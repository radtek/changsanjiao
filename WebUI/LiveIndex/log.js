$(function () {
    getPeople();
    var date = new Date();
    var sDate = date.Format("yyyy-MM-dd");
    var eDate = date.Format("yyyy-MM-dd");
    $("#startTime").val(sDate);
    $("#endTime").val(eDate);
})

//点击查询
function query() {
    getTable();
}
//获取人员
function getPeople() {
    $.ajax({
        url: "log.aspx/GetPeople",
        type: "POST",
        contentType: "application/json",
        dataType: "JSON",
        success: function (results) {
            var data = results.d.rows;
            var html = "<option value=全部>全部</option>";
            for (i = 0; i < data.length; i++) {
                html += "<option value=" + data[i].alias + ">" + data[i].alias + "</option>";
            }
            $("#people").html(html);
            getTable();
        }
    });
}
var oTable =null;
function getTable() {
    //右边要素信息表格清空
    var startTime = $('#startTime').val()+" 00:00:00";
    var endTime = $("#endTime").val() + " 23:59:59";
    var people = $("#people").val();
    var fun = $("#fun").val();
    $('#table').empty();
    oTable = $('#table').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "bPaginate": false,
        "searching": false,
        "bAutoWidth": false,
        "ordering":false,
        "info": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=GetLogTable&startTime=" + encodeURI(startTime) + "&endTime=" + encodeURI(endTime) + "&people=" + encodeURI(people) + "&fun=" + encodeURI(fun)+"",
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