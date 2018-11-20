//存储暂存上传ftp的word文件名
var wordFileName = "";
var win;
var wrfChemWin;
var nmcWin;
var MapPannel;
var userName = "";
var oldPeriod = "p08";

window.onresize = function () {
    SetSize();
}

function SetSize() {
    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("body").css("min-width", $(window).width() + "px");
    //    $("#rightContent").height(pageHeight - 40);
    $("#textPart").height(pageHeight - 479);
    $("#aqiText").height(pageHeight - 469 - 50);
    $(".districtArea").css({ marginTop: (pageHeight - 50 - 616) / 2 });
    $.each($(".aqiAreaTable"), function (i, n) {
        $($(".aqiAreaTable")[i]).css({ width: (pageWidth - 545) });

    });
}

Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("20时");

    /*薛辉*/
    radioClickModule(oldPeriod);

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
    SetSize();

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

    /*薛辉*/
    //王斌  2017.5.8
    $.each($(".dateSelect .firstPolUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".firstPolText")[i]).html($(m).html());
                var firstItem = $.trim(m.innerText);

                var polLevelIndex = "1";
                switch (firstItem) {
                    case "五星":
                        polLevelIndex = "1";
                        break;
                    case "四星":
                        polLevelIndex = "2";
                        break;
                    case "三星":
                        polLevelIndex = "3";
                        break;
                    case "二星":
                        polLevelIndex = "4";
                        break;
                }

                var c = $($(".aqiInputs")[i]);
                c.removeClass();
                c.addClass("levelColor " + "levelColor_" + polLevelIndex + " aqiInputs");

                if (i > -1 && i <= 17) {
                    RefershMap("1");
                }
                if (i > 17 && i <= 35) {
                    RefershMap("2");
                }
                if (i > 35 && i <= 53) {
                    RefershMap("3");
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

    $("#dataFileContent").niceScroll({
        cursorcolor: "#667A6D",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        background: "#EDEDED"
    });

    //保存按钮
    $("#foreSave").click(function () {
        var vs = "";
        $.each($(".areaName"), function (i, n) {
            var text = n.innerText;
            var area = $.trim(text);
            vs += area + "@";
        });
        $.each($(".firstPolText"), function (i2, n2) {
            var text = n2.innerText;
            var firstItem = $.trim(text);
            vs += firstItem + "@";
        });
        $.each($(".aqiValue"), function (i3, n3) {
            var text = n3.value;
            var value = $.trim(text);
            vs += value + "@";
        });
        $.each($(".aqiValueII"), function (i4, n4) {
            var text = n4.value;
            var valueII = $.trim(text);
            vs += valueII + "@";
        });

        var user = Ext.getDom("forecaster").innerText;
        var forecastTime = Ext.getDom("forecastTime").innerText;
        var type = "综合观景指数";
        var priod;
        if (oldPeriod == "p08")
            priod = "08";
        else
            priod = "20";

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.IndexRelease', 'SaveIndexRelease'),
            params: { texts: vs, priod: priod, type: type, forecastTime: forecastTime, user: user },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "true") {
                    Ext.Msg.alert("提示", "保存成功!");
                }
                else {
                    Ext.Msg.alert("提示", "保存失败，请重试或者联系系统管理员!");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });
    //提示
    $('[data-toggle="tooltip"]').tooltip();
});

//王斌  2017.5.8
function GetFirstLevel(firstItem) {
    var polLevelIndex;
    switch (firstItem) {
        case "五星":
            polLevelIndex = "1";
            break;
        case "四星":
            polLevelIndex = "2";
            break;
        case "三星":
            polLevelIndex = "3";
            break;
        case "二星":
            polLevelIndex = "4";
            break;
    }
    return polLevelIndex;
}
/*薛辉*/
function RefershMap(index) {
    $.each($(".firstPolText"), function (i, n) {
        var x = 0;
        var text = n.innerText;
        var firstItem = $.trim(text);
        var polLevelIndexs = GetFirstLevel(firstItem);
        var j = i;
        if (index == "1") {
            if (i > 17) return true;
        }
        else if (index == "2") {
            if (i > 35) return true;
            else if (i >= 18) {
                j = i - (18 * 1);
            }

        }
        else if (index == "3") {
            if (i > 53) return true;
            else if (i > 35) {
                j = i - (18 * 2);
            }
            else if (i >= 18) {  //xuehui 05-02
                j = i - 18;
            }

        }


          //等图做好了
        if (index == 1 || index == 2 || index == 3) {
            $($(".mps div")[j]).removeClass();
            $($(".mps div")[j]).addClass($(".mps div")[j].id + " " + $(".mps div")[j].id + "_" + polLevelIndexs);
            var Start = "";
            if (polLevelIndexs == "4") { Start = "★★"; }
            if (polLevelIndexs == "3") { Start = "★★★"; }
            if (polLevelIndexs == "2") { Start = "★★★★"; }
            if (polLevelIndexs == "1") { Start = "★★★★★"; }

            var t = $($(".areaName")[i]).html();
            if ($($(".areaName")[i]).html() == "上海市青少年校外活动营地—东方绿舟")
                t = "上海青少年校外活动营地—东方绿舟";

            var title = (t + "<br>" + Start + "<br>" + $($(".aqiValue")[i]).val());
            $($(".mps div")[j]).attr("data-original-title", title); //修改title值  
            var c = $($(".aqiInputs")[i]);
            c.removeClass();
            c.addClass("levelColor " + "levelColor_" + polLevelIndexs + " aqiInputs");
        }

    });
}

/*刷新数据   -薛辉*/
function RefreshData() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询..." });
    myMask.show();
    refershChildForecast("1");
    myMask.hide();
}

function refershChildForecast(flag) {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p08")
        priod = "08";
    else
        priod = "20";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.IndexRelease', 'QueryIndexReleaseForecastII'),
        params: { priod: priod, type: "综合观景指数", forecastTime: forecastTime },
        success: function (response) {
            if (response.responseText != "") {
                var vs = response.responseText.split('*');
                //alert(vs);
                var dj = vs[0].split(',');
                var rq = vs[1].split(',');
                var cs = vs[2].split(',');
                var lst = vs[3].split(',');
                var sites = vs[4].split(',');
                var time = Ext.getDom("dd1er").innerText;
                var timeII = Ext.getDom("dd2er").innerText;
                var timeIII = Ext.getDom("dd3er").innerText;
                for (var i = 0; i < $(".firstPolText").length; i++) {

                    if (i <= 17) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == time && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = dj[(j)];
                                $($(".firstPolText")[i]).html(polValue);
                                setColor(i, polValue);
                                break;
                            }
                        }
                    }

                    if (i > 17 && i <= 35) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = dj[(j)];
                                $($(".firstPolText")[i]).html(polValue);
                                setColor(i, polValue);
                                break;
                            }
                        }
                    }

                    if (i > 35 && i <= 53) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeIII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = dj[(j)];
                                $($(".firstPolText")[i]).html(polValue);
                                setColor(i, polValue);
                                break;
                            }
                        }
                    }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                    if (i <= 17) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == time && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = rq[(j)];
                                $($(".aqiValue")[i]).val(polValue);
                                break;
                            }
                        }
                    }


                    if (i > 17 && i <= 35) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = rq[(j)];
                                $($(".aqiValue")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                    if (i > 35 && i <= 53) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeIII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = rq[(j)];
                                $($(".aqiValue")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                }

                for (var i = 0; i < $(".aqiValueII").length; i++) {

                    if (i <= 17) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == time && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = cs[(j)];
                                $($(".aqiValueII")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                    if (i > 17 && i <= 35) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = cs[(j)];
                                $($(".aqiValueII")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                    if (i > 35 && i <= 53) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeIII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = cs[(j)];
                                $($(".aqiValueII")[i]).val(polValue);
                                break;
                            }
                        }
                    }
                }
            }

            if (flag == "1") {
                refershChild();
            }
            else {
                if (Ext.getDom("dd1er").className == "singleHazeLevel singleHazeLevel_Selected") {
                    RefershMap("1");
                }

                if (Ext.getDom("dd2er").className == "singleHazeLevel singleHazeLevel_Selected") {
                    RefershMap("2");
                }

                if (Ext.getDom("dd3er").className == "singleHazeLevel singleHazeLevel_Selected") {
                    RefershMap("3");
                }
            }

        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });


}

function refershChild() {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p08")
        priod = "08";
    else
        priod = "20";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.IndexRelease', 'QueryIndexReleaseII'),
        params: { priod: priod, type: "综合观景指数", forecastTime: forecastTime },
        success: function (response) {
            if (response.responseText != "") {
                var vs = response.responseText.split('*');
                var dj = vs[0].split(',');
                var rq = vs[1].split(',');
                var cs = vs[2].split(',');
                var lst = vs[3].split(',');
                var sites = vs[4].split(',');
                var time = Ext.getDom("dd1er").innerText;
                var timeII = Ext.getDom("dd2er").innerText;
                var timeIII = Ext.getDom("dd3er").innerText;
                for (var i = 0; i < $(".firstPolText").length; i++) {
                    if (i <= 17) {
                        for (var j = 0; j < lst.length; j++) {
                            var v = $($(".c_RegionName")[i]).text();
                            if (lst[j] == time && sites[j] == v) {
                                var polValue = dj[(j)];
                                $($(".firstPolText")[i]).html(polValue);
                                setColor(i, polValue);
                                break;
                            }
                        }
                    }

                    if (i > 17 && i <= 35) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = dj[(j)];
                                $($(".firstPolText")[i]).html(polValue);
                                setColor(i, polValue);
                                break;
                            }
                        }
                    }

                    if (i > 35 && i <= 53) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeIII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = dj[(j)];
                                $($(".firstPolText")[i]).html(polValue);
                                setColor(i, polValue);
                                break;
                            }
                        }
                    }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                    if (i <= 17) {
                        for (var j = 0; j < lst.length; j++) {
                            var v = $($(".c_RegionName")[i]).text();
                            if (lst[j] == time && sites[j] == v) {
                                var polValue = rq[(j)];
                                $($(".aqiValue")[i]).val(polValue);
                                break;
                            }
                        }
                    }


                    if (i > 17 && i <= 35) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = rq[(j)];
                                $($(".aqiValue")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                    if (i > 35 && i <= 53) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeIII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = rq[(j)];
                                $($(".aqiValue")[i]).val(polValue);
                                break;
                            }
                        }
                    }
                }

                for (var i = 0; i < $(".aqiValueII").length; i++) {
                    if (i <= 17) {
                        for (var j = 0; j < lst.length; j++) {
                            var v = $($(".c_RegionName")[i]).text();
                            if (lst[j] == time && sites[j] == v) {
                                var polValue = cs[(j)];
                                $($(".aqiValueII")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                    if (i > 17 && i <= 35) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = cs[(j)];
                                $($(".aqiValueII")[i]).val(polValue);
                                break;
                            }
                        }
                    }

                    if (i > 35 && i <= 53) {
                        for (var j = 0; j < lst.length; j++) {
                            if (lst[j] == timeIII && sites[j] == $($(".c_RegionName")[i]).text()) {
                                var polValue = cs[(j)];
                                $($(".aqiValueII")[i]).val(polValue);
                                break;
                            }
                        }
                    }
                }
            }
            if (Ext.getDom("dd1er").className == "singleHazeLevel singleHazeLevel_Selected") {
                RefershMap("1");
            }
            if (Ext.getDom("dd2er").className == "singleHazeLevel singleHazeLevel_Selected") {
                RefershMap("2");
            }
            if (Ext.getDom("dd3er").className == "singleHazeLevel singleHazeLevel_Selected") {
                RefershMap("3");
            }
        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "儿童数据请求失败，错误代码为：" + response.status);
        }
    });
}

/*薛辉 自动刷新*/
function AutoLoad() {
    clear();
    refershChildForecast("0");
    if (Ext.getDom("dd1er").className == "singleHazeLevel singleHazeLevel_Selected") {
        RefershMap("1");
    }

    if (Ext.getDom("dd2er").className == "singleHazeLevel singleHazeLevel_Selected") {
        RefershMap("2");
    }

    if (Ext.getDom("dd3er").className == "singleHazeLevel singleHazeLevel_Selected") {
        RefershMap("3");
    }
}
//王斌  2017.5.8
function setColor(i, text) {
    var c = $($(".aqiInputs")[i]);
    c.removeClass();
    if (text == "五星" || text == "")
        c.addClass("levelColor " + "levelColor_1 aqiInputs");
    if (text == "四星")
        c.addClass("levelColor " + "levelColor_2 aqiInputs");
    if (text == "三星")
        c.addClass("levelColor " + "levelColor_3 aqiInputs");
    if (text == "二星")
        c.addClass("levelColor " + "levelColor_4 aqiInputs");
    //            if (text == "高")
    //                c.addClass("levelColor " + "levelColor_5 aqiInputs");
}

/*暂存-薛辉*/
function TempSave(type, t) {
    var vs = "";
    var type = "";
    var lst = Ext.getDom(t).innerText;
    $.each($(".areaName"), function (i, n) {
        if (i < 18) {
            var text = n.innerText;
            var area = $.trim(text);
            vs += area + "@";
        }
    });
    $.each($(".firstPolText"), function (i2, n2) {

        if (t == "dd1er") {
            if (i2 < 18) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
            }
        }
        else if (t == "dd2er") {
            if (i2 >= 18 && i2 <= 35) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
            }
        }
        else if (t == "dd3er") {
            if (i2 >= 36 && i2 <= 53) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
            }
        }

    });
    $.each($(".aqiValue"), function (i3, n3) {
        if (t == "dd1er") {
            if (i3 < 18) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
                type = "综合观景指数";
            }
        }
        else if (t == "dd2er") {
            if (i3 >= 18 && i3 <= 35) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
                type = "综合观景指数";
            }
        }
        else if (t == "dd3er") {
            if (i3 >= 36 && i3 <= 53) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
                type = "综合观景指数";
            }
        }
    });
    $.each($(".aqiValueII"), function (i4, n4) {
        if (t == "dd1er") {
            if (i4 < 18) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
            }
        }
        else if (t == "dd2er") {
            if (i4 >= 18 && i4 <= 35) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
            }
        }
        else if (t == "dd3er") {
            if (i4 >= 36 && i4 <= 53) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + "@";
            }
        }

    });

    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p08")
        priod = "08";
    else
        priod = "20";

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.IndexRelease', 'SaveTempIndexRelease'),
        params: { texts: vs, priod: priod, type: type, forecastTime: forecastTime, user: user, tableName: t },
        success: function (response) {
            myMask.hide();
            if (response.responseText == "true") {

                Ext.Msg.alert("提示", "保存成功!");
            }
            else {
                Ext.Msg.alert("提示", "保存失败，请重试或者联系系统管理员!");
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

/*复制首要污染物*/
function copyFirstPol(index) {
    var polValue = $($(".firstPolText")[0]).html();
    var aqiValue1 = $($(".aqiValue")[0]).val(); // xuehui 0901
    var aqiValue2 = $($(".aqiValueII")[0]).val();
    for (var i = 1; i < $(".firstPolText").length; i++) {
        if (index == "1") {
            if (i <= 17) {
                $($(".firstPolText")[i]).html(polValue);
                $($(".aqiValue")[i]).val(aqiValue1);
                $($(".aqiValueII")[i]).val(aqiValue2);
            }
        }
        else if (index == "2") {
            polValue = $($(".firstPolText")[18]).html();
            aqiValue1 = $($(".aqiValue")[18]).val();
            aqiValue2 = $($(".aqiValueII")[18]).val();
            if (i > 17 && i <= 35) {
                $($(".firstPolText")[i]).html(polValue);
                $($(".aqiValue")[i]).val(aqiValue1);
                $($(".aqiValueII")[i]).val(aqiValue2);
            }
        }
        else if (index == "3") {
            polValue = $($(".firstPolText")[36]).html();
            aqiValue1 = $($(".aqiValue")[36]).val();
            aqiValue2 = $($(".aqiValueII")[36]).val();
            if (i > 35 && i <= 53) {
                $($(".firstPolText")[i]).html(polValue);
                $($(".aqiValue")[i]).val(aqiValue1);
                $($(".aqiValueII")[i]).val(aqiValue2);
            }
        }
    }
    RefershMap(index);
}


//function GetAreaPinying(siteID) {
//    if (siteID != "") {
//        switch (siteID) {
//            case "58367":
//                return "XuHui";
//                break;
//            case "58370":
//                return "PuDong";
//                break;
//            case "58361":
//                return "MinHang";
//                break;
//            case "58362":
//                return "BaoShanArea";
//                break;
//            case "58462":
//                return "SongJiang";
//                break;
//            case "58460":
//                return "JinShan";
//                break;
//            case "58461":
//                return "QingPu";
//                break;
//            case "58463":
//                return "FengXian";
//                break;
//            case "58365":
//                return "JiaDing";
//                break;
//            case "58366":
//                return "ChongMing";
//                break;
//        }
//    }
//}


/*薛辉*/
//王斌  2017.5.8
function clear() {
    $.each($(".firstPolText"), function (i2, n2) {
        n2.innerText = "五星";
    });
    $.each($(".aqiValue"), function (i3, n3) {
        n3.value = "";
    });
    $.each($(".aqiValueII"), function (i4, n4) {
        n4.value = "";
    });
    $.each($(".dateSelect .firstPolUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            var c = $($(".aqiInputs")[i]);
            c.removeClass();
            c.addClass("levelColor " + "levelColor_1 aqiInputs");
        })
    });

    RefershMap("1");
}


function radioClickModule(id) {

    var el = Ext.getDom(id);

    var forecatTimes = Ext.getDom("forecastTime").innerText;
    var time = convertDate(forecatTimes);

    clear();

    if (el.id == "p08") {
        Ext.getDom("dd1er").innerHTML = time.add('d', 0).format("m月d日");
        Ext.getDom("dd2er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd3er").innerHTML = time.add('d', 2).format("m月d日");

        Ext.getDom("pubTime").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHour").innerHTML = "08";

        if (Ext.getDom("dd1er").className == "singleHazeLevel singleHazeLevel_Selected") {
            Ext.getDom("mapDate").innerHTML = time.add('d', 0).format("Y年m月d日");
        }
        if (Ext.getDom("dd2er").className == "singleHazeLevel singleHazeLevel_Selected") {
            Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
        }
        if (Ext.getDom("dd3er").className == "singleHazeLevel singleHazeLevel_Selected") {
            Ext.getDom("mapDate").innerHTML = time.add('d', 2).format("Y年m月d日");
        }

    } else {
        Ext.getDom("dd1er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd2er").innerHTML = time.add('d', 2).format("m月d日");
        Ext.getDom("dd3er").innerHTML = time.add('d', 3).format("m月d日");

        Ext.getDom("pubTime").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHour").innerHTML = "20";

        if (Ext.getDom("dd1er").className == "singleHazeLevel singleHazeLevel_Selected") {
            Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
        }
        if (Ext.getDom("dd2er").className == "singleHazeLevel singleHazeLevel_Selected") {
            Ext.getDom("mapDate").innerHTML = time.add('d', 2).format("Y年m月d日");
        }
        if (Ext.getDom("dd3er").className == "singleHazeLevel singleHazeLevel_Selected") {
            Ext.getDom("mapDate").innerHTML = time.add('d', 3).format("Y年m月d日");
        }
    }

    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";

        if (oldPeriod != "") {
            var oldObj = Ext.getDom(oldPeriod);
            oldObj.className = "radioUnChecked";
            oldPeriod = id;
        }
    }

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询数据..." });
    myMask.show();
    RefreshData();
    myMask.hide();

}

/*薛辉*/
function radioClickDate(id) {
    var id2;
    var el = Ext.getDom(id);
    //alert(el.innerHTML);
    var forecatTimes = Ext.getDom("forecastTime").innerText;
    var time = convertDate(forecatTimes);

    if (el.className == "singleHazeLevel singleHazeLevel_UnSelected") {
        el.className = "singleHazeLevel singleHazeLevel_Selected";

        if (id == "dd1er") {

            id = "dd2er";
            id2 = "dd3er";
            $('#' + id).css("margin-top", "-33px");
            $('#' + id2).css("margin-top", "-33px");
            $('#aqiAreaTableII-ertong').css("display", "none");
            $('#aqiAreaTableIII-ertong').css("display", "none");
            $('#aqiAreaTable').css("display", "block");
            Ext.getDom(id).className = "singleHazeLevel singleHazeLevel_UnSelected";
            Ext.getDom(id2).className = "singleHazeLevel singleHazeLevel_UnSelected";
            RefershMap("1");

            if (oldPeriod == "p08")
                Ext.getDom("mapDate").innerHTML = time.add('d', 0).format("Y年m月d日");
            else
                Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
        }
        else if (id == "dd2er") {
            $('#' + id).css("margin-top", "-28px");
            id = "dd1er";
            id2 = "dd3er";
            $('#' + id2).css("margin-top", "-33px");
            $('#aqiAreaTableII-ertong').css("display", "block");
            $('#aqiAreaTable').css("display", "none");
            $('#aqiAreaTableIII-ertong').css("display", "none");
            Ext.getDom(id).className = "singleHazeLevel singleHazeLevel_UnSelected";
            Ext.getDom(id2).className = "singleHazeLevel singleHazeLevel_UnSelected";
            RefershMap("2");

            if (oldPeriod == "p08")
                Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
            else
                Ext.getDom("mapDate").innerHTML = time.add('d', 2).format("Y年m月d日");
        }
        else if (id == "dd3er") {
            $('#' + id).css("margin-top", "-28px");
            id = "dd1er";
            id2 = "dd2er";
            $('#' + id2).css("margin-top", "-28px");
            $('#aqiAreaTableII-ertong').css("display", "none");
            $('#aqiAreaTableIII-ertong').css("display", "block");
            $('#aqiAreaTable').css("display", "none");
            Ext.getDom(id).className = "singleHazeLevel singleHazeLevel_UnSelected";
            Ext.getDom(id2).className = "singleHazeLevel singleHazeLevel_UnSelected";
            RefershMap("3");

            if (oldPeriod == "p08")
                Ext.getDom("mapDate").innerHTML = time.add('d', 2).format("Y年m月d日");
            else
                Ext.getDom("mapDate").innerHTML = time.add('d', 3).format("Y年m月d日");
        }
    }
}


// 王斌  2017.4.18
function publish() {
    var cf = confirm("是否要发布数据？");
    if (cf == false)
        return;

    var p08 = Ext.getDom("p08")
    var p20 = Ext.getDom("p20")
    var period = "";
    var type = "综合观景指数"
    if (p08.className == "radioChecked") {
        period = "08";
    }
    else {
        period = "20";
    }
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.IndexRelease', 'Publish'),
        params: { period: period, type: type },
        success: function (response) {
            if (response.responseText == "OK") {
                alert("发布成功！");
            }
            else if (response.responseText == "请保存数据后发布") {
                alert("未保存数据，请保存数据后发布")
            }
            else {
                alert("文件上传失败,错误信息为：" + response.responseText)
            }
        }
    });
}

function DateChange(el) {
    $("#forecastTime").html(el.value);
    radioClickModule(oldPeriod);
}