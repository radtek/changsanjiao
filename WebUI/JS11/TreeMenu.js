//树状子菜单，此子菜单有别于预报信息和图片信息，属于其他范畴，用户可以自定义的第四级菜单

TreeMenu = function(center,node){
    var firstLoad = true;//表明首次载入
    var root = new Ext.tree.AsyncTreeNode({
                    id :node.id,
                    text : node.text,
                    loader : new Ext.tree.TreeLoader({
                                 dataUrl: getUrl('MMShareBLL.DAL.TreeMenu','GetList')
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
 	                        click : nodeClick
                       }
	});    
//	// 加载时自动展开根节点 
//    treePanel.expandAll();   
   
    
    //获取基菜单
    var pMenu = node.parentMenu;
    var ownerCtr = node.ownerCt;
    while(Ext.isDefined(pMenu)){
        ownerCtr = pMenu.ownerCt;
        pMenu = pMenu.parentMenu;
    }
   
    TreeMenu.superclass.constructor.call(this,{
             border:false,
             header:false,
             layout : "fit",
             layoutConfig:{animate:true},
             bodyStyle:'background-color: transparent',
             title: ownerCtr.text,
             items:[treePanel]
        });

 
//*************************************************************************************************
//  下面开始为此类的模块级别函数
//*************************************************************************************************
    function nodeClick(node, e){
        var htmPanel = new Ext.Panel({
            border:false,
            header:false,
            layout : "fit",
            html : "<iframe src='" + node.id +"' height='100%' width='100%' frameborder='0'></iframe>"
        });
        center.removeAll(); 
        center.add(htmPanel);
        center.doLayout();
    }
	
}


//继承，添加函数，或者重写函数
Ext.extend(TreeMenu, Ext.Panel, {
   
});
//注册，可以通过xtype:TreeMenu来创建面板
Ext.reg('treeMenu', TreeMenu); 
