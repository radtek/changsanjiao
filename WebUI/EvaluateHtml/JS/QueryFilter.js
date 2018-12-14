var userName="瞿元昊";
var userLimit="333";
Ext.onReady(function () {
    initInputHighlightScript();
    supportInnerText(); //使得火狐支持innerText
    //显示预报员，预报时间和时次
//    var loginParams = getCookie("UserInfo");
//    var result = Ext.util.JSON.decode(loginParams);
//    userName = result["Alias"];
    //userLimit= result["JB"];
    userLimit = "333";
    if (userLimit != "333") {
        var showImg = Ext.getDom("listTable");
        showImg.style.display = "none";
    }
    $("#yearSelect").on("change", function () {
        var o;
        var value;
        var opt = $(this).find('option');
        opt.each(function (i) {
            if (opt[i].selected == true) {
                o = opt[i].innerHTML;
                value = opt[i].value;
            }
        })
        $(this).find('label').html(o);
        if (value != "")
            trickYearData(value);
    }).trigger('change');

})
function trickYearData(year) {
    trickData(year);
}
function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;
}
function trickData(year) {//coutTable0trMonthClick
    if(userLimit!="333")
    {
          Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.ScoreData', 'retrunSinglePerson'),
            params: { time: year, userName: userName },
            success: function (response) {
                if (response.responseText != "") {
                      $('#coutTable0').html(response.responseText);
                        var showImg = Ext.getDom("coutTable0");
                       showImg.style.display = "block";
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
     }
    else 
    {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'trickMonthData'),
        params: { year: year},
        success: function (response) {
            if (response.responseText != "") {
                $('#coutTable0').html(response.responseText);
                var showImg1 = Ext.getDom("container");
                var showImg = Ext.getDom("coutTable0");
                showImg.style.display = "block";
                showImg1.style.display = "none";
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
   }
}
function trickMonthData(month) {
    var year = $('#yearID option:selected')[0].value;
    trickData(year, month);
}
function trYearClick(time, userName, el) {
    var obj = el;
    var durarion = $(obj).children('td').eq(1).html();
    var haze = $(obj).children('td').eq(2).html();
    var Uv = $(obj).children('td').eq(3).html();
    var china = $(obj).children('td').eq(6).html();
    var totalScore = $(obj).children('td').eq(4).html();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'retrunYearList'),
        params: { time: time, userName: userName, durarion: durarion, haze: haze, Uv: Uv, china: china, totalScore: totalScore },
        success: function (response) {
            if (response.responseText != "") {
                $('#personMonth').html(response.responseText);
                var showImg = Ext.getDom("showImg");
                showImg.className = "show1";
                $('.bg').fadeIn(200);
                $('.showImg').fadeIn(400);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function trDayClick(time, userName, el) {
    var obj = el;
    var durarion = $(obj).children('td').eq(1).html();
    var haze = $(obj).children('td').eq(2).html();
    var Uv = $(obj).children('td').eq(3).html();
    var china = $(obj).children('td').eq(4).html();
    var totalScore = $(obj).children('td').eq(5).html();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'retrunSingleEveryDay'),
        params: { time: time, userName: userName, durarion: durarion, haze: haze, Uv: Uv, china: china, totalScore: totalScore },
        success: function (response) {
            if (response.responseText != "") {
                $('#personDay').html(response.responseText);
                var showImg = Ext.getDom("showEveryDay");
                showImg.className = "show";
                $('.showEveryDay').fadeIn(400);
                $('.bg').fadeIn(200);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function trMonthClick(time, userName) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'retrunSinglePerson'),
        params: { time: time, userName: userName },
        success: function (response) {
            if (response.responseText != "") {
                $('#personMonth').html(response.responseText);
                var showImg = Ext.getDom("showImg");
                showImg.className = "show1";
                $('.bg').fadeIn(200);
                $('.showImg').fadeIn(400);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
$('.bg').click(function () {
    var showImg = Ext.getDom("showImg");
    var showImg1 = Ext.getDom("showEveryDay");
    if (showImg1.className == "show") {
        $('.showEveryDay').fadeOut(800);
        showImg1.className = "hidden";
        showImg.className = "show1";
    }
    else {
        $('.showImg').fadeOut(800);
        showImg.className = "hidden";
        $('.bg').fadeOut(800);
    }

});
function fadeOut() {
    var showImg = Ext.getDom("showImg");
    var showImg1 = Ext.getDom("showEveryDay");
    if (showImg1.className == "show") {
        $('.showEveryDay').fadeOut(800);
        showImg1.className = "hidden";
        showImg.className = "show1";
    }
    else {
        $('.showImg').fadeOut(800);
        showImg.className = "hidden";
        $('.bg').fadeOut(800);
    }
}
function radioClick(id) {
    var el = Ext.getDom(id);
    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        el = getGroupEl(id);
        el.className = "radioUnChecked";
    }
    var year = $('#yearID option:selected')[0].value;
    if (id == "rd1")
        trickData(year);
    else {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.ScoreData', 'trickMonthChart'),
            params: { year: year},
            success: function (response) {
                if (response.responseText != "") {
                    var showImg1 = Ext.getDom("container");
                    var showImg = Ext.getDom("coutTable0");
                    RenderChart(response.responseText);
                    showImg.style.display = "none";
                    showImg1.style.display = "block";
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }

}
function ConvertToFloat(x) {
    var floatTemp = parseFloat(x).toFixed(1);
    var floatValue = parseFloat(floatTemp);
    return floatValue;

}
function RenderChart(result) {
    var yTitle = "";
    var ArrayData = new Array();

    var cataData = new Array();
    var cl = result.split('*');
    if (cl.length == 2) {
        var clx = cl[0].split('|');
        var cly = cl[1].split('|');
        for (var k = 0; k < clx.length; k++) {
            cataData[k] = clx[k];
            ArrayData[k] = ConvertToFloat(cly[k]);
        }
    }
    // create the chart
    $('#container').highcharts({
        chart: {
            type: 'column'
        },
        credits: { enabled: false },
        title: {
            text: ""
        },
        global: { useUTC: false },
        exporting: {
            enabled: false
        },
        xAxis: {
            categories: cataData
        },
        legend: {
            enabled: false
        },
        yAxis:
            [{
                title: { text: "个人得分" },
                offset: 0,
                lineWidth: 2,
                max: 100
            }],
        plotOptions: {
            column: {
                cursor: 'pointer',
                stacking: 'normal',
                borderWidth: 0,
                point: {
                    events: {
                        click: function() {
                        var month = $('#monthID option:selected')[0].value;
                        var year = $('#yearID option:selected')[0].value;
                        trMonthClick(year+month,this.category);
                            
                        }
                    }
                }
            }
        },
        series:
            [{
                name: '得分',
                data: ArrayData

            }]
    });
}
function getGroupEl(id) {
    var groupEl = "";
    switch (id) {
        case "rd1":
            groupEl = "rd2";
            break;
        case "rd2":
            groupEl = "rd1";
            break;
    }
    return Ext.getDom(groupEl);
}
function ExportTable()
{
    var year = $('#yearID option:selected')[0].value;
    var content = year+"|"+userLimit+"|"+userName;
    var Element = document.getElementById("Element");
    Element.setAttribute("value", content);
    document.getElementById("btnExport").click();
}