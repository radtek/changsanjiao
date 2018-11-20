var id="100000001";
Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        listValue();
        queryDayComment();
    }
)
function  queryDayComment()
{
     var el=Ext.getDom("H00");
     var todayDate=el.value;
     todayDate=todayDate.replace(" ","");
     todayDate=todayDate.replace("年","");
     todayDate=todayDate.replace("月","");
     todayDate=todayDate.replace("日","");
     todayDate=todayDate.replace("时","");
     todayDate="2014033112";
     
     Ext.getDom("qixiangForecast").src="Product/figure/D2/"+todayDate+"/StaMet/3DAYs/"+id+".gif";
     Ext.getDom("polluteForecast").src="Product/figure/D2/"+todayDate+"/StaPol/3DAYs/"+id+".gif";
     Ext.getDom("qrj").src="Product/figure/D2/"+todayDate+"/AeroCom/"+id+".gif";
     Ext.getDom("Pm25").src="Product/figure/D2/"+todayDate+"/PM25prof/"+id+".gif";
}
function listValue()
{
      Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.SiteData','SiteDataList'),
            success: function(response){
                if(response.responseText!=""){
                     Ext.getDom("list").innerHTML=response.responseText;
                    
                }
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function changeDate(el){
    queryDayComment();
    }
function liClick(srcID)
{
  Ext.getDom(id).style.backgroundColor = "#fff";
  id=srcID;
  Ext.getDom(srcID).style.backgroundColor = "#badbff";
  queryDayComment();
}
function mouseOver(obj)
{
    if(obj!=null)
    {
       obj.style.backgroundColor = "#badbff";
    }

}
function mouseOut(obj)
{
 if(obj!=null)
    {  
    if(id!=obj.id)
       obj.style.backgroundColor = "#fff";
    }
}