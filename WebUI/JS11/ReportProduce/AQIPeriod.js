Ext.onReady(function () {


    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").text(result["Alias"]);
    $("#forecastTime").text(result["Local"]);
    $("#forecastTimeLevel").text("17时");
    var todayDate = new Date();

    $(".today").text(getDateString(todayDate, "today"));
    $(".tomorrow").text(getDateString(todayDate, "tomorrow"));
    $(".theDayAfter").text(getDateString(todayDate, "after"));

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetAQIForecast'),
        //            params: { entityName: id },
        params: { tableName: "T_ForecastSite", curDate: todayDate, siteID: "58637" },
        success: function (response) {
            //var result = Ext.util.JSON.decode(response.responseText);                

        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }

    });
});


//将从json获取的日期字符串转换为显示格式的日期
function getDateString(date,type) {
    var newdate = date.getFullYear() + '年';
    newdate = date.getMonth() + 1 + '月';
    
//    var hour = date.getHours();
//    hour = hour < 10 ? "0" + hour.toString() : hour;
//    newdate += hour + '时';
    //    newdate += '00分';
    if (type == "today") {
        newdate += date.getDate() + '日';
        return newdate;
    }
    else if (type == "tomorrow") {
        newdate += date.getDate()+1 + '日';
        return newdate;
    }
    else if (type == "after") {
        newdate += date.getDate() + 2 + '日';
        return newdate;
    }
}