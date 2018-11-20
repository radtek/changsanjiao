

function doQueryChart() {

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


