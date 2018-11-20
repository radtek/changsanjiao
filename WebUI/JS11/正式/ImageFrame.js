// 图片框架，根据产品名称调取图片
var startTime;
var endDateTime;
var flag=false;
ImageFrame = function(fieldInfo,entity){

    var regionType = "north";
    var labelAlign = "left";
    if(entity.align == "V"){
        regionType = "east";
        labelAlign = "top";

    }
    flag=true;
    Ext.TaskMgr.stopAll(); 
    var entityName = fieldInfo[0].EntityName;
    var name=entity.name;
    var lstHeight = 0;
    
    supportInnerText();//使得火狐支持innerText

  
    
    //创建查询窗体
    var queryPanel = new Ext.FormPanel({
        id:'eastPanel',
        labelAlign: labelAlign,
        region: regionType,
        width:180,

        collapseMode:'mini',
        collapsed:false,
        bodyStyle:'padding: 5px;',
        defaults:{width:165},
        items : [{
                     xtype: "label",
                     cls : "imageTitle",
                     html : entity.alias
                }],
        buttons: [
        {
            xtype: 'combo',
            id: 'cmbTimes',
            typeAhead: true,
            lazyRender:true,
            mode: 'local',
            triggerAction:'all',
	        forceSelection: true,
	        editable:       false,
	        value:          1,
	        width: 40,
            store: new Ext.data.ArrayStore({
                fields: [
                    'value',
                    'text'
                ],
                data: [
                        [1, '1'], [2, '2'],[3, '3'],[5, '5'],[10, '10']]
            }),
            valueField: 'value',
            displayField: 'text',
            listeners : { 
                render : function(){
                            Ext.QuickTips.register({ 
                                target : this.id, 
                                text : '动画幅数' 
                            });
                          }
                        }
        },
        {
            text: '慢',
            width:30,
            handler:function(){if (playTask.interval < 2000) playTask.interval = playTask.interval + 100;}
        },{
            id:'animation',
            text: '动 画',
            width:50,
            handler:playToggle
        },{
            text: '快',
            width:30,
            handler:function(){if (playTask.interval > 100) playTask.interval = playTask.interval - 100;}
        }]
    });
    
    //获取字典数，用于决定列表框的高度
    var dicCount = 0;
    var fieldValue;
    var dicHeight = 80;
    for(var i=0;i<fieldInfo.length;i++){
        fieldValue = fieldInfo[i];
        if(fieldValue.FieldType == 4)
            dicCount = dicCount + 1;
    }
    
    if (dicCount == 1)
        dicHeight = 180;
    
    //加载查询内容
    for(var i=0;i<fieldInfo.length;i++){
        fieldValue = fieldInfo[i];
        var fieldControl = createControl(fieldValue,dicHeight);
        if (Ext.isDefined(fieldControl)){
            queryPanel.add(fieldControl);
            if(Ext.isArray(fieldControl))
                for(j=0;j<fieldControl.length;j++)
                    lstHeight = lstHeight + fieldControl[j].height + 24;
            else
                lstHeight = lstHeight + fieldControl.height + 24;
        }
        
        //实时监测里面特殊判断的时候调整一下高度  张伟锋    2014-03-25
        if (fieldValue.IsEvent)
            lstHeight = lstHeight - 20;
    }
    
    
    //针对title字符太多，显示不全的问题。通过判断字符数量调整高度   张伟锋     2014-03-25
    var titleLength = get_length(entity.alias);
    if (titleLength > 8)
        lstHeight = lstHeight + 22;

    
    fieldControl = new Ext.Button({
        id: 'btnQuery',
        text: '查 询',
        handler:function(){queryList(entityName,false);}
    });
    queryPanel.add(fieldControl);

    //创建具有数据的listview   
    fieldControl = new Ext.ListView({
        id: entityName,
        store: {},
        fieldLabel: '列表',
        hideLabel: true,
        ctCls: 'listBox',
        loadingText : '载入中...',
        singleSelect : true,
        selectedClass : 'listBox-selected',
        hideHeaders: true,
        anchor: '99% ' + (-60 - lstHeight) ,
        listeners: {
                        selectionchange: selectChange,afterrender: 
                        function(){
                            queryList(entityName,false);
                        }
                    },
        columns: [{ header: 'DM', width: 1, dataIndex: 'MC' }]
    });   
    queryPanel.add(fieldControl);
    AddKeyEvent(entityName);
    //定义动画任务
    var playTask = {
        run: function(){
        play(entityName);},
        interval: 700 //700 毫秒
    }
    var playTaskRefresh = {
        run: function(){
           // playRefresh(entityName);
        },
        interval: 600000 //1小时3600000当页面一直放着不动的话，过1小时就会自动刷新
    }
    var imageViewer = new ImageViewer(Ext.BLANK_IMAGE_URL,entityName,name);
    ImageFrame.superclass.constructor.call(this,{   
        border:false,   
        layout:'border',// 边框布局
        items:[imageViewer,queryPanel]        
    });
//*************************************************************************************************
//  下面开始为此类的模块级别函数
//*************************************************************************************************

//增加键盘左右键
    function AddKeyEvent(viewID){
        var el=Ext.getDoc();
        var component = Ext.getCmp(viewID);
        new Ext.KeyNav(el, {
            forceKeyDown:true,
            left : function(e){
                    MoveSelection(component,-1);
                },
            up : function(e){
                    MoveSelection(component,-1);
                },
            right : function(e){
                    MoveSelection(component,1);
                },
            down : function(e){
                    MoveSelection(component,1);
                },
            scope : el
            }
        );

    }
    
    //根据step的值，来移动选择的记录
    function MoveSelection(view,step){
        //如果正在动画播放，那么停止动画
        var button = Ext.getCmp("animation");
        if(button.getText() == "停 止")
            playToggle(button);
        var selectionsArray = view.getSelectedIndexes();
        var lstCount = view.getNodes().length;//此处getStore()，报错，因此采用getNodes
 
        if (selectionsArray.length >0){
            var selectedIndex = selectionsArray[0];
            selectedIndex = selectedIndex + step;
            if(selectedIndex == -1)
                selectedIndex = lstCount - 1;
            else if(selectedIndex == lstCount)
                selectedIndex = 0;
            //选择记录   
            view.select(selectedIndex);
        }
    }
    
    function playToggle(button,e){
        if(button.getText() == "动 画"){
            button.setText("停 止");
            startTask(playTask);
        }
        else{
            button.setText("动 画");
            stopTask(playTask);
        }
    }
    //点击查询以后罗列出查询列表
    function queryList(listID,isInitial){
            var fieldValue;
            var endTime;
            for(i=0;i<fieldInfo.length;i++){
                fieldValue = fieldInfo[i];
                if(fieldValue.FieldType == 8){
                    //开始时间
                    var startDateCmp = Ext.getCmp(fieldValue.Name + 'startDate');
                    var startHourCmp = Ext.getCmp('hour' + startDateCmp.getId());
                    var startDate = startDateCmp.getValue().add(Date.HOUR, startHourCmp.getValue());
                    startTime=startDate;
                    fieldValue.ShowValue = startDate.format('Y-m-d H:i:s'); 
                    //时效存在设置往后多少日期的情况
                    if(entity.realtime == "" || entity.realtime.indexOf("R") ==0){
                        //结束时间
                        var endDateCmp = Ext.getCmp(fieldValue.Name + 'endDate');
                        var endHourCmp = Ext.getCmp('hour' + endDateCmp.getId());
                        //var endDate = endDateCmp.getValue().add(Date.HOUR, endHourCmp.getValue()).add(Date.MINUTE,59).add(Date.SECOND,59);
                        var endDate = endDateCmp.getValue().add(Date.HOUR, endHourCmp.getValue());
                        endDateTime=endDate;
                        fieldValue.ShowValue = fieldValue.ShowValue + '||' + endDate.format('Y-m-d H:i:s');  
                        endTime = fieldValue.Name + " = '" + endDate.format('Y-m-d H:i:s') + "'";   
                    }   

                   if (endDateTime < startTime) 
                          return;                             
                    
                }else if (fieldValue.FieldType == 4){
                    var fieldControl = Ext.getCmp(fieldValue.Name);
                    if(fieldValue.IsEvent == false){
                        var selectNode = fieldControl.getSelectedNodes()[0];
                        if (Ext.isDefined(selectNode)) {
                            var regN = /\n/g;
                            var nodeText = selectNode.innerText.replace(regN, "");
                            fieldValue.ShowValue = nodeText;
                        }
                    }else{
                        if(fieldControl != null){
                            var checkBoxes = fieldControl.items.items;
                            var period = "";
                            fieldValue.ShowValue = "风场、气压场";
                            for(j=1;j<checkBoxes.length;j++){
                                if(checkBoxes[j].getValue() == true)
                                {
                                    if(checkBoxes[j].boxLabel.indexOf('预报')>0)
                                        period = "0" + checkBoxes[j].boxLabel.substring(0,2);
                                    else
                                        fieldValue.ShowValue = fieldValue.ShowValue + "、" + checkBoxes[j].boxLabel;
                                }  
                            }
                            if(period != "")
                                fieldValue.ShowValue = fieldValue.ShowValue + "+" + period + "+" + endTime;  
                       }
                    }
                }
            }
            var entityObj = "";
            if (isInitial == false)
                entityObj = Ext.util.JSON.encode(fieldInfo);
            var fieldControl = Ext.getCmp(listID);
            var store = new Ext.data.JsonStore({
                url:getUrl('MMShareBLL.DAL.Forecast','QueryList'),
                baseParams:{entityName: entityName,entityObj: entityObj},
                autoLoad:true,
                fields:['DM','MC'],
                listeners:{"load":
                    function(){
                        var nodes = fieldControl.getNodes();
                        if(entity.realtime!= ""&&entity.realtime.indexOf("R")!=0)
                            fieldControl.select(0);
                        else 
                          fieldControl.select(nodes.length-1);
                        
                    }
                }
                });
            
            fieldControl.bindStore(store);
    }
    
    //依次选择要素
    function play(listID){
        var fieldControl = Ext.getCmp(listID);
        var selectionsArray = fieldControl.getSelectedIndexes();
        if (selectionsArray.length >0){
            var timesCmp = Ext.getCmp("cmbTimes");
            var intTimes = timesCmp.getValue();
            var currentSelectIndex = selectionsArray[0] + intTimes;
            var store = fieldControl.getNodes();
            if (store.length <= currentSelectIndex)
                currentSelectIndex = 0; 
            
            fieldControl.select(currentSelectIndex);
        }
    }
    function playRefresh(entityName){
    if(flag)
       flag=false;
    else 
    {
        Ext.Ajax.request({ 
                    url: getUrl('MMShareBLL.DAL.Forecast','GetEntity'),
                    params: { entityName: entityName}, 
                    success: function(response){
                        var result = Ext.util.JSON.decode(response.responseText);
                        fieldInfo=result;
                        for(var i=0;i<fieldInfo.length;i++){
                            fieldValue = fieldInfo[i];
                            if (fieldValue.FieldType == 8)
                            {
                                var lastTimes = fieldValue.ShowValue.split(" ");
                                var startDateCmp = Ext.getCmp(fieldValue.Name + 'startDate');     
                                var startHourCmp = Ext.getCmp('hour' + startDateCmp.getId());
                                if(entity.realtime == "" || entity.realtime.indexOf("R") ==0)
                                {
                                    var startDate = convertDate(lastTimes[0]);
                                    var backDay = -1;
                                    if(entity.realtime.indexOf("R") ==0)
                                        backDay = -parseInt(entity.realtime.substr(2));
                                    startDate = startDate.add(Date.DAY,backDay);
                                    var hour = lastTimes[1].substring(0,2);
                                    startHourCmp.setValue(hour);
                                    startDateCmp.setValue(startDate.format('Y-m-d'));
                                    var endDateCmp = Ext.getCmp(fieldValue.Name + 'endDate');
                                    var endHourCmp = Ext.getCmp('hour' + endDateCmp.getId());
                                    endHourCmp.setValue(hour);
                                    endDateCmp.setValue(lastTimes[0]);
                                }
                                else 
                                {
                                    var lastHour = lastTimes[1].split(":");
                                    startHourCmp.setValue(parseInt(lastHour[0], 10));
                                    startDateCmp.setValue(lastTimes[0]); 
                                }
                            }
                        }
                        queryList(entityName,false);
                  }, 
                    failure: function(response) { 
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                    }
                });
            }
    }
    
    //在查询的结果列表框中选择
    function selectChange(view, nodes){
        view.focus();
        var nodes=view.getNodes();  
        var selectionsArray = view.getSelectedIndexes();
        if (selectionsArray.length >0){
            nodes[selectionsArray[0]].scrollIntoView();
            var store = view.getStore();
            var triggerValue = store.getAt(selectionsArray[0]).get("DM");

            imageViewer.setImageSrc(triggerValue,startTime,endDateTime);
        }else{
            imageViewer.setImageSrc(Ext.BLANK_IMAGE_URL,startTime,endDateTime);
        }
        startTask(playTaskRefresh);
    }
    
    function listSelected(view, nodes){
        var selectionsArray = view.getSelectedIndexes();
        var store = view.getStore();
        var triggerValue = store.getAt(selectionsArray[0]).get("DM");
        var fieldControl = Ext.getCmp(view.linkList);
        
        store = new Ext.data.JsonStore({
            url:getUrl('MMShareBLL.DAL.DictionaryManager','GetDictionaryByDP'),
            baseParams:{dictionaryName:fieldControl.DictName,dm:triggerValue},
            autoLoad:true,
            fields:['DM','MC'],
            listeners:{"load":function(){fieldControl.select(0);}}
        });
        
        fieldControl.bindStore(store);
        
    }
    function trickQueryList(){queryList(entityName,false);}
    
    function createControl(fieldValue,dicHeight){
        var fieldControl;
        //字典字段
        if(fieldValue.FieldType == 4){
            
            if(fieldValue.IsEvent == false){
                //此处设置为autoLoad，那么就会自动请求服务器
	            var store = {};
	            if(fieldValue.YField == "")
	                store = new Ext.data.JsonStore({
                            url:getUrl('MMShareBLL.DAL.DictionaryManager','GetDictionary'),
	                        baseParams:{dictionaryName:fieldValue.DictName},
	                        autoLoad:true,
	                        fields:['DM','MC'],
	                        listeners:{"load":function(){
	                            //当类型比较少的时候，自动调整原先设置的高度  
	                            var length = fieldControl.getStore().getCount();
	                            if(length >0 && length <5){
	                                 fieldControl.setHeight(25 * length );
	                                 var lstView = Ext.getCmp(entityName);
	                                 lstView.setHeight(lstView.getHeight() + (dicHeight - 25 * length ));
	                            }
	                            fieldControl.select(0);
	                          }
	                        }
                        });
                else{
                    var triggerComBox = Ext.getCmp(fieldValue.YField);
                    triggerComBox.linkList = fieldValue.Name;
                    triggerComBox.removeListener("selectionchange",trickQueryList);
                    triggerComBox.addListener("selectionchange",listSelected);
                }
                
                    
                //创建具有数据的listview   
                fieldControl = new Ext.ListView({
                    id: fieldValue.Name,
                    store: store,
                    fieldLabel: fieldValue.Alias,
                    ctCls: 'listBox',
                    loadingText : '载入中...',
                    singleSelect : true,
                    selectedClass : 'listBox-selected',
                    hideHeaders: true,
                    height: dicHeight,
                    columns: [{ header: 'DM', width: 1, dataIndex: 'MC' }],
                    listeners:{"selectionchange":trickQueryList}
                });   
            }else{
                    fieldControl = {    id: fieldValue.Name,
                                        xtype: 'fieldset',
                                        title: fieldValue.Alias,
                                        height: dicHeight,
                                        defaultType: 'checkbox', // each item will be a checkbox
                                        items: [{
                                            checked: true,
                                            boxLabel: '风场、气压场',
                                            hideLabel : true,
                                            name: 'dic01',
                                            disabled:true
                                        }, {
                                            boxLabel: '温度',
                                            hideLabel : true,
                                            name: 'dic02',
                                            listeners:{"check":trickQueryList}
                                        }, {
                                            boxLabel: '12小时预报',
                                            hideLabel : true,
                                            name: 'period12',
                                            listeners:{"check":trickQueryList}
                                        }, {
                                            boxLabel: '24小时预报',
                                            hideLabel : true,
                                            name: 'period24',
                                            listeners:{"check":trickQueryList}
                                        }, {
                                            boxLabel: '36小时预报',
                                            hideLabel : true,
                                            name: 'period36',
                                            listeners:{"check":trickQueryList}
                                        }]
                                    };
            }
//            fieldControl.addListener("selectionchange",trickQueryList);




            fieldControl.DictName = fieldValue.DictName;
        }
        else if (fieldValue.FieldType == 8)
        {   
            var lastTimes = fieldValue.ShowValue.split(" ");
            
            if(entity.realtime == "" || entity.realtime.indexOf("R") ==0){
                var startDate = convertDate(lastTimes[0]);
                var backDay = -1;
                //对于实时数据可以通过设置T_ImageProduct表中的Period来控制往后的天数
                if(entity.realtime.indexOf("R") ==0)
                    backDay = -parseInt(entity.realtime.substr(2));
                startDate = startDate.add(Date.DAY,backDay);
                var hour = lastTimes[1].substring(0,2);
                var cmboStartDate = new HourComboBox(hour,'hour' + fieldValue.Name + 'startDate');
                cmboStartDate.addListener("select",trickQueryList); 
                var cmboEndDate = new HourComboBox(hour,'hour' + fieldValue.Name + 'endDate');
                cmboEndDate.addListener("select",trickQueryList);
                
                fieldControl = [new Ext.form.CompositeField({
                    fieldLabel: '开始',
                    height: 22,

                    defaults: {
                        flex: 1
                    },
                    items: [{
                            xtype     : 'datefield',
                            id      : fieldValue.Name + 'startDate',
                            format: 'Y-m-d',
                            value:startDate.format('Y-m-d'),
                            enableKeyEvents : true,
                            editable : false,
                            width:       120,
                            listeners:{"select":trickQueryList}
                        },
                        cmboStartDate
                    ]
                }),
                new Ext.form.CompositeField({
                    fieldLabel: '结束',
                    height: 22,
                    defaults: {
                        flex: 1
                    },
                    items: [{
                            xtype     : 'datefield',
                            id      : fieldValue.Name + 'endDate',
                            format: 'Y-m-d',
                            value:lastTimes[0],
                            enableKeyEvents : true,
                            editable : false,
                            width:       120,
                            listeners:{"select":trickQueryList}
                        },
                        cmboEndDate
                    ]
                })
                ];
            }
            else{
                var periods = entity.realtime.split(",");
                var periodData = new Array;
                var lastHour = lastTimes[1].split(":");
                
                
                for(var i=0;i<periods.length;i++)
                {
                    periodData[i] = [parseInt(periods[i],10),periods[i]];
                }
                var hourComboBox = new HourComboBox(parseInt(lastHour[0],10),'hour' + fieldValue.Name + 'startDate',periodData );
                hourComboBox.addListener("select",trickQueryList);

                fieldControl = new Ext.form.CompositeField({
                    fieldLabel: fieldValue.Alias,
                    height: 22,
                    defaults: {
                        flex: 1
                    },
                    items: [{
                            xtype     : 'datefield',
                            id      : fieldValue.Name + 'startDate',
                            format: 'Y-m-d',
                            value:  lastTimes[0],
                            enableKeyEvents : true,
                            editable : false,
                            width:       120,
                            listeners:{"select":trickQueryList}
                        },
                        hourComboBox
                    ]
                });
            }
            
            
            
            
        }
        return fieldControl;
    }

}

//继承，添加函数，或者重写函数
Ext.extend(ImageFrame, Ext.Panel, {
// destroy: function() {  
//        alert("ok");   
//    }
   
});
//注册，可以通过xtype:imageViewer来创建面板
Ext.reg('imageFrame', ImageFrame); 

