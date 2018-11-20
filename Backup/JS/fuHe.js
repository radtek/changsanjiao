// 根据不同的用户可以实现不同的界面


//初始化视窗函数
FuHeViewer=function(loginTime){
    //打开HTML的Frame
    function addHtmlPanel(iframeSrc){
        var htmPanel = new Ext.Panel({
            border:false,
            header:false,
            layout : "fit",
            html : "<iframe src='" + iframeSrc +"' height='100%' width='100%' frameborder='0'></iframe>"
        });
        center.removeAll(); 
        center.add(htmPanel);
        center.doLayout();
    }
    
    //系统抬头，包括系统的名称、模块列表
    var north = new Ext.Panel({
        region:"north",
        border:false,
        baseCls :"wHeaderOther",
        html:"<div class='loginfont otherDefaultBackimage'><div class='OtherrightHeader'><br>项目资助：上海市科学技术委员会&nbsp&nbsp<br>资助（11231200500）<br>系统开发：上海市环境监测中心&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</div></div>",
        listeners :{"resize":
            function(sender,adjWidth){
                if(adjWidth<1232)
                    sender.setWidth(1232);
            }
        }
    });
    
    //地图面板
    var center = new Ext.Panel({
                    id: "frameCenter",
                    layout : "fit",

                    region:"center"
                    
                });
                
	
	function createButton(id){
	    var btnText = "";
	    var url = "";
	    var entity="";
	    switch(id){
        case "forecast":
            btnText = "72小时预报";
            url = "AQI/ForecastQuenceDiagram.aspx";
            break;
        case "PM2":
            btnText = "PM2.5分布图";
            entity="ShanghaiFCPM25";
            break;
        case "O3":
            btnText = "O3分布图";
            entity="ShanghaiFCO3";
            break;          
        
        } 
            
        var btn = new Ext.BoxComponent({
            id:id,
            width:88,
            height:200,
            cls : 'airButtonFuhe ' + id,
            overCls:id + '_h',
            listeners: {
                            render: function(component){
                                component.getEl().on('click', function(e){
                                        if(url=="")
                                            {
                                               Ext.Ajax.request({ 
                                                    url: getUrl('MMShareBLL.DAL.Forecast','GetEntity'),
                                                    params: { entityName: entity}, 
                                                    success: function(response){
                                                        var name;
                                                        var result = Ext.util.JSON.decode(response.responseText);
                                                        if(id=="PM2")
                                                            name="PM2.5";
                                                        if(id=="O3")
                                                            name="O3";
                                                        var entity = {
                                                            align: "V",
                                                            alias: name,
                                                            realtime: ""
                                                        }
                                                        center.removeAll(); 
                                                        center.add(new ImageFrame(result,entity));
                                                        center.doLayout();
                                                    }, 
                                                    failure: function(response) { 
                                                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                                                    }
                                                    }); 
                                            }  
                                            else 
                                            {
                                              addHtmlPanel(url);
                                             }
                                });
                        }                         
                 },
            html:"<div class='ariButtonText' ><a href='#'>"+btnText+"</a></div>"
        });
        
	    return btn;
	}
	
	
    //左侧功能面板，此面板可以根据工具栏按钮事件进行切换
    var west = new Ext.Panel({
        id:'westPanel',
        region:'west',
        layout: {
                    type:'vbox',
                    padding:'25',
                    align:'center'
                },
        width:190,
        margins: '0 0 0 0',
        bodyCssClass:"wWestBackground"
    });
	
	var	btn = createButton("forecast");
	west.add(btn);
	
	btn = createButton("PM2");
	west.add(btn);
	
	btn = createButton("O3");
	west.add(btn);
	
    //主窗口
    var viewport = new Ext.Viewport({// 填充整个浏览器窗口
            id:'mainLayout',
            layout:'border',// 边框布局
            items:[north,west,center]
        });
        
        
  //初始化
  addHtmlPanel("AQI/ForecastQuenceDiagram.aspx");
    
  
}