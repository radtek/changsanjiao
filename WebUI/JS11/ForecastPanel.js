// 预报信息面板


ForecastPanel = function(center){
    
    var root = new Ext.tree.AsyncTreeNode({
                    id :"0",
                    loader : new Ext.tree.TreeLoader({
                                 dataUrl: getUrl('MMShareBLL.DAL.Forecast','GetList')
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
                        rootVisible:false,
                        listeners : {
	                        click : nodeClick
                        }
	});    
//	// 加载时自动展开根节点 
    treePanel.expandAll();   
    
    ForecastPanel.superclass.constructor.call(this,{
             border:false,
             header:false,
             layout : "fit",
             bodyStyle:'background-color: transparent',
             title: '海洋气象',
             items:[treePanel]
        });

 
//*************************************************************************************************
//  下面开始为此类的模块级别函数
//*************************************************************************************************
    function nodeClick(node, e){
            
            center.removeAll(); 
//            var flexGrid = new FlexGrid();
            center.add(new FlexGrid(node.id));
            center.doLayout();
    }
	
}


//继承，添加函数，或者重写函数
Ext.extend(ForecastPanel, Ext.Panel, {
   
});
//注册，可以通过xtype:forecastPanel来创建面板
Ext.reg('forecastPanel', ForecastPanel); 
