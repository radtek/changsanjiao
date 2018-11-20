$(function () {
    SetSize();
    //GetLoginJB();
    GetModules();
});
window.onresize = function () {
    SetSize();
}
function SetSize() {
    $("#otherHtml").height(GetWinHeight() - $("header").height());
    $("#mainContent").width($(window).width() - 272);
    $("#mainContent").height($(window).height() - 88);
    $("#mainContent").css('background-color', 'white');
    $("#leftMenu").height($("#mainContent").height()); 
    
}
function GetModules() {
    $.ajax({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetModules'),
        type: "get",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            var strHtml = "";

            for (var i = 0; i < results.length; i++) {
                strHtml += "<li id='li" + results[i].DM + "' class='commin_img " + results[i].DM + "_img' ></li>";
            }
            $('nav ul').html(strHtml);

            $("nav li").click(function () {
                GotoPage(this);
            });
            GotoPage($("nav li")[1]);
        },
        error: function (ex) {
            alert("加载菜单异常，" + ex.responseText);
        }
    });
}
function GotoPage(obj) {
    if (obj.className.indexOf("active") == -1) {
        var btnObj = $("nav .active");
        if (btnObj.length > 0) {
            btnObj.attr("class", btnObj[0].className.replace('_img_d', '_img'));
        }
        $("nav .active").removeClass("active");
        $(obj).addClass("active");
        var id = obj.id.substring(2);
        if (id == "webGIS") {

            $("#addMunu").css('display', 'none');
            $("#mainPanel").css('display', 'none');
            $("#qyyj").css('display', 'block');
            var height = GetWinHeight() - $("header").height();
            $('#qyyj').html("<iframe id='iframewebGIS' src='http://222.66.83.21:8282/CSJGateWay/CSJWebGIS/index.html' frameborder='0' style='position:absolute;right:0;left:0px;top:0px;bottom:0;width:1920px;height:860px' z-index=3></iframe>");
            $(obj).attr("class", obj.className.replace(id + '_img', id + '_img_d'));

        }
        else {
            $("#addMunu").css('display', 'block');
            $("#mainPanel").css('display', 'block');
            $("#qyyj").css('display', 'none');
            $(obj).attr("class", obj.className.replace(id + '_img', id + '_img_d'));
            CreateMainTree(id);
        }
    }
}
function CreateChildPage(obj) {
    if (obj.className.indexOf("focus") == -1) {
        $("main .focus").removeClass("focus");
        $(obj).addClass("focus");
        CreateChildTree(obj.id);
    }
}
function CreateChildTree(id) {//ProductHomePage
    var leftMenu = $("#leftMenu");
    var mainMenu = $("#mainContent");


    if (id == "ProductHomePage" ) {
        $("#mainPanel").css('display', 'none');
        $("#qyyj").css('display', 'block');
        var height = GetWinHeight() - $("header").height();
        $('#qyyj').html("<iframe id='uPanel' src='ProductHomePage.aspx?id=" + id + "'  height='" + height + "px' width='100%' frameborder='0'></iframe>");
    }
    else {
        $("#mainPanel").css('display', 'block');
        $("#qyyj").css('display', 'none');
        $.ajax({
            url: getUrl('MMShareBLL.DAL.Forecast', 'GetLeftPanelTree'),
            type: 'get',
            cache: false,
            dataType: 'json',
            data: { id: id },
            success: function (data) {
                var theHtml = "", firstNodeText = "", secondNodeText = "", firstNodeTextNew = "", secondNodeTextNew = ""; needUl = false;
                var id = "";
                theHtml += "<div class='menuTop'>";
                for (var i = 0; i < data.length; i++) {
                    if (firstNodeText == "") {
                        theHtml += "<div class='firstMenu btnCollapse' >" + data[i].CLASS + "<div class=\"arrow down_tree\" ></div></div>"; //<div class=\"arrow_collapsed up_collapsed\" onclick='collapsedPanel()'></div>
                        theHtml += "<div class='slist'>";
                        firstNodeText = data[i].CLASS;
                        if (secondNodeText == "") {
                            id = data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                            if (data[i].flag != null) {
                                theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class='rightCommom rightCss'></div></div>";
                                theHtml += " <ul class='sslist' >";
                                if (data[i].flag != "noclick")
                                    id = data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                                else
                                    id = data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME;
                                theHtml += "<li  class='ulSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                needUl = true;
                                secondNodeText = data[i].MenuName;
                            }
                            else {
                                if (id == "ReportWorkArea" || id == "ForecastScore") {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCss'></div></div>";
                                }
                                else {
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCssClick leftCommom'></div><div class='middleCssClick middleCommom'><div class='menuarrow' ></div><label class='OnlyLable'>" + data[i].HINT + "</label></div><div class='rightCommom rightCssClick'></div></div>";
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
                            theHtml += "<div class='firstMenu btnCollapse'>" + firstNodeTextNew + "<div class=\"arrow down_tree\" ></div></div>";
                            theHtml += "<div class='slist'>";


                            if (data[i].flag != null) {
                                theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                theHtml += "<ul class='sslist'>";
                                theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                needUl = true;
                            }
                            else {
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
                                    theHtml += "<div class='secondMenu' id='" + data[i].flag + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'><div class='leftCss leftCommom'></div><div class='middleCss middleCommom'><div class='menuarrowUp menuarrow1' ></div><label class='secondLable'>" + data[i].MenuName + "</label></div><div class=' rightCommom rightCss'></div></div>";
                                    theHtml += "<ul class='sslist'>";
                                    theHtml += "<li  class='ulUnSlect' id='" + data[i].ENTITYNAME + "_" + data[i].ALIGN + "_" + data[i].PERIOD + "_" + data[i].AliasName + "_" + data[i].HINT + "_" + data[i].ENTITYNAME + "'>" + data[i].HINT + "</li>";
                                    needUl = true;
                                    secondNodeText = secondNodeTextNew;
                                }
                                else {
                                    secondNodeText = data[i].HINT;
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
                if (id != "ReportWorkArea" && id != "ForecastScore") {
                    secondNodeClick(id);
                }
                leftMenu.html(theHtml);
                childScrollPage();
                $(".btnCollapse").click(function () {
                    CollapsePanel(this);
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

                $(".secondMenu").click(function () {
                    CollapseSecondPanel(this);

                });
                $(".secondLable").click(function () {
                    var parent = $(this).parent().parent();
                    var array = parent[0].id.split('_');
                    if (array[0] != "noclick") {
                        secondNodeClick(parent[0].id);
                        $(".sslist li").removeClass("ulSlect");
                        $(".sslist li").addClass("ulUnSlect");
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
                if (id != "ReportWorkArea" && id != "ForecastScore") {
                    $(".sslist li").click(function () { nodeClickFirst(this); });
                }
            },
            error: function (ex) {
                alert("加载列表异常，" + ex.responseText);
            }
        });
    }
}
function childScrollPage() {
    $("#leftMenu").niceScroll({
        cursorcolor: "#00A0EA",
        cursoropacitymax: 1,
        touchbehavior: false,
        cursorwidth: "5px",
        cursorborder: "0",
        cursorborderradius: "5px",
        enablekeyboard: false, //设置nicescroll是否可以管理键盘事件
        background: "#f2f2f2"
    });
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
    $(".sslist li").removeClass("ulSlect");
    $(".sslist li").addClass("ulUnSlect");
    obj.className = "ulSlect";
    var array = obj.id.split('_');
    if (array.length > 4)
        nodeClick(array[0], array[1], array[2], array[3], array[4]);
}
function nodeClick(id, tag, html, name, text) {

    $("#mainContent").html("");
    if (tag == "U") {
        addHtmlPanel(html, id);
    }
    else {
        $('#mainContent').html("<iframe id='uPanel' src='ImageView.aspx?id=" + id+"|"+tag+"|"+text+"|" +html+"|"+name+ "'  height='100%' width='100%' frameborder='0'></iframe>");
    }
}
function addHtmlPanel(iframeSrc, id) {
    Ext.TaskMgr.stopAll();
    $('#mainContent').html("<iframe src='" + iframeSrc + "&id=" + id + "' height='100%' width='100%' frameborder='0'></iframe>");
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
function CollapseSecondPanel(obj) {
    var array = obj.id.split('_');
    if (array[0] == "null") {
        secondNodeClick(obj.id);
        $(".sslist li").removeClass("ulSlect");
        $(".sslist li").addClass("ulUnSlect");
        var btn = $($(".secondMenu").find('.leftCssClick'));
        $(btn).removeClass("leftCssClick").addClass("leftCss");
        var btn = $($(".secondMenu").find('.middleCssClick'));
        $(btn).removeClass("middleCssClick").addClass("middleCss");
        var btn = $($(".secondMenu").find('.rightCssClick'));
        $(btn).removeClass("rightCssClick").addClass("rightCss");

        var btnObj = $($(obj).find('.leftCommom'));
        $(btnObj).attr("class", "leftCommom leftCssClick");
        btnObj = $($(obj).find('.middleCommom'));
        $(btnObj).attr("class", "middleCommom middleCssClick");
        btnObj = $($(obj).find('.rightCommom'));
        $(btnObj).attr("class", "rightCommom rightCssClick");
    }
    else {
        var array = obj.id.split('_');
        var obj1 = event.srcElement ? event.srcElement : event.target;
        if (array[0] == "noclick" || obj1.className.indexOf("secondLable") < 0) {
            var btnObj = $($(obj).find('.menuarrow1'));
            var theClass = btnObj.attr("class");
            if (theClass != undefined) {
                if (theClass.indexOf("menuarrowUp") >= 0) {
                    btnObj.attr("class", theClass.replace('menuarrowUp', 'menuarrowDown'));
                    $(obj).next(".sslist").animate({ height: "0px" }, "slow");
                }
                else {
                    btnObj.attr("class", theClass.replace('menuarrowDown', 'menuarrowUp'));
                    $(obj).next(".sslist").css({ "height": "auto" });
                }
            }
        }  
    }
}
function CollapsePanel(obj) {
    var btnObj = $($(obj).find('.arrow'));
    var theClass = btnObj.attr("class");
    if (theClass.indexOf("up_tree") > 0) {
        btnObj.attr("class", theClass.replace('up_tree', 'down_tree'));
        $(obj).next(".slist").css({ "height": "auto" });
    }
    else {
        btnObj.attr("class", theClass.replace('down_tree', 'up_tree'));
        $(obj).next(".slist").animate({ height: "0px" }, "slow");
    }

}
function CreateMainTree(id) {
    var addMunu = $("#addMunu");
    var leftMenu = $("#leftMenu");
    var mainMenu = $("#mainContent");
    var width = $(window).width()-272;
    mainMenu.width(width);
    if (id == "webGIS") {
        $("#otherHtml").css('display', 'none');
        $('#GisHtml').html("<iframe id='uPanel' src='http://222.66.83.21:8282/CSJGateWay/CSJWebGIS/index.html'  height='100%' width='100%' frameborder='0'></iframe>");
    }
    else {
        $("#GisHtml").css('display', 'none');
        $("#otherHtml").css('display', 'block');
        $.ajax({
            url: getUrl('MMShareBLL.DAL.Forecast', 'GetLeftPanelMain'),
            type: 'get',
            cache: false,
            dataType: 'json',
            data: { moduleId: id, limit: '1' },
            success: function (results) {
                var data = results;
                var theHtml = "";
                var height = GetWinHeight() - $("header").height();
                if (data) {
                    for (var i = 0; i < data.length; i++) {
                        var temp = data[i].childModuleCName.split("</br>");
                        var li_height = height / data.length;
                        var line_height = (li_height / temp.length).toString() + "px;";
                        theHtml = theHtml + "<li class='mainMunu_left' style=\"line-height:" + line_height + ";height:"+li_height+"px;\" id='" + data[i].childModuleName + "'>" + data[i].childModuleCName + "</li>";
                    }
                }
                $('main ul').html(theHtml);
                $("main li").click(function () {
                    CreateChildPage(this);
                });
                CreateChildPage($("main li")[0]);
            },
            error: function (ex) {
                alert("加载列表异常，" + ex.responseText);
            }
        });
    }

}
function exit() {
    delCookie("URLSTR");
    var userName = $("#userName").html();
    var ip = "";
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
function collapsePanel() {
//    $("#leftMenu").css("display", "none");
    $("#collapsed").layout('collapse', 'west');  
}