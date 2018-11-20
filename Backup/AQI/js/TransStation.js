Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        queryData();
    }
)
function  queryData()
{
    var fromDate=Ext.getDom("H00").value;
     Ext.getDom("titleDatetime").innerHTML="日期："+fromDate;
    var myMask = new Ext.LoadMask(document.body, {msg:"数据正在查询中..."});
    myMask.show();
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.DayForecast','TransStaionData'),
       params: { fromDate: fromDate},
            success: function(response){
                        myMask.hide();
                        clearElement();
                        if(response.responseText!=""){
                            var result = Ext.util.JSON.decode(response.responseText);
                            changeDateSucessed(result);
                            }
            }, 
            failure: function(response) { 
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         }); 
 
}
function CreateDayProduct()
{
    var fromDate=Ext.getDom("H00").value;
    var myMask = new Ext.LoadMask(document.body, {msg:"数据正在查询中..."});
    myMask.show();
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.DayForecast','CreateDayProduct'),
       params: { fromDate: fromDate},
            success: function(response){
                        myMask.hide();
                        if(response.responseText!=""){
                        queryData();
                            }
            }, 
            failure: function(response) { 
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         }); 
}
//当没有数据的时候按行清空
function clearElement() {
    var aryDiv = forecastTable.getElementsByTagName("div");
    for (var i = 0; i < aryDiv.length; i++) {
       if (aryDiv[i].id.substr(0,1) == "H" && aryDiv[i].id != "H00"){//当前切换的日期控件的不清空
            aryDiv[i].innerHTML = "";
        }
    }
}
function  ExportDayData()
{
    var fromDate=Ext.getDom("H00").value;
    var content=fromDate;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("ExportBut").click();
}
function printit()
{
    bdhtml = window.document.body.innerHTML; //获取当前页的html代码 
    sprnstr = "<!--startprint-->"; //设置打印开始区域 
    eprnstr = "<!--endprint-->"; //设置打印结束区域 
    prnhtml = bdhtml.substring(bdhtml.indexOf(sprnstr) + 18); //从开始代码向后取html
    prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr)); //从结束代码向前取html
    window.document.body.innerHTML = prnhtml;
    window.print();
    window.document.body.innerHTML = bdhtml;

}