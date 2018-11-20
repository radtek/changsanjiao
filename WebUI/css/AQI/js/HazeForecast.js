
var userName = "";
var todayContent = "1";
var tomorrowContent = "1";
var afterContent = "1"
var win;
var todayFullContent = "无霾";
var tomorrowFullContent = "无霾";
var afterFullContent = "无霾"

var hazeLevel24="无霾"

//三天的能见度
var todayVis = "null";
var tomorrowVis = "null";
var afterVis = "null"
var vis24 = "null"

//标记预报的时段
var periodIndex = 0;
//标记霾的等级
var hazeLevel = "无霾";

var todayLevelIndex = 0;
var tomorrowLevelIndex = 0;
var afterLevelIndex = 0;

//用于上传发布的txt内容
var upLoadContent = "";

var nowDateTime=getNowFormatDate();
function CreateTxtProductResult2(time_index, content) {
    var textContent = ""; //= "<p>上海市霾的预报</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ({PublishDate}{HazeType}发布)</p><p>&nbsp;</p><p>{Today}：{TodayContent}</p><p>{Tomorrow}：{TomorrowContent}</p><p>{AfterDay}：{AfterDayContent}</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 上海市城市环境气象中心</p>";
    textContent = $("#txtHideTempleteContent05").val();    
    if (time_index == 0) {
        //        todayContent = $("#ddlHazeLevel").val();
        todayContent = GetHazeLevelNumFormText(hazeLevel); 
        todayFullContent = content;
        textContent = textContent.replace("{TodayContent}", todayFullContent);
        textContent = textContent.replace("{TomorrowContent}", tomorrowFullContent);
        textContent = textContent.replace("{AfterDayContent}", afterFullContent);
        $("#txtVisibilityMin").is(":hidden")
        todayVis = $("#txtVisibilityMin").is(":hidden") ? "" : $("#txtVisibilityMin").val();
//        $("#todayLevel").html(hazeLevel);
        //        $("#visToday").html(todayVis);
        $("#todayLevel").val(hazeLevel);
        $("#visToday").val(todayVis);        
    }
    else if (time_index == 1) {
        //        tomorrowContent = $("#ddlHazeLevel").val();
        tomorrowContent = GetHazeLevelNumFormText(hazeLevel);
        tomorrowFullContent = content;
        textContent = textContent.replace("{TomorrowContent}", tomorrowFullContent);
        textContent = textContent.replace("{TodayContent}", todayFullContent);
        textContent = textContent.replace("{AfterDayContent}", afterFullContent);
        tomorrowVis = $("#txtVisibilityMin").is(":hidden") ? "" : $("#txtVisibilityMin").val();
        $("#tomorrowLevel").val(hazeLevel);
        $("#visTom").val(tomorrowVis);
    }
    else if (time_index == 2) {
//        afterContent = $("#ddlHazeLevel").val();
        afterContent = GetHazeLevelNumFormText(hazeLevel); ;
        afterFullContent = content;
        textContent = textContent.replace("{AfterDayContent}", afterFullContent);
        textContent = textContent.replace("{TomorrowContent}", tomorrowFullContent);
        textContent = textContent.replace("{TodayContent}", todayFullContent);
        afterVis = $("#txtVisibilityMin").is(":hidden") ? "" : $("#txtVisibilityMin").val();
        $("#afterLevel").val(hazeLevel);
        $("#visAfter").val(afterVis);
    }
    return textContent;
}

function CreateTxtProductResult2Copy(time_index, content) {
    var textContent = ""; //= "<p>上海市霾的预报</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ({PublishDate}{HazeType}发布)</p><p>&nbsp;</p><p>{Today}：{TodayContent}</p><p>{Tomorrow}：{TomorrowContent}</p><p>{AfterDay}：{AfterDayContent}</p><p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 上海市城市环境气象中心</p>";
    textContent = $("#txtHideTempleteContent05").val();
    
    if (time_index == 0) {
        //        todayContent = $("#ddlHazeLevel").val();
        todayContent = GetHazeLevelNumFormText(hazeLevel); ;
        todayFullContent = content;
        textContent = textContent.replace("{TodayContent}", todayFullContent);
        textContent = textContent.replace("{TomorrowContent}", tomorrowFullContent);
        textContent = textContent.replace("{AfterDayContent}", afterFullContent);
        $("#txtVisibilityMin").is(":hidden")
        todayVis = $("#txtVisibilityMin").is(":hidden") ? "null" : $("#txtVisibilityMin").val();
    }
    else if (time_index == 1) {
        //        tomorrowContent = $("#ddlHazeLevel").val();
        tomorrowContent = GetHazeLevelNumFormText(hazeLevel);
        tomorrowFullContent = content;
        textContent = textContent.replace("{TomorrowContent}", tomorrowFullContent);
        textContent = textContent.replace("{TodayContent}", todayFullContent);
        textContent = textContent.replace("{AfterDayContent}", afterFullContent);
        tomorrowVis = $("#txtVisibilityMin").is(":hidden") ? "null" : $("#txtVisibilityMin").val();
    }
    else if (time_index == 2) {
        //        afterContent = $("#ddlHazeLevel").val();
        afterContent = GetHazeLevelNumFormText(hazeLevel); ;
        afterFullContent = content;
        textContent = textContent.replace("{AfterDayContent}", afterFullContent);
        textContent = textContent.replace("{TomorrowContent}", tomorrowFullContent);
        textContent = textContent.replace("{TodayContent}", todayFullContent);
        afterVis = $("#txtVisibilityMin").is(":hidden") ? "null" : $("#txtVisibilityMin").val();
    }
    return textContent;
}

Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    userName = result["Alias"];

    $(".visibility").hide();
    var pubHour = " 05时";
    var pubHourNum = "05";
    var curHour = new Date().getHours();
    if (curHour < 12) {
        pubHour = " 05时";
        pubHourNum = "05";
    }
    else {
        pubHour = " 17时";
        pubHourNum = "17";
    }
    $("#forecastTimeLevel").html(pubHour);
    var dateText = $("#hazeContentDate").text().replace("{TodayDate}{Time}", getFormatDate("") + pubHourNum);
    $("#hazeContentDate").text(dateText);
    $("#txtPublishEndDate").val(getFormatDate("") + pubHour);
    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#hazeArea").width(($(window).width() - 30) * 0.55);
    $("#rightArea").width(($(window).width() - 30) * 0.44);
    $("#hazeArea").height($(window).height() - 50);
    $("#rightArea").height($(window).height() - 50);
    $("#txtArea_Haze").height($(window).height() - 290);

    $("#ozoneArea").height($(window).height() - 50 - 295);

    $("#ozoneContent").height(($(window).height() - 50 - 415) / 2);
    $("#displayTable").height(($(window).height() - 50 - 415) / 2);

    $("body").css("min-width", $(window).width() + "px");

    $("#todayLevel").val(hazeLevel);
    $("#tomorrowLevel").val(hazeLevel);
    $("#afterLevel").val(hazeLevel);

    $("#visToday").val("");
    $("#visTom").val("");
    $("#visAfter").val("");
    $("#visTom24").val("");




    if (!win) {//如果不存在win对象择创建
        win = new Ext.Window({
            title: '预报文本预览',
            width: 500,
            height: 350,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            bodyStyle: "color:Red",
            items: new Ext.Panel({//窗体中中是一个一个TabPanel
                deferredRender: false,
                border: false,
                id: "hazeText",
                html: "<textarea id='hazeTextArea' style='width:100%;height:100%'>分区表格</textarea>"
            })
        });
    }

    if ($.browser.msie) {
        $("#txtVisibilityMin").get(0).attachEvent("onpropertychange", function (o) {
            SetContent();
        });
        //非IE  
    }
    else {
        $("#txtVisibilityMin").get(0).addEventListener("input", function (o) {
            SetContent();
        }, true);
    }
    //最小能见度
    $("#txtVisibilityMin").change(function () {
        SetContent();
    });

    $(".visTag").hide();
    $(".hazeDis").hide();

    $(".km").hide();

    $(".visTag24").hide();
    $(".km24").hide();
    //SetContent();
    //GetHazeHistory();

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadSavedHazeForecastData'),
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                var hazeText = "";
                for (var obj in result) {
                    switch (result[obj].split('_')[0]) {
//                        case "1":
//                            hazeText = "无霾";
//                            break;
//                        case "2":
//                            hazeText = "轻微霾";
//                            break;
//                        case "3":
//                            hazeText = "轻度霾";
//                            break;
//                        case "4":
//                            hazeText = "中度霾";
//                            break;
//                        case "5":
//                            hazeText = "重度霾";
//                            break;
//                        case "6":
//                            hazeText = "严重霾";
                        //                            break; 
                        case "1":
                            hazeText = "无霾";
                            break;
                        case "2":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "3":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "4":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "5":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "6":
                            hazeText = result[obj].split('_')[2];
                            break;
                    }
                    if (obj == "Today") {
                        $("#todayLevel").val(hazeText);
                        todayContent = result[obj].split('_')[0];
                        if (result[obj].split('_')[1] != "") {
                            $($(".visTag")[0]).show();
                            $($(".hazeDis")[0]).show();
                            $($(".km")[0]).show();
                            $("#visToday").val(result[obj].split('_')[1]);
                            todayVis = result[obj].split('_')[1];
                        }

                    }
                    else if (obj == "Tomorrow") {
                        $("#tomorrowLevel").val(hazeText);
                        tomorrowContent = result[obj].split('_')[0];
                        if (result[obj].split('_')[1] != "") {
                            $($(".visTag")[1]).show();
                            $($(".hazeDis")[1]).show();
                            $($(".km")[1]).show();
                            $("#visTom").val(result[obj].split('_')[1]);
                            tomorrowVis = result[obj].split('_')[1];
                        }
                    }
                    else if (obj == "After") {
                        $("#afterLevel").val(hazeText);
                        afterContent = result[obj].split('_')[0];
                        if (result[obj].split('_')[1] != "") {
                            $($(".visTag")[2]).show();
                            $($(".hazeDis")[2]).show();
                            $($(".km")[2]).show();
                            $("#visAfter").val(result[obj].split('_')[1]);
                            afterVis = result[obj].split('_')[1];
                        }
                    }
                    else if (obj == "Haze_24") {
                        $("#58367_Item").text(GetHazeLevelTextFormNum(result[obj]));
                    }

                }
            }

        },
        failure: function (response) {
        }
    });

    //获取历史记录
    $("#btnLast").click(function () {
        GetHazeHistory();
    });

    //预览按钮
    $("#hazePreview").click(function () {
        if (win) {
            win.show();
            $("#hazeTextArea").val(GetPubContent());
        }
    });


    $.each($(".dateDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".firstPolUl")[i]).is(":hidden")) {
                $($(".firstPolUl")[i]).addClass("display");
                $($(".firstPolUl")[i]).removeClass("hide");
            }
            else {
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
                $("#visTom24").val("");
                hazeLevel24 = $(m).text();
                if ($(m).text() == "无霾") {
                    $($(".visTag24")[0]).hide();
                    $($(".km24")[0]).hide();
                    //content = hazeLevel;
                }
                else {
                    $($(".visTag24")[0]).show();
                    $($(".km24")[0]).show();
                    //content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
                }
            });
        });
    });

    //24小时霾预报保存入库
    $("#saveHaze24").click(function () {
        var hourType = "05";
        var curHour = new Date().getHours();
        if (curHour < 12) {
            hourType = "05";
        }
        else {
            hourType = "17";
        }
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveHazeForecastData_24'),
            params: { hourType: hourType, tomHaze24: GetHazeLevelNumFormText(hazeLevel24), userName: userName },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("保存成功");
                }
                else if (response.responseText == "published") {
                    alert("数据已保存！");
                }
                else {
                    alert("保存失败");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });



    //保存按键
    $("#hazeSave").click(function () {
        var todayDate = $("#todayPeriod").html();
        var tomorrowDate = $("#tomorrowPeriod").html();
        var afterDate = $("#afterPeriod").html();

        var hourType = "05";

        var todayTextContent = $("#todayLevel").val();
        var tomorrowTextContent = $("#tomorrowLevel").val();
        var afterTextContent = $("#afterLevel").val();

        var curHour = new Date().getHours();
        if (curHour < 12) {
            hourType = "05";
        }
        else {
            hourType = "17";
        }
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveHazeForecastData'),
            params: { todayDate: todayDate, tomorrowDate: tomorrowDate, afterDate: afterDate, todayHaze: todayContent, tomorrowHaze: tomorrowContent, afterHaze: afterContent, todayVis: $("#visToday").val(), tomorrowVis: $("#visTom").val(), afterVis: $("#visAfter").val(), todayTextContent: todayTextContent, tomorrowTextContent: tomorrowTextContent, afterTextContent: afterTextContent, releaseTime: getReleaseTime(), hourType: hourType },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("保存成功");
                    $("#hazeCheck").show();
                }
                else if (response.responseText == "published") {
                    alert("数据已保存！");
                    $("#hazeCheck").show();
                }
                else {
                    alert("保存失败");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //审核按键
    $("#hazeCheck").click(function () {
        alert("审核");
        $("#hazePub").show();
    });

    //发布按键
    $("#hazePub").click(function () {
        upLoadContent = GetPubContent();
        if (upLoadContent == "") {
            alert('霾预报稿不能为空！');
            return false;
        }
        //        textContent = EncodeURI(textContent, false);
        //ftp地址集合字符串
        var ftpString = $("#hazeFtpCollection").val();
        //根据当前日期获取替换文件名的字符串
        //var replaceDate = GetReplaceDateStringShortYear(new Date());
        var curHour = new Date().getHours();
        //        if (curHour < 12) {
        //            ftpString = ftpString.replace("YYMMDDHH", replaceDate + "05");
        //        }
        //        else {
        //            ftpString = ftpString.replace("YYMMDDHH", replaceDate + "17");
        //        }
        //        if (curHour < 12) {
        //            ftpString = ftpString.replace("YYMMDDHH", "YYMMDD" + "05");
        //        }
        //        else {
        //            ftpString = ftpString.replace("YYMMDDHH", "YYMMDD" + "17");
        //        }

        var fileDate = getFormatDate('');

        var publishContent = upLoadContent;
        publishContent = publishContent.replace("{PubDateContent}", $("#hazeContentDate").html());
        var hourType = "05";
        var curHour = new Date().getHours();
        if (curHour < 12) {
            hourType = "05";
            ftpString = ftpString.replace("YYMMDDHH", "YYMMDD" + "05");
        }
        else {
            hourType = "17";
            ftpString = ftpString.replace("YYMMDDHH", "YYMMDD" + "17");
        }
        //        if ($("#hazeContentDate").html().indexOf("05时") > 0) {
        //            hourType = "05";
        //            ftpString = ftpString.replace("YYMMDDHH", "YYMMDD" + "05");
        //        }
        //        else if ($("#hazeContentDate").html().indexOf("17时") > 0) {
        //            hourType = "17";
        //            ftpString = ftpString.replace("YYMMDDHH", "YYMMDD" + "17");
        //        }

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpLoadTxtFtpLatestForHaze'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "HazeForecast", txtContent: publishContent, userName: userName, hourType: hourType },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("发布成功");
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


    //日期切换与霾等级切换
    $.each($(".singlePeriod"), function (i, n) {
        //时段为今天
        $(n).click(function () {
            $(n).addClass("singlePeriod_Selected");
            $.each($(".singlePeriod"), function (j, m) {
                if (m != n) {
                    $(m).removeClass("singlePeriod_Selected");
                }
            });
            if (n.id == "todayPeriod") {
                periodIndex = 0;
                $.each($(".singleHazeLevel"), function (j, m) {
                    if (j == todayLevelIndex) {
                        $(m).addClass("singleHazeLevel_Selected");
                    }
                    else {
                        $(m).removeClass("singleHazeLevel_Selected");
                    }
                });
            }
            else if (n.id == "tomorrowPeriod") {
                periodIndex = 1;
                $.each($(".singleHazeLevel"), function (j, m) {
                    if (j == tomorrowLevelIndex) {
                        $(m).addClass("singleHazeLevel_Selected");
                    }
                    else {
                        $(m).removeClass("singleHazeLevel_Selected");
                    }
                });
            }
            else if (n.id == "afterPeriod") {
                periodIndex = 2;
                $.each($(".singleHazeLevel"), function (j, m) {
                    if (j == afterLevelIndex) {
                        $(m).addClass("singleHazeLevel_Selected");
                    }
                    else {
                        $(m).removeClass("singleHazeLevel_Selected");
                    }
                });
            }
        });

    });

    //霾等级切换
    $.each($(".singleHazeLevel"), function (i, n) {
        //时段为今天
        $(n).click(function () {
            $(n).addClass("singleHazeLevel_Selected");
            $.each($(".singleHazeLevel"), function (j, m) {
                if (m != n) {
                    $(m).removeClass("singleHazeLevel_Selected");
                }
            });
            if (i == 0) {
                $(".visibility").hide();

            }
            else {

                $(".visibility").show();
            }
            if (periodIndex == 0) {
                todayLevelIndex = i;
            }
            else if (periodIndex == 1) {
                tomorrowLevelIndex = i;
            }
            else if (periodIndex == 2) {
                afterLevelIndex = i;
            }
            hazeLevel = $(n).text();
            SetContent();
        });
    });

});

function SetContent() {
    var visibilityMin = $("#txtVisibilityMin").val();
    var content = "";
    var todayFullContent = "无霾";
    var tomorrowFullContent = "无霾";
    var afterFullContent = "无霾"
    if (periodIndex == 0) {
        todayContent = GetHazeLevelNumFormText(hazeLevel);

        if (hazeLevel == "无霾") {
            $($(".visTag")[periodIndex]).hide();
            $($(".hazeDis")[periodIndex]).hide();
            $($(".hazeDis")[periodIndex]).val("");
            $($(".km")[periodIndex]).hide();
            content = hazeLevel;           
        }
        else {
            $($(".visTag")[periodIndex]).show();
            $($(".hazeDis")[periodIndex]).show();
            $($(".km")[periodIndex]).show();
            content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
        }
    }
    else if (periodIndex == 1) {        
        tomorrowContent = GetHazeLevelNumFormText(hazeLevel);
        if (hazeLevel == "无霾") {
            $($(".visTag")[periodIndex]).hide();
            $($(".hazeDis")[periodIndex]).val("");
            $($(".hazeDis")[periodIndex]).hide();
            $($(".km")[periodIndex]).hide();
            content = hazeLevel;
        }
        else {
            $($(".visTag")[periodIndex]).show();
            $($(".hazeDis")[periodIndex]).show();
            $($(".km")[periodIndex]).show();
            content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
        };
    }
    else {       
        afterContent = GetHazeLevelNumFormText(hazeLevel);
        if (hazeLevel == "无霾") {
            $($(".visTag")[periodIndex]).hide();
            $($(".hazeDis")[periodIndex]).val("");
            $($(".hazeDis")[periodIndex]).hide();
            $($(".km")[periodIndex]).hide();
            content = hazeLevel;
        }
        else {
            $($(".visTag")[periodIndex]).show();
            $($(".hazeDis")[periodIndex]).show();
            $($(".km")[periodIndex]).show();
            content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
        }
    }

    var result = CreateTxtProductResult2(periodIndex, content);
    upLoadContent = result;
    //$("#hazeContent").text(result);
}

function SetContentCopy() {
    var visibilityMin = $("#txtVisibilityMin").val();
    var content = "";
    var todayFullContent = "无霾";
    var tomorrowFullContent = "无霾";
    var afterFullContent = "无霾"
    if (periodIndex == 0) {
        todayContent = GetHazeLevelNumFormText(hazeLevel);
        if (hazeLevel == "无霾") {
            content = hazeLevel;
        }
        else {
            content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
        }
    }
    else if (periodIndex == 1) {
        tomorrowContent = GetHazeLevelNumFormText(hazeLevel);
        if (hazeLevel == "无霾") {
            content = hazeLevel;
        }
        else {
            content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
        };
    }
    else {
        afterContent = GetHazeLevelNumFormText(hazeLevel);
        if (hazeLevel == "无霾") {
            content = hazeLevel;
        }
        else {
            content = hazeLevel + "，期间最小能见度在" + visibilityMin + "km";
        };
    }
    var result = CreateTxtProductResult2(periodIndex, content);
    $("#hazeContent").text(result);
}

//获取发布时间（（5:00,17：00））
function getReleaseTime() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var hour = date.getHours();
    
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
//    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
//            + " " + date.getHours() + seperator2 + date.getMinutes()
    //            + seperator2 + date.getSeconds();
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + "00" + seperator2 + "00"
            + seperator2 + "00";
    return currentdate;
}

//根据霾级别编码获取对应的级别文字
function GetHazeLevelTextFormNum(hazeNum) {
    if (hazeNum && hazeNum != "") {
        switch (hazeNum) {
            case "1":
                return "无霾";
            case "2":
                return "轻微霾";
            case "3":
                return "轻度霾";
            case "4":
                return "中度霾";
            case "5":
                return "重度霾";
            case "6":
                return "严重霾";
        }
    }
}

function GetHazeLevelNumFormText(hazeText) {
    if (hazeText && hazeText != "") {
        switch (hazeText) {
            case "无霾":
                return "1";
            case "轻微霾":
                return "2";
            case "轻度霾":
                return "3";
            case "中度霾":
                return "4";
            case "重度霾":
                return "5";
            case "严重霾":
                return "6";
        }
    }
}

//设置时段选中
function SetPeriodSelected(index) {
    if (index) {
        $.each($(".singlePeriod"), function (j, m) {
            if (j == index) {
                $(m).addClass("singlePeriod_Selected");
            }
            else {
                $(m).removeClass("singlePeriod_Selected");
            }
        });
    }
}

//设置无霾级别选中
function SetHazeLevelSelected(index) {
    if (index) {
        $.each($(".singleHazeLevel"), function (j, m) {
            if (j == index) {
                $(m).addClass("singleHazeLevel_Selected");
            }
            else {
                $(m).removeClass("singleHazeLevel_Selected");
            }
        });
    }
}

function changeDate(el) {
    var value = el.value;
    var nowDate = convertDate(value);
    var content = $("#hazeContentDate").html();
    var pubDateTime = $("#txtHidePubDateTime").text();
    pubDateTime = pubDateTime.replace("{TodayDate}{Time}", value);
    pubDateTime = pubDateTime.replace(" ", "");
    $("#hazeContentDate").html(pubDateTime+"发布");
}

//去掉日期当中月数字内的0
function TrimZeroInMonth(dateString) {
    var trimedDate = dateString;
    if (dateString != "") {
        var month = dateString.split('年')[1].split('月')[0];
        //月份以0开头
        if (month.indexOf('0') == 0) {
            trimedDate = trimedDate.replace(month, parseInt(month).toString());
        }
    }
    return trimedDate;
}

//获取霾的历史预报内容
//function GetHazeHistory() {
//    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
//    myMask.show();
//    Ext.Ajax.request({
//        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadHistoryHaze'),
//        success: function (response) {
//            myMask.hide();
//            if (response.responseText != "") {
//                var textContent = "";
//                textContent = $("#hazeContent").text();
//                var results = Ext.util.JSON.decode(response.responseText);
//                var hazeText = "";
//                var replaceText = "";
//                for (var i = 0; i < results.length; i++) {
//                    var foreLength = $(".tdDate").length;
//                    for (var j = 0; j < foreLength; j++) {
//                        if ($($(".tdDate")[j]).html() == results[i]["LST"]) {
//                            var disVis = results[i]["Vis"];
//                            if (disVis == "null") {
//                                disVis = "";
//                            }
//                            $($(".tdHaze")[j]).children().val(GetHazeLevelTextFormNum(results[i]["Haze"]));
//                            $($(".tdVis")[j]).children().val(disVis);
//                        }
//                    }
//                }

//            }
//            else {
//                alert("获取历史记录失败！");
//            }
//        },
//        failure: function (response) {
//            myMask.hide();
//            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
//        }
//    });
//}

function GetHazeHistory() {
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadHistoryHaze'),
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                var hazeText = "";
                for (var obj in result) {
                    switch (result[obj].split('_')[0]) {
//                        case "1":
//                            hazeText = "无霾";
//                            break;
//                        case "2":
//                            hazeText = "轻微霾";
//                            break;
//                        case "3":
//                            hazeText = "轻度霾";
//                            break;
//                        case "4":
//                            hazeText = "中度霾";
//                            break;
//                        case "5":
//                            hazeText = "重度霾";
//                            break;
//                        case "6":
//                            hazeText = "严重霾";
                        //                            break; 
                        case "1":
                            hazeText = "无霾";
                            break;
                        case "2":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "3":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "4":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "5":
                            hazeText = result[obj].split('_')[2];
                            break;
                        case "6":
                            hazeText = result[obj].split('_')[2];
                            break;
                    }
                    if (obj == "Today") {
                        $("#todayLevel").val(hazeText);
                        todayContent = result[obj].split('_')[0];
                        if (result[obj].split('_')[1] != "") {
                            $($(".visTag")[0]).show();
                            $($(".hazeDis")[0]).show();
                            $($(".km")[0]).show();
                            $("#visToday").val(result[obj].split('_')[1]);
                            todayVis = result[obj].split('_')[1];
                        }
                    }
                    else if (obj == "Tomorrow") {
                        $("#tomorrowLevel").val(hazeText);
                        tomorrowContent = result[obj].split('_')[0];
                        if (result[obj].split('_')[1] != "") {
                            $($(".visTag")[1]).show();
                            $($(".hazeDis")[1]).show();
                            $($(".km")[1]).show();
                            $("#visTom").val(result[obj].split('_')[1]);
                            tomorrowVis = result[obj].split('_')[1];
                        }
                    }
                    else if (obj == "After") {
                        $("#afterLevel").val(hazeText);
                        afterContent = result[obj].split('_')[0];
                        if (result[obj].split('_')[1] != "") {
                            $($(".visTag")[2]).show();
                            $($(".hazeDis")[2]).show();
                            $($(".km")[2]).show();
                            $("#visAfter").val(result[obj].split('_')[1]);
                            afterVis = result[obj].split('_')[1];
                        }
                    }
                    else if (obj == "Haze_24") {
                        $("#58367_Item").text(GetHazeLevelTextFormNum(result[obj]));
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

//获取页面上要发布的霾内容
function GetPubContent() {
    var content = $("#txtHideTempleteContent05").text();
    content = content.replace("{PubDateContent}", $("#hazeContentDate").html());
    var todayContent = ($("#todayLevel").val() == "无霾") ? $("#todayLevel").val() : $("#todayLevel").val() + "，期间最小能见度在" + $("#visToday").val() + "km";
    var tomorrowContent = ($("#tomorrowLevel").val() == "无霾") ? $("#tomorrowLevel").val() : $("#tomorrowLevel").val() + "，期间最小能见度在" + $("#visTom").val() + "km";
    var afterContent = ($("#afterLevel").val() == "无霾") ? $("#afterLevel").val() : $("#afterLevel").val() + "，期间最小能见度在" + $("#visAfter").val() + "km";
    content = content.replace("{TodayContent}", todayContent);
    content = content.replace("{TomorrowContent}", tomorrowContent);
    content = content.replace("{AfterDayContent}", afterContent);
    return content;
}