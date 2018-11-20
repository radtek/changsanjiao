var titleMartop = 57;
var arr_id = ["userName", "phone", "age", "gender", "education", "company", "occupation", "postCode", "address", "phoneStatus"];
var m_left = 0;  //弹出层的margin-left值
var m_top = 0;
var tabW = "";   //模态框弹出之前，记录整个表格的宽度，然后在赋值，负责会出现闪动
var addButtonL = ""; //模态框弹出之前，记录添加用户按钮的宽度，
$(function () {
    $("body").width(($(document).width()-30)+"px");
    m_left = $(window).width() / 3.5;
    m_top = ($(window).height() - $(".modal-dialog").height()) / 20;
    //getMessUser();
    $("#J_submit").on("click", submit);
    drag($(".modal-header"), $(".modal-dialog"));
    getCompany();
});
function getCompany() {
    $.ajax({
        url: "MessageManage.aspx/GetCompany",
        type: "POST",
        //data: "{id:'" + id + "'}",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var data = results.d.rows;
            var html = "<select id='firm'><option value='全部'>全部</optopn>";
            for (i = 0; i < data.length; i++) {
                html += "<option value='" + data[i].company + "'>" + data[i].company + "</option>";
            }
            html += "</select>";
            getMessUser(html);
        }
    });
}

//第一次进入页面获取数据
function getMessUser(htmlCompany) {
    var htmlRadio = '是否启用：<input type="radio" name="1" id="yes" value="1">是</label><label><input style="margin-left:15px;" type="radio" name="1" id="no" value="0"></label>否<label>';
    $('#table').empty();
    var table = $('#table').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true,
        "bSortClasses": false,
        "bPaginate": false,
        "ordering": false,
        "searching": true,
        "bLengthChange": true,
        "bAutoWidth": true,
        "info":true,
        "lengthMenu": [
            [30, 50, 100, -1],
            ['30', '50', '100', '全部']
        ],
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=GetMessUser",
        "columns": [
           { "visible": false },
            { "title": "姓名", class: "_title", "width": "50px" },
            { "title": "手机", class: "_title", "width": "75px" },
            { "title": "年龄", class: "_title", "width": "40px" },
            { "title": "性别", class: "_title", "width": "40px" },
            { "title": "学历", class: "_title", "width": "50px" },
            { "title": "单位：" + htmlCompany, class: "_title firm", "width": "150px" },
            { "title": "职称", class: "_title", "width": "80px" },
            { "title": "邮编", class: "_title", "width": "60px" },
            { "title": "地址", class: "_title", "width": "60px" },
            { "title": htmlRadio, class: "_title start", "width": "150px" },
            { "title": "编辑", class: "_title", "width": "30px" },
            { "title": "删除", class: "_title", "width": "30px" },

        ],
        "columnDefs": [{
            "targets": 11, "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 12, "data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
        }],
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            callRow($(nRow).find('td').eq(3), "1", "2", "女", "男");
            callRow($(nRow).find('td').eq(9), "1", "0", "是", "否");
        },
        "fnInitComplete": function (oSettings, json) {
            $("#table_length").css("display","block");
            //changeSelPositio(0);
            $('#table_filter input[type="search"]').addClass('defineform');
            //改变每页显示多少行时触发，让改框随着表格变动
           // $(".dataTables_length select").change({ val: -3 }, changeSelPositio);
            //单击第几页时触发，与上面相同
           // $("#table_paginate").click({ val: -3 }, changeSelPositio);
            $("#yes").attr("checked",true);
            $("table :radio").click(function () {
                clickRadio($(this).attr("id"),"phone");
            });
            clickRadio("yes","phone");
            $("#firm").on("change", function () { clickRadio($(this).val(), "company"); });
            tabW = $(".tab").width();    //模态框弹出之前，记录整个表格的宽度，然后在赋值，负责会出现闪动
            addButtonL = $(".head .add").css("left");
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
   
    $("table tbody").on("click", 'tr img', function () {
        var trData = table.row($(this).parents('tr')).data();
        if (this.title == "删除") {
            //删除操作
            delUser(trData[0]);
        } else {
            //编辑操作
            editUser(this.title,trData);
        }
    });
}
function clickRadio(id, type) {
    var num = 0;    //当前页面显示了多少条数据
    var sum = 0;   //记录总共有多少条数据
    var txt = id, selector = "#table tbody tr .firm", dis = "firmdis";
    if (txt == "无") txt = "";
    if (type == "phone") {
        dis = "phonedis"
        selector = "#table tbody tr .start";
        txt = "是";
        if (id == "no") {
            txt = "否";
        }
    }
    $(selector).each(function (index, element) {     //遍历所有的行
        sum++;
        $(element).parents("tr").addClass(dis);
        if (txt == "全部") {
            $(element).parents("tr").removeClass(dis);
        }
        else if ($(this).text() == txt) {
            $(element).parents("tr").removeClass(dis);
        }
        if ($(this).parents("tr").css("display") != "none") {    //该行如果是显示则num+1
            num++;    
        }
    })
    $("#table_info").text("当前显示 "+num+" 条，共 "+sum+" 条记录");
}
function changeSelPositio(e) {
    var val = "";
    if (e.data == undefined) {
        val = e;
    } else {
        val = e.data.val;
    }
    var top = $(".dataTables_info").offset().top +val;
    var left = $(".dataTables_info").offset().left + $(".dataTables_info").width();
    $('.dataTables_length').css({ "top": top, "left": left });
}

//每一行创建完成执行这个函数，把相对应列的数字值转换成值所代表的含义
function callRow(id,val1,val2,str1,str2) {
    if ($(id).text() === val1) {
        $(id).text(str1);
    } else if ($(id).text() === val2) {
        $(id).text(str2);
    } else {
        $(id).text("");
    }
}

//删除用户的函数
function delUser(id) {
    if (!confirm("是否要删除该用户？")) return;
    $.ajax({
        url: "MessageManage.aspx/DelUser",
        type: "POST",
        data: "{id:'" + id + "'}",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            alert("删除成功！");
            getCompany();;
        },
        error: function (ex) {
            alert("失败！");
            getCompany();;
        }
    });
}

//编辑用户的函数
function editUser(title, data) {
    $(".tab").width(tabW);
    $(".head .add").css("left", addButtonL);
    clear();
    if (title == "编辑") {
        $('#myModalTitle').text("编辑用户");
        for (var i = 0; i < arr_id.length; i++) {
            $("#" + arr_id[i]).val(data[i+1]);
        }
    } else {
        $('#myModalTitle').text("添加用户");
    }
    $("#myModal").modal('show');
    $('.modal-dialog').css({ "margin-left": m_left, "margin-top": m_top });   //设置弹出层的位置，不设置就会与上次的位置相同（上次移动之后的位置）
    $("#userName").attr("userId",data[0]);   //点击编辑或删除时将这个用户的id作为自定义属性添加到姓名框中，作为sql语句的条件方便编辑或删除操作
}

//清除操作
function clear() {
    for (var i = 0; i < arr_id.length; i++) {
        $("#" + arr_id[i]).val("");
    }
    $("#gender").val("1");   //性别默认为女
    $("#phoneStatus").val("1");   //手机默认可用
}

//提交
function submit() {
    var phoneReg = /^[0-9]{11}$/;
    var val_arr = [], str_val = "";
    var title = $('#myModalTitle').text();
    var tip = title == "编辑用户" ? "编辑成功" : "添加成功";
    var userId = $("#userName").attr("userId"); 
    for (var i = 0; i < arr_id.length; i++) {
        val_arr.push($("#" + arr_id[i]).val());
    }
    if ($("#userName").val() == "") { alert("姓名不能为空！"); return; }
    if (!phoneReg.test(val_arr[1])) { alert("您输入的手机号不正确！请确认后再提交！"); return; }
    $.ajax({
        url: "MessageManage.aspx/EditUser",
        type: "POST",
        contentType: "application/json",
        data: "{value:'" + val_arr + "',userId:'" + userId + "',title:'" + title + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "success") {
                alert(tip);
                $("#myModal").modal('hide');
                getCompany();;
            } else {
                alert("失败");
            }
            
        },
        error: function (ex) {
            alert("失败！");
        }
    });
}

//导出Excel   不做了
function exportToExcel() {
    document.getElementById('btnExport').click();
}
