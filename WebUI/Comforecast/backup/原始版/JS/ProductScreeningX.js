Ext.onReady(function () {
    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#outLine").width($(window).width() - 40);
    $("#outLine").height($(window).height() - 40);

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AllCityForecast', 'ReadStateTable'),
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
           
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
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    $.each($(".displayItem"), function (i, n) {
        $.each($(n).find(".condition"), function (j, m) {
            $(m).click(function () {
                var urlName = "AllCityForecast";
         
                window.location.href = "../Comforecast/" + urlName + ".aspx";
            });
        });
    });

});
