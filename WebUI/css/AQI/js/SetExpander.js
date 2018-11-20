Ext.grid.RowExpander = function (config) {
    Ext.apply(this, config);
    this.addEvents({
        beforeexpand: true,
        expand: true,
        beforecollapse: true,
        collapse: true
    });

    Ext.grid.RowExpander.superclass.constructor.call(this);

    if (this.tpl) {
        if (typeof this.tpl == 'string') {
            this.tpl = new Ext.Template(this.tpl);
        }
        this.tpl.compile();
    }

    this.state = {};
    this.bodyContent = {};
};

Ext.extend(Ext.grid.RowExpander, Ext.util.Observable, {
    header: "",
    width: 28,
    sortable: false,
    fixed: true,
    menuDisabled: true,
    dataIndex: '',
    id: 'expander',
    lazyRender: true,
    enableCaching: true,

    getRowClass: function (record, rowIndex, p, ds) {
        p.cols = p.cols - 1;
        var content = this.bodyContent[record.id]
        if (!content && !this.lazyRender) {
            content = this.getBodyContent(record, rowIndex);
        }
        if (content) {
            p.body = content;
        }
        return this.state[record.id] ? 'x-grid3-row-expanded' : 'x-grid3-row-collapsed';
    },

    init: function (grid) {
        this.grid = grid;

        var view = grid.getView();
        view.getRowClass = this.getRowClass.createDelegate(this);

        view.enableRowBody = true;

        grid.on('render', function () {
            view.mainBody.on('mousedown', this.onMouseDown, this);
        }, this);
    },

    getBodyContent: function (record, index) {
        if (!this.enableCaching) {
            return this.tpl.apply(record.data);
        }
        var content = this.bodyContent[record.id];
        if (!content) {
            content = this.tpl.apply(record.data);
            this.bodyContent[record.id] = content;
        }
        return content;
    },

    onMouseDown: function (e, t) {
        if (t.className == 'x-grid3-row-expander') {
            e.stopEvent();
            var row = e.getTarget('.x-grid3-row');
            this.toggleRow(row);
        }
    },

    renderer: function (v, p, record) {
        p.cellAttr = 'rowspan="2"';
        return '<div class="x-grid3-row-expander">&#160;</div>';
    },

    beforeExpand: function (record, body, rowIndex) {
        //        if (this.fireEvent('beforeexpand', this, record, body, rowIndex) !== false) {
        //            if (this.tpl && this.lazyRender) {
        //                body.innerHTML = this.getBodyContent(record, rowIndex);
        //            }
        //            return true;
        //        } else {
        //            return false;
        //        }

        if (this.fireEvent('beforeexpand', this, record, body, rowIndex) !== false) {
            var funcName;
            var iframeSrc;
            var releaseDate = record.data['ProductName'];
            var tempFilaPath = record.data['FileTempPath']; //F:\EMFCDatabase\FtpTemp\WRTJ_2016020320.txt
            //临时保存文件路径存在
            if (tempFilaPath != "") {
                if (tempFilaPath.indexOf(".TXT") > 0 || tempFilaPath.indexOf(".txt")>0 || tempFilaPath.indexOf(".URP")>0) {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', "GetTempPubLogTextContent"),
                        params: { filePath: tempFilaPath },
                        success: function (response) {
                            if (response.responseText != "") {
                                body.innerHTML = "<textarea class='previewText'>" + response.responseText + "</textarea>";
                            }
                            else {
                                alert("发布失败");
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
                //图片产品
                else if (tempFilaPath.indexOf(".GIF") > 0 || tempFilaPath.indexOf(".gif") > 0 || tempFilaPath.indexOf(".jpg") > 0) {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', "GetTempPubLogImgContent"),
                        params: { filePath: tempFilaPath },
                        success: function (response) {
                            if (response.responseText != "") {
                                body.innerHTML = "<img class='previewImg' src='" + response.responseText + "' />";
                            }
                            else {
                                alert("发布失败");
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
                //word文档产品
                else if (tempFilaPath.indexOf(".doc") > 0 || tempFilaPath.indexOf(".docx") > 0) {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', "GetTempPubLogWordContent"),
                        params: { filePath: tempFilaPath },
                        success: function (response) {
                            if (response.responseText != "") {
                                body.innerHTML = "<div id='pdfFrame' style='width:100%;height:800px;-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + response.responseText + "' height='100%' width='100%' frameborder='0'></iframe></div>";
                            }
                            else {
                                alert("发布失败");
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    }); 
                }
            }            
            if (this.tpl && this.lazyRender) {
                body.innerHTML = "细节";
            }
            return true;
        } else {
            return false;
        }
    },

    toggleRow: function (row) {
        if (typeof row == 'number') {
            row = this.grid.view.getRow(row);
        }
        this[Ext.fly(row).hasClass('x-grid3-row-collapsed') ? 'expandRow' : 'collapseRow'](row);
    },

    expandRow: function (row) {
        if (typeof row == 'number') {
            row = this.grid.view.getRow(row);
        }
        var record = this.grid.store.getAt(row.rowIndex);
        var body = Ext.DomQuery.selectNode('tr:nth(2) div.x-grid3-row-body', row);
        if (this.beforeExpand(record, body, row.rowIndex)) {
            this.state[record.id] = true;
            Ext.fly(row).replaceClass('x-grid3-row-collapsed', 'x-grid3-row-expanded');
            this.fireEvent('expand', this, record, body, row.rowIndex);
        }
    },

    collapseRow: function (row) {
        if (typeof row == 'number') {
            row = this.grid.view.getRow(row);
        }
        var record = this.grid.store.getAt(row.rowIndex);
        var body = Ext.fly(row).child('tr:nth(1) div.x-grid3-row-body', true);
        if (this.fireEvent('beforcollapse', this, record, body, row.rowIndex) !== false) {
            this.state[record.id] = false;
            Ext.fly(row).replaceClass('x-grid3-row-expanded', 'x-grid3-row-collapsed');
            this.fireEvent('collapse', this, record, body, row.rowIndex);
        }
    }
});