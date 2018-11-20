
var CurTab, dialog, UID;
var chk_value = [];
var name;
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    loadWG();
    queryData();
    openDiv();
   }
)

function openDiv() {
    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 590,
        width: 490,
        modal: true,
        buttons: {
            "开始排班": add,
            "关闭": function () {
                dialog.dialog("close");
            }
        },
        close: function () {
            form[0].reset();
        }
    });

    form = dialog.find("form").on("submit", function (event) {
        event.preventDefault();
        add();
    });
}

function findCheck() {
    var userStr="";
    var el = document.getElementById("example_us").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox') && (el[i].checked)) {
            var tr = $(el[i]).parent();
            users = tr.parent().parent()[0].cells[1].innerText;
            userStr = userStr + users+","
        }
    }
    if (userStr.length > 0)
        userStr = userStr.substr(0, userStr.length - 1); 
    return userStr;
}
function add() {
    var DText = $("#wg").find("option:selected").text();
    var UserName = Ext.getDom("userNames").value;
    var group = $('#wg option:selected').val();
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var type = $("#TypeList").find("option:selected").text();

    var name = findCheck();
    if (toDate == "") {
          Ext.Msg.alert("提示", "请选择终止时间！");
          $('#H01').focus();
            return;
        }
    if (DText == undefined || DText == "") {
        Ext.Msg.alert("提示", "请选择工作组！");
        $('#wg').focus();
        return;
    }
    if (name == undefined || name == "") {
        Ext.Msg.alert("提示", "请选择人员！");
        return;
    }
    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "AddScheduing";
    var url = getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        params: { DText: DText, Users: UserName, beginTime: fromDate, endTime: toDate, Type: type, group: group, name: name },
        success: function (response) {
            if (response.responseText != "0") {
                Ext.Msg.alert("提示", "排班成功！");
                //刷新
                $("#calendar").fullCalendar('removeEvents');
                $('#calendar').fullCalendar('refetchEvents');
                //queryData();
                dialog.dialog("close");
            } else {
                Ext.Msg.alert("提示", "排班失败，请联系系统管理员！");
            }
        }
    });
}
function queryEndtime() {
    var wrkg = $('#wg option:selected').val();
    var type = $('#TypeList option:selected').val();
    if (wrkg == undefined) {
        try {
            wrkg = Ext.getDom("wg").options[0].value;
        }
        catch (exception) {
            return false;
        }
    }
    var fromDate = Ext.getDom("H00").value;
    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "getEndTime";
    var url = getUrl(provider, method);
    var para = "&text=" + wrkg + "&type=" + type + "&fromDate=" + fromDate;
    Ext.Ajax.request({
        url: url,
        params: { text: wrkg, type: type, fromDate: fromDate },
        success: function (response) {
            if (response.responseText != "") {
                var split = response.responseText.split(',')
                Ext.getDom("H01").value = split[0];
                name = split[1];
            }
            else
                name = "";
            getDataTable(wrkg,type);
        }
    });
}
function getDataTable(wrkg,type) {
    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "GetWorkGroupUserII";
    var para = "&text=" + wrkg + "&type=" + type ;
    var url = "PatrolHandler.do?provider=" + provider + "&method=" + method;
    $('#example_us').empty();
    var datable = $('#example_us').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": false,
        "bSortClasses": false,
        "bPaginate": false,
        'bLengthChange': false,
        "bAutoWidth": true,
        "aLengthMenu": [3],
        "sAjaxSource": (url + para),
        "columns": [
            { "title": "序号", "class": "left" }, //v 0
            {"title": "姓名", "class": "left" }, //v 1
            {"title": "起始人员", "class": "center", "width": 40 },
            { "title": "上移", "class": "center", "width": 40 },
            { "title": "下移", "class": "center", "width": 40 }
                     ],
          "columnDefs": [{
            "targets": -1,
            "data": null,
            "defaultContent": "<img src=\"images/yj.png\" id='xy' title=\"下移\" style=\"cursor: pointer\"></img>"
        }, {
//       "columnDefs": [{
            "targets": 3,
            "data": null,
            "defaultContent": "<img src=\"images/shiftUp.png\" id='sy' title=\"上移\" style=\"cursor: pointer\"></img>"
        }, {
//        "columnDefs": [{
            "targets": 2,
            "data": null,
            "defaultContent": "<a><input  type=\"checkbox\" checked=\"checked\"  name=\"InsuscheckBox\" value=\"0\"/></a>"
        }],
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>", //<h1 style=\"font-size:8px;\">正在加载...</h1>
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "搜索",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上一页",
                "sNext": "下一页",
                "sLast": "末页"
            }
        },
        "fnInitComplete": function (oSettings, json) {
            var el = document.getElementById("example_us").getElementsByTagName("input");
            for (var i = 0; i < el.length; i++) {
                if ((el[i].type == 'checkbox')) {
                    var tr = $(el[i]).parent();

                    users = tr.parent().parent()[0].cells[1].innerText;
                    if (name == "")
                        el[i].checked = true;
                    else {
                        if (users != name)
                            el[i].checked = false;
                        else
                            el[i].checked = true;
                    }
                }
            }
        }
    });

    $('#example_us tbody').on('mouseover', 'tr', function () {
        mouseOver(this);
    });
    $('#example_us tbody').on('mouseout', 'tr', function () {
        mouseOut(this);
    });
    $('#example_us tbody').on('click', 'img', function () {
        var data = datable.row($(this).parents('tr')).data();
        var id = this.id;
        var index;
        var tip = "";
        if (id == "sy") {
            tip = "上移";
            index = 1;
        }
        else {
            tip = "下移";
            index = -1;
        }
        var siteID = data[0];
        if (siteID != "1") {
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.Scheduing', 'changeIndex'),
                params: { text: wrkg, type: type, siteID: siteID, index: index },
                success: function (response) {
                    if (response.responseText == "1") {
                        getDataTable(wrkg, type);
                        Ext.Msg.alert("提示", tip+"成功！");
                    }
                    else
                        Ext.Msg.alert("提示", tip+"失败，请联系系统管理员！");
                }
            });
        }
    });

}
function queryData1() {
    var wrkg =  $('#wg option:selected').val();
    var type = $('#TypeList option:selected').val();
    if (wrkg == undefined) {
        try {
            wrkg = Ext.getDom("wg").options[0].value;
        }
        catch (exception) {
            return false;
        }
    }
    queryEndtime();



}


function loadWG(week) {
    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "GetWorkGroup";
    var url = getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        async: true,
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                var InnerHTML = "<select id=\"wg\" style=\"width:150px;\" onchange=\"queryData1();\" >";
                var json = result.data;
                for (var i = 0; i < json.length; i++) {
                    var values = json[i].toString().split(',');
                    if (week != "Mon") {
                        if (values[1] == "副班")
                            continue;
                    }
                    InnerHTML += "<option value=" + values[6] + ">" + values[1] + "</option>";
                }
                InnerHTML += "</select>";
                document.getElementById("workGroup").innerHTML = ("工作组：" + InnerHTML);
//                queryData1();
            }
        }
    });
}

function mouseOver(obj) {
    var id = obj.id;
    id.substring(6, id.length);
    var periodCount = 1;
    if (obj != null) {
        if (parseInt(id.substring(6, id.length)) == 1 || (parseInt(id.substring(6, id.length)) - 1) % periodCount == 0) {
            for (var i = 1; i < obj.cells.length; i++)
                obj.cells[i].bgColor = "#badbff";
        }
        else {
            for (var i = 0; i < obj.cells.length; i++)
                obj.cells[i].bgColor = "#badbff";
        }
    }
}

function mouseOut(obj) {
    var id = obj.id;
    id.substring(6, id.length);
    var periodCount = 1;
    if (obj != null) {
        if (parseInt(id.substring(6, id.length)) == 1 || (parseInt(id.substring(6, id.length)) - 1) % periodCount == 0) {
            for (var i = 1; i < obj.cells.length; i++)
                obj.cells[i].bgColor = "#FFFFFF";
        }
        else {
            for (var i = 0; i < obj.cells.length; i++)
                obj.cells[i].bgColor = "#FFFFFF";
        }
    }
}

function queryData() {
    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "GetScheduing";
    var fromDate = "2014-11-8";//  $('#calendar').fullCalendar('getDate');
   
    var urls = "PatrolHandler.do?provider=" + provider + "&method=" + method;
    var para = "&date=" + fromDate + "";
    urls = urls + para;
    $('#calendar').fullCalendar({
        editable: true,
        buttonText: {
            prev: '上月',
            next: '下月',
            prevYear: '去年',
            nextYear: '明年',
            today: '本月',
            month: '月',
            week: '周',
            day: '日'
        },
        firstDay: 1,
        monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
        dayNames: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
        dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
        eventAfterRender: function (event, element, view) {
            //alert(event.start);
            //fullCalendar('refetchEvents')
            // $("#calendar").fullCalendar('refetchEvents');
        },
        theme: true,
        header: {
            left: '',
            center: 'title',
            id:'titleTime',
            right: 'prevYear,prev,today,next,nextYear'
        },
        eventLimit: true,
        dayClick: function (date, allDay, jsEvent, view) {
            var m = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Spt", "Oct", "Nov", "Dec");
            //var w = new Array("Monday", "Tuseday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday");Mon
            var dts = date.toString().split(" ");
            var month = 0;
            for (var i = 0; i < m.length; i++) {
                if (m[i] == dts[1]) {
                    month = i;
                    break;
                }
            }
            loadWG(dts[0]);
            var mon = (month + 1);
            if (mon.toString().length == 1)
                mon = "0" + mon;

            var dt = dts[3] + "-" + mon + "-" + dts[2]
            //alert(dt);
            Ext.getDom("H00").value = dt;
            $('#H01').focus();
            UID = "";
            queryData1();
            dialog.dialog("open");
        },
        eventClick: function (event, element) {
            //queryData1();
            //dialog.dialog("open");
        },
        events: {
            url: urls,
            success: function (doc) {
                $("#calendar").fullCalendar('removeEvents');
                var info = doc.data;
                for (var i = 0; i < info.length; i++) {
                    var ev = info[i];
                    var title = ev[1];
                    var evtstart = ev[3];
                    var evtend = ev[4];
                    var user = ev[2];
                    var type = ev[5];
                    var color = "#7CFC00";
                    var textcolor = "black";
                    if (type == "节假日值班") {
                        color = "#FF8000";
                        //textcolor = "white";
                    }
                    if (title.indexOf("正") >= 0)
                        textcolor = "#0000FF";
                    if (title.indexOf("副") >= 0)
                        textcolor = "#00C78C";

                    var obj = new Object();
                    obj.id = 1;
                    obj.title = title + "   " + user;
                    obj.start = evtstart;
                    obj.end = evtend;
                    obj.color = color;
                    obj.textColor = textcolor;
                    $("#calendar").fullCalendar('renderEvent', obj, true);
                }
            }
        }
    });
    var a = getElementsClass("fc-left");
    var b = getElementsClass("fc-center");
    var titleTime = b[0].innerText;
    var dts = titleTime.toString().split(" ");
    var monthStr = dts[0];
    var month;
    var m = new Array("一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月");
    for (var i = 0; i < m.length; i++) {
        if (m[i] == monthStr) {
            month = i;
            break;
        }
    }
    month = month + 1;
    var dateTime = dts[1] + "年" + month + "月";
    a[0].innerHTML = "<div style=\"float: left;height:20px;line-height:20px\">日常值班</div><div style=\" float: left;width:20px;height:20px;background-color:#7CFC00\"></div><div style=\"float: left;height:20px; line-height:20px\">节假日值班</div><div style=\"float: left;width:20px;height:20px;background-color:#FF8000\"></div><div><input type='button' style=' float:left;  margin-left:20px;' id='ScanBack' class='button' value='导出' onclick=\"ExportTable('" + dateTime + "')\"/></div>";
}
function ExportTable(dateTime) {
    var content = dateTime;
    var Element = document.getElementById("Element");
    Element.setAttribute("value", content);
    document.getElementById("btnExport").click();
}
function getElementsClass(classnames) {
    var classobj = new Array(); //定义数组 
    var classint = 0; //定义数组的下标 
    var tags = document.getElementsByTagName("*"); //获取HTML的所有标签 
    for (var i in tags) {//对标签进行遍历 
        if (tags[i].nodeType == 1) {//判断节点类型 
            if (tags[i].getAttribute("class") == classnames)//判断和需要CLASS名字相同的，并组成一个数组 
            {
                classobj[classint] = tags[i];
                classint++;
            }
        }
    }
    return classobj; //返回组成的数组 
} 

function getDays() {
    var time = Ext.getDom("YearLabel").innerHTML;
    var month = parseInt(time.substring(5, 7));
    var year = parseInt(time.substring(0, 4));
    var y = year;
    var m = month;
    if (m == 2) {
        return y % 4 == 0 ? 29 : 28;
    } else if (m == 1 || m == 3 || m == 5 || m == 7 || m == 8 || m == 10 || m == 12) {
        return 31;
    } else {
        return 30;
    }
}
