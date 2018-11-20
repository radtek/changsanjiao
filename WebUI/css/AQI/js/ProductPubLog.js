//发布的产品名称
var productName = "";
//发布方式
var releaseMethod = "";
//查询起始时间
var pubDateSearStart = "";
//查询结束时间
var pubDateSearEnd = "";
//发布状态
var pubCondition = "";
//发布人
var pubUser = "";
//var journal_grid;
//var colModel;
//var barPagingBar;
Ext.onReady(function () {
    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#tableOutLine").width($(window).width() - 30);
    //$("#tableOutLine").height($(window).height() - 70);
    //$("body").css("min-width", $(window).width() + "px");

    $("#txtPublishStartDate_Search").val(GetDateStrNoYearNoCn(-3));
    $("#txtPublishEndDate_Search").val(GetDateStrNoYearNoCn(0));


    LoadPubLogData();
    //查询按钮
    $("#btnSearch").click(function () {
        LoadPubLogDataNew();
    });

    document.body.onclick = function (e) {
        if (e.target.className != "dateSelect" && e.target.className != "firstPolUl" && e.target.className != "firstPolText" && e.target.className != "selIcon" && e.target.className != "dateDiv") {
            $(".firstPolUl").hide();
        }

        if (e.target.className != "hazeLevelSelect" && e.target.className != "hazeDiv" && e.target.className != "hazeLevelText" && e.target.className != "selIcon" && e.target.className != "hazeLevelUl") {
            $(".hazeLevelUl").hide();
        }
    }


    //绑定首要污染物选择下拉菜单的事件
    $.each($(".dateDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".firstPolUl")[i]).is(":hidden")) {
                //                $($(".firstPolUl")[i]).addClass("display");
                //                $($(".firstPolUl")[i]).removeClass("hide");
                $($(".firstPolUl")[i]).show();
            }
            else {
                //                $($(".firstPolUl")[i]).addClass("hide");
                //                $($(".firstPolUl")[i]).removeClass("display");
                $($(".firstPolUl")[i]).hide();
            }
        });
    });

    $.each($($(".proNameSelect .firstPolUl")[0]).find("li"), function (j, m) {
        $(m).click(function () {
            var proName = $($(m).find("div")[0]).html();
            $($(".proNameText")[0]).html($($(m).find("div")[0]).html());
            $($(".proNameSelect .firstPolUl")[0]).addClass("hide");
            $($(".proNameSelect .firstPolUl")[0]).removeClass("display");
            LoadPubLogDataNew();
        })
    });

    $.each($($(".pubMethodSelect .firstPolUl")[0]).find("li"), function (j, m) {
        $(m).click(function () {
            var proMethod = $($(m).find("div")[0]).html();
            $($(".pubMethodText")[0]).html($($(m).find("div")[0]).html());
            $($(".pubMethodSelect .firstPolUl")[0]).addClass("hide");
            $($(".pubMethodSelect .firstPolUl")[0]).removeClass("display");
            LoadPubLogDataNew();
        })
    });

    $.each($($(".pubConSelect .firstPolUl")[0]).find("li"), function (j, m) {
        $(m).click(function () {
            var pubCon = $($(m).find("div")[0]).html();
            $($(".pubConText")[0]).html($($(m).find("div")[0]).html());
            $($(".pubConSelect .firstPolUl")[0]).addClass("hide");
            $($(".pubConSelect .firstPolUl")[0]).removeClass("display");
            LoadPubLogDataNew();
        })
    });
    //    var store = new Ext.data.Store({
    //        reader: new Ext.data.JsonReader(),
    //        autoLoad: { params: { start: 0, limit: 15} },

    //        proxy: new Ext.data.HttpProxy({
    //            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogData')
    //        })
    //    });


    //    $(".firstPolUl").niceScroll({
    //        cursorcolor: "#667A6D",
    //        cursoropacitymax: 1,
    //        touchbehavior: false,
    //        cursorwidth: "5px",
    //        cursorborder: "0",
    //        cursorborderradius: "5px",
    //        background: "#EDEDED"
    //    });
    //    var expander = new Ext.grid.RowExpander({
    //});

    //var sm = new Ext.grid.CheckboxSelectionModel({
    //    dataIndex: "roleId",
    //    width: 28
    //});
    //var previewExpand = new Ext.grid.CheckboxSelectionModel({
    //    dataIndex: "roleId",
    //    width: 28
    //});
    //var index = new Ext.grid.RowNumberer(); //行号   
    ////--------------------------------------------------列头   
    //colModel = new Ext.grid.ColumnModel
    //        (
    //            [
    //                index,
    //                expander,
    ////sm,
    //                {dataIndex: 'ProductType', width: 30, sortable: false, renderer: function (v) {
    //                    return '<input type="checkbox" />';
    //                }
    //            },
    //                { header: "产品类别", width: 150, dataIndex: 'ProductType', sortable: false },
    //                { header: "产品名称", width: 150, dataIndex: 'ProductName', sortable: false, renderer: function (value, meta, record) {
    //                    meta.attr = 'style="white-space:normal;word-wrap: break-word;"';
    //                    return value
    //                }
    //                },
    //                { header: "发布方式", width: 65, dataIndex: 'ReleaseType', sortable: false },
    //                { header: "发布开始时间", width: 130, dataIndex: 'StartTime', sortable: false },
    //                { header: "发布结束时间", width: 130, dataIndex: 'EndTime', sortable: false },
    //                { header: "发布状态", width: 80, dataIndex: 'State', sortable: false, renderer: function (v) {
    //                    switch (v) {
    //                        case "0":
    //                            //return "发布成功";
    //                            return "<div class='successIcon'></div>";
    //                        case "1":
    //                            return "<div class='failIcon'></div>";
    //                        default:
    //                            //return "发布失败";
    //                    }
    //                }
    //                },
    //                { header: "发布地址 ", width: 340, dataIndex: 'Address', sortable: false, renderer: function (value, meta, record) {
    //                    meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left;"';
    //                    return value
    //                }
    //                },
    //                { header: "发布人", width: 60, dataIndex: 'User', sortable: false },
    //                { header: "发布机器IP ", width: 100, dataIndex: 'IPAddress', sortable: false },
    //                { header: "发布描述", width: 200, dataIndex: 'Detail', sortable: false, renderer: function (v) {
    //                    //                    return '<div style="word-wrap:break-word;word-break: break-all;overflow:visible;" mce_style="word-wrap:break-word;word-break: break-all;overflow:visible;">' + v + '</div>';
    //                    return '<div>' + v + '</div>';
    //                }
    //                }

    //            ]
    //        );

    //function createButton() {
    //    return "<input type='button' value='展开'>"
    //}

    //var viewConfig = {
    //    templates: {
    //        //        hcell: new Ext.Template(
    //        //                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
    //        //                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
    //        //                            '</div></td>'),
    //        hcell: new Ext.Template(
    //                    '<td class="x-grid3-hd x-grid3-cell tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
    //                    '{value}',
    //                            '</div></td>'),
    //        cell: new Ext.Template(
    //                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaContent" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="istyle">', '<a class="x-grid3-hd-btn" href="#"></a>',
    //                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
    //                    '</div></td>')
    //    }

    //};

    //barPagingBar = new Ext.PagingToolbar({
    //    store: store,           //数据源   
    //    pageSize: 15,
    //    //显示右下角信息   
    //    displayInfo: true,
    //    displayMsg: '当前记录 {0} -- {1} 条 共 {2} 条记录',
    //    emptyMsg: "No results to display",
    //    prevText: "上一页",
    //    nextText: "下一页",
    //    refreshText: "刷新",
    //    lastText: "最后页",
    //    firstText: "第一页",
    //    beforePageText: "当前页",
    //    afterPageText: "共{0}页",
    //    style: {
    //        backgroundColor: '#ECECEC'
    //    }
    //});

    //journal_grid = new Ext.grid.GridPanel({
    //    id: 'idProductLog',                     //grid的id  
    //    //autoHeight: true,
    //    autoWidth: true,
    //    //width: 1300,
    //    height: 750,
    //    sm: sm,
    //    cm: colModel, //行列
    //    loadMask: { msg: '正在加载数据...' },
    //    plugins: expander,
    //    store: store, //数据源
    //    renderTo: "logTable",
    //    trackMouseOver: true, //鼠标特效
    //    autoScroll: true,
    //    cls:"backgroundColor:#ebebeb",
    //    //stripeRows: true,
    //    renderer: function (value, meta, record) {
    //        //meta.attr = 'style="white-space:normal;word-wrap: break-word;overflow:visible;"';
    //        return value
    //    },
    //    viewConfig: viewConfig,
    //    bbar: barPagingBar
    //});
    //journal_grid.setAutoScroll(true);

    ////查询按钮
    //$("#btnSearch").click(function () {
    //    store = new Ext.data.Store({
    //        reader: new Ext.data.JsonReader(),
    //        autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //        proxy: new Ext.data.HttpProxy({
    //            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //        })
    //    });
    //    journal_grid.reconfigure(store, colModel);
    //    barPagingBar.bindStore(store, true);
    //    journal_grid.bbar = barPagingBar;
    //    barPagingBar.updateInfo();
    //    //    barPagingBar.moveFirst();
    //    barPagingBar.doRefresh();
    //    barPagingBar.doLayout();
    //});

    //$.each($($(".proNameSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //    $(m).click(function () {
    //        var proName = $($(m).find("div")[0]).html();
    //        $($(".proNameText")[0]).html($($(m).find("div")[0]).html());
    //        $($(".proNameSelect .firstPolUl")[0]).addClass("hide");
    //        $($(".proNameSelect .firstPolUl")[0]).removeClass("display");
    //        store = new Ext.data.Store({
    //            reader: new Ext.data.JsonReader(),
    //            autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //            proxy: new Ext.data.HttpProxy({
    //                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //            })
    //        });
    //        barPagingBar.bindStore(store, true);
    //        journal_grid.reconfigure(store, colModel);
    //        journal_grid.bbar = barPagingBar;
    //        barPagingBar.updateInfo();
    //        //        barPagingBar.moveFirst();
    //        barPagingBar.doRefresh();
    //        barPagingBar.doLayout();
    //    })
    //});

    //$.each($($(".pubMethodSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //    $(m).click(function () {
    //        var proMethod = $($(m).find("div")[0]).html();
    //        $($(".pubMethodText")[0]).html($($(m).find("div")[0]).html());
    //        $($(".pubMethodSelect .firstPolUl")[0]).addClass("hide");
    //        $($(".pubMethodSelect .firstPolUl")[0]).removeClass("display");
    //        store = new Ext.data.Store({
    //            reader: new Ext.data.JsonReader(),
    //            autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },
    //            proxy: new Ext.data.HttpProxy({
    //                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //            })
    //        });
    //        journal_grid.reconfigure(store, colModel);
    //        barPagingBar.bindStore(store, true);
    //        journal_grid.bbar = barPagingBar;
    //        barPagingBar.updateInfo();
    //        //        barPagingBar.moveFirst();
    //        barPagingBar.doRefresh();
    //        barPagingBar.doLayout();
    //    })
    //});

    //$.each($($(".pubConSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //    $(m).click(function () {
    //        var pubCon = $($(m).find("div")[0]).html();
    //        $($(".pubConText")[0]).html($($(m).find("div")[0]).html());
    //        $($(".pubConSelect .firstPolUl")[0]).addClass("hide");
    //        $($(".pubConSelect .firstPolUl")[0]).removeClass("display");
    //        store = new Ext.data.Store({
    //            reader: new Ext.data.JsonReader(),
    //            autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //            proxy: new Ext.data.HttpProxy({
    //                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //            })
    //        });
    //        journal_grid.reconfigure(store, colModel);
    //        barPagingBar.bindStore(store, true);
    //        journal_grid.bbar = barPagingBar;
    //        barPagingBar.updateInfo();
    //        //        barPagingBar.moveFirst();
    //        barPagingBar.doRefresh();
    //        barPagingBar.doLayout();
    //    })
    //});

});

//读取发布日志的数据
function LoadPubLogData() {
    var store = new Ext.data.Store({
        reader: new Ext.data.JsonReader(),
        autoLoad: { params: { start: 0, limit: 15} },

        proxy: new Ext.data.HttpProxy({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogData')
        })
    });
    $(".firstPolUl").niceScroll({
        cursorcolor: "#667A6D",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        background: "#EDEDED"
    });
    var expander = new Ext.grid.RowExpander({
});

var sm = new Ext.grid.CheckboxSelectionModel({
    dataIndex: "roleId",
    width: 28
});
var previewExpand = new Ext.grid.CheckboxSelectionModel({
    dataIndex: "roleId",
    width: 28
});
var index = new Ext.grid.RowNumberer(); //行号   
//--------------------------------------------------列头   
var colModel = new Ext.grid.ColumnModel
        (
            [
                index,
                expander,
//sm,
                {dataIndex: 'ProductType', width: 30, sortable: false, renderer: function (v) {
                    return '<input type="checkbox" />';
                }
            },
                { header: "产品类别", width: 150, dataIndex: 'ProductType', sortable: false },
                { header: "产品名称", width: 150, dataIndex: 'ProductName', sortable: false, renderer: function (value, meta, record) {
                    meta.attr = 'style="white-space:normal;word-wrap: break-word;"';
                    return value
                }
                },
                { header: "发布方式", width: 65, dataIndex: 'ReleaseType', sortable: false },
                { header: "发布开始时间", width: 130, dataIndex: 'StartTime', sortable: false },
                { header: "发布结束时间", width: 130, dataIndex: 'EndTime', sortable: false },
                { header: "发布状态", width: 80, dataIndex: 'State', sortable: false, renderer: function (v) {
                    switch (v) {
                        case "0":
                            //return "发布成功";
                            return "<div class='successIcon'></div>";
                        case "1":
                            return "<div class='failIcon'></div>";
                        default:
                            //return "发布失败";
                    }
                }
                },
                { header: "发布地址 ", width: 340, dataIndex: 'Address', sortable: false, renderer: function (value, meta, record) {
                    meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left;"';
                    return value
                }
                },
                { header: "发布人", width: 60, dataIndex: 'User', sortable: false },
                { header: "发布机器IP ", width: 100, dataIndex: 'IPAddress', sortable: false },
                { header: "发布描述", width: 200, dataIndex: 'Detail', sortable: false, renderer: function (v) {
                    //                    return '<div style="word-wrap:break-word;word-break: break-all;overflow:visible;" mce_style="word-wrap:break-word;word-break: break-all;overflow:visible;">' + v + '</div>';
                    return '<div>' + v + '</div>';
                }
                }

            ]
        );

function createButton() {
    return "<input type='button' value='展开'>"
}

var viewConfig = {
    templates: {
        //        hcell: new Ext.Template(
        //                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
        //                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
        //                            '</div></td>'),
        hcell: new Ext.Template(
                    '<td class="x-grid3-hd x-grid3-cell tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
                    '{value}',
                            '</div></td>'),
        cell: new Ext.Template(
                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaContent" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="istyle">', '<a class="x-grid3-hd-btn" href="#"></a>',
                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
                    '</div></td>')
    }

};

var barPagingBar = new Ext.PagingToolbar({
    store: store,           //数据源   
    pageSize: 15,
    //显示右下角信息   
    displayInfo: true,
    displayMsg: '当前记录 {0} -- {1} 条 共 {2} 条记录',
    emptyMsg: "No results to display",
    prevText: "上一页",
    nextText: "下一页",
    refreshText: "刷新",
    lastText: "最后页",
    firstText: "第一页",
    beforePageText: "当前页",
    afterPageText: "共{0}页",
    style: {
        backgroundColor: '#ECECEC'
    }
});

var journal_grid = new Ext.grid.GridPanel({
    id: 'idProductLog',                     //grid的id  
    //autoHeight: true,
    autoWidth: true,
    //width: 1300,
    height: 750,
    sm: sm,
    cm: colModel, //行列
    loadMask: { msg: '正在加载数据...' },
    plugins: expander,
    store: store, //数据源
    renderTo: "logTable",
    trackMouseOver: true, //鼠标特效
    autoScroll: true,
    cls: "backgroundColor:#ebebeb",
    //stripeRows: true,
    renderer: function (value, meta, record) {
        //meta.attr = 'style="white-space:normal;word-wrap: break-word;overflow:visible;"';
        return value
    },
    viewConfig: viewConfig,
    bbar: barPagingBar
});
journal_grid.setAutoScroll(true);

}


//读取发布日志的数据

function LoadPubLogDataNew() {
    $('#logTable').html('');
    Ext.getCmp("idProductLog").destroy(); 
    var store = new Ext.data.Store({
        reader: new Ext.data.JsonReader(),
        autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },
        
        proxy: new Ext.data.HttpProxy({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
        })
//        autoLoad: { params: { start: 0, limit: 15} },

//        proxy: new Ext.data.HttpProxy({
//            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogData')
//        })
    });
    //store.load({ start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val() })
    store.on('beforeload', function (store, options){
                var params = {startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()};
    Ext.apply(options.params, params);
    });

    $(".firstPolUl").niceScroll({
        cursorcolor: "#667A6D",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        background: "#EDEDED"
    });
    var expander = new Ext.grid.RowExpander({
});

var sm = new Ext.grid.CheckboxSelectionModel({
    dataIndex: "roleId",
    width: 28
});
var previewExpand = new Ext.grid.CheckboxSelectionModel({
    dataIndex: "roleId",
    width: 28
});
var index = new Ext.grid.RowNumberer(); //行号   
//--------------------------------------------------列头   
var colModel = new Ext.grid.ColumnModel
        (
            [
                index,
                expander,
//sm,
                {dataIndex: 'ProductType', width: 30, sortable: false, renderer: function (v) {
                    return '<input type="checkbox" />';
                }
            },
                { header: "产品类别", width: 150, dataIndex: 'ProductType', sortable: false },
                { header: "产品名称", width: 150, dataIndex: 'ProductName', sortable: false, renderer: function (value, meta, record) {
                    meta.attr = 'style="white-space:normal;word-wrap: break-word;"';
                    return value
                }
                },
                { header: "发布方式", width: 65, dataIndex: 'ReleaseType', sortable: false },
                { header: "发布开始时间", width: 130, dataIndex: 'StartTime', sortable: false },
                { header: "发布结束时间", width: 130, dataIndex: 'EndTime', sortable: false },
                { header: "发布状态", width: 80, dataIndex: 'State', sortable: false, renderer: function (v) {
                    switch (v) {
                        case "0":
                            //return "发布成功";
                            return "<div class='successIcon'></div>";
                        case "1":
                            return "<div class='failIcon'></div>";
                        default:
                            //return "发布失败";
                    }
                }
                },
                { header: "发布地址 ", width: 340, dataIndex: 'Address', sortable: false, renderer: function (value, meta, record) {
                    meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left;"';
                    return value
                }
                },
                { header: "发布人", width: 60, dataIndex: 'User', sortable: false },
                { header: "发布机器IP ", width: 100, dataIndex: 'IPAddress', sortable: false },
                { header: "发布描述", width: 200, dataIndex: 'Detail', sortable: false, renderer: function (v) {
                    //                    return '<div style="word-wrap:break-word;word-break: break-all;overflow:visible;" mce_style="word-wrap:break-word;word-break: break-all;overflow:visible;">' + v + '</div>';
                    return '<div>' + v + '</div>';
                }
                }

            ]
        );

function createButton() {
    return "<input type='button' value='展开'>"
}

var viewConfig = {
    templates: {
        //        hcell: new Ext.Template(
        //                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
        //                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
        //                            '</div></td>'),
        hcell: new Ext.Template(
                    '<td class="x-grid3-hd x-grid3-cell tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
                    '{value}',
                            '</div></td>'),
        cell: new Ext.Template(
                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaContent" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="istyle">', '<a class="x-grid3-hd-btn" href="#"></a>',
                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
                    '</div></td>')
    }

};

var barPagingBar = new Ext.PagingToolbar({
    store: store,           //数据源   
    pageSize: 15,
    //显示右下角信息   
    displayInfo: true,
    displayMsg: '当前记录 {0} -- {1} 条 共 {2} 条记录',
    emptyMsg: "No results to display",
    prevText: "上一页",
    nextText: "下一页",
    refreshText: "刷新",
    lastText: "最后页",
    firstText: "第一页",
    beforePageText: "当前页",
    afterPageText: "共{0}页",
    style: {
        backgroundColor: '#ECECEC'
    }
});

var journal_grid = new Ext.grid.GridPanel({
    id: 'idProductLog',                     //grid的id  
    //autoHeight: true,
    autoWidth: true,
    //width: 1300,
    height: 750,
    sm: sm,
    cm: colModel, //行列
    loadMask: { msg: '正在加载数据...' },
    plugins: expander,
    store: store, //数据源
    renderTo: "logTable",
    trackMouseOver: true, //鼠标特效
    autoScroll: true,
    cls: "backgroundColor:#ebebeb",
    //stripeRows: true,
    renderer: function (value, meta, record) {
        //meta.attr = 'style="white-space:normal;word-wrap: break-word;overflow:visible;"';
        return value
    },
    viewConfig: viewConfig,
    bbar: barPagingBar
});
journal_grid.setAutoScroll(true);
}


