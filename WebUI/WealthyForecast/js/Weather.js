
var idd = "day1";
var selectDate = "";

$(function () {
    //首先判断上午还是下午
    getSite();
    getForecastDate();
    getWind();
    getWeather();

})

//获取站点
function getSite() {
    $.ajax({
        url: "Weather.aspx/getSite",
        type: "POST",
        contentType: "application/json",
        dateType: "JSON",
        success: function (results) {
            var data = results.d.rows, html = "";
            for (var i = 0; i < data.length; i++) {
                html += "<option>" + data[i].station_name + "</option>";
            }
            $("#site").html(html);
            getWind();
            refresh();
            //getWindGrade("PJFS1", "1");
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
            for (var i = 0; i < datas.length; i++) {
                html += "<option value='" + datas[i].wind + "'>" + datas[i].wind + "</option>";
            }
            $("#speed1").html(html);
            $("#speed2").html(html);
            $("#speed3").html(html);
            //getWindGrade("PJFS1", "1");
        }
    });
}
//获取天气现象的值
function getWeather() {
//    $.ajax({
//        url: "Weather.aspx/getWeather",
//        type: "POST",
//        contentType: "application/json",
//        dataType: "JSON",
//        success: function (results) {
//            var data = results.d.rows, html = "";
//            for (var i = 0; i < data.length; i++) {
//                html += "<option>" + data[i].WeaPhenomena + "</option>"
//            }
//            $(".WeaPhenomenaM").html(html);
//            $(".WeaPhenomenaN").html(html);
//        }
    //    });

    var  html = "<option value='晴'>晴</option>";
    html += "<option value='多云'>多云</option>";
    html += "<option value='少云'>少云</option>";
    html += "<option value='阴天'>阴天</option>";
    html += "<option value='弱降水'>弱降水</option>";
    html += "<option value='有降水'>有降水</option>";
    html += "<option value='有明显降水'>有明显降水</option>";
 
    $(".WeaPhenomenaN").html(html);
    $(".WeaPhenomenaM").html(html);


//    $('#pollutant').multipleSelect({
//        width: 100,
//        selectAll: false
//    });
//    $('#pollutant2').multipleSelect({
//        width: 100,
//        selectAll: false
//    });
//    $('#pollutant3').multipleSelect({
//        width: 100,
//        selectAll: false
//    })
  
    
}
//切换页面
function selectDates(id) {

    var id2;
    var useId = document.getElementById(id);

    if (useId.className == "unSelect") {
        useId.className = "select active";

        var dates = " 08:00:00";
        var curDate = new Date();
        var h = 0;
        if (curDate.getHours() > 12) {
            dates = " 20:00:00"; h = 1;
        }

        if (id == "day1") {
            curDate.setDate(curDate.getDate() + h);
            selectDate = curDate.getFullYear() + "-" + (curDate.getMonth() + 1) + "-" + (curDate.getDate()) + dates;
            idd = "day1";
            id =  "day2";
            id2 = "day3";
            document.getElementById(id).className = "unSelect";
            document.getElementById(id2).className = "unSelect";
            $("#table1").css("display", "block");
            $("#table2").css("display", "none");
            $("#table3").css("display", "none");
            getWindGrade("PJFS1", "1");

        }
        else if (id == "day2") {
             curDate.setDate(curDate.getDate() + h+1);
             selectDate = curDate.getFullYear() + "-" + (curDate.getMonth() + 1) + "-" + (curDate.getDate()) + dates;

            id = "day1";
            id2 = "day3";
            idd = "day2";
            document.getElementById(id).className = "unSelect";
            document.getElementById(id2).className = "unSelect";
            $("#table1").css("display", "none");
            $("#table2").css("display", "block");
            $("#table3").css("display", "none");
            getWindGrade("PJFS2", "2");

        }
        else if (id == "day3") {
            var c = curDate.setDate(curDate.getDate() + h+2);
            selectDate = curDate.getFullYear() + "-" + (curDate.getMonth() + 1) + "-" + (curDate.getDate()) + dates;
            id = "day1";
            id2 = "day2";
            idd = "day3";
            $("#table1").css("display", "none");
            $("#table2").css("display", "none");
            $("#table3").css("display", "block");
            document.getElementById(id).className = "unSelect";
            document.getElementById(id2).className = "unSelect";
            getWindGrade("PJFS3", "3");
        }

        refresh();
    }

}

//获取预报日期
function getForecastDate() {

    var date1, date2, date3;

    var CurrDate = new Date();
    var CurrDate2 = new Date();
    var h="08:00:00";
    if (CurrDate.getHours() < 12) {
        CurrDate2 = CurrDate.getFullYear() + "-" + (CurrDate.getMonth() + 1) + "-" + CurrDate.getDate() + " " + h;
        date1 = "今天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + CurrDate.getDate() + "日预报";
         CurrDate.setDate(CurrDate.getDate() + 1);
        date2 = "明天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + (CurrDate.getDate()) + "日预报";
         CurrDate.setDate(CurrDate.getDate() + 1);
        date3 = "后天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + (CurrDate.getDate()) + "日预报";
    }
    else {
        h = "20:00:00";
        CurrDate.setDate(CurrDate.getDate() + 1);    
        CurrDate2 = CurrDate.getFullYear() + "-" + (CurrDate.getMonth() + 1) + "-" + CurrDate.getDate() + " " + h;
        date1 = "明天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + CurrDate.getDate() + "日预报";
        CurrDate.setDate(CurrDate.getDate() + 1);
        date2 = "后天：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + (CurrDate.getDate()) + "日预报";
        CurrDate.setDate(CurrDate.getDate() + 1);
        date3 = "第三日：" + CurrDate.getFullYear() + "年" + (CurrDate.getMonth() + 1) + "月" + (CurrDate.getDate()) + "日预报";
    }
    $("#a1").html(date1);
    $("#a2").html(date2);
    $("#a3").html(date3);


    selectDate = CurrDate2;
}

//实现订正功能     由于字段太多，修改sql语句有点麻烦，所以添加风速时另起了一个name
function save() {
    var content = "";
    var useAqi = "";
    var pollutantM = "";
    var pollutantA = "";
    if (idd == "day1") {
        
        //var pollutants = $("#pollutant").innerHTML();
        //var AQI = $("#aqiM").val();
        var wind = document.getElementsByName("speed")[0].value;
        var AQI = document.getElementsByName("aqi");
        var text = document.getElementsByName("text");
        pollutantM = $("#pollutantM").combobox('getValues');
        pollutantA = $("#pollutantA").combobox('getValues');
        for (var i = 0; i < text.length; i++) {
            content += text[i].value + ",";
        }
        for (var i = 0; i < AQI.length; i++) {
            useAqi += AQI[i].value + ",";
        }
    }

    else if (idd == "day2") {
        //var pollutants = $("#pollutant2").combobox('getValues');
        //var AQI = $("#aqi2").val();
        //var pollutants = document.getElementsByName("pollutant2")
        var wind = document.getElementsByName("speed")[1].value;
        var AQI = document.getElementsByName("aqi2");
        var text = document.getElementsByName("text2");
        pollutantM = $("#pollutantM2").combobox('getValues');
        pollutantA = $("#pollutantA2").combobox('getValues');
        for (var i = 0; i < text.length; i++) {
            content += text[i].value + ",";
        }

        for (var i = 0; i < AQI.length; i++) {
            useAqi += AQI[i].value + ",";
        }
    }
    else if (idd == "day3") {
        //var pollutants = $("#pollutant3").combobox('getValues');
        //var AQI = $("#aqi3").val();
        //var pollutants = document.getElementsByName("pollutant3")
        var wind = document.getElementsByName("speed")[2].value;
        var AQI = document.getElementsByName("aqi3");
        var text = document.getElementsByName("text3");
        pollutantM = $("#pollutantM3").combobox('getValues');
        pollutantA = $("#pollutantA3").combobox('getValues');
        for (var i = 0; i < text.length; i++) {
            content += text[i].value + ",";
        }

        for (var i = 0; i < AQI.length; i++) {
            useAqi += AQI[i].value + ",";
        }
    }
    
    // usePollutant = usePollutant.substring(0, usePollutant.length - 1);
    useAqi = useAqi.substring(0, useAqi.length - 1);
    content = content.substring(0, content.length - 1);

    var site = $("#site").val();
    $.ajax({
        url: "Weather.aspx/save",
        type: "POST",
        contentType: "application/json",
        data: "{ con: '" + content + "',forecast:'" + selectDate + "',site:'" + site + "',pollutantA:'" + pollutantA + "',pollutantM:'" + pollutantM + "',aqi:'" + useAqi + "',wind:'"+wind+"'}",
        dataType: 'json',
        success: function (results) {
            alert("保存成功！");
        }
    });
}
function clear() {
    if(idd=="day1"){
        $("input[name='text']").each(function () {
             $(this).val('');
         });
         $("input[name='aqi']").each(function () {
             $(this).val('');
         });
        //王斌  2017.5.2  清除新增的风速下拉框
         //$("select[name='speed']")[0].innerHTML="";
         $("#pollutantM").combobox("setValue", '');
         $("#pollutantA").combobox("setValue", '');
     }
     if (idd == "day2") {
         $("input[name='text2']").each(function () {
             $(this).val('');
         });
         $("input[name='aqi2']").each(function () {
             $(this).val('');
         });
         //王斌  2017.5.2  清除新增的风速下拉框
       //  $("select[name='speed']")[1].innerHTML = "";
         $("#pollutantM2").combobox("setValue", '');
         $("#pollutantA2").combobox("setValue", '');
     }
     if (idd == "day3") {
         $("input[name='text3']").each(function () {
             $(this).val('');
         });
         $("input[name='aqi3']").each(function () {
             $(this).val('');
         });
         //王斌  2017.5.2  清除新增的风速下拉框
         //$("select[name='speed']")[2].innerHTML="";
         $("#pollutantM3").combobox("setValue", '');
         $("#pollutantA3").combobox("setValue", '');
     }


        $(".WeaPhenomenaM").each(function (i, n) {
             $(this).val('');
         });


         $(".WeaPhenomenaN" ).each(function (i, n) {
             $(this).val('');
         });
     

 }


 function refreshII() {
     var site = $("#site").val();
     $.ajax({
         url: "Weather.aspx/refreshII",
         type: "POST",
         data: "{dates:'" + selectDate + "',site:'" + site + "'}",
         contentType: "application/json",
         dataType: "JSON",
         success: function (results) {
             if (results.d != "") {
                 var vs = results.d.toString().split('*');
                 $(vs).each(function (i, n) {
                     var result = n;
                     var v = result.split(':');
                     $("#" + v[0]).val(v[1]);
                     if (v[0].toString().indexOf("pollutant") >= 0) {
                         $("#" + v[0]).combobox("setValue", v[1]);
                     }
                     if (v[0].toString().indexOf("WeaPhenomenaM") >= 0) {
                         $("." + v[0]).each(function (i, n) {
                             $(this).val(v[1]);
                         });
                     }
                     if (v[0].toString().indexOf("WeaPhenomenaN") >= 0) {
                         $("." + v[0]).each(function (i, n) {
                             $(this).val(v[1]);
                         });
                     }
                 });
                 if (idd == "day1") {
                     getWindGrade("PJFS1", "1");
                 } else if (idd == "day2") {
                     getWindGrade("PJFS2", "2");
                 }
                 else if (idd == "day3") {
                     getWindGrade("PJFS3", "3");
                 }

             }
         }

     });
 }


 function Update() {
     clear();
     var site = $("#site").val();
     $.ajax({
         url: "Weather.aspx/refreshIII",
         type: "POST",
         data: "{dates:'" + selectDate + "',site:'" + site + "'}",
         contentType: "application/json",
         dataType: "JSON",
         success: function (results) {
             if (results.d != "") {
                 var vs = results.d.toString().split('*');
                 $(vs).each(function (i, n) {
                     var result = n;
                     var v = result.split(':');
                     $("#" + v[0]).val(v[1]);
                     if (v[0].toString().indexOf("pollutant") >= 0) {
                         $("#" + v[0]).combobox("setValue", v[1]);
                     }
                     if (v[0].toString().indexOf("WeaPhenomenaM") >= 0) {
                         $("." + v[0]).each(function (i, n) {
                             $(this).val(v[1]);
                         });
                     }
                     if (v[0].toString().indexOf("WeaPhenomenaN") >= 0) {
                         $("." + v[0]).each(function (i, n) {
                             $(this).val(v[1]);
                         });
                     }
                 });

                 if (idd == "day1") {
                     getWindGrade("PJFS1", "1");
                 } else if (idd == "day2") {
                     getWindGrade("PJFS2", "2");
                 }
                 else if (idd == "day3") {
                     getWindGrade("PJFS3", "3");
                 }

             }
         }
     });
 }

//第一次进入页面，获取上次订正的数据并显示
//王斌  2017.5.2
function refresh() {

    clear();
    refreshII();
   // getWindGrade("PJFS1", "speed1");
    var site = $("#site").val();
    $.ajax({
        url: "Weather.aspx/refresh",
        type: "POST",
        data: "{dates:'" + selectDate + "',site:'" + site + "'}",
        contentType: "application/json",
        dataType: "JSON",
        success: function (results) {
            
            if (results.d != "") {
                var data = results.d.split("_");
                if (idd == "day1") {
                    var aqi = document.getElementsByName("aqi");
                    var text = document.getElementsByName("text");
                    //document.getElementsByName("speed")[0].value=data[38];
                    //$('#speed1').val(data[38]);
                    if (data[38] != "") {
                        $("#speed1").val(data[38]);
                    }
                    if (data[34] != "") {
                        $("#pollutantM").combobox('setValue', data[34])
                    }
                    if (data[35] != "") {
                        $("#pollutantA").combobox('setValue', data[35])
                    }


                }
                else if (idd == "day2") {
                    var aqi = document.getElementsByName("aqi2");
                    var text = document.getElementsByName("text2");
                    //document.getElementsByName("speed")[1].innerHTML = data[38];
                    if (data[38] != "") {
                        $("#speed2").val(data[38]);
                    }
                    if (data[34] != "") {
                        $("#pollutantM2").combobox('setValue', data[34])
                    }
                    if (data[35] != "") {
                        $("#pollutantA2").combobox('setValue', data[35])
                    }
                }
                else if (idd = "day3") {
                    var aqi = document.getElementsByName("aqi3");
                    var text = document.getElementsByName("text3");
                    //document.getElementsByName("speed")[2].innerHTML = data[38];
                    if (data[38] != "") {
                        $("#speed3").val(data[38]);
                    }
                    if (data[34] != "") {
                        $("#pollutantM3").combobox('setValue', data[34])
                    }
                    if (data[35] != "") {
                        $("#pollutantA4").combobox('setValue', data[35])
                    }
                }

                for (var i = 0; i < text.length; i++) {
                    text[i].value = data[i];
                }
                for (var i = 0; i < aqi.length; i++) {
                    aqi[i].value = data[data.length - 3 + i];
                }

            }

        }

    });

}


//重新计算
function reCalculate() {
   

    if (!confirm("是否要订正预报数据？")) {
        return;
    } 

    var period = 12;
    var date1 = new Date();
    var hour = date1.getHours();
    var content="";
    var site = $("#site").val();
    var ws = "";
    var wind = ""; 
    if (idd == "day1") {
            var text = document.getElementsByName("text");
            for (var i = 0; i < text.length / 2; i++) {
                content += text[i * 2].value;
            }
            ws = document.getElementById("speed1").value;
            var poll = $("#pollutantM").combobox("getText");
            var aqi = document.getElementsByName("aqi")[0].value;
            wind = $("#speed1").val();
        }
    if (idd == "day2") {
            var text = document.getElementsByName("text2");
            for (var i = 0; i < text.length / 2; i++) {
                content += text[i * 2].value;
            }
            ws = document.getElementById("speed2").value;
            var poll = $("#pollutantM2").combobox("getText");
            var aqi = document.getElementsByName("aqi2")[0].value;
            wind = $("#speed2").val();
        }
    if (idd == "day3") {
            var text = document.getElementsByName("text3");
            for (var i = 0; i < text.length / 2; i++) {
                content += text[i * 2].value;
            }
            ws = document.getElementById("speed3").value;
            var poll = $("#pollutantM3").combobox("getText");
            var aqi = document.getElementsByName("aqi3")[0].value;
            wind = $("#speed3").val();
        }

//        var wind =  text[18].value;
//        //判断有没有选择6级风
//        if (text[22].value == "有" || text[23].value == "有")
//            wind = 11;  // 03-27



        $.ajax({
            url: "Weather.aspx/Calc",
            type: "POST",
            data: "{aqi:'" + aqi + "',wind:'" + wind + "',site:'" + site + "',forecastTimes:'" + selectDate
             + "',rain:'" + text[14].value + "',temp:'" + text[16].value + "',cldf:'" + text[20].value + "',hze:'" + text[12].value + "',items:'" + poll + "',windspeed:'"+ws+"',tqxxb:'"+text[10].value+"',tqxxy:'"+text[11].value+"'}",
            contentType: "application/json",
            dataType: "JSON",
            success: function (results) {
                alert("重新计算成功！");
            },
             error: function (ex) {
                alert("重新计算失败，请检查输入内容的格式是否真确");
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

//王斌  2017.5.10
//根据平均风速的值获取风速等级
function getWindGrade(w_speedId, gradeId) {
    var num = $("#" + w_speedId).val();
    var windGrade = "1-2级";
    if (num >= 0 && num <= 1.5) {
        windGrade = "1-2级";
    } else if (num <= 3.3) {
        windGrade = "2-3级";
    } else if (num <= 5.4) {
        windGrade = "3-4级";
    } else if (num <= 7.9) {
        windGrade = "4-5级";
    } else if (num <= 10.7) {
        windGrade = "5-6级";
    } else if (num <= 13.8) {
        windGrade = "6-7级";
    } else if (num <= 17.1) {
        windGrade = "7-8级";
    } else if (num <= 20.7) {
        windGrade = "8-9级";
    } else if (num < 24.4) {
        windGrade = "9-10级";
    } else {
        windGrade = "大于10级";
    }

    $("#speed" + gradeId).val(windGrade);
    //document.getElementById('sel').options[document.getElementById('sel').selectedIndex].value
    // alert(windGrade);
   // $(".selector").val("pxx");
    //$("#speed" + gradeId).find("option[text='" + windGrade + "']").attr("selected", true);
    //$("#speed" + gradeId).find("option[text='yy']").attr("selected", true);
}
