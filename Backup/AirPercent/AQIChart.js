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
    var toDate=Ext.getDom("H01").value;
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ComForecast','GetAQIChart'),
        params: { fromDate: fromDate,toDate:toDate,station:station}, 
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
function ConvertToFloat(x)
{
 var floatTemp=parseFloat(x).toFixed(1);
 var floatValue=parseFloat(floatTemp); 
 return floatValue;
 
}

function RenderChart(result)
{
    var lineTilte="气溶胶组分分析";
    if (station=="CM")
        lineTilte="崇明" + lineTilte;
    else if (station=="QP")
        lineTilte="青浦" + lineTilte;
    else if (station=="NJ")
        lineTilte="南京" + lineTilte;
    var dblMax = 0;
    var dblMin = 10000;
    var clArray=new Array();var naArray=new Array();var mgArray=new Array();
    var noArray=new Array();var nhArray=new Array();var caArray=new Array();
    var soArray=new Array();var kArray=new Array();var pmArray=new Array();
    var ocArray=new Array();var ecArray=new Array();var ocGArray=new Array();
    var ecGArray=new Array();
    var indexData=12;
    //Cl,NO3,SO4,Na,NH4,K,Mg,Ca, OC(热学),EC(热学),OC(光学),EC(光学),[PM2#5(ug/m3)]
    if (result!=null)
    {
      var arrayTmp=new Array();
      if(result['0']!=null&&result['0']!="*" && station!="NJ")
      {
            indexData=0;
            var cl=result['0'].split('*');
            if (cl.length==2){
                var clx=cl[0].split('|');
                var cly=cl[1].split('|');
                for (var i = 0; i < clx.length; i++) {
                    var tmp = { x: clx[i]*1000, y: ConvertToFloat(cly[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(cly[i]))
                        dblMax = ConvertToFloat(cly[i]);
                    
                    if (dblMin > ConvertToFloat(cly[i]))
                        dblMin = ConvertToFloat(cly[i]);
                    clArray[i] = tmp;
                }
            }
        }
        else 
            clArray=arrayTmp;
        if(result['1']!=null&&result['1']!="*" && station!="NJ")
          {
            indexData=1;
            var no=result['1'].split('*');
            if (no.length==2){
                var nox=no[0].split('|');
                var noy=no[1].split('|');
                for (var i = 0; i < nox.length; i++) {
                    var tmp = { x: nox[i]*1000, y: ConvertToFloat(noy[i])};
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(noy[i]))
                        dblMax = ConvertToFloat(noy[i]);
                    
                    if (dblMin > ConvertToFloat(noy[i]))
                        dblMin = ConvertToFloat(noy[i]);
                    noArray[i] = tmp;
                }
            }
        }
        else 
            noArray=arrayTmp;
        if(result['2']!=null&&result['2']!="*" && station!="NJ")
        {
            indexData=2;
            var so=result['2'].split('*');
            if (so.length==2){
                var sox=so[0].split('|');
                var soy=so[1].split('|');
                for (var i = 0; i < sox.length; i++) {
                    var tmp = { x: sox[i]*1000, y: ConvertToFloat(soy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(soy[i]))
                        dblMax = ConvertToFloat(soy[i]);
                    
                    if (dblMin > ConvertToFloat(soy[i]))
                        dblMin = ConvertToFloat(soy[i]);
                    soArray[i] = tmp;
                }
            }
        }
        else 
          soArray=arrayTmp;
        if(result['3']!=null&&result['3']!="*" && station!="NJ")
        {
            indexData=3;
            var na=result['3'].split('*');
            if (na.length==2){
                var nax=na[0].split('|');
                var nay=na[1].split('|');
                for (var i = 0; i < nax.length; i++) {
                    var tmp = { x: nax[i]*1000, y: ConvertToFloat(nay[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(nay[i]))
                        dblMax = ConvertToFloat(nay[i]);
                    
                    if (dblMin > ConvertToFloat(nay[i]))
                        dblMin = ConvertToFloat(nay[i]);
                    naArray[i] = tmp;
                }
            }
        }
        else 
            naArray=arrayTmp;
        if(result['4']!=null&&result['4']!="*" && station!="NJ")   
        {
            indexData=4;
            var nh=result['4'].split('*');
            if (nh.length==2){
                var nhx=nh[0].split('|');
                var nhy=nh[1].split('|');
                for (var i = 0; i < nhx.length; i++) {
                    var tmp = { x: nhx[i]*1000, y: ConvertToFloat(nhy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(nhy[i]))
                        dblMax = ConvertToFloat(nhy[i]);
                    
                    if (dblMin > ConvertToFloat(nhy[i]))
                        dblMin = ConvertToFloat(nhy[i]);
                    nhArray[i] = tmp;
                }
            }
        }
        else 
            nhArray=arrayTmp;
        if(result['5']!=null&&result['5']!="*" && station!="NJ")  
        {   
            indexData=5;    
            var k=result['5'].split('*');
            if (k.length==2){
                var kx=k[0].split('|');
                var ky=k[1].split('|');
                for (var i = 0; i < kx.length; i++) {
                    var tmp = { x: kx[i]*1000, y: ConvertToFloat(ky[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(ky[i]))
                        dblMax = ConvertToFloat(ky[i]);
                    
                    if (dblMin > ConvertToFloat(ky[i]))
                        dblMin = ConvertToFloat(ky[i]);
                    kArray[i] = tmp;
                }
            }
        }
        else 
            kArray=arrayTmp;
        if(result['6']!=null&&result['6']!="*" && station!="NJ")  
        {   
            indexData=6;
            var mg=result['6'].split('*');
            if (mg.length==2){
                var mgx=mg[0].split('|');
                var mgy=mg[1].split('|');
                for (var i = 0; i < mgx.length; i++) {
                    var tmp = { x: mgx[i]*1000, y: ConvertToFloat(mgy[i]) };
                    //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(mgy[i]))
                        dblMax = ConvertToFloat(mgy[i]);
                    
                    if (dblMin > ConvertToFloat(mgy[i]))
                        dblMin = ConvertToFloat(mgy[i]);
                    mgArray[i] = tmp;
                }
            }
        }
        else
            mgArray=arrayTmp;
        if(result['7']!=null&&result['7']!="*" && station!="NJ")   
        {  
            indexData=7;
            var ca=result['7'].split('*');
            if (ca.length==2){
                var cax=ca[0].split('|');
                var cay=ca[1].split('|');
                for (var i = 0; i < cax.length; i++) {
                    var tmp = { x: cax[i]*1000, y: ConvertToFloat(cay[i]) };
                     //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(cay[i]))
                        dblMax = ConvertToFloat(cay[i]);
                    
                    if (dblMin > ConvertToFloat(cay[i]))
                        dblMin = ConvertToFloat(cay[i]);
                    caArray[i] = tmp;
                }
            }
        }
        else 
            caArray=arrayTmp;
        if(result['8']!=null&&result['8']!="*")
        {
            indexData=8;
            var oc=result['8'].split('*');
            if (oc.length==2){
                var ocx=oc[0].split('|');
                var ocy=oc[1].split('|');
                for (var i = 0; i < ocx.length; i++) {
                    var tmp = { x: ocx[i]*1000, y: ConvertToFloat(ocy[i]) };
                     //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(ocy[i]))
                        dblMax = ConvertToFloat(ocy[i]);
                    
                    if (dblMin > ConvertToFloat(ocy[i]))
                        dblMin = ConvertToFloat(ocy[i]);
                    ocArray[i] = tmp;
                }
            }
        }
        else 
            ocArray=arrayTmp;;
        if(result['9']!=null&&result['9']!="*")
        {
            indexData=9;
            var ec=result['9'].split('*');
            if (ec.length==2){
                var ecx=ec[0].split('|');
                var ecy=ec[1].split('|');
                for (var i = 0; i < ecx.length; i++) {
                    var tmp = { x: ecx[i]*1000, y: ConvertToFloat(ecy[i]) };
                     //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(ecy[i]))
                        dblMax = ConvertToFloat(ecy[i]);
                    
                    if (dblMin > ConvertToFloat(ecy[i]))
                        dblMin = ConvertToFloat(ecy[i]);
                    ecArray[i] = tmp;
                }
            }
        }
        else 
            ecArray=arrayTmp;
        if(result['10']!=null&&result['10']!="*")
        {
            indexData=10;
            var ocG=result['10'].split('*');
            if (ocG.length==2){
                var ocGx=ocG[0].split('|');
                var ocGy=ocG[1].split('|');
                for (var i = 0; i < ocGx.length; i++) {
                    var tmp = { x: ocGx[i]*1000, y: ConvertToFloat(ocGy[i]) };
                     //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(ocGy[i]))
                        dblMax = ConvertToFloat(ocGy[i]);
                    
                    if (dblMin > ConvertToFloat(ocGy[i]))
                        dblMin = ConvertToFloat(ocGy[i]);
                    ocGArray[i] = tmp;
                }
            }
        }
         else 
            ocGArray=arrayTmp;
        if(result['11']!=null&&result['11']!="*")
        {
         
            indexData=11;
            var ecG=result['11'].split('*');
            if (ecG.length==2){
                var ecGx=ecG[0].split('|');
                var ecGy=ecG[1].split('|');
                for (var i = 0; i < ecGx.length; i++) {
                    var tmp = { x: ecGx[i]*1000, y: ConvertToFloat(ecGy[i]) };
                     //获取Y轴的最大和最小值
                    if (dblMax < ConvertToFloat(ecGy[i]))
                        dblMax = ConvertToFloat(ecGy[i]);
                    
                    if (dblMin > ConvertToFloat(ecGy[i]))
                        dblMin = ConvertToFloat(ecGy[i]);
                    ecGArray[i] = tmp;
                }
            }
        }
         else 
            ecGArray=arrayTmp;
        var pm=result['12'].split('*');
        if (pm[1]!=""){
            indexData=12;
            var pmx=pm[0].split('|');
            var pmy=pm[1].split('|');
            for (var i = 0; i < pmx.length; i++) {
                var yValue=null;
                if (pmy[i]!="")
                    yValue= ConvertToFloat(pmy[i]);
                var tmp = { x: pmx[i]*1000, y: yValue };
                //获取Y轴的最大和最小值
                if (dblMax < ConvertToFloat(pmy[i]))
                    dblMax = ConvertToFloat(pmy[i]);
                
                if (dblMin > ConvertToFloat(pmy[i]))
                    dblMin = ConvertToFloat(pmy[i]);
                
                pmArray[i] = tmp;
            }
        }
        else 
        {
             pmArray=arrayTmp;
         }
        dblMax = dblMax + 1;
        dblMin = dblMin - 1;
        if (dblMin < 0)
            dblMin = 0;
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
        // create the chart
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
                colors: ['#a568d2', '#c00000', '#f814e2', '#9e480e', '#00b0f0', '#997300', '#70ad47', '#ed7d31','#ddd233', '#5694ba', '#bbac56', '#3f753b', 'gray'],
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
                navigator:{
                    baseSeries:indexData,
                    series: {
	                    type: 'spline',
	                    color: '#4572A7',
	                    fillOpacity: 0.4,
	                    dataGrouping: {
		                    smoothed: true,
		                    enabled:false
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
                                return  Highcharts.dateFormat('%m月%d', this.value);
                            }
	                    }
                    }
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
                    title: {text: 'ug/m3'},
                    height: 270,
                    offset: 0,
                    lineWidth: 2
                }],
                plotOptions: {
                    spline: {
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
                    column: {
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
                    type: 'column',
                    name: 'K',
                    data: kArray
                },{
                    type: 'column',
                    name: 'Ca',
                    data: caArray
                },{
                    type: 'column',
                    name: 'Na',
                    data: naArray,
                    visible:false
                },{
                    type: 'column',
                    name: 'Mg',
                    data: mgArray,
                    visible:false
                },{
                    type: 'column',
                    name: 'CL',
                    data: clArray,
                    visible:false
                },{
                    type: 'column',
                    name: 'NH4',
                    data: nhArray
                },{
                    type: 'column',
                    name: 'NO3',
                    data: noArray
                },{
                    type: 'column',
                    name: 'SO4',
                    data: soArray
                },{
                    type: 'column',
                    name: 'OC*1.4(热学)',
                    data: ocArray
                },{
                    type: 'column',
                    name: 'EC(热学)',
                    data: ecArray
                },{
                    type: 'column',
                    name: 'OC*1.4(光学)',
                    data: ocGArray,
                    visible:false
                },{
                   type: 'column',
                    name: 'EC(光学)',
                    data: ecGArray,
                    visible:false
                },{
                    type: 'spline',
                    name: 'PM2.5',
                    data: pmArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}