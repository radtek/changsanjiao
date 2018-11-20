$(function () {
    $("#btnupdown").click(function () {
        $(".trimg").slideToggle("slow");
        $(this).toggleClass("buttondown");
        return false;
    });

    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

    var forecastDate = getNowDate();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetJiangXiAirQuaIndexImg'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            if (response.responseText != "") {
                $("#hazeDropImg").attr("src", "../../" + response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
});