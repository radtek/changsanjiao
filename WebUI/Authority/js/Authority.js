var name; //用户名
var user;  //姓名
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    //    initInputHighlightScript();
    
}
)

$(function () {
    ShowUser();
    ShowroTree("");
})

//检测窗口大小动态设置User高度
//方法一（未遂，有滚动条）
//window.onresize = function () { setheight(); };
//function setheight() { $("#User tbody").height($(window).height() - 130); }
//方法二（未遂，没反应，放入初始页面加载函数也没用）
//var height = $(window).height() - 130;
//window.onresize = function () { height = $(window).height() - 130; };
//方法三（未遂，似乎不能这样对DataTable单个属性设置）
//window.onresize = function () { $("#User").DataTable('sScrollY', $(window).height() - 130); };
function ShowUser() {
    $("#User").empty();
    user_table = $("#User").DataTable({
        "bProcessing": true,
        "sScrollY": $(window).height() - 130,
        "sScrollYInner": "110%",
        "bScrollCollapse": true,
        "bFilter": true,
        "bPaginate": false,
        "sAjaxSource": "./PatrolHandler.do?provider=MMShareBLL.DAL.AuthoritySys&method=ShowUser",
        "columns": [
            { "title": "姓名", "class": "center"},
            { "title": "用户名", "class": "center" }],
        "oLanguage": {
            "sProcessing": "<img src='./css/icons/loading.gif'/></br>",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sInfoEmpty": "当前显示 0 到 0 条记录，共 0 条",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "用户搜索"
        }
    })
    $('#edit').hide();
    $('#User tbody').on("click", "tr", function () {
      $('#User .selected').removeClass("selected");
        $(this).addClass("selected");
        $('#edit').show();
        name = this.cells[1].innerHTML;
        user = this.cells[0].innerHTML;
        ShowroTree(name);
    })
}

//点击表格内数据展示权限树
function ShowroTree(user) {
    if (user != "") { $("#tree").treegrid("clearChecked"); }
    $("#tree").treegrid({
        "url": "./PatrolHandler.do?provider=MMShareBLL.DAL.AuthoritySys&method=ShowReadOnlyTree&UserName=" + user,
        "method": "post",
        "onContextMenu":function(e,row){

        },
        
        "idField": "id",
        "treeField": "name",
        "checkbox": true,
        "rownumbers": true,
        "singleSelect": false,
        //不支持的属性
        //"selectOnCheck": true,
        "checkOnSelect": true,
        //"animate":true,
        columns: [[
                  { field: 'name', title: '名称',width:'100%'},
                  { field: 'id', title: 'id' }]],
        
    });
    $('#tree').datagrid('hideColumn', 'id');
}

function compare(){
    return function(a,b){
        var al=a.length;
        var bl=b.length;
        return al-bl;
    }
}

function cancel() {
    ShowroTree("1");
    $("#User tr:contains(" + name + ")").removeClass('select');
    $('#edit').hide();
}

//wb 2017.6.23
function editAuthority() {
    var mk = new Ext.LoadMask(document.body, {
        msg: '正在更新数据，请稍候！',
        removeMask: true //完成后移除  
    });
    mk.show();
    var checkBox1 = $(".tree-checkbox1").parent().parent().next();
    var authority = "";
    var auth = "";
    var temp = "";
    var arr = [];
    for (var i = 0; i < checkBox1.length; i++) {
        temp = checkBox1[i].innerText;
        arr = temp.split("-");
        if (arr[0] == "webGIS") {
            authority += temp + "-" + temp + "-" + temp + "-" + temp + "/";
        }
        if (arr.length >= 4) {
            //if (arr.length == 5 || (arr.length == 4 && arr[3] != "后台管理")) {
                authority += temp + "/";
            //}
        }
    }


    authority = authority.substring(0, authority.length - 1)
    if (confirm("确定要修改" + user + "的权限?")) {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AuthoritySys', 'EditAuthority'),
            params: { name: name, authorities: authority },
            success: function (response) {
                alert("权限更改成功！");
                mk.hide();
            },
            error: function (ex) {
                alert("异常，" + ex.responseText + "！");
            }
        });
    }
}