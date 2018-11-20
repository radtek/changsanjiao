// JScript 文件
Ext.onReady(function(){
    supportInnerText();//使得火狐支持innerText
    querySiteData();
   }
)

function addTabClick(sender){
    var popup = Ext.getDom("Add_popup");
    popup.style.left = getElementLeft(sender,popup.parent) + "px";
    popup.style.top =  getElementTop(sender,popup.parent) + sender.offsetHeight + 2 + "px";    
    popup.style.display = "block";
    var el = Ext.get("Add_popup");

    el.setHeight(170, true);//动画效果

}

function bodyClick(evt){
    var eventSource = getEventSource(evt);
    var parentNode = eventSource.parentNode;
    if(parentNode!=null)
    {
        if(parentNode.id != "addTab")
        {
          hidePopup();
        } 
    }  
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
       obj.style.backgroundColor = "#fff";
    }
}
//在页面上任意处点击，隐藏tab选择面板，除了弹出按钮
function hidePopup()
{
    var popup = Ext.getDom("Add_popup");
    popup.style.height = "0px";
    popup.style.display = "none";
}

function addSite(sender,id){
    var name = sender.innerHTML;

    //隐藏站点选择面板
    hidePopup();
    //使得选中的标签变为灰色
    var parentNode = sender.parentNode;
    parentNode.className = "popup_text_disable";
    parentNode.innerHTML = name;
    
    
    var li =document.createElement("li");
    //li.id = "T" + id;
    
    var liHtml = String.format("<span id='T{0}'><a href=\"javascript:tabClick('T{0}');\">{1}<img src='images/b_close.png' class='close_ico' onclick=\"closeTab('T{0}');\" /></a></span>",id,name);
    li.innerHTML = liHtml;
    
    var liEl = new Ext.Element(li);
    
    var el = Ext.get("addTab");
    liEl.insertBefore(el);
    var lastTabID=lastTab;
    var lastEl = Ext.get(lastTab);
    if(lastEl != null){
        lastEl.removeClass("tabHighlight");
        var innerHTML = lastEl.dom.innerHTML;
        lastEl.dom.innerHTML ="";
        lastEl.createChild({tag:'a',href:String.format("javascript:tabClick('{0}');",lastTabID),html:innerHTML});
    }  
    lastTab=String.format("T{0}",id);
    var curEl = Ext.get(lastTab);
    curEl.addClass("tabHighlight");
    var child = curEl.first();
    curEl.dom.innerHTML = child.dom.innerHTML;
    var aryDiv = Ext.query("div[class=show]","siteDataTable"); 
    for(var i=0;i<aryDiv.length;i++){
        hideEl(aryDiv[i].id);
    }
    

    querySiteData();
    
    
}

//关闭指定的tab页
function closeTab(tabID){
    var el = Ext.get(tabID);
    var li = el.findParent("li",2,true);
    li.remove();
    if(el.dom.className==" tabHighlight")
    {
       lastTab="T183";
       var curEl = Ext.get(lastTab);
       curEl.addClass(" tabHighlight");
       var child = curEl.first();
       curEl.dom.innerHTML = child.dom.innerHTML;
    }
    
    var siteID = tabID.substr(1,tabID.length-1);
    var siteDiv = Ext.getDom("P" + siteID);
    siteDiv.className = "popup_text";
    siteDiv.innerHTML = String.format("<a href='#' onclick='addSite(this,{0})'>{1}</a>",siteID,siteDiv.innerHTML);
    
}

//站点切换
function tabClick(id){
    //这里主要来避免点击的时候删除了此Tab页
    var curEl = Ext.get(id);
    if(curEl == null)
        return;
    //由于设置显示和隐藏的div标签比较多，耗时，因此在这里就设置上一个id，以便于快速选择tab页的时候不出错  
    var lastTabID = lastTab;
    lastTab = id;

    var lastEl = Ext.get(lastTabID);
    if(lastEl != null){
        lastEl.removeClass("tabHighlight");
        
        var innerHTML = lastEl.dom.innerHTML;
        lastEl.dom.innerHTML ="";
        lastEl.createChild({tag:'a',href:String.format("javascript:tabClick('{0}');",lastTabID),html:innerHTML});
    
    }
    
    curEl.addClass("tabHighlight");
    
    var child = curEl.first();
    curEl.dom.innerHTML = child.dom.innerHTML;
    
    //隐藏当前所有显示的div
    var aryDiv = Ext.query("div[class=show]","siteDataTable"); 
    for(var i=0;i<aryDiv.length;i++){
        hideEl(aryDiv[i].id);
    }
    
    //id后三位包含当前的站点编号的
    var idMatch = String.format("div[id$={0}]",id.substr(1,id.length-1));
    aryDiv=  Ext.query(idMatch,"siteDataTable");
    for(var i=0;i<aryDiv.length;i++){
        showEl(aryDiv[i].id);
    }  

}

//查询站点空气质量数据
function querySiteData(){
    var fromDate = Ext.getDom("H00").value;
//    var toDate = Ext.getDom("H01").value;
    var siteIDs="";

    var el = Ext.get("tabItem");
    var child = el.first("li");
    var tabSite;
    while(child != null){
        tabSite = child.first("span",true);
        if(tabSite != null)
            siteIDs = siteIDs + tabSite.id.substr(1,lastTab.length-1) + ",";
        child = child.next();
    }
    
    
    siteIDs = siteIDs.substr(0,siteIDs.length-1);
    var curSiteID = lastTab.substr(1,lastTab.length-1);
    var myMask = new Ext.LoadMask(document.body, {msg:"数据正在查询中..."});
    myMask.show();
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.SiteData','QueryPerHour'),
        params: { fromDate: fromDate,siteIDs:siteIDs,curSiteID:curSiteID}, 
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
    var curSiteID = lastTab.substr(1,lastTab.length-1);
    var content=fromDate+"|"+curSiteID;
    var Element=document.getElementById("Element");
    Element.setAttribute("value",content);
    document.getElementById("ButtonExport").click();
}
