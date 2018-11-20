// 根据不同的用户可以实现不同的界面


//初始化视窗函数
var nowComponent=null;
OtherViewer =function(loginTime){
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
        baseCls :"wHeader",
        html:"<div class='loginfont wDefaultBackimage'><div class='rightHeaderQi'><br>欢迎第<b style='color:Orange'>"+loginTime.LoginCount+"</b>次登陆，"+loginTime.Alias+"<br>"+loginTime.Local+"<br>技术支持：上海地听" + "&nbsp;</div></div>",
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
	    switch(id){
        case "potting":
            btnText = "空气质量";
            url = "AQI/AirQuality.aspx";
            break;
        case "paper":
            btnText = "气象数据";
             url = "AQI/DataShare.aspx";
            break;
        case "people":
            btnText = "预报会商";
            url = "AQI/Consultation.aspx";
            break;            
        case "compare":
            btnText = "对比分析";
            url = "AQI/AQICompare.aspx";
            break;
        case "comforecast": 
            btnText = "综合预报";
            url = "AQI/Default.aspx";
            break;
        case "comforecastModify": 
            btnText = "预报更正";
            url = "AQI/ForecastRevise.aspx";
            break;
        
        }    
            
        var btn = new Ext.BoxComponent({
            id:id,
            width:65,
            height:100,
            cls : 'airButton ' + id ,
            overCls:id + '_h',
            listeners: {
                            render: function(component){
                                component.getEl().on('click', function(e){
                                    addHtmlPanel(url);changeBackground(component);
                                });
                            
                        }                         
                 },
            html:"<div class='ariButtonText' ><a href='#'>"+btnText+"</a></div>"
        });
        
	    return btn;
	}
	
	function changeBackground(evt)
	{
	 var compoent=evt.getEl();
	 if (nowComponent!=compoent&&nowComponent!=null)
	 {
	  nowComponent.removeClass(nowComponent.id+'_m');

	 }
	 var id=compoent.id;
	 compoent.addClass(compoent.id+'_m');
	 nowComponent=compoent;
	}
    //左侧功能面板，此面板可以根据工具栏按钮事件进行切换
    var west = new Ext.Panel({
        id:'westPanel',
        region:'west',
        layout: {
                    type:'vbox',
                    padding:'5',
                    align:'center'
                },
        width:190,
        margins: '0 0 0 0',
        bodyCssClass:"wWestBackground"
    });
	
	var	btn = createButton("compare");
	west.add(btn);
	
	btn = createButton("people");
	west.add(btn);
	
	btn = createButton("potting");
	west.add(btn);
	
	btn = createButton("paper");
	west.add(btn);
	
	btn = createButton("compare");
	west.add(btn);
	
    btn = createButton("comforecast");
	west.add(btn);
		
    btn = createButton("comforecastModify");
	west.add(btn);
	
    //主窗口
    var viewport = new Ext.Viewport({// 填充整个浏览器窗口
            id:'mainLayout',
            layout:'border',// 边框布局
            items:[north,west,center]
        });
        
        
  //初始化
  addHtmlPanel("AQI/AQICompare.aspx");
    
  
}