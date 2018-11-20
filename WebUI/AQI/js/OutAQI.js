// 用于处理AQI
var inputEditor;
var weekday = new Array(7); weekday[0] = "星期日"; weekday[1] = "星期一"; weekday[2] = "星期二";
weekday[3] = "星期三"; weekday[4] = "星期四"; weekday[5] = "星期五"; weekday[6] = "星期六";
var hasEdit = false; //标识当前是否处于需要保持的状态
var checkBox = "";
var checkBoxDX = "";
var win;
var storeTextContent = "";
var storeMsgContent = "";
//标记日期的类型（昨天/今天）
var generalDateType = "Today";
var userName = "";
var msgPubState = "unPub";
//用于发布的文本内容
var textContent = "";
//用于发布的短信内容
var msgContent = "";

Ext.onReady(function () {
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#outLine").width($(window).width() - 30);

    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    changeDate("");

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadSavedAQIPeriodData'),
        success: function (response) {
            if (response.responseText != "") {
                var results = Ext.util.JSON.decode(response.responseText);
                for (var obj in results) {
                    var divContaner = Ext.getDom(obj);
                    if (divContaner != null) {
                        if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                            divContaner.value = results[obj];
                        else {
                            if (divContaner != "" && divContaner != null)
                                divContaner.innerHTML = results[obj]; //日平均值
                        }
                    }
                }
            }
        },
        failure: function (response) {
        }
    });

    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    userName = result["Alias"];
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

    //读取当天的AQI分时段短信是否已发送
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'JudgeAQIPeriodMessage'),
        success: function (response) {
            if (response.responseText == "Pub") {
                msgPubState = "Pub";
            }
        },
        failure: function (response) {
            msgPubState = "unPub";
        }
    });


    var productState = "undone";
    //读取状态表，设置底部按键的文字和功能
    //    Ext.Ajax.request({
    //        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetProductState'),
    //        params: { functionName: "AQIPeriod", hourType: "17:00" },
    //        success: function (response) {
    //            if (response.responseText != "") {
    //                productState = response.responseText;
    //                if (response.responseText == "undone") {
    //                    $("#foreSave").text("保存");
    //                }
    //                else if (response.responseText == "saved") {
    //                    $("#foreSave").text("审核");
    //                }
    //                else if (response.responseText == "checked") {
    //                    $("#foreSave").text("发布");
    //                }
    //                else if (response.responseText == "published") {
    //                    $("#foreSave").text("发布");
    //                }
    //                else if (response.responseText == "less") {
    //                    $("#foreSave").text("发布");
    //                }
    //            }
    //        },
    //        failure: function (response) {
    //            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
    //        }
    //    });

    //2015年11月11日，预览按钮点击事件

    //ftp地址集合字符串
    var ftpString = $("#FtpCollection").val();
    //根据当前日期获取替换文件名的字符串
    //    var replaceDate = GetReplaceDateString(new Date());
    //    ftpString = ftpString.replace("YYYYMMDD", replaceDate);
    if (!win) {//如果不存在win对象择创建
        win = new Ext.Window({
            title: 'Ext窗口',
            width: 1000,
            height: 500,
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
                            title: 'Tab1',
                            html: '<textarea id="textContentArea" class="textPrev"></textarea>'//内部显示内容
                        },
                        {
                            id: "tabMsg",
                            title: 'Tab2',
                            html: '<textarea id="msgArea" class="textPrev"></textarea>'
                        }
                    ]
            }),
            buttons: [
                              {
                                  text: '保存',
                                  handler: function () {//点击时触发的事件
                                      var cells = "";
                                      $.each($(".divInputType"), function (i, n) {
                                          var cell = n.id + ":" + $(this).text() + ",";
                                          cells += cell;
                                      });
                                      Ext.Ajax.request({
                                          url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIPeriodDataAndText'),
                                          params: { forecastDate: getNowDate(), data: cells, functionName: 'AQIPeriod', ftpString: ftpString, txtContent: GetAQIPeriodTextContent(), msgContent: GetAQIPeriodContent("msg"), fileTxtName: 'AQI_SH_YYYYMMDDHHmm.txt', fileMsgName: 'msg.txt', userName: userName, textContent: $("#textContentArea").val(), msgContent: $("#msgArea").val(), dateType: generalDateType },
                                          success: function (response) {
                                              if (response.responseText == "success") {
                                                  textContent = $("#textContentArea").val();
                                                  msgContent = $("#msgArea").val()
                                                  alert("保存成功");
                                              }
                                              else {
                                                  alert("保存失败！");
                                              }
                                          },
                                          failure: function (response) {
                                              Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                                          }
                                      });
                                      //                                      Ext.Ajax.request({
                                      //                                          url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIPeriodTextAngMsgNew'),
                                      //                                          params: { textContent: $("#textContentArea").val(), msgContent: $("#msgArea").val(), dateType: generalDateType },
                                      //                                          success: function (response) {
                                      //                                              if (response.responseText == "success") {
                                      //                                                  textContent = $("#textContentArea").val();
                                      //                                                  msgContent = $("#msgArea").val()
                                      //                                                  alert("保存成功");
                                      //                                              }
                                      //                                              else {
                                      //                                                  alert("保存失败！");
                                      //                                              }
                                      //                                          },
                                      //                                          failure: function (response) {
                                      //                                              Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                                      //                                          }
                                      //                                      });
                                  }
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

    //预览按键
    $("#forePreview").click(function () {
        if (win) {
            win.setTitle("AQI分时段预报产品预览");
            Ext.getCmp("tabTxt").setTitle("24小时AQI分时段预报");
            Ext.getCmp("tabMsg").setTitle("AQI分时段预报短信");
            win.show();

            //发布的FTP文本
            var txtPubContent = "";
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetAQIPeriodTextContentNew'),
                params: { contentType: "text", dateType: generalDateType },
                success: function (response) {
                    if (response.responseText != "") {
                        txtPubContent = response.responseText;
                        $("#textContentArea").val(txtPubContent);
                        textContent = txtPubContent;
                    }
                    else {
                        if (generalDateType == "Today") {
                            txtPubContent = GetAQIPeriodTextContent();
                            $("#textContentArea").val(txtPubContent);
                            textContent = txtPubContent;
                        }
                        else if (generalDateType == "Yesterday" || generalDateType == "Before") {
                            txtPubContent = GetAQIPeriodTextContentYesterday(generalDateType);
                            $("#textContentArea").val(txtPubContent);
                            textContent = txtPubContent;
                        }
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });

            //发布的短信文本内容
            var txtMsgContent = "";
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetAQIPeriodTextContentNew'),
                params: { contentType: "msg", dateType: generalDateType },
                success: function (response) {
                    if (response.responseText != "") {
                        txtMsgContent = response.responseText;
                        $("#msgArea").val(txtMsgContent);
                        //Ext.get("msgArea").update(txtMsgContent);
                        msgContent = txtMsgContent;
                    }
                    else {
                        if (generalDateType == "Today") {
                            txtMsgContent = GetAQIPeriodContent("msg");
                            $("#msgArea").val(txtMsgContent);
                            //Ext.get("msgArea").update(txtMsgContent);
                            msgContent = txtMsgContent;
                        }
                        else if (generalDateType == "Yesterday" || generalDateType == "Before") {
                            var curHour = new Date().getHours();
                            if (curHour < 12) {
                                txtMsgContent = GetAQIPeriodContentTwoLines("msg");
                                $("#msgArea").val(txtMsgContent);
                                //Ext.get("msgArea").update(txtMsgContent);
                            }
                            else if (curHour >= 12) {
                                txtMsgContent = GetAQIPeriodContentOneLine("msg");
                                $("#msgArea").val(txtMsgContent);
                                //Ext.get("msgArea").update(txtMsgContent);
                            }
                            msgContent = txtMsgContent;
                        }
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
    });

    $("#foreSave").click(function () {
        var cells = "";
        $.each($(".divInputType"), function (i, n) {
            var cell = n.id + ":" + $(this).text() + ",";
            cells += cell;
        });
        if (generalDateType == "Today") {
            var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
            myMask.show();
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIPeriodData'),
                params: { forecastDate: getNowDate(), data: cells, functionName: 'AQIPeriod', ftpString: ftpString, txtContent: GetAQIPeriodTextContent(), msgContent: GetAQIPeriodContent("msg"), fileTxtName: 'AQI_SH_YYYYMMDDHHmm.txt', fileMsgName: 'msg.txt', userName: userName },
                success: function (response) {
                    if (response.responseText != "") {
                        if (response.responseText == "success") {
                            myMask.hide();
                            alert("保存成功！");
                        }
                        else if (response.responseText == "published") {
                            myMask.hide();
                            alert("数据已保存！");
                        }
                        else {
                            myMask.hide();
                            alert("保存失败！");
                        }
                    }
                },
                failure: function (response) {
                    myMask.hide();
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
        else {
            var textHisContent = "";
            var msgHisContent = "";
            var curHour = new Date().getHours();
            if (curHour < 12) {
                msgHisContent = GetAQIPeriodContentTwoLines("msg");
                Ext.get("msgArea").update(msgHisContent);
            }
            else if (curHour >= 12) {
                msgHisContent = GetAQIPeriodContentOneLine("msg");
                Ext.get("msgArea").update(msgHisContent);
            }

            textHisContent = GetAQIPeriodTextContentYesterday(generalDateType);
            $("#textContentArea").val(textHisContent);
            textContent = textHisContent;
            msgContent = msgHisContent;
            var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
            myMask.show();
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveAQIPeriodHistoryData'),
                params: { textContent: textHisContent, msgContent: msgHisContent, fileTxtName: 'AQI_SH_YYYYMMDDHHmm.txt', fileMsgName: 'msg.txt', dateType: generalDateType },
                success: function (response) {
                    if (response.responseText != "") {
                        if (response.responseText == "success") {
                            myMask.hide();
                            alert("保存成功！");
                        }
                        else if (response.responseText == "published") {
                            myMask.hide();
                            alert("数据已保存！");
                        }
                        else {
                            myMask.hide();
                            alert("保存失败！");
                        }
                    }
                },
                failure: function (response) {
                    myMask.hide();
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
    });

    //审核按键
    $("#foreCheck").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SetChecked'),
            params: { functionName: "AQIPeriod", hourType: "17:00" },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("审核成功");
                }
                else {
                    alert("审核失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //发布按键
    $("#forePub").click(function () {
        var fileDate = getFormatDate('');
        Ext.Ajax.timeout = 900000;
        //UpLoadAQIPeriodTextAndMsg(配置文件测试方法)
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        //发布的文本内容
        var txtPubContent = "";
        if (textContent != "") {
            txtPubContent = textContent;
        }
        else {
            if (generalDateType == "Today") {
                txtPubContent = GetAQIPeriodTextContent();
                $("#textContentArea").val(txtPubContent);
            }
            else if (generalDateType == "Yesterday" || generalDateType == "Before") {
                fileDate = getFormatDate('Yesterday');

                txtPubContent = GetAQIPeriodTextContentYesterday(generalDateType);
                $("#textContentArea").val(txtPubContent);
            }
        }
        //发布的短信内容
        var txtMsgContent = "";
        if (msgContent != "") {
            txtMsgContent = msgContent;
        }
        else {
            if (generalDateType == "Today") {
                txtMsgContent = GetAQIPeriodContent("msg");
                Ext.get("msgArea").update(txtMsgContent);
            }
            else if (generalDateType == "Yesterday" || generalDateType == "Before") {
                var curHour = new Date().getHours();
                if (curHour < 12) {
                    txtMsgContent = GetAQIPeriodContentTwoLines("msg");
                    Ext.get("msgArea").update(txtMsgContent);
                }
                else if (curHour >= 12) {
                    txtMsgContent = GetAQIPeriodContentOneLine("msg");
                    Ext.get("msgArea").update(txtMsgContent);
                }
            }
        }
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpLoadAQIPeriodTextAndMsgReal'),
            //            params: { ftpString: ftpString, fileDate: fileDate, functionName: 'AQIPeriod', txtContent: GetAQIPeriodContent("txt"), msgContent: GetAQIPeriodContent("msg") },
            params: { ftpString: ftpString, fileDate: fileDate, functionName: 'AQIPeriod', txtContent: txtPubContent, txtMsg: txtMsgContent, userName: userName, dateType: generalDateType },
            success: function (response) {
                if (response.responseText != "") {
                    if (response.responseText != "") {
                        myMask.hide();
                        if (response.responseText == "success") {
                            alert("发布成功！");
                        }
                        else if (response.responseText == "less") {
                            alert("发布不完全！");
                        }
                        else {
                            alert("发布失败！");
                        }
                    }
                    else {
                        alert("发布失败！");
                    }

                }
                myMask.hide();
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    var startDate = new Date();
    startDate.setDate(startDate.getDate() - 1);
    //主观数据导入
    $("#subjImport").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadSavedAQIPeriodData'),
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    var results = Ext.util.JSON.decode(response.responseText);
                    for (var obj in results) {
                        var divContaner = Ext.getDom(obj);
                        if (divContaner != null) {
                            if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                                divContaner.value = results[obj];
                            else {
                                if (divContaner != "" && divContaner != null)
                                    divContaner.innerHTML = results[obj]; //日平均值
                            }
                        }
                    }
                    
                }
                else {
                    var nowDate = getNowDate();
                    //        waitInfo = Ext.MessageBox.wait('请等待', '读取数据中');
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'ReadFromDataInterface'),
                        params: { forecastDate: nowDate },
                        success: function (response) {
                            if (response.responseText != "") {
                                var dateType = response.responseText.split('&')[0];
                                if (dateType == "Yesterday") {
                                    generalDateType = dateType;
                                    var startDate = new Date();
                                    startDate.setDate(startDate.getDate() - 1);
                                }
                                else if (dateType == "Today") {
                                    generalDateType = dateType;
                                    var startDate = new Date();
                                    startDate.setDate(startDate.getDate());
                                }

                                var date = startDate.format("m月d日");
                                var nextDate = startDate.add('d', 2);
                                var threeDate = nextDate.format("m月d日");
                                var tomorrowDay = startDate.add('d', 1);
                                var tomoDate = tomorrowDay.format("m月d日");

                                var ePreview = Ext.getDom("Ptd11");
                                ePreview.innerText = date;
                                for (var i = 2; i <= 3; i++) {
                                    ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                                    ePreview.innerText = tomoDate;
                                }
                                ePreview = Ext.getDom("Ptd41");
                                ePreview.innerText = threeDate;
                                ePreview = Ext.getDom("Ptd51");
                                ePreview.innerText = tomoDate;
                                ePreview = Ext.getDom("Ptd61");
                                ePreview.innerText = threeDate;


                                var cellsString = response.responseText.split('&')[1];
                                var results = Ext.util.JSON.decode(cellsString);
                                calculateAQI(results);

                                for (var obj in results) {
                                    divContaner = Ext.getDom(obj);
                                    if (divContaner != null) {
                                        if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                                            divContaner.value = results[obj];
                                        else {
                                            if (divContaner != "" && divContaner != null)
                                                divContaner.innerHTML = results[obj]; //日平均值
                                        }
                                    }
                                }
                                calculatrNO2(results);
                                buildPreview("H3141");
                                //                    waitInfo.hide();
                            }                            
                        },
                        failure: function (response) {
                            myMask.hide();
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }

                    });
                }
            },
            failure: function (response) {
                myMask.hide();
            }
        });
    });


    //获取昨天数据
    $("#historyData").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'ReadFromDataInterfaceHistory'),
            //            params: { forecastDate: nowDate },
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    
                    var dateType = response.responseText.split('&')[0];
                    if (dateType == "Yesterday") {
                        generalDateType = dateType;
                        var startDate = new Date();
                        startDate.setDate(startDate.getDate() - 1);
                    }
                    else if (dateType == "Before") {
                        generalDateType = dateType;
                        var startDate = new Date();
                        startDate.setDate(startDate.getDate() - 2);
                    }

                    var date = startDate.format("m月d日");
                    var nextDate = startDate.add('d', 2);
                    var threeDate = nextDate.format("m月d日");
                    var tomorrowDay = startDate.add('d', 1);
                    var tomoDate = tomorrowDay.format("m月d日");

                    var ePreview = Ext.getDom("Ptd11");
                    ePreview.innerText = date;
                    for (var i = 2; i <= 3; i++) {
                        ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                        ePreview.innerText = tomoDate;
                    }
                    ePreview = Ext.getDom("Ptd41");
                    ePreview.innerText = threeDate;
                    ePreview = Ext.getDom("Ptd51");
                    ePreview.innerText = tomoDate;
                    ePreview = Ext.getDom("Ptd61");
                    ePreview.innerText = threeDate;


                    var cellsString = response.responseText.split('&')[1];
                    var results = Ext.util.JSON.decode(cellsString);
                    calculateAQI(results);

                    for (var obj in results) {
                        divContaner = Ext.getDom(obj);
                        if (divContaner != null) {
                            if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                                divContaner.value = results[obj];
                            else {
                                if (divContaner != "" && divContaner != null)
                                    divContaner.innerHTML = results[obj]; //日平均值
                            }
                        }
                    }
                    calculatrNO2(results);
                    buildPreview("H3141");
                    //                    waitInfo.hide();
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

});



var waits = 30;

function closeCheck(divName) {
    var divUsers = Ext.getDom(divName);
    divUsers.style.display = "none";
}
function changeDate(el) {
    //var forecastDate = el.value;
    var forecastDate = "2015年11月02日";

    //获取当前时间
    //var forecastDate = getNowDate();
    //titleDate.innerHTML = forecastDate;
    //    var startDate = convertDate(forecastDate);
    var startDate = new Date();
    var date = startDate.format("m月d日");
    var nextDate = startDate.add('d', 2);
    var threeDate = nextDate.format("m月d日");
    var tomorrowDay = startDate.add('d', 1);
    var tomoDate = tomorrowDay.format("m月d日");

    var ePreview = Ext.getDom("Ptd11");
    ePreview.innerText = date;
    for (var i = 2; i <= 3; i++) {
        ePreview = Ext.getDom(String.format("Ptd{0}1", i));
        ePreview.innerText = tomoDate;
    }
    ePreview = Ext.getDom("Ptd41");
    ePreview.innerText = threeDate;
    ePreview = Ext.getDom("Ptd51");
    ePreview.innerText = tomoDate;
    ePreview = Ext.getDom("Ptd61");
    ePreview.innerText = threeDate;

    //模型数据导入
    $("#modelImport").click(function () {


        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'BuildPreconsation'),
            params: { forecastDate: forecastDate },
            success: function (response) {
                clearElement(); //先清空所有数据             
                if (response.responseText != "") {
                    var startDate = new Date();
                    var date = startDate.format("m月d日");
                    var nextDate = startDate.add('d', 2);
                    var threeDate = nextDate.format("m月d日");
                    var tomorrowDay = startDate.add('d', 1);
                    var tomoDate = tomorrowDay.format("m月d日");

                    var ePreview = Ext.getDom("Ptd11");
                    ePreview.innerText = date;
                    for (var i = 2; i <= 3; i++) {
                        ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                        ePreview.innerText = tomoDate;
                    }
                    ePreview = Ext.getDom("Ptd41");
                    ePreview.innerText = threeDate;
                    ePreview = Ext.getDom("Ptd51");
                    ePreview.innerText = tomoDate;
                    ePreview = Ext.getDom("Ptd61");
                    ePreview.innerText = threeDate;

                    var result = Ext.util.JSON.decode(response.responseText);
                    //calculateAQI(result);
                    // calculatrNO2(result);
                    calculateAQI(result);
                    changeDateSucessed(result);
                    
                    var nowDateTime = Ext.getDom("nowDateTime");
                    myMask.hide();
                    buildPreview("H3141");
                }
                SetFoucs();
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                myMask.hide();
            }
        })
    });
}
//让光标停留在第一个输入位置
function SetFoucs() {
    var address = String.format("H314{0}", "1");
    var sender = Ext.getDom(address);
    alterationTextValue(sender);

}
//当没有数据的时候清空
function clearElement() {
    var aryDiv = contentTable.getElementsByTagName("div");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 1) == "H" || aryDiv[i].id.substr(0, 1) == "P") {
            aryDiv[i].innerHTML = "/";
        }
    }

    aryDiv = document.getElementsByTagName("INPUT");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 1) == "H" && aryDiv[i].id != "H00") {//当前切换的日期控件的不清空
            aryDiv[i].value = "";
        }
    }

    aryDiv = document.getElementsByTagName("textarea");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].id.substr(0, 1) == "H" || aryDiv[i].id.substr(1, 1) == "H") {
            aryDiv[i].value = "";
        }
    }
}
//清空事件readOnly = true
function clear() {
    aryDiv = document.getElementsByTagName("textarea");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].readOnly == false) {
            aryDiv[i].value = "";
        }
    }
}

//当点击输入的div后，显示输入的文本框
function showInput(evt, sender) {
    //var date = Ext.getDom("H00").value;
    var nowDate = new Date();
    var hour = nowDate.getHours();
    //20点前可以修改
    if (hour < 20) {
        var eventSource = getEventSource(evt);
        if (eventSource.tagName == "INPUT")
            return;
        alterationTextValue(sender);
    }
    else {
        if (sender.className.toString().indexOf("limitEdit") > 0) {
            alert("数据无法编辑！")
            return;
        }
        else {
            var eventSource = getEventSource(evt);
            if (eventSource.tagName == "INPUT")
                return;
            alterationTextValue(sender);
        }
    }   
}

function showInputCopy(evt, sender) {
    //var date = Ext.getDom("H00").value;
    var eventSource = getEventSource(evt);
    if (eventSource.tagName == "INPUT")
        return;
    alterationTextValue(sender);
}

function alterationTextValue(sender) {

    //对于firefox只能用innerHTML
    var cellValue = sender.innerHTML.trim();
    var lastValue = "";
    var splitIndex = cellValue.indexOf('/');
    //最后两列的首要污染物
    var firstItemValue = "";
    if (splitIndex > 0) {
        lastValue = cellValue.substr(0, splitIndex);
        firstItemValue = cellValue.substr(splitIndex + 1);
        firstItemValue = firstItemValue.replace("<span>", "");
        firstItemValue = firstItemValue.replace("</span>", "");
    }
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
                            //                            parentNode.innerHTML = "";
                            parentNode.innerHTML = "";
                            //buildPreview(parentNode.id);
                        }
                        else {
                            var value = this.getValue().toFixed(1);
                            //最后AQI两列
                            if (sender.id.charAt(sender.id.length - 1) == '6') {
                                Ext.Ajax.request({
                                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ChangeAQI'),
                                    params: { value: value, itemID: itemID, firstItem: firstItemValue },
                                    success: function (response) {
                                        if (response.responseText != "") {
                                            //                                            parentNode.innerHTML = response.responseText;
                                            $("#" + parentNode.id).html(response.responseText);
                                            //buildPreview(parentNode.id);
                                        }
                                    },
                                    failure: function (response) {
                                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                                    }
                                });
                            }
                            else {
                                Ext.Ajax.request({
                                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                                    params: { value: value, itemID: itemID },
                                    success: function (response) {
                                        if (response.responseText != "") {
                                            //                                            parentNode.innerHTML = response.responseText;
                                            $("#" + parentNode.id).html(response.responseText);
                                            //buildPreview(parentNode.id);

                                            //每个单元格内数据改变后，对应行末的AQI单元格内内容改变
                                            //calculateAQIOfSingleLine(sender.id);
                                            calculateAQIOfSingleLineSecond(sender.id);
                                            processAverageValue(sender.id);
                                        }
                                    },
                                    failure: function (response) {
                                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                                    }
                                });
                            }

                        }
                    } else {
                        $("#" + parentNode.id).html(parentNode.getAttribute("AQI"));
                        //parentNode.innerHTML = parentNode.getAttribute("AQI");
                        //buildPreview(parentNode.id);
                    }
                    var kuang = document.getElementById(sender.id);
                    kuang.style.border = "1px solid #C0C0C0";
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
                            //                            parentNode.innerHTML = "";
                            parentNode.innerHTML = "/";
                            //buildPreview(parentNode.id);
                        }
                        else {
                            var value = this.getValue().toFixed(1);
                            //最后AQI两列
                            if (sender.id.charAt(sender.id.length - 1) == '6') {
                                Ext.Ajax.request({
                                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ChangeAQI'),
                                    params: { value: value, itemID: itemID, firstItem:firstItemValue },
                                    success: function (response) {
                                        if (response.responseText != "") {
                                            //parentNode.innerHTML = response.responseText;
                                            $("#" + parentNode.id).html(response.responseText);
                                            //buildPreview(parentNode.id);
                                        }
                                    },
                                    failure: function (response) {
                                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                                    }
                                });
                            }
                            else {
                                Ext.Ajax.request({
                                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                                    params: { value: value, itemID: itemID },
                                    success: function (response) {
                                        if (response.responseText != "") {
                                            //parentNode.innerHTML = response.responseText;
                                            $("#" + parentNode.id).html(response.responseText);
                                            //buildPreview(parentNode.id);

                                            //每个单元格内数据改变后，对应行末的AQI单元格内内容改变
                                            //calculateAQIOfSingleLine(sender.id);
                                            calculateAQIOfSingleLineSecond(sender.id);
                                            processAverageValue(sender.id);
                                        }
                                    },
                                    failure: function (response) {
                                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                                    }
                                });
                            }

                        }
                    } else {
                        //parentNode.innerHTML = parentNode.getAttribute("AQI");
                        $("#" + parentNode.id).html(parentNode.getAttribute("AQI"));
                        //buildPreview(parentNode.id);
                    }
                    var kuang = document.getElementById(sender.id);
                    kuang.style.border = "1px solid #C0C0C0";
                }
            }
        });
        txtInput.focus();
        inputEditor = txtInput;

        txtInput.container.dom.firstChild.select();
        add = sender.id;
    }
}
//当改变单元格预报内容后，即创建预报预览，并根据当前的ID判断是否需要计算日平均，并计算日平均
function buildPreview(curID) {
    var rowIndex = curID.substr(1, 1);
    var itemID = "";
    var divID = "";
    var par = curID.substr(0, 4);
    var t = curID.substr(4, 1);
    var caculateValue = "";
    var durationIndex = curID.substr(3, 1);
    var valueCurr = "";
    var iD;
    var Module = "ManualCenter";
    var style = curID.substr(2, 1);
    if (rowIndex > 2) {
        if (rowIndex > 3) {
            divID = String.format("{0}{1}{2}", curID.substr(0, 3), 7, curID.substr(4, 1)); //日平均所显示的DIVID
            itemID = curID.substr(4, 1); //当前污染物ID
        }

        if (rowIndex == 4 && durationIndex == 4 && curID.substr(4, 1) != 4 && curID.substr(4, 1) != 5) {
            for (var i = 1; i <= 3; i++) {
                var tempText = String.format("{0}{1}{2}{3}{4}", curID.substr(0, 1), 5, 1, i, curID.substr(4, 1));
                if (Ext.getDom(tempText).innerText == "" || Ext.getDom(tempText).innerText == "/") {
                    Ext.getDom(tempText).innerHTML = Ext.getDom(curID).innerHTML;
                }

            }
        }
        var lastValue = Ext.getDom(curID).innerHTML;
        var splitIndex = lastValue.indexOf('/');
        if (splitIndex > 0)
            valueCurr = lastValue.substr(0, splitIndex);
        if (durationIndex == 4 && valueCurr != "" && rowIndex != 5) {
            var night = String.format("H{0}1{1}{2}", rowIndex, 6, t);
            var even = Ext.getDom(night).innerHTML;
            var moning = String.format("H{0}1{1}{2}", parseInt(rowIndex) + 1, 1, t);
            var mo = Ext.getDom(moning).innerHTML;
            if (mo.indexOf('/') > 0) {
                caculateValue = ((valueCurr * 4 + mo.split('/')[0] * 6) / 10).toFixed(1);
                iD = night;
            }
            else {
                if (even.indexOf('/') > 0) {
                    caculateValue = ((even.split('/')[0] * 10 - valueCurr * 4) / 6).toFixed(1);
                    iD = moning;
                }
            }
        }
        else if (durationIndex == 6 && valueCurr != "" && valueCurr != "/") {
            var night = String.format("H{0}1{1}{2}", rowIndex, 4, t);
            var even = Ext.getDom(night).innerHTML;
            var moning = String.format("H{0}1{1}{2}", parseInt(rowIndex) + 1, 1, t);
            var mo = Ext.getDom(moning).innerHTML;
            if (even.indexOf('/') > 0) {
                caculateValue = ((valueCurr * 10 - even.split('/')[0] * 4) / 6).toFixed(1);
                iD = moning;
            }
            else {
                if (mo.indexOf('/') > 0) {
                    caculateValue = ((valueCurr * 10 - mo.split('/')[0] * 6) / 4).toFixed(1);
                    iD = night;
                }
            }

        }
        else if (durationIndex == 1 && valueCurr != "" && valueCurr != "/") {
            var night = String.format("H{0}1{1}{2}", parseInt(rowIndex) - 1, 4, t);
            var even = Ext.getDom(night).innerHTML;
            var moning = String.format("H{0}1{1}{2}", parseInt(rowIndex) - 1, 6, t);
            var mo = Ext.getDom(moning).innerHTML;
            if (even.indexOf('/') > 0) {
                caculateValue = ((even.split('/')[0] * 4 + valueCurr * 6) / 10).toFixed(1);
                iD = moning;
            }
            else {
                if (mo.indexOf('/') > 0) {
                    caculateValue = ((mo.split('/')[0] * 10 - valueCurr * 6) / 4).toFixed(1);
                    iD = night;
                }
            }
        }
        if (caculateValue != "") {
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: caculateValue, itemID: curID.substr(4, 1) },
                success: function (response) {
                    Ext.getDom(iD).innerHTML = response.responseText;
                    var content = getContent();
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast', 'BuildPreview'),
                        params: { forecastDate: Ext.getDom("H00").value, postJson: content, divID: divID, itemID: itemID, detail: "", Module: Module },
                        success: function (response) {
                            if (response.responseText != "") {
                                var result = Ext.util.JSON.decode(response.responseText);
                                changeDateSucessed(result);
                            }
                            else {
                                clearPreview(divID);
                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    });
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
        else {
            var content = getContent();
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'BuildPreview'),
                params: { forecastDate: Ext.getDom("H00").value, postJson: content, divID: divID, itemID: itemID, detail: "", Module: Module },
                success: function (response) {
                    if (response.responseText != "") {
                        var result = Ext.util.JSON.decode(response.responseText);
                        changeDateSucessed(result);
                        //获取最后一列AQI显示内容

                    }
                    else {
                        clearPreview(divID);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
    }

}
function clearPreview(id) {
    Ext.getDom("H09").value = "";
    Ext.getDom("H10").value = "";
    //    if(id="")
    //    Ext.getDom(id).innerHTML="";
}
//获取预报内容
function getContent() {
    var postJson = "";
    var aryDiv = contentTable.getElementsByTagName("div");
    for (var i = 0; i < aryDiv.length; i++) {
        if (aryDiv[i].className.indexOf('divInputType') >= 0 && aryDiv[i].id.substr(4,1)!=6) {
            var lastValue = aryDiv[i].innerHTML.trim();
            var splitIndex = lastValue.indexOf('/');
            if (splitIndex > 0) {
                lastValue = lastValue.substr(0, splitIndex);
                var span = aryDiv[i].getElementsByTagName("SPAN")[0];
                postJson = postJson + aryDiv[i].id + ":" + lastValue + "/" + span.innerHTML + ",";
            } else
                postJson = postJson + aryDiv[i].id + ":,";

        }
    }
    return postJson;

}


//保存入库
function Save() {

}
//发布
function Publish() {

}
//内容审核
function Check() {
   
}

//获取AQI分时段的文本内容
function GetAQIPeriodContent(type) {   
    var txtContent  = $("#txtHidePeriodAQITxtTemplete").val();
    //文本类型
    if (type == "msg") {
        txtContent  = $("#txtHidePeriodAQIMsgTemplete").val();
    }
    else {
        txtContent = $("#txtHidePeriodAQITxtTemplete").val();
    }  
    txtContent = txtContent.replace("{PublishDate}", getFormatDate(generalDateType));
    txtContent = txtContent.replace("{Hour}", "17时");
    //今天夜间 H3166
    var toNightCellValue = $("#H3166").text();
    var tonightAQI = "";
    var tonightItem = "";
    if (toNightCellValue != "") {
        tonightAQI = toNightCellValue.split('/')[0];
        tonightItem = toNightCellValue.split('/')[1];
    }   
    var toRange = "";
    var toLevel
    if (tonightAQI != "") {
        toRange = CalculateAQIRange(tonightAQI);
        toLevel = CalculateAQLLevelRange(tonightAQI);
    }
    else {
        toRange = "--/--";
        toLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTonight}", toRange);
    if (toLevel == "优") {
        txtContent = txtContent.replace("{LevelNight}", toLevel);
    }
    else {
        txtContent = txtContent.replace("{LevelNight}", toLevel + "(" + "首要污染物" + tonightItem + ")");
    }    

    //明天上午 H4126
    var tomMorCellValue = $("#H4126").text();
    var tomMorAQI = "";
    var tmMorItem = "";
    if (tomMorCellValue != "") {
        tomMorAQI = tomMorCellValue.split('/')[0];
        tmMorItem = tomMorCellValue.split('/')[1];
    }
    var tomMorRange = "";
    var tomMorLevel
    if (tonightAQI != "") {
        tomMorRange = CalculateAQIRange(tomMorAQI);
        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
    }
    else {
        tomMorRange = "--/--";
        tomMorLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
    if (tomMorLevel == "优") {
        txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
    }
    else {
        txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + "首要污染物" + tmMorItem + ")");
    }    
    
    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "";
    if (toNightCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        tmAftItem = tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    if (tomAftLevel == "优") {
        txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    }
    else {
        txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
    }  
    return txtContent;
}

//点击历史数据获取昨天数据后，当前系统时间为上午，显示文本或短信只有今天上午和下午
function GetAQIPeriodContentTwoLines(type) {
    var txtContent = $("#txtHidePeriodAQITxtTwoLines").val();
    //文本类型
    if (type == "msg") {
        txtContent = $("#txtHidePeriodAQIMsgTempleteTwoLines").val();
    }
    else {
        txtContent = $("#txtHidePeriodAQITxtTwoLines").val();
    }

//    //当天的短信已发送
//    if (msgPubState == "Pub") {
//        var curHour = new Date().getHours();
//        txtContent = txtContent.replace("{Hour}", curHour + "时");
//    }
//    else {
//        txtContent = txtContent.replace("{Hour}", "17时");
//    }
    var curHour = new Date().getHours();
    txtContent = txtContent.replace("{Hour}", curHour + "时");
    txtContent = txtContent.replace("{PublishDate}", getFormatDate(""));

    //今天夜间 H3166
//    var toNightCellValue = $("#H3166").text();
//    var tonightAQI = "";
//    var tonightItem = "";
//    if (toNightCellValue != "") {
//        tonightAQI = toNightCellValue.split('/')[0];
//        tonightItem = toNightCellValue.split('/')[1];
//    }
//    var toRange = "";
//    var toLevel
//    if (tonightAQI != "") {
//        toRange = CalculateAQIRange(tonightAQI);
//        toLevel = CalculateAQLLevelRange(tonightAQI);
//    }
//    else {
//        toRange = "--/--";
//        toLevel = "无";
//    }

//    txtContent = txtContent.replace("{RangeTonight}", toRange);
//    if (toLevel == "优") {
//        txtContent = txtContent.replace("{LevelNight}", toLevel);
//    }
//    else {
//        txtContent = txtContent.replace("{LevelNight}", toLevel + "(" + "首要污染物" + tonightItem + ")");
//    }

    //明天上午 H4126
    var tomMorCellValue = $("#H4126").text();
    var tomMorAQI = "";
    var tmMorItem = "";
    if (tomMorCellValue != "") {
        tomMorAQI = tomMorCellValue.split('/')[0];
        tmMorItem = tomMorCellValue.split('/')[1];
    }
    var tomMorRange = "";
    var tomMorLevel
    if (tomMorAQI != "") {
        tomMorRange = CalculateAQIRange(tomMorAQI);
        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
    }
    else {
        tomMorRange = "--/--";
        tomMorLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
  

    if (tomMorLevel == "优") {
        txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
    }
    else {
        txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + "首要污染物" + tmMorItem + ")");
    }

    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "";
    if (tomAfterCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        tmAftItem = tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    if (tomAftLevel == "优") {
        txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    }
    else {
        txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
    }
    return txtContent;
}
//点击历史数据获取昨天数据后，当前系统时间为下午，显示文本或短信只有今天下午
function GetAQIPeriodContentOneLine(type) {
    var txtContent = $("#txtHidePeriodAQITxtOneLine").val();
    //文本类型
    if (type == "msg") {
        txtContent = $("#txtHidePeriodAQIMsgTempleteOneLine").val();
    }
    else {
        txtContent = $("#txtHidePeriodAQITxtOneLine").val();
    }

//    //当天的短信已发送
//    if (msgPubState == "Pub") {
//        var curHour = new Date().getHours();
//        txtContent = txtContent.replace("{Hour}", curHour + "时");
//    }
//    else {
//        txtContent = txtContent.replace("{Hour}", "17时");
//    }

    var curHour = new Date().getHours();
    txtContent = txtContent.replace("{Hour}", curHour + "时");
    txtContent = txtContent.replace("{PublishDate}", getFormatDate(""));    
    //今天夜间 H3166
//    var toNightCellValue = $("#H3166").text();
//    var tonightAQI = "";
//    var tonightItem = "";
//    if (toNightCellValue != "") {
//        tonightAQI = toNightCellValue.split('/')[0];
//        tonightItem = toNightCellValue.split('/')[1];
//    }
//    var toRange = "";
//    var toLevel
//    if (tonightAQI != "") {
//        toRange = CalculateAQIRange(tonightAQI);
//        toLevel = CalculateAQLLevelRange(tonightAQI);
//    }
//    else {
//        toRange = "--/--";
//        toLevel = "无";
//    }

//    txtContent = txtContent.replace("{RangeTonight}", toRange);
//    if (toLevel == "优") {
//        txtContent = txtContent.replace("{LevelNight}", toLevel);
//    }
//    else {
//        txtContent = txtContent.replace("{LevelNight}", toLevel + "(" + "首要污染物" + tonightItem + ")");
//    }

//    //明天上午 H4126
//    var tomMorCellValue = $("#H4126").text();
//    var tomMorAQI = "";
//    var tmMorItem = "";
//    if (tomMorCellValue != "") {
//        tomMorAQI = tomMorCellValue.split('/')[0];
//        tmMorItem = tomMorCellValue.split('/')[1];
//    }
//    var tomMorRange = "";
//    var tomMorLevel
//    if (tonightAQI != "") {
//        tomMorRange = CalculateAQIRange(tomMorAQI);
//        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
//    }
//    else {
//        tomMorRange = "--/--";
//        tomMorLevel = "无";
//    }

//    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
//    if (toLevel == "优") {
//        txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
//    }
//    else {
//        txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + "首要污染物" + tmMorItem + ")");
//    }

    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "";
    if (tomAfterCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        tmAftItem = tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    if (tomAftLevel == "优") {
        txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    }
    else {
        txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
    }
    return txtContent;
}

function GetAQIPeriodTextContent() {
    var txtContent = $("#txtHidePeriodAQITxtTemplete").val();    
    //文本类型   

    txtContent = txtContent.replace("{PublishDate}", getFormatDateForAQIPeriod(generalDateType));
    txtContent = txtContent.replace("{Hour}", "17时");
    //今天夜间 H3166
    var toNightCellValue = $("#H3166").text();
    var tonightAQI = "";

    var tonightItem = "PM2.5";
    if (toNightCellValue != "") {
        tonightAQI = toNightCellValue.split('/')[0];
        tonightItem = (toNightCellValue.split('/')[1] == "O38h") ? "" : toNightCellValue.split('/')[1];
        
    }
    var toRange = "";
    var toLevel
    if (tonightAQI != "") {
        toRange = CalculateAQIRange(tonightAQI);
        toLevel = CalculateAQLLevelRange(tonightAQI);
    }
    else {
        toRange = "--/--";
        toLevel = "无";
    }
    if (toLevel == "优") {
        tonightItem = "-";
    }
    txtContent = txtContent.replace("{RangeTonight}", toRange);
    txtContent = txtContent.replace("{LevelNight}", toLevel);
    txtContent = txtContent.replace("{AQIItemTonight}", tonightItem);
    
    //明天上午 H4126
    var tomMorCellValue = $("#H4126").text();
    var tomMorAQI = "";
    var tmMorItem = "PM2.5";
    if (tomMorCellValue != "") {
        tomMorAQI = tomMorCellValue.split('/')[0];
        //        tmMorItem = tomMorCellValue.split('/')[1];
        tmMorItem = (tomMorCellValue.split('/')[1] == "O38h") ? "" : tomMorCellValue.split('/')[1];
    }
    var tomMorRange = "";
    var tomMorLevel
    if (tonightAQI != "") {
        tomMorRange = CalculateAQIRange(tomMorAQI);
        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
    }
    else {
        tomMorRange = "--/--";
        tomMorLevel = "无";
    }
    if (tomMorLevel == "优") {
        tmMorItem = "-";
    }
    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
    txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
    txtContent = txtContent.replace("{AQIItemTomMorning}", tmMorItem);
   
    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "PM2.5";
    if (toNightCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        //tmAftItem = tomAfterCellValue.split('/')[1];
        tmAftItem = (tomAfterCellValue.split('/')[1] == "O38h") ? "" : tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }
    if (tomAftLevel == "优") {
        tmAftItem = "-";
    }
    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    txtContent = txtContent.replace("{AQIItemTomAfternoon}", tmAftItem);
    return txtContent;
}

//点击获取 历史数据之后，文本内容读取为昨天的数据，日期也全部是昨天的
function GetAQIPeriodTextContentYesterday(dateType) {
    var txtContent = $("#txtHidePeriodAQITxtTemplete").val();
    //文本类型
    if (dateType == "" || dateType == null) {
        dateType = "Yesterday";
    }
    txtContent = txtContent.replace("{PublishDate}", getFormatDateForAQIPeriod(dateType));
    txtContent = txtContent.replace("{Hour}", "17时");
    //今天夜间 H3166
    var toNightCellValue = $("#H3166").text();
    var tonightAQI = "";

    var tonightItem = "PM2.5";
    if (toNightCellValue != "") {
        tonightAQI = toNightCellValue.split('/')[0];
        tonightItem = (toNightCellValue.split('/')[1] == "O38h") ? "" : toNightCellValue.split('/')[1];
    }
    var toRange = "";
    var toLevel
    if (tonightAQI != "") {
        toRange = CalculateAQIRange(tonightAQI);
        toLevel = CalculateAQLLevelRange(tonightAQI);
    }
    else {
        toRange = "--/--";
        toLevel = "无";
    }
    if (toLevel == "优") {
        tonightItem = "-";
    }
    txtContent = txtContent.replace("{RangeTonight}", toRange);
    txtContent = txtContent.replace("{LevelNight}", toLevel);
    txtContent = txtContent.replace("{AQIItemTonight}", tonightItem);

    //明天上午 H4126
    var tomMorCellValue = $("#H4126").text();
    var tomMorAQI = "";
    var tmMorItem = "PM2.5";
    if (tomMorCellValue != "") {
        tomMorAQI = tomMorCellValue.split('/')[0];
        //        tmMorItem = tomMorCellValue.split('/')[1];
        tmMorItem = (tomMorCellValue.split('/')[1] == "O38h") ? "" : tomMorCellValue.split('/')[1];
    }
    var tomMorRange = "";
    var tomMorLevel
    if (tonightAQI != "") {
        tomMorRange = CalculateAQIRange(tomMorAQI);
        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
    }
    else {
        tomMorRange = "--/--";
        tomMorLevel = "无";
    }
    if (tomMorLevel == "优") {
        tmMorItem = "-";
    }
    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
    txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
    txtContent = txtContent.replace("{AQIItemTomMorning}", tmMorItem);

    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "PM2.5";
    if (toNightCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        //tmAftItem = tomAfterCellValue.split('/')[1];
        tmAftItem = (tomAfterCellValue.split('/')[1] == "O38h") ? "" : tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }
    if (tomAftLevel == "优") {
        tmAftItem = "-";
    }
    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    txtContent = txtContent.replace("{AQIItemTomAfternoon}", tmAftItem);
    return txtContent;
}


function GetAQIPeriodTextContentTwoLines() {
    var txtContent = $("#txtHidePeriodAQITxtTwoLines").val();
    //文本类型   

    txtContent = txtContent.replace("{PublishDate}", getFormatDate(generalDateType));
    txtContent = txtContent.replace("{Hour}", "17时");
    //今天夜间 H3166
//    var toNightCellValue = $("#H3166").text();
//    var tonightAQI = "";

//    var tonightItem = "PM2.5";
//    if (toNightCellValue != "") {
//        tonightAQI = toNightCellValue.split('/')[0];
//        tonightItem = (toNightCellValue.split('/')[1] == "O38h") ? "" : toNightCellValue.split('/')[1];
//    }
//    var toRange = "";
//    var toLevel
//    if (tonightAQI != "") {
//        toRange = CalculateAQIRange(tonightAQI);
//        toLevel = CalculateAQLLevelRange(tonightAQI);
//    }
//    else {
//        toRange = "--/--";
//        toLevel = "无";
//    }

//    txtContent = txtContent.replace("{RangeTonight}", toRange);
//    txtContent = txtContent.replace("{LevelNight}", toLevel);
//    txtContent = txtContent.replace("{AQIItemTonight}", tonightItem);

    //明天上午 H4126
    var tomMorCellValue = $("#H4126").text();
    var tomMorAQI = "";
    var tmMorItem = "PM2.5";
    if (tomMorCellValue != "") {
        tomMorAQI = tomMorCellValue.split('/')[0];
        //        tmMorItem = tomMorCellValue.split('/')[1];
        tmMorItem = (tomMorCellValue.split('/')[1] == "O38h") ? "" : tomMorCellValue.split('/')[1];
    }
    var tomMorRange = "";
    var tomMorLevel
    if (tomMorAQI != "") {
        tomMorRange = CalculateAQIRange(tomMorAQI);
        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
    }
    else {
        tomMorRange = "--/--";
        tomMorLevel = "无";
    }
    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
    txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
    txtContent = txtContent.replace("{AQIItemTomMorning}", tmMorItem);

    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "PM2.5";
    if (tomAfterCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        //tmAftItem = tomAfterCellValue.split('/')[1];
        tmAftItem = (tomAfterCellValue.split('/')[1] == "O38h") ? "" : tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }
    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    txtContent = txtContent.replace("{AQIItemTomAfternoon}", tmAftItem);
    return txtContent;
}

function GetAQIPeriodTextContentOneLine() {
    var txtContent = $("#txtHidePeriodAQITxtOneLine").val();
    //文本类型   

    txtContent = txtContent.replace("{PublishDate}", getFormatDate(generalDateType));
    txtContent = txtContent.replace("{Hour}", "17时");
    //今天夜间 H3166
//    var toNightCellValue = $("#H3166").text();
//    var tonightAQI = "";

//    var tonightItem = "PM2.5";
//    if (toNightCellValue != "") {
//        tonightAQI = toNightCellValue.split('/')[0];
//        tonightItem = (toNightCellValue.split('/')[1] == "O38h") ? "" : toNightCellValue.split('/')[1];
//    }
//    var toRange = "";
//    var toLevel
//    if (tonightAQI != "") {
//        toRange = CalculateAQIRange(tonightAQI);
//        toLevel = CalculateAQLLevelRange(tonightAQI);
//    }
//    else {
//        toRange = "--/--";
//        toLevel = "无";
//    }

//    txtContent = txtContent.replace("{RangeTonight}", toRange);
//    txtContent = txtContent.replace("{LevelNight}", toLevel);
//    txtContent = txtContent.replace("{AQIItemTonight}", tonightItem);

    //明天上午 H4126
//    var tomMorCellValue = $("#H4126").text();
//    var tomMorAQI = "";
//    var tmMorItem = "PM2.5";
//    if (tomMorCellValue != "") {
//        tomMorAQI = tomMorCellValue.split('/')[0];
//        //        tmMorItem = tomMorCellValue.split('/')[1];
//        tmMorItem = (tomMorCellValue.split('/')[1] == "O38h") ? "" : tomMorCellValue.split('/')[1];
//    }
//    var tomMorRange = "";
//    var tomMorLevel
//    if (tonightAQI != "") {
//        tomMorRange = CalculateAQIRange(tomMorAQI);
//        tomMorLevel = CalculateAQLLevelRange(tomMorAQI);
//    }
//    else {
//        tomMorRange = "--/--";
//        tomMorLevel = "无";
//    }
//    txtContent = txtContent.replace("{RangeTomMorning}", tomMorRange);
//    txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel);
//    txtContent = txtContent.replace("{AQIItemTomMorning}", tmMorItem);

    //明天下午 H4136
    var tomAfterCellValue = $("#H4136").text();
    var tomAftAQI = "";
    var tmAftItem = "PM2.5";
    if (tomAfterCellValue != "") {
        tomAftAQI = tomAfterCellValue.split('/')[0];
        //tmAftItem = tomAfterCellValue.split('/')[1];
        tmAftItem = (tomAfterCellValue.split('/')[1] == "O38h") ? "" : tomAfterCellValue.split('/')[1];
    }
    var tomAftRange = "";
    var tomAftLevel
    if (tomAftAQI != "") {
        tomAftRange = CalculateAQIRange(tomAftAQI);
        tomAftLevel = CalculateAQLLevelRange(tomAftAQI);
    }
    else {
        tomAftRange = "--/--";
        tomAftLevel = "无";
    }
    txtContent = txtContent.replace("{RangeTomAfternoon}", tomAftRange);
    txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel);
    txtContent = txtContent.replace("{AQIItemTomAfternoon}", tmAftItem);
    return txtContent;
}


//获取AQI分时段的短信内容
function GetAQIPeriodMsg() {
    var msgContent = textContent = $("#txtHidePeriodAQIMsgTemplete").text();
    Ext.get("msgArea").update(msgContent);
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

//获取最后一列AQI的显示内容
function calculateAQI(result) {
    var idPrefixArray = ['P314', 'P411', 'P316', 'P412', 'P413', 'P414', 'P511', 'P416', 'P512', 'P513', 'P417', 'P517', 'P324', 'P421', 'P326', 'P422', 'P423', 'P424', 'P521', 'P426', 'P522', 'P523', 'P427', 'P527', 'H314', 'H411', 'H316', 'H412', 'H413', 'H414', 'H511', 'H416', 'H512', 'H513', 'H417', 'H517'];
    for (var i = 0; i < idPrefixArray.length; i++) {
            var cellValue = "";
            var aqiValue = "";
            var itemType = "";
            for (var obj in result) {
                if (obj.substr(0, 4) == idPrefixArray[i] && obj.substr(4, 1) != "0") {
                    var tempCellValue = result[obj];
                    var tempAqiValue = result[obj].substr(tempCellValue.indexOf('>') + 1);
                    tempAqiValue = tempAqiValue.substr(0, tempAqiValue.indexOf('<'));
                    if (aqiValue == "") {
                        cellValue = tempCellValue;
                        aqiValue = tempAqiValue;
                         switch (obj.substr(obj.length - 1, 1)) {
                            case '1':
                                itemType = "PM2.5";
                                break;
                            case '2':
                                itemType = "PM10";
                                break;
                            case '3':
                                itemType = "NO2";
                                break;
                            case '4':
                                //itemType = "O31h";
                                itemType = "O3";
                                break;
                            case '5':
                                var firsItemAndAQI = calculateSecondItem(idPrefixArray[i], result);
                                itemType = firsItemAndAQI.split('&')[0];
                                aqiValue = firsItemAndAQI.split('&')[1];
                                break;
                        }
                    }
                    else {
                        if (parseInt(tempAqiValue) > parseInt(aqiValue)) {
                            aqiValue = tempAqiValue;
                            cellValue = tempCellValue;
                            switch (obj.substr(obj.length - 1, 1)) {
                                case '1':
                                    itemType = "PM2.5";
                                    break;
                                case '2':
                                    itemType = "PM10";
                                    break;
                                case '3':
                                    itemType = "NO2";
                                    break;
                                case '4':
                                    //itemType = "O31h";
                                    itemType = "O3";
                                    break;
                                case '5':
                                    var firsItemAndAQI=calculateSecondItem(idPrefixArray[i], result);
                                    itemType = firsItemAndAQI.split('&')[0];
                                    aqiValue = firsItemAndAQI.split('&')[1];
                                    break;
                            }
                        }
                    }                    
                }                
            }
            result[idPrefixArray[i] + '6'] = aqiValue +"/"+ "<span>" + itemType + "</span>"; 
    }
}

    //计算一行内首要污染物，根据一行所有单元格所有共同的前缀
    function calculateFirstItemSingleLine(result,idPrefix) {
        if (reslut.length > 0 && idPrefix != "") {
            for (var obj in result) {
                if (obj.substr(0, 4) == idPrefix && obj.substr(4, 1) != "0") {
                    var tempCellValue = result[obj];
                    var tempAqiValue = result[obj].substr(tempCellValue.indexOf('>') + 1);
                    tempAqiValue = tempAqiValue.substr(0, tempAqiValue.indexOf('<'));
                    if (aqiValue == "") {
                        cellValue = tempCellValue;
                        aqiValue = tempAqiValue;
                        switch (obj.substr(obj.length - 1, 1)) {
                            case '1':
                                itemType = "PM2.5";
                                break;
                            case '2':
                                itemType = "PM10";
                                break;
                            case '3':
                                itemType = "NO2";
                                break;
                            case '4':
                                itemType = "O3";
                                break;
                            case '5':
                                itemType = "O38h";
                                //计算指数第二大的那个类型
                                break;
                        }
                    }
                    else {
                        if (parseInt(tempAqiValue) > parseInt(aqiValue)) {
                            aqiValue = tempAqiValue;
                            cellValue = tempCellValue;
                            switch (obj.substr(obj.length - 1, 1)) {
                                case '1':
                                    itemType = "PM2.5";
                                    break;
                                case '2':
                                    itemType = "PM10";
                                    break;
                                case '3':
                                    itemType = "NO2";
                                    break;
                                case '4':
                                    itemType = "O3";
                                    break;
                                case '5':
                                    //计算指数第二大的那个类型
                                    itemType = "O38h";
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    //首要污染物为O38h，选择第二大作为首要污染物
    function calculateSecondItem(idPrefix, result) {
        var firstItem = "";
        var firstAQI = "";
        if (result && idPrefix != "") {
            var aqiValue = "";
            var itemType="";
            for (var obj in result) {
                if (obj.substr(0, 4) == idPrefix && obj.substr(4, 1) != "0" && obj.substr(4, 1) != "5") {
                    var tempCellValue = result[obj];
                    var tempAqiValue = result[obj].substr(tempCellValue.indexOf('>') + 1);
                    tempAqiValue = tempAqiValue.substr(0, tempAqiValue.indexOf('<'));
                    if (aqiValue == "") {
                        cellValue = tempCellValue;
                        aqiValue = tempAqiValue;
                        switch (obj.substr(obj.length - 1, 1)) {
                            case '1':
                                itemType = "PM2.5";
                                break;
                            case '2':
                                itemType = "PM10";
                                break;
                            case '3':
                                itemType = "NO2";
                                break;
                            case '4':
                                itemType = "O3";
                                break;
                            default:
                                itemType = "PM2.5";
                                //计算指数第二大的那个类型
                                break;
                        }
                    }
                    else {
                        if (parseInt(tempAqiValue) > parseInt(aqiValue)) {
                            aqiValue = tempAqiValue;
                            cellValue = tempCellValue;
                            switch (obj.substr(obj.length - 1, 1)) {
                                case '1':
                                    itemType = "PM2.5";
                                    break;
                                case '2':
                                    itemType = "PM10";
                                    break;
                                case '3':
                                    itemType = "NO2";
                                    break;
                                case '4':
                                    itemType = "O3";
                                    break;
                                default:
                                    //计算指数第二大的那个类型
                                    itemType = "PM2.5";
                                    break;
                            }
                        }
                    }
                    firstItem = itemType;
                    firstAQI = aqiValue;
                }
            }
        }
        return firstItem + "&" + firstAQI;
    }


    //从html标签字符串内提取出数字（格式为：68<span>100</span>）
    function getValueFromHtml(htmlString) {
        if (htmlString) {
            var tempAqiValue = result[obj].substr(htmlString.indexOf('>') + 1);
            tempAqiValue = tempAqiValue.substr(0, tempAqiValue.indexOf('<'));
            return tempAqiValue;
        }
    }

    //根据AQI值计算AQI预报范围
    function CalculateAQIRange(aqiString) {
        var aqiRange = "";
        if (aqiString != "") {
            var aqiValue = parseInt(aqiString);
            //根据靠近0还是5计算出的实际使用值
            var useValue = 0;
            if (aqiValue > 0) {
                if (aqiValue % 10 == 0) {
                    useValue = aqiValue;
                    if ((useValue - 10) == 50 || (useValue - 10) == 100 || (useValue - 10) == 150 || (useValue - 10) == 200 || (useValue - 10) == 250 || (useValue - 10) == 300) {
                        useValue = useValue + 5;
                    }
                }
                else {
                    //更靠近0
                    //                    if (Math.abs(aqiValue % 10) < Math.abs(aqiValue % 10 - 5)) {
                    var zeroValue =0;
                    if (aqiValue % 10 > 5) {
                        zeroValue = Math.abs(aqiValue % 10 - 10);
                    }
                    else {
                        zeroValue = Math.abs(aqiValue % 10);
                    }
                    if (zeroValue < Math.abs(aqiValue % 10 - 5)) {
                        if (aqiValue % 10 > 5) {
                            useValue = 10 * (Math.ceil(aqiValue / 10));
                        }
                        else {
                            useValue = 10 * (Math.floor(aqiValue / 10));
                        }
//                        useValue = 10 * (Math.floor(aqiValue / 10));
                        if ((useValue - 10) == 50 || (useValue - 10) == 100 || (useValue - 10) == 150 || (useValue - 10) == 200 || (useValue - 10) == 250 || (useValue - 10) == 300) {
                            useValue = useValue + 5;
                        }

                    }
                    //更靠近5
                    else {
                        useValue = 10 * (Math.floor(aqiValue / 10)) + 5;
                    }
                }
                aqiRange = (useValue - 10).toString() + "-" + (useValue + 10).toString();
            }
        }
        return aqiRange;
    }

    function CalculateAQLLevelRange(aqiString) {
        var aqiRange = "";
        if (aqiString != "") {
            var aqiValue = parseInt(aqiString);
            //根据靠近0还是5计算出的事迹使用值
            var useValue = 0;
            if (aqiValue > 0) {
                //更靠近0
                if (Math.abs(aqiValue % 10 - 10) < Math.abs(aqiValue % 10 - 5)) {
                    useValue = 10 * (Math.ceil(aqiValue / 10));
                    if ((useValue - 10) == 50 || (useValue - 10) == 100 || (useValue - 10) == 150 || (useValue - 10) == 200 || (useValue - 10) == 250 || (useValue - 10) == 300) {
                        useValue = useValue + 5;
                    }
                }
                //更靠近5
                else {
                    useValue = 10 * (Math.floor(aqiValue / 10)) + 5;
                }
                aqiRange = (useValue - 10).toString() + "-" + (useValue + 10).toString();
                var leftRange = CalculateAQLLevel(useValue - 10);
                var rightRange = CalculateAQLLevel(useValue + 10);
                if (leftRange == rightRange) {
                    aqiLevelRange = leftRange;
                }
                else {
                    //                    aqiLevelRange = leftRange + "到" + rightRange;
                    if (leftRange.indexOf('污染') > 0) {
                        aqiLevelRange = leftRange.substr(0, 2) + "到" + rightRange;
                    }
                    else {
                        aqiLevelRange = leftRange + "到" + rightRange;
                    }
                }
            }
        }
        return aqiLevelRange;
    }

    //使用1小时的算法重新计算No2的指数值
    function calculatrNO2(result) {
        var idPrefixArray = ['P4173', 'P4273', 'H4173', 'P5173', 'P5273', 'H5173'];
        var calSource = "";
        for (var i = 0; i < idPrefixArray.length; i++) {
            var cellValue = "";
            var aqiValue = "";
            var itemType = "";
            for (var obj in result) {
                if (obj == idPrefixArray[i]) {
//                    var tempCellValue = result[obj];
//                    var tempAqiValue = result[obj].substr(tempCellValue.indexOf('>') + 1);
//                    tempAqiValue = tempAqiValue.substr(0, tempAqiValue.indexOf('<'));
                    var dencity = result[obj].split('/')[0];
                    if (dencity != "") {
                        calSource += obj + ":" + dencity + ",";
                    }
                }

             }
         }
        calSource = calSource.substr(0, calSource.length - 2);
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'CalculateNO2WithHourMethod'),
            params: { content: calSource },
            success: function (response) {
                if (response.responseText != "") {
                    var contents = response.responseText.split(',');
                    for (var i = 0; i < contents.length; i++) {
                        var oriDen = result[contents[i].split(':')[0]].split('/')[0]
                        $("#" + contents[i].split(':')[0]).html(oriDen + "/" + "<span>" + contents[i].split(':')[1] + "</span>");
                        //result[contents[i].split(':')[0]] = oriDen + "/" + "<span>" + contents[i].split(':')[1] + "</span>";
                    }

                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }

    //当一行内一个单元格内数据改变时，行末的AQI联动变化
    function calculateAQIOfSingleLine(cellId) {
        if (cellId != "") {
            var cellPrefix = cellId.substring(0, 4);
            var maxAQIValue = $("#" + cellPrefix + "1").html().split('/')[1];
            maxAQIValue = maxAQIValue.replace(/<[^>]+>/g, "");
            maxAQIValue = maxAQIValue.replace("<", ""); 
            //            var itemId = cellPrefix.substr(2, 1);
            var itemId = "1";
            var tempAQIValue = $("#" + cellPrefix + "1").html().split('/')[1];
            tempAQIValue = tempAQIValue.replace(/<[^>]+>/g, "");
            tempAQIValue = tempAQIValue.replace("<", ""); 
            for (var i = 1; i < 6; i++) {
                tempAQIValue = $("#" + cellPrefix + i.toString()).html().split('/')[1];
                tempAQIValue = tempAQIValue.replace(/<[^>]+>/g, "");
                tempAQIValue = tempAQIValue.replace("<", ""); 
                if (parseInt(tempAQIValue) > parseInt(maxAQIValue)) {
                    maxAQIValue = tempAQIValue;
                    itemId = i.toString();
                }                
            }
           
            var item = "PM2.5";
            switch (itemId) {
                case "1":
                    item = "PM2.5";
                    break;
                case "2":
                    item = "PM10";
                    break;
                case "3":
                    item = "NO2";
                    break;
                case "4":
                    item = "O31_h";
                    break;
                case "5":
                    item = "O38_h";
                    break;
                default:
                    item = "PM2.5";
                    break;

            }
            var lineEndId = cellPrefix + "6";
            $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
        }
    }
 
 //当首要污染物为O3_8h时，选择第二大的为首要污染物
    function calculateAQIOfSingleLineSecondCopy(cellId) {
        if (cellId != "") {
            var cellPrefix = cellId.substring(0, 4);
            var maxAQIValue = $("#" + cellPrefix + "1").html().split('/')[1];
            maxAQIValue = maxAQIValue.replace(/<[^>]+>/g, "");
            maxAQIValue = maxAQIValue.replace("<", "");
            //第二大的AQI值
            var secondMaxAQIValue = $("#" + cellPrefix + "2").html().split('/')[1];
            secondMaxAQIValue = secondMaxAQIValue.replace(/<[^>]+>/g, "");
            secondMaxAQIValue = secondMaxAQIValue.replace("<", "");

            var itemId = "1";
            var secondItemID = "2";
            var tempAQIValue = $("#" + cellPrefix + "1").html().split('/')[1];
            tempAQIValue = tempAQIValue.replace(/<[^>]+>/g, "");
            tempAQIValue = tempAQIValue.replace("<", "");
            for (var i = 1; i < 6; i++) {
                tempAQIValue = $("#" + cellPrefix + i.toString()).html().split('/')[1];
                tempAQIValue = tempAQIValue.replace(/<[^>]+>/g, "");
                tempAQIValue = tempAQIValue.replace("<", "");
                if (parseInt(tempAQIValue) > parseInt(maxAQIValue)) {
                    secondMaxAQIValue = maxAQIValue;
                    maxAQIValue = tempAQIValue;
                    secondItemID = itemId;
                    itemId = i.toString();
                    
                }
                else if (parseInt(tempAQIValue) < parseInt(maxAQIValue) && parseInt(tempAQIValue) > parseInt(secondMaxAQIValue)) {
                    secondMaxAQIValue = tempAQIValue;
                    secondItemID = i.toString();
                }
            }
            var lineEndId = cellPrefix + "6";
            var item = "PM2.5";
            var seconfItem="PM2.5";
            switch (secondItemID) {
                case "1":
                    seconfItem = "PM2.5";
                    break;
                case "2":
                    seconfItem = "PM10";
                    break;
                case "3":
                    seconfItem = "NO2";
                    break;
                case "4":
                    seconfItem = "O31_h";
                    break;                             
                default:
                    seconfItem = "PM2.5";
                    break;
            }
            switch (itemId) {
                case "1":
                    item = "PM2.5";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "2":
                    item = "PM10";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "3":
                    item = "NO2";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "4":
                    item = "O31_h";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "5":
                    item = "O38_h";
                    $("#" + lineEndId).html(secondMaxAQIValue + "/<span>" + seconfItem + "</span>");
                    break;
                default:
                    item = "PM2.5";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
            }
        }
    }

    //当首要污染物为O3_8h时，选择第二大的为首要污染物
    function calculateAQIOfSingleLineSecond(cellId) {
        if (cellId != "") {
            var cellPrefix = cellId.substring(0, 4);
            var maxAQIValue = $("#" + cellPrefix + "1").html().split('/')[1];
            maxAQIValue = maxAQIValue.replace(/<[^>]+>/g, "");
            maxAQIValue = maxAQIValue.replace("<", "");
            //第二大的AQI值
            var secondMaxAQIValue = $("#" + cellPrefix + "2").html().split('/')[1];
            secondMaxAQIValue = secondMaxAQIValue.replace(/<[^>]+>/g, "");
            secondMaxAQIValue = secondMaxAQIValue.replace("<", "");

            var itemId = "1";
            var secondItemID = "2";
            var tempAQIValue = $("#" + cellPrefix + "1").html().split('/')[1];
            tempAQIValue = tempAQIValue.replace(/<[^>]+>/g, "");
            tempAQIValue = tempAQIValue.replace("<", "");
            for (var i = 1; i < 6; i++) {
                tempAQIValue = $("#" + cellPrefix + i.toString()).html().split('/')[1];
                tempAQIValue = tempAQIValue.replace(/<[^>]+>/g, "");
                tempAQIValue = tempAQIValue.replace("<", "");
                if (parseInt(tempAQIValue) > parseInt(maxAQIValue)) {
                    secondMaxAQIValue = maxAQIValue;
                    maxAQIValue = tempAQIValue;
                    secondItemID = itemId;
                    itemId = i.toString();
                }
                else if (parseInt(tempAQIValue) < parseInt(maxAQIValue) && parseInt(tempAQIValue) > parseInt(secondMaxAQIValue)) {
                    secondMaxAQIValue = tempAQIValue;
                    secondItemID = i.toString();
                }
            }
            var lineEndId = cellPrefix + "6";
            var item = "PM2.5";
            var seconfItem = "PM2.5";
            switch (secondItemID) {
                case "1":
                    seconfItem = "PM2.5";
                    break;
                case "2":
                    seconfItem = "PM10";
                    break;
                case "3":
                    seconfItem = "NO2";
                    break;
                case "4":
                    seconfItem = "O3";
                    break;
                default:
                    seconfItem = "PM2.5";
                    break;
            }
            switch (itemId) {
                case "1":
                    item = "PM2.5";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "2":
                    item = "PM10";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "3":
                    item = "NO2";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "4":
                    item = "O3";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
                case "5":
                    item = "O38_h";
                    $("#" + lineEndId).html(secondMaxAQIValue + "/<span>" + seconfItem + "</span>");
                    break;
                default:
                    item = "PM2.5";
                    $("#" + lineEndId).html(maxAQIValue + "/<span>" + item + "</span>");
                    break;
            }            
        }
    }

    //当上半夜，下半夜和夜间的数据变动时，相关单元格的数据联动
    function processAverageValue(cellId) {
        if (cellId[4] != "4" && cellId[4] != "5") {
            //上半夜或者下半夜数据改变，重新计算夜间的数据
            if (cellId.substring(0, 4) == "P314" || cellId.substring(0, 4) == "P411") {
                var cellEnd = cellId[4];
                var earlyNinghtDenValue = $("#" + "P314" + cellEnd).html().split('/')[0];
                var lateNightDenValue = $("#" + "P411" + cellEnd).html().split('/')[0];
                var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: nightValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + "P316" + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond("P316" + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }

            if (cellId.substring(0, 4) == "P324" || cellId.substring(0, 4) == "P421") {
                var cellEnd = cellId[4];
                var earlyNinghtDenValue = $("#" + "P324" + cellEnd).html().split('/')[0];
                var lateNightDenValue = $("#" + "P421" + cellEnd).html().split('/')[0];
                var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: nightValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + "P326" + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond("P326" + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }

            if (cellId.substring(0, 4) == "H314" || cellId.substring(0, 4) == "H411") {
                var cellEnd = cellId[4];
                var earlyNinghtDenValue = $("#" + "H314" + cellEnd).html().split('/')[0];
                var lateNightDenValue = $("#" + "H411" + cellEnd).html().split('/')[0];
                var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: nightValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + "H316" + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond("H316" + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }


            if (cellId.substring(0, 4) == "P414" || cellId.substring(0, 4) == "P511") {
                var cellEnd = cellId[4];
                var earlyNinghtDenValue = $("#" + "P414" + cellEnd).html().split('/')[0];
                var lateNightDenValue = $("#" + "P511" + cellEnd).html().split('/')[0];
                var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: nightValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + "P416" + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond("P416" + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }

            if (cellId.substring(0, 4) == "P424" || cellId.substring(0, 4) == "P521") {
                var cellEnd = cellId[4];
                var earlyNinghtDenValue = $("#" + "P424" + cellEnd).html().split('/')[0];
                var lateNightDenValue = $("#" + "P521" + cellEnd).html().split('/')[0];
                var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: nightValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + "P426" + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond("P426" + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }

            if (cellId.substring(0, 4) == "H414" || cellId.substring(0, 4) == "H511") {
                var cellEnd = cellId[4];
                var earlyNinghtDenValue = $("#" + "H414" + cellEnd).html().split('/')[0];
                var lateNightDenValue = $("#" + "H511" + cellEnd).html().split('/')[0];
                var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: nightValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + "H416" + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond("H416" + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }

            //夜间数据改变，下半夜的值改变
            if (cellId.substring(0, 4) == "P316" || cellId.substring(0, 4) == "P326" || cellId.substring(0, 4) == "H316") {
                var cellEnd = cellId[4];
                var earlyNinghtPrefix = "";
                var lateNightPrefix = "";
                if (cellId.substring(0, 4) == "P316") {
                    earlyNinghtPrefix = "P314";
                    lateNightPrefix = "P411";
                }
                if (cellId.substring(0, 4) == "P326") {
                    earlyNinghtPrefix = "P324";
                    lateNightPrefix = "P421";
                } if (cellId.substring(0, 4) == "H316") {
                    earlyNinghtPrefix = "H314";
                    lateNightPrefix = "H411";
                }
                var nightValue = $("#" + cellId).html().split('/')[0];
                var earlyNinghtDenValue = $("#" + earlyNinghtPrefix + cellEnd).html().split('/')[0];
                var lateNightDenValue = parseFloat((parseFloat(nightValue) * 10 - parseFloat(earlyNinghtDenValue) * 4) / 6).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: lateNightDenValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + lateNightPrefix + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond(lateNightPrefix + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }

            if (cellId.substring(0, 4) == "P416" || cellId.substring(0, 4) == "P426" || cellId.substring(0, 4) == "H416") {
                var cellEnd = cellId[4];
                var earlyNinghtPrefix = "";
                var lateNightPrefix = "";
                if (cellId.substring(0, 4) == "P416") {
                    earlyNinghtPrefix = "P414";
                    lateNightPrefix = "P511";
                }
                if (cellId.substring(0, 4) == "P426") {
                    earlyNinghtPrefix = "P424";
                    lateNightPrefix = "P521";
                } if (cellId.substring(0, 4) == "H416") {
                    earlyNinghtPrefix = "H414";
                    lateNightPrefix = "H511";
                }
                var nightValue = $("#" + cellId).html().split('/')[0];
                var earlyNinghtDenValue = $("#" + earlyNinghtPrefix + cellEnd).html().split('/')[0];
                var lateNightDenValue = parseFloat((parseFloat(nightValue) * 10 - parseFloat(earlyNinghtDenValue) * 4) / 6).toFixed(1);

                //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                    params: { value: lateNightDenValue, itemID: cellEnd },
                    success: function (response) {
                        if (response.responseText != "") {
                            $("#" + lateNightPrefix + cellEnd).html(response.responseText);
                            calculateAQIOfSingleLineSecond(lateNightPrefix + cellEnd);
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }
            calculateDayAverage_night(cellId);
            calculateDayAverage_earlyAdLate(cellId);
        }
    }

    //当上午、下午、上半夜、下半夜的数据改变时，日平均的联动
    function calculateDayAverage_earlyAdLate(cellId) {
        //重新计算日平均的数据
        if (cellId.substring(0, 4) == "P314" || cellId.substring(0, 4) == "P411" || cellId.substring(0, 4) == "P412" || cellId.substring(0, 4) == "P413") {
            var cellEnd = cellId[4];
            //上半夜
            var earlyNinghtDenValue = $("#" + "P314" + cellEnd).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P411" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P412" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P413" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P417" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P417" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "P324" || cellId.substring(0, 4) == "P421" || cellId.substring(0, 4) == "P422" || cellId.substring(0, 4) == "P423") {
            var cellEnd = cellId[4];
            //上半夜
            var earlyNinghtDenValue = $("#" + "P324" + cellEnd).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P421" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P422" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P423" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P427" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P427" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "H314" || cellId.substring(0, 4) == "H411" || cellId.substring(0, 4) == "H412" || cellId.substring(0, 4) == "H413") {
            var cellEnd = cellId[4];
            //上半夜
            var earlyNinghtDenValue = $("#" + "H314" + cellEnd).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "H411" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "H412" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "H413" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "H417" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("H417" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }


        if (cellId.substring(0, 4) == "P414" || cellId.substring(0, 4) == "P511" || cellId.substring(0, 4) == "P512" || cellId.substring(0, 4) == "P513") {
            var cellEnd = cellId[4];
            //上半夜
            var earlyNinghtDenValue = $("#" + "P414" + cellEnd).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P511" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P512" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P513" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P517" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P517" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "P424" || cellId.substring(0, 4) == "P521" || cellId.substring(0, 4) == "P522" || cellId.substring(0, 4) == "P523") {
            var cellEnd = cellId[4];
            //上半夜
            var earlyNinghtDenValue = $("#" + "P424" + cellEnd).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P521" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P522" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P523" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P527" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P527" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "H414" || cellId.substring(0, 4) == "H511" || cellId.substring(0, 4) == "H512" || cellId.substring(0, 4) == "H513") {
            var cellEnd = cellId[4];
            //上半夜
            var earlyNinghtDenValue = $("#" + "H414" + cellEnd).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "H511" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "H512" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "H513" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "H517" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("H517" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
    }

    //当夜间的数据改变时，日平均的联动
    function calculateDayAverage_night(cellId) {
        //重新计算日平均的数据
        if (cellId.substring(0, 4) == "P316") {
            var cellEnd = cellId[4];
            //夜间
            var nightDenValue = $("#" + cellId).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P411" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P412" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P413" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(nightDenValue) * 10 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P417" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P417" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "P326") {
            var cellEnd = cellId[4];
            //夜间
            var nightDenValue = $("#" + cellId).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P421" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P422" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P423" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(nightDenValue) * 10 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P427" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P427" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "H316") {
            var cellEnd = cellId[4];
            //夜间
            var nightDenValue = $("#" + cellId).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "H411" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "H412" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "H413" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(nightDenValue) * 10 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "H417" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("H417" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }


        if (cellId.substring(0, 4) == "P416") {
            var cellEnd = cellId[4];
            //夜间
            var nightDenValue = $("#" + cellId).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P511" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P512" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P513" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(nightDenValue) * 10 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P517" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P517" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "P426") {
            var cellEnd = cellId[4];
            //夜间
            var nightDenValue = $("#" + cellId).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "P521" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "P522" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "P523" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(nightDenValue) * 10 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);

            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "P527" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("P527" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }

        if (cellId.substring(0, 4) == "H416") {
            var cellEnd = cellId[4];
            //夜间
            var nightDenValue = $("#" + cellId).html().split('/')[0];
            //上半夜
            var lateNightDenValue = $("#" + "H511" + cellEnd).html().split('/')[0];
            //上午
            var morDenValue = $("#" + "H512" + cellEnd).html().split('/')[0];
            //下午
            var afterDenValue = $("#" + "H513" + cellEnd).html().split('/')[0];
            //日平均
            var averValue = ((parseFloat(nightDenValue) * 10 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'ConvertToAQI'),
                params: { value: averValue, itemID: cellEnd },
                success: function (response) {
                    if (response.responseText != "") {
                        $("#" + "H517" + cellEnd).html(response.responseText);
                        calculateAQIOfSingleLineSecond("H517" + cellEnd);
                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
    }
