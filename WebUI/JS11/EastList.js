var oldID = "L0";
Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    CreateHtml();
})
function CreateHtml() {
    var temp = json.substring(0, json.length - 1);
    var period = temp.split(':');
    var realTime = period[5].split(';')[0];
    var type = period[4].split(';')[0];
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetEntityType'),
        params: { entityName: id, type: type },
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            var entity = {
                align: "U",
                alias: "",
                realtime: realTime,
                name: ""
            }
            var selectTime = Ext.getDom("selectTime");
            selectTime.innerHTML = "";
            var ImageFrameEntity = new ImageFrame(result, entity, json);
            ImageFrameEntity.render("selectTime");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function ReduceSelect(day) {
    var selectIndex = $("#selectID").get(0).selectedIndex;
    $("#selectID").get(0).selectedIndex = selectIndex + day;
    $("#selectID").trigger("change");


}
function TypeChange(newjJson, nowId) {
    if (nowId != oldID) {
        var oldDom = Ext.getDom(oldID);
        var newDom = Ext.getDom(nowId);
        newDom.className = "foucs";
        oldDom.className = "line";
        json = newjJson;
        oldID = nowId;
        CreateHtml();
    }
}
function trickQueryList(Datetime) {
    Ext.Ajax.request({
        //        url: getUrl('MMShareBLL.DAL.Forecast', 'trickQueryListII'),
        url: getUrl('MMShareBLL.DAL.Forecast', 'trickQueryListMM'),
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
//    imageViewer = new ImageViewer(src, id, "");


    var showImg = Ext.getDom("showImg");
    showImg.className = "show1";

    var left = ($("#bg").width() - $("#showImg").width()) / 2;
    $("#showImg").css({ left: left, right: left });
    $("#view-image").css({marginTop:"50px",marginBottom:"50px"});
    $('.bg').fadeIn(200);
    $('#showImg').fadeIn(400);
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