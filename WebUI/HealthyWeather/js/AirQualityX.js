var oldID = "L0";
var entityNameNow = "";
Ext.onReady(function () {
    initInputHighlightScript();
    supportInnerText(); //使得火狐支持innerText
    //CreateHtml();

    //绑定时间选择下拉菜单的事件
    $("#selectID").live('click', function () {
        $("#dateUl").toggleClass("display");
    });


    $("#dateUl li").live('click', function () {
        $("#dateTxt").html(this.innerHTML);
        $("#dateUl").toggleClass("display");
        $("#dateUl").toggleClass("hide");
        trickQueryList(this.innerHTML);
    });

    ReduceSelect(0);
})
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

//function ReduceSelect(day) {
//    var dates = $("#dateUl li");
//    var selectIndex = $("#selectID").get(0).selectedIndex;
//    $("#selectID").get(0).selectedIndex = selectIndex + day;
//    $("#selectID").trigger("change");
//}

function ReduceSelect(day) {
    var el = Ext.getDom("H00");
    var oldDate = el.value.toString();
    var startDate = convertDate(oldDate);
    var nextDate = startDate.add('d',day);
    var date = nextDate.format("Y-m-d");
    el.value = date;
    trickQueryList(el.value);
}

function convertDates(date) {
    var flag = true;

    if (date.indexOf('年') > 0)
        date = date.replace('年', '-');
    if (date.indexOf('月') > 0)
        date = date.replace('月', '-');
    if (date.indexOf('日') > 0)
        date = date.replace('日', '');

    var dateParts = date.split(" ");
    var dateArray = dateParts[0].split("-");
    if (dateArray.length != 3) {
        dateArray = date.split("/");
        if (dateArray.length != 3) {
            return null;
        }
        flag = false;
    }
    var newDate = new Date();

    var y = newDate.getFullYear();
    var m = newDate.getMonth() + 1;
        m = m < 10 ? '0' + m : m;
        var d = newDate.getDate();
        d = d < 10 ? ('0' + d) : d;

    newDate = (y + '-' + m + '-' + d) ;
    if (flag) {
        // month从0开始
        newDate.setFullYear(dateArray[0], dateArray[1] - 1, dateArray[2]);
    }
    else {
        newDate.setFullYear(dateArray[2], dateArray[1] - 1, dateArray[0]);
    }
    if (dateParts.lenght > 1) {
        var times = dateParts[1].split(":");
        newDate.setHours(times[0], times[1], times[2]);
    } else
        newDate.setHours(0, 0, 0);
    
    return newDate;
}

function changeDates(el) {
    trickQueryList(el.value);
   // alert(el.value);
}

function CreateHtml() {
    if (id.split(",").length > 1) {
        Ext.getDom("selectTime").style.display = "none";
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.Forecast', 'AirQualityQueryList'),
            params: { entityName: id, json: json },
            success: function (response) {
                if (response.responseText != ""){
                    Ext.getDom("contentNone").innerHTML = response.responseText;
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
    else {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.Forecast', 'PublicQueryListX'),
            params: { entityName: id, json: json },
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
}
function CityChange(newjJson, nowId) {
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
    imageViewer = new ImageViewer(Ext.BLANK_IMAGE_URL, entityName, "");
    imageViewer.render("OnlyOne");
    imageViewer.setImageSrc(src, entityName, "");
//    imageViewer = new ImageViewer(src, entityName, "");

   

    entityNameNow = entityName;
    var showImg = Ext.getDom("showImg");
    showImg.className = "show1";

    var left = ($("#bg").width() - $("#showImg").width()) / 2;
    $("#showImg").css({ left: left, right: left });
    $("#view-image").css({marginTop:"50px",marginBottom:"50px"});
    $('.bg').fadeIn(200);
    $('#showImg').fadeIn(400);
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'AirQualityBottomSelect'),
        params: { Datetime: dateTime, entityName: entityName, json: json, type: type },
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
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'ReduceButton'),
        params: { entityName: entityNameNow, dateTime: dateTime, hour: hour, json: json, type: type, period: "airQuality" },
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
            imageViewer.setImageSrc(result[obj], entityNameNow, "");
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