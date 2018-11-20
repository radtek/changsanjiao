// 此工具栏用于开发GIS系统，响应按钮的点击事件，分为工具按钮，和一般按钮。根据工具和按钮为其添加统一的事件
var lastaction=null;

Ext.override(Ext.menu.Item, {
    // private
    handleClick: function(e) {
        if (!this.href) { // if no link defined, stop the event automatically
            e.stopEvent();
        }
        //改成点击展开
        if(this.menu)
            this.expandMenu();
        else
            Ext.menu.Item.superclass.handleClick.apply(this, arguments);
    }
})
//类定义部分主要完成构造函数，以及定义属性   
GISToolbar = function(tempArray){
    //注册事件，一般都放在最前面，保证所有触发的地方都可以引用

 this.addEvents({itemClick:true});
 function createButton(id) {
       var titleText;
       var inconClass;
       switch(id)
       {
        case "airQuality":
            titleText="实时监测";
            inconClass="tree";
            break;
        case "jgRadar":
            titleText="超级站";
            inconClass="chartsDots";
            break;
        case "innp":
            titleText="气象观测";
            inconClass="weather";
            break;
        case "xsforcast":
            titleText="数值预报";
            inconClass="dataforcast";
            break;
        case "comForecasts":
            titleText="综合预报";
            inconClass="forcast";
            break;
        case "webGIS":
            titleText="GIS";
            inconClass="world";
            break;
        case "mmcShare":
            titleText="数据共享";
            inconClass="share";
            break;
        case "systemManage":
            titleText="系统管理";
            inconClass="manage";
            break;
        case "dayForecast":
            titleText="日报";
            inconClass="dayReport";
            break;
        case "zx":
            titleText="退出系统";
            inconClass="zx";
            break;

        }
        var stringHtml= "<div><a href='#'><span class='buttonTop " + inconClass + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + titleText + "</span></a><div>";
        var btn = new Ext.BoxComponent({
            id: id,
            height: 28,
            overCls:'navi-btn-h',
            listeners: {
                render: function(component) {
                    component.getEl().on('click', function(e) {
                        var ownerCt = component.ownerCt;
                        ownerCt.onItemClick(component,inconClass);
                    });
                }
            },
              html:stringHtml  

        });
        btn.text=titleText;
        return btn;
    }

    function createSplit(id)
    {
        var btn = new Ext.BoxComponent({
            id: id,
            height: 28,
            html: "<div class='navi-line'><ul></ul></div>"  
        });
        return btn;
    }

     GISToolbar.superclass.constructor.call(this, {
        baseCls: 'navi-area',
        id: 'gisToolbar',
	    layout: 'hbox',

        items:[]
    });
    for(var i=0;i<tempArray.length-1;i++)
    {
        this.add(createButton(tempArray[i]));
        this.add(createSplit(i));
    }
    this.add(createButton(tempArray[tempArray.length-1]));
    lastaction=this.getComponent('airQuality');
}

Ext.extend(GISToolbar, Ext.Panel, {

    //触发itemClick事件

    onItemClick: function(action,iconCls) {  
        if (lastaction!=action&&lastaction!=null)
       {
           lastaction.removeClass("navi");
       }
       this.fireEvent('itemClick', action,iconCls);
       action.addClass("navi");
       lastaction=action;        
    }
});

