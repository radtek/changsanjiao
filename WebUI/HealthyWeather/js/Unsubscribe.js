m_canTab = [];
$(function () {

    //    var dtNow = new Date();
    //    $("#startDate").val(dtNow.getFullYear() + "-" + (dtNow.getMonth()) + "-" + dtNow.getDate());
    //    $("#endDate").val(dtNow.getFullYear() + "-" + (dtNow.getMonth() + 1) + "-" + dtNow.getDate());
    getDiseaseType();
    getUserGroup();
    getAllName();
    

})
function getUserGroup() {
    $.ajax({
        url: "Unsubscribe.aspx/getUserGroup",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows, html = "<option>全部</option>";
            for (var i = 0; i < datas.length; i++) {
                html+="<option>"+datas[i].GROUPNAME+"</option>";
            }
            $("#selUserGroup").html(html);
            Query();
        }

     });

    $("#selUserGroup").change(function () {
         $("#selUserGroup").attr("value", this.value);
         var data = $("#selUserGroup").val();
         if (data == "全部") {
             getAllName();
         }
         else {
             getName(data);
          } 
     });
    
    
}

function getName(data) {
    $.ajax({
        url: "Unsubscribe.aspx/getName",
        type: "POST",
        contentType: "application/json",
        data: "{userGroup:'" + data + "'}",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows, html = "<option>全部</option>";
            for (var i = 0; i < datas.length; i++) {
                html += "<option>" + datas[i].NAME + "</option>";
            }
            $("#selName").html(html);
            Query();
        }
    });
}
function getAllName() {

    $.ajax({
        url: "Unsubscribe.aspx/getAllName",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows, html = "<option>全部</option>";
            for (var i = 0; i < datas.length; i++) {
                html += "<option>" + datas[i].NAME + "</option>";
            }
            $("#selName").html(html);
        }
    });
}

function getDiseaseType() {
    $.ajax({
        url: "Unsubscribe.aspx/getDiseaseType",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows, html = "<option>全部</option>";
            for (var i = 0; i < datas.length; i++) {
                html += "<option>" + datas[i].HEATHYTYPE + "</option>"
            }
            $("#diseaseType").html(html);
        }
    });
}

function Query() {
   
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();
    if (!startDate||!endDate) {
        alert("申请日期不能为空！");
        return;
    }
    var userGroup = $("#selUserGroup").val();
    var name = $("#selName").val();
    var applyType = $("#applyType").val();
    var diseaseType = $("#diseaseType").val();
    $('#cancelTab').empty();
    m_canTab = $('#cancelTab').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "order": [[4, "desc"]],
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.HealthyWeather&method=Query&userGroup=" + userGroup + "&name="
         + name + "&startDate=" + startDate + "&endDate=" + endDate + "&applyType=" + applyType + "&diseaseType=" + diseaseType,

        "columns": [
            {"visible": false },
            { "title": "ID", "class": "center" ,"visible": false },
            { "title": "申请人", "class": "center" },
            { "title": "所属组名称", "class": "center" },
            { "title": "申请日期", "class": "center", "width": 150 },
            { "title": "申请类型", "class": "center" },
            {"visible": false },
            { "title": "邮件地址", "class": "center" },
            { "title": "电话号码", "class": "center" },
            { "title": "处理结果", "class": "center" },
            { "title": "处理日期", "class": "center" , "width": 150 }
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
            "oPaginate": { "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页"
            }
        },
        
    });

    $('#cancelTab tbody').on('click', 'tr', function () {
        var data = m_canTab.row($(this)).data();

        if (data[data.length-2] == "已处理") {
         if ($(this).hasClass('selected')) {

                $(this).removeClass('selected');
                var index = $.inArray(data, m_canTab);
                if (index > -1) m_canTab.splice(index, 1);
            } else {
                $(this).addClass('selected');
                m_canTab.push(data);
            }
        }
        else {
            if ($(this).hasClass('selected')) {

                $(this).removeClass('selected');
                var index = $.inArray(data, m_canTab);
                if (index > -1) m_canTab.splice(index, 1);
            } else {
                $(this).addClass('selected');
                m_canTab.push(data);
            }
        }

    });
}


function Del(){
 
    var userID="";
    var count = m_canTab.length
    if (count == 0) {
        alert("你没有选择要删除的信息，请选择");
    }
    else if (window.confirm('你选择了' + count + '信息，你确定要删除吗?')) {
        for (var i = 0; i < m_canTab.length; i++) {
            userID+=m_canTab[i][0]+",";
        }
        userID=userID.substring(0, userID.length - 1);
      
        $.ajax({
            url: "Unsubscribe.aspx/Del",
            type: "POST",
            data: "{ID:'"+userID+"'}",
            contentType: "application/json",
            dataType: "json",
            success: function (results) {
                Query();
                alert("删除成功！");
            }
        });
    }
}

function Agree() {
    var applyUser = "";
    var group = "";
    var applyTime = "";
    var diseaseType = "";
    var applyType="";
    var userID="";
    var count = m_canTab.length
    if (count == 0) {
        alert("你没有选择要处理的信息，请选择");
    }
    else if (window.confirm('你选择了' + count + '信息，你确定要处理吗?')) {
        for (var i = 0; i < m_canTab.length; i++) {
            userID+=m_canTab[i][1]+",";
            applyUser+=m_canTab[i][2]+",";
            group+=m_canTab[i][3]+",";
            applyTime+=m_canTab[i][4]+",";
            applyType+=m_canTab[i][5]+",";
            diseaseType+=m_canTab[i][6]+",";
        }
        userID=userID.substring(0, userID.length - 1);
        applyUser = applyUser.substring(0, applyUser.length - 1);
        group = group.substring(0, group.length - 1);
        applyTime = applyTime.substring(0, applyTime.length - 1);
        applyType=applyType.substring(0,applyType.length-1);
        diseaseType = diseaseType.substring(0, diseaseType.length - 1);
        $.ajax({
            url: "Unsubscribe.aspx/Agree",
            type: "POST",
            data: "{userID:'"+userID+"',applyUser:'"+applyUser+"',group:'"+group+"',applyTime:'"+applyTime+"',diseaseType:'"+diseaseType+"',applyType:'"+applyType+"'}",
            contentType: "application/json",
            dataType: "json",
            success: function (results) {
                Query();
                alert("处理成功！");
            }
        });
    }
   
}