$(function () {
    getData();
});
function getData() {
    $('#orgTab').empty();
    var orgTable = $('#orgTab').DataTable({
        "bProcessing": true,
        "searching": false,
        "bLengthChange":false,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "order": [[0,"asc"]],
        "bFilter": true,
        "bPaginate": false,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.OrgManage&method=GetData",
        "columns": [
            {"visible":false},
            { "title": "区域", "class": "center" },
            { "title": "机构名称", "class": "center" },
            { "title": "编辑", "class": "center","width":90 },
            ],
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
        },
        "columnDefs": [{
            "targets": 3,
            "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }]
    });
    $('#orgTab tbody').on('click', 'img', function (obj) {
        var myData = orgTable.row($(this).parents("tr")).data();
        if (this.title == "编辑") {
            edit(myData);
           
        }
    });
    
}

//删除
/*
function delOrg(id) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.orgManage', 'delOrg'),
        params: {idd:id},
        success: function () {
            alert("你已经成功删除了这条信息！");
        },
        failure: function () {
            alert("删除失败！");
        }
    });
    getData();
}
*/
//编辑
function edit(info) {
    $('#myModal').modal('show').css({
        width: 'auto',
        'margin-left': function () {
            return -($(this).width() /7);
        }
    });

    var obj = ["idd", "region", "orgName"];
    for (var i = 0; i < obj.length; i++) {
        $("#" + obj[i]).val(info[i]);
    }
}

//modal框中按确认键所触发的函数
function queren() {
    var idd = $("#idd").val();
    var region = $("#region").val();
    var orgName = $("#orgName").val();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.OrgManage', 'edit'),
        params: { idd: idd, regions: region, orgName: orgName },
        success: function (response) {
            $("#myModal").modal("hide");
            alert("保存成功！");
            getData();
        },
        failure: function (response) {
            alert("保存失败，请仔细检查");
        }
    });
}