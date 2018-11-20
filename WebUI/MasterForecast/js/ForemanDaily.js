
$(function () {
    var date = new Date();
    $("#time").val(date.getFullYear() + "-" + proDate(date));
    date.setDate(date.getDate() + 1);
    $("#foreTime").text(date.getFullYear() + "-" + proDate(date));
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    userName = result["Alias"];
    $("#forecaster").html(result["Alias"]);    //
    $("#people").text($("#forecaster").text());   //预报员为系统登录人员
    clickQuery();
    getPollValue();
    getChart();
})
function getforeTime() {
    var pubTime = $("#time").val();
    //var time = pubTime.split('-');
    var pubDate = new Date(pubTime);
    pubDate.setDate(pubDate.getDate() + 1);
    $("#foreTime").text(pubDate.getFullYear() + "-" + proDate(pubDate));
    //有点复杂
    //var nowDate = new Date();
    //var idays = parseInt((nowDate - pubDate) / 1000 / 60 / 60 / 24);
    //if (idays <= 0) idays = idays - 1;
    //nowDate.setDate(nowDate.getDate() +( 1-idays));
    //$("#foreTime").text(nowDate.getFullYear() + "-" + proDate(nowDate));
}
function show(id,e) {    //显示文本框和下拉选择框
    var evt = e ? e : window.event;
    if (($("#poll").css("display")=="none")) {  //若下拉选择框已经显示则不需要组织冒泡，否则下拉列表出不来
        if (evt.stopPropagation) {//W3C 
            evt.stopPropagation();
        }
        else { //IE 
            evt.cancelBubble = true;
        }
    }
    
    if (id == "par_aqi") {      //显示文本框，并赋值
        $("#aqi").css("display", "block");
        $("#td_value").css("display", "none");
        var td_value = document.getElementById("td_value").innerHTML;
        if (td_value != "") {
            $("#aqi").val(td_value);
        }
        $("#aqi").focus();
        
    } else {           //显示多选框，并赋值
        $("#poll").css("display", "block");
        $("#poll_val").css("display", "none");
        var value = document.getElementById("poll_val").innerHTML;
        if (value != "") {
            $("#multSel").val(value);
        }
        $("#poll>.bootstrap-select").addClass("open");
    }
    clearSelections();
}
//清除选中
function clearSelections() {
    if (window.getSelector) {
        // 获取选中
        var selection = window.getSelection();
        // 清除选中
        selection.removeAllRanges();
    } else if (document.selection && document.selection.empty) {
        // 兼容 IE8 以下，但 IE9+ 以上同样可用
        document.selection.empty();
        // 或使用 clear() 方法
        // document.selection.clear();
    }
}
document.onclick = function () {    //点击屏幕，隐藏文本框和下拉多选框
    if ($("#aqi").css("display")== "block") {   //隐藏文本框并赋值
        var aqi = $("#aqi").val();
        if (aqi != "") {
            $("#td_value").text(aqi);
        }
        $("#aqi").css("display", "none");
        $("#td_value").css("display", "block");
    }
    else if ($("#poll").css("display")=="block") {    //隐藏下拉多选框并赋值
        var poll_val = $("#poll .btn .pull-left").text();
        if (poll_val != "") {
            $("#poll_val").text(poll_val);
        }
        $("#poll").css("display", "none");
        $("#poll_val").css("display", "block");
    }
    if ($("#td_value").text() != "") {
        $("#grade").text(getAQIGrade(parseFloat($("#td_value").text())));
    }
    clearSelections();
}
//获得AQI等级
function getAQIGrade(aqi) {
    var grade = "";
    if (aqi <= 50) grade = "优";
    else if (aqi <= 100) grade = "良";
    else if (aqi <= 150) grade = "轻度污染";
    else if (aqi <= 200) grade = "中度污染";
    else if (aqi <= 300) grade = "重度污染";
    else if (aqi > 300) grade = "严重污染";
    return grade
}
//获取下拉列表的值
function getPollValue() {
    var val = ["PM2.5", "PM10", "O3", "CO", "SO2", "NO2"]
    var html = "";
    for (var i = 0; i < val.length; i++) {
        html += "<option value='" + val[i] + "'>" + val[i] + "</option>";
    }
    $("#poll select").html(html);
}

//保存按钮
function clickSave() {
    var aqi = "";
    if ($("#aqi").css("display") == "block") {
        aqi = $("#aqi").val();
    } else {
        aqi = $("#td_value").text();    //aqi
    }
    var forecaster = $("#people").text();    //预报人员
    var poll = $("#poll_val").text();   //首要污染物
    var lst = $("#time").val();     //发布时间
    var foreTime = $("#foreTime").text();//预报时间
    if (poll == "" || aqi == "") {
        alert("AQI或首要污染物可能为空，请填写完之后再保存"); return;
    }
    if (!confirm("是否要保存？")) return;

    var curHour = new Date().getHours(); // xuehui 08-01
    if (curHour > 17) {
        alert("已经超过17时，不能再保存！")
        return;
    }

    // ext ajax  xuehui 07-11
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.MasterForecast', 'Save'),
        params: { forecaster: forecaster, aqi: aqi, poll: poll, foreTime: foreTime, lst: lst },
        success: function (response) {
            if (response.responseText == "ok") {
                Ext.Msg.alert("提示", "保存成功!");
                getChart();
            }
            else {
                Ext.Msg.alert("提示", "保存失败，请重试或者联系系统管理员!");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

//    $.ajax({
//        url: "ForemanDaily.aspx/Save",
//        type: "POST",
//        contentType: "application/json",
//        dataType: 'json',
//        data: "{forecaster:'" + forecaster + "',aqi:'" + aqi + "',poll:'" + poll + "',foreTime:'" + foreTime + "',lst:'"+lst+"'}",
//        success: function (results) {
//            var datas = results.d;
//            if (datas=="ok") {
//                alert("保存成功！");
//            } else {
//                alert("保存失败！");
//            }
//            getChart();
//        },
//        error: function (ex) {
//            alert("异常，" + ex.responseText + "！");
//        }
//    });
}
function clickQuery() {
    var pubTime = $("#time").val();
    getforeTime();

    // ext ajax  xuehui 07-11
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.MasterForecast', 'Query'),
        params: { pubTime: pubTime },
        success: function (response) {
            var datas = Ext.util.JSON.decode(response.responseText);
            var data = datas.data;
            if (data.length > 0) {
                $("#people").text(data[0][0]);
                $("#td_value").text(data[0][1]);
                $("#poll_val").text(data[0][2]);
                $("#grade").text(getAQIGrade(parseFloat($("#td_value").text())));
                getforeTime();
            } else {
                $("#td_value").text("");
                $("#poll_val").text("");
                $("#grade").text("");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

//    $.ajax({
//        url: "ForemanDaily.aspx/Query",
//        type: "POST",
//        contentType: "application/json",
//        data: "{pubTime:'" + pubTime + "'}",
//        dataType: 'json',
//        success: function (results) {
//            var datas = results.d;
//            if (datas.rows.length > 0) {
//                for (var i = 0; i < datas.rows.length; i++) {
//                    $("#people").text(datas.rows[i]["forecaster"])
//                    $("#td_value").text(datas.rows[i].AQI);
//                    $("#poll_val").text(datas.rows[i].pollution);
//                    $("#grade").text(getAQIGrade(parseFloat($("#td_value").text())));
//                }
//                getforeTime();
//            }
//            else alert("查询失败，原因如下：" + results.d);
//        },
//        error: function (ex) {
//            alert("异常，" + ex.responseText + "！");
//        }
//    });
}

function getChart() {
    var nowdate = new Date();   //查询到今天的数据
    var pubTime = nowdate.getFullYear() + "-" + proDate(nowdate);


    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.MasterForecast', 'GetChartII'),
        params: { pubTime: pubTime },
        success: function (response) {
            var datasII = null;
            if (response.responseText != "") {
                var datass = response.responseText.toString().split('*');
                datasII = eval(datass[0]);
            }


            // ext ajax  xuehui 07-17
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.MasterForecast', 'GetChart'),
                params: { pubTime: pubTime },
                success: function (response) {

                    if (response.responseText != "") {
                        var datas = response.responseText.toString().split('*');
                        var x_time = datas[1].substring(0, datas[1].length).split('|');
                        var val = eval(datas[0]);
                        renderChart(val, x_time, datasII);
                    }

                },
                failure: function (response) {
                    //Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });


        },
        failure: function (response) {
            //Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });



   


//    $.ajax({
//        url: "ForemanDaily.aspx/GetChart",
//        type: "POST",
//        contentType: "application/json",
//        data: "{pubTime:'" + pubTime + "'}",
//        dataType: 'json',
//        success: function (results) {
//            var data = results.d;
//            //var datas = eval(data);
//            if (data != "") {
//                var datas = data.split('*');
//                var x_time = datas[1].substring(0, datas[1].length).split('|');
//                var val = eval(datas[0]);
//                renderChart(val,x_time);
//            }
//        }
    //    });


}

function proDate(date) {
    var month = (date.getMonth() + 1);
    var day = date.getDate();
    month = month < 10 ? "0" + month : month;   //若时间小于10则在前面加0
    day = day < 10 ? "0" + day : day;
    return month + "-" + day;
}
function renderChart(datas, x_time, datasII) {
    var x =[], y = [];
    for (var i = 0; i < x_time.length; i++) {
        var _x = parseFloat(x_time[i] * 1000);
        var date = new Date(_x);
        x.push(proDate(date));
    }
    Highcharts.setOptions({
        lang: {
            printChart: "打印图表",
            downloadJPEG: "下载JPEG 图片",
            downloadPDF: "下载PDF文档",
            downloadPNG: "下载PNG 图片",
            downloadSVG: "下载SVG 矢量图",
            exportButtonTitle: "导出图片"
        }
    });
    var chart = new Highcharts.Chart('container', {
        title: {
            text: '领班15天逐日预报情况',
            x: -10
        },
        exporting: {
            enabled: true,
            url: 'http://export.highcharts.com'
        },
        tooltip: {
            shadow: true,                 // 是否显示阴影
            animation: true,
            formatter: function () {
                var name = this.point.name + "<br/>";
                //if (this.series.name.toString() == "AQIII")
                  //  name = "";

                var aqi = this.point.y;
                var gride = "";
                if (aqi <= 50) {
                    gride = "AQI:" + aqi + ",优";
                }
                else if (aqi <= 100) gride = "AQI:" + aqi + ",良";
                else if (aqi <= 150) gride = "AQI:" + aqi + ",轻度污染";
                else if (aqi <= 200) gride = "AQI:" + aqi + ",中度污染";
                else if (aqi <= 300) gride = "AQI:" + aqi + ",重度污染";
                else if (aqi > 300) gride = "AQI:" + aqi + ",严重污染";
                return name  + gride;
            }
        },
        credits: {
            enabled: false     //不显示LOGO 
        },
        //xAxis: {
        //    categories: x

        //},
        xAxis: { labels: { formatter: function () { return x[this.value]; } }, tickInterval: 1 },
        yAxis: {
            title: null

        },
        lang: {
            noData: "没有要显示的数据"//自定义显示文本 
        },
        noData: { //自定义样式
            // Custom positioning/aligning options 
            position: { //相对于绘图区定位无数据标签的位置。 默认值：[object Object].
                //align: 'right', 
                //verticalAlign: 'bottom' 
            },
            // Custom svg attributes 
            attr: { //无数据标签中额外的SVG属性
                // 'stroke-width': 1, 
                // stroke: '#cccccc' 
            },
            // Custom css 
            style: { //对无数据标签的CSS样式。 默认值：[object Object]. 
                fontWeight: 'bold',
                fontSize: '20px'
            }
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0,
            enabled: false
        },
        series: [{
            name: 'AQI',
            colorByPoint: true,
            type: 'column',
            data: datas
        }, {
            name: 'AQIII',
            colorByPoint: false,
            type: 'spline',
            data: datasII
        }
            ]
    });
    }
