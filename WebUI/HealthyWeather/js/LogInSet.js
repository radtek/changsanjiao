var m_staInfo = { station: { data: [], htmlContent: { select: { elementId: "#SearchbyStation", html: "<option value=''>全部</option>" }, radio: { elementId: "#iptStation", html: ""}} }
, area: { data: [], htmlContent: { checkbox: { elementId: "#iptArea,#SearchByArea", html: ""}}}
};
$(function () {
    InitSA();
    InitCompany();
    QueryGroup("init", "");
    $("#SearchbyStation").change(function () {
        var station = $("#SearchbyStation").val();
        var area = encodeURI(GetCheckedValue("chkName"));
        QueryGroup(station, area);
    });
    $("#SearchByArea").change(function () {
        var station = $("#SearchbyStation").val();
        var area = encodeURI(GetCheckedValue("chkName"));
        QueryGroup(station, area);
    });
});

//显示各选项
function InitSA() {
    $.ajax({
        url: "LogInSet.aspx/GetStationInfo",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d;
            m_staInfo.station.data = datas[0].rows;
            m_staInfo.area.data = datas[1].rows;
            var strHtml = "";
            for (var item in m_staInfo) {
                if (m_staInfo[item].data.length > 0) {
                    for (var i = 0; i < m_staInfo[item].data.length; i++) {
                        if (m_staInfo[item].htmlContent.select)
                            m_staInfo[item].htmlContent.select.html += '<option value="' + m_staInfo[item].data[i].DM + '">' + m_staInfo[item].data[i].MC
                             + '(' + m_staInfo[item].data[i].DM + ')' + '</option>';
                        if (m_staInfo[item].htmlContent.checkbox)
                            m_staInfo[item].htmlContent.checkbox.html += '<label class="forcheck"><input type="checkbox" name="chkName" value="'
                            + m_staInfo[item].data[i].MC + '">' + m_staInfo[item].data[i].MC + '</label>';
                        if (m_staInfo[item].htmlContent.radio)
                            m_staInfo[item].htmlContent.radio.html += '<label class="forradio"><input type="radio" name="radioName" value="'
                            + m_staInfo[item].data[i].DM + '">' + m_staInfo[item].data[i].MC + '(' + m_staInfo[item].data[i].DM + ')' + '</label>';
                    }
                    if (m_staInfo[item].htmlContent.select) $(m_staInfo[item].htmlContent.select.elementId).html(m_staInfo[item].htmlContent.select.html);
                    if (m_staInfo[item].htmlContent.checkbox) {
                        var itemIDs = m_staInfo[item].htmlContent.checkbox.elementId.split(",");
                        for (var i = 0; i < itemIDs.length; i++) {
                            if (itemIDs[i] == "#iptArea") {
                                var tempt = m_staInfo[item].htmlContent.checkbox.html.replace(/forcheck/g, "forradio1");
                                $(itemIDs[i]).html(tempt.replace(/chkName/g, "chk" + itemIDs[i].substring(1)));
                            }
                            else
                                $(itemIDs[i]).html(m_staInfo[item].htmlContent.checkbox.html);
                        }
                    }
                    if (m_staInfo[item].htmlContent.radio) $(m_staInfo[item].htmlContent.radio.elementId).html(m_staInfo[item].htmlContent.radio.html.replace(/radioName/g, "radio" + m_staInfo[item].htmlContent.radio.elementId.substring(1)));
                }
                $.each($(".forradio"), function (i, n) {
                    $(n).click(function () {
//                        if ($($(".forradio")[i]).html().indexOf("value=\"3\"") < 0) {
//                            $("#selPubRegion").attr("disabled", "disabled");
//                            $("#selPubRegion").selectpicker('val', '');
//                        }
//                        else
//                            $('#selPubRegion').removeAttr("disabled");
                    });
                });
            }
        },
        error: function (ex) {
            //alert("异常，" + ex.responseText + "！");
        }
    });
}

function InitCompany() {
    $.ajax({
        url: "LogInSet.aspx/GetCompanys",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var datas = results.d.rows;
            var htmls = "";
            if (datas.length > 0) {
                for (var i = 0; i < datas.length; i++)
                    htmls += "<option value='" + datas[i].companName + "'>" + datas[i].companName + "</option>";

                $("#selPubRegion").html(htmls);
                $('#selPubRegion').selectpicker({
                    'selectedText': 'cat',
                    'noneSelectedText': '请选择'
                });
                $("#selPubRegion").selectpicker('refresh');
            }
        },
        error: function (ex) {
            //alert("异常，" + ex.responseText + "！");
        }
    });
}

//获取被选checkbox的value
function GetCheckedValue(name) {
    var group = document.getElementsByName(name);
    var checked = new Array();
    for (var i = 0; i < group.length;i++ ) {
        if (group[i].checked) checked.push(group[i].value);
    }
    return checked.join(",");
}

//分组查询
function QueryGroup(station,area) {
    $('#register').empty();
    m_userTable = $('#register').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "iDisplayLength": 15,
        "lengthMenu": [[15, 50, 100, -1], [15, 50, 100, "所有"]],
        "bFilter": true, "bPaginate": true,
        "bSortClasses": true,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.HealthyWeather&method=QueryGroupII&station=" + station + "&area=" + area,
        "columns": [
            { "title": "姓名", "class": "center" },
            { "title": "用户名", "class": "center" },
            { "title": "密码", "class": "center" },
            { "title": "角色", "class": "center" },
            { "title": "邮件地址", "class": "center", "orderable": false, "width": 70 },
            { "title": "所属区域", "class": "center", "orderable": false },
            { "title": "所属单位", "class": "center", "orderable": false },
            { "title": "编辑", "class": "center", "orderable": false, "width": 15 },
            { "title": "删除", "class": "center", "orderable": false, "width": 15}],
        "columnDefs": [{
            "targets": 7, "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 8, "data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
        }],
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>",
            "sLengthMenu": "每页显示 _MENU_ 条记录&nbsp;&nbsp;",
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

    $('#register tbody').on('click', 'img', function (obj) {
        var data = m_userTable.row($(this).parents('tr')).data();
        if (this.title == "删除") {
            if (confirm("确认要删除这个用户么？")) DelUser(data[1]);
        }
        else ShowUserWin("编辑用户", data);
        if (obj.stopPropagation) {
            obj.preventDefault();
            obj.stopPropagation();
        }
        else if (window.event) window.event.cancelBubble = true;
    });

}


//显示编辑、创建对象窗口
function ShowUserWin(text, info) {
    $('#mdlUser').modal('show');
    $('#mdlUser .modal-title').text(text);
    $('#iptStation [value=' + $('input:radio:checked').val() + ']').prop('checked', false);
    var checked = GetCheckedValue('chkiptArea');
    var array1 = checked.split(",");
    for (var j = 0; j < array1.length; j++) {
        $('#iptArea [value=' + array1[j] + ']').prop('checked', false);
    }
    var objName = ['iptName', 'iptuserName', 'key', 'iptStation', 'iptEmail', 'iptArea', 'DateTime'];
    if (text.indexOf("创建") == -1) {
        for (var i = 0; i < objName.length; i++) {
            if (i == 3) {
                var obj = info[i].replace(/[^0-9]/g,"");
                $('#iptStation [value=' + obj + ']').prop('checked', true);
            }
            if (i == 5) {
                var obj1 = info[i].split(",");
                for (j = 0; j < obj1.length; j++) {
                    $('#iptArea [value=' + obj1[j] + ']').prop('checked', true);
                }
            }
            else $('#' + objName[i]).val(info[i]);
        }
        $("#iptuserName").prop("disabled", true);
        $("#key").prop("disabled", true);
    }
    else {
        $("#iptuserName").prop("disabled", false);
        $("#key").prop("disabled", false);
        for (var i = 0; i < objName.length; i++) {
            if (i != 3 || i != 5) $('#' + objName[i]).val("");
        }
    }
}

//删除用户
function DelUser(userName) {
    $.ajax({
        url: "LogInSet.aspx/DelUser",
        type: "POST",
        contentType: "application/json",
        data: "{userName:'" + userName + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "1") {
                alert("恭喜，删除成功！");
                $('#mdlUser').modal('hide');
                var station = $("#SearchbyStation").val();
                var area = encodeURI(GetCheckedValue("chkName"));
                QueryGroup(station, area);
            }
            else alert("删除失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}

//创建、编辑用户
function EditUser() {
    var type = $('#mdlUser .modal-title').text().substring(0, 2);
    var Name = $("#iptName").val();
    var userName = $("#iptuserName").val();
    if (!userName) { alert("请输入用户名！"); return; }
    var SN = $("#key").val();
    if (!SN) { alert("请输入密码！"); return; }
    var BZ = parseInt($('input:radio:checked').val());  //格式为station（ID）
    if (!BZ) { alert("请选择工作单位！"); return; }
    var email = $("#iptEmail").val();
    var postionarea = GetCheckedValue("chkiptArea");
    var company = $(".filter-option").html();
    $.ajax({
        url: "LogInSet.aspx/EditUser",
        type: "POST",
        contentType: "application/json",
        data: "{type:'" + type + "',UserName:'" + encodeURI(userName) + "',SN:'" + encodeURI(SN) + "',BZ:'" + BZ + "',Alias:'"
        + encodeURI(Name) + "',EMail:'" + email + "',POSTIONAREA:'" + encodeURI(postionarea) + "',WindowsUser:'" + encodeURI(company) + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d == "1") {
                alert("恭喜，" + type + "成功！");
                $('#mdlUser').modal('hide');
                var station = $("#SearchbyStation").val();
                var area = encodeURI(GetCheckedValue("chkName"));
                QueryGroup(station, area);
            }
            else alert(type + "失败，原因如下：" + results.d);
        },
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}