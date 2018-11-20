// JScript 文件

Ext.onReady(function(){
        initInputHighlightScript();
        //默认进入的时候点击查询按钮所做的查询
        doQueryChart();
    }
)

function doQueryChart()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ComForecast','GetOzoneChart'),
        params: { fromDate: fromDate, toDate: toDate, siteID: siteID }, 
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                RenderChart(result);
            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。"); 
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });     
}

function RenderChart(result)
{
    var lineTilte="臭氧";
    if (siteID=="228")
        lineTilte="浦东超级站" + lineTilte;
    else if (siteID=="250")
        lineTilte="嘉定超级站" + lineTilte;
    var dblMax = 0;
//    var dblMin = 10000;
    
    var ArrayData=new Array(); 
    //增加曲线
    for (var j = 0; j < 5; j++) {
        var arrayTmp = new Array();

        if (result[j] != null) {
            var cl = result[j].split('*');
            if (result[j] == "*") {
//                arrayTmp[0] = { x: result["minX"] * 1000, y:0 };
                ArrayData[j] = null ;
            }
            else {
                if (cl.length == 2) {
                    var clx = cl[0].split('|');
                    var cly = cl[1].split('|');

                    for (var k = 0; k < clx.length; k++) {
                        var yValue = null;
                        if (cly[k] != null && cly[k] != " ") {
                            yValue = fomatFloat(parseFloat(cly[k]), 1);
                            //只要大于0的值
                            if (yValue > 0) {
                                //获取Y轴的最大和最小值
                                if (dblMax < yValue)
                                    dblMax = yValue;
                            }
                            else
                                yValue = null;
                        }
                        var tmp = { x: clx[k] * 1000, y: yValue };
                        arrayTmp[k] = tmp;
                    }
                    ArrayData[j] = arrayTmp;
                }
                else
                    ArrayData[j] = arrayTmp;
            }
        }
        else 
        {
            arrayTmp[0] = { x: result[minX] * 1000, y: yValue };
            ArrayData[j] = arrayTmp;
        }
    }
    
    dblMax = dblMax + 1;
//    dblMin = dblMin - 1;
//    if (dblMin < 0)
//        dblMin = 0;
    var groupingUnits = [[
		'week',                         // unit name
		[1]                             // allowed multiples
	], [
		'month',
		[1, 2, 3, 4, 6]
	]];
	
	Highcharts.setOptions({
        lang:{
            rangeSelectorFrom:'从',
            rangeSelectorTo:'到',
            rangeSelectorZoom:''
        }
    });

    $('#container').highcharts('StockChart', {
        rangeSelector:
        {
            buttons: [{
                type: 'day',
                count: 2,
                text: '天'
            }, {
                type: 'week',
                count: 1,
                text: '星期'
            }, {
                type: 'month',
                count: 1,
                text: '月'
            }, {
                type: 'all',
                text: '全部'
            }],
            selected: 0,
            inputDateFormat:'%Y-%m-%d'
        },
        colors: ['aqua', 'blue', 'green', 'red', 'gray', '#997300', '#70ad47', '#ed7d31', 'gray'],
        credits:{enabled : false},
        title: {
            text: lineTilte
        },
        global:{useUTC : false},
        exporting: {
            enabled: false
        },
        tooltip: {  
            xDateFormat: '%Y-%m-%d, %H'//鼠标移动到趋势线上时显示的日期格式  
        },
        
        xAxis: {
            labels: {
                formatter: function() {
                    return  Highcharts.dateFormat('%d-%H', this.value);
                }
            },
            dateTimeLabelFormats: {
                second: '%H',
                minute: '%H',
                hour: '%H',
                day: '%d日<br/>%H时',
                week: '%d',
                month: '%m',
                year: '%Y'
            }
        },
        yAxis: 
        [{
            title: {text: 'ppb'},
            height: 270,
            offset: 0,
            lineWidth: 2
        }],
        plotOptions: {
            line: {
                lineWidth: 2,
                states: {
                    hover: {
                        lineWidth: 1
                    }
                },
                marker: {
                    enabled: true,
                    radius: 2,
                    lineWidth: 1
                }
            },
            area: {
                stacking: 'normal',
                pointWidth: 10,
                borderWidth: 0
            },
            series:{
                dataGrouping:{
                    enabled:false
                }
            }
        },
        navigator: {
            baseSeries: 4,
            margin: 0,
            series: {
                type: 'line',
                color: '#4572A7',
                fillOpacity: 0.4,
                dataGrouping: {
                    smoothed: true,
                    enabled: false
                },
                lineWidth: 1,
                marker: {
                    enabled: false
                },
                shadow: false
            },
            xAxis: {
                tickWidth: 0,
                lineWidth: 0,
                gridLineWidth: 1,
                tickPixelInterval: 100,
                labels: {
                    align: 'center',
                    x: 0,
                    y: -4,
                    formatter: function() {
                        return Highcharts.dateFormat('%m月%d', this.value);
                    }
                }
            }
        },
        legend: {
            style: {
                left: 'auto',
                bottom: 'auto',
                right: 'auto',
                top: 'auto'
            },
            backgroundColor: '#FFFFFF',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false,
            enabled: true 
        },
        series:
        [{
            type: 'area',
            name: '烷烃',
            data: ArrayData[0]
        },{
            type: 'area',
            name: '烯烃',
            data: ArrayData[1]
        },{
            type: 'area',
            name: '芳香烃',
            data: ArrayData[2]
        },{
            type: 'line',
            name: '总VOC',
            data: ArrayData[3]
        },{
            type: 'line',
            name: '臭氧',
            data: ArrayData[4]
//            visible: false
        }]
    });
}