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
    var status = $("#status").val();
    $('#table').empty();
    oTable = $('#table').DataTable({
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
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=GetLogTable&startTime="
            + encodeURI(startTime) + "&endTime=" + encodeURI(endTime) + "&people=" + encodeURI(people) + "&fun=" + encodeURI(fun) + "&status=" + encodeURI(status) + "",
        "columns": [
            { "title": "操作人", "class": "center" },
            { "title": "操作时间", "class": "center" },
            { "title": "功能模块", "class": "center" },
            { "title": "操作说明", "class": "center" },
            { "title": "是否成功", "class": "center" },
            { "title": '查看',"width":"30px"},
        ],
        "columnDefs": [{
            "targets": 5, "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"查看\" style=\"cursor: pointer\"></img>"
        }],
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
        },
    });
    $("table tbody").on("click", 'tr img', function () {
        var trData = oTable.row($(this).parents('tr')).data();
        getLogTxt(trData);
    });
}
function getLogTxt(trData) {
    initModal();
    $("#myModal").modal('show');
    var sendDate = trData[1];
    var sendFileName = trData[3];
    $("#txt").text("");
    $.ajax({
        url: "log.aspx/GetLogTxt",
        type: "POST",
        contentType: "application/json",
        data: "{sendDate:'" + sendDate + "',option:'" + sendFileName + "'}",
        dataType: 'json',
        success: function (results) {
            var _data = results.d;
            var content = [];
            if (_data.length > 0) {
                var data = _data.split("#");
                $(".tab").html("<ul class='nav nav-tabs'></ul>");
                for (i = 0; i < data.length; i++) {
                    var txt = data[i].split("$")[0];
                    var fileName = data[i].split('$')[1];
                    content.push(txt);
                    var a="12";
                    $("<li class='item" + (i + 1) + "'><a href='#'>" + fileName + "</a></li>").appendTo(".nav");
                }
                $("#txt").text(content[0]);
                $(".item1").addClass("active");
            }
            $("#myModal li").click(function () {
                $("#myModal li").removeClass("active");
                var strIndex = $(this).attr("class");
                var index = strIndex.substring(4,strIndex.length)-1;
                $(this).addClass("active");
                $("#txt").text(content[index]);
                console.log(this);
            });
        },
        error: function () {
            $("#txt").text("错误！");
        }
    });
}

function initModal() {
    var pageW = $(window).width();
    var pageH = $(window).height();
    console.log(pageH);
    var modalW = "";
    var modalH = 550;
    if (parseFloat(pageH) < 750) {
        modalH = pageH - 210;
    }
    $("#txt").height(modalH);
}