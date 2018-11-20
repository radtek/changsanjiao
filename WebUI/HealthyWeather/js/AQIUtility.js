  //相应文本框的点击事件
function Select(sender,fldValue) 
{

    var textValue= Ext.getDom(sender).value;
    var textLength=textValue.length;
    var divUsers=Ext.getDom("filter");
    var users =  Ext.util.JSON.decode(userJson);
    var userArray = users[fldValue].split('|');
    
    divUsers.innerHTML = "";

   divUsers.style.left = getElementLeft(sender,divUsers.parent) + "px";
   divUsers.style.top =  getElementTop(sender,divUsers.parent) + sender.offsetHeight + "px";
    
    for(i=0;i<userArray.length;i++){
        var childDiv = document.createElement("DIV");
        if(userArray[i].substr(0,textLength)==textValue||textValue=="")
        {
            childDiv.innerHTML = userArray[i];
            childDiv.className="userLi";
            childDiv.onmousemove = function(){bgcolor(this);};
            childDiv.onmouseout = function(){nocolor(this);};
            childDiv.onmousedown=function(){pick(sender,this);};
            divUsers.appendChild(childDiv);
        }
    }   
    divUsers.style.display="block";    
}
//当鼠标移进时字体背景颜色改变
function bgcolor(obj)
{
   obj.style.background="#436EEE";
   obj.style.color="#000000";
}
//当鼠标移出时字体背景颜色改变
function nocolor(obj)
{
   obj.style.background="";
   obj.style.color="#000000";

}
//触发窗体的click事件
function hide(evt)
{
    var eventSource = getEventSource(evt);
    if(eventSource.id != "H03"&&eventSource.id != "H04"&&eventSource.id != "H01"&&eventSource.id != "H02")
    {
      var obj=Ext.getDom("filter");
      obj.style.display="none";
    }   
}
   //改变日期成功后,，刷新获取的值
function changeDateSucessed(result){
    for(var obj in result){
      divContaner = Ext.getDom(obj);
        if(divContaner != null){
            if(divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA" )
                divContaner.value = result[obj];
            else
            {
                if(obj.length==5&&obj.substr(2,1)==2)
                {
                   if(userLimit=="2")
                     divContaner.innerHTML=="";
                   else 
                     divContaner.innerHTML = result[obj];//日平均值
                }
                else 
                    divContaner.innerHTML = result[obj];//日平均值
            }
      }  
   }
} 
  