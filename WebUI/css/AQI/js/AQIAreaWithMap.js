//存储暂存上传ftp的word文件名
var wordFileName = "";
var win;
var wrfChemWin;
var nmcWin;
var MapPannel;
var userName = "";
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
        if (e.target.className != "dateSelect" && e.target.className != "firstPolUl" && e.target.className != "firstPolText" && e.target.className != "selIcon" && e.target.className != "dateDiv") {
            $(".firstPolUl").hide();
        }
        if (e.target.className != "hazeLevelSelect" && e.target.className != "hazeDiv" && e.target.className != "hazeLevelText" && e.target.className != "selIcon" && e.target.className != "hazeLevelUl") {
            $(".hazeLevelUl").hide();
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
    //        $("#mapControl").css({ marginTop: (pageWidth - 605 - 60 - $("#districtArea").height()) / 2 });
    //    if (document.documentElement.clientHeight - 80 - 691 > 0) {
    //        $("#mapControl").css({ marginTop: (pageHeight - 80 - 691) / 2 });
    //    }
    //    else {
    //        $("#mapControl").css({ marginTop: (pageWidth - 605 - 60 - $("#districtArea").height()) / 2 });
    //    }

    //        var forecastDate = getNowDate();
    //        Ext.Ajax.request({
    //            url: getUrl('MMShareBLL.DAL.AQIForecast', 'JudgeState'),
    //            params: { functionName: "AQIArea", forecastDate: forecastDate },
    //            success: function (response) {
    //                if (response.responseText != "") {
    //                    InitialContent();
    //                }
    //                else {
    //                    AutoGet();
    //                }
    //            },
    //            failure: function (response) {
    //                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
    //            }
    //        });

    var productState = "undone";
    //读取状态表，设置底部按键的文字和功能
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetProductState'),
        params: { functionName: "AQIArea", hourType: "16:30" },
        success: function (response) {
            if (response.responseText != "") {
                productState = response.responseText;
                if (response.responseText == "undone") {
                    AutoGet();
                }
                else if (response.responseText == "saved") {
                    $("#foreCheck").show();
                    InitialContent();
                    //LoadSavedAQIAreaText();
                }
                else if (response.responseText == "checked") {
                    $("#foreCheck").show();
                    $("#forePub").show();
                    InitialContent();
                    //LoadSavedAQIAreaText();
                }
                else if (response.responseText == "published") {
                    $("#foreCheck").show();
                    $("#forePub").show();
                    InitialContent();
                    //LoadSavedAQIAreaText();
                }
                else if (response.responseText == "less") {
                    $("#foreCheck").show();
                    $("#forePub").show();
                    InitialContent();
                    //LoadSavedAQIAreaText();
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    //    $($(".firstPolUl")[i]).focus();
    //    $($(".firstPolUl")[i]).blur(function () {
    //        alert("blur");
    //        $($(".firstPolUl")[i]).hide();
    //    });

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


    $.each($(".dateSelect .firstPolUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".firstPolText")[i]).html($(m).html());
                $(n).addClass("hide");
                $(n).removeClass("display");
                //中心城区联动预报文本更新
                //                if ($(n).parent().id = "58367_FirstPol") {
                if (n.id == "firstPolUl_58367") {
                    //                    var firstItem = $($("#58367_Item").find("div")[0]).text();
                    var firstItem = $("#58367_Item").text();
                    var aqiValue = $("#58367_AQI").val();
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevelAndReplaceNew'),
                        params: { firstItem: $(m).text(), aqi: aqiValue },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = n.id.split('_')[0];
                                $("#" + siteId + "_Level").val(response.responseText.split('&')[0]);
                                var polLevelIndex = "1";
                                switch (response.responseText.split('&')[0]) {
                                    case "优":
                                        polLevelIndex = "1";
                                        break;
                                    case "良":
                                        polLevelIndex = "2";
                                        break;
                                    case "轻度污染":
                                        polLevelIndex = "3";
                                        break;
                                    case "中度污染":
                                        polLevelIndex = "4";
                                        break;
                                    case "重度污染":
                                        polLevelIndex = "5";
                                        break;
                                    case "严重污染":
                                        polLevelIndex = "6";
                                        break;
                                    default:
                                        polLevelIndex = "1";
                                        break;
                                }
                                $($(".districtArea div")[i]).removeClass();
                                $($(".districtArea div")[i]).addClass($(".districtArea div")[i].id + " " + $(".districtArea div")[i].id + "_" + polLevelIndex);

                                $($(".aqiInput")[i]).next("div").removeClass();
                                $($(".aqiInput")[i]).next("div").addClass("levelColor " + "levelColor_" + polLevelIndex);

                                $("#dataFileContent").val(response.responseText.split('&')[1]);
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }

            })
        })
    });

    //绑定霾级别选择下拉菜单的事件
    $.each($(".hazeDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".hazeLevelUl")[i]).is(":hidden")) {
                $($(".hazeLevelUl")[i]).show();
                $($(".hazeLevelUl")[i]).addClass("display");
                $($(".hazeLevelUl")[i]).removeClass("hide");
            }
            else {
                $($(".hazeLevelUl")[i]).hide();
                $($(".hazeLevelUl")[i]).addClass("hide");
                $($(".hazeLevelUl")[i]).removeClass("display");
            }
        });
    });

    $.each($(".hazeLevelSelect .hazeLevelUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".hazeLevelText")[i]).html($(m).html());
                $(n).addClass("hide");
                $(n).removeClass("display");
            })
        })
    });

    if (!win) {//如果不存在win对象择创建
        win = new Ext.Window({
            title: 'AQI分区预报预览',
            width: 700,
            height: 550,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            bodyStyle: "color:Red",
            items: new Ext.TabPanel({//窗体中中是一个一个TabPanel
                autoTabs: true,
                activeTab: 0,
                deferredRender: false,
                border: false,
                buttonAlign: "center",
                items: [
                        {
                            id: "tabTxt",
                            title: '指导预报',
                            html: "<div id='areaTable'>分区表格</div>"
                        },
                        {
                            id: "tabPic",
                            title: '数据文件',
                            html: '<div class="textPrewDiv"><textarea id="dateTextTab" class="textPreview">' + $("#dataFileContent").text() + '</textarea></div>'
                        }
                    ]
            })
            //            buttons: [
            //                    {
            //                        text: '关闭',
            //                        handler: function () {//点击时触发的事件
            //                            win.hide();
            //                        }
            //                    }
            //                    ]
        });
    }


    if (!wrfChemWin) {//如果不存在win对象择创建
        wrfChemWin = new Ext.Window({
            title: 'WRF-chem 20时-20时预报产品',
            width: 500,
            height: 120,
            x: 13,
            y: 46,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            bodyStyle: "color:Red",
            modal: false,
            items: new Ext.Panel({//窗体中中是一个一个TabPanel
                autoTabs: true,
                activeTab: 0,
                deferredRender: false,
                border: false,
                buttonAlign: "center",
                items: [
                        {
                            id: "tabPic",
                            html: '<div class="textPrewDiv"><textarea id="WRFChemText" class="textPreview_WFRChem">' + '</textarea></div>'
                        }
                    ]
            })
        });
    }

    if (!nmcWin) {//如果不存在win对象择创建
        nmcWin = new Ext.Window({
            title: '国家局预报产品',
            width: 550,
            height: 120,
            x: 520,
            y: 46,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            bodyStyle: "color:Red",
            modal: false,
            items: new Ext.Panel({//窗体中中是一个一个TabPanel
                autoTabs: true,
                activeTab: 0,
                deferredRender: false,
                border: false,
                buttonAlign: "center",
                items: [
                        {
                            html: '<div class="textPrewDiv"><textarea id="nmcText" class="textPreview_WFRChem">' + '</textarea></div>'
                        }
                    ]
            })
        });
    }

    //生成预报文件
    readReportTxt();
    $("#dataFileContent").niceScroll({
        cursorcolor: "#667A6D",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        background: "#EDEDED"
    });



    //自动获取预报文本
    $("#autoGetTxt").click(function () {
        readReportTxt();
    });

    //WRF-chem 20时-20时预报产品
    $("#WRFChem").click(function () {
        if (wrfChemWin) {
            readWRFChem24Txt();  
            wrfChemWin.show();          
        }
    });
    $("#WRFChem").click();

    //WRF-chem 20时-20时预报产品
    $("#nmcContent").click(function () {
        if (nmcWin) {
            readNMCTxt(); 
            nmcWin.show();           
        }
    });

    $("#nmcContent").click();
    //获取历史按键
    $("#getHistory").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取历史数据..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadAQIAreaHistory'),
            success: function (response) {
                myMask.hide();
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
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //暂存按键
    $("#tempSave").click(function () {
        var cells = "";
        var areaId;
        var polLevel;
        var firstPol;
        var aqiValue;
        var hazeLevel;
        var areaCount = $(".aqiLevelTd").length;
        for (var i = 0; i < areaCount; i++) {
            areaId = $(".aqiLevelInput")[i].id.split("_")[0];
            polLevel = $($(".aqiLevelInput")[i]).val();
            firstPol = $($(".firstPolText")[i]).text();
            aqiValue = $($(".aqiValue")[i]).val();
            hazeLevel = $($(".hazeLevelText")[i]).text();
            if (i < areaCount - 1) {
                cell = areaId + "_" + polLevel + "_" + firstPol + "_" + aqiValue + "_" + hazeLevel + "&";
            }
            else {
                cell = areaId + "_" + polLevel + "_" + firstPol + "_" + aqiValue + "_" + hazeLevel;
            }
            cells += cell;
        }

        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIAreaData'),
            params: { data: cells, period: "24", duratonId: "7" },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("保存成功！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //自动获取
    $("#autoCreate").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'QueryAreaAQI'),
            success: function (response) {

                if (response.responseText != "") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    try {
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
                    catch (e) {

                    }
                    finally {

                    }
                }

            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);

            }
        });
    });



    //保存入库按键
    //    $("#foreSave").click(function () {
    //        if (productState == "checked" || productState == "published" || productState == "less") {
    //            //文件发布的ftp地址内容
    //            var ftpString = $("#FtpCollection").val();
    //            var wordModelName = $("#wordModelName").val();
    //            var fileDate = getFormatDate('');
    //            var publishContent = $("#dataFileContent").val();
    //            var cellsContent = "";
    //            var wrLevelContent = "";
    //            var firstPolContent = "";
    //            var aqiContent = "";
    //            var hazeLevelContent = "";
    //            //存储预报日期和发布日期
    //            var twoDateContent = "";

    //            var todayDate = getFormatDate("");
    //            var tomDate = getFormatDate("tomorrow");

    //            var areaCount = $(".aqiLevelInput").length;
    //            for (var i = 0; i < areaCount; i++) {
    //                if (i < areaCount - 1) {
    //                    hazeLevelContent += "HazeLevel" + (parseInt(i) + 1).toString() + ":" + $($(".hazeLevelText")[i]).text() + ",";
    //                    wrLevelContent += "WRLevel" + (parseInt(i) + 1).toString() + ":" + $($(".aqiLevelInput")[i]).val() + ",";
    //                    firstPolContent += "PP" + (parseInt(i) + 1).toString() + ":" + $($(".firstPolText")[i]).text() + ",";
    //                    aqiContent += "AQI" + (parseInt(i) + 1).toString() + ":" + $($(".aqiValue")[i]).val() + ",";
    //                }
    //                else {
    //                    hazeLevelContent += "HazeLevel" + (parseInt(i) + 1).toString() + ":" + $($(".hazeLevelText")[i]).text();
    //                    wrLevelContent += "WRLevel" + (parseInt(i) + 1).toString() + ":" + $($(".aqiLevelInput")[i]).val();
    //                    firstPolContent += "PP" + (parseInt(i) + 1).toString() + ":" + $($(".firstPolText")[i]).text();
    //                    aqiContent += "AQI" + (parseInt(i) + 1).toString() + ":" + $($(".aqiValue")[i]).val();
    //                }
    //            }
    //            twoDateContent = "ForecastDate:" + tomDate + ",PubDate:" + todayDate;
    //            cellsContent += hazeLevelContent + "&" + wrLevelContent + "&" + firstPolContent + "&" + aqiContent + "&" + twoDateContent;

    //            var todayDate = getFormatDate("");
    //            var tomDate = getFormatDate("tomorrow");

    //            Ext.Ajax.request({
    //                url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadAQIAreaProductToFTP'),
    //                params: { ftpString: ftpString, fileDate: fileDate, wordModelName: wordModelName, functionName: "AQIArea", txtContent: publishContent, cellsContent: cellsContent, userName: userName },
    //                success: function (response) {
    //                    if (response.responseText == "success") {
    //                        $("#foreSave").text("发布");
    //                        productState = "published";
    //                        alert("发布成功！");
    //                    }
    //                    else if (response.responseText == "less") {
    //                        productState = "less"
    //                        alert("发布不完全");
    //                    }
    //                    else {
    //                        alert("发布失败");
    //                    }
    //                },
    //                failure: function (response) {
    //                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
    //                }
    //            });
    //        }
    //        //改为审核状态
    //        else if (productState == "saved") {
    //            Ext.Ajax.request({
    //                url: getUrl('MMShareBLL.DAL.AQIForecast', 'SetChecked'),
    //                params: { functionName: "AQIArea", hourType: "16:30" },
    //                success: function (response) {
    //                    if (response.responseText == "success") {
    //                        productState = "checked";
    //                        alert("审核成功");
    //                        $("#foreSave").text("发布");
    //                    }
    //                    else {
    //                        alert("审核失败！");
    //                    }
    //                },
    //                failure: function (response) {
    //                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
    //                }
    //            });

    //        }
    //        else {
    //            var cells = "";
    //            var areaId;
    //            var polLevel;
    //            var firstPol;
    //            var aqiValue = 0;
    //            var hazeLevel;
    //            var areaCount = $(".aqiLevelTd").length;
    //            for (var i = 0; i < areaCount; i++) {
    //                areaId = $(".aqiLevelInput")[i].id.split("_")[0];
    //                polLevel = $($(".aqiLevelInput")[i]).val();
    //                firstPol = $($(".dateDiv")[i]).text();
    //                aqiValue = $($(".aqiValue")[i]).val();
    //                hazeLevel = $($(".hazeDiv")[i]).text();
    //                if (i < areaCount - 1) {
    //                    cell = areaId + "_" + polLevel + "_" + firstPol + "_" + aqiValue + "_" + hazeLevel + "&";
    //                }
    //                else {
    //                    cell = areaId + "_" + polLevel + "_" + firstPol + "_" + aqiValue + "_" + hazeLevel;
    //                }
    //                cells += cell;
    //            }

    //            Ext.Ajax.request({
    //                url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIAreaData'),
    //                params: { data: cells, period: "24", duratonId: "7" },
    //                success: function (response) {
    //                    if (response.responseText == "success") {
    //                        productState = "saved";
    //                        $("#foreSave").text("审核");
    //                        alert("保存成功！");
    //                        $("#forePublish").show();
    //                    }
    //                    else if (response.responseText == "published") {
    //                        alert("数据已保存！");
    //                        $("#foreSave").text("审核");
    //                        $("#forePublish").show();
    //                    }
    //                    else {
    //                        alert("保存失败！");
    //                    }
    //                },
    //                failure: function (response) {
    //                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
    //                }
    //            });
    //        }
    //    });


    //预览按钮
    $("#forePreview").click(function () {
        if (win) {
            win.show();
            Ext.get("areaTable").update(CreatePreviewTable());

            Ext.get("dateTextTab").update($("#dataFileContent").val());

        }
    });

    //保存按钮
    $("#foreSave").click(function () {
        var cells = "";
        var areaId;
        var polLevel;
        var firstPol;
        var aqiValue = 0;
        var hazeLevel;
        var areaCount = $(".aqiLevelTd").length;
        for (var i = 0; i < areaCount; i++) {
            areaId = $(".aqiLevelInput")[i].id.split("_")[0];
            polLevel = $($(".aqiLevelInput")[i]).val();
            firstPol = $($(".dateDiv")[i]).text();
            aqiValue = $($(".aqiValue")[i]).val();
            hazeLevel = $($(".hazeDiv")[i]).text();
            if (i < areaCount - 1) {
                cell = areaId + "_" + polLevel + "_" + firstPol + "_" + aqiValue + "_" + hazeLevel + "&";
            }
            else {
                cell = areaId + "_" + polLevel + "_" + firstPol + "_" + aqiValue + "_" + hazeLevel;
            }
            cells += cell;
        }
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIAreaDataAndText'),
            params: { data: cells, period: "24", duratonId: "7", fileContent: $("#dataFileContent").val() },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("保存成功！");
                    $("#foreCheck").show();
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
                    //$($oldInput).addClass("backgroundColor_orange");
                    if (n.id == "58367_AQI") {
                        //                    var firstItem = $($("#58367_Item").find("div")[0]).text();
                        var firstItem = $("#58367_Item").text();
                        Ext.Ajax.request({
                            url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevelAndReplaceNew'),
                            params: { firstItem: firstItem, aqi: newAQIValue },
                            success: function (response) {
                                if (response.responseText != "") {
                                    var siteId = n.id.split('_')[0];
                                    $("#" + siteId + "_Level").val(response.responseText.split('&')[0]);
                                    var polLevelIndex = "1";
                                    switch (response.responseText) {
                                        case "优":
                                            polLevelIndex = "1";
                                            break;
                                        case "良":
                                            polLevelIndex = "2";
                                            break;
                                        case "轻度污染":
                                            polLevelIndex = "3";
                                            break;
                                        case "中度污染":
                                            polLevelIndex = "4";
                                            break;
                                        case "重度污染":
                                            polLevelIndex = "5";
                                            break;
                                        case "严重污染":
                                            polLevelIndex = "6";
                                            break;
                                        default:
                                            polLevelIndex = "1";
                                            break;
                                    }
                                    $($(".districtArea div")[i]).removeClass();
                                    $($(".districtArea div")[i]).addClass($(".districtArea div")[i].id + " " + $(".districtArea div")[i].id + "_" + polLevelIndex);

                                    $($(".aqiInput")[i]).next("div").removeClass();
                                    $($(".aqiInput")[i]).next("div").addClass("levelColor " + "levelColor_" + polLevelIndex);

                                    $("#dataFileContent").val(response.responseText.split('&')[1]);
                                }
                            },
                            failure: function (response) {
                                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            }
                        });
                    }
                    //                    else {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevel'),
                        params: { aqiValue: newAQIValue },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = n.id.split('_')[0];
                                $("#" + siteId + "_Level").val(response.responseText);
                                var polLevelIndex = "1";
                                switch (response.responseText) {
                                    case "优":
                                        polLevelIndex = "1";
                                        break;
                                    case "良":
                                        polLevelIndex = "2";
                                        break;
                                    case "轻度污染":
                                        polLevelIndex = "3";
                                        break;
                                    case "中度污染":
                                        polLevelIndex = "4";
                                        break;
                                    case "重度污染":
                                        polLevelIndex = "5";
                                        break;
                                    case "严重污染":
                                        polLevelIndex = "6";
                                        break;
                                    default:
                                        polLevelIndex = "1";
                                        break;
                                }
                                $($(".districtArea div")[i]).removeClass();
                                $($(".districtArea div")[i]).addClass($(".districtArea div")[i].id + " " + $(".districtArea div")[i].id + "_" + polLevelIndex);

                                $($(".aqiInput")[i]).next("div").removeClass();
                                $($(".aqiInput")[i]).next("div").addClass("levelColor " + "levelColor_" + polLevelIndex);
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                    //                    }
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

//生成预报的txt文件
function readReportTxt() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetShanghaiReportContentNew'),
        //params: { siteID: "58367",maxDate:"" },
        success: function (response) {
            if (response.responseText != "") {
                $("#dataFileContent").val(response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

//WRF-chem 20时-20时预报产品
function readWRFChem24Txt() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'RreadWRFchem20Data'),
        //params: { siteID: "58367",maxDate:"" },
        success: function (response) {
            if (response.responseText != "") {
                $("#WRFChemText").val(response.responseText);
                
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

//国家局预报产品
function readNMCTxt() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'ReadNMCData'),
        //params: { siteID: "58367",maxDate:"" },
        success: function (response) {
            if (response.responseText != "") {
                $("#nmcText").val(response.responseText);
                
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
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
//    //读取原始库
    //    if (result == "fail") {
//    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
//    myMask.show();
//        //读取AQI数据
//        Ext.Ajax.request({
//            url: getUrl('MMShareBLL.DAL.AQIForecast', 'QueryAreaAQI'),
//            params: { forecastDate: forecastDate },
//            success: function (response) {

//                if (response.responseText != "") {
//                    var result = Ext.util.JSON.decode(response.responseText);
//                    try {

//                        for (var aqi in result["AQI"]) {
//                            $("#" + aqi).val(result["AQI"][aqi]);
//                        }
//                        for (var firstPol in result["FirstPol"]) {
//                            $("#" + firstPol).html(result["FirstPol"][firstPol]);
//                        }
//                        for (var siteColor in result["LevelColor"]) {
//                            //单元格右侧的颜色条
//                            $("#" + siteColor + "_ColorNo").removeClass();
//                            $("#" + siteColor + "_ColorNo").addClass("levelColor levelColor_" + result["LevelColor"][siteColor]);
//                            //地图界面颜色联动
//                            var areaPinyin = GetAreaPinying(siteColor);
//                            $("#" + areaPinyin).removeClass();
//                            $("#" + areaPinyin).addClass(areaPinyin + " " + areaPinyin + "_" + result["LevelColor"][siteColor]);
//                        }
//                        for (var polLevel in result["PolLevel"]) {
//                            $("#" + polLevel).val(result["PolLevel"][polLevel]);
//                        }

//                    }
//                    catch (e) {
//                        myMask.hide();
//                    }
//                    finally {
//                        myMask.hide();
//                    }
//                }

//            },
//            failure: function (response) {
//                myMask.hide();
//                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);

//            }
//        });
//    }
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
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'QueryAreaAQI'),
        params: { forecastDate: forecastDate },
        success: function (response) {

            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                try {

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