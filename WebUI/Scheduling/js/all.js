
function $g(elementId)
{
    if (null == elementId)
        return null;
    return document.getElementById(elementId);
}

var defaultSettings =
{
    datarow_onmouseover_backgroundColor: "#bde17d",
    datarow_onmouseout_backgroundColor: "#FFFFFF",
    tableHead_onmouseover_backgroundColor: "#fffdd7",
    tableHead_onmouseout_backgroundColor: "e9f3fc"
}

// Browser Type
var Browser = {};
Browser.agt = navigator.userAgent.toLowerCase();
Browser.isW3C = document.getElementById ? true : false;
Browser.isIE = ((Browser.agt.indexOf("msie") != -1) && (Browser.agt.indexOf("opera") == -1) && (Browser.agt.indexOf("omniweb") == -1));
Browser.isNS6 = Browser.isW3C && (navigator.appName == "Netscape");
Browser.isOpera = Browser.agt.indexOf("opera") != -1;
Browser.isGecko = Browser.agt.indexOf("gecko") != -1;
Browser.ieTrueBody = function()
{
    return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body;
};

//firefox innerText define
if (Browser.isNS6)
{
    HTMLElement.prototype.__defineGetter__("innerText",
  function()
  {
      return this.textContent;
  }
  );
    HTMLElement.prototype.__defineSetter__("innerText",
  function(sText)
  {
      this.textContent = sText;
  }
  );
}

function parseDate(dateStr)
{
    var dateObj = null;
    if (checkDate(dateStr))
        dateObj = new Date(Date.parse(dateStr.replace(/-/g, "/")));
    return dateObj;
}

function addOnloadEvent(fnc)
{
    if (typeof window.addEventListener != "undefined")
        window.addEventListener("load", fnc, false);
    else if (typeof window.attachEvent != "undefined")
    {
        window.attachEvent("onload", fnc);
    }
    else
    {
        if (window.onload != null)
        {
            var oldOnload = window.onload;
            window.onload = function(e)
            {
                oldOnload(e);
                window[fnc]();
            };
        }
        else
            window.onload = fnc;
    }
}

//get a random number
function GetRandom()
{
    var randomValue = Math.random();
    randomValue = Math.round(randomValue * 1000000);
    return randomValue;
}

function setCookie(key, value, expiresSeconds)
{
    var d = new Date();
    if (expiresSeconds && !isNaN(expiresSeconds))
        d.setTime(d.getTime() + expiresSeconds);
    else
        d.setTime(d.getTime() + 2678400000);
    document.cookie = key + "=" + value + ";expires" + d.toUTCString();
}

function getCookie(key)
{
    var search = key + "=";
    begin = document.cookie.indexOf(search);
    if (begin != -1)
    {
        begin += search.length;
        end = document.cookie.indexOf(";", begin);
        if (end == -1)
            end = document.cookie.length;
        return document.cookie.substring(begin, end);
    }
}

function getHeadElement()
{
    var head = document.getElementsByTagName('head')[0];
    if (head == null)
    {
        head = document.getElementsByTagName('body')[0];
    }
    return head;
}

//change url
function ChangeSrc(sender, imgUrl)
{
    if (sender)
    {
        sender.src = "";
        sender.src = imgUrl;
    }
}

function SetForeColor(sender, color)
{
    if (sender)
        sender.style.color = color;
}

function clearOptions(selectObj, defaultOp) {
    if (!selectObj)
        return false;
    var optionsLen = selectObj.options.length;
    for (var i = optionsLen - 1; i >= 0; i--)
    {
        selectObj.removeChild(selectObj.options[i]);
    }
    if (defaultOp)
        selectObj.options[0] = new Option("--请选择--", "-1");
}

function clearControls(control)
{
    if (!control)
        return;
    var childCount = control.childNodes.length;
    for (var i = childCount - 1; i >= 0; i--)
        control.removeChild(control.childNodes[i]);
}

//change css
function ChangeClass(sender, className)
{
    if (sender)
    {
        sender.setAttribute("class", className);
        sender.setAttribute("className", className);
    }
}

function setBackgroundPosition(target, bottom, center)
{
    if (target)
    {
        target.style.backgroundPosition = bottom + "px" + " " + center + "px";
    }
}

function EnterEventJump(TargetControlID)
{
    if (event.keyCode == 13)
    {
        event.keyCode = 0;
        var targetControl = $g(TargetControlID);
        if (targetControl)
            targetControl.click();
        return false;
    }
}

function ExecScript(scriptStr)
{
    eval(scriptStr);
}

String.prototype.trim = function()
{
    return this.replace(/(^\s*)|(\s*$g)/g, "");
}

function showProperties(obj)
{
    var properties = "";
    for (var p in obj)
        properties += p + ":" + obj[p] + ",";
    alert(properties);
}

function getPosition(targetID)
{
    var targetControl = $g(targetID);
    if (targetControl)
    {

        var location = new Point(targetControl.offsetLeft, targetControl.offsetTop);
        var offParent = targetControl.offsetparent;

        while (offParent)
        {
            location.x += offParent.offsetLeft;
            location.y += offParent.offsetTop;
            offParent = offParent.offsetParent;
        }

        return location;
    }
}

function pngFix(imgObj)
{
    if ($.browser.msie && $.browser.version == 6)
    {
        if (!(imgObj instanceof Object))
            imgObj = $g(imgObj);
        if (imgObj)
        {
            var es = imgObj.style;
            //    imgObj.height = 20;
            //    imgObj.width = 20;
            es.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + imgObj.src + "',sizingMethod='crop')";
            imgObj.src = "script/images/clear.gif";
        }
    }
}

function setUrl(url, paramName, value)
{
    var c = url.indexOf("?") == -1 ? "?" : "&";
    var indexOfParam = -1;
    if (c != "?")
    {
        indexOfParam = url.indexOf("?" + paramName + "=");
        if (indexOfParam == -1)
        {
            indexOfParam = url.indexOf("&" + paramName + "=");
            if (indexOfParam != -1)
            {
                c = "&";
            }
        } else
            c = "?";
    }
    var newParam = c + paramName + "=" + escape(value);
    if (indexOfParam != -1)
    {
        var paraEndIndex = url.indexOf("&", indexOfParam + 1);
        if (paraEndIndex == -1)
            paraEndIndex = url.length;
        var theParamStr = url.substring(indexOfParam, paraEndIndex);
        url = url.replace(theParamStr, newParam)
    } else
        url += newParam;
    return url;
}

function onmouserver_row(sender)
{
    sender.style.backgroundColor = "#fffdd7";
}

function onmouseout_row(sender)
{
    sender.style.backgroundColor = "#FFFFFF";
}

function lockWindow(lock)
{
    var lockPanel = $g("lockPanel");
    if (!lockPanel)
    {
        lockPanel = document.createElement("DIV");
        lockPanel.id = "lockPanel";
        document.body.appendChild(lockPanel);
    }
    var selects = document.getElementsByTagName("select");

    if (lock == true)
        lockPanel.style.display = "block";
    else
        lockPanel.style.display = "none";

    for (var i = 0; i < selects.length; i++)
    {
        selects[i].style.display = lock ? "none" : "inline";
    }
}

function dispalyData(dataPanelID, lockWin)
{
    var dataPanel = $g(dataPanelID);
    if (dataPanel)
    {
        if (lockWin)
            lockWindow(true);
        dataPanel.style.display = "block";
    }
}

function hideData(dataPanelID, lockWin)
{
    var dataPanel = $g(dataPanelID);
    if (dataPanel)
    {
        if (lockWin)
            lockWindow(false);
        dataPanel.style.display = "none";
    }
}

function dataRow_onmouserver(sender)
{
    setTableRowBackgroundColor(sender, defaultSettings.datarow_onmouseover_backgroundColor);
}

function dataRow_onmouseout(sender)
{
    setTableRowBackgroundColor(sender, defaultSettings.datarow_onmouseout_backgroundColor);

}

function tableHead_onmouseover(sender, toCells)
{
    sender.style.backgroundColor = defaultSettings.tableHead_onmouseover_backgroundColor;

    if (toCells)
        setTableColumnBackgroundColor(sender, defaultSettings.tableHead_onmouseover_backgroundColor);
}

function tableHead_onmouseout(sender, toCells)
{
    sender.style.backgroundColor = defaultSettings.tableHead_onmouseout_backgroundColor;
    if (toCells)
        setTableColumnBackgroundColor(sender, defaultSettings.datarow_onmouseout_backgroundColor);
}

function getControlIndexInParent(control)
{
    var headRow = control.parentNode;
    var columnsCount = headRow.childNodes.length;
    for (var i = 0; i < columnsCount; i++)
    {
        if (headRow.childNodes[i] == control)
        {
            return i;
        }
    }
    return -1;
}

function setTableRowBackgroundColor(row, color)
{
    var cellsCount = row.cells.length;
    for (var i = 0; i < cellsCount; i++)
        row.cells[i].style.backgroundColor = color;
}

function setTableColumnBackgroundColor(headCell, color)
{
    var index = getControlIndexInParent(headCell);
    if (index == -1)
        return;
    var table = headCell.parentNode.parentNode;
    for (var i = 1; i < table.rows.length; i++)
    {
        table.rows[i].cells[index].style.backgroundColor = color;
    }
}


function mouseX(evt)
{
    if (evt.pageX)
        return evt.pageX;
    else if (evt.clientX)
        return evt.clientX + (document.documentElement.scrollLeft ?
       document.documentElement.scrollLeft :
       document.body.scrollLeft);
    else
        return null;
}

function mouseY(evt)
{
    if (evt.pageY)
        return evt.pageY;
    else if (evt.clientY)
        return evt.clientY + (document.documentElement.scrollTop ?
       document.documentElement.scrollTop :
       document.body.scrollTop);
    else
        return null;
}

function mousePosition(ev)
{
    if (ev.pageX || ev.pageY)
    {
        return { x: ev.pageX, y: ev.pageY };
    }
    return { x: ev.clientX + document.body.scrollLeft - document.body.clientLeft, y: ev.clientY + document.body.scrollTop - document.body.clientTop };
}

function getEventSource(evt)
{
    evt = getEvent(evt);
    if (evt)
    {
        var eventSource = (evt.target) ? evt.target : evt.srcElement;
        return eventSource;
    }
    return null;
}

function getEvent(evt)
{
    evt = (evt) ? evt : ((window.event) ? window.event : "");
    return evt;
}

function togglePanel(targetId, sender)
{
    var targetPanel = $g(targetId);
    if (targetPanel.style.display == "none")
    {
        targetPanel.style.display = "block";
        ChangeClass(sender, "tooglePanel-expand");
    }
    else
    {
        targetPanel.style.display = "none";
        ChangeClass(sender, "tooglePanel-collapse");
    }
}

function timeUpDown(targetControlID, targetButtonUpID, targetButtonDownID)
{
    this.targetControl = $g(targetControlID);
    this.targetButtonUp = $g(targetButtonUpID);
    this.targetButtonDown = $g(targetButtonDownID);
    var refObj = this;
    this.targetButtonUp.onclick = function()
    {
        refObj.up();
    };
    //    this.targetButtonUp.onmousedown=function(){
    //        refObj.up();
    //    };
    this.targetButtonDown.onclick = function()
    {
        refObj.down();
    };
    //    this.targetButtonDown.onmousedown=function(){
    //        refObj.down();
    //    };
}

timeUpDown.prototype.up = function()
{
    var dateStr = this.targetControl.value;
    var dateObj = new Date();
    if (checkDate(dateStr))
        dateObj = new Date(Date.parse(dateStr.replace(/-/g, "/")));
    dateObj.setDate(dateObj.getDate() + 1);
    var timeStr = dateObj.getFullYear() + "-" + (dateObj.getMonth() + 1) + "-" + dateObj.getDate();
    this.targetControl.value = timeStr;
}

timeUpDown.prototype.down = function()
{
    var dateStr = this.targetControl.value;
    var dateObj = new Date();
    if (checkDate(dateStr))
        dateObj = new Date(Date.parse(dateStr.replace(/-/g, "/")));
    dateObj.setDate(dateObj.getDate() - 1);
    var timeStr = dateObj.getFullYear() + "-" + (dateObj.getMonth() + 1) + "-" + dateObj.getDate();
    this.targetControl.value = timeStr;
}

function numericUpDown(targetControlID, targetButtonUpID, targetButtonDownID, min, max, cyc)
{
    this.targetControl = $g(targetControlID);
    this.targetButtonUp = $g(targetButtonUpID);
    this.targetButtonDown = $g(targetButtonDownID);
    this.min = 0;
    this.max = 100;
    this.cyc = cyc;
    if (!isNaN(min))
        this.min = min;
    else if ("infinity" == min)
        this.min = "infinity";
    if (!isNaN(max))
        this.max = max;
    else if ("infinity" == max)
        this.max = "infinity";
    var refObj = this;
    this.targetButtonUp.onmousedown = function()
    {
        refObj.up();
    };
    this.targetButtonDown.onmousedown = function()
    {
        refObj.down();
    };
    //    ondblclick
    this.targetControl.onblur = function()
    {
        refObj.checkInput();
    }
}
numericUpDown.prototype.up = function()
{
    var num = this.targetControl.value;
    if (isNaN(num) || num == 0)
        num = 0;
    else
        num = parseInt(num);
    if (!isNaN(this.max))
    {
        if (num < parseInt(this.max))
            num++;
        else if (this.cyc)
        {
            if (!isNaN(this.min))
                num = this.min;
        }
    }
    else if ("infinity" == this.max)
        num++;
    this.targetControl.value = num;
}

numericUpDown.prototype.down = function()
{
    var num = this.targetControl.value;
    if (isNaN(num) || num == 0)
        num = 0;
    else
        num = parseInt(num);
    if (!isNaN(this.min))
    {
        if (num > parseInt(this.min))
            num--;
        else if (this.cyc)
        {
            if (!isNaN(this.max))
                num = this.max;
        }
    }
    else if ("infinity" == this.min)
        num--;
    this.targetControl.value = num;
}

numericUpDown.prototype.toMax = function()
{

}

numericUpDown.prototype.checkInput = function()
{
    var inputValue = this.targetControl.value;
    if (isNaN(inputValue) || inputValue == 0)
        inputValue = 0;
    else
        inputValue = parseInt(inputValue);
    if (!isNaN(this.max) && inputValue > this.max)
        inputValue = this.max;
    if (!isNaN(this.min) && inputValue < this.min)
        inputValue = this.min;
    this.targetControl.value = inputValue;
}

var doAjax =
{
    send: function(url, cb)
    {
        var xmlHttp;
        if (window.ActiveXObject)
        {
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        else if (window.XMLHttpRequest)
        {
            xmlHttp = new XMLHttpRequest();
        }
        xmlHttp.open("GET", url + (url.indexOf("?") > 0 ? "&" : "?") + "randnum=" + Math.random());
        xmlHttp.onreadystatechange = function()
        {
            if (xmlHttp.readyState == 4 && (xmlHttp.status == 200 || xmlHttp.status == 304))
            {
                cb(xmlHttp.responseText);
            }
        };

        xmlHttp.send(null);
    }
};

//Check DateTime String
function checkDate(sDate)
{

    var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    var iaDate = new Array(3);
    var year, month, day;

    if (arguments.length != 1) return false;
    iaDate = sDate.toString().split("-");
    if (iaDate.length != 3) return false;
    if (iaDate[1].length > 2 || iaDate[2].length > 2) return false;

    year = parseFloat(iaDate[0]);
    month = parseFloat(iaDate[1]);
    day = parseFloat(iaDate[2]);

    if (year < 1900 || year > 2100) return false;
    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
    if (month < 1 || month > 12) return false;
    if (day < 1 || day > iaMonthDays[month - 1]) return false;

    return true;
}


/*
* DownData Model
**/
function DownData(dataID, imgUrl, title, width, height, isTableData, removed, category, downTime, releaseTime, barbarismUrl, categoryId)
{
    this.dataID = dataID;
    this.imgUrl = imgUrl;
    this.title = title;
    this.width = width;
    this.height = height;
    this.isTableData = isTableData;
    this.removed = removed;
    this.category = category;
    this.downTime = downTime;
    this.releaseTime = releaseTime;
    this.barbarismUrl = barbarismUrl;
    this.categoryId = categoryId;
}

/*
* Category Model
**/
function category(categoryID, categoryName, parentID, isFinallyNode, backward, pri)
{
    this.categoryID = categoryID;
    this.categoryName = categoryName;
    this.parentID = parentID;
    this.isFinallyNode = isFinallyNode;
    this.backward = backward;
    this.pri = pri;
}

function County(id, name, sites)
{
    this.id = id;
    this.name = name;
    this.sites = sites;
}

function Site(id, name, countyID, code, siteType,orderId)
{
    this.id = id;
    this.name = name;
    this.countyID = countyID;
    this.code = code;
    this.siteType = siteType;
    this.orderId = orderId;
}


function SiteGroup(id, groupId, groupName, readOnly)
{
    this.id = id;
    this.groupID = groupId;
    this.groupName = groupName;
    this.readOnly = readOnly;
    this.Items = new Array();
}

function Parameter(id, code, name)
{
    this.id = id;
    this.code = code;
    this.name = name;
}

function SiteData()
{
    this.site = null;
    this.DataCollection = new Array();
}

function MonitoringData()
{
    this.api = 0;
    this.thickness = 0;
    this.parameter = null;
}

/*
* TabControl Class
**/
//function tabControl(tabPages)
//{
//    this.pages = new Array();
//    for (var i = 0; i < tabPages.length; i++)
//    {
//        this.pages[i] = $g(tabPages[i]);
//    }
//    if (this.pages.length > 0)
//    {
//        if (this.pages[0])
//            this.pages[0].className = "pageOn";
//        this.pageIndex = 0;
//    }
//}

//tabControl.prototype.setPage = function(pageIndex)
//{
//    if (pageIndex == this.pageIndex)
//        return;
//    for (var i = 0; i < this.pages.length; i++)
//    {
//        this.pages[i].className = "pageOff";
//    }
//    this.pages[pageIndex].className = "pageOn";
//    this.pageIndex = pageIndex;
//}


/*
* Drag Class
*/
function Point(x, y)
{
    this.x = x;
    this.y = y;
}

function Drag(mover, eventSrc, defaultPoint)
{
    this.init(mover, eventSrc, defaultPoint);
}

Drag.prototype.eventSrc = null;
Drag.prototype.mover = null;
Drag.prototype.mousePosition_before = new Point(0, 0);
Drag.prototype.start = false;

Drag.prototype.init = function(mover, eventSrc, defaultPoint)
{
    this.eventSrc = $g(eventSrc);
    if (!this.eventSrc)
        return;
    this.mover = $g(mover);
    if (!this.mover)
        return;

    if (defaultPoint)
    {
        this.mover.style.left = defaultPoint.x;
        this.mover.style.top = defaultPoint.y;
    }
    if (this.eventSrc)
    {
        this.eventSrc.style.cursor = "move";
        var instance = this;
        this.eventSrc.onmousedown = function() { instance.startDrag(); }
        this.eventSrc.onmousemove = function() { instance.drag(); }
        this.eventSrc.onmouseup = function() { instance.stopDrag(); };
    }
};


Drag.prototype.startDrag = function(evt)
{
    evt = (evt) ? evt : ((window.event) ? window.event : "")
    if (evt.button == 1)
    {
        this.mousePosition_before = new Point(event.clientX, event.clientY);
        this.eventSrc.setCapture();
        this.start = true;
    }
};

Drag.prototype.drag = function()
{
    if (this.start)
    {
        var location = new Point(parseInt(this.mover.style.left), parseInt(this.mover.style.top));
        var mousePosition = new Point(event.clientX, event.clientY);
        location.x += mousePosition.x - this.mousePosition_before.x;
        location.y += mousePosition.y - this.mousePosition_before.y;

        this.mousePosition_before = mousePosition;

        //$g("position").innerText=location.x+","+location.y;

        if (location.x >= 0)
            this.mover.style.left = location.x;
        if (location.y >= 0)
            this.mover.style.top = location.y;
    }
};

Drag.prototype.stopDrag = function()
{
    if (this.start)
    {
        this.eventSrc.releaseCapture();
        this.start = false;
    }
};

/*
*  menuStrip Class
*/

function menuStrip(referencePoint)
{
    this.referencePoint = referencePoint;
}

function menuItem(text, click)
{
    this.text = text;
    this.onclick = click;
}


menuStrip.prototype.menuItems = new Array();
menuStrip.prototype.referencePoint = null;
menuStrip.prototype.menu_obj = null;
menuStrip.prototype.showing = false;

menuItem.prototype.text = "";
menuItem.prototype.onclick = null;
menuItem.prototype.navigateUrl = "";
menuItem.prototype.target = "";
menuItem.prototype.nenu_obj = null;

menuStrip.prototype.show = function()
{
    if (this.referencePoint)
    {
        var thePoint = getPosition(this.referencePoint);
        this.menu_obj.style.left = thePoint.x;
        this.menu_obj.style.top = thePoint.y;
    }
    this.menu_obj.style.display = "block";
    this.showing = true;
}

menuStrip.prototype.hide = function()
{

}

menuStrip.prototype.init = function()
{

    this.menu_obj = document.createElement("DIV");
    this.menu_obj.id = "menuStrip" + GetRandom();
    this.menu_obj.className = "menuStrip";
    this.menu_obj.tag = "menu";
    document.body.appendChild(this.menu_obj);
    var tempMenu = this;
    document.documentElement.onclick = (function(event)
    {
        var sender = getEventSource(event);
        if ("menu" != sender.tag && !tempMenu.showing)
            tempMenu.menu_obj.style.display = "none";
        tempMenu.showing = false;
    })

    for (var i = 0; i < this.menuItems.length; i++)
    {
        var item = this.menuItems[i];

        if (item.onclick)
        {
            item.menu_obj = document.createElement("DIV");
            item.menu_obj.id = this.menu_obj.id + "$gitem" + GetRandom();
            item.menu_obj.onclick = item.onclick;
        }
        else if (item.navigateUrl != 0)
        {
            item.menu_obj = document.createElement("A");
            item.menu_obj.href = item.navigateUrl;
            item.menu_obj.target = item.target;
        }
        item.menu_obj.className = "menuItem";
        item.menu_obj.onmouseover = function(event)
        {
            var sender = getEventSource(event);
            sender.className = 'menuItem_hover';
        };
        item.menu_obj.onmouseout = function(event)
        {
            var sender = getEventSource(event);
            sender.className = 'menuItem';
        };
        item.menu_obj.innerText = item.text;
        this.menu_obj.appendChild(item.menu_obj);
    }
}

function Receiver(name, address)
{
    this.name = name;
    this.address = address;
}

function getCenterPosition(pos, control) {
    
    var wnd = $(window), doc = $(document),
	    pTop = doc.scrollTop(), pLeft = doc.scrollLeft(),
	    minTop = pTop;

    if ($.inArray(pos, ['center','top','right','bottom','left']) >= 0) {
	    pos = [
		    pos == 'right' || pos == 'left' ? pos : 'center',
		    pos == 'top' || pos == 'bottom' ? pos : 'middle'
	    ];
    }
    if (pos.constructor != Array) {
	    pos = ['center', 'middle'];
    }
    if (pos[0].constructor == Number) {
	    pLeft += pos[0];
    } else {
	    switch (pos[0]) {
		    case 'left':
			    pLeft += 0;
			    break;
		    case 'right':
		        pLeft += wnd.width() - control.outerWidth();
			    break;
		    default:
		    case 'center':
		        pLeft += (wnd.width() - control.outerWidth()) / 2;
	    }
    }
    if (pos[1].constructor == Number) {
	    pTop += pos[1];
    } else {
	    switch (pos[1]) {
		    case 'top':
			    pTop += 0;
			    break;
		    case 'bottom':
		        pTop += wnd.height() - control.outerHeight();
			    break;
		    default:
		    case 'middle':
		        pTop += (wnd.height() - control.outerHeight()) / 2;
	    }
    }
    pTop = Math.max(pTop, minTop);
    return {top: pTop, left: pLeft};
}