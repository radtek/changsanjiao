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
     //    $("#PO_docDate").val(getFormatDate("") + "17时");
    var curDate = new Date();
    $("#PO_docDate").val(curDate.getFullYear() + "年" + (curDate.getMonth() + 1) + "月" + curDate.getDate() + "日" + "17时");
    $("#PO_year").val(new Date().getFullYear());

    //setTableDates();

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetFutureTenDaysData'),
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                //                for (var obj in result) {
                //                    $("#" + obj).val(result[obj])
                //                }
                for (var obj in result) {
                    //                    if ($("#" + obj).get(0).tagName == "INPUT" || $("#" + obj).get(0).tagName == "input") {
                    if ($("#" + obj).is("INPUT") || $("#" + obj).is("input") || $("#" + obj).is("textarea") || $("#" + obj).is("TEXTAREA")) {
                        $("#" + obj).val(result[obj])
                    }
                    //else if ($("#" + obj).get(0).tagName == "SPAN" || $("#" + obj).get(0).tagName == "span") {
                    else if ($("#" + obj).is("SPAN") || $("#" + obj).is("span")) {
                        $("#" + obj).text(result[obj])
                    }
                }
            }
            else {
                setTableDates();
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    //自动获取数据
    $("#readForeData").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetFutureTenDaysData'),
            success: function (response) {
                if (response.responseText != "") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    //                for (var obj in result) {
                    //                    $("#" + obj).val(result[obj])
                    //                }
                    for (var obj in result) {
                        if ($("#" + obj).get(0).tagName == "INPUT" || $("#" + obj).get(0).tagName == "input") {
                            $("#" + obj).val(result[obj])
                        }
                        else if ($("#" + obj).get(0).tagName == "SPAN" || $("#" + obj).get(0).tagName == "span") {
                            $("#" + obj).text(result[obj])
                        }
                    }
                }
                else {
                    setTableDates();
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //保存
    $("#foreSave").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveFutureTenDaysWord'),
            params: { wordTempContent: getWordContent(), productName: "FutureTenDays" },
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
            params: { productName: "FutureTenDays", searchDate: searchDate },
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
            params: { productName: "FutureTenDays" },
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
        var issueNum = $("#PO_issueNum").val();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'PublishFutureTenDaysWord'),
            params: { ftpString: ftpString, functionName: "FutureTenDays", issueNum: issueNum, userName: userName },
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
            params: { productName: "FutureTenDays" },
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
        //期数设置
        $("#issueSet").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_issueNum").val($("#issueSet").val());
        });

        //两个表格内容的联动

        $("#PO_PoLevel1").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao1").val($("#PO_PoLevel1").val());
            $("#PO_Jinshan1").val($("#PO_PoLevel1").val());
            $("#PO_Fenxian1").val($("#PO_PoLevel1").val());
            $("#PO_Baogang1").val($("#PO_PoLevel1").val());
            $("#PO_Minhang1").val($("#PO_PoLevel1").val());
        });

        $("#PO_PoLevel2").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao2").val($("#PO_PoLevel2").val());
            $("#PO_Jinshan2").val($("#PO_PoLevel2").val());
            $("#PO_Fenxian2").val($("#PO_PoLevel2").val());
            $("#PO_Baogang2").val($("#PO_PoLevel2").val());
            $("#PO_Minhang2").val($("#PO_PoLevel2").val());
        });

        $("#PO_PoLevel3").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao3").val($("#PO_PoLevel3").val());
            $("#PO_Jinshan3").val($("#PO_PoLevel3").val());
            $("#PO_Fenxian3").val($("#PO_PoLevel3").val());
            $("#PO_Baogang3").val($("#PO_PoLevel3").val());
            $("#PO_Minhang3").val($("#PO_PoLevel3").val());
        });

        $("#PO_PoLevel4").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao4").val($("#PO_PoLevel4").val());
            $("#PO_Jinshan4").val($("#PO_PoLevel4").val());
            $("#PO_Fenxian4").val($("#PO_PoLevel4").val());
            $("#PO_Baogang4").val($("#PO_PoLevel4").val());
            $("#PO_Minhang4").val($("#PO_PoLevel4").val());
        });

        $("#PO_PoLevel5").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao5").val($("#PO_PoLevel5").val());
            $("#PO_Jinshan5").val($("#PO_PoLevel5").val());
            $("#PO_Fenxian5").val($("#PO_PoLevel5").val());
            $("#PO_Baogang5").val($("#PO_PoLevel5").val());
            $("#PO_Minhang5").val($("#PO_PoLevel5").val());
        });

        $("#PO_PoLevel6").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao6").val($("#PO_PoLevel6").val());
            $("#PO_Jinshan6").val($("#PO_PoLevel6").val());
            $("#PO_Fenxian6").val($("#PO_PoLevel6").val());
            $("#PO_Baogang6").val($("#PO_PoLevel6").val());
            $("#PO_Minhang6").val($("#PO_PoLevel6").val());
        });

        $("#PO_PoLevel7").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_Gaoqiao7").val($("#PO_PoLevel7").val());
            $("#PO_Jinshan7").val($("#PO_PoLevel7").val());
            $("#PO_Fenxian7").val($("#PO_PoLevel7").val());
            $("#PO_Baogang7").val($("#PO_PoLevel7").val());
            $("#PO_Minhang7").val($("#PO_PoLevel7").val());
        });

    }
    else {

        $("#issueSet").get(0).addEventListener("input", function (o) {
            $("#PO_issueNum").val($("#issueSet").val());
        }, false);

        //两个表格内容的联动
        $("#PO_PoLevel1").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao1").val($("#PO_PoLevel1").val());
            $("#PO_Jinshan1").val($("#PO_PoLevel1").val());
            $("#PO_Fenxian1").val($("#PO_PoLevel1").val());
            $("#PO_Baogang1").val($("#PO_PoLevel1").val());
            $("#PO_Minhang1").val($("#PO_PoLevel1").val());
        }, false);

        $("#PO_PoLevel2").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao2").val($("#PO_PoLevel2").val());
            $("#PO_Jinshan2").val($("#PO_PoLevel2").val());
            $("#PO_Fenxian2").val($("#PO_PoLevel2").val());
            $("#PO_Baogang2").val($("#PO_PoLevel2").val());
            $("#PO_Minhang2").val($("#PO_PoLevel2").val());
        }, false);

        $("#PO_PoLevel3").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao3").val($("#PO_PoLevel3").val());
            $("#PO_Jinshan3").val($("#PO_PoLevel3").val());
            $("#PO_Fenxian3").val($("#PO_PoLevel3").val());
            $("#PO_Baogang3").val($("#PO_PoLevel3").val());
            $("#PO_Minhang3").val($("#PO_PoLevel3").val());
        }, false);

        $("#PO_PoLevel4").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao4").val($("#PO_PoLevel4").val());
            $("#PO_Jinshan4").val($("#PO_PoLevel4").val());
            $("#PO_Fenxian4").val($("#PO_PoLevel4").val());
            $("#PO_Baogang4").val($("#PO_PoLevel4").val());
            $("#PO_Minhang4").val($("#PO_PoLevel4").val());
        }, false);

        $("#PO_PoLevel5").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao5").val($("#PO_PoLevel5").val());
            $("#PO_Jinshan5").val($("#PO_PoLevel5").val());
            $("#PO_Fenxian5").val($("#PO_PoLevel5").val());
            $("#PO_Baogang5").val($("#PO_PoLevel5").val());
            $("#PO_Minhang5").val($("#PO_PoLevel5").val());
        }, false);

        $("#PO_PoLevel6").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao6").val($("#PO_PoLevel6").val());
            $("#PO_Jinshan6").val($("#PO_PoLevel6").val());
            $("#PO_Fenxian6").val($("#PO_PoLevel6").val());
            $("#PO_Baogang6").val($("#PO_PoLevel6").val());
            $("#PO_Minhang6").val($("#PO_PoLevel6").val());
        }, false);

        $("#PO_PoLevel7").get(0).addEventListener("input", function (o) {
            $("#PO_Gaoqiao7").val($("#PO_PoLevel7").val());
            $("#PO_Jinshan7").val($("#PO_PoLevel7").val());
            $("#PO_Fenxian7").val($("#PO_PoLevel7").val());
            $("#PO_Baogang7").val($("#PO_PoLevel7").val());
            $("#PO_Minhang7").val($("#PO_PoLevel7").val());
        }, false);

    }

});

//将页面上的内容传给PageOffice Word预览页面
function prepare() {
    var pageContent = "";
    
    pageContent += "PO_year" + "=" + $("#PO_year").val() + "&";
    pageContent += "PO_issueNum" + "=" + $("#PO_issueNum").val() + "&";
    pageContent += "PO_docDate" + "=" + $("#PO_docDate").val() + "&";
    pageContent += "PO_Signer" + "=" + $("#PO_Signer").val() + "&";
    pageContent += "PO_TenDaysAirFore" + "=" + $("#PO_TenDaysAirFore").val() + "&";


    for (var i = 1; i < 11; i++) {
        pageContent += "PO_Date" +i.toString()+ "=" + $("#PO_Date"+i.toString()).text() + "&";
        pageContent += "PO_PoLevel" + i.toString() + "=" + $("#PO_PoLevel" + i.toString()).val() + "&";
        pageContent += "PO_FirstItem" + i.toString() + "=" + $("#PO_FirstItem" + i.toString()).val() + "&";
        pageContent += "PO_Weather" + i.toString() + "=" + $("#PO_Weather" + i.toString()).val() + "&";
        pageContent += "PO_WindSpeed" + i.toString() + "=" + $("#PO_WindSpeed" + i.toString()).val() + "&";
    }

    for (var j = 1; j < 8; j++) {
        pageContent += "PO_DateSeven" + j.toString() + "=" + $("#PO_DateSeven" + j.toString()).text() + "&";
        pageContent += "PO_Gaoqiao" + j.toString() + "=" + $("#PO_Gaoqiao" + j.toString()).val() + "&";
        pageContent += "PO_Jinshan" + j.toString() + "=" + $("#PO_Jinshan" + j.toString()).val() + "&";
        pageContent += "PO_Fenxian" + j.toString() + "=" + $("#PO_Fenxian" + j.toString()).val() + "&";
        pageContent += "PO_Baogang" + j.toString() + "=" + $("#PO_Baogang" + j.toString()).val() + "&";
        pageContent += "PO_Minhang" + j.toString() + "=" + $("#PO_Minhang" + j.toString()).val() + "&";
    }

    pageContent += "PO_FutureThreDays" + "=" + $("#PO_FutureThreDays").val() + "&";
    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_reporter" + "=" + $("#PO_reporter").val() ;
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
function setTableDatesCopy() {
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

function setTableDates() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'SetFutureTenDaysDate'),
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                //                for (var obj in result) {
                //                    $("#" + obj).val(result[obj])
                //                }
                for (var obj in result) {
                    if ($("#" + obj).get(0).tagName == "INPUT" || $("#" + obj).get(0).tagName == "input") {
                        $("#" + obj).val(result[obj])
                    }
                    else if ($("#" + obj).get(0).tagName == "SPAN" || $("#" + obj).get(0).tagName == "span") {
                        $("#" + obj).text(result[obj])
                    }
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function getWordContent(){
   var pageContent = "";
    
    pageContent += "PO_year" + "=" + $("#PO_year").val() + "&";
    pageContent += "PO_issueNum" + "=" + $("#PO_issueNum").val() + "&";
    pageContent += "PO_docDate" + "=" + $("#PO_docDate").val() + "&";
    pageContent += "PO_Signer" + "=" + $("#PO_Signer").val() + "&";
    pageContent += "PO_TenDaysAirFore" + "=" + $("#PO_TenDaysAirFore").val().replace(/(^\s*)|(\s*$)/g, "") + "&";


    for (var i = 1; i < 11; i++) {
        pageContent += "PO_Date" +i.toString()+ "=" + $("#PO_Date"+i.toString()).text() + "&";
        pageContent += "PO_PoLevel" + i.toString() + "=" + $("#PO_PoLevel" + i.toString()).val() + "&";
        pageContent += "PO_FirstItem" + i.toString() + "=" + $("#PO_FirstItem" + i.toString()).val() + "&";
        pageContent += "PO_Weather" + i.toString() + "=" + $("#PO_Weather" + i.toString()).val() + "&";
        pageContent += "PO_WindSpeed" + i.toString() + "=" + $("#PO_WindSpeed" + i.toString()).val() + "&";
    }

    for (var j = 1; j < 8; j++) {
        pageContent += "PO_DateSeven" + j.toString() + "=" + $("#PO_DateSeven" + j.toString()).text() + "&";
        pageContent += "PO_Gaoqiao" + j.toString() + "=" + $("#PO_Gaoqiao" + j.toString()).val() + "&";
        pageContent += "PO_Jinshan" + j.toString() + "=" + $("#PO_Jinshan" + j.toString()).val() + "&";
        pageContent += "PO_Fenxian" + j.toString() + "=" + $("#PO_Fenxian" + j.toString()).val() + "&";
        pageContent += "PO_Baogang" + j.toString() + "=" + $("#PO_Baogang" + j.toString()).val() + "&";
        pageContent += "PO_Minhang" + j.toString() + "=" + $("#PO_Minhang" + j.toString()).val() + "&";
    }

    pageContent += "PO_FutureThreDays" + "=" + $("#PO_FutureThreDays").val().replace(/(^\s*)|(\s*$)/g, "") +"&";
    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_reporter" + "=" + $("#PO_reporter").val() ;
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
    $("#closePreview").css({right: left-32 });
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

//function changeDate(el) {
//    var value = el.value;
//    var nowDate = convertDate(value);
//    var content = $("#hazeContentDate").html();
//    var pubDateTime = $("#txtHidePubDateTime").text();
//    pubDateTime = pubDateTime.replace("{TodayDate}{Time}", value);
//    pubDateTime = pubDateTime.replace(" ", "");
//    $("#hazeContentDate").html(pubDateTime + "发布");
//}
//预报时间改变是的事件
function changeDate(el) {
//    var value = el.value;
//    $("#docDate").text(value);
}
