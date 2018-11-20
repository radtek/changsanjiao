// JScript 文件

FlexGrid = function(recordset){

    // 分页大小
    var pageSize = 10;
	// 字段
	var fields = ["ForecastDay","ConditionsAM","ConditionsPM","ConditionsNight",
	            "HighTemperature","LowTemperature","Direction","Speed"];
	// 记录
	var record = new Ext.data.Record.create([{
				name : "ForecastDay"
			}, {
				name : "ConditionsAM"
			}, {
				name : "ConditionsPM"
			}, {
				name : "ConditionsNight"
			}, {
				name : "HighTemperature"
			}, {
				name : "LowTemperature"
			}, {
				name : "Direction"
			}, {
			    name : "Speed"
			}]);
	// 数据
	cardStore = new Ext.data.Store({
				// 代理
				proxy : new Ext.data.HttpProxy({
							url : getUrl('MMShareBLL.DAL.Forecast','GetForecast'),
							method : "POST"
						}),
				// 解析器
				reader : new Ext.data.JsonReader({
							fields : fields,
							root : "data",
							id : "ForecastDay",
							totalProperty : "totalCount"
						}, record)
			});
	// 数据加载时分页
	cardStore.load({
				params : {
					name : recordset
				}
			});


	// 选择模式
	var sm = new Ext.grid.CheckboxSelectionModel({
				dataIndex : "ForecastDay"
			});
	// 列头
	var cm = new Ext.grid.ColumnModel([new Ext.grid.RowNumberer(),{
				header : "日期",
				tooltip : "日期",
				dataIndex : "ForecastDay",
				sortable : true
			}, {
				header : "上午",
				tooltip : "上午",
				dataIndex : "ConditionsAM",
				sortable : true
			}, {
				header : "下午",
				tooltip : "下午",
				dataIndex : "ConditionsPM",
				sortable : true
			}, {
				header : "晚上",
				tooltip : "晚上",
				dataIndex : "ConditionsNight",
				sortable : true
			}, {
				header : "最高温度",
				tooltip : "最高温度",
				dataIndex : "HighTemperature",
				sortable : true
			}, {
				header : "最低温度",
				tooltip : "最低温度",
				dataIndex : "LowTemperature",
				sortable : true
			}, {
				header : "风向",
				tooltip : "风向",
				dataIndex : "Direction",
				sortable : true
			}, {
				header : "风速",
				tooltip : "风速",
				dataIndex : "Speed",
				sortable : true
			}]);


	// 表格
	 FlexGrid.superclass.constructor.call(this,{
				store : cardStore,
				sm : sm,
				cm : cm,
				loadMask : true,
				autoScroll : true,
				border : false,
				layout : "fit",
				viewConfig : {
					columnsText : "显示/隐藏列",
					sortAscText : "正序排列",
					sortDescText : "倒序排列",
					forceFit : true
				}

			});

}

//继承，添加函数，或者重写函数
Ext.extend(FlexGrid,Ext.grid.GridPanel, {
   
});
//注册，可以通过xtype:flexGrid来创建此表格
Ext.reg('flexGrid', FlexGrid); 