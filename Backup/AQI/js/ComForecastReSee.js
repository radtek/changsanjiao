// 用于处理AQI

var replaceString;
var hasEdit = false;//标识当前是否处于需要保持的状态
var inPutText=new Array(7);
var buttonValue=new Array(4);
var checkBox="";
Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        initInputHighlightScript();
        changeDate(Ext.getDom("H00"));
        
    }
)

function radioClick(id){
    var el = Ext.getDom(id);
    
    if(el.className == "radioUnChecked"){
        el.className = "radioChecked";
        el = getGroupEl(id);
        el.className = "radioUnChecked";       
    }  
}
function changeDateSelect(obj)
{
     var el=Ext.getDom("H00");
     var oldDate=el.value;
     var startDate=convertDate(oldDate);
     var nextDate = startDate.add('d',obj);
     var date= nextDate.format("Y年m月d日");
     el.value=date;
     changeDate(el);
 
}
function today()
{
    var el=Ext.getDom("H00");
    var todayDate = Ext.getDom("H00").getAttribute("todayDateTime");
    var nowDate=convertDate(todayDate);
    var catualDate=nowDate.format("Y年m月d日");
    el.value=catualDate;
    changeDate(el);

}
function getGroupEl(id){
    var groupEl = "";
    switch(id){
    case "rd1":
        groupEl = "rd2";
        changePeriod(24);
        break;
    case "rd2":
        groupEl = "rd1";
        changePeriod(48);
        break;
    case "rd3":
        groupEl = "rd4";
        changeModule("CMAQ");
        break;
    case "rd4":
        groupEl = "rd3";
        changeModule("WRF");
        break;
    }   
    return Ext.getDom(groupEl);
}

//请求不同模式的数据
function changeModule(module){
   var forecastDate= Ext.getDom("H00").value;
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.ComForecast','GetModuleForecast'),
            params: { hour: forecastDate,module:module}, 
            success: function(response){
                if(response.responseText!=""){
                    var result = Ext.util.JSON.decode(response.responseText);
                    changeDateSucessed(result);
                    
                }
                else
                    clearElementByModule();
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
     }
     

//请求不同时效的数据
function changePeriod(period) {

    var backDays = parseInt(Ext.getDom("H00").getAttribute("tag"));
    var timePeriod = "";
    for (i = 0; i <= backDays; i++) {
        var address = String.format("td{0}1Buddy", i);
        timePeriod =timePeriod + Ext.getDom(address).value + ";";
    }

    var Module="";
    if(userLimit=="2")
      Module="SMC";
    else 
      Module="Manual";
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.ComForecast', 'GetForecastByPeriod'),
        params: { hour: timePeriod, period: period,Module:Module},
        success: function(response) {
            clearElementByzonghe();

            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
            } 
        },
        failure: function(response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
     });     
}

//预报污染物tab切换
function tabClick(id){
    var lastEl = Ext.getDom(lastTab);
    var curEl = Ext.getDom(id);
    lastEl.className = "";
    
    //tab标签的名称和代码之间通过“_”分隔
    var lastParts = lastTab.split("_");
    var curParts = id.split("_");

    lastEl.innerHTML = curEl.innerHTML.replace(id,lastTab).replace(curParts[0],lastParts[0]);
    
    curEl.className = "tabHighlight";
    curEl.innerHTML = curParts[0];
    
    //根据切换的污染物，改变显示的数据
    var aryDiv=forecastTable.getElementsByTagName("div");
    for(var i=0;i<aryDiv.length;i++){
        var divID = aryDiv[i].id;
        if (divID.substr(0,1) == "H"){
            if (divID.substr(divID.length-1,1) == lastParts[1])
                hideEl(divID);
            else if(divID.substr(divID.length-1,1) == curParts[1])
                showEl(divID);
        }
    }
    lastTab = id;    
    
    SetFoucs();
}

function createItem(id,value,AQI){
    var item = "<div id='" + id +"'>" + value +"/<span class='green'>" +AQI +"</span></div>"
    return item;
}

//初始化所有界面上的信息
function changeDate(el){
    var forecastDate = el.value;  
    titleDate.innerHTML = forecastDate;
    var backDays = parseInt(el.getAttribute("tag"));
    var span,input;
    var startDate = convertDate(forecastDate);
    startDate = startDate.add('d',-backDays);
    for(var i=0;i<=backDays;i++){
        startDate = startDate.add('d',1);
        span = Ext.getDom(String.format("td{0}1",i));
        span.innerText = startDate.format("m月d日");
        input = Ext.getDom(String.format("td{0}1Buddy", i));

        input.value = startDate.format("Y-m-d");
    }
    startDate = startDate.add('d',-1);
    for(var i=3;i<=backDays + 3;i++){
       span = Ext.getDom(String.format("td{0}1",i));
       span.innerText = startDate.format("m月d日");
       startDate = startDate.add('d',1);
    }
    
    
    //根据当前选择的时间获取预报单信息
    var period = "24";
    var moduleStyle="WRF";
    if (rd1.className == "radioUnChecked")
       period = "48";
    if (rd4.className == "radioUnChecked")
       moduleStyle= "CMAQ";
    var Module="";
//    if(userLimit=="2")
//      Module="SMC";
//    else 
      Module="Manual";
       var tag="zhonghe";
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.ComForecast','GetForecast'),
       params: {flag:"2",hour: forecastDate, period: period,Module:Module,tag:tag,moduleStyle:moduleStyle},
            success: function(response){
                clearElement();//先清空所有数据

                if(response.responseText!=""){
                    var result = Ext.util.JSON.decode(response.responseText);
                    changeDateSucessed(result);
                    var nowDateTime=Ext.getDom("nowDateTime");
                    var nowTime=nowDateTime.value;//当前的时间
                    var nowDate=convertDate(nowTime);
                    var catualDate=nowDate.format("Y年m月d日");//当前的日期
                    var limit = Ext.getDom("H00").getAttribute("todayDateTime");//限制的时间
                    var obj = Ext.getDom("btnSave");

                    
                    var toDayDisable = Ext.getDom("tomoDay");
                    if(catualDate>forecastDate)
                    {
                       toDayDisable.disabled=false; 
                    }
                   else  
                     {
                      toDayDisable.disabled=true; 
                     }   

                }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
        }); 
            

}
function SaveTextArea()
{
     var forecastDate= Ext.getDom("H00").value;
     var wether24=Ext.getDom("H13").value;
     var wether48=Ext.getDom("H14").value;
     var polution24=Ext.getDom("H15").value;
     var polution48=Ext.getDom("H16").value;
     Ext.Ajax.request({ 
    url: getUrl('MMShareBLL.DAL.ComForecast','SaveComForecastReSee'),
    params: { forecastDate:forecastDate,wether24: wether24,wether48:wether48,polution24: polution24,polution48:polution48}, 
    success: function(response){
       Ext.Msg.alert("信息", response.responseText+"!"); 
    }, 
    failure: function(response) { 
        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
    }
 });

}
function clearTextArea()
{
     Ext.getDom("H13").value="";
     Ext.getDom("H14").value="";
     Ext.getDom("H15").value="";
     Ext.getDom("H16").value="";
}
//当没有数据的时候清空
function clearElement(){
    var aryDiv=forecastTable.getElementsByTagName("div");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].id.substr(0,1) == "H"){
            aryDiv[i].innerHTML = "";
        }
    }
    
    aryDiv=document.getElementsByTagName("INPUT");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].id.substr(0,1) == "H" && aryDiv[i].id != "H00"){//当前切换的日期控件的不清空
            aryDiv[i].value = "";
        }
    }
    
    aryDiv=document.getElementsByTagName("textarea");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].id.substr(0,1) == "H"){
            aryDiv[i].value = "";
        }
    }   
}


//当没有数据的时候按综合预报清空
function clearElementByzonghe() {
    var aryDiv = forecastTable.getElementsByTagName("div");
    var backDays = parseInt(Ext.getDom("H00").getAttribute("tag"));
    for (var i = 0; i < aryDiv.length; i++) {
        for (var j = 0; j <= backDays; j++) {
            if (aryDiv[i].id.substr(0, 3) == String.format("H{0}1", j)) {
                aryDiv[i].innerHTML = "";
            }
        }

    }
}
//当没有数据的时候按模式清空
function clearElementByModule() {
    var aryDiv = forecastTable.getElementsByTagName("div");
    var backDays = parseInt(Ext.getDom("H00").getAttribute("tag"));
    for (var i = 0; i < aryDiv.length; i++) {
        for (var j = 3; j <= backDays+3; j++) {
            if (aryDiv[i].id.substr(0, 3) == String.format("H{0}2", j)) {
                aryDiv[i].innerHTML = "";
            }
        }

    }
}


//获取预报内容
function getContent(){
    var postJson = "";
    var aryDiv=forecastTable.getElementsByTagName("div");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].className.indexOf('divInputType') >=0){
            var lastValue = aryDiv[i].innerHTML.trim();
            var splitIndex = lastValue.indexOf('/');
            if(splitIndex>0){
                lastValue = lastValue.substr(0,splitIndex);
                var span = aryDiv[i].getElementsByTagName("SPAN")[0];
                postJson = postJson + aryDiv[i].id + ":" + lastValue + "/"  + span.innerHTML + ",";
            }else
                postJson = postJson + aryDiv[i].id + ":,";

        }
    }
    return postJson;

}


     
     

 