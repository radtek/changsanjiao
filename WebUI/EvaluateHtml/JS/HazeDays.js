
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTable();
    openDiv();
}
)
//获取鼠标按下时的值
function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'HazeDaysScore'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                coutTable0.innerHTML = response.responseText;
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}
function OutTable() {
    var dateTime = Ext.getDom("H00").value;
    var content = dateTime;
    var Element = document.getElementById("Element");
    Element.setAttribute("value", content);
    document.getElementById("btnExport").click();
}
var dialog, dialog_rename, dialog_del;
function openDiv() {
    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 150,
        width: 380,
        title: "导入数据",
        modal: true
    });
}
function getFileName(el) {
    var titleInput = document.getElementById("title");
    var path = el.value;
    var tempPath = GetFileNameNoExt(path);
    var pos1 = tempPath.lastIndexOf('/');
    var pos2 = tempPath.lastIndexOf('\\');
    var pos = Math.max(pos1, pos2)
    var fileName = "";
    if (pos < 0)
        fileName = tempPath;
    else
        fileName = tempPath.substring(pos + 1);

}
//取文件后缀名
function GetFileExt(filepath) {
    if (filepath != "") {
        var pos = "." + filepath.replace(/.+\./, "");
        return pos;
    }
}
//取文件名不带后缀
function GetFileNameNoExt(filepath) {
    var pos = GetFileExt(filepath);
    //    Ext.getDom("fileType").innerHTML = pos.substring(1)
    var pos1 = filepath.replace(pos, "");
    return pos1;

}
var iframeOnload = function () { }
function add() {
    document.getElementById("actionForm").submit();
    iframeOnload = function () {

        dialog.dialog("close");
        InitTable();
    }
    document.getElementById("actionForm").style.display = "none";
    document.getElementById("uploadStatus").style.display = "block";
}
function getDate() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    if (month.toString().length == 1)
        month = "0" + month;
    if (day.toString().length == 1)
        day = "0" + day;

    CurDate = year + '-' + month + '-' + day;
    return CurDate;
}
function closeDialog() {
    dialog.dialog("close");
}
function InsertData() {
    document.getElementById("actionForm").style.display = "block";
    document.getElementById("uploadStatus").style.display = "none";
    var SELDATE = getDate();
    var path = "~/CJDATA/Excel/" + SELDATE + "/";
    var defaultURL = "WebExplorerss.ashx";

    document.getElementById("actionForm").action = defaultURL + "?action=UPLOAD&value1=" + encodeURIComponent(path) ;

    dialog.dialog("open");
}
function Evaluate() {
    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'HazeEvaluate'),
        params: { TimeDate: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                InitTable();
                Ext.Msg.alert("信息", response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
//下载模板 2018.3.7 by 孙明宇
function DownloadData() {
    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'ExportHazeData'),
        params: { TimeDate: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                var defaultURL = "WebExplorerss.ashx";
                window.onbeforeunload = function () { }
                window.location.href = defaultURL + "?action=DOWNLOAD&value1=" + encodeURIComponent(response.responseText);
                window.onbeforeunload = function () {
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
