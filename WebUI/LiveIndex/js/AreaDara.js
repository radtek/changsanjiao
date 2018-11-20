var flag = "";   //标识所提交的内容是增加还是修改
var m_left = 0;  //弹出层的margin-left值
var m_top = 0;
var orgTable = "";
$(function () {
    m_left = $("#myModal").width() / 3;
    m_top = $("#myModal").height() / 8;
    getData();
    $('#J_insert').on('click', insert);
    var Del = clicktr();     //使用闭包函数
    $('#J_del').on('click', Del);
    drag($(".modal-header"), $(".modal-dialog"));
});

function getData() {
    $('#orgTab').empty();
    orgTable = $('#orgTab').DataTable({
        "bProcessing": true,
        "searching": false,
        "bLengthChange": false,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        //"order": [[0, "asc"]],
        "ordering": false,
        "bFilter": true,
        "bPaginate": false,
        "bSortClasses": false,
        // "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.OrgManage&method=GetData",
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=AreaData",
        "columns": [
            { "title": "站点编号", "class": "center" },
            { "title": "站点说明", "class": "center" },
            { "title": "主要站点", "class": "center" },
        //{ "title": "编辑", "class": "center","width":90 },
            ],
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            if (aData[2] == "True") {
                $(nRow).find("td").eq(2).text("是");
            } else {
                $(nRow).find("td").eq(2).text("非");
            }
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
            "oPaginate": { "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页" }
        }
    });
    $('#orgTab tbody').on('dblclick', 'tr', function (obj) {
        //var myData = orgTable.row($(this)).data();
        var myData = this.outerText.split("	");
        edit(myData);
    });
    $('#orgTab tbody').on('click', 'tr', clicktr);
}

function clicktr() {
    cancelBubble();
    del_id = "";
    if (this.outerText) {
        del_id = this.outerText.split('	')[0];
    }
    $('#orgTab tbody tr').removeClass('selected');
    $(this).addClass('selected');
    function del() {
        cancelBubble();
        if (!del_id || $('#orgTab tbody tr.selected').length==0) {
            alert("请选择要删除的数据！");
            return;
        }
        if (!confirm('是否要删除？')) return;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.LiveIndex', 'Del'),
            params: { idd: del_id },
            success: function (response) {
                alert("删除成功！");
                orgTable.row('.selected').remove().draw(false);
                //getData();
                del_id = "";
            },
            failure: function (response) {
                alert("失败！");
            }
        });
    }
    return del;
}

function edit(info) {
    flag = "0";
    $('#idd').attr('disabled', true);
    $('#myModal').modal('show');
    $('.modal-dialog').css({ "margin-left": m_left, "margin-top": m_top });   //设置弹出层的位置，不设置就会与上次的位置相同（上次移动之后的位置）
    var obj = ["idd", "region", "orgName"];
    for (var i = 0; i < obj.length; i++) {
        $("#" + obj[i]).val("");
        $("#" + obj[i]).val(info[i]);
    }
}
function insert() {
    cancelBubble();
    flag = "1";
    $('#idd').attr('disabled', false);
    $('#myModal').modal('show');
    $('.modal-dialog').css({ "margin-left": m_left, "margin-top": m_top });
    var obj = ["idd", "region"];
    $('#orgName').val('是');
    for (var i = 0; i < obj.length; i++) {
        $("#" + obj[i]).val("");
        $("#" + obj[i]).attr('placeholder',"该项不能为空");
    }
}

//modal框中按确认键所触发的函数
function queren() {
    var tip = "修改成功！";
    if (flag == "1") {
        tip = "添加成功！";
    }
    var idd = $("#idd").val();
    var region = $("#region").val();
    var orgName = $("#orgName").val();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', '_Submit'),
        params: { idd: idd, region: region, orgName: orgName,flag:flag},
        success: function (response) {
            $("#myModal").modal("hide");
            alert(tip);
            getData();
        },
        failure: function (response) {
            alert("失败");
        }
    });
}

//function del() {
//    var idd = myData[0];
//    Ext.Ajax.request({
//        url: getUrl('MMShareBLL.DAL.LiveIndex', 'Del'),
//        params: { idd: idd },
//        success: function (response) {
//            alert("删除成功！");
//            getData();
//        },
//        failure: function (response) {
//            alert("失败！");
//        }
//    });
//}
$(document).click(function () {
    $('#orgTab tbody tr').removeClass("selected");
});
function cancelBubble(e) { 
    var evt = e ? e : window.event;
    if (evt.stopPropagation) {
        //W3C 
        evt.stopPropagation();
    }
    else {
        //IE 
        evt.cancelBubble = true;
    }
}

function drag(current,move) {
    current.on("mousedown", function (event) {
        $(current).css("cursor", "move");
        var evt = event || window.event;
        var offset = $(move).offset();
        var dialogX = offset.left;
        var dialogY = offset.top;
        var mx = evt.pageX;
        var my = evt.pageY;
        $(document).on("mousemove", function (evt) {
            var evt = evt || window.event;
            var x = evt.pageX;
            var y = evt.pageY;
            var moveX = dialogX + (x - mx);
            var moveY = dialogY + (y - my);
            $(move).css({ "margin-left": moveX, "margin-top": moveY });
            window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
        });
    });
    $(document).on("mouseup", function () {
        $(current).css("cursor", "default");
        $(document).off("mousemove");
        
    })
}
