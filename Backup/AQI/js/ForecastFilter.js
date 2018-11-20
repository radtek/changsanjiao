

Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        initInputHighlightScript();
        queryData();
    }
)
function mouseOver(obj)
{
    var id=obj.id;
    id.substring(6,id.length);
    var forecasPeriod=getCheckBValue("forecasPeriod");
    var periodCount=forecasPeriod.split(",").length;
    if(obj!=null)
    {
         if(parseInt(id.substring(6,id.length))==1||(parseInt(id.substring(6,id.length))-1)%periodCount==0)
         {
               for(var i=1;i<obj.cells.length;i++)
               {
                  
                    obj.cells[i].bgColor= "#badbff";
               }
         }
          else 
          {
               for(var i=0;i<obj.cells.length;i++)
               {              
                 obj.cells[i].bgColor= "#badbff";
               }
          }
       
    }

}
function mouseOut(obj)
{
    var id=obj.id;
    id.substring(6,id.length);
    var forecasPeriod=getCheckBValue("forecasPeriod");
    var periodCount=forecasPeriod.split(",").length;
    if(obj!=null)
    {
         if(parseInt(id.substring(6,id.length))==1||(parseInt(id.substring(6,id.length))-1)%periodCount==0)
         {
               for(var i=1;i<obj.cells.length;i++)
               {
                 obj.cells[i].bgColor = "#FFFFFF";
               }
         }
          else 
          {
               for(var i=0;i<obj.cells.length;i++)
               {
                  obj.cells[i].bgColor= "#FFFFFF";
               }
          }
       
    }
}
function getRadioValue()
{
 var obj=document.getElementsByName("period");
   if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
       if(obj[i].checked)
       { 
        return obj[i].value;
       }
      }
    }
}
function getCheckBValue(objName)
{
var postJson = "";
 var forecasArray=new Array();
 var obj=document.getElementsByName(objName);
   if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
       if(obj[i].checked)
       {
       postJson=postJson+obj[i].value+",";
       }
      }
    }
    if(postJson.length>0)
    {
     postJson=postJson.substring(0,postJson.length-1);
    }
   return postJson;
}
function radioClick(id){
    var el = Ext.getDom(id); 
    if(el.className == "radioUnChecked"){
        el.className = "radioChecked";
        el = getGroupEl(id);
        el.className = "radioUnChecked";       
    } 
     queryData();
}
//预报污染物tab切换
function tabClick(id){
    var lastParts = lastTab.split("_");
    var lastID=lastParts[1];
    var curParts = id.split("_");
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTab);
    lastEl.className = "";
    
    lastEl.innerHTML = curEl.innerHTML.replace(id,lastTab).replace(curParts[0],lastParts[0]);
    
    curEl.className = "tabHighlight";
    curEl.innerHTML = curParts[0];
    var currentID=curParts[1];
    var tableTitle=document.getElementById("Tb"+currentID);
    var forecastTable=document.getElementById("Tb"+lastID);
    tableTitle.className="show";
    forecastTable.className="hidden";
    
    lastTab = id;    
    

}
function getGroupEl(id){
    var groupEl = "";
    switch(id){
    case "rd1":
        groupEl = "rd2";
        break;
    case "rd2":
        groupEl = "rd1";
        break;
    }   
    return Ext.getDom(groupEl);
}
function  queryData()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var period;
    if (rd1.className == "radioUnChecked")
       period = "48";
    else period="24";
    var forecasPeriod=getCheckBValue("forecasPeriod");
    var dataType=getCheckBValue("dataType");
    var dataModule=getCheckBValue("dataModule");
    var myMask = new Ext.LoadMask(document.body, {msg:"数据正在查询中..."});
    myMask.show();
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.ComForecast','GetFilterDataTables'),
       params: { fromDate: fromDate,toDate:toDate,period:period,forecasPeriod:forecasPeriod,dataType:dataType,dataModule:dataModule},
            success: function(response){
                        myMask.hide();
                        if(response.responseText!=""){
                            var tableHtml=new Array();
                            tableHtml=response.responseText.split("|");
                            Ext.getDom("Tb0").innerHTML =tableHtml[5];
                            Ext.getDom("Tb1").innerHTML =tableHtml[0];
                            Ext.getDom("Tb2").innerHTML =tableHtml[1];
                            Ext.getDom("Tb3").innerHTML =tableHtml[2];
                            Ext.getDom("Tb4").innerHTML =tableHtml[3];
                            Ext.getDom("Tb5").innerHTML =tableHtml[4];

                            }
            }, 
            failure: function(response) { 
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         }); 
 
}
function queryExportData()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var period;
    if (rd1.className == "radioUnChecked")
       period = "48";
    else period="24";
    var forecasPeriod=getCheckBValue("forecasPeriod");
    var dataType=getCheckBValue("dataType");
    var dataModule=getCheckBValue("dataModule");
    var content=fromDate+"|"+toDate+"|"+period+"|"+forecasPeriod+"|"+dataType+"|"+dataModule;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("SearchBut").click();
}
