var userName="";
$(function () {
    $("#btnupdown").click(function () {
        $(".trimg").slideToggle("slow");
        $(this).toggleClass("buttondown");
        return false;
    });

    var pageHeight = document.body.clientHeight - 107;

    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    userName = result["Alias"];
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

    AutoGetAirPolDropZone();
    $("#autoGetAirPol").click(function () {
        //AutoGetAirPolDropZone();
        RefreshAirPolDropZone();
    });

    $("#btnAirPolSave").click(function () {
        $("#btnAirCheck").show();
        alert("保存成功！");
    });

    $("#btnAirCheck").click(function () {
        $("#btnAirPolPub").show();
        alert("审核成功！");
    });


    $("#btnAirPolPub").click(function () {
        //ftp地址集合字符串    
        var ftpString = $("#FtpAirPolCollection").val();
        var imgsArray = new Array();
        var fileDate = getFormatDate('');
        var imgsContent = "";
        imgsContent = "airPolImg" + ":" + $("#airPolImg").attr("src");
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadAirPolDropZoneImgsToFTP'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "AirPollutionDropZone", imgURLs: imgsContent, userName: userName },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("发布成功！");
                }
                else if (response.responseText == "less") {
                    alert("发布不完全");
                }
                else {
                    alert("发布失败");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });
});

function AutoGetAirPolDropZone() {
    var forecastDate = getNowDate();
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetAirPollutionDropAreaData'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                var imgPath = response.responseText;
                if (imgPath.indexOf("?") < 0) {
                    imgPath += "?=v" + Math.random() * 1000;
                }
                $("#airPolImg").attr("src", "../" + imgPath);
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function RefreshAirPolDropZone() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取图片" });
    myMask.show();
    var imgUrl = $("#airPolImg").attr("src");
    if (imgUrl != "../css/images/noImg.GIF") {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'RefreshAirPollutionDropAreaData'),
            params: { imgUrl: imgUrl },
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    var date = new Date();
                    var month = ((date.getMonth() + 1) > 10) ? (date.getMonth() + 1).toString : "0" + (date.getMonth() + 1).toString();
                    var dateNum = (date.getDate() > 10) ? date.getDate() : "0" + (date.getDate() + 1).toString();
                    var timeSuffux = date.getFullYear() + month + dateNum + date.getHours() + date.getMinutes() + date.getSeconds(); ;
                    $("#airPolImg").attr("src", response.responseText + "?V=" + timeSuffux);
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
    else {
        AutoGetAirPolDropZone();
    }
}