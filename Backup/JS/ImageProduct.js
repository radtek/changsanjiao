// JScript 文件



ImageProduct = function(center,node){
    
    var firstLoad = true;//表明首次载入
    var root = new Ext.tree.AsyncTreeNode({
                    id :node.id,
                    text : node.text,
                    loader : new Ext.tree.TreeLoader({
                                 dataUrl: getUrl('MMShareBLL.DAL.Forecast','GetImageProduct')
                             })
        });
        
    var treePanel = new Ext.tree.TreePanel({
        containerScroll : true,
        autoScroll : true,
        animate : true,
        height:150,
        border:false,
        root : root,
        region:"center",
        bodyStyle:'background-color: transparent',
        // 默认根目录不显示
        rootVisible:true,
        listeners : {
            click : nodeClick,
            load : afterLoaded,
            expandnode:afterExpand
       }
	}); 
	treePanel.expandAll();   
//    root.expand();
//   
       
    ImageProduct.superclass.constructor.call(this,{
             border:false,
             header:false,
             layout : "fit",
             layoutConfig:{animate:true},
             bodyStyle:'background-color: transparent',
             title: node.text,
             items:[treePanel]
        });

 
//*************************************************************************************************
//  下面开始为此类的模块级别函数
//*************************************************************************************************
    function nodeClick(node, e){
        if (node.leaf == true && node.id.indexOf("|") != -1) { 
            var aryEntityInfo = node.id.split("|");//获取实体的名称和排列方式
            if(aryEntityInfo[1] == "U")//Alain为U的时候，表明是指向一个网页
                addHtmlPanel(node.attributes.tag);
            else{
                Ext.Ajax.request({ 
                url: getUrl('MMShareBLL.DAL.Forecast','GetEntity'),
                params: { entityName: aryEntityInfo[0] }, 
                success: function(response){
                    var result = Ext.util.JSON.decode(response.responseText);
                    var entity = {
                        align: aryEntityInfo[1],
                        alias: node.text,
                        realtime: node.attributes.tag,
                        name:node.attributes.aliasName
                    }
                    center.removeAll();
                    center.add(new ImageFrame(result, entity));
                    center.doLayout();
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
                }); 
            }
        }
            
            
    }
    
    //配置的树状结构是指向一个URL
    function addHtmlPanel(iframeSrc){
         Ext.TaskMgr.stopAll();
         var height;
         if(!Ext.isIE8)
           height="100%";
        else 
           height=window.screen.availHeight-window.screenTop-145+"px";//IE8高度控制，如果不加的话，页面显示不全
        var htmPanel = new Ext.Panel({
            border:false,
            header:false,
            layout : "fit",
            html : "<div id='frame' style='width:100%;height:"+ height +";-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + iframeSrc +"' height='100%' width='100%' frameborder='0'></iframe></div>"
        });
        center.removeAll(); 
        center.add(htmPanel);
        center.doLayout();
    }
    
    //当图片产品加载完成后自动触发图片显示
    function afterLoaded(node){
        if (firstLoad){
            while(node != null && node.hasChildNodes()){               
                node = node.item(0);
            }
            if(node != null){
                 nodeClick(node);               
                 firstLoad = false;
                
            }
        }
    }
    var firstLeaf=true ;
    function afterExpand(node)
    {
        if (firstLeaf && node.isLeaf())
        {
            node.select();
            firstLeaf = false;

        }
    }
    
    //请求成功以后
    function doSuccess(response)
    {
        
    } 
	
}


//继承，添加函数，或者重写函数
Ext.extend(ImageProduct, Ext.Panel, {
   
});
//注册，可以通过xtype:forecastPanel来创建面板
Ext.reg('imageProduct', ImageProduct); 
