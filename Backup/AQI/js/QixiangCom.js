// 用于处理AQI
var add;


var hasEdit = false;//标识当前是否处于需要保持的状态
var inPutText=new Array(7);
var buttonValue=new Array(4);
var checkBox="";
Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        initInputHighlightScript();
        checkBoxString();
        changeDate(Ext.getDom("H00"));
        
        wait();
    }
)
function checkBoxString()
{
 var table='<table id="cblSiteList" border="0">';       
 var users =  peopleJson ;
    var userArray = users .split(','); 
    var temp="";   
    var strTemp="";

    for( var i =0;i< userArray .length;i++){
        var userTemp    =  userArray[i].split(':');
        if(i==0||i%4==0)
        {
        strTemp=strTemp+'<tr>';
        }
        strTemp  = strTemp+ '<td style="width: 250px;padding-top: 10px"><input type="checkbox" name="CheckType" checked ="checked" value ='+userTemp[0]+ ' >'+userTemp[1]+'</td>'; 
        if((i+1)%4==0)
        {
        strTemp=strTemp+'</tr>';
        }
    } 
    checkBox=table+strTemp+"</table>";
    //初始化短信用户
    receivePeople("none");
        
}
function receivePeople(displayValue)
{ 
    var obj =Ext.getDom("re");
    var temp =Ext.getDom("chkSiteContainer");
    var divUsers=Ext.getDom("panGroupContainer"); 
   divUsers.style.left = getElementLeft(obj,divUsers.parent) + "px";
   divUsers.style.top =  getElementTop(obj,divUsers.parent) + obj.offsetHeight-320 + "px";
   
    temp.innerHTML =checkBox;   

    divUsers.style.display= displayValue;
    divUsers.style .zIndex =100;
}

function allSelecet()
{
   var obj=document.getElementsByName("CheckType");
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
 var obj=document.getElementsByName("CheckType");
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
   var temp =Ext.getDom("chkSiteContainer");
   checkBox=temp.innerHTML;
   divUsers.style.display="none";
}
function closeCheck()
{
   var divUsers=Ext.getDom("panGroupContainer");
   divUsers.style.display="none";
}
function textAreaChange(sender)
{
 var index
 var id=sender.id;
 if(id.substr(1,1)=="0")
 {
  index=id.substr(2,1);
 }
 else 
 {
  index=id.substr(1,2);
 }
 var vale=inPutText[index-5];
 var textAreaValue= Ext.getDom(sender).value;
 if(textAreaValue!=vale)
 {
  hasEdit=true;
 }
}
//获取鼠标按下时的值
function pick(senderValue,obj)
{
 var id=senderValue.id;
 var index=id.substr(2,1);
 var vale=buttonValue[index-1];
 if(vale!=obj.innerText)
 {
  hasEdit=true;
 }
  Ext.getDom(senderValue).value=obj.innerText;
  
  var obj=Ext.getDom("filter");
  obj.style.display="none";

}
//让光标停留在第一个输入位置
function SetFoucs() {
    var part = lastTab.split("_");
    var address = String.format("H312{0}", part[1]);
    var sender = Ext.getDom(address);
    alterationTextValue(sender);
}
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
                    
                }else
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

//文本框信息改变后，改变相应的buddy的显示信息
function hiddenTextChanged(dateText){
    
    var el = Ext.getDom(dateText.id.replace('Buddy',''));
    var parts = dateText.value.split('-');
    el.innerText = parts[1] + "月" + parts[2] + "日";

    var r = el.id[2]; //输出相应文本框信息改变的行号
    var str = dateText.value + ";" + r;
    var period = "24";
       if (rd1.className == "radioUnChecked")
       period = "48";
    var Module="";
    if(userLimit=="2")
      Module="SMC";
    else 
      Module="Manual";
    Ext.Ajax.request({
    url: getUrl('MMShareBLL.DAL.ComForecast', 'GetForecastByPeriod'),
        params: { hour: str, period: period,Module:Module},
        success: function(response) {
             clearElementByRow(r);
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
//文本内容改变，按钮改变
function textChange(dateText)
{
     var parts = dateText.value
     var obj = Ext.getDom("sendSM");
     if(parts == "")
     {
       obj.className="save graybutton";
       obj.disabled=true;
     }
     else 
     {
      obj.className="save defaultButton";
      obj.disabled=false;

     }
}

//enter键切换输入框
document.onkeydown = function(event) {
    var obj= document.activeElement;
    if(document.activeElement.nodeName=="INPUT")
    {
        e = event ? event : (window.event ? window.event : null);
        if (e.keyCode == 13) {
            var par = add.substr(0, 4);
            var t = add.substr(4, 1);
            var start=add.substr(1, 1);
            var end=add.substr(3, 1);
            var dizhi;
            if(userLimit=="2")
            {
              if(par=="H513")
              {
                 add=String.format("H316{0}",t);
                 dizhi = Ext.getDom(add);              
              }
              else 
              {
              if(end==6)
              {
                add=String.format("H{0}12{1}",parseInt(start)+1,t);
                dizhi = Ext.getDom(add);
              }
              else if(end==3)
              {
                 add=String.format("H{0}1{1}{2}",start,6,t);
                  dizhi = Ext.getDom(add);
               }
               else 
               {
                add=String.format("H{0}1{1}{2}",start,parseInt(end)+1,t);
                dizhi = Ext.getDom(add);
               }
               
              }
            }
            else 
            {
                if(par=="H513")
                {
                 add=String.format("H314{0}",t);
                 dizhi = Ext.getDom(add);
                }
                else 
                {
                    if(end==6)
                    {
                     add=String.format("H{0}11{1}",parseInt(start)+1,t);
                     dizhi = Ext.getDom(add);
                    }
                    else if(end==4)
                    {
                      add=String.format("H{0}1{1}{2}",start,6,t);
                      dizhi = Ext.getDom(add);
                    }
                    else {
                      add=String.format("H{0}1{1}{2}",start,parseInt(end)+1,t);
                      dizhi = Ext.getDom(add);
                    }
                 }
             }
            if (add != "") {
                alterationTextValue(dizhi);
               
            }

        }
    }
}
//当点击输入的div后，显示输入的文本框
function showInput(evt,sender){

    var eventSource = getEventSource(evt);
    if(eventSource.tagName == "INPUT")
        return;
    alterationTextValue(sender);
}
  
function alterationTextValue(sender){

    //对于firefox只能用innerHTML
    var lastValue = sender.innerHTML.trim();
    var splitIndex = lastValue.indexOf('/');
    if(splitIndex>0)
        lastValue = lastValue.substr(0,splitIndex);
    sender.setAttribute("tag",lastValue);
    sender.setAttribute("AQI",sender.innerHTML);
    sender.innerHTML = "";
    sender.style.border = "none";
    var txtInput = new Ext.form.NumberField({
        renderTo:sender.id,
        width:sender.offsetWidth,
        value:lastValue,
        maxValue:2000,
        listeners: {
            blur:function(){
                var parentNode = this.container.dom;
                var itemID = parentNode.id.substr(parentNode.id.length-1,1);
                //标识是否经过编辑
                var divTag = parentNode.getAttribute("tag");
                if (this.getValue() != divTag) {
                    hasEdit = true;
                    //根据当前输入的浓度值和污染物ID，返回带颜色标识的浓度和AQI组合
                    if (this.getValue() === "")
                    {
                         parentNode.innerHTML ="";
                         buildPreview(parentNode.id);
                         }
                    else
                    { 
                        var value = this.getValue().toFixed(1);
                        Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.ComForecast', 'ConvertToAQI'),
                        params: { value: value, itemID: itemID },
                        success: function(response) {
                            parentNode.innerHTML = response.responseText;
                            buildPreview(parentNode.id);
                        },
                        failure: function(response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                    
                    }
                } else
                {
                    parentNode.innerHTML = parentNode.getAttribute("AQI");
                    buildPreview(parentNode.id);
                }
                var kuang = document.getElementById(sender.id);
                kuang.style.border = "1px solid #C0C0C0";
            }
        }
    });
    txtInput.focus();
    txtInput.container.dom.firstChild.select();
    add = sender.id; 
}

//////创建汇总数据
function BuildCollect(){
    var content = getContent();
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.ComForecast','BuildCollect'),
            params: { forecastDate:Ext.getDom("H00").value, postJson:content}, 
            success: function(response){
                var result = Ext.util.JSON.decode(response.responseText);
                for(var obj in result){
                    divContaner = Ext.getDom(obj);
                    if(divContaner != null){
                            divContaner.innerHTML = result[obj];//日平均值
                    }
               }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         }); 
    
}
//当改变单元格预报内容后，即创建预报预览，并根据当前的ID判断是否需要计算日平均，并计算日平均
function buildPreview(curID){
    var rowIndex = curID.substr(1,1);
    var itemID = "";
    var divID = "";
    var par = curID.substr(0, 4);
    var t = curID.substr(4, 1);
    var caculateValue="";
    var durationIndex=curID.substr(3,1);
     var valueCurr="";
     var iD;
    var Module="";   
    if(userLimit=="2")
       Module="SMC";
    else 
       Module="Manual";
        if(rowIndex>2)
        {    
            if(rowIndex > 3)
            {
                divID = String.format("{0}{1}{2}",curID.substr(0,3),7,curID.substr(4,1));//日平均所显示的DIVID
                itemID = curID.substr(4,1);//当前污染物ID
            }

            if(rowIndex==4&&durationIndex==4&&curID.substr(4,1)!=4&&curID.substr(4,1)!=5)
            {
                for(var  i=1;i<=3;i++)
                {
                      var tempText=String.format("{0}{1}{2}{3}{4}",curID.substr(0,1),5,1,i,curID.substr(4,1));
                      if(Ext.getDom(tempText).innerText==""||Ext.getDom(tempText).innerText=="/")
                      {              
                         Ext.getDom(tempText).innerHTML=Ext.getDom(curID).innerHTML;
                      }
                
                }
            }
             var lastValue=Ext.getDom(curID).innerHTML;
             var splitIndex = lastValue.indexOf('/');
             if(splitIndex>0)
                   valueCurr = lastValue.substr(0,splitIndex);
             if(durationIndex==4&&valueCurr!=""&&rowIndex!=5)
             {
                  var night=String.format("H{0}1{1}{2}",rowIndex,6,t);
                  var even=Ext.getDom(night).innerHTML;
                  var moning=String.format("H{0}1{1}{2}",parseInt(rowIndex)+1,1,t);
                  var mo=Ext.getDom(moning).innerHTML;
                  if(mo.indexOf('/')>0)
                  {
                    caculateValue=((valueCurr*4+mo.split('/')[0]*6)/10).toFixed(1);
                    iD=night;
                  }
                  else
                  { 
                      if(even.indexOf('/')>0)
                      {
                         caculateValue=((even.split('/')[0]*10-valueCurr*4)/6).toFixed(1);
                         iD=moning;
                      }
                  }     
             }
             else if(durationIndex==6&&valueCurr!=""&&valueCurr!="/")
             {
                  var night=String.format("H{0}1{1}{2}",rowIndex,4,t);
                  var even=Ext.getDom(night).innerHTML;
                  var moning=String.format("H{0}1{1}{2}",parseInt(rowIndex)+1,1,t);
                  var mo=Ext.getDom(moning).innerHTML;
                  if(even.indexOf('/')>0)
                  {
                    caculateValue=((valueCurr*10-even.split('/')[0]*4)/6).toFixed(1);
                    iD=moning;
                  }
                  else
                  { 
                      if(mo.indexOf('/')>0)
                      {
                         caculateValue=((valueCurr*10-mo.split('/')[0]*6)/4).toFixed(1);
                         iD=night;
                      }
                  }     
               
             }
             else if(durationIndex==1&&valueCurr!=""&&valueCurr!="/")
             {
                  var night=String.format("H{0}1{1}{2}",parseInt(rowIndex)-1,4,t);
                  var even=Ext.getDom(night).innerHTML;
                  var moning=String.format("H{0}1{1}{2}",parseInt(rowIndex)-1,6,t);
                  var mo=Ext.getDom(moning).innerHTML;
                  if(even.indexOf('/')>0)
                  {
                    caculateValue=((even.split('/')[0]*4+valueCurr*6)/10).toFixed(1);
                    iD=moning;
                  }
                  else
                  { 
                      if(mo.indexOf('/')>0)
                      {
                         caculateValue=((mo.split('/')[0]*10-valueCurr*6)/4).toFixed(1);
                         iD=night;                               
                     }
                  }                 
             }
             if(caculateValue!="")
             {
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.ComForecast', 'ConvertToAQI'),
                    params: { value: caculateValue, itemID: curID.substr(4, 1)},
                    success: function(response) {
                        Ext.getDom(iD).innerHTML = response.responseText;
                             var content = getContent();
                            Ext.Ajax.request({ 
                                    url: getUrl('MMShareBLL.DAL.ComForecast','BuildPreview'),
                                    params: { forecastDate:Ext.getDom("H00").value, postJson:content,divID:divID,itemID:itemID,detail:Ext.getDom("H11").value,Module:Module}, 
                                    success: function(response){
                                    if(response.responseText!=""){
                                        var result = Ext.util.JSON.decode(response.responseText);
                                        changeDateSucessed(result); 
                                        }  
                                        else 
                                        {
                                          clearPreview(divID);
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
            else 
            {
                 var content = getContent();
                Ext.Ajax.request({ 
                        url: getUrl('MMShareBLL.DAL.ComForecast','BuildPreview'),
                        params: { forecastDate:Ext.getDom("H00").value, postJson:content,divID:divID,itemID:itemID,detail:Ext.getDom("H11").value,Module:Module}, 
                        success: function(response){
                        if(response.responseText!=""){
                            var result = Ext.util.JSON.decode(response.responseText);
                            changeDateSucessed(result); 
                            } 
                        else 
                          {
                            clearPreview(divID);
                          }                
                        }, 
                        failure: function(response) { 
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                        }
                     });  
           } 
      }      
    
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
        span = Ext.getDom(String.format("td{0}1",i));
        span.innerText = startDate.format("m月d日");
        input = Ext.getDom(String.format("td{0}1Buddy", i));
        input.value = startDate.format("Y-m-d");
        startDate = startDate.add('d',1);
    }
    startDate = startDate.add('d',-1);
    for(var i=3;i<=backDays + 3;i++){
       span = Ext.getDom(String.format("td{0}1",i));
       span.innerText = startDate.format("m月d日");
       startDate = startDate.add('d',1);
    }
    
    
    //根据当前选择的时间获取预报单信息
    var period = "24";
    if (rd1.className == "radioUnChecked")
       period = "48";
    var Module="";
    if(userLimit=="2")
      Module="SMC";
    else 
      Module="Manual";
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.ComForecast','GetForecast'),
       params: {flag:"0", hour: forecastDate, period: period,Module:Module},
            success: function(response){
                clearElement();//先清空所有数据

                if(response.responseText!=""){
                    var result = Ext.util.JSON.decode(response.responseText);
                    changeDateSucessed(result);
                    gettextValue();
                    var nowDateTime=Ext.getDom("nowDateTime");
                    var nowTime=nowDateTime.value;//当前的时间
                    var nowDate=convertDate(nowTime);
                    var catualDate=nowDate.format("Y年m月d日");//当前的日期
                    var limit = Ext.getDom("H00").getAttribute("todayDateTime");//限制的时间
                    var obj = Ext.getDom("btnSave");
                    if(forecastDate==catualDate&&limit>=nowTime)
                    {
                       obj.className="save defaultButton";
                       obj.disabled=false;
                    }
                    else 
                    {
                      obj.className="save graybutton";
                       obj.disabled=true;
                    }
                    
                    var toDayDisable = Ext.getDom("tomoDay");
                    if(catualDate>forecastDate)
                    {
                       toDayDisable.disabled=false;
                       toDayDisable.innerHTML="<a href='javascript:changeDateSelect(1);'>下一天</a>";     
                    }
                   else  
                     {
                      toDayDisable.disabled=true; 
                      toDayDisable.innerHTML="下一天";
                     }   
                    buildPreview("H3141");

                }
           SetFoucs(); 
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         }); 
            

}
function gettextValue()
{
    for(var i=0;i<inPutText.length;i++)
    {
         var idPut;
         if(i<5)
         {
          idPut=String.format("H0{0}",i+5);
         }
         else 
         {
          idPut=String.format("H{0}",i+5);
         }
         idPut=Ext.getDom(idPut).value;
         inPutText[i]=idPut
    }
  for(var i=0;i<buttonValue.length;i++)
   {
    var  idPut=String.format("H0{0}",i+1);
    idPut=Ext.getDom(idPut).value;
    buttonValue[i]=idPut;
   }
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

//当没有数据的时候按行清空
function clearElementByRow(row) {
    var aryDiv = forecastTable.getElementsByTagName("div");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 2) == String.format("H{0}", row)) {
            aryDiv[i].innerHTML = "";
        }
    }
}
function clearPreview(id)
{
    Ext.getDom("H09").value="";
    Ext.getDom("H10").value="";
    Ext.getDom("PH10").value="";
    Ext.getDom(id).innerHTML="";
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

//保存当前页面的内容
function doSubmit(tipInfo){
    var postJson = "";
    
    //获取表单信息
    var aryDiv=document.getElementsByTagName("INPUT");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].id.substr(0,1) == "H"){
            postJson = postJson + aryDiv[i].id + ":" + aryDiv[i].value + ",";
        }
    }
    
    aryDiv=document.getElementsByTagName("textarea");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].id.substr(0,1) == "H"){
            postJson = postJson + aryDiv[i].id + ":" + aryDiv[i].value + ",";
        }
    }   
    postJson = postJson.substr(0,postJson.length-1) + ";";//把表单信息和预报内容分隔开
    
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
    var Module="";
    if(userLimit=="2")
      Module="SMC";
    else 
      Module="Manual";
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ComForecast','SaveEdits'),
        params: { postJson: postJson,Module:Module}, 
        success: function(response){
            if(response.responseText!=""){
                var result = Ext.util.JSON.decode(response.responseText);
                if(result.success == true){
                    hasEdit = false;
                    if(tipInfo)
                        Ext.Msg.alert("信息", "保存成功！"); 
                }
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     }); 
    
}


     
     
 //提交界面，创建汇总表
 function goBack(){
    hideEl("preview");
    showEl("comForecast");    
 }
 
 function createSummary(){
 
    var zhuBan=Ext.getDom("H02");
    var fuBan=Ext.getDom("H01");
    var qixiangzhuBan=Ext.getDom("H04");
    var qixiangfuBan=Ext.getDom("H04");
    if(userLimit=="1")
    {
      if(zhuBan.value==""||fuBan.value=="")
          Ext.Msg.alert('提示',"请填写主班、副班");
    }
    else 
    {
      if(qixiangzhuBan.value==""||qixiangfuBan.value=="")
          Ext.Msg.alert('提示',"请填写气象主班、气象副班");
    }
    
    hideEl("comForecast");
    showEl("preview");
    var obj = Ext.getDom("btnSave");
    if(hasEdit && obj.className == "save defaultButton")
    {
     doSubmit();
    }
     Ext.getDom("publishTime")
    var todayDate = Ext.getDom("H00").getAttribute("todayDateTime");
    var nowDate=convertDate(todayDate);
    var month=nowDate.getMonth()+1;
    var day=nowDate.getDate();
    var publishDate=month+"月"+day+"日"+"17时发布";
    Ext.getDom("publishTime").value=publishDate;
    buildPreview("H3141");
    var BackDays = parseInt(Ext.getDom("H00").getAttribute("tag"));
    BuildCollect();
    var el=Ext.getDom("H00");
    var forecastDate = el.value;
    titleDate.innerHTML = forecastDate;
    var startDate=convertDate(forecastDate);
    var date=startDate.format("m月d日");
    var nextDate = startDate.add('d',2);
    var threeDate=nextDate.format("m月d日");
    var tomorrowDay=startDate.add('d',1);
    var tomoDate=tomorrowDay.format("m月d日");

    var ePreview = Ext.getDom("Ptd11");
    ePreview.innerText = date;
    for(var i=2;i<=3;i++)
    {
      ePreview=Ext.getDom(String.format("Ptd{0}1",i));
      ePreview.innerText = tomoDate;
    }
    ePreview=Ext.getDom("Ptd41");
    ePreview.innerText = threeDate;
    ePreview=Ext.getDom("Ptd51");
    ePreview.innerText = tomoDate;
    ePreview=Ext.getDom("Ptd61");
    ePreview.innerText = threeDate;


    var el;
    //获取表单信息
    var aryDiv=document.getElementsByTagName("INPUT");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].id.substr(0,1) == "H"){
            el = Ext.getDom("P" + aryDiv[i].id);
            if(el != null)
                el.innerHTML = aryDiv[i].value;
        }
    }
    
    //短信
    
    var hValue = Ext.getDom("H11").value;
    hValue = hValue.substr(hValue.length-1,1);
    if(hValue != "." && hValue != "。" && hValue !="")
        Ext.getDom("H11").value = Ext.getDom("H11").value + "。";
    else if(hValue == ".") 
        Ext.getDom("H11").value = Ext.getDom("H11").value.substring(0,Ext.getDom("H11").value.length-1) + "。";
    
    el = Ext.getDom("PH10");
    hValue = el.value;
    hValue = hValue.replace('臭氧1h','O3').replace('臭氧8h','O3');
    el = Ext.getDom("PH09");
    
    el.value = "【上海市空气质量预报】" + Ext.getDom("H11").value + hValue + "上海市环境监测中心 上海中心气象台";
    textChange(el);
    
//    aryDiv=document.getElementsByTagName("textarea");
//    for(var i=0;i<aryDiv.length;i++){
//        if (aryDiv[i].id.substr(0,1) == "H"){
//            el = Ext.getDom("P" + aryDiv[i].id);
//            if(el != null)
//            {
//                el.innerHTML = aryDiv[i].value;
//                textChange(el);
//            }
//        }
//    }   
    
    var aryDiv=forecastTable.getElementsByTagName("div");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].className.indexOf('divInputType') >=0){
            el = Ext.getDom("P" + aryDiv[i].id);
            if(el != null)
                el.innerHTML =  aryDiv[i].innerHTML;

        }
    }
    
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
function sendSM(){
    var el = Ext.getDom("PH09");
    var obj=Ext.getDom("H00");
    var todayDate = Ext.getDom("H00").getAttribute("todayDateTime");
    var nowDate=convertDate(todayDate);
    var catualDate=nowDate.format("Y年m月d日");
    var forecastDate = obj.value;
    var select=getCheckBValue("Type");
    var temp=getCheckBValue("CheckType");
    var publishTime= Ext.getDom("publishTime").value;
    var pubString=el.innerHTML;
    var tipInfo = "确定要发布数据吗？";
    if(pubString.indexOf("夜间")>0&&pubString.indexOf("上午")>0&&pubString.indexOf("下午")>0)
    {
        if(el.innerHTML != ""){
            if(catualDate!=forecastDate)
                tipInfo = "您发送的不是今天的数据，你还确定要发布数据吗？";
            Ext.Msg.confirm('提示',tipInfo,
                function(btn){
                    if(btn=='yes'){
                         var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"预报数据正在发布中..."});
                         myMask.show();
                         Ext.Ajax.request({ 
                            url: getUrl('MMShareBLL.DAL.ComForecast','PublicData'),
                            timeout:12000,
                            params: {checkBoxSelect:select, content: el.innerHTML,forecastDate:forecastDate,temp:temp,publishTime:publishTime}, 
                            success: function(response){
                                if(response.responseText!=""){
                                        hasEdit = false;
                                        myMask.hide();
                                        Ext.Msg.alert("信息", response.responseText.substr(1,response.responseText.length-2)+"！"); 
                                }
                            }, 
                            failure: function(response) { 
                                myMask.hide();
                                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                            }
                         });  
                    }
                },this);
        }
        else 
            alert("数据不完全，不能发布数据");
    }         
}
