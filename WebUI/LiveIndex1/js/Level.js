var m_left = 0;  //弹出层的margin-left值
var m_top = 0;
$(function () {
    m_left = $("#myModal").width() / 3.5;
    m_top = $("#myModal").height() / 16;
    displayData();
    //$("#table .details-control").css("width","50px");
    $("#tijiao").click(_submit);  //model中的提交
    drag($(".modal-header"), $(".modal-dialog"));
});

function displayData() {
    var region = "";
    $('#table').empty();
    var table = $('#table').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        //"order": [[3, "desc"]],
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "searching": false,
        "bLengthChange": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=GetData",
        "columns": [
             {
                 "class": 'details-control',
                 "orderable": false,
                 "data": null,
                 //"defaultContent": '',
                 "width":"50px"
             },
            { "title": "主级别","class":"text-l" }
        ],
        "aoColumnDefs": [{
            //设置第一列不排序
            "bSortable": false,
            "aTargets": [0]
        }],
        "columnDefs": [
         {
             "targets": [1],
             "visible": false,
             "searchable": false
         }],
        "fnCreatedRow": function(nRow, aData, iDataIndex) {
            $('td:eq(0)', nRow).html("<span class='row-details row-details-close' data_id='" + aData[0] + "'></span>");
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
    $('.tab tbody').on('dblclick', 'tr', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        statusChange(row,tr);
    });
    $('.tab tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        statusChange(row,tr);
    });
}

function statusChange(row,tr) {
    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        // Open this row
        fnFormatDetails(row.data()[0], row);
        tr.addClass('shown');
    }
}
//获取详细信息
function fnFormatDetails(data, row) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetDataDetails'),
        params: { code: data },
        success: function (response) {
            details = response.responseText;
            row.child(details).show();
        },
        failure: function (response) {
            alert("保存失败，请仔细检查");
        }
    });
}

function getModelSel() {
    var arr1 = [1, 2, 3, 4, 5];  //主级别   级别
    var arr2 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]   //子级别
    var html = "", html_sub = "";
    for (var i = 0; i < arr1.length; i++) {
        html += "<option value='"+arr1[i]+"'>"+arr1[i]+"</option>";
    }
    for (var j = 0; j < arr2.length; j++) {
        html_sub += "<option value='" + arr2[j] + "'>" + arr2[j] + "</option>";
    }
    $("#subjb").html(html_sub);
    $("#jb").html(html);
    $("#mainjb").html(html);
}

//双击显示模态框
function showModel(obj) {
    getModelSel();
    var id = ["mainjb", "jb", "subjb", "grademean", "gradeparam", "textlevelname", "scopelower", "scopeexplain", "tip"];
    var data = "";   //行数据
    var value = [];
    
    clear(id);    //先清空
    var zhishu = $(".shown .text-l").text();
    for (var i = 0; i < obj.children().length; i++) {
        data += obj.children().eq(i).text() + "#";
    }
    value = data.split('#');    //分割成数据给model中每一项赋值
    for (var j = 0; j < id.length; j++) {
        var a = value[j];
        $("#" + id[j]).val(a);
    }
    $("#myModal").modal('show');
    $('.modal-dialog').css({ "margin-left": m_left, "margin-top": m_top });   //设置弹出层的位置，不设置就会与上次的位置相同（上次移动之后的位置）
    $("#zs").val(zhishu);
}
function clear(obj) {
    $("#zs").val("");
    for (var i = 0; i < obj.length; i++) {
        $("#" + obj[i]).val("");
    }
}

function _submit() {
   // var zhishu = $("#zs").val();
    var code = $(".shown .row-details").attr("data_id");
    var jibie = $("#jb").val();
    var mainjibie = $("#mainjb").val();
    var subjibie = $("#subjb").val();
    var mean = $("#grademean").val();
    var param = $("#gradeparam").val();
    var explain = $("#textlevelname").val();
    var scopelimit = $("#scopelower").val();
    var scopexplain = $("#scopeexplain").val();
    var tip = $("#tip").val();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'Submit'),
        params: { code: code,jibie:jibie,mainjibie:mainjibie, subjibie:subjibie,mean:mean,param:param,explain:explain,scopelimit:scopelimit,scopexplain:scopexplain,tip:tip },
        success: function (response) {
            if (response.responseText == "ok") {
                alert("保存成功");
            } else {
                alert("保存失败，请仔细检查");
            }
            $("#myModal").modal('hide');
            displayData();

        },
        failure: function (response) {
            alert("保存失败，请仔细检查");
        }

    });
}