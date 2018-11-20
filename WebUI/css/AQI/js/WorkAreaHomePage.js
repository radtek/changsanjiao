var win;
var warnInfoList = new Array();
Ext.onReady(function () {
    //显示预报员，预报时间和时次
    //设置界面宽度
    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;

    $("body").css("min-width", $(window).width() + "px");
    
    $(".contenttitle_Top").height((pageHeight - 98) * 0.3);
    $(".contenttitle_Bottom").height((pageHeight - 98) * 0.9);
    GetNotice(10);
    GetWarningInfo();

    if (!win) {//如果不存在win对象择创建
        win = new Ext.Window({
            title: '预警信息',
            width: 700,
            height: 400,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            items: new Ext.Panel({//窗体中中是一个一个TabPanel
                autoTabs: true,
                id: "warnInfoPanel",
                activeTab: 0,
                deferredRender: false,
                border: false,
                buttonAlign: "center",
                html: "<div id='warnDetail' style='width:100%;height:100%;'></div>"
            }),
            buttons: [
                    {
                        text: '关闭',                        
                        handler: function () {//点击时触发的事件
                            win.hide();
                        }
                    }
                    ]
        });
    }
    
});
    

function noticeScoll() {
    var obj = document.getElementById("Logcontent");
//    if (obj.scrollHeight > obj.clientHeight || obj.offsetHeight > obj.clientHeight) {
        var scrtime;
        $("#Logcontent").hover(function () {
            clearInterval(scrtime);

        }, function () {

            scrtime = setInterval(function () {
                var $ul = $("#Logcontent ul");
                var liHeight = $ul.find("li:last").height(); //行高
                var h = $("#Logcontent").height() - liHeight;
                //var liHeight2 = $ul.find("li").eq(-2).height();
                //var difference = liHeight2 - liHeight;
                // alert(difference);
                var _marginTop = liHeight + 30 + "px";
                //var _marginTop = liHeight + 40 + "px";
                $ul.animate({ marginTop: _marginTop }, 1000, function () {

                    $ul.find("li:last").prependTo($ul)
                    $ul.find("li:first").hide();
                    $ul.css({ marginTop: 0 });
                    $ul.find("li:first").fadeIn(1000);
                });
            }, 3000);

        }).trigger("mouseleave");
//    }
}

//重要通知
function GetNotice(top) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetImportantNoticeDataForHomePage'),
        params: { top: "10" },
        success: function (response) {
            if (response.responseText != "") {
                noticeScoll();
                var result = Ext.util.JSON.decode(response.responseText);
                if (result.length > 0) {
                    var ulHtml = "<ul>";
                    for (var i = 0; i < result.length; i++) {
                        ulHtml += "<li><p><span class='span_worklog'>[" + result[i]['Type'] + "]</span><span class='span_worklog'>" + result[i]['EndTime'] + "</span><br/>" + result[i]['Content'] + "</p></li>"
                    }
                    ulHtml += "</ul>";
                    $("#Logcontent").html(ulHtml);
                }
            }
            else {

            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

//预警通知的显示
function GetWarningInfo() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetWarningInfoForHomePage'),
        params: { top: "5" },
        success: function (response) {
            if (response.responseText != "") {
                //noticeScoll();
                var result = Ext.util.JSON.decode(response.responseText);
                if (result.length > 0) {
                    $("#div_AlarmProduct").css({ "background-color": 'White' });
                    var ulHtml = "<ul class='warnUL'>";
                    var warnType = "";
                    var warnLevel = "";
                    for (var i = 0; i < result.length; i++) {
                        switch (result[i]["warnType"]) {
                            case "大雾":
                                warnType = "dawu";
                                break;
                            case "大风":
                                warnType = "dafeng";
                                break;
                            case "暴雨":
                                warnType = "baoyu";
                                break;
                            case "冰雹":
                                warnType = "bingbao";
                                break;
                            case "雷电":
                                warnType = "leidian";
                                break;
                            case "霾":
                                warnType = "mai";
                                break;
                            default:
                                warnType = "none";
                                break;
                        }
                        switch (result[i]["warnLevel"]) {
                            case "黄色":
                                warnLevel = "y";
                                break;
                            case "蓝色":
                                warnLevel = "b";
                                break;
                            case "橙色":
                                warnLevel = "o";
                                break;
                            case "红色":
                                warnLevel = "r";
                                break;
                            default:
                                warnLevel = "b";
                                break;
                        }
                        ulHtml += "<li><div class='imgAndTitle'><div class='imgDiv'><img class='warnIcon' src='../css/images/warningIcon/" + warnType + "-" + warnLevel + ".png" + "'/></div><div class='warnTitle'>" + result[i]["warnTitle"] + "</div>" + "<div class='warnTime'>" + result[i]["pubTime"] + "</div></div></li>";
                        warnInfoList.push("<div class='detailTop' ><div><img class='warnIconBig' src='../css/images/warningIcon/" + warnType + "-" + warnLevel + ".png" + "'/><div class='warnTitle_Big'>" + result[i]["warnTitle"] + "</div></div></div><div class='detailBottom'>" + result[i]["content"] + "</div>");
                       
                    }
                    ulHtml += "</ul>";
                    $("#div_AlarmProduct").html(ulHtml);
                    $.each($(".warnTitle"), function (j, n) {
                        $(n).live('click', function () {
                            win.show();
                            $("#warnDetail").html(warnInfoList[j]);
                        });
                    });
                }
            }
            else {

            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}