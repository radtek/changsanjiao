Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    
    //InitTable();
}
)
//获取鼠标按下时的值
function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'PersonnalScoreData'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                coutTable0.innerHTML = response.responseText;
            }
        },
        failure: function (response) {/// <reference path="PersonRating.js" />

            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function OutTable() {
    var dateTime = Ext.getDom("H00").value;
    var content = dateTime;
    var Element = document.getElementById("Element");
    Element.setAttribute("value", content);
    document.getElementById("btnExport").click();
}
function ResoreData() {
    var dateTime = Ext.getDom("H00").value;
    var content = getContent();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'PersonnalResoreData'),
        params: { dateTime: dateTime, content: content },
        success: function (response) {
            Ext.Msg.alert("信息", response.responseText);
            InitTable();
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function trMonthClick(time, userName) {
    return;   //后来加的，不需要点击之后查看其它的表格
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'retrunSinglePerson'),
        params: { time: time, userName: userName },
        success: function (response) {
            if (response.responseText != "") {
                $('#personMonth').html(response.responseText);
                var showImg = Ext.getDom("showImg");
                showImg.className = "show1";
                $('.bg').fadeIn(200);
                $('.showImg').fadeIn(400);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function trDayClick(time, userName, el) {
    var obj = el;
    var durarion = $(obj).children('td').eq(1).html();
    var haze = $(obj).children('td').eq(2).html();
    var Uv = $(obj).children('td').eq(3).html();
    var china = $(obj).children('td').eq(4).html();
    var totalScore = $(obj).children('td').eq(5).html();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ScoreData', 'retrunSingleEveryDay'),
        params: { time: time, userName: userName, durarion: durarion, haze: haze, Uv: Uv, china: china, totalScore: totalScore },
        success: function (response) {
            if (response.responseText != "") {
                $('#personDay').html(response.responseText);
                var showImg = Ext.getDom("showEveryDay");
                showImg.className = "show";
                $('.showEveryDay').fadeIn(400);
                $('.bg').fadeIn(200);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
$('.bg').click(function () {
    var showImg = Ext.getDom("showImg");
    var showImg1 = Ext.getDom("showEveryDay");
    if (showImg1.className == "show") {
        $('.showEveryDay').fadeOut(800);
        showImg1.className = "hidden";
        showImg.className = "show1";
    }
    else {
        $('.showImg').fadeOut(800);
        showImg.className = "hidden";
        $('.bg').fadeOut(800);
    }
});
function fadeOut() {
    var showImg = Ext.getDom("showImg");
    var showImg1 = Ext.getDom("showEveryDay");
    if (showImg1.className == "show") {
        $('.showEveryDay').fadeOut(800);
        showImg1.className = "hidden";
        showImg.className = "show1";
    }
    else {
        $('.showImg').fadeOut(800);
        showImg.className = "hidden";
        $('.bg').fadeOut(800);
    }
}
function getContent() {
    var postJson = "";
    var name = "";
    var oldName="";
    var index = 0;
    var tempJson = "";
    var id = "";
    var aryDiv = forecastTable.getElementsByTagName("div");
    var length = forecastTable.getElementsByTagName('tr').length - 2;
    for (var j = 0; j < length; j++) {
        tempJson = "";
        for (var i = 0; i < aryDiv.length; i++) {
            if (aryDiv[i].id.indexOf('_') > 0) {
                if (j < 10) {
                    id = aryDiv[i].id.substr(3, 1);
                }
                else {
                    id = aryDiv[i].id.substr(3, 2);
                }
                if (id == j.toString()) {
                    var lastValue = aryDiv[i].innerHTML.trim();
                    index = aryDiv[i].id.indexOf('_');
                    name = aryDiv[i].id.substr(index + 1);
                    tempJson = tempJson + aryDiv[i].id + ":" + lastValue + ",";
                }
            }
        }
        if (tempJson.length > 0) {
            tempJson = tempJson + "textarea" + j.toString() + "_" + name + ":" + Ext.getDom("textarea" + j.toString() + "_" + name).value;
        }
        if (postJson != "")
            postJson = postJson + "|" + name + "*" + tempJson;
        else
            postJson = name + "*" + tempJson;
    }
    return postJson;
}
var result = {
    //"row0": [{ "forecaster": "没有数据" }]
};
var tabTitleOne = [{ "colspan": 2, "val": "预报员<br/>及班次" }
                    , { "colspan": 2, "val": "上传国家<br/>局AQI评分" }
                    , { "colspan": 8, "val": "总分" }
                    , { "colspan": 8, "val": "各项平均分" }
                    , { "colspan": 3, "val": "分时段评分" }
                    , { "colspan": 3, "val": "" }
];
var vm = new Vue({
    el: '#page'
    , data: {
        date: new Date().Format("yyyy年MM月")
        , tabTitleOne: tabTitleOne
        , tabTitleTwo: ["预报员", "班次", "总分", "平均分", "环境", "PM2.5", "O3-1h", "O3-8h", "PM10", "NO2", "霾", "UV", "环境", "PM2.5", "O3-1h"
                        , "O3-8h", "PM10", "NO2", "霾", "UV", "夜间", "上午", "下午", "日评分", "环境<br/>总分", "个人<br/>成绩"]
        , results: result
    }
    , mounted: function () {
        this.$nextTick(function () {
            $("#H00").val(new Date().Format("yyyy年MM月"));
            InitTable();
            this.getSecondTabData();
        })
    }
    , methods: {
        wp: function () {
            WdatePicker({ dateFmt: 'yyyy年MM月' });
        }
        , btnQuery: function () {
            InitTable();
            this.getSecondTabData();

        }
        , getSecondTabData: function () {
            this.$http.post('PersonRating.aspx/BtnQuery', { date: $("#H00").val() }).then(function (response) {
                var data = eval('(' + response.data.d + ')');
                this.results = data;
            });
        }
        , tempDownload: function () {
            var url = "../EvaluateHtml/个人评分模板下载.xlsx";
            var iframe = document.createElement("iframe")
            iframe.style.display = "none";
            iframe.src = url;
            document.body.appendChild(iframe);
        }
        , save: function () {
            ResoreData();
        }
        , evaluate: function () {
            var mk = new Ext.LoadMask(document.body, {  
                msg: '正在评分，请稍候！',  
                removeMask: true //完成后移除  
            });  
            mk.show(); //显示  
            this.$http.post('PersonRating.aspx/PersonDown', { date: $("#H00").val() }).then(function (response) {
                mk.hide(); //关闭  
                this.btnQuery();
                alert(response.data.d);
            });
        }
        , upLoad: function () {
            $("#uploadFrm").load(function () {
                var response = $(window.frames['uploadFrm'].document.body)[0].innerHTML;
                $("#mdlImport").modal('hide');
                if (response.indexOf("ok") > -1) {
                    alert("上传成功！");
                } else {
                    alert("上传失败" + response);
                }
            })
            $("#actionForm").submit();
        }
        , importData: function () {
            var defaultURL = "WebExplorer.ashx";
            var path = "uploadDir";
            document.getElementById("actionForm").action = defaultURL + "?action=upload&value=" + encodeURIComponent(path);
            $("#mdlImport").modal('show');
            $("#mdlDia").css({
                width: '400'
            });
        }
        , exportDate: function () {
            //$(".contentTab").table2excel({
            //    //exclude: ".noExl",
            //    name: "分段日评分",
            //    filename: this.date + '.xls',
            //    exclude_img: true,
            //    exclude_links: false,
            //    exclude_inputs: true
            //});
            var date = $("#H00").val();
            var defaultURL = "WebExplorer.ashx";
            document.getElementById("actionForm").action = defaultURL + "?action=dataExport&date=" + encodeURIComponent(date) + "";
            document.getElementById("actionForm").submit();
            //OutTable();

        }
    }
});
