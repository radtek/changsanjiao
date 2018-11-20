Ext.onReady(function () {
//    var loginParams = getCookie("UserInfo");
//    var logResult = Ext.util.JSON.decode(loginParams);
//    userName = logResult["Alias"];
    tomainviewer('airQuality,jgRadar,guidance,diagnostic');
    $("#btnBack").click(function () {
//        if (logResult["UserName"] == "JX") {
            window.location.href = "JiangXiHomePage.aspx";
//        }
    });
});