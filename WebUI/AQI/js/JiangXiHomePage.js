$(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    var alias = "";
    var time = "";
    var JB = "";
    var LoginCount = "";
    var UserName = "";
    if (logResult) {
        alias = logResult["Alias"];
        time = logResult["Local"];
        JB = logResult["JB"];
        LoginCount = logResult["LoginCount"];
        UserName = logResult["UserName"];
    }
    if (logResult && logResult["Alias"] == "江西" && logResult["JB"] == "666") {
        $("#outPdPreview").click(function () {
            //            window.location.href = "http://222.66.83.21:8282/PEMFCShare/ProductPrev.aspx?User=" + logResult["Alias"] + "&JB=" + logResult["JB"];
            window.location.href = "http://222.66.83.21:8282/PEMFCShare/ProductPrev.aspx?logPara=" + loginParams;
            
        });
        $("#InteractiveOUT").click(function () {
            window.location.href = "InteractAna.aspx?V=1";
        });
        $("#productMake").click(function () {
            window.location.href = "MakeAndPub.aspx?V=1";
        });
        $("#dataService").click(function () {
            window.location.href = "DataService.aspx?V=1";
        });
    }
    else {
        window.location.href = "Default.aspx";
    }
});

function addCookie(objName, objValue, objHours) {//添加cookie 
    if (objHours > 0) {
        var exp = new Date();
        exp.setTime(exp.getTime() + objHours * 60 * 60 * 1000);
        document.cookie = objName + "=" + escape(objValue) + ";path=/;expires=" + exp.toGMTString();
    } else {
        document.cookie = objName + "=" + escape(objValue);
    }
}