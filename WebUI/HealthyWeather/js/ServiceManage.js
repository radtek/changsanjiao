var serviceId = "";
$(function () {
    getSelVal();
    query();
})

function query() {
    var region="";
    $('#serviceTab').empty();
    var table = $('#serviceTab').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "order": [[6, "desc"]],
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.ServiceManage&method=serviceQuery",

        "columns": [
            { "title": "id", "class": "center"},
            { "title": "地址", "class": "center" },
            { "title": "区域", "class": "center","width":350},
            { "title": "产品", "class": "center","width":350},
            { "title": "密钥", "class": "center"},
            { "title": "接收单位", "class": "center" ,"width":150 },
            { "title": "创建时间", "class": "center","width":150},
            { "title": "编辑", "class": "center","width":50 },
            { "title": "删除", "class": "center", "width": 50 },
            
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
            "oPaginate": {
                "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页"
            }
        },
        "columnDefs": [
         {
             "targets": 0,
             "visible": false
         },
         {
             "targets": 7, "data": null,
             "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
         }, {
             "targets": 8, "data": null,
             "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
         }],
    });
    $("#serviceTab tbody").on('click', 'img', function (obj) {
        var data = table.row($(this).parents('tr')).data();
        if (this.title == "删除") {
            if (confirm("是否要删除？")) delService(data[0]);
        }
        else {
            create_Service("编辑服务接口", data);
        }
    });
}

function getSelVal() {
    var arr = ["原中心城区","中心城区", "原浦东","浦东新区", "闵行区", "宝山区", "松江区", "金山区", "青浦区", "奉贤区", "嘉定区", "崇明"];
    var html = "";
    for (var i = 0; i < arr.length; i++) {
        html += "<option value='" + arr[i] + "'>" + arr[i] + "</option>";
    }
    $("#region").html(html);
    $('#region').selectpicker({
        'selectedText': 'cat',
        'noneSelectedText': '==请选择=='
    });
}
function create_Service(title, info) {
    var idName = ["address", "region", "key","receiver", "product"]
    $("#mymodal").modal("show");
    $(".modal-body").css('height', '273px');
    if (title.indexOf("编辑") > -1) {
        serviceId = info[0];
        $("#title").text("编辑服务接口");
        var type = "";    //产品类型
        var arr=[];     //将info转成数组用来下拉多选使用
        for (var i = 0; i < idName.length; i++) {
            if (i == 0||i==1) {
                $("#" + idName[i]).val(info[i + 1]);
                arr=info[i+1].split(',');
                $("#" + idName[i]).selectpicker('val',arr);
            }
            else if (i == 2 || i==3) {
                $("#" + idName[i]).val(info[i+2]);
            }
            else if (i == 4) {
                type = info[3].split('_');
                var num = "";
                for (var j = 0; j < type.length; j++) {
                    switch (type[j]) {
                        case "儿童感冒": num += "2,"; break;
                        case "青年感冒": num += "3,"; break;
                        case "老年感冒": num += "4,"; break;
                        case "COPD": num += "5,"; break;
                        case "儿童哮喘": num += "6,"; break;
                        case "中暑": num += "7,"; break;
                        case "重污染": num += "8,"; break;
                    }
                }
                num = num.substring(0, num.length - 1);
                type = num.split(',');
                $("input:checkbox").prop("checked", false);
                for (var j = 0; j < type.length; j++) {
                    $("input:checkbox[value='" + type[j] + "']").prop("checked", true);
                }
            }
        }
    }
    else {
        $("#region").selectpicker('val', "");
        $("#title").text("创建服务接口");
        serviceId = "";
        for (var i = 0; i < idName.length; i++) {
            $("#" + idName[i]).val("");
            $("input:checkbox").prop("checked", false);
        }
    }
}
function delService(data) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ServiceManage', 'delService'),
        params: { s_id: data },
        success: function (response) {
            $("#mymodal").modal("hide");
            if (response.responseText == "成功") {
                alert("删除成功");
            }
            else {
                alert(response);
            }
            query();
        }
    });
}

function confirm_create() {
    var title = $("#title").text();
    if (title.indexOf("创建") > -1) {   //如果大于-1则说明是要创建服务接口，则要提示创建成功，否则是编辑成功
        var tip = "创建成功";
    } else {
        tip = "编辑成功";
    }
    
    var address = $("#address").val();
    var region = $("#region").val();
    var key = $("#key").val();
    var type = document.getElementsByName("type");
    var productValue = [];
    for (var i = 0; i < type.length; i++) {
        if (type[i].checked) {
            productValue.push(type[i].value);
        }
    }
    var receiver = $("#receiver").val();
    if (key == "") {
        alert("密钥不能为空！"); return;
    }
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ServiceManage', 'confirm_create'),
        params: {id:serviceId,address: address, region: region, product: productValue, key: key,title:title,receiver:receiver },
        success: function (response) {
            $("#mymodal").modal("hide");
            if (response.responseText == "成功") {
                alert(tip);
            }
            else {
                alert(response);
            }
            query();
        }
    });
}