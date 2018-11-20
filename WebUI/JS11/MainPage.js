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
    $("#mainContent").height($(window).height - 88);
    $("#mainContent").css('background-color', 'white');
    $("#leftMenu").height($("#mainContent").height()); 
    
}

   //xuehui 2017-06-09
    function QX(id) {
        var userName = $("#userName").html();
        var URLSTR = getCookie("URLSTR");

//        var ids=URLSTR.split(';')[1];

//        if (ids == "Beij" || ids == "XiaM" ) {
//             if(id.toString()=="liproductMake"){
//                return false;
//             }
//        }
        return true;
    }

    function GetModules() {

      var URLSTR = getCookie("URLSTR");
      var ids = URLSTR.split(';')[1];

      $.ajax({
          url: getUrl('MMShareBLL.DAL.Forecast', 'GetModules'),
          type: "get",
          contentType: "application/json",
          dataType: 'json',
          data: { userName: ids },
          success: function (results) {
              var strHtml = "";
              var index = 0;
              for (var i = 0; i < results.length; i++) {
                  strHtml += "<li id='li" + results[i].DM + "' class='commin_img " + results[i].DM + "_img' ></li>";
                  if (results[i].DM == 'outPdPreview') {// xuehui 06-26
                      index = i;
                  }
              }
              $('nav ul').html(strHtml);

              $("nav li").click(function () {
                  if (QX(this.id)) {
                      GotoPage(this);
                      Ref();
                  }
              });
              GotoPage($("nav li")[index]); // xuehui 06-15  [1]
              ImageClick();
          },
          error: function (ex) {
              //alert("加载菜单异常，" + ex.responseText);
              exit();
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
            var height = $("#mainPanel").height()-87;
            $('#qyyj').height(height);
            $('#qyyj').html("<iframe id='iframewebGIS' src='http://222.66.83.21:8282/CSJGateWayV2/CSJWebGISV2/index.html' frameborder='0' style='position:absolute;right:0;left:0px;top:0px;bottom:0;width:100%;height:" + height + "px' z-index=3></iframe>");
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

//获取取消订阅的总数
function GetQXCount() {
    $.ajax({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetCount'),
        type: "get",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {

            $.each($(".ulUnSlect"), function (i, n) {
                var v = $(this)[0].id;
                if (v.indexOf("Unsubscribe.aspx") >= 0) {
                    var marginRight = "5px";
                    if (results.toString().length >= 2)
                        marginRight = "2px";

                    if (results.toString().length > 0 && results.toString()!='0') {
                        var htmls = "取消订阅 <div style='width:20px; height:20px; background-color:#F00; border-radius:55px; margin-right:50px; float:right'> " +
                     "<span style='height:20px; line-height:20px; display:block; color:#FFF; text-align:center; float:right; margin-right:" + marginRight + "'>" + results.toString() + "</span></div> ";
                        $(this).html(htmls);
                        return;
                    }
                }
            });
        }
    });

}

function CreateChildTree(id) {//ProductHomePage
    var leftMenu = $("#leftMenu");
    var mainMenu = $("#mainContent");
    var URLSTR = getCookie("URLSTR");
    var ids = URLSTR.split(';')[1];// xuehui 06-23

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
            data: { id: id, uid: ids }, // 06-23 xuehui 
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
                //if (id != "ReportWorkArea" && id != "ForecastScore") {
                //    secondNodeClick(id);
                //}
                leftMenu.html(theHtml);
                //xuehui 0921 
                GetQXCount();
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
                if (id != "ReportWorkArea" && id != "ForecastScore") {
                    secondNodeClick(id);
                }
            },
            error: function (ex) {
                //alert("加载列表异常，" + ex.responseText);
                exit();
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
function secondNodeClick(obj,ele) {

    var array = obj.split('_');
    if (array[0] != "noclick") {
        if (array.length > 5) {
            if (array[0] == "null" || array[0] == null)
                nodeClick(array[5], array[1], array[2], array[3], array[4],ele);
            else
                nodeClick(array[0], array[1], array[2], array[3], array[4],ele);
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
        nodeClick(array[0], array[1], array[2], array[3], array[4],obj);
}
function nodeClick(id, tag, html, name, text,ele) {

    $("#mainContent").html("");
    if (tag == "U") {
        addHtmlPanel(html, id, ele);
        $("#mainContent").height($(window).height() - 88);
        //$("#leftMenu").height($("#mainContent").height());       
        var height = $("#mainPanel").height()-87;
        $("#leftMenu").height(height);   
    }
    else {
        $("#mainContent").height($(window).height());
        //$("#leftMenu").height($("#mainContent").height()); 
        var height = $("#mainPanel").height() - 87;
        $("#leftMenu").height(height);   
        $('#mainContent').html("<iframe id='uPanel' src='ImageView.aspx?id=" + id+"|"+tag+"|"+text+"|" +html+"|"+name+ "'  height='100%' width='100%' frameborder='0'></iframe>");
    }
}
function addHtmlPanel(iframeSrc, id,ele) {
    Ext.TaskMgr.stopAll();
    //王斌   0413  把该页面的父模块的文本传入iframe中 主要为了延伸期预报每个页面中抬头部分的说明，这个是每一个大模块的说明相同
    var parentText = $(ele).parents(".slist").prev().text();
    if (ele == undefined) {
        if ($(".ulSlect").length > 0) {   //有四级菜单，优先
            parentText = $(".ulSlect").parents(".slist").prev().text();
        } else {
            parentText = $(".middleCssClick").parents(".slist").prev().text();
        }
    }
    
    //if (ele != undefined) {
    //    parentText = $(ele).parents(".slist").prev().text();
    //}
    if (iframeSrc == "http://www.soweather.com/Environment.html") {
            $('#mainContent').html("<iframe src='" + iframeSrc + "' height='100%' width='100%' frameborder='0'></iframe>");
        } else {
            $('#mainContent').html("<iframe src='" + iframeSrc + "&id=" + id + "&parentText="+parentText+"' height='100%' width='100%' frameborder='0'></iframe>");
        }
        //$('#mainContent').html("<iframe src='" + iframeSrc + "&id=" + id + "' height='100%' width='100%' frameborder='0'></iframe>");
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
        secondNodeClick(obj.id,obj);
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

    var URLSTR = getCookie("URLSTR");
    var ids = URLSTR.split(';')[1];
    
    var addMunu = $("#addMunu");
    var leftMenu = $("#leftMenu");
    var mainMenu = $("#mainContent");
    var width = $(window).width()-272;
    mainMenu.width(width);
    if (id == "webGIS") {
        $("#otherHtml").css('display', 'none');
        $('#GisHtml').html("<iframe id='uPanel' src='http://222.66.83.21:8282/CSJGateWayV2/CSJWebGISV2/index.html'  height='100%' width='100%' frameborder='0'></iframe>");
    }
    else {
        $("#GisHtml").css('display', 'none');
        $("#otherHtml").css('display', 'block');
        $.ajax({
            url: getUrl('MMShareBLL.DAL.Forecast', 'GetLeftPanelMain'),
            type: 'get',
            cache: false,
            dataType: 'json',
            data: { moduleId: ids, limit: id },// ids  xuehui 06-23
            success: function (results) {
                var data = results;
                var theHtml = "";
                var height = GetWinHeight() - $("header").height();
                if (data) {
                    for (var i = 0; i < data.length; i++) {
                        var temp = data[i].childModuleCName.split("</br>");
                        var li_height = height / data.length;
                        var line_height = (li_height / temp.length).toString() + "px;";
                        theHtml = theHtml + "<li class='mainMunu_left' style=\"line-height:" + line_height + ";height:" + li_height + "px;\" id='" + data[i].childModuleName + "'>" + data[i].childModuleCName + "</li>";
                    }
                }
                $('main ul').html(theHtml);
                $("main li").click(function () {
                    CreateChildPage(this);
                });
                CreateChildPage($("main li")[0]);
            },
            error: function (ex) {
                //alert("加载列表异常，" + ex.responseText);
                exit();
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

function ImageClick() {
    $.each($(".commin_img"), function (i, n) {
        $(n).hover(function () {
            $(this).css("background-image", $(this).css('backgroundImage').toString().replace(".png", "1.png")); 
        });
    });
    $.each($(".commin_img"), function (i, n) {
        $(n).mouseleave(function () {
            $(this).css("background-image","");
        });
    });
    //xuehui 2017-05-10
    $.each($(".icon_img_left"), function (i, n) {
        $(n).click(function () {
            var userName = $("#userName").html();
            var URLSTR = getCookie("URLSTR");
            if (URLSTR == "exit" || URLSTR == "" || URLSTR == undefined) {
                exit();
                return;
            }
            if (URLSTR.split(';')[1] == "readearth") {
                $("#GisHtml").css('display', 'block');
                $("#otherHtml").css('display', 'none');
                $('#GisHtml').html("<iframe id='uPanel' src='./HealthyWeather/LogInSet.aspx'  height='100%' width='100%' frameborder='0'></iframe>");
            } else {
                alert("您没有此功能权限！");
            }
        });
    });


    //xuehui 2017-06-26
    $.each($(".user_img"), function (i, n) {
        $(n).click(function () {
            var URLSTR = getCookie("URLSTR");
            if (URLSTR == "exit" || URLSTR == "" || URLSTR == undefined) {
                exit();
                return;
            }
            if (URLSTR.split(';')[1] == "readearth") {
                $("#GisHtml").css('display', 'block');
                $("#otherHtml").css('display', 'none');
                $('#GisHtml').html("<iframe id='uPanel' src='./Authority/Authority.aspx'  height='90%' width='100%' frameborder='0'></iframe>");
            } else {
               // alert("您没有此功能权限！");
            }
        });
    });

    $.each($(".bit"), function (i, n) {
        $(n).click(function () {
            $(".bit ").css('background-image', 'url(images/ico-01.png)'); // xuehui 2017-04-20 
            if ($("#leftMenu").parent("div").width() <= 6) {
                $(".nicescroll-rails").getNiceScroll().show();
                $("#leftMenu").parent("div").css('background-color', 'rgb(216,215,213)');
                $("#leftMenu").parent("div").css('width', 200); // xuehui 2017-04-20
  
                $("#leftMenu").css('width', 200); // xuehui 2017-04-20
                $(".bit").css('left', 200); // xuehui 2017-04-20 
                var width = $("#mainContent").width() - 200;
                $("#mainContent").parent().css('width', width);
                $("#mainContent").parent().css('left', 200);
                $("#mainContent").css('width', width);
                $("#leftMenu").css('overflow', 'hidden');
                $(".ishow ").css('background-image', 'url(images/jt-01.png)'); // xuehui 2017-04-20 
                this.title = "收缩";

            } else {
                $(".nicescroll-rails").getNiceScroll().hide();
                $("#leftMenu").parent("div").css('background-color', 'rgb(216,215,213)');
                $("#leftMenu").parent("div").css('width', 6); // xuehui 2017-04-20
 
                $("#leftMenu").css('width', 0); // xuehui 2017-04-20
                $("#leftMenu").css('overflow', 'auto'); // xuehui 2017-04-20
                $(".bit").css('left', 6); // xuehui 2017-04-20 
                var width = $("#mainContent").width() +200;
                $("#mainContent").parent().css('width', width);
                $("#mainContent").parent().css('left', 6);
                $("#mainContent").css('width', width);
                $(".ishow ").css('background-image', 'url(images/jt-01-1.png)'); // xuehui 2017-04-20 
                this.title = "展开";
            }

        });
    });
}

function Ref() {
    $(".bit ").css('background-image', 'url(images/ico-01.png)'); // xuehui 2017-04-20 
    if ($("#leftMenu").parent("div").width() <= 6) {
        $(".nicescroll-rails").getNiceScroll().show();
        $("#leftMenu").parent("div").css('background-color', 'rgb(216,215,213)');
        $("#leftMenu").parent("div").css('width', 200); // xuehui 2017-04-20

        $("#leftMenu").css('width', 200); // xuehui 2017-04-20
        $(".bit").css('left', 200); // xuehui 2017-04-20 
        var width = $("#mainContent").width();
        $("#mainContent").parent().css('width', width);
        $("#mainContent").parent().css('left', 200);
        $("#mainContent").css('width', width);
        $("#leftMenu").css('overflow', 'hidden');
        $(".ishow ").css('background-image', 'url(images/jt-01.png)'); // xuehui 2017-04-20 
        this.title = "收缩";

    }
}