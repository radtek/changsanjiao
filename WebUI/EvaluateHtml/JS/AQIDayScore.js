Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTable();
    $("#H00").val();
    //openDiv();
}
)
//获取鼠标按下时的值
function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionReport', 'CalculateIcsAndSts'),
        params: { forecastDate: dateTime },
        success: function (response) {
            var tableHtml = "";
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                tableHtml = "<table width='100%' border='0' cellpadding='0' cellspacing='0'><thead><td class='tabletitleHazeDay'>日期</td><td class='tabletitleHazeDay'>临界成功指数</td><td class='tabletitleHazeDay'>真实技巧统计量</td><thead>";
                for (var obj in result) {
                    tableHtml += "<tr><td class='tableRow' >" + obj + "</td><td class='tableRow' >" + result[obj].split('&')[0] + "</td><td class='tableRow' >" + result[obj].split('&')[1] + "</td><tr>";
                }
                tableHtml += "</table>";
                $("#leftTable").html(tableHtml);
            }
            else {
                tableHtml = "<table width='100%' border='0' cellpadding='0' cellspacing='0'><thead><td class='tabletitleHazeDay'>日期</td><td class='tabletitleHazeDay'>临界成功指数</td><td class='tabletitleHazeDay'>真实技巧统计量</td><thead></table>";
                $("#leftTable").html(tableHtml);
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