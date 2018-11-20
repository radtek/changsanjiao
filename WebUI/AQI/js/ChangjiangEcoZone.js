//存储暂存上传ftp的word文件名
var wordFileName = "";

var MapPannel;
var userName = "";
var interval = "024";
Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");
    $("#copyFirstPol").click(function () {
        copyFirstPol();
    });
    $("#copyHaze").click(function () {
        copyHaze();
    });

    document.body.onclick = function (e) {
        if (e != undefined) {
            if (e.target.className != "dateSelect" && e.target.className != "firstPolUl" && e.target.className != "firstPolText" && e.target.className != "selIcon" && e.target.className != "dateDiv") {
                $(".firstPolUl").hide();
            }
            if (e.target.className != "hazeLevelSelect" && e.target.className != "hazeDiv" && e.target.className != "hazeLevelText" && e.target.className != "selIcon" && e.target.className != "hazeLevelUl") {
                $(".hazeLevelUl").hide();
            }
        }
    }

    $.each($(".selIcon"), function (i, n) {
        $(n).click(function () {
            $.each($(".selIcon"), function (j, m) {
                if (m.id != n.id) {
                    $($(".hazeLevelUl")[j]).hide();
                    $($(".hazeLevelUl")[j]).removeClass("display");
                    $($(".hazeLevelUl")[j]).addClass("hide");

                    $($(".firstPolUl")[j]).hide();
                    $($(".firstPolUl")[j]).removeClass("display");
                    $($(".firstPolUl")[j]).addClass("hide");
                }
            });
        });
    });

    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#map").width($(window).width() - 563 - 40);
    $("body").css("min-width", $(window).width() + "px");
    $("#map").height(pageHeight - 50);
    $("#rightContent").height(pageHeight - 40);
    $("#textPart").height(pageHeight - 479);
    $("#aqiText").height(pageHeight - 469 - 50);

    $(".districtArea").css({ marginTop: (pageHeight - 50 - 616) / 2 });

    var productState = "undone";
    //读取状态表，设置底部按键的文字和功能

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ChangjiangEconomy', 'GetChangjiangThreeImg'),
        params: { forecastDate: "" },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                
                var i = 0;
                for (var obj in result) {
                    i = i + 1;

                    $("#tab" + i.toString()).text(obj.toString());
                    $("#img_" + (24 * i).toString()).attr("src", "../" + result[obj]);
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
    AutoGet();

    //绑定首要污染物选择下拉菜单的事件
    $.each($(".dateDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".firstPolUl")[i]).is(":hidden")) {
                $($(".firstPolUl")[i]).show();
                $($(".firstPolUl")[i]).addClass("display");
                $($(".firstPolUl")[i]).removeClass("hide");
                $($(".firstPolUl")[i]).focus();
                $($(".firstPolUl")[i]).blur(function () {
                    alert("blur");
                });
            }
            else {
                $($(".firstPolUl")[i]).hide();
                $($(".firstPolUl")[i]).addClass("hide");
                $($(".firstPolUl")[i]).removeClass("display");
            }
        });
    });

    $.each($(".tabItem"), function (i, n) {
        $(n).click(function () {
            $("#downLoad").hide();
            if (i == 0) {
                interval = "024";
            }
            else if (i == 1) {
                interval = "048";
            }
            else if (i == 2) {
                interval = "072";
            }
            $.each($(".changjingIMG"), function (j, m) {
                if (i != j) {
                    $($(".changjingIMG")[j]).hide();
                    $($(".tabItem")[j]).removeClass("selectedTab");
                }
                else {
                    $($(".changjingIMG")[j]).show();
                    $($(".tabItem")[j]).addClass("selectedTab");
                }
            });
            AutoGet();
        });
    });

    $.each($(".dateSelect .firstPolUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".firstPolText")[i]).html($(m).html());
                $(n).addClass("hide");
                $(n).removeClass("display");

                //与左侧地图上的标记联动
                var site = n.id.split('_')[1];
                var itemID = "";
                switch ($(m).text()) {
                    case "SO2":
                        itemID = "1";
                        break;
                    case "NO2":
                        itemID = "2";
                        break;
                    case "PM10":
                        itemID = "3";
                        break;
                    case "CO":
                        itemID = "4";
                        break;
                    case "O3":
                        itemID = "5";
                        break;
                    case "PM2.5":
                        itemID = "6";
                        break;
                }
                //                $("#" + site + "_Value").text($("#" + site + "_AQI").val() + "/" + itemID);
                $("#" + site + "_Value").text($("#" + site + "_AQI").val() + "/" + $(m).text());
            })
        })
    });

    //绑定霾级别选择下拉菜单的事件
    //    $.each($(".hazeDiv .selIcon"), function (i, n) {
    //        $(n).click(function () {
    //            if ($($(".hazeLevelUl")[i]).is(":hidden")) {
    //                $($(".hazeLevelUl")[i]).show();
    //                $($(".hazeLevelUl")[i]).addClass("display");
    //                $($(".hazeLevelUl")[i]).removeClass("hide");
    //            }
    //            else {
    //                $($(".hazeLevelUl")[i]).hide();
    //                $($(".hazeLevelUl")[i]).addClass("hide");
    //                $($(".hazeLevelUl")[i]).removeClass("display");
    //            }
    //        });
    //    });

    //    $.each($(".hazeLevelSelect .hazeLevelUl"), function (i, n) {
    //        $.each($(n).find("li"), function (j, m) {
    //            $(m).click(function () {
    //                $($(".hazeLevelText")[i]).html($(m).html());
    //                $(n).addClass("hide");
    //                $(n).removeClass("display");
    //            })
    //        })
    //    });

    //获取历史按键
    $("#getHistory").click(function () {
        LoadHistoryChangjiangData();
    });

    //暂存按键
    $("#tempSave").click(function () {
        var cells = "";
        var areaId;
        var polLevel;
        var firstPol;
        var aqiValue = 0;
        var hazeLevel;
        var areaCount = $(".firstPolText").length;
        for (var i = 0; i < areaCount; i++) {
            areaId = $(".firstPolText")[i].id.split("_")[0];
            firstPol = $($(".dateDiv")[i]).text();
            aqiValue = $($(".aqiValue")[i]).val();
            if (i < areaCount - 1) {
                cell = areaId + "_" + firstPol + "_" + aqiValue + "&";
            }
            else {
                cell = areaId + "_" + firstPol + "_" + aqiValue;
            }
            cells += cell;
        }
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.ChangjiangEconomy', 'SaveChangjiangData'),
            params: { data: cells, interval: interval },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("保存成功！");
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

    //自动获取
    $("#autoCreate").click(function () {
        AutoGet();
    });


    //保存按钮
    $("#foreSave").click(function () {
        var cells = "";
        var areaId;
        var polLevel;
        var firstPol;
        var aqiValue = 0;
        var hazeLevel;
        var areaCount = $(".firstPolText").length;
        for (var i = 0; i < areaCount; i++) {
            areaId = $(".firstPolText")[i].id.split("_")[0];
            //polLevel = $($(".aqiLevelInput")[i]).val();
            firstPol = $($(".dateDiv")[i]).text();
            aqiValue = $($(".aqiValue")[i]).val();
            //hazeLevel = $($(".hazeDiv")[i]).text();
            if (i < areaCount - 1) {
                cell = areaId + "_" + firstPol + "_" + aqiValue + "&";
            }
            else {
                cell = areaId + "_" + firstPol + "_" + aqiValue;
            }
            cells += cell;
        }

        //获取图片底图的路径
        var imgSrc = "";
        if (interval == "024") {
            if ($("#img_24").attr("src") != "../AQI/img/zw.png") {
                if ($("#img_24").attr("src").indexOf("?V") > 0) {
                    imgSrc = $("#img_24").attr("src").substring(0, $("#img_24").attr("src").indexOf("?V"));
                }
            }
        }
        if (interval == "048") {
            if ($("#img_48").attr("src") != "../AQI/img/zw.png") {
                if ($("#img_48").attr("src").indexOf("?V") > 0) {
                    imgSrc = $("#img_48").attr("src").substring(0, $("#img_48").attr("src").indexOf("?V"));
                }
            }
        }
        if (interval == "072") {
            if ($("#img_72").attr("src") != "../AQI/img/zw.png") {
                if ($("#img_72").attr("src").indexOf("?V") > 0) {
                    imgSrc = $("#img_72").attr("src").substring(0, $("#img_72").attr("src").indexOf("?V"));
                }
            }
        }

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.ChangjiangEconomy', 'SaveChangjiangDataAndPic'),
            params: { data: cells, interval: interval, imgUrl: imgSrc },
            success: function (response) {
                myMask.hide();
                if (response.responseText.indexOf("success") != -1) {
                    alert("保存成功！");
                    var imgUrl = response.responseText.split('&')[1];
                    if (imgUrl != "") {
                        $("#downLoad").show();
                        $("#downLoadImg").attr("href", "../ReportProduce/ChangjiangImgDownload.ashx?ImgPath=" + imgUrl);
                    }

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

    //审核按钮
    $("#foreCheck").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SetChecked'),
            params: { functionName: "AQIArea", hourType: "16:30" },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("审核成功");
                    $("#forePub").show();
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

    //发布按钮
    $("#forePub").click(function () {
        //文件发布的ftp地址内容
        var ftpString = $("#FtpCollection").val();
        var wordModelName = $("#wordModelName").val();
        var fileDate = getFormatDate('');
        var publishContent = $("#dataFileContent").val();
        var cellsContent = "";
        var wrLevelContent = "";
        var firstPolContent = "";
        var aqiContent = "";
        var hazeLevelContent = "";
        //存储预报日期和发布日期
        var twoDateContent = "";

        var todayDate = getFormatDate("");
        var tomDate = getFormatDate("tomorrow");

        var areaCount = $(".aqiLevelInput").length;
        for (var i = 0; i < areaCount; i++) {
            var firstPolText = $($(".firstPolText")[i]).text();
            if ($($(".aqiLevelInput")[i]).val() == "优") {
                firstPolText = "-";
            }
            if (firstPolText == "O3-8h" || firstPolText == "O3-1h") {
                firstPolText = "O3";
            }
            if (i < areaCount - 1) {
                hazeLevelContent += "HazeLevel" + (parseInt(i) + 1).toString() + ":" + $($(".hazeLevelText")[i]).text() + ",";
                wrLevelContent += "WRLevel" + (parseInt(i) + 1).toString() + ":" + $($(".aqiLevelInput")[i]).val() + ",";
                firstPolContent += "PP" + (parseInt(i) + 1).toString() + ":" + firstPolText + ",";
                aqiContent += "AQI" + (parseInt(i) + 1).toString() + ":" + $($(".aqiValue")[i]).val() + ",";
            }
            else {
                hazeLevelContent += "HazeLevel" + (parseInt(i) + 1).toString() + ":" + $($(".hazeLevelText")[i]).text();
                wrLevelContent += "WRLevel" + (parseInt(i) + 1).toString() + ":" + $($(".aqiLevelInput")[i]).val();
                firstPolContent += "PP" + (parseInt(i) + 1).toString() + ":" + firstPolText;
                aqiContent += "AQI" + (parseInt(i) + 1).toString() + ":" + $($(".aqiValue")[i]).val();
            }
            //            if (i < areaCount - 1) {
            //                hazeLevelContent += "HazeLevel" + (parseInt(i) + 1).toString() + ":" + $($(".hazeLevelText")[i]).text() + ",";
            //                wrLevelContent += "WRLevel" + (parseInt(i) + 1).toString() + ":" + $($(".aqiLevelInput")[i]).val() + ",";
            //                firstPolContent += "PP" + (parseInt(i) + 1).toString() + ":" + $($(".firstPolText")[i]).text() + ",";
            //                aqiContent += "AQI" + (parseInt(i) + 1).toString() + ":" + $($(".aqiValue")[i]).val() + ",";
            //            }
            //            else {
            //                hazeLevelContent += "HazeLevel" + (parseInt(i) + 1).toString() + ":" + $($(".hazeLevelText")[i]).text();
            //                wrLevelContent += "WRLevel" + (parseInt(i) + 1).toString() + ":" + $($(".aqiLevelInput")[i]).val();
            //                firstPolContent += "PP" + (parseInt(i) + 1).toString() + ":" + $($(".firstPolText")[i]).text();
            //                aqiContent += "AQI" + (parseInt(i) + 1).toString() + ":" + $($(".aqiValue")[i]).val();
            //            }
        }
        twoDateContent = "ForecastDate:" + tomDate + ",PubDate:" + todayDate;
        cellsContent += hazeLevelContent + "&" + wrLevelContent + "&" + firstPolContent + "&" + aqiContent + "&" + twoDateContent;

        var todayDate = getFormatDate("");
        var tomDate = getFormatDate("tomorrow");
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadAQIAreaProductToFTP'),
            params: { ftpString: ftpString, fileDate: fileDate, wordModelName: wordModelName, functionName: "AQIArea", txtContent: publishContent, cellsContent: cellsContent, userName: userName },
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

    $.each($(".aqiValue"), function (i, n) {
        $(n).change(function () {
            var $oldInput = $(this);
            var oldAQIValue = $(n).val();
            oldAQIValue = $.trim(oldAQIValue);

            var currentInputName = $(n).attr("name");
            var newAQIValue = $(n).val();
            newAQIValue = $.trim(newAQIValue);
            if (currentInputName == "AQI") {

                if (newAQIValue == null || newAQIValue == "" || !isFloat(newAQIValue)) {
                    alert("AQI指数输入有误！");

                    if (oldAQIValue != null && oldAQIValue != "") {
                        $(n).val(oldAQIValue);
                    }
                }
                else {
                    var site = n.id.split('_')[0];
                    var itemID = "";
                    switch ($("#" + site + "_Item").text()) {
                        case "SO2":
                            itemID = "1";
                            break;
                        case "NO2":
                            itemID = "2";
                            break;
                        case "PM10":
                            itemID = "3";
                            break;
                        case "CO":
                            itemID = "4";
                            break;
                        case "O3":
                            itemID = "5";
                            break;
                        case "PM2.5":
                            itemID = "6";
                            break;
                    }
                    $("#" + site + "_Value").text($("#" + site + "_AQI").val() + "/" + itemID);
                }
            }
        });
    });

});


//复制首要污染物
function copyFirstPol() {
//    var polValue = $(".firstPollutionSel").get(0).value
//    for (var i = 1; i < $(".firstPollutionSel").length; i++) {
//        $(".firstPollutionSel").get(i).value = polValue;
    //    }

    var polValue = $($(".firstPolText")[0]).html();
    for (var i = 1; i < $(".firstPolText").length; i++) {
        $($(".firstPolText")[i]).html(polValue);
        }
}

//复制霾
function copyHaze() {
//    var hazeValue = $(".hazeLevel").get(0).value
//    for (var i = 1; i < $(".hazeLevel").length; i++) {
//        $(".hazeLevel").get(i).value = hazeValue;
    //    }
    var hazeLevelValue = $($(".hazeLevelText")[0]).html();
    for (var i = 1; i < $(".hazeLevelText").length; i++) {
        $($(".hazeLevelText")[i]).html(hazeLevelValue);
    }
}

///检测输入的是否是数字
function isFloat(v) {
    if (v == "" || v == null) return false;
    var str = v + "";
    var f = parseFloat(str);
    if (isNaN(f)) {
        return false;
    }

    if (str != f.toString()) { return false; }

    return true;
}

//当点击输入的div后，显示输入的文本框
function showInput(evt, sender) {
    //var date = Ext.getDom("H00").value;
//    var eventSource = getEventSource(evt);
//    if (eventSource.tagName == "INPUT")
//        return;
//    alterationTextValue(sender);
}




//生成预览窗体内的分区预报表格内容
function CreatePreviewTable() {
    var todayDate = getFormatDate("");
    var tomDate = getFormatDate("tomorrow");
    var areaCount = $(".areaName").length;
    var tableTitle = "<div class='prevTitleContent'>" + tomDate + "上海市空气质量AQI分区预报</div><div class='prevTitleContent'>上海市城市环境气象中心" + todayDate + "14时" + "发布</div>";
    var tableHtml = tableTitle+"<table class='previewTable'><tr><th></th><th>污染等级</th><th>首要污染物</th><th>AQI指数</th><th>霾预报</th></tr>";
    if (areaCount > 0) {        
        var singleRow = "";
        for (var i = 0; i < areaCount; i++) {
            var firstPolText = $($(".firstPolText")[i]).text();
            if ($($(".aqiLevelInput")[i]).val() == "优") {
                firstPolText = "-";
            }
            if (firstPolText == "O3-8h" || firstPolText == "O3-1h") {
                firstPolText = "O3";
            }
            singleRow = "<tr>" + "<td class='areaNamePrev'>" + $($(".areaName")[i]).html() + "</td>" + "<td class='polLevelPrev'>" + $($(".aqiLevelInput")[i]).val() + "</td>" + "<td class='firstPolPrev'>" + firstPolText + "</td>" + "<td class='aqiPrev'>" + $($(".aqiValue")[i]).val() + "</td>" + "<td class='hazeLevelPrev'>" + $($(".hazeLevelText")[i]).html() + "</td>" + "</tr>";
           tableHtml += singleRow;
       }
   }
   tableHtml += "</table>";
   return tableHtml;
}


function GetAreaPinying(siteID) {
    if (siteID != "") {
       switch (siteID){
           case "58367":
               return "XuHui";
               break;
           case "58370":
               return "PuDong";
               break;
           case "58361":
               return "MinHang";
               break;
           case "58362":
               return "BaoShanArea";
               break;
           case "58462":
               return "SongJiang";
               break;
           case "58460":
               return "JinShan";
               break;
           case "58461":
               return "QingPu";
               break;
           case "58463":
               return "FengXian";
               break;
           case "58365":
               return "JiaDing";
               break;
           case "58366":
               return "ChongMing";
               break;
       }
    }
}

//页面初始化获取内容，先读库，没有的再取读原始库
function InitialContent() {
    var result = "";
    //获取历史
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadAQIAreaHistory'),
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var aqi in result["AQI"]) {
                    $("#" + aqi).val(result["AQI"][aqi]);
                }
                for (var firstPol in result["FirstPol"]) {
                    $("#" + firstPol).html(result["FirstPol"][firstPol]);
                }
                for (var siteColor in result["LevelColor"]) {
                    //单元格右侧的颜色条
                    $("#" + siteColor + "_ColorNo").removeClass();
                    $("#" + siteColor + "_ColorNo").addClass("levelColor levelColor_" + result["LevelColor"][siteColor]);
                    //地图界面颜色联动
                    var areaPinyin = GetAreaPinying(siteColor);
                    $("#" + areaPinyin).removeClass();
                    $("#" + areaPinyin).addClass(areaPinyin + " " + areaPinyin + "_" + result["LevelColor"][siteColor]);
                }
                for (var polLevel in result["PolLevel"]) {
                    $("#" + polLevel).val(result["PolLevel"][polLevel]);
                }
            }
        },
        failure: function (response) {
            result = "fail";
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}


function alterationTextValue(sender) {

    //对于firefox只能用innerHTML
    var lastValue = sender.innerHTML.trim();
    var splitIndex = lastValue.indexOf('/');
    if (splitIndex > 0)
        lastValue = lastValue.substr(0, splitIndex);
    sender.setAttribute("tag", lastValue);
    sender.setAttribute("AQI", sender.innerHTML);
    sender.innerHTML = "";
    sender.style.border = "none";
    var nowDate = new Date();
    //    var strDate = nowDate.format("Y年m月d日");
    //    var date = Ext.getDom("H00").value;
    var strDate = "2015年11月06日";
    var date = "2015年11月06日";
    if (strDate != date) {
        var txtInput = new Ext.form.NumberField({
            renderTo: sender.id,
            width: sender.offsetWidth - 25,
            //width: 40,
            value: lastValue,
            maxValue: 2000,
            readOnly: true,
            listeners: {
                blur: function () {
                    var parentNode = this.container.dom;
                    var itemID = parentNode.id.substr(parentNode.id.length - 1, 1);
                    //标识是否经过编辑
                    var divTag = parentNode.getAttribute("tag");
                    if (this.getValue() != divTag) {
                        hasEdit = true;
                        //根据当前输入的浓度值和污染物ID，返回带颜色标识的浓度和AQI组合
                        if (this.getValue() === "") {
                            parentNode.innerHTML = "";
                            //                            buildPreview(parentNode.id);
                        }
                        else {
                            var value = this.getValue().toFixed(1);
                            //                            Ext.Ajax.request({
                            //                                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                            //                                params: { value: value, itemID: itemID },
                            //                                success: function (response) {
                            //                                    if (response.responseText != "") {
                            //                                        parentNode.innerHTML = response.responseText;
                            //                                        buildPreview(parentNode.id);
                            //                                    }
                            //                                },
                            //                                failure: function (response) {
                            //                                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            //                                }
                            //                            });

                        }
                    } else {
                        parentNode.innerHTML = parentNode.getAttribute("AQI");
                        //buildPreview(parentNode.id);
                    }
                    var kuang = document.getElementById(sender.id);
                    kuang.style.border = "1px solid #C0C0C0";

                },
                change: function () {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevel'),
                        params: { aqiValue: this.getValue() },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = sender.id.split('_')[0];
                                $("#" + siteId + "_Level").text(response.responseText);
                                var kuang = document.getElementById(sender.id);
                                kuang.style.border = "1px solid #C0C0C0";
                                kuang.style.height = "20px";
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
            }
        });
        txtInput.focus();
        inputEditor = txtInput;

        txtInput.container.dom.firstChild.select();
        add = sender.id;
    }
    else {
        var txtInput = new Ext.form.NumberField({
            renderTo: sender.id,
            width: sender.offsetWidth - 25,
            //width: 40,
            value: lastValue,
            maxValue: 2000,
            readOnly: false,
            listeners: {
                blur: function () {
                    var parentNode = this.container.dom;
                    var itemID = parentNode.id.substr(parentNode.id.length - 1, 1);
                    //标识是否经过编辑
                    var divTag = parentNode.getAttribute("tag");
                    if (this.getValue() != divTag) {
                        hasEdit = true;
                        //根据当前输入的浓度值和污染物ID，返回带颜色标识的浓度和AQI组合
                        if (this.getValue() === "") {
                            parentNode.innerHTML = "";
                            buildPreview(parentNode.id);
                        }
                        else {
                            var value = this.getValue().toFixed(1);
                            //                            Ext.Ajax.request({
                            //                                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                            //                                params: { value: value, itemID: itemID },
                            //                                success: function (response) {
                            //                                    if (response.responseText != "") {
                            //                                        parentNode.innerHTML = response.responseText;
                            //                                        buildPreview(parentNode.id);
                            //                                    }
                            //                                },
                            //                                failure: function (response) {
                            //                                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            //                                }
                            //                            });
                        }
                    } else {
                        parentNode.innerHTML = parentNode.getAttribute("AQI");
                        //buildPreview(parentNode.id);
                    }
                    var kuang = document.getElementById(sender.id);
                    kuang.style.border = "1px solid #C0C0C0";
                    

                },
                change: function () {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevel'),
                        params: { aqiValue: this.getValue() },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = sender.id.split('_')[0];
                                $("#" + siteId + "_Level").text(response.responseText);
                                var kuang = document.getElementById(sender.id);
                                kuang.style.border = "1px solid #C0C0C0";
                                kuang.style.height = "20px";
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
            }
        });
        //txtInput.focus();
        inputEditor = txtInput;

        txtInput.container.dom.firstChild.select();
        add = sender.id;
    }

}

function AutoGet() {
    var forecastDate = getNowDate();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ChangjiangEconomy', 'QueryChangjiangAQIData'),
        params: { interval: interval },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                try {
                    var itemID = "";
                    for (var aqi in result["AQI"]) {
                        $("#" + aqi).val(result["AQI"][aqi]);
                        switch (result["FirstPol"][aqi.split('_')[0] + "_Item"]) {
                            case "SO2":
                                itemID = "1";
                                break;
                            case "NO2":
                                itemID = "2";
                                break;
                            case "PM10":
                                itemID = "3";
                                break;
                            case "CO":
                                itemID = "4";
                                break;
                            case "O3":
                                itemID = "5";
                                break;
                            case "PM2.5":
                                itemID = "6";
                                break;
                        }
                        //                        DrawSitePoint($("#img_24").width(), $("#img_24").height(), 1153, 794, aqi.split('_')[0], result["AQI"][aqi] + "/" + itemID);
                        DrawSitePoint($("#img_24").width(), $("#img_24").height(), 1153, 794, aqi.split('_')[0], result["AQI"][aqi] + "/" + result["FirstPol"][aqi.split('_')[0] + "_Item"]);
                    }
                    for (var firstPol in result["FirstPol"]) {
                        $("#" + firstPol).html(result["FirstPol"][firstPol]);
                    }
                    for (var siteColor in result["LevelColor"]) {
                        //单元格右侧的颜色条
                        $("#" + siteColor + "_ColorNo").removeClass();
                        $("#" + siteColor + "_ColorNo").addClass("levelColor levelColor_" + result["LevelColor"][siteColor]);
                        //地图界面颜色联动
                        var areaPinyin = GetAreaPinying(siteColor);
                        $("#" + areaPinyin).removeClass();
                        $("#" + areaPinyin).addClass(areaPinyin + " " + areaPinyin + "_" + result["LevelColor"][siteColor]);
                    }
                    for (var polLevel in result["PolLevel"]) {
                        $("#" + polLevel).val(result["PolLevel"][polLevel]);
                    }

                }
                catch (e) {

                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);

        }
    });
}

function LoadHistoryChangjiangData() {
    var forecastDate = getNowDate();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ChangjiangEconomy', 'QueryChangjiangAQIDataHistory'),
        params: { interval: interval },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                try {
                    var itemID = "";
                    for (var aqi in result["AQI"]) {
                        $("#" + aqi).val(result["AQI"][aqi]);
                        switch (result["FirstPol"][aqi.split('_')[0] + "_Item"]) {
                            case "SO2":
                                itemID = "1";
                                break;
                            case "NO2":
                                itemID = "2";
                                break;
                            case "PM10":
                                itemID = "3";
                                break;
                            case "CO":
                                itemID = "4";
                                break;
                            case "O3":
                                itemID = "5";
                                break;
                            case "PM2.5":
                                itemID = "6";
                                break;
                        }
                        DrawSitePoint($("#img_24").width(), $("#img_24").height(), 1153, 794, aqi.split('_')[0], result["AQI"][aqi] + "/" + itemID);
                    }
                    for (var firstPol in result["FirstPol"]) {
                        $("#" + firstPol).html(result["FirstPol"][firstPol]);
                    }
                    for (var siteColor in result["LevelColor"]) {
                        //单元格右侧的颜色条
                        $("#" + siteColor + "_ColorNo").removeClass();
                        $("#" + siteColor + "_ColorNo").addClass("levelColor levelColor_" + result["LevelColor"][siteColor]);
                        //地图界面颜色联动
                        var areaPinyin = GetAreaPinying(siteColor);
                        $("#" + areaPinyin).removeClass();
                        $("#" + areaPinyin).addClass(areaPinyin + " " + areaPinyin + "_" + result["LevelColor"][siteColor]);
                    }
                    for (var polLevel in result["PolLevel"]) {
                        $("#" + polLevel).val(result["PolLevel"][polLevel]);
                    }

                }
                catch (e) {

                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);

        }
    });
}

function LoadSavedAQIAreaText() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadSavedAQIAreaText'),
        success: function (response) {
            if (response.responseText != "") {
                $("#dataFileContent").val(response.responseText);
            }
        },
        failure: function (response) {
        }
    });
}

function DrawSitePoint(realWidth, realHeight, sourceWidth, sourceHeight,site,valueText) {
    var sourceX;
    var sourceY;
    switch (site) {
        case "58367A":
            sourceX = 988;
            sourceY = 333;
            break;
        case "58468A":
            sourceX = 984;
            sourceY = 378;
            break;
        case "58457A":
            sourceX = 949;
            sourceY = 374;
            break;
        case "58259A":
            sourceX = 979;
            sourceY = 297;

            break;
        case "58349A":
            sourceX = 957;
            sourceY = 332;
            break;
        case "58245A":
            sourceX = 917;
            sourceY = 289;
            break;
            //南京
        case "58238A":
            sourceX = 918;
            sourceY = 288;
            break;
        case "58334A":
            sourceX = 882;
            sourceY = 331;
            break;
        case "58321A":
            sourceX = 846;
            sourceY = 321;
            break
        case "58424A":
            sourceX = 840;
            sourceY = 368;
            break;
        case "58502A":
            sourceX = 802;
            sourceY = 403;
            break;
        case "57494A":
            sourceX = 734;
            sourceY = 363;
            break;
        case "57461A":
            sourceX = 633;
            sourceY = 354;
            break;
        case "57516A":
            sourceX = 455;
            sourceY = 377;
            break;
        case "56491A":
            sourceX = 380;
            sourceY = 401;
            break;
        case "56289A":
            sourceX = 366;
            sourceY = 321;
            break;
    }
    var useX=0;
    var useY = 0;
    useX = sourceX * (realWidth / sourceWidth);
    useY = sourceY * (realHeight / sourceHeight);
//    $("#" + site + "_Point").css("left", useX-4);
//    $("#" + site + "_Point").css("top", useY + 36);

    $("#" + site + "_Value").text(valueText);
    if (site == "58321A") {
        $("#" + site + "_Value").css("left", useX - 30);
        $("#" + site + "_Value").css("top", useY + 20);
    }
    else if (site == "58424A") {
        $("#" + site + "_Value").css("left", useX - 7);
        $("#" + site + "_Value").css("top", useY + 56);
    }
    else if (site == "58334A") {
        $("#" + site + "_Value").css("left", useX - 24);
        $("#" + site + "_Value").css("top", useY + 56);
    }
    else if (site == "58457A") {
        $("#" + site + "_Value").css("left", useX - 30);
        $("#" + site + "_Value").css("top", useY + 56);
    }
   
    else if (site == "58468A") {
        $("#" + site + "_Value").css("left", useX-1);
        $("#" + site + "_Value").css("top", useY + 56);
    }
    else if (site == "58245A") {
        $("#" + site + "_Value").css("left", useX - 24);
        $("#" + site + "_Value").css("top", useY + 16);
    }
    else if (site == "58259A") {
        $("#" + site + "_Value").css("left", useX);
        $("#" + site + "_Value").css("top", useY + 27);
    }
    else if (site == "58367A") {
        $("#" + site + "_Value").css("left", useX +10);
        $("#" + site + "_Value").css("top", useY + 33);
    }

    else {
        $("#" + site + "_Value").css("left", useX - 19);
        $("#" + site + "_Value").css("top", useY + 56);
    }
}

//当点击输入的div后，显示输入的文本框
function showInput(evt, sender) {
    //var date = Ext.getDom("H00").value;
    var nowDate = new Date();
    var hour = nowDate.getHours();
    //20点前可以修改
    if (hour < 20) {
        var eventSource = getEventSource(evt);
        if (eventSource.tagName == "INPUT")
            return;
        alterationTextValue(sender);
    }
    else {
        if (sender.className.toString().indexOf("limitEdit") > 0) {
            alert("数据无法编辑！")
            return;
        }
        else {
            var eventSource = getEventSource(evt);
            if (eventSource.tagName == "INPUT")
                return;
            alterationTextValue(sender);
        }
    }
}

function alterationTextValue(sender) {
    //对于firefox只能用innerHTML
    //    var cellValue = sender.innerHTML.trim();
    var cellValue = sender.innerHTML;
    sender.innerHTML = "";
    var txtInput = new Ext.form.Field({
        renderTo: sender.id,
        width: 60,
        value: cellValue,
        //maxValue: 2000,
        readOnly: false,
        listeners: {
            blur: function () {
                var parentNode = this.container.dom;
                if (this.getValue() === "") {
                    //                            parentNode.innerHTML = "";
                    parentNode.innerHTML = "";
                    //buildPreview(parentNode.id);
                }
                else {
                    //                        var value = this.getValue().toFixed(1);
                    //                        parentNode.innerHTML = value;
                    //var value = this.getValue().toFixed(1);
                    parentNode.innerHTML = this.getValue();
                    changeTableValue(sender.id, this.getValue());
                }
            }
        }
    });
    txtInput.focus();
    
    //        inputEditor = txtInput;

    //        txtInput.container.dom.firstChild.select();
    //        add = sender.id;   
}



//地图上的标注值改变与表格内数据的联动
function changeTableValue(divId,value) {
    if (divId != "" && value!="") {
        var site = divId.split('_')[0];
        var aqiValue = value.split('/')[0];
        var firstItem = "";
        switch (value.split('/')[1]) {
            case "1":
                firstItem = "SO2";
                break;
            case "2":
                firstItem = "NO2";
                break;
            case "3":
                firstItem = "PM10";
                break;
            case "4":
                firstItem = "CO";
                break;
            case "5":
                firstItem = "O3";
                break;
            case "6":
                firstItem = "PM2.5";
                break;
        }
        $("#" + site + "_Item").text(firstItem);
        $("#" + site + "_AQI").val(aqiValue);
    }
}