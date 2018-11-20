// 用于处理AQI


var hasEdit = false;//标识当前是否处于需要保持的状态

Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        initInputHighlightScript();
        changeDate(Ext.getDom("H00"));
        wait();
    }
)
function changeDate(el){
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
    
     Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.ComForecast','BuildPreconsation'),
       params: { forecastDate: forecastDate},
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
                    var obj = Ext.getDom("sendSM");
                    if(forecastDate==catualDate&&limit>=nowTime)
                    {
//                       obj.className="save defaultButton";
                       obj.disabled=false;
                    }
                    else 
                    {
//                      obj.className="save graybutton";
                      obj.disabled=true;
                    }

                }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
    //当没有数据的时候清空
function clearElement(){
    var aryDiv=pTable.getElementsByTagName("div");
    for(var i=0;i<aryDiv.length;i++)
    {
      if(aryDiv[i].id.substr(0,1) =="H")
      {
       aryDiv[i].innerHTML="";
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
        if (aryDiv[i].id.substr(0,1) == "H"||aryDiv[i].id.substr(1,1) == "H"){
            aryDiv[i].value = "";
        }
    }   
}
//清空事件readOnly = true
function clear(){
    aryDiv=document.getElementsByTagName("textarea");
    for(var i=0;i<aryDiv.length;i++){
        if (aryDiv[i].readOnly==false)
        {
           aryDiv[i].value = "";
        }
    }  
}
//获取鼠标按下时的值
function pick(senderValue,obj)
{
  Ext.getDom(senderValue).value=obj.innerText;
  
  var obj=Ext.getDom("filter");
  obj.style.display="none";

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
//保存当前页面的内容
function doSubmit(){
    var postJson = "";
    
    var els = ["H00","H03","H04","H05","H06"];//获取气象de输入单元格
    var el;
    for(i=0;i<els.length;i++){
        el = Ext.getDom(els[i]);
        postJson = postJson + els[i] + ":" +  el.value + ",";
    }
    postJson = postJson.substr(0,postJson.length-1);
    
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
                    Ext.Msg.alert("信息", "保存成功！"); 
                }
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     }); 
    
}