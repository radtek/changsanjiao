
Ext.onReady(function(){
    initInputHighlightScript();
    //默认进入的时候点击查询按钮所做的查询
    doCompareChart();
})
//返回比较的结果
function doCompareChart()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"曲线正在生成中..."});
     myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.SiteData','GetAQIMethodCompare'),
        timeout:120000,
        params: { fromDate: fromDate,toDate:toDate}, 
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                 myMask.hide();
                RenderChart(result);
            }
            else
            {
               myMask.hide();
                Ext.Msg.alert("提示", "没有满足条件的信息。"); 
                }
        },
        failure: function(response) { 
         myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}

function RenderChart(result)
{
    var yTitle="";
        yTitle='AQI';
    var ArrayData=new Array(); 
        //增加曲线
        for (var j = 0; j < 5; j++) {
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
        $('#container').highcharts('StockChart', {
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
                selected: 0,
                inputDateFormat: '%Y-%m-%d'
            },
            colors: ['#0000FF', '#009900', '#990033', 'Black', '#FF0000'],
            credits: { enabled: false },
            title: {
                text: "AQI方法对比"
            },
            global: { useUTC: false },
            exporting: {
                enabled: false
            },
            tooltip: {
                formatter: function() {
                    var dt=new Date(parseInt(this.points[0].x)-8*3600*1000);
                    var hour=dt.getHours();
                    var tipHour="上午";
                    if (hour=="06")
                        tipHour="上午";
                    else if (hour=="12")
                        tipHour="下午";
                    else if (hour=="20")
                        tipHour="夜间";
                    var tipMessage= dt.getFullYear() + "-" + (dt.getMonth()+1) + "-" + dt.getDate() + " " + tipHour + "<br/>";
                    for (var i=0;i<this.points.length;i++)
                    {
                        tipMessage=tipMessage + this.points[i].series.name+ ":" + this.points[i].y ;
                        tipMessage=tipMessage + "<br/>";
                    }
                    return tipMessage;
                }
            },
            xAxis: {
                labels: {
                    formatter: function() {
                        return Highcharts.dateFormat('%H:00', this.value);
                    }
                },
                offset: 0
            },
            yAxis:
            [{
                title: { text:yTitle },
                height: 350,
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
                        radius: 0,
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
                name: '1小时',
                data: ArrayData[0]
            }, {
                type: 'line',
                name: '3小时',
                data: ArrayData[1]
            }, {
                type: 'line',
                name: '24小时',
                data: ArrayData[2]
            }, {
                type: 'line',
                name: 'Nowcast',
                data: ArrayData[3] 
            }, {
                type: 'line',
                name: 'SAQI',
                data: ArrayData[4] 

            }]
        });
        var chart = $('#container').highcharts();

}
function exportSiteData()
{
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var period;
    var content=fromDate+"|"+toDate;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("btnExport").click();
}