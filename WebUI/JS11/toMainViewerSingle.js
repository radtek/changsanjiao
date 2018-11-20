/*
此JS主要用于主页面的布局，包括抬头（north）、左侧面板（west）、中间面板（center）和右侧面板（east）。
版权所有：上海地听信息科技有限公司  http://www.readearth.cn
作者：张伟锋        日期：2010年11月20日
*/
var nowClickName = "airQuality";
Ext.BLANK_IMAGE_URL = 'Ext/resources/images/default/s.gif';
var tempArray = new Array();
var oldId = "";
var widthPX = 255;
var target = "WebGIS/WaiAQIForcastWebGIS/index.html?v=20150403";
var objWin;
//定义MainViewer类
MainViewer = {};
var currentUserName;
function toolbarClick(itemID) {
    var toolItem = Ext.getCmp(itemID);
    toolItem.fireEvent("click", toolItem);
}
function mouseOver(id) {
    var arrow = Ext.getDom(id);
    if (id != oldId)
        arrow.className = "ulSlectOver";
}
function mouseOut(id) {
    var arrow = Ext.getDom(id);
    if (id != oldId) {
        arrow.className = "ulUnSlect";
    }
}
//界面初始化
//去除动态加载效果
//    Ext.removeNode(Ext.getDom("loading"));
function tomainviewer(product) {
    var loginParams = getCookie("UserInfo");
    Ext.removeNode(Ext.getDom("forswitching"));

    var result = Ext.util.JSON.decode(loginParams);
    InitialViewer(result, product);
}
function clearTbl(tbl) {
    while ((tbl.rows.length + 1) > 1) {
        tbl.deleteRow();
    }
}
function reSetPassWord(userName) {
    var thePassword = Ext.getCmp("password");
    if (thePassword == null) {
        thePassword = new Password(userName);
        thePassword.show();
    }
}
function goback() {
    window.location.href = 'default2.aspx';
}
//初始化视窗函数
function InitialViewer(loginTime, product) {
    //Ext.getDom("loginParams").value = loginTime.UserName;
    tempArray = product.split(',');
    nowClickName = tempArray[0];
    var north;
    var west;
    var center;
    var viewport;

    if (nowClickName == "airQuality")
        widthPX = 255;
    else
        widthPX = 200;
       
    function itemClick(action, iconCls) {//Guidance
        titleNamestr = action.id;
        oldId = "";
        if (action.id == "airQuality")
            widthPX = 255;
        else
            widthPX = 200;
        switch (action.id) {
//            case "airQuality": case "xsforcast": case "guidance": case "diagnostic": case "innp": case "jgRadar": case "comForecasts": case "SuperStation": case "WeatherPollution": case "systemManage": case "dayForecast":
            case "airQuality": case "xsforcast": case "SuperStation": case "guidance": case "diagnostic": case "innp": case "jgRadar": case "comForecasts": case "WeatherPollution": case "systemManage": case "dayForecast":
                center.removeAll(true);
                west.removeAll(true);

                west.setIconClass(iconCls);
                if (west.hidden)
                    west.show();
                west.expand(false);
                //SetWestPanelHtml(action);
                SetWestPanelHtml_JX(action);
                west.doLayout();

                break;
            case "webGIS":
                {
                    //显示预报员，预报时间和时次
                    var loginParams = getCookie("UserInfo");
                    var result = Ext.util.JSON.decode(loginParams);
                    var JB = result["JB"]
                    var type = "normal";
                    if (window.location.href.indexOf("222.66.83.21") > 0) {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("WebGIS/WaiAQIForcastWebGIS/index.html?v=20150403");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                            objWin.level = JB;
                        } else {
                            objWin.type = type;
                            objWin.level = JB;
                            objWin.test();
                        }

                        break;
                    }
                    else {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("WebGIS/WaiAQIForcastWebGIS/index.html?v=20150403");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                            objWin.level = JB;
                        } else {
                            objWin.type = type;
                            objWin.level = JB;
                            objWin.test();
                        }
                        break;
                    }
                }
            case "webGISJiangXi":
                {
                    var type = "normal";
                    if (window.location.href.indexOf("222.66.83.21") > 0) {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("http://222.66.83.21:8282/AQIForcastUserGate/AQIForcastWebGIS/index.html");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                            
                        } else {
                            objWin.type = type;
                            objWin.test();
                        }

                        break;
                    }
                    else {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("http://222.66.83.21:8282/AQIForcastUserGate/AQIForcastWebGIS/index.html");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                        } else {
                            objWin.type = type;
                            objWin.test();
                        }
                        break;
                    }
                }
case "webGISJiangXi2":
                {
                    var type = "normal";
                    if (window.location.href.indexOf("222.66.83.21") > 0) {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("http://222.66.83.21:8282/AQIForcastUserGate/AQIForcastWebGIS/index.html");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;

                        } else {
                            objWin.type = type;
                            objWin.test();
                        }

                        break;
                    }
                    else {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("http://222.66.83.21:8282/AQIForcastUserGate/AQIForcastWebGIS/index.html");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                        } else {
                            objWin.type = type;
                            objWin.test();
                        }
                        break;
                    }
                }
            case "webGIS1":
                {
                    var type = "collapse";
                    if (window.location.href.indexOf("222.66.83.21") > 0) {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("WebGIS/WaiAQIForcastWebGIS/index.html?v=20150403");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                        } else {
                            objWin.type = type;
                            objWin.test();
                        }

                        break;
                    }
                    else {
                        if (objWin == null || objWin.closed) {
                            addHtmlPanelWebGIS("WebGIS/WaiAQIForcastWebGIS/index.html?v=20150403");
                            objWin = document.getElementById("gis").contentWindow;
                            objWin.type = type;
                        } else {
                            objWin.type = type;
                            objWin.test();
                        }
                        break;
                    }
                }


//            case "Superstation":
//                //                addHtmlPanel("station.aspx?v=20150403", "");
//                addHtmlPanel("NSWebSite/NS3HWeb/NS3HWeb.html", "");                
//                break;
            case "whpollution":
                addHtmlPanel("pollution.aspx?v=20150403", "");
                break;

            //预报制作部分
//            //case "reprotProduce": case "SuperStation": case "EastChinaReprotProduce": case "JiangXiAQIPart": case "ForecastScore":  
//                center.removeAll(true);
//                west.removeAll(true);

//                west.setIconClass(iconCls);
//                if (west.hidden)
//                    west.show();
//                west.expand(false);
//                SetWestPanelHtml(action);
//                west.doLayout();
            //                break; 
            case "reprotProduce": case "EastChinaReprotProduce": case "ForecastScore":
                center.removeAll(true);
                west.removeAll(true);

                west.setIconClass(iconCls);
                if (west.hidden)
                    west.show();
                west.expand(false);
                SetWestPanelHtml(action);
                west.doLayout();
                break;
            case "reprotProduceJX": case "JiangXiAQIPart": case "ForecastScoreJX":
                center.removeAll(true);
                west.removeAll(true);

                west.setIconClass(iconCls);
                if (west.hidden)
                    west.show();
                west.expand(false);
                SetWestPanelHtml_JX(action);
                west.doLayout();
                break;

            case "ReportWorkArea":
                center.removeAll(true);
                west.removeAll(true);

                west.setIconClass(iconCls);
                if (west.hidden)
                    west.show();
                west.expand(false);
                SetWestPanelHtml(action);
                addHtmlPanel("ReportProduce/WorkAreaHomePage.aspx?v=1", "HomePage");
                west.doLayout();

                break;
            case "ReportWorkAreaJX":
                center.removeAll(true);
                west.removeAll(true);

                west.setIconClass(iconCls);
                if (west.hidden)
                    west.show();
                west.expand(false);
                SetWestPanelHtml_JX(action);
                addHtmlPanel("ReportProduce/WorkAreaHomePage.aspx?v=1", "HomePage");
                west.doLayout();

                break;
                          
        }
        viewport.doLayout();
    }
    //工具栏
    var toolbar = new GISToolbar(tempArray, loginTime, nowClickName);
    toolbar.on('itemClick', itemClick);


    function SetWestPanelHtml_JX(node) {
        $.ajax({ url: getUrl('MMShareBLL.DAL.ForecastJX', 'GetLeftPanel_JX'),
            type: 'get',
            cache: false,
            dataType: 'json',
            data: { node: node.id },
            success: function (data) {
                var theHtml = "", firstNodeText = "", secondNodeText = "", firstNodeTextNew = "", secondNodeTextNew = ""; needUl = false;
                var id = "";
                theHtml += "<div class='menuTop'>";
                for (var i = 0; i < data.length; i++) {
                    if (firstNodeText == "") {
                        theHtml += "<div class='firstMenu' ><div class='" + data[i].cssName + "'></div><div class='label'>" + data[i].CLASS + "</div><div class=\"arrow down\" ></div></div>";
                        if (node.id == "airQuality")
                            theHtml += "<div class='slist'  style='width:228px;'>";
                        else
                            theHtml += "<div class='slist'>";
                        firstNodeText = data[i].CLASS;
                        if (secondNodeText == "") {
                            id = data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                            if (data[i].flag != null) {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    theHtml += " <ul class='sslist' style='width:226px;'>";
                                }
                                else {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    theHtml += " <ul class='sslist' >";
                                }
                                if (data[i].flag != "noclick")
                                    id = data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                                else
                                    id = data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                                theHtml += "<li  class='ulSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                needUl = true;
                                secondNodeText = data[i].MenuName;
                            }
                            else {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom' style='width:217px;'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
                                }
                                //                                else 
                                //                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
                                else {
                                    if (node.id == "ReportWorkArea") {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    }
                                    else {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
                                    }
                                }
                                secondNodeText = data[i].HINT;
                            }
                        }
                    }
                    else {
                        firstNodeTextNew = data[i].CLASS;
                        if (data[i].MenuName == null || data[i].MenuName == "")
                            secondNodeTextNew = data[i].HINT;
                        else
                            secondNodeTextNew = data[i].MenuName;
                        if (firstNodeText != firstNodeTextNew) {
                            if (needUl) {
                                theHtml += "</ul>";
                                theHtml += "</div>";
                                needUl = false;
                            }
                            theHtml += "</div>";
                            theHtml += "<div class='firstMenu'><div class='" + data[i].cssName + "'></div><div class='label'>" + firstNodeTextNew + "</div><div class=\"arrow down\" ></div></div>";
                            if (node.id == "airQuality")
                                theHtml += "<div class='slist'  style='width:228px;'>";
                            else
                                theHtml += "<div class='slist'>";


                            if (data[i].flag != null) {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    theHtml += " <ul class='sslist' style='width:226px;'>";
                                }
                                else {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                    theHtml += "<ul class='sslist'>";
                                }
                                theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                needUl = true;
                            }
                            else {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'  style='width:217px;'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                }
                                else
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                            }
                            secondNodeText = secondNodeTextNew;
                            firstNodeText = firstNodeTextNew;
                        }
                        else {
                            if (secondNodeText != secondNodeTextNew) {
                                if (needUl) {
                                    theHtml += "</ul>";
                                    needUl = false;
                                }

                                if (data[i].flag != null) {
                                    if (node.id == "airQuality") {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                        theHtml += "<ul class='sslist' style='width:226px;'>";
                                    }
                                    else {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                        theHtml += "<ul class='sslist'>";
                                    }
                                    theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                    needUl = true;
                                    secondNodeText = secondNodeTextNew;
                                }
                                else {
                                    secondNodeText = data[i].HINT;
                                    if (node.id == "airQuality") {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                    }
                                    else
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                }
                            }
                            else {
                                theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                            }
                        }

                    }
                }
                theHtml += "</div>";
                if (node.id != "ReportWorkArea") {
                    secondNodeClick(id);
                }
                west.removeAll(true);
                var el = Ext.getCmp('westPanel');
                el.setSize(widthPX);
                var htmPanel = new Ext.Panel({
                    border: false,
                    header: false,
                    id: 'htmPanelInner',
                    containerScroll: true,
                    autoScroll: true,
                    layout: "fit",
                    bodyStyle: 'background-color: #F4F4EC;',
                    html: theHtml
                });

                west.add(htmPanel);
                west.doLayout();
                viewport.doLayout();

                $(".firstMenu").toggle(function () {
                    CollapsePanel(this);
                    $(this).next(".slist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                }, function () {
                    CollapsePanel(this);
                    $(this).next(".slist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                });

                $('.secondMenu').hover(function () {
                    var id = this.id;
                    var array = id.split('_');
                    var labelObj = $($(this).find('label'));

                    var btnObj = $($(this).find('.leftCommom'));
                    var theClass = btnObj.attr("class");
                    btnObj = $($(this).find('.middleCommom'));
                    var theClass1 = btnObj.attr("class");
                    btnObj = $($(this).find('.rightCommom'));
                    var theClass2 = btnObj.attr("class");
                    if (theClass.indexOf("leftCss") >= 0) {
                        $($(this).find(".leftCss")).attr("class", theClass.replace('leftCss', 'leftCssv'));
                    }
                    if (theClass1.indexOf("middleCss") >= 0) {
                        $($(this).find(".middleCss")).attr("class", theClass1.replace('middleCss', 'middleCssv'));
                    }
                    if (theClass2.indexOf("rightCss") >= 0) {
                        $($(this).find(".rightCss")).attr("class", theClass2.replace('rightCss', 'rightCssv'));
                    }
                    if (array[0] != "noclick" && array[0] != "null" && array[0] != null) {
                        var btnObj = $($(this).find('.middleCommom'));
                        var theClass1 = btnObj.attr("class");
                        if (theClass1.indexOf("middleCssv") >= 0) {
                            labelObj.removeClass("secondLable");
                            labelObj.addClass("secondLableOrange");
                        }
                        else if (theClass1.indexOf("middleCssClick") >= 0) {
                            labelObj.removeClass("secondLable");
                            labelObj.addClass("secondLableGreen");
                        }
                    }
                },
                function () {
                    var id = this.id;
                    var array = id.split('_');
                    var labelObj = $($(this).find('label'));

                    var btnObj = $($(this).find('.leftCommom'));
                    var theClass = btnObj.attr("class");
                    btnObj = $($(this).find('.middleCommom'));
                    var theClass1 = btnObj.attr("class");
                    btnObj = $($(this).find('.rightCommom'));
                    var theClass2 = btnObj.attr("class");
                    if (theClass.indexOf("leftCssv") >= 0) {
                        $($(this).find(".leftCssv")).attr("class", theClass.replace('leftCssv', 'leftCss'));
                    }
                    if (theClass1.indexOf("middleCssv") >= 0) {
                        $($(this).find(".middleCssv")).attr("class", theClass1.replace('middleCssv', 'middleCss'));
                    }
                    if (theClass2.indexOf("rightCssv") >= 0) {
                        $($(this).find(".rightCssv")).attr("class", theClass2.replace('rightCssv', 'rightCss'));
                    }
                    if (array[0] != "noclick" && array[0] != "null" && array[0] != null) {
                        labelObj.removeClass("secondLableGreen");
                        labelObj.removeClass("secondLableOrange");
                        labelObj.addClass("secondLable");
                    }
                });

                $(".secondMenu").toggle(function (event) {
                    var array = this.id.split('_');
                    if (array[0] == "noclick") {
                        CollapsePanelSecond(this);
                        $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                    }
                    else {
                        var obj = event.srcElement ? event.srcElement : event.target;
                        if (obj.className.indexOf("secondLable") < 0) {
                            CollapsePanelSecond(this);
                            $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                        }
                    }
                }, function () {
                    var array = this.id.split('_');
                    if (array[0] == "noclick") {
                        CollapsePanelSecond(this);
                        $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                    }
                    else {
                        var obj = event.srcElement ? event.srcElement : event.target;
                        if (obj.className.indexOf("secondLable") < 0) {
                            CollapsePanelSecond(this);
                            $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                        }
                    }
                });
                $(".secondMenu").click(function () {
                    var array = this.id.split('_');
                    if (array[0] == "null") {
                        secondNodeClick(this.id);
                        $("li").removeClass("ulSlect");
                        $("li").addClass("ulUnSlect");
                        var btn = $($(".secondMenu").find('.leftCssClick'));
                        $(btn).removeClass("leftCssClick");
                        $(btn).addClass("leftCss");
                        var btn = $($(".secondMenu").find('.middleCssClick'));
                        $(btn).removeClass("middleCssClick");
                        $(btn).addClass("middleCss");
                        var btn = $($(".secondMenu").find('.rightCssClick'));
                        $(btn).removeClass("rightCssClick");
                        $(btn).addClass("rightCss");

                        var btnObj = $($(this).find('.leftCommom'));
                        var theClass = btnObj.attr("class");
                        btnObj = $($(this).find('.middleCommom'));
                        var theClass1 = btnObj.attr("class");
                        btnObj = $($(this).find('.rightCommom'));
                        var theClass2 = btnObj.attr("class");
                        if (theClass.indexOf("leftCssv") >= 0) {
                            $($(this).find(".leftCssv")).attr("class", theClass.replace('leftCssv', 'leftCssClick'));
                        }
                        else if (theClass.indexOf("leftCss") >= 0) {
                            $($(this).find(".leftCss")).attr("class", theClass.replace('leftCss', 'leftCssClick'));
                        }

                        if (theClass1.indexOf("middleCssv") >= 0) {
                            $($(this).find(".middleCssv")).attr("class", theClass1.replace('middleCssv', 'middleCssClick'));
                        }
                        else if (theClass1.indexOf("middleCss") >= 0) {
                            $($(this).find(".middleCss")).attr("class", theClass1.replace('middleCss', 'middleCssClick'));
                        }
                        if (theClass2.indexOf("rightCssv") >= 0) {
                            $($(this).find(".rightCssv")).attr("class", theClass2.replace('rightCssv', 'rightCssClick'));
                        }
                        else if (theClass2.indexOf("rightCss") >= 0) {
                            $($(this).find(".rightCss")).attr("class", theClass2.replace('rightCss', 'rightCssClick'));
                        }
                    }
                });
                $(".secondLable").click(function () {
                    var parent = $(this).parent().parent();
                    var array = parent[0].id.split('_');
                    if (array[0] != "noclick") {
                        secondNodeClick(parent[0].id);
                        $("li").removeClass("ulSlect");
                        $("li").addClass("ulUnSlect");
                        var btn = $($(".secondMenu").find('.leftCssClick'));
                        $(btn).removeClass("leftCssClick");
                        $(btn).addClass("leftCss");
                        var btn = $($(".secondMenu").find('.middleCssClick'));
                        $(btn).removeClass("middleCssClick");
                        $(btn).addClass("middleCss");
                        var btn = $($(".secondMenu").find('.rightCssClick'));
                        $(btn).removeClass("rightCssClick");
                        $(btn).addClass("rightCss");

                        var btnObj = $(parent.find('.leftCommom'));
                        var theClass = btnObj.attr("class");
                        btnObj = $(parent.find('.middleCommom'));
                        var theClass1 = btnObj.attr("class");
                        btnObj = $(parent.find('.rightCommom'));
                        var theClass2 = btnObj.attr("class");
                        if (theClass.indexOf("leftCssv") >= 0) {
                            $(parent.find(".leftCssv")).attr("class", theClass.replace('leftCssv', 'leftCssClick'));
                        }
                        else if (theClass.indexOf("leftCss") >= 0) {
                            $(parent.find(".leftCss")).attr("class", theClass.replace('leftCss', 'leftCssClick'));
                        }

                        if (theClass1.indexOf("middleCssv") >= 0) {
                            $(parent.find(".middleCssv")).attr("class", theClass1.replace('middleCssv', 'middleCssClick'));
                        }
                        else if (theClass1.indexOf("middleCss") >= 0) {
                            $(parent.find(".middleCss")).attr("class", theClass1.replace('middleCss', 'middleCssClick'));
                        }
                        if (theClass2.indexOf("rightCssv") >= 0) {
                            $(parent.find(".rightCssv")).attr("class", theClass2.replace('rightCssv', 'rightCssClick'));
                        }
                        else if (theClass2.indexOf("rightCss") >= 0) {
                            $(parent.find(".rightCss")).attr("class", theClass2.replace('rightCss', 'rightCssClick'));
                        }
                    }
                });
                secondID = this.id;
                if (node.id != "ReportWorkAreaJX") {
                    $("li").click(function () { nodeClickFirst(this); });
                }
            },
            error: function (ex) {
                if (ex.statusText != "OK") alert("异°¨?常?ê，ê?请?检¨?查¨|！ê?");
            }
        });
    }


    function SetWestPanelHtml(node) {
        $.ajax({ url: getUrl('MMShareBLL.DAL.Forecast', 'GetLeftPanel'),
            type: 'get',
            cache: false,
            dataType: 'json',
            data: { node: node.id },
            success: function (data) {
                var theHtml = "", firstNodeText = "", secondNodeText = "", firstNodeTextNew = "", secondNodeTextNew = ""; needUl = false;
                var id = "";
                theHtml += "<div class='menuTop'>";
                for (var i = 0; i < data.length; i++) {
                    if (firstNodeText == "") {
                        theHtml += "<div class='firstMenu' ><div class='" + data[i].cssName + "'></div><div class='label'>" + data[i].CLASS + "</div><div class=\"arrow down\" ></div></div>";
                        if (node.id == "airQuality")
                            theHtml += "<div class='slist'  style='width:228px;'>";
                        else
                            theHtml += "<div class='slist'>";
                        firstNodeText = data[i].CLASS;
                        if (secondNodeText == "") {
                            id = data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                            if (data[i].flag != null) {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    theHtml += " <ul class='sslist' style='width:226px;'>";
                                }
                                else {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    theHtml += " <ul class='sslist' >";
                                }
                                if (data[i].flag != "noclick")
                                    id = data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                                else
                                    id = data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                                theHtml += "<li  class='ulSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                needUl = true;
                                secondNodeText = data[i].MenuName;
                            }
                            else {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom' style='width:217px;'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
                                }
                                //                                else 
                                //                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
                                else {
                                    if (node.id == "ReportWorkArea") {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    }
                                    else {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
                                    }
                                }
                                secondNodeText = data[i].HINT;
                            }
                        }
                    }
                    else {
                        firstNodeTextNew = data[i].CLASS;
                        if (data[i].MenuName == null || data[i].MenuName == "")
                            secondNodeTextNew = data[i].HINT;
                        else
                            secondNodeTextNew = data[i].MenuName;
                        if (firstNodeText != firstNodeTextNew) {
                            if (needUl) {
                                theHtml += "</ul>";
                                theHtml += "</div>";
                                needUl = false;
                            }
                            theHtml += "</div>";
                            theHtml += "<div class='firstMenu'><div class='" + data[i].cssName + "'></div><div class='label'>" + firstNodeTextNew + "</div><div class=\"arrow down\" ></div></div>";
                            if (node.id == "airQuality")
                                theHtml += "<div class='slist'  style='width:228px;'>";
                            else
                                theHtml += "<div class='slist'>";


                            if (data[i].flag != null) {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                    theHtml += " <ul class='sslist' style='width:226px;'>";
                                }
                                else {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                    theHtml += "<ul class='sslist'>";
                                }
                                theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                needUl = true;
                            }
                            else {
                                if (node.id == "airQuality") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'  style='width:217px;'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                }
                                else
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                            }
                            secondNodeText = secondNodeTextNew;
                            firstNodeText = firstNodeTextNew;
                        }
                        else {
                            if (secondNodeText != secondNodeTextNew) {
                                if (needUl) {
                                    theHtml += "</ul>";
                                    needUl = false;
                                }

                                if (data[i].flag != null) {
                                    if (node.id == "airQuality") {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                        theHtml += "<ul class='sslist' style='width:226px;'>";
                                    }
                                    else {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                        theHtml += "<ul class='sslist'>";
                                    }
                                    theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                    needUl = true;
                                    secondNodeText = secondNodeTextNew;
                                }
                                else {
                                    secondNodeText = data[i].HINT;
                                    if (node.id == "airQuality") {
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom' style='width:217px;'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                    }
                                    else
                                        theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                }
                            }
                            else {
                                theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                            }
                        }

                    }
                }
                theHtml += "</div>";
                if (node.id != "ReportWorkArea") {
                    secondNodeClick(id);
                }
                west.removeAll(true);
                var el = Ext.getCmp('westPanel');
                el.setSize(widthPX);
                var htmPanel = new Ext.Panel({
                    border: false,
                    header: false,
                    id: 'htmPanelInner',
                    containerScroll: true,
                    autoScroll: true,
                    layout: "fit",
                    bodyStyle: 'background-color: #F4F4EC;',
                    html: theHtml
                });

                west.add(htmPanel);
                west.doLayout();
                viewport.doLayout();

                $(".firstMenu").toggle(function () {
                    CollapsePanel(this);
                    $(this).next(".slist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                }, function () {
                    CollapsePanel(this);
                    $(this).next(".slist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                });

                $('.secondMenu').hover(function () {
                    var id = this.id;
                    var array = id.split('_');
                    var labelObj = $($(this).find('label'));

                    var btnObj = $($(this).find('.leftCommom'));
                    var theClass = btnObj.attr("class");
                    btnObj = $($(this).find('.middleCommom'));
                    var theClass1 = btnObj.attr("class");
                    btnObj = $($(this).find('.rightCommom'));
                    var theClass2 = btnObj.attr("class");
                    if (theClass.indexOf("leftCss") >= 0) {
                        $($(this).find(".leftCss")).attr("class", theClass.replace('leftCss', 'leftCssv'));
                    }
                    if (theClass1.indexOf("middleCss") >= 0) {
                        $($(this).find(".middleCss")).attr("class", theClass1.replace('middleCss', 'middleCssv'));
                    }
                    if (theClass2.indexOf("rightCss") >= 0) {
                        $($(this).find(".rightCss")).attr("class", theClass2.replace('rightCss', 'rightCssv'));
                    }
                    if (array[0] != "noclick" && array[0] != "null" && array[0] != null) {
                        var btnObj = $($(this).find('.middleCommom'));
                        var theClass1 = btnObj.attr("class");
                        if (theClass1.indexOf("middleCssv") >= 0) {
                            labelObj.removeClass("secondLable");
                            labelObj.addClass("secondLableOrange");
                        }
                        else if (theClass1.indexOf("middleCssClick") >= 0) {
                            labelObj.removeClass("secondLable");
                            labelObj.addClass("secondLableGreen");
                        }
                    }
                },
                function () {
                    var id = this.id;
                    var array = id.split('_');
                    var labelObj = $($(this).find('label'));

                    var btnObj = $($(this).find('.leftCommom'));
                    var theClass = btnObj.attr("class");
                    btnObj = $($(this).find('.middleCommom'));
                    var theClass1 = btnObj.attr("class");
                    btnObj = $($(this).find('.rightCommom'));
                    var theClass2 = btnObj.attr("class");
                    if (theClass.indexOf("leftCssv") >= 0) {
                        $($(this).find(".leftCssv")).attr("class", theClass.replace('leftCssv', 'leftCss'));
                    }
                    if (theClass1.indexOf("middleCssv") >= 0) {
                        $($(this).find(".middleCssv")).attr("class", theClass1.replace('middleCssv', 'middleCss'));
                    }
                    if (theClass2.indexOf("rightCssv") >= 0) {
                        $($(this).find(".rightCssv")).attr("class", theClass2.replace('rightCssv', 'rightCss'));
                    }
                    if (array[0] != "noclick" && array[0] != "null" && array[0] != null) {
                        labelObj.removeClass("secondLableGreen");
                        labelObj.removeClass("secondLableOrange");
                        labelObj.addClass("secondLable");
                    }
                });

                $(".secondMenu").toggle(function (event) {
                    var array = this.id.split('_');
                    if (array[0] == "noclick") {
                        CollapsePanelSecond(this);
                        $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                    }
                    else {
                        var obj = event.srcElement ? event.srcElement : event.target;
                        if (obj.className.indexOf("secondLable") < 0) {
                            CollapsePanelSecond(this);
                            $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                        }
                    }
                }, function () {
                    var array = this.id.split('_');
                    if (array[0] == "noclick") {
                        CollapsePanelSecond(this);
                        $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                    }
                    else {
                        var obj = event.srcElement ? event.srcElement : event.target;
                        if (obj.className.indexOf("secondLable") < 0) {
                            CollapsePanelSecond(this);
                            $(this).next(".sslist").animate({ height: 'toggle', opacity: 'toggle' }, "slow");
                        }
                    }
                });
                $(".secondMenu").click(function () {
                    var array = this.id.split('_');
                    if (array[0] == "null") {
                        secondNodeClick(this.id);
                        $("li").removeClass("ulSlect");
                        $("li").addClass("ulUnSlect");
                        var btn = $($(".secondMenu").find('.leftCssClick'));
                        $(btn).removeClass("leftCssClick");
                        $(btn).addClass("leftCss");
                        var btn = $($(".secondMenu").find('.middleCssClick'));
                        $(btn).removeClass("middleCssClick");
                        $(btn).addClass("middleCss");
                        var btn = $($(".secondMenu").find('.rightCssClick'));
                        $(btn).removeClass("rightCssClick");
                        $(btn).addClass("rightCss");

                        var btnObj = $($(this).find('.leftCommom'));
                        var theClass = btnObj.attr("class");
                        btnObj = $($(this).find('.middleCommom'));
                        var theClass1 = btnObj.attr("class");
                        btnObj = $($(this).find('.rightCommom'));
                        var theClass2 = btnObj.attr("class");
                        if (theClass.indexOf("leftCssv") >= 0) {
                            $($(this).find(".leftCssv")).attr("class", theClass.replace('leftCssv', 'leftCssClick'));
                        }
                        else if (theClass.indexOf("leftCss") >= 0) {
                            $($(this).find(".leftCss")).attr("class", theClass.replace('leftCss', 'leftCssClick'));
                        }

                        if (theClass1.indexOf("middleCssv") >= 0) {
                            $($(this).find(".middleCssv")).attr("class", theClass1.replace('middleCssv', 'middleCssClick'));
                        }
                        else if (theClass1.indexOf("middleCss") >= 0) {
                            $($(this).find(".middleCss")).attr("class", theClass1.replace('middleCss', 'middleCssClick'));
                        }
                        if (theClass2.indexOf("rightCssv") >= 0) {
                            $($(this).find(".rightCssv")).attr("class", theClass2.replace('rightCssv', 'rightCssClick'));
                        }
                        else if (theClass2.indexOf("rightCss") >= 0) {
                            $($(this).find(".rightCss")).attr("class", theClass2.replace('rightCss', 'rightCssClick'));
                        }
                    }
                });
                $(".secondLable").click(function () {
                    var parent = $(this).parent().parent();
                    var array = parent[0].id.split('_');
                    if (array[0] != "noclick") {
                        secondNodeClick(parent[0].id);
                        $("li").removeClass("ulSlect");
                        $("li").addClass("ulUnSlect");
                        var btn = $($(".secondMenu").find('.leftCssClick'));
                        $(btn).removeClass("leftCssClick");
                        $(btn).addClass("leftCss");
                        var btn = $($(".secondMenu").find('.middleCssClick'));
                        $(btn).removeClass("middleCssClick");
                        $(btn).addClass("middleCss");
                        var btn = $($(".secondMenu").find('.rightCssClick'));
                        $(btn).removeClass("rightCssClick");
                        $(btn).addClass("rightCss");

                        var btnObj = $(parent.find('.leftCommom'));
                        var theClass = btnObj.attr("class");
                        btnObj = $(parent.find('.middleCommom'));
                        var theClass1 = btnObj.attr("class");
                        btnObj = $(parent.find('.rightCommom'));
                        var theClass2 = btnObj.attr("class");
                        if (theClass.indexOf("leftCssv") >= 0) {
                            $(parent.find(".leftCssv")).attr("class", theClass.replace('leftCssv', 'leftCssClick'));
                        }
                        else if (theClass.indexOf("leftCss") >= 0) {
                            $(parent.find(".leftCss")).attr("class", theClass.replace('leftCss', 'leftCssClick'));
                        }

                        if (theClass1.indexOf("middleCssv") >= 0) {
                            $(parent.find(".middleCssv")).attr("class", theClass1.replace('middleCssv', 'middleCssClick'));
                        }
                        else if (theClass1.indexOf("middleCss") >= 0) {
                            $(parent.find(".middleCss")).attr("class", theClass1.replace('middleCss', 'middleCssClick'));
                        }
                        if (theClass2.indexOf("rightCssv") >= 0) {
                            $(parent.find(".rightCssv")).attr("class", theClass2.replace('rightCssv', 'rightCssClick'));
                        }
                        else if (theClass2.indexOf("rightCss") >= 0) {
                            $(parent.find(".rightCss")).attr("class", theClass2.replace('rightCss', 'rightCssClick'));
                        }
                    }
                });
                secondID = this.id;
                if (node.id != "ReportWorkArea") {
                    $("li").click(function () { nodeClickFirst(this); });
                }
            },
            error: function (ex) {
                if (ex.statusText != "OK") alert("异°¨?常?ê，ê?请?检¨?查¨|！ê?");
            }
        });
    }

    function addHtmlPanel(iframeSrc, id) {
        Ext.TaskMgr.stopAll();
        var height=$(window).height()-107+"px";
//        if (!Ext.isIE8)
//            height = "100%";
//        else
        //            height = window.screen.availHeight - window.screenTop - 145 + "px"; //IE8高度控制，如果不加的话，页面显示不全

        var htmPanel = new Ext.Panel({
            border: false,
            header: false,
            layout: "fit",
            html: "<div id='frame' style='width:100%;height:" + height + ";-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + iframeSrc + "&id=" + id + "' height='100%' width='100%' frameborder='0'></iframe></div>"
        });
        center.removeAll();
        center.add(htmPanel);
        center.doLayout();
    }

    function addWebHtmlPanel(iframeSrc) {
        Ext.TaskMgr.stopAll();
        var height = $(window).height() - 107+237 + "px";
        //        if (!Ext.isIE8)
        //            height = "100%";
        //        else
        //            height = window.screen.availHeight - window.screenTop - 145 + "px"; //IE8高度控制，如果不加的话，页面显示不全

        var htmPanel = new Ext.Panel({
            border: false,
            header: false,
            layout: "fit",
            html: "<div id='frame' class='hideTop' style='width:100%;height:" + height + ";-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + iframeSrc + "' height='100%' width='100%' frameborder='0'></iframe></div>"
        });
        center.removeAll();
        center.add(htmPanel);
        center.doLayout();
    }

    function nodeClick(id, tag, html, name, text) {
        if (tag == "U") {
            if (id == "LastestProduct") {
                addWebHtmlPanel(html);
            }
            else {
                addHtmlPanel(html, id);
            }
        }
        else {
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.Forecast', 'GetEntity'),
                params: { entityName: id },
                success: function (response) {
                    var result = Ext.util.JSON.decode(response.responseText);
                    var entity = {
                        align: tag,
                        alias: text,
                        realtime: html,
                        name: name
                    }
                    center.removeAll();
                    var ImageFrameEntity = new ImageFrame(result, entity);
                    center.add(ImageFrameEntity);
                    center.doLayout();
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
    }
    function secondNodeClick(obj) {

        var array = obj.split('_');
        if (array[0] != "noclick") {
            if (array.length > 5) {
                
                if (array[0] == "null" || array[0] == null)
                    nodeClick(array[5], array[1], array[2], array[3], array[4]);
                else
                    nodeClick(array[0], array[1], array[2], array[3], array[4]);
                
            }
        }
    }
    function nodeClickFirst(obj) {
        var btn = $($(".secondMenu").find('.leftCssClick'));
        $(btn).removeClass("leftCssClick");
        $(btn).addClass("leftCss");
        var btn = $($(".secondMenu").find('.middleCssClick'));
        $(btn).removeClass("middleCssClick");
        $(btn).addClass("middleCss");
        var btn = $($(".secondMenu").find('.rightCssClick'));
        $(btn).removeClass("rightCssClick");
        $(btn).addClass("rightCss");
        $("li").removeClass("ulSlect");
        $("li").addClass("ulUnSlect");
        obj.className = "ulSlect";
        var array = obj.id.split('_');
        if (array.length > 4)
            nodeClick(array[0], array[1], array[2], array[3], array[4]);
    }
    function CollapsePanel(obj) {
        var btnObj = $($(obj).find('.arrow'));
        var theClass = btnObj.attr("class");
        if (theClass.indexOf("up") > 0) {
            btnObj.attr("class", theClass.replace('up', 'down'));
        }
        else {
            btnObj.attr("class", theClass.replace('down', 'up'));
        }

    }
    function CollapsePanelSecond(obj) {
        var btnObj = $($(obj).find('.menuarrow1'));
        var theClass = btnObj.attr("class");
        if (theClass != undefined) {
            if (theClass.indexOf("menuarrowUp") >= 0) {
                btnObj.attr("class", theClass.replace('menuarrowUp', 'menuarrowDown'));
            }
            else {
                btnObj.attr("class", theClass.replace('menuarrowDown', 'menuarrowUp'));
            }
        }

    }
    function exit() {
        delCookie("URLSTR");
        var userName = loginTime.UserName;
        var ip = loginTime.LoginIP;
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

    function showLoginCount() {
        var win = new Ext.Window({
            title: "操作日志",
            layout: "fit",
            width: 300,
            height: 350,
            resizable: false,
            shadow: true,
            modal: true,
            closable: true,
            bodyStyle: "padding:1 1 1 1",
            animCollapse: true,
            html: "<iframe src='Logs.aspx' height='100%' width='100%' frameborder='0'></iframe>"

        });
        win.show();
    }

    //    //打开HTML的Frame
    //    function addHtmlPanel(iframeSrc) {
    //        if (west.hidden == false)
    //            west.hide();
    //        var height = "100%";
    //        var htmPanel = new Ext.Panel({
    //            border: false,
    //            header: false,
    //            layout: "fit",
    //            html: "<div id='frameID' style='width:100%;height:" + height + ";-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + iframeSrc + "' height='100%' width='100%' frameborder='0'></iframe></div>"
    //        });
    //        center.removeAll();
    //        center.add(htmPanel);
    //        center.doLayout();
    //    }
    //打开 WebGIS  HTML的Frame ,只是为了获取 iframe 的window对象 判定webgis是否打开
    function addHtmlPanelWebGIS(iframeSrc) {
        if (west.hidden == false)
            west.hide();
        var height = "100%";
        var htmPanel = new Ext.Panel({
            border: false,
            header: false,
            layout: "fit",
            html: "<div id='frameID' style='width:100%;height:" + height + ";-webkit-overflow-scrolling:touch; overflow: auto;'><iframe id='gis' src='" + iframeSrc + "' height='100%' width='100%' frameborder='0'></iframe></div>"
        });
        center.removeAll();
        center.add(htmPanel);
        center.doLayout();
    }


    //系统抬头，包括系统的名称、模块列表
    var north = new Ext.Panel({
        region: "north",
        border: false,
        baseCls: "header",
        //       html:"<div class='loginfont defaultBackimage'>欢迎第<b style='color:Orange'>"+loginTime.LoginCount+"</b>次登陆，<a  style='color: #FFFFFF' href='javascript:reSetPassWord(\""+ //loginTime.UserName + "\")'>"+loginTime.Alias+"</a><br>"+loginTime.Local+"<br>技术支持：上海地听" + "&nbsp;</div>",
        //html: "<div class='loginfont defaultBackimage'><div class='rightHeader'><div class='login-info'> <p class='user'>" + loginTime.Alias + "</a><p //class='access'>欢迎第<b //style='color:red'>" + loginTime.LoginCount + "</b>次登陆</div></div></div>",
html: "<div class='loginfont defaultBackimage'><div class='rightHeader'><div class='login-info'> <p class='user'>" + loginTime.Alias + "</a><p class='access'>欢迎第<b style='color:red'>" + loginTime.LoginCount + "</b>次登陆</div></div></div>",

        bbar: toolbar
    });

    //地图面板
    center = new Ext.Panel({
        id: "frameCenter",
        layout: "fit",
        baseCls: 'borderCls',
        region: "center"

    });

    //左侧功能面板，此面板可以根据工具栏按钮事件进行切换
    west = new Ext.Panel({
        id: 'westPanel',
        region: 'west',
        width: widthPX,
        layout: 'fit',
        margins: '0 0 0 0',
        border: false,
        containerScroll: true,
        autoScroll: true,
        collapsible: false,
        bodyStyle: 'background-color: #F4F4EC;'


        //        iconCls: 'weather',
        //        listeners: { "beforeadd": function (ownerCt, functionPanel, index) { ownerCt.setTitle(functionPanel.title); } }//动态改变面板抬头
    });


    //当图片产品加载完成后自动触发图片显示

    //右侧查询结果面板，此面板缺省处于隐藏状态，可以改变大小
    east = new Ext.Panel({
        id: 'eastPanel',
        region: 'east',
        width: 200,
        layout: 'fit',
        margins: '0 0 0 0',
        collapsible: true,
        collapsed: true,
        collapseMode: 'mini',
        split: true,
        items: {
            baseCls: 'x-plain',
            html: '<p style="padding:10px;color:#556677;font-size:11px;">Select a configuration to the right &raquo;</p>'
        }

    });

    //主窗口
    viewport = new Ext.Viewport({// 填充整个浏览器窗口
        id: 'mainLayout',
        layout: 'border', // 边框布局
        items: [north, west, center]
    });

    //new code
    itemClick(toolbar.getComponent(titleNamestr == "" ? nowClickName : titleNamestr), '')
    {
        toolbar.getComponent(titleNamestr == "" ? nowClickName : titleNamestr).addClass("navi");
    }    
    //InitialViewer方法结束
}


function divClick(id) {
    var arrow = Ext.getDom("arrow" + id);
    var menu = Ext.getDom("menu" + id);
    if (arrow.className == "arrow up") {
        arrow.className = "arrow down";
        menu.style.display = "none";

    }
    else {
        arrow.className = "arrow up";
        menu.style.display = "block";
    }


}