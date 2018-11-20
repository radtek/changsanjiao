// JScript 文件
var oldCity = "c1";
var oldCityName="上海市"
Ext.onReady(function () {
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doQueryChart("上海市");
    //$('#selCity').combobox('disable');   //不可用

    $("#abc li").click(function () {
        var categoryIndex = $(this).attr("index");
        showLi(categoryIndex);

    });

    $("input[name='CheckProvince']").click(function () {
        if ($(this).attr("checked") == "checked") {
            $("input[title*=" + this["title"] + "]").each(function () {
                $(this).attr("checked", "checked");
            });
        }
        else {
            $("input[title*=" + this["title"] + "]").each(function () {
                $(this).attr("checked", false);
            });
        }

    });
 }
)

function doQueryChart(citys) {

    citys = getCheckBValue("CheckCity2");
    if (citys == "") {
        alert("请选择城市！");
        return;
    }
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var cityName= Ext.getDom("city").value;
    var duration="10";
    if(Ext.getDom("day").checked)
        duration = "7";

    var count=citys.split(',').length;

    var model = "";
    $.each($(".textbox-value"), function (i, n) {
        model=$(n).val();
    });
        
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在查询中，请稍候..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.CitysForecast', 'GetAQIChartDBIII'),
        params: { fromDate: fromDate, toDate: toDate, city: citys, duration: duration, model: model },
        success: function (response) {
            if (response.responseText != "") {
                var sps = response.responseText.toString().split("&");
                var i = 0;
                for (var v in sps) {
                    try {
                        if (sps[i] != "") {
                            var result = Ext.util.JSON.decode(sps[i]);
                            if (duration == "10")
                                RenderChart(result, i, "#container" + (i + 1) + "", count);
                            else
                                RenderChartDay(result, i, "#container" + (i + 1) + "", count);
                        }
                        i++;
                    } catch (exception) { myMask.hide(); }
                }
            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。");

            myMask.hide();
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
 
}

function ConvertToFloat(x) {
  if (x != "null") {
        var floatTemp = parseFloat(x).toFixed(1);
        var floatValue = parseFloat(floatTemp);
        if(floatValue>0)
            return floatValue;
        else
            return null;
    }
    else
        return null;

}

function RenderChart(result,lineName,contonter,count) {
      var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var chartName = "";
    if (lineName == 0)
        lineName = "PM2.5";
    if (lineName == 1)
        lineName = "PM10";
    if (lineName == 2)
        lineName = "O3";
    if (lineName == 3) 
        lineName = "NO2";
    if (lineName == 4) 
        lineName = "CO";
    if (lineName == 5) 
        lineName = "SO2";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var CuaceArray = new Array(); var ChemArray = new Array();
    var CmaqArray = new Array(); var AqiArray = new Array(); 
    var CMAQ10Array = new Array(); 

    var indexData = 5;
    var series = [];
    var temp = [];
    for (var k = 0; k < count; k++) {
        try {
            var AqiArray = new Array(); 
            if (result[k] != null && result[k] != "*") {
                var Pd = result[k].split('*');
                if (Pd.length == 3) {
                    var Pdx = Pd[0].split('|');
                    var Pdy = Pd[1].split('|');
                    var Pdz = Pd[2].split('|');
                    for (var i = 0; i < Pdx.length; i++) {
                        var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]), z: Pdz[i] };
                        AqiArray[i] = tmp;
                    }
                }
            }
            else {
                AqiArray = arrayTmp;
            }

            temp.push(AqiArray);
        } catch (exception) { }
      }

      try {
          for (var i in temp) {
              try {
                  var name = temp[i][0].z;
                  series.push({ "name": name, "data": temp[i] });
              } catch (exception) { }
          }
      } catch (exception) { }

        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
        // create the chart
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
        $(contonter).highcharts({
            chart: {
               type: 'spline'
            },
            credits: { enabled: false },
            title: {
                text: LineName,
                style: {
                    fontSize: 18,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
             exporting:{
               url: 'http://export.hcharts.cn'
            },
            global: { useUTC: false },
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%Y-%m-%d, %H'//鼠标移动到趋势线上时显示的日期格式 
            },
       
          xAxis: {
            type: 'datetime',
            tickInterval: 24 * 3600 * 1000,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%m月', this.value) + "" + Highcharts.dateFormat('%d日', this.value);
                    return tipMessage;
                }
            },
            offset: 0,
            lineColor: '#473C8B',
            lineWidth: 2,
            minorGridLineWidth: 0,
            minorTickInterval: 6 * 3600 * 1000,
            minorTickWidth: 1,
            minorTickLength: 5
        },

          legend: {

            borderWidth: 0
        },
         yAxis: { // Primary yAxis
                lineColor: '#473C8B',
                lineWidth: 2,
                style: {
                    color: '#473C8B'
                },
                title: {
                    text: '（ug/m3）',
                    style: {
                        color: '#080808'
                    }
                },
                showEmpty: false
            },

            plotOptions: {
                series: {
                  connectNulls: true
                } ,
              spline: {
                  lineWidth: 2,
                  states: {
                      hover: {
                          lineWidth: 1
                      }
                  },
                  marker: {
                      enabled: true,
                      radius: 3,
                      symbol: 'circle',
                      lineWidth: 1
                  }
              }
            },
            series: series
        });
    
}



function RenderChartDay(result,lineName,contonter,count) {
      var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var chartName = "";
    if (lineName == 0)
        lineName = "PM2.5";
    if (lineName == 1)
        lineName = "PM10";
    if (lineName == 2)
        lineName = "O3";
    if (lineName == 3) 
        lineName = "NO2";
    if (lineName == 4) 
        lineName = "CO";
    if (lineName == 5) 
        lineName = "SO2";
    
    var LineName = lineName;
    var dblMax = 0;
    var dblMin = 10000;
    var CuaceArray = new Array(); var ChemArray = new Array();
    var CmaqArray = new Array(); 
    var CMAQ10Array = new Array(); 

    var indexData = 5;


    var series = [];
    var temp = [];
    for (var k = 0; k < count; k++) {
        try {
            var AqiArray = new Array(); 
            if (result[k] != null && result[k] != "*") {
                var Pd = result[k].split('*');
                if (Pd.length == 3) {
                    var Pdx = Pd[0].split('|');
                    var Pdy = Pd[1].split('|');
                    var Pdz = Pd[2].split('|');
                    for (var i = 0; i < Pdx.length; i++) {
                        var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]), z: Pdz[i] };
                        AqiArray[i] = tmp;
                    }
                }
            }
            else {
                AqiArray = arrayTmp;
            }

            temp.push(AqiArray);
        } catch (exception) { }
      }

      try {
          for (var i in temp) {
              try {
                  var name = temp[i][0].z;
                  series.push({ "name": name, "data": temp[i] });
              } catch (exception) { }
          }
      } catch (exception) { }
      

        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;

        // create the chart
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

   

        $(contonter).highcharts({
            chart: {
               type: 'spline'
            },
            credits: { enabled: false },
            title: {
                text: LineName,
                style: {
                    fontSize: 18,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
             exporting:{
               url: 'http://export.hcharts.cn'
            },
            global: { useUTC: false },
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%Y-%m-%d'//鼠标移动到趋势线上时显示的日期格式 
            },
       
          xAxis: {
              type: 'datetime',
            tickInterval: 24 * 3600 * 1000,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%m月', this.value) + "<br/>" + Highcharts.dateFormat('%d日', this.value);

                    return tipMessage;
                }
            },
            offset: 0,
            lineColor: '#473C8B',
            lineWidth: 2,
            minorGridLineWidth: 0,
            minorTickInterval: 6 * 3600 * 1000,
            minorTickWidth: 1,
            minorTickLength: 5
        },

        legend: {

            borderWidth: 0
        },
         yAxis: { // Primary yAxis
                lineColor: '#473C8B',
                lineWidth: 2,
                style: {
                    color: '#473C8B'
                },
                title: {
                    text: '（ug/m3）',
                    style: {
                        color: '#080808'
                    }
                },
                showEmpty: false
            },

            plotOptions: {
          series: {
                  connectNulls: true
                }  
                ,
              spline: {
                  lineWidth: 2,
                  states: {
                      hover: {
                          lineWidth: 1
                      }
                  },
                  marker: {
                      enabled: true,
                      radius: 3,
                      symbol: 'circle',
                      lineWidth: 1
                  }
              }
            },
            series: series
        });

}

function clickQuery(){
       doQueryChart(oldCityName);
}

function radioClickModule(id,name) {
    var el = Ext.getDom(id);

    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        doQueryChart(name);
        if (oldCity != "") {
            var oldObj = Ext.getDom(oldCity);
            oldObj.className = "radioUnChecked";
            oldCity = id;
            oldCityName=name;
        }
    }
}

function showCityPanel2() {
    var obj = Ext.getDom("cs");
    var divUsers = Ext.getDom("cityHtml2");
    divUsers.style.left = getElementLeft(obj, divUsers.parent) + "px";
    divUsers.style.top = getElementTop(obj, divUsers.parent) + obj.offsetHeight + "px";
    divUsers.style.display = "block";
    divUsers.style.zIndex = 100;
}

function showLi(categoryIndex) {
    $(".popcitylist").hide();
    $("#abc li").removeClass("action");
    $("#nav_list" + categoryIndex).addClass("action");
    $("#ul_list" + categoryIndex).show();
}

function OKSelecet2() {
    Ext.getDom("cityHtml2").style.display = "none";

}

function closeCheck2() {
    Ext.getDom("cityHtml2").style.display = "none";
}

function del() {
    $("input[name='CheckProvince']").attr("checked", false);
    $("input[name='CheckCity2']").attr("checked", false);
}


function getCheckBValue(objName) {
    var postJson = "";
    var obj = document.getElementsByName(objName);
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].checked) {
                postJson = postJson + obj[i].value + ",";
            }
        }
    }
    if (postJson.length > 0) {
        postJson = postJson.substring(0, postJson.length - 1);
    }
    return postJson;
}