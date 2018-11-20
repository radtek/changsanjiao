Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    //tomainviewer('ReportWorkArea,reprotProduce,EastChinaReprotProduce,ForecastScore');
    tomainviewer('ReportWorkAreaJX,JiangXiAQIPart,ForecastScoreJX');
    $("#btnBack").click(function () {
        if (logResult["UserName"] == "JX") {
            window.location.href = "JiangXiHomePage.aspx";
        }
    });
});