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
        url: getUrl('MMShareBLL.DAL.ScoreData', 'ReturnDayScore'),//2018-3-12 by 孙明宇
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

function OutTable() {
    var dateTime = Ext.getDom("H00").value;
    var content = dateTime;
    var Element = document.getElementById("Element");
    Element.setAttribute("value", content);
    document.getElementById("btnExport").click();
}
