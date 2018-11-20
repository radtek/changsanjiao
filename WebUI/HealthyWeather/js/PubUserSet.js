var m_healInfo = { type: { data: [], htmlContent: { select: { elementId: "#selDisease", html: "<option value=''>全部</option>" }, checkbox: { elementId: "#pubHealType,#pubHealTypeII,#selUserHealType", html: ""}} }
, level: { data: [], htmlContent: { radio: { elementId: "#publishLvl,#emailLvl,#selUserPubLvl,#selUserEmailLvl", html: ""}} }
, period: { data: [], htmlContent: { checkbox: { elementId: "#publishPeriod,#emailPeriod,#selUserPubTime,#regularDiv,#selUserEmailTime", html: ""}} }
};
var m_groupNames = [], m_selectUser = [],m_userTable;
var m_numberReg = new RegExp("^[0-9]*$");
$(function () {
    InitDisease();
    InitPubRegion();
    QueryGroup();
    $("#selDisease").change(function () {
        $("#selDisease").attr("value", this.value);
        QueryGroup();
    });
    $("#selDiseaseII").change(function () {
        $("#selDiseaseII").attr("value", this.value);
        QueryGroupII();
    });
    $("#selUserGroup").change(GetGroupInfo);
    $("#btnAddGroup,#btnEditGroup").click(function () {
        ShowGroupWin(this.innerText);
    });
    $("#selUserPub,#selUserEmail").change(function () { StatusChange(this); });
    $("#chkAll").change(function () {
        var objs = $("#tblUsers tbody tr");
        if ($(this).is(':checked')) {
            for (var i = 0; i < objs.length; i++) {
                if (!$(objs[i]).hasClass('selected')) {
                    m_selectUser.push(m_userTable.row($(objs[i])).data());
                    $(objs[i]).addClass("selected");
                }
            }
        }
        else {
            objs.removeClass("selected");
            m_selectUser = [];
        }

    });
});
function StatusChange(obj) {
    if ($(obj).is(':checked'))
        $("#" + obj.id + "Lvl input,#" + obj.id + "Time input").removeAttr("disabled");
    else {
        $("#" + obj.id + "Lvl input,#" + obj.id + "Time input").attr("disabled", "true");
        CheckObjByText($("input[name='radio" + obj.id + "Lvl']"));
        CheckObjByText($("input[name='chk" + obj.id + "Time']"));
    }
}
function InitPubRegion() {
    var html = "",region=["中心城区","浦东新区","闵行区","宝山区","松江区","金山区","青浦区","奉贤区","嘉定区","崇明"];
    for (var i = 0; i < region.length; i++) {
        html += "<option value='" + region[i] + "'>" + region[i] + "</option>";
    }


    var htmlii = "", region = ["全部","中心城区", "浦东新区", "闵行区", "宝山区", "松江区", "金山区", "青浦区", "奉贤区", "嘉定区", "崇明"];
    for (var i = 0; i < region.length; i++) {
        htmlii += "<option value='" + region[i] + "'>" + region[i] + "</option>";
    }


    $("#selPubRegion").html(html);
    $("#selDiseaseII").html(htmlii);


    $('#selPubRegion').selectpicker({
        'selectedText': 'cat',
        'noneSelectedText': '请选择'
    });
   // $("#selPubRegion").selectpicker({ 'selectAllText': '请选择' });
}
function InitDisease() {
    $.ajax({
        url: "PubUserSet.aspx/GetHealthyInfo",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d;
            m_healInfo.type.data = datas[0].rows;
            m_healInfo.level.data = datas[1].rows;
            m_healInfo.period.data = datas[2].rows;
            var strHtml = "";
            for (var item in m_healInfo) {
                if (m_healInfo[item].data.length > 0) {
                    for (var i = 0; i < m_healInfo[item].data.length; i++) {
                        if (m_healInfo[item].htmlContent.select)
                            m_healInfo[item].htmlContent.select.html += "<option value='" + m_healInfo[item].data[i].DM + "'>" + m_healInfo[item].data[i].MC + "</option>";
                        if (m_healInfo[item].htmlContent.checkbox)
                            m_healInfo[item].htmlContent.checkbox.html += '<label class="checkbox-inline"><input type="checkbox" name="chkName" value="'
                            + m_healInfo[item].data[i].DM + '">' + m_healInfo[item].data[i].MC + '</label>';
                        if (m_healInfo[item].htmlContent.radio)
                            m_healInfo[item].htmlContent.radio.html += '<label class="radio-inline"><input type="radio" name="radioName" value="'
                            + m_healInfo[item].data[i].DM + '">' + m_healInfo[item].data[i].MC + '</label>';
                    }
                    if (m_healInfo[item].htmlContent.select) $(m_healInfo[item].htmlContent.select.elementId).html(m_healInfo[item].htmlContent.select.html);
                    if (m_healInfo[item].htmlContent.checkbox) {
                        var itemIDs = m_healInfo[item].htmlContent.checkbox.elementId.split(",");
                        for (var i = 0; i < itemIDs.length; i++) {
                            $(itemIDs[i]).html(m_healInfo[item].htmlContent.checkbox.html.replace(/chkName/g, "chk"+itemIDs[i].substring(1)));
                        }
                    }
                    if (m_healInfo[item].htmlContent.radio) {
                        var itemIDs = m_healInfo[item].htmlContent.radio.elementId.split(",");
                        for (var i = 0; i < itemIDs.length; i++) {
                            $(itemIDs[i]).html(m_healInfo[item].htmlContent.radio.html.replace(/radioName/g, "radio" + itemIDs[i].substring(1)));
                        }
                    }
                }
            }
        },
        error: function (ex) {
           // alert("异常，" + ex.responseText + "！");
        }
    });
}


function QueryGroupII() {
    var healthyType = $("#selDiseaseII").attr("value");
    var groupID = "";
    if ($("#groupList .active").length > 0) groupID = $("#groupList .active")[0].id;
    $.ajax({
        url: "PubUserSet.aspx/QueryGroupII",
        type: "POST",
        contentType: "application/json",
        data: "{healthyType:'" + healthyType + "'}",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows;
            var html = '<a href="#" class="list-group-item" id="list-item-all"><span class="badge">total&Count</span>全部区域</a>';
            var totalCount = 0;
            if (datas) {
                var names = [], strHtml = "";
                for (var item in datas) {
                    totalCount += datas[item].USERCOUNT;
                    html += '<a href="#" class="list-group-item" id="list-item' + item + '" tag="' + datas[item].Region + '-' + datas[item].HealthyType + '-' + datas[item].Message_PubLvl + '-' + datas[item].Message_PubTime + '-' + datas[item].Email_PubLvl + '-' + datas[item].Email_PubTime
                    + '"><span class="badge">' + datas[item].USERCOUNT + '</span>' + datas[item].GroupName + '</a>';
                    names.push({ name: datas[item].GroupName, info: [datas[item].HealthyType, datas[item].Message_PubLvl, datas[item].Message_PubTime, datas[item].Email_PubLvl, datas[item].Email_PubTime] });
                    strHtml += "<option>" + datas[item].GroupName + "</option>";
                }
                // if (!m_groupNames.length) {
                m_groupNames = names;
                $("#selUserGroup").html(strHtml);
                // }
            }
            $("#groupList").html(html.replace("total&Count", totalCount));
            $("#groupList .list-group-item").click(function () {
                $("#groupList .active").removeClass("active");
                $(this).addClass("active");
                if (this.text.indexOf("全部区域") > -1)
                    QueryUserII("", healthyType);
                else QueryUserII(this.innerHTML.split("</span>")[1], healthyType);
            });
            if (groupID) $("#" + groupID).click();
            else $("#list-item-all").click();

            if (datas == null) {
                $("#list-item-all").addClass('active');
            }
            goPage(1, 10);
        },
        error: function (ex) {
            // alert("异常，" + ex.responseText + "！");
        }
    });
}

function QueryGroup() {
    var healthyType = $("#selDisease").attr("value");
    var groupID = "";
    if ($("#groupList .active").length > 0) groupID=$("#groupList .active")[0].id;
    $.ajax({
        url: "PubUserSet.aspx/QueryGroup",
        type: "POST",
        contentType: "application/json",
        data: "{healthyType:'" + healthyType + "'}",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows;
            var html = '<a href="#" class="list-group-item" id="list-item-all"><span class="badge">total&Count</span>全部分组用户</a>';
            var totalCount = 0;
            if (datas) {
                var names = [], strHtml = "";
                for (var item in datas) {
                    totalCount += datas[item].USERCOUNT;
                    html += '<a href="#" class="list-group-item" id="list-item' + item + '" tag="' + datas[item].Region + '-' + datas[item].HealthyType + '-' + datas[item].Message_PubLvl + '-' + datas[item].Message_PubTime + '-' + datas[item].Email_PubLvl + '-' + datas[item].Email_PubTime
                    + '"><span class="badge">' + datas[item].USERCOUNT + '</span>' + datas[item].GroupName + '</a>';
                    names.push({ name: datas[item].GroupName, info: [datas[item].HealthyType, datas[item].Message_PubLvl, datas[item].Message_PubTime, datas[item].Email_PubLvl, datas[item].Email_PubTime] });
                    strHtml += "<option>" + datas[item].GroupName + "</option>";
                }
//                if (!m_groupNames.length) {
                       m_groupNames = names;
                     $("#selUserGroup").html(strHtml);
//                }
            }
            $("#groupList").html(html.replace("total&Count", totalCount));
            $("#groupList .list-group-item").click(function () {
                $("#groupList .active").removeClass("active");
                $(this).addClass("active");
                if (this.text.indexOf("全部分组用户") > -1) 
                    QueryUser("", healthyType);
                else QueryUser(this.innerHTML.split("</span>")[1], healthyType);
            });
            if (groupID) $("#" + groupID).click();
            else $("#list-item-all").click();

            if (datas == null) {
                $("#list-item-all").addClass('active');
            }
            goPage(1,10);
        },
        error: function (ex) {
           // alert("异常，" + ex.responseText + "！");
        }
    });
}



function QueryUserII(groupName, postionAreas) {
    m_selectUser = [];
    m_delUser = [];  //删除多行数据时，存储要删除行的ID
    var num = 1;   //如果是1则删除一行，如果是2则删除多行
    var count = 0;  //记录选中的行数
    $('#tblUsers').empty();
    m_userTable = $('#tblUsers').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.HealthyWeather&method=QueryUserII&groupName=" + encodeURI(groupName) + "&postionAreas=" + postionAreas,
        "columns": [
            { "visible": false },
            { "title": "姓名", "class": "center" },
            { "title": "所属组名", "class": "center" },
            { "title": "手机号码", "class": "center", "orderable": false, "width": 30 },
            { "title": "邮件地址", "class": "center", "orderable": false, "width": 30 },
            { "title": "疾病类型", "class": "center", "orderable": false },
            { "title": "短信等级", "class": "center" },
            { "title": "短信时效", "class": "center", "orderable": false },
            { "title": "邮件等级", "class": "center" },
            { "title": "邮件时效", "class": "center", "orderable": false },
            { "title": "区域", "class": "center", "orderable": false },
            { "title": "备注", "class": "center", "orderable": false },
            { "title": "编辑", "class": "center", "orderable": false, "width": 10 },
            { "title": "删除", "class": "center", "orderable": false, "width": 10}],
        "columnDefs": [{
            "targets": 12, "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 13, "data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
        }],
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
        }
    });

    $('#tblUsers tbody').on('click', 'img', function (obj) {
        var data = m_userTable.row($(this).parents('tr')).data();
        if (num == 1) {
           if (this.title == "删除") {
                if (confirm("确认要删除这个用户么？")) DelUser(data[0]);
            }
        }
        else if (num == 2) {
            if (this.title == "删除") {
                if (confirm("确认要删除" + count + "个用户么？")) DelUser(m_delUser);
            }
        }
        if (this.title == "编辑") {
            $("#btnEditUser").attr("tag", data[0]);
            ShowUserWin("编辑用户", data.slice(1));
        }
        if (obj.stopPropagation) {
            obj.preventDefault();
            obj.stopPropagation();
        }
        else if (window.event) window.event.cancelBubble = true;
    });

    $('#tblUsers tbody').on('click', 'tr', function () {
        num = 2;
        var delData = m_userTable.row($(this)).data()[0];
        var data = m_userTable.row($(this)).data();
        if ($(this).hasClass('selected')) {
            count--;
            $(this).removeClass('selected');
            var index = $.inArray(data, m_selectUser);
            var del_index = m_delUser.indexOf(delData);
            if (del_index > -1) m_delUser.splice(del_index, 1)
            if (index > -1) m_selectUser.splice(index, 1);
        } else {
            count++;
            $(this).addClass('selected');
            m_delUser.push(delData);
            m_selectUser.push(data);
        }
        if (m_delUser.length <= 0) {
            num = 1;
        }
    });
}


//王斌  2017.5.16
function QueryUser(groupName, healthyType) {
    m_selectUser = [];
    m_delUser = [];  //删除多行数据时，存储要删除行的ID
    var num = 1;   //如果是1则删除一行，如果是2则删除多行
    var count = 0;  //记录选中的行数
    $('#tblUsers').empty();
    m_userTable = $('#tblUsers').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true,"bPaginate":true,
        "bSortClasses": false,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.HealthyWeather&method=QueryUser&groupName=" + encodeURI(groupName) + "&healthyType=" + healthyType,
        "columns": [
            {"visible": false},
            {"title": "姓名", "class": "center" }, 
            {"title": "所属组名", "class": "center" },
            { "title": "手机号码", "class": "center", "orderable": false, "width": 30 },
            { "title": "邮件地址", "class": "center", "orderable": false, "width": 30 },
            { "title": "疾病类型", "class": "center", "orderable": false},
            { "title": "短信等级", "class": "center" },
            { "title": "短信时效", "class": "center", "orderable": false },
            { "title": "邮件等级", "class": "center" },
            { "title": "邮件时效", "class": "center", "orderable": false },   
            { "title": "区域", "class": "center", "orderable": false },
            { "title": "备注", "class": "center", "orderable": false },
            {"title": "编辑", "class": "center", "orderable": false,"width": 10   },
            { "title": "删除", "class": "center","orderable": false,"width": 10 }],
        "columnDefs": [{
            "targets": 12,"data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 13,"data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
        }],
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
        }
    });

    $('#tblUsers tbody').on('click', 'img', function (obj) {
        var data = m_userTable.row($(this).parents('tr')).data();
        if (num == 1) {
            if (this.title == "删除") {
                if (confirm("确认要删除这个用户么？")) DelUser(data[0]);
            }
            
        }
        else if (num == 2) {
            if (this.title == "删除") {
                if (confirm("确认要删除"+count+"个用户么？")) DelUser(m_delUser);
            }
        }
        if(this.title=="编辑") {
            $("#btnEditUser").attr("tag", data[0]);
            ShowUserWin("编辑用户", data.slice(1));
        }
        
        if (obj.stopPropagation) {
            obj.preventDefault();
            obj.stopPropagation();
        }
        else if (window.event) window.event.cancelBubble = true;
    });

    $('#tblUsers tbody').on('click', 'tr', function () {
        num = 2;
        var delData = m_userTable.row($(this)).data()[0];
        var data = m_userTable.row($(this)).data();
        if ($(this).hasClass('selected')) {
            count--;
            $(this).removeClass('selected');
            var index = $.inArray(data, m_selectUser);
            var del_index = m_delUser.indexOf(delData);
            if(del_index>-1) m_delUser.splice(del_index,1)
            if (index > -1) m_selectUser.splice(index, 1);
        } else {
            count++;
            $(this).addClass('selected');
            m_delUser.push(delData);
            m_selectUser.push(data);
        }
        if (m_delUser.length <= 0) {
           num = 1;
        }
    });
}

function ShowGroupWin(text) {
        var groupName = $("#groupList .active")[0].innerHTML.split("</span>")[1];
        if (groupName != "全部分组用户" || text == "创建分组") {
            $('#mdlGroup').modal('show');
            $('#mdlGroup .modal-title').text(text);
            var objName = ['selPubRegion','chkpubHealType', 'radiopublishLvl', 'chkpublishPeriod', 'radioemailLvl', 'chkemailPeriod'];
            if (text == "创建分组") {
                $('#mdlGroup .edit').addClass("hidden");
                $("#groupName").val("").attr("disabled", false);
                for (var i = 0; i < objName.length; i++) {
                    if (i == 0) $('#' + objName[i]).selectpicker('val', ''); // $('#' + objName[i]).val(""); 03-20
                else
                    CheckObjByValue($("input[name='" + objName[i] + "']"));
                }
            }
            else {
                $('#mdlGroup .edit').removeClass("hidden");
                var info = $("#groupList .active").attr("tag");
                if (info) {
                    info = info.split("-");
                    $("#groupName,#editGroupName").val($("#groupList .active")[0].innerHTML.split("</span>")[1]);
                    $("#groupName").attr("disabled", true);
                    for (var i = 0; i < objName.length; i++) {
                        var arr = info[i].toString().replace(/_/g, ',').split(',');
                        if (i == 0) { $('#' + objName[i]).selectpicker('val', arr); } // xuehui 03-20 }
                        CheckObjByValue($("input[name='" + objName[i] + "']"), info[i].split("_"));
                    }
                }
            }
        }
}
function CheckObjByValue(objs,vals) {
    for (var i = 0; i < objs.length; i++) {
        if (vals && $.inArray(objs[i].value, vals) > -1) $(objs[i]).prop("checked", true);
        else $(objs[i]).prop("checked", false);
    }
}
function CheckObjByText(objs, vals) {
    for (var i = 0; i < objs.length; i++) {
        if (vals && $.inArray(objs[i].parentElement.innerText, vals) > -1) $(objs[i]).prop("checked", true);
        else $(objs[i]).prop("checked", false);
    }
}
function GetObjByName(arr,name) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].name == name) return arr[i];
    }
    return null;
}
function GetGroupInfo() {
    var objName = ['chkselUserHealType', 'radioselUserPubLvl', 'chkselUserPubTime', 'radioselUserEmailLvl', 'chkselUserEmailTime'];  //wb 2017.7.3  
    var obj = GetObjByName(m_groupNames, $("#selUserGroup").val());
    for (var i = 0; i < objName.length; i++) {
        CheckObjByValue($("input[name='" + objName[i] + "']"), obj.info[i].split("_"));
    }

}

function ShowUserWin(text, info) {
    $('#mdlUser').modal('show');
    $('#mdlUser .modal-title').text(text);
    var objName = ['iptUserName', 'selUserGroup', 'iptUserPhone', 'iptUserEmail', 'chkselUserHealType', 'radioselUserPubLvl', 'chkselUserPubTime', 'radioselUserEmailLvl', 'chkselUserEmailTime', 'selUserRemark'];
    if (text.indexOf("创建") > -1) {
        $("#selUserPub,#selUserEmail").prop("checked", false);
        StatusChange($("#selUserPub")[0]); StatusChange($("#selUserEmail")[0]);
        $("#btnEditUser").attr("tag", "");
        /*2017.4.17  王斌   如果选择某一分组用户，则在创建用户时分组名称默认为选中的用户不能修改*/
        //        for (var i = 0; i < objName.length; i++) {       //原版
        //            if (i < 4 || i == objName.length-1) $('#' + objName[i]).val("");
        //            else CheckObjByText($("input[name='" + objName[i] + "']"));
        //        }
        for (var i = 0; i < objName.length; i++) {
            GetGroupInfo();
            if (i == 0 || i == 2 || i == 3 || i == objName.length - 1) $('#' + objName[i]).val("");
            else if (i == 1) {
                var infoText = $("#groupList .active")[0].innerHTML.split("</span>")[1];
                $('#' + objName[i]).removeAttr("disabled");
                if (infoText != "全部分组用户" && infoText != "全部区域") {
                    $('#' + objName[i]).val(infoText);
                    $('#' + objName[i]).attr("disabled", "disabled");

                }
            }
            else CheckObjByText($("input[name='" + objName[i] + "']"));
        }

    }
    else {
        //王斌  修改  2017.4.17  如果是编辑用户时则分组名称可以修改
        for (var i = 0; i < objName.length; i++) {
            if (i == 0 || i == 2 || i == 3) $('#' + objName[i]).val(info[i]);
            else if (i == objName.length - 1) $('#' + objName[i]).val(info[i + 1]);
            else if (i == 1) {
                $('#' + objName[i]).removeAttr("disabled");
                $('#' + objName[i]).val(info[i]);
            }
            else {
                if (objName[i] == 'radioselUserPubLvl') {
                    if (!info[i]) $("#selUserPub").prop("checked", false);
                    else $("#selUserPub").prop("checked", true);
                    StatusChange($("#selUserPub")[0]);
                }
                if (objName[i] == 'radioselUserEmailLvl') {
                    if (!info[i]) $("#selUserEmail").prop("checked", false);
                    else $("#selUserEmail").prop("checked", true);
                    StatusChange($("#selUserEmail")[0]);
                }
                CheckObjByText($("input[name='" + objName[i] + "']"), info[i].split(","));
            }
        }
    }
}

function GetObjVal(obj) {
    var result = "";
    for (var i = 0; i < obj.length; i++) {
        result += "_" + obj[i].value;
    }
    return result.substring(1);
}

function GetObjValII(obj) {
    var result = "";
    for (var i = 0; i < obj.length; i++) {
        result += "," + obj[i].value;
    }
    return result.substring(1);
}

function EditGroup() {
    var type = $('#mdlGroup .modal-title').text().substring(0,2);
    var groupName = $("#groupName").val();
    if (!groupName) { alert("分组名称不能为空！"); return; }
    var region = $("#selPubRegion").val();

    if (!region) { alert("所属区域不能为空！"); return; } else {
        region = region.toString().replace(/,/g, '$'); // 03-20
    }
    var newName = $("#editGroupName").val();
    if (!newName && type!= "创建") { alert("新分组名不能为空！"); return; }
    var healthyType = GetObjVal($("input[name='chkpubHealType']:checked"));
    if (!healthyType) { alert("至少为该组选择一种疾病类型！"); return; }
    var messagePubLvl = $("input[name='radiopublishLvl']:checked").val();
    var messagePubTime = GetObjVal($("input[name='chkpublishPeriod']:checked"));
    if (messagePubLvl && !messagePubTime) { alert("至少为该组选择一种短信发送时间！"); return; }
    var emailPubLvl = $("input[name='radioemailLvl']:checked").val();
    var emailPubTime = GetObjVal($("input[name='chkemailPeriod']:checked"));
    if (emailPubLvl && !emailPubTime) { alert("至少为该组选择一种短信发送时间！"); return; }
    $.ajax({
        url: "PubUserSet.aspx/EditGroup",
        type: "POST",
        contentType: "application/json",
        data: "{type:'" + (type == "创建" ? "new" : "edit") + "',newName:'" + encodeURI(newName) + "',groupName:'" + encodeURI(groupName) + "',region:'" + encodeURI(region) + "',healthyType:'" + healthyType 
        + "',Message_PubLvl:'" + messagePubLvl + "',Message_PubTime:'" + messagePubTime + "',Email_PubLvl:'" + emailPubLvl + "',Email_PubTime:'" + emailPubTime + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "1") {
                alert("恭喜，" + type + "成功！");
                $('#mdlGroup').modal('hide');
                QueryGroup();
                m_groupNames = [];
            }
            else alert(type + "失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}

function DelGroup() {
    var groupName = $("#groupList .active")[0].innerHTML.split("</span>")[1];
    if (groupName != "全部分组用户") {
        if (confirm("该组内的" + $("#groupList .active .badge").text() + "个用户也会被删除！确定要删除该组么？")) {
            $.ajax({
                url: "PubUserSet.aspx/DelGroup",
                type: "POST",
                contentType: "application/json",
                data: "{groupName:'" + encodeURI(groupName) + "'}",
                dataType: 'json',
                success: function (results) {
                    if (results.d == "1") {
                        alert("恭喜，删除成功！");
                        $('#mdlGroup').modal('hide');
                        QueryGroup();
                        m_groupNames = [];
                    }
                    else alert("删除失败，原因如下：" + results.d);
                },
                error: function (ex) {
                    alert("异常，" + ex.responseText + "！");
                }
            });
        }
    }
}

function DelUser(userID) {
    $.ajax({
        url: "PubUserSet.aspx/DelUser",
        type: "POST",
        contentType: "application/json",
        data: "{userID:'" + userID + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d) {
                alert("恭喜，删除成功！");
                $('#mdlUser').modal('hide');
                QueryGroup();
            }
            else alert("删除失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}

function EditUser() {
    var userID=$("#btnEditUser").attr("tag");
    var type = $('#mdlUser .modal-title').text().substring(0, 2);
    var userName = $("#iptUserName").val();
    var telephone = $("#iptUserPhone").val();
    if (!m_numberReg.test(telephone)) { alert("请输入正确的手机号码！"); return; }
    var email = $("#iptUserEmail").val();
    var groupName = $("#selUserGroup").val();
    if (!groupName) { alert("分组名称不能为空！"); return; }
    var healthyType = GetObjVal($("input[name='chkselUserHealType']:checked"));
    if (!healthyType) { alert("至少为该组选择一种疾病类型！"); return; }
    var messagePubLvl = $("input[name='radioselUserPubLvl']:checked").val();
    if (messagePubLvl == undefined) messagePubLvl = "";
    var messagePubTime = GetObjVal($("input[name='chkselUserPubTime']:checked"));
    if ($("#selUserPub").is(':checked')) {
        if (!messagePubLvl) { alert("短信发送等级不能为空，或者设置该用户为不允许发送短信！"); return; }
        if (!messagePubTime) { alert("至少为该组选择一种短信发送时间！"); return; }
    }
    var emailPubLvl = $("input[name='radioselUserEmailLvl']:checked").val();
    if (emailPubLvl == undefined) emailPubLvl = "";
    var emailPubTime = GetObjVal($("input[name='chkselUserEmailTime']:checked"));
    if ($("#selUserEmail").is(':checked')) {
        if (!emailPubLvl) { alert("邮件发送等级不能为空，或者设置该用户为不允许发送邮件！"); return; }
        if (!emailPubTime) { alert("至少为该组选择一种邮件发送时间！"); return; }
    }
    //wb  2017.7.3
    if (!($("#selUserPub").is(':checked'))) {     //如果没选中不支持发送短信，其值为空
        messagePubLvl = "", messagePubTime = "";
    }
    if (!($("#selUserEmail").is(':checked'))) {
        emailPubLvl = "", emailPubTime = "";
    }
    var remark = $("#selUserRemark").val();
    $.ajax({
        url: "PubUserSet.aspx/EditUser",
        type: "POST",
        contentType: "application/json",
        data: "{userID:'" + userID + "',Name:'" + encodeURI(userName) + "',groupName:'" + encodeURI(groupName) + "',phone:'" + telephone + "',email:'" + email + "',healthyType:'" + healthyType
        + "',Message_PubLvl:'" + messagePubLvl + "',Message_PubTime:'" + messagePubTime + "',Email_PubLvl:'" + emailPubLvl + "',Email_PubTime:'" + emailPubTime + "',Remark:'" + encodeURI(remark) + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "1") {
                alert("恭喜，" + type + "成功！");
                $('#mdlUser').modal('hide');
                QueryGroup();
                /*2017.4.17  王斌  */
                QueryGroupII();
            }
            else alert(type + "失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}

function ShowSendWin(text) {
  
    if (m_selectUser.length) {
        CheckObjByValue($("chkregularDiv"));
        $("#txtContent,#divTitle input").val("");
        $(".modal-footer span").text(m_selectUser.length);
        $('#mdlSend').modal('show');
        $('#mdlSend .modal-title').text(text);
        var html = "";
        if (text == "短信发送") {
            $("#divTitle").addClass("hide");
            for (var i = 0; i < m_selectUser.length; i++) {
                if (!m_selectUser[i][3]) html += m_selectUser[i][1] + ",";
            }
        }
        else {
            $("#divTitle").removeClass("hide");
            for (var i = 0; i < m_selectUser.length; i++) {
                if (!m_selectUser[i][4]) html += m_selectUser[i][1] + ",";
            }
        }
        $(".modal-footer strong").text(html ? (" 其中:(" + html.substring(0, html.length - 1) + ")没有" + (text == "短信发送" ? "电话" : "邮件地址")) : "");
    }
    else {
        //没有选择弹出任意短信或邮件发送
        if (text == "短信发送") {
            $("#msgContent,#tels input").val("");
            $('#customSendMessage').modal('show');
            $('#customSendMessage .modal-title').text(text);
        } else {
            $("#txt_content,#customTitle input,#customURL input").val("");
            $('#customSend').modal('show');
            $('#customSend .modal-title').text(text);
        }
    }

}
function ToggleCustom(obj) {
    if (obj.innerText.indexOf("编辑") > -1) {
        obj.innerText = "取消自定义内容";
        $("#customDiv").removeClass("hide");
        $("#regularDiv").addClass("hide");
        $("#regularDivII").addClass("hide");
    }
    else {
        obj.innerText = "编辑自定义内容";
        $("#regularDiv").removeClass("hide");
        $("#regularDivII").removeClass("hide");
        $("#customDiv").addClass("hide");
    }
}
function Send() {
    var method,datas;
    if ($("#customDiv").attr("class").indexOf("hide") > -1) {
        var times = GetObjVal($("input[name='chkregularDiv']:checked"));
        if (!times) { alert("请至少选择一个发送时效！"); return; }

        var products = GetObjValII($("input[name='chkpubHealTypeII']:checked"));
        if (!products) { alert("请至少选择一个疾病类型！"); return; }

        if ($("#divTitle").attr("class").indexOf("hide") < 0) method = "EmailRegular";
        else method = "MessageRegular";
        var userID = "";
        for (var i = 0; i < m_selectUser.length; i++) {
            if ((method == "EmailRegular" && m_selectUser[i][4]) || (m_selectUser[i][3] && method == "MessageRegular")) 
                userID += m_selectUser[i][0] + "&";
        }
        if (!userID) { alert("没有有效的发送对象！"); return; }

        if (method == "EmailRegular")
            datas = "{UserIDS:'" + userID + "',time:'" + times + "',isAll:0,m_aliass:'',products:'"+products+"'}";
        else
            datas = "{UserIDS:'" + userID + "',time:'" + times + "',isAll:0,products:'" + products + "',m_aliass:''}";
    }
    else {
        var content = $("#txtContent").val();
        if (!content) { alert("发送内容不能为空！"); return; }
        var title = $("#divTitle input").val();
        if ($("#divTitle").attr("class").indexOf("hide") < 0) {
            if (!title) { alert("邮件标题不能为空！"); return; }
            var emails = "";
            for (var i = 0; i < m_selectUser.length; i++) {
                if (m_selectUser[i][4]) emails += m_selectUser[i][0] + "\"" + m_selectUser[i][4] + "&";
            }
            method = "EmailCustom";
            if (!emails) { alert("没有有效的发送对象！"); return; }
            datas = "{title:'" + encodeURI(title) + "',msg:'" + encodeURI(content) + "',emailAddress:'" + emails.substring(0, emails.length - 1) + "'}";
        }
        else {
            var phone = "";
            for (var i = 0; i < m_selectUser.length; i++) {
                if (m_selectUser[i][3]) phone += m_selectUser[i][0] + "\"" + m_selectUser[i][3] + "&";
            }
            method = "MessageCustom";
            if (!phone) { alert("没有有效的发送对象！"); return; }
            datas = "{title:'" + encodeURI(title) + "',msg:'" + encodeURI(content) + "',phone:'" + phone.substring(0, phone.length - 1) + "'}";         
        }
    }
    $.ajax({
        url: "PubUserSet.aspx/" + method,
        type: "POST",
        contentType: "application/json",
        data: datas,
        dataType: 'json',
        success: function (results) {
            if (method.indexOf("Email") >= 0) {
                alert(results.d);
            } else {
                if (parseFloat(results.d)) {
                    if (results.d != "1.1") {
                        alert("发送失败！原因" + results.d + "");
                    } else {
                        alert("短信发送成功！");
                    }
                }
                else {

                    alert("短信发送失败，原因是：" + results.d);
                }
            }
            //$('#mdlSend').modal('hide');
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });

}

function CustomSend(type) {

    var method = "";
    var datas = "";
    if (type == "邮件") {
        method = "EmailCustomII";
        var title = $("#customTitle input").val();
        var content = $("#txt_content").val();
        var emails = $("#customURL input").val();
        if (emails == "" || content == "") {
            alert("请输入相关信息后重试！");
            return;
         }
        datas = "{title:'" + encodeURI(title) + "',msg:'" + encodeURI(content) + "',emailAddress:'" + emails + "'}";
    } else {
        method = "MessageCustomII";
        var content = $("#msgContent").val();
        var phone = $("#tels input").val();
        if (phone == "" || content == "") {
            alert("请输入相关信息后重试！");
            return;
        }
        datas = "{phone:'" + encodeURI(phone) + "',msg:'" + encodeURI(content) + "'}";
    }
    $.ajax({
        url: "PubUserSet.aspx/" + method,
        type: "POST",
        contentType: "application/json",
        data: datas,
        dataType: 'json',
        success: function (results) {
            if (method.indexOf("Email") >= 0) {
                alert(results.d);
            } else {
                if (parseFloat(results.d)) {
                    if (results.d != "1.1") {
                        alert("发送失败！原因:" + results.d + "");
                    } else {
                        alert("短信发送成功！");
                    }
                }
                else {

                    alert("短信发送失败，原因是：" + results.d);
                }
            }
            //$('#customSendMessage').modal('hide');
            //$('#customSend').modal('hide');
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
            //$('#customSendMessage').modal('hide');
            //$('#customSend').modal('hide');
        }
    });
}

//王斌  2017.4.11
var iframeOnload = function () { }
function dataImport() {
    document.getElementById("actionForm").style.display = "block";
    document.getElementById("uploadStatus").style.display = "none";
    var path = "~/CJDATA/";
    var defaultURL = "WebExplorerss.ashx";
    document.getElementById("actionForm").action = defaultURL + "?action=UpLoadII&value2=" + encodeURIComponent(path);
    $("#mdlImport").modal('show');
    $("#mdlDia").css({
        width: '400'
    });
}
var sccishu = 0;
function upLoad() {
    $("#uploadFrm").load(function () {
        if (sccishu == 1) {
            var body = $(window.frames['uploadFrm'].document.body);
            $("#mdlImport").modal('hide');
            alert(body[0].innerHTML.toString());
            //if ((body[0].innerHTML.toString().indexOf("error") >= 0) || (body[0].innerHTML.toString().indexOf("错误") >= 0)) {
            //    $("#mdlImport").modal('hide');
            //    alert("文件上传失败，请检查文件类型或格式，或者文件内容！ " + body[0].innerHTML.toString());
            //} else {

            //    $("#mdlImport").modal('hide');
            //    alert("文件上传成功！");
            //}
            sccishu = 0;
            QueryGroup();
            QueryUser();
            QueryGroup();
        }
    });
    document.getElementById("actionForm").submit();
    sccishu = 1;
    document.getElementById("actionForm").style.display = "none";
    document.getElementById("uploadStatus").style.display = "block";
}


//王斌  2017.5.11
function dataExport() {
    var defaultURL = "WebExplorerss.ashx";
    document.getElementById("actionForm").action = defaultURL + "?action=DataExport";
    document.getElementById("actionForm").submit();
}

//wb   2017.06.29
//对分组进行分页
//pno---页数   psize---每页显示的记录数
function goPage(pno, psize) {
    var tempStr = "";
    var groupList = $("#groupList .list-group-item");
    var num = groupList.length - 1;  //总行数
    if (num <= 10) { $("#barcon").html(tempStr); return; }
    var totalPage = 0;  //总页数
    var pageSize = psize  //每页显示的行数
    //总共分几页
    if (num / pageSize > parseInt(num / pageSize)) {
        totalPage =parseInt(num / pageSize) + 1;
    } else {
        totalPage = num / pageSize;
    }
    var currentPage = pno;  //当前页数
    var startRow = (currentPage - 1) * pageSize + 1;  //每页开始显示的行数
    var endRow = currentPage * pageSize;   //每页结束的行数
    endRow = (endRow > num) ? num : endRow;  //如果endrow大于总行数，说明可能是最后一页
    //遍历显示数据实现分页
    for (var i = 1; i <= num+1; i++) {
        var irow = groupList[i-1];
        if (i > startRow && i <= (endRow+1)) {
            irow.style.display="block";
        } else {
            irow.style.display = "none";
        }
    }
    groupList[0].style.display = "block";
    tempStr = "共" + num + "条记录 第" + currentPage + "页";
    if (currentPage > 1) {
        tempStr += "<a href=\"#\" onclick=\"goPage(" + 1 + "," + psize + ")\"> 首页</a>";
        tempStr += "<a href=\"#\" onclick=\"goPage(" + (currentPage - 1) + "," + psize + ")\"><上一页</a>";
    } else {
        tempStr += " 首页";
        tempStr += "<上一页";
    }
    if (currentPage < totalPage) {
        tempStr += "<a href=\"#\" onclick=\"goPage(" + (currentPage + 1) + "," + psize + ")\"> 下一页></a>";
        tempStr += "<a href=\"#\" onclick=\"goPage(" + totalPage + "," + psize + ")\">尾页</a>";
    } else {
        tempStr += " 下一页>";
        tempStr += "尾页";
    }
    $("#barcon").html(tempStr);
}