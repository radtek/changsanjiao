Ext.onReady(function () {
    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#outLine").width($(window).width() - 40);
    $("#outLine").height($(window).height() - 40);

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'ReadStateTable'),
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    //一天发送两次的
                    if (obj.indexOf("05") > 0 || obj.indexOf("07") > 0 || obj.indexOf("17") > 0) {
                        var conditionDiv = $("#"+obj);
                        if (result[obj] == "3") {
                            conditionDiv.text("已完成");
                            className = "success";
                        }
                        else if (result[obj] == "1") {
                            conditionDiv.text("待审核");
                            className = "saved";
                        }
                        else if (result[obj] == "2") {
                            conditionDiv.text("待发布");
                            className = "update";
                        }
                        else if (result[obj] == "4") {
                            conditionDiv.text("未完全");
                            className = "pub_notCompleted";
                        }
                        else if (result[obj] == "0") {
                            conditionDiv.text("待完成");
                            className = "undone";
                        }
                        conditionDiv.removeClass();
                        conditionDiv.addClass("condition " + className);
                    }
                    //一天只发送一次的
                    else {
                        var conditionDiv = $($("#" + obj).find(".condition")[0]);
                        if (result[obj] == "3") {
                            conditionDiv.text("已完成");
                            className = "success";
                        }
                        else if (result[obj] == "1") {
                            conditionDiv.text("待审核");
                            className = "saved";
                        }
                        else if (result[obj] == "2") {
                            conditionDiv.text("待发布");
                            className = "update";
                        }
                        else if (result[obj] == "4") {
                            conditionDiv.text("未完全");
                            className = "pub_notCompleted";
                        }
                        else if (result[obj] == "0") {
                            conditionDiv.text("待完成");
                            className = "undone";
                        }

                        conditionDiv.removeClass();
                        conditionDiv.addClass("condition " + className);
                    }
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    $.each($(".displayItem"), function (i, n) {
        $.each($(n).find(".condition"), function (j, m) {
            $(m).click(function () {
                var urlName = "";
                switch (n.id) {
                    case "AQIArea":
                        urlName = "AQIAreaWithMap";
                        break;
                    case "AQIAreaForeFile":
                        urlName = "AQIAreaWithMap";
                        break;
                    case "AQIPeriod48":
                        urlName = "AQIPeriod48";
                        break;
                    case "OzoneForecast":
                        urlName = "ThreeGeneralReport";
                        break;
                    case "HazeForecast":
                        urlName = "ThreeGeneralReport";
                        break;
                    case "UVForecast":
                        urlName = "ThreeGeneralReport";
                        break;
                    case "AirPollutionForecast":
                        urlName = "AirPollutionForecast";
                        break;
                    case "guideReportFile":
                        urlName = "AQIDropZone";
                        break;
                    case "AQIDropZone":
                        urlName = "AQIDropZone";
                        break;
                    case "HazeDropZone":
                        urlName = "HazeDropZone";
                        break;
                    case "AirPollutionDropZone":
                        urlName = "HazeDropZone";
                        break;
                    case "eastChinaMainCity":
                        urlName = "MainCityForecast";
                        break;
                }
                window.location.href = "../ReportProduce/" + urlName + ".aspx";
            });
        });
    });

});
