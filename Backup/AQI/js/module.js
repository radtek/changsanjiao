Ext.onReady(function(){
    supportInnerText();//使得火狐支持innerText
    queryMap();
})
function changeDate(el)
{
    var todayDate = el.value; 
    todayDate=todayDate.replace("年","");
    todayDate=todayDate.replace("月","");
    todayDate=todayDate.replace("日","");
    //todayDate="20140225";
    var moduleStyle=module;
    var zone=getRadioValue("zone");
    var pollution=polluteStyle;
     Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.SiteData','QueryFileExists'),
            params: { html: moduleStyle,todayDate:todayDate}, 
            success: function(response){
                if(response.responseText!=""){
                    todayDate=response.responseText;
                    var html="Product/"+moduleStyle+"/"+todayDate+"/"+zone+"/srf/"+pollution+".htm";
                    document.getElementById("iframePage").src=html;
                   
                }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function queryMap()
{
    radioClick(module);
   var todayDate = Ext.getDom("H00").value;
   todayDate=todayDate.replace("年","");
   todayDate=todayDate.replace("月","");
   todayDate=todayDate.replace("日","");
   //todayDate="20140225";
    var moduleStyle=module;
    var zone=getRadioValue("zone");
    var pollution=polluteStyle;

    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.SiteData','QueryFileExists'),
        params: { html: moduleStyle,todayDate:todayDate}, 
        success: function(response){
            if(response.responseText!=""){
                todayDate=response.responseText;
                var html="Product/"+moduleStyle+"/"+todayDate+"/"+zone+"/srf/"+pollution+".htm";
                document.getElementById("iframePage").src=html;
               
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
//   document.getElementById("iframePage").src=html;
   
  
}
function getRadioValue(objName)
{
    var value;
    var obj=document.getElementsByName(objName);
    if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
           if(obj[i].checked)
           {
                value=obj[i].value;
           }
      }
    }
    return value;
}
function setRadioDisabled(objStyle,objName)
{
    var obj=document.getElementsByName(objName);
    if(obj!=null)
    {
      if(objStyle=="")
      {
          for(var i=0;i<obj.length-2;i++)
          {
               obj[i].disabled=false;
          }
      }
      else 
      {
         for(var i=0;i<obj.length-2;i++)
          {
              if(objStyle.indexOf(obj[i].value)>=0)
              {
                obj[i].disabled=true;
                if(obj[i].checked)
                {
                 obj[i].checked=false;
                 obj[i+1].checked=true;
                }
               }
                
              else 
                obj[i].disabled=false;
          }
      }
    }
}
function radioClick(obj)
{
  switch(obj){
    case "naqpms2008":
        setRadioDisabled("","zone");
        break;
    case "CMAQ":
        setRadioDisabled("D1","zone");
        break;
    case "CAMX":
        setRadioDisabled("D1,D2","zone");
        break;
    }   
}
function changeClick()
{
  queryMap();
}


