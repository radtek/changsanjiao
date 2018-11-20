var id;
var titleText;
function mouseDown(evt)
{
    evt=evt || event;
    var obj=Ext.getDom("tableDiv");
    id=obj;
    posX=evt.clientX-parseInt(obj.offsetLeft);
    posY=evt.clientY - parseInt(obj.offsetTop);
    document.onmousemove = mousemove; 
    
}
function mouseDownComment(evt)
{
    evt=evt || event;
    var obj=Ext.getDom("CommentDiv");
    id=obj;
    var el=evt.target|| evt.srcElement;
    if(el.id!="OutDiv")
    {
        posX=evt.clientX-parseInt(obj.offsetLeft);
        posY=evt.clientY - parseInt(obj.offsetTop);
        document.onmousemove = mousemove; 
    }
    
}
function weekmouseDown(evt)
{
    evt=evt || event;
    var obj=Ext.getDom("WeekDiv");
    id=obj;
    posX=evt.clientX-parseInt(obj.offsetLeft);
    posY=evt.clientY - parseInt(obj.offsetTop);
    document.onmousemove = mousemove; 
}
function mousemove(ev)
{
    if(ev==null) ev = window.event;//如果是IE
    var obj=id;
    obj.style.left = (ev.clientX - posX) + "px";
    obj.style.top = (ev.clientY - posY) + "px";	
    obj.style.cursor="move";
}
document.onmouseup = function()
{
  document.onmousemove = null;
}
function closeTable()
{
    Ext.getDom("tableDiv").style.display="none";
}
function closeWeek()
{
 Ext.getDom("WeekDiv").style.display="none";
}
function closeComment()
{
    Ext.getDom("CommentDiv").style.display="none";
}
function tableImage(el,entityName){
    if(Ext.getDom("quxianWai")!=null)
       Ext.getDom("quxianWai").style.display = "none";
    var src=el.dom.src;
    var index=src.lastIndexOf("/");
    var nameProp=src.substr(index+1);
    var lastIndex=nameProp.lastIndexOf("_");
    var subString=nameProp.substr(0,lastIndex);
    var Pm25Class=nameProp.substr(lastIndex+1);
    
    var secIndex=subString.lastIndexOf("_");
    subString=subString.substr(secIndex+1);
    var methodName;
    if(entityName=="PM2_5")
     methodName="chinaTable";
    else if(entityName=="pm2.5_class")
    {
     methodName="DateChinaTable";
     subString=Pm25Class;
     }
    else
    {
     methodName="PM10ChinaTable";
     }
     Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog',methodName),
            params: { dateTime:subString},
            success: function(response){
                if(response.responseText!=""){
                   Ext.getDom("tableDiv").innerHTML=response.responseText;
                   Ext.getDom("tableDiv").style.display = "block";
                }
                 
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function quxianImage(entityName)
{
  if(Ext.getDom("tableDiv")!=null)
    Ext.getDom("tableDiv").style.display="none";
  var stationName="";
  var initStation="上海,嘉兴,苏州,南通,无锡";
  var station= getCheckBValue("CheckStation");
  if(station=="")
     stationName=initStation;
  else 
     stationName=station;
  stationCheckStr=stationName;
  arrayStation=stationName.split(',');
  
  
    var startDateTime;
    var endDateTime;
    var methodName;
    startDateTime=startDate.format('Y-m-d H:i:s');
    endDateTime=endDate.format('Y-m-d H:i:s');
    if(entityName=="PM2_5")
    {
      methodName="chinaQuxian";
      titleText="PM2.5污染物小时曲线";
    }
    if(entityName=="pm2.5_class")
    {
      methodName="DatechinaQuxian";
      startDateTime=startDate.format('Y/m/d 0:00:00');
      endDateTime=endDate.format('Y/m/d 0:00:00');
      titleText="PM2.5污染物日报曲线";
    }
    if(entityName=="PM10")
    {
      methodName="chinaPM10Quxian";
      titleText="PM10污染物小时曲线";
    }
  var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"曲线正在生成中..."});
     myMask.show();
      Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.PublicLog',methodName),
        params: { startDateTime:startDateTime,endDateTime:endDateTime,initStation:stationName},
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                 myMask.hide();
                Ext.getDom("quxianWai").style.display = "block";
                RenderChart(result);
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
    var length=0;
    while(result[length]!=null)
      length++;
    var yTitle='ug/m3';
    var ArrayData=new Array(); 
        //增加曲线
        for (var j = 0; j < length; j++) {
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
                        var tmp= { x: clx[k] * 1000, y: yValue };
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
        $('#quxianDiv').highcharts('StockChart', {
         chart: {
                type: 'line',
                backgroundColor:""
            },

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
                inputDateFormat: '%m-%d %H:00'
            },
            colors: ['#0000FF', '#009900', '#990033', 'Black', '#FF0000', '#996600', 'Teal', 'Purple','Aqua', 'Lime', 'Yellow', 'gray'],
            credits: { enabled: false },
            title: {
                text: titleText
            },
            global: { useUTC: false },
            exporting: {
                enabled: false
            },
            tooltip: {
                formatter: function() {
                    var dt=new Date(parseInt(this.points[0].x)-8*3600*1000);
                    var hour=parseInt(dt.getHours());
                    var tipHour="上午";
                    if (hour<12&&hour>=6)
                        tipHour="上午";
                    else if (hour<20&&hour>=12)
                        tipHour="下午";
                    else
                        tipHour="夜间";
                    var tipMessage= dt.getFullYear() + "-" + (dt.getMonth()+1) + "-" + dt.getDate() + " "+dt.getHours()+":00" +" " + tipHour + "<br/>";
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
                        return Highcharts.dateFormat('%H:00 ', this.value);
                    }
                },
                offset: 0
            },
            yAxis:
            [{
                title: { text:yTitle },
                height: 200,
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
                        lineWidth:2
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
                    cursor: 'pointer', 
                    events: { 
                        click: function(e) { 
                           var dt=new Date(parseInt(e.point.x)-8*3600*1000);
                            lianDong(dt); 
                        } 
                    } 
                }

            },
            legend: {
                y:0,
                backgroundColor: '#FFFFFF',
                borderColor: '#CCC',
                borderWidth: 2,
                shadow: false,
                enabled: true,
                click:function(e)
                {
                legenClick(e)
                }                
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
                    lineWidth: 2,
                    marker: {
                        enabled: false
                    },
                    shadow: false
                },
                xAxis: {
                    tickWidth: 0,
                    lineWidth: 0,
                    gridLineWidth: 2,
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
                name:arrayStation[0],
                data: ArrayData[0]
            }]
        });
        var chart = $('#quxianDiv').highcharts();
        chart.series[0].remove(true);
        for(var i=0;i<length;i++)
        {
            chart.addSeries(
                {
                name: arrayStation[i],
                type: 'line',
                data: ArrayData[i]
                },true
            );
        }
}
function stationName(entityName)
{
 var startDateTime=startDate.format('Y-m-d H:i:s')
 var endDateTime=endDate.format('Y-m-d H:i:s')
      Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.PublicLog','chinaStationName'),
        params: { startDateTime:startDateTime,endDateTime:endDateTime,checkStation:stationCheckStr,entityName:entityName},
        success: function(response){
            if(response.responseText!=""){
                var obj =Ext.getDom("stationName");
                var temp =Ext.getDom("chkSiteContainer");
                var divUsers=Ext.getDom("panGroupContainer"); 
                divUsers.style.left = getElementLeft(obj,divUsers.parent)-600 + "px";
                divUsers.style.bottom ="30px";
                temp.innerHTML ="";
                temp.innerHTML =response.responseText;   

                divUsers.style.display= "block";
                divUsers.style.zIndex =999999999;
            }
             
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function allSelecet()
{
   var obj=document.getElementsByName("CheckStation");
   if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
           if(!obj[i].checked)
           {
                obj[i].checked=true;
           }
      }
    }
}
function fanSelecet()
{
    var obj=document.getElementsByName("CheckStation");
    if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
           if(obj[i].checked)
           {
               obj[i].checked=false;
           }
           else 
           {
               obj[i].checked=true;
           }
      }
    }
}
function OKSelecet()
{
  
   var divUsers=Ext.getDom("panGroupContainer");
   divUsers.style.display="none";
   quxianImage(entityNameLink);
}
function closeCheck()
{
   var divUsers=Ext.getDom("panGroupContainer");
   divUsers.style.display="none";
}
function getCheckBValue(objName)
{
    var postJson = "";
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
    {
         postJson=postJson.substring(0,postJson.length-1);
    }
    return postJson;
} 
function ascTable(dateTime,flag)
{
    PxFunction="ascTable";
    Ext.Ajax.request({ 
                url: getUrl('MMShareBLL.DAL.PublicLog','ascTable'),
                params: { dateTime:dateTime,flag:flag},
                success: function(response){
                    if(response.responseText!=""){
                       var obj=Ext.getDom("tableDiv");
                       obj.innerHTML=response.responseText;
                       obj.style.display = "block";
                    }
                     
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
             });
}
function descTable(dateTime,flag)
{
    PxFunction="descTable";
    Ext.Ajax.request({ 
                url: getUrl('MMShareBLL.DAL.PublicLog','descTable'),
                params: { dateTime:dateTime,flag:flag},
                success: function(response){
                    if(response.responseText!=""){
                       var obj=Ext.getDom("tableDiv");
                       obj.innerHTML=response.responseText;
                       obj.style.display = "block";
                    }
                     
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
             });
}
function normalTable(dateTime,flag)
{
    PxFunction="normalTable";
    Ext.Ajax.request({ 
                url: getUrl('MMShareBLL.DAL.PublicLog','normalTable'),
                params: { dateTime:dateTime,flag:flag},
                success: function(response){
                    if(response.responseText!=""){
                       var obj=Ext.getDom("tableDiv");
                       obj.innerHTML=response.responseText;
                       obj.style.display = "block";
                    }
                     
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
             });
}
function lianDong(time) 
{
   var selectIndex;
   var date=time.format("Y-m-d H:00");
   var fieldControl=Ext.getCmp(entityNameLink); 
   var selectionsArray = fieldControl.getSelectedIndexes();
   var store = fieldControl.getNodes();
   for(var i=0;i<store.length;i++)
   {
       if(store[i].innerText==date)
       {
           selectIndex=i;
           break;
       }
   
   }
   fieldControl.select(selectIndex);
}
function closeQuxian()
{
   if(Ext.getDom("quxianWai")!=null)
       Ext.getDom("quxianWai").style.display = "none";
}
function bpxClick()
{
 var obj=document.getElementsByName("bpx");
 var obj1=document.getElementsByName("xn")
 var obj2=document.getElementsByName("xb")
 var str="a";
 checkStateOp(obj,obj1,obj2,str);
}
function xnClick()
{
 var obj=document.getElementsByName("xn");
 var obj1=document.getElementsByName("bpx")
 var obj2=document.getElementsByName("xb")
 var str="c";
 checkStateOp(obj,obj1,obj2,str);
}
function xbClick()
{
 var obj=document.getElementsByName("xb");
 var obj1=document.getElementsByName("bpx")
 var obj2=document.getElementsByName("xn")
 var str="b";
 checkStateOp(obj,obj1,obj2,str);
}
function checkStateOp(obj,obj1,obj2,str)
{
  var Check=document.getElementsByName("CheckStation");
  if(obj!=null)
    {
           if(obj[0].checked)
           {
                obj1[0].checked=false;
                obj2[0].checked=false;
                if(Check!=null)
                {
                     for(var i=0;i<Check.length;i++)
                     {
                      if(Check[i].id.indexOf(str)>=0)
                          Check[i].checked="checked";
                      else 
                          Check[i].checked=false;
                     }
                
                }
           }
           else 
           {
               if(Check!=null)
                {
                     for(var i=0;i<Check.length;i++)
                     {
                      if(Check[i].id.indexOf(str)>=0)
                          Check[i].checked=false;
                     }
                    
                }
           }
      }
}
function tableImageLianD(el,entity){
if(Ext.getDom("quxianWai")!=null)
   Ext.getDom("quxianWai").style.display = "none";
   var index=el.lastIndexOf("/");
   var nameProp=el.substr(index+1);
   var lastIndex=nameProp.lastIndexOf("_");
   var subString;
   var dateTime;
   var flag;
   if(entity=="pm2.5_class")
   {
        subString=nameProp.substr(lastIndex+1);
        var month=parseInt(subString.substr(4,2),10);
        var day=parseInt(subString.substr(6,2),10);
        dateTime=subString.substr(0,4)+"/"+month.toString()+"/"+day+" 0:00:00"
        flag="date";
   }
    else if(entity=="DustPM10")
   {
        subString=nameProp.substr(lastIndex+1);
        index=subString.lastIndexOf(".");
        subString=subString.substr(0,index);
        dateTime=subString.substr(0,4)+"/"+subString.substr(4,2)+"/"+subString.substr(6,2)+" "+parseInt(subString.substr(8,2),10)+":00:00"
        flag="PM10";
   } 
    else if(entity=="PM10")
   {
        subString=nameProp.substr(0,lastIndex);
        var secIndex=subString.lastIndexOf("_");
        subString=subString.substr(secIndex+1);
        dateTime=subString.substr(0,4)+"-"+subString.substr(4,2)+"-"+subString.substr(6,2)+" "+subString.substr(8,2)+":00:00"
        flag="PM10Air";
   } 
   else 
   {
        subString=nameProp.substr(0,lastIndex);
        var secIndex=subString.lastIndexOf("_");
        subString=subString.substr(secIndex+1);
        dateTime=subString.substr(0,4)+"-"+subString.substr(4,2)+"-"+subString.substr(6,2)+" "+subString.substr(8,2)+":00:00"
        flag="hour";
   } 
    if(PxFunction=="")
        PxFunction="normalTable";
     Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog',PxFunction),
            params: { dateTime:dateTime,flag:flag},
            success: function(response){
                if(response.responseText!=""){
                       var obj=Ext.getDom("tableDiv");
                       obj.innerHTML=response.responseText;
                       obj.style.display = "block";
                }
                 
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });

}
function commentContent()
{
 Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','commentCotent'),
            success: function(response){
                if(response.responseText!=""){
                    var obj= document.getElementById('CommentDiv'); 
                    obj.innerHTML=response.responseText;
                    obj.style.display = "block";
                    var temp= document.getElementById('OutDiv');
                    if(temp!=null)
                       temp.scrollTop=temp.scrollHeight;
                    var el= document.getElementById('WeekDiv');
                    if(el.style.display=="block") 
                        el.style.display="none";
                }
                else 
                  Ext.getDom("CommentDiv").innerHTML="";
                
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function addCart(time,name,content)
{
  Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','AddcommentCart'),
            params: { dateTime:time,name:name,content:content},
            success: function(response){
                if(response.responseText!=""){
                if(response.responseText=="成功")
                    Ext.Msg.alert("信息", "加入会商资料库成功！");
                else 
                    Ext.Msg.alert("信息", response.responseText); 
                }
                else  
                    Ext.Msg.alert("信息", "加入会商资料库失败！");               
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function addCollect(time,name)
{
 Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','AddcommentCollect'),
            params: { dateTime:time,name:name},
            success: function(response){
                if(response.responseText!=""){
                    if(response.responseText=="成功")
                    {
                      // labelValue();
                        Ext.Msg.alert("信息", "加入收藏夹成功！");
                    }
                    else 
                        Ext.Msg.alert("信息", response.responseText); 
                }
                else  
                    Ext.Msg.alert("信息", "加入收藏夹失败！");               
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function labelValue()
{
     Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','commentLabelValue'),
            success: function(response){
                if(response.responseText!=""){
                  var commentCount = Ext.getDom("comment");
                  if(response.responseText=="0")
                      commentCount.innerHTML="关注";
                  else 
                      commentCount.innerHTML="关注&nbsp;"+response.responseText;
                 
                }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function getContent(){
    var postJson = "";
    var aryDiv=WeekDataTable.getElementsByTagName("textarea");
    for(var i=0;i<aryDiv.length;i++){
            var lastValue = aryDiv[i].value.trim();
            postJson = postJson + aryDiv[i].id + ":" + lastValue + ",";

        }
   aryDiv=WeekDataTable.getElementsByTagName("input");
   for(var i=0;i<aryDiv.length;i++){
            var lastValue = aryDiv[i].value.trim();
                if(i!=aryDiv.length-1)
                   postJson = postJson + aryDiv[i].id + ":" + lastValue + ",";
                else 
                   postJson = postJson + aryDiv[i].id + ":" + lastValue;

        }
    return postJson;

}