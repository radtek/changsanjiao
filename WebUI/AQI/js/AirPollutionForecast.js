//预览窗体
var win;
var reportText = "";
var userName = "";
//生成上传的图片名称
var uploadImgName = "";

Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    userName = result["Alias"];
    $("#forecastTime").html(getNowFormatDate());

    $("#forecastTimeLevel").html("20时");
    $("#mapDate").html(getFormatDate(""));
    $("#pubTime").html(getFormatDate(""));

    var curHour = new Date().getHours();
    if (curHour < 12) {
        $("#pubHour").html("08");
        $("#mapDate").html(getFormatDate(""));
        $("#forecastTimeLevel").html("08时");
    }
    else {
        var tomDate = GetDateStr('1');
        $("#pubHour").html("20");
        $("#mapDate").html(tomDate);
    }

    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#map").width($(window).width() - 563 - 40);
    $("body").css("min-width", $(window).width() + "px");

    $("#map").height(pageHeight - 50);
    $("#rightContent").height(pageHeight - 40);
    $("#textPart").height(pageHeight - 456 - 10);
    $("#aqiText").height(pageHeight - 456 - 40);
    $("#textContentArea").height(pageHeight - 456 - 50);

    $(".districtArea").css({ marginTop: (pageHeight - 50 - 616) / 2 });

    //$("#mapControl").css({ marginTop: (pageWidth - 605 - 20 - $("#districtArea").height()) / 2 });
    //    if (document.documentElement.clientHeight - 80 - 691 > 0) {
    //        $("#mapControl").css({ marginTop: (pageHeight - 80 - 691) / 2 });
    //    }
    //    else {
    //        $("#mapControl").css({ marginTop: (pageWidth - 605 - 60 - $("#districtArea").height()) / 2 });
    //    }

    //读取AQI数据
    //AutoGetData();

    var productState = "undone";
    var curHour = new Date().getHours();
    var funcName = "AirPollutionForecast_07";
    var pubHour = "07:00";
    if (curHour < 12) {
        funcName = "AirPollutionForecast_07";
        pubHour = "07:00";
    }
    else {
        funcName = "AirPollutionForecast_17";
        pubHour = "17:00";
    }
    //读取状态表，设置底部按键的文字和功能
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetProductState'),
        params: { functionName: funcName, hourType: pubHour },
        success: function (response) {
            if (response.responseText != "") {
                productState = response.responseText;
                if (response.responseText == "undone") {
                    AutoGetData();
                    View();
                }
                else if (response.responseText == "saved") {
                    GetHistory();
                    $("#authbutton").show();
                }
                else if (response.responseText == "checked") {
                    GetHistory();
                    $("#authbutton").show();
                    $("#pulishbutton").show();
                }
                else if (response.responseText == "published") {
                    GetHistory();
                    $("#authbutton").show();
                    $("#pulishbutton").show();
                }
                else if (response.responseText == "less") {
                    GetHistory();
                    $("#authbutton").show();
                    $("#pulishbutton").show();
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    $("#aqiText").niceScroll({
        cursorcolor: "#667A6D",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        background: "#EDEDED"
    });

    //绑定空气质量等级选择下拉菜单的事件
    $.each($(".aiqQuaDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".airQuaUl")[i]).is(":hidden")) {
                $($(".airQuaUl")[i]).addClass("display");
                $($(".airQuaUl")[i]).removeClass("hide");
            }
            else {
                $($(".airQuaUl")[i]).addClass("hide");
                $($(".airQuaUl")[i]).removeClass("display");
            }
        });
    });

    $.each($(".airQuaSelect .airQuaUl,"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".airQuaText")[i]).html($(m).html());
                $(n).addClass("hide");
                $(n).removeClass("display");
            })
        })
    });

    //绑定空气污染气象条件等级选择下拉菜单的事件
    $.each($(".airpolConDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".airpolConUl")[i]).is(":hidden")) {
                $($(".airpolConUl")[i]).addClass("display");
                $($(".airpolConUl")[i]).removeClass("hide");
            }
            else {
                $($(".airpolConUl")[i]).addClass("hide");
                $($(".airpolConUl")[i]).removeClass("display");
            }
        });
    });

    $.each($(".airpolConSelect .airpolConUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".airpolConText")[i]).html($(m).html().trim());
                var polLevelIndex = "1";
                switch ($(m).text().trim()) {
                    case "一级":
                        polLevelIndex = "1";
                        break;
                    case "二级":
                        polLevelIndex = "2";
                        break;
                    case "三级":
                        polLevelIndex = "3";
                        break;
                    case "四级":
                        polLevelIndex = "4";
                        break;
                    case "五级":
                        polLevelIndex = "5";
                        break;
                    case "六级":
                        polLevelIndex = "6";
                        break;
                    default:
                        polLevelIndex = "1";
                        break;
                }
                $($(".districtArea div[pos=pos]")[i]).removeClass();
                $($(".districtArea div[pos=pos]")[i]).addClass($(".districtArea div[pos=pos]")[i].id + " " + $(".districtArea div[pos=pos]")[i].id + "_" + polLevelIndex);

                $(n).addClass("hide");
                $(n).removeClass("display");
                View();
            })
        })
    });


    //ftp地址集合字符串
    var ftpString = $("#FtpCollection").val();
    //预览窗体初始化    
    var curHour = new Date().getHours();
    var releaseTime = "7";
    if (curHour < 12) {
        releaseTime = "7";
    }
    else {
        releaseTime = "17";
    }
    //获取图片
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetRegionAirPollutionImg'),
        params: { releaseTime: releaseTime },
        success: function (response) {
            if (response.responseText != "") {
                $("#airimageshow").attr("src", "../" + response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
    //GetAirPollutionRegionImg();

    //复制中心城区按键
    $("#btnCopyCenter").click(function () {
        var polValue = $(".PM25").get(0).value;
        for (var i = 1; i < $(".PM25").length; i++) {
            $(".PM25").get(i).value = polValue;
        }

        var aiqQuaValue = $($(".airQuaText")[0]).html();
        var aiqPolConValue = $($(".airpolConText")[0]).html();
        for (var i = 1; i < $(".airQuaText").length; i++) {
            $($(".airQuaText")[i]).html(aiqQuaValue);
            $($(".airpolConText")[i]).html(aiqPolConValue);
        }
        var firstPolLevel = $($(".airpolConText")[0]).text();
        var polLevelIndex = "1";
        switch (firstPolLevel) {
            case "一级":
                polLevelIndex = "1";
                break;
            case "二级":
                polLevelIndex = "2";
                break;
            case "三级":
                polLevelIndex = "3";
                break;
            case "四级":
                polLevelIndex = "4";
                break;
            case "五级":
                polLevelIndex = "5";
                break;
            case "六级":
                polLevelIndex = "6";
                break;
            default:
                polLevelIndex = "1";
                break;
        }
        for (var j = 1; j < $(".districtArea div").length - 3; j++) {
            $($(".districtArea div")[j]).removeClass();
            $($(".districtArea div")[j]).addClass($(".districtArea div")[j].id + " " + $(".districtArea div")[j].id + "_" + polLevelIndex);
        }
        View();
    });

    //自动获取按键
    $("#btnAutoGet").click(function () {
        AutoGetData();
    });


    //获取历史数据
    $("#btnLastOne").click(function () {
        GetHistory();
    });

    //保存按键
    $("#savebutton").click(function () {
        var areaCount = $(".PM25").length;
        var cells = "";
        for (var i = 0; i < areaCount; i++) {
            var cell;
            if (i < areaCount - 1) {
                cell = $(".PM25")[i].id.split('_')[0] + "_" + $($(".PM25")[i]).val() + "_" + $($(".airQuaText")[i]).text() + "_" + $($(".airpolConText")[i]).text()+"_"+ $($(".symbolConText")[i]).text().trim() + "&";
            }
            else {
                cell = $(".PM25")[i].id.split('_')[0] + "_" + $($(".PM25")[i]).val() + "_" + $($(".airQuaText")[i]).text() + "_" + $($(".airpolConText")[i]).text() + "_" + $($(".symbolConText")[i]).text().trim();
            }
            cells += cell;
        }

        var hourType = "07";
        var date = new Date();
        var hour = date.getHours();
        if (hour < 12) {
            hourType = "07";
        }
        else if (hour > 12) {
            hourType = "17";
        }
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAirPollutionForecastAndDrawMap'),
            params: { data: cells, strForecastDate: "", strPeriod: "24", hourType: hourType },
            success: function (response) {
                myMask.hide();
                if (response.responseText.indexOf("success") > -1) {
                    alert("保存成功！");
                    $("#authbutton").show();
                }
                else if (response.responseText == "published") {
                    alert("数据已保存！");
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
    });

    //审核按键
    $("#authbutton").click(function () {
        var date = new Date();
        var hour = date.getHours();
        var hourType = "07";
        if (hour < 12) {
            hourType = "07";
        }
        else if (hour > 12) {
            hourType = "17";
        }
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SetChecked'),
            params: { functionName: funcName, hourType: hourType },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("审核成功");
                    $("#pulishbutton").show();
                }
                else {
                    alert("审核失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //发布按键
    $("#pulishbutton").click(function () {
        var date = new Date();
        var hour = date.getHours();
        var hourType = "07";
        var counthour = '0';
        var strhour = '08时';
       if (hour > 12) {
            hourType = "17";
            counthour = '1';
            strhour = '20时';
        }

        var imgName = $("#airimageshow").attr("src");
        var fileDate = getFormatDate('');
        var todayDate = getFormatDate("");
        var tomDate = GetDateStr(counthour);
        var areaTemplete = $("#txtHide_AreaForecast").val(); //分区预报稿模板
        var areaForecastTxt = GetViewAreaForecastTxt();
        areaTemplete = areaTemplete.replace("{RegionName_Level}", areaForecastTxt);
        areaTemplete = areaTemplete.replace("{ForecastDate}", tomDate);
        areaTemplete = areaTemplete.replace("{PubDate}", todayDate);
        areaTemplete = areaTemplete.replace("{Hour}", strhour);
        reportText = areaTemplete;

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpLoadTxtAndImgFtpLatest'),
            //                params: { ftpString: ftpString, fileDate: fileDate, sourceFileName: imgName, functionName: "AirPollutionForecast", txtContent: reportText, userName: userName, hourType: hourType },
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "AirPollutionForecast", txtContent: reportText, userName: userName, hourType: hourType },
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    if (response.responseText == "success") {
                        productState = "published";
                        alert("发布成功");
                    }
                    else if (response.responseText == "less") {
                        productState = "less";
                        alert("发布不完全");
                    }
                    else {
                        alert("发布失败");
                    }
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    $.each($(".PM25"), function (i, n) {
        $(n).change(function () {
            var $oldInput = $(this);
            var oldAQIValue = $(n).val();
            oldAQIValue = $.trim(oldAQIValue);

            var currentInputName = $(n).attr("name");
            var newAQIValue = $(n).val();
            newAQIValue = $.trim(newAQIValue);
            //            if (currentInputName == "AQI") {
            if (newAQIValue == null || newAQIValue == "" || !isFloat(newAQIValue)) {
                alert("AQI指数输入有误！");
                if (oldAQIValue != null && oldAQIValue != "") {
                    $(n).val(oldAQIValue);
                }
            }
            else {
                var aqiLevel = GetLevelFromAQI(newAQIValue);
                $($(".airQuaText")[i]).text(aqiLevel);
                $($(".airpolConText")[i]).text(aqiLevel);

                var aqiLevelNum = GetLevelNumFromAQI(newAQIValue);
                $($(".districtArea div")[i]).removeClass();
                $($(".districtArea div")[i]).addClass($(".districtArea div")[i].id + " " + $(".districtArea div")[i].id + "_" + aqiLevelNum);
            }
            //            }
        });
    });

    bindSelect();
});

function GetAirPollutionRegionImg() {
    alert("获取图片");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast','GetRegionAirPollutionImg'),
        params: { releaseTime:"7" },
        success: function (response) {
            if (response.responseText != "") {
                $("#airimageshow").attr("src", "../" + response.responseText);                
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function View() {
    var todayDate = getFormatDate("");
   
    var areaTemplete = $("#txtHide_AreaForecast").val(); //分区预报稿模板
    var areaForecastTxt = GetViewAreaForecastTxt();
    areaTemplete = areaTemplete.replace("{RegionName_Level}", areaForecastTxt);
   
    areaTemplete = areaTemplete.replace("{PubDate}", todayDate);
    var curHour = new Date().getHours();
    var disHour = "08";
    var tomDate = getFormatDate("");
    if (curHour >= 12) {
        disHour = "20";
        tomDate = GetDateStr("1");
    }
    areaTemplete = areaTemplete.replace("{Hour}", disHour + "时");
    areaTemplete = areaTemplete.replace("{ForecastDate}", tomDate);
    reportText=areaTemplete;
    //areaTemplete = areaTemplete.replace("<br/>", "\n");
    $("#textContentArea").val(areaTemplete);
    //Ext.get("textArea").update(areaTemplete);
}

function GetViewAreaForecastTxt() {
    var areaInfo = "";
    var areaCount = $(".c_RegionName").length;
    for (var i = 0; i < areaCount; i++) {
        if ($($(".c_RegionName")[i]).text() == "中心城区" || $($(".c_RegionName")[i]).text() == "浦东新区") {
            //            areaInfo += $($(".c_RegionName")[i]).text() + " " + " " + " " + " " + $($(".airpolConText")[i]).text() + "\r\n";
            areaInfo += $($(".c_RegionName")[i]).text() + " " + " " + " " + " " + $($(".airpolConText")[i]).text() + "\n";
        }
        else {
            //            areaInfo += $($(".c_RegionName")[i]).text() + " " + " " + " " + " " + " " + " " + $($(".airpolConText")[i]).text() + "\r\n";
            areaInfo += $($(".c_RegionName")[i]).text() + " " + " " + " " + " " + " " + " " + $($(".airpolConText")[i]).text() + "\n";
        }
    }
        return areaInfo;
}

//根据AQI计算空气质量等级和污染条件气象等级
function GetLevelFromAQI(aqi) {
    var strAQLLevel="";
    if (aqi != "") {
        var aqiValue = parseInt(aqi);
        if (aqiValue > 0 && aqiValue <= 50)
        {
            strAQLLevel = "一级";
        }
        else if (aqiValue > 50 && aqiValue <= 100)
        {
            strAQLLevel = "二级";
        }
        else if (aqiValue > 100 && aqiValue <= 150)
        {
            strAQLLevel = "三级";
        }
        else if (aqiValue > 150 && aqiValue <= 200)
        {
            strAQLLevel = "四级";
        }
        else if (aqiValue > 200 && aqiValue <= 300)
        {
            strAQLLevel = "五级";
        }
        else if (aqiValue > 300)
        {
            strAQLLevel = "六级";
        }
    }
    return strAQLLevel;
}

//根据AQI计算空气质量等级和污染条件气象等级数字
function GetLevelNumFromAQI(aqi) {
    var strAQLLevel = "";
    if (aqi != "") {
        var aqiValue = parseInt(aqi);
        if (aqiValue > 0 && aqiValue <= 50) {
            strAQLLevel = "1";
        }
        else if (aqiValue > 50 && aqiValue <= 100) {
            strAQLLevel = "2";
        }
        else if (aqiValue > 100 && aqiValue <= 150) {
            strAQLLevel = "3";
        }
        else if (aqiValue > 150 && aqiValue <= 200) {
            strAQLLevel = "4";
        }
        else if (aqiValue > 200 && aqiValue <= 300) {
            strAQLLevel = "5";
        }
        else if (aqiValue > 300) {
            strAQLLevel = "6";
        }
    }
    return strAQLLevel;
}

function AutoGetData() {
    var forecastDate = getNowDate();

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetAirPollutionForecast'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    var singleAqiLevel = GetLevelFromAQI(result[obj]);
                    var singleAqiLevelNum = GetLevelNumFromAQI(result[obj]);
                    $("#" + obj + "_Value").val(result[obj]);
                    $("#" + obj + "_airQua").text(singleAqiLevel);
                    $("#" + obj + "_aqiPolCon").text(singleAqiLevel);

                    var areaName = GetAreaPinying(obj);

                    $("#" + areaName).removeClass();
                    $("#" + areaName).addClass(areaName + " " + areaName + "_" + singleAqiLevelNum);
                    //                    myMask.hide();
                }
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function GetHistory() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadAirPollutionForecastHistory'),
        success: function (response) {
            if (response.responseText != "") {
                myMask.hide();
                var result = Ext.util.JSON.decode(response.responseText);
                for (var pm25 in result["PM25"]) {
                    $("#" + pm25).val(result["PM25"][pm25]);
                }
                for (var airQua in result["AirQua"]) {
                    $("#" + airQua).text(result["AirQua"][airQua]);
                }
                for (var airPolLevel in result["AirPolLevel"]) {
                    $("#" + airPolLevel).text(result["AirPolLevel"][airPolLevel]);
                }
                var itemNum = 0;
                for (var airColor in result["Color"]) {
                    $("#" + airColor).text(result["Color"][airColor]);
                    //地图界面颜色联动
                    var areaPinyin = GetAreaPinying(airColor);
                    $("#" + areaPinyin).removeClass();
                    $("#" + areaPinyin).addClass(areaPinyin + " " + areaPinyin + "_" + result["Color"][airColor]);

                    //设置对应区域的地图符号    这里绑定图片符号（前期做的，因后期不需要就注释了。防止以后再加此功能，后台及数据库不需要变更）
                    //var symbol = airColor;
                    //var strSym = result["Sym"][symbol];
                    //$(".symbolConText").eq(itemNum).text(strSym);
                    //var className = getClassName(strSym);
                    //$("#" + areaPinyin).children().removeClass();
                    //$("#" + areaPinyin).children().addClass("symSize");
                    //$("#" + areaPinyin).children().addClass(className);
                    //itemNum++;
                }
                View();
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function bindSelect() {
    $.each($(".symbolConSelect .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".symbolConUl")[i]).is(":hidden")) {
                $($(".symbolConUl")[i]).addClass("display");
                $($(".symbolConUl")[i]).removeClass("hide");
            }
            else {
                $($(".symbolConUl")[i]).addClass("hide");
                $($(".symbolConUl")[i]).removeClass("display");
            }
        });
    });

    $.each($(".symbolConSelect .symbolConUl,"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".symbolConText")[i]).html($(m).children("div").text());
                $(n).addClass("hide");
                $(n).removeClass("display");
                var objEle = $($(".districtArea div[pos=pos]")[i]).children();
                var className = getClassName($(m).children("div").text().trim());
                objEle.removeClass();
                objEle.addClass("symSize");
                objEle.addClass(className);
            })
        })
    });
}

function getClassName(name) {
    var className = "";
    switch (name) {
        case "浮尘": className = "yangshaSym"; break;
        case "扬沙": className = "fuchenSym"; break;
        case "沙尘暴": className = "shachenSym"; break;
        case "霾": className = "maiSym"; break;
        case "光化学烟雾": className = "guanghuaxueSym"; break;
    }
    return className;
}