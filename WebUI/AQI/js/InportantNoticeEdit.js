var editWin;
var checkList = new Array();
var periodType = "day";
function EditNotice(value) {
    if (editWin) {
        editWin.destroy();
    }
    editWin = CreateNoticeEditPanel(value);
    editWin.show();

    $("#ddlPeriodType").click(function () {
        if ($("#ddlPeriodType").val() == "4") {
            $("#txtNoticePeriod").hide();
        }
        else {
            $("#txtNoticePeriod").show();
        }
    });

    if ($("#ddlPeriodType").val() == "4") {
        $("#txtNoticePeriod").hide();
        periodType = "long";
    }
    else {
        $("#txtNoticePeriod").show();
        if ($("#ddlPeriodType").val() == "1") {
            periodType = "day";
//            $("#txtNoticePeriod").val(parseInt(Math.abs(dtend - dtstart) / 1000 / 60 / 60 / 24));
        }
        if ($("#ddlPeriodType").val() == "2") {
            periodType = "week";
//            $("#txtNoticePeriod").val(parseInt(Math.abs(dtend - dtstart) / 1000 / 60 / 60 / 24 / 7));
        }
        if ($("#ddlPeriodType").val() == "3") {
            periodType = "month";
//            $("#txtNoticePeriod").val(parseInt(Math.abs(dtend - dtstart) / 1000 / 60 / 60 / 24 / 30));
        }

    }
}

function AddNewNotice(userName) {
    if (editWin) {
        editWin.destroy();
    }
    editWin = CreateNewNoticePanel(userName)
    editWin.show();

    $("#ddlPeriodType").click(function () {
        if ($("#ddlPeriodType").val() == "4") {
            $("#txtNoticePeriod").hide();
        }
        else {
            $("#txtNoticePeriod").show();
        }
    });

    if ($("#ddlPeriodType").val() == "4") {
        $("#txtNoticePeriod").hide();
        periodType = "long";
    }
    else {
        $("#txtNoticePeriod").show();
        if ($("#ddlPeriodType").val() == "1") {
            periodType = "day";
            //            $("#txtNoticePeriod").val(parseInt(Math.abs(dtend - dtstart) / 1000 / 60 / 60 / 24));
        }
        if ($("#ddlPeriodType").val() == "2") {
            periodType = "week";
            //            $("#txtNoticePeriod").val(parseInt(Math.abs(dtend - dtstart) / 1000 / 60 / 60 / 24 / 7));
        }
        if ($("#ddlPeriodType").val() == "3") {
            periodType = "month";
            //            $("#txtNoticePeriod").val(parseInt(Math.abs(dtend - dtstart) / 1000 / 60 / 60 / 24 / 30));
        }

    }
}

function CreateNoticeEditPanel(value) {
    var idValue = "";
    var reTime = "";
    var type = "";
    var period = "";
    var timeType = "";
    var isDisable = "";
    var content = "";
    var user = "";
    var typeSelHtml = "";
    var timeTypeSelHtml = "";

    if (value != "") {
        idValue = value.split('&')[0];
        reTime = value.split('&')[1];
        type = value.split('&')[2];

        switch (value.split('&')[2]) {
            case "全系统通知":
                typeSelHtml = '<option value="0">==请选择通知类型==</option>'
                                    + '<option value="1" selected="selected">全系统通知</option>'
                                    + '<option value="2">全市通知</option>'
                                    + '<option value="3">城环中心通知</option>'
                                    + '<option value="4">会商通知</option>';
                break;
            case "全市通知":
                typeSelHtml = '<option value="0">==请选择通知类型==</option>'
                                    + '<option value="1">全系统通知</option>'
                                    + '<option value="2" selected="selected">全市通知</option>'
                                    + '<option value="3">城环中心通知</option>'
                                    + '<option value="4">会商通知</option>';
                break;
            case "城环中心通知":
                typeSelHtml = '<option value="0">==请选择通知类型==</option>'
                                    + '<option value="1">全系统通知</option>'
                                    + '<option value="2">全市通知</option>'
                                    + '<option value="3" selected="selected">城环中心通知</option>'
                                    + '<option value="4">会商通知</option>';
                break;
            case "会商通知":
                typeSelHtml = '<option value="0">==请选择通知类型==</option>'
                                    + '<option value="1">全系统通知</option>'
                                    + '<option value="2">全市通知</option>'
                                    + '<option value="3">城环中心通知</option>'
                                    + '<option value="4" selected="selected">会商通知</option>';
                break;
            default:
                typeSelHtml = '<option value="0">==请选择通知类型==</option>'
                                    + '<option value="1">全系统通知</option>'
                                    + '<option value="2">全市通知</option>'
                                    + '<option value="3">城环中心通知</option>'
                                    + '<option value="4">会商通知</option>';
                break;
        }


        switch (value.split('&')[4]) {
            case "周":
                timeTypeSelHtml = '<option value="1">天</option>'
                                    + '<option value="2" selected="selected">周</option>'
                                    + '<option value="3">月</option>'
                                    + '<option value="4">长期</option>'
                break;
            case "天":
                timeTypeSelHtml = '<option value="1" selected="selected">天</option>'
                                    + '<option value="2" selected="selected">周</option>'
                                    + '<option value="3">月</option>'
                                    + '<option value="4">长期</option>'
                break;
            case "月":
                timeTypeSelHtml = '<option value="1">天</option>'
                                    + '<option value="2" selected="selected">周</option>'
                                    + '<option value="3" selected="selected">月</option>'
                                    + '<option value="4">长期</option>'
                break;
            case "长期":
                timeTypeSelHtml = '<option value="1">天</option>'
                                    + '<option value="2" selected="selected">周</option>'
                                    + '<option value="3">月</option>'
                                    + '<option value="4" selected="selected">长期</option>'
                break;
            default:
                timeTypeSelHtml = '<option value="1">天</option>'
                                    + '<option value="2">周</option>'
                                    + '<option value="3">月</option>'
                                    + '<option value="4" selected="selected">长期</option>'
                break;
        }
        if (value.split('&')[3] == "0") {
            timeTypeSelHtml = '<option value="1">天</option>'
                                    + '<option value="2">周</option>'
                                    + '<option value="3">月</option>'
                                    + '<option value="4" selected="selected">长期</option>'
        }
        period = value.split('&')[3];
        timeType = value.split('&')[4];
        isDisable = (value.split('&')[5] == 1) ? "checked='checked'" : "";
        content = value.split('&')[6];
        user = value.split('&')[7];
        }

   
    var editPanel = new Ext.Window({
        title: '重要通知信息',
        width: 500,
        height: 410,
        layout: 'fit', //设置窗口内部布局
        closeAction: 'hide',
        plain: true, //true则主体背景透明，false则和主体背景有些差别
        collapsible: true, //是否可收缩
        modal: true, //是否为模式窗体
        items: new Ext.Panel({//窗体中中是一个一个TabPanel               
            items: [
                {
                    id: "tabTxt",
                    html: '<table class="TableDetail" >'
                        + '<tbody><tr>'
                            + '<td class="titleTD" style="width:80px;text-align:center;"><div class="editTableAttrName">通知类型:</div></td>'
                            + '<td class="contentTD">'
                                + '<select id="ddlNoticeType" class="s_k3 fontColor">'
                                + typeSelHtml
                                + '</select>'
                            + '</td>'
                        + '</tr>'
                        + ' <tr>'
                            + '<td class="titleTD" style="text-align:center;"><div class="editTableAttrName">通知期限:</div>'

                            + ' </td>'
                            + ' <td class="contentTD" style="width:98%;">'
                                + '<input id="txtNoticePeriod" type="text" class="timeInput s_k3 fontColor input" style="display: none;" value="'+period+'">'
                                + ' <select id="ddlPeriodType" class="s_k3">'
                                    + timeTypeSelHtml
                                + '</select>'
                            + '</td>'
                        + '</tr>'
                        + '<tr>'
                            + '<td class="titleTD" style="text-align:center;"><div class="editTableAttrName">是否停用:</div>'

                            + '</td>'
                            + '<td class="contentTD">'
                                + '<input id="chkIsInUse" type="checkbox" '+isDisable+'><span>&nbsp;&nbsp;(打勾表示停用该通知)</span>'
                            + '</td>'
                        + '</tr>'
                        + '<tr>'
                            + '<td class="titleTD" style="text-align:center;"><div class="editTableAttrName">通知内容:</div>'

                            + '</td>'
                            + '<td colspan="3" class="contentTD">'
                                + '<textarea id="txtNoticeContent" cols="20" rows="2" class="s_k3 fontColor input" style="height:200px;width:94%;">'+content+'</textarea>'
                            + '</td>'
                        + '</tr>'
                    + '</tbody></table>'
                }
            ]
        }),
        buttons: [
            {
                text: '保存',
                handler: function () {//点击时触发的事件
                    var dayCount = "-1";
                    if ($("#ddlPeriodType").find("option:selected").text() == "长期") {
                        dayCount = "0";
                    }
                    else if (periodType == "long") {
                        dayCount = $("#txtNoticePeriod").val();
                    }

                    var isDisable = "0";
                    if ($('#chkIsInUse').is(':checked')) {
                        isDisable = "1";
                    }
//                    if (idValue != "") {
                        Ext.Ajax.request({
                            url: getUrl('MMShareBLL.DAL.AQIForecast', 'RefreshImportantNotice'),
                            params: { id: idValue, type: $("#ddlNoticeType").find("option:selected").text(), period: dayCount, timeType: $("#ddlPeriodType").find("option:selected").text(), isDisable: isDisable, content: $("#txtNoticeContent").val()},
                            success: function (response) {
                                if (response.responseText == "success") {
                                    editPanel.hide();
                                    alert("保存成功");
                                    $('#logTable').html('')
                                    LoadImportNotice();
                                }
                                else {
                                    alert("保存失败！");
                                }
                            },
                            failure: function (response) {
                                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            }
                        });
//                    }
                }
            },
            {
                text: '关闭',
                handler: function () {//点击时触发的事件
                    editPanel.hide();
                }
            }
            ]
        });
        
        return editPanel;
}

//点击新增通知
function CreateNewNoticePanel(userName) {
    var editPanel = new Ext.Window({
        title: '重要通知信息',
        width: 500,
        height: 410,
        layout: 'fit', //设置窗口内部布局
        closeAction: 'hide',
        plain: true, //true则主体背景透明，false则和主体背景有些差别
        collapsible: true, //是否可收缩
        modal: true, //是否为模式窗体
        items: new Ext.Panel({//窗体中中是一个一个TabPanel               
            items: [
                {
                    id: "tabTxt",
                    html: '<table class="TableDetail" style="height:100%">'
                        + '<tbody><tr>'
                            + '<td class="titleTD" style="width:80px;text-align:center;"><div class="editTableAttrName">通知类型:</div></td>'
                            + '<td class="contentTD">'
                                + '<select id="ddlNoticeType" class="s_k3 fontColor">'
                                + '<option value="0">==请选择通知类型==</option>'
                                    + '<option value="1">全系统通知</option>'
                                    + '<option value="2">全市通知</option>'
                                    + '<option value="3">城环中心通知</option>'
                                        + '<option value="4">会商通知</option>'
                                + '</select>'
                            + '</td>'
                        + '</tr>'
                        + ' <tr>'
                            + '<td class="titleTD" style="text-align:center;"><div class="editTableAttrName">通知期限:</div>'

                            + ' </td>'
                            + ' <td class="contentTD" style="width:98%;">'
                                + '<input id="txtNoticePeriod" type="text" class="timeInput s_k3 fontColor input" style="display: none;">'
                                + ' <select id="ddlPeriodType" class="s_k3">'
                                    + ' <option value="1">天</option>'
                                    + '<option value="2">周</option>'
                                    + '<option value="3">月</option>'
                                    + '<option value="4">长期</option>'
                                + '</select>'
                            + '</td>'
                        + '</tr>'
                        + '<tr>'
                            + '<td class="titleTD" style="text-align:center;"><div class="editTableAttrName">是否停用:</div>'

                            + '</td>'
                            + '<td class="contentTD">'
                                + '<input id="chkIsInUse" type="checkbox"><span>&nbsp;&nbsp;(打勾表示停用该通知)</span>'
                            + '</td>'
                        + '</tr>'
                        + '<tr>'
                            + '<td class="titleTD" style="text-align:center;"><div class="editTableAttrName">通知内容:</div>'

                            + '</td>'
                            + '<td colspan="3" class="contentTD">'
                                + '<textarea id="txtNoticeContent" cols="20" rows="2" class="s_k3 fontColor input" style="height:200px;width:94%;"></textarea>'
                            + '</td>'
                        + '</tr>'
                    + '</tbody></table>'
                }
            ]
        }),
        buttons: [
            {
                text: '保存',
                handler: function () {//点击时触发的事件
                    var dayCount = "-1";
                    if (periodType == "day" || periodType == "week" || periodType == "month") {
                        dayCount = $("#txtNoticePeriod").val();
                    }
                    else if (periodType == "long") {
                        dayCount = "";
                    }

                    var isDisable = "0";
                    if ($('#chkIsInUse').is(':checked')) {
                        isDisable = "1";
                    }
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'AddImportantNotice'),
                        params: { type: $("#ddlNoticeType").find("option:selected").text(), period: dayCount, timeType: $("#ddlPeriodType").find("option:selected").text(), isDisable: isDisable, content: $("#txtNoticeContent").val(), user: userName },
                        success: function (response) {
                            if (response.responseText == "success") {
                                editPanel.hide();
                                alert("保存成功");
                                $('#logTable').html('')
                                LoadImportNotice();
                            }
                            else {
                                alert("保存失败！");
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
            },
            {
                text: '关闭',
                handler: function () {//点击时触发的事件
                    win.hide();
                }
            }
            ]
    });
    return editPanel;
}

function Checked(ID) {
    checkList.push(ID);
    
}

function DeleteSelectNotices() {
    var checkJson = "";
    if (checkList != null && checkList.length > 0) {
        for (var i = 0; i < checkList.length; i++) {
            if (i < checkList.length - 1) {
                checkJson += checkList[i] + ",";
            }
            else {
                checkJson += checkList[i]
            }
        }
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'DeleteImportantNotice'),
            params: { ids: checkJson},
            success: function (response) {
                if (response.responseText == "success") {
                    alert("删除成功");
                    $('#logTable').html('')
                    LoadImportNotice();
                }
                else {
                    alert("删除失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
}