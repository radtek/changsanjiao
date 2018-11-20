var userName = "";
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


    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#all_Left").width(($(window).width() - 30) / 2);
    $("#all_Right").width(($(window).width() - 30) / 2);
    $("#all_Left").height($(window).height() - 60);
    $("#all_Right").height($(window).height() - 60);

    $("#hazeDropImg").height($(window).height() - 147);
    $("#airPolImg").height($(window).height() - 147);


    $("body").css("min-width", $(window).width() + "px");

    AutoGetHazeDropZone();

    $("#autoGetHaze").click(function () {
        //        AutoGetHazeDropZone(); 
        RefreshHazeDropZone();
    });

    $("#btnHazeSave").click(function () {
        $("#btnHazeCheck").show();
        alert("保存成功！");
    });

    $("#btnHazeCheck").click(function () {
        $("#btnHazePublish").show();
        alert("审核成功！");
    });

    $("#btnHazePublish").click(function () {
        //ftp地址集合字符串
        var ftpString = $("#FtpHazeCollection").val();
        var imgsArray = new Array();
        var fileDate = getFormatDate('');
        var imgsContent = "";

        imgsContent = "hazeDropImg" + ":" + $("#hazeDropImg").attr("src");
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadHazeDropZoneImgsToFTP'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "HazeDropZone", imgURLs: imgsContent, userName: userName },
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

function AutoGetHazeDropZone() {
    var forecastDate = getNowDate();
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetHazeDropAreaData'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                $("#hazeDropImg").attr("src", "../" + response.responseText);
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function RefreshHazeDropZone() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取图片" });
    myMask.show();
    var imgUrl = $("#hazeDropImg").attr("src");
    if (imgUrl != "../css/images/noImg.GIF") {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'RefreshHazeDropAreaData'),
            params: { imgUrl: imgUrl },
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    var date = new Date();
                    var month = ((date.getMonth() + 1) > 10) ? (date.getMonth() + 1).toString : "0" + (date.getMonth() + 1).toString();
                    var dateNum = (date.getDate() > 10) ? date.getDate() : "0" + (date.getDate() + 1).toString();
                    var timeSuffux = date.getFullYear() + month + dateNum + date.getHours() + date.getMinutes() + date.getSeconds(); ;
                    $("#hazeDropImg").attr("src", response.responseText + "?V=" + timeSuffux);
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
    else {
        AutoGetHazeDropZone();
    }
}