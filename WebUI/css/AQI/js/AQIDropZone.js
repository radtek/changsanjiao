var userName = "";
Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    userName = result["Alias"];
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

    $("#downloadText").text("未获取文件");

    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#leftArea").width(($(window).width() - 30) * 0.7);
    $("#guideReport").width(($(window).width() - 30) * 0.3);
    // $("#leftArea").height($(window).height() - 40);
    $("#guideReport").height($(window).height() - 50);
    $("body").css("min-width", $(window).width() + "px");
    $(".disImg").height(($(window).height() - 110) / 2);

    $(".totalArea").width(($(window).width() - 30) * 0.7 - 40);
    $(".showimages").width((($(window).width() - 30) * 0.7 - 100) / 3);
    $("#textdownload").width(($(window).width() - 30) * 0.3 - 20);
    $("#textdownload").height(($(window).width() - 30) * 0.3 - 20);
    $("#rightCorner").width((($(window).width() - 30) * 0.7 - 100) / 3);
    $("#rightCorner").height(($(window).height() - 110) / 2);
    AutoGet();
    AutoGetReportText();

    //自动获取(刷新图片)
    $("#autoGet").click(function () {
        //        AutoGet();
        RefreshImg();
    });

    //预报文件自动获取
    $("#txtAutoGet").click(function () {
        AutoGetReportText();
    });

    //保存站点预报文件
    $("#btnSaveAQI").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'InsertIntoStateTable'),
            params: { functionName: "guideReportFile", reTime: GetSQLDatetime(""), deadLine: GetSQLDatetime("after2"), state: "1", type: "2" },
            success: function (response) {
                myMask.hide();
                $("#btnAuthProduct").show();
                alert("保存成功");
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //保存AQI落区图
    $("#btnSave").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'InsertIntoStateTable'),
            params: { functionName: "AQIDropZone", reTime: GetSQLDatetime(""), deadLine: GetSQLDatetime("after2"), state: "1", type: "2" },
            success: function (response) {
                myMask.hide();
                alert("保存成功");
                $("#btnCheck").show();
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //文本的审核
    $("#btnAuthProduct").click(function () {
        $("#btnPush").show();
        alert("审核完成");

    });

    //图片的审核
    $("#btnCheck").click(function () {
        $("#btnPublish").show();
        alert("审核完成");

    });

    //发布站点预报文件
    $("#btnPush").click(function () {
        var ftpString = $("#SiteGuideReportFtp").val();
        var fileDate = getFormatDate('');
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadSiteReportFTP'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "guideReportFile", userName: userName },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("发布成功");
                }
                else if (response.responseText == "less") {
                    alert("发布不完全");
                }
                else {
                    alert(response.responseText);
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //全选
    //    $("#selectAll").click(function () {

    //    });

    $("#selectAll").bind("click", function () {
        $(".topCheckBox").attr("checked", true);
    });

    $('#selectAll').on('change', function () {
        if ($('#selectAll:checked').val()) {
            $(".topCheckBox").attr("checked", $('#selectAll:checked').val());
        }
    })

    //发布到FTP
    $("#btnPublish").click(function () {
        //ftp地址集合字符串
        var ftpString = $("#FtpCollection").val();
        var fileDate = getFormatDate('');
        var imgsArray = new Array();
        var imgsContent = "";
        $.each($(".disImg"), function (i, n) {
            if ($(n).attr("src") != "../css/images/noImg.GIF") {
                imgsArray.push($(n).attr("src"));
                var useSrc = $(n).attr("src").substring(0, $(n).attr("src").indexOf("?V"));
                if ($(n).attr("src").indexOf("?V") > 0) {
                    useSrc = $(n).attr("src").substring(0, $(n).attr("src").indexOf("?V"));
                }
                else {
                    useSrc = $(n).attr("src");
                }

                imgsContent += n.id + ":" + useSrc + ",";
            }
        });
        imgsContent = imgsContent.substring(0, imgsContent.length - 1);
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadAQIDropZoneImgsToFTP'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "AQIDropZone", imgURLs: imgsContent, userName: userName },
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

    $('#bg').live('click', function () {
        $('.bg').fadeOut(800);
        $('#showImg').fadeOut(800);
    });


    //闫海涛修改，添加图片关闭按钮
    $("#closeBtn").live('click', function () {
        $('.bg').fadeOut(800);
        $('#showImg').fadeOut(800);
    });

});

function fadeOut() {
    //    var showImg = Ext.getDom("showImg");
    //    showImg.className = "hidden";
    $('.bg').fadeOut(800);
    $('#showImg').fadeOut(800);
    $('#detailView').fadeOut(800);
}

function showOne(node) {
    $('.bg').fadeIn(200);
    $('#showImg').height($(window).height() - 60);
    $('#showImg').height($(window).height() - 60)
    $('#showImg').fadeIn(400);
    $('#detailView').fadeIn(400);
    $('#detailView').attr("src", $("#" + node.id).attr("src"));
}



function AutoGet() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
    myMask.show();
    //获取一系列图片
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'QueryAQIDropZoneImgsII'),
        //        params: { forecastDate: forecastDate },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                if (response.responseText == "{}") {
                    RefreshImg();
                    return;
                }

                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    if (result[obj] != "") {
                        $("#" + obj).attr("src", "../" + result[obj]);
                    }
                }
            }
            else {
                alert("");
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function RefreshImg() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取图片" });
    myMask.show();
    var imgsContent = "";
    var imgsArray = new Array();
    $.each($(".disImg"), function (i, n) {
        if ($(n).attr("src") != "../css/images/noImg.GIF") {
            imgsArray.push($(n).attr("src"));
            var useSrc = $(n).attr("src").substring(0, $(n).attr("src").indexOf("?V"));
            if ($(n).attr("src").indexOf("?V") > 0) {
                useSrc = $(n).attr("src").substring(0, $(n).attr("src").indexOf("?V"));
            }
            else {
                useSrc = $(n).attr("src");
            }
            imgsContent += n.id + ":" + useSrc + ",";
        }
    });
    imgsContent = imgsContent.substring(0, imgsContent.length - 1);
    //重新获取一系列图片
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'RefreshAQIDropZoneImgsII'),
        params: { imgURLs: imgsContent },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    if (result[obj] != "") {
                        var date = new Date();
                        var month = ((date.getMonth() + 1) > 10) ? (date.getMonth() + 1).toString : "0" + (date.getMonth() + 1).toString();
                        var dateNum = (date.getDate() > 10) ? date.getDate() : "0" + (date.getDate() + 1).toString();
                        var timeSuffux = date.getFullYear() + month + dateNum + date.getHours() + date.getMinutes() + date.getSeconds(); ;
                        $("#" + obj).attr("src", result[obj] + "?V=" + timeSuffux);
                    }
                }
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

//获取用语存入SQL数据库的时间日期字符串
function GetSQLDatetime(type) {

    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var hour = date.getHours();
    var min = date.getMinutes();
    var sec = date.getSeconds();
    var strDate = date.getDate();
    if (type == "tomorrow") {
        strDate = strDate + 1;
    }
    else if (type == "after1") {
        strDate = strDate + 2;
    }
    else if (type == "after2") {
        strDate = strDate + 3;
    }
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + "-" + month + "-" + strDate
            + " 00:00:00.000";
    return currentdate;
}

//自动获取预报文本
function AutoGetReportText() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取预报文件" });
    myMask.show();
    Ext.Ajax.timeout = 900000;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetSiteForecastReport'),
        success: function (response) {
            myMask.hide();
            if (response.responseText.indexOf("success") > -1) {
                var fileName = response.responseText.split('&')[1];
                $("#downloadUrl").attr("href", "../ReportProduce/" + "TxtDownload.ashx?ProductPath=" + fileName);
                $("#downloadIcon").removeClass();
                $("#downloadIcon").addClass("downloadIcon_Success");
                $("#downloadText").text("恭喜！" + "\n" + "文件获取成功");
            }
            else {
                myMask.hide();
                $("#downloadUrl").removeAttr('href');
                $("#downloadIcon").removeClass();
                $("#downloadIcon").addClass("downloadIcon_Fail");
                $("#downloadText").text("抱歉！" + "\n" + "文件获取失败");
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}