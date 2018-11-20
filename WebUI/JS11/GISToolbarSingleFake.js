// 此工具栏用于开发GIS系统，响应按钮的点击事件，分为工具按钮，和一般按钮。根据工具和按钮为其添加统一的事件
var lastaction = null;
var loginInfo;
Ext.override(Ext.menu.Item, {
    // private
    handleClick: function (e) {
        if (!this.href) { // if no link defined, stop the event automatically
            e.stopEvent();
        }
        //改成点击展开
        if (this.menu)
            this.expandMenu();
        else
            Ext.menu.Item.superclass.handleClick.apply(this, arguments);
    }
})
//类定义部分主要完成构造函数，以及定义属性
GISToolbar = function (tempArray, loginTime, nowClickName) {
    //注册事件，一般都放在最前面，保证所有触发的地方都可以引用
    loginInfo = loginTime;
    this.addEvents({ itemClick: true });
    var menuWidth = 112;
    function createButton(id) {
        var titleText;
        var inconClass;
        switch (id) {
            case "airQuality":
                titleText = "污染天气实况";
                inconClass = "";
                break;
            case "jgRadar":
                titleText = "模式分析";
                inconClass = "";
                break;
            case "guidance":
                titleText = "指导预报";
                inconClass = "";
                break;
            case "innp":
                titleText = "边界层";
                inconClass = "";
                break;
            case "diagnostic":
                titleText = "诊断分析";
                inconClass = "";
                break;
            case "webGIS":
                titleText = "污染天气实况";
                inconClass = "";
                break;
            case "webGISJiangXi":
                titleText = "污染天气实况";
                inconClass = "";
                break;
            case "webGIS1":
                titleText = "数值模式释用";
                inconClass = "";
                break;
            case "SuperStation":
                titleText = "超级站";
                inconClass = "";
                break;
            case "WeatherPollution":
                titleText = "空气质量回顾";
                inconClass = "";
                break;
            case "reprotProduce":
                titleText = "本市产品制作与发布";
                menuWidth = 162;
                inconClass = "_Wide";
                break;
            case "EastChinaReprotProduce":
                titleText = "华东区域预报制作与发布";
                menuWidth = 182;
                inconClass = "_Wider";
                break;
            case "JiangXiAQIPart":
                titleText = "江西AQI预报与制作";
                menuWidth = 162;
                inconClass = "_Wide";
                break;
            case "ReportWorkArea": case "ReportWorkAreaJX":
                titleText = "预报工作区";
                inconClass = "";
                break;
            case "ForecastScore": case "ForecastScoreJX":
                titleText = "预报评分";
                inconClass = "";
                break;
            //            case "SuperStation":  
            //                titleText = "超级站";  
            //                inconClass = "";  
            //                break;  

        }
        var stringHtml = "<div ><span class='buttonTop" + inconClass + "'>" + titleText + "</span><div>";
        var btn = new Ext.BoxComponent({
            id: id,
            height: 35,
            width: menuWidth,
            cls: 'btnBoxCls',
            overCls: 'navi-btn-h',
            listeners: {
                render: function (component) {
                    component.getEl().on('click', function (e) {
                        var ownerCt = component.ownerCt;
                        ownerCt.onItemClick(component, inconClass);
                    });
                }
            },
            html: stringHtml
        });
        btn.text = titleText;
        return btn;
    }
    function createTime(id) {
        var btn = new Ext.BoxComponent({
            id: id,
            height: 35,
            html: "<div class='time'><ul>" + new Date().format("Y年") + ((new Date().getMonth() + 1)) + "月" + ((new Date().getDate())) + "日 " + "</ul></div><div class='space'></div>"
        });
        return btn;
    }
    function createSpace(id) {
        var btn = new Ext.BoxComponent({
            id: id,
            height: 35,
            html: "<div class='spaceK'></div>"
        });
        return btn;
    }

    GISToolbar.superclass.constructor.call(this, {
        baseCls: 'navi-area',
        id: 'gisToolbar',
        layout: 'column',
        //        html: "<div class='manage_h' style='margin-right:5px;' title='管理' onmouseover=\"this.className='manage_h'\" onmouseout=\"this.className='manage'\"></div><div class='zx' onclick='exit()' title='退出' onmouseover=\"this.className='zx_h'\" onmouseout=\"this.className='zx'\"></div><div class='OUTBack' onclick='window.location.href=\"default2.aspx?JB=" + loginInfo.JB + "&loginParams=" + loginInfo.toString() + "\"'  title='返回' onmouseover=\"this.className='INBack'\" onmouseout=\"this.className='OUTBack'\"><ul></ul></div>",
        html: "<div class='manage_h' style='margin-right:5px;' title='管理' onmouseover=\"this.className='manage_h'\" onmouseout=\"this.className='manage'\"></div><div class='zx' onclick='exit()' title='退出' onmouseover=\"this.className='zx_h'\" onmouseout=\"this.className='zx'\"></div><div class='OUTBack' id='btnBack' title='返回' onmouseover=\"this.className='INBack'\" onmouseout=\"this.className='OUTBack'\"><ul></ul></div>",
        items: []
    });
    this.add(createTime());
    this.add(createSpace());
    for (var i = 0; i < tempArray.length - 1; i++) {
        this.add(createButton(tempArray[i]));
    }
    this.add(createButton(tempArray[tempArray.length - 1]));
    lastaction = this.getComponent(titleNamestr == "" ? nowClickName : titleNamestr);
}

Ext.extend(GISToolbar, Ext.Panel, {

    //触发itemClick事件

    onItemClick: function (action, iconCls) {
        if (lastaction != action && lastaction != null) {
            lastaction.removeClass("navi");
        }
        this.fireEvent('itemClick', action, iconCls);
        action.addClass("navi");
        lastaction = action;
    }
});
    function exit() {
        delCookie("URLSTR");
        var userName = loginInfo.UserName;
        var ip = loginInfo.LoginIP;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.UserManager', 'Exit'),
            timeout: 120000,
            params: { userName: userName, ip: ip },
            success: function (response) {
                window.location.href = "Default.aspx?Method=loginOut";

            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }

        })
    }
