////发布的产品名称
//var productName = "";
////发布方式
//var releaseMethod = "";
////查询起始时间
//var pubDateSearStart = "";
////查询结束时间
//var pubDateSearEnd = "";
////发布状态
//var pubCondition = "";
////发布人
//var pubUser = "";
//var journal_grid;
//var colModel;
//var barPagingBar;
var userName = "";
Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    LoadImportNotice(userName);
    $("#addNew").click(function () {
        AddNewNotice(userName);
    });

    //删除选中的通知
    $("#delete").click(function () {
        DeleteSelectNotices();
    });
    //    //设置界面宽度
        var pageWidth = document.body.clientWidth;
        var pageHeight = document.documentElement.clientHeight;
        $("#tableOutLine").height(pageHeight - 70);
        //$("#tableOutLine").height($(window).height() - 70);
        //$("body").css("min-width", $(window).width() + "px");

    //    $("#txtPublishStartDate_Search").val(GetDateStrNoYearNoCn(-3));
    //    $("#txtPublishEndDate_Search").val(GetDateStrNoYearNoCn(0));

    //    //绑定首要污染物选择下拉菜单的事件
    //    $.each($(".dateDiv .selIcon"), function (i, n) {
    //        $(n).click(function () {
    //            if ($($(".firstPolUl")[i]).is(":hidden")) {
    //                $($(".firstPolUl")[i]).addClass("display");
    //                $($(".firstPolUl")[i]).removeClass("hide");
    //            }
    //            else {
    //                $($(".firstPolUl")[i]).addClass("hide");
    //                $($(".firstPolUl")[i]).removeClass("display");
    //            }
    //        });
    //    });


    //    var store = new Ext.data.Store({
    //        reader: new Ext.data.JsonReader(),
    //        autoLoad: { params: { start: 0, limit: 15} },

    //        proxy: new Ext.data.HttpProxy({
    //            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetImportantNoticeData')
    //        })
    //    });

    //    //    $.each($($(".proNameSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //    //        $(m).click(function () {
    //    //            var proName = $($(m).find("div")[0]).html();
    //    //            $($(".proNameText")[0]).html($($(m).find("div")[0]).html());
    //    //            $($(".proNameSelect .firstPolUl")[0]).addClass("hide");
    //    //            $($(".proNameSelect .firstPolUl")[0]).removeClass("display");
    //    //            store = new Ext.data.Store({
    //    //                reader: new Ext.data.JsonReader(),
    //    //                autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //    //                proxy: new Ext.data.HttpProxy({
    //    //                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //    //                })
    //    //            });
    //    //            journal_grid.reconfigure(store, colModel);

    //    //        })
    //    //    });

    //    //    $.each($($(".pubMethodSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //    //        $(m).click(function () {
    //    //            var proMethod = $($(m).find("div")[0]).html();
    //    //            $($(".pubMethodText")[0]).html($($(m).find("div")[0]).html());
    //    //            $($(".pubMethodSelect .firstPolUl")[0]).addClass("hide");
    //    //            $($(".pubMethodSelect .firstPolUl")[0]).removeClass("display");
    //    //            store = new Ext.data.Store({
    //    //                reader: new Ext.data.JsonReader(),
    //    //                autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },
    //    //                proxy: new Ext.data.HttpProxy({
    //    //                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //    //                })
    //    //            });
    //    //            journal_grid.reconfigure(store, colModel);
    //    //        })
    //    //    });

    //    //    $.each($($(".pubConSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //    //        $(m).click(function () {
    //    //            var pubCon = $($(m).find("div")[0]).html();
    //    //            $($(".pubConText")[0]).html($($(m).find("div")[0]).html());
    //    //            $($(".pubConSelect .firstPolUl")[0]).addClass("hide");
    //    //            $($(".pubConSelect .firstPolUl")[0]).removeClass("display");
    //    //            store = new Ext.data.Store({
    //    //                reader: new Ext.data.JsonReader(),
    //    //                autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //    //                proxy: new Ext.data.HttpProxy({
    //    //                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //    //                })
    //    //            });
    //    //            journal_grid.reconfigure(store, colModel);
    //    //        })
    //    //    });

    //    $(".firstPolUl").niceScroll({
    //        cursorcolor: "#667A6D",
    //        cursoropacitymax: 1,
    //        touchbehavior: false,
    //        cursorwidth: "5px",
    //        cursorborder: "0",
    //        cursorborderradius: "5px",
    //        background: "#EDEDED"
    //    });


    //    //store.load({ params: { start: 1, limit: 15} });  

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
    //                { dataIndex: 'ProductType', width: 30, sortable: false, renderer: function (v) {
    //                    return '<input type="checkbox" />';
    //                }
    //                },
    //                { header: "通知内容", width: 450, dataIndex: 'Content', sortable: false, renderer: function (value, meta, record) {
    //                    meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left;"';
    //                    return value
    //                }
    //                },
    //                { header: "通知类型", width: 150, dataIndex: 'Type', sortable: false },
    //                { header: "有效时段", width: 105, dataIndex: 'Period', sortable: false },
    //                { header: "通知开始时间", width: 130, dataIndex: 'StartTime', sortable: false },
    //                { header: "通知结束时间", width: 130, dataIndex: 'EndTime', sortable: false },
    //                { header: "是否停用", width: 80, dataIndex: 'IsDisable', sortable: false },
    //                { header: "操作", width: 80, dataIndex: 'ID', sortable: false, renderer: function (value, meta, record) {
    //                    //return '<div class="editBtn" onclick="EditNotice(' + value + ')">编辑</div>';
    //                    return '<div class="editBtn">编辑</div>';
    //                }
    //                }
    //            ]
    //        );

    //function createButton() {
    //    return "<input type='button' value='展开'>"
    //}


    ////var viewConfig = {
    ////    templates: {

    ////        //  在模板中引入自己定义的样式。使用这个view的grid的header的样式就修改了。   
    ////        header: new Ext.Template(
    ////                  ' <table border="0" cellspacing="0" cellpadding="0" style="{tstyle}">',
    ////                  ' <thead> <tr id="my-grid-head">{mergecells} </tr>' +
    ////                  ' <tr id="x-grid3-hd-row">{cells} </tr> </thead>',
    ////                  " </table>"
    ////                  ),
    ////        mhcell: new Ext.Template(
    ////                  ' <td id="myrow" colspan="{mcols}"> <div align="center"> <b>{value} </b> </div>',
    ////                  " </td>"
    ////                  )
    ////    }
    ////};
    //var viewConfig = {
    //    templates: {
    //        hcell: new Ext.Template(
    //                            '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
    //                            '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
    //                                    '</div></td>'),
    //        //        hcell: new Ext.Template(
    //        //                    '<td class="x-grid3-hd x-grid3-cell tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
    //        //                    '{value}',
    //        //                            '</div></td>'),
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
    //    height: 730,
    //    sm: sm,
    //    cm: colModel, //行列
    //    loadMask: { msg: '正在加载数据...' },
    //    plugins: expander,
    //    store: store, //数据源
    //    renderTo: "logTable",
    //    trackMouseOver: true, //鼠标特效
    //    autoScroll: true,
    //    //stripeRows: true,
    //    renderer: function (value, meta, record) {
    //        //meta.attr = 'style="white-space:normal;word-wrap: break-word;overflow:visible;"';
    //        return value
    //    },
    //    viewConfig: viewConfig,

    //    bbar: barPagingBar,
    //    afterrender: function () {
    //        //每条记录点击编辑后弹出界面
    //        $.each($(".editBtn"), function (i, n) {
    //            $(n).click(function () {
    //                alert("编辑");
    //            });
    //        });
    //    }
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
    //        journal_grid.reconfigure(store, colModel);
    //        barPagingBar.bindStore(store, true);
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

function LoadImportNotice(user) {
    //查询结束时间
    var pubDateSearEnd = "";
    //发布状态
    var pubCondition = "";
    //发布人
    var pubUser = "";
    var journal_grid;
    var colModel;
    var barPagingBar;

    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#tableOutLine").width($(window).width() - 30);
    //$("#tableOutLine").height($(window).height() - 70);
    //$("body").css("min-width", $(window).width() + "px");

    $("#txtPublishStartDate_Search").val(GetDateStrNoYearNoCn(-3));
    $("#txtPublishEndDate_Search").val(GetDateStrNoYearNoCn(0));

    //绑定首要污染物选择下拉菜单的事件
    $.each($(".dateDiv .selIcon"), function (i, n) {
        $(n).click(function () {
            if ($($(".firstPolUl")[i]).is(":hidden")) {
                $($(".firstPolUl")[i]).addClass("display");
                $($(".firstPolUl")[i]).removeClass("hide");
            }
            else {
                $($(".firstPolUl")[i]).addClass("hide");
                $($(".firstPolUl")[i]).removeClass("display");
            }
        });
    });


    var store = new Ext.data.Store({
        reader: new Ext.data.JsonReader(),
        autoLoad: { params: { start: 0, limit: 15} },

        proxy: new Ext.data.HttpProxy({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetImportantNoticeData')
        })
    });

    //    $.each($($(".proNameSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //        $(m).click(function () {
    //            var proName = $($(m).find("div")[0]).html();
    //            $($(".proNameText")[0]).html($($(m).find("div")[0]).html());
    //            $($(".proNameSelect .firstPolUl")[0]).addClass("hide");
    //            $($(".proNameSelect .firstPolUl")[0]).removeClass("display");
    //            store = new Ext.data.Store({
    //                reader: new Ext.data.JsonReader(),
    //                autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //                proxy: new Ext.data.HttpProxy({
    //                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //                })
    //            });
    //            journal_grid.reconfigure(store, colModel);

    //        })
    //    });

    //    $.each($($(".pubMethodSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //        $(m).click(function () {
    //            var proMethod = $($(m).find("div")[0]).html();
    //            $($(".pubMethodText")[0]).html($($(m).find("div")[0]).html());
    //            $($(".pubMethodSelect .firstPolUl")[0]).addClass("hide");
    //            $($(".pubMethodSelect .firstPolUl")[0]).removeClass("display");
    //            store = new Ext.data.Store({
    //                reader: new Ext.data.JsonReader(),
    //                autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },
    //                proxy: new Ext.data.HttpProxy({
    //                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //                })
    //            });
    //            journal_grid.reconfigure(store, colModel);
    //        })
    //    });

    //    $.each($($(".pubConSelect .firstPolUl")[0]).find("li"), function (j, m) {
    //        $(m).click(function () {
    //            var pubCon = $($(m).find("div")[0]).html();
    //            $($(".pubConText")[0]).html($($(m).find("div")[0]).html());
    //            $($(".pubConSelect .firstPolUl")[0]).addClass("hide");
    //            $($(".pubConSelect .firstPolUl")[0]).removeClass("display");
    //            store = new Ext.data.Store({
    //                reader: new Ext.data.JsonReader(),
    //                autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

    //                proxy: new Ext.data.HttpProxy({
    //                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
    //                })
    //            });
    //            journal_grid.reconfigure(store, colModel);
    //        })
    //    });

    $(".firstPolUl").niceScroll({
        cursorcolor: "#667A6D",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        background: "#EDEDED"
    });


    //store.load({ params: { start: 1, limit: 15} });  

    var expander = new Ext.grid.RowExpander({
});

var sm = new Ext.grid.CheckboxSelectionModel({
    dataIndex: "roleId",
    width: 28,
    listener: {
        rowselect: function (sm, row, rec) {
            alert(row + 1); //计算机计算是从0开始算第一行的，所以加1 补充一下  
            //  store.indexOf(rec); //这个是取该选中的rec在store中的位置,应该就是行号  
        }
    }

});
var previewExpand = new Ext.grid.CheckboxSelectionModel({
    dataIndex: "roleId",
    width: 28
});
var index = new Ext.grid.RowNumberer(); //行号   
//--------------------------------------------------列头
colModel = new Ext.grid.ColumnModel
        (
            [
                index,
                { dataIndex: 'ID', width: 30, sortable: false, renderer: function (value, meta, record) {
                    return '<input type="checkbox" onchange="Checked(' + value + ')" />';
                }
                },
                { header: "通知内容", width: 450, dataIndex: 'Content', sortable: false, renderer: function (value, meta, record) {
                    meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left;"';
                    return value
                }
                },
                { header: "通知类型", width: 150, dataIndex: 'Type', sortable: false },
                { header: "有效时段", width: 105, dataIndex: 'Period', sortable: false },
                { header: "通知开始时间", width: 150, dataIndex: 'StartTime', sortable: false },
                { header: "通知结束时间", width: 150, dataIndex: 'EndTime', sortable: false },
                { header: "是否停用", width: 80, dataIndex: 'IsDisable', sortable: false },
                { header: "操作", width: 142, dataIndex: 'AllPara', sortable: false, renderer: function (value, meta, record) {
                    return '<div class="editBtn" onclick="EditNotice(' +"'" +value.toString()+"'" + ')">编辑</div>';
                }
                }
            ]
        );

function createButton() {
    return "<input type='button' value='展开'>"
}


//var viewConfig = {
//    templates: {

//        //  在模板中引入自己定义的样式。使用这个view的grid的header的样式就修改了。   
//        header: new Ext.Template(
//                  ' <table border="0" cellspacing="0" cellpadding="0" style="{tstyle}">',
//                  ' <thead> <tr id="my-grid-head">{mergecells} </tr>' +
//                  ' <tr id="x-grid3-hd-row">{cells} </tr> </thead>',
//                  " </table>"
//                  ),
//        mhcell: new Ext.Template(
//                  ' <td id="myrow" colspan="{mcols}"> <div align="center"> <b>{value} </b> </div>',
//                  " </td>"
//                  )
//    }
//};
var viewConfig = {
    templates: {
        hcell: new Ext.Template(
                            '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
                            '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
                                    '</div></td>'),
        //        hcell: new Ext.Template(
        //                    '<td class="x-grid3-hd x-grid3-cell tablaHeader" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="{istyle}">', '<a class="x-grid3-hd-btn" href="#"></a>',
        //                    '{value}',
        //                            '</div></td>'),
        cell: new Ext.Template(
                    '<td class="x-grid3-hd x-grid3-cell x-grid-hcell-bgcolor x-grid3-td-{id} tablaContent" style="{style}"><div {tooltip} {attr} class="x-grid3-hd-inner x-grid3-hd-{id}" unselectable="on" style="istyle">', '<a class="x-grid3-hd-btn" href="#"></a>',
                    '{value}<img class="x-grid3-sort-icon" src="', Ext.BLANK_IMAGE_URL, '" />',
                    '</div></td>')
    }

};

barPagingBar = new Ext.PagingToolbar({
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

journal_grid = new Ext.grid.GridPanel({
    id: 'idProductLog',                     //grid的id  
    autoHeight: true,
    autoWidth: true,
    //width: 1300,
    //height: 730,
    sm: sm,
    cm: colModel, //行列
    loadMask: { msg: '正在加载数据...' },
    plugins: expander,
    store: store, //数据源
    renderTo: "logTable",
    trackMouseOver: true, //鼠标特效
    autoScroll: true,
    //stripeRows: true,
    renderer: function (value, meta, record) {
        //meta.attr = 'style="white-space:normal;word-wrap: break-word;overflow:visible;"';
        return value
    },
    viewConfig: viewConfig,

    bbar: barPagingBar,
    afterrender: function () {
        //每条记录点击编辑后弹出界面
        $.each($(".editBtn"), function (i, n) {
            $(n).click(function () {
                alert("编辑");
            });
        });
    }
});
journal_grid.setAutoScroll(true);

//查询按钮
$("#btnSearch").click(function () {
    store = new Ext.data.Store({
        reader: new Ext.data.JsonReader(),
        autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

        proxy: new Ext.data.HttpProxy({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetImportantNoticeData')
        })
    });
    journal_grid.reconfigure(store, colModel);
    barPagingBar.bindStore(store, true);
    journal_grid.bbar = barPagingBar;
    barPagingBar.updateInfo();
    //    barPagingBar.moveFirst();
    barPagingBar.doRefresh();
    barPagingBar.doLayout();
});

$("#Ul2").blur(function () {
    $(this).hide();
});


$.each($($(".proNameSelect .firstPolUl")[0]).find("li"), function (j, m) {
    $(m).click(function () {
        var proName = $($(m).find("div")[0]).html();
        $($(".proNameText")[0]).html($($(m).find("div")[0]).html());
        $($(".proNameSelect .firstPolUl")[0]).addClass("hide");
        $($(".proNameSelect .firstPolUl")[0]).removeClass("display");
        store = new Ext.data.Store({
            reader: new Ext.data.JsonReader(),
            autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

            proxy: new Ext.data.HttpProxy({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
            })
        });
        journal_grid.reconfigure(store, colModel);
        barPagingBar.bindStore(store, true);
        journal_grid.bbar = barPagingBar;
        barPagingBar.updateInfo();
        //        barPagingBar.moveFirst();
        barPagingBar.doRefresh();
        barPagingBar.doLayout();
    })
});

$.each($($(".pubMethodSelect .firstPolUl")[0]).find("li"), function (j, m) {
    $(m).click(function () {
        var proMethod = $($(m).find("div")[0]).html();
        $($(".pubMethodText")[0]).html($($(m).find("div")[0]).html());
        $($(".pubMethodSelect .firstPolUl")[0]).addClass("hide");
        $($(".pubMethodSelect .firstPolUl")[0]).removeClass("display");
        store = new Ext.data.Store({
            reader: new Ext.data.JsonReader(),
            autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },
            proxy: new Ext.data.HttpProxy({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
            })
        });
        journal_grid.reconfigure(store, colModel);
        barPagingBar.bindStore(store, true);
        journal_grid.bbar = barPagingBar;
        barPagingBar.updateInfo();
        //        barPagingBar.moveFirst();
        barPagingBar.doRefresh();
        barPagingBar.doLayout();
    })
});

$.each($($(".pubConSelect .firstPolUl")[0]).find("li"), function (j, m) {
    $(m).click(function () {
        var pubCon = $($(m).find("div")[0]).html();
        $($(".pubConText")[0]).html($($(m).find("div")[0]).html());
        $($(".pubConSelect .firstPolUl")[0]).addClass("hide");
        $($(".pubConSelect .firstPolUl")[0]).removeClass("display");
        store = new Ext.data.Store({
            reader: new Ext.data.JsonReader(),
            autoLoad: { params: { start: 0, limit: 15, startTime: $("#txtPublishStartDate_Search").val(), endTime: $("#txtPublishEndDate_Search").val(), productName: $("#proName").html(), pubMethod: $("#pubMethod").html(), pubState: $("#pubCon").html(), user: $("#txtPublishClerk_Search").val()} },

            proxy: new Ext.data.HttpProxy({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetPublishLogDataNew')
            })
        });
        journal_grid.reconfigure(store, colModel);
        barPagingBar.bindStore(store, true);
        journal_grid.bbar = barPagingBar;
        barPagingBar.updateInfo();
        //        barPagingBar.moveFirst();
        barPagingBar.doRefresh();
        barPagingBar.doLayout();
    })
});
}