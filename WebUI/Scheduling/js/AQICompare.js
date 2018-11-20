var tempArray=new Array();
tempArray[0]=true;tempArray[1]=true;tempArray[2]=true;tempArray[3]=true;
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
function getCheckBoxValue(objName)
{
    var postJson = "";
    var forecasArray=new Array();
    var obj=document.getElementsByName(objName);
    if(obj!=null)
    {
        for(var i=0;i<obj.length;i++)
        {
            if(obj[i].checked)
            {
                postJson=postJson+obj[i].value+",";
            }
        }
    }
    if(postJson.length>0)
        postJson=postJson.substring(0,postJson.length-1);
    return postJson;
}
//预报污染物tab切换
function tabClick(id){
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTab);
    lastEl.className = "shortdan";
    curEl.className = "shortdan_highlight";
    lastTab = id;
    
    doCompareChart();
}
var itemID;
//返回比较的结果
function doCompareChart()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var period;
    if (rd1.className == "radioUnChecked")
        period = "48";
    else 
        period = "24";
    
    if(AQI.className== "shortdan_highlight") 
        itemID=0;
    else if(item1.className== "shortdan_highlight") 
        itemID=1;
    else if(item2.className== "shortdan_highlight") 
        itemID=2;
    else if(item3.className== "shortdan_highlight") 
        itemID=3;
    else if(item4.className== "shortdan_highlight") 
        itemID=4;
    else if(item5.className== "shortdan_highlight") 
        itemID=5;
        
    var typeID="0,1,2,3";//getCheckBoxValue("dataType");
    if (partID==2)
        typeID="0,1"
    if (typeID == "") {
        Ext.Msg.alert("提示","请至少选择一种类型");
        return;
    }
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ComForecast','GetAQICompare'),
        params: { fromDate: fromDate,toDate:toDate,typeID:typeID,period:period,itemID:itemID}, 
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
    if (itemID=='0')
        yTitle='AQI';
    else
        yTitle='ug/m3';
    var ArrayData=new Array(); 
        //增加曲线
        for (var j = 0; j < 4; j++) {
            var arrayTmp=new Array();
            if (result[j] != null&&result[j] !="**") {
                var cl = result[j].split('*');
                if (cl.length == 2 || cl.length == 3) {
                    var clx = cl[0].split('|');
                    var cly = cl[1].split('|');
                    var para; 
                    if (itemID=='0')
                    {
                        para= cl[2].split('|');
                    }
                    for (var k = 0; k < clx.length; k++) {
                        var yValue=null;
                        if (cly[k]!=null )
                        {
                            if (cly[k]!=" ")
                                yValue= parseFloat(cly[k]);
                        }
                        var tmp;
                        if (itemID=='0')
                            tmp = { x: clx[k] * 1000, y: yValue ,name:para[k]};
                        else
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
                selected: 1,
                inputDateFormat: '%Y-%m-%d'
            },
            colors: ['#0000ff', '#009900', '#f814e2', '#9e480e', '#ed7d31', '#997300', '#70ad47', '#00b0f0', 'gray'],
            credits: { enabled: false },
            title: {
                text: "污染物曲线对比"
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
                        if (itemID=='0')
                        {
                            var parameter=this.points[i].key;
                            tipMessage=tipMessage + "\\" + parameter;
                        }
                        tipMessage=tipMessage + "<br/>";
                    }
                    return tipMessage;
                }
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
                        radius: 3,
                        lineWidth: 1,
                        symbol:'circle'
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
                    },
                    events: {
                        legendItemClick: function (event) {
                          legenClick(event); 
                        }
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
                name: '实测数据',
                data: ArrayData[0],
                visible:tempArray[0]
            }, {
                type: 'line',
                name: '综合预报',
                data: ArrayData[1],
                visible:tempArray[1]
            }, {
                type: 'line',
                name: 'WRF-Chem',
                data: ArrayData[2],
                visible:tempArray[2]
            }, {
                type: 'line',
                name: 'CMAQ',
                data: ArrayData[3],
                visible:tempArray[3] 

            }]
        });
        var chart = $('#container').highcharts();
        if (partID==2)
        {
            chart.series[3].remove(true);
            chart.series[2].remove(true);
        }
}
function legenClick(e)
{
   var legenClick=e.target.name;
   if(e.target.visible==true)
      changeVisible(legenClick,false);	
   else 
   {
      changeVisible(legenClick,true);
   }	

}
function changeVisible(el,type)
{
    switch(el){
    case "实测数据":
        tempArray[0]=type;
        break;
    case "综合预报":
        tempArray[1]=type;
        break;
    case "WRF-Chem":
        tempArray[2]=type;
        break;
    case "CMAQ":
        tempArray[3]=type;
        break;
    }  
}
function exportSiteData()
{
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var period;
    var itemID;
    if (rd1.className == "radioUnChecked")
       period = "48";
    else period="24";
    if(AQI.className== "shortdan_highlight") 
        itemID=0;
    else if(item1.className== "shortdan_highlight") 
        itemID=1;
    else if(item2.className== "shortdan_highlight") 
        itemID=2;
    else if(item3.className== "shortdan_highlight") 
        itemID=3;
    else if(item4.className== "shortdan_highlight") 
        itemID=4;
    else if(item5.className== "shortdan_highlight") 
        itemID=5;
        
    var typeID="0,1,2,3";
    if (partID==2)
        typeID="0,1"
    var content=fromDate+"|"+toDate+"|"+period+"|"+itemID+"|"+typeID;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("btnExport").click();
}