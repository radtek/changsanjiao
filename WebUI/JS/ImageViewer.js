// 实现图片浏览，,可以上下左右移动,也可以用鼠标随意拖动,放大(双击或点加号图片),缩小,复原等
var count;
var time;
var posX;
var posY
var entityNameLink;
var aliasName;
var parent="";
var thisparent;
var eName;
ImageViewer = function (imageSrc, entityName, name) {
   parent = this;
    thisparent=this;
    eName=entityName;
    /**  
    * 初始化  
    */
    function pageInit(imageViewer) {
        var image = Ext.get('view-image');
        imageFull(image);
        aliasName = name;

        
        entityNameLink = entityName;


        if (image != null) {
            image.dom.onmousewheel = bbimg;

            image.dom.onload = function () {

                imageFull(image);
                //Ext.getDom('syy').style.display = "none"; Ext.getDom('xyy').style.display = "none";
            }
            listener_onmousewheel(image);
            image.on({
                'click': { fn: function () {
                    var cur = Ext.getDom('view-image').style.cursor.toString();
                    if (cur.indexOf('prev') >= 0) {
                        upImg();
                    }
                    if (cur.indexOf('next') >= 0) {
                        downImg();
                    }
                }, scope: image
                },
                'mouseup': { fn: function () {
                }, scope: image
                },
                'dblclick': { fn: function () {
                    var cur = Ext.getDom('view-image').style.cursor.toString();
                    if (cur.indexOf('prev') >= 0) { upImg(); }
                    if (cur.indexOf('next') >= 0) { downImg(); }
                }
                }
            });
            new Ext.dd.DD(image, 'pic');
            imgLoad(image.dom, function (e) { fullExtent(e); })

            Ext.get('up').on('click', function () { imageMove('up', image); });       //向上移动   
            Ext.get('down').on('click', function () { imageMove('down', image); });   //向下移动   
            Ext.get('left').on('click', function () { imageMove('left', image); });   //左移   
            Ext.get('right').on('click', function () { imageMove('right', image); }); //右移动   
            Ext.get('in').on('click', function () { zoom(image, true, 1.5); });        //放大   
            Ext.get('out').on('click', function () { zoom(image, false, 1.5); });      //缩小   
            Ext.get('zoom').on('click', function () { restore(image); });            //还原   
            Ext.get('zoomf').on('click', function () { imageFull(image); });            //全图显示  
            Ext.get('print').on('click', function () { printImage(image); });            //打印    
        }

    };
    function printImage(el) {
        var imgWidth = images2.width;
        var imgHeight = images2.height;
        var parentWidth = 800;
        var parentHeight = 620;
        var wK = imgWidth / parentWidth;
        var hK = imgHeight / parentHeight;


        if (imgWidth > 1) {
            if (wK > hK && wK > 1) {
                imgWidth = parentWidth;
                imgHeight = imgHeight / wK;
            }
            else if (hK > wK && hK > 1) {
                imgWidth = imgWidth / hK;
                imgHeight = parentHeight;
            }
        }
        var oWin = window.open('', '打印', 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
        if (oWin) {
            oWin.document.open();
            oWin.document.write("<html>");
            oWin.document.write("<body>");
            oWin.document.write("<center>");
            oWin.document.write("<img id='img' width='" + imgWidth + "' height = '" + imgHeight + "'  src=" + el.dom.src + " border='0'>");
            oWin.document.write("</center>");
            oWin.document.write("</body>");
            oWin.document.write("</html>");
            oWin.document.close();
            oWin.print();
            oWin.close();
        }

    }
    /**  
    * 按比例全图显示 
    */
    function imageFull(el) {
        var imgWidth = images2.width;
        var imgHeight = images2.height;
        var parentWidth = parent.getWidth();
        var parentHeight = parent.getHeight();
        var wK = imgWidth / parentWidth;
        var hK = imgHeight / parentHeight;

        if (imgWidth > 1) {
            if (wK > hK && wK > 1)
                el.setSize(parentWidth, imgHeight / wK);
            else if (hK > wK && hK > 1)
                el.setSize(imgWidth / hK, parentHeight);

            el.center(parent.el); //图片居中   
            el.dom.onload = "";
        }
    }

    /**  
    * 图片移动  
    */
    function imageMove(direction, el) {
        el.move(direction, 50, true);
        Ext.getDom('syy').style.display = "none";
        Ext.getDom('xyy').style.display = "none";
    }



    /**  
    * 图片还原  
    */
    function restore(el) {
        var size = {
            width: images2.width,
            height: images2.height
        };
        el.setSize(size.width, size.height);
        el.center(parent.el);
    }



    ImageViewer.superclass.constructor.call(this, {
        border: false,
        layout: "fit",
        region: "center",
        html:
            '<div id="mapPic" ><div class="nav" >'
            + '<div class="up" id="up"></div><div class="right" id="right"></div>'
            + '<div class="down" id="down"></div><div class="left" id="left"></div>'
            + '<div class="zoom" id="zoom"></div><div class="in" id="in"></div>'
            + '<div class="zoomf" id="zoomf"></div><div class="out" id="out"></div>'
            + '<div class="print" id="print" title = "打印"></div></div>'
            + '<div id="syy" title="上一张图"  onmousedown="upImg()" class="commentII"     style="cursor: none; display:none; width:32px; height:48px;background-position: 50% 50%; background-repeat: no-repeat no-repeat; " ></div>'
            + '<div id="xyy" title="下一张图"  ></div>'
            + '<div id="hiddenPic" style="position:absolute; left:0px; top:0px; width:0px; height:0px; z-index:1; visibility: hidden;">'
            + '<img name="images2" src="' + imageSrc + '" border="0"></div>'
//            + '<img id="view-image"  src="' + imageSrc + '" border="0" style="cursor:url(images/pic_prev.cur),auto;"  > </div>'
             + '<img id="view-image"  src="' + imageSrc + '" border="0" style="cursor:url(images/pic_prev.cur),auto;height:"' + ($("#bg").height() - 120) + ';width:auto"  > </div>'  
    });

    this.on('afterrender', pageInit);


}

function MoveSelection(view, step) {
    //如果正在动画播放，那么停止动画
    var checkPlay = $("#move");
    if (checkPlay.length > 0) {
        if (checkPlay[0].className.indexOf("Stop") > 0) {
            checkPlay[0].click();
        }
    }
    //存在列表选项
    if ($("#timeTable tr").length > 0) {
        var index = parseInt($("#timeTable").find("tr.trClick")[0].id.substr(2));
        var totalLength = $("#timeTable tr").length;
        var nowIndex;
        if (index == 1 && step == -1)
            nowIndex = totalLength;
        else if (index == totalLength && step == -1)
            nowIndex = totalLength - 1;
        else
            if (index < totalLength)
                nowIndex = index + step;
            else
                nowIndex = 1;
        $("#Tr" + nowIndex).click();
    }
}
upSelect = function (viewID) {
    var component = Ext.getCmp(viewID);
    MoveSelection(component, -1);
}

downSelect = function (viewID) {
    var component = Ext.getCmp(viewID);
    MoveSelection(component, 1);
}
function upImg() {
    upSelect(entityNameLink);
}

function downImg() {
    downSelect(entityNameLink);
}

function getX(obj) {
    var parObj = obj;
    var left = obj.offsetLeft;
    while (parObj = parObj.offsetParent) {
        left += parObj.offsetLeft;
    }
    return left;
}

function getY(obj) {
    var parObj = obj;
    var top = obj.offsetTop;
    while (parObj = parObj.offsetParent) {
        top += parObj.offsetTop;
    }
    return top;
}

function DisplayCoord(event) {
    var top, left, oDiv;
    oDiv = document.getElementById("view-image");
    top = getY(oDiv);
    left = getX(oDiv);
    var x = (event.clientX - left + document.documentElement.scrollLeft) + "px";
    var y = innerHTML = (event.clientY - top + document.documentElement.scrollTop) + "px";
    return x + "," + y;
}

function mouseOvers(obj) {
    $('#syy').css("display", "block");
    $('#syy').css("background-image", "url('images/jt-1-v.png')");
    $('#xyy').css("display", "block");
    $('#xyy').css("background-image", "url('images/jt-2-n.png')");
}

function mouseOuts(obj) {
    var e = null || window.event;
    var _tar = toTarget(e);
    var f = checkcontainss(obj, _tar);
    $(obj).css("background-image", "url('images/jt-1-n.png')");
    if (!_tar || f) {
    } else {
        Ext.getDom('syy').style.display = "none";
        Ext.getDom('xyy').style.display = "none";
    }
}

function mouseOverss(obj) {
    $('#syy').css("display", "block");
    $('#syy').css("background-image", "url('images/jt-1-n.png')");
    $('#xyy').css("display", "block");
    $('#xyy').css("background-image", "url('images/jt-2-v.png')");
}

function mouseOutss(obj) {
    var e = null || window.event;
    var _tar = toTarget(e);
    var f = checkcontainss(obj, _tar);
    $(obj).css("background-image", "url('images/jt-2-n.png')");
    if (!_tar || f) {
    } else {
        Ext.getDom('syy').style.display = "none";
        Ext.getDom('xyy').style.display = "none";
    }
}

function checkcontainss(parentNode, childNode) {
    if (parentNode.contains) {
        return parentNode != childNode && parentNode.contains(childNode);
    } else {
        return !!(parentNode.compareDocumentPosition(childNode) & 16);
    }
}

var formTarget = function (e) {
    var e = e || window.event;
    if (e.relatedTarget) { return e.relatedTarget } else if (e.fromElement) { return e.fromElement }
    return null;
}
var toTarget = function (e) {
    var e = e || window.event;
    if (e.relatedTarget) { return e.relatedTarget } else if (e.toElement) { return e.toElement }
    return null;
}

//继承，添加函数，或者重写函数
Ext.extend(ImageViewer, Ext.Panel, {
    setImageSrc: function (imgSrc, star, end) {
        images2.src = imgSrc;
        var image = Ext.get('view-image');
        image.dom.src = imgSrc;
        var el = document.getElementById("zoomf");
        el.click();
        $("#mapPic").height($(window).height()-85);
        $('#view-image').bind('mousemove', function (event) {
            var point = DisplayCoord(event);
            var width = Ext.get('view-image').getWidth();
            var left = parseInt(point.split(',')[0], 10);
            var viLeft = Ext.get('view-image').getLeft();
            var top = parseInt(point.split(',')[1], 10);
            var viTop = Ext.get('view-image').getTop();
            $('#view-image').css("cursor", "url(img/imageViewer/pic_prev.cur),auto");

            if ((left + viLeft - 225) <= ((width / 8) + viLeft - 225)) {
                // $('#syy').css("background-image", "url('images/nextBtnTop.png')");
                //alert("xxx");
                $('#view-image').css("cursor", "url(images/pic_prev.cur),auto");
                return;
            }

            if ((left + viLeft - 225) > ((width / 8) + viLeft - 225) && (left + viLeft - 225) <= ((width / 8) * 7 + viLeft - 225)) {
                // $('#syy').css("background-image", "url('images/nextBtnTop.png')");
                //alert("xxx");
                $('#view-image').css("cursor", "url(images/openhand_8_8.cur),auto");
                return;
            }

            if ((left + viLeft - 225) > ((width / 8) * 7 + viLeft - 225)) {
                $('#view-image').css("cursor", "url(images/pic_next.cur),auto");
                return;
            }

        });
    }

});
//注册，可以通过xtype:imageViewer来创建面板
Ext.reg('imageViewer', ImageViewer);

 function imgLoad(img, callback) {
  var timer = setInterval(function() {
      if (img.width>1) {
          callback(img);
          clearInterval(timer);
      }
  }, 50)
}
function fullExtent(e) {
    var el = Ext.get(e.id);
    var imgWidth = el.dom.width;
    var imgHeight = el.dom.height;
    var parentWidth = thisparent.getWidth();
    var parentHeight = thisparent.getHeight();
    var wK = imgWidth / parentWidth;
    var hK = imgHeight / parentHeight;

    if (imgWidth > 1) {
        if (wK > hK && wK > 1)
            el.setSize(parentWidth, imgHeight / wK);
        else if (hK > wK && hK > 1)
            el.setSize(imgWidth / hK, parentHeight);

        el.center(thisparent.el); //图片居中   
        el.dom.onload = "";
    }
//    Ext.getCmp(eName).focus();
}
function bbimg(ev) {
    var up; //标识滚轮向上

    var evn = ev || event; //IE11 既包括wheelDelta又包括detail属性，而一般的IE只包括wheelDelta，火狐只包括detail属性
    if (evn.wheelDelta) {
        if (evn.wheelDelta >= 120)
            up = true;
        else if (evn.wheelDelta <= -120)
            up = false;
    }
    else if (evn.detail) {
        if (evn.detail <= -3)
            up = true;
        else if (evn.detail >= 3)
            up = false;
    }

    zoom(Ext.get('view-image'), up, 1.2);

}
function zoom(el, type, offset) {
    var width = el.getWidth();
    var height = el.getHeight();
    var nwidth = type ? (width * offset) : (width / offset);
    var nheight = type ? (height * offset) : (height / offset);
    var left = type ? -((nwidth - width) / 2) : ((width - nwidth) / 2);
    var top = type ? -((nheight - height) / 2) : ((height - nheight) / 2);
    el.animate(
        {
            height: { to: nheight, from: height },
            width: { to: nwidth, from: width },
            left: { by: left },
            top: { by: top }
        },
        null,
        null,
        'easeOut',
        'motion'
    );

}

