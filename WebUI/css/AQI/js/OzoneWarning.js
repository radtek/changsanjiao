var tablePanel;
Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");


    $(".radioTypeItem").change(function () {
        var selectedvalue = $("input[name='radWarn']:checked").val();
        //         alert($selectedvalue);    
        //document.all.WebOffice1.LoadOriginalFile("", "doc");
        var wordModelDocPath = "../AQI/WordModel/";
        //新发布预警
        if (selectedvalue == "1") {
            wordModelDocPath += "发布201311152128_(13)257.doc";
        }
        //新发布预警
        else if (selectedvalue == "2") {
            wordModelDocPath += "更新201312020720_(13)264.doc";
        }
        //新发布预警
        else if (selectedvalue == "3") {
            wordModelDocPath += "解除201404190745_(14)021.doc";
        }
        document.all.WebOffice1.LoadOriginalFile(wordModelDocPath, "doc");
    });

    var tableArea = Ext.get("productContent");
    //预警颜色级别的改变
    $(".radioColorItem").change(function () {
        var selectedvalue = $("input[name='radWarnLevel']:checked").val();
        //生成预警颜色表格的内容
        GetWarnContent(selectedvalue);
    });

    //保存按键，将word文档存在临时文件夹内
    $("#btnSave").click(function () {
        UpdateWordProduct("OzoneWarning", "OzoneWarning");
    });

    //发布按键，将word文档发布到FTP
    $("#btnPublish").click(function () {

    });



});
//关闭页面时调用此函数，关闭文件 
function window_onunload() {
    try {
        var webObjCloseAlarm = document.getElementById("WebOffice1");
        webObjCloseAlarm.Close();
    } catch (e) {
        //	alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
    }
}


//生成预警颜色表格的内容
function GetWarnContent(warnColor) {

    var imgMinUrl = "<img src=\"";
    var imgBigUrl = "<img src=\"";
    //图标路径前缀
    var imgMinUrlPrefix = "../css/OzoneWarning/";
    imgMinUrl += imgMinUrlPrefix;
    imgBigUrl += imgMinUrlPrefix;
    var reportText = "";
    var proGuide = "";
    var serviceInfo = "";
    var data;
    if (warnColor == "Yellow") {
        imgMinUrl += "Yellow_Min.jpg\"></img>";
        imgBigUrl += "Yellow_Min.jpg\"></img>";
        reportText = "减少户外活动，不可进行剧烈运动；有哮喘或呼吸道疾病的敏感人群、小孩和老人尽量减少外出。";
        proGuide = "1、适当关闭屋室门窗，减少空气流通； 2、减少户外活动，不可进行剧烈运动； 3、有哮喘或呼吸道疾病的敏感人群、小孩和老人尽量减少外出。 4、局部街区实行交通管制，减少汽车流量。";

        serviceInfo = "减少户外活动，不可进行剧烈运动；有哮喘或呼吸道疾病的敏感人群、小孩和老人尽量减少外出。";

    }
    else if (warnColor == "Orange") {
        imgMinUrl += "Orange_Min.jpg\"></img>";
        imgBigUrl += "Orange_Min.jpg\"></img>";
        reportText = "未来4小时内可能出现小时平均浓度大于120ppb的臭氧，或者已经出现小时平均浓度大于120ppb的臭氧且可能持续。";
        proGuide = "1、关闭屋室门窗； 2、停止户外运动； 3、空气质量差，外出人员适当进行防护。 4、有哮喘或呼吸道疾病的敏感人群、小孩和老人呆在室内； 5、局部地区实行交通管制，减少汽车流量。";

        serviceInfo = "关闭屋室门窗；停止户外运动；空气质量差，外出人员适当进行防护；有哮喘或呼吸道疾病的敏感人群、小孩和老人呆在室内。";
    }
    else if (warnColor == "Red") {
        imgMinUrl += "Red_min.jpg\"></img>";
        imgBigUrl += "Red_Big.jpg\"></img>";
        reportText = "";
        proGuide = "";
        serviceInfo = "";
    }
    $("#reportText").text(reportText);
    data = [
            [imgMinUrl, imgMinUrl, reportText, proGuide, serviceInfo]
        ];
    var store = new Ext.data.Store({
        proxy: new Ext.data.MemoryProxy(data),
        reader: new Ext.data.ArrayReader({}, [
                { name: 'smallIcon', mapping: 0 },
                { name: 'bigIcon', mapping: 1 },
                { name: 'foreText', mapping: 2 },
                { name: 'proGuide', mapping: 3 },
                { name: 'serviceInfo', mapping: 4 }
        /* 
        type告诉reader在解析原始数据时把对应的列做为日期类型处理 
        dateFormat属性把得到的字符串转化为相应的日期格式 
        */
            ])
    });
    var cm = new Ext.grid.ColumnModel([
    //            { header: '小图标', dataIndex: 'smallIcon', width: 200, resizable: false },
    //            { header: '大图标', dataIndex: 'bigIcon', width: 200, resizable: false },
    //            { header: '预报用语', dataIndex: 'foreText', width: 200, resizable: false },
    //            { header: '防御指南', dataIndex: 'proGuide', width: 200, resizable: false },
    //            { header: '服务提示', dataIndex: 'serviceInfo', width: 200, resizable: false }
            {header: '小图标', dataIndex: 'smallIcon', width: 150, resizable: false, renderer: function (value, meta, record) {
                meta.attr = 'style="white-space:normal;word-wrap: break-word;"';
                return value
            }
        },
            { header: '大图标', dataIndex: 'bigIcon', width: 150, resizable: false, renderer: function (value, meta, record) {
                meta.attr = 'style="white-space:normal;word-wrap: break-word;"';
                return value
            }
            },
            { header: '预报用语', dataIndex: 'foreText', width: 250, resizable: false, renderer: function (value, meta, record) {
                meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left"';
                return value
            }
            },
            { header: '防御指南', dataIndex: 'proGuide', width: 250, resizable: false, renderer: function (value, meta, record) {
                meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left"';
                return value
            }
            },
            { header: '服务提示', dataIndex: 'serviceInfo', width: 250, resizable: false, renderer: function (value, meta, record) {
                meta.attr = 'style="white-space:normal;word-wrap: break-word;text-align:left"';
                return value
            }
            }

    ]);

    var grid = new Ext.grid.GridPanel({
        renderer: function (value, meta, record) {
            meta.attr = 'style="white-space:normal;"';
            return value
        },
        store: store,
        height: 130,
        width: 1070,
        cm: cm,
        listeners: {
            'beforerender': function () {
                store.load();
            }
        }
    });
    //    grid.store.on('load', function () {
    //        grid.el.select("table[class=x-grid3-row-table]").each(function (x) {
    //            x.addClass('x-grid3-cell-text-visible');
    //        });

    //    });
    if (!tablePanel) {
        tablePanel = new Ext.Panel({
            renderTo: 'productContent',  //div中的ID
            width: 1070,
            height: 130,
            items: [grid] //管理grid
        });
    }
    else {
        tablePanel.removeAll();
        tablePanel.add(grid);
        tablePanel.doLayout();
    }
    //    var panel = new Ext.Panel({
    //        renderTo: 'productContent',  //div中的ID
    //        width: '100%',
    //        height: '700px',
    //        items: [grid] //管理grid
    //    });
}

