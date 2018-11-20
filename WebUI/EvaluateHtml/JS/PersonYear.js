Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTable();
}
)
//获取鼠标按下时的值
function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'PersonYearData'),
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
function ResoreData() {
    var dateTime = Ext.getDom("H00").value;
    var content = getContent();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'PersonnalResoreData'),
        params: { dateTime: dateTime, content: content },
        success: function (response) {
            Ext.Msg.alert("信息", response.responseText);
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function getContent() {
    var postJson = "";
    var name = "";
    var oldName = "";
    var index = 0;
    var tempJson = "";
    var id = "";
    var aryDiv = forecastTable.getElementsByTagName("div");
    var length = forecastTable.getElementsByTagName('tr').length - 2;
    for (var j = 0; j < length; j++) {
        tempJson = "";
        for (var i = 0; i < aryDiv.length; i++) {
            if (aryDiv[i].id.indexOf('_') > 0) {
                if (i < 10) {
                    id = aryDiv[i].id.substr(3, 1);
                }
                else {
                    id = aryDiv[i].id.substr(3, 2);
                }
                if (id == j.toString()) {
                    var lastValue = aryDiv[i].innerHTML.trim();
                    index = aryDiv[i].id.indexOf('_');
                    name = aryDiv[i].id.substr(index + 1);
                    tempJson = tempJson + aryDiv[i].id + ":" + lastValue + ",";
                }
            }
        }

        if (postJson == "" && tempJson.length > 0) {
            tempJson = tempJson + "textarea" + j.toString() + "_" + name + ":" + Ext.getDom("textarea" + j.toString() + "_" + name).value;
        }
        postJson = postJson + name + "*" + tempJson;
    }
    return postJson;
}