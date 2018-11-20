// JScript 文件

//获取系统提交的URL的格式
function getUrl(provider,method)
{
    return "PatrolHandler.do?provider=" + provider + "&method=" + method;
}
function changeDateSucessed(result){
    for(var obj in result){
      divContaner = Ext.getDom(obj);
        if(divContaner != null){
            if(divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA" )
                divContaner.value = result[obj];
            else
            {
              divContaner.innerHTML = result[obj];//日平均值
            }
      }  
   }
} 
function showsea(whichSea) {
    
    SelectThisRow(whichSea);
                
}
function simple_tooltip(target_items, name){
 $(target_items).each(function(i){
        var temp=this.id;
        var m=document.getElementById(temp);
        var id;
        if(temp.length==4)
            id=parseInt(temp.substr(1,2));
        else 
           id=parseInt(temp.substr(1,3)); 
           
		var my_tooltip = $("#ph"+id.toString()+"1");
		var imgID="m"+temp;
		var swfID="";
		$(this).removeAttr("title").mouseover(function(){
		        var left=getElementLeft(document.getElementById(imgID),null)
                var top=getElementTop(document.getElementById(imgID),null);
                var src=this.src;
                var position=src.lastIndexOf("_");
                var index=src.substr(position+1,1);
                swfID="swf"+index;
                $("#"+swfID).css({left:left-15+'px',top:top-18+'px'}).show();
                $("#"+swfID).mouseout(function(){$(this).hide(),my_tooltip.hide();})
				my_tooltip.css({ display:"none"}).fadeIn(400);
		}).mousemove(function(kmouse){
		        showsea("h"+id.toString());
				my_tooltip.css({left:kmouse.pageX-my_tooltip.width()/2, top:kmouse.pageY-my_tooltip.height()-10});
		});
		
	});
} 
function changeDate()
{
 Ext.Ajax.request({ 
        url: getUrl('SEMCShare.WebSiteJS','ZoneForecastData'),
        timeout:120000, 
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                    changeDateSucessed(result);
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     }); 
}
function SelectThisRow(cellID) {
    var cell=cellID+"1";
    var tableCell = document.getElementById(cell);  
    if(tableCell != null){  
        if (selectedRow != null) {
	        selectedRow.style.backgroundColor = lastColor;
        }
        
        selectedRow = tableCell.parentNode;
        lastColor = selectedRow.style.backgroundColor;
        selectedRow.style.backgroundColor = "#97cbff";
    }
}
function mouseOver(obj)
{
    if(obj!=null)
    {
       obj.style.backgroundColor = "#97cbff";
    }

}
function mouseOut(obj)
{
 if(obj!=null)
    {
       obj.style.backgroundColor = "";
    }
}
function convertDate(date){
    var flag = true; 
    
    if(date.indexOf('年')>0)
        date = date.replace('年','-');
    if(date.indexOf('月')>0)
        date = date.replace('月','-');
    if(date.indexOf('日')>0)
        date = date.replace('日','');
        
    var dateParts =  date.split(" ");
    var dateArray = dateParts[0].split("-");
    if (dateArray.length != 3) {
        dateArray = date.split("/");
        if (dateArray.length != 3) {
             return null;
        }
        flag = false;
     }
     var newDate = new Date();
     if (flag) {
        // month从0开始
        newDate.setFullYear(dateArray[0], dateArray[1] - 1, dateArray[2]);   
     }
     else {
           newDate.setFullYear(dateArray[2], dateArray[1] - 1, dateArray[0]);
      }
      if(dateParts.lenght > 1){
        var times = dateParts[1].split(":");
        newDate.setHours(times[0], times[1], times[2]);
      }else
        newDate.setHours(0, 0, 0);
        
      return newDate;
}

//获取事件触发的控件,兼容IE和firefox
function getEventSource(evt){
   return evt.target || window.event.srcElement;
}

//显示控件
function showEl(elID){
    var el = Ext.getDom(elID);
    el.className = el.className.replace('hidden','show');
}
//隐藏控件
function hideEl(elID){
    var el = Ext.getDom(elID);
    el.className = el.className.replace('show','hidden');
}

//由于火狐浏览器不支持innerText而支持，因此对于设置innerText的功能需要进行判断
function supportInnerText(){
    if(!Ext.isIE){ //firefox innerText define
        HTMLElement.prototype.__defineGetter__("innerText", 
        function(){
            var anyString = "";
            var childS = this.childNodes;
            for(var i=0; i<childS.length; i++) { 
                if(childS[i].nodeType==1)
                    //anyString += childS[i].tagName=="BR" ? "\n" : childS[i].innerText;
                    anyString += childS[i].innerText;
                else if(childS[i].nodeType==3)
                    anyString += childS[i].nodeValue;
            }
            return anyString;
        } 
        ); 
        HTMLElement.prototype.__defineSetter__("innerText", 
        function(sText){
            this.textContent=sText; 
        } 
        ); 
    }
}

//获取元素在指定容器中的相对位置。
function getElementLeft(element,targetParent){
　　　　var actualLeft = element.offsetLeft;
　　　　var current = element.offsetParent;
　　　　while (current !== null){
　　　　　　actualLeft += current.offsetLeft;
　　　　　　current = current.offsetParent;
　　　　　　if(current == targetParent)
　　　　        break;

　　　　}
　　　　return actualLeft;
}
function getElementTop(element,targetParent){
　　var actualTop = element.offsetTop;
　　var current = element.offsetParent;
　　while (current !== null){
　　　　actualTop += current.offsetTop;
　　　　current = current.offsetParent;
　　　　if(current == targetParent)
　　　　    break;
　　}
　　return actualTop;
}
//取Js变量的类型
function getParamType(param) {
    return ((_t = typeof (param)) == "object" ? Object.prototype.toString.call(param).slice(8, -1) : _t).toLowerCase();
}
//保留小数
function fomatFloat(src, pos) {
    return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
}

 function get_length(s){
    var char_length = 0;
    for (var i = 0; i < s.length; i++){
        var son_char = s.charAt(i);
        encodeURI(son_char).length > 2 ? char_length += 1 : char_length += 0.5;
    }
    return char_length;
}
    
    
function cut_str(str, len){
    var char_length = 0;
    for (var i = 0; i < str.length; i++){
        var son_str = str.charAt(i);
        encodeURI(son_str).length > 2 ? char_length += 1 : char_length += 0.5;
        if (char_length >= len){
            var sub_len = char_length == len ? i+1 : i;
            return str.substr(0, sub_len);
            break;
        }
    } 
}
function refreshDataTable(groupID)
{
 var obj= Ext.getDom("polluTable");
     Ext.Ajax.request({ 
                url: getUrl('SEMCShare.WebSiteJS','ZoneData'),
                 params: {groupID:groupID}, 
                timeout:120000,
                success: function(response){
                   obj.innerHTML=response.responseText;
                    Ext.Ajax.request({ 
                        url: getUrl('SEMCShare.WebSiteJS','SrcData'),
                        params: {groupID:groupID},
                        success: function(response){
                           if(response.responseText!=""){
                                var result = Ext.util.JSON.decode(response.responseText);
                                    changeDateData(result);
                            }
                        }, 
                        failure: function(response) { 
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                        }
                    });  
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
            });  
}
function changeDateData(result){
    for(var obj in result){
      divContaner = Ext.getDom(obj);
        if(divContaner != null){
        divContaner.src=result[obj];
      }  
   }
} 
function hideZoneData(groupID)
{
 Ext.Ajax.request({ 
                url: getUrl('SEMCShare.WebSiteJS','HiddenZoneDataDiv'),
                params: {groupID:groupID},
                timeout:120000,
                success: function(response){
                    if(response.responseText!=""){
                        
                        GiveData(response.responseText);
                            }
                   
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
            });  
}
function GiveData(result)
{
if (result!=null)
    {
       var clx=result.split('|');
       for(var i=0;i<clx.length;i++)
       {
          var clData=clx[i].split('*');
          var id=clData[0];
          var obj= Ext.getDom(id);
          if(obj!=null)
              obj.innerHTML=clData[1];
          
       }
   }
}
function Chart(groupID)
{
    Ext.Ajax.request({
        url: getUrl('SEMCShare.WebSiteJS','chartAQI'),
        params: {groupID:groupID}, 
        timeout:120000,
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                 if(Ext.isChrome)
                     RenderChartChrom(result);
                 else 
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
function ChartDay(groupID)
{
    Ext.Ajax.request({
        url: getUrl('SEMCShare.WebSiteJS','chartAQIDay'),
        params: {groupID:groupID}, 
        timeout:120000,
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                if(Ext.isChrome)
                     RenderChartDayChrom(result);
                 else 
                    RenderChartDay(result);
            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。"); 
        },
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function ChartMonth(groupID)
{
    Ext.Ajax.request({
        url: getUrl('SEMCShare.WebSiteJS','chartAQIMonth'),
        params: {groupID:groupID}, 
        timeout:120000,
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                 if(Ext.isChrome)
                     RenderChartMonthChrom(result);
                 else 
                    RenderChartMonth(result);
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
             
    var dblMax = 0;
    var color;
    if(Ext.isIE8||Ext.isIE7||Ext.isIE6)
     color="white";
     else 
     color="rgba(255,255,255,0)";
    var dblMin = 10000;
     var AQIArray=new Array()
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==6){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            var clz=cl[2].split('|');
            var clk=cl[3].split('|');
            var clt=cl[4].split('|');
            var clm=cl[5].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y:parseInt(cly[i]), z:clz[i], k:clk[i], t:clt[i], m:clm[i]};
                //获取Y轴的最大和最小值
                if (dblMax < parseInt(cly[i]))
                    dblMax =parseInt(cly[i]);
                
                if (dblMin > parseInt(cly[i]))
                    dblMin =parseInt(cly[i]);
                AQIArray[i] = tmp;
            }
        }
        var obj=Ext.getDom("left");
        if(dblMax<=200)
        {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE1";
         else 
            obj.className="img1";
           for(var i=0;i<5;i++)
           {
            var tick=50*i;
             positions1.push(tick);
            }
         }
         else if(dblMax<=250)
         {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE2";
         else 
            obj.className="img2";
           for(var i=0;i<6;i++)
           {
            var tick=50*i;
             positions1.push(tick);
            }
         }
         else
         {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE3";
         else 
            obj.className="img3";
           for(var i=0;i<7;i++)
           {
            var tick=50*i;
             positions1.push(tick);
            }
         }
        Highcharts.setOptions({
            lang: {
                rangeSelectorFrom: '从',
                rangeSelectorTo: '到',
                rangeSelectorZoom: ''
            }
        });
        // create the chart
       $('#AQI_R').highcharts({
                chart: {
                backgroundColor: 'rgba(255,255,255,0)',
                type: 'spline'
                },
                colors: [ '#E48701'],
                credits:{enabled : false},
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                headerFormat: '',
                  pointFormat: '实时指数:&nbsp;&nbsp;{point.m}<br/>'+
                  '质量状况:&nbsp;&nbsp;{point.k}<br/>'+
                    '首要污染物:&nbsp;&nbsp;{point.z}<br/>'+
                   '时间:&nbsp;&nbsp;{point.t}',
                footerFormat: '',
                shared: true,
                useHTML: true
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval:3600 * 1000*3,
                    
                    labels: {
                      style: {
                        fontSize: '10px'
                     },
                        formatter: function() {
                            return  Highcharts.dateFormat('%H:00', this.value);
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
                  title: {
                    enabled: false
                },
                labels: {
                    style: {
                        color: color
                        }
                },
                  tickPositions: positions1 // 指定竖轴坐标点的值
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
                            radius: 3,
                            symbol:"circle",
                            fillColor:"#FFFFFF",
                            lineColor:"#E48701",
                            lineWidth: 3
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
               title: {
                text: ''
            },
                legend: {
                    enabled: false
                },
                series:
                [{
                    type: 'spline',
                    name: 'PM2.5',
                    data: AQIArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartChrom(result)
{     
             
    var dblMax = 0;
    var color;
    if(Ext.isIE8||Ext.isIE7||Ext.isIE6)
     color="white";
     else 
     color="rgba(255,255,255,0)";
    var dblMin = 10000;
     var AQIArray=new Array()
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==6){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            var clz=cl[2].split('|');
            var clk=cl[3].split('|');
            var clt=cl[4].split('|');
            var clm=cl[5].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y:parseInt(cly[i]), z:clz[i], k:clk[i], t:clt[i], m:clm[i]};
                //获取Y轴的最大和最小值
                if (dblMax < parseInt(cly[i]))
                    dblMax =parseInt(cly[i]);
                
                if (dblMin > parseInt(cly[i]))
                    dblMin =parseInt(cly[i]);
                AQIArray[i] = tmp;
            }
        }
        var obj=Ext.getDom("left");
        if(dblMax<=200)
        {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE1";
         else 
            obj.className="img1";
           for(var i=0;i<5;i++)
           {
            var tick=50*i;
             positions1.push(tick);
            }
         }
         else if(dblMax<=250)
         {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE2";
         else 
            obj.className="img2";
           for(var i=0;i<6;i++)
           {
            var tick=50*i;
             positions1.push(tick);
            }
         }
         else
         {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE3";
         else 
            obj.className="img3";
           for(var i=0;i<7;i++)
           {
            var tick=50*i;
             positions1.push(tick);
            }
         }
        Highcharts.setOptions({
            lang: {
                rangeSelectorFrom: '从',
                rangeSelectorTo: '到',
                rangeSelectorZoom: ''
            }
        });
        // create the chart
       $('#AQI_R').highcharts({
                chart: {
                backgroundColor: 'rgba(255,255,255,0)',
                type: 'spline'
                },
                colors: [ '#E48701'],
                credits:{enabled : false},
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                headerFormat: '',
                  pointFormat: '实时指数:&nbsp;&nbsp;{point.m}<br/>'+
                  '质量状况:&nbsp;&nbsp;{point.k}<br/>'+
                    '首要污染物:&nbsp;&nbsp;{point.z}<br/>'+
                   '时间:&nbsp;&nbsp;{point.t}',
                footerFormat: '',
                shared: true,
                useHTML: true
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval:3600 * 1000*3,
                    
                    labels: {
                      style: {
                        font: '8px Trebuchet MS, Verdana, sans-serif'
                     },
                        formatter: function() {
                            return  Highcharts.dateFormat('%H:00', this.value);
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
                  title: {
                    enabled: false
                },
                labels: {
                    style: {
                        color: color
                        }
                },
                  tickPositions: positions1 // 指定竖轴坐标点的值
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
                            radius: 3,
                            symbol:"circle",
                            fillColor:"#FFFFFF",
                            lineColor:"#E48701",
                            lineWidth: 3
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
               title: {
                text: ''
            },
                legend: {
                    enabled: false
                },
                series:
                [{
                    type: 'spline',
                    name: 'PM2.5',
                    data: AQIArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartMonth(result)
{
    var dblMax = 0;
    var dblMin = 10000;
     var AQIArray=new Array();
     var colorArray=new Array();
    if (result!=null)
    {
        var cl=result['0'].split('*');
        var clx=cl[0].split('|');
        if (cl.length==5){
            var cly=cl[1].split('|');
            for (var i = 0; i < clx.length; i++) {
                if(parseInt(cly[i])<=50)
                    color="#00e400";
                else if(parseInt(cly[i])<=100)    
                    color="#FFFF00";
                else if(parseInt(cly[i])<=150)    
                    color="#ffa500";
                else if(parseInt(cly[i])<=200)    
                    color="#ff0000";
                else if(parseInt(cly[i])<=300)    
                    color="#800080";
                else 
                    color="#7e0023";
                colorArray[i]=color;
            }
        }
        Highcharts.setOptions({
           colors: colorArray  
        });
       Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function(color) {
		    return {
		        linearGradient: { x1: 1, y1: 1, x2: 1, y2: 0 },
		        stops: [
		            [0, 'rgb(255, 255, 255)'],
		            [1,color] // darken
		        ]
		    };
		});
		var colors=Highcharts.getOptions().colors;
		if(Ext.isIE8||Ext.isIE7||Ext.isIE6)
          color="white";
        else 
          color="rgba(255,255,255,0)";
		
        if (cl.length==5){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            var clz=cl[2].split('|');
            var clm=cl[3].split('|');
            var clt=cl[4].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y:parseInt(cly[i]), z:clz[i], m:clm[i], t:clt[i],color:colors[i]};
                if (dblMax < parseInt(cly[i]))
                    dblMax =parseInt(cly[i]);
                
                if (dblMin > parseInt(cly[i]))
                    dblMin =parseInt(cly[i]);
                AQIArray[i] = tmp;
            }
        }
        var obj=Ext.getDom("column");
        if(dblMax<=200)
        {
        
        if(Ext.isIE7||Ext.isIE6)
             obj.className="imgR1_IE1";
         else 
            obj.className="imgR1";
           for(var i=0;i<5;i++)
           {
            var tick=50*i;
             positions3.push(tick);
            }
         }
         else if(dblMax<=250)
         {
            if(Ext.isIE7||Ext.isIE6)
             obj.className="imgR2_IE2";
         else 
            obj.className="imgR2";
           for(var i=0;i<6;i++)
           {
            var tick=50*i;
             positions3.push(tick);
            }
         }
         else
         {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="imgR3_IE3";
         else 
            obj.className="imgR3";
           for(var i=0;i<7;i++)
           {
            var tick=50*i;
             positions3.push(tick);
            }
         }
        // create the chart
       $('#30AQI').highcharts({
                chart: {
                backgroundColor: 'rgba(255,255,255,0)',
                type: 'column'
                },
                credits:{enabled : false},
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },

                tooltip: {  
                  headerFormat: '',
                  pointFormat: 'AQI:&nbsp;&nbsp;{point.m}<br/>'+
                    '首要污染物:&nbsp;&nbsp;{point.z}<br/>'+
                      '时间:&nbsp;&nbsp;{point.t}',
                footerFormat: '',
                shared: true,
                useHTML: true
                },
                xAxis: {
                    type: 'datetime',
                    tickPixelInterval: 30,
                    tickInterval:3600 * 1000*24,
                    labels: {
                      style: {
                      fontSize: '10px'
                     },
                        formatter: function() {
                            return  Highcharts.dateFormat('%m-%d', this.value);
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
                  title: {
                    text: ''
                },
                labels: {
                    style: {
                        color: color 
                        }
                },
                  tickPositions: positions3 // 指定竖轴坐标点的值
                }],
                plotOptions: {
                    series:{
//                    allowPointSelect: true,
                     pointPadding: 0.7,
                     pointWidth: 25
                    }
                    
                },
            title: {
                text: ''
                },
                legend: {
                    enabled: false
                },
                series:
                [{
                    type: 'column',
                    data: AQIArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartMonthChrom(result)
{
    var dblMax = 0;
    var dblMin = 10000;
     var AQIArray=new Array();
     var colorArray=new Array();
    if (result!=null)
    {
        var cl=result['0'].split('*');
        var clx=cl[0].split('|');
        if (cl.length==5){
            var cly=cl[1].split('|');
            for (var i = 0; i < clx.length; i++) {
                if(parseInt(cly[i])<=50)
                    color="#00e400";
                else if(parseInt(cly[i])<=100)    
                    color="#FFFF00";
                else if(parseInt(cly[i])<=150)    
                    color="#ffa500";
                else if(parseInt(cly[i])<=200)    
                    color="#ff0000";
                else if(parseInt(cly[i])<=300)    
                    color="#800080";
                else 
                    color="#7e0023";
                colorArray[i]=color;
            }
        }
        Highcharts.setOptions({
           colors: colorArray  
        });
       Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function(color) {
		    return {
		        linearGradient: { x1: 1, y1: 1, x2: 1, y2: 0 },
		        stops: [
		            [0, 'rgb(255, 255, 255)'],
		            [1,color] // darken
		        ]
		    };
		});
		var colors=Highcharts.getOptions().colors;
		if(Ext.isIE8||Ext.isIE7||Ext.isIE6)
          color="white";
        else 
          color="rgba(255,255,255,0)";
		
        if (cl.length==5){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            var clz=cl[2].split('|');
            var clm=cl[3].split('|');
            var clt=cl[4].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y:parseInt(cly[i]), z:clz[i], m:clm[i],t:clt[i],color:colors[i]};
                if (dblMax < parseInt(cly[i]))
                    dblMax =parseInt(cly[i]);
                
                if (dblMin > parseInt(cly[i]))
                    dblMin =parseInt(cly[i]);
                AQIArray[i] = tmp;
            }
        }
        var obj=Ext.getDom("column");
        if(dblMax<=200)
        {
        
        if(Ext.isIE7||Ext.isIE6)
             obj.className="imgR1_IE1";
         else 
            obj.className="imgR1";
           for(var i=0;i<5;i++)
           {
            var tick=50*i;
             positions3.push(tick);
            }
         }
         else if(dblMax<=250)
         {
            if(Ext.isIE7||Ext.isIE6)
             obj.className="imgR2_IE2";
         else 
            obj.className="imgR2";
           for(var i=0;i<6;i++)
           {
            var tick=50*i;
             positions3.push(tick);
            }
         }
         else
         {
           if(Ext.isIE7||Ext.isIE6)
             obj.className="imgR3_IE3";
         else 
            obj.className="imgR3";
           for(var i=0;i<7;i++)
           {
            var tick=50*i;
             positions3.push(tick);
            }
         }
        // create the chart
       $('#30AQI').highcharts({
                chart: {
                backgroundColor: 'rgba(255,255,255,0)',
                type: 'column'
                },
                credits:{enabled : false},
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },

                tooltip: {  
                  headerFormat: '',
                  pointFormat: 'AQI:&nbsp;&nbsp;{point.m}<br/>'+
                    '首要污染物:&nbsp;&nbsp;{point.z}<br/>'+
                      '时间:&nbsp;&nbsp;{point.t}',
                footerFormat: '',
                shared: true,
                useHTML: true
                },
                xAxis: {
                    type: 'datetime',
                    tickPixelInterval: 30,
                    tickInterval:3600 * 1000*24,
                    labels: {
                      style: {
                      font: '8px Trebuchet MS, Verdana, sans-serif'
                     },
                        formatter: function() {
                            return  Highcharts.dateFormat('%m-%d', this.value);
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
                  title: {
                    text: ''
                },
                labels: {
                    style: {
                        color: color 
                        }
                },
                  tickPositions: positions3 // 指定竖轴坐标点的值
                }],
                plotOptions: {
                    series:{
//                    allowPointSelect: true,
                     pointPadding: 0.7,
                     pointWidth: 25
                    }
                    
                },
            title: {
                text: ''
                },
                legend: {
                    enabled: false
                },
                series:
                [{
                    type: 'column',
                    data: AQIArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}
function RenderChartDay(result)
{
    var dblMax = 0;
    var dblMin = 10000;
     var AQIArray=new Array()
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==6){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            var clz=cl[2].split('|');
            var clk=cl[3].split('|');
            var clt=cl[4].split('|');
            var clm=cl[5].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y:parseInt(cly[i]), z:clz[i], k:clk[i], t:clt[i], m:clm[i]};
                //获取Y轴的最大和最小值
                if (dblMax < parseInt(cly[i]))
                    dblMax =parseInt(cly[i]);
                
                if (dblMin > parseInt(cly[i]))
                    dblMin =parseInt(cly[i]);
                AQIArray[i] = tmp;
            }
        }
        if(Ext.isIE8||Ext.isIE7||Ext.isIE6)
         color="white";
       else 
         color="rgba(255,255,255,0)";
         var obj=Ext.getDom("middleLine");
        if(dblMax<=200)
        {
         if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE1";
         else 
            obj.className="img1";
           for(var i=0;i<5;i++)
           {
            var tick=50*i;
             positions2.push(tick);
            }
         }
         else if(dblMax<=250)
         {
          if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE2";
         else 
            obj.className="img2";
           for(var i=0;i<6;i++)
           {
            var tick=50*i;
             positions2.push(tick);
            }
         }
         else
         {
          if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE3";
         else 
            obj.className="img3";
           for(var i=0;i<7;i++)
           {
            var tick=50*i;
             positions2.push(tick);
            }
         }
        Highcharts.setOptions({
            lang: {
                rangeSelectorFrom: '从',
                rangeSelectorTo: '到',
                rangeSelectorZoom: ''
            }
        });
        // create the chart
       $('#24AQI').highcharts({
                chart: {
                backgroundColor: 'rgba(255,255,255,0)',
                type: 'spline'
                },
                colors: [ '#E48701'],
                credits:{enabled : false},
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: { 
 
                headerFormat: '',
                  pointFormat: '实时指数:&nbsp;&nbsp;{point.m}<br/>'+
                  '质量状况:&nbsp;&nbsp;{point.k}<br/>'+
                    '首要污染物:&nbsp;&nbsp;{point.z}<br/>'+
                   '时间:&nbsp;&nbsp;{point.t}',
                footerFormat: '',
                shared: true,
                useHTML: true
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval:3600 * 1000*3,
                    labels: {
                     style: {
                     fontSize: '10px'
                     },
                        formatter: function() {
                            return  Highcharts.dateFormat('%H:00', this.value);
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
                  title: {
                    text: ''
                },
                labels: {
                    style: {
                        color: color
                        }
                },
                  tickPositions: positions2  // 指定竖轴坐标点的值
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
                            radius: 3,
                            symbol:"circle",
                            fillColor:"#FFFFFF",
                            lineColor:"#E48701",
                            lineWidth: 3
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
                            title: {
                text: ''
            },
                legend: {
                    enabled: false
                },
                series:
                [{
                    type: 'spline',
                    data: AQIArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
} 
function RenderChartDayChrom(result)
{
    var dblMax = 0;
    var dblMin = 10000;
     var AQIArray=new Array()
    if (result!=null)
    {
        var cl=result['0'].split('*');
        if (cl.length==6){
            var clx=cl[0].split('|');
            var cly=cl[1].split('|');
            var clz=cl[2].split('|');
            var clk=cl[3].split('|');
            var clt=cl[4].split('|');
            var clm=cl[5].split('|');
            for (var i = 0; i < clx.length; i++) {
                var tmp = { x: clx[i]*1000, y:parseInt(cly[i]), z:clz[i], k:clk[i], t:clt[i], m:clm[i]};
                //获取Y轴的最大和最小值
                if (dblMax < parseInt(cly[i]))
                    dblMax =parseInt(cly[i]);
                
                if (dblMin > parseInt(cly[i]))
                    dblMin =parseInt(cly[i]);
                AQIArray[i] = tmp;
            }
        }
        if(Ext.isIE8||Ext.isIE7||Ext.isIE6)
         color="white";
       else 
         color="rgba(255,255,255,0)";
         var obj=Ext.getDom("middleLine");
        if(dblMax<=200)
        {
         if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE1";
         else 
            obj.className="img1";
           for(var i=0;i<5;i++)
           {
            var tick=50*i;
             positions2.push(tick);
            }
         }
         else if(dblMax<=250)
         {
          if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE2";
         else 
            obj.className="img2";
           for(var i=0;i<6;i++)
           {
            var tick=50*i;
             positions2.push(tick);
            }
         }
         else
         {
          if(Ext.isIE7||Ext.isIE6)
             obj.className="img1_IE3";
         else 
            obj.className="img3";
           for(var i=0;i<7;i++)
           {
            var tick=50*i;
             positions2.push(tick);
            }
         }
        Highcharts.setOptions({
            lang: {
                rangeSelectorFrom: '从',
                rangeSelectorTo: '到',
                rangeSelectorZoom: ''
            }
        });
        // create the chart
       $('#24AQI').highcharts({
                chart: {
                backgroundColor: 'rgba(255,255,255,0)',
                type: 'spline'
                },
                colors: [ '#E48701'],
                credits:{enabled : false},
                global:{useUTC : false},
                exporting: {
                    enabled: false
                },
                tooltip: {  
                headerFormat: '',
                  pointFormat: '实时指数:&nbsp;&nbsp;{point.m}<br/>'+
                  '质量状况:&nbsp;&nbsp;{point.k}<br/>'+
                    '首要污染物:&nbsp;&nbsp;{point.z}<br/>'+
                   '时间:&nbsp;&nbsp;{point.t}',
                footerFormat: '',
                shared: true,
                useHTML: true
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval:3600 * 1000*3,
                    labels: {
                     style: {
                         font: '8px Trebuchet MS, Verdana, sans-serif'
                     },
                        formatter: function() {
                            return  Highcharts.dateFormat('%H:00', this.value);
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
                  title: {
                    text: ''
                },
                labels: {
                    style: {
                        color: color
                        }
                },
                  tickPositions: positions2  // 指定竖轴坐标点的值
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
                            radius: 3,
                            symbol:"circle",
                            fillColor:"#FFFFFF",
                            lineColor:"#E48701",
                            lineWidth: 3
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
                            title: {
                text: ''
            },
                legend: {
                    enabled: false
                },
                series:
                [{
                    type: 'spline',
                    data: AQIArray
                }]
            });
    }
    else
        alert("没有满足条件的信息!");
}   

function showDiv(){
    document.getElementById("Div1").style.display="block";
    document.getElementById("Div1").style.left=(document.body.clientWidth-document.getElementById("Div1").clientWidth)/2+document.body.scrollLeft;   
}
        
function hideDiv(){
    document.getElementById("Div1").style.display="none";
}
 function showDjDiv(obj){
    var src=obj.src;
    if(src.indexOf("1.png")>0){
        document.getElementById("Djimg1").className="popup2_info1"
    }
     if(src.indexOf("2.png")>0){
        document.getElementById("Djimg2").className="popup2_info2"
    }
     if(src.indexOf("3.png")>0){
        document.getElementById("Djimg3").className="popup2_info3"
    }
     if(src.indexOf("4.png")>0){
        document.getElementById("Djimg4").className="popup2_info4"
    }
     if(src.indexOf("5.png")>0){
        document.getElementById("Djimg5").className="popup2_info5"
    }
     if(src.indexOf("6.png")>0){
        document.getElementById("Djimg6").className="popup2_info6"
    }
    document.getElementById("DjDiv").style.display="block";
     document.getElementById("DjDiv").style.left=(document.body.clientWidth-document.getElementById("DjDiv").clientWidth)/2+document.body.scrollLeft;   
}
function hideDjDiv(){
    document.getElementById("DjDiv").style.display="none";
}