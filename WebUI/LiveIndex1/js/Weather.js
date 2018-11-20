
var idd = "day1";   //全局变量，记录当前页面是哪一天
var selectDate = "";

$(function () {
    //首先判断上午还是下午
//    var loginParams = getCookie('UserInfo');
//    var loginResult = Ext.util.JSON.decode(loginParams);
//    useName = loginResult['Alias'];
    getSite();
    getForecastDate();
    getWind();
    getWeather();
    $('input[type="checkbox"]').click(function () {
        //$('input[type="checkbox"]').prop("checked", false).attr("value", 0);
        if ($(this).val()==0) {
            $(this).prop("checked", true).attr("value", 1);
        } else {
            $(this).prop("checked", false).attr("value", 0);
        }
    });
})

//获取站点
function getSite() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetSite'),
        success: function (response) {
            var data = eval(response.responseText);
            var html = "";
            for (var i = 0; i < data.length; i++) {
                html += "<option value=" + data[i].stationCo + ">" + data[i].name + "</option>"
            }
            $("#site").html(html);
            refresh();
        }
    });
}
//获取风速   王斌  2017.5.2
function getWind() {
    $.ajax({
        url:"Weather.aspx/getWind",
        type: "POST",
        contentType: "application/json",
        dataType: "JSON",
        success: function (results) {
            var datas = results.d.rows, html = "";
            if (datas != null && datas.length > 0) {
                for (var i = 0; i < datas.length; i++) {
                    html += "<option value='" + datas[i].wind + "'>" + datas[i].wind + "</option>";
                }
                $("#speed1").html(html);
                $("#speed2").html(html);
                $("#speed3").html(html);
            }
        }
    });
}
//获取天气现象的值
function getWeather() {
    $.ajax({
        url: "Weather.aspx/getWeather",
        type: "POST",
        contentType: "application/json",
        dataType: "JSON",
        success: function (results) {
            var data = results.d.rows, html = "";
            if (data != null && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    html += "<option value='"+data[i].DM+"'>" + data[i].MC + "</option>"
                }
                $(".WeaPhenomenaM").html(html);
                $(".WeaPhenomenaN").html(html);
            }
        }
    });
}
//切换页面
function selectDates(id) {
    var id2;
    var useId = document.getElementById(id);
    if (useId.className == "unSelect") {
        useId.className = "select active";
        var curDate = new Date();
        if (id == "day1") {
            selectDate = curDate.getFullYear() + "年" + proDate(curDate) ;
            idd = "day1";
            id = "day2";
            id2 = "day3";
            document.getElementById(id).className = "unSelect";
            document.getElementById(id2).className = "unSelect";
            $("#table1").css("display", "block");
            $("#table2").css("display", "none");
            $("#table3").css("display", "none");
        }
        else if (id == "day2") {
             curDate.setDate(curDate.getDate() + 1);
             selectDate = curDate.getFullYear() + "年" + proDate(curDate);
            id = "day1";
            id2 = "day3";
            idd = "day2";
            document.getElementById(id).className = "unSelect";
            document.getElementById(id2).className = "unSelect";
            $("#table1").css("display", "none");
            $("#table2").css("display", "block");
            $("#table3").css("display", "none");
        }
        else if (id == "day3") {
            var c = curDate.setDate(curDate.getDate() +2);
            selectDate = curDate.getFullYear() + "年" + proDate(curDate);
            id = "day1";
            id2 = "day2";
            idd = "day3";
            $("#table1").css("display", "none");
            $("#table2").css("display", "none");
            $("#table3").css("display", "block");
            document.getElementById(id).className = "unSelect";
            document.getElementById(id2).className = "unSelect";
        }
        //refresh();
    }
}

//获取预报日期
function getForecastDate() {
    var date1, date2, date3;
    var CurrDate = new Date();
    var CurrDate2 = new Date();
    CurrDate2 = CurrDate.getFullYear() + "年" + proDate(CurrDate);
    date1 = "今天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + CurrDate.getDate() + "日预报";
    CurrDate.setDate(CurrDate.getDate() + 1);
    date2 = "明天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + (CurrDate.getDate()) + "日预报";
    CurrDate.setDate(CurrDate.getDate() + 1);
    date3 = "后天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + (CurrDate.getDate()) + "日预报";
   
    $("#a1").html(date1);
    $("#a2").html(date2);
    $("#a3").html(date3);
    selectDate = CurrDate2;
}

//实现保存功能    
function save(obj) {
    var val = [], name = [],code=[], label = "", tagName = "";
    var nowDay = new Date();
    var day = 0;
    if (idd == "day1") {
        tagName = document.getElementsByName('text');
        label = $('#table1 label.fac1');
    }
    else if (idd == "day2") {
        tagName = document.getElementsByName('text2');
        label = $('#table2 label.fac2');
        day = 1;
    }
    else if (idd == "day3") {
        tagName = document.getElementsByName('text3');
        label = $('#table3 label.fac3');
        day = 2;
    }
    val = getTagNameVal(tagName);
    name = getTagNameVal(label, "text");
    code = getTagNameVal(label, "code");
    nowDay.setDate(nowDay.getDate() + day);
    var lst = nowDay.getFullYear() + "年" + proDate(nowDay);
    var site = $("#site").val();
    if (!confirm("是否要保存？")) return;
    $.ajax({
        url: "Weather.aspx/save",
        type: "POST",
        contentType: "application/json",
        data: "{name:'" + name + "', val: '" + val + "',LST:'" + lst + "',site:'" + site + "',code:'" + code + "'}",
        dataType: 'json',
        success: function (results) {
            if (results.d.indexOf("error") > -1) {
                alert("保存失败！");
            } else {
                alert("保存成功！");
            }
            refresh();
        }
    });
}
//日期如果小于10则在前面加0
function proDate(date) {
    var month = (date.getMonth() + 1);
    var day = date.getDate();
    month = month < 10 ? "0" + month : month;   //若时间小于10则在前面加0
    day = day < 10 ? "0" + day : day;
    return month + "月" + day+"日";
}

function getTagNameVal(tagName,type) {
    var val = [];
    for (var i = 0; i < tagName.length; i++) {
        if (type == "text") {
            val[i] = tagName[i].innerText.split(":")[0];
        }
        else if (type == "code") {
            val[i] = tagName[i].getAttribute("code");
        }
        else {
            val[i] = tagName[i].value;
        }
    }
    return val;
}
function clear() {
    if(idd=="day1"){
        $("input[name='text']").each(function () {
             $(this).val('');
         });
     }
     if (idd == "day2") {
         $("input[name='text2']").each(function () {
             $(this).val('');
         });
     }
     if (idd == "day3") {
         $("input[name='text3']").each(function () {
             $(this).val('');
         });
    }
 }

 //第一次进入页面，获取上次订正的数据并显示以及重新获取接口数据
//id为getele是只获取接口数据
//王斌  2017.5.2
function refresh(id) {
    clear();
    var site = $("#site").val();
    var url = "Weather.aspx/Refresh";
    var data = "{dates:'" + selectDate + "',site:'" + site + "',day:'" + idd + "'}";
    if (id == "getEle") {
        url = "Weather.aspx/GetEleInterface";
        data = "{site:'" + site + "'}";
    }
    var mk = new Ext.LoadMask(document.body, {
        msg: '正在获取数据，请稍候！',
        removeMask: true //完成后移除  
    });
    mk.show();
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        contentType: "application/json",
        dataType: "JSON",
        success: function (results) {
            if (results.d != null && results.d != "") {
                var data = results.d.rows;
                var date = new Date();
                var dNow = new Date(date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate()).getTime();   //今天的毫秒数
                var num = "", LST = "";
                if (data != null && data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var code = data[i].code;
                        var name = data[i].name;
                        var value = data[i].value;
                        if ((data[i].lst).indexOf('/Date(') > -1) {
                            LST = (data[i].lst).substr(6, (data[i].lst).length - 8);
                        }
                        else {
                            LST = new Date(data[i].lst.split(' ')[0]).getTime();
                        }
                        var total = parseInt((LST - dNow) / 1000 / (24 * 60 * 60));

                        if (total == 0) {    //第一天
                            num = 1;
                        } else if (total == 1) {   //第二天
                            num = 2;
                        } else if (total == 2) {     //第三天
                            num = 3;
                        }
                        if (code == 'w1' || code == 'w51') {
                            $("#table" + num + " label[code='" + code + "']").next().find("option[value='" + value + "']").attr("selected", true);
                        } else {
                            $("#table" + num + " label[code='" + code + "']").next().val(value);
                        }
                    }
                }
            } else {
                alert("获取数据失败！");
            }
            mk.hide();
        }
    });

}
//重新计算
//若勾选“重新计算今天指数”
//1.页面数据保存到保存表中T_Weather_Cor
//2.数据保存到要素表中Weather_Eledata
//3.数据保存到指数订正预报表中Weather_IndexResult
//不勾选则是保存到保存表中T_Weather_Cor
function correct() {
    var temp = "", tagName = "", val = "", label = "", name = "", lst = "", code = "";
    var site = $('#site').val();
    if (idd == "day1") {
        temp = "1";
        tagName = document.getElementsByName('text');   //第一天的name命名后面没有加1所以单独处理
        label = $('#table1 label.fac'+temp+'');
    } else if (idd == "day2") {
        temp = "2";
    } else if (idd == "day3") {
        temp = "3";
    }
    if ($("#check" + temp).val() == 0) {
        save();
        return;
    } 
    var dNow = new Date();
    dNow.setDate(dNow.getDate() + (temp - 1));
    lst = dNow.getFullYear() + "年" + proDate(dNow);
    if (idd != "day1") {     //获取当前页面的值以及要素名称
        tagName = document.getElementsByName('text'+temp);
        label = $('#table' + temp + ' label.fac' + temp + '');
    }
    name = getTagNameVal(label, "text");
    val = getTagNameVal(tagName);
    code = getTagNameVal(label, "code");
    if (!confirm("是否要订正？")) return;
    var mk = new Ext.LoadMask(document.body, {
        msg: '正在订正，请稍候！',
        removeMask: true //完成后移除  
    });
    mk.show();
    $.ajax({
        url: "Weather.aspx/Corrent",
        type: "POST",
        contentType: "application/json",
        data: "{name:'" + name + "', val: '" + val + "',LST:'" + lst + "',site:'" + site + "',code:'" + code + "'}",
        dataType: 'json',
        success: function (results) {
            mk.hide();
            alert("订正成功！");
        }
    });
}

function ConvertToFloat(x) {
    if (x != "null") {
        var floatTemp = parseFloat(x).toFixed(1);
        var floatValue = parseFloat(floatTemp);
        if (floatValue > 0)
            return floatValue;
        else
            return null;
    }
    else
        return null;
}
