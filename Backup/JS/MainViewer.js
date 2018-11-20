/*
此JS主要用于主页面的布局，包括抬头（north）、左侧面板（west）、中间面板（center）和右侧面板（east）。
版权所有：上海地听信息科技有限公司  http://www.readearth.cn
作者：张伟锋        日期：2010年11月20日
*/

Ext.BLANK_IMAGE_URL = 'Ext/resources/images/default/s.gif';
var tempArray=new Array();
//定义MainViewer类
MainViewer = {};
var currentUserName;
function toolbarClick(itemID){
    var toolItem = Ext.getCmp(itemID);
    toolItem.fireEvent("click",toolItem);
}
//界面初始化
Ext.onReady(function(){
    Ext.QuickTips.init();
    Ext.useShims=true;
    var resultInfo= Ext.getDom("loginResult").value;
    if(resultInfo!="")
    {
       var result = Ext.util.JSON.decode(resultInfo);
       Ext.removeNode(Ext.getDom("loginpanelTop"));
       InitialViewer(result);
    }
    else 
    {    
        var loginWin = new Login("loginPanel",InitialViewer);

    }
   
    //去除动态加载效果
//    Ext.removeNode(Ext.getDom("loading"));
});
function clearTbl(tbl){            
       while((tbl.rows.length+1)>1){
              tbl.deleteRow();    
       }    
}
function reSetPassWord(userName)
{
  var thePassword = Ext.getCmp("password");
                if (thePassword == null) {
                    thePassword = new Password(userName);
                    thePassword.show();
                }
}
//初始化视窗函数
function InitialViewer(loginTime){
    Ext.getDom("loginResult").value=loginTime.UserName;
    if(loginTime.JB == 2){
        new OtherViewer(loginTime);
        return;
    }
    var authority=loginTime.UserAuthority;
    tempArray=authority.split(',');
  
    //工具栏
    var toolbar = new GISToolbar(tempArray);
    toolbar.on('itemClick', itemClick);
    

    function itemClick(action,iconCls){
        switch(action.id){
   case "airQuality": case "xsforcast": case "innp": case "jgRadar":case "comForecasts":case "mmcShare":case "systemManage":case "dayForecast":
            center.removeAll(true); 
            west.removeAll(true); 
 
            west.setIconClass(iconCls);
            if(west.hidden)
                west.show();
            west.expand(false);
        
            west.add(new ImageProduct(center,action));
            
            
            west.doLayout();

            break;
        case "webGIS":
            addHtmlPanel("WebGIS/index.html");
            break;
        case "jhForecast":
            addHtmlPanel("JhForecast/Default.aspx");
            break;
        case "zx":
            exit();
        }
        viewport.doLayout();
         
    }
    function exit()
    {

      var userName=loginTime.UserName;
      var ip=loginTime.LoginIP;
      Ext.Ajax.request({
                    url:getUrl('MMShareBLL.DAL.UserManager','Exit'),
                    timeout:120000,
                    params: { userName: userName,ip:ip},
                            success: function(response){   
                                window.location.href = "Default.aspx?Method=loginOut"; 
                                  
                                }, 
                                failure: function(response) { 
                                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                                }
                        
                })                     
    }
    
    
    
    function showLoginCount(){
       var win = new Ext.Window({
						title : "操作日志",
						layout : "fit",
						width : 300,
						height : 350,
						resizable : false,
						shadow : true,
						modal : true,
						closable : true,
						bodyStyle : "padding:1 1 1 1",
						animCollapse : true,
                        html : "<iframe src='Logs.aspx' height='100%' width='100%' frameborder='0'></iframe>"
						
					});
			win.show();
    }
    
    //打开HTML的Frame
    function addHtmlPanel(iframeSrc){
        if(west.hidden == false)
            west.hide();
            var height="100%";
        var htmPanel = new Ext.Panel({
            border:false,
            header:false,
            layout : "fit",
            html : "<div id='frameID' style='width:100%;height:"+ height +";-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + iframeSrc +"' height='100%' width='100%' frameborder='0'></iframe></div>"
        });
        center.removeAll(); 
        center.add(htmPanel);
        center.doLayout();
    }
    
    //系统抬头，包括系统的名称、模块列表
    var north = new Ext.Panel({
        region:"north",
        border:false,
        baseCls :"header",
//       html:"<div class='loginfont defaultBackimage'>欢迎第<b style='color:Orange'>"+loginTime.LoginCount+"</b>次登陆，<a  style='color: #FFFFFF' href='javascript:reSetPassWord(\""+ loginTime.UserName + "\")'>"+loginTime.Alias+"</a><br>"+loginTime.Local+"<br>技术支持：上海地听" + "&nbsp;</div>",
        html:"<div class='loginfont defaultBackimage'><div class='rightHeader'><div class='login-info'><p class='access'>欢迎第<b style='color:Orange'>"+loginTime.LoginCount+"</b>次登陆<p class='user'><a  href='javascript:reSetPassWord(\""+ loginTime.UserName + "\")'>"+loginTime.Alias+"</a><p class='date'>"+loginTime.Local+"</div></div></div>",
        bbar:toolbar
    });
        
    //地图面板
    var center = new Ext.Panel({
                    id: "frameCenter",
                    layout : "fit",

                    region:"center"
                    
                });
                
    
    //左侧功能面板，此面板可以根据工具栏按钮事件进行切换
    var west = new Ext.Panel({
        id:'westPanel',
        region:'west',
        width:200,
        layout:'fit',
        margins: '0 5 0 0',
        collapsible:true,
        bodyCssClass:"westBackground",
        iconCls: 'weather',
        listeners:{"beforeadd":function(ownerCt,functionPanel,index){ownerCt.setTitle(functionPanel.title);}}//动态改变面板抬头
    });

    
	
	//右侧查询结果面板，此面板缺省处于隐藏状态，可以改变大小
	var east = new Ext.Panel({
        id:'eastPanel',
        region:'east',
        width:200,
        layout:'fit',
        margins: '0 0 0 0',
        collapsible:true,
        collapsed:true,
        collapseMode:'mini',
        split: true,
          items: {
                                    baseCls: 'x-plain',
                                    html: '<p style="padding:10px;color:#556677;font-size:11px;">Select a configuration to the right &raquo;</p>'
                                }
        
	});
	
	//主窗口
    var viewport = new Ext.Viewport({// 填充整个浏览器窗口
            id:'mainLayout',
            layout:'border',// 边框布局
            items:[north,west,center]
        });
        
  
  itemClick(toolbar.getComponent('airQuality'),'weather')
  {
   toolbar.getComponent('airQuality').addClass("navi");
  }
}