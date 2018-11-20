
var dialogs_dels, dialogs_add, dialogs_send, dialogs_preView;
var curWarningNum, UserName;
var m_selectUser = [];
var m_delUser = [];
var warningcode = "";
var html = "<select name='TypeItems' class='grouplist'  style='width:85px; margin-left:2px;'>" +
              "        <option selected='selected' value='蓝色'>蓝色</option> " +
              "                   <option value='黄色'>黄色</option> " +
              "                   <option value='橙色'>橙色</option> " +
              " </select>";
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    //initInputHighlightScript();
    GetUserName();
    loadWarningType(); //绑定预警类型
    LoadDelConfirm(); //初始化删除的模态框
    LoadFTPConfirm(); //初始化上传模态框
    LoadAddConfirm(); //初始化新增的模态框
    LoadPreViewConfirm();
  
 }
)


function GetUserName()
{
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    UserName = logResult["Alias"];
}

//获取新增预警下面的内容，以为在div里面嵌套了其他的标签
function getWarnContent() {
    var msg = $('#contentS').html();
    var reg = /<select([\s\S]*?)<\/select>/;
    var reg2 = /<input([\s\S]*?)\/?>/;
    var grade = "";
    $.each($(".grouplist"), function (i, n) {
        if (i > 0) {
            grade = $($(".grouplist option:selected")[i]).text();
            var time = $(".time").val();
            msg = msg.replace(reg, grade);
            msg = msg.replace(reg2, time);
        }
    });
    return msg;
}

function ftpSend() {
    if (!confirm("是否要发布？")) return;
    var pubTxt = getWarnContent();
    var title = $("#wr2 option:selected").text();
    var warninggrade = "";
    var beforeUpdate = $("#contentS .grouplist").eq(0).val();
    if ($("#contentS .grouplist").eq(1).length>0) {   //两种情况，有更新和没有更新
        warninggrade = $("#contentS .grouplist").eq(1).val();
    } else {
        warninggrade = beforeUpdate;
        beforeUpdate = "";
    }
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.WarningMsg', 'SendFtp'),
        timeout: 120000,
        params: { sendmessage: pubTxt, beforeUpdate: beforeUpdate, warninggrade: warninggrade, user: UserName, title: title },
        success: function (response) {
            if (response.responseText == "ok") {
                alert("发布成功！");
                dialogs_preView.dialog('close');
                $("#dialog-Ftp").dialog('close');

            } else {
                alert("发布失败！");
                dialogs_preView.dialog('close');
                $("#dialog-Ftp").dialog('close');
            }
            queryData();
        },
        //failure: function (response) {
        //    Ext.Msg.alert("错误", "上传失败，错误代码为：" + response.status);
        //}
    });
}
function LoadDelConfirm() {
    dialogs_dels = $("#dialog-confirm").dialog({
        autoOpen: false,
        modal: true,
        width: 550,
        buttons: {
            "删除": delWarning,
            "关闭": function () {
                $(this).dialog("close");
            }
        }
    });
}


function LoadFTPConfirm() {
    dialogs_send = $("#dialog-Ftp").dialog({
        autoOpen: false,
        modal: true,
        width: 550,
        buttons: {
            "上传": ftpSend,
            "关闭": function () {
                $(this).dialog("close");
         
            }
        }
    });
}

//这个方法后来加的，主要是为了替换文本显示的时间
function showAddDialogs() {
    QueryWarningContent();
    dialogs_add.dialog('open')
   // dialogs_preView.dialog('open')
}
function LoadAddConfirm() {
    dialogs_add = $('#dialog-form-add').dialog(
      {
       autoOpen: false,
       modal: true,
       width: 670,
       buttons: {
        //"预览": preView,
        "保存": AddWarning,
        //"关闭": function () {
        //    $(this).dialog("close");
           //}
        "发布": preView
      }
    });
}

function LoadPreViewConfirm() {
    dialogs_preView = $('#dialog-preView').dialog(
      {
          autoOpen: false,
          modal: true,
          width: 670,
          title:"",
          buttons: {
              "关闭": function () {
                  $(this).dialog("close");
              },
              "确认": ftpSend,
          }
      });
}


function preView() {
    $.ajax({    //写在后台不好调试
        url: "WarningGroup.aspx/PreView",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var data = results.d;
            $("#dialog-preView").attr("title", data.split("#")[1])
            dialogs_preView.dialog('open');
            dialogs_preView.title = data.split("#")[1];
            $("#preView").text(data.split("#")[0]);
        }
    });
}




function ftpsendModule() {
    warningcode = "";
    LoadFTPConfirm();
    for (var i = 0; i < m_selectUser.length; i++) {
        warningcode += m_selectUser[i][0].toString()+',';
    }
    warningcode = warningcode.substring(0, warningcode.length - 1);
    Ext.getDom("toolstr1").innerHTML = "是否要发布预警编号为【" + warningcode + "】的记录？";
    if (warningcode != "") {
        dialogs_send.dialog("open");
    }
    else {
        alert("请选择需要发布的预警信息!");
    }
}


function insertData(rec)
{

    if (rec != "") {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.WarningMsg', 'InsertMsg'),
            timeout: 120000,
            params: { InsertRecord: rec },
            success: function (response) {
                if (response.responseText == "OK") {
                    Ext.Msg.alert("提示", "添加成功！");
                    $('#dialog-form-add').dialog('close');
                    queryData();
                } else {
                    Ext.Msg.alert("错误", "添加失败，错误代码为：" + response.status);
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "添加失败，错误代码为：" + response.status);
            }
        });
    }
}

function AddWarning()
{
    var msg = getWarnContent();
    //var record = "'number','" + UserName + "','time','" + warType + "','','" + grade + "','" + msg + "',''";
    // insertData(record);
    save(msg);  //保存文本到本地
}


function save(msg) {
    $.ajax({    //写在后台不好调试
        url: "WarningGroup.aspx/Save",
        type: "POST",
        contentType: "application/json",
        data: "{content:'" + msg + "'}",
        dataType: 'json',
        success: function (results) {
            var data = results.d;
            if (data == "ok") {
                alert("保存成功！");
            } else {
                alert("保存失败");
            }
        }
    });
}

Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}


function delWarning() {
    dialogs_dels.dialog("close");
    if (curWarningNum != "") {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.WarningMsg', 'DelWarningMsg'),
            timeout: 120000,
            params: { IwaningNum: curWarningNum },
            success: function (response) {
                if (response.responseText == "OK") {
                    Ext.Msg.alert("提示", "删除成功！");
                    queryData();
                } else {
                    Ext.Msg.alert("错误", "删除失败，错误代码为：" + response.status);
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "删除失败，错误代码为：" + response.status);
            }
        });
    }
}

function QueryWarningContent() {
    var time = new Date().Format("dd日hh时mm分");
    var wrkg = $('#wr2 option:selected').val();
    var reg = /\{0\}/g;   // /g设置全局
    wrkg = wrkg.replace(reg, html).replace("{time}","<input class='time' value="+time+" />");
    $("#contentS").html(wrkg);
}

function loadWarningType() {
    var provider = "MMShareBLL.DAL.WarningMsg";
    var method = "GetWarningType";
    var url = getUrl(provider, method);
    Ext.Ajax.request({
        url: url,
        async: true,
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                var InnerHTML = "<select id=\"wr\" style=\"width:150px;\" onchange=\"queryData();\" >  ";
                InnerHTML += "<option value=全部>全部</option>";
                var json = result.data;
                for (var i = 0; i < json.length; i++) {
                    var values = json[i].toString().split(',');
                    InnerHTML += "<option value=" + values[1] + ">" + values[1] + "</option>";
                }
                InnerHTML += "</select>";
                document.getElementById("workGroup").innerHTML = ("预警类型：" + InnerHTML);


                var InnerHTML = "<select id=\"wr2\" style=\"width:85px;\" onchange=\"QueryWarningContent();\" >";
                json = result.data;
                for (var i = 0; i < json.length; i++) {
                    var values = json[i].toString().split(',');
                    InnerHTML += "<option value=" + values[2] + ">" + values[1] + "</option>";
                }
                InnerHTML += "</select>";
                document.getElementById("wrnTypeAdd").innerHTML = ("预警类型：" + InnerHTML);
                queryData();
                QueryWarningContent();
            }
        }
    });
}

function queryData() {
    var wrkg = $('#wr option:selected').val();
    var type = $('#TypeList option:selected').val();
    var status = $("#status").val();
    if (wrkg == undefined) {
        try{
            wrkg = Ext.getDom("wr").options[0].value;
           }
        catch (exception) {
          return false;
        }
    }
 
  var provider = "MMShareBLL.DAL.WarningMsg";
  var method = "GetWarningTable";
  var para = "&text=" + wrkg + "&type=" + type + "&status=" + status;
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
        "ordering":false,
        "columns": [
            { "title": "预警编号", "class": "center" }, //v 0
            { "title": "发布人", "class": "center" }, //v 0
            { "title": "发布时间", "class": "center" }, //v 1
            { "title": "标题", "class": "center" }, //v 2
            { "title": "预警级别", "class": "center" }, //v 2
            { "title": "状态", "class": "center" }, //v 2
            { "title": "预警内容", "class": "center" }, //v 2
            { "title": "删除", "class": "left" },
                     ],
        "columnDefs": [
        {
            "targets":7,
            "data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
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

    $('#example_us tbody').on('click', 'img', function () {
        var data = datable.row($(this).parents('tr')).data();
        var standID = data[2];
        var monthID = data[4];
        if (this.src.indexOf("delete") >= 0) {
            //绑定数据源   
            Ext.getDom("toolstr").innerHTML = "是否要删除预警编号为【" + data[0] + "】的记录？";
            curWarningNum = data[0];
            dialogs_dels.dialog("open");
        }
    });

    m_selectUser =[];
    m_delUser =[];  //删除多行数据时，存储要删除行的ID
    var num = 1;   //如果是1则删除一行，如果是2则删除多行
    var count = 0;  //记录选中的行数
    $('#example_us tbody').on('click', 'tr', function (event) {
        if (event.target.nodeName=="IMG") {
            return;
        }
        num = 2;
        var delData = datable.row($(this)).data()[0];
        var data = datable.row($(this)).data();
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



