// 图片框架，根据产品名称调取图片
var startTime;
var endDateTime;

ImageFrame = function (lastTime,layers,nowType,period) {

//    var entityName = fieldInfo[0].EntityName;
    var lstHeight = 0;
    var labelAlign = "top";

    supportInnerText(); //使得火狐支持innerText



    //创建查询窗体
    var queryPanel = new Ext.FormPanel({
        id: 'eastPanel',
        width: 210,
        labelAlign: labelAlign,
        border: false,
        collapseMode: 'mini',
        collapsed: false,
        bodyStyle: 'padding: 5px;',
        defaults: { width: 195 }

    });

    //获取字典数，用于决定列表框的高度
    var dicCount = 1;
    var fieldValue;
    var dicHeight = 80;
//    if (dicCount == 1)
//        dicHeight = 180;


    var fieldControl = createControl();
    if (Ext.isDefined(fieldControl)) {
        queryPanel.add(fieldControl);
        if (Ext.isArray(fieldControl))
            for (j = 0; j < fieldControl.length; j++)
                lstHeight = lstHeight + fieldControl[j].height + 23;
        else
            lstHeight = lstHeight + fieldControl.height + 23;
    }


    //创建具有数据的listview   
    fieldControl = new Ext.ListView({
        id: "list",
        store: {},
        border: false,
        fieldLabel: '列表',
        hideLabel: true,
        ctCls: 'listBox',
        height: 440,
        loadingText: '载入中...',
        singleSelect: true,
        selectedClass: 'listBox-selected',
        hideHeaders: true,
//        anchor: '99% ' + (-60 - lstHeight),
        listeners: {
            selectionchange: selectChange, afterrender:
                        function () {
                            queryList();
                        }
        },
        columns: [{ header: 'MC', width: 1, align: 'center', dataIndex: 'MC'}]
    });
    queryPanel.add(fieldControl);
//    AddKeyEvent(entityName);
    //定义动画任务
    var playTask = {
        run: function () {
            play(entityName);
        },
        interval: 700 //700 毫秒
    }
    ImageFrame.superclass.constructor.call(this, {
        border: false,
        items: [queryPanel]
    });
    //*************************************************************************************************
    //  下面开始为此类的模块级别函数
    //*************************************************************************************************

    //增加键盘左右键
    function AddKeyEvent(viewID) {
        var el = Ext.getDoc();
        var component = Ext.getCmp(viewID);
        new Ext.KeyNav(el, {
            forceKeyDown: true,
            left: function (e) {
                MoveSelection(component, -1);
            },
            up: function (e) {
                MoveSelection(component, -1);
            },
            right: function (e) {
                MoveSelection(component, 1);
            },
            down: function (e) {
                MoveSelection(component, 1);
            },
            scope: el
        }
        );

    }

    //点击查询以后罗列出查询列表
    function queryList() {
                //开始时间
        var startDateCmp = Ext.getCmp('startDate');
        var startHourCmp = Ext.getCmp('hourstartDate');
        var startDate = startDateCmp.getValue().add(Date.HOUR, startHourCmp.getValue());
        startTime = startDate.format('Y-m-d H:i:s');
//        fieldValue.ShowValue = startDate.format('Y-m-d H:i:s');

        var fieldControl = Ext.getCmp("list");
        var store = new Ext.data.JsonStore({
            url: getUrl('MMShareBLL.DAL.FreeQuery', 'QueryModuleFree'),
            baseParams: { lastTime: startTime, layer: layers, type: nowType },
            autoLoad: true,
            fields: ['MC']
        });
        store.on('load', function () {
            var nodes = fieldControl.getNodes();
            fieldControl.select(nodes.length - 1);
//            var triggerValue = store.getAt(nodes.length - 1).get("MC");
        });
        fieldControl.bindStore(store);
    }

    function trickQueryList() { queryList(); }
    //改变日期成功后,，刷新获取的值
    function changeDateSucessed(result) {
        for (var obj in result) {
            divContaner = Ext.getDom(obj);
            if (divContaner != null) {
                if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                    divContaner.value = result[obj];
                else {
                    if (result[obj] == "")
                        divContaner.innerHTML = "\\"; //日平均值
                    else
                        divContaner.innerHTML = result[obj]; //日平均值


                }
            }
        }
    }
    function createControl() {
        var fieldControl;
        var periods = period.split(',');
        var periodData = new Array;
        for (var i = 0; i < periods.length; i++) {
            periodData[i] = [parseInt(periods[i], 10), periods[i]];
        }
        var hourComboBox = new HourComboBox(parseInt(periodData[0], 10), 'hourstartDate', periodData);
        hourComboBox.addListener("select", trickQueryList);

        fieldControl = new Ext.form.CompositeField({
            fieldLabel: "起报时间",
            labelSeparator: "",
            height: 22,
            defaults: {
                flex: 1
            },
            items: [{
                xtype: 'datefield',
                id:'startDate',
                format: 'Y-m-d',
                value:lastTime,

                enableKeyEvents: true,
                editable: false,
                width: 120,
                listeners: { "select": trickQueryList }
            },
                hourComboBox
            ]
        });

        return fieldControl;
    }

}
//继承，添加函数，或者重写函数
Ext.extend(ImageFrame, Ext.Panel, {

});
//注册，可以通过xtype:imageViewer来创建面板
Ext.reg('imageFrame', ImageFrame);

