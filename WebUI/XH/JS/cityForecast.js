Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    DateChange(Ext.getDom("H00"));
}
)

var loginParams = getCookie("UserInfo");
var logResult = Ext.util.JSON.decode(loginParams);
var userName = logResult["Alias"];

function DateChange(el) {
    var forecastDate = el.value;
    var startDate = convertDate(forecastDate);
    var nowDate = new Date();
    var obj = Ext.getDom("foreSave");
    if (startDate.format("Y年m月d日") != nowDate.format("Y年m月d日")) {
        obj.disabled = true;
        obj.className = "button_Bottom1";
    }
    else {
        obj.disabled = false;
        obj.className = "button_Bottom";
    }

    var aryDiv = comforecastTable.getElementsByTagName("span");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 3) == "Ptd") {
            var day = aryDiv[i].id.replace("Ptd", "").replace("31000", "");
            var nextDate = startDate.add('d', (parseInt(day)));
            var threeDate = nextDate.format("m月d日")
            aryDiv[i].innerHTML = threeDate;
        }
    }
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AllCityForecastII', 'GetSHForecast'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            clearElement(); //先清空所有数据
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed1(result);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}

function HistoryForecast() {
    var forecastDate = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AllCityForecastII', 'GetSHForecastHistory'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            clearElement(); //先清空所有数据
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                getHistory(result);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}
function exportWord() {
    var postJson = "";
    var siteID = ["31000"];
    var itenID = ["AQI", "primeplu", "text"];
    var forecastDate = Ext.getDom("H00").value;

    for (var i = 0; i < siteID.length; i++) {
        for (var j = 1; j < 11; j++) {
            postJson = postJson + "XH" + siteID[i] + j.toString() + ":";
            for (var k = 0; k < itenID.length; k++) {
                var id = "XH" + siteID[i] + j.toString() + itenID[k].toString();
                var dateID = "Ptd" + j.toString() + siteID[i];
                var temp = Ext.getDom(id);
                var dateTemp = Ext.getDom(dateID);
                try {
                    var lastValue = dateTemp.innerHTML.trim();
                    if (itenID[k].toString() == "text") {
                        lastValue = temp.innerHTML;
                    }
                    if (itenID[k].toString() == "primeplu") {
                        lastValue =  $('#' + id + '').combobox('getText');
                    }
                    postJson = postJson + lastValue + "#";
                } catch (exception) { }
            }
            postJson = postJson.substr(0, postJson.length - 1) + ";"; //把表单信息和预报内容分隔开
        }
    }

    postJson = postJson.substr(0, postJson.length - 1); //把表单信息和预报内容分隔开
    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.WordHelper', 'exportWord'),
        params: { wordPartContent: postJson, userName: userName, dateTime: dateTime },
        success: function (response) {
            $('#WordInfo').val(response.responseText);
            $('#DownloadWord').click();
        }
    })
}


//发布是发布到mySQL数据库（先保存，再发布）
function PublishForecast() {
    var postJson = "";
    var siteID = ["50745", "50873", "54453", "54337", "54324", "54237", "54497", "54347", "54094", "50850", "50853", "54827", "50953"];
    var itenID = ["AQI", "primeplu", "tqxs"];
    var forecastDate = Ext.getDom("H00").value;
    try {
        for (var i = 0; i < siteID.length; i++) {
            for (var j = 1; j < 4; j++) {
                postJson = postJson + "XH" + siteID[i] + j.toString() + ":";
                for (var k = 0; k < itenID.length; k++) {
                    var id = "XH" + siteID[i] + j.toString() + itenID[k].toString();
                    var temp = Ext.getDom(id);
                    try {
                        var lastValue = temp.innerHTML.trim();
                        if (itenID[k].toString() == "tqxs") {
                            lastValue = temp.value;
                        }
                        if (itenID[k].toString() == "primeplu") {
                            lastValue = "[" + $('#' + id + '').combobox('getText') + "]";
                        }
                        postJson = postJson + lastValue + ",";
                    } catch (exception) { }
                }
                postJson = postJson.substr(0, postJson.length - 1) + ";"; //把表单信息和预报内容分隔开
            }
        }
    } catch (exception) {
        alert("发布失败，请联系系统管理员！");
        return;
    }
    postJson = postJson.substr(0, postJson.length - 1); //把表单信息和预报内容分隔开


    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AllCityForecastII', 'PublishForecast'),
        params: { postJson: postJson, forecastDate: forecastDate },
        success: function (response) {
            if (response.responseText != "") {
                Ext.Msg.alert("信息", response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function SaveForecast() {
    var postJson = "";
    var siteID = ["31000"];
    var itenID = ["AQI", "primeplu", "tqxs"];
    var forecastDate = Ext.getDom("H00").value;
    try {
        for (var i = 0; i < siteID.length; i++) {
            for (var j = 1; j < 11; j++) {
                postJson = postJson + "XH" + siteID[i] + j.toString() + ":";
                for (var k = 0; k < itenID.length; k++) {
                    var id = "XH" + siteID[i] + j.toString() + itenID[k].toString();
                    var temp = Ext.getDom(id);
                    try {
                        var lastValue = temp.innerHTML.trim();
                        if (itenID[k].toString() == "tqxs") {
                            lastValue = temp.value;
                        }
                        if (itenID[k].toString() == "primeplu") {
                            lastValue = "[" + $('#' + id + '').combobox('getText') + "]";
                        }
                        postJson = postJson + lastValue + ",";
                    } catch (exception) { }
                }
                postJson = postJson.substr(0, postJson.length - 1) + ";"; //把表单信息和预报内容分隔开
            }
        }
    } catch (exception) {
        alert("保存失败，请联系系统管理员！");
        return;
    }
    postJson = postJson.substr(0, postJson.length - 1); //把表单信息和预报内容分隔开
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AllCityForecastII', 'SaveSHEdits'),
        params: { postJson: postJson, forecastDate: forecastDate },
        success: function (response) {
            if (response.responseText != "") {
                Ext.Msg.alert("信息", response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
//获取预报内容
function getContentDay(id) {
    var postJson = "";
    var aryDiv = comforecastTable.getElementsByTagName("div");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].className.indexOf('divInputType') >= 0 && aryDiv[i].id.substr(0, aryDiv[i].id.length - 1) == id) {
            var lastValue = aryDiv[i].innerHTML.trim();
            postJson = postJson + aryDiv[i].id + ":" + lastValue + ",";

        }
    }
    return postJson;

}
//当点击输入的div后，显示输入的文本框
function showInput(evt, sender) {

    var eventSource = getEventSource(evt);
    if (eventSource.tagName == "INPUT")
        return;
    alterationTextValue(sender);
}

function showInputII(evt, sender) {

    var eventSource = getEventSource(evt);
    if (eventSource.tagName == "INPUT")
        return;
    alterationTextValueII(sender);
}


function alterationTextValueII(sender) {

    //对于firefox只能用innerHTML
    var lastValue = sender.innerHTML.trim();
    sender.setAttribute("tag", lastValue);
    sender.setAttribute("AQI", sender.innerHTML);
    sender.innerHTML = "";
    //sender.style.border = "none";
    var maxAQI = 0;
    var txtInput = new Ext.form.TextArea({
        renderTo: sender.id,
        width: sender.offsetWidth,
        height: 86,
        widht: 188,
        text: lastValue,
        preventScrollbars: true
    });
    txtInput.focus();
    add = sender.id;
}

function alterationTextValue(sender) {

    //对于firefox只能用innerHTML
    var lastValue = sender.innerHTML.trim();
    var splitIndex = lastValue.indexOf('/');
    if (splitIndex > 0)
        lastValue = lastValue.substr(0, splitIndex);
    sender.setAttribute("tag", lastValue);
    sender.setAttribute("AQI", sender.innerHTML);
    sender.innerHTML = "";
    sender.style.border = "none";
    var maxAQI = 0;
    var txtInput = new Ext.form.NumberField({
        renderTo: sender.id,
        width: sender.offsetWidth,
        height: 25,
        value: lastValue,
        maxValue: 2000,
        listeners: {
            blur: function () {
                var parentNode = this.container.dom;
                var itemID = parentNode.id.substr(parentNode.id.length - 1, 1);
                //标识是否经过编辑
                var divTag = parentNode.getAttribute("tag");
                if (this.getValue() != divTag) {
                    hasEdit = true;
                    //根据当前输入的浓度值和污染物ID，返回带颜色标识的浓度和AQI组合
                    if (this.getValue() === "") {
                        parentNode.innerHTML = "";
                    }
                    else {
                        var value = this.getValue().toFixed(0);
                        parentNode.innerHTML = value;
                        var content = getContentDay(sender.id.substr(0, sender.id.length - 1));

                        Ext.Ajax.request({
                            url: getUrl('MMShareBLL.DAL.AllCityForecastII', 'AQIDescribeII'),
                            params: { content: content },
                            success: function (response) {
                                if (response.responseText != "") {
                                    var result = Ext.util.JSON.decode(response.responseText);
                                    changeDateSucessed1(result);
                                    parentNode.innerHTML = value;
                                }
                                else
                                    parentNode.innerHTML = value;
                            },
                            failure: function (response) {
                                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            }
                        });

                    }
                } else {
                    parentNode.innerHTML = parentNode.getAttribute("AQI");
                }
                var kuang = document.getElementById(sender.id);
                kuang.style.border = "1px solid #C0C0C0";
            }
        }
    });
    txtInput.focus();
    add = sender.id;
}


function clearElement() {
    var aryDiv = comforecastTable.getElementsByTagName("div");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 2) == "XH") {
            if (aryDiv[i].id.indexOf("color") > 0)
                aryDiv[i].style.backgroundColor = "";
            else if (aryDiv[i].id.substr(4) == "kqzl")
                aryDiv[i].innerHTML = "-";
            else
                aryDiv[i].innerHTML = "";
        }
    }

    aryDiv = comforecastTable.getElementsByTagName("textarea");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 2) == "XH") {
            if (aryDiv[i].id.indexOf("color") > 0)
                aryDiv[i].style.backgroundColor = "";
            else if (aryDiv[i].id.substr(4) == "kqzl")
                aryDiv[i].innerHTML = "-";
            else
                $("#" + aryDiv[i].id + "").val("");
        }
    }
    var aryDiv = comforecastTable.getElementsByTagName("input");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.indexOf("primeplu") > 0) {
            $("#" + aryDiv[i].id + "").combobox('clear');
        }
    }
}
//改变日期成功后,，刷新获取的值
function changeDateSucessed1(result) {
    for (var obj in result) {
        divContaner = Ext.getDom(obj);
        if (divContaner != null) {
            if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA") {
                divContaner.value = result[obj];
                if (obj.indexOf("primeplu") > 0) {
                    if (result[obj] != "") {
                        $('#' + obj + '').combobox('setValues', result[obj]);
                    } else {
                        $('#' + obj + '').combobox('clear');
                    }
                }
            }
            else {
                if (obj.length == 5 && obj.substr(2, 1) == 2) {
                    if (userLimit == "2")
                        divContaner.innerHTML == "";
                    else
                        divContaner.innerHTML = result[obj]; //日平均值backgroundColor = ""
                }
                else {
                    if (obj.substr(obj.length - 5) == "color")
                        divContaner.style.backgroundColor = changeBackgroundColor(result[obj]);
                    else
                        divContaner.innerHTML = result[obj];  //日平均值divColor


                }
            }
        }
    }
}
function changeBackgroundColor(colorName) {
    var color = "";
    switch (colorName) {
        case "green":
            color = "#00e400";
            break;
        case "yellow":
            color = "#ffff00";
            break;
        case "orange":
            color = "#ffa500";
            break;
        case "red":
            color = "#ff0000";
            break;
        case "purple":
            color = "#800080";
            break;
        case "grayred":
            color = "#7e0023";
            break;
        case "#7E0023":
            color = "#7e0023";
            break;
    }
    return color;
}

function getHistory(result) {
    for (var obj in result) {
        divContaner = Ext.getDom(obj);
        if (divContaner != null) {
            if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA") {
                divContaner.value = result[obj];
                if (obj.indexOf("primeplu") > 0) {
                    if (result[obj] != "") {
                        $('#' + obj + '').combobox('setValues', result[obj]);
                    } else {
                        $('#' + obj + '').combobox('clear');
                    }
                }
            }
            else {
                if (obj.length == 5 && obj.substr(2, 1) == 2) {
                    if (userLimit == "2")
                        divContaner.innerHTML == "";
                    else
                        divContaner.innerHTML = result[obj]; //日平均值backgroundColor = ""
                }
                else {
                    if (obj.substr(obj.length - 5) == "color")
                        divContaner.style.backgroundColor = changeBackgroundColor(result[obj]);
                    else
                        divContaner.innerHTML = result[obj];  //日平均值divColor


                }
            }
        }
    }
}



function uploadWord() {
    var postJson = "";
    var siteID = ["31000"];
    var itenID = ["AQI", "primeplu", "text"];
    var forecastDate = Ext.getDom("H00").value;

    for (var i = 0; i < siteID.length; i++) {
        for (var j = 1; j < 11; j++) {
            postJson = postJson + "XH" + siteID[i] + j.toString() + ":";
            for (var k = 0; k < itenID.length; k++) {
                var id = "XH" + siteID[i] + j.toString() + itenID[k].toString();
                var dateID = "Ptd" + j.toString() + siteID[i];
                var temp = Ext.getDom(id);
                var dateTemp = Ext.getDom(dateID);
                try {
                    var lastValue = dateTemp.innerHTML.trim();
                    if (itenID[k].toString() == "text") {
                        lastValue = temp.innerHTML;
                    }
                    if (itenID[k].toString() == "primeplu") {
                        lastValue = $('#' + id + '').combobox('getText');
                    }
                    postJson = postJson + lastValue + "#";
                } catch (exception) { }
            }
            postJson = postJson.substr(0, postJson.length - 1) + ";"; //把表单信息和预报内容分隔开
        }
    }

    postJson = postJson.substr(0, postJson.length - 1); //把表单信息和预报内容分隔开
    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.WordHelper', 'exportWord'),
        params: { wordPartContent: postJson, userName: userName, dateTime: dateTime },
        success: function (response) {
            var WordInfo = response.responseText;
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AllCityForecastII', 'UpLoadFileToFTP'),
                params: { WordInfo: WordInfo },
                success: function (response) {
                    if (response.responseText == "ok") {
                        alert("上传成功!");
                    } else {
                        alert("上传失败 原因： " + response.responseText);
                    }

                }, failure: function (response) {
                    alert("上传失败", "请求失败，错误代码为：" + response.status);
                }
            })
        }, failure: function (response) {
            alert("上传失败", "请求失败，错误代码为：" + response.status);
        }
    })
 
}
