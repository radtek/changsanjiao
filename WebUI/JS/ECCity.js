var oldID = "L04";
var json = "";
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    oldID = T;
    CreateHtml(oldID);
    if (id == "Integ") {
        $("#replaceImg").show();
        $("#replaceImg").live('click', function () {
            //替换当前图片
            if ($($("#imgHtml").find('img')[0]).attr('src') != "") {
                var imgPath = $($("#imgHtml").find('img')[0]).attr('src');
                //imgPath = imgPath.replace("F:/EMFCDatabase", "../Product");
                var versionInfo = imgPath.substring(imgPath.indexOf("?"));
                imgPath = imgPath.substring(imgPath.indexOf('integ'));
                imgPath = (imgPath.contains('V')) ? imgPath.substring(0, imgPath.indexOf("V") - 1) : imgPath;
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpdateChemistryImg'),
                    params: { imgFileName: imgPath },
                    success: function (response) {
                        var newPath = response.responseText;
                        if (response.responseText != "") {
                            newPath = newPath.replace("F:/EMFCDatabase", "Product");
                            alert("替换成功!");
                        }

                        $($("#imgHtml").find('img')[0]).attr('src', newPath);
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }
        });
    }
})
function CreateHtml(nowId) {
    var type = nowId.substr(1, 2);
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetEntity'),
        params: { entityName: id },
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            var entity = {
                align: "U",
                alias: "",
                realtime: period,
                name: ""
            }
            var selectTime = Ext.getDom("selectTime");
            selectTime.innerHTML = "";
            if (width > 0) {
                json = "{N:1;C:" + column + ";R:" + totalCount + ";S:" + type + ";P:" + period + ";w:" + width + "}";
            }
            else {
                json = "{N:1;C:" + column + ";R:" + totalCount + ";S:" + type + ";P:" + period + "}";
            }

            var ImageFrameEntity = new ImageFrame(result, entity, json);
            
            ImageFrameEntity.render("selectTime");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
        //    Ext.Ajax.request({
        //        url: getUrl('MMShareBLL.DAL.Forecast', 'ECCity'),
        //        params: { entityName: id, type: type, column: column, totalCount: totalCount, period: period },
        //        success: function (response) {
        //            if (response.responseText != "") {
        //                var result = Ext.util.JSON.decode(response.responseText);
        //                changeDateSucessed(result);
        //            }
        //        },
        //        failure: function (response) {
        //            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        //        }
    });
}
function ReduceSelect(day) {
    var selectIndex = $("#selectID").get(0).selectedIndex;
    $("#selectID").get(0).selectedIndex = selectIndex + day;
    $("#selectID").trigger("change");


}
function CityChange(nowId) {
    if (nowId != oldID) {
        var oldDom = Ext.getDom(oldID);
        var newDom = Ext.getDom(nowId);
        newDom.className = "foucs1";
        oldDom.className = "line";
        oldID = nowId;
        CreateHtml(nowId);
    }
}
function trickQueryList(Datetime) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'trickQueryListII'),
        params: { Datetime: Datetime, entityName: id, json: json },
        success: function (response) {
            if (response.responseText != "") {
                var contentNone = Ext.getDom("contentNone");
                contentNone.innerHTML = response.responseText;
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function showOne(entityName, type, src, time, type, Period) {
    var dateTime = new Date(Date.parse(time.replace(/-/g, "/"))); //转换成Data();
    var date = dateTime.format("Y-m-d");
    Ext.getDom("OnlyOne").innerHTML = "";
    var hour = dateTime.getHours();
    var time = Ext.getDom("time").value = date;
    Ext.getDom("type").innerHTML = type;
    imageViewer = new ImageViewer(Ext.BLANK_IMAGE_URL, id, "");
    imageViewer.render("OnlyOne");
    imageViewer.setImageSrc(src, id, "");

    var showImg = Ext.getDom("showImg");
    showImg.className = "show1";

    var left = ($("#bg").width() - $("#showImg").width()) / 2;
    $("#showImg").css({ left: left, right: left });
    $("#view-image").css({marginTop:"50px",marginBottom:"50px"});
    $('.bg').fadeIn(200);
    $('#showImg').fadeIn(400);
    var type = oldID.substr(1, 2);
    json = "{N:;C:" + column + ";R:" + totalCount + ";S:" + type + ";P:}";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'CreateBottomSelectII'),
        params: { Datetime: dateTime, entityName: id, json: json, type: type, Period: Period,src:src },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}

//闫海涛修改
//$('#showImg').live('click',function () {
//    $('.bg').fadeOut(800);
//    $('#showImg').fadeOut(800);   
//});

$('#bg').live('click', function () { 
    $('.bg').fadeOut(800);
    $('#showImg').fadeOut(800);
});

//闫海涛修改，添加图片关闭按钮
$("#closeBtn").live('click', function () {
    $('.bg').fadeOut(800);
    $('#showImg').fadeOut(800);
});       

function fadeOut() {
    var showImg = Ext.getDom("showImg");
    showImg.className = "hidden";
    $('.bg').fadeOut(800);
    $('#showImg').fadeOut(800);
}
function ReduceButton() {
    var dateTime = Ext.getDom("time").value;
    var selectIndex = $("#selectHour").get(0).selectedIndex;
    if (selectIndex != 0) {
        $("#selectHour").get(0).selectedIndex = selectIndex - 1;
        hour = $('#selectHour option:selected').val();
    }
    else
        hour = "-1";
    QueryImg(dateTime, hour);

}
function addButton() {
    var dateTime = Ext.getDom("time").value;
    var selectIndex = $("#selectHour").get(0).selectedIndex;
    var length = $('#selectHour option').length - 1;
    if (selectIndex != length) {
        $("#selectHour").get(0).selectedIndex = selectIndex + 1;
        hour = $('#selectHour option:selected').val();
    }
    else
        hour = "-2";
    QueryImg(dateTime, hour);
}
function changeDate(el) {
    var dateTime = el.value;
    var hour = $('#selectHour option:selected').val();
    QueryImg(dateTime, hour);
}
function selectChange() {
    var dateTime = Ext.getDom("time").value;
    var hour = $('#selectHour option:selected').val();
    QueryImg(dateTime, hour);
}
function QueryImg(dateTime, hour) {
    var type = Ext.getDom("type").innerHTML;
    var period = $('#selectperiod option:selected').val();
    if (period == "" || period == null)
        period = "";

    var src = $("#view-image")[0].src;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'ReduceButtonII'),
        params: { entityName: id, dateTime: dateTime, hour: hour, json: json, type: type, period: period,src:src },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
//改变日期成功后,，刷新获取的值
function changeDateSucessed(result) {
    for (var obj in result) {
        if (obj == "src") {
            imageViewer.setImageSrc(result[obj], id, "");
        }
        else {
            divContaner = Ext.getDom(obj);
            if (obj == "period")
                divContaner.className = "hourBut";
            if (divContaner != null) {
                if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                    divContaner.value = result[obj];
                else {
                    if (result[obj] == "")
                        divContaner.innerHTML = "\\"; //日平均值
                    else
                        divContaner.innerHTML = result[obj]; //日平均值


                }
            }
        }
    }
}