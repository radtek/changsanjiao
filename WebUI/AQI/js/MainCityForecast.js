var previewFile = "";
var userName = "";
Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");
    $("#productType").text("未来10天本市空气质量预测专报");
    $("#PO_docDate").val(getFormatDate(""));
    $("#PO_year").val(new Date().getFullYear());

    setTableDates();

    //获取主要城市的数据
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetMainCityForecastData'),
        //params: { data: cells, strForecastDate: "", strPeriod: "24" },
        success: function (response) {
            if (response.responseText != "") {
                if (response.responseText != "fail") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    if (result.length > 0) {
                        for (var obj in result) {
                            $("#PO_" + obj).val(result[obj]);
                        }
                    }
                }
                else {
                    alert("暂未读取到数据！");
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    //保存
    $("#foreSave").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveFutureTenDaysWord'),
            params: { wordTempContent: getWordContent(), productName: "MainCityForecast" },
            success: function (response) {
                myMask.hide();
                if (response.responseText.split('&')[0] == "success") {

                    previewFile = response.responseText.split('&')[1];
                    alert("保存成功！");
                    $("#preview").show();
                    $("#forePub").show();
                }
                else {
                    alert("保存失败！");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    })

    //一键查询
    $("#clickQuery").click(function () {

        var searchDate = $("#searchDate").val();
        if (searchDate == "") {
            alert("请选择查询日期！");
            return;
        }
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'QueryWordWithYearAndIssue'),
            params: { productName: "MainCityForecast", searchDate: searchDate },
            success: function (response) {
                if (response.responseText != "") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        $("#" + obj).val(result[obj])
                    }
                    alert("查询获取成功！");
                }
                else {
                    alert("获取失败！");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });

    })

    //读取模板
    $("#readMOdel").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'ReadPreviewModelModel'),
            params: { productName: "MainCityForecast" },
            success: function (response) {
                if (response.responseText != "") {
                    var content = s = response.responseText.replace('\n', '');
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        $("#" + obj).val(result[obj])
                    }
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    })


    //发布
    $("#forePub").click(function () {
        var ftpString = $("#FtpCollection").val();
        var issueNum = $("#issueSet").val();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'PublishFutureTenDaysWord'),
            params: { ftpString: ftpString, functionName: "MainCityForecast", issueNum: issueNum, userName: userName },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("发布成功！");
                }
                else {
                    alert("发布失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //历史记录，读取上一次存储的word内容json文本，替换到页面上，但是日期是最新的

    $("#getHihstory").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetWordHistory'),
            params: { productName: "MainCityForecast" },
            success: function (response) {
                if (response.responseText != "") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        $("#" + obj).val(result[obj])
                    }
                }
                else {
                    alert("获取历史失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });
    if ($.browser.msie) {
        //实况值改变 
        $("#issueSet").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_issueNum").text($("#issueSet").val());
        });
    }
    else {
        //实况值改变
        $("#issueSet").get(0).addEventListener("input", function (o) {
            $("#PO_issueNum").text($("#issueSet").val());
        }, false);
    }

});

//将页面上的内容传给PageOffice Word预览页面
function prepare() {
    var pageContent = "";
    var cityArray = ["Shanghai", "Nanjing", "Suzhou", "Hangzhou", "Ningbo", "Hefei", "Fuzhou", "Xiamen", "Nanchang", "Jinan", "Qingdao"];
    for (var i = 0; i < cityArray.length; i++) {
        pageContent += "PO_PoLevel_" + cityArray[i] + "=" + $("#" + "PO_PoLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_FirstItem_" + cityArray[i] + "=" + $("#" + "PO_FirstItem_" + cityArray[i]).val() + "&";
        pageContent += "PO_AQI_" + cityArray[i] + "=" + $("#" + "PO_AQI_" + cityArray[i]).val() + "&";
        pageContent += "PO_AirPolLevel_" + cityArray[i] + "=" + $("#" + "PO_AirPolLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_Haze_" + cityArray[i] + "=" + $("#" + "PO_Haze_" + cityArray[i]).val() + "&";
    }
    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_Sign" + "=" + $("#PO_Sign").val();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveWordContentToText'),
        params: { wordPartContent: pageContent, pruductFileName: "FutureTenDays" },
        success: function (response) {
            if (response.responseText == "success") {
                $("#preview").show();
                alert("暂存成功");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

//设置页面内容当中表格内的日期
function setTableDates() {
    var date = new Date();
    for (var i = 1; i < 11; i++) {

        var date = new Date();
        date.setDate(date.getDate() + i);
        var dateString = date.getMonth() + 1 + "月" + date.getDate() + "日";
        $("#PO_Date" + i.toString()).text(dateString);
        if (i < 8) {
            $("#PO_DateSeven" + i.toString()).text(dateString);
        }
    }
}

function getWordContent() {
    var pageContent = "";
    pageContent += "PO_year" + "=" + $("#PO_year").val() + "&";
    pageContent += "PO_issueNum" + "=" + $("#PO_issueNum").val() + "&";
    pageContent += "PO_docDate" + "=" + $("#PO_docDate").val() + "&";
    pageContent += "Po_TodayDate" + "=" + $("#Po_TodayDate").val() + "&";
    
    pageContent += "PO_ForeText" + "=" + $("#PO_ForeText").val() + "&";
    var cityArray = ["Shanghai", "Nanjing", "Suzhou", "Hangzhou", "Ningbo", "Hefei", "Fuzhou", "Xiamen", "Nanchang", "Jinan", "Qingdao"];
    for (var i = 0; i < cityArray.length; i++) {
        pageContent += "PO_PoLevel_" + cityArray[i] + "=" + $("#" + "PO_PoLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_FirstItem_" + cityArray[i] + "=" + $("#" + "PO_FirstItem_" + cityArray[i]).val() + "&";
        pageContent += "PO_AQI_" + cityArray[i] + "=" + $("#" + "PO_AQI_" + cityArray[i]).val() + "&";
        pageContent += "PO_AirPolLevel_" + cityArray[i] + "=" + $("#" + "PO_AirPolLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_Haze_" + cityArray[i] + "=" + $("#" + "PO_Haze_" + cityArray[i]).val() + "&";
    }
    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_Sign" + "=" + $("#PO_Sign").val();
    return pageContent;
}
function fadeOut() {
    var showImg = Ext.getDom("showImg");
    showImg.className = "hidden";
    $('.bg').fadeOut(800);
    $('#showImg').fadeOut(800);
    $("#closePreview").fadeOut(800);
}

function showOne() {
    var left = ($(".total").width() - $("#showImg").width()) / 2;
    $("#showImg").css({ left: left, right: left });
    $("#closePreview").css({ right: left - 32 });
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在生成预览" });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'PreviewFutureTenDaysWord'),
        params: { fileName: previewFile },
        success: function (response) {
            if (response.responseText != "") {
                myMask.hide();
                var imgs = response.responseText.split(",");
                $("#showImg").children('img').remove();
                for (var i = 0; i < imgs.length; i++) {
                    $("#showImg").append("<img class='previewImg' src='" + imgs[i] + "'/>");
                }
                $("#wordPreview").attr("src", response.responseText);
                $('.bg').fadeIn(200);
                $('#showImg').fadeIn(400);
                $('#closePreview').fadeIn(400);
            }
            else {
                alert("暂无预览数据");
                myMask.hide();
            }

        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}

function changeDate(el) {
    var value = el.value;
    var nowDate = convertDate(value);
    var content = $("#hazeContentDate").html();
    var pubDateTime = $("#txtHidePubDateTime").text();
    pubDateTime = pubDateTime.replace("{TodayDate}{Time}", value);
    pubDateTime = pubDateTime.replace(" ", "");
    $("#hazeContentDate").html(pubDateTime + "发布");
}
