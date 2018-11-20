//相应文本框的点击事件
function Select(sender, fldValue) {

    var textValue = Ext.getDom(sender).value;
    var textLength = textValue.length;
    var divUsers = Ext.getDom("filter");
    var users = Ext.util.JSON.decode(userJson);
    var userArray = users[fldValue].split('|');
    divUsers.innerHTML = "";
    divUsers.style.left = getElementLeft(sender, divUsers.parent) + "px";
    divUsers.style.top = getElementTop(sender, divUsers.parent) + sender.offsetHeight + "px";

    for (i = 0; i < userArray.length; i++) {
        var childDiv = document.createElement("DIV");
        if (userArray[i].substr(0, textLength) == textValue || textValue == "") {
            childDiv.innerHTML = userArray[i];
            childDiv.className = "userLi";
            childDiv.onmousemove = function () { bgcolor(this); };
            childDiv.onmouseout = function () { nocolor(this); };
            childDiv.onmousedown = function () { pick(sender, this); };
            divUsers.appendChild(childDiv);
        }
    }
    divUsers.style.display = "block";
}
//当鼠标移进时字体背景颜色改变
function bgcolor(obj) {
    obj.style.background = "#436EEE";
    obj.style.color = "#000000";
}
//当鼠标移出时字体背景颜色改变
function nocolor(obj) {
    obj.style.background = "";
    obj.style.color = "#000000";
}
//触发窗体的click事件
function hide(evt) {
    var eventSource = getEventSource(evt);
    if (eventSource.id != "H03" && eventSource.id != "H04" && eventSource.id != "H01" && eventSource.id != "H02") {
        var obj = Ext.getDom("filter");
        obj.style.display = "none";
    }
    if (eventSource.className.indexOf("divInputType") < 0) {
        var obj = Ext.getDom("comforecastPrepare");
        obj.style.display = "none";
    }
}
//改变日期成功后,，刷新获取的值
function changeDateSucessedPast(result) {
    for (var obj in result) {
        divContaner = Ext.getDom(obj);
        if (divContaner != null) {
            if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                divContaner.value = result[obj];
            else {
                if (divContaner != "" && divContaner != null)
                    divContaner.innerHTML = result[obj]; //日平均值
            }
        }
    }
}

function changeDateSucessed(result) {
    for (var obj in result) {
        divContaner = Ext.getDom(obj);
        if (divContaner != null) {
            if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                divContaner.value = result[obj];
            else {
                if (divContaner != "" && divContaner != null)
                    divContaner.innerHTML = result[obj]; //日平均值
            }
        }
    }
}

function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();
    return currentdate;
}

//获取日期中文字符串
function getFormatDate(type) {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (type == "tomorrow") {

        date.setDate(date.getDate() + 1);
        month = date.getMonth() + 1;
        strDate = date.getDate();
        
    }
    else if (type == "Yesterday") {
        date.setDate(date.getDate() - 1);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    else if (type == "after1") {
        date.setDate(date.getDate() + 2);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    else if (type == "after2") {
        date.setDate(date.getDate() + 3);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    else if (type == "Before") {
        date.setDate(date.getDate() - 2);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + "年" + month + "月" + strDate
            + "日";
    return currentdate;
}

function getFormatDateForAQIPeriod(type) {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (type == "tomorrow") {

        date.setDate(date.getDate() + 1);
        month = date.getMonth() + 1;
        strDate = date.getDate();

    }
    else if (type == "Yesterday") {
        date.setDate(date.getDate() - 1);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    else if (type == "after1") {
        date.setDate(date.getDate() + 2);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    else if (type == "after2") {
        date.setDate(date.getDate() + 3);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
    else if (type == "Before") {
        date.setDate(date.getDate() - 2);
        month = date.getMonth() + 1;
        strDate = date.getDate();
    }
//    if (month >= 1 && month <= 9) {
//        month = "0" + month;
//    }
//    if (strDate >= 0 && strDate <= 9) {
//        strDate = "0" + strDate;
//    }
    var currentdate = date.getFullYear() + "年" + month + "月" + strDate
            + "日";
    return currentdate;
}


//获取日期包含时间中文字符串
function getFormatDateTime(type) {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var hour = date.getHours();
    var strDate = date.getDate();
    if (type == "tomorrow") {
        strDate = strDate + 1;
    }
    else if (type == "after1") {
        strDate = strDate + 2;
    }
    else if (type == "after2") {
        strDate = strDate + 3;
    }
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + "年" + month + "月" + strDate
            + "日" + hour+"时";
    return currentdate;
}


//获取日期包含时间中文字符串
function getFormatDateTimeFromDateString(dateString) {
    
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var hour = date.getHours();
    var strDate = date.getDate();
    if (type == "tomorrow") {
        strDate = strDate + 1;
    }
    else if (type == "after1") {
        strDate = strDate + 2;
    }
    else if (type == "after2") {
        strDate = strDate + 3;
    }
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + "年" + month + "月" + strDate
            + "日" + hour + "时";
    return currentdate;
}

function getNowDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
    return currentdate;
}

//获取中文月份和日期
function getMonthAndDateCN(date) {   
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = month + "月" + strDate + "日";
    return currentdate;
}

function getTomMonthAndDate() {
    var date = new Date();
    var month = date.getMonth() + 1;
    var strDate = date.getDate()+1;
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = month + "月" + strDate + "日";
    return currentdate;
}

function getOzoneTomMonthAndDate() {
    var date = new Date();
    var month = date.getMonth() + 1;
    var strDate = date.getDate() + 1;
//    if (month >= 1 && month <= 9) {
//        month = "0" + month;
//    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = month + "月" + strDate + "日";
    return currentdate;
}
function GetDateStr(AddDayCount) {

    var dd = new Date();
    var dayCount = parseInt(AddDayCount);
    dd.setDate(dd.getDate() + dayCount); //获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1; //获取当前月份的日期
    var d = dd.getDate();
    return y + "年" + m + "月" + d+"日";
}

function GetDateStrNoYear(AddDayCount) {
    var dd = new Date();
    var dayCount = parseInt(AddDayCount);
    dd.setDate(dd.getDate() + dayCount); //获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1; //获取当前月份的日期
    var d = dd.getDate();
    return  m + "月" + d + "日";
}

function GetDateStrNoYearNoCn(AddDayCount) {
    var dd = new Date();
    var dayCount = parseInt(AddDayCount);
    dd.setDate(dd.getDate() + dayCount); //获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1; //获取当前月份的日期
    var d = dd.getDate();
    return y + "-" + m + "-" + d;
}

function GetDateWeekDay(AddDayCount) {
    var dd = new Date();
    var dayCount = parseInt(AddDayCount);
    dd.setDate(dd.getDate() + dayCount); //获取AddDayCount天后的日期
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1; //获取当前月份的日期
    var d = dd.getDate();
    var numOfWeek = dd.getDay();
    switch (numOfWeek) {
        case 0:
            day = "周日";
            break;
        case 1:
            day = "周一";
            break;
        case 2:
            day = "周二";
            break;
        case 3:
            day = "周三";
            break;
        case 4:
            day = "周四";
            break;
        case 5:
            day = "周五";
            break;
        case 6:
            day = "周六";
            break;
    }
    return day;
}

//获取年份
function getDateYear() {
    var date = new Date();
    var year = date.getFullYear()
    return year;
}
//获取月份
function getDateMonth() {
    var date = new Date();  
    var month = date.getMonth() + 1;
    return month;
}

//获取日期的日
function getDateDay() {
    var date = new Date();
    var strDate = date.getDate();
    return strDate;
}

var EncodeURI = function (unzipStr, isCusEncode) {
    if (isCusEncode) {
        var zipArray = new Array();
        var zipstr = "";
        var lens = new Array();
        if (unzipStr != null && unzipStr != "" && unzipStr != "undefined") {
            for (var i = 0; i < unzipStr.length; i++) {
                var ac = unzipStr.charCodeAt(i);
                zipstr += ac;
                lens = lens.concat(ac.toString().length);
            }
            zipArray = zipArray.concat(zipstr);
            zipArray = zipArray.concat(lens.join("O"));
            return zipArray.join("N");
        }
        return unzipStr;
    } else {
        //return encodeURI(unzipStr);
        var zipstr = "";
        var strSpecial = "!\"#$%&'()*+,/:;<=>?[]^`{|}~%";
        var tt = "";
        if (unzipStr != null && unzipStr != "" && unzipStr != "undefined") {
            for (var i = 0; i < unzipStr.length; i++) {
                var chr = unzipStr.charAt(i);
                var c = StringToAscii(chr);
                tt += chr + ":" + c + "n";
                if (parseInt("0x" + c) > 0x7f) {
                    zipstr += encodeURI(unzipStr.substr(i, 1));
                } else {
                    if (chr == " ")
                        zipstr += "+";
                    else if (strSpecial.indexOf(chr) != -1)
                        zipstr += "%" + c.toString(16);
                    else
                        zipstr += chr;
                }
            }
        }
        return zipstr;
    }
}

var DecodeURI = function (zipStr, isCusEncode) {
    if (zipStr == null || zipStr == "" || zipStr == "undefined") {
        return zipStr;
    }
    if (isCusEncode) {
        var zipArray = zipStr.split("N");
        var zipSrcStr = zipArray[0];
        var zipLens;
        if (zipArray[1]) {
            zipLens = zipArray[1].split("O");
        } else {
            zipLens.length = 0;
        }

        var uzipStr = "";

        for (var j = 0; j < zipLens.length; j++) {
            var charLen = parseInt(zipLens[j]);
            uzipStr += String.fromCharCode(zipSrcStr.substr(0, charLen));
            zipSrcStr = zipSrcStr.slice(charLen, zipSrcStr.length);
        }
        return uzipStr;
    } else {
        //return decodeURI(zipStr);
        var uzipStr = "";

        for (var i = 0; i < zipStr.length; i++) {
            var chr = zipStr.charAt(i);
            if (chr == "+") {
                uzipStr += " ";
            } else if (chr == "%") {
                var asc = zipStr.substring(i + 1, i + 3);
                if (parseInt("0x" + asc) > 0x7f) {
                    uzipStr += decodeURI("%" + asc.toString() + zipStr.substring(i + 3, i + 9).toString()); ;
                    i += 8;
                } else {
                    uzipStr += AsciiToString(parseInt("0x" + asc));
                    i += 2;
                }
            } else {
                uzipStr += chr;
            }
        }
        return uzipStr;
    }
}
var StringToAscii = function (str) {
    return str.charCodeAt(0).toString(16);
}

var AsciiToString = function (asccode) {
    return String.fromCharCode(asccode);
}

function CalculateAQLLevel(aqiValue)
        {
            var strAQLLevel = "";
            if (aqiValue != null)
            {
                var intAQI = parseInt(aqiValue);
                if (intAQI > 0 && intAQI <= 50)
                {
                    strAQLLevel="优";
                }
                else if (intAQI > 50 && intAQI <= 100)
                {
                    strAQLLevel = "良";
                }
                else if (intAQI > 100 && intAQI <= 150)
                {
                    strAQLLevel = "轻度污染";
                }
                else if (intAQI > 150 && intAQI <= 200)
                {
                    strAQLLevel = "中度污染";
                }
                else if (intAQI > 200 && intAQI <= 300)
                {
                    strAQLLevel = "重度污染";
                }
                else if (intAQI > 300)
                {
                    strAQLLevel = "严重污染";
                }                               
            }
            return strAQLLevel;
        }

       

        function UpdateWordProduct(functionName, fileName) {

            //    if (wpid == "" || wpid == "0") {
            //        alert("参数有误！");
            //        return false;
            //    }

            //    var forcastClerk = $("#txtHideUserName").val();
            //    var forcastTime = $("#txtForecastTime").val();
            var webObj = document.getElementById("WebOffice1");
            var ret;
            webObj.HttpInit(); 	//初始化Http引擎
            // 添加相应的Post元素 
            webObj.HttpAddPostString("wpid", "645");

            //根据功能的名称，确定存储的临时文件夹名称
            webObj.HttpAddPostString("FunctionFile", functionName);
            webObj.HttpAddPostString("FileName", fileName);

            //    webObj.HttpAddPostString("ForcastTime", forcastTime);
            //    webObj.HttpAddPostString("ForcastClerk", forcastClerk);
            webObj.HttpAddPostString("Flag", "Update");
            webObj.HttpAddPostCurrFile("DocContent", ""); 	// 上传文件
            //    var hostName = $("#txtHideHostName").val()
            //    var hostName = "http://localhost:21765/WebUI/AQI/";
            var hostName = $("#txtHideHostName").val() + "AQI/";
            webObj.HttpAddPostString("WordType", "doc");

            var serviceUrl = hostName + "WordService/WordProduct.ashx";
            var ret = webObj.HttpPost(serviceUrl); // 判断上传是否成功
            ret = DecodeURI(ret, false);
            if (ret == "succeed") {
                alert("保存成功！");
            }
            else {
                alert("保存失败！");
            }

        }

        //生成替换文件名内YYYYMMDD的日期字符
        function GetReplaceDateString(date) {
            if (date) {                
                var seperator1 = "-";
                var seperator2 = ":";
                var month = date.getMonth() + 1;
                var strDate = date.getDate();
                if (month >= 1 && month <= 9) {
                    month = "0" + month;
                }
                if (strDate >= 0 && strDate <= 9) {
                    strDate = "0" + strDate;
                }
                var currentdate = date.getFullYear().toString() + month.toString() + strDate.toString();
                return currentdate;
            }
        }

        //生成替换文件名内YYMMDD的日期字符(不包含小时)
        function GetReplaceDateStringShortYear(date) {
            if (date) {
                var seperator1 = "-";
                var seperator2 = ":";
                var month = date.getMonth() + 1;
                var strDate = date.getDate();
                if (month >= 1 && month <= 9) {
                    month = "0" + month;
                }
                if (strDate >= 0 && strDate <= 9) {
                    strDate = "0" + strDate;
                }
                var currentdate = (date.getFullYear() % 1000).toString() + month.toString() + strDate.toString();
                return currentdate;
            }
        }

        //生成替换文件名内MMDD的日期字符
        function GetReplaceDateStringMMDD(date) {
            if (date) {
                var seperator1 = "-";
                var seperator2 = ":";
                var month = date.getMonth() + 1;
                var strDate = date.getDate();
                if (month >= 1 && month <= 9) {
                    month = "0" + month;
                }
                if (strDate >= 0 && strDate <= 9) {
                    strDate = "0" + strDate;
                }
                var currentdate = month.toString() + strDate.toString();
                return currentdate;
            }
        }

        function getReleaseTime() {
            var date = new Date();
            var seperator1 = "-";
            var seperator2 = ":";
            var month = date.getMonth() + 1;
            var hour = date.getHours();

            var strDate = date.getDate();
            if (month >= 1 && month <= 9) {
                month = "0" + month;
            }
            if (strDate >= 0 && strDate <= 9) {
                strDate = "0" + strDate;
            }
            var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + date.getSeconds();
            return currentdate;
        }

        ///检测输入的是否是数字
        function isFloat(v) {
            if (v == "" || v == null) return false;
            var str = v + "";
            var f = parseFloat(str);
            if (isNaN(f)) {
                return false;
            }

            if (str != f.toString()) { return false; }

            return true;
        }

        function GetAreaPinying(siteID) {
            if (siteID != "") {
                switch (siteID) {
                    case "58367":
                        return "XuHui";
                        break;
                    case "58370":
                        return "PuDong";
                        break;
                    case "58361":
                        return "MinHang";
                        break;
                    case "58362":
                        return "BaoShanArea";
                        break;
                    case "58462":
                        return "SongJiang";
                        break;
                    case "58460":
                        return "JinShan";
                        break;
                    case "58461":
                        return "QingPu";
                        break;
                    case "58463":
                        return "FengXian";
                        break;
                    case "58365":
                        return "JiaDing";
                        break;
                    case "58366":
                        return "ChongMing";
                        break;
                }
            }
        }



