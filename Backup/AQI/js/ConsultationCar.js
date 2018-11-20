Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        queryDayComment();
    }
)
function  queryDayComment()
{
     var el=Ext.getDom("H00");
     var DateTime=el.value;
      Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','QueryDayCollect'),
            params: { dateTime: DateTime}, 
            success: function(response){
                if(response.responseText!=""){
                     Ext.getDom("content").innerHTML=response.responseText;     
                }
                else 
                    Ext.getDom("content").innerHTML="";
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function changeDate(el){
     var forecastDate = el.value;
    titleDate.innerHTML = forecastDate;
    queryDayComment();
    }
    
 window.onscroll=function(){
     var obj=document.getElementById("top_div");
     getScrollTop()>0?obj.style.display="":obj.style.display="none";
 }
 function getScrollTop(){
    return document.documentElement.scrollTop || document.body.scrollTop;
  }
  function goToTop()
  {
   var goTop=setInterval(scrollMove,10);
     function scrollMove(){
      setScrollTop(getScrollTop()/1.2);
      if(getScrollTop()<1)clearInterval(goTop);
    }
  }
  function setScrollTop(value){
    document.documentElement.scrollTop=value;
    document.body.scrollTop = value;
  }
  function mouseOver()
  {
   var obj=document.getElementById("img");
   obj.className="imgIcomOver";
  }
  function mouseOut()
  {
   var obj=document.getElementById("img");
   obj.className="imgIcom";
  }
function exportSiteData()
{
    var fromDate = Ext.getDom("H00").value;
    var content=fromDate;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("ButtonExport").click();
}
