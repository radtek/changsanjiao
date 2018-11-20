var nowDateTime = getNowFormatDate();
var txtContent;
var userName = "";
Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    userName = result["Alias"];
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

    var nowDate = new Date();
    var tomorrowDay = new Date();
    tomorrowDay.setDate(nowDate.getDate() + 1);
    var hideOzoneContent = $("#txtHideOzoneTemplate").val();
    hideOzoneContent = hideOzoneContent.replace('{TomorrowDate}', GetDateStrNoYear(1));
    hideOzoneContent = hideOzoneContent.replace('{TodayDate}', GetDateStr(0));
    $("#txtHideOzoneTemplate").val(hideOzoneContent);
    txtContent = $("#txtHideOzoneTemplate").val();

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadSavedOzoneForecastData'),
        success: function (response) {
            if (response.responseText != "") {
                var results = Ext.util.JSON.decode(response.responseText);
                $("#txtO3_1H").val(results["O3"]);
                $("#txtO3_8H").val(results["O38"]);
                $("#txtO3_1H_Time").val(results["O3Period"]);
                $("#txtO3_8H_Time").val(results["O38Period"]);
                CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
                CalculationRangeByOx($("#txtO3_8H"), $("#txtO3_8H_AVG"));
                SetInput();
                SetTxtContent();

            }
        },
        failure: function (response) {
        }
    });

    SetTxtContent();
    SetInput();

    $("#autoOzoneGet").click(function () {
        AutoGet();
    })

    //保存按键点击
    $("#ozoneSave").click(function () {
        var o3Value = $("#txtO3_1H").val();
        var o38Value = $("#txtO3_8H").val();
        var o3Period = $("#txtO3_1H_Time").val();
        var o38Period = $("#txtO3_8H_Time").val();
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在保存..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveOzoneData'),
            params: { releaseTime: getReleaseTime(), o3Value: o3Value, o38Value: o38Value, o3Period: o3Period, o38Period: o38Period },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("保存成功！");
                    $("#ozoneCheck").show();
                }
                else if (response.responseText == "published") {
                    alert("数据已保存！");
                    $("#ozoneCheck").show();
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
    });

    //审核按键
    $("#ozoneCheck").click(function () {
        alert("审核");
        $("#oazonePub").show();
    });


    //获取历史记录
    $("#ozoneHistory").click(function () {
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'LoadHistoryOzone'),
            success: function (response) {
                myMask.hide();
                if (response.responseText != "") {
                    var results = Ext.util.JSON.decode(response.responseText);
                    $("#txtO3_1H").val(results["O3"]);
                    $("#txtO3_8H").val(results["O38"]);
                    $("#txtO3_1H_Time").val(results["O3Period"]);
                    $("#txtO3_8H_Time").val(results["O38Period"]);
                    CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
                    CalculationRangeByOx($("#txtO3_8H"), $("#txtO3_8H_AVG"));
                    SetInput();
                    SetTxtContent();
                }
                else {
                    alert("获取历史记录失败！");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //发布按键点击
    $("#oazonePub").click(function () {
        var fileDate = getFormatDate('');
        //ftp地址集合字符串
        var ftpString = $("#ozoneFtpCollection").val();
        var publishContent = "";
        publishContent = $("#ozoneContent").text();
        var fileName = "ozoneYYMMDDHHmm.txt";
        var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在发布..." });
        myMask.show();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'UpLoadTxtFtpLatestForOzone'),
            params: { ftpString: ftpString, fileDate: fileDate, functionName: "OzoneForecast", txtContent: publishContent, userName: userName },
            success: function (response) {
                myMask.hide();
                if (response.responseText == "success") {
                    alert("发布成功！");
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

function AutoGet() {

    //AutoGet(int BigPeriodType,string PeriodDate)
    var BigPeriodType = 1; //24时
    var PeriodDate = $("#txtForecastTime").val();

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
    myMask.show();

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetOzoneData'),
        params: { forecastDate: nowDateTime },
        success: function (response) {
            myMask.hide();
            if (response.responseText != "")
                try {
                    var result = Ext.util.JSON.decode(response.responseText);
                    if (result['O3'] == "") {
                        $("#txtO3_1H").val("0");
                    }
                    else {
                        $("#txtO3_1H").val(result['O3']);
                    }

                    CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
                    if (result['O3_8'] == "") {
                        $("#txtO3_8H").val("0");
                    }
                    else {
                        $("#txtO3_8H").val(result['O3_8']);
                    }
                    //$("#txtO3_8H").val(result['O3_8']);
                    CalculationRangeByOx($("#txtO3_8H"), $("#txtO3_8H_AVG"));
                }
                catch (e) {
                    myMask.hide();
                }
                finally {
                    myMask.hide();
                }
        },
        failure: function (response) {
            myMask.hide();
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function SetInput() {

    if ($.browser.msie) {
        $("#txtO3_1H").get(0).attachEvent("onpropertychange", function (o) {
            CalculationRangeByOx(o.srcElement, $("#txtO3_1H_AVG"));
        });
        $("#txtO3_8H").get(0).attachEvent("onpropertychange", function (o) {
            CalculationRangeByOx(o.srcElement, $("#txtO3_8H_AVG"));
        });
        $("#txtO3_1H_Time").get(0).attachEvent("onpropertychange", function (o) {
            CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
        });
        $("#txtO3_8H_Time").get(0).attachEvent("onpropertychange", function (o) {
            CalculationRangeByOx($("#txtO3_8H"), $("#txtO3_8H_AVG"));
        });
        //非IE  
    }
    else {
        $("#txtO3_1H").get(0).addEventListener("input", function (o) {
            CalculationRangeByOx(o.srcElement, $("#txtO3_1H_AVG"));
        }, true);

        $("#txtO3_8H").get(0).addEventListener("input", function (o) {
            CalculationRangeByOx(o.srcElement, $("#txtO3_8H_AVG"));
        }, true);
        $("#txtO3_1H_Time").get(0).addEventListener("input", function (o) {
            CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
        });
        $("#txtO3_8H_Time").get(0).addEventListener("input", function (o) {
            CalculationRangeByOx($("#txtO3_8H"), $("#txtO3_8H_AVG"));
        });
    }
    $("#txtO3_1H").change(function () {
        CalculationRangeByOx(this, $("#txtO3_1H_AVG"));
    });
    $("#txtO3_8H").change(function () {
        CalculationRangeByOx(this, $("#txtO3_8H_AVG"));
    });

    $("#txtO3_1H_Time").change(function () {
        CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
    });
    $("#txtO3_8H_Time").change(function () {
        CalculationRangeByOx($("#txtO3_8H"), $("#txtO3_8H_AVG"));
    });

}

function CalculationRangeByOx(objValue, obgTarget) {

    //ppb*48/22.4=μg/m³ 
    //ppb = (μg/m³) * 22.4 /48
    var Ox = $(objValue).val();
    var rang = "";
    if (Ox && Ox != "" && Ox != " " && $.isNumeric(Ox)) {
        var ret = Ox * parseFloat((22.4 / 48));
        /*ret = Math.round(ret);
        rang = (ret - 5) + "-" + (ret + 5);*/
        ret = RoundJS(ret, 0);
        rang = GetO3Region(ret);

    }
    $(obgTarget).val(rang);

    SetTxtContent();
}

function SetTxtContent() {

    txtContent = $("#txtHideOzoneTemplate").val();
    txtContent = txtContent.replace("{ForecastHour}", "17时");
    var O3_1H = $("#txtO3_1H_AVG").val();
    var O3_1H_Time_Range = $("#txtO3_1H_Time").val();
    var O3_8H = $("#txtO3_8H_AVG").val();
    var O3_8H_Time_Range = $("#txtO3_8H_Time").val();

    txtContent = txtContent.replace("{O3_1h}", O3_1H);
    txtContent = txtContent.replace("{O3_1h_Time_Rang}", O3_1H_Time_Range);
    txtContent = txtContent.replace("{O3_8h}", O3_8H);
    txtContent = txtContent.replace("{O3_8h_Time_Rang}", O3_8H_Time_Range);

    //CKEDITOR.instances.ozoneContent.setData(txtContent);
    $("#ozoneContent").text(txtContent);
}

function GetO3Region(idx) {
    /*
    * 个位数是1和2那么向下靠0；3、4、5向上靠5；6,7向下靠5；8，9,0向上靠0
    */
    //new 
    if (idx < 0) {
        return (idx - 5) + "-" + (idx + 5);
    }
    var remainder = idx % 5; //取余数
    var integer = parseInt(idx / 5); //取整

    if (remainder == 1 || remainder == 2) {
        idx = integer * 5;
    }
    else if (remainder == 3 || remainder == 4 || remainder == 6 || remainder == 7) {
        idx = integer * 5 + 5;
    }
    else if (remainder == 8 || remainder == 9) {
        idx = integer * 5 + 10;
    }
    else if (remainder == 0 || remainder == 5) {
        idx = idx + 0;
    }
    //上下浮动5
    var lowerLimit = (idx - 5); //下限
    var upperLimit = (idx + 5); //上限
    lowerLimit = (lowerLimit >= 0) ? lowerLimit : 0;
    var region = lowerLimit + "-" + upperLimit;
    return region;
}

function RoundJS(num, digit) {
    var ratio = Math.pow(10, digit),
        _num = num * ratio,
        mod = _num % 1,
        integer = Math.floor(_num);
    var ret;
    if (mod > 0.5) {
        ret = (integer + 1) / ratio;
    }
    else if (mod < 0.5) {
        ret = integer / ratio;
    }
    else {
        ret = (integer % 2 === 0 ? integer : integer + 1) / ratio;
    }
    return ret;
}