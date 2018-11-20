Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    //tomainviewer('webGIS,webGIS1');
    tomainviewer('webGISJiangXi,webGISJiangXi2');

    $("#btnBack").click(function () {
        if (logResult["UserName"] == "JX") {
            window.location.href = "JiangXiHomePage.aspx";
        }
    });
});