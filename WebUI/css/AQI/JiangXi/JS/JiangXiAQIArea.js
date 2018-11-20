//存储暂存上传ftp的word文件名
var wordFileName = "";
var win;

Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");
    $("#copyFirstPol").click(function () {
        copyFirstPol();
    });
    $("#copyHaze").click(function () {
        copyHaze();
    });


    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#all-Left").width(pageWidth * 0.5 - 20);
    $("#all-Right").width(pageWidth * 0.5 - 20);

    $("#all-Left").height((pageHeight - 30) * 0.99 - 15);
    $("#all-Right").height((pageHeight - 30) * 0.99 - 15);


//    $("#hazeDropImg").width((pageWidth * 0.5 - 20)*0.6);
//    $("textContent").height(pageHeight - 180);
    
    
    

    if (!win) {//如果不存在win对象择创建
        win = new Ext.Window({
            title: 'Ext窗口',
            width: 1000,
            height: 900,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            items: new Ext.TabPanel({//窗体中中是一个一个TabPanel
                autoTabs: true,
                activeTab: 0,
                deferredRender: false,
                border: false,
                buttonAlign: "center",
                items: [
                        {
                            id: "tabTxt",
                            title: 'Tab1'
                            //html: "<div id='wordframe' style='width:100%;height:700px;'" + "><iframe src='" + "../AQI/EmptyWebOffice.aspx" + "' height='100%' width='100%' frameborder='0'></iframe></div>"
                        }
                    ]
            }),
            buttons: [
                    {
                        text: '提交'
                    },
                    {
                        text: '关闭',
                        handler: function () {//点击时触发的事件
                            win.hide();
                        }
                    }
                    ]
        });
    }


    var forecastDate = getNowDate();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetJiangXiAirQuaIndexImg'),
        params: { forecastDate: forecastDate },
        success: function (response) {
            if (response.responseText != "") {
                $("#hazeDropImg").attr("src", "../../" + response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
    readReportTxt();

    //保存上传txt文件到ftp
    $("#foreSave").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpLoadTxtFtpNew'),
            params: { functionName: "AQIArea", txtContent: $("#dataFileContent").text(), fileName: "AQIAreaForecast.txt" },
            success: function (response) {
                if (response.responseText != "fail") {
                    wordFileName = response.responseText.split('_')[0];
                    alert("上传成功！")
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //预览按钮
    $("#forePreview").click(function () {
        if (win) {
            win.setTitle("AQI分时段预报产品预览");
            Ext.getCmp("tabTxt").setTitle("24小时AQI分时段预报");
            Ext.getCmp("tabTxt").html = "<div id='frame' style='width:100%;height:700px;'" + "><iframe src='" + "../AQI/EmptyWebOffice.aspx?fileName=" + "123456.doc" + "' height='100%' width='100%' frameborder='0'></iframe></div>";
            win.show();
            alert(document.all.WebOffice1);
        }
    });


    $.each($(".aqiValue"), function (i, n) {
        $(n).change(function () {
            var $oldInput = $(this);
            var oldAQIValue = $(n).val();
            oldAQIValue = $.trim(oldAQIValue);

            var currentInputName = $(n).attr("name");
            var newAQIValue = $(n).val();
            newAQIValue = $.trim(newAQIValue);
            if (currentInputName == "AQI") {

                if (newAQIValue == null || newAQIValue == "" || !isFloat(newAQIValue)) {
                    alert("AQI指数输入有误！");

                    if (oldAQIValue != null && oldAQIValue != "") {
                        $(n).val(oldAQIValue);
                    }
                }
                else {

                    $($oldInput).addClass("backgroundColor_orange");
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevel'),
                        params: { aqiValue: newAQIValue },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = n.id.split('_')[0];
                                $("#" + siteId + "_Level").val(response.responseText);
                                $("#" + siteId + "_Level").addClass("backgroundColor_orange");
                                var kuang = document.getElementById(n.id);
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
            }
        });
    });

});


//复制首要污染物
function copyFirstPol() {
    var polValue = $(".firstPollutionSel").get(0).value
    for (var i = 1; i < $(".firstPollutionSel").length; i++) {
        $(".firstPollutionSel").get(i).value = polValue;
    }
}

//复制霾
function copyHaze() {
    var hazeValue = $(".hazeLevel").get(0).value
    for (var i = 1; i < $(".hazeLevel").length; i++) {
        $(".hazeLevel").get(i).value = hazeValue;
    }
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

//当点击输入的div后，显示输入的文本框
function showInput(evt, sender) {
    //var date = Ext.getDom("H00").value;
    var eventSource = getEventSource(evt);
    if (eventSource.tagName == "INPUT")
        return;
    alterationTextValue(sender);
}

//生成预报的txt文件
function readReportTxt() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetJiangXiReportContent'),
        success: function (response) {
            if (response.responseText != "") {
                $("#dataFileContent").text(response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
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
    var nowDate = new Date();
    //    var strDate = nowDate.format("Y年m月d日");
    //    var date = Ext.getDom("H00").value;
    var strDate = "2015年11月06日";
    var date = "2015年11月06日";
    if (strDate != date) {
        var txtInput = new Ext.form.NumberField({
            renderTo: sender.id,
            width: sender.offsetWidth - 25,
            //width: 40,
            value: lastValue,
            maxValue: 2000,
            readOnly: true,
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
                            //                            buildPreview(parentNode.id);
                        }
                        else {
                            var value = this.getValue().toFixed(1);
                            //                            Ext.Ajax.request({
                            //                                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                            //                                params: { value: value, itemID: itemID },
                            //                                success: function (response) {
                            //                                    if (response.responseText != "") {
                            //                                        parentNode.innerHTML = response.responseText;
                            //                                        buildPreview(parentNode.id);
                            //                                    }
                            //                                },
                            //                                failure: function (response) {
                            //                                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            //                                }
                            //                            });

                        }
                    } else {
                        parentNode.innerHTML = parentNode.getAttribute("AQI");
                        //buildPreview(parentNode.id);
                    }
                    var kuang = document.getElementById(sender.id);
                    kuang.style.border = "1px solid #C0C0C0";

                },
                change: function () {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevel'),
                        params: { aqiValue: this.getValue() },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = sender.id.split('_')[0];
                                $("#" + siteId + "_Level").text(response.responseText);
                                var kuang = document.getElementById(sender.id);
                                kuang.style.border = "1px solid #C0C0C0";
                                kuang.style.height = "20px";
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
            }
        });
        txtInput.focus();
        inputEditor = txtInput;

        txtInput.container.dom.firstChild.select();
        add = sender.id;
    }
    else {
        var txtInput = new Ext.form.NumberField({
            renderTo: sender.id,
            width: sender.offsetWidth - 25,
            //width: 40,
            value: lastValue,
            maxValue: 2000,
            readOnly: false,
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
                            buildPreview(parentNode.id);
                        }
                        else {
                            var value = this.getValue().toFixed(1);
                            //                            Ext.Ajax.request({
                            //                                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                            //                                params: { value: value, itemID: itemID },
                            //                                success: function (response) {
                            //                                    if (response.responseText != "") {
                            //                                        parentNode.innerHTML = response.responseText;
                            //                                        buildPreview(parentNode.id);
                            //                                    }
                            //                                },
                            //                                failure: function (response) {
                            //                                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                            //                                }
                            //                            });
                        }
                    } else {
                        parentNode.innerHTML = parentNode.getAttribute("AQI");
                        //buildPreview(parentNode.id);
                    }
                    var kuang = document.getElementById(sender.id);
                    kuang.style.border = "1px solid #C0C0C0";


                },
                change: function () {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateAQLLevel'),
                        params: { aqiValue: this.getValue() },
                        success: function (response) {
                            if (response.responseText != "") {
                                var siteId = sender.id.split('_')[0];
                                $("#" + siteId + "_Level").text(response.responseText);
                                var kuang = document.getElementById(sender.id);
                                kuang.style.border = "1px solid #C0C0C0";
                                kuang.style.height = "20px";
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                }
            }
        });
        //txtInput.focus();
        inputEditor = txtInput;

        txtInput.container.dom.firstChild.select();
        add = sender.id;
    }

}
