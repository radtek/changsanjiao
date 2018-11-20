//存储暂存上传ftp的word文件名
var wordFileName = "";
var win;
var wrfChemWin;
var nmcWin;
var MapPannel;
var userName = "";
var oldPeriod = "p10";
Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

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
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("body").css("min-width", $(window).width() + "px");
    //    $("#rightContent").height(pageHeight - 40);
    $("#textPart").height(pageHeight - 479);
    $("#aqiText").height(pageHeight - 469 - 50);
    $(".districtArea").css({ marginTop: (pageHeight - 50 - 616) / 2 });
    $.each($(".aqiAreaTable"), function (i, n) {
        $($(".aqiAreaTable")[i]).css({ width: (pageWidth - 552) });

    });

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
    $.each($(".dateSelect .firstPolUl"), function (i, n) {
        $.each($(n).find("li"), function (j, m) {
            $(m).click(function () {
                $($(".firstPolText")[i]).html($(m).html());
                var firstItem = $.trim(m.innerText);

                var polLevelIndex = "1";
                switch (firstItem) {
                    case "低":
                        polLevelIndex = "1";
                        break;
                    case "轻微":
                        polLevelIndex = "2";
                        break;
                    case "中等":
                        polLevelIndex = "3";
                        break;
                    case "较高":
                        polLevelIndex = "4";
                        break;
                    case "高":
                        polLevelIndex = "6";
                        break;
                }

                var c = $($(".aqiInputs")[i]);
                c.removeClass();
                c.addClass("levelColor " + "levelColor_" + polLevelIndex + " aqiInputs");

                if (i > -1 && i <= 9) {
                    RefershMap("1");
                }
                if (i > 9 && i <= 19) {
                    RefershMap("2");
                }
                if (i > 19 && i <= 29) {
                    RefershMap("3");
                }
                if (i > 29 && i <= 39) {
                    RefershMap("4");
                }
                if (i > 39 && i <= 49) {
                    RefershMap("5");
                }
                if (i > 49 && i <= 59) {
                    RefershMap("6");
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


    //保存按钮
    $("#foreSave").click(function () {
        var vs = "";
        $.each($(".areaName"), function (i, n) {
            var text = n.innerText;
            var area = $.trim(text);
            vs += area + ",";
        });
        $.each($(".firstPolText"), function (i2, n2) {
            var text = n2.innerText;
            var firstItem = $.trim(text);
            vs += firstItem + ",";
        });
        $.each($(".aqiValue"), function (i3, n3) {
            var text = n3.value;
            var value = $.trim(text);
            vs += value + ",";
        });
        $.each($(".aqiValueII"), function (i4, n4) {
            var text = n4.value;
            var valueII = $.trim(text);
            vs += valueII + ",";
        });

        var user = Ext.getDom("forecaster").innerText;
        var forecastTime = Ext.getDom("forecastTime").innerText;
        var type = "感冒";
        var priod;
        if (oldPeriod == "p10")
            priod = "10";
        else
            priod = "17";

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.HealthyWeather', 'SaveHealthyWeather'),
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

    /*薛辉 , 底图暂时没有做好代码先屏蔽*/
    //    RefershMap("1");
    //    RefershMap("3");
    //    RefershMap("5");
});

function GetFirstLevel(firstItem) {
    var polLevelIndex;
    switch (firstItem) {
        case "低":
            polLevelIndex = "1";
            break;
        case "轻微":
            polLevelIndex = "2";
            break;
        case "中等":
            polLevelIndex = "3";
            break;
        case "较高":
            polLevelIndex = "4";
            break;
        case "高":
            polLevelIndex = "6";
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
        if (index == "1") {
            if (i > 9) return true;
        }
        if (index == "2") {
            x = 1;
            if (i > 19) return true;
        }
        if (index == "3") {
            x = 2;
            if (i > 29) return true;
        }
        if (index == "4") {
            x = 3;
            if (i > 39) return true;
        }
        if (index == "5") {
            x = 4;
            if (i > 49) return true;
        }
        if (index == "6") {
            x = 5;
            if (i > 59) return true;
        }

        var j = i;
        if (i >= 10) {
            j = i - (10 * x);
        }

        //等图做好了
        if (index == 1 || index == 2) {
            $($(".mps div")[j]).removeClass();
            $($(".mps div")[j]).addClass($(".mps div")[j].id + " " + $(".mps div")[j].id + "_" + polLevelIndexs);
            var c = $($(".aqiInputs")[i]);
            c.removeClass();
            c.addClass("levelColor " + "levelColor_" + polLevelIndexs + " aqiInputs");
        }
        else if (index == 3) {
            if (i > 19 && i <= 29) {
                $($(".mpsII div")[j]).removeClass();
                $($(".mpsII div")[j]).addClass($(".mpsII div")[j].id + " " + $(".mpsII div")[j].id + "_" + polLevelIndexs);
                var c = $($(".aqiInputs")[i]);
                c.removeClass();
                c.addClass("levelColor " + "levelColor_" + polLevelIndexs + " aqiInputs");
            }
        }
        else if (index == 4) {
            if (i > 29 && i <= 39) {
                $($(".mpsII div")[j]).removeClass();
                $($(".mpsII div")[j]).addClass($(".mpsII div")[j].id + " " + $(".mpsII div")[j].id + "_" + polLevelIndexs);
                var c = $($(".aqiInputs")[i]);
                c.removeClass();
                c.addClass("levelColor " + "levelColor_" + polLevelIndexs + " aqiInputs");
            }
        }
        else if (index == 5) {
            if (i > 39 && i <= 49) {
                $($(".mpsIII div")[j]).removeClass();
                $($(".mpsIII div")[j]).addClass($(".mpsIII div")[j].id + " " + $(".mpsIII div")[j].id + "_" + polLevelIndexs);
                var c = $($(".aqiInputs")[i]);
                c.removeClass();
                c.addClass("levelColor " + "levelColor_" + polLevelIndexs + " aqiInputs");
            }
        }
        else if (index == 6) {
            if (i > 49 && i <= 59) {
                $($(".mpsIII div")[j]).removeClass();
                $($(".mpsIII div")[j]).addClass($(".mpsIII div")[j].id + " " + $(".mpsIII div")[j].id + "_" + polLevelIndexs);
                var c = $($(".aqiInputs")[i]);
                c.removeClass();
                c.addClass("levelColor " + "levelColor_" + polLevelIndexs + " aqiInputs");
            }
        }


       

    });
}

/*刷新数据   -薛辉*/
function RefreshData() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询..." });
    myMask.show();
    refershChildForecast("1");
    refershYougForecast("1");  
    refershOldForecast("1");
    myMask.hide();
}

function refershChildForecast(flag){
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'QueryHealthyWeatherForecast'),
        params: { priod: priod, type: "儿童感冒", forecastTime: forecastTime },
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
                for (var i = 0; i < $(".firstPolText").length; i++) {

                        if (i<=9) {
                            for(var j=0; j<lst.length;j++){
                                   if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                    
                        if (i > 9 && i <= 19) {
                          for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                        if (i <= 9) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
       

                         if (i > 9 && i <= 19) {
                             for(var j=0; j<lst.length;j++){
                             if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }

                 for (var i = 0; i < $(".aqiValueII").length; i++) {

                        if (i <= 9) {
                             for(var j=0; j<lst.length;j++){
                            if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                    break;
                              }
                            }
                        }
       

                       if (i > 9 && i <= 19) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                     break;
                              }
                            }
                        }

                }
              }
                 if (flag == "1")
                     refershChild();
        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "儿童数据请求失败，错误代码为：" + response.status);
        }
    });

    
}

function refershChild() {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'QueryHealthyWeather'),
        params: { priod: priod, type: "儿童感冒", forecastTime: forecastTime,user:user },
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
                for (var i = 0; i < $(".firstPolText").length; i++) {

                        if (i<=9) {
                            for(var j=0; j<lst.length;j++){
                                  var v=$($(".c_RegionName")[i]).text();
                                 if(lst[j]==time && sites[j]==v){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                    
                        if (i > 9 && i <= 19) {
                          for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                        if (i <= 9) {
                             for(var j=0; j<lst.length;j++){
                                var v=$($(".c_RegionName")[i]).text();
                                if(lst[j]==time && sites[j]==v){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
       

                         if (i > 9 && i <= 19) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }

                 for (var i = 0; i < $(".aqiValueII").length; i++) {

                        if (i <= 9) {
                             for(var j=0; j<lst.length;j++){
                                var v=$($(".c_RegionName")[i]).text();
                                if(lst[j]==time && sites[j]==v){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                    break;
                              }
                            }
                        }
       

                       if (i > 9 && i <= 19) {
                             for(var j=0; j<lst.length;j++){
                              if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                     break;
                              }
                            }
                         }
                     }
                 }
                 if (Ext.getDom("dd1er").className == "singleHazeLevel singleHazeLevel_Selected")
                     RefershMap("1");
                 else
                     RefershMap("2")
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
   refershYougForecast("0");
   refershOldForecast("0");
}

function refershYougForecast(flag) {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'QueryHealthyWeatherForecast'),
        params: { priod: priod, type: "成人感冒", forecastTime: forecastTime },
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
                for (var i = 0; i < $(".firstPolText").length; i++) {

                        if (i > 19 && i<=29) {
                            for(var j=0; j<lst.length;j++){
                               if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                    
                        if (i > 29 && i <= 39) {
                          for(var j=0; j<lst.length;j++){
                              if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                        if (i > 19 && i <= 29) {
                             for(var j=0; j<lst.length;j++){
                              if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
       
                        if (i > 29 && i <= 39) {
                             for(var j=0; j<lst.length;j++){
                                if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                }

                 for (var i = 0; i < $(".aqiValueII").length; i++) {

                        if (i > 19 && i <= 29) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                    break;
                              }
                            }
                        }
       

                        if (i > 29 && i <= 39) {
                             for(var j=0; j<lst.length;j++){
                             if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }
             }
                 if (flag == "1")
                     refershYoug();
        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "青年数据请求失败，错误代码为：" + response.status);
        }
    });

    
}

function refershYoug() {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'QueryHealthyWeather'),
        params: { priod: priod, type: "青年感冒", forecastTime: forecastTime,user:user },
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
                for (var i = 0; i < $(".firstPolText").length; i++) {

                        if (i > 19 && i<=29) {
                            for(var j=0; j<lst.length;j++){
                              if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                    
                        if (i > 29 && i <= 39) {
                          for(var j=0; j<lst.length;j++){
                              if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                        if (i > 19 && i <= 29) {
                             for(var j=0; j<lst.length;j++){
                                var v=$($(".c_RegionName")[i]).text();
                                if(lst[j]==time && sites[j]==v){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
       

                        if (i > 29 && i <= 39) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }

                 for (var i = 0; i < $(".aqiValueII").length; i++) {

                        if (i > 19 && i <= 29) {
                             for(var j=0; j<lst.length;j++){
                               var v=$($(".c_RegionName")[i]).text();
                                if(lst[j]==time && sites[j]==v){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                    break;
                              }
                            }
                        }
       

                        if (i > 29 && i <= 39) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }
              }
                 if (Ext.getDom("dd3er").className == "singleHazeLevel singleHazeLevel_Selected")
                     RefershMap("3");
                 else
                     RefershMap("4")
        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "青年数据请求失败，错误代码为：" + response.status);
        }
    });
}

function refershOldForecast(flag) {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'QueryHealthyWeatherForecast'),
        params: { priod: priod, type: "老人感冒", forecastTime: forecastTime },
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
                for (var i = 0; i < $(".firstPolText").length; i++) {

                       if (i > 39 && i<=49) {
                            for(var j=0; j<lst.length;j++){
                               if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                    
                        if (i > 49 && i <= 59) {
                          for(var j=0; j<lst.length;j++){
                               if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                        if (i > 39 && i<=49) {
                             for(var j=0; j<lst.length;j++){
                               if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
       

                        if (i > 49 && i <= 59) {
                             for(var j=0; j<lst.length;j++){
                             if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }

                 for (var i = 0; i < $(".aqiValueII").length; i++) {

                        if (i > 39 && i<=49) {
                             for(var j=0; j<lst.length;j++){
                             if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                    break;
                              }
                            }
                        }
       

                      if (i > 49 && i <= 59) {
                             for(var j=0; j<lst.length;j++){
                              if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }
              }
                 if (flag == "1")
                     refershOld();
        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "老人数据请求失败，错误代码为：" + response.status);
        }
    });
   
}

function refershOld() {
    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'QueryHealthyWeather'),
        params: { priod: priod, type: "老年感冒", forecastTime: forecastTime ,user:user},
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
                for (var i = 0; i < $(".firstPolText").length; i++) {

                       if (i > 39 && i<=49) {
                            for(var j=0; j<lst.length;j++){
                               if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                    
                        if (i > 49 && i <= 59) {
                          for(var j=0; j<lst.length;j++){
                             if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = dj[(j)];
                                    $($(".firstPolText")[i]).html(polValue);
                                    setColor(i, polValue);
                                    break;
                              }
                            }
                        }
                }

                for (var i = 0; i < $(".aqiValue").length; i++) {

                        if (i > 39 && i<=49) {
                             for(var j=0; j<lst.length;j++){
                                if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
       

                        if (i > 49 && i <= 59) {
                             for(var j=0; j<lst.length;j++){
                             if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = rq[(j)];
                                    $($(".aqiValue")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                    
                }

                 for (var i = 0; i < $(".aqiValueII").length; i++) {

                        if (i > 39 && i<=49) {
                             for(var j=0; j<lst.length;j++){
                                if(lst[j]==time && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                    break;
                              }
                            }
                        }
       

                      if (i > 49 && i <= 59) {
                             for(var j=0; j<lst.length;j++){
                            if(lst[j]==timeII && sites[j]==$($(".c_RegionName")[i]).text()){
                                    var polValue = cs[(j)];
                                    $($(".aqiValueII")[i]).val(polValue);
                                     break;
                              }
                            }
                        }
                }
                 }

                 if (Ext.getDom("dd5er").className == "singleHazeLevel singleHazeLevel_Selected")
                     RefershMap("5");
                 else
                     RefershMap("6")
        },
        failure: function (response) {
            //myMask.hide();
            //Ext.Msg.alert("错误", "老人数据请求失败，错误代码为：" + response.status);
        }
    });

}

function setColor(i,text) {
            var c = $($(".aqiInputs")[i]);
            c.removeClass();
            if(text=="低")
                c.addClass("levelColor " + "levelColor_1 aqiInputs");
            if (text == "轻微")
                c.addClass("levelColor " + "levelColor_2 aqiInputs");
            if (text == "中等")
                c.addClass("levelColor " + "levelColor_3 aqiInputs");
            if (text == "较高")
                c.addClass("levelColor " + "levelColor_4 aqiInputs");
            if (text == "高")
                c.addClass("levelColor " + "levelColor_5 aqiInputs");
 }

/*暂存-薛辉*/
function TempSave(type,t) {
    var vs = "";
    var type = "";
    var lst = Ext.getDom(t).innerText;
    $.each($(".areaName"), function (i, n) {
        if (i < 10) {
            var text = n.innerText;
            var area = $.trim(text);
            vs += area + ",";
        }
    });
    $.each($(".firstPolText"), function (i2, n2) {

        if (t == "dd1er") {
            if (i2 < 10) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd2er") {
            if (i2 >9 && i2<20 ) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd3er") {
            if (i2 > 19 && i2 < 30) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd4er") {
            if (i2 > 29 && i2 < 40) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd5er") {
            if (i2 > 39 && i2 < 50) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd6er") {
            if (i2 > 49 && i2 < 60) {
                var text = n2.innerText;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }

    });
    $.each($(".aqiValue"), function (i3, n3) {
        if (t == "dd1er") {
            if (i3 < 10) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
                type = "儿童感冒";
            }
        }
        else if (t == "dd2er") {
            if (i3 > 9 && i3 < 20) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
                type = "儿童感冒";
            }
        }
        else if (t == "dd3er") {
            if (i3 > 19 && i3 < 30) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
                type = "青年感冒";
            }
        }
        else if (t == "dd4er") {
            if (i3 > 29 && i3 < 40) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
                type = "青年感冒";
            }
        }
        else if (t == "dd5er") {
            if (i3 > 39 && i3 < 50) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
                type = "老年感冒";
            }
        }
        else if (t == "dd6er") {
            if (i3 > 49 && i3 < 60) {
                var text = n3.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
                type = "老年感冒";
            }
        }
    });
    $.each($(".aqiValueII"), function (i4, n4) {
        if (t == "dd1er") {
            if (i4 < 10) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd2er") {
            if (i4 > 9 && i4 < 20) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd3er") {
            if (i4 > 19 && i4 < 30) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd4er") {
            if (i4 > 29 && i4 < 40) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd5er") {
            if (i4 > 39 && i4 < 50) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
        else if (t == "dd6er") {
            if (i4 > 49 && i4 < 60) {
                var text = n4.value;
                var firstItem = $.trim(text);
                vs += firstItem + ",";
            }
        }
    });

    var user = Ext.getDom("forecaster").innerText;
    var forecastTime = Ext.getDom("forecastTime").innerText;
    var priod;
    if (oldPeriod == "p10")
        priod = "10";
    else
        priod = "17";

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealthyWeather', 'SaveTempHealthyWeather'),
        params: { texts: vs, priod: priod, type: type, forecastTime: forecastTime, user: user,lst:lst},
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
      for (var i = 1; i < $(".firstPolText").length; i++) {
          if (index == "1") {
              if(i<=9)
                 $($(".firstPolText")[i]).html(polValue);
          }
          else if (index == "2") {
              polValue = $($(".firstPolText")[10]).html();
              if (i >9 && i<=19)
                  $($(".firstPolText")[i]).html(polValue);
          }
          else if (index == "3") {
              polValue = $($(".firstPolText")[20]).html();
              if (i > 19 && i <= 29)
                  $($(".firstPolText")[i]).html(polValue);
          }
          else if (index == "4") {
              polValue = $($(".firstPolText")[30]).html();
              if (i > 29 && i <= 39)
                  $($(".firstPolText")[i]).html(polValue);
          }
          else if (index == "5") {
              polValue = $($(".firstPolText")[40]).html();
              if (i > 39 && i <= 49)
                  $($(".firstPolText")[i]).html(polValue);
          }
          else if (index == "6") {
              polValue = $($(".firstPolText")[50]).html();
              if (i > 49 && i <= 59)
                  $($(".firstPolText")[i]).html(polValue);
          }
      }
      RefershMap(index);
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

/*薛辉*/
function clear() {
    $.each($(".firstPolText"), function (i2, n2) {
         n2.innerText="低";
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
    RefershMap("3");
    RefershMap("5");
}

  

function radioClickModule(id) {

    var el = Ext.getDom(id);

    var forecatTimes = Ext.getDom("forecastTime").innerText;
    var time=convertDate(forecatTimes);

    clear();

    if (el.id == "p10") {
        Ext.getDom("dd1er").innerHTML = time.add('d', 0).format("m月d日");
        Ext.getDom("dd2er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd3er").innerHTML = time.add('d', 0).format("m月d日");
        Ext.getDom("dd4er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd5er").innerHTML = time.add('d', 0).format("m月d日");
        Ext.getDom("dd6er").innerHTML = time.add('d', 1).format("m月d日");

        Ext.getDom("pubTime").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHour").innerHTML = "10";
        Ext.getDom("pubTimeII").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHourII").innerHTML = "10";
        Ext.getDom("pubTimeIII").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHourIII").innerHTML = "10"; 

        if(Ext.getDom("dd1er").className=="singleHazeLevel singleHazeLevel_Selected")
           Ext.getDom("mapDate").innerHTML = time.add('d', 0).format("Y年m月d日");
        else
           Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");

        if(Ext.getDom("dd3er").className=="singleHazeLevel singleHazeLevel_Selected")
           Ext.getDom("mapDateII").innerHTML = time.add('d', 0).format("Y年m月d日");
        else
           Ext.getDom("mapDateII").innerHTML = time.add('d', 1).format("Y年m月d日");

        if(Ext.getDom("dd5er").className=="singleHazeLevel singleHazeLevel_Selected")
           Ext.getDom("mapDateIII").innerHTML = time.add('d', 0).format("Y年m月d日");
        else
           Ext.getDom("mapDateIII").innerHTML = time.add('d', 1).format("Y年m月d日");

    } else {
        Ext.getDom("dd1er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd2er").innerHTML = time.add('d', 2).format("m月d日");
        Ext.getDom("dd3er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd4er").innerHTML = time.add('d', 2).format("m月d日");
        Ext.getDom("dd5er").innerHTML = time.add('d', 1).format("m月d日");
        Ext.getDom("dd6er").innerHTML = time.add('d', 2).format("m月d日");

        Ext.getDom("pubTime").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHour").innerHTML = "17";
        Ext.getDom("pubTimeII").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHourII").innerHTML = "17";
        Ext.getDom("pubTimeIII").innerHTML = time.format("Y年m月d日");
        Ext.getDom("pubHourIII").innerHTML = "17"; 

       if(Ext.getDom("dd1er").className=="singleHazeLevel singleHazeLevel_Selected")
           Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
        else
           Ext.getDom("mapDate").innerHTML = time.add('d', 2).format("Y年m月d日");

        if(Ext.getDom("dd3er").className=="singleHazeLevel singleHazeLevel_Selected")
           Ext.getDom("mapDateII").innerHTML = time.add('d', 1).format("Y年m月d日");
        else
           Ext.getDom("mapDateII").innerHTML = time.add('d', 2).format("Y年m月d日");

        if(Ext.getDom("dd5er").className=="singleHazeLevel singleHazeLevel_Selected")
           Ext.getDom("mapDateIII").innerHTML = time.add('d', 1).format("Y年m月d日");
        else
           Ext.getDom("mapDateIII").innerHTML = time.add('d', 2).format("Y年m月d日");
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
    var el = Ext.getDom(id);

    var forecatTimes = Ext.getDom("forecastTime").innerText;
    var time = convertDate(forecatTimes);

    if (el.className == "singleHazeLevel singleHazeLevel_UnSelected") {
        el.className = "singleHazeLevel singleHazeLevel_Selected";

        if (id == "dd1er") {
            id = "dd2er";
            $('#' + id).css("margin-top", "-32px");
            $('#aqiAreaTable').css("display", "block");
            $('#aqiAreaTableII-ertong').css("display", "none");
            RefershMap("1");

             if(oldPeriod=="p10")
              Ext.getDom("mapDate").innerHTML = time.add('d', 0).format("Y年m月d日");
            else
              Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
        }
        else {
            $('#' + id).css("margin-top", "-28px");
            id = "dd1er";
            $('#aqiAreaTableII-ertong').css("display", "block");
            $('#aqiAreaTable').css("display", "none");
            RefershMap("2");

            if(oldPeriod=="p10")
              Ext.getDom("mapDate").innerHTML = time.add('d', 1).format("Y年m月d日");
            else
              Ext.getDom("mapDate").innerHTML = time.add('d', 2).format("Y年m月d日");
        }

        if (id != "") {
            var oldObj = Ext.getDom(id);
            oldObj.className = "singleHazeLevel singleHazeLevel_UnSelected";
        }
    }
}

function radioClickDateII(id) {
    var el = Ext.getDom(id);

    var forecatTimes = Ext.getDom("forecastTime").innerText;
    var time = convertDate(forecatTimes);

    if (el.className == "singleHazeLevel singleHazeLevel_UnSelected") {
        el.className = "singleHazeLevel singleHazeLevel_Selected";

        if (id == "dd3er") {
            id = "dd4er";
            $('#' + id).css("margin-top", "-32px");
            $('#aqiAreaTableQN').css("display", "block");
            $('#aqiAreaTableQNII').css("display", "none");
            RefershMap("3");

            if(oldPeriod=="p10")
              Ext.getDom("mapDateII").innerHTML = time.add('d', 0).format("Y年m月d日");
            else
              Ext.getDom("mapDateII").innerHTML = time.add('d', 1).format("Y年m月d日");
        }
        else {
            $('#' + id).css("margin-top", "-28px");
            id = "dd3er";
            $('#aqiAreaTableQNII').css("display", "block");
            $('#aqiAreaTableQN').css("display", "none");
            RefershMap("4");

            if(oldPeriod=="p10")
              Ext.getDom("mapDateII").innerHTML = time.add('d', 1).format("Y年m月d日");
            else
              Ext.getDom("mapDateII").innerHTML = time.add('d', 2).format("Y年m月d日");
        }

        if (id != "") {
            var oldObj = Ext.getDom(id);
            oldObj.className = "singleHazeLevel singleHazeLevel_UnSelected";
        }
    }
}

function radioClickDateIII(id) {
    var el = Ext.getDom(id);

    var forecatTimes = Ext.getDom("forecastTime").innerText;
    var time = convertDate(forecatTimes);

    if (el.className == "singleHazeLevel singleHazeLevel_UnSelected") {
        el.className = "singleHazeLevel singleHazeLevel_Selected";

        if (id == "dd5er") {
            id = "dd6er";
            $('#' + id).css("margin-top", "-32px");
            $('#aqiAreaTableLR').css("display", "block");
            $('#aqiAreaTableLRII').css("display", "none");
            RefershMap("5");

             if(oldPeriod=="p10")
              Ext.getDom("mapDateIII").innerHTML = time.add('d', 0).format("Y年m月d日");
            else
              Ext.getDom("mapDateIII").innerHTML = time.add('d', 1).format("Y年m月d日");
        }
        else {
            $('#' + id).css("margin-top", "-28px");
            id = "dd5er";
            $('#aqiAreaTableLRII').css("display", "block");
            $('#aqiAreaTableLR').css("display", "none");
            RefershMap("6");
            
             if(oldPeriod=="p10")
              Ext.getDom("mapDateIII").innerHTML = time.add('d', 1).format("Y年m月d日");
             else
              Ext.getDom("mapDateIII").innerHTML = time.add('d', 2).format("Y年m月d日");
        }

        if (id != "") {
            var oldObj = Ext.getDom(id);
            oldObj.className = "singleHazeLevel singleHazeLevel_UnSelected";
        }
    }
}


function DateChange(el) {
       $("#forecastTime").html(el.value);
       radioClickModule(oldPeriod);
  }





function cutDiv(type, foretime, pubtime,mp,hr) {

    var data = "";
    $(('#' + mp + ' div')).each(function (i) {
        data+=(($(this).attr('class')).split(" ")[1]+"*");
    });
    var strForecastDate = Ext.getDom(foretime).innerHTML;
    var strPublishDate = Ext.getDom(pubtime).innerHTML;
    var hour = Ext.getDom(hr).innerHTML + ":00:00";
    strPublishDate = strPublishDate  +" "+hour;
    

       Ext.Ajax.request({
           url: getUrl('MMShareBLL.DAL.HealthyWeather', 'SaveDrawMap'),
           params: { data: data, strForecastDate: strForecastDate, strPublishDate: strPublishDate, Type: type },
           success: function (response) {
                        if (response.responseText != "") {
                            var defaultURL = "WebExplorers.ashx";
                            window.onbeforeunload = function () { }
                            window.location.href = defaultURL + "?action=DOWNLOAD&value1=" + encodeURIComponent(response.responseText);
                            window.onbeforeunload = function () {
                            }
                        }
                    },
                    failure: function (response) {
                        myMask.hide();
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                 });

               
        }