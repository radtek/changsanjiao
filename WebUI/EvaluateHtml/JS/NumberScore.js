Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTable();
}
)


function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'returnDayDurScore'),//2018-3-12 by 孙明宇
        params: { TimeDate: dateTime },//
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


//实况数据预报数据
function originTable(id) {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    var type = id;
    $.ajax({
        url: "NumberScore.aspx/ReturnOriginData",
        type: "POST",
        contentType: "application/json",
        data: "{dateTime:'" + dateTime + "',type:'"+type+"'}",
        dataType: 'json',
        success: function (response) {
            if (response.d != "") {
                coutTable0.innerHTML = response.d;
                //可以通过hideRow方法隐藏行
            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}


function hideRow() {
    var tableInfo = "";
    var tableObj = $("#coutTable0").children("table")[0];//通过children或者find方法获取的table对象
    for (var i = 2; i < tableObj.rows.length; i++) {    //遍历Table的所有Row
        var row = tableObj.rows[i];
        var length = $(row).find("td").length;
        var span = $(row).find("td")[0].rowSpan = 1;
        var grid;
        if (length==1) {
            grid = $(row).find("td")[0].innerText;
        }
        else {
            grid = $(row).find("td")[1].innerText;
        }
        if (grid.indexOf("气象部门") == -1) {
            
            $(row).hide();
        }
    }
}

function OutTable() {
    var dateTime = Ext.getDom("H00").value;
    var content = dateTime;
    var Element = document.getElementById("Element");
    Element.setAttribute("value", content);
    document.getElementById("btnExport").click();

}

//返回详细数据 2018-3-12 by 孙明宇
function ForeTable() {
    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'returnForeData'),//
        params: { TimeDate: dateTime },//
        success: function (response) {
            if (response.responseText != "") {
                $('#personMonth').html(response.responseText);
                var showImg = Ext.getDom("showImg");
                showImg.className = "show2";
                $('.bg').fadeIn(200);
                $('.showImg').fadeIn(400);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

//隐藏详细数据 2018-3-12 by 孙明宇
function fadeOut() {
    var showImg = Ext.getDom("showImg");
    $('.showImg').fadeOut(800);
    showImg.className = "hidden";
    $('.bg').fadeOut(800);
}