var vm = null;
var beforeVal = "", beforeAqi="";     //记录单元格值发生变化之前的值，用来判断单元格的值是否发生变化，如果变化则重新计算aqi值
//var result = {
//   // 数组一是检测中心的值，二是气象局的值，三是两家预报的值    data数组表是一行的值，里面的项值表示单元格的值
//    "row1": {
//        "Period": [{ "val": "24小时" }, { "rowspan": "1" }, { "colspan": "1" }, { "class": "dis" }]
//       , "Date": [{ "val": "12月31日" }, { "rowspan": "1" }, { "colspan": "1" }, { "class": "dis" }]
//       , "Interval": [{ "val": "上半夜" }, { "rowspan": "1" }, { "colspan": "1" }, { "class": "dis" }]
//       , "Ele": {
//           "PM25": [{ "val": "135.0/179" }, { "val": "115.0/150" }, { "val": "125.0/165" }]
//           , "PM10": [{ "val": "150.0/100" } , { "val": "130.0/90" }, { "val": "140.0/95" }]
//           , "NO2": [{ "val": "90.0/45" }, { "val": "110.0/55" }, { "val": "100.0/50" }]
//           , "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
//           , "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
//           , "AQI": [{ "val": "179/PM2.5" }, { "val": "150/PM2.5" }, { "val": "165/PM2.5" }]
//       }
//    }
//};

var title = {
    "PM25": [{ "val": "监测中心" }, { "val": "气象局" }, { "val": "PM2.5" }]
    , "PM10": [{ "val": "监测中心" }, { "val": "气象局" }, { "val": "PM10" }]
    , "NO2": [{ "val": "监测中心" }, { "val": "气象局" }, { "val": "NO2" }]
    , "O31": [{ "val": "监测中心" }, { "val": "气象局" }, { "val": "O3-1h" }]
    , "O38": [{ "val": "监测中心" }, { "val": "气象局" }, { "val": "O3-8h" }]
    , "AQI": [{ "val": "监测中心" }, { "val": "气象局" }, { "val": "AQI" }]
} 
var result = getData();
$(function () {
    var loginParams = getCookie('UserInfo');
    var loginResult = eval('('+loginParams+')');
    userName = loginResult['Alias'];
    user = loginResult['UserName'];
    var pageWidth = $(document).width();
    $(".btnArea").width(pageWidth);
    vm = new Vue({
        el: "#page"
        , data: {
              result: result
            , option: ["PM2.5", "PM10", "NO2", "O3"]
            , isSubData: true    //是否是主观数据
            , userName: userName
            , period:period+"时"
            , forecastDate: new Date().Format("yyyy-MM-dd hh:mm:ss")
            , disabled: false    //是否禁用
            , inborder: false    //是否显示边框
           
        }
        , mounted: function () {
            this.$nextTick(function () {
                this.getSubData("init");
            })
        }
        , methods: {
            init: function () {
                var period = this.period.substring(0, 2);
                this.$http.post('AQIPeriod72.aspx/Init', {period: period }).then(function (response) {
                    var data = eval('(' + response.data.d + ')');
                    this.result = data;
                });
            }
            , test: function () {
                this.$http.post('AQIPeriod72.aspx/Reader').then(function (response) {
                    
                });
            }
            , getSubData: function (type) {                 //获取主观数据
                var period = this.period.substring(0, 2);
                this.$http.post('AQIPeriod72.aspx/GetSubData', { period: period,type:type }).then(function (response) {
                    var data =eval('('+ response.data.d + ')');
                    this.result = data;
                    this.isSubData = true;
                    this.disabled = false;   //输入框能编辑
                    this.inborder = false;    //显示输入框的边框
                });
            }
            , getHistoryData: function () {                  //获取历史数据
                this.$http.post('AQIPeriod72.aspx/GetHistoryData').then(function (response) {
                    var data = eval('(' + response.data.d + ')');
                    this.result = data;
                    this.isSubData = false;
                    this.disabled = true;
                    this.inborder = true;    //不显示输入框的边框
                });
            }
            , save: function (type) {        //保存按钮
                if (type != "contentSave") {    //说明是预览里面的保存按钮，调用之前提醒过了，不需要提醒
                    if (!confirm("是否要保存数据？")) return;
                }
                var str = JSON.stringify(this.result);
                str = "[" + str + "]";
                var period = $("#period").text().substring(0,2);
                this.$http.post('AQIPeriod72.aspx/Save', { json: str, UserName: userName, period: period }).then(function (response) {
                    var data = response.data.d;
                        if (data.indexOf("error") > -1) {
                            alert("保存失败！");
                        } else {
                            alert("保存成功！");
                            this.getSubData("init");   //刷新页面，获取主观数据，主观数据是先去保存的数据，然后再取接口数据，在这里由于保存成功了，所以直接调用返回的是保存在库里的数据
                        }
                });
            }
            , txtSave: function () {     //文本保存，既要保存要发布的文本，同事也要保存数据入库
                if (!confirm("是否要保存文本以及数据？")) return;
                var type = this.getSaveOrPublishType();
                var content = this.getSaveOrPublishContent(type);
                var period = this.period.substring(0, 2);
                this.$http.post('AQIPeriod72.aspx/TxtSave', { content:content, isSubData: this.isSubData, period: period,type:type}).then(function (response) {
                    var data = response.data.d;
                    if (data.indexOf("error") > -1) {
                        alert("保存失败！");
                    } else {
                        this.save("contentSave");   //数据入库
                    }
                });
            }
            , publish: function () {
                if (!confirm("是否要发布文本？")) return;
                var type = this.getSaveOrPublishType();
                var content = this.getSaveOrPublishContent(type);
                var period = $("#period").text().substring(0, 2);
                var message = $("#message").val();
                this.$http.post('AQIPeriod72.aspx/Publish', { content:content,period:period ,type:type,user:userName}).then(function (response) {
                    var data = response.data.d;
                    if (data.indexOf("error") > -1) {
                        alert("发布失败！");
                    } else {
                        alert("发布成功！");
                    }
                });
            }
            , preView: function () {    //预览按钮
                this.initModal();
                if (!this.isSubData) {    //如果是历史数据则要读取昨天保存的文本
                    this.readerHistoryTxt();
                    return;
                }
                var Time = new Date();
                var hour = $("#period").text().substring(0,2);
                var textTemp = $("#TxtTemplete_06").text();
                var messTemp = "中心气象台和监测中心{PublishDate}06时联合发布的上海市空气质量预报：第一个上午{firstSWLevel}({firstSWItem})，AQI为{firstSWAQI}；下午{firstXWLevel}({firstXWItem})，" +
                    "AQI为{firstXWAQI}；夜间{secondYJLevel}({secondYJItem})，AQI为{secondYJAQI}；第一个{firstQTLevel}({firstQTItem})，AQI为{firstQTAQI}；第二个{secondQTLevel}({secondQTItem})，AQI为{secondQTAQI}。";
                if (parseInt(hour) == 17) {
                    textTemp = $("#TxtTemplete_17").text();
                    messTemp = "中心气象台和监测中心{PublishDate}17时联合发布的上海市空气质量预报：第一个夜间{firstYJLevel}({firstYJItem})，AQI为{firstYJAQI}；第一个上午{firstSWLevel}({firstSWItem})，AQI为{firstSWAQI}；下午{firstXWLevel}({firstXWItem})，" +
                    "AQI为{firstXWAQI}；夜间{secondYJLevel}({secondYJItem})，AQI为{secondYJAQI}；第一个{firstQTLevel}({firstQTItem})，AQI为{firstQTAQI}；第二个{secondQTLevel}({secondQTItem})，AQI为{secondQTAQI}。";
                }
                textTemp = this.proTemplete(textTemp, Time, hour,"ftp");
                $("#text").text(textTemp);   //赋值
                messTemp = this.proTemplete(messTemp, Time, hour,"message");
                $("#message").text(messTemp);
            }
            , readerHistoryTxt: function () {   //读取历史文本数据
                this.$http.post('AQIPeriod72.aspx/ReaderHistoryTxt', { type: "history" }).then(function (response) {
                    var data = response.data.d;
                    var txt = data.split("#")[0];
                    var message = data.split("#")[1];
                    if (txt == "|" && message=="|") {
                        alert("昨天数据未保存");
                    }else if (message == "|") {
                        alert("昨天短信内容未保存");
                    }else if (txt == "|") {
                        alert("昨天72小时AQI分时段预报未保存");
                    }
                    $("#text").text(txt);
                    $("#message").text(message);
                });
            }
            //点击预览之后里面文本的处理
            , proTemplete: function (txt,Time,hour,type) {
                txt = txt.replace("{PublishDate}", Time.Format("yyyy年MM月dd日")).replace("{Hour}", hour);
                //读取值////////////////////////////////    06是只是没有第一个夜间，其他的和17时的一样，以此类推
                var firstYJ = this.result.row3.Ele.AQI[2].val;    //第一个夜间的值
                var firstYJAQI = this.CalculateAQIRange(firstYJ.split('/')[0]);
                var firstYJLevel = this.CalculateAQLLevelRange(firstYJAQI);
                var firstYJItem = firstYJ.split('/')[1] == "" ? "--" : firstYJ.split('/')[1];
                var firstYJTime = this.result.row3.Date[0].val.split("月")[1];    //第一个夜间的日期

                var firstSW = this.result.row4.Ele.AQI[2].val;    //第一个上午的值
                var firstSWAQI = this.CalculateAQIRange(firstSW.split('/')[0]);
                var firstSWLevel = this.CalculateAQLLevelRange(firstSWAQI);
                var firstSWItem = firstSW.split('/')[1] == "" ? "--" : firstSW.split('/')[1];
                var firstSWTime = this.result.row4.Date[0].val.split("月")[1];   //第一个上午的日期

                var firstXW = this.result.row5.Ele.AQI[2].val;     //第一个下午的值
                var firstXWAQI = this.CalculateAQIRange(firstXW.split('/')[0]);
                var firstXWLevel = this.CalculateAQLLevelRange(firstXWAQI);
                var firstXWItem = firstXW.split('/')[1] == "" ? "--" : firstXW.split('/')[1];
                var firstXWTime = this.result.row5.Date[0].val.split("月")[1];    //第一个下午的值

                var secondYJ = this.result.row8.Ele.AQI[2].val;     //第二个夜间的值
                var secondYJAQI = this.CalculateAQIRange(secondYJ.split('/')[0]);
                var secondYJLevel = this.CalculateAQLLevelRange(secondYJAQI);
                var secondYJItem = secondYJ.split('/')[1] == "" ? "--" : secondYJ.split('/')[1];
                var secondYJTime = this.result.row8.Date[0].val.split("月")[1];    //第二个夜间的时间

                var firstQT = this.result.row14.Ele.AQI[2].val;     //第一个全天的值
                var firstQTAQI = this.CalculateAQIRange(firstQT.split('/')[0]);
                var firstQTLevel = this.CalculateAQLLevelRange(firstQTAQI);
                var firstQTItem = firstQT.split('/')[1] == "" ? "--" : firstQT.split('/')[1];
                var firstQTTime = this.result.row14.Date[0].val.split("月")[1];     //第一个全天的时间

                var secondQT = this.result.row12.Ele.AQI[2].val;    //第二个全天的首要污染物值
                var secondQTAQI = this.CalculateAQIRange(secondQT);
                var secondQTLevel = this.CalculateAQLLevelRange(secondQTAQI);
                var secondQTItem = this.result.row12.Ele.AQI[2].poll == "" ? "--" : this.result.row12.Ele.AQI[2].poll;   //第二个全天的首要污染物
                var secondQTTime = this.result.row12.Date[0].val.split("月")[1];     //第二个污染物的时间

                //当AQI值完全在优等级时（没有出现跨良等级），没有首要污染物
                firstYJItem = firstYJLevel == "优" ? "" : firstYJItem;
                firstSWItem = firstSWLevel == "优" ? "" : firstSWItem;
                firstXWItem = firstXWLevel == "优" ? "" : firstXWItem;
                secondYJItem = secondYJLevel == "优" ? "" : secondYJItem;
                firstQTItem = firstQTLevel == "优" ? "" : firstQTItem;
                secondQTItem = secondQTLevel == "优" ? "" : secondQTItem;
                //文本替换
                txt = txt.replace("{firstYJAQI}", firstYJAQI).replace("{firstYJLevel}", firstYJLevel).replace('{firstYJItem}', firstYJItem).replace("第一个夜间", firstYJTime + "夜间");     //替换第一个夜间
                txt = txt.replace("{firstSWAQI}", firstSWAQI).replace("{firstSWLevel}", firstSWLevel).replace('{firstSWItem}', firstSWItem).replace("第一个上午", firstSWTime + "上午");     //替换第一个上午
                txt = txt.replace("{firstXWAQI}", firstXWAQI).replace("{firstXWLevel}", firstXWLevel).replace('{firstXWItem}', firstXWItem).replace("第一个下午", firstXWTime + "下午");     //替换第一个下午
                txt = txt.replace("{secondYJAQI}", secondYJAQI).replace("{secondYJLevel}", secondYJLevel).replace('{secondYJItem}', secondYJItem).replace("第二个夜间", secondYJTime + "夜间");     //替换第二个夜间
                txt = txt.replace("{firstQTAQI}", firstQTAQI).replace("{firstQTLevel}", firstQTLevel).replace('{firstQTItem}', firstQTItem).replace("第一个", firstQTTime);;     //替换第一个全天
                txt = txt.replace("{secondQTAQI}", secondQTAQI).replace("{secondQTLevel}", secondQTLevel).replace('{secondQTItem}', secondQTItem).replace("第二个", secondQTTime);;     //替换第二个全天
                if (type == "message") {    //如果是短信文本则需要把污染两字去掉
                    txt = txt.replace(/污染/g, "").replace("()", '');
                }
                return txt;
            }
            , getSaveOrPublishContent: function (type) {
                var content = $("#text").val();
                if (type == "message") {
                    content = $("#message").val();
                }
                return content;
            }
            , getSaveOrPublishType: function () {
                var type = $("#myModal .active a").text();
                if (type.indexOf("短信") > -1) {
                    type = "message";
                } else {
                    type = "ftp";
                }
                return type;
            }
            //根据AQI值计算AQI预报范围
            , CalculateAQIRange: function (aqiString) {
                    var aqiRange = "--";
                    if (aqiString != "") {
                        var aqiValue = parseInt(aqiString);
                        //根据靠近0还是5计算出的实际使用值
                        var useValue = 0;
                        if (aqiValue > 0) {
                            //更靠近0
                            if (this.IsNearZero(aqiValue)) {
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
            , IsNearZero: function (number) {
                var unitNumber = number % 10;
                var nearZero = !(unitNumber >= 3 && unitNumber <= 7);
                return nearZero;
            }
            , CalculateAQLLevelRange: function (aqiString) {
                var aqiLevelRange = "--";
                if(aqiString!=""){
                    var aqiRange = aqiString.split("-");
                    var leftRange =this.CalculateAQLLevel(parseInt(aqiRange[0]));
                    var rightRange = this.CalculateAQLLevel(parseInt(aqiRange[1]));
                    aqiLevelRange = leftRange;
                    if (leftRange != rightRange)
                        if (leftRange == "") {
                            aqiLevelRange = rightRange;
                        }else{
                            aqiLevelRange = leftRange.replace("污染", "") + "到" + rightRange;
                        }
                }
                    return aqiLevelRange;
            }
            , CalculateAQLLevel: function (aqiValue) {
                var strAQLLevel = "";
                if (aqiValue != null) {
                    var intAQI = parseInt(aqiValue);
                    if (intAQI > 0 && intAQI <= 50) {
                        strAQLLevel = "优";
                    }
                    else if (intAQI > 50 && intAQI <= 100) {
                        strAQLLevel = "良";
                    }
                    else if (intAQI > 100 && intAQI <= 150) {
                        strAQLLevel = "轻度污染";
                    }
                    else if (intAQI > 150 && intAQI <= 200) {
                        strAQLLevel = "中度污染";
                    }
                    else if (intAQI > 200 && intAQI <= 300) {
                        strAQLLevel = "重度污染";
                    }
                    else if (intAQI > 300) {
                        strAQLLevel = "严重污染";
                    }
                }
                return strAQLLevel;
            }
            , initModal: function () {     //初始化modal框
                var pageWidth = $(document).width();
                var dialogW = $(".modal-dialog").width();
                var m_left = (pageWidth - dialogW) / 2;
                $('.modal-dialog').css({ "margin-left": m_left });
                $("#myModal").modal('show');
                drag($(".modal-header"), $(".modal-dialog"));   //拖动
            }
        }
    });
});

Vue.component("my-component", {
    template:"#template",
    props: {
        results: Object,
        options: Array,
        disable: Boolean,
        inborder: Boolean
    }
    , data: function () {
        return {
            "titles":title
        }
    }
    , watch: {
        selected: function ($event) {
            event.currentTarget.select();
        }
    }
    , methods: {
        focus: function (key, index, dateItem, value, event) {
            this.$nextTick(function () {
                beforeVal = value.split('/')[0] == undefined ? "" : value.split('/')[0];
                beforeAqi = value.split('/')[1] == undefined ? "" : value.split('/')[1];
                //result['' + keys + ''].Ele['' + key + ''][index].indis = false;
                dateItem[index].val = beforeVal;
                var currentDom = event.currentTarget;     //记录当前DOM元素，在setTimeout中使用，因为setTimeout中的currentTarget为NULL
                setTimeout(function () { currentDom.select(); }, 2);    //vue在这里先执行操作最后才渲染，所以延迟2毫秒
            })
            
            
        }
        , blur: function (key, index, dataItem, value, rowItems) {    //key污染物，index：1表示气象局，2表示中心，item:这一行的json，value：单元格的值,rowItems:一行的json数据，在计算aqi列的时候用来比较大小
            var currentEle = event.currentTarget;
            currentEle.classList.remove("error-border");
            if (beforeVal == value || key == "AQI") {    //值没有发生变化或改变的是aqi的值得话，不需要重新计算首要污染物，只改变aqi浓度值
                dataItem[index].val = dataItem[index].val + "/" + beforeAqi;
            }else {
                //污染物的值发生变化（不包括首要污染物）需要重新计算aqi浓度值
                this.calculateItemAqiValue(key, index, dataItem, value, rowItems,false);
            }
        }
        , keyup: function (key, index, dataItem) {
            var currentEle = event.currentTarget;
            var isError = new RegExp(/[^\d]/g).test(currentEle.value);
            if (isError) {
                currentEle.classList.add("error-border");
                dataItem[index].val = "";
            } else {
                currentEle.classList.remove("error-border");
                //value = currentEle.value.replace(/[^\d]/g, '');
                dataItem[index].val = currentEle.value;
            }
        }
        //计算每个污染物的值发生变化之后所对应的浓度值
        //cellValue在单元格联动时记录重新计算后的结果值
        , calculateItemAqiValue: function (key, index, dataItem, value, rowItems,over) {
            if (value == "") {
                dataItem[index].val = "/";
                this.calSdValue(key, index, dataItem, rowItems, false);
                return;
            }
            this.$http.post('AQIPeriod72.aspx/ToAQI', { value: value, item: key }).then(function (response) {
                var aqiValue = response.data.d;
                //dataItem[index].val = parseFloat(dataItem[index].val).toFixed(1) + "/" + aqiValue;
                dataItem[index].val = parseFloat(value).toFixed(1) + "/" + aqiValue;
                this.calculateAQI(index,rowItems);
                if (over) return;
                this.calSdValue(key, index, dataItem, rowItems, false);
            });
        }
        //计算最后一列的AQI，当首要污染物为O3_8h时，选择第二大的为首要污染物
        , calculateAQI: function (index, rowItems) {
            var poll = ["PM25", "PM10", "NO2", "O31", "O38"]    //在aqi浓度值相同的情况下有优先等级
            var maxAqiValue = rowItems.Ele[poll[0]][index].val.split('/')[1];      //记录最大的aqi的值，默认是PM2.5的最大
            maxAqiValue = maxAqiValue == "" ? "0" : maxAqiValue;     //当单元格的值为空，即“/”时，maxAqiValue的值为“”，需要处理成number类型
            var firstPoll = poll[0];         //记录最大的aqi值所对应的污染物
            var secondAqiValue = 0;     //记录第二大的aqi值
            var secondPoll = "";        //记录第二大的aqi值所对应的污染物
            for (i = 1; i < poll.length; i++) {     //遍历获取最大的污染物值以及所对应的污染物
                var aqiValue = rowItems.Ele[poll[i]][index].val.split('/')[1]
                aqiValue = aqiValue == "" ? "0" : aqiValue;     //如果值是控制则赋值为零，否则后面转成float做比较时会是一个NAN
                if (parseFloat(maxAqiValue) < parseFloat(aqiValue)) {
                    secondAqiValue = maxAqiValue;    //把最大aqi的值赋给第二大aqi
                    secondPoll = firstPoll;
                    maxAqiValue = aqiValue;
                    firstPoll = poll[i];
                }
            }
            if (firstPoll == "O38") {    //如果首要污染物是o38的话就取第二大污染物为首要污染物
                maxAqiValue = secondAqiValue;
                firstPoll = secondPoll;
            }
            //给最后一列aqi赋值
            rowItems.Ele.AQI[index].val = maxAqiValue + "/" + firstPoll;
        }
         , calSdValue: function (key, index, dataItem, rowItems,over) {
             var result = this.results;
             var dNow = new Date();
             var sd = rowItems.Interval[0].val;
             var date = rowItems.Date[0].val;
             date = date.replace("月", "-").substring(0, date.length - 1);
             var _date = dNow.getFullYear() + "-" + date;
             var addDate = this.addDate(_date,1).Format("MM-dd");
             var mNightValue = 0, aNightValue = 0, night = 0,mValue=0,aValue=0;  //记录时段所对应的值
             var calResult = 0;
            // var flag = false;
             var aNight_num = 0, night_num = 0, day_num = 0; num = 0;  //记录需要变化的索引
             var length = Object.getOwnPropertyNames(result).length - 1;
             for (i = 1; i <= length; i++) {     //遍历找出对应需要变化的单元格
                 var changeSD = result["row" + i].Interval[0].val;   //需要变化的时段
                 if (changeSD == "全天") {
                     continue;
                 }
                 var changeDate = result["row" + i].Date[0].val;   //需要变化的日期
                 changeDate = changeDate.replace("月", "-").substring(0, changeDate.length - 1);
                 var value = result["row" + i].Ele[key][index].val.split('/')[0];
                 
                 //获取参与计算夜间的值（上半夜和下半夜的值）
                 if (sd == "上半夜" || sd == "下半夜" || sd == "夜间") {
                     if (changeSD == "日平均" && addDate == changeDate) {      //改变的是第二天的平均值
                         day_num = i;
                     }else if (changeSD == "上半夜" && changeDate == date) {   //参与计算的值是当天的
                         mNightValue = value;
                     }else if (changeSD == "下半夜" && changeDate == date) {
                         aNightValue = value;
                         aNight_num = i;
                     }else if (changeSD == "夜间" && changeDate == date) {
                         night = value;
                         night_num = i;
                     }else if (changeSD == "上午" && addDate == changeDate) {   ////参与计算的是第二天上下午的值
                         mValue = value;
                     }else if (changeSD == "下午" && addDate == changeDate) {
                         aValue = value;
                     }
                 }
                 if (sd == "上午" || sd == "下午") {
                     if (changeSD == "日平均" && date == changeDate) {    //改变的是当天的平均值
                         day_num = i;  
                     }else if (changeSD == "上半夜" && this.addDate(changeDate,1).Format("MM-dd") == date) {  //参与计算的是前一天的上下半夜
                         mNightValue = value;
                     }else if (changeSD == "下半夜" && this.addDate(changeDate, 1).Format("MM-dd") == date) {
                         aNightValue = value;
                         aNight_num = i;
                     }
                     if (changeSD == "上午" && date == changeDate) {
                         mValue = value;
                     }
                     if (changeSD == "下午" && date == changeDate) {
                         aValue = value;
                     }
                 }
                 //flag = false;
             }
             if (sd == "上半夜" || sd == "下半夜") {   //改变的单元格是上或下半夜，需要重新计算夜间及全天
                 //改变夜间的值
                num = night_num;    //时段为夜间的数据需要重新计算
                calResult = this.changeYJ(mNightValue, aNightValue);
                this.calculateItemAqiValue(key, index, result["row" + num].Ele[key], calResult, result["row" + num],true);
                 //改变全天的值
                calResult = this.changeAverageDay(mNightValue, aNightValue, mValue, aValue);
                num = day_num;
                this.calculateItemAqiValue(key, index, result["row" + num].Ele[key], calResult, result["row" + num],true);
            } else if(sd=="夜间") {    //改变的是夜间的值，需根据上半夜重新计算下半夜的值
                num = aNight_num;  //时段为下半夜的数据需要重新计算
                calResult = this.changeAftNight(mNightValue, night);
                this.calculateItemAqiValue(key, index, result["row" + num].Ele[key], calResult, result["row" + num],false);
            } else if (sd == "上午" || sd == "下午") {
                num = day_num;
                calResult = this.changeAverageDay(mNightValue, aNightValue, mValue, aValue);
                this.calculateItemAqiValue(key, index, result["row" + num].Ele[key], calResult, result["row" + num], true);
            }
         }
        , addDate:function(date,days) {
            var d = new Date(date);
            d.setDate(d.getDate() + days);
            return d;
        }
        , changeYJ: function (mNightValue,aNightValue) {
            var result = 0;
            result = (mNightValue * 4 + aNightValue * 6) / 10;
            return result
        }
        , changeAftNight: function (mNightValue,nightValue) {
            var result = 0;
            result = (nightValue * 10 - mNightValue * 4) / 6;
            return result
        }
        , changeAverageDay: function (mNightValue, aNightValue,morning,afternoon) {
            var result = 0;
            result = (mNightValue * 4 + aNightValue * 6 + morning * 6 + afternoon * 8) / 24;
            return result
        }
    }
});