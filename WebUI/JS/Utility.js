// JScript 文件

//获取系统提交的URL的格式
function getUrl(provider, method) {
    return "PatrolHandler.do?provider=" + provider + "&method=" + method;
}

//开始时钟
function startTask(theTask) {
    //判断是否为执行的，如果已经执行的那么不在重复执行
    if (!theTask.enabled) {
        //alert("start"+theTask.id);
        Ext.TaskMgr.start(theTask);
        theTask.enabled = true;
    }
}
//结束时钟
function stopTask(theTask) {
    if (theTask.enabled) {
        //alert("stop"+theTask.id);
        Ext.TaskMgr.stop(theTask);
        theTask.enabled = false;
    }
}
function convertDate(date) {
    var flag = true;

    if (date.indexOf('年') > 0)
        date = date.replace('年', '-');
    if (date.indexOf('月') > 0)
        date = date.replace('月', '-');
    if (date.indexOf('日') > 0)
        date = date.replace('日', '');

    var dateParts = date.split(" ");
    var dateArray = dateParts[0].split("-");
    if (dateArray.length != 3) {
        dateArray = date.split("/");
        if (dateArray.length != 3) {
            return null;
        }
        flag = false;
    }
    var newDate = new Date();
    if (flag) {
        // month从0开始
        newDate.setFullYear(dateArray[0], dateArray[1] - 1, dateArray[2]);
    }
    else {
        newDate.setFullYear(dateArray[2], dateArray[1] - 1, dateArray[0]);
    }
    if (dateParts.lenght > 1) {
        var times = dateParts[1].split(":");
        newDate.setHours(times[0], times[1], times[2]);
    } else
        newDate.setHours(0, 0, 0);

    return newDate;
}

// create the combo instance
HourComboBox = function (hourValue, cmbID, customData) {
    var comboBoxData;
    if (Ext.isDefined(customData))
        comboBoxData = customData;
    else
        comboBoxData = [
                    [0, '00'], [1, '01'], [2, '02'], [3, '03'], [4, '04'], [5, '05'], [6, '06'], [7, '07'], [8, '08'], [9, '09'], [10, '10'], [11, '11'], [12, '12'],
                    [13, '13'], [14, '14'], [15, '15'], [16, '16'], [17, '17'], [18, '18'], [19, '19'], [20, '20'], [21, '21'], [22, '22'], [23, '23']
        ];
    HourComboBox.superclass.constructor.call(this, {
        id: cmbID,
        typeAhead: true,
        lazyRender: true,
        mode: 'local',
        triggerAction: 'all',
        forceSelection: true,
        editable: false,
        value: hourValue,
        store: new Ext.data.ArrayStore({
            fields: [
                'myId',
                'displayText'
            ],
            data: comboBoxData
        }),
        valueField: 'myId',
        displayField: 'displayText'
    })
}
//继承，添加函数，或者重写函数
Ext.extend(HourComboBox, Ext.form.ComboBox, {

});
//打印
function printSea() {
    bdhtml = window.document.body.innerHTML; //获取当前页的html代码 
    window.document.body.innerHTML = printArea.innerHTML;
    window.print();
    window.document.body.innerHTML = bdhtml;//还原html
}

//获取事件触发的控件,兼容IE和firefox
function getEventSource(evt) {
    return evt.target || window.event.srcElement;
}

//显示控件
function showEl(elID) {
    var el = Ext.getDom(elID);
    el.className = el.className.replace('hidden', 'show');
}
//隐藏控件
function hideEl(elID) {
    var el = Ext.getDom(elID);
    el.className = el.className.replace('show', 'hidden');
}

//由于火狐浏览器不支持innerText而支持，因此对于设置innerText的功能需要进行判断
function supportInnerText() {
    if (!Ext.isIE) { //firefox innerText define
        HTMLElement.prototype.__defineGetter__("innerText",
        function () {
            var anyString = "";
            var childS = this.childNodes;
            for (var i = 0; i < childS.length; i++) {
                if (childS[i].nodeType == 1)
                    //anyString += childS[i].tagName=="BR" ? "\n" : childS[i].innerText;
                    anyString += childS[i].innerText;
                else if (childS[i].nodeType == 3)
                    anyString += childS[i].nodeValue;
            }
            return anyString;
        }
        );
        HTMLElement.prototype.__defineSetter__("innerText",
        function (sText) {
            this.textContent = sText;
        }
        );
    }
}

//获取元素在指定容器中的相对位置。
function getElementLeft(element, targetParent) {
    var actualLeft = element.offsetLeft;
    var current = element.offsetParent;
    while (current !== null) {
        actualLeft += current.offsetLeft;
        current = current.offsetParent;
        if (current == targetParent)
            break;

    }
    return actualLeft;
}
function getElementTop(element, targetParent) {
    var actualTop = element.offsetTop;
    var current = element.offsetParent;
    while (current !== null) {
        actualTop += current.offsetTop;
        current = current.offsetParent;
        if (current == targetParent)
            break;
    }
    return actualTop;
}
//取Js变量的类型
function getParamType(param) {
    return ((_t = typeof (param)) == "object" ? Object.prototype.toString.call(param).slice(8, -1) : _t).toLowerCase();
}

//如果只需要firefox下支持onmousewheel功能，只需要以下代码：
function listener_onmousewheel(e) {
    e = e || event;
    if (Ext.isGecko) {
        var parentDIV = e.findParent('div');
        if (parentDIV != null) {
            parentDIV = parentDIV.parentNode;
        }
        addEventListener('DOMMouseScroll', function (e) {
            var onmousewheel = e.target.onmousewheel;
            if (onmousewheel) {
                if (e.preventDefault) e.preventDefault();
                e.returnValue = false;    //禁止页面滚动

                if (typeof e.target.onmousewheel != 'function') {
                    //将onmousewheel转换成function
                    eval('window._tmpFun = function(event){' + onmousewheel + '}');
                    e.target.onmousewheel = window._tmpFun;
                    window._tmpFun = null;
                }
                // 不直接执行是因为若onmousewheel(e)运行时间较长的话，会导致锁定滚动失效，使用setTimeout可避免
                //setTimeout(function(){
                e.target.onmousewheel(e);
                // },1);
            }
        }, false);
    }
}
//保留小数
function fomatFloat(src, pos) {
    return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
}

function get_length(s) {
    var char_length = 0;
    for (var i = 0; i < s.length; i++) {
        var son_char = s.charAt(i);
        encodeURI(son_char).length > 2 ? char_length += 1 : char_length += 0.5;
    }
    return char_length;
}


function cut_str(str, len) {
    var char_length = 0;
    for (var i = 0; i < str.length; i++) {
        var son_str = str.charAt(i);
        encodeURI(son_str).length > 2 ? char_length += 1 : char_length += 0.5;
        if (char_length >= len) {
            var sub_len = char_length == len ? i + 1 : i;
            return str.substr(0, sub_len);
            break;
        }
    }
}

function addCookie(objName, objValue, objHours) {//添加cookie 
    if (objHours > 0) {
        var exp = new Date();
        exp.setTime(exp.getTime() + objHours * 60 * 60 * 1000);
        document.cookie = objName + "=" + escape(objValue) + ";path=/;expires=" + exp.toGMTString();
    } else {
        document.cookie = objName + "=" + escape(objValue);
    }
}

function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = escape("exit"); // getCookie(name);
    document.cookie = name + "=" + cval;
}

function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;
}
function GetWinHeight() {
    var Height = window.innerHeight;
    if (typeof Height != 'number') {
        if (document.compatMode == 'CSS1Compat') Height = document.documentElement.clientHeight;
        else Height = document.body.clientHeight;
    }
    return Height;
}
//日期格式化
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function drag(current, move) {
    current.on("mousedown", function (event) {
        $(current).css("cursor", "move");
        var evt = event || window.event;
        var offset = $(move).offset();
        var dialogX = offset.left-$(window).scrollLeft();
        var dialogY = offset.top - $(window).scrollTop();
        var mx = evt.pageX;
        var my = evt.pageY;
        $(current).on("mousemove", function (evt) {
            var evt = evt || window.event;
            var x = evt.pageX;
            var y = evt.pageY;
            var moveX = dialogX + (x - mx);
            var moveY = dialogY + (y - my);
            $(move).css({ "margin-left": moveX, "margin-top": moveY });
            window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
        });
    });
    $(document).on("mouseup", function () {
        $(current).css("cursor", "default");
        $(current).off("mousemove");

    })
}

function isIE() { //ie?   判断是否是ie
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}
