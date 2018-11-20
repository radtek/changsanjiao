Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    tomainviewer('WeatherPollution,SuperStation');
    $("#btnBack").click(function () {
        if (logResult["UserName"] == "JX") {
            window.location.href = "JiangXiHomePage.aspx";
        }
    });
});