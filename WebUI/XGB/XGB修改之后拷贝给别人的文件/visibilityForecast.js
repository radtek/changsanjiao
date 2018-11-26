/// <reference path="../../EvaluateHtml/JS/jquery-1.9.1.js" />
/// <reference path="../../AQI/js/Leaflet/leaflet-src.js" />
var map;
var siteData;
var weatherTypes = "";
var siteLayer = "";
var openPopup = "";
var siteCode = ""; //记录点击地图上站点的编号
var hours = "24"; //默认选中“24小时”的radio
var startTime = "";
var endTime = "";

$(function () {
  creatMapCommon();
  getLastForecastTimeVisValue() //获取ForecastTime的最新时间(利用能见度值表中的最新时间来赋值时间输入框的起始时间)
  getLastLstTimeVisValue(); //获取Lst的最新时间(利用能见度值表中的Lst)
  querySiteData();

  //生成表格
  createTable();

  $(".glyphicon").click(function () {
    $("#sitePanel").css("display", "none");
  })


  //var content = L.DomUtil.get('TimeBar');
  var content2 = L.DomUtil.get('sitePanel');
  //var content3 = L.DomUtil.get('layerPanel');
  var stop = L.DomEvent.stopPropagation;
  var fakeStop = L.DomEvent._fakeStop || stop;
  L.DomEvent

    .on(content2, 'contextmenu', stop)
    .on(content2, 'click', fakeStop)
    .on(content2, 'mousedown', stop)
    .on(content2, 'touchstart', stop)
    .on(content2, 'dblclick', fakeStop)
    .on(content2, 'mousewheel', stop)
    .on(content2, 'MozMousePixelScroll', stop)

  $('#sitePanel').draggable();




  //为radio注册点击事件  更新由能见度等级算出来的table
  $(".radioHours").click(function () {
    if ($(".radioHours").is(":checked")) {
      hours = $(this).val();
      //更新能见度等级的柱状图和由能见度等级值计算出来的表
      getChartDataAndTableData();
      //利用能见度等级表中最新的事件


    }
  });

  $("#closeButton").click(function () {
    $("#Radio1").prop('checked', true);
    $("#Radio2").prop('checked', false)
    siteCode = "";

    //$("#Radio1").attr("checked", "checked");
    //$("#Radio2").removeAttr("checked");

    //$("#Radio1").attr("checked", "checked");
    //$("#Radio2").attr("checked", false);

    //$("input[name='optionsRadiosinline']:eq(0)").attr("checked", 'checked');
    //$("input[name='optionsRadiosinline']:eq(1)").attr("checked", 'checked');
    $("#sitePanel").css("display", "none");
    $("#legend").css("display", "none");
    chartData = [];
    chartData1 = [];
    chartData2 = [];
    chartData3 = [];
    chartData4 = [];
    chartData5 = [];
    $("#chartContainer").html("");

  });

  //每个Marker点的面板中选择显示不同类型数据的图表
  $(".chartType").click(function () {
    var chartType = $("input[name='optionsRadiosinline']:checked").val();
    $("#chartContainer").html("");
    $("#chartContainer").mLoading("show");
    switch (chartType) {
      case "visClass":
        //getLastForecastTime();//获取ForecastTime的最新时间
        //getLastLstTime();//获取Lst的最新时间
        //加载柱状图之前清除折线图
        chartData1 = []; //预报数据  高能见度预测模型
        chartData2 = []; //实况数据
        chartData3 = []; //低能见度预测模型
        chartData4 = []; //融合模型  lightgbm_predicted
        chartData5 = []; //lightgbm_predicted
        getChartData(siteCode);
        break;
      case "visValue":
        //getLastForecastTimeVisValue();
        //getLastLstTimeVisValue();
        //加载折线图之前清除柱状图
        chartData = [];
        //$("#chartContainer").html("");
        getVisValueData(siteCode);
        break;

    }
  })

})


function creatMapCommon() {
  map = L.map('map', {
    center: [31.187649, 119.091256],
    zoom: 5.6, //上海10    长江流域6.5
    maxZoom: 19, //原版为11  
    minZoom: 4
    //    crs:m_crs
  });


  //L.esri.tiledMapLayer({
  //  url: "http://139.196.174.214/arcgis/rest/services/WorldMap_Blue_Fill/MapServer "
  //}, {
  //  attribution: ''
  //}).addTo(map).setZIndex(0);

  //L.esri.tiledMapLayer( //tiledMapLayer加载切片地图服务
  //  {
  //    url: "http://139.196.174.214/arcgis/rest/services/WorldMap_Blue_Border/MapServer "
  //  }, {
  //    attribution: ''
  //  }).addTo(map).setZIndex(100);
  //L.esri.dynamicMapLayer( //dynamicMapLayer加载动态地图服务
  //  {
  //    url: "http://139.196.174.214/arcgis/rest/services/WorldMap_Blue_Label/MapServer "
  //  }, {
  //    attribution: ''
    //  }).addTo(map);

    //更换薛辉给的地图
  L.esri.tiledMapLayer({
      url: "http://10.228.8.199/pbs/rest/services/ChinaOnlineStreetGray/MapServer/"
  }, {
      attribution: ''
  }).addTo(map).setZIndex(0);
  L.esri.tiledMapLayer({
      url: "http://10.228.8.199/pbs/rest/services/ChinaOnlineStreetGray/MapServer/tile/{z}/{y}/{x}"
  }, {
      attribution: ''
  }).addTo(map).setZIndex(0);


  map.on('click', function (ev) {
    var x1 = ev.containerPoint.x;
    var y1 = ev.containerPoint.y;
    for (var i = 0; i < siteData.features.length; i++) {
      var neighborPoint = map.latLngToContainerPoint([siteData.features[i].properties.Lat, siteData.features[i].properties.Lon]); //将所有marker点的经纬度坐标转化为屏幕坐标
      var x0 = neighborPoint.x,
        y0 = neighborPoint.y;
      var calX = x0 - x1; //当前点减邻近点
      var calY = y0 - y1;
      if (Math.pow((calX * calX + calY * calY), 0.5) < 10) { ////寻找到当前悬浮的点，即，如果两点之间的距离小于10，就执行
        //map.removeLayer(rainChartMarkerHover);//移除创建的Marker的悬浮效果

        //生成面板之前将radio切换一下
        $("#Radio1").prop('checked', true);
        $("#Radio2").prop('checked', false)
        //创建rain的chart图表
        creatRainChart(siteData.features[i], ev.containerPoint, weatherTypes);
        $("#legend").css("display", "block");
        break;
      }
    }
  });
  map.on("zoomstart", function () {
    clearCanvas(siteLayer);
  });
  map.on("zoomend", function () {
    drawSurface("Vis");
  });
  map.on('mousemove', function (ev) {
    $(".leaflet-canvas-icon2").css("cursor", "-webkit-grab"); //设置地图上的“咸猪手”
    var x1 = ev.containerPoint.x; // ev is an event object (MouseEvent in this case)
    var y1 = ev.containerPoint.y;
    if (siteData != undefined && siteData.features.length != 0) {
      for (var i = 0; i < siteData.features.length; i++) {
        var neighborPoint = map.latLngToContainerPoint([siteData.features[i].properties.Lat, siteData.features[i].properties.Lon]);
        var x0 = neighborPoint.x,
          y0 = neighborPoint.y;
        var calX = x0 - x1;
        var calY = y0 - y1;
        if (Math.pow((calX * calX + calY * calY), 0.5) < 10) {

          $(".leaflet-canvas-icon2").css("cursor", "pointer"); //将鼠标由“咸猪手”变成“指向手形”
          var popup = L.popup().setLatLng([siteData.features[i].properties.Lat, siteData.features[i].properties.Lon]).setContent("<p>" + siteData.features[i].properties.name + "  (" + siteData.features[i].properties.Code + ")</p>");
          openPopup = popup.openOn(map); //注意：要定义一个全局对象openPopup=L.popup;
          break;
        }

      }
    }
  });
  map.on('mouseout', function () {
    map.removeLayer(openPopup); //移除地图上打开的openPopup
  });
}
/**
 *功能：创建rain的chart图表                          
 *@param {Object} feature siteData.features[i] 后台传过来的数据整理成json格式后的值  
 *@param {Object} containerPoint ev.containerPoint  当前marker点的容器屏幕坐标
 *@param {string} weatherType  Rain  各种天气类型  根据天气类型来创建不同的面板
 */
function creatRainChart(feature, containerPoint, weatherType) {
  var mapHeight, mapWidth;
  var pageHeight = document.documentElement.clientHeight;
  mapHeight = pageHeight - 75;
  var pageWidth = document.documentElement.clientWidth;
  mapWidth = pageWidth - 80;

  //定义函数字面量    
  var creatPanel = function (point) {
    var topX, topY;
    if ((point.x + 680) > mapWidth) { //地图上最右边的marker点加上680也超过了mapWidth，就设置面板距离
      topX = mapWidth - 700;
    } else {
      topX = point.x + 10;
    }
    if ((point.y + 310) > mapHeight) {
      topY = mapHeight - 300;
    } else {
      topY = point.y + 10;
    }
    $("#sitePanel").css("top", topY);
    $("#sitePanel").css("left", topX); //距离地图容器最左边的距离
    $("#sitePanel").css("display", "block");
  }

  var creatChart = function (feature, weatherType) {
    switch (weatherType) {
      case "Vis":
        var panelTime = "";
        siteCode = feature.properties.Code.replace(/(^\s*)|(\s*$)/g, "");
        var siteName = feature.properties.name;
        $("#panelTitle").html(siteName + "(" + siteCode + ")");
        //var typeName = "";
        //$("#typeName").html(typeName);
        ////获取当前点击点的ID，预报信息表中的数据生成折线图
        getChartData(siteCode);
        break;

        //继续case   break

    }
  }

  $("#chartContainer").mLoading("show"); //生成chart面板之前显示loading效果
  creatPanel(containerPoint);
  creatChart(feature, weatherType);
}
var chartData = [];
//生成能见度等级的柱状图
function getChartData(StationID) {
  //startTime = "2018-07-31 06:00:00";//待修改，获取输入框中的值
  //endTime = "2018-07-31 10:00:00";

  startTime = $("#startDate").val() + ":00:00";
  endTime = $("#endDate").val() + ":00:00";




  var timeArray = [];
  var timeMMDDArray = [];
  var degArray = [];
  var xAxisValue;
  var yAxisValue; //注意：由于大多数数值都为0，所以，x轴的起始值可以放到y轴等于-1的点上  （待做）

  //定义5个能见度等级数组
  var r0 = [],
    r1 = [],
    r2 = [],
    r3 = [],
    r4 = [],
    r5 = [],
    rn = [];

  if (siteCode != "") {
    $.ajax({
      url: "visibilityForecast.aspx/GetChartData", //利用服务器数据
      type: "POST",
      contentType: "application/json",
      dataType: 'json',

      //url: "http://localhost:32778/WebUI/XGB/data/clickData.txt",//利用本地数据
      //type: "get",
      //dataType:"text",

      data: "{ startTime:'" + startTime + "',endTime:'" + endTime + "',codeId:'" + StationID + "',hours:'" + hours + "' }",
      success: function (responseData) {
        //console.log(responseData);
        $("#chartContainer").mLoading("hide")
        var data = responseData.d.rows; //利用服务器数据

        //var data = JSON.parse(responseData);//利用本地的数据

        if (data != null) {
          ////对原始数据中不等null的值都加1，形成新的数组   
          //var data = [];
          //for (var j = 0; j < dataRes.length; j++) {
          //    if (dataRes[j].vis_scale_predicted != null) {
          //        data.push(dataRes[j].vis_scale_predicted + 1);
          //    } else {
          //        data.push(dataRes[j].vis_scale_predicted);
          //    }
          //}

          for (var i = 0; i < data.length; i++) {
            var rv = data[i].vis_scale_predicted; //浓度等级值
            if (rv != null) {
              rv = rv + 1;
            }

            //var rv = data[i];//浓度等级值
            if (i == 0) { //之所以要加i==0，是因为，起始时间采用的是forecastdate
              if (rv == 0) {
                r0.push(rv);
              } else {
                r0.push(null);
              }
              if (rv == 1) {
                r1.push(rv);
              } else {
                r1.push(null);
              }
              if (rv == 2) {
                r2.push(rv);
              } else {
                r2.push(null);
              }
              if (rv == 3) {
                r3.push(rv);
              } else {
                r3.push(null);
              }
              if (rv == 4) {
                r4.push(rv);
              } else {
                r4.push(null);
              }
              if (rv == 5) {
                r5.push(rv);
              } else {
                r5.push(null);
              }
              if (rv == null) {
                rn.push(rv);
              } else {
                rn.push(null);
              }

              timeArray.push(jsonDateFormat(data[i].forecastdate));
              timeMMDDArray.push(jsonDateFormatWithMMDDHH(data[i].forecastdate));
            } else {
              if (rv == 0) {
                r0.push(rv);
              } else {
                r0.push(null);
              }
              if (rv == 1) {
                r1.push(rv);
              } else {
                r1.push(null);
              }
              if (rv == 2) {
                r2.push(rv);
              } else {
                r2.push(null);
              }
              if (rv == 3) {
                r3.push(rv);
              } else {
                r3.push(null);
              }
              if (rv == 4) {
                r4.push(rv);
              } else {
                r4.push(null);
              }
              if (rv == 5) {
                r5.push(rv);
              } else {
                r5.push(null);
              }
              if (rv == null) {
                rn.push(rv);
              } else {
                rn.push(null);
              }


              timeArray.push(jsonDateFormat(data[i].lst));
              timeMMDDArray.push(jsonDateFormatWithMMDDHH(data[i].lst));
            }
            //chartData.push(data[i].vis_scale_predicted);
            //timeArray.push(jsonDateFormat(data[i].lst));
            //timeMMDDArray.push(jsonDateFormatWithMMDD(data[i].lst));

          }
          var xAxisValue = timeArray;
          //var xAxisValue = timeMMDDArray;
          var chartData = getSeriesData();

          function getSeriesData() {
            return [{
              color: '#00FF00',
              name: '0 (>5000m)',
              data: r1,
            }, {
              color: '#FEAA00',
              name: '1 (3000-5000m)',
              data: r2,
            }, {
              color: '#FA0101',
              name: '2 (1000-3000m)',
              data: r3,
            }, {
              color: '#C103F9',
              name: '3 (500-1000m)',
              data: r4,
            }, {
              color: '#712201',
              name: '4 (<500m)',
              data: r5,
            }];
          }

          $(function () {
            drawChart = Highcharts.chart('chartContainer', {
              chart: {
                type: 'column',
                height: 300,
                width: 590,
                //height: 200,
                //width: 890,
                backgroundColor: ''
              },
              credits: {
                enabled: false
              },
              title: {
                useHTML: true,
                text: '',
                margin: 10,
                style: {
                  fontFamily: '微软雅黑',
                  fontSize: '10px'
                }
              },
              subtitle: {
                text: '',
                align: 'right',
                style: {
                  fontFamily: '微软雅黑',
                  color: '#ba8444',
                  fontSize: '12px'
                }
              },
              showInLegend: false,
              legend: {
                ////align: 'left',
                //verticalAlign: 'bottom',

                ////layout: 'vertical',
                ////align: 'left',
                ////verticalAlign: 'top',
                ////y: 2,

                ////x: 0,
                ////y: -15//当 y 值为负值时，图例往上偏移；正值时，图例往下偏移。 默认是：0

                //title: {
                //    text: '能见度等级',
                //    style: {
                //        fontWeight: "bold"
                //    }
                //},
                //symbolRadius: 0,//将图标设置为方形
                //itemMarginTop: 5,//图例间距

                enabled: false
              },
              xAxis: {
                //lineWidth: 1.5,
                //lineColor: '#3780B5',
                //tickInterval: 4,//时间间隔
                //gridLineWidth: 0.5,//网格线线条宽度
                //gridLineColor: '#C5D2E2',
                //tickWidth: 2.5,
                //tickColor: '#3780B5',
                //tickLength: 5,
                //categories: xAxisValue,
                //labels: {
                //    y: 20, //x轴刻度往下移动20px
                //    style: {

                //        fontSize: '12px'  //字体
                //    }
                //}

                categories: xAxisValue
              },
              yAxis: {
                floor: 0,
                offset: 0,
                gridLineWidth: 0.5,
                gridLineColor: '#C5D2E2',
                lineWidth: 1.5,
                lineColor: '#3780B5',
                min: 0,
                tickWidth: 2.5,
                tickColor: '#3780B5',
                tickLength: 5,
                title: {
                  text: null
                },
                labels: {
                  x: -10, //y轴刻度往左移动10px
                  y: -10,
                  style: {

                    fontSize: '12px' //字体
                  }
                },
                tickPositions: [0, 1, 2, 3, 4, 5], // 指定竖轴坐标点的值
                showLastLabel: false //是否显示最后一个轴标签
              },
              plotOptions: {
                column: {
                  //将多个柱状图堆叠起来作为一个显示
                  stacking: 'normal',
                  pointWidth: 5, //柱子宽度
                }
              },
              series: chartData,
              tooltip: {
                shared: true,
                //useHTML: true,
                headerFormat: '<small></small><table>',
                pointFormatter: function () {
                  //console.log(this.index);
                  //console.log(this);
                  return '<p>' + timeMMDDArray[this.index] + '</p> <p>' +
                    this.series.name
                },

                valueDecimals: 2
              }
            });
          });
        } else {
          alert("此时间段无数据！");
        }

      },
      error: function (e) {
        console.log(e);
      }
    });
  } else {
    alert("请先点击地图上的站点！");
  }

}

var chartData1 = []; //预报数据  高能见度预测模型
var chartData2 = []; //实况数据
var chartData3 = []; //低能见度预测模型
var chartData4 = []; //融合模型  lightgbm_predicted
var chartData5 = []; //lightgbm_predicted
//根据能见度值生成折线图（切换radio时，调用）
function getVisValueData(StationID) {
  startTime = $("#startDate").val() + ":00:00";
  endTime = $("#endDate").val() + ":00:00";

  var lstTime = ""; //将预报时效中的最后一次时间给实况，用于实况数据的查询

  var timeArray = [];
  var timeMMDDArray = [];
  var degArray = [];
  //var xAxisValue;
  var yAxisValue; //注意：由于大多数数值都为0，所以，x轴的起始值可以放到y轴等于-1的点上  （待做）


  var xAxisValue1 = []; //生成折线图的x轴，采用预报数据中的时间作为x轴


  if (siteCode != "") {
    //请求预报的数据
    $.ajax({
      url: "visibilityForecast.aspx/GetVisValueData", //利用服务器数据
      type: "POST",
      contentType: "application/json",
      dataType: 'json',
      async: false,

      //url: "http://localhost:32778/WebUI/XGB/data/clickData.txt",//利用本地数据
      //type: "get",
      //dataType:"text",

      data: "{ startTime:'" + startTime + "',endTime:'" + endTime + "',codeId:'" + StationID + "',hours:'" + hours + "' }",
      success: function (responseData) {
        //console.log(responseData);

        var data = responseData.d.rows; //利用服务器数据

        //var data = JSON.parse(responseData);//利用本地的数据

        if (data != null) {
          for (var i = 0; i < data.length; i++) {

            if (i == 0) {
              chartData1.push(parseFloat(((data[i].vishigh_value_predicted)).toFixed(2)) / 1000);
              chartData3.push(parseFloat(((data[i].vislow_value_predicted)).toFixed(2)) / 1000);
              chartData4.push(parseFloat(((data[i].merge)).toFixed(2)) / 1000);
              timeArray.push(jsonDateFormat(data[i].forecastdate));
              timeMMDDArray.push(jsonDateFormatWithMMDDHH(data[i].forecastdate));
            } else {
              chartData1.push(parseFloat(((data[i].vishigh_value_predicted)).toFixed(2)) / 1000);
              chartData3.push(parseFloat(((data[i].vislow_value_predicted)).toFixed(2)) / 1000);
              chartData4.push(parseFloat(((data[i].merge)).toFixed(2)) / 1000);
              timeArray.push(jsonDateFormat(data[i].lst));
              timeMMDDArray.push(jsonDateFormatWithMMDDHH(data[i].lst));
            }
            if (i == data.length - 1) {
              lstTime = jsonDateFormatWithMMDDHH(data[i].lst);
            }

            //chartData.push(data[i].vis_scale_predicted);
            //timeArray.push(jsonDateFormat(data[i].lst));
            //timeMMDDArray.push(jsonDateFormatWithMMDD(data[i].lst));

          }
          xAxisValue1 = timeArray;

        } else {
          alert("此时间段无数据！");
        }

      },
      error: function (e) {
        console.log(e);
      }
    });

    //请求实况数据
    $.ajax({
      url: "visibilityForecast.aspx/GetVisRealValueData", //利用服务器数据
      type: "POST",
      contentType: "application/json",
      dataType: 'json',
      async: false,

      //url: "http://localhost:32778/WebUI/XGB/data/clickData.txt",//利用本地数据
      //type: "get",
      //dataType:"text",

      data: "{ startTime:'" + startTime + "',endTime:'" + lstTime + "',codeId:'" + StationID + "',hours:'" + hours + "' }",
      success: function (responseData) {
        //console.log(responseData);

        var data = responseData.d.rows; //利用服务器数据

        //var data = JSON.parse(responseData);//利用本地的数据

        if (data != null) {
          for (var i = 0; i < data.length; i++) {

            if (i == 0) {
              chartData2.push(parseFloat(((data[i].vis)).toFixed(2)) / 1000);

            } else {
              chartData2.push(parseFloat(((data[i].vis)).toFixed(2)) / 1000);

            }

            //chartData.push(data[i].vis_scale_predicted);
            //timeArray.push(jsonDateFormat(data[i].lst));
            //timeMMDDArray.push(jsonDateFormatWithMMDD(data[i].lst));

          }



        } else {
          alert("此时间段无数据！");
        }

      },
      error: function (e) {
        console.log(e);
      }
    });

    //请求lightgbm_predicted数据
    $.ajax({
      url: "visibilityForecast.aspx/GetLightgbmData", //利用服务器数据
      type: "POST",
      contentType: "application/json",
      dataType: 'json',
      async: false,

      //url: "http://localhost:32778/WebUI/XGB/data/clickData.txt",//利用本地数据
      //type: "get",
      //dataType:"text",

      data: "{ startTime:'" + startTime + "',endTime:'" + lstTime + "',codeId:'" + StationID + "',hours:'" + hours + "' }",
      success: function (responseData) {
        //console.log(responseData);

        var data = responseData.d.rows; //利用服务器数据

        //var data = JSON.parse(responseData);//利用本地的数据

        if (data != null) {
          for (var i = 0; i < data.length; i++) {

            if (i == 0) {
              chartData5.push(parseFloat(((data[i].lightgbm_predicted)).toFixed(2)) / 1000);

            } else {
              chartData5.push(parseFloat(((data[i].lightgbm_predicted)).toFixed(2)) / 1000);

            }

          }



        } else {
          alert("此时间段无数据！");
        }

      },
      error: function (e) {
        console.log(e);
      }
    });

    if (chartData1.length > 0 && chartData2.length > 0 && chartData3.length > 0 && chartData4.length > 0 && chartData5.length > 0) {
      $("#chartContainer").mLoading("hide")
      //根据请求到的数据生成折线图
      $(function () {
        drawChart = Highcharts.chart('chartContainer', {
          chart: {
            type: 'line',
            height: 300,
            width: 590,
            backgroundColor: '#FBFBFB'
          },
          credits: {
            enabled: false
          },
          title: {
            useHTML: true,
            text: '',
            margin: 10,
            style: {
              fontFamily: '微软雅黑',

              fontSize: '10px'


            }
          },
          subtitle: {
            //text: '(°C)',
            align: 'left',
            style: {
              fontFamily: '微软雅黑',
              color: 'black',
              fontSize: '12px'
            },
            floating: true,
            y: 10,
            x: 10
          },
          legend: {
            //align: 'right',
            //verticalAlign: 'bottom',
            //x: 0,
            //y: 100,
            enabled: true
          },
          xAxis: {

            tickWidth: 0, //刻度标签宽度

            lineWidth: 1.5,
            lineColor: '#3780B5',
            tickInterval: 4,
            gridLineWidth: 0.5,
            gridLineColor: '#C5D2E2',
            categories: xAxisValue1,
            //labels: {
            //    y: 34, //x轴刻度往下移动20px
            //    style: {

            //        fontSize: '12px'  //字体
            //    }
            //}
          },
          yAxis: {
            //floor: 0,//不设置y轴的下限，即，当温度为负数时也能显示出来
            offset: -2, //
            gridLineWidth: 0.5,
            gridLineColor: '#C5D2E2', //
            lineWidth: 1.5,
            lineColor: '#3780B5',
            //min: 0,//不限制最低值，当温度为零下时，能显示出来
            title: {
              text: '能见度值（Km）'
            },
            labels: {
              x: -10, //y轴刻度往左移动10px
              style: {

                fontSize: '12px' //字体
              }
            },
            //tickPositions: [0, 10, 15, 20, 25, 30, 35] // 指定竖轴坐标点的值
          },
          tooltip: {
            shared: true,
            //useHTML: true,
            headerFormat: '<small></small><table>',
            pointFormatter: function () {
              //console.log(this.index);
              return '<p>' + timeMMDDArray[this.index] + '</p> <p>' +
                this.series.name + ': <b>' + ((this.y) * 1000).toFixed(2) + '</b><br/></p>'
            },
            //dateTimeLabelFormats: {
            //    day: '%Y-%m-%d'
            //},
            valueDecimals: 2
          },


          series: [{
              name: '高能见度预测模型',
              data: chartData1,
              //data:[2,-10,5,10,20,-20],//测试数据
              color: '#F7C12D'
            },

            {
              name: '低能见度预测模型',
              data: chartData3,
              //data:[2,-10,5,10,20,-20],//测试数据
              color: 'red'
            },
            {
              name: '融合模型',
              data: chartData4,
              //data:[2,-10,5,10,20,-20],//测试数据
              color: 'green'
            },
            {
              name: 'lightgbm',
              data: chartData5,
              //data:[2,-10,5,10,20,-20],//测试数据
              color: 'pink'
            },
            {
              name: '能见度实况值',
              data: chartData2,
              //data:[2,-10,5,10,20,-20],//测试数据
              color: 'blue'
            }
          ]
        });
      });
    } else {

    }




  } else {
    alert("请先点击地图上的站点！");
  }
}
//修改radio时，动态修改表格中的数据

//获取能见度等级最新的起报时间
function getLastForecastTime() {
  $.ajax({
    url: "visibilityForecast.aspx/GetLastForecastTime", //利用服务器数据
    type: "POST",
    contentType: "application/json",
    dataType: 'json',
    async: false,
    success: function (responseData) {
      var data = responseData.d.rows;
      startTime = jsonDateFormatWithMMDDHH(data[0].forecastdate);
      //startTime = jsonDateFormatWithMMDDHHMMSS(data[0].forecastdate);
      $("#startDate").val(startTime);
    },
    error: function (e) {
      console.log(e);
    }
  });
}
//获取能见度等级最新lst时间(距离起报时间72小时后的日期)
function getLastLstTime() {
  $.ajax({
    url: "visibilityForecast.aspx/GetLastLstTime", //利用服务器数据
    type: "POST",
    contentType: "application/json",
    dataType: 'json',
    async: false,
    success: function (responseData) {
      var data = responseData.d.rows;
      endTime = jsonDateFormatWithMMDDHH(data[0].lst);
      //endTime = jsonDateFormatWithMMDDHHMMSS(data[0].lst);
      $("#endDate").val(endTime);
    },
    error: function (e) {
      console.log(e);
    }
  });
}

//获取能见度值表中的最新起报时间
function getLastForecastTimeVisValue() {
  $.ajax({
    url: "visibilityForecast.aspx/GetLastForecastTimeVisValue", //利用服务器数据
    type: "POST",
    contentType: "application/json",
    dataType: 'json',
    async: false,
    success: function (responseData) {
      var data = responseData.d.rows;
      startTime = jsonDateFormatWithMMDDHH(data[0].forecastdate);
      //startTime = jsonDateFormatWithMMDDHHMMSS(data[0].forecastdate);
      $("#startDate").val(startTime);
    },
    error: function (e) {
      console.log(e);
    }
  });
}
//获取能见度值表中的最新的lst
function getLastLstTimeVisValue() {
  $.ajax({
    url: "visibilityForecast.aspx/GetLastLstTimeVisValue", //利用服务器数据
    type: "POST",
    contentType: "application/json",
    dataType: 'json',
    async: false,
    success: function (responseData) {
      var data = responseData.d.rows;
      endTime = jsonDateFormatWithMMDDHH(data[0].lst);
      $("#endDate").val(endTime);
    },
    error: function (e) {
      console.log(e);
    }
  });
}

//更新生成由能见度等级算出来的表
function createTable() {
  startTime = $("#startDate").val() + ":00:00";
  endTime = $("#endDate").val() + ":00:00";
  $.ajax({
    url: "visibilityForecast.aspx/CreateTable", //采用数据库中的数据
    type: "POST",
    contentType: "application/json",
    dataType: 'json',
    data: "{ startTime:'" + startTime + "',endTime:'" + endTime + "',period:'" + hours + "' }",
    success: function (response) {
      $("table tbody tr").remove();
      //动态填充表格面板的站点
      var siteFeatures = response.d.rows;
      var html = "";
      for (var i = 0; i < siteFeatures.length; i++) {
        html = html + "<tr><td class='td-left'> (" + siteFeatures[i].siteid + ") " + siteFeatures[i].sitename + "</td><td>" + siteFeatures[i].avgPiancha + "</td><td>" + siteFeatures[i].jfcha + "</td><td>" + siteFeatures[i].relXS + "</td><td>" + siteFeatures[i].ts + "</td></tr>";
      }
      $("table tbody").append(html);
      $("table tbody tr.no-records-found").remove();


    },
    error: function (e) {
      //alert("所选时间没有数据，请修改起报时间。温馨提示：所选时间间隔小于4天为宜");
    }

  });
}


//选择日期点击“确定” 和点击radio    生成折线图和表格中的内容
function getChartDataAndTableData() {
  //$("#Radio1").prop("checked") = true;
  //$("#Radio2").prop("checked") = false;
  //getChartData(siteCode);

  //更新生成由能见度等级算出来的表
  createTable();
}
//根据后台的数据，计算表格中的平均偏差、均方根偏差、相关系数、TS评分  并append标签中


//选时间，点击“查询” 请求站点数据，进行地图撒点 
//不选时间，直接查询站点表，加载站点数据，进行撒点
function querySiteData() {

  $.ajax({
    url: "visibilityForecast.aspx/QuerySiteData", //采用数据库中的数据
    type: "POST",
    contentType: "application/json",
    dataType: 'json',

    ////使用本地数据的配置方式
    //url: "http://localhost:32778/WebUI/XGB/data/sitesData.txt",//采用本地数据
    //type: "get",
    //dataType: "text",

    //data: "{ dTime:'" + dateTime + "' }",
    success: function (response) {
      ////console.log(response.d.rows);

      //利用服务器数据
      var data = response.d.rows;
      var incidents = data;

      ////使用本地数据
      //var data = JSON.parse(response);//将本地的JSON字符串数据转化为JSON对象
      //var incidents = data;

      //重新格式化数据
      function reformat(array) {
        var data = [];
        array.map(function (d, i) {
          data.push({
            id: i,
            type: "Feature",
            geometry: {
              coordinates: [+d.lon, +d.lat],
              type: "Point"
            },
            "properties": {
              "Code": d.siteid,
              "name": d.sitename,
              "Lon": d.lon,
              "Lat": d.lat,
              //"Vis":d.vis_scale_predicted

            }
          });
        });

        return data;
      }

      var geoData = {
        type: "FeatureCollection",
        features: reformat(incidents)
      };

      siteData = geoData;

      weatherTypes = "Vis";

      drawSurface(weatherTypes);


      ////动态填充表格面板的站点
      //var siteFeatures=siteData.features;
      //var html="";
      //for (var i = 0; i < siteFeatures.length; i++) {
      //    html = html + "<tr><td class='td-left'> (" + siteFeatures[i].properties.Code + ") " + siteFeatures[i].properties.name + "</td><td>---</td><td>---</td><td>---</td><td>---</td></tr>";
      //}
      //$("table tbody").append(html);
      //$("table tbody tr.no-records-found").remove();

    },
    error: function (e) {
      console.log(e);
    }

  });

}

/**
 *功能：绘制各种天气的点图层                      
 *@param {String} WeatherType Rain 天气类型
 */
function drawSurface(WeatherType) {
  var itemValue = []; //获取每个点儿的id和对应的值（温度、相对湿度、降雨量），目的是将这些值用作canvas画的文字标注信息
  switch (WeatherType) {
    case "Vis":
      clearCanvas(siteLayer);
      var startPointLatLng = map.containerPointToLatLng([-1000, -1000]);
      siteLayer = L.marker(L.latLng([startPointLatLng.lat, startPointLatLng.lng]), {
        icon: L.canvasIcon2({
          iconSize: [3500, 3500],
          iconAnchor: [8, 8], //采用之前设置的锚点值
          drawIcon: function (icon, type) {
            if (type == 'icon') {
              var ctx = icon.getContext('2d');
              var offsetX = 8;
              var offsetY = 8;
              for (var i = 0; i < siteData.features.length; i++) {

                var pid = parseInt(siteData.features[i].properties.Code);
                //itemValue[pid] = siteData.features[i].properties.Rain;

                //利用canvas在此画布上绘制小icon
                insertImgToCanvas(ctx, siteData.features[i].properties.Lat, siteData.features[i].properties.Lon, itemValue[pid], offsetX, offsetY, WeatherType); //HighLayerType去掉了(主要目的：对于不同的页面，子体和圆点设置不同的颜色)

              }
            }
          }
        })
      }).addTo(map);
      break;

      //继续case其它的导航栏
  }
}


/**
 *功能：利用canvas绘制文字和圆点                      
 *@param {Object} ctx var ctx = icon.getContext('2d');  canvas对象
 *@param {Num} lat siteData.features[i].properties.Lat   维度
 *@param {Num} lon siteData.features[i].properties.Lon  经度
 *@param {String} itemValue siteData.features[i].properties.Rain  降雨量值（各天气现象的值）
 *@param {String} oX offsetX  绘制的图形相对于画布左上角的偏移量
 *@param {String} oY offsetY  绘制的图形相对于画布左上角的偏移量
 *@param {String} type  WeatherType 天气类型
 */
function getFontAndCircleCanvas(ctx, lat, lon, itemValue, oX, oY, type) {
  ctx.beginPath();

  var point = map.latLngToContainerPoint([lat, lon]); //将各地理坐标转化为容器的屏幕坐标，进而进行canvas的绘制
  point.x = point.x + 1000;
  point.y = point.y + 1000;


  //根据传进来的不同类型设置字体和小圆点为不同的颜色（将类型和对应的值都传进来，进行判断）
  switch (type) {
    case "Vis":
      //字体样式
      ctx.font = "12px Arial";
      //绘制文本时，当前的文本基线，中间
      ctx.textAlign = "middle";
      //文本水平对齐
      ctx.textAlign = "center";
      //ctx.fillStyle = getRainColor(itemValue);//字体填充的颜色
      //ctx.fillText(text(itemValue), point.x, point.y);//绘制文字字体

      //绘制站点的小圆点儿
      ctx.moveTo(point.x + oX, point.y + oY);
      ctx.arc(point.x + oX, point.y + oY, 10, 0, Math.PI * 2, true);
      //ctx.fillStyle = getRainColor(itemValue);//小圆点儿填充的颜色
      ctx.fillStyle = "blue";
      ctx.fill();
      break;
  }

  ctx.closePath();
}

/**
 *功能：向canvas画布上插入不同类型的图片                      
 *@param {Object} ctx var ctx = icon.getContext('2d');  canvas对象
 *@param {Num} lat siteData.features[i].properties.Lat   维度
 *@param {Num} lon siteData.features[i].properties.Lon  经度
 *@param {String} itemValue siteData.features[i].properties.Rain  降雨量值（各天气现象的值）
 *@param {String} oX offsetX  绘制的图形相对于画布左上角的偏移量
 *@param {String} oY offsetY  绘制的图形相对于画布左上角的偏移量
 *@param {String} type  WeatherType 天气类型
 *@param {String} SOType  ["PRE","Win","Hail","Thund"] 四种强对流天气["强降水","大风","冰雹","雷暴"]  作用：判断引用哪种图片
 */
function insertImgToCanvas(ctx, lat, lon, itemValue, oX, oY, type, SOType) {

  var point = map.latLngToContainerPoint([lat, lon]); //将各地理坐标转化为容器的屏幕坐标，进而进行canvas的绘制
  point.x = point.x + 1000;
  point.y = point.y + 1000;

  //根据传进来的不同类型设置字体和小圆点为不同的颜色（将类型和对应的值都传进来，进行判断）
  switch (type) {

    case "Vis":
      var imgObj3 = new Image();
      imgObj3.src = "css/images/marker-icon.png";
      imgObj3.onload = function () {
        //ctx.drawImage(this, 0, 0);//this即是imgObj,保持图片的原始大小：470*480
        //ctx.drawImage(this, 0, 0,1024,768);//改变图片的大小到1024*768
        ctx.drawImage(this, point.x - 5, point.y - 15, 25, 41); //this即是imgObj,设置图片偏移左上角的尺度（oX，oY），设置图片的大小：20*15
      }
      break;

    case "ICERain": //冻雨
      var imgObj3 = new Image();
      imgObj3.src = "css/images/H_icerain.png";
      imgObj3.onload = function () {
        //ctx.drawImage(this, 0, 0);//this即是imgObj,保持图片的原始大小：470*480
        //ctx.drawImage(this, 0, 0,1024,768);//改变图片的大小到1024*768
        ctx.drawImage(this, point.x, point.y, 15, 15); //this即是imgObj,设置图片偏移左上角的尺度（oX，oY），设置图片的大小：20*15
      }
      break;
  }


}

function clearCanvas(type) {

  try {

    map.removeLayer(type);

  } catch (ex) {};
}
//只返回小时和分  即，06:00
function jsonDateFormat(jsonDate) {
  try {
    var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hours = "0" + date.getHours();
    hours = hours.slice(-2);
    var minutes = "0" + date.getMinutes();
    minutes = minutes.slice(-2);
    return hours + ":" + minutes; //返回时间格式为，时：分
  } catch (ex) {
    return "";
  }
}
//返回时间格式为，年月日时
function jsonDateFormatWithMMDDHH(jsonDate) { //json日期格式转换为正常格式
  try {
    var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hours = "0" + date.getHours();
    hours = hours.slice(-2);
    var minutes = "0" + date.getMinutes();
    minutes = minutes.slice(-2);
    return date.getFullYear() + "-" + month + "-" + day + " " + hours; //返回时间格式为，年月日时
  } catch (ex) {
    return "";
  }
}
//返回时间格式为，年月日时分
function jsonDateFormatWithMMDDHHMM(jsonDate) { //json日期格式转换为正常格式
  try {
    var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hours = "0" + date.getHours();
    hours = hours.slice(-2);
    var minutes = "0" + date.getMinutes();
    minutes = minutes.slice(-2);
    return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes; //返回时间格式为，年月日时分
  } catch (ex) {
    return "";
  }
}
//返回时间格式为，年月日时分秒
function jsonDateFormatWithMMDDHHMMSS(jsonDate) { //json日期格式转换为正常格式
  try {
    var date = new Date(parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hours = "0" + date.getHours();
    hours = hours.slice(-2);
    var minutes = "0" + date.getMinutes();
    minutes = minutes.slice(-2);
    var second = "0" + date.getSeconds();
    return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + second; //返回时间格式为，年月日时分秒
  } catch (ex) {
    return "";
  }
}

function getDateFromMapHour() {
  var newDate = $("#startDate").val();
  newDate = newDate + " " + $("#hourButton").val() + ":00:00.000";
  return newDate;
}

//点击日历表上的确定时，执行相应的函数
function toDOFun() {
  //根据能见度值，生成table表
  getChartDataAndTableData();
  //如果点击了地图上的站点，就执行生成列表的方法
  if (siteCode != "") {
    var chartType = $("input[name='optionsRadiosinline']:checked").val();
    switch (chartType) {
      case "visClass":

        getChartData(siteCode);
        break;
      case "visValue":

        getVisValueData(siteCode);
        break;

    }
  } else {}
}