Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());


    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#all-Left").width(pageWidth * 0.5 - 20);
    $("#all-Right").width(pageWidth * 0.5 - 20);

    $("#all-Left").height((pageHeight - 30)*0.99  - 20);
    $("#all-Right").height((pageHeight - 30) * 0.99 - 20);
//    $(".disImg").height((pageHeight - 30) * 0.6);
    var oDiv = document.getElementById("change");
    var oLi = oDiv.getElementsByTagName("div")[0].getElementsByTagName("li");
    var aCon = oDiv.getElementsByTagName("div")[1].getElementsByClassName("panel");
    for (var i = 0; i < oLi.length; i++) {
        oLi[i].index = i;
        oLi[i].onmouseover = function () {     /* 鼠标移动到当前时运行的脚本 */
            show(this.index);
        }
    }
    function show(a) {
        aCon[a].style.display = "block";  /* 格式设置为可见 */
        oLi[a].className = "tabs-selected";        /* 将li的class定义为tabs-select */
        for (var j = 0; j < oLi.length; j++) {
            if (j != a) {                /* 如果是其他与鼠标移动到的界面不同的界面 */
                aCon[j].style.display = "none";
                oLi[j].className = "";
            }
        }
    }

    $("#btnupdown").click(function () {
        $(".trimg").slideToggle("slow");
        $(this).toggleClass("buttondown");
        return false;
    });


    //获取一系列图片
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetJiangXiAirPollutionTwo'),
        //        params: { forecastDate: forecastDate },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    if (result[obj] != "") {
                        $("#" + obj).attr("src", "../../" + result[obj]);
                    }
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
});