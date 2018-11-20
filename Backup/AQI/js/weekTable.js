Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        queryDayComment();
    }
)
function  queryDayComment()
{
     var el=Ext.getDom("H00");
     var Time=el.value;
      Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','QueryWeekRevise'),
            params: { dateTime: Time}, 
            success: function(response){
                if(response.responseText!=""){
                     Ext.getDom("content").innerHTML=response.responseText;
                    
                }
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