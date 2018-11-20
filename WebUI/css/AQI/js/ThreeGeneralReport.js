Ext.onReady(function () {
    //显示预报员，预报时间和时次
    var loginParams = getCookie("UserInfo");
    var result = Ext.util.JSON.decode(loginParams);
    $("#forecaster").html(result["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");

    //设置界面宽度
    var pageWidth = document.body.clientWidth;
    var pageHeight = document.documentElement.clientHeight;
    $("#hazeArea").width(($(window).width() - 30) * 0.55);
    $("#rightArea").width(($(window).width() - 30) * 0.45);
    $("#hazeArea").height($(window).height() - 50);
    $("#rightArea").height($(window).height() - 50);
    $("#txtArea_Haze").height($(window).height() - 260);
    
    $("body").css("min-width", $(window).width() + "px");

   

    //初始化获取数据

    //紫外线
    var currentDate = getNowDate();
    //获取紫外线实况值
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetUVRealData'),
        params: { currentDate: currentDate },
        success: function (response) {

            if (response.responseText != "") {
                $("#txtAvgUVAB").val(response.responseText);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
    //臭氧
    AutoGet();
    SetInput();
});

function AutoGet() {

    //AutoGet(int BigPeriodType,string PeriodDate)
    var BigPeriodType = 1; //24时
    var PeriodDate = $("#txtForecastTime").val();

    var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "正在获取数据" });
    myMask.show();
    var nowDate = getNowDate();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetOzoneData'),
        params: { forecastDate: nowDate },
        success: function (response) {
            if (response.responseText != "")
                try {
                    var result = Ext.util.JSON.decode(response.responseText);
                    $("#txtO3_1H").val(result['O3']);
                    CalculationRangeByOx($("#txtO3_1H"), $("#txtO3_1H_AVG"));
                    $("#txtO3_8H").val(result['O3_8']);
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

    txtContent = $("#txtHideTempleteContent").val();
    txtContent = txtContent.replace("{ForecastHour}", "17时");
    var O3_1H = $("#txtO3_1H_AVG").val();
    var O3_1H_Time_Range = $("#txtO3_1H_Time").val();
    var O3_8H = $("#txtO3_8H_AVG").val();
    var O3_8H_Time_Range = $("#txtO3_8H_Time").val();

    txtContent = txtContent.replace("{O3_1h}", O3_1H);
    txtContent = txtContent.replace("{O3_1h_Time_Rang}", O3_1H_Time_Range);
    txtContent = txtContent.replace("{O3_8h}", O3_8H);
    txtContent = txtContent.replace("{O3_8h_Time_Rang}", O3_8H_Time_Range);

    CKEDITOR.instances.ozoneContent.setData(txtContent);
}