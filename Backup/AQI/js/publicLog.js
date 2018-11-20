// JScript 文件
Ext.onReady(function(){
    supportInnerText();//使得火狐支持innerText
    publicLogQuery();
   }
)
function selectStyleChange(value)
{
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var selectSource= Ext.getDom("selectSource").value;
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.PublicLog','PublicLogQuery'),
        params: { fromDate: fromDate,toDate:toDate,publicStyle:value,selectSource:selectSource}, 
        success: function(response){
            if(response.responseText!=""){
                
                Ext.getDom("content").innerHTML = response.responseText;
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
 
}
function selectResourceChange(value)
{
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var publicStyle= Ext.getDom("select").value;
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.PublicLog','PublicLogQuery'),
        params: { fromDate: fromDate,toDate:toDate,publicStyle:publicStyle,selectSource:value}, 
        success: function(response){
            if(response.responseText!=""){
                
                Ext.getDom("content").innerHTML = response.responseText;
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
 
}
function publicLogQuery()
{
    var selectValue= Ext.getDom("select").value;
    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var selectSource= Ext.getDom("selectSource").value;
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.PublicLog','PublicLogQuery'),
        params: { fromDate: fromDate,toDate:toDate,publicStyle:selectValue,selectSource:selectSource}, 
        success: function(response){
            if(response.responseText!=""){
                
                Ext.getDom("content").innerHTML = response.responseText;
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function mouseCoords(ev)
{
 if(ev.pageX || ev.pageY){//event.screenY
   return {x:ev.pageX, y:ev.pageY};
 }
 else 
     return {
          x:ev.clientX + document.documentElement.scrollLeft || document.body.scrollLeft,
          y:ev.clientY + document.documentElement.scrollTop || document.body.scrollTop
     }

}
function mouseOver(obj,value,ev)
{
    
    ev=ev || event;
    var height;
    var width;
    var topHeight;
    var Y=ev.clientY;
    var X=ev.clientX;
    height=document.documentElement.clientHeight;
    width=document.documentElement.clientWidth;
    var mousePos = mouseCoords(ev);
    if(obj!=null)
    {   
        var divUsers;
        obj.parentNode.style.backgroundColor = "#badbff";
        var tag;
        var divHeight;
        if(value.indexOf("|")>0)
        {
            tag=0;
            divUsers=Ext.getDom("webViewDiv"); 
            divUsers.style.display= "block";
            divUsers.style .zIndex =99999900;
            divHeight=238;
        }
        else
        {
            tag=1;
            divUsers=Ext.getDom("messageDiv"); 
            divUsers.style.display= "block";
            divUsers.style .zIndex =99999900;
            divHeight=65;
        }
     Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','divEvery'),
            params: {value:value,tag:tag}, 
            success: function(response){
                if(response.responseText!=""){
                     if(height-Y<divHeight&&width-X>400)
                     {
                      divUsers.style.top = mousePos.y-divHeight+ "px";   
                      divUsers.style.left = mousePos.x+8+ "px";
                     }
                     else if(height-Y<divHeight&&width-X<400)
                     {
                      divUsers.style.top = mousePos.y-divHeight+ "px";   
                      divUsers.style.left = mousePos.x-400+8+ "px";
                     }
                     else if(height-Y>divHeight&&width-X<400)
                     {
                      divUsers.style.top = mousePos.y+ "px";   
                      divUsers.style.left = mousePos.x-400+8+ "px";
                     }
                     else 
                     {
                     divUsers.style.left = mousePos.x+8+ "px";
                     divUsers.style.top = mousePos.y+ "px"; 
                     }  
                     divUsers.innerHTML=response.responseText;
                }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
     }

}

function mouseOutS(obj,value,ev)
{
  var tag;
    var divUsers;
    		
    if(value.indexOf("|")>0)
    {
        tag=0;
        divUsers=Ext.getDom("webViewDiv"); 
    }
    else
    {
        tag=1;
        divUsers=Ext.getDom("messageDiv"); 
        
    }
 if(obj!=null)
    {
       obj.parentNode.style.backgroundColor = "#fff";                     
       //divUsers.style.display= "none";
    }
}

function mouseOverS(obj,value,ev)
{
  var tag;
    var divUsers;
    		
    if(value.indexOf("|")>0)
    {
        tag=0;
        divUsers=Ext.getDom("webViewDiv"); 
    }
    else
    {
        tag=1;
        divUsers=Ext.getDom("messageDiv"); 
        
    }
 if(obj!=null)
    {
       obj.parentNode.style.backgroundColor = "#badbff";                     
       //divUsers.style.display= "none";
    }
}

function mouseOut(obj,value,ev)
{
  var tag;
    var divUsers;
    		
    if(value.indexOf("|")>0)
    {
        tag=0;
        divUsers=Ext.getDom("webViewDiv"); 
    }
    else
    {
        tag=1;
        divUsers=Ext.getDom("messageDiv"); 
        
    }
 if(obj!=null)
    {
       obj.parentNode.style.backgroundColor = "#fff";                     
       divUsers.style.display= "none";
    }
}
//function hide(obj,value)
//{
//    var tag;
//    var divUsers;
//    		
//    if(value.indexOf("|")>0)
//    {
//        tag=0;
//        divUsers=Ext.getDom("webViewDiv"); 
//    }
//    else
//    {
//        tag=1;
//        divUsers=Ext.getDom("messageDiv"); 
//        
//    }
// if(obj!=null)
//    {
//       obj.style.backgroundColor = "#fff";                     
//       divUsers.style.display= "none";
//    }
//}
function contentMouseOut()
{
   Ext.getDom("webViewDiv").style.display= "none"; 
   Ext.getDom("messageDiv").style.display= "none";
}
function reSentMes(sendTime,sendStyle)
{
    var sendLogTime=sendTime;
    var sendLogStyle=sendStyle;
    Ext.Msg.confirm('提示',"你确定要重新发布数据？",
    function(btn){
      if(btn=='yes'){
            var myMask = new Ext.LoadMask(document.body, {msg:sendStyle+"数据正在发布中..."});
            myMask.show();
             Ext.Ajax.request({ 
                url: getUrl('MMShareBLL.DAL.PublicLog','reSend'),
                timeout:60000,
                params: { sendLogTime:sendLogTime,sendLogStyle:sendLogStyle}, 
                success: function(response){
                    if(response.responseText!=""){
                         myMask.hide();
                         Ext.Msg.alert("信息", response.responseText+"！");
                         publicLogQuery();
                    }
                }, 
                failure: function(response) { 
                    myMask.hide();
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    publicLogQuery(); 
                }
             });
            }
    },this);
     
}
