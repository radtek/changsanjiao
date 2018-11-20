

function doQueryChart() {


//var v="{'0':'1483866000|1483869600|1483873200|1483876800|1483880400|1483884000|1483887600|1483891200|1483894800|1483898400|1483902000|1483905600|1483909200|1483912800|1483916400|1483920000|1483923600|1483927200|1483930800|1483934400|1483938000|1483941600|1483945200|1483948800|1483952400|1483956000|1483959600|1483963200|1483966800|1483970400|1483974000|1483977600|1483981200|1483984800|1483988400|1483992000|1483995600|1483999200|1484002800|1484006400|1484010000|1484013600|1484017200|1484020800|1484024400|1484028000|1484031600|1484035200|1484038800|1484042400|1484046000|1484049600|1484053200|1484056800|1484060400|1484064000|1484067600|1484071200|1484074800|1484078400|1484082000|1484085600|1484089200|1484092800|1484096400|1484100000|1484103600|1484107200|1484110800|1484114400|1484118000|1484121600|1484125200*101.15|107.63|111.68|108.62|114.48|120.31|122.18|119.25|106.76|96.51|65.91|52.82|41.46|40.31|35.71|36.20|54.27|74.82|61.15|55.34|65.59|68.23|68.19|64.14|71.59|69.77|65.72|58.50|59.16|67.28|75.35|84.43|90.81|90.36|90.33|98.83|85.96|68.82|81.48|102.83|112.63|108.32|97.21|92.84|93.33|87.48|78.05|81.15|95.62|104.41|111.10|99.70|105.05|103.93|100.77|92.86|88.93|72.14|76.13|88.96|81.21|73.76|62.75|50.12|35.62|33.58|33.74|23.14|20.71|20.88|20.38|25.06|30.77','1':'1484013600|1484017200|1484020800|1484024400|1484028000|1484031600|1484035200|1484038800|1484042400|1484046000|1484049600|1484053200|1484056800|1484060400|1484064000|1484067600|1484071200|1484074800|1484078400|1484082000|1484085600|1484089200|1484092800|1484096400|1484100000|1484103600|1484107200|1484110800|1484114400|1484118000|1484121600|1484125200*56.57|48.84|72.76|55.01|37.67|34.74|32.18|38.76|47.54|71.38|70.47|79.16|92.75|69.07|48.71|53.44|45.14|50.29|61.96|60.06|50.47|45.18|36.59|29.77|32.48|25.49|18.44|19.76|16.87|16.51|21.50|22.10','2':'1483866000|1483869600|1483873200|1483876800|1483880400|1483884000|1483887600|1483891200|1483894800|1483898400|1483902000|1483905600|1483909200|1483912800|1483916400|1483920000|1483923600|1483927200|1483930800|1483934400|1483938000|1483941600|1483945200|1483948800|1483952400|1483956000|1483959600|1483963200|1483966800|1483970400|1483974000|1483977600|1483981200|1483984800|1483988400|1483992000|1483995600|1483999200|1484002800|1484006400|1484010000|1484013600|1484017200|1484020800|1484024400|1484028000|1484031600|1484035200|1484038800|1484042400|1484046000|1484049600|1484053200|1484056800|1484060400|1484064000|1484067600|1484071200|1484074800|1484078400|1484082000|1484085600|1484089200|1484092800|1484096400|1484100000|1484103600|1484107200|1484110800|1484114400|1484118000|1484121600|1484125200*41.56|56.38|67.72|74.28|64.17|50.94|38.31|35.34|23.04|16.92|14.91|12.01|8.21|8.00|11.08|13.02|19.88|19.43|23.25|27.21|33.70|35.35|31.76|33.40|36.05|37.44|39.45|41.85|42.34|43.79|46.82|50.62|53.11|52.41|54.97|55.85|56.97|61.87|61.90|64.36|66.10|68.68|NULL|46.92|38.06|49.56|56.73|61.56|61.96|57.42|46.91|39.01|36.80|33.95|29.36|28.02|32.05|31.08|27.80|22.65|18.41|10.49|7.64|8.71|6.86|6.33|4.06|5.11|7.02|8.26|13.68|13.71|6.54','3':'1483866000|1483869600|1483873200|1483876800|1483880400|1483884000|1483887600|1483891200|1483894800|1483898400|1483902000|1483905600|1483909200|1483912800|1483916400|1483920000|1483923600|1483927200|1483930800|1483934400|1483938000|1483941600|1483945200|1483948800|1483952400|1483956000|1483959600|1483963200|1483966800|1483970400|1483974000|1483977600|1483981200|1483984800|1483988400|1483992000|1483995600|1483999200|1484002800|1484006400|1484010000|1484013600|1484017200|1484020800|1484024400|1484028000|1484031600|1484035200|1484038800|1484042400|1484046000|1484049600|1484053200|1484056800|1484060400|1484064000|1484067600|1484071200|1484074800|1484078400|1484082000|1484085600|1484089200|1484092800|1484096400|1484100000|1484103600|1484107200|1484110800|1484114400|1484118000|1484121600|1484125200*99.625|104.125|109.125|111.000|110.375|111.750|115.000|112.000|103.375|90.625|75.000|52.625|37.000|30.111|31.888|31.555|32.000|47.333|52.111|44.777|50.777|62.777|65.111|64.000|64.111|60.777|60.777|58.625|56.444|59.888|67.555|73.222|76.333|77.333|81.000|79.222|69.222|68.222|72.333|79.444|84.666|86.777|86.444|82.000|84.111|81.333|69.222|68.111|79.222|92.222|97.111|94.555|93.888|92.777|86.444|80.444|77.333|68.000|67.888|76.000|77.555|70.222|61.666|55.888|43.777|32.111|31.750|27.555|22.888|20.777|21.222|23.888|29.111'}";
// var data = eval("(" + v + ")");
//  RenderChart(data, "#container1");
 $.ajax({
        url: getUrl("MMShareBLL.DAL.CitysForecast", "GetAQIChartDBPM"),
        cache: false,
        type: 'post',
        data: {},
        success: function (msg) {
            var data = eval("(" + msg + ")");
            var result = data;
            RenderChart(result, "#container1");
        },
        error: function (msg) {
            //console.log("status:" + msg.status + ", statusText:" + msg.statusText);
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

function RenderChart(result,contonter) {
    var winHeight = $(window).height();
    $('#container1').css("height", winHeight - 15);
    var chartName = "";
    var LineName = "";
    var dblMax = 0;
    var dblMin = 10000;
    var CuaceArray = new Array(); var ChemArray = new Array();
    var CmaqArray = new Array(); var AqiArray = new Array(); 
    var CMAQ10Array = new Array(); var Cuace9Array = new Array(); 
    var DMSArray = new Array(); 

    var indexData = 4;
    if (result != null) {
        var arrayTmp = new Array();
        if (result['0'] != null && result['0'] != "*") {
            indexData = 0;
            var Pb = result['0'].split('*');
            if (Pb.length == 2) {
                var Pbx = Pb[0].split('|');
                var Pby = Pb[1].split('|');
                for (var i = 0; i < Pbx.length; i++) {
                    var tmp = { x: Pbx[i] * 1000, y: ConvertToFloat(Pby[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pby[i]))
                        dblMax = ConvertToFloat(Pby[i]);

                    if (dblMin > ConvertToFloat(Pby[i]))
                        dblMin = ConvertToFloat(Pby[i]);
                    CuaceArray[i] = tmp;
                }
            }
        }
        else
            CuaceArray = arrayTmp;

        if (result['1'] != null && result['1'] != "*") {
            indexData = 1;
            var Fe = result['1'].split('*');
            if (Fe.length == 2) {
                var Fex = Fe[0].split('|');
                var Fey = Fe[1].split('|');
                for (var i = 0; i < Fex.length; i++) {
                    var tmp = { x: Fex[i] * 1000, y: ConvertToFloat(Fey[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Fey[i]))
                        dblMax = ConvertToFloat(Fey[i]);

                    if (dblMin > ConvertToFloat(Fey[i]))
                        dblMin = ConvertToFloat(Fey[i]);
                    ChemArray[i] = tmp;
                }
            }
        }
        else
            ChemArray = arrayTmp;

        if (result['2'] != null && result['2'] != "*") {
            indexData = 2;
            var Pd = result['2'].split('*');
            if (Pd.length == 2) {
                var Pdx = Pd[0].split('|');
                var Pdy = Pd[1].split('|');
                for (var i = 0; i < Pdx.length; i++) {
                    var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pdy[i]))
                        dblMax = ConvertToFloat(Pdy[i]);

                    if (dblMin > ConvertToFloat(Pdy[i]))
                        dblMin = ConvertToFloat(Pdy[i]);
                    CmaqArray[i] = tmp;
                }
            }
        }
        else
            CmaqArray = arrayTmp;

        if (result['3'] != null && result['3'] != "*") {
            indexData = 3;
            var Pd = result['3'].split('*');
            if (Pd.length == 2) {
                var Pdx = Pd[0].split('|');
                var Pdy = Pd[1].split('|');
                for (var i = 0; i < Pdx.length; i++) {
                    var tmp = { x: Pdx[i] * 1000, y: ConvertToFloat(Pdy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(Pdy[i]))
                        dblMax = ConvertToFloat(Pdy[i]);

                    if (dblMin > ConvertToFloat(Pdy[i]))
                        dblMin = ConvertToFloat(Pdy[i]);
                    CMAQ10Array[i] = tmp;
                }
            }
        }
        else
            CMAQ10Array = arrayTmp;


        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
        // create the chart
  
        $(contonter).highcharts({
            chart: {
               type: 'spline',
                   backgroundColor: '#013253',
                     spacingTop: 0
            },

            colors: ['rgb(124,181,236)','#D2B48C', 'rgb(144,237,145)', 'rgb(255,188,117)' ],
            credits: { enabled: false },
            title: {
                text: LineName,
                style: {
                    fontSize: 22,
                    fontName: '宋体',
                    fontWeight: 'bold'
                }
            },
            global: { useUTC: false },
            tooltip: {
                shared: true,
                crosshairs: true,
                xDateFormat: '%Y-%m-%d, %H'//鼠标移动到趋势线上时显示的日期格式 
            },
       exporting: {
enabled:false
},
          xAxis: {
    
    
            type: 'datetime',
            tickInterval: 24 * 3600 * 1000,
            labels: {
                formatter: function () {
                    var tipMessage = "";
                    tipMessage = Highcharts.dateFormat('%m月', this.value) + "" + Highcharts.dateFormat('%d日', this.value);
                    return tipMessage;
                },
                 style: {
                    color: '#e8f3ff',
                    fontSize: '20px',
                    paddingTop:'20px'
                },
                                y: 20
            },
            offset: 0,
            lineColor: 'rgb(169,184,207)',
            lineWidth: 2,
            minorGridLineWidth: 0,
            minorTickInterval: 6 * 3600 * 1000,
            minorTickWidth: 1,
            minorTickLength: 5
        },
       legend: {
       itemStyle : {
        'fontSize' : '22px',
         'color':'white'
    },
          
            borderColor: '#CCC',
            borderWidth: 0,
        
            shadow: false,
            align: 'right',
            verticalAlign: 'top',
            x: -50,
            y: 10,
            floating: true,
            borderWidth: 0,
        },
         yAxis: { // Primary yAxis

              title: {
                text: ''
            },
            labels: {
                style: {
                    color: '#e8f3ff',
                    fontSize: '20px'
                }
            },
            min: 0,
            gridLineColor: '#4e7087',
                lineColor: 'rgb(169,184,207)',
                min:0, // 定义最小值  
                lineWidth: 2,
                style: {
                    color: '#473C8B'
                },
                title: {
                    text: '（ug/m3）',
                    style: {
                        color: 'white' ,
                      fontSize: 22
                    }
                },
                showEmpty: false
            },

            plotOptions: {
             series: {
                  connectNulls: false
                },
                line: {
                    lineWidth: 4,
                    states: {
                        hover: {
                            lineWidth: 1
                        }
                    },
                    marker: {
  
                      radius: 5,
                      symbol: 'circle',
                      lineWidth: 1
                  }
                },
                column: {
                    stacking: 'normal',
                    pointWidth: 8,
                    borderWidth: 0
                },
   
                series: {
                    dataGrouping: {
                        enabled: false
                     },
              
                }
            },
            series: [
             {
                 name: '徐家汇',
                 type: 'line',
                 data: CuaceArray
                
             },{
                 name: '上海中心大厦',
                 type: 'line',
                 data: ChemArray
                
            }, {
                 name: '东滩',
                 type: 'line',
                 data: CmaqArray
            }, {
                name: '上海市国控',
                type: 'line',
                data: CMAQ10Array
           } ]
        });
    }
    else{}
      
}


