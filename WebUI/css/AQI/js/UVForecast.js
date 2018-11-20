var userName = "";
$(function () {
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    userName = result["Alias"];
    var currentDate = getNowDate();

    //10点之前显示9:45自动上传的那个文件的内容
    var curHour = new Date().getHours();
    if (curHour < 10) {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadHistoryUV'),
            success: function (response) {
                if (response.responseText != "") {
                    $("#txtAvgUVAB").val(response.responseText.split('_')[0]);
                    $("#txtUVIndex").val(response.responseText.split('_')[1]);
                    $("#txtUVLevel").val(GetUVLevel($("#txtUVIndex").val()));
                    SetUVDefense();
                    SetProductContent();
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
    else {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadSavedUVForecastData'),
            success: function (response) {
                if (response.responseText != "") {
                    $("#txtAvgUVAB").val(response.responseText.split('_')[0]);
                    $("#txtUVIndex").val(response.responseText.split('_')[1]);
                    $("#txtUVLevel").val(GetUVLevel($("#txtUVIndex").val()));
                    SetUVDefense();
                    SetProductContent();
                }
            },
            failure: function (response) {
            }
        });
    }

    if ($.browser.msie) {
        //实况值改变 
        $("#txtAvgUVAB").get(0).attachEvent("onpropertychange", function (o) {
            //FormatUVAvg();
            //            SetProductContent();
            $("#txtUVIndex").val();
        });

        //预报指数改变
        $("#txtUVIndex").get(0).attachEvent("onpropertychange", function (o) {
            $("#txtUVLevel").val(GetUVLevel($("#txtUVIndex").val()));
            SetUVDefense();
            SetProductContent();
        });
        //非IE  
    }
    else {
        //实况值改变
        $("#txtAvgUVAB").get(0).addEventListener("input", function (o) {
            // FormatUVAvg();
            SetProductContent();
        }, false);

        //预报指数改变
        $("#txtUVIndex").get(0).addEventListener("input", function (o) {
            $("#txtUVLevel").val(GetUVLevel($("#txtUVIndex").val()));
            SetUVDefense();
            SetProductContent();
        }, false);
    }

    //获取实况按键事件
    $("#btnAutoGetUV").click(function () {
        GetRealTimeUVValue();
    });

    //获取历史记录
    $("#btnLastOneUV").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadHistoryUV'),
            //params: { dateTime: getNowDate(), UVAB: uvabValue, uvIndex: uvIndex },
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    $("#txtAvgUVAB").val(response.responseText.split('_')[0]);
                    $("#txtUVIndex").val(response.responseText.split('_')[1]);
                    $("#txtUVLevel").val(GetUVLevel($("#txtUVIndex").val()));
                    SetUVDefense();
                    SetProductContent();
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //保存按键
    $("#uvSave").click(function () {
        //紫外线强度值
        var uvabValue = $("#txtAvgUVAB").val();
        //紫外线预报指数
        var uvIndex = $("#txtUVIndex").val();
        var fileDate = getFormatDate('');
        var tomorrowContent = $("#tom10Result").val();
        if (uvabValue != '') {
            var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
            myMask.show();
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveUVForecastData'),
                params: { dateTime: getNowDate(), UVAB: uvabValue, uvIndex: uvIndex, fileDate: fileDate, tomorrowContent: tomorrowContent, userName: userName },
                success: function (response) {
                    myMask.hide();
                    if (response.responseText == "success") {
                        alert("保存成功");
                        $("#uvCheck").show();
                    }
                    else if (response.responseText == "published") {
                        alert("数据已保存！");
                        $("#uvCheck").show();
                    }
                    else {
                        alert("保存失败！");
                    }
                },
                failure: function (response) {
                    myMask.hide();
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                }
            });
        }
        else {

        }
    });

    //审核按键
    $("#uvCheck").click(function () {
        alert("审核");
        $("#uvPub").show();
    });


    //发布按键
    $("#uvPub").click(function () {
        var fileDate = getFormatDate('');
        //根据上传时间来判断
        var timeType = "";
        var txtContent = "";
        var tomorrowContent = "";
        var fileName = "";

        //ftp地址集合字符串
        var ftpString = $("#uvFtpCollection").val();
        var ftpStringTom = $("#uvFtpCollectionTom").val();
        //        var replaceDate = GetReplaceDateStringMMDD(new Date());
        var curHour = new Date().getHours();
        txtContent = $("#txtToday16Result").val();
        tomorrowContent = $("#tom10Result").val();
        //        if (curHour < 16) {
        //            txtContent = $("#txtToday16Result").val();
        //            tomorrowContent = $("#tom10Result").val();            
        //        }
        //        else {
        //            alert("已过发布时间！");
        //            return;
        //        }
        if (txtContent == "") {
            alert("文本内容不能为空！");
            return;
        }
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpLoadTxtFtpLatestForUV'),
            params: { ftpString: ftpString, ftpStringTom: ftpStringTom, fileDate: fileDate, functionName: "UVForecast", txtContent: txtContent, tomorrowContent: tomorrowContent, userName: userName },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("发布成功");
                }
                else if (response.responseText == "less") {
                    alert("发布不完全");
                }
                else {
                    alert("发布失败");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

});


//设置防御措施
function SetUVDefense() {
    var uvIndex = $("#txtUVIndex").val(); //紫外线预报指数
    var uvLevel = GetUVLevel(uvIndex); //紫外线等级
    var uvDefense = ""; //防御措施
    if (uvIndex >= 0 && uvIndex <= 2) {
        uvDefense = "不需要采取防护措施。";
    }
    else if (uvIndex >= 3 && uvIndex <= 4) {
        uvDefense = "可以适当采取一些防护措施，如：涂擦防护霜等。";
    }
    else if (uvIndex >= 5 && uvIndex <= 6) {
        uvDefense = "外出时戴好遮阳帽、太阳镜和太阳伞等，涂擦SPF指数大于15的防护霜。";
    }
    else if (uvIndex >= 7 && uvIndex <= 9) {
        uvDefense = "除上述防护措施外，上午十点至下午四点时段避免外出，或尽可能在遮荫处。";
    }
    else if (uvIndex >= 10) {
        uvDefense = "尽可能不在室外活动，必须外出时，要采取各种有效的防护措施。";
    }
    //防御措施
    //$("#txtUVDefense").val(uvDefense);
    $("#uvProtectContent").val(uvDefense);
    //紫外线等级
    var uvLevelDesc = uvLevel + "级";
    //$("#lblUVLevelDesc").html(uvLevelDesc);
    $("#lblUVLevelDesc").val(uvLevelDesc);
}

//根据实况值 预报指数值生成产品结果
function SetProductContent() {
    //16时模板
    var textContent = $("#txtHideTempleteContent").val();
    //10时模板
    var textContentTomorrow10 = $("#txtHideTempleteContent_Tomorrow10").val(); 

    var uvType = $("#txtHideUVType").val();

    //预报指数
    var uvIndex = $("#txtUVIndex").val();

    //根据预报指数获取等级
    var uvLevel = GetUVLevel(uvIndex);

    //实况值
    var avgUVAB = $("#txtAvgUVAB").val();

    //替换UV等级
    textContent = textContent.replace("{UVLevel}", uvLevel);
    textContentTomorrow10 = textContentTomorrow10.replace("{UVLevel}", uvLevel);  

    var tempAvgUVAB = pad(avgUVAB,3);
    if ($.trim(tempAvgUVAB) == "") {
        tempAvgUVAB = "000";
    }
    //将实际值去掉小数点
    var usrValue = parseFloat(avgUVAB);
    textContentTomorrow10 = textContentTomorrow10.replace("{10}", tempAvgUVAB.toString());


    //将结果赋值
    $("#txtToday16Result").val(textContent);
    $("#tom10Result").val(textContentTomorrow10);
}

function GetUVLevel(UVIndex) {

    if (UVIndex >= 0 && UVIndex <= 2) {
        return 1;
    }
    else if (UVIndex >= 3 && UVIndex <= 4) {
        return 2;
    }
    else if (UVIndex >= 5 && UVIndex <= 6) {
        return 3;
    }
    else if (UVIndex >= 7 && UVIndex <= 9) {
        return 4;
    }
    else if (UVIndex >= 10) {
        return 5;
    }
    return 1;
}

function pad(inputNum, n) {
    if (inputNum == "") {

        return "000";
    }

    var num = Math.round(inputNum * 10) / 10; 

    num = num + "";
    if (num.indexOf('.') == -1) {
        num = num + ".0";
    }
    num = num.replace(".", "");
    num = num.replace(".", "");
    var len = num.toString().length;
    if (len < n) {
        while (len < n)
        { num = "0" + num; len++; }
    }
    else {
        num = num.substring(0, 3);
    }
    return num;
}

//    function pad(num) {
//        var useValue = parseFloat(num);
//        if (useValue < 10) {
//            return "0" + (useValue * 10).toString();
//        }
//        else if (useValue >= 10 && useValue < 100) {
//            return (useValue * 10).toString();
//        }
//        else {
//            return useValue;
//        }
//    }

//获取紫外线实况值
function GetRealTimeUVValue() {
    var h_time = $("#txtForcastDate").val();
    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
    myMask.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetUVRealData'),
        params: { currentDate: h_time },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "") {
                $("#txtAvgUVAB").val(response.responseText);
                $("#txtUVIndex").val(0);
                $("#txtUVLevel").val(0);
                SetUVDefense();
                SetProductContent();
            }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}


function toDecimal(x) { 
    var f = parseFloat(x); 
    if (isNaN(f)) { 
       return false; 
    } 
    var f = Math.round(x*10)/10; 
    var s = f.toString(); 
    var rs = s.indexOf('.');
    if (rs < 0) {
        rs = s.length;
        s += '.';
    }
    while (s.length <= rs + 1) { 
        s += '0'; 
    } 
    return s;
} 

function UploadTomorrowUV() {
    var fileDate = getFormatDate('');
        //根据上传时间来判断
        var timeType = "";
        var txtContent = "";
        var fileName = "";

        //ftp地址集合字符串
        var ftpString = $("#uvFtpCollection").val();
        var fileDate = getFormatDate('');

        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UploadTomorrowUV'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "UVForecast", userName: userName },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("发布成功");
                }
                else if (response.responseText == "less") {
                    alert("发布不完全");
                }
                else {
                    alert("发布失败");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
}
