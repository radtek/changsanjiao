// 巡检人员面板，完成巡检人员的实时定位、轨迹回放和人员管理

PatrolCheck = function(){
//     var item1 = new Ext.Panel({
//                border:false,
//                title: 'T639格点 ',
//                bodyCssClass:"t639"
//            });

//            var item2 = new Ext.Panel({
//                border:false,
//                title: '欧洲中心 ',
//                html: '&lt;empty panel&gt;',
//                cls:'empty'
//            });
            
//              // simple array store
//    var store = new Ext.data.ArrayStore({
//        fields: ['abbr', 'state', 'nick'],
//        data : [
//        ['AL', '男', 'The Heart of Dixie'],
//        ['AK', '女', 'The Land of the Midnight Sun']]
//    });
//    var combo = new Ext.form.ComboBox({
//        store: store,
//        displayField:'state',
//        typeAhead: true,
//        mode: 'local',
//        forceSelection: true,
//        triggerAction: 'all',
//        emptyText:'Select a state...',
//        selectOnFocus:true
//    });
            
    PatrolCheck.superclass.constructor.call(this,{
         border:false,
         header:false,
         title: '卫星反演',
         layout:'accordion',
                items: [item1, item2]
    });

};


//继承，添加函数，或者重写函数
Ext.extend(PatrolCheck, Ext.Panel, {

    
});
//注册，可以通过xtype:ReadearthGISToolbar来创建此工具条
Ext.reg('patrolcheck', PatrolCheck); 
