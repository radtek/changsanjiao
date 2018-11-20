
Ext.onReady(function(){
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doCompareChart();
})
function radioClick(id){
    var el = Ext.getDom(id); 
    if(el.className == "radioUnChecked"){
        el.className = "radioChecked";
        el = getGroupEl(id);
        el.className = "radioUnChecked";       
    }  
}
function getGroupEl(id){
    var groupEl = "";
    switch(id){
    case "rd1":
        groupEl = "rd2";
        break;
    case "rd2":
        groupEl = "rd1";
        break;
    }   
    return Ext.getDom(groupEl);
}
var itemID;
//返回比较的结果
function doCompareChart()
{
    var forecastDate = Ext.getDom("H00").value;
    var forecastToDate = Ext.getDom("H01").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ComForecast','GetForecastCompare'),
        params: { forecastDate: forecastDate,forecastToDate:forecastToDate}, 
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
    var yTitle="";
        yTitle='ug/m3';
    var ArrayData=new Array(); 
        //增加曲线
        for (var j = 0; j <2; j++) {
            var arrayTmp=new Array();
            if (result[j] != null) {
                var cl = result[j].split('*');
                if (cl.length == 2 || cl.length == 3) {
                    var clx = cl[0].split('|');
                    var cly = cl[1].split('|');
                    var para; 
                    for (var k = 0; k < clx.length; k++) {
                        var yValue=null;
                        if (cly[k]!=null )
                        {
                            if (cly[k]!=" ")
                                yValue= parseFloat(cly[k]);
                        }
                        var tmp;
                            tmp = { x: clx[k] * 1000, y: yValue };
                        arrayTmp[k] = tmp;
                    }
                    ArrayData[j] = arrayTmp;
                }
                else
                    ArrayData[j] = arrayTmp;
            }
            else
                ArrayData[j] = arrayTmp;
        }

        
        Highcharts.setOptions({
            lang: {
                rangeSelectorFrom: '从',
                rangeSelectorTo: '到',
                rangeSelectorZoom: ''
            }
        });
        // create the chart
        $('#containerForecast').highcharts('StockChart', {
            rangeSelector:
            {
                buttons: [{
                        type: 'day',
                        count: 1,
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
                selected: 1,
                inputDateFormat: '%Y-%m-%d'
            },
            colors: ['#9e480e', 'gray', '#997300', '#70ad47', '#00b0f0', 'gray'],
            credits: { enabled: false },
            title: {
                text: "48小时预报小时变化图"
            },
            global: { useUTC: false },
            exporting: {
                enabled: false
            },
            tooltip: {
                xDateFormat: '%Y-%m-%d, %H' //鼠标移动到趋势线上时显示的日期格式
            },
            xAxis: {
                labels: {
                    formatter: function() {
                        return Highcharts.dateFormat('%d-%H', this.value);
                    }
                },
                offset: 0
            },
            yAxis:
            [{
                title: { text:yTitle },
                height: 250,
                offset: 0,
                lineWidth: 2,
                min :0
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
                        radius: 4,
                        lineWidth: 1
                    }
                },
                column: {
                    stacking: 'normal',
                    pointWidth: 10,
                    borderWidth: 0
                },
                series: {
                    dataGrouping: {
                        enabled: false
                    }
                }

            },
            legend: {
                y:0,
                backgroundColor: '#FFFFFF',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false,
                enabled: true
            },
            navigator: {
                baseSeries: 1,
                margin :0,
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
            series:
            [{
                type: 'line',
                name: 'PM2.5',
                data: ArrayData[0]
            }, {
                type: 'line',
                name: '臭氧',
                data: ArrayData[1]
            }]
        });
        var chart = $('#containerForecast').highcharts();
}
function exportSiteData()
{
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var content=fromDate+"|"+toDate;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("btnExport").click();
}
