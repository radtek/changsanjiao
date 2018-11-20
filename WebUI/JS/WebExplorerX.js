Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    selectOption();
    //queryData();
    openDiv();
}
)
var CurTab;
var CurDate;
var CurArea = "全部";
var CurAreaName = "";
var timer;
var flag = false;
var CurFile, CurID;
var dialog, dialog_rename, dialog_del;
function selectOption() {
    var UserArray = Ext.getDom("Province");
    for (var i = 0; i < UserArray.options.length; i++) {
        if (UserArray.options[i].value == Province) {
            UserArray.options[i].selected = true;
            break;
        }
    }
}
function mouseDown(obj) {
   // $(obj).css("background-image", "url('images/tianjia-v_03.png')");
}

function mouseUp(obj) {
    //$(obj).css("background-image", "url('images/tj-n_03.png')");
}

function mouseOvers(obj) {
    var e = null;
    var _tar = formTarget(e);
    if (!_tar || checkcontainss(obj, _tar)) return true;

    $(obj).css("background-image", "url('images/tj-v.png')");
}

function mouseOuts(obj) {
    var e = null ;
    var _tar = toTarget(e);
    var f = checkcontainss(obj, _tar);
    if (!_tar || f) return;
    $(obj).css("background-image", "url('images/tj-n.png')");
}

function openDiv() {
    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 220,
        width: 380,
        title: "上传文件",
        modal: true
    });

    dialog_rename = $("#dialog-rename").dialog({
        autoOpen: false,
        height: 180,
        width: 410,
        modal: true,
        buttons: {
            "更新": renamefile,
            "关闭": function () {
                dialog_rename.dialog("close");
            }
        },
        close: function () {
            //form[0].reset();
        }
    });


    dialog_del = $("#dialog-del").dialog({
        autoOpen: false,
        height: 200,
        width: 410,
        modal: true,
        buttons: {
            "是": delfile,
            "否": function () {
                dialog_del.dialog("close");
            }
        },
        close: function () {
            //form[0].reset();
        }
    });
}
var iframeOnload = function () { }
function add() {
//    document.getElementById("actionForm").submit();
//    iframeOnload = function () {

//        dialog.dialog("close");
//        
//        queryData();
//    }
//    

//    document.getElementById("actionForm").style.display = "none";
    //    document.getElementById("uploadStatus").style.display = "block";
    $("#uploadFrm").load(function () {
        var body = $(window.frames['uploadFrm'].document.body);
        if (body[0].textContent != "") {
            var result = Ext.util.JSON.decode(body[0].textContent);
            var siteCnName = "";
            for (var obj in result) {
                var siteID = obj;
                for (var cell in result[obj]) {
                    $("#" + cell).text(result[obj][cell]);
                }
            }
            dialog.dialog("close");
        }
    });
    document.getElementById("actionForm").submit();
    document.getElementById("actionForm").style.display = "none";
    document.getElementById("uploadStatus").style.display = "block";
}

function show(obj) {
    //alert(obj);
    var titleValue = "测试文件";
    var titleInput = document.getElementById("title");
  var accept= document.getElementById("fileAccept");
    accept.outerHTML = accept.outerHTML.replace(/(value=\").+\"/i, "$1\"");
    var value = "";
    var UserArray = Ext.getDom("Province"); 
       var selectValue= $('#Province option:selected').val(); //选中的值
    if (obj == "会商PPT") {
//        accept.accept = ".ppt";
        value = titleValue +selectValue+ "会商资料";
    }
    else {
//        accept.accept = ".doc,.docx";
        value = titleValue + obj;
    }
    titleInput.value = value;
    document.getElementById("actionForm").style.display = "block";
    document.getElementById("uploadStatus").style.display = "none";
    CurTab = obj.split(',')[1];
    CurAreaName = obj.split(',')[0];

    var SELDATE = getDate();
    var path = "~/CJDATA/" + CurAreaName + "/" + SELDATE + "/";
    var defaultURL = "WebExplorerss.ashx";
    document.getElementById("actionForm").action = defaultURL + "?action=READSITE&value1=" + encodeURIComponent(path) + "&value2=" + selectValue + "&forecastDate=" + Ext.getDom("H00").value + "&period=" + $("#forePeriodSel").children('option:selected').val() + "&moduleName=" + "NMC";
    dialog.dialog("open");
}
function closeDialog() {
    dialog.dialog("close");
}
function downloads(obj) {

    var ID = obj.parentNode.parentNode.parentNode.parentNode.parentNode.cells[0].innerHTML;
    ID = cutstr(ID, '>', '<');
    var paths = document.getElementById(('hiddenPath' + ID)).value;
    var defaultURL = "WebExplorerss.ashx";
    window.onbeforeunload = function () { }
    window.location.href = defaultURL + "?action=DOWNLOAD&value1=" + encodeURIComponent(paths);
    window.onbeforeunload = function () {

    }
}

function renamefile() {
    var paths = document.getElementById(('hidden' + CurID)).value;
    var newPath =Ext.getDom("file_name").value;
    var defaultURL = "WebExplorerss.ashx";

    iframeOnload = function () {
        // document.getElementById("txt").innerHTML = "文件名修改成功！";
        // queryData();
        dialog_rename.dialog("close");
        Ext.Msg.alert("提示", "文件名修改成功！");
        queryData();

    }

    document.getElementById("actionForms").action = defaultURL + "?action=RENAME&value1=" + paths + "&value2=" + newPath + "&value3=" + CurID;
    document.getElementById("actionForms").submit();
    document.getElementById("actionForms").style.display = "none";
    document.getElementById("txt").style.display = "block";
}

function delfile() {
    var paths = document.getElementById(('hidden' + CurID)).value;
    var defaultURL = "WebExplorerss.ashx";

    iframeOnload = function () {
        dialog_del.dialog("close");
        Ext.Msg.alert("提示", "文件删除成功！");
        queryData();
        dialog_del.dialog("close");
    }

    document.getElementById("actionFormss").action = defaultURL + "?action=DELETE&value1=" + encodeURIComponent(paths) + "&value2=" + CurID;
    document.getElementById("actionFormss").submit();
    document.getElementById("actionFormss").style.display = "none";
    document.getElementById("txt_del").style.display = "block";
}


function rename_open(obj) {
    CurFile = obj.parentNode.parentNode.parentNode.parentNode.parentNode.cells[1].innerHTML;
    CurID = obj.parentNode.parentNode.parentNode.parentNode.parentNode.cells[0].innerHTML;
    CurID = cutstr(CurID, '>', '<');
    CurFile = CurFile.replace(/[ ]/g, "");
    Ext.getDom("file_name").value = CurFile;
    document.getElementById("actionForms").style.display = "block";
    document.getElementById("txt").style.display = "none";
    dialog_rename.dialog("open");
}

function del_open(obj) {
    CurFile = obj.parentNode.parentNode.parentNode.parentNode.parentNode.cells[1].innerHTML;
    CurID = obj.parentNode.parentNode.parentNode.parentNode.parentNode.cells[0].innerHTML;
    CurID = cutstr(CurID, '>', '<');
    CurFile = CurFile.replace(/[ ]/g, "");
    Ext.getDom("txt_file").innerHTML = CurFile;
    document.getElementById("actionFormss").style.display = "block";
    document.getElementById("txt_del").style.display = "none";
    dialog_del.dialog("open");
}

function cutstr(text, start, end) {
    var s = text.indexOf(start)
    if (s > -1) {
        var text2 = text.substr(s + start.length);
        var s2 = text2.indexOf(end);
        if (s2 > -1) {
            result = text2.substr(0, s2);
        } else result = '';

    } else result = '';

    return result;
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

function checkHover(e, target) {
    if (getEvent(e).type == "mouseover") {
        return checkcontains(target, getEvent(e).relatedTarget || getEvent(e).fromElement) && ((getEvent(e).relatedTarget));
    } else {
        return checkcontains(target, getEvent(e).relatedTarget || getEvent(e).toElement) && ((getEvent(e).relatedTarget));
    }
}

function getEvent(e) {
    return e || window.event;
}

function mouseOverII(obj) {
    if (obj != null) {
        flag = true;
    }
}

function mouseOutII(obj) {
    if (obj != null) {
        flag = false;
    }
}

function checkcontains(parentNode, childNode) {
    if (parentNode.contains) {
        return parentNode != childNode && parentNode.contains(childNode);
    } else {
        return !!(parentNode.compareDocumentPosition(childNode) & 16);
    }
}

function checkcontainss(parentNode, childNode) {
    if (parentNode.contains) {
        return parentNode.contains(childNode);
    } else {
        return (parentNode.compareDocumentPosition(childNode) & 16);
    }
}

var formTarget = function (e) {
    var e = e || window.event;
    if (e.relatedTarget) { return e.relatedTarget } else if (e.fromElement) { return e.fromElement }
    return null;
}
var toTarget = function (e) {
    var e = e || window.event;
    if (e.relatedTarget) { return e.relatedTarget } else if (e.toElement) { return e.toElement }
    return null;
}

function mouseOver(obj, e) {
    var _tar = formTarget(e);

    if (!_tar || checkcontainss(obj, _tar)) return true;
    for (var i = 0; i < obj.cells.length; i++) {
        obj.cells[i].bgColor = "#DCDCDC";
        if (i == 4) {
            obj.cells[i].innerHTML = document.getElementById("repeat").innerHTML;

        }
    }
}

function mouseOut(obj, e) {
    var e = e || window.event;
    var _tar = toTarget(e);
    var f = checkcontainss(obj, _tar);
    if (!_tar || f) return;

    //if (e.fromElement.type != "button") {
    for (var i = 0; i < obj.cells.length; i++) {
        obj.cells[i].bgColor = "#FFFFFF";
        if (i == 4) {
            obj.cells[i].innerHTML = "";
        }
    }
}

function getRadioValue() {
    var obj = document.getElementsByName("period");
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].checked) {
                return obj[i].value;
            }
        }
    }
}

function getCheckBValue(objName) {
    var postJson = "";
    var forecasArray = new Array();
    var obj = document.getElementsByName(objName);
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].checked) {
                postJson = postJson + obj[i].value + ",";
            }
        }
    }
    if (postJson.length > 0) {
        postJson = postJson.substring(0, postJson.length - 1);
    }
    return postJson;
}




/** 
* 时间对象的格式化 
*/
Date.prototype.format = function (format) {
    /* 
    * format="yyyy-MM-dd hh:mm:ss"; 
    */
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
- RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
? o[k]
: ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}




//function getGroupEl(id) {
//    var groupEl = "";
//    switch (id) {
//        case "rd1":
//            groupEl = "rd2";
//            break;
//        case "rd2":
//            groupEl = "rd1";
//            break;
//        case "rd3":
//            groupEl = "rd1";
//            break;
//    }
//    return Ext.getDom(groupEl);
//}

function queryData() {
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var area = CurArea;

    QueryTb1("MMShareBLL.DAL.ChangjiangData",
             "GetChangjiangData", fromDate, toDate,"");

}

function today() {
    Ext.getDom("H00").value = Ext.getDom("localTime").value;
    Ext.getDom("H01").value = Ext.getDom("localTime").value;
    queryData();
}


function QueryTb1(provider, method, fromDate, toDate,type) {

    Ext.Ajax.request({
        url: getUrl(provider, method),
        params: { type: type, beginTime: fromDate, endTime: toDate },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);

                var InnerHTML = "<table id=\"DS\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"text-align:center; font-size:15px;table-layout: fixed; \" class=\"treetable\">  " +
                                "    <thead style=\"  visibility:hidden;\">    " +
                                "        <tr>" +
                                "        <th class=\"tabletitleNews\" style=\"width: 50px\"></th>" +
                                "        <th class=\"tabletitleNews\" style=\"width: 180px;\">标题</th>" +
                                "        <th class=\"tabletitleNews\" style=\"width: 134px\">上传时间</th>" +
                                "        <th class=\"tabletitleNews\" style=\"width: 64px\">上传人</th>" +
                                "        <th class=\"tabletitleNews\" style=\"width: 140px\"></th>" +
                                "        </tr> " +
                                "    </thead>" +
                                "     <tbody>";

                var arry = new Array(["会商PPT"], ["预报简报"], ["一周回顾"], ["每日专报"], ["短期专报"]);
//                var areas = new Array(["上海"], ["江苏"], ["安徽"], ["浙江"]);
//                if (area != "")
//                    areas = new Array(area);

                for (var v in arry) {

                    if (v == "remove" || v == "indexOf")
                        continue;
                    InnerHTML += " <tr style=\"text-align:left;\" data-tt-id=\"0\" class=\"branch expanded\">" +
                                 "      <td colspan=\"6\" style=\"text-align: left;  line-height:30px; font-size:18px; font-weight:bold; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: rgb(111, 157, 217); background-color: rgb(255, 255, 255);\"><span class=\"indenter\" style=\"padding-left: 0px;\"><a href=\"#\" title=\"折叠\">&nbsp;</a></span><span style=\"float:left;\">" + arry[v] + " </span><div style=\"  float:right;width:70px;\"><div style=\"  float:left; width:26px; height:30px;\"><button onmouseout=\"mouseOuts(this)\" onmouseover=\"mouseOvers(this)\" onmouseup=\"mouseUp(this)\" onmousedown=\"mouseDown(this)\" onclick=\"show('" + arry[v] + "')\" style=\" border:0 white none; background-color:white; margin-top:2px; width:29px; background-image:url('images/tj-n.png');background-repeat:no-repeat; background-position:center; width:24px; height:27px;  \"  type=\"button\" class=\"ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only\" role=\"button\" aria-disabled=\"false\" title=\"文件上传\"><span class=\"ui-button-text\"></span></button> </div><a href='#' style='float:right; z-index=200000; font-size:14px; color:#000000' >更多....</a> </div></td> " +
                                 " </tr>  ";
                        var json = result.ChangjiangData;
                        for (var i = 0; i < json.length; i++) {

                            var title = json[i][0];
                            if (title == arry[v] ) {
                                var filename = json[i][9];
                                var fileType = json[i][3];
                                var fileSize = json[i][4];
                                var uploadTime = json[i][5];
                                var uploadUser = json[i][6];
                                var filePath = json[i][7];
                                var fileID = json[i][8];
                                //alert(filePath);
                                InnerHTML += "  <tr data-tt-id=\"0.0\" data-tt-parent-id=\"0\" onmouseover=\"mouseOver(this,null)\" onmouseout=\"mouseOut(this,null)\" style=\" cursor:pointer; display: table-row;line-height:25px;\" class=\"leaf expanded\">" +
                                             "    <td  style=\" \" bgcolor=\"#FFFFFF\" ><span class=\"indenter\" style=\"width:0px;padding-left: 0px;display:none; \">" + fileID + "</span><img src=\"images/dot.png\"></td> " +
                                             "    <td colspan=\"2\" style=\"text-align:left;\" bgcolor=\"#FFFFFF\"> " + filename + " </td> " +
                                             "    <td colspan=\"2\"  style=\"text-align:left;\" bgcolor=\"#FFFFFF\"> " + uploadTime + " </td> " +
                                             "    <td style=\"text-align:center;\" bgcolor=\"#FFFFFF\">  " + uploadUser + " <input id='hidden" + fileID + "' type=\"hidden\" value='" + filename + "' runat=\"server\"/><input id='hiddenPath" + fileID + "' type=\"hidden\" value='" + filePath + "' runat=\"server\"/></td> " +
                                             "    <td style=\"text-align:left;\" bgcolor=\"#FFFFFF\">  </td>  " +
                                             "  </tr> ";
                            }
                        }
                    }
                InnerHTML += "     </tbody>" +
                                "     </table>";
                //******************************
                document.getElementById("content").innerHTML = InnerHTML;

            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}





