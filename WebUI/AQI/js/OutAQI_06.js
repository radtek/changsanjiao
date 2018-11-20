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
    getPollutant();   //wb  2017.6.1
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    changeDate("");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'LoadSavedAQIPeriodData'),
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
                                if (divContaner.id == "P5276p" || divContaner.id == "H5176p" || divContaner.id == "P5176p") {     //wb  2017.6.1
                                    divContaner.value = results[obj];
                                }
                                else { divContaner.innerHTML = results[obj]; } //日平均值
                        }
                    }
                }


                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'LoadSavedAQIPeriodData2'),
                    success: function (response) {
                        if (response.responseText != "") {

                            {
                                var results = Ext.util.JSON.decode(response.responseText);
                                for (var obj in results) {
                                    var divContaner = Ext.getDom(obj);
                                    if (divContaner != null) {
                                        if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                                            divContaner.value = results[obj];
                                        else {
                                            if (divContaner != "" && divContaner != null)
                                                if (divContaner.id == "P5270p" || divContaner.id == "H5170p" || divContaner.id == "P5170p") {     //wb  2017.6.1
                                                    divContaner.value = results[obj];
                                                }
                                                else { divContaner.innerHTML = results[obj]; } //日平均值
                                        }
                                    }
                                }
                            }
                        }
                    },
                    failure: function (response) {
                    }
                });

            }
            else {
                getHistoryData();
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
    $("#forecastTimeLevel").html("06时");

    //读取当天的AQI分时段短信是否已发送
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'JudgeAQIPeriodMessage'),
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

    //2015年11月11日，预览按钮点击事件

    //ftp地址集合字符串
    var ftpString = $("#FtpCollection").val();
    if (!win) {//如果不存在win对象择创建
        win = new Ext.Window({
            title: 'AQI分时段预报产品预览',
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
                            title: '72小时AQI分时段预报',
                            html: '<textarea id="textContentArea" class="textPrev"></textarea>'//内部显示内容
                        },
                        {
                            id: "tabMsg",
                            title: 'AQI分时段预报短信',
                            html: '<textarea id="msgArea" class="textPrev_48"></textarea><div><div id="countLabel" class="countLabel">字数：</div><input id="charCount" class="charCount" type="text"/></div>'
                        }
                    ]
            }),
            buttons: [
                       {
                           text: '保存',
                           handler: function () {//点击时触发的事件
                               var cells = "";
                               var ps = "";
                               $.each($(".show"), function (i, n) {
                                   var cell = n.id + ":" + $(this).text() + ",";
                                   cells += cell;
                               });

//                               $.each($(".txt"), function (i, n) {
//                                   var cell = n.id + ":" + $(this).text() + ",";
//                                   cells += cell;
//                               });


                               // xuehui 07-06 
                               $.each($(".txt"), function (i, n) {
                                   var temp = n.id + ":" + $(this).val() + ",";
                                   if (n.id.toString().indexOf('p') >= 0 || n.id.toString().indexOf('v') >= 0) {
                                       ps += (n.id + ":" + $(this).val() + ",");
                                   } else {
                                       cells += temp;
                                   }
                               });
                               cells += ps;


                               Ext.Ajax.request({
                                   url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'SaveAQIPeriodDataAndText'),
                                   params: { forecastDate: getNowDate(), data: cells, functionName: 'AQIPeriod48', ftpString: ftpString, txtContent: '', msgContent: '', fileTxtName: 'AQI_SH_YYYYMMDDHHmm.txt', fileMsgName: 'msg.txt', userName: userName, textContent: $("#textContentArea").val(), msgContent: $("#msgArea").val(), dateType: generalDateType },
                                   success: function (response) {
                                       if (response.responseText == "success") {
                                           textContent = $("#textContentArea").val();
                                           msgContent = $("#msgArea").val()
                                           judgeMsgCharCount();
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
                           }
                       },
                        {
                            text: '发布',
                            handler: function () {//点击时触发的事件

                                var tabPanel = win.items.items[0];
                                var activeTab = tabPanel.getActiveTab();

                                var fileDate = getFormatDate('');
                                Ext.Ajax.timeout = 900000;
                                //UpLoadAQIPeriodTextAndMsg(配置文件测试方法)
                                var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
                                //发布的文本内容
                                var txtPubContent = textContent;
                                if (activeTab.id == "tabTxt") {
                                    if (textContent != "") {
                                        txtPubContent = textContent;
                                    }
                                }
                                //发布的短信内容
                                var txtMsgContent = "";
                                if (activeTab.id == "tabMsg") {
                                    if (msgContent != "") {
                                        txtMsgContent = msgContent;
                                        Ext.get("msgArea").update(txtMsgContent);
                                        judgeMsgCharCount();
                                    }
                                }
                                myMask.show();



                                Ext.Ajax.request({
                                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'UpLoadAQIPeriodTextAndMsgReal'),
                                    params: { ftpString: ftpString, fileDate: fileDate, functionName: 'AQIPeriod48_06', txtContent: txtPubContent, txtMsg: txtMsgContent, userName: userName, dateType: "Today" },
                                    success: function (response) {
                                        if (response.responseText != "") {
                                            if (response.responseText != "") {
                                                myMask.hide();
                                                if (response.responseText == "fail") {
                                                    alert("发布失败！");

                                                }
                                                else if (response.responseText == "Ftp") {
                                                    alert("文本发布成功！");
                                                }
                                                else if (response.responseText == "Message") {
                                                    alert("短信发布成功！");
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
            win.show();

            //发布的FTP文本
            var now = new Date();
            //var date = startDate.format("m月d日");
            var today = now.add('d', 0);
            today = today.format("d日");
            var tom = now.add('d', 1);
            tom = tom.format("d日");
            var third = now.add('d', 2);
            third = third.format("d日");
            var four = now.add('d', 3);
            four = four.format('d日');
            var txtPubContent = "";
            //以前的，用来计算第三日白天
            var SecVaules = GetAfterDaySecVaule();
            var SecVaules = GetAfterTotallyDayValue();
            //generalDateType = "Yesterday";
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'GetAQIPeriodTextContentNew'),
                params: { contentType: "text", dateType: generalDateType, values: SecVaules, type: 'AQIPeriod48_06' },
                success: function (response) {
                    if (response.responseText.indexOf('cal') < 0) {
                        txtPubContent = response.responseText;
                        $("#textContentArea").val(txtPubContent);
                        textContent = txtPubContent;
                    }
                    else {
                        var afterDayAQI = response.responseText.split(':')[1].split('&')[0];
                        var afterDayItem = response.responseText.split(':')[1].split('&')[1];
                        var afterDayRange = "";
                        var afterDayLevel = "";
                        if (afterDayAQI != "") {
                            afterDayRange = CalculateAQIRange(afterDayAQI);
                            afterDayLevel = CalculateAQLLevelRange(afterDayRange);
                        }
                        else {
                            afterDayRange = "--/--";
                            afterDayLevel = "无";
                        }
                        if (afterDayLevel == "优") {
                            afterDayItem = "-";
                        }


                        if (generalDateType == "Today") {
                            txtPubContent = GetAQIPeriodTextContent();
                        }
                        else if (generalDateType == "Yesterday" || generalDateType == "Before") {
                            txtPubContent = GetAQIPeriodTextContentYesterday(generalDateType);
                        }
                        textContent = txtPubContent;

                        if (generalDateType == "Yesterday" || generalDateType == "Before") { // xuehui 06-08

                            today = now.add('d', -1);
                            today = today.format("d日");
                            tom = now.add('d', 0);
                            tom = tom.format("d日");
                            third = now.add('d', 1);
                            third = third.format("d日");
                            four = now.add('d', 2);
                            four = four.format('d日');
                            textContent = textContent.replace("今天夜间", today + "夜间");
                            textContent = textContent.replace("明天上午", tom + "上午");
                            textContent = textContent.replace("明天下午", tom + "下午");
                            textContent = textContent.replace("明天夜间", tom + "夜间");
                            textContent = textContent.replace("后天  ", third);
                            textContent = textContent.replace("第三日", four);
                        } else {
                            textContent = textContent.replace("今天夜间", today + "夜间");
                            textContent = textContent.replace("明天上午", tom + "上午");
                            textContent = textContent.replace("明天下午", tom + "下午");
                            textContent = textContent.replace("明天夜间", tom + "夜间");
                            textContent = textContent.replace("后天  ", third);
                            textContent = textContent.replace("第三日", four);
                        }
                        $("#textContentArea").val(textContent);

                    }
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });

            //发布的短信文本内容
            var txtMsgContent = "";
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'GetAQIPeriodTextContentNew'),
                params: { contentType: "msg", dateType: generalDateType, values: SecVaules, type: 'AQIPeriod48_06' },
                success: function (response) {
                    if (response.responseText.indexOf('cal') < 0) {

                        if (generalDateType == "Yesterday" || generalDateType == "Before") {
                            txtMsgContent = GetAQIPeriodContentOneLine("msg");
                            $("#msgArea").val(txtMsgContent);
                        } else {

                            txtMsgContent = response.responseText;
                            $("#msgArea").val(txtMsgContent);
                        }
                    }
                    else {

                        if (generalDateType == "Today") {
                            var curHour = new Date().getHours(); //薛辉
                            if (curHour < 20) {
                                txtMsgContent = GetAQIPeriodContent("msg");
                            } else {
                                txtMsgContent = GetAQIPeriodContentTwoLines("msg");
                            }
                        }
                        else if (generalDateType == "Yesterday" || generalDateType == "Before") {
                            var curHour = new Date().getHours();
                            if (curHour <= 12) {
                                txtMsgContent = GetAQIPeriodContentTwoLines("msg"); // xuehui 06-08
                            }
                            else if (curHour > 12) {
                                txtMsgContent = GetAQIPeriodContentOneLine("msg"); // xuehui 06-08
                            }
                        }
                        var afterDayAQI = response.responseText.split(':')[1].split('&')[0];
                        var afterDayItem = response.responseText.split(':')[1].split('&')[1];
                        var afterDayRange = "";
                        var afterDayLevel = "";
                        if (afterDayAQI != "") {
                            afterDayRange = CalculateAQIRange(afterDayAQI);
                            afterDayLevel = CalculateAQLLevelRangeForMsg(afterDayRange);
                        }
                        else {
                            afterDayRange = "--/--";
                            afterDayLevel = "无";
                        }
                        //txtMsgContent = txtMsgContent.replace("{RangeAfterDay}", afterDayRange);
                        //if (afterDayLevel == "优") {
                        //    txtMsgContent = txtMsgContent.replace("{LevelAfterDay}", afterDayLevel);
                        //}
                        //else {
                        //    txtMsgContent = txtMsgContent.replace("{LevelAfterDay}", afterDayLevel + "(" + afterDayItem + ")");
                        //}
                        msgContent = txtMsgContent;
                        $("#msgArea").val(txtMsgContent);
                    }
                    judgeMsgCharCount();
                },
                failure: function (response) {
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });

            $("#msgArea").keyup(function () {
                if ($("#msgArea").val().length <= 190) {
                    $("#charCount").val($("#msgArea").val().length + "/190");
                    $("#msgArea").css("background-color", "white")
                }
                else {
                    $("#charCount").val("已超出" + ($("#msgArea").val().length - 190) + "个字");
                    $("#msgArea").css("background-color", "lightpink")
                }
            });
        }
    });

    $("#foreSave").click(function () {
        var cells = "";
        var ps = "";

        $.each($(".show"), function (i, n) {
            var cell = n.id + ":" + $(this).text() + ",";
            cells += cell;

        });

        $.each($(".txt"), function (i, n) {
            var temp = n.id + ":" + $(this).val() + ",";
            if (n.id.toString().indexOf('p') >= 0 || n.id.toString().indexOf('v') >= 0) {
                ps += (n.id + ":" + $(this).val() + ",");
            } else {
                cells += temp;
            }
        });
        cells += ps;

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'DeleteAQIPeriodHistoryData'),
            params: { dateType: generalDateType },
            success: function (response) {
                if (response.responseText != "") {
                    if (response.responseText == "success") {

                        Ext.Ajax.request({
                            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'SaveAQIPeriodData'),
                            params: { forecastDate: getNowDate(), data: cells, functionName: 'AQIPeriod48', ftpString: ftpString, txtContent: '', msgContent: '', fileTxtName: 'AQI_SH_YYYYMMDDHHmm.txt', fileMsgName: 'msg.txt', userName: userName },
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
                        myMask.hide();
                        alert("保存失败！");
                    }
                }
            },
            failure: function (response) {
                //                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });

    });

    //审核按键
    $("#foreCheck").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'SetChecked'),
            params: { functionName: "AQIPeriod48", hourType: "17:00" },
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
    var startDate = new Date();
    startDate.setDate(startDate.getDate() - 1);
    //主观数据导入
    $("#subjImport").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'LoadSavedAQIPeriodDataII'),
            success: function (response) {
                myMask.hide();
                //response.responseText="";
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



                    Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'LoadSavedAQIPeriodData2II'),
                    success: function (response) {
                        if (response.responseText != "") {

                            {
                                var results = Ext.util.JSON.decode(response.responseText);
                                for (var obj in results) {
                                    var divContaner = Ext.getDom(obj);
                                    if (divContaner != null) {
                                        if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                                            divContaner.value = results[obj];
                                        else {
                                            if (divContaner != "" && divContaner != null)
                                                if (divContaner.id == "P5270p" || divContaner.id == "H5170p" || divContaner.id == "P5170p") {     //wb  2017.6.1
                                                    divContaner.value = results[obj];
                                                }
                                                else { divContaner.innerHTML = results[obj]; } //日平均值
                                        }
                                    }
                                }
                            }
                        }
                    },
                    failure: function (response) {
                    }
                });

                }
                else {
                    var nowDate = getNowDate();
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ReadFromDataInterface'),
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
                                var sDay = startDate.add('d', 3);    //wb  2017.6.2
                                var sanDay = sDay.format("m月d日");

                                var ePreview = Ext.getDom("Ptd11");
                                ePreview.innerText = date;
                                for (var i = 2; i <= 3; i++) {
                                    ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                                    ePreview.innerText = tomoDate;
                                }
                                ePreview = Ext.getDom("Ptd41");
                                ePreview.innerText = threeDate;
                                ePreview = Ext.getDom("Ptd51");
                                ePreview.innerText = threeDate;
                                ePreview = Ext.getDom("Ptd61");
                                ePreview.innerText = tomoDate;
                                ePreview = Ext.getDom("Ptd71");
                                ePreview.innerText = threeDate;

                                ePreview = Ext.getDom("Ptd81");
                                ePreview.innerText = sanDay;



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
                                                if (divContaner.id == "P5270v" || divContaner.id == "H5170v" || divContaner.id == "P5170v"
                                                || divContaner.id == "P5270p" || divContaner.id == "H5170p" || divContaner.id == "P5170p") {     //wb  2017.6.1
                                                    divContaner.value = results[obj];
                                                }
                                                else { divContaner.innerHTML = results[obj]; } //日平均值
                                        }
                                    }
                                }
                                calculatrNO2(results);
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

    //主观数据重新导入
    $("#refreshData").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'DeleteSavedAQIPeriodData'),
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    var nowDate = getNowDate();
                    //        waitInfo = Ext.MessageBox.wait('请等待', '读取数据中');
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ReadFromDataInterface'),
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
                                var sDay = startDate.add('d', 3);    //wb  2017.6.2
                                var sanDay = sDay.format("m月d日");

                                var ePreview = Ext.getDom("Ptd11");
                                ePreview.innerText = date;
                                for (var i = 2; i <= 3; i++) {
                                    ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                                    ePreview.innerText = tomoDate;
                                }
                                ePreview = Ext.getDom("Ptd41");
                                ePreview.innerText = threeDate;
                                ePreview = Ext.getDom("Ptd51");
                                ePreview.innerText = threeDate;
                                ePreview = Ext.getDom("Ptd61");
                                ePreview.innerText = tomoDate;
                                ePreview = Ext.getDom("Ptd71");
                                ePreview.innerText = threeDate;

                                ePreview = Ext.getDom("Ptd81");
                                ePreview.innerText = sanDay;


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
                            }

                        },
                        failure: function (response) {
                            myMask.hide();
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }

                    });

                }
                else {
                    myMask.hide();

                }
            },
            failure: function (response) {
                myMask.hide();
            }
        });
    });

    //获取昨天数据
    $("#historyData").click(function () {
        getHistoryData();
    });

    EnableII();

});
function getHistoryData() {

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ReadFromDataInterfaceHistory'),
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
                var sDay = startDate.add('d', 3);    //wb  2017.6.2
                var sanDay = sDay.format("m月d日");

                var ePreview = Ext.getDom("Ptd11");
                ePreview.innerText = date;
                for (var i = 2; i <= 3; i++) {
                    ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                    ePreview.innerText = tomoDate;
                }
                ePreview = Ext.getDom("Ptd41");
                ePreview.innerText = threeDate;
                ePreview = Ext.getDom("Ptd51");
                ePreview.innerText = threeDate;
                ePreview = Ext.getDom("Ptd61");
                ePreview.innerText = tomoDate;
                ePreview = Ext.getDom("Ptd71");
                ePreview.innerText = threeDate;
                ePreview = Ext.getDom("Ptd81");
                ePreview.innerText = sanDay;

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
                                if (divContaner.id == "P5270p" || divContaner.id == "H5170p" || divContaner.id == "P5170p") {     //xh  2017.6.6
                                    divContaner.value = results[obj];
                                }
                                else { divContaner.innerHTML = results[obj]; } //日平均值
                        }
                    }
                }
                calculatrNO2(results);
                //薛辉
                EnableTXT();
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

}
function EnableTXT() {
    //薛辉
    $.each($(".show"), function (i, n) {
        if (n.id.substring(0, 1) == "H") {

            if (n.id.substring(0, 4) != "H417" &&
               n.id.substring(0, 4) != "H517") {
                $("#" + n.id).removeClass("show");
                $("#" + n.id).removeClass("divInputType show");
                $("#" + n.id).addClass("divInputType show");
                $("#" + n.id).removeAttr("onclick");
                $("#" + n.id).attr("onclick", "showInput(event,this)");
            }

        } else if (n.id.substring(0, 1) == "P") {
            $("#" + n.id).removeAttr("onclick");
            $("#" + n.id).removeClass("show");
            $("#" + n.id).removeClass("divInputType show");
            $("#" + n.id).addClass("show");
        }
    });
}

function EnableII() {
    //薛辉
    $.each($(".show"), function (i, n) {
        if (n.id.substring(0, 1) == "H") {

            if (n.id.substring(0, 4) != "H417" &&
               n.id.substring(0, 4) != "H517") {
                $("#" + n.id).removeClass("show");
                $("#" + n.id).removeClass("divInputType show");
                $("#" + n.id).addClass("divInputType show");
                $("#" + n.id).removeAttr("onclick");
                $("#" + n.id).attr("onclick", "showInput(event,this)");
            }

        }
    });
}

var waits = 30;

function closeCheck(divName) {
    var divUsers = Ext.getDom(divName);
    divUsers.style.display = "none";
}
function changeDate(el) {
    var forecastDate = "2015年11月02日";

    var startDate = new Date();
    var date = startDate.format("m月d日");
    var nextDate = startDate.add('d', 2);
    var threeDate = nextDate.format("m月d日");
    var tomorrowDay = startDate.add('d', 1);
    var tomoDate = tomorrowDay.format("m月d日");
    var sDay = startDate.add('d', 3);      //wb  2017.6.2
    var sanDay = sDay.format("m月d日");

    var ePreview = Ext.getDom("Ptd11");
    ePreview.innerText = date;
    for (var i = 2; i <= 3; i++) {
        ePreview = Ext.getDom(String.format("Ptd{0}1", i));
        ePreview.innerText = tomoDate;
    }
    ePreview = Ext.getDom("Ptd41");
    ePreview.innerText = threeDate;
    ePreview = Ext.getDom("Ptd51");
    ePreview.innerText = threeDate;
    ePreview = Ext.getDom("Ptd61");
    ePreview.innerText = tomoDate;
    ePreview = Ext.getDom("Ptd71");
    ePreview.innerText = threeDate;
    ePreview = Ext.getDom("Ptd81");
    ePreview.innerText = sanDay;

    //模型数据导入
    $("#modelImport").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'BuildPreconsation'),
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
                    var sDay = startDate.add('d', 3);    //wb  2017.6.2
                    var sanDay = sDay.format("m月d日");

                    var ePreview = Ext.getDom("Ptd11");
                    ePreview.innerText = date;
                    for (var i = 2; i <= 3; i++) {
                        ePreview = Ext.getDom(String.format("Ptd{0}1", i));
                        ePreview.innerText = tomoDate;
                    }
                    ePreview = Ext.getDom("Ptd41");
                    ePreview.innerText = threeDate;
                    ePreview = Ext.getDom("Ptd51");
                    ePreview.innerText = threeDate;
                    ePreview = Ext.getDom("Ptd61");
                    ePreview.innerText = threeDate;
                    ePreview = Ext.getDom("Ptd81");
                    ePreview.innerText = sanDay;

                    var result = Ext.util.JSON.decode(response.responseText);
                    //calculateAQI(result);
                    // calculatrNO2(result);
                    calculateAQI(result);
                    changeDateSucessed(result);

                    var nowDateTime = Ext.getDom("nowDateTime");
                    myMask.hide();
                    //buildPreview("H3141");
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
                                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ChangeAQI'),
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
                                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
                                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ChangeAQI'),
                                    params: { value: value, itemID: itemID, firstItem: firstItemValue },
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
                                    url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
                        $("#" + parentNode.id).html(parentNode.getAttribute("AQI"));
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

        //第二天或第一天下半夜 上半夜 不是03-1h和03-8h
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

            //            if (caculateValue == "NaN")
            //                return;

            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
                params: { value: caculateValue, itemID: curID.substr(4, 1) },
                success: function (response) {
                    Ext.getDom(iD).innerHTML = response.responseText;
                    var content = getContent();
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'BuildPreview'),
                        params: { forecastDate: Ext.getDom("Ptd11").value, postJson: content, divID: divID, itemID: itemID, detail: "", Module: Module },
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
                url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'BuildPreview'),
                params: { forecastDate: Ext.getDom("Ptd11").value, postJson: content, divID: divID, itemID: itemID, detail: "", Module: Module },
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
        if (aryDiv[i].className.indexOf('divInputType') >= 0 && aryDiv[i].id.substr(4, 1) != 6) {
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
    var txtContent = $("#txtHidePeriodAQITxtTemplete").val();
    //文本类型
    if (type == "msg") {
        txtContent = $("#txtHidePeriodAQIMsgTemplete").val();
    }
    else {
        txtContent = $("#txtHidePeriodAQITxtTemplete").val();
    }


    txtContent = txtContent.replace("{PublishDate}", getFormatDate(generalDateType));
    txtContent = txtContent.replace("{Hour}", "06时");
    if (type == "msg") {
        var curDate = new Date();
        txtContent = txtContent.replace("{TodayDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{TomorrowDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{AfterDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{third}", curDate.getDate() + "日");
    }
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
        toLevel = CalculateAQLLevelRangeForMsg(toRange);
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
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelNight}", toLevel + "(" + tonightItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelNight}", toLevel + "(" + "首要污染物" + tonightItem + ")");
        }
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
        tomMorLevel = CalculateAQLLevelRangeForMsg(tomMorRange);
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
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + tmMorItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + "首要污染物" + tmMorItem + ")");
        }
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
        tomAftLevel = CalculateAQLLevelRangeForMsg(tomAftRange);
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
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + tmAftItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
        }
    }

    //明天夜间 H4166
    var tomNightCellValue = $("#H4166").text();
    var tomNightAQI = "";
    var tomNightItem = "";
    if (tomNightCellValue != "") {
        tomNightAQI = tomNightCellValue.split('/')[0];
        tomNightItem = tomNightCellValue.split('/')[1];
    }
    var tomNightRange = "";
    var tomNightLevel
    if (tomNightAQI != "") {
        tomNightRange = CalculateAQIRange(tomNightAQI);
        tomNightLevel = CalculateAQLLevelRangeForMsg(tomNightRange);
    }
    else {
        tomNightRange = "--/--";
        tomNightLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomNight}", tomNightRange);
    if (tomNightLevel == "优") {
        txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel + "(" + tomNightItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel + "(" + "首要污染物" + tomNightItem + ")");
        }
    }
    //后天 H5176
    var secondValue = $("#H5176").text();
    var secondAQI = "";
    var secondItem = "";
    if (secondValue != "") {
        secondAQI = secondValue.split('/')[0];
        secondItem = secondValue.split('/')[1];
    }
    var secondRange = "";
    var secondLevel
    if (secondAQI != "") {
        secondRange = CalculateAQIRange(secondAQI);
        secondLevel = CalculateAQLLevelRangeForMsg(secondRange);
    }
    else {
        secondRange = "--/--";
        secondLevel = "无";
    }

    txtContent = txtContent.replace("{RangeAfterDay}", secondRange);
    if (secondLevel == "优") {
        txtContent = txtContent.replace("{LevelAfterDay}", secondLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelAfterDay}", secondLevel + "(" + secondItem + ")");
        }
        else {
            txtContent = txtContent.replace("{RangeAfterDay}", secondLevel + "(" + "首要污染物" + secondItem + ")");
        }
    }

    //第三日
    //var thirdValue = $("#H5170v").val();
    var thirdAQI = $("#H5170v").val();
    var thirdItem = $("#H5170p").val();
    //if (secondValue != "") {
    //    secondAQI = tomMorCellValue.split('/')[0];
    //    secondItem = tomMorCellValue.split('/')[1];
    //}
    var thirdRange = "";
    var thirdLevel = "";
    if (thirdAQI != "") {
        thirdRange = CalculateAQIRange(thirdAQI);
        thirdLevel = CalculateAQLLevelRangeForMsg(thirdRange);
    }
    else {
        thirdRange = "--/--";
        thirdLevel = "无";
    }

    txtContent = txtContent.replace("{RangethirdDay}", thirdRange);
    if (thirdLevel == "优") {
        txtContent = txtContent.replace("{LevelthirdDay}", thirdLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelthirdDay}", thirdLevel + "(" + thirdItem + ")");
        }
        else {
            txtContent = txtContent.replace("{RangethirdDay}", thirdLevel + "(" + "首要污染物" + thirdItem + ")");
        }
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
    var curHour = new Date().getHours();
    txtContent = txtContent.replace("{Hour}", curHour + "时");
    txtContent = txtContent.replace("{PublishDate}", getFormatDate(""));
    if (type == "msg") {
        var curDate = new Date();
        txtContent = txtContent.replace("{TodayDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{TomorrowDay}", curDate.getDate() + "日");
        //        curDate.setDate(curDate.getDate() + 1);
        //        txtContent = txtContent.replace("{AfterDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{secondDay}", curDate.getDate() + "日");
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
    if (tomMorAQI != "") {
        tomMorRange = CalculateAQIRange(tomMorAQI);
        tomMorLevel = CalculateAQLLevelRangeForMsg(tomMorRange);
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
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + tmMorItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomMorning}", tomMorLevel + "(" + "首要污染物" + tmMorItem + ")");
        }
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
        tomAftLevel = CalculateAQLLevelRangeForMsg(tomAftRange);
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
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + tmAftItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
        }
    }

    //明天夜间 H4166
    var tomNightCellValue = $("#H4166").text();
    var tomNightAQI = "";
    var tomNightItem = "";
    if (tomNightCellValue != "") {
        tomNightAQI = tomNightCellValue.split('/')[0];
        tomNightItem = tomNightCellValue.split('/')[1];
    }
    var tomNightRange = "";
    var tomNightLevel
    if (tomNightAQI != "") {
        tomNightRange = CalculateAQIRange(tomNightAQI);
        tomNightLevel = CalculateAQLLevelRangeForMsg(tomNightRange);
    }
    else {
        tomNightRange = "--/--";
        tomNightLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomNight}", tomNightRange);
    if (tomNightLevel == "优") {
        txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel + "(" + tomNightItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel + "(" + "首要污染物" + tomNightItem + ")");
        }
    }

    //后天全天   H5176
    //var aqiAndItem = calculateAfterDayAQI();
    var aqiAndItem = $("#H5176").text();
    var afterDayAQI = aqiAndItem.split('/')[0];
    var afterDayItem = aqiAndItem.split('/')[1];
    var afterDayRange = "";
    var afterDayLevel = "";
    if (afterDayAQI != "") {
        afterDayRange = CalculateAQIRange(afterDayAQI);
        afterDayLevel = CalculateAQLLevelRangeForMsg(afterDayRange);

    }
    else {
        afterDayRange = "--/--";
        afterDayLevel = "无";
    }
    txtContent = txtContent.replace("{RangeAfterDay}", afterDayRange);
    if (afterDayLevel == "优") {
        txtContent = txtContent.replace("{LevelAfterDay}", afterDayLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelAfterDay}", afterDayLevel + "(" + afterDayItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelAfterDay}", afterDayLevel + "(" + "首要污染物" + afterDayItem + ")");
        }
    }

    //第三日全天  H5170v
    //var aqiAndItem = calculateAfterDayAQI();
    var aqiAndItem = $("#H5170v").text();
    var afterDayAQI = $("#H5170v").val();
    var afterDayItem = $("#H5170p").val();
    var afterDayRange = "";
    var afterDayLevel = "";
    if (afterDayAQI != "") {
        afterDayRange = CalculateAQIRange(afterDayAQI);
        afterDayLevel = CalculateAQLLevelRangeForMsg(afterDayRange);

    }
    else {
        afterDayRange = "--/--";
        afterDayLevel = "无";
    }
    txtContent = txtContent.replace("{RangesecondDay}", afterDayRange);
    if (afterDayLevel == "优") {
        txtContent = txtContent.replace("{LevelsecondDay}", afterDayLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelsecondDay}", afterDayLevel + "(" + afterDayItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelsecondDay}", afterDayLevel + "(" + "首要污染物" + afterDayItem + ")");
        }
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
    var curHour = new Date().getHours();
    txtContent = txtContent.replace("{Hour}", curHour + "时");
    txtContent = txtContent.replace("{PublishDate}", getFormatDate(""));
    if (type == "msg") {
        var curDate = new Date();
        curDate.setDate(curDate.getDate());
        txtContent = txtContent.replace("{TodayDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{TomorrowDay}", curDate.getDate() + "日");
        //curDate.setDate(curDate.getDate() + 1);
        //txtContent = txtContent.replace("{AfterDay}", curDate.getDate() + "日");
        curDate.setDate(curDate.getDate() + 1);
        txtContent = txtContent.replace("{secondDay}", curDate.getDate() + "日");
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
        tomAftLevel = CalculateAQLLevelRangeForMsg(tomAftRange);
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
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + tmAftItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
        }
        //txtContent = txtContent.replace("{LevelTomAfternoon}", tomAftLevel + "(" + "首要污染物" + tmAftItem + ")");
    }

    //明天夜间 H4166
    var tomNightCellValue = $("#H4166").text();
    var tomNightAQI = "";
    var tomNightItem = "";
    if (tomNightCellValue != "") {
        tomNightAQI = tomNightCellValue.split('/')[0];
        tomNightItem = tomNightCellValue.split('/')[1];
    }
    var tomNightRange = "";
    var tomNightLevel
    if (tomNightAQI != "") {
        tomNightRange = CalculateAQIRange(tomNightAQI);
        tomNightLevel = CalculateAQLLevelRangeForMsg(tomNightRange);
    }
    else {
        tomNightRange = "--/--";
        tomNightLevel = "无";
    }

    txtContent = txtContent.replace("{RangeTomNight}", tomNightRange);
    if (tomNightLevel == "优") {
        txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel + "(" + tomNightItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel + "(" + "首要污染物" + tomNightItem + ")");
        }
    }

    //后天全天   H5176
    //var aqiAndItem = calculateAfterDayAQI();
    var aqiAndItem = $("#H5176").text();
    var afterDayAQI = aqiAndItem.split('/')[0];
    var afterDayItem = aqiAndItem.split('/')[1];
    var afterDayRange = "";
    var afterDayLevel = "";
    if (afterDayAQI != "") {
        afterDayRange = CalculateAQIRange(afterDayAQI);
        afterDayLevel = CalculateAQLLevelRangeForMsg(afterDayRange);

    }
    else {
        afterDayRange = "--/--";
        afterDayLevel = "无";
    }
    txtContent = txtContent.replace("{RangeAfterDay}", afterDayRange);
    if (afterDayLevel == "优") {
        txtContent = txtContent.replace("{LevelAfterDay}", afterDayLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelAfterDay}", afterDayLevel + "(" + afterDayItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelAfterDay}", afterDayLevel + "(" + "首要污染物" + afterDayItem + ")");
        }
    }

    //第三日全天  H5170v
    //var aqiAndItem = calculateAfterDayAQI();
    var aqiAndItem = $("#H5170v").text();
    var afterDayAQI = $("#H5170v").val();
    var afterDayItem = $("#H5170p").val();
    var afterDayRange = "";
    var afterDayLevel = "";
    if (afterDayAQI != "") {
        afterDayRange = CalculateAQIRange(afterDayAQI);
        afterDayLevel = CalculateAQLLevelRangeForMsg(afterDayRange);

    }
    else {
        afterDayRange = "--/--";
        afterDayLevel = "无";
    }
    txtContent = txtContent.replace("{RangesecondDay}", afterDayRange);
    if (afterDayLevel == "优") {
        txtContent = txtContent.replace("{LevelsecondDay}", afterDayLevel);
    }
    else {
        if (type == "msg") {
            txtContent = txtContent.replace("{LevelsecondDay}", afterDayLevel + "(" + afterDayItem + ")");
        }
        else {
            txtContent = txtContent.replace("{LevelsecondDay}", afterDayLevel + "(" + "首要污染物" + afterDayItem + ")");
        }
    }
    return txtContent;
}

function GetAQIPeriodTextContent() {
    var txtContent = $("#txtHidePeriodAQITxtTemplete").val();
    //文本类型   

    txtContent = txtContent.replace("{PublishDate}", getFormatDateForAQIPeriod(generalDateType));
    txtContent = txtContent.replace("{Hour}", "06时");
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
        toLevel = CalculateAQLLevelRange(toRange);
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
        tomMorLevel = CalculateAQLLevelRange(tomMorRange);
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
        tomAftLevel = CalculateAQLLevelRange(tomAftRange);
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

    //明天夜间 H4166
    var tomNightCellValue = $("#H4166").text();
    var tomNightAQI = "";
    var tomNightItem = "";
    if (tomNightCellValue != "") {
        tomNightAQI = tomNightCellValue.split('/')[0];
        tomNightItem = tomNightCellValue.split('/')[1];
    }
    var tomNightRange = "";
    var tomNightLevel
    if (tomNightAQI != "") {
        tomNightRange = CalculateAQIRange(tomNightAQI);
        tomNightLevel = CalculateAQLLevelRange(tomNightRange);
    }
    else {
        tomNightRange = "--/--";
        tomNightLevel = "无";
    }
    if (tomNightLevel == "优") {
        tomNightItem = "-";
    }

    txtContent = txtContent.replace("{RangeTomNight}", tomNightRange);
    txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel);
    txtContent = txtContent.replace("{AQIItemTomNight}", tomNightItem);

    //第二日
    var tomNightCellValueII = $("#H5176").text();
    var tomNightAQIII = "";
    var tomNightItemII = "";
    if (tomNightCellValueII != "") {
        tomNightAQIII = tomNightCellValueII.split('/')[0];
        tomNightItemII = tomNightCellValueII.split('/')[1];
    }
    var tomNightRangeII = "";
    var tomNightLevelII
    if (tomNightAQIII != "") {
        tomNightRangeII = CalculateAQIRange(tomNightAQIII);
        tomNightLevelII = CalculateAQLLevelRange(tomNightRangeII);
    }
    else {
        tomNightRangeII = "--/--";
        tomNightLevelII = "无";
    }
    if (tomNightLevelII == "优") {
        tomNightItemII = "-";
    }

    txtContent = txtContent.replace("{RangeAfterDay}", tomNightRangeII);
    txtContent = txtContent.replace("{LevelAfterDay}", tomNightLevelII);
    txtContent = txtContent.replace("{AQIItemAfterDay}", tomNightItemII);

    //第三日
    var thirdValue = $("#H5170v").val();
    var thirdAQI = $("#H5170p").val();
    //if (tomNightCellValue != "") {
    //    tomNightAQI = tomNightCellValue.split('/')[0];
    //    tomNightItem = tomNightCellValue.split('/')[1];
    //}
    var thirdRange = "";
    var thirdLevel
    if (thirdValue != "") {
        thirdRange = CalculateAQIRange(thirdValue);
        thirdLevel = CalculateAQLLevelRange(thirdRange);
    }
    else {
        thirdValue = "--/--";
        thirdLevel = "无";
    }
    if (thirdLevel == "优") {
        thirdAQI = "-";
    }

    txtContent = txtContent.replace("{RangeThirdDay}", thirdRange);
    txtContent = txtContent.replace("{LevelThirdDay}", thirdLevel);
    txtContent = txtContent.replace("{AQIItemThirdDay}", thirdAQI);
    return txtContent;
}

//点击获取 历史数据之后，文本内容读取为昨天的数据，日期也全部是昨天的
function GetAQIPeriodTextContentYesterday(dateType) {
    var txtContent = $("#txtHidePeriodAQITxtTemplete").val();
    //文本类型
    if (dateType == "" || dateType == null) {
        dateType = "Yesterday";
    }
    txtContent = txtContent.replace("{PublishDate}", getFormatDateForAQIPeriod("Today"));
    txtContent = txtContent.replace("{Hour}", "06时");
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
        toLevel = CalculateAQLLevelRange(toRange);
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
        tomMorLevel = CalculateAQLLevelRange(tomMorRange);
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
        tomAftLevel = CalculateAQLLevelRange(tomAftRange);
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

    //明天夜间 H4166
    var tomNightCellValue = $("#H4166").text();
    var tomNightAQI = "";
    var tomNightItem = "";
    if (tomNightCellValue != "") {
        tomNightAQI = tomNightCellValue.split('/')[0];
        tomNightItem = tomNightCellValue.split('/')[1];
    }
    var tomNightRange = "";
    var tomNightLevel
    if (tomNightAQI != "") {
        tomNightRange = CalculateAQIRange(tomNightAQI);
        tomNightLevel = CalculateAQLLevelRange(tomNightRange);
    }
    else {
        tomNightRange = "--/--";
        tomNightLevel = "无";
    }
    if (tomNightLevel == "优") {
        tomNightItem = "-";
    }
    txtContent = txtContent.replace("{RangeTomNight}", tomNightRange);
    txtContent = txtContent.replace("{LevelTomNight}", tomNightLevel);
    txtContent = txtContent.replace("{AQIItemTomNight}", tomNightItem);
    //第三日    H5176
    var thirdCellValue = $("#H5176").text();
    var thirdAQI = "";
    var thirdItem = "";
    if (thirdCellValue != "") {
        thirdAQI = thirdCellValue.split('/')[0];
        thirdItem = thirdCellValue.split('/')[1];
    }
    var thirdRange = "";
    var thirdLevel
    if (thirdAQI != "") {
        thirdRange = CalculateAQIRange(thirdAQI);
        thirdLevel = CalculateAQLLevelRange(thirdRange);
    }
    else {
        thirdRange = "--/--";
        thirdLevel = "无";
    }
    if (thirdLevel == "优") {
        thirdItem = "-";
    }
    txtContent = txtContent.replace("{RangeAfterDay}", thirdRange);
    txtContent = txtContent.replace("{LevelAfterDay}", thirdLevel);
    txtContent = txtContent.replace("{AQIItemAfterDay}", thirdItem);

    //第四日    H5170v   H5170p
    var fourAQI = $("#H5170v").val();
    var fourItem = $("#H5170p").val();
    //var fourAQI = "";
    //var fourItem = "";
    //if (thirdCellValue != "") {
    //    thirdAQI = thirdCellValue.split('/')[0];
    //    thirdItem = thirdCellValue.split('/')[1];
    //}
    var fourRange = "";
    var fourLevel
    if (fourAQI != "") {
        fourRange = CalculateAQIRange(fourAQI);
        fourLevel = CalculateAQLLevelRange(fourRange);
    }
    else {
        fourRange = "--/--";
        fourLevel = "无";
    }
    if (fourLevel == "优") {
        fourItem = "-";
    }
    txtContent = txtContent.replace("{RangeThirdDay}", fourRange);
    txtContent = txtContent.replace("{LevelThirdDay}", fourLevel);
    txtContent = txtContent.replace("{AQIItemThirdDay}", fourItem);
    return txtContent;
}




//获取AQI分时段的短信内容
function GetAQIPeriodMsg() {
    var msgContent = textContent = $("#txtHidePeriodAQIMsgTemplete").text();
    Ext.get("msgArea").update(msgContent);
    $("#charCount").val($("#msgArea").val().length + "/190");
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
    var idPrefixArray = ['P314', 'P411', 'P316', 'P412', 'P413', 'P414', 'P511', 'P416', 'P512', 'P513', 'P514', 'P417', 'P517', 'P324', 'P421', 'P326', 'P422', 'P423', 'P424', 'P521', 'P426', 'P522', 'P523', 'P524', 'P427', 'P527', 'H314', 'H411', 'H316', 'H412', 'H413', 'H414', 'H511', 'H416', 'H512', 'H513', 'H417', 'H517', 'H514'];
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
                                var firsItemAndAQI = calculateSecondItem(idPrefixArray[i], result);
                                itemType = firsItemAndAQI.split('&')[0];
                                aqiValue = firsItemAndAQI.split('&')[1];
                                break;
                        }
                    }
                }
            }
        }

        //xuehui 0605
        var ss = idPrefixArray[i] + '6';
        if (ss == "P5176" || ss == "P5276" || ss == "H5176" ||
          ss == "P4176" || ss == "P4276" || ss == "H4176"
          || ss == "P4226" || ss == "P5226" || ss == "H5126" || ss == "P4126" || ss == "H4126"
          || ss == "P5126" || ss == "P4146" || ss == "H4146" || ss == "P5146" || ss == "H5146") {
            continue;
        }

        result[idPrefixArray[i] + '6'] = aqiValue + "/" + "<span>" + itemType + "</span>";
    }
}

//计算一行内首要污染物，根据一行所有单元格所有共同的前缀
function calculateFirstItemSingleLine(result, idPrefix) {
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
        var itemType = "";
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
            //更靠近0
            if (IsNearZero(aqiValue)) {
                useValue = 10 * (Math.round(aqiValue / 10));
                lowValue = useValue - 10;
                if (lowValue == 50 || lowValue == 100 || lowValue == 150 || lowValue == 200 || lowValue == 250 || lowValue == 300) {
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

    return aqiRange;
}

//判断一个数字的个位数，是否更靠近0，是的话返回TRUE
//作者：张伟锋    日期：2016-05-29
function IsNearZero(number) {
    var unitNumber = number % 10;
    var nearZero = !(unitNumber >= 3 && unitNumber <= 7);
    return nearZero;
}

//代码优化
//作者：张伟锋   日期：2016-05-29
function CalculateAQLLevelRange(aqiString) {

    var aqiRange = aqiString.split("-");


    var leftRange = CalculateAQLLevel(parseInt(aqiRange[0]));
    var rightRange = CalculateAQLLevel(parseInt(aqiRange[1]));
    var aqiLevelRange = leftRange;
    if (leftRange != rightRange)
        aqiLevelRange = leftRange.replace("污染", "") + "到" + rightRange;

    return aqiLevelRange;
}
//
function CalculateAQLLevelRangeForMsg(aqiString) {
    var aqiRange = aqiString.split("-");

    var leftRange = CalculateAQLLevel(parseInt(aqiRange[0]));
    var rightRange = CalculateAQLLevel(parseInt(aqiRange[1]));
    aqiLevelRange = leftRange.replace("污染", "")

    if (leftRange != rightRange)
        aqiLevelRange = aqiLevelRange.substr(0, 1) + "到" + rightRange.substr(0, 1);

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
                var dencity = result[obj].split('/')[0];
                if (dencity != "") {
                    calSource += obj + ":" + dencity + ",";
                }
            }

        }
    }
    calSource = calSource.substr(0, calSource.length - 2);
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'CalculateNO2WithHourMethod'),
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
    //原先排除了O3的计算，现在注释掉以后就加上去了  
    //作者：张伟锋    日期：2016-06-25
    //    if (cellId[4] != "4" && cellId[4] != "5") {
    //上半夜或者下半夜数据改变，重新计算夜间的数据
    if (cellId.substring(0, 4) == "P314" || cellId.substring(0, 4) == "P411") {
        var cellEnd = cellId[4];
        var earlyNinghtDenValue = $("#" + "P314" + cellEnd).html().split('/')[0];
        var lateNightDenValue = $("#" + "P411" + cellEnd).html().split('/')[0];
        var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);

        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");

        if (nightValue == "NaN")
            return;

        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (earlyNinghtDenValue == "")
            earlyNinghtDenValue = "0";
        var lateNightDenValue = $("#" + "P421" + cellEnd).html().split('/')[0];
        if (lateNightDenValue == "")
            lateNightDenValue = "0";
        var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);
        //如果是臭氧，那么就取分段的最大值
        if (cellId[4] == "4" || cellId[4] == "5") {
            if (parseFloat(earlyNinghtDenValue) > nightValue)
                nightValue = parseFloat(earlyNinghtDenValue);
            if (parseFloat(lateNightDenValue) > nightValue)
                nightValue = parseFloat(lateNightDenValue);
        }

        if (nightValue == "NaN") {
            return;
        }

        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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

        if (nightValue == "NaN") {
            return;
        }

        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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

        if (nightValue == "NaN") {
            return;
        }

        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (earlyNinghtDenValue == "")
            earlyNinghtDenValue = "0";
        var lateNightDenValue = $("#" + "P521" + cellEnd).html().split('/')[0];
        if (lateNightDenValue == "")
            lateNightDenValue = "0";
        var nightValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6) / 10).toFixed(1);
        //如果是臭氧，那么就取分段的最大值
        if (cellId[4] == "4" || cellId[4] == "5") {
            if (parseFloat(earlyNinghtDenValue) > nightValue)
                nightValue = parseFloat(earlyNinghtDenValue);
            if (parseFloat(lateNightDenValue) > nightValue)
                nightValue = parseFloat(lateNightDenValue);
        }

        if (nightValue == "NaN") {
            return;
        }
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        //wb  2017.6.2
        if (nightValue == "NaN") {
            return;
        }
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (lateNightDenValue == "NaN") return;
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (nightValue == "")
            nightValue = "0";
        var earlyNinghtDenValue = $("#" + earlyNinghtPrefix + cellEnd).html().split('/')[0];
        if (earlyNinghtDenValue == "")
            earlyNinghtDenValue = "0";
        var lateNightDenValue = parseFloat((parseFloat(nightValue) * 10 - parseFloat(earlyNinghtDenValue) * 4) / 6).toFixed(1);
        if (lateNightDenValue == "NaN") return;
        //如果是臭氧，那么就取分段的最大值
        if (cellId[4] == "4" || cellId[4] == "5") {
            if (parseFloat(nightValue) > lateNightDenValue)
                lateNightDenValue = parseFloat(nightValue);
            if (parseFloat(earlyNinghtDenValue) > lateNightDenValue)
                lateNightDenValue = parseFloat(earlyNinghtDenValue);
        }
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
    //    }
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
        if (averValue == "NaN") return;
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (earlyNinghtDenValue == "")
            earlyNinghtDenValue = "0";
        //上半夜
        var lateNightDenValue = $("#" + "P421" + cellEnd).html().split('/')[0];
        if (lateNightDenValue == "")
            lateNightDenValue = "0";
        //上午
        var morDenValue = $("#" + "P422" + cellEnd).html().split('/')[0];
        if (morDenValue == "")
            morDenValue = "0";
        //下午
        var afterDenValue = $("#" + "P423" + cellEnd).html().split('/')[0];
        if (afterDenValue == "")
            afterDenValue = "0";
        //日平均
        var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);
        if (averValue == "NaN") return;
        //如果是臭氧，那么就取分段的最大值
        if (cellId[4] == "4" || cellId[4] == "5") {
            if (parseFloat(earlyNinghtDenValue) > averValue)
                averValue = parseFloat(earlyNinghtDenValue);
            if (parseFloat(lateNightDenValue) > averValue)
                averValue = parseFloat(lateNightDenValue);
            if (parseFloat(morDenValue) > averValue)
                averValue = parseFloat(morDenValue);
            if (parseFloat(afterDenValue) > averValue)
                averValue = parseFloat(afterDenValue);
        }
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (earlyNinghtDenValue == "")
            earlyNinghtDenValue = "0";
        //上半夜
        var lateNightDenValue = $("#" + "P521" + cellEnd).html().split('/')[0];
        if (lateNightDenValue == "")
            lateNightDenValue = "0";
        //上午
        var morDenValue = $("#" + "P522" + cellEnd).html().split('/')[0];
        if (morDenValue == "")
            morDenValue = "0";
        //下午
        var afterDenValue = $("#" + "P523" + cellEnd).html().split('/')[0];
        if (afterDenValue == "")
            afterDenValue = "0";
        //日平均
        var averValue = ((parseFloat(earlyNinghtDenValue) * 4 + parseFloat(lateNightDenValue) * 6 + parseFloat(morDenValue) * 6 + parseFloat(afterDenValue) * 8) / 24).toFixed(1);
        if (averValue == "NaN") return;
        //如果是臭氧，那么就取分段的最大值
        if (cellId[4] == "4" || cellId[4] == "5") {
            if (parseFloat(earlyNinghtDenValue) > averValue)
                averValue = parseFloat(earlyNinghtDenValue);
            if (parseFloat(lateNightDenValue) > averValue)
                averValue = parseFloat(lateNightDenValue);
            if (parseFloat(morDenValue) > averValue)
                averValue = parseFloat(morDenValue);
            if (parseFloat(afterDenValue) > averValue)
                averValue = parseFloat(afterDenValue);
        }
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        //$("#" + "P316" + cellEnd).html(maxAQIValue + "/<span>" + item + "</span>");
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
        if (averValue == "NaN") return;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
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
//获取参与计算后天全天的各要素的值
function GetAfterTotallyDayValue() {
    var sendValue = "";
    for (var i = 0; i < 4; i++) {
        var cellValue = $("#H517" + (i + 1) + "").text();
        if (cellValue != "") {
            sendValue += cellValue.split('/')[0] + ",";
        }
        else
            sendValue += ",";
    }
    return sendValue + ";0,0,0,0,;";    //第一个冒号后面的值是后台需要，安装冒号分割取值（调用后台原先计算白天的方法）
}
//获取参与计算后天白天的各要素值
function GetAfterDaySecVaule() {
    //上午：5121 --5125(PM2.5,PM10,NO2,O3,O8)
    //下午：5131 --5135(PM2.5,PM10,NO2,O3,O8)
    var sendContent = "";
    for (var i = 0; i < 2; i++) {
        var cellID = 5121 + 10 * i;
        for (var j = 0; j < 4; j++) {
            var cellValue = $("#H" + cellID.toString()).text();
            if (cellValue != "")
                sendContent = sendContent + cellValue.split('/')[0] + ",";
            else
                sendContent = sendContent + ",";
            cellID = cellID + 1;
        }
        sendContent = sendContent + ";";
    }
    return sendContent;
}
//计算后天白天的AQI值

function calculateAfterDayAQI() {
    //上午：1521 --1525(PM2.5,PM10,NO2,O3,O8)
    //下午：1531 --1535(PM2.5,PM10,NO2,O3,O8)
    var sendContent = "";
    for (var i = 0; i < 2; i++) {
        var cellID = 1521 + 10 * i;
        for (var j = 0; j < 4; j++) {
            cellID = cellID + j;
            var cellValue = $("#H" + cellID.toString()).text();
            if (cellValue != "")
                sendContent = sendContent + cellValue.split('/')[0] + ",";
            else
                sendContent = sendContent + ",";
        }
    }
    //        Ext.Ajax.request({
    //            url: getUrl('MMShareBLL.DAL.AQIForecast_48', 'ConvertToAQI'),
    //            params: { value: averValue, itemID: cellEnd },
    //            success: function (response) {
    //                if (response.responseText != "") {
    //                    $("#" + "H517" + cellEnd).html(response.responseText);
    //                    calculateAQIOfSingleLineSecond("H517" + cellEnd);
    //                }
    //            },
    //            failure: function (response) {
    //                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
    //            }
    //        });


    var afterDayMorCellValue = $("#H5126").text();
    var afterDayMorAQI = afterDayMorCellValue.split('/')[0];
    var afterDayMorItem = (afterDayMorCellValue.split('/')[1] == "O38h") ? "" : afterDayMorCellValue.split('/')[1];
    var afterDayAfterCellValue = $("#H5136").text();
    var afterDayAfterAQI = afterDayAfterCellValue.split('/')[0];
    var afterDayAfterItem = (afterDayAfterCellValue.split('/')[1] == "O38h") ? "" : afterDayAfterCellValue.split('/')[1];

    var afterDayAQI = (parseFloat(afterDayMorAQI) * 6 + parseFloat(afterDayAfterAQI) * 8) / 14;
    var afterDayItem = afterDayMorItem;
    return afterDayAQI + "&" + afterDayItem
}
function judgeMsgCharCount() {
    if ($("#msgArea").val().length <= 190) {
        $("#charCount").val($("#msgArea").val().length + "/190");
        $("#msgArea").css("background-color", "white")
    }
    else {
        $("#charCount").val("已超出" + ($("#msgArea").val().length - 190) + "个字");
        $("#msgArea").css("background-color", "lightpink")
    }
}

//xuehui  2017.6.1
function getPollutant() {
    var arr = ["PM2.5", "PM10", "NO2", "O3"];
    var html = "";
    for (var i = 0; i < arr.length; i++) {
        html += "<option value='" + arr[i] + "'>" + arr[i] + "</option>";
    }
    $("#P5170p").html(html);
    $("#P5270p").html(html);
    $("#H5170p").html(html);


    document.getElementById("P5170v").disabled = true;
    document.getElementById("P5170p").disabled = true;
}
