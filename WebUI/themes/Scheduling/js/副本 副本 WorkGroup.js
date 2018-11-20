

Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    queryData();
}
)

var CurTab; 

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

//预报污染物tab切换
function tabClick(id) {
    var lastParts = lastTab.split("_");
    var lastID = lastParts[1];
    var curParts = id.split("_");
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTab);
    lastEl.className = "";

    hideSiteContainer();
    hideGroupContainer();

    CurTab = curParts[0];
    if (curParts[0] == "工作组管理") {
        queryData();
    }

    if (curParts[0] == "工作人员管理") {
        queryData();
    }

    if (curParts[0] == "排班顺序管理") {
        queryData();
    }

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
       
        var obj = datable.row($(this).parents('tr'));
        var data = datable.row($(this).parents('tr')).data();
        if (this.src.indexOf("edit")>=0)
            alert("编辑");
        else
            alert("删除");

    });

}

function addUser() {
    alert("xx");
}


function hideSiteContainer() {
    document.getElementById('panSiteContainer').style.display = 'none';
}

function hideGroupContainer() {
    document.getElementById('panGroupContainer').style.display = 'none';
}

function displaySiteContainer(container) {
    var currID = document.getElementById("selectFilter").innerHTML;
    if (currID == "选择站点") {
        document.getElementById('panSiteContainer').style.display = 'block';
        var str = $("#HiddenField_Sites").val()
        document.getElementById("HiddenField_currID").value = document.getElementById("selectFilter").innerHTML;

        var theChkList = container.find("INPUT");
        for (var i = 0; i < theChkList.length; i++) {
            var theChk = theChkList[i];
            if ("checkbox" == theChk.type.toLowerCase())
                theChk.checked = false;
        }

        var strs = str.split(",")

        for (var i = 0; i < theChkList.length; i++) {
            var theChk = theChkList[i];
            if ("checkbox" == theChk.type.toLowerCase()) {
                for (j = 0; j < strs.length; j++) {

                    if (theChk.parentNode.getAttribute("si") == null)
                        continue;

                    if (strs[j].split(":")[0].toString() == theChk.parentNode.
                                          getAttribute("si").toString())
                    { theChk.checked = true; }
                }
            }
        }
    } else {

        document.getElementById('panGroupContainer').style.display = 'block';
        var str = $("#HiddenField_Groups").val()
        document.getElementById("HiddenField_currID").value = document.getElementById("selectFilter").innerHTML;

        var theChkList = container.find("INPUT");
        for (var i = 0; i < theChkList.length; i++) {
            var theChk = theChkList[i];
            if ("checkbox" == theChk.type.toLowerCase())
                theChk.checked = false;
        }

        var strs = str.split(",")

        for (var i = 0; i < theChkList.length; i++) {
            var theChk = theChkList[i];
            if ("checkbox" == theChk.type.toLowerCase()) {
                for (j = 0; j < strs.length; j++) {

                    if (theChk.parentNode.getAttribute("si") == null)
                        continue;

                    if (strs[j].split(":")[0].toString() == theChk.parentNode.
                                          getAttribute("si").toString())
                    { theChk.checked = true; }
                }
            }
        }
    }

}

function selectAll(container) {
    var theChkList = container.find("INPUT");
    for (var i = 0; i < theChkList.length; i++) {
        var theChk = theChkList[i];
        if ("checkbox" == theChk.type.toLowerCase())
            theChk.checked = true;
    }
}

function reverseSelect(container) {
    var theChkList = container.find("INPUT");
    for (var i = 0; i < theChkList.length; i++) {
        var theChk = theChkList[i];
        if ("checkbox" == theChk.type.toLowerCase())
            theChk.checked = !theChk.checked;
    }
}

function getSelectNodes(container) {
    var strParams = "";
    var siteStatus = "";
    var theChkList = container.find("INPUT");
    for (var i = 0; i < theChkList.length; i++) {
        var theChk = theChkList[i];
        if ("checkbox" == theChk.type.toLowerCase() && theChk.checked) {

            if (theChk.parentNode.getAttribute("si") == null)
                continue;

            strParams += (theChk.parentNode.getAttribute("si") + ":" + theChk.parentNode.getAttribute("sn") + ",")
            siteStatus += (theChk.parentNode.getAttribute("si") + ":" + theChk.parentNode.getAttribute("Type") + ",")
        }
    }
    if (strParams.length > 0)
        strParams = strParams.substring(0, strParams.length - 1);

    if (siteStatus.length > 0)
        siteStatus = siteStatus.substring(0, siteStatus.length - 1);

    document.getElementById("HiddenField_Sites_status").value = siteStatus;
    document.getElementById("HiddenField_Sites").value = strParams;
}

function getSelectedParameters(container) {
    getSelectNodes(container);
    queryData1();
}

function getGroupSelectNodes(container) {
    var strParams = "";
    var theChkList = container.find("INPUT");
    for (var i = 0; i < theChkList.length; i++) {
        var theChk = theChkList[i];
        if ("checkbox" == theChk.type.toLowerCase() && theChk.checked) {

            if (theChk.parentNode.getAttribute("si") == null)
                continue;

            strParams += (theChk.parentNode.getAttribute("si") + ":" + theChk.parentNode.getAttribute("sn") + ",")
        }
    }
    if (strParams.length > 0)
        strParams = strParams.substring(0, strParams.length - 1);

    document.getElementById("HiddenField_Groups").value = strParams;

}
