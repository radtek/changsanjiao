var oldID = "L0";
var entityNameNow = "";
var explain = "",instroHeihgt=0;  //说明栏的内容、高度
var arr_area = [];
var arr_station = ["ecmf", "kwbc", "babj", "ritd", "rksl", "cwao", "isac", "egrr", "lfpw"];   //这里要内容要显示的顺序和下拉框显示的顺序不一致，所以在这里写死了
Ext.onReady(function () {
    initInputHighlightScript();
    supportInnerText(); //使得火狐支持innerText
    //CreateHtml();
    //绑定时间选择下拉菜单的事件
    $("#selectID").on('click', function () {
        $("#dateUl").toggleClass("display");
    });
    $(".month,.day").on('click', monthOrDay);

    // $("#page").height($(document).height());
    $("#dateUl li").on('click', function () {
        $("#dateTxt").html(this.innerHTML);
        $("#dateUl").toggleClass("display");
        $("#dateUl").toggleClass("hide");
        trickQueryList(this.innerHTML);
    });
    $('#page').height($(document).height());
    getExplain();
})
//点击显示按钮显示或隐藏说明文字
function showInstro(h) {
    if ($(".introduce").hasClass("indisInstro")) {
        $(".introduce").removeClass("indisInstro");
        $(".introduce").css({
            height: h,
            opacity: 1,
            padding: "10px 0 10px 20px"
        });
        $(".show-intro img").attr("src", "images/up.png");
        $(".show-intro img").attr("title", "折叠");
    } else {
        $(".introduce").addClass("indisInstro");
        $(".introduce").css({
            opacity: 0,
            padding: 0,
            height: 0,
        });
        $(".show-intro img").attr("src", "images/down.png");
        $(".show-intro img").attr("title", "展开");
    }
}
//获取抬头的说明文字和标题
function getExplain() {
    $.ajax({
        url: "AirQualityEXT.aspx/GetExplain",
        type: "POST",
        contentType: "application/json",
        data: "{parentTxt:'" + parentTxt + "'}",
        dataType: 'json',
        success: function (results) {
            var data = results.d;
            if (data.length > 0) {
                explain = data.split("#")[0];
                var title = data.split("#")[1];
               // proTitle(explain, $(".introduce"),"introduce");
                proTitle(title, $(".header .show-title"), "title");
            } else {
                $(".header").remove();
                $(".introduce").remove();
            }
            isShowCondition();
        }
    });
}

function proTitle(title, ele,type) {
    if (title == "") {
        $(ele).remove();
    } else {
        $(ele).html(title);
    }
}
//显示页面的查询条件，每个模块的可能不同，以及初始化日期、下拉选择框
function isShowCondition() {
    $("#coumtry").selectpicker({
        width: "150px"
    });
    $("#area").selectpicker({
        width: "100px"
    });
    $("#_period").selectpicker({
        width: "100px"
    });
    //var height = $(document).height() - 265;
    var height = $(document).height() - $("#contentNone").offset().top;
    $("#contentNone").height(height);
    if (json.indexOf("T:0") > 0) {    //东亚重要环流型预测模块，需要隐藏查询条件，只保留日期
        if (id == "WPSH" || id == "SouthAsiahigh") {   //西太副高、南亚高压两个页面要区分日数据和月数据
            $(".monthOrday").removeClass("indis");
        }
        getDate();
    } else {
        if (parentTxt == "多中心对比图") {
            $(".special").addClass("indis");
        }
        $("#contentImg1 .condition").css("display", "block");
        getSelectVal();
    }
}

function monthOrDay() {
    if (!$(this).hasClass('active')) {
        $(".month,.day").removeClass('active');
        $(this).addClass('active');
        //当选日数据或月数据的时候要重新给日期赋值
        getDate();
    }
}

//切换月、日按钮时获取时期，以及东亚重要环流型预测模块第一次加载时调用
function getDate(first) {
    var type = $(".active").text();
    type = type == "月" ? "Month" : "Day";
    if (json.indexOf("T:0") > 0) {
        if (id == "AGCM" || id == "CSM") {
            type = "Day";
        } else if (id == "MATEWinter") {
            type = "Year";
        }
        if (type == "Year") {
            document.getElementById("H00").onfocus = function () {
                WdatePicker({ dateFmt: 'yyyy' })
            }
        } else if (type == "Month") {
            document.getElementById("H00").onfocus = function () {
                WdatePicker({ dateFmt: 'yyyy-MM' })
            }
        } else {
            document.getElementById("H00").onfocus = function () {
                WdatePicker({ dateFmt: 'yyyy-MM-dd' })
            }
        }
    } else {
        document.getElementById("H00").onfocus = function () {
            WdatePicker({ dateFmt: 'yyyy-MM-dd' })
        }
    }

    $.ajax({
        url: "AirQualityEXT.aspx/GetDate",
        type: "POST",
        contentType: "application/json",
        data: "{type:'" + type + "'}",
        dataType: 'json',
        success: function (results) {
            $("#H00").val(results.d);
            if (first == "1") {
                changeCondition();   //改变默认条件
            } else {
                trickQueryList();
            }
            var num = 0;
            $(".show-intro").on('click', function () {
                if (num == 0) {
                    proTitle(explain, $(".introduce"), "introduce");
                    instroHeihgt = $(".introduce").outerHeight();
                    //第一次点击的时候加transition属性，要不然页面加载的时候初始化默认选项会有动画效果，用display第一次没有动画效果但是transition不支持
                    $(".introduce").css("transition", "all 1s");
                    $(".introduce").addClass("indisInstro");
                }
                showInstro(instroHeihgt);
                num++;
            })
        }
    });
}
function getSelectVal() {
    var m_id = id;
    $.ajax({
        url: "AirQualityEXT.aspx/GetSelectVal",
        type: "POST",
        contentType: "application/json",
        // data: "{userGroup:'" + data + "'}",
        dataType: 'json',
        success: function (results) {
            //后台返回的是dataset的集合，包含了国家、区域、时效的值，这里就通过循环绑定数据
            var data = results.d.tables;
            //遍历tables
            var id = ["coumtry", "area", "_period"];
            for (var tableCount = 0; tableCount < data.length; tableCount++) {
                var rowContent = data[tableCount].rows;
                if (rowContent.length > 0) {
                    //遍历行数据
                    var html = "";
                    if (tableCount == 2 && m_id.indexOf('Wind') >= 0) {
                        html = "<option value=1>日(1d)</option>";
                        $("#_period").attr("disabled", true);
                    } else {
                        if (tableCount == 2) {
                            html = "<option value=0>全部</option>";
                        }
                        for (var rowCount = 0; rowCount < rowContent.length; rowCount++) {
                            html += "<option value=" + rowContent[rowCount].code + ">" + rowContent[rowCount].MC + "</option>";
                            if (tableCount == 1) {
                                arr_area.push(rowContent[rowCount].code);
                            }
                        }
                    }
                    if (tableCount == 0) {
                        html += "<option value='cfs'>CFS</option>";
                    }
                    $("#" + id[tableCount]).html(html);
                    $('#' + id[tableCount]).selectpicker('refresh');
                    
                }
            }
            if (parentTxt != "多中心对比图") {
                $('.selectpicker').selectpicker('val', 'ecmf');
            }
            $('#area').selectpicker('val', 'EASTASIA');
            getDate("1");

            //ReduceSelect(0);
        }
    });
}
//如果默认条件下没有数据则改变默认条件
function changeCondition() {
    var time = $("#H00").val();
    var period = $('#_period').val();
    $.ajax({
        url: "AirQualityEXT.aspx/ChangeCondition",
        type: "POST",
        contentType: "application/json",
        data: "{id:'" + id + "',parentTxt:'" + parentTxt + "',time:'" + time + "',period:'" + period + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d.length > 0) {
                var station = results.d.split('#')[0];
                var area = results.d.split('#')[1];
                $('.selectpicker').selectpicker('val', station);
                $('#area').selectpicker('val', area);
                trickQueryList();
            }
        }
    })
}

function trickQueryList(Datetime) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ForecastExt', 'trickQueryListII'),
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

function convertDate(time, num) {
    var reg = new RegExp("[\\u4E00-\\u9FFF]+", "g");
    time = time.replace(reg, "-");
    var newDate = "";
    if (time.substring(time.length - 1, time.length) == "-") {
        time = time.substring(0, time.length - 1);
    }
    if (time.split('-').length == 2) {   //月
        newDate = addMonth(time, num);
    } else if (time.indexOf('-') < 0) {   //年
        newDate = addYear(time, num);
    } else {      //日
        newDate = addDay(time, num);
    }
    return newDate;
}
function addDay(time, num) {
    var date = new Date(time);
    date.setDate(date.getDate() + num);
    return date.format('Y-m-d');
}
function addMonth(time, num) {
    var date = new Date(time);
    var m = date.getMonth() + num;
    var y = date.getFullYear();
    while (m > 12) {
        y = y + 1;
        m = m - 12;
    }
    var newDate = new Date(y, m);
    return newDate.format('Y-m');
}

function addYear(time, num) {
    var date = new Date(time);
    var y = date.getFullYear() + num;
    return new Date(y + "").format('Y');
}
function ReduceSelect(day) {
    var el = Ext.getDom("H00");
    var oldDate = el.value.toString();
    var startDate = convertDate(oldDate, day);
    el.value = startDate;
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

    newDate = (y + '-' + m + '-' + d);
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
            url: getUrl('MMShareBLL.DAL.ForecastExt', 'AirQualityQueryList'),
            params: { entityName: id, json: json },
            success: function (response) {
                if (response.responseText != "") {
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
            url: getUrl('MMShareBLL.DAL.ForecastExt', 'PublicQueryListX'),
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
    var country = $('#coumtry').selectpicker('val');
    if (parentTxt == "多中心对比图") {  //多中心对比没有这个选项
        country = "";
    }
    var area = $('#area').val();
    var period = $('#_period').val();
    if (Datetime == undefined) {
        Datetime = $("#H00").val();
    }
    var monthOrDay = $("#contentImg1 .active span").text();
    if ($(".monthOrday").css("display") == "none" && json.indexOf("T:0") < 0) {
        monthOrDay = "";
    }
    else {
        monthOrDay = monthOrDay == "月" ? "Month" : "Day";
    }
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ForecastExt', 'trickQueryListII'),
        params: { Datetime: Datetime, entityName: id, json: json, country: country, area: area, period: period, monthOrDay: monthOrDay },
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
    $("#view-image").css({ marginTop: "50px", marginBottom: "50px" });
    $('.bg').fadeIn(200);
    $('#showImg').fadeIn(400);
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ForecastExt', 'AirQualityBottomSelect'),
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
    //闫海涛修改，添加图片关闭按钮
    $("#closeBtn").on('click', function () {
        $('.bg').fadeOut(800);
        $('#showImg').fadeOut(800);
    });
}

//闫海涛修改
//$('#showImg').on('click',function () {
//    $('.bg').fadeOut(800);
//    $('#showImg').fadeOut(800);   
//});

$('#bg').on('click', function () {
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
        url: getUrl('MMShareBLL.DAL.ForecastExt', 'ReduceButton'),
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