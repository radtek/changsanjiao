// 图片框架，根据产品名称调取图片
var startTime;
var endDateTime;

ImageFrame = function (fieldInfo, entity, json) {

    Ext.TaskMgr.stopAll();
    var entityName = fieldInfo[0].EntityName;
    var lstHeight = 0;
    var labelAlign = "top";

    supportInnerText(); //使得火狐支持innerText



    //创建查询窗体
    var queryPanel = new Ext.FormPanel({
        id: 'eastPanel',
        //        width: 210,
        labelAlign: labelAlign,
        region: 'east',
        border: false,
        collapseMode: 'mini',
        collapsed: false,
        bodyStyle: 'padding: 5px;'
        //        defaults: { width: 195 }

    });

    //获取字典数，用于决定列表框的高度
    var dicCount = 0;
    var fieldValue;
    var dicHeight = 80;
    for (var i = 0; i < fieldInfo.length; i++) {
        fieldValue = fieldInfo[i];
        if (fieldValue.FieldType == 4)
            dicCount = dicCount + 1;
    }

    if (dicCount == 1)
        dicHeight = 180;

    //加载查询内容
    fieldValue = fieldInfo[0];
    var fieldControl = createControl(fieldValue, dicHeight);
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
        id: entityName,
        store: {},
        border: false,
        fieldLabel: '列表',
        hideLabel: true,
        ctCls: 'listBox',
        height: 600,
        loadingText: '载入中...',
        singleSelect: true,
        selectedClass: 'listBox-selected',
        hideHeaders: true,
        anchor: '99% ' + (-60 - lstHeight),
        listeners: {
            selectionchange: selectChange, afterrender:
                        function () {
                            queryList(entityName, false);
                        }
        },
        columns: [{ header: 'MC', width: 1, align: 'center', dataIndex: 'MC'}]
    });
    queryPanel.add(fieldControl);
    AddKeyEvent(entityName);
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
    function queryList(listID, isInitial) {
        var fieldValue;
        var endTime;
        for (i = 0; i < fieldInfo.length; i++) {
            fieldValue = fieldInfo[i];
            if (fieldValue.FieldType == 8) {
                //开始时间
                var startDateCmp = Ext.getCmp(fieldValue.Name + 'startDate');
                var startHourCmp = Ext.getCmp('hour' + startDateCmp.getId());
                var startDate = startDateCmp.getValue().add(Date.HOUR, startHourCmp.getValue());
                startTime = startDate;
                fieldValue.ShowValue = startDate.format('Y-m-d H:i:s');
                //时效存在设置往后多少日期的情况
                if (entity.realtime == "" || entity.realtime.indexOf("R") == 0) {
                    //结束时间
                    var endDateCmp = Ext.getCmp(fieldValue.Name + 'endDate');
                    var endHourCmp = Ext.getCmp('hour' + endDateCmp.getId());
                    //var endDate = endDateCmp.getValue().add(Date.HOUR, endHourCmp.getValue()).add(Date.MINUTE,59).add(Date.SECOND,59);
                    //var endDate = endDateCmp.getValue().add(Date.HOUR, endHourCmp.getValue());
                    endDateTime = endDate;
                    fieldValue.ShowValue = fieldValue.ShowValue + '||' + endDate.format('Y-m-d H:i:s');
                    endTime = fieldValue.Name + " = '" + endDate.format('Y-m-d H:i:s') + "'";
                    if (endDateTime < startTime)
                        return;
                }

            }

        }
        var entityObj = "";
        if (isInitial == false)
            entityObj = Ext.util.JSON.encode(fieldInfo);
        var fieldControl = Ext.getCmp(listID);
        var store = new Ext.data.JsonStore({
            url: getUrl('MMShareBLL.DAL.Forecast', 'QueryTimeList'),
            baseParams: { entityName: entityName, entityObj: entityObj, json: json },
            autoLoad: true,
            fields: ['MC', 'DM']
        });
        store.on('load', function () {
            var nodes = fieldControl.getNodes();
            fieldControl.select(nodes.length - 1);
            var triggerValue = store.getAt(nodes.length - 1).get("DM");
            //            var triggerValue = store.getAt(0).get("MC");
        });
        fieldControl.bindStore(store);
    }

    function selectChange(view, nodes) {
        view.focus();
        var store = view.getStore();
        var nodes = view.getNodes();
        var selectionsArray = view.getSelectedIndexes();
        var triggerValue = store.getAt(selectionsArray[0]).get("MC");
        var period = store.getAt(selectionsArray[0]).get("DM");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.Forecast', 'trickQueryEastList'),
            params: { Datetime: triggerValue, entityName: entityName, json: json, period: period },
            success: function (response) {
                if (response.responseText != "") {
                    var contentNone = Ext.getDom("imgHtml");
                    contentNone.innerHTML = response.responseText;
                    try {
                        var startDateCmp = Ext.getCmp(fieldValue.Name + 'startDate');
                        doQueryChart(startDateCmp.getValue().format('Y-m-d H:i:s'), triggerValue);
                   } catch (Exception) { }
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
    function trickQueryList() { queryList(entityName, false); }

    function createControl(fieldValue, dicHeight) {
        var fieldControl;
        if (fieldValue.FieldType == 8) {
            var lastTimes = fieldValue.ShowValue.split(" ");

            if (entity.realtime == "" || entity.realtime.indexOf("R") == 0) {
                var startDate = convertDate(lastTimes[0]);
                var backDay = -1;
                //对于实时数据可以通过设置T_ImageProduct表中的Period来控制往后的天数
                if (entity.realtime.indexOf("R") == 0)
                    backDay = -parseInt(entity.realtime.substr(2));
                startDate = startDate.add(Date.DAY, backDay);
                var hour = lastTimes[1].substring(0, 2);
                var cmboStartDate = new HourComboBox(hour, 'hour' + fieldValue.Name + 'startDate');
                cmboStartDate.addListener("select", trickQueryList);
                var cmboEndDate = new HourComboBox(hour, 'hour' + fieldValue.Name + 'endDate');
                cmboEndDate.addListener("select", trickQueryList);

                fieldControl = [new Ext.form.CompositeField({
                    fieldLabel: '开始',
                    labelSeparator: "",
                    height: 22,

                    defaults: {
                        flex: 1
                    },
                    items: [{
                        xtype: 'datefield',
                        id: fieldValue.Name + 'startDate',
                        format: 'Y-m-d',
                        value: startDate.format('Y-m-d'),
                        enableKeyEvents: true,
                        editable: false,
                        width: 120,
                        listeners: { "select": trickQueryList }
                    },
                        cmboStartDate
                    ]
                }),
                new Ext.form.CompositeField({
                    fieldLabel: '结束',
                    labelSeparator: "",
                    height: 22,
                    defaults: {
                        flex: 1
                    },
                    items: [{
                        xtype: 'datefield',
                        id: fieldValue.Name + 'endDate',
                        format: 'Y-m-d',
                        value: lastTimes[0],
                        enableKeyEvents: true,
                        editable: false,
                        width: 120,
                        listeners: { "select": trickQueryList }
                    },
                        cmboEndDate
                    ]
                })
                ];
            }
            else {
                var periods = entity.realtime.split(",");
                var periodData = new Array;
                var lastHour = lastTimes[1].split(":");


                for (var i = 0; i < periods.length; i++) {
                    periodData[i] = [parseInt(periods[i], 10), periods[i]];
                }
                var hourComboBox = new HourComboBox(parseInt(lastHour[0], 10), 'hour' + fieldValue.Name + 'startDate', periodData);
                hourComboBox.addListener("select", trickQueryList);

                fieldControl = new Ext.form.CompositeField({
                    fieldLabel: fieldValue.Alias,
                    labelSeparator: "",
                    height: 22,
                    defaults: {
                        flex: 1
                    },
                    items: [{
                        xtype: 'datefield',
                        id: fieldValue.Name + 'startDate',
                        format: 'Y-m-d',
                        value: lastTimes[0],

                        enableKeyEvents: true,
                        editable: false,
                        width: 120,
                        listeners: { "select": trickQueryList }
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

});
//注册，可以通过xtype:imageViewer来创建面板
Ext.reg('imageFrame', ImageFrame);

