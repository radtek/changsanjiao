
var CurTab;
var CurPID, UID,userNames,DIDs,Types ;
var dialog, dialogs, dialog_usr, dialog_edit, form,dialogII;
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    loadFZ();
    loadWG();
    openConfirm();
    openDiv();
    openUser();
    openEdit();
    openDivSC();
}
)


function openUser() {
   
    dialog_usr = $("#dialog-form-user").dialog({
        autoOpen: false,
        width: 613, 
        height:450,
        modal: true,
        buttons: {
            "保存": saveFYZY,
            "关闭": function () {
                $(this).dialog("close");
            }
        }
    });
}

function openEdit() {

    dialog_edit = $("#dialog-form-edit").dialog({
        autoOpen: false,
        modal: true,
        width: 613,
        height: 460,
        buttons: {
            "更新": updateFYZY,
            "关闭": function () {
                $(this).dialog("close");
            }
        }
    });
}

function openDivSC() {
    dialogII = $("#dialog-formS").dialog({
        autoOpen: false,
        height: 230,
        width: 260,
        modal: true,
        buttons: {
            "上传": addII,
            "关闭": function () {
                dialogII.dialog("close");
            }
        },
        close: function () {
        }
    });

}

var iframeOnload = function () { }
function addII() {

    $("#uploadFrm").load(function () {
        var body = $(window.frames['uploadFrm'].document.body);
        // alert(body[0].innerHTML.toString());
        if (body[0].innerHTML.toString().indexOf("error") >= 0) {
            dialogII.dialog("close");
            Ext.Msg.alert("提示", "文件上传失败，请检查文件类型或格式，或者上传文件大小！ " + body[0].innerHTML.toString());
            // querySiteDataII();
        } else {
            if (body[0].innerHTML.toString().indexOf("err") >= 0) {
                dialogII.dialog("close");
                Ext.Msg.alert("提示", "上传失败！请检查原因:"+body[0].innerHTML.toString().replace("err",""));
                queryData1();
            } else { 
                dialogII.dialog("close");
                Ext.Msg.alert("提示", "文件上传成功！");
                queryData1();
            }
            // GetStatus();
        }
    });

    document.getElementById("actionForm").submit();
    document.getElementById("actionForm").style.display = "none";
    document.getElementById("uploadStatus").style.display = "block";
}

function show(obj) {
    document.getElementById("actionForm").style.display = "block";
    document.getElementById("uploadStatus").style.display = "none";

    var path = "~/CJDATA/";
    var defaultURL = "WebExplorerss.ashx";
    document.getElementById("actionForm").action = defaultURL + "?action=UPLOAD&value1=" + encodeURIComponent(path);
    dialogII.dialog("open");
}

function updateFYZY() {
    var type = $('#wg3 option:selected').val();
    var itemName = $('#items_3 option:selected').val();
    var zy1 = $("#zy1_3").val();
    var zy2 = $("#zy2_3").val();
    var months = "";
    var el = document.getElementById("months_3").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox') && (el[i].checked)) {
            months += (el[i].id.split('_')[0] + ",");
        }
    }
    var s1 = $('#s1_3 option:selected').val();
    var s2 = $('#s2_3').val();
    var s3 = $('#s3_3 option:selected').val();
    var s4 = $('#s4_3 option:selected').val();
    var s5 = $('#s5_3').val();
    var s = s1 + s2 + s3 + s4 + s5;

    var standID = $('#standID').val();
    var monthID = $('#monthID').val();
    var itemNames = $('#itemName').val();
    var typeName= $('#typeName').val();

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'UpdateFYZY'),
        timeout: 120000,
        params: { type: type, itemName: itemName, zy1: zy1, zy2: zy2, tj: s, months: months, standID: standID, monthID: monthID, oldType: typeName, oldItem: itemNames },
        success: function (response) {
            if (response.responseText != "0") {
                Ext.Msg.alert("提示", "更新成功！");
                queryData1();
                dialog_edit.dialog("close");
            } else {
                Ext.Msg.alert("提示", "更新失败，请检查相关信息是否填写错误或者请直接联系【系统管理员】！");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "更新失败，错误代码为：" + response.status);
        }
    });
}

function saveFYZY() {
    var type = $('#wg2 option:selected').val();
    var itemName = $('#items option:selected').val();
    var zy1 = $("#zy1").val();
    var zy2 = $("#zy2").val(); 
    var months = "";
    var el = document.getElementById("months").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox') && (el[i].checked)) {
            months += (el[i].id.split('_')[0] + ",");
        }
    }

    var s1=  $('#s1 option:selected').val();
    var s2 = $('#s2').val();
    var s3 = $('#s3 option:selected').val();
    var s4 = $('#s4 option:selected').val();
    var s5 = $('#s5').val();
    var s = s1 + s2 + s3 + s4 + s5;

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'SaveFYZY'),
        timeout: 120000,
        params: { type: type, itemName: itemName, zy1: zy1, zy2: zy2, tj: s, months: months },
        success: function (response) {
            if (response.responseText != "0") {
                Ext.Msg.alert("提示", "保存成功！");
                queryData1();
                dialog_usr.dialog("close");
            } else {
                Ext.Msg.alert("提示", "保存失败，请联系【系统管理员】！");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "保存失败，错误代码为：" + response.status);
        }
    });
}

function iniInfo() {
    $("#zy1").val("");
    $("#zy2").val("");
    var el = document.getElementById("months").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox')) {
            el[i].checked = false;
        }
    }
    var s2 = $('#s2').val("");
    var s5 = $('#s5').val("");
}

function iniInfo2() {
    $("#zy1_3").val("");
    $("#zy2_3").val("");
    var el = document.getElementById("months_3").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox')) {
            el[i].checked = false;
        }
    }
    $('#s2_3').val("");
    $('#s5_3').val("");
    $("#s3_3 option[value='X']").attr("selected", true);
}

function openUserInfo() {
    openUser();
    loadUser();
    iniInfo();
    dialog_usr.dialog("open");
}

function openConfirm() {
     dialogs = $("#dialog-confirm").dialog({
        autoOpen: false,
        modal: true,
        buttons: {
            "删除": del,
            "关闭": function () {
                $(this).dialog("close");
            }
        }
    });
}



function delUser() {
    dialogs.dialog("close");
    if (UID != "") {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.Scheduing', 'DelWorkUser'),
            timeout: 120000,
            params: { ID: UID },
            success: function (response) {
                if (response.responseText != "") {
                    Ext.Msg.alert("提示", "删除成功！");
                    queryData1();
                } else {
                    Ext.Msg.alert("提示", "删除失败，请联系【系统管理员】！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "删除失败，错误代码为：" + response.status);
            }
        });
    }
}

function loadUser() {

    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "GetWorkUser";
    var url = getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        async: true,
        success: function (response) {
            if (response.responseText != "") {
                var v=response.responseText;
                v= v.replace(/\r\n/g,"");
                var result = Ext.util.JSON.decode(v);
                var InnerHTML = "";
                InnerHTML += "<table cellspacing=\"0\" rules=\"all\"  border=\"1\" id=\"gv_user\" style=\" border-color:Black; border:1px;border-style:outset;width:300px;\">";
			    InnerHTML += "<tbody>";
			    InnerHTML += "  <tr style=\" text-align:center; font-weight:bold;height:25px;\">";
			    InnerHTML += "  <th scope=\"col\" style=\" border-color:Black; border:1px;border-style:outset;text-align:center; font-weight:bold;\">序号</th>";
			    InnerHTML += "  <th scope=\"col\" style=\" border-color:Black;border:1px;border-style:outset;width:160px;\ text-align:center;font-weight:bold;\">姓名</th>";
			    InnerHTML += "  <th scope=\"col\" style=\" border-color:Black; border:1px;border-style:outset;text-align:center;font-weight:bold;\">选择</th>";
			    InnerHTML += " </tr>";
                var json = result.data;
                for (var i = 0; i < json.length; i++) {
                    var values = json[i].toString().split(',');
                    InnerHTML += "<tr>";
                    InnerHTML += " <td align=\"center\" style=\"  border-color:Black;border:1px;border-style:outset;\" >" + values[0] + "</td>";
                    InnerHTML += " <td align=\"center\" style=\"  border-color:Black;border:1px;border-style:outset;\" >" + values[4] + "</td>";
                    InnerHTML += " <td align=\"center\" style=\"  border-color:Black;border:1px;border-style:outset;\"><input name=\"" + values[0] + "\" type=\"hidden\" id=\"" + values[0] + "\" value=\"" + values[0] + "\"><input id=\"" + values[1] + "_" + i + "\" type=\"checkbox\"  onclick=\"\"></td>";
			        InnerHTML += "</tr>";
                }
                InnerHTML += "</tbody>";
                InnerHTML += "</table>";
                InnerHTML += "<div style=\" height:30px; margin-top: 10px;\">";
                InnerHTML += "   <span class=\"selectAll\" style=\"float: left; margin-left:10px; padding-right:2px;\"><input id=\"SelectAll\" type=\"checkbox\" name=\"SelectAll\" onclick=\"javascript:SelectAlls(this);\"><label for=\"SelectAll\" style=\" margin-left:-7px;\">全选</label></span>";
                InnerHTML += "   <span class=\"selectAll\" style=\"float: left;margin-left:10px;padding-right:2px;\"><input id=\"UnSelectAll\" type=\"checkbox\" name=\"UnSelectAll\" onclick=\"javascript:UnSelectAlls(this);\"><label for=\"UnSelectAll\" style=\" margin-left:-7px;\">反选</label></span>";
                InnerHTML += "</div>";
                //alert(InnerHTML);
                //document.getElementById("adUser").innerHTML =  InnerHTML;
            }
        }
    });
}

function SelectAlls(obj) {
    Ext.getDom("UnSelectAll").checked=false;
    var el = document.getElementById("gv_user").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox')) {
            el[i].checked = obj.checked;
        }
    }
}

function UnSelectAlls(obj) {
    Ext.getDom("SelectAll").checked=false;
    var el = document.getElementById("gv_user").getElementsByTagName("input");
    for (var i = 0; i < el.length; i++) {
        if ((el[i].type == 'checkbox')) {
            el[i].checked = (!el[i].checked);
        }
    }
}

function loadWG() {
    var provider = "MMShareBLL.DAL.HealthyWeather";
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
                    InnerHTML += "<option value=" + values[6] + ">" + values[1] + "</option>";
                }
                InnerHTML += "</select>";
                document.getElementById("workGroup").innerHTML = ("产品类型：" + InnerHTML);
                queryData1();
            }
        }
    });
}

function loadFZ() {

    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "GetWorkGroupFZ";
    var url=getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        async: true,
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                var InnerHTML = "<select id=\"sel\" style=\"width:180px;\"  >";
                var json = result.data;
                for (var i = 0; i < json.length; i++) {
                    var values = json[i].toString().split(',');
                    InnerHTML += "<option value=" + values[0] + ">"+values[1]+"</option>";
                }
                InnerHTML += "</select>";
                document.getElementById("Deputylist").innerHTML = InnerHTML;
                $('#sel').attr("disabled", false);
            }
        }
    });

}

function loadFZII(data) {

    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "GetWorkGroupFZ";
    var url = getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        async: false,
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                var InnerHTML = "<select id=\"sel\" style=\"width:180px;\" >";
                var json = result.data;
                for (var i = 0; i < json.length; i++) {
                    var values = json[i].toString().split(',');
                    InnerHTML += "<option value=" + values[0] + ">" + values[1] + "</option>";
                }
                InnerHTML += "</select>";
                document.getElementById("Deputylist").innerHTML = InnerHTML;

                if (data[2].toString() == "正组") {
                    Ext.getDom("rblSize_1").checked = true;
                    $('#sel').attr("disabled", false);
                }
                if (data[2].toString() == "副组") {
                    $('#sel').attr("disabled", "disabled");
                    $("#sel").empty();
                    Ext.getDom("rblSize_2").checked = true;
                }

                if (data[2].toString() == "默认") {
                    $('#sel').attr("disabled", "disabled");
                    $("#sel").empty();
                    Ext.getDom("rblSize_0").checked = true;
                }

                var title = Ext.getDom("sel")
                var count = 0;
                for (var i = 0; i < title.options.length; i++) {
                    if (title.options[i].value == data[3]) {
                        title.options[i].selected = true;
                        count++;
                        break;
                    }
                }
                if(count<=0)
                     $("#sel").empty(); 
            }
        }
    });
}

function radioClick(obj) {
    var title = Ext.getDom("sel")
    if (obj.id == "rblSize_1") {
         loadFZ();
         $('#sel').attr("disabled", false);
    } else {
         $('#sel').attr("disabled", "disabled");
         $("#sel").empty(); 
    }
}

function mouseOver(obj) {
    var id = obj.id;
    id.substring(6, id.length);
    var forecasPeriod = getCheckBValue("forecasPeriod");
    var periodCount = forecasPeriod.split(",").length;
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
    var forecasPeriod = getCheckBValue("forecasPeriod");
    var periodCount = forecasPeriod.split(",").length;
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

function getCheckBValue(objName) {
    var postJson = "";
    var forecasArray = new Array();
    var obj = document.getElementsByName(objName);
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].checked)
                postJson = postJson + obj[i].value + ",";
        }
    }
    if (postJson.length > 0)
        postJson = postJson.substring(0, postJson.length - 1);

    return postJson;
}


//预报污染物tab切换
function tabClick(id) {
    var lastParts = lastTab.split("_");
    var lastID = lastParts[1];
    var curParts = id.split("_");
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTab);
    lastEl.className = "";

    CurTab = curParts[0];

        loadWG();
        queryData1();
    

    lastEl.innerHTML = curEl.innerHTML.replace(id, lastTab).replace(curParts[0], lastParts[0]);
    curEl.className = "tabHighlight";
    curEl.innerHTML = curParts[0];
    var currentID = curParts[1];
    var tableTitle = document.getElementById("Tb" + currentID);
    var forecastTable = document.getElementById("Tb" + lastID);
    tableTitle.className = "show";
    forecastTable.className = "hidden";
    lastTab = id;
}

function setSelectChecked(selectId, checkValue) {
    var select = document.getElementById(selectId);
    for (var i = 0; i < select.options.length; i++) {
        if (select.options[i].innerHTML == checkValue) {
            select.options[i].selected = true;
            break;
        }
    }
}

function queryData1() {
    var wrkg = $('#wg option:selected').val();
    var type = $('#TypeList option:selected').val();
    if (wrkg == undefined) {
        try{
            wrkg = Ext.getDom("wg").options[0].value;
           }
        catch(exception){
          return false;
        }
    }
    //alert(wrkg + " " + type); 
  var provider = "MMShareBLL.DAL.HealthyWeather";
  var method = "GetFYZY";
    var para = "&text=" + wrkg + "&type=" + type;
    var url = "PatrolHandler.do?provider=" + provider + "&method=" + method;
    $('#example_us').empty();
    var datable = $('#example_us').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": false,
        "bSortClasses": false,
        "searching": true, //本地搜索
        "Info": true, //页脚信息
        "autoWidth": true, //自动宽度
        "sAjaxSource": (url + para),
        "columns": [
            { "title": "序号", "class": "center", "width": 30 }, //v 0
            {"title": "类型", "class": "center","width": 60 }, //v 1
            {"title": "指引1", "class": "center" }, //v 2
            {"title": "指引2", "class": "center" }, //v 2
            {"title": "要素名称", "class": "center", "width": 60 }, //v 2
            {"title": "判断条件", "class": "center" }, //v 2
            {"title": "判断月份", "class": "center" }, //v 2
            {"title": "标准编号", "class": "center", "width": 40 },
            { "title": "月份编号", "class": "center", "width": 40 },
            { "title": "左区间", "class": "center", "width": 40 },
            { "title": "右区间", "class": "center", "width": 40 },
            {"title": "编辑", "class": "center", "width": 40 },
            {"title": "删除", "class": "center", "width": 40 },
                     ],
        "columnDefs": [{
            "targets": 11,
            "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 12,
            "data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
        }
        , {
            "targets": 9,
            "visible": false
        }
         , {
             "targets": 10,
             "visible": false
         }
          , {
              "targets": 7,
              "visible": false
          }
         , {
             "targets": 8,
             "visible": false
         }
        ],
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>", //<h1 style=\"font-size:8px;\">正在加载...</h1>
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "<span style='float:left'>搜索&nbsp;&nbsp</span>",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上一页",
                "sNext": "下一页",
                "sLast": "末页"
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
        var standID = data[7];
        var monthID = data[8];
        if (this.src.indexOf("edit") >= 0) {
            //绑定数据源
            iniInfo2();
            $("#wg3 option[value='" + data[1] + "']").attr("selected", true);
            $("#items_3 option[value='" + data[4] + "']").attr("selected", true);
            $("#zy1_3").val(data[2]);
            $("#zy2_3").val(data[3]);
            var months = data[6];
            var el = document.getElementById("months_3").getElementsByTagName("input");
            for (var i = 0; i < el.length; i++) {
                var id = el[i].id;
                id = id.toString().replace('Checkbox', '').replace('_3', '');
                if ((el[i].type == 'checkbox') && (months.indexOf(id) >= 0)) {
                    el[i].checked = true;
                }
            }

            //分析判断
            if (data[9] != '-9999') {
                $('#s2_3').val(data[9].replace('[', ''));
            }
            if (data[10] != '9999') {
                $('#s5_3').val(data[10].replace(']', ''));
                if (data[9] == '-9999') {
                    $('#s2_3').val(data[10].replace(']', ''));
                    $('#s5_3').val('');
                    //$("#s3_3 option[value='X']").attr("selected", true);
                    setSelectChecked("s3_3", "X")
                }
            }
            if (data[9] != '-9999' && data[10] != '9999') {
                $("#s3_3 option[value='并且']").attr("selected", true);
                if (data[9].indexOf('[') >= 0) {
                    $("#s1_3 option[value='大于等于']").attr("selected", true);
                } else {
                    $("#s1_3 option[value='大于']").attr("selected", true);
                }
                if (data[10].indexOf(']') >= 0) {
                    $("#s4_3 option[value='小于等于']").attr("selected", true);
                } else {
                    $("#s4_3 option[value='小于']").attr("selected", true);
                }
            }
            if (data[9] != '-9999' && data[10] == '9999') {
                if (data[9].indexOf('[') >= 0) {
                    $("#s1_3 option[value='大于等于']").attr("selected", true);
                } else {
                    $("#s1_3 option[value='大于']").attr("selected", true);
                }
               // $("#s3_3 option[value='X']").attr("selected", true);
                setSelectChecked("s3_3", "X")
            }
            if (data[9] == '-9999' && data[10] != '9999') {
                if (data[10].indexOf(']') >= 0) {
                    $("#s1_3 option[value='小于等于']").attr("selected", true);
                } else {
                    $("#s1_3 option[value='小于']").attr("selected", true);
                }
                //$("#s3_3 option[value='X']").attr("selected", true);
                setSelectChecked("s3_3", "X")
            }

            //主键
            $('#standID').val(data[7]);
            $('#monthID').val(data[8]);
            $('#itemName').val(data[4]);
            $('#typeName').val(data[1]);

            dialog_edit.dialog("open");

        }
        else {
            $('#standID').val(data[7]);
            $('#monthID').val(data[8]);
            $('#itemName').val(data[4]);
            $('#typeName').val(data[1]);
            Ext.getDom("toolstr").innerHTML = "是否要删除序号为【" + data[0] + "】的记录？";
            dialogs.dialog("open");
        }
    });

}

function queryData2() { 
    
}

function queryData() {

    if (CurTab == "" || CurTab == "工作组管理" || CurTab == undefined)
        QueryTb0("MMShareBLL.DAL.Scheduing","GetWorkGroup");

    if (CurTab == "工作组人员管理")
        queryData1();

    if (CurTab == "排班顺序管理")
        queryData2();

}

function QueryTb0(provider, method) {

    var url = "PatrolHandler.do?provider=" + provider + "&method=" + method;
    $('#example').empty();
    var datable = $('#example').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": false,
        "bSortClasses": false,
        "sAjaxSource": url,
        "columns": [
            {"title": "序号", "class": "center" }, //v 0
            {"title": "名称", "class": "center" }, //v 1
            {"title": "类型", "class": "center" }, //v 2
            {"title": "编辑", "class": "center","width":40 },
            {"title": "删除", "class": "center","width":40 }             
                     ],
        "columnDefs": [{
            "targets": 3,
            "data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 4,
            "data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
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
        }
    });

    $('#example tbody').on('mouseover', 'tr', function () {
        mouseOver(this);
    });
    $('#example tbody').on('mouseout', 'tr', function () {
        mouseOut(this);
    });

    $('#example tbody').on('click', 'img', function () {
        var data = datable.row($(this).parents('tr')).data();
        CurPID = data[6];
        if (this.src.indexOf("edit") >= 0) {
            openDivII();
            loadFZII(data);
            //绑定数据源
            var name = data[1].toString();
            Ext.getDom("EditorName").value = name;
            var descript = data[4].toString();
            Ext.getDom("description").value = descript;
            var memo = data[5].toString();
            Ext.getDom("memo").value = memo;
            dialog.dialog("open");
        }
        else {
            Ext.getDom("toolstr").innerHTML = "是否要删除【" + data[1] + "】？";
            dialogs.dialog("open");
        }
    });

}

function del() {
  
    var standID = $('#standID').val();
    var monthID = $('#monthID').val();
    var itemNames = $('#itemName').val();
    var typeName = $('#typeName').val();

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'DeleteFYZY'),
        timeout: 120000,
        params: {  standID: standID, monthID: monthID, oldType: typeName, oldItem: itemNames },
        success: function (response) {
            if (response.responseText != "0") {
                Ext.Msg.alert("提示", "删除成功！");
                queryData1();
                dialogs.dialog("close");
            } else {
                Ext.Msg.alert("提示", "删除失败，请联系【系统管理员】！");
                dialogs.dialog("close");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "删除失败，错误代码为：" + response.status);
            dialogs.dialog("close");
        }
    });
}

function openDivII(){
      dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 320,
        width: 363,
        modal: true,
        buttons: {
             "保存": update,
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
        update();
    });
}

function openDiv() {

    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 320,
        width: 363,
        modal: true,
        buttons: {
             "保存": add,
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


function update() {
    var name = Ext.getDom("EditorName").value;
    var type = "默认";
    var pID = "0";
    var IDS = CurPID;
    var descript = Ext.getDom("description").value; ;
    var memo = Ext.getDom("memo").value; ;
    var r1=Ext.getDom("rblSize_1").checked;
    var r2 = Ext.getDom("rblSize_2").checked;
    if (r1 == true) {
        type = "正组";
        pID = $('#sel option:selected').val();
    }
    if (r2 == true)
        type = "副组";

    if (name == "") {
        Ext.Msg.alert("提示", "请输入名称！");
        $('#EditorName').focus();
        return;
    }

    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "UpdateWorkGroup";
    var url = getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        params: { name: name, pid: pID, Type: type, Descript: descript, Memo: memo, IDS: IDS },
        success: function (response) {
            if (response.responseText != "0") {
                Ext.Msg.alert("提示", "编辑成功！");
                //刷新
                QueryTb0("MMShareBLL.DAL.Scheduing", "GetWorkGroup");
                dialog.dialog("close");
            } else {
                Ext.Msg.alert("提示", "编辑失败！");
            }
        }
    });
}

function add() {
    var name = Ext.getDom("EditorName").value;
    var type = "默认";
    var pID = "0";
    var descript = Ext.getDom("description").value; ;
    var memo = Ext.getDom("memo").value; ;
    var r1=Ext.getDom("rblSize_1").checked;
    var r2 = Ext.getDom("rblSize_2").checked;
    if (r1 == true) {
        type = "正组";
        pID = $('#sel option:selected').val();
    }
    if (r2 == true)
        type = "副组";

    if (name == "") {
        Ext.Msg.alert("提示", "请输入名称！");
        $('#EditorName').focus();
        return;
    }

    var provider = "MMShareBLL.DAL.Scheduing";
    var method = "AddWorkGroup";
    var url = getUrl(provider, method);

    Ext.Ajax.request({
        url: url,
        params: { name: name, pid: pID, Type: type, Descript: descript, Memo: memo },
        success: function (response) {
            if (response.responseText != "0") {
                Ext.Msg.alert("提示", "新增成功！");
                //刷新
                QueryTb0("MMShareBLL.DAL.Scheduing", "GetWorkGroup");
                dialog.dialog("close");
            } else {
                Ext.Msg.alert("提示", "新增失败！");
            }
        }
    });
}

function addUser() {
       $('#sel').attr("disabled", "disabled");
       $("#sel").empty();
       openDiv();
       dialog.dialog("open");
    }



function dataExport() {
    var defaultURL = "WebExplorerss.ashx";
    document.getElementById("actionForm").action = defaultURL + "?action=DataExportGuidelines";
    document.getElementById("actionForm").submit();
}


    //王斌  2017/5/12
    function cal() {
        var defaultURL = "WebExplorerss.ashx";
        $.ajax({
            type: "POST",
            url: "WebExplorerss.ashx",
            //data: "{action:'" + Cal + "'}",
            data: { action: "Cal" }, // 你的formid
            async: false,
            success: function (data) {
                if (data == "成功") {
                    alert("重新计算成功");
                } else {
                    alert("重新计算失败");
                }
            }
        });
    }