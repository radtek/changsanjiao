Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        queryDayComment();
    }
)
function  queryDayComment()
{
     var el=Ext.getDom("H00");
     var DateTime=el.value;
     var user=userName;
     var DateTime=el.value;
      Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','QueryDayComment'),
            params: { dateTime: DateTime,user:user}, 
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
function  ExpandCommentContent(hiddenValue,value,id)
{
     Ext.getDom(id).innerHTML=value+"<a href=\"javascript:HiddenCommentContent('"+hiddenValue+"','"+value+"','"+id+"')\"><img src='css/images/sq_n.png' width='15px' height='15px'/></a>";
     
}
function HiddenCommentContent(hiddenValue,value,id)
{
     Ext.getDom(id).innerHTML=hiddenValue+"<a href=\"javascript:ExpandCommentContent('"+hiddenValue+"','"+value+"','"+id+"')\"><img src='css/images/gd_d.png' width='15px' height='15px'/></a>";
}