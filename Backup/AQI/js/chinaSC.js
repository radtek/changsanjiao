// JScript 文件
Ext.onReady(function(){
    supportInnerText();//使得火狐支持innerText
    querySiteData();
   }
)

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
       obj.style.backgroundColor = "#fff";
    }
}

//查询站点空气质量数据
function querySiteData(){
    var fromDate = Ext.getDom("H00").value;
    var myMask = new Ext.LoadMask(document.body, {msg:"数据正在查询中..."});
    myMask.show();
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.SiteData','QueryChinaSC'),
        params: { fromDate: fromDate}, 
        success: function(response){
        myMask.hide();
            if(response.responseText!=""){
                
                Ext.getDom("content").innerHTML = response.responseText;
            }
        }, 
        failure: function(response) { 
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });

}
function exportSiteData()
{
    var fromDate = Ext.getDom("H00").value;
    var content=fromDate;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("ButtonExport").click();
}
